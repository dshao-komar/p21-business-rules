using System;
using System.Data;
using System.Linq;
using P21.Extensions.BusinessRule;

namespace Set_Acknowledged_for_POs
{
    public class Set_Scheduled_for_POs : P21.Extensions.BusinessRule.Rule
    {
        private const string HeaderScheduledField = "ufc_po_hdr_ud_scheduled";
        private const string LineScheduledField = "ufc_po_line_ud_scheduled";

        public override RuleResult Execute()
        {
            var rr = new RuleResult();
            DataSet ds;

            try { ds = Data.Set; }
            catch
            {
                rr.Success = false;
                rr.Message = "Set_Scheduled_for_POs must run in Multi-Row mode.";
                return rr;
            }

            string trigger = Data.TriggerColumn;
            if (!string.IsNullOrEmpty(trigger) && !IsScheduledTrigger(trigger))
            {
                return rr;
            }

            if (!HasHeaderColumn(ds, HeaderScheduledField))
            {
                rr.Success = false;
                rr.Message = string.Format("PO header table is missing {0}.", HeaderScheduledField);
                return rr;
            }

            string lineTableName = ResolveLineTableName(ds);
            if (lineTableName == null)
            {
                rr.Success = false;
                rr.Message = "Could not find PO line table.";
                return rr;
            }

            var lines = ds.Tables[lineTableName];
            if (!lines.Columns.Contains(LineScheduledField))
            {
                rr.Success = false;
                rr.Message = string.Format("PO line table is missing {0}.", LineScheduledField);
                return rr;
            }

            object headerValue = TryGetHeaderObject(ds, HeaderScheduledField);
            string normalizedValue = NormalizeYN(Convert.ToString(headerValue));

            foreach (DataRow row in lines.Rows)
                row[LineScheduledField] = normalizedValue;

            rr.Success = true;
            rr.Message = "Scheduled Checkbox copied silently to all PO Lines.";
            return rr;
        }

        public override string GetDescription()
        {
            return "Copies the header Scheduled checkbox to all PO line Scheduled checkboxes.";
        }

        public override string GetName()
        {
            return "Set_Scheduled_for_POs";
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

        private bool HasHeaderColumn(DataSet ds, string column)
        {
            if (ds.Tables.Contains("po_hdr_ud") && ds.Tables["po_hdr_ud"].Columns.Contains(column))
                return true;
            if (ds.Tables.Contains("d_po_purchase_order_sheet") && ds.Tables["d_po_purchase_order_sheet"].Columns.Contains(column))
                return true;
            return false;
        }

        private bool IsScheduledTrigger(string trigger)
        {
            return HeaderScheduledField.Equals(trigger, StringComparison.OrdinalIgnoreCase) ||
                   trigger.EndsWith("." + HeaderScheduledField, StringComparison.OrdinalIgnoreCase);
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
    }
}
