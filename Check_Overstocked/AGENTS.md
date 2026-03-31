# Check_Overstocked Project Details

## Business Goal

The main business rule is `Check_Overstocked`.

Intent:

- identify purchase orders that would create more than 3 months available for an item
- if overstocked and not manager-approved:
  - allow the PO save
  - force `approved` to `N`
  - show a notification
  - attempt an email alert
- if overstocked and manager-approved:
  - allow approval to remain

There is also a second rule:

- `Validate_Manager_Approval_for_Overstocked_POs`

Intent:

- control edits to `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`
- allowed users can check or uncheck silently
- forbidden users cannot modify it and receive a message

## Current Rule Files

Main code files:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Validate_Manager_Approval_for_Overstocked_POs.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked.csproj`

Support docs created during this project:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\P21_Rule_Setup_Guide.md`
- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\PowerAutomate_OpenPO_ManagerApproval_Alert.md`

Additional workspace note:

- this workspace is **not** a git repository, so future agents should not expect `git status` or commit history to be available here

## Confirmed P21 Dataset Fields

These were confirmed by the user as relevant P21 multi-row dataset fields:

- `d_po_item_sheet.item_id`
- `d_po_item_sheet.inv_mast_uid`
- `d_po_item_sheet.qty_ordered`
- `d_po_item_sheet.location_id`
- `d_po_item_sheet.unit_of_measure`
- `d_po_item_sheet.cancel_flag`
- `d_po_item_sheet.delete_flag`
- `d_po_classes_sheet.expected_date`
- `d_po_purchase_order_sheet.po_no`
- `d_po_purchase_order_sheet.approved`
- `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`

Important current fallback behavior:

- the main rule still prefers `d_po_classes_sheet.expected_date` per line
- but the current code now falls back to a header-level date when line expected date is missing
- current header fallback candidates in code are:
  - `order_date`
  - `po_date`
  - `date_created`
  - `created_date`
  - `creation_date`

## Confirmed SQL / Business Logic Assumptions

### Current quantity source

User supplied this pattern for on-hand by item/location:

```sql
select m.inv_mast_uid, qty_on_hand, m.default_purchasing_unit
from inv_loc l
join inv_mast m on m.inv_mast_uid = l.inv_mast_uid
where l.location_id = 210
  and m.item_id = 'MRDSAS2366';
```

### Usage calculation

The rule logic is based on the user-provided `inv_period_usage` / `demand_period` pattern and uses `usage3` as the effective monthly usage for overstock projections.

### Other inbound POs

Important confirmed detail:

- `po_hdr.location_id` is the location source for inbound PO filtering
- `po_hdr` joins to `po_line` by `po_no`
- other inbound expected date should use `po_hdr.expected_date`

### Projection method

The current rule projects on-hand forward by subtracting `usage3` in 30-day steps before the current PO expected date, then adds:

- qualifying other inbound PO quantity due on or before that date
- current PO inbound quantity due on or before that date

If a line-level expected date is missing, the current code uses the header date fallback described above instead of skipping the line immediately.

### Ignored lines

Current logic ignores PO lines where:

- `d_po_item_sheet.cancel_flag = 'Y'`
- `d_po_item_sheet.delete_flag = 'Y'`

Zero-usage items are excluded from overstock evaluation.

## Current User-Facing Behavior

### Check_Overstocked

When overstocked and manager approval is not checked:

- PO save is allowed
- `approved` is forced to `N`
- message begins with:
  - `PO cannot be saved as Approved because one or more items would exceed 3 months available.`
- message ends with:
  - `Manager has been notified.`

The message includes:

- item
- location
- expected date
- current on hand
- projected on-hand before this PO
- other inbound quantity and PO numbers
- this PO inbound
- 3-month usage
- projected months available

### Validate_Manager_Approval_for_Overstocked_POs

Current allow-list:

- `CWESTBY`
- `DMCKINLEY`
- `JFELDMAN`
- `DSHAO`

Current behavior:

- allowed users can check/uncheck `ufc_po_hdr_ud_manager_approved` silently
- forbidden users cannot change it in either direction
- forbidden message:
  - `Manager Approved cannot be modified by user <<Session.UserID>>.`
- the rule is intentionally narrow and does **not** query SQL for roles or overstock state anymore
- it only controls whether the checkbox may be edited by an allowed `Session.UserID`

Important implementation detail:

- the rule uses `Session.UserID`
- this was validated in live testing
- examples returned by the user:
  - forbidden account: `SCAN14`
  - permitted account: `DSHAO`

## Known Working Status

As of the latest user confirmation:

- `Check_Overstocked` is working in the live environment
- `Validate_Manager_Approval_for_Overstocked_POs` works correctly for:
  - allowed user behavior
  - forbidden user behavior

The forbidden-user freeze issue was resolved by rejecting the field edit through the validator result instead of writing the field back directly.

## Build Notes

The project has been compiled successfully using the .NET Framework C# compiler directly:

- `C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe`

MSBuild from SSMS was not reliable because of missing Roslyn targets, so future agents should not assume that `MSBuild.exe` in the SSMS path is sufficient.

The project currently references Prophet 21 DLLs from:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\`

Expected reference files:

- `P21.Clients.dll`
- `P21.DomainObject.dll`
- `P21.DomainObject.UDF.dll`
- `P21.Extensions.dll`
- `P21.UI.Service.Model.dll`

## P21 Configuration Summary

Full setup details live in:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\P21_Rule_Setup_Guide.md`

High-level summary:

### Rule: Check_Overstocked

- `Rule Type`: `Validator`
- `Apply Rule On`: `Save`
- `Multi-Row`: checked

### Rule: Validate_Manager_Approval_for_Overstocked_POs`

- `Rule Type`: `Validator`
- `Apply Rule On`: `Field Edit`
- `Multi-Row`: checked
- field selected/triggered:
  - `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`

## Power Automate Work

There is also a Power Automate guidance document:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\PowerAutomate_OpenPO_ManagerApproval_Alert.md`

Intent:

- query open POs requiring manager approval every 2 hours
- only during 8 AM to 6 PM Pacific
- email the manager if rows exist

The current recommended Power Automate pattern is:

- create SQL view(s)
- use Power Automate `Get rows (V2)`
- optionally use a SQL alert-log table to avoid duplicate alerts

Recommended anti-duplicate approach now documented:

- base view:
  - `vw_open_pos_require_manager_approval`
- alert log table:
  - `po_manager_approval_alert_log`
- unsent-only view:
  - `vw_open_pos_require_manager_approval_unsent`
- optional cleanup marks log rows as cleared when the PO leaves the exception set
