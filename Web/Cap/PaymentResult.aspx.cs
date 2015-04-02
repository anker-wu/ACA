#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentResult.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PaymentResult.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.Security;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation payment result. 
    /// </summary>
    [SuppressCsrfCheck]
    public partial class PaymentResult : Page
    {
        #region Fields

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentResult));

        #endregion Fields

        #region Methods

        /// <summary>
        /// <c>OnPreInit</c> event.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreInit(EventArgs e)
        {
            Uri url = Session[ACAConstant.CURRENT_URL] as Uri;

            if (url != null && url.AbsoluteUri.IndexOf("CapCompletion.aspx") > -1 && url.AbsoluteUri.IndexOf("&fromResult=true") > -1)
            {
                Response.Redirect(url.AbsoluteUri);
                return;
            }

            base.OnPreInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.Etisalat)
            {
                Hashtable htPostBackData = EtisalatAdapter.GetPostBackData();
                htPostBackData["fromResult"] = "true";
                string redirectionURL = EtisalatAdapter.CompletePayment(htPostBackData);
                Response.Redirect(redirectionURL);
            }
            else
            {
                try
                {
                    //IPayment paymentProcessor = PaymentProcessorFactory.CreateProcessor();
                    //paymentProcessor.HandlePostbackData(this);
                    IHandler handler = PaymentProcessorFactory.CreateHandler();
                    handler.HandlePostbackData(this);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error occurred when handling payment result.", ex);
                    string errorMessageID = DateTime.Now.Ticks.ToString();
                    Session[errorMessageID] = ex.Message;
                    Response.Redirect(string.Format("../Payment/PaymentErrorPage.aspx?ErrorMessageID={0}", errorMessageID), false);
                }
            }
        }

        #endregion Methods
    }
}