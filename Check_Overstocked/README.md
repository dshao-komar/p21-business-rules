# Check_Overstocked

This project is scaffolded from the supplied Prophet 21 example rule and aligned to the Epicor `DynaChange Rules Guide.pdf` sections on:

- `Create a Visual Studio Project`
- `How They Work`
- `Coding Callback Rules`
- `Link Business Rules to Prophet 21`

Current status:

- The project structure matches the sample `.sln`/`.csproj` pattern and targets `.NET Framework 4.7.2`.
- The rule class derives from `P21.Extensions.BusinessRule.Rule` and uses `Data.Set` in multi-row mode, matching the SDK contract shown in the bundled `AllFiles - BusinessRule.cs`.
- The rule now uses the supplied PO dataset fields:
  - `d_po_item_sheet.item_id`
  - `d_po_item_sheet.inv_mast_uid`
  - `d_po_item_sheet.qty_ordered`
  - `d_po_item_sheet.location_id`
  - `d_po_item_sheet.unit_of_measure`
  - `d_po_classes_sheet.expected_date`
- The inventory check now combines:
  - current `inv_loc.qty_on_hand`
  - other open inbound PO quantity due on or before the line expected date
  - current PO inbound quantity due on or before the line expected date
  - usage windows from `inv_period_usage` and `demand_period`
- Zero-usage items are excluded from the rule.
- The rule currently prefers `usage3`, then falls back to `usage6`, then `usage12` when the shorter window is zero.
- The project now points to `Check_Overstocked\bin\Debug\` for the Prophet 21 DLL references, matching the example pattern you described.

Before treating this as production-ready, confirm these remaining items in your database:

1. Which PO screen event should fire the rule that blocks save.
2. The exact `po_line` database columns for:
   - expected date
   - ordered quantity
   - received quantity
   - completion / cancellation flags
3. Whether the monthly-usage window should prefer:
   - usage3
   - usage6
   - usage12
   - another explicit business rule
4. Whether a unit-of-measure mismatch between PO line UOM and default purchasing UOM should be converted or skipped.
5. Whether non-stock, special-order, or direct-ship lines need explicit exclusion columns later.

Suggested SQL proof checks in Prophet 21:

1. Recreate a known overstock example, such as item `MRDSAS2366` from PO `6067986`, and verify:
   - on hand
   - other open inbound due on or before expected date
   - current PO quantity
   - usage3 / usage6 / usage12
   - projected months available
2. Confirm which `po_line` columns the rule should use for expected date and received quantity.
3. Test at least one item expected to pass and one expected to fail the 3-month rule.
