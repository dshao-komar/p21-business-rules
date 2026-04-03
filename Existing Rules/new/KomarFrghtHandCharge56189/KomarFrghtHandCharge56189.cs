using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using P21.Extensions.BusinessRule;

namespace KomarFrghtHandCharge56189
{
    public class KomarFrghtHandCharge56189 : P21.Extensions.BusinessRule.Rule
    {
        private string sRMAFlag = "";
        private decimal dSalesSubTotal = 0.0m;
        private decimal dNewFreightOut = 0.0m;
        private decimal dFreightOut = 0.0m;
        private string sFreightOut = "";
        private string sNewFreightOut = "";
        private string sRoute = "";
        private string sSalesSubTotal = "";
        private string sRequireHandlingCharge = "";
        private string sShipToID = "";
        private string sShipToExempt = "";
        private string sOeHdrExempt = "";
        private bool bDebugMode;

        public override RuleResult Execute()
        {
            RuleResult result = new RuleResult();
            result.Success = true;

            DataFields fields = Data.Fields;

            sRMAFlag = fields.GetFieldByAlias("rma_flag").FieldValue;
            sOeHdrExempt = fields.GetFieldByAlias("exempt_delivery_charge").FieldValue;
            sShipToID = fields.GetFieldByAlias("ship_to_id").FieldValue;

            if (sRMAFlag != "Y")
            {
                if (sOeHdrExempt != "Y")
                {
                    CheckExempt(sShipToID, result);

                    if (sShipToExempt != "Y")
                    {
                        sRoute = fields.GetFieldByAlias("ship_route").FieldValue;

                        if (bDebugMode)
                        {
                            MessageBox.Show("sRoute: " + sRoute);
                        }

                        if (!string.IsNullOrEmpty(sRoute))
                        {
                            if (bDebugMode)
                            {
                                MessageBox.Show("About to check route.");
                            }

                            CheckRoute(sRoute, result);

                            if (sRequireHandlingCharge == "Y")
                            {
                                if (bDebugMode)
                                {
                                    MessageBox.Show("We will now check and see if the freight should be applied and then allow the save");
                                }

                                sSalesSubTotal = fields.GetFieldByAlias("sales_sub_total").FieldValue;

                                if (bDebugMode)
                                {
                                    MessageBox.Show("sSalesSubTotal: " + sSalesSubTotal);
                                }

                                dSalesSubTotal = Convert.ToDecimal(sSalesSubTotal);

                                if (bDebugMode)
                                {
                                    MessageBox.Show("dSalesSubTotal: " + dSalesSubTotal);
                                    MessageBox.Show("About to check subtotal.");
                                }

                                if (dSalesSubTotal < 250.00m && dSalesSubTotal > 0.00m)
                                {
                                    if (bDebugMode)
                                    {
                                        MessageBox.Show("Less than 250.00 and greater than 0.00.");
                                        MessageBox.Show("Charge required.");
                                    }

                                    sFreightOut = fields.GetFieldByAlias("freight_out").FieldValue;

                                    if (bDebugMode)
                                    {
                                        MessageBox.Show("sFreightOut: " + sFreightOut);
                                    }

                                    if (!string.IsNullOrEmpty(sFreightOut))
                                    {
                                        dFreightOut = Convert.ToDecimal(sFreightOut);
                                    }

                                    dNewFreightOut = 15.00m;

                                    if (bDebugMode)
                                    {
                                        MessageBox.Show("dNewFreightOut: " + dNewFreightOut);
                                    }

                                    sNewFreightOut = dNewFreightOut.ToString();

                                    if (bDebugMode)
                                    {
                                        MessageBox.Show("sNewFreightOut: " + sNewFreightOut);
                                        MessageBox.Show("Set the new freight out.");
                                    }

                                    fields.GetFieldByAlias("freight_out").FieldValue = sNewFreightOut;
                                }
                                else
                                {
                                    if (bDebugMode)
                                    {
                                        MessageBox.Show("About to zero out the freight.");
                                    }

                                    // Preserve legacy behavior exactly: this branch writes the
                                    // existing sNewFreightOut field value, which defaults to "".
                                    fields.GetFieldByAlias("freight_out").FieldValue = sNewFreightOut;
                                }
                            }
                        }
                    }
                    else if (bDebugMode)
                    {
                        MessageBox.Show("We have an exempt ship to so we didn't do anything.");
                    }
                }
                else if (bDebugMode)
                {
                    MessageBox.Show("We have an exempt order so we didn't do anything.");
                }
            }
            else if (bDebugMode)
            {
                MessageBox.Show("We have an RMA so we didn't do anything.");
            }

            return result;
        }

        public RuleResult CheckRoute(string sRouteValue, RuleResult result)
        {
            const string sql = @"
select shipping_route_ud.require_handling_charge
from shipping_route
left join shipping_route_ud
    on shipping_route.shipping_route_uid = shipping_route_ud.shipping_route_uid
where shipping_route.route_code = @routeCode";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, P21SqlConnection))
                {
                    command.Parameters.Add("@routeCode", SqlDbType.VarChar).Value = (object)sRouteValue ?? DBNull.Value;
                    object scalar = command.ExecuteScalar();

                    if (scalar != null && scalar != DBNull.Value)
                    {
                        if (bDebugMode)
                        {
                            MessageBox.Show("We have a route.");
                        }

                        sRequireHandlingCharge = Convert.ToString(scalar);
                        if (string.IsNullOrEmpty(sRequireHandlingCharge))
                        {
                            sRequireHandlingCharge = "N";
                        }
                    }
                    else if (bDebugMode)
                    {
                        MessageBox.Show("We do NOT have a route, this is a problem.");
                    }
                }
            }
            catch (SqlException ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public RuleResult CheckExempt(string sShipToIdValue, RuleResult result)
        {
            const string sql = @"
select exempt_delivery_charges
from ship_to_ud
where ship_to_id = @shipToId";

            try
            {
                using (SqlCommand command = new SqlCommand(sql, P21SqlConnection))
                {
                    command.Parameters.Add("@shipToId", SqlDbType.VarChar).Value = (object)sShipToIdValue ?? DBNull.Value;
                    object scalar = command.ExecuteScalar();

                    if (scalar != null && scalar != DBNull.Value)
                    {
                        if (bDebugMode)
                        {
                            MessageBox.Show("We have a ship_to_ud.");
                        }

                        sShipToExempt = Convert.ToString(scalar);
                        if (string.IsNullOrEmpty(sShipToExempt))
                        {
                            sShipToExempt = "N";
                        }
                    }
                    else
                    {
                        if (bDebugMode)
                        {
                            MessageBox.Show("We do NOT have a ship_to_ud, this is okay.");
                        }

                        sShipToExempt = "N";
                    }
                }
            }
            catch (SqlException ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public override string GetDescription()
        {
            return "KomarFrghtHandCharge56189 - Sets freight handling charge as required";
        }

        public override string GetName()
        {
            return "KomarFrghtHandCharge56189";
        }
    }
}
