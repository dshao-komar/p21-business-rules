# Unapproved_Order_No_Price Project Details

## Business Goal

This folder contains sales-order rules for missing line pricing.

Intent:

- on sales-order save, find selected order lines with blank, null, nonnumeric, or zero `unit_price`
- if any selected line is missing pricing and Manager Approved is not checked:
  - allow the save
  - force `d_oe_header.approved` to `N`
  - show a warning message
- if Manager Approved is checked, allow the order to remain approved
- block users from checking `d_oe_header.approved` when any selected line is missing pricing unless Manager Approved is checked

`d_oe_header.ufc_oe_hdr_ud_manager_approved` is the manual approval override for unpriced orders.

## Current Rule Files

Main code files:

- `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\Unapproved_Order_No_Price.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\Unapproved_Order_No_Price.csproj`
- `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\Properties\AssemblyInfo.cs`

Primary output DLL:

- `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\bin\Debug\Unapproved_Order_No_Price.dll`

## Confirmed P21 Dataset Fields

These fields are used by the rules:

- `d_oe_header.approved`
- `d_oe_header.ufc_oe_hdr_ud_manager_approved`
- `d_dw_oe_line_dataentry.unit_price`
- `d_dw_oe_line_dataentry.oe_order_item_id`

## Rule: Unapproved_Order_No_Price

### Behavior

This rule runs on sales-order save in multi-row mode.

Missing pricing means:

- null
- blank
- nonnumeric
- exactly `0`

The rule evaluates every selected row in `d_dw_oe_line_dataentry`. It does not filter canceled, deleted, non-stock, comment, or service rows because no additional line-scope fields are selected for this rule.

When one item is missing pricing and Manager Approved is not checked, the message is:

`Item {oe_order_item_id} is missing pricing. The sales order has been marked Unapproved and must be manually approved before processing.`

When multiple items are missing pricing and Manager Approved is not checked, the message is:

`Multiple items are missing pricing. The sales order has been marked Unapproved and must be manually approved before processing.`

When one item is missing pricing but `oe_order_item_id` is blank, the message is:

`Item on one sales order line is missing pricing. The sales order has been marked Unapproved and must be manually approved before processing.`

### P21 Configuration Summary

- `Rule Name`: `Unapproved_Order_No_Price`
- `Rule Type`: `Validator`
- `Apply Rule On`: `Save`
- `Multi-Row`: checked
- `Callback Rule`: none
- `DLL path`: `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\bin\Debug\Unapproved_Order_No_Price.dll`

### Field Selector Setup

Select these fields for the save rule:

- `d_oe_header.approved`
- `d_oe_header.ufc_oe_hdr_ud_manager_approved`
- `d_dw_oe_line_dataentry.unit_price`
- `d_dw_oe_line_dataentry.oe_order_item_id`

### Triggered Fields

This rule runs on `Save`, not `Field Edit`, so there is no single field-specific trigger to configure.

- `Triggered field(s)`: none
- `Execution event`: sales-order `Save`
- `Important`: the rule still depends on the Field Selector entries above being included in the multi-row dataset even though no field-level trigger is used

## Rule: Validate_Approved_for_Unpriced_Order

### Behavior

This rule runs when `d_oe_header.approved` is edited.

Current diagnostic status:

- `EnableApprovedCheckDiagnostics` is enabled in code for live testing
- when the rule fires on the Approved edit, it intentionally rejects the edit and shows trigger/data diagnostics
- if no diagnostic appears when Approved is checked, the rule is not firing or is not attached to the active Approved field trigger
- remove or set `EnableApprovedCheckDiagnostics` to `false` after the live trigger/data issue is resolved

- checking `approved` is allowed when all selected lines have a nonblank, nonzero `unit_price`
- checking `approved` is also allowed when `d_oe_header.ufc_oe_hdr_ud_manager_approved = Y`
- checking `approved` is blocked when any selected line is missing pricing and Manager Approved is not checked
- unchecking `approved` is allowed silently
- the rule rejects the field edit through `RuleResult`; it does not write back to `approved`

Blocked message:

`This order cannot be approved withouth manager approval`

### P21 Configuration Summary

- `Rule Name`: `Validate_Approved_for_Unpriced_Order`
- `Rule Type`: `Validator`
- `Apply Rule On`: `Field Edit`
- `Multi-Row`: checked
- `Callback Rule`: none
- `DLL path`: `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\bin\Debug\Unapproved_Order_No_Price.dll`

### Field Selector Setup

Select these fields for the field-edit rule:

- `d_oe_header.approved`
- `d_oe_header.ufc_oe_hdr_ud_manager_approved`
- `d_dw_oe_line_dataentry.unit_price`

### Triggered Fields

Configure this field as the trigger:

- `d_oe_header.approved`

## Setup References

Setup labels and patterns were verified against:

- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\DynaChange Rules Guide.pdf`
- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\AGENTS.md`
- `C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\AGENTS.md`

## Build Notes

Compile with the Windows .NET Framework compiler and the existing local P21 DLL references:

```powershell
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:library /out:'C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\bin\Debug\Unapproved_Order_No_Price.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Clients.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.UDF.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Extensions.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.UI.Service.Model.dll' /r:System.Data.dll /r:System.Core.dll /r:System.Data.DataSetExtensions.dll /r:System.Xml.dll /r:System.Xml.Linq.dll 'C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\Unapproved_Order_No_Price.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\Properties\AssemblyInfo.cs'
```
