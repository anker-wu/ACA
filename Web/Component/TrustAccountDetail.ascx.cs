#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TrustAccountDetail.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: TrustAccountDetail.cs 178503 2010-08-10 05:49:16Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation TrustAccountDetail. 
    /// </summary>
    public partial class TrustAccountDetail : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// Display trust account.
        /// </summary>
        /// <param name="trustAccount">TrustAccount model.</param>
        public void Display(TrustAccountModel trustAccount)
        {
            if (trustAccount != null)
            {
                lblTrustAcctIDValue.Text = trustAccount.acctID;
                double balance = trustAccount.acctBalance.HasValue ? trustAccount.acctBalance.Value : 0.0;
                lblBalanceValue.Text = I18nNumberUtil.FormatMoneyForUI(balance);

                lblLedgerAccountValue.Text = trustAccount.ledgerAccount;
                lblStatusValue.Text = TrustAccountUtil.ConvertStatusField2Display(trustAccount.acctStatus);
                lblDescriptionValue.Text = I18nStringUtil.GetString(trustAccount.resDescription, trustAccount.description);
            }
        }

        #endregion Methods
    }
}
