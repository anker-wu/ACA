#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Payment_PaymentErrorPage.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PaymentErrorPage.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// This class provide the ability to operation Payment_PaymentErrorPage.
    /// </summary>
    public partial class Payment_PaymentErrorPage : BasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMessageID = Request["ErrorMessageID"];
            string errorMessage = string.Empty;

            if (!string.IsNullOrEmpty(errorMessageID) && Session[errorMessageID] != null)
            {
                errorMessage = Session[errorMessageID].ToString();
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = LabelUtil.GetGlobalTextByKey("aca_online_payment_failed");
            }

            lblErrorMessage.Text = HttpUtility.UrlDecode(errorMessage);

            // update the ETransaction indicates the payment is failed.
            if (!IsPostBack)
            {
                string agencyCode = PaymentHelper.ConvertToString(Request[UrlConstant.AgencyCode]);
                string strTransId = PaymentHelper.ConvertToString(Request[PaymentConstant.ADAPTER2ACA_TRANSACTION_ID]);
                long transactionId = 0;

                if (!string.IsNullOrEmpty(strTransId) && long.TryParse(strTransId, out transactionId))
                {
                    IOnlinePaymenBll onlinePayment = ObjectFactory.GetObject<IOnlinePaymenBll>();
                    onlinePayment.UpdateFailStatus4AllTransactions(agencyCode, transactionId, errorMessage);

                    // If it has paid in 3rd payment gateway but process failed, these paid records will not at the shopping cart, 
                    // so it need update the shopping cart item number.
                    ShoppingCartUtil.SetCartItemNumber();
                }
            }
        }

        #endregion Methods
    }
}