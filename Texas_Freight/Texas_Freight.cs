using System;
using System.Data;
using System.Globalization;
using System.Linq;
using P21.Extensions.BusinessRule;

namespace Texas_Freight
{
    public class Texas_Freight : P21.Extensions.BusinessRule.Rule
    {
        private const string HeaderTableName = "d_oe_header";
        private const string LineTableName = "d_dw_oe_line_dataentry";
        private const string ShipToStateFieldName = "oe_hdr_ship2_state";
        private const string FreightCodeUidFieldName = "freight_code_uid";
        private const string ManagerApprovedFieldName = "ufc_oe_hdr_ud_manager_approved";
        private const string SalesTaxFieldName = "sales_tax";
        private const string DetailTypeFieldName = "detail_type";
        private const string CancelFlagFieldName = "oe_line_cancel_flag";
        private const string DeleteFlagFieldName = "delete_flag";
        private const int CollectFreightCodeUid = 2;

        public override string GetName()
        {
            return "Texas_Freight";
        }

        public override string GetDescription()
        {
            return "For taxable Texas sales orders, sets freight code to COLLECT unless Manager Approved is checked.";
        }

        public override RuleResult Execute()
        {
            RuleResult result = new RuleResult();
            DataSet dataSet;

            try
            {
                dataSet = Data.Set;
            }
            catch
            {
                result.Success = false;
                result.Message = "Texas_Freight must run as a multi-row sales-order save rule.";
                return result;
            }

            string setupError = ValidateDataset(dataSet);
            if (!string.IsNullOrWhiteSpace(setupError))
            {
                result.Success = false;
                result.Message = setupError;
                return result;
            }

            DataTable headerTable = dataSet.Tables[HeaderTableName];
            DataTable lineTable = dataSet.Tables[LineTableName];

            if (!IsTexasShipTo(headerTable) || IsHeaderYes(headerTable, ManagerApprovedFieldName) || !HasTaxableActiveLine(lineTable))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            if (GetHeaderInt(headerTable, FreightCodeUidFieldName) == CollectFreightCodeUid)
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            headerTable.Rows[0][FreightCodeUidFieldName] = CollectFreightCodeUid;

            result.Success = true;
            result.Message = "This taxable Texas order requires freight code COLLECT. Freight code has been changed to COLLECT.";
            return result;
        }

        private static string ValidateDataset(DataSet dataSet)
        {
            if (!dataSet.Tables.Contains(HeaderTableName))
                return "The multi-row dataset is missing d_oe_header.";

            if (!dataSet.Tables.Contains(LineTableName))
                return "The multi-row dataset is missing d_dw_oe_line_dataentry.";

            DataTable headerTable = dataSet.Tables[HeaderTableName];
            DataTable lineTable = dataSet.Tables[LineTableName];

            if (headerTable.Rows.Count == 0)
                return "The multi-row dataset has no d_oe_header row.";

            if (!headerTable.Columns.Contains(ShipToStateFieldName))
                return "d_oe_header.oe_hdr_ship2_state must be selected in Field Selector.";

            if (!headerTable.Columns.Contains(FreightCodeUidFieldName))
                return "d_oe_header.freight_code_uid must be selected in Field Selector.";

            if (!headerTable.Columns.Contains(ManagerApprovedFieldName))
                return "d_oe_header.ufc_oe_hdr_ud_manager_approved must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(SalesTaxFieldName))
                return "d_dw_oe_line_dataentry.sales_tax must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(DetailTypeFieldName))
                return "d_dw_oe_line_dataentry.detail_type must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(CancelFlagFieldName))
                return "d_dw_oe_line_dataentry.oe_line_cancel_flag must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(DeleteFlagFieldName))
                return "d_dw_oe_line_dataentry.delete_flag must be selected in Field Selector.";

            return string.Empty;
        }

        private static bool IsTexasShipTo(DataTable headerTable)
        {
            string state = GetHeaderString(headerTable, ShipToStateFieldName).Trim();
            return string.Equals(state, "TX", StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasTaxableActiveLine(DataTable lineTable)
        {
            return lineTable.Rows
                .Cast<DataRow>()
                .Any(row => ShouldEvaluateLine(row) && GetDecimal(row, SalesTaxFieldName) != 0m);
        }

        private static bool ShouldEvaluateLine(DataRow row)
        {
            int detailType;
            if (!TryGetInt(row, DetailTypeFieldName, out detailType) || detailType != 0)
                return false;

            if (IsYes(row, CancelFlagFieldName) || IsYes(row, DeleteFlagFieldName))
                return false;

            return true;
        }

        private static bool IsHeaderYes(DataTable headerTable, string columnName)
        {
            return NormalizeYN(GetHeaderString(headerTable, columnName)) == "Y";
        }

        private static bool IsYes(DataRow row, string columnName)
        {
            return NormalizeYN(GetString(row, columnName)) == "Y";
        }

        private static string NormalizeYN(string value)
        {
            return (value ?? string.Empty).Trim().ToUpperInvariant();
        }

        private static string GetHeaderString(DataTable headerTable, string columnName)
        {
            if (headerTable.Rows.Count == 0 || !headerTable.Columns.Contains(columnName))
                return string.Empty;

            object value = headerTable.Rows[0][columnName];
            if (value == null || value == DBNull.Value)
                return string.Empty;

            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static int? GetHeaderInt(DataTable headerTable, string columnName)
        {
            int value;
            if (TryParseInt(GetHeaderString(headerTable, columnName), out value))
                return value;

            return null;
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static bool TryGetInt(DataRow row, string columnName, out int value)
        {
            value = 0;

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return false;

            try
            {
                value = Convert.ToInt32(row[columnName], CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return TryParseInt(GetString(row, columnName), out value);
            }
        }

        private static bool TryParseInt(string text, out int value)
        {
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out value) ||
                int.TryParse(text, NumberStyles.Integer, CultureInfo.CurrentCulture, out value);
        }

        private static decimal GetDecimal(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return 0m;

            object value = row[columnName];
            try
            {
                return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                string text = Convert.ToString(value, CultureInfo.InvariantCulture);
                decimal parsed;
                if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) ||
                    decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
                {
                    return parsed;
                }

                return 0m;
            }
        }
    }
}
