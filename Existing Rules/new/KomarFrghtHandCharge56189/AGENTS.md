# KomarFrghtHandCharge56189

## Rule Name

- `KomarFrghtHandCharge56189`

## Rule Type

- Assumed `Form` rule

## Apply Rule On

- Assumed purchase order order-entry event where the header freight charge is recalculated from header values

## Multi-Row

- Assumed `unchecked`

## DLL Path To Attach

- `C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\new\KomarFrghtHandCharge56189\bin\Debug\KomarFrghtHandCharge56189.dll`

## Field Selector Setup

Select each of these fields explicitly:

- `global_server`
- `global_database`
- `rma_flag`
- `exempt_delivery_charge`
- `ship_to_id`
- `ship_route`
- `sales_sub_total`
- `freight_out`

Notes:

- `global_server` and `global_database` were required by the legacy DLL.
- the rebuilt DLL no longer uses those values for the SQL connection, but they are still listed here because they were part of the original selector footprint and we have not yet validated whether removing them from the rule setup has any side effects in this environment

## Triggered Fields

The original DLL does not embed enough setup metadata to prove the exact trigger field list.

Best reconstruction from the logic:

- `ship_to_id`
- `ship_route`
- `sales_sub_total`
- `freight_out`
- `rma_flag`
- `exempt_delivery_charge`

## Behavioral Notes

- if `rma_flag = Y`, the rule exits without changing freight
- if header `exempt_delivery_charge = Y`, the rule exits without changing freight
- if `ship_to_ud.exempt_delivery_charges = Y`, the rule exits without changing freight
- if the shipping route does not require handling charge, the rule exits without changing freight
- if `0 < sales_sub_total < 250`, the rule sets `freight_out` to `15.00`
- otherwise, the legacy rule writes the current `sNewFreightOut` field value back to `freight_out`
- the legacy implementation leaves `sNewFreightOut` as an empty string in that branch, so the rebuilt DLL intentionally preserves that behavior for parity testing

## Database Access Change

The old DLL manually built an `OleDb` connection string with:

- `provider=sqloledb`
- `server=global_server`
- `database=global_database`
- `Integrated Security=SSPI`

The rebuilt DLL instead uses `Rule.P21SqlConnection`, which uses the active Prophet 21 server/database/session context and avoids failures in environments where the company database is not the default `P21` catalog.
