#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentCompletion.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PaymentCompletion.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation payment completion. 
    /// </summary>
    public partial class PaymentCompletion : BasePage
    {
        #region Fields

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentCompletion));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string redirectURL = string.Empty;

            try
            {
                IHandler handler = PaymentProcessorFactory.CreateHandler();
                redirectURL = handler.HandlePaymentResult(this);
                redirectURL = FileUtil.AppendApplicationRoot(redirectURL);
                Session[ACAConstant.CURRENT_URL] = redirectURL;
                Response.Redirect(ACAConstant.URL_DEFAULT_PAGE);
            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred.", ex);
                string errorMessageID = DateTime.Now.Ticks.ToString();
                Session[errorMessageID] = ex.Message;
                redirectURL = string.Format("../Payment/PaymentErrorPage.aspx?ErrorMessageID={0}", errorMessageID);
            }

            Logger.InfoFormat("begin to redirect to url={0} \n\r", redirectURL);
            Response.Redirect(redirectURL);
        }

        #endregion Methods
    }
}