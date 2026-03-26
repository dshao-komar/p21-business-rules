using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using P21.Extensions.BusinessRule;

namespace Check_Overstocked
{
    public class Check_Overstocked : P21.Extensions.BusinessRule.Rule
    {
        private const decimal MaxMonthsAvailable = 3m;
        private const string ManagerApprovedFieldName = "ufc_po_hdr_ud_manager_approved";
        private static readonly bool EnableManagerEmail = true;
        private const string ManagerEmailAddress = "dshao@komar.com";
        private const string AlertFromEmailAddress = "P21Alerts@komar.com";
        private const string SmtpHost = "localhost";
        private const int SmtpPort = 25;

        public override string GetName()
        {
            return "Check_Overstocked";
        }

        public override string GetDescription()
        {
            return "Removes Approved when projected availability exceeds 3 months unless manager approval has been granted.";
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
                result.Message = "Check_Overstocked must run as a multi-row purchase-order rule.";
                return result;
            }

            if (!dataSet.Tables.Contains("d_po_item_sheet"))
            {
                result.Success = false;
                result.Message = "The multi-row dataset is missing d_po_item_sheet.";
                return result;
            }

            List<PoLineRequest> lineRequests = GetLineRequests(dataSet);
            if (!lineRequests.Any())
            {
                result.Success = true;
                result.Message = "No stock purchase-order lines required overstock validation.";
                return result;
            }

            string currentPoNumber = GetCurrentPoNumber(dataSet);
            List<OverstockFinding> findings;

            try
            {
                findings = EvaluateProjectedAvailability(lineRequests, currentPoNumber);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Check_Overstocked could not evaluate projected availability. " + ex.Message;
                return result;
            }

            if (!findings.Any())
            {
                result.Success = true;
                result.Message = "Projected availability is within the 3-month threshold.";
                return result;
            }

            if (IsHeaderYes(dataSet, ManagerApprovedFieldName))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            string message = BuildBlockedSaveMessage(findings);
            SetHeaderFlag(dataSet, "approved", "N");
            TrySendManagerEmail(currentPoNumber, message);

            result.Success = true;
            result.Message = message;
            return result;
        }

        private static List<PoLineRequest> GetLineRequests(DataSet dataSet)
        {
            DataTable itemTable = dataSet.Tables["d_po_item_sheet"];
            DataTable classTable = dataSet.Tables.Contains("d_po_classes_sheet")
                ? dataSet.Tables["d_po_classes_sheet"]
                : null;
            DateTime? headerOrderDate = GetHeaderOrderDate(dataSet);

            Dictionary<int, DateTime?> expectedDatesByRow = GetExpectedDatesByRow(classTable);
            List<PoLineRequest> requests = new List<PoLineRequest>();

            foreach (DataRow row in itemTable.Rows)
            {
                int rowId;
                int invMastUid;
                int locationId;
                decimal qtyOrdered;

                if (IsYes(row, "cancel_flag") || IsYes(row, "delete_flag"))
                    continue;

                if (!TryGetInt(row, "rowID", out rowId))
                    continue;

                if (!TryGetInt(row, "inv_mast_uid", out invMastUid) || invMastUid <= 0)
                    continue;

                if (!TryGetInt(row, "location_id", out locationId) || locationId <= 0)
                    continue;

                if (!TryGetDecimal(row, "qty_ordered", out qtyOrdered) || qtyOrdered <= 0)
                    continue;

                DateTime? expectedDate;
                if (!expectedDatesByRow.TryGetValue(rowId, out expectedDate) || expectedDate == null)
                    expectedDate = headerOrderDate;

                if (expectedDate == null)
                    continue;

                requests.Add(new PoLineRequest
                {
                    RowId = rowId,
                    InvMastUid = invMastUid,
                    LocationId = locationId,
                    ItemId = GetString(row, "item_id"),
                    UnitOfMeasure = GetString(row, "unit_of_measure"),
                    ExpectedDate = expectedDate.Value.Date,
                    OrderedQty = qtyOrdered
                });
            }

            return requests;
        }

        private static DateTime? GetHeaderOrderDate(DataSet dataSet)
        {
            string[] candidateTables = { "d_po_purchase_order_sheet", "po_hdr", "po_hdr_ud" };
            string[] candidateColumns = { "order_date", "po_date", "date_created", "created_date", "creation_date" };

            foreach (string tableName in candidateTables)
            {
                if (!dataSet.Tables.Contains(tableName) || dataSet.Tables[tableName].Rows.Count == 0)
                    continue;

                DataRow row = dataSet.Tables[tableName].Rows[0];
                foreach (string columnName in candidateColumns)
                {
                    DateTime value;
                    if (TryGetDate(row, columnName, out value))
                        return value.Date;
                }
            }

            return null;
        }

        private static Dictionary<int, DateTime?> GetExpectedDatesByRow(DataTable classTable)
        {
            Dictionary<int, DateTime?> results = new Dictionary<int, DateTime?>();

            if (classTable == null)
                return results;

            foreach (DataRow row in classTable.Rows)
            {
                int rowId;
                DateTime expectedDate;

                if (!TryGetInt(row, "rowID", out rowId))
                    continue;

                if (TryGetDate(row, "expected_date", out expectedDate))
                    results[rowId] = expectedDate.Date;
            }

            return results;
        }

        private string GetCurrentPoNumber(DataSet dataSet)
        {
            string[] candidateTables = { "d_po_purchase_order_sheet", "po_hdr", "po_hdr_ud" };
            string[] candidateColumns = { "po_no", "purchase_order_no" };

            foreach (string tableName in candidateTables)
            {
                if (!dataSet.Tables.Contains(tableName) || dataSet.Tables[tableName].Rows.Count == 0)
                    continue;

                DataRow row = dataSet.Tables[tableName].Rows[0];
                foreach (string columnName in candidateColumns)
                {
                    string value = GetString(row, columnName);
                    if (!string.IsNullOrWhiteSpace(value))
                        return value;
                }
            }

            return string.Empty;
        }

        private List<OverstockFinding> EvaluateProjectedAvailability(IEnumerable<PoLineRequest> lineRequests, string currentPoNumber)
        {
            List<PoLineRequest> requests = lineRequests.ToList();
            OpenPurchaseOrderSchema openPoSchema = DiscoverOpenPurchaseOrderSchema();
            Dictionary<string, ItemMetrics> metricsCache = new Dictionary<string, ItemMetrics>(StringComparer.OrdinalIgnoreCase);
            List<OverstockFinding> findings = new List<OverstockFinding>();

            foreach (PoLineRequest request in requests.OrderBy(r => r.ExpectedDate).ThenBy(r => r.ItemId))
            {
                string metricKey = BuildMetricKey(request.InvMastUid, request.LocationId, request.ExpectedDate);
                ItemMetrics metrics;

                if (!metricsCache.TryGetValue(metricKey, out metrics))
                {
                    metrics = GetItemMetrics(request, currentPoNumber, openPoSchema);
                    metricsCache[metricKey] = metrics;
                }

                if (metrics.EffectiveMonthlyUsage <= 0m)
                    continue;

                if (!string.IsNullOrWhiteSpace(request.UnitOfMeasure) &&
                    !string.IsNullOrWhiteSpace(metrics.DefaultPurchasingUnit) &&
                    !request.UnitOfMeasure.Equals(metrics.DefaultPurchasingUnit, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                decimal currentPoInboundQty = requests
                    .Where(r => r.InvMastUid == request.InvMastUid &&
                                r.LocationId == request.LocationId &&
                                r.ExpectedDate <= request.ExpectedDate)
                    .Sum(r => r.OrderedQty);

                int usageMonthsApplied = GetElapsedThirtyDayPeriods(DateTime.Today, request.ExpectedDate);
                decimal projectedOnHandBeforeInbound = metrics.QtyOnHand - (metrics.Usage3 * usageMonthsApplied);
                decimal projectedAvailable = projectedOnHandBeforeInbound + metrics.OtherOpenInboundQty + currentPoInboundQty;
                decimal projectedMonthsAvailable = projectedAvailable / metrics.EffectiveMonthlyUsage;

                if (projectedMonthsAvailable > MaxMonthsAvailable)
                {
                    findings.Add(new OverstockFinding
                    {
                        ItemId = request.ItemId,
                        InvMastUid = request.InvMastUid,
                        LocationId = request.LocationId,
                        ExpectedDate = request.ExpectedDate,
                        QtyOnHand = metrics.QtyOnHand,
                        ProjectedOnHandBeforeInbound = projectedOnHandBeforeInbound,
                        OtherOpenInboundQty = metrics.OtherOpenInboundQty,
                        OtherOpenInboundPoNumbers = metrics.OtherOpenInboundPoNumbers,
                        CurrentPoInboundQty = currentPoInboundQty,
                        EffectiveMonthlyUsage = metrics.EffectiveMonthlyUsage,
                        Usage3 = metrics.Usage3,
                        Usage6 = metrics.Usage6,
                        Usage12 = metrics.Usage12,
                        UsageMonthsApplied = usageMonthsApplied,
                        ProjectedMonthsAvailable = projectedMonthsAvailable
                    });
                }
            }

            return findings;
        }

        private ItemMetrics GetItemMetrics(PoLineRequest request, string currentPoNumber, OpenPurchaseOrderSchema openPoSchema)
        {
            string sql = BuildMetricsSql(openPoSchema);

            using (SqlCommand command = new SqlCommand(sql, P21SqlConnection))
            {
                command.Parameters.AddWithValue("@inv_mast_uid", request.InvMastUid);
                command.Parameters.AddWithValue("@location_id", request.LocationId);
                command.Parameters.AddWithValue("@expected_date", request.ExpectedDate);
                command.Parameters.AddWithValue("@current_po_no", string.IsNullOrWhiteSpace(currentPoNumber) ? (object)DBNull.Value : currentPoNumber);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return new ItemMetrics
                        {
                            ItemId = request.ItemId,
                            QtyOnHand = 0m,
                            OtherOpenInboundQty = 0m,
                            Usage3 = 0m,
                            Usage6 = 0m,
                            Usage12 = 0m,
                            EffectiveMonthlyUsage = 0m,
                            DefaultPurchasingUnit = string.Empty
                        };
                    }

                    return new ItemMetrics
                    {
                        ItemId = ReadString(reader, "item_id"),
                        QtyOnHand = ReadDecimal(reader, "qty_on_hand"),
                        OtherOpenInboundQty = ReadDecimal(reader, "other_open_inbound_qty"),
                        OtherOpenInboundPoNumbers = ReadString(reader, "other_open_inbound_po_numbers"),
                        Usage3 = ReadDecimal(reader, "usage3"),
                        Usage6 = ReadDecimal(reader, "usage6"),
                        Usage12 = ReadDecimal(reader, "usage12"),
                        EffectiveMonthlyUsage = ReadDecimal(reader, "effective_monthly_usage"),
                        DefaultPurchasingUnit = ReadString(reader, "default_purchasing_unit")
                    };
                }
            }
        }

        private OpenPurchaseOrderSchema DiscoverOpenPurchaseOrderSchema()
        {
            List<string> poLineColumns = GetTableColumns("po_line");
            List<string> poHeaderColumns = GetTableColumns("po_hdr");

            if (!poLineColumns.Any())
                throw new ApplicationException("The po_line table was not found while preparing the overstock query.");
            if (!poHeaderColumns.Any())
                throw new ApplicationException("The po_hdr table was not found while preparing the overstock query.");

            OpenPurchaseOrderSchema schema = new OpenPurchaseOrderSchema();
            schema.ExpectedDateColumn = PickColumn(poLineColumns, "expected_date", "date_due", "promise_date", "requested_date");
            schema.OrderedQtyColumn = PickColumn(poLineColumns, "qty_ordered", "qty_ordered_base");
            schema.ReceivedQtyColumn = PickColumn(poLineColumns, "qty_received", "qty_received_base", "qty_received_supplier");
            schema.PoNumberColumn = PickColumn(poLineColumns, "po_no", "purchase_order_no");
            schema.DeleteFlagColumn = PickColumn(poLineColumns, "delete_flag", "deleted_flag");
            schema.CancelFlagColumn = PickColumn(poLineColumns, "cancel_flag", "cancelled_flag");
            schema.CompleteFlagColumn = PickColumn(poLineColumns, "complete", "complete_flag", "completed_flag");
            schema.HeaderExpectedDateColumn = PickColumn(poHeaderColumns, "expected_date", "date_due", "promise_date", "requested_date");

            if (string.IsNullOrWhiteSpace(schema.ExpectedDateColumn) ||
                string.IsNullOrWhiteSpace(schema.OrderedQtyColumn) ||
                string.IsNullOrWhiteSpace(schema.ReceivedQtyColumn) ||
                string.IsNullOrWhiteSpace(schema.PoNumberColumn) ||
                string.IsNullOrWhiteSpace(schema.HeaderExpectedDateColumn))
            {
                throw new ApplicationException(
                    "Could not determine the open-PO columns needed for Check_Overstocked. " +
                    "Please confirm the po_line po_no/qty columns and po_hdr expected_date column names.");
            }

            return schema;
        }

        private List<string> GetTableColumns(string tableName)
        {
            const string sql = @"
SELECT c.name
FROM sys.columns c
JOIN sys.tables t
    ON t.object_id = c.object_id
WHERE t.name = @table_name;";

            List<string> columns = new List<string>();

            using (SqlCommand command = new SqlCommand(sql, P21SqlConnection))
            {
                command.Parameters.AddWithValue("@table_name", tableName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(Convert.ToString(reader[0], CultureInfo.InvariantCulture));
                    }
                }
            }

            return columns;
        }

        private static string PickColumn(IEnumerable<string> availableColumns, params string[] candidates)
        {
            foreach (string candidate in candidates)
            {
                string match = availableColumns.FirstOrDefault(c => c.Equals(candidate, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(match))
                    return match;
            }

            return string.Empty;
        }

        private static string BuildMetricsSql(OpenPurchaseOrderSchema schema)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("WITH usage_stats AS (");
            sql.AppendLine("    SELECT");
            sql.AppendLine("          ipu.location_id");
            sql.AppendLine("        , ipu.inv_mast_uid");
            sql.AppendLine("        , CAST(SUM(CASE");
            sql.AppendLine("              WHEN dp.beginning_date >= DATEADD(month, -3, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))");
            sql.AppendLine("               AND dp.beginning_date < DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)");
            sql.AppendLine("              THEN CAST(ipu.inv_period_usage AS decimal(19, 6)) / NULLIF(CAST(im.sales_pricing_unit_size AS decimal(19, 6)), 0)");
            sql.AppendLine("              ELSE 0 END) / 3.0 AS decimal(19, 6)) AS usage3");
            sql.AppendLine("        , CAST(SUM(CASE");
            sql.AppendLine("              WHEN dp.beginning_date >= DATEADD(month, -6, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))");
            sql.AppendLine("               AND dp.beginning_date < DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)");
            sql.AppendLine("              THEN CAST(ipu.inv_period_usage AS decimal(19, 6)) / NULLIF(CAST(im.sales_pricing_unit_size AS decimal(19, 6)), 0)");
            sql.AppendLine("              ELSE 0 END) / 6.0 AS decimal(19, 6)) AS usage6");
            sql.AppendLine("        , CAST(SUM(CASE");
            sql.AppendLine("              WHEN dp.beginning_date >= DATEADD(month, -12, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))");
            sql.AppendLine("               AND dp.beginning_date < DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)");
            sql.AppendLine("              THEN CAST(ipu.inv_period_usage AS decimal(19, 6)) / NULLIF(CAST(im.sales_pricing_unit_size AS decimal(19, 6)), 0)");
            sql.AppendLine("              ELSE 0 END) / 12.0 AS decimal(19, 6)) AS usage12");
            sql.AppendLine("    FROM inv_period_usage ipu");
            sql.AppendLine("    JOIN inv_mast im");
            sql.AppendLine("      ON im.inv_mast_uid = ipu.inv_mast_uid");
            sql.AppendLine("    JOIN demand_period dp");
            sql.AppendLine("      ON dp.demand_period_uid = ipu.demand_period_uid");
            sql.AppendLine("    WHERE ipu.inv_mast_uid = @inv_mast_uid");
            sql.AppendLine("      AND ipu.location_id = @location_id");
            sql.AppendLine("      AND dp.beginning_date >= DATEADD(month, -12, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1))");
            sql.AppendLine("    GROUP BY ipu.location_id, ipu.inv_mast_uid");
            sql.AppendLine("), open_po AS (");
            sql.AppendLine("    SELECT");
            sql.AppendLine("          pl.inv_mast_uid");
            sql.AppendLine("        , ph.location_id");
            sql.AppendLine("        , CAST(SUM(CASE");
            sql.AppendLine("              WHEN ph." + schema.HeaderExpectedDateColumn + " <= @expected_date");
            sql.AppendLine("              THEN ISNULL(CAST(pl." + schema.OrderedQtyColumn + " AS decimal(19, 6)), 0)");
            sql.AppendLine("                 - ISNULL(CAST(pl." + schema.ReceivedQtyColumn + " AS decimal(19, 6)), 0)");
            sql.AppendLine("              ELSE 0 END) AS decimal(19, 6)) AS other_open_inbound_qty");
            sql.AppendLine("    FROM po_line pl");
            sql.AppendLine("    JOIN po_hdr ph");
            sql.AppendLine("      ON ph.po_no = pl." + schema.PoNumberColumn);
            sql.AppendLine("    WHERE pl.inv_mast_uid = @inv_mast_uid");
            sql.AppendLine("      AND ph.location_id = @location_id");
            sql.AppendLine("      AND ph." + schema.HeaderExpectedDateColumn + " IS NOT NULL");
            sql.AppendLine("      AND (ISNULL(CAST(pl." + schema.OrderedQtyColumn + " AS decimal(19, 6)), 0)");
            sql.AppendLine("         - ISNULL(CAST(pl." + schema.ReceivedQtyColumn + " AS decimal(19, 6)), 0)) > 0");

            if (!string.IsNullOrWhiteSpace(schema.PoNumberColumn))
                sql.AppendLine("      AND (@current_po_no IS NULL OR pl." + schema.PoNumberColumn + " <> @current_po_no)");

            if (!string.IsNullOrWhiteSpace(schema.DeleteFlagColumn))
                sql.AppendLine("      AND ISNULL(pl." + schema.DeleteFlagColumn + ", 'N') <> 'Y'");

            if (!string.IsNullOrWhiteSpace(schema.CancelFlagColumn))
                sql.AppendLine("      AND ISNULL(pl." + schema.CancelFlagColumn + ", 'N') <> 'Y'");

            if (!string.IsNullOrWhiteSpace(schema.CompleteFlagColumn))
                sql.AppendLine("      AND ISNULL(pl." + schema.CompleteFlagColumn + ", 'N') <> 'Y'");

            sql.AppendLine("    GROUP BY pl.inv_mast_uid, ph.location_id");
            sql.AppendLine("), open_po_numbers AS (");
            sql.AppendLine("    SELECT");
            sql.AppendLine("          src.inv_mast_uid");
            sql.AppendLine("        , src.location_id");
            sql.AppendLine("        , STUFF((");
            sql.AppendLine("            SELECT DISTINCT ', ' + CONVERT(varchar(50), src2.po_no)");
            sql.AppendLine("            FROM (");
            sql.AppendLine("                SELECT");
            sql.AppendLine("                      pl2.inv_mast_uid");
            sql.AppendLine("                    , ph2.location_id");
            sql.AppendLine("                    , pl2." + schema.PoNumberColumn + " AS po_no");
            sql.AppendLine("                FROM po_line pl2");
            sql.AppendLine("                JOIN po_hdr ph2");
            sql.AppendLine("                  ON ph2.po_no = pl2." + schema.PoNumberColumn);
            sql.AppendLine("                WHERE pl2.inv_mast_uid = @inv_mast_uid");
            sql.AppendLine("                  AND ph2.location_id = @location_id");
            sql.AppendLine("                  AND ph2." + schema.HeaderExpectedDateColumn + " IS NOT NULL");
            sql.AppendLine("                  AND ph2." + schema.HeaderExpectedDateColumn + " <= @expected_date");
            sql.AppendLine("                  AND (ISNULL(CAST(pl2." + schema.OrderedQtyColumn + " AS decimal(19, 6)), 0)");
            sql.AppendLine("                     - ISNULL(CAST(pl2." + schema.ReceivedQtyColumn + " AS decimal(19, 6)), 0)) > 0");

            if (!string.IsNullOrWhiteSpace(schema.PoNumberColumn))
                sql.AppendLine("                  AND (@current_po_no IS NULL OR pl2." + schema.PoNumberColumn + " <> @current_po_no)");

            if (!string.IsNullOrWhiteSpace(schema.DeleteFlagColumn))
                sql.AppendLine("                  AND ISNULL(pl2." + schema.DeleteFlagColumn + ", 'N') <> 'Y'");

            if (!string.IsNullOrWhiteSpace(schema.CancelFlagColumn))
                sql.AppendLine("                  AND ISNULL(pl2." + schema.CancelFlagColumn + ", 'N') <> 'Y'");

            if (!string.IsNullOrWhiteSpace(schema.CompleteFlagColumn))
                sql.AppendLine("                  AND ISNULL(pl2." + schema.CompleteFlagColumn + ", 'N') <> 'Y'");

            sql.AppendLine("            ) src2");
            sql.AppendLine("            WHERE src2.inv_mast_uid = src.inv_mast_uid");
            sql.AppendLine("              AND src2.location_id = src.location_id");
            sql.AppendLine("            FOR XML PATH(''), TYPE).value('.', 'varchar(max)'), 1, 2, '') AS po_numbers");
            sql.AppendLine("    FROM open_po src");
            sql.AppendLine(")");
            sql.AppendLine("SELECT");
            sql.AppendLine("      m.item_id");
            sql.AppendLine("    , CAST(ISNULL(l.qty_on_hand, 0) AS decimal(19, 6)) AS qty_on_hand");
            sql.AppendLine("    , CAST(ISNULL(op.other_open_inbound_qty, 0) AS decimal(19, 6)) AS other_open_inbound_qty");
            sql.AppendLine("    , ISNULL(opn.po_numbers, '') AS other_open_inbound_po_numbers");
            sql.AppendLine("    , CAST(ISNULL(us.usage3, 0) AS decimal(19, 6)) AS usage3");
            sql.AppendLine("    , CAST(ISNULL(us.usage6, 0) AS decimal(19, 6)) AS usage6");
            sql.AppendLine("    , CAST(ISNULL(us.usage12, 0) AS decimal(19, 6)) AS usage12");
            sql.AppendLine("    , CAST(ISNULL(us.usage3, 0) AS decimal(19, 6)) AS effective_monthly_usage");
            sql.AppendLine("    , m.default_purchasing_unit");
            sql.AppendLine("FROM inv_mast m");
            sql.AppendLine("LEFT JOIN inv_loc l");
            sql.AppendLine("  ON l.inv_mast_uid = m.inv_mast_uid");
            sql.AppendLine(" AND l.location_id = @location_id");
            sql.AppendLine("LEFT JOIN usage_stats us");
            sql.AppendLine("  ON us.inv_mast_uid = m.inv_mast_uid");
            sql.AppendLine(" AND us.location_id = @location_id");
            sql.AppendLine("LEFT JOIN open_po op");
            sql.AppendLine("  ON op.inv_mast_uid = m.inv_mast_uid");
            sql.AppendLine(" AND op.location_id = @location_id");
            sql.AppendLine("LEFT JOIN open_po_numbers opn");
            sql.AppendLine("  ON opn.inv_mast_uid = m.inv_mast_uid");
            sql.AppendLine(" AND opn.location_id = @location_id");
            sql.AppendLine("WHERE m.inv_mast_uid = @inv_mast_uid;");

            return sql.ToString();
        }

        private static string BuildBlockedSaveMessage(IEnumerable<OverstockFinding> findings)
        {
            IEnumerable<string> details = findings
                .OrderByDescending(f => f.ProjectedMonthsAvailable)
                .Take(5)
                .Select(f =>
                {
                    string itemLabel = string.IsNullOrWhiteSpace(f.ItemId)
                        ? "inv_mast_uid " + f.InvMastUid.ToString(CultureInfo.InvariantCulture)
                        : f.ItemId;

                    string inboundText = f.OtherOpenInboundQty.ToString("0.##", CultureInfo.InvariantCulture);
                    if (!string.IsNullOrWhiteSpace(f.OtherOpenInboundPoNumbers))
                        inboundText += " (POs: " + f.OtherOpenInboundPoNumbers + ")";

                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "- {0}\r\n  Location: {1}\r\n  Expected date: {2:d}\r\n  Current on hand: {3:0.##}\r\n  Projected on-hand before this PO: {5:0.##}\r\n  Other inbound: {6}\r\n  This PO inbound: {7:0.##}\r\n  3-month usage: {8:0.##}\r\n  Projected months available: {9:0.##}",
                        itemLabel,
                        f.LocationId,
                        f.ExpectedDate,
                        f.QtyOnHand,
                        f.UsageMonthsApplied,
                        f.ProjectedOnHandBeforeInbound,
                        inboundText,
                        f.CurrentPoInboundQty,
                        f.Usage3,
                        f.ProjectedMonthsAvailable);
                });

            return "PO cannot be saved as Approved because one or more items would exceed 3 months available.\r\n\r\n" +
                   string.Join("\r\n\r\n", details) +
                   "\r\n\r\nManager has been notified.";
        }

        private static string BuildMetricKey(int invMastUid, int locationId, DateTime expectedDate)
        {
            return invMastUid.ToString(CultureInfo.InvariantCulture) + "|" +
                   locationId.ToString(CultureInfo.InvariantCulture) + "|" +
                   expectedDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
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

        private static bool TryGetDecimal(DataRow row, string columnName, out decimal value)
        {
            value = 0m;

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return false;

            try
            {
                value = Convert.ToDecimal(row[columnName], CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryGetDate(DataRow row, string columnName, out DateTime value)
        {
            value = DateTime.MinValue;

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return false;

            try
            {
                value = Convert.ToDateTime(row[columnName], CultureInfo.InvariantCulture);
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

        private static int GetElapsedThirtyDayPeriods(DateTime fromDate, DateTime toDate)
        {
            if (toDate.Date <= fromDate.Date)
                return 0;

            return (int)Math.Floor((toDate.Date - fromDate.Date).TotalDays / 30d);
        }

        private static bool IsHeaderYes(DataSet dataSet, string columnName)
        {
            if (!dataSet.Tables.Contains("d_po_purchase_order_sheet"))
                return false;

            DataTable headerTable = dataSet.Tables["d_po_purchase_order_sheet"];
            if (!headerTable.Columns.Contains(columnName) || headerTable.Rows.Count == 0)
                return false;

            return Convert.ToString(headerTable.Rows[0][columnName], CultureInfo.InvariantCulture)
                .Equals("Y", StringComparison.OrdinalIgnoreCase);
        }

        private static void SetHeaderFlag(DataSet dataSet, string columnName, string value)
        {
            if (!dataSet.Tables.Contains("d_po_purchase_order_sheet"))
                return;

            DataTable headerTable = dataSet.Tables["d_po_purchase_order_sheet"];
            if (!headerTable.Columns.Contains(columnName) || headerTable.Rows.Count == 0)
                return;

            headerTable.Rows[0][columnName] = value;
        }

        private static void TrySendManagerEmail(string poNumber, string messageBody)
        {
            if (!EnableManagerEmail ||
                string.IsNullOrWhiteSpace(ManagerEmailAddress) ||
                string.IsNullOrWhiteSpace(AlertFromEmailAddress) ||
                string.IsNullOrWhiteSpace(SmtpHost))
            {
                return;
            }

            try
            {
                using (MailMessage message = new MailMessage(AlertFromEmailAddress, ManagerEmailAddress))
                {
                    message.Subject = "PO de-approved by Check_Overstocked" +
                                      (string.IsNullOrWhiteSpace(poNumber) ? string.Empty : " - PO " + poNumber);
                    message.Body = messageBody;

                    using (SmtpClient client = new SmtpClient(SmtpHost, SmtpPort))
                    {
                        client.Send(message);
                    }
                }
            }
            catch
            {
                // Do not let email failures interrupt the PO save flow.
            }
        }

        private static decimal ReadDecimal(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0m : Convert.ToDecimal(reader.GetValue(ordinal), CultureInfo.InvariantCulture);
        }

        private static string ReadString(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal)
                ? string.Empty
                : Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private sealed class PoLineRequest
        {
            public int RowId { get; set; }
            public int InvMastUid { get; set; }
            public int LocationId { get; set; }
            public string ItemId { get; set; }
            public string UnitOfMeasure { get; set; }
            public DateTime ExpectedDate { get; set; }
            public decimal OrderedQty { get; set; }
        }

        private sealed class ItemMetrics
        {
            public string ItemId { get; set; }
            public decimal QtyOnHand { get; set; }
            public decimal OtherOpenInboundQty { get; set; }
            public string OtherOpenInboundPoNumbers { get; set; }
            public decimal Usage3 { get; set; }
            public decimal Usage6 { get; set; }
            public decimal Usage12 { get; set; }
            public decimal EffectiveMonthlyUsage { get; set; }
            public string DefaultPurchasingUnit { get; set; }
        }

        private sealed class OverstockFinding
        {
            public string ItemId { get; set; }
            public int InvMastUid { get; set; }
            public int LocationId { get; set; }
            public DateTime ExpectedDate { get; set; }
            public decimal QtyOnHand { get; set; }
            public decimal ProjectedOnHandBeforeInbound { get; set; }
            public decimal OtherOpenInboundQty { get; set; }
            public string OtherOpenInboundPoNumbers { get; set; }
            public decimal CurrentPoInboundQty { get; set; }
            public decimal EffectiveMonthlyUsage { get; set; }
            public decimal Usage3 { get; set; }
            public decimal Usage6 { get; set; }
            public decimal Usage12 { get; set; }
            public int UsageMonthsApplied { get; set; }
            public decimal ProjectedMonthsAvailable { get; set; }
        }

        private sealed class OpenPurchaseOrderSchema
        {
            public string ExpectedDateColumn { get; set; }
            public string OrderedQtyColumn { get; set; }
            public string ReceivedQtyColumn { get; set; }
            public string PoNumberColumn { get; set; }
            public string DeleteFlagColumn { get; set; }
            public string CancelFlagColumn { get; set; }
            public string CompleteFlagColumn { get; set; }
            public string HeaderExpectedDateColumn { get; set; }
        }
    }
}
