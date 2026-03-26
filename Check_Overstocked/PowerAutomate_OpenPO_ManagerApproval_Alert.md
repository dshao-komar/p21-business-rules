# Power Automate Setup: Open POs Requiring Manager Approval

This guide walks through creating a scheduled Power Automate cloud flow that:

- checks every 2 hours
- only takes action during business hours from `8:00 AM` through `6:00 PM`
- looks for open purchase orders that still require manager approval
- sends an email if any matching rows exist
- can optionally be extended so each PO alerts only once until it is cleared

## Important design choice

For a typical Prophet 21 / on-prem SQL Server environment, use a SQL **view** plus the SQL Server **Get rows (V2)** action.

Reason:

- Microsoft’s current SQL Server connector documentation says `Execute a SQL query (V2)` is **not supported for on-premises SQL Server**.
- `Get rows (V2)` is supported.
- If your SQL Server is on-premises, Power Automate also needs an **on-premises data gateway**.

## Prerequisites

Before building the flow, make sure all of these are true:

1. You have a Power Automate license/environment that allows the **SQL Server Premium connector**.
2. You know whether your Prophet 21 SQL Server is:
   - on-premises SQL Server, or
   - Azure SQL
3. If it is on-premises SQL Server:
   - an **on-premises data gateway** is installed in **standard mode**
   - the gateway is online
   - your Power Automate account has permission to use the gateway connection
4. You know the:
   - SQL Server name
   - database name
   - SQL credentials or Windows credentials used by the SQL connection
5. You know the manager email address you want to notify.

## Step 1: Create the base SQL view

Run this in SQL Server Management Studio against the Prophet 21 database.

If your actual field name in `po_hdr_ud` is `ufc_po_hdr_ud_manager_approved` instead of `manager_approved`, replace it below before creating the view.

```sql
CREATE VIEW dbo.vw_open_pos_require_manager_approval
AS
SELECT
      p.po_no
    , p.approved
    , p.cancel_flag
    , p.delete_flag
    , p.complete
    , u.manager_approved
FROM po_hdr p
JOIN po_hdr_ud u
    ON u.po_no = p.po_no
WHERE p.approved = 'N'
  AND COALESCE(p.cancel_flag, 'N') = 'N'
  AND p.delete_flag = 'N'
  AND p.complete = 'N'
  AND u.manager_approved = 'N';
GO
```

Test it:

```sql
SELECT *
FROM dbo.vw_open_pos_require_manager_approval
ORDER BY po_no;
```

If the result set is correct, continue.

## Recommended anti-duplicate design

If you do nothing else, this flow will send the same alert every 2 hours while the PO remains in the result set.

The cleanest way to avoid that is to create:

1. a log table
2. an `unsent` view that only returns POs never alerted before, or POs whose previous alert was cleared

### Step 1A: Create the alert log table

```sql
CREATE TABLE dbo.po_manager_approval_alert_log
(
      po_no                 varchar(50)  NOT NULL
    , first_alert_sent_utc  datetime2(0) NOT NULL
    , last_alert_sent_utc   datetime2(0) NOT NULL
    , alert_count           int          NOT NULL
    , cleared_utc           datetime2(0) NULL
    , CONSTRAINT PK_po_manager_approval_alert_log PRIMARY KEY (po_no)
);
GO
```

### Step 1B: Create the unsent-only view

```sql
CREATE VIEW dbo.vw_open_pos_require_manager_approval_unsent
AS
SELECT
      v.po_no
    , v.approved
    , v.cancel_flag
    , v.delete_flag
    , v.complete
    , v.manager_approved
FROM dbo.vw_open_pos_require_manager_approval v
LEFT JOIN dbo.po_manager_approval_alert_log l
    ON l.po_no = v.po_no
   AND l.cleared_utc IS NULL
WHERE l.po_no IS NULL;
GO
```

### Optional reset process

If you want the same PO to alert again after it has been resolved and later becomes a problem again, mark the old log row as cleared once the PO no longer appears in the base view.

Example cleanup SQL:

```sql
UPDATE l
SET cleared_utc = SYSUTCDATETIME()
FROM dbo.po_manager_approval_alert_log l
WHERE l.cleared_utc IS NULL
  AND NOT EXISTS
  (
      SELECT 1
      FROM dbo.vw_open_pos_require_manager_approval v
      WHERE v.po_no = l.po_no
  );
```

You can run that cleanup:

- in a second Power Automate flow
- as a SQL Agent job
- or manually

## Step 2: Create the scheduled cloud flow

1. Go to [Power Automate](https://make.powerautomate.com/).
2. Sign in to the correct environment.
3. In the left menu, select `Create`.
4. Select `Scheduled cloud flow`.
5. Enter the flow name:
   - `Alert - Open POs Requiring Manager Approval`
6. Set the schedule:
   - `Starting`: set this to the next desired `8:00 AM` start time in your local business time
   - `Repeat every`: `2`
   - `Frequency`: `Hour`
7. Select `Create`.

## Step 3: Configure the Recurrence trigger

In the `Recurrence` trigger:

1. Open the trigger card.
2. Set:
   - `Frequency`: `Hour`
   - `Interval`: `2`
3. Open `Show advanced options`.
4. Set:
   - `Time zone`: `Pacific Standard Time`
   - `Start time`: your chosen start timestamp at `8:00 AM`

Recommended behavior:

- Starting at `8:00 AM` with a 2-hour interval gives runs at approximately `8, 10, 12, 2, 4, 6, 8, 10...`
- We will add a business-hours check so the flow does nothing outside `8:00 AM` to `6:00 PM`.

## Step 4: Add a business-hours condition

1. Select `+ New step`.
2. Search for `Condition`.
3. Add the `Condition` control.
4. In the condition card, select `Edit in advanced mode` if shown, or use the `fx` expression box.
5. Use this expression:

```text
@and(
  greaterOrEquals(
    int(formatDateTime(convertTimeZone(utcNow(),'UTC','Pacific Standard Time'),'HH')),
    8
  ),
  lessOrEquals(
    int(formatDateTime(convertTimeZone(utcNow(),'UTC','Pacific Standard Time'),'HH')),
    18
  )
)
```

What this does:

- `True` branch: flow continues only during `8 AM` through `6 PM` Pacific time
- `False` branch: do nothing

## Step 5: Add the SQL Server action

Inside the **If yes** branch of the business-hours condition:

1. Select `Add an action`.
2. Search for `SQL Server`.
3. Choose `Get rows (V2)`.

### If this is your first SQL connection

Power Automate will prompt for a connection.

If SQL Server is on-premises:

1. Choose the existing SQL connection if one already exists, or create a new one.
2. Select the gateway-backed connection.
3. Confirm the correct:
   - server
   - database
   - authentication method
   - gateway

### Configure Get rows (V2)

Fill in:

- `Server name`: your SQL Server name
- `Database name`: your Prophet 21 database
- `Table name`: use one of these:
  - `dbo.vw_open_pos_require_manager_approval` if you want repeated reminders every 2 hours
  - `dbo.vw_open_pos_require_manager_approval_unsent` if you want one alert per PO until it is cleared

Optional recommended settings:

- `Order By`: `po_no asc`
- `Top Count`: `5000`

## Step 6: Check whether any rows were returned

1. Under the `Get rows (V2)` action, select `Add an action`.
2. Add another `Condition`.
3. Use this expression:

```text
@greater(length(body('Get_rows_(V2)')?['value']), 0)
```

What this does:

- `True` branch: at least one PO needs manager approval
- `False` branch: no matching rows, so do nothing

## Step 7: Create an HTML table for the email

Inside the **If yes** branch of the second condition:

1. Select `Add an action`.
2. Search for `Create HTML table`.
3. Choose the `Data Operations` action named `Create HTML table`.

Set:

- `From`: select the `value` output from `Get rows (V2)`
- `Columns`: `Automatic`

If you prefer fewer columns:

1. Change `Columns` to `Custom`.
2. Add columns such as:
   - `PO Number` -> `po_no`
   - `Approved` -> `approved`
   - `Manager Approved` -> `manager_approved`

## Step 8: Add the email action

1. Under `Create HTML table`, select `Add an action`.
2. Search for `Send an email (V2)`.
3. Choose `Office 365 Outlook - Send an email (V2)`.

Configure:

- `To`: manager email address
- `Subject`: `Open POs requiring manager approval`
- `Body`: use the example below
- `Is HTML`: `Yes`

Suggested email body:

```html
<p>The following open purchase orders are not approved and still require manager approval:</p>
@{outputs('Create_HTML_table')}
<p>This alert runs every 2 hours from 8:00 AM to 6:00 PM Pacific Time.</p>
```

## Step 9: If using the unsent-view design, log each sent PO

Only do this step if you chose `dbo.vw_open_pos_require_manager_approval_unsent`.

After the email step:

1. Add `Apply to each`
   - `From`: the `value` output from `Get rows (V2)`
2. Inside `Apply to each`, add `SQL Server - Insert row (V2)`
3. Point it to:
   - `dbo.po_manager_approval_alert_log`
4. Map:
   - `po_no` -> current item `po_no`
   - `first_alert_sent_utc` -> `utcNow()`
   - `last_alert_sent_utc` -> `utcNow()`
   - `alert_count` -> `1`
   - `cleared_utc` -> leave blank

Because the unsent view hides already-logged POs, this insert should only happen once per active PO.

## Step 10: Save the flow

1. Select `Save`.
2. Wait for Power Automate to confirm the save was successful.

## Step 11: Test the flow

### Manual test

1. Make sure the chosen view returns at least one row.
2. In Power Automate, open the flow.
3. Select `Test`.
4. Choose manual testing if prompted.
5. Run the flow.

Expected result:

- The SQL action returns one or more rows.
- The second condition evaluates to `true`.
- An email is sent to the manager.

### No-row test

1. Make sure the chosen view returns zero rows.
2. Test the flow again.

Expected result:

- The SQL action runs.
- The second condition evaluates to `false`.
- No email is sent.

## Step 12: Confirm live schedule behavior

With the configuration above, the flow will wake up every 2 hours.

Because of the business-hours condition, it will only actually check and email during Pacific business hours:

- `8:00 AM`
- `10:00 AM`
- `12:00 PM`
- `2:00 PM`
- `4:00 PM`
- `6:00 PM`

Outside those hours, the flow should run and immediately do nothing.

## Recommended flow layout

Your finished flow should look like this:

1. `Recurrence`
2. `Condition` - business hours check
3. `Get rows (V2)` - in the `If yes` branch
4. `Condition` - row count > 0
5. `Create HTML table` - in the second `If yes` branch
6. `Send an email (V2)` - after `Create HTML table`
7. `Apply to each` + `Insert row (V2)` - only if using the unsent-view design

## Common gotchas

### 1. SQL Server connector is Premium

The SQL Server managed connector is listed by Microsoft as a **Premium** connector in Power Automate.

### 2. On-premises SQL needs a gateway

If your Prophet 21 SQL Server is on-premises, Power Automate needs an **on-premises data gateway** connection.

### 3. Do not use Execute a SQL query (V2) for on-prem SQL

Microsoft’s current SQL connector documentation says `Execute a SQL query (V2)` has limited support and is **not supported for on-premises SQL Server**.

That is why this guide uses:

- a SQL view
- `Get rows (V2)`

### 4. Choose your alert behavior up front

You now have two clean options:

- `vw_open_pos_require_manager_approval`
  - sends repeated reminders every 2 hours while rows remain
- `vw_open_pos_require_manager_approval_unsent`
  - sends one alert per PO until that PO is cleared from the log

## Sources

- Microsoft Learn: [Triggers - Power Automate](https://learn.microsoft.com/en-us/power-automate/triggers-introduction)
- Microsoft Learn: [SQL Server connector reference](https://learn.microsoft.com/en-us/connectors/sql/)
- Microsoft Learn: [What is an on-premises data gateway?](https://learn.microsoft.com/en-us/power-automate/gateway-reference)
- Microsoft Learn: [Create flows for popular email scenarios in Power Automate](https://learn.microsoft.com/en-us/power-automate/email-top-scenarios)
- Microsoft Learn: [Troubleshoot common issues with Power Automate triggers](https://learn.microsoft.com/en-us/troubleshoot/power-platform/power-automate/flow-run-issues/triggers-troubleshoot)
