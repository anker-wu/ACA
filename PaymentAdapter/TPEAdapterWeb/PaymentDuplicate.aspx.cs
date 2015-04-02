#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentDuplicate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *  Begin Payment
 *
 *  Notes:
 * $Id: PaymentDuplicate.cs 135124 2010-07-19 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    using System;

    /// <summary>
    /// deal with dulicate payment
    /// </summary>
    public partial class PaymentDuplicate : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentDuplicate));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string errorMessage = "Duplicate Transaction Found.";
            _logger.Error(errorMessage);
            PaymentHelper.HandleErrorRedirect(errorMessage, PaymentConstant.FAILURE_CODE);
        }
    }
}
