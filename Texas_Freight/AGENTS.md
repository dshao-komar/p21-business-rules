# Texas_Freight Project Details

## Business Goal

This folder contains a sales-order save rule for taxable orders shipping to Texas.

Intent:

- when a taxable sales order ships to Texas, force the freight code to `COLLECT`
- allow a non-COLLECT freight code only when Manager Approved is checked
- make the order taker workflow easy by correcting the freight code automatically instead of blocking the save

## Current Rule Files

Main code files:

- `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Texas_Freight.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Texas_Freight.csproj`
- `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Properties\AssemblyInfo.cs`

Primary output DLL:

- `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\bin\Debug\Texas_Freight.dll`

## Source Request

Original request PDF:

- `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Outlook Email - Request - Business Rule for Order Entry Texas State Sales Tax - Dan Shao - Outlook.pdf`

Confirmed implementation adjustment:

- instead of marking the order unapproved when freight is not COLLECT, the rule now auto-sets the freight code to COLLECT
- a non-COLLECT freight code is allowed only when Manager Approved is checked

## Confirmed P21 Dataset Fields

These fields are used by the rule:

- `d_oe_header.ship2_state`
- `d_oe_header.freight_code_uid`
- `d_oe_header.ufc_oe_hdr_ud_manager_approved`
- `d_dw_oe_line_dataentry.sales_tax`
- `d_dw_oe_line_dataentry.detail_type`
- `d_dw_oe_line_dataentry.oe_line_cancel_flag`
- `d_dw_oe_line_dataentry.delete_flag`

## Rule: Texas_Freight

### Behavior

This rule runs on sales-order save in multi-row mode.

The rule applies when:

- `d_oe_header.ship2_state = TX`
- `d_oe_header.ufc_oe_hdr_ud_manager_approved <> Y`
- at least one active sales-order line has nonzero `d_dw_oe_line_dataentry.sales_tax`
- `d_oe_header.freight_code_uid <> 2`

When all conditions are met, the rule sets:

- `d_oe_header.freight_code_uid = 2`

Freight code `2` is `COLLECT` per the source request:

- `freight_code_uid`: `2`
- `freight_cd`: `COLLECT`
- `freight_desc`: `Freight Collect`

The popup message is:

`This taxable Texas order requires freight code COLLECT. Freight code has been changed to COLLECT.`

The rule intentionally does not change `d_oe_header.approved`.

### Active Line Logic

The rule evaluates only active parent/order rows in `d_dw_oe_line_dataentry`:

- `detail_type = 0`
- `oe_line_cancel_flag <> Y`
- `delete_flag <> Y`

Canceled, deleted, and component/child rows are ignored.

Taxable means:

- `d_dw_oe_line_dataentry.sales_tax` parses to a nonzero decimal

Blank, null, or nonnumeric `sales_tax` values are treated as zero tax.

### P21 Configuration Summary

- `Rule Name`: `Texas_Freight`
- `Rule Type`: `Validator`
- `Apply Rule On`: `Save`
- `Multi-Row`: checked
- `Callback Rule`: none
- `DLL path`: `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\bin\Debug\Texas_Freight.dll`

### Field Selector Setup

Select these fields for the save rule:

- `d_oe_header.ship2_state`
- `d_oe_header.freight_code_uid`
- `d_oe_header.ufc_oe_hdr_ud_manager_approved`
- `d_dw_oe_line_dataentry.sales_tax`
- `d_dw_oe_line_dataentry.detail_type`
- `d_dw_oe_line_dataentry.oe_line_cancel_flag`
- `d_dw_oe_line_dataentry.delete_flag`

### Triggered Fields

This rule runs on `Save`, not `Field Edit`, so there is no single field-specific trigger to configure.

- `Triggered field(s)`: none
- `Execution event`: sales-order `Save`
- `Important`: the rule still depends on the Field Selector entries above being included in the multi-row dataset even though no field-level trigger is used

## Setup References

Setup labels and patterns were verified against:

- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\DynaChange Rules Guide.pdf`
- `C:\Users\DanShao\.vscode\p21_business_rules\Unapproved_Order_No_Price\AGENTS.md`
- `C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Outlook Email - Request - Business Rule for Order Entry Texas State Sales Tax - Dan Shao - Outlook.pdf`

## Build Notes

Compile with the Windows .NET Framework compiler and the existing local P21 DLL references:

```powershell
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:library /out:'C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\bin\Debug\Texas_Freight.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Clients.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.UDF.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Extensions.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.UI.Service.Model.dll' /r:System.Data.dll /r:System.Core.dll /r:System.Data.DataSetExtensions.dll /r:System.Xml.dll /r:System.Xml.Linq.dll 'C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Texas_Freight.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Texas_Freight\Properties\AssemblyInfo.cs'
```

## Test Scenarios

- TX ship-to, taxable active line, Manager Approved unchecked, freight code not `2`: sets freight code to `2` and returns popup message
- TX ship-to, taxable active line, Manager Approved unchecked, freight code already `2`: no change and no message
- TX ship-to, taxable active line, Manager Approved checked, freight code not `2`: no change and no message
- TX ship-to, no taxable active lines: no change and no message
- non-TX ship-to, taxable active line, non-COLLECT freight: no change and no message
- canceled, deleted, and component lines with sales tax are ignored
- missing Field Selector field returns a clear setup error
