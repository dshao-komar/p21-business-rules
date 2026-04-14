using System;
using System.Data;
using System.Linq;
using P21.Extensions.BusinessRule;

namespace Set_Acknowledged_for_POs
{
    public class Set_Acknowledged_for_POs_Callback : P21.Extensions.BusinessRule.Rule
    {
        public override string GetName()
        {
            return "Set_Acknowledged_for_POs_Callback";
        }

        public override string GetDescription()
        {
            return "Executes when user clicks Yes on the confirmation popup and copies the header PO date field to all PO lines.";
        }

        public override RuleResult Execute()
        {
            var rr = new RuleResult();
            DataSet ds;

            try { ds = Data.Set; }
            catch
            {
                rr.Success = false;
                rr.Message = "Must be a Multi-Row callback.";
                return rr;
            }

            string lineTableName = ResolveLineTableName(ds);
            if (lineTableName == null)
            {
                rr.Success = false;
                rr.Message = "Line table not found.";
                return rr;
            }

            var lines = ds.Tables[lineTableName];

            object hdrAckDate = TryGetHeaderObject(ds, "ufc_po_hdr_ud_acknowledged_date");
            object hdrSupplierDate = TryGetHeaderObject(ds, "ufc_po_hdr_ud_supplier_ship_date");

            bool triggeredByAckDate = hdrAckDate != null &&
                                      Data.TriggerColumn != null &&
                                      Data.TriggerColumn.Equals("ufc_po_hdr_ud_acknowledged_date",
                                          StringComparison.OrdinalIgnoreCase);

            bool triggeredBySupplierDate = hdrSupplierDate != null &&
                                           Data.TriggerColumn != null &&
                                           Data.TriggerColumn.Equals("ufc_po_hdr_ud_supplier_ship_date",
                                               StringComparison.OrdinalIgnoreCase);

            if (!triggeredByAckDate && !triggeredBySupplierDate)
            {
                if (hdrSupplierDate != null && hdrAckDate == null)
                    triggeredBySupplierDate = true;
                else
                    triggeredByAckDate = true;
            }

            int updated = 0;

            if (triggeredByAckDate)
            {
                if (lines.Columns.Contains("acknowledged_date"))
                {
                    foreach (DataRow r in lines.Rows)
                    {
                        r["acknowledged_date"] = hdrAckDate ?? DBNull.Value;
                        updated++;
                    }
                }

                rr.Success = true;
                rr.Message = string.Format("Overrode {0} line Acknowledged Date values from header.", updated);
                return rr;
            }

            if (triggeredBySupplierDate)
            {
                if (lines.Columns.Contains("supplier_ship_date"))
                {
                    foreach (DataRow r in lines.Rows)
                    {
                        r["supplier_ship_date"] = hdrSupplierDate ?? DBNull.Value;
                        updated++;
                    }
                }

                rr.Success = true;
                rr.Message = string.Format("Overrode {0} line Supplier Ship Date values from header.", updated);
                return rr;
            }

            rr.Success = false;
            rr.Message = "Callback could not determine which field to update.";
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
    }
}
