using System;
using System.Text;

namespace PaymentProviderEmulator
{
    public partial class Default : System.Web.UI.Page
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(Default));

        #endregion Fields

        #region Properties

        /// <summary>
        /// The parameters from payment adapter.
        /// </summary>
        protected string FromAdapterParameters { get; set; }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StringBuilder sb = new StringBuilder();

                // agency, application id, transaction id
                sb.AppendFormat(
                    "<strong>Transaction Info</strong><br/>Agency Code: {0}<br/>Application ID: {1}<br/>Transaction ID: {2}<br/><br/>",
                    Request[Constant.AGENCY_CODE],
                    Request[Constant.APPLICATION_ID],
                    Request[Constant.TRANSACTION_ID]);

                // user info
                sb.AppendFormat(
                    "<strong>User Info</strong><br/>User ID: {0}<br/>Full Name: {1}<br/>Email: {2}<br/><br/>",
                    Request[Constant.USER_ID],
                    Request[Constant.USER_FULL_NAME],
                    Request[Constant.USER_EMAIL]);
                    
                // payment info
                sb.AppendFormat(
                    "<strong>Payment Info</strong><br/>Payment Method: {0}<br/>Pay Amount: {1}<br/>Conv Fee: {2}<br/><br/>",
                    Request[Constant.PAYMENT_METHOD],
                    Request[Constant.PAY_AMOUNT],
                    Request[Constant.CONV_FEE]);

                // merchant account info
                sb.Append("<strong>Merchant Account Info</strong><br/>");

                if (string.IsNullOrEmpty(Request[Constant.MERCHANT_ACCOUNT_ID]) || Request[Constant.MERCHANT_ACCOUNT_ID] == "0")
                {
                    sb.Append("No Merchant Account<br/><br/>");
                }
                else
                {
                    sb.AppendFormat(
                        "Merchant Account ID: {0}<br/>Account Name: {1}<br/><br/>" +
                        "Account Number: {2}<br/>Principal Settle Number: {3}<br/>Principal Password: {4}<br/><br/>" +
                        "Conv Account Number: {5}<br/>Conv Settle Number: {6}<br/>Conv Settle Number: {7}<br/>Description: {8}<br/><br/>",
                        Request[Constant.MERCHANT_ACCOUNT_ID],
                        Request[Constant.MERCHANT_ACCOUNT_NAME],
                        Request[Constant.PRINCIPAL_ACCOUNT_NBR],
                        Request[Constant.PRINCIPAL_SETTLE_NBR],
                        Request[Constant.PRINCIPAL_PASSWORD],
                        Request[Constant.CONV_ACCOUNT_NBR],
                        Request[Constant.CONV_SETTLE_NBR],
                        Request[Constant.CONV_PASSWORD],
                        Request[Constant.MERCHANT_DESCRIPTION]);
                }

                FromAdapterParameters = sb.ToString();
            }
        }

        /// <summary>
        /// Handles the success button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnSuccess_Click(object sender, EventArgs e)
        {
            string redirectUrl = string.Format(
                "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}&{11}={12}",
                Request[Constant.PAYMENT_SUCCESS_URL],
                Constant.TRANSACTION_ID,
                Request[Constant.TRANSACTION_ID],
                Constant.APPLICATION_ID,
                Request[Constant.APPLICATION_ID],
                Constant.PAY_AMOUNT,
                Request[Constant.PAY_AMOUNT],
                Constant.CONV_FEE,
                Request[Constant.CONV_FEE],
                Constant.PAYMENT_METHOD,
                Request[Constant.PAYMENT_METHOD],
                Constant.PROVIDER_TRANS_ID,
                Guid.NewGuid().ToString());

            Logger.InfoFormat("Redirect to adapter URL when paid successful: {0}", redirectUrl);
            Response.Redirect(redirectUrl);
        }

        /// <summary>
        /// Handles the fail button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnFail_Click(object sender, EventArgs e)
        {
            string redirectUrl = string.Format(
                "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}",
                Request[Constant.PAYMENT_FAIL_URL],
                Constant.TRANSACTION_ID,
                Request[Constant.TRANSACTION_ID],
                Constant.APPLICATION_ID,
                Request[Constant.APPLICATION_ID],
                Constant.PAY_AMOUNT,
                Request[Constant.PAY_AMOUNT],
                Constant.CONV_FEE,
                Request[Constant.CONV_FEE],
                Constant.PAYMENT_METHOD,
                Request[Constant.PAYMENT_METHOD]);

            Logger.InfoFormat("Redirect to adapter URL when paid failed: {0}", redirectUrl);
            Response.Redirect(redirectUrl);
        }

        /// <summary>
        /// Handles the cancel button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string redirectUrl = string.Format(
                "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}",
                Request[Constant.PAYMENT_CANCEL_URL],
                Constant.TRANSACTION_ID,
                Request[Constant.TRANSACTION_ID],
                Constant.APPLICATION_ID,
                Request[Constant.APPLICATION_ID],
                Constant.PAY_AMOUNT,
                Request[Constant.PAY_AMOUNT],
                Constant.CONV_FEE,
                Request[Constant.CONV_FEE]);

            Logger.InfoFormat("Redirect to adapter URL when cancel payment: {0}", redirectUrl);
            Response.Redirect(redirectUrl);
        }

        #endregion Methods
    }
}