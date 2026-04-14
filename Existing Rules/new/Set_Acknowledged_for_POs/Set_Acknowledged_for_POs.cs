using System;
using System.Data;
using System.Linq;
using P21.Extensions.BusinessRule;

namespace Set_Acknowledged_for_POs
{
    public class Set_Acknowledged_for_POs : P21.Extensions.BusinessRule.Rule
    {
        public override RuleResult Execute()
        {
            var rr = new RuleResult();
            DataSet ds;

            try { ds = Data.Set; }
            catch
            {
                rr.Success = false;
                rr.Message = "This rule must run in Multi-Row mode.";
                return rr;
            }

            string trigger = Data.TriggerColumn == null ? null : Data.TriggerColumn.ToLowerInvariant();
            if (string.IsNullOrEmpty(trigger))
                return rr;

            string lineTable = ResolveLineTableName(ds);
            if (lineTable == null)
            {
                rr.Success = false;
                rr.Message = "Could not find PO line table.";
                return rr;
            }

            var map = new[]
            {
                new { Header = "ufc_po_hdr_ud_acknowledged", Line = "acknowledged", Desc = "Acknowledged Checkbox" },
                new { Header = "ufc_po_hdr_ud_acknowledged_date", Line = "acknowledged_date", Desc = "Acknowledged Date" },
                new { Header = "ufc_po_hdr_ud_supplier_ship_date", Line = "supplier_ship_date", Desc = "Actual Supplier Ship Date" },
                new { Header = "ufc_po_hdr_ud_shipped_flag", Line = "c_shipped_flag", Desc = "Shipped Flag" }
            };

            var current = map.FirstOrDefault(m => m.Header.Equals(trigger, StringComparison.OrdinalIgnoreCase));
            if (current == null)
                return rr;

            object headerValue = TryGetHeaderObject(ds, current.Header);
            bool isDateField = current.Header.Contains("_date");
            bool isFlagField = current.Header.Contains("_flag") || current.Header.Contains("acknowledged");

            var lines = ds.Tables[lineTable];

            if (isFlagField && !isDateField)
            {
                foreach (DataRow row in lines.Rows)
                {
                    if (!lines.Columns.Contains(current.Line)) continue;
                    row[current.Line] = NormalizeYN(Convert.ToString(headerValue));
                }

                rr.Success = true;
                rr.Message = string.Format("{0} copied silently to all PO Lines.", current.Desc);
                return rr;
            }

            if (isDateField)
            {
                string newDateStr = headerValue == null ? "" : Convert.ToDateTime(headerValue).ToString("yyyy-MM-dd");

                bool anyDifferent = lines.Rows.Cast<DataRow>().Any(r =>
                    r.Table.Columns.Contains(current.Line) &&
                    r[current.Line] != DBNull.Value &&
                    !Convert.ToDateTime(r[current.Line]).ToString("yyyy-MM-dd").Equals(newDateStr));

                if (anyDifferent)
                {
                    rr.ShowResponse = true;
                    rr.ResponseAttributes = new ResponseAttributes
                    {
                        ResponseTitle = string.Format("{0} Update Confirmation", current.Desc),
                        ResponseText = string.Format("One or more PO lines already have a different {0}. ", current.Desc) +
                                       "Do you want to overwrite them with the header value?",
                        CallbackRule = "Set_Acknowledged_for_POs_Callback",
                        Buttons = new[]
                        {
                            new ResponseButton { ButtonName = "YES", ButtonText = "Yes", ButtonValue = "YES" },
                            new ResponseButton { ButtonName = "NO", ButtonText = "No", ButtonValue = "NO" }
                        }
                    };
                    return rr;
                }

                foreach (DataRow r in lines.Rows)
                {
                    if (lines.Columns.Contains(current.Line))
                        r[current.Line] = headerValue ?? DBNull.Value;
                }

                rr.Success = true;
                rr.Message = string.Format("{0} copied silently (no differences found).", current.Desc);
                return rr;
            }

            return rr;
        }

        private string ResolveLineTableName(DataSet ds)
        {
            if (ds.Tables.Contains("d_po_ext_info_sheet")) return "d_po_ext_info_sheet";
            if (ds.Tables.Contains("po_line")) return "po_line";
            return null;
        }

        private object TryGetHeaderObject(DataSet ds, string column)
        {
            DataRow hdr = null;
            if (ds.Tables.Contains("po_hdr_ud"))
                hdr = ds.Tables["po_hdr_ud"].Rows.Cast<DataRow>().FirstOrDefault();
            if (hdr == null && ds.Tables.Contains("d_po_purchase_order_sheet"))
                hdr = ds.Tables["d_po_purchase_order_sheet"].Rows.Cast<DataRow>().FirstOrDefault();
            if (hdr != null && hdr.Table.Columns.Contains(column))
                return hdr[column] == DBNull.Value ? null : hdr[column];
            return null;
        }

        private string NormalizeYN(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "N";
            s = s.Trim();
            if (s.Equals("Y", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("YES", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("1")) return "Y";
            if (s.Equals("N", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("NO", StringComparison.OrdinalIgnoreCase) ||
                s.Equals("0")) return "N";
            bool b;
            if (bool.TryParse(s, out b)) return b ? "Y" : "N";
            return s;
        }

        public override string GetDescription()
        {
            return "Copies header UD fields (Acknowledged, Acknowledged Date, Supplier Ship Date, Shipped Flag) to PO Lines.";
        }

        public override string GetName()
        {
            return "Set_Acknowledged_and_Shipped_for_POs";
        }
    }
}
