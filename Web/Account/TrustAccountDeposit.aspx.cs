#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TrustAccountDeposit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TrustAccountDeposit.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Services;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// trust account deposit page.
    /// </summary>
    public partial class TrustAccountDeposit : BasePage, IPaymentPage
    {
        /// <summary>
        /// Gets the payment entity type
        /// </summary>
        public ACAConstant.PaymentEntityType PaymentEntityType
        {
            get
            {
                return ACAConstant.PaymentEntityType.TrustAccount;
            }
        }

        #region Methods

        /// <summary>
        /// Get Default Public User Model
        /// </summary>
        /// <param name="seqNumber">The sequence number.</param>
        /// <returns>string for public user info.</returns>
        [WebMethod(Description = "GetContactModel", EnableSession = true)]
        public static string GetContactModel(string seqNumber)
        {
            return AccountUtil.GetContact(seqNumber);
        }

        /// <summary>
        /// Construct CheckModel.
        /// </summary>
        /// <returns>a CheckModel4WS</returns>
        public CheckModel4WS GetCheckModel()
        {
            return this.Payment.GetCheckModel();
        }

        /// <summary>
        /// Construct CreditCardModel.
        /// </summary>
        /// <returns>a CreditCardModel4WS</returns>
        public CreditCardModel4WS GetCreditCardModel()
        {
            return this.Payment.GetCreditCardModel();
        }

        /// <summary>
        /// Construct a TrustAccountModel4WS from user's input
        /// </summary>
        /// <returns>a TrustAccountModel4WS</returns>
        public TrustAccountModel GetTrustAccountModel()
        {
            TrustAccountModel trustAccountModel = new TrustAccountModel();
            trustAccountModel.acctID = this.Payment.TrustAccountID;
            return trustAccountModel;
        }

        /// <summary>
        /// Get payment method.
        /// </summary>
        /// <returns>a PaymentMethod</returns>
        public PaymentMethod GetPaymentMethod()
        {
            return this.Payment.GetPaymentMethod();
        }

        /// <summary>
        /// Get Total Fee
        /// </summary>
        /// <returns>double for total fee.</returns>
        public double GetTotalFee()
        {
            // Only get the pay amount in trust account deposit, the convenience fee must NOT deposited into trust account.
            return Payment.GetChargedAmount();
        }

        /// <summary>
        /// Raises the page load event 
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // remove the cap id list session that maybe set in the shopping cart checkout
            AppSession.SetCapIDModelsToSession(null);
        }

        #endregion
    }
}
