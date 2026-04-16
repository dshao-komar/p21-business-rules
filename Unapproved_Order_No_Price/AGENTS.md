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
- `d_oe_header.oe_hdr_terms`
- `d_oe_payment_details.cc_creditcard_number`
- `d_oe_payment_details.cc_expiration_date`
- `d_oe_payment_details.cc_name`
- `d_dw_oe_line_dataentry.unit_price`
- `d_dw_oe_line_dataentry.oe_order_item_id`
- `d_dw_oe_line_dataentry.detail_type`
- `d_dw_oe_line_dataentry.cancel_flag`
- `d_dw_oe_line_dataentry.delete_flag`

## Rule: Unapproved_Order_No_Price

### Behavior

This rule runs on sales-order save in multi-row mode.

Missing pricing means:

- null
- blank
- nonnumeric
- exactly `0`

The rule evaluates only active parent/order rows in `d_dw_oe_line_dataentry`:

- `detail_type = 0`
- `cancel_flag <> Y`
- `delete_flag <> Y`

It intentionally ignores component/child rows such as `detail_type = 1`, even when those rows have `unit_price = 0`.

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
- `d_dw_oe_line_dataentry.detail_type`
- `d_dw_oe_line_dataentry.cancel_flag`
- `d_dw_oe_line_dataentry.delete_flag`

### Triggered Fields

This rule runs on `Save`, not `Field Edit`, so there is no single field-specific trigger to configure.

- `Triggered field(s)`: none
- `Execution event`: sales-order `Save`
- `Important`: the rule still depends on the Field Selector entries above being included in the multi-row dataset even though no field-level trigger is used

## Rule: Validate_Approved_for_Unpriced_Order

### Behavior

This rule runs when `d_oe_header.approved` is edited.

- checking `approved` is allowed when all selected lines have a nonblank, nonzero `unit_price`
- checking `approved` is also allowed when `d_oe_header.ufc_oe_hdr_ud_manager_approved = Y`
- checking `approved` is blocked when any selected line is missing pricing and Manager Approved is not checked
- checking `approved` is also blocked when `oe_hdr_terms = Credit Card` and any required credit-card detail is blank
- unchecking `approved` is allowed silently
- the rule rejects the field edit through `RuleResult`; it does not write back to `approved`
- this rule replaces the active behavior of the older `Existing Rules\OE_CreditCard.dll` rule on the `approved` trigger; keep the old credit-card rule inactive when this combined validator is active

Important implementation detail confirmed by live diagnostics:

- during the `approved` field-edit event, P21 passes the prior value in `Data.TriggerOriginalValue`
- `d_oe_header.approved` in the multi-row dataset may still show the prior value, not the attempted checked value
- the validator therefore treats `Data.TriggerOriginalValue = N` as an attempted approval check and `Data.TriggerOriginalValue = Y` as an uncheck/already-approved edit
- production build version: `1.0.6.0`

Production false-positive lesson:

- order `1281674` had priced parent rows with `detail_type = 0`, plus many component/child rows with `detail_type = 1` and `unit_price = 0`
- the rule must ignore `detail_type = 1` rows or those component rows can incorrectly unapprove a priced order

The credit-card validation mirrors `Existing Rules\OE_CreditCard.dll`:

- terms field: `d_oe_header.oe_hdr_terms`
- credit-card detail fields:
  - `d_oe_payment_details.cc_name`
  - `d_oe_payment_details.cc_creditcard_number`
  - `d_oe_payment_details.cc_expiration_date`
- when terms equal `Credit Card` and Approved is being checked, the first blank detail blocks approval with the same message pattern as the old DLL

Blocked message:

`This order cannot be approved without manager approval`

Credit-card blocked messages:

- `No credit card name has been specified. Order must remain Unapproved until all credit card details are entered.`
- `No credit card number has been specified. Order must remain Unapproved until all credit card details are entered.`
- `No credit card expiration date has been specified. Order must remain Unapproved until all credit card details are entered.`

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
- `d_oe_header.oe_hdr_terms`
- `d_oe_payment_details.cc_creditcard_number`
- `d_oe_payment_details.cc_expiration_date`
- `d_oe_payment_details.cc_name`
- `d_dw_oe_line_dataentry.unit_price`
- `d_dw_oe_line_dataentry.detail_type`
- `d_dw_oe_line_dataentry.cancel_flag`
- `d_dw_oe_line_dataentry.delete_flag`

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
