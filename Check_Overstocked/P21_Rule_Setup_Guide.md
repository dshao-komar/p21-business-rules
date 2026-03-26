# P21 Rule Setup Guide

This guide documents the Prophet 21 configuration for the rules compiled into `Check_Overstocked.dll`.

## DLL

Use this DLL:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\bin\Debug\Check_Overstocked.dll`

The DLL currently contains these rule classes:

- `Check_Overstocked`
- `Validate_Manager_Approval_for_Overstocked_POs`

## Rule 1: Check_Overstocked

### Purpose

- Evaluates whether the PO would create more than 3 months available.
- If overstocked and `ufc_po_hdr_ud_manager_approved <> 'Y'`, the rule:
  - changes `d_po_purchase_order_sheet.approved` to `N`
  - shows the overstock notification
  - attempts to send an alert email
- If overstocked and `ufc_po_hdr_ud_manager_approved = 'Y'`, the rule allows approval to remain.

### Configuration Options

- `Rule Type`: `Validator`
- `Apply Rule On`: `Save`
- `Multi-Row`: checked
- `Global Rule`: unchecked
- `Run Type`: blank
- `Rule Consumer Key`: blank
- `Web Visual Rule URL`: blank

### Field Selector

Select these fields.

#### Header fields

- `d_po_purchase_order_sheet.po_no`
- `d_po_purchase_order_sheet.approved`
- `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`

#### PO item fields

- `d_po_item_sheet.item_id`
- `d_po_item_sheet.inv_mast_uid`
- `d_po_item_sheet.qty_ordered`
- `d_po_item_sheet.location_id`
- `d_po_item_sheet.unit_of_measure`
- `d_po_item_sheet.cancel_flag`
- `d_po_item_sheet.delete_flag`

#### PO class fields

- `d_po_classes_sheet.expected_date`

### Expected date fallback behavior

- The rule prefers `d_po_classes_sheet.expected_date` per line.
- If that line-level expected date is blank, the current code falls back to a header-level date.
- Current header fallback candidates in code are:
  - `order_date`
  - `po_date`
  - `date_created`
  - `created_date`
  - `creation_date`

### Trigger settings

- `Selected`: checked for all fields above
- `Triggers Rule`: unchecked for this save rule
- `Pass To Rule As`: leave blank unless P21 requires an alias

## Rule 2: Validate_Manager_Approval_for_Overstocked_POs

### Purpose

- Fires when the `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved` checkbox is edited.
- Uses `Session.UserID` directly.
- Allows silent check and uncheck for:
  - `CWESTBY`
  - `DMCKINLEY`
  - `JFELDMAN`
  - `DSHAO`
- Blocks all other users from changing the checkbox in either direction.
- Forbidden users get:
  - `Manager Approved cannot be modified by user <<Session.UserID>>.`

### Configuration Options

- `Rule Type`: `Validator`
- `Apply Rule On`: `Field Edit`
- `Multi-Row`: checked
- `Global Rule`: unchecked
- `Run Type`: blank
- `Rule Consumer Key`: blank
- `Web Visual Rule URL`: blank

### Field Selector

Select only this field:

- `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`

### Trigger settings

- `Selected`: checked for `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`
- `Triggers Rule`: checked for `d_po_purchase_order_sheet.ufc_po_hdr_ud_manager_approved`
- `Pass To Rule As`: leave blank

## Expected Behavior

### When PO is overstocked and manager approval is not checked

- The PO can save.
- `approved` is forced to `N`.
- The overstock message appears.

### When Manager Approved is edited

- Allowed users can check and uncheck silently.
- Forbidden users cannot check or uncheck it.
- Forbidden users get:
  - `Manager Approved cannot be modified by user <<Session.UserID>>.`

## Email Alert Notes

The current alert settings compiled into the rule are:

- SMTP server: `localhost`
- Port: `25`
- To: `dshao@komar.com`
- From: `P21Alerts@komar.com`

Email failures are ignored so they do not interrupt saving the PO.
