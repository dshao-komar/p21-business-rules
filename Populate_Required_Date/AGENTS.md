# Populate_Required_Date Project Details

## Business Goal

The main business rule is `Populate_Required_Date`.

Intent:

- run on production-order save in multi-row mode
- inspect all active finished-item rows from `d_prod_order_line`
- find the earliest qualifying open sales order for any finished item on the production order
- populate the production-order header fields:
  - `d_prod_order_hdr.required_date`
  - `d_prod_order_hdr.ufc_prod_order_hdr_ud_customer_name`
- do not overwrite either field when no qualifying open sales order is found

## Confirmed Dataset Fields

Confirmed production-order dataset fields for this rule:

- `d_prod_order_hdr.required_date`
- `d_prod_order_hdr.ufc_prod_order_hdr_ud_customer_name`
- `d_prod_order_line.inv_mast_uid`
- `d_prod_order_line.item_id`
- `d_prod_order_line.cancel`

## Confirmed SQL / Business Logic

### Open sales order filters

Qualifying sales-order rows must meet all of the following:

- `oe_line.complete = 'N'`
- `oe_line.detail_type <> 1`
- `COALESCE(oe_hdr.order_type, 0) NOT IN (1877, 1706)`
- `ISNULL(oe_hdr.rma_flag, 'N') <> 'Y'`
- `ISNULL(oe_hdr.warranty_rma_flag, 'N') <> 'Y'`
- `COALESCE(oe_line_schedule.release_date, oe_line.required_date) IS NOT NULL`

### Required date source

The rule must mirror the `required_dt` logic used in `fact_open_orders`:

- if a release schedule exists for the sales-order line, use `oe_line_schedule.release_date`
- otherwise use `oe_line.required_date`

Current implementation:

- `required_date = COALESCE(oe_line_schedule.release_date, oe_line.required_date)`
- `qty_ordered = COALESCE(oe_line_schedule.release_qty, oe_line.qty_ordered)` for tie-break consistency when a release schedule row exists

### Finished-item scope

The rule evaluates all active finished-item rows on the current production order and writes one header result for the entire order.

Excluded production-order rows:

- `d_prod_order_line.cancel = 'Y'`
- rows with missing or non-positive `inv_mast_uid`

### Tie-break rules

The winner is selected using this order:

1. earliest `oe_line.required_date`
2. highest `oe_line.qty_ordered`
3. alphabetical `customer_name`

The implementation also applies `order_no` as a final deterministic fallback after the confirmed business tie-breakers so the SQL always returns one row consistently.

### Overwrite behavior

- if a qualifying open sales order is found, overwrite both header fields on save
- if no qualifying open sales order is found, leave the current header values unchanged

## Current Rule Files

Main code files:

- `C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\Populate_Required_Date.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\Populate_Required_Date.csproj`
- `C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\Properties\AssemblyInfo.cs`

Primary output DLL:

- `C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\bin\Debug\Populate_Required_Date.dll`

## Build Notes

Compile with the Windows .NET Framework compiler and the existing local P21 DLL references:

```powershell
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:library /out:'C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\bin\Debug\Populate_Required_Date.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Clients.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.UDF.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Extensions.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.UI.Service.Model.dll' /r:System.Data.dll /r:System.Core.dll /r:System.Data.DataSetExtensions.dll /r:System.Xml.dll /r:System.Xml.Linq.dll 'C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\Populate_Required_Date.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\Properties\AssemblyInfo.cs'
```

## P21 Configuration Summary

Recommended rule setup:

- `Rule Name`: `Populate_Required_Date`
- `Rule Type`: `Validator`
- `Apply Rule On`: `Save`
- `Multi-Row`: checked

### Field Selector Setup

Select these dataset fields for the rule so the save-time multi-row dataset contains everything the rule reads or writes:

- `d_prod_order_hdr.required_date`
- `d_prod_order_hdr.ufc_prod_order_hdr_ud_customer_name`
- `d_prod_order_line.inv_mast_uid`
- `d_prod_order_line.item_id`
- `d_prod_order_line.cancel`

### Triggered Fields

This rule runs on `Save`, not `Field Edit`, so there is no single field-specific trigger to configure.

- `Triggered field(s)`: none
- `Execution event`: production-order `Save`
- `Important`: the rule still depends on the Field Selector entries above being included in the multi-row dataset even though no field-level trigger is used

### Setup Checklist

Use this exact setup pattern in Prophet 21:

1. Create or import the rule from `Populate_Required_Date.dll`.
2. Set `Rule Type` to `Validator`.
3. Set `Apply Rule On` to `Save`.
4. Check `Multi-Row`.
5. In Field Selector, add:
   - `d_prod_order_hdr.required_date`
   - `d_prod_order_hdr.ufc_prod_order_hdr_ud_customer_name`
   - `d_prod_order_line.inv_mast_uid`
   - `d_prod_order_line.item_id`
   - `d_prod_order_line.cancel`
6. Do not configure a field-edit trigger for this rule.

Attach the compiled DLL from:

- `C:\Users\DanShao\.vscode\p21_business_rules\Populate_Required_Date\bin\Debug\Populate_Required_Date.dll`

## References

Continue to align future changes to these source materials:

- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\DynaChange Rules Guide.pdf`
- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\AllFiles - BusinessRule.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\AllFiles - DataAccess.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\AllFiles - Web.cs`

## Nightly SQL Alignment

If a nightly SQL synchronization job is used to backfill or correct production-order header values, keep it aligned with the same open-sales-order filters as the DLL:

- exclude `oe_hdr.rma_flag = 'Y'`
- exclude `oe_hdr.warranty_rma_flag = 'Y'`
- derive `required_date` from `COALESCE(oe_line_schedule.release_date, oe_line.required_date)`
- keep the same tie-break order:
  1. earliest `required_date`
  2. highest `qty_ordered`
  3. alphabetical `customer_name`
  4. lowest `order_no` as final deterministic fallback
