using System;
using System.Collections.Generic;
using System.Data;

namespace Check_Overstocked
{
    public class Validate_Manager_Approval_for_Overstocked_POs : P21.Extensions.BusinessRule.Rule
    {
        private const string ManagerApprovedFieldName = "ufc_po_hdr_ud_manager_approved";
        private static readonly HashSet<string> AllowedUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CWESTBY",
            "DMCKINLEY",
            "JFELDMAN",
            "DSHAO"
        };

        public override string GetName()
        {
            return "Validate_Manager_Approval_for_Overstocked_POs";
        }

        public override string GetDescription()
        {
            return "Prevents unauthorized users from modifying Manager Approved and reports Session.UserID only when blocked.";
        }

        public override P21.Extensions.BusinessRule.RuleResult Execute()
        {
            P21.Extensions.BusinessRule.RuleResult result = new P21.Extensions.BusinessRule.RuleResult();
            DataSet dataSet;

            try
            {
                dataSet = Data.Set;
            }
            catch
            {
                result.Success = false;
                result.Message = "Validate_Manager_Approval_for_Overstocked_POs must run as a multi-row purchase-order rule. Session.UserID = " + Session.UserID + ".";
                return result;
            }

            string sessionUserId = Session.UserID ?? string.Empty;

            if (!dataSet.Tables.Contains("d_po_purchase_order_sheet"))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            string currentValue = GetHeaderFlag(dataSet, ManagerApprovedFieldName);
            string originalValue = NormalizeYN(Data.TriggerOriginalValue);

            if (string.IsNullOrWhiteSpace(originalValue))
                originalValue = currentValue;

            if (currentValue == originalValue)
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            if (AllowedUsers.Contains(sessionUserId))
            {
                result.Success = true;
                result.Message = string.Empty;
                return result;
            }

            result.Success = false;
            result.Message = "Manager Approved cannot be modified by user " + sessionUserId + ".";
            return result;
        }

        private static string GetHeaderFlag(DataSet dataSet, string columnName)
        {
            if (!dataSet.Tables.Contains("d_po_purchase_order_sheet"))
                return "N";

            DataTable headerTable = dataSet.Tables["d_po_purchase_order_sheet"];
            if (!headerTable.Columns.Contains(columnName) || headerTable.Rows.Count == 0)
                return "N";

            object value = headerTable.Rows[0][columnName];
            if (value == DBNull.Value)
                return "N";

            return NormalizeYN(Convert.ToString(value));
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
