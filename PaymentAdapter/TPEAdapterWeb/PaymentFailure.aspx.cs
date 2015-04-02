#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentFailure.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *  Begin Payment
 *
 *  Notes:
 * $Id: PaymentFailure.cs 135124 2010-07-19 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    using System;
    using System.Collections;
    using TPEWebService;

    /// <summary>
    /// deal with failed payment
    /// </summary>
    public partial class PaymentFailure : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentFailure));

        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// Initializes a new instance of the PaymentFailure class.
        /// </summary>
        public PaymentFailure()
        {
            _timeFlag = DateTime.Now.Ticks;

            _logger.DebugFormat("---Page {0} Load begin [{1}]---", this.GetType().FullName, _timeFlag.ToString());
        }

        /// <summary>
        /// do dispose when leave this page
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            _logger.DebugFormat("---Page {0} Load End [{1}]---", this.GetType().FullName, _timeFlag.ToString());
        } 

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string redirectUrl = String.Empty;
            string errormessage = String.Empty;

            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);
            string token = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PROC_TRANS_ID);

            if (!String.IsNullOrEmpty(token))
            {
                try
                {
                    using (ServiceWebClient proxy = new ServiceWebClient())
                    {
                        PaymentResult paymentResult = proxy.QueryPayment(token);

                        if (paymentResult == null)
                        {
                            errormessage = "Can not get transaction from web service or redirect url is empty.";
                        }
                        else if (paymentResult.FAILCODE.ToUpper() != "N")
                        {
                            errormessage = paymentResult.FAILMESSAGE;
                        }
                        else
                        {
                            redirectUrl = "/PaymentSuccess.aspx";
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error("Error occurred when querying payment by token.", ex);
                    errormessage = ex.Message;
                }
            }
            else
            {
                errormessage = "payment failed, no token";
            }

            if (!String.IsNullOrEmpty(redirectUrl))
            {
                Response.Redirect(redirectUrl);
            }
            else
            {
                if (String.IsNullOrEmpty(errormessage))
                {
                    errormessage = "Unknown error.";
                }

                _logger.Error(errormessage);
                PaymentHelper.HandleErrorRedirect(errormessage, PaymentConstant.FAILURE_CODE);
            } 
        }
    }
}
