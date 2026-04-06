using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using P21.Extensions.BusinessRule;

namespace Populate_Required_Date
{
    public class Populate_Required_Date : P21.Extensions.BusinessRule.Rule
    {
        private const string HeaderTableName = "d_prod_order_hdr";
        private const string LineTableName = "d_prod_order_line";
        private const string HeaderRequiredDateField = "required_date";
        private const string HeaderCustomerNameField = "ufc_prod_order_hdr_ud_customer_name";

        public override string GetName()
        {
            return "Populate_Required_Date";
        }

        public override string GetDescription()
        {
            return "Populates the production-order required date and customer name from the earliest qualifying open sales order for the finished item lines.";
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
                result.Message = "Populate_Required_Date must run as a multi-row production-order rule.";
                return result;
            }

            if (!dataSet.Tables.Contains(HeaderTableName))
            {
                result.Success = false;
                result.Message = "The multi-row dataset is missing d_prod_order_hdr.";
                return result;
            }

            if (!dataSet.Tables.Contains(LineTableName))
            {
                result.Success = false;
                result.Message = "The multi-row dataset is missing d_prod_order_line.";
                return result;
            }

            List<FinishedItemLine> activeLines = GetActiveFinishedItemLines(dataSet.Tables[LineTableName]);
            if (!activeLines.Any())
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            SalesOrderMatch earliestMatch;

            try
            {
                earliestMatch = GetEarliestOpenSalesOrder(activeLines);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Populate_Required_Date could not evaluate the earliest open sales order. " + ex.Message;
                return result;
            }

            if (earliestMatch == null)
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            ApplyHeaderValues(dataSet.Tables[HeaderTableName], earliestMatch);
            result.Success = true;
            result.Message = string.Empty;
            return result;
        }

        private static List<FinishedItemLine> GetActiveFinishedItemLines(DataTable lineTable)
        {
            List<FinishedItemLine> lines = new List<FinishedItemLine>();

            foreach (DataRow row in lineTable.Rows)
            {
                int invMastUid;

                if (IsYes(row, "cancel"))
                    continue;

                if (!TryGetInt(row, "inv_mast_uid", out invMastUid) || invMastUid <= 0)
                    continue;

                lines.Add(new FinishedItemLine
                {
                    InvMastUid = invMastUid,
                    ItemId = GetString(row, "item_id")
                });
            }

            return lines;
        }

        private SalesOrderMatch GetEarliestOpenSalesOrder(IEnumerable<FinishedItemLine> lines)
        {
            List<int> invMastUids = lines
                .Select(line => line.InvMastUid)
                .Distinct()
                .OrderBy(uid => uid)
                .ToList();

            if (!invMastUids.Any())
                return null;

            string sql = BuildSalesOrderSql(invMastUids.Count);

            using (SqlCommand command = new SqlCommand(sql, P21SqlConnection))
            {
                for (int i = 0; i < invMastUids.Count; i++)
                {
                    command.Parameters.AddWithValue("@inv_mast_uid_" + i.ToString(CultureInfo.InvariantCulture), invMastUids[i]);
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new SalesOrderMatch
                    {
                        InvMastUid = ReadInt(reader, "inv_mast_uid"),
                        ItemId = ReadString(reader, "item_id"),
                        CustomerName = ReadString(reader, "customer_name"),
                        RequiredDate = ReadDate(reader, "required_date"),
                        QtyOrdered = ReadDecimal(reader, "qty_ordered"),
                        OrderNo = ReadString(reader, "order_no")
                    };
                }
            }
        }

        private static string BuildSalesOrderSql(int invMastUidCount)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("WITH open_sales_orders AS (");
            sql.AppendLine("    SELECT");
            sql.AppendLine("          oel.inv_mast_uid");
            sql.AppendLine("        , m.item_id");
            sql.AppendLine("        , cst.customer_name");
            sql.AppendLine("        , CAST(COALESCE(oels.release_date, oel.required_date) AS date) AS required_date");
            sql.AppendLine("        , CAST(COALESCE(oels.release_qty, oel.qty_ordered) AS decimal(19, 6)) AS qty_ordered");
            sql.AppendLine("        , CONVERT(varchar(50), oel.order_no) AS order_no");
            sql.AppendLine("    FROM oe_line oel");
            sql.AppendLine("    INNER JOIN oe_hdr oeh");
            sql.AppendLine("        ON oeh.order_no = oel.order_no");
            sql.AppendLine("    LEFT JOIN oe_line_schedule oels");
            sql.AppendLine("        ON oels.order_no = oel.order_no");
            sql.AppendLine("       AND oels.line_no = oel.line_no");
            sql.AppendLine("    INNER JOIN customer cst");
            sql.AppendLine("        ON cst.customer_id = oeh.customer_id");
            sql.AppendLine("    INNER JOIN inv_mast m");
            sql.AppendLine("        ON m.inv_mast_uid = oel.inv_mast_uid");
            sql.AppendLine("    WHERE oel.complete = 'N'");
            sql.AppendLine("      AND oel.detail_type <> 1");
            sql.AppendLine("      AND COALESCE(oeh.order_type, 0) NOT IN (1877, 1706)");
            sql.AppendLine("      AND ISNULL(oeh.rma_flag, 'N') <> 'Y'");
            sql.AppendLine("      AND ISNULL(oeh.warranty_rma_flag, 'N') <> 'Y'");
            sql.AppendLine("      AND COALESCE(oels.release_date, oel.required_date) IS NOT NULL");
            sql.AppendLine("      AND oel.inv_mast_uid IN (" + BuildParameterList(invMastUidCount) + ")");
            sql.AppendLine(")");
            sql.AppendLine("SELECT TOP (1)");
            sql.AppendLine("      inv_mast_uid");
            sql.AppendLine("    , item_id");
            sql.AppendLine("    , customer_name");
            sql.AppendLine("    , required_date");
            sql.AppendLine("    , qty_ordered");
            sql.AppendLine("    , order_no");
            sql.AppendLine("FROM open_sales_orders");
            sql.AppendLine("ORDER BY required_date ASC, qty_ordered DESC, customer_name ASC, order_no ASC;");

            return sql.ToString();
        }

        private static string BuildParameterList(int count)
        {
            List<string> parameters = new List<string>();

            for (int i = 0; i < count; i++)
            {
                parameters.Add("@inv_mast_uid_" + i.ToString(CultureInfo.InvariantCulture));
            }

            return string.Join(", ", parameters);
        }

        private static void ApplyHeaderValues(DataTable headerTable, SalesOrderMatch match)
        {
            if (headerTable.Rows.Count == 0)
                throw new ApplicationException("d_prod_order_hdr has no header row to update.");

            DataRow headerRow = headerTable.Rows[0];

            if (!headerTable.Columns.Contains(HeaderRequiredDateField))
                throw new ApplicationException("d_prod_order_hdr is missing required_date.");

            if (!headerTable.Columns.Contains(HeaderCustomerNameField))
                throw new ApplicationException("d_prod_order_hdr is missing ufc_prod_order_hdr_ud_customer_name.");

            headerRow[HeaderRequiredDateField] = match.RequiredDate;
            headerRow[HeaderCustomerNameField] = string.IsNullOrWhiteSpace(match.CustomerName)
                ? (object)DBNull.Value
                : match.CustomerName;
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
                return false;
            }
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static bool IsYes(DataRow row, string columnName)
        {
            string value = GetString(row, columnName);
            return value.Equals("Y", StringComparison.OrdinalIgnoreCase);
        }

        private static int ReadInt(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : Convert.ToInt32(reader.GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        private static decimal ReadDecimal(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0m : Convert.ToDecimal(reader.GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        private static DateTime ReadDate(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal)
                ? DateTime.MinValue
                : Convert.ToDateTime(reader.GetValue(ordinal), CultureInfo.InvariantCulture).Date;
        }

        private static string ReadString(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal)
                ? string.Empty
                : Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private sealed class FinishedItemLine
        {
            public int InvMastUid { get; set; }
            public string ItemId { get; set; }
        }

        private sealed class SalesOrderMatch
        {
            public int InvMastUid { get; set; }
            public string ItemId { get; set; }
            public string CustomerName { get; set; }
            public DateTime RequiredDate { get; set; }
            public decimal QtyOrdered { get; set; }
            public string OrderNo { get; set; }
        }
    }
}
