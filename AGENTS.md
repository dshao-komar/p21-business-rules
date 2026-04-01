# AGENTS.md

## Project Summary

This workspace contains a Prophet 21 / DynaChange business rule project for purchase-order overstock handling.

Primary project folder:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked`

Primary output DLL:

- `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\bin\Debug\Check_Overstocked.dll`

The user provided:

- an Epicor DynaChange Rules PDF guide
- bundled P21 extension source files named `AllFiles - *.cs`
- an example business-rule Visual Studio project

Future work in this project should continue to follow those references.

## Source-of-Truth Inputs

These references were explicitly provided by the user and should continue to guide the implementation:

- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\DynaChange Rules Guide.pdf`
- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\P21.Extensions dll class code\P21.Extensions.BusinessRule\AllFiles - BusinessRule.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\P21.Extensions dll class code\P21.Extensions.DataAccess\AllFiles - DataAccess.cs`
- `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\P21.Extensions dll class code\P21.Extensions.Web\AllFiles - Web.cs`
- Example project:
  - `C:\Users\DanShao\OneDrive - Komar Alliance\Development Backlog\Example Business Rule\Set_Acknowledged_for_POs.sln`
  - `C:\Users\DanShao\OneDrive - Komar Alliance\Development Backlog\Example Business Rule\Set_Acknowledged_and_Shipped_for_POs.csproj`
  - `C:\Users\DanShao\OneDrive - Komar Alliance\Development Backlog\Example Business Rule\Set_Acknowledged_for_POs.cs`
  - `C:\Users\DanShao\OneDrive - Komar Alliance\Development Backlog\Example Business Rule\Set_Acknowledged_for_POs_Callback.cs`

See [Check_Overstocked/AGENTS.md](Check_Overstocked/AGENTS.md) for detailed business goal, implementation details, and configuration specific to the Check_Overstocked project.

## Future-Agent Guidance

When changing this project:

1. Read the current rule files before editing.
2. Preserve the confirmed field/table names unless the user explicitly changes them.
3. Be careful with field-edit validators:
   - direct writeback to the same field during edit can freeze the screen
   - prefer validator rejection when blocking a field edit
4. Keep using `Session.UserID` for the validation rule unless the user asks to change it.
5. If changing messages, keep the business wording aligned with the current live process.
6. Rebuild the DLL after any code change.
7. Do not rely on WSL/Linux to build the DLL; the working compile path is Windows `csc.exe` with the local P21 DLL references.

Recommended manual compile pattern:

```powershell
& 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe' /nologo /target:library /out:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\bin\Debug\Check_Overstocked.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Clients.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.DomainObject.UDF.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.Extensions.dll' /r:'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked\bin\Debug\P21.UI.Service.Model.dll' /r:System.Data.dll /r:System.Core.dll /r:System.Data.DataSetExtensions.dll /r:System.Xml.dll /r:System.Xml.Linq.dll 'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Check_Overstocked.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Validate_Manager_Approval_for_Overstocked_POs.cs' 'C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\Properties\AssemblyInfo.cs'
```

## SQL Database Access

The P21 database can be queried directly from VS Code using the MS SQL extension.

---

### Safety Rules (MANDATORY)
- ONLY run SELECT queries
- NEVER run INSERT, UPDATE, DELETE, MERGE, or DDL (CREATE/ALTER/DROP)
- Assume read-only access at all times

---

### Connection
- Use mssql_list_servers to discover servers
- Use mssql_connect to connect
- Use mssql_run_query to execute queries

---

### Query Guidelines
- Use SELECT TOP n for exploration
- Avoid SELECT * in production queries
- Always filter large tables when possible
- Be cautious with large tables:
  inv_tran, inv_tran_bin_detail, oe_line

---

### Schema Discovery
- Tables:
  SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'

- Columns:
  SELECT COLUMN_NAME, DATA_TYPE
  FROM INFORMATION_SCHEMA.COLUMNS
  WHERE TABLE_NAME = 'inv_mast'

---

### Key Tables
- inv_mast — item master (inv_mast_uid, item_id, description)
- inv_loc — inventory by location
- prod_order_hdr — production orders
- prod_order_line — production lines
- prod_order_line_component — raw materials
- oe_hdr / oe_line — sales orders

---

### Common Join Patterns
- inv_mast ↔ inv_loc via inv_mast_uid
- prod_order_hdr ↔ prod_order_line via prod_order_number
- prod_order_line_component ↔ inv_mast via inv_mast_uid
- oe_hdr ↔ oe_line via order_no

---

### Time Handling
- Convert to Pacific Time when needed:
  your_date_column AT TIME ZONE 'UTC' AT TIME ZONE 'Pacific Standard Time'

## Caveats

- The SMTP email in `Check_Overstocked` is best-effort only and failures are swallowed.
- Power Automate import/export artifacts were not created; only documentation and SQL patterns exist.
- The P21 guide and Power Automate guide should be updated if rule behavior changes.
