#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentHandle.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PaymentHandle.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common.Log;
using log4net;

namespace Accela.ACA.Web.Payment.Govolution
{
    /// <summary>
    /// This class provide the ability to operation GOVOLUTION.
    /// </summary>
    public partial class Payment_Govolution_PaymentHandle : System.Web.UI.Page
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Payment_Govolution_PaymentHandle));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LogPostbackDataFromGovolution();

            try
            {
                IHandler handler = PaymentProcessorFactory.CreateHandler();
                handler.HandlePostbackData(this);
            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred during handling postback data.", ex);
            }
        }

        /// <summary>
        /// Log post back data from GOVOLUTION.
        /// </summary>
        private void LogPostbackDataFromGovolution()
        {
            string allRequestString = PaymentHelper.GetPostbackDataString();
            Logger.DebugFormat("Entering PaymentHandle.aspx Page, the request data from Govolution V+Relay are: \n\r" + allRequestString);
        }

        #endregion Methods
    }
}