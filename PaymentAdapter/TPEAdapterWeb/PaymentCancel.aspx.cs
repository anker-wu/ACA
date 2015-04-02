#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentCancel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *  Begin Payment
 *
 *  Notes:
 * $Id: PaymentCancel.cs 135124 2010-07-19 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    using System;

    /// <summary>
    /// deal with cancelled payment 
    /// </summary>
    public partial class PaymentCancel : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentCancel));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMessage = "The 3rd party payment is cancelled.";
            _logger.Error(errorMessage);
            PaymentHelper.HandleErrorRedirect(errorMessage, PaymentConstant.FAILURE_CODE);
        }
    }
}
