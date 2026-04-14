# AGENTS.md

## Project Summary

This workspace contains a Prophet 21 / DynaChange business rule project for purchase-order overstock handling.

Primary project folder:

- `C:\Users\DanShao\.vscode\p21_business_rules`

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
2. Make a detailed AGENTS.md file within the rule folder that documents exactly how to setup the rule. See example: `C:\Users\DanShao\.vscode\p21_business_rules\Check_Overstocked\AGENTS.md`
   - include an explicit `Field Selector Setup` section
   - list every field that must be selected in Prophet 21
   - list every field that is used as a trigger, or explicitly state that there are no field-specific triggers for save rules
   - do not leave Field Selector details implied by the code
3. Preserve the confirmed field/table names unless the user explicitly changes them.
4. Be careful with field-edit validators:
   - direct writeback to the same field during edit can freeze the screen
   - prefer validator rejection when blocking a field edit
5. Keep using `Session.UserID` for the validation rule unless the user asks to change it.
6. If changing messages, keep the business wording aligned with the current live process.
7. Rebuild the DLL after any code change.
8. Do not rely on WSL/Linux to build the DLL; the working compile path is Windows `csc.exe` with the local P21 DLL references.
9. After any substantial code or SQL logic change, create a git commit and push it unless the user explicitly asks not to.

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

## Rule Setup Documentation Standard

For every business-rule folder, the local `AGENTS.md` must document the Prophet 21 setup in enough detail that someone can configure the rule without reverse-engineering the code.

Rule setup values must be verified against source-of-truth documentation before they are written:

- Do not infer Prophet 21 setup labels from code behavior, class names, or plain-English descriptions.
- `Rule Type`, `Apply Rule On`, Multi-Row behavior, callback setup, and field selector/trigger instructions must use the exact valid labels from the official/reference material.
- Acceptable references are:
  - `C:\Users\DanShao\.vscode\p21_business_rules\Rules References\DynaChange Rules Guide.pdf`
  - the bundled `AllFiles - *.cs` P21 extension source files under `Rules References`
  - an existing confirmed local rule `AGENTS.md` only when it is clearly documenting the same P21 setup pattern
  - explicit user confirmation in the current thread
- If a setup value cannot be verified from those references, write `NEEDS CONFIRMATION` for that value instead of guessing.
- When adding or changing setup values, include a short `Setup References` section in the rule-folder `AGENTS.md` naming the reference used for `Rule Type` and `Apply Rule On`.

Minimum required setup details:

- `Rule Name`
- `Rule Type`
- `Apply Rule On`
- whether `Multi-Row` is checked
- `Field Selector Setup` with each selected field listed individually
- `Triggered Fields` with each trigger field listed individually, or an explicit note that no field-level trigger applies
- DLL path to attach

For save-time validator rules in particular:

- state clearly that there is no field-edit trigger
- still list every header and line field that must be selected so the multi-row dataset includes all required inputs and outputs
