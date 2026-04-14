# Set_Acknowledged_for_POs

## Project Summary

This folder contains maintainable source for the existing PO header-to-line propagation DLL:

- `C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\Set_Acknowledged_for_POs.dll`

The DLL contains multiple P21 business rule classes:

- `Set_Acknowledged_for_POs`
- `Set_Acknowledged_for_POs_Callback`
- `Set_Scheduled_for_POs`

## Set_Scheduled_for_POs

### Business Goal

When the PO header Scheduled checkbox is changed, copy that value to every PO line Scheduled checkbox.

The scheduled rule is intentionally separate from the acknowledged/shipped rule so its Prophet 21 field selector only needs the scheduled fields.

### Rule Setup

- Rule Name: `Set_Scheduled_for_POs`
- Rule Type: `Validator`
- Apply Rule On: `Field Edit`
- Multi-Row: checked
- DLL path: `C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\Set_Acknowledged_for_POs.dll`
- Callback Rule: none

### Field Selector Setup

Select these fields for the scheduled rule:

- `d_po_purchase_order_sheet.ufc_po_hdr_ud_scheduled`
- `d_po_ext_info_sheet.ufc_po_line_ud_scheduled`

The scheduled rule does not require these acknowledged/shipped fields:

- `d_po_purchase_order_sheet.ufc_po_hdr_ud_acknowledged`
- `d_po_purchase_order_sheet.ufc_po_hdr_ud_acknowledged_date`
- `d_po_purchase_order_sheet.ufc_po_hdr_ud_supplier_ship_date`
- `d_po_purchase_order_sheet.ufc_po_hdr_ud_shipped_flag`
- `d_po_ext_info_sheet.acknowledged`
- `d_po_ext_info_sheet.acknowledged_date`
- `d_po_ext_info_sheet.supplier_ship_date`
- `d_po_ext_info_sheet.c_shipped_flag`

### Triggered Fields

The scheduled rule should trigger on:

- `d_po_purchase_order_sheet.ufc_po_hdr_ud_scheduled`

### Behavior

- Header values `Y`, `YES`, `1`, and `true` are copied to lines as `Y`.
- Header values blank, `N`, `NO`, `0`, and `false` are copied to lines as `N`.
- Every selected PO line is overwritten silently.
- The rule returns an error if it is not run in Multi-Row mode.
- The rule returns an error if the PO line table or line scheduled column is missing from the selected dataset.

## Existing Acknowledged/Shipped Rules

The existing rule classes are preserved in this DLL:

- `Set_Acknowledged_for_POs`
- `Set_Acknowledged_for_POs_Callback`

The acknowledged/shipped field selector and trigger setup should remain as previously configured for those rules.

## Manual Compile

Use Windows `csc.exe`; do not rely on WSL/Linux to build the DLL.

Recommended compile command:

```powershell
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:library /out:'C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\Set_Acknowledged_for_POs.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Clients.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.UDF.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Extensions.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.UI.Service.Model.dll' /r:System.Data.dll /r:System.Core.dll /r:System.Data.DataSetExtensions.dll /r:System.Xml.dll /r:System.Xml.Linq.dll 'C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\new\Set_Acknowledged_for_POs\Set_Acknowledged_for_POs.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\new\Set_Acknowledged_for_POs\Set_Acknowledged_for_POs_Callback.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\new\Set_Acknowledged_for_POs\Set_Scheduled_for_POs.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\new\Set_Acknowledged_for_POs\Properties\AssemblyInfo.cs'
```
