# KomarFrghtHandCharge56189 Regression Test Plan

## Goal

Verify that the rebuilt DLL matches the old DLL's pass/fail and field-write behavior in P21Import, except for the database-login failure caused by the legacy custom connection path.

## Test Harness

- Keep the old DLL available:
  - `C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\KomarFrghtHandCharge56189.dll`
- Use the rebuilt DLL separately:
  - `C:\Users\DanShao\.vscode\p21_business_rules\Existing Rules\new\KomarFrghtHandCharge56189\bin\Debug\KomarFrghtHandCharge56189.dll`
- Run the same import/input set twice:
  - once with old DLL attached
  - once with new DLL attached
- Capture for each run:
  - whether the rule executes successfully
  - exact final `freight_out`
  - any rule message shown
  - whether save/import succeeds or fails

## Core Cases

1. Non-`P21` test database
- Input: any record that causes the legacy rule to query the database
- Old DLL expected: database login failure like `Cannot open database ...`
- New DLL expected: no database catalog login failure

2. `rma_flag = Y`
- Expected old/new parity: no freight change

3. Header exempt order
- Input: `exempt_delivery_charge = Y`
- Expected old/new parity: no freight change

4. Ship-to exempt
- Input: `ship_to_ud.exempt_delivery_charges = Y`
- Expected old/new parity: no freight change

5. Route not found
- Input: `ship_route` value missing from `shipping_route`
- Expected old/new parity: no freight change

6. Route found, no handling charge
- Input: route exists, `require_handling_charge <> Y`
- Expected old/new parity: no freight change

7. Route found, handling charge required, subtotal between 0 and 250
- Input: `0 < sales_sub_total < 250`
- Expected old/new parity: `freight_out` becomes `15.00`

8. Route found, handling charge required, subtotal exactly 250
- Input: `sales_sub_total = 250.00`
- Expected old/new parity: use legacy else-branch behavior
- Important: legacy code writes `sNewFreightOut`, which is typically an empty string in this branch

9. Route found, handling charge required, subtotal greater than 250
- Input: `sales_sub_total > 250`
- Expected old/new parity: use legacy else-branch behavior

10. Route found, handling charge required, subtotal zero
- Input: `sales_sub_total = 0`
- Expected old/new parity: use legacy else-branch behavior

11. Blank freight_out with subtotal below 250
- Input: blank `freight_out`, `0 < sales_sub_total < 250`
- Expected old/new parity: `freight_out` becomes `15.00`

12. Existing nonblank freight_out with subtotal below 250
- Input: populated `freight_out`, `0 < sales_sub_total < 250`
- Expected old/new parity: `freight_out` still becomes `15.00`

## Data Points To Capture

For every case, record:

- company database name
- server
- ship-to
- route
- `require_handling_charge`
- `exempt_delivery_charge`
- `rma_flag`
- `sales_sub_total`
- starting `freight_out`
- ending `freight_out`
- import/save success
- exact message text

## Expected Difference

The only intended difference is:

- the new DLL should use the active P21 database context and therefore should not fail just because the company database name is not `P21`

Any other behavior difference should be treated as a regression and investigated before rollout.
