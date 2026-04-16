using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using P21.Extensions.BusinessRule;

namespace Unapproved_Order_No_Price
{
    public class Unapproved_Order_No_Price : P21.Extensions.BusinessRule.Rule
    {
        private const string HeaderTableName = "d_oe_header";
        private const string LineTableName = "d_dw_oe_line_dataentry";
        private const string ApprovedFieldName = "approved";
        private const string ManagerApprovedFieldName = "ufc_oe_hdr_ud_manager_approved";
        private const string UnitPriceFieldName = "unit_price";
        private const string OrderItemIdFieldName = "oe_order_item_id";
        private const string DetailTypeFieldName = "detail_type";
        private const string CancelFlagFieldName = "oe_line_cancel_flag";
        private const string DeleteFlagFieldName = "delete_flag";

        public override string GetName()
        {
            return "Unapproved_Order_No_Price";
        }

        public override string GetDescription()
        {
            return "Marks sales orders unapproved on save when lines are missing pricing unless Manager Approved is checked.";
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
                result.Message = "Unapproved_Order_No_Price must run as a multi-row sales-order save rule.";
                return result;
            }

            string setupError = ValidateSaveDataset(dataSet);
            if (!string.IsNullOrWhiteSpace(setupError))
            {
                result.Success = false;
                result.Message = setupError;
                return result;
            }

            List<string> missingPriceItemIds = GetMissingPriceItemIds(dataSet.Tables[LineTableName]);
            if (!missingPriceItemIds.Any())
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            if (IsHeaderYes(dataSet, ManagerApprovedFieldName))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            SetHeaderFlag(dataSet, ApprovedFieldName, "N");

            result.Success = true;
            result.Message = BuildMissingPriceMessage(missingPriceItemIds);
            return result;
        }

        private static string ValidateSaveDataset(DataSet dataSet)
        {
            if (!dataSet.Tables.Contains(HeaderTableName))
                return "The multi-row dataset is missing d_oe_header.";

            if (!dataSet.Tables.Contains(LineTableName))
                return "The multi-row dataset is missing d_dw_oe_line_dataentry.";

            DataTable headerTable = dataSet.Tables[HeaderTableName];
            DataTable lineTable = dataSet.Tables[LineTableName];

            if (headerTable.Rows.Count == 0)
                return "The multi-row dataset has no d_oe_header row.";

            if (!headerTable.Columns.Contains(ApprovedFieldName))
                return "d_oe_header.approved must be selected in Field Selector.";

            if (!headerTable.Columns.Contains(ManagerApprovedFieldName))
                return "d_oe_header.ufc_oe_hdr_ud_manager_approved must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(UnitPriceFieldName))
                return "d_dw_oe_line_dataentry.unit_price must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(OrderItemIdFieldName))
                return "d_dw_oe_line_dataentry.oe_order_item_id must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(DetailTypeFieldName))
                return "d_dw_oe_line_dataentry.detail_type must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(CancelFlagFieldName))
                return "d_dw_oe_line_dataentry.oe_line_cancel_flag must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(DeleteFlagFieldName))
                return "d_dw_oe_line_dataentry.delete_flag must be selected in Field Selector.";

            return string.Empty;
        }

        private static List<string> GetMissingPriceItemIds(DataTable lineTable)
        {
            List<string> itemIds = new List<string>();

            foreach (DataRow row in lineTable.Rows)
            {
                if (ShouldEvaluateLine(row) && IsMissingPrice(row[UnitPriceFieldName]))
                    itemIds.Add(GetString(row, OrderItemIdFieldName));
            }

            return itemIds;
        }

        private static bool IsMissingPrice(object value)
        {
            if (value == null || value == DBNull.Value)
                return true;

            string text = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(text))
                return true;

            decimal price;
            try
            {
                price = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out price) &&
                    !decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out price))
                {
                    return true;
                }
            }

            return price == 0m;
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

        private static string BuildMissingPriceMessage(IList<string> missingPriceItemIds)
        {
            if (missingPriceItemIds.Count > 1)
                return "Multiple items are missing pricing. The sales order has been marked Unapproved and must be manually approved before processing.";

            string itemId = missingPriceItemIds.Count == 0 ? string.Empty : missingPriceItemIds[0];
            if (string.IsNullOrWhiteSpace(itemId))
                return "Item on one sales order line is missing pricing. The sales order has been marked Unapproved and must be manually approved before processing.";

            return "Item " + itemId + " is missing pricing. The sales order has been marked Unapproved and must be manually approved before processing.";
        }

        private static bool IsHeaderYes(DataSet dataSet, string columnName)
        {
            DataTable headerTable = dataSet.Tables[HeaderTableName];
            return NormalizeYN(Convert.ToString(headerTable.Rows[0][columnName], CultureInfo.InvariantCulture)) == "Y";
        }

        private static void SetHeaderFlag(DataSet dataSet, string columnName, string value)
        {
            DataTable headerTable = dataSet.Tables[HeaderTableName];
            headerTable.Rows[0][columnName] = value;
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
                return false;
            }
        }

        private static bool IsYes(DataRow row, string columnName)
        {
            return NormalizeYN(GetString(row, columnName)) == "Y";
        }

        private static string NormalizeYN(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "N";

            string trimmed = value.Trim();

            if (trimmed.Equals("Y", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("YES", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("1"))
            {
                return "Y";
            }

            if (trimmed.Equals("N", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("NO", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("0"))
            {
                return "N";
            }

            bool boolValue;
            if (bool.TryParse(trimmed, out boolValue))
                return boolValue ? "Y" : "N";

            return trimmed.ToUpperInvariant();
        }
    }

    public class Validate_Approved_for_Unpriced_Order : P21.Extensions.BusinessRule.Rule
    {
        private const string HeaderTableName = "d_oe_header";
        private const string LineTableName = "d_dw_oe_line_dataentry";
        private const string PaymentDetailsTableName = "d_oe_payment_details";
        private const string ApprovedFieldName = "approved";
        private const string ManagerApprovedFieldName = "ufc_oe_hdr_ud_manager_approved";
        private const string UnitPriceFieldName = "unit_price";
        private const string DetailTypeFieldName = "detail_type";
        private const string CancelFlagFieldName = "oe_line_cancel_flag";
        private const string DeleteFlagFieldName = "delete_flag";
        private const string TermsFieldName = "oe_hdr_terms";
        private const string CreditCardNameFieldName = "cc_name";
        private const string CreditCardNumberFieldName = "cc_creditcard_number";
        private const string CreditCardExpirationDateFieldName = "cc_expiration_date";

        public override string GetName()
        {
            return "Validate_Approved_for_Unpriced_Order";
        }

        public override string GetDescription()
        {
            return "Blocks approval for missing pricing or incomplete credit-card details.";
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
                result.Message = "Validate_Approved_for_Unpriced_Order must run as a multi-row sales-order field-edit rule.";
                return result;
            }

            string setupError = ValidateFieldEditDataset(dataSet);
            if (!string.IsNullOrWhiteSpace(setupError))
            {
                result.Success = false;
                result.Message = setupError;
                return result;
            }

            string trigger = Data.TriggerColumn == null ? string.Empty : Data.TriggerColumn.Trim();
            if (!IsApprovedTrigger(trigger))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            string originalValue = NormalizeYN(Data.TriggerOriginalValue);

            if (originalValue == "Y")
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            string missingCreditCardDetail = GetMissingCreditCardDetail(dataSet);
            if (!string.IsNullOrWhiteSpace(missingCreditCardDetail))
            {
                result.Success = false;
                result.Message = "No " + missingCreditCardDetail + " has been specified. Order must remain Unapproved until all credit card details are entered.";
                return result;
            }

            if (GetHeaderFlag(dataSet, ManagerApprovedFieldName) == "Y" ||
                !HasMissingPrice(dataSet.Tables[LineTableName]))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            result.Success = false;
            result.Message = "This order cannot be approved without manager approval";
            return result;
        }

        private static string ValidateFieldEditDataset(DataSet dataSet)
        {
            if (!dataSet.Tables.Contains(HeaderTableName))
                return "The multi-row dataset is missing d_oe_header.";

            if (!dataSet.Tables.Contains(LineTableName))
                return "The multi-row dataset is missing d_dw_oe_line_dataentry.";

            if (!dataSet.Tables.Contains(PaymentDetailsTableName))
                return "The multi-row dataset is missing d_oe_payment_details.";

            DataTable headerTable = dataSet.Tables[HeaderTableName];
            DataTable lineTable = dataSet.Tables[LineTableName];
            DataTable paymentDetailsTable = dataSet.Tables[PaymentDetailsTableName];

            if (headerTable.Rows.Count == 0)
                return "The multi-row dataset has no d_oe_header row.";

            if (!headerTable.Columns.Contains(ApprovedFieldName))
                return "d_oe_header.approved must be selected in Field Selector.";

            if (!headerTable.Columns.Contains(ManagerApprovedFieldName))
                return "d_oe_header.ufc_oe_hdr_ud_manager_approved must be selected in Field Selector.";

            if (!headerTable.Columns.Contains(TermsFieldName))
                return "d_oe_header.oe_hdr_terms must be selected in Field Selector.";

            if (!paymentDetailsTable.Columns.Contains(CreditCardNameFieldName))
                return "d_oe_payment_details.cc_name must be selected in Field Selector.";

            if (!paymentDetailsTable.Columns.Contains(CreditCardNumberFieldName))
                return "d_oe_payment_details.cc_creditcard_number must be selected in Field Selector.";

            if (!paymentDetailsTable.Columns.Contains(CreditCardExpirationDateFieldName))
                return "d_oe_payment_details.cc_expiration_date must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(UnitPriceFieldName))
                return "d_dw_oe_line_dataentry.unit_price must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(DetailTypeFieldName))
                return "d_dw_oe_line_dataentry.detail_type must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(CancelFlagFieldName))
                return "d_dw_oe_line_dataentry.oe_line_cancel_flag must be selected in Field Selector.";

            if (!lineTable.Columns.Contains(DeleteFlagFieldName))
                return "d_dw_oe_line_dataentry.delete_flag must be selected in Field Selector.";

            return string.Empty;
        }

        private static bool IsApprovedTrigger(string trigger)
        {
            if (string.IsNullOrWhiteSpace(trigger))
                return true;

            return trigger.Equals(ApprovedFieldName, StringComparison.OrdinalIgnoreCase) ||
                   trigger.Equals(HeaderTableName + "." + ApprovedFieldName, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetMissingCreditCardDetail(DataSet dataSet)
        {
            string terms = GetHeaderString(dataSet, TermsFieldName);
            if (string.IsNullOrWhiteSpace(terms) ||
                !terms.Equals("Credit Card", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(GetPaymentDetailString(dataSet, CreditCardNameFieldName)))
                return "credit card name";

            if (string.IsNullOrWhiteSpace(GetPaymentDetailString(dataSet, CreditCardNumberFieldName)))
                return "credit card number";

            if (string.IsNullOrWhiteSpace(GetPaymentDetailString(dataSet, CreditCardExpirationDateFieldName)))
                return "credit card expiration date";

            return string.Empty;
        }

        private static bool HasMissingPrice(DataTable lineTable)
        {
            foreach (DataRow row in lineTable.Rows)
            {
                if (ShouldEvaluateLine(row) && IsMissingPrice(row[UnitPriceFieldName]))
                    return true;
            }

            return false;
        }

        private static bool IsMissingPrice(object value)
        {
            if (value == null || value == DBNull.Value)
                return true;

            string text = Convert.ToString(value, CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(text))
                return true;

            decimal price;
            try
            {
                price = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out price) &&
                    !decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out price))
                {
                    return true;
                }
            }

            return price == 0m;
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

        private static string GetHeaderFlag(DataSet dataSet, string columnName)
        {
            return NormalizeYN(GetHeaderString(dataSet, columnName));
        }

        private static string GetHeaderString(DataSet dataSet, string columnName)
        {
            DataTable headerTable = dataSet.Tables[HeaderTableName];
            if (!headerTable.Columns.Contains(columnName) || headerTable.Rows.Count == 0 || headerTable.Rows[0][columnName] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(headerTable.Rows[0][columnName], CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static string GetPaymentDetailString(DataSet dataSet, string columnName)
        {
            DataTable paymentDetailsTable = dataSet.Tables[PaymentDetailsTableName];
            if (!paymentDetailsTable.Columns.Contains(columnName) || paymentDetailsTable.Rows.Count == 0 || paymentDetailsTable.Rows[0][columnName] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(paymentDetailsTable.Rows[0][columnName], CultureInfo.InvariantCulture) ?? string.Empty;
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

        private static bool IsYes(DataRow row, string columnName)
        {
            return NormalizeYN(GetString(row, columnName)) == "Y";
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return string.Empty;

            return Convert.ToString(row[columnName], CultureInfo.InvariantCulture) ?? string.Empty;
        }

        private static string NormalizeYN(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "N";

            string trimmed = value.Trim();

            if (trimmed.Equals("Y", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("YES", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("1"))
            {
                return "Y";
            }

            if (trimmed.Equals("N", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("NO", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("0"))
            {
                return "N";
            }

            bool boolValue;
            if (bool.TryParse(trimmed, out boolValue))
                return boolValue ? "Y" : "N";

            return trimmed.ToUpperInvariant();
        }
    }
}
