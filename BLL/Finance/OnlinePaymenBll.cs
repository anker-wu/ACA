#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: OnlinePaymenBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: OnlinePaymenBll.cs 278225 2014-08-29 08:02:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/15/2007    ken.huang    Initial.
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operation online payment.
    /// </summary>
    public class OnlinePaymenBll : BaseBll, IOnlinePaymenBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of OnlinePaymentService.
        /// </summary>
        private OnlinePaymentWebServiceService OnlinePaymentService
        {
            get
            {
                return WSFactory.Instance.GetWebService<OnlinePaymentWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Calculate Convenience Fee
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="amount">the amount of charge</param>
        /// <param name="paymentMethod">The payment method, Credit Card/Check.</param>
        /// <param name="merchantAccountID">The merchant account ID.</param>
        /// <returns>On Line Payment Return Info</returns>
        CreditCardModel4WS IOnlinePaymenBll.CalculateConvenienceFee(string agencyCode, double amount, string paymentMethod, string merchantAccountID)
        {
            try
            {
                return OnlinePaymentService.calculateConvenienceFee(agencyCode, amount, paymentMethod, merchantAccountID, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Calculate Convenience Fee
        /// </summary>
        /// <param name="servProvCode">Agency code</param>
        /// <param name="adapterName">the payment adapter name</param>
        /// <param name="capIds">the CapIDModel4WS array</param>
        /// <param name="paymentMethod">The payment method, Credit Card/Check.</param>
        /// <param name="merchantAccountID">The merchant account ID.</param>
        /// <returns>OnLine Payment Return Info</returns>
        CreditCardModel4WS IOnlinePaymenBll.CalculateConvenienceFeeForShoppingCart(string servProvCode, string adapterName, CapIDModel[] capIds, string paymentMethod, string merchantAccountID)
        {
            try
            {
                return OnlinePaymentService.calculateConvenienceFee4ShoppingCart(AgencyCode, capIds, adapterName, paymentMethod, merchantAccountID, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Call EMSE to pay online.
        /// </summary>
        /// <param name="capIDModel">CapIDModel4WS entity</param>
        /// <param name="acaModel">ACAModel4WS entity</param>
        /// <param name="urlParameters">url Parameters</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>EMSEResultBaseModel4WS entity</returns>
        EMSEResultBaseModel4WS IOnlinePaymenBll.CallEMSEtoPayOnline(CapIDModel4WS capIDModel, ACAModel4WS acaModel, string urlParameters, string callerID)
        {
            try
            {
                return OnlinePaymentService.callEMSEtoPayOnline(capIDModel, acaModel, urlParameters, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Credit payment for self plan checking.
        /// </summary>
        /// <param name="ccModel">Credit Card entity</param>
        /// <param name="paymentModel">payment entity</param>
        /// <returns>Online Payment result entity</returns>
        /// <exception cref="DataValidateException">{ <c>ccModel4ws, paymentModel4ws, planSeqNbr</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        OnLinePaymentReturnInfo4WS IOnlinePaymenBll.CreditCardPayment4PlanReview(CreditCardModel4WS ccModel, PaymentModel paymentModel)
        {
            if (ccModel == null || paymentModel == null)
            {
                throw new DataValidateException(new string[] { "ccModel", "paymentModel", "planSeqNbr" });
            }

            try
            {
                OnLinePaymentReturnInfo4WS response = OnlinePaymentService.creditCardPayment4PlanReview(ccModel, paymentModel);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets receipt number by cap ID.
        /// </summary>
        /// <param name="capID">cap ID</param>
        /// <param name="qf">query format</param>
        /// <param name="callerID">caller ID</param>
        /// <returns>receipt number</returns>
        /// <exception cref="DataValidateException">{ <c>capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        string IOnlinePaymenBll.GetReceiptNoByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callerID)
        {
            if (capID == null)
            {
                throw new DataValidateException(new string[] { "capID" });
            }

            try
            {
                return OnlinePaymentService.getReceiptNoByCapID(capID, qf, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Online payment process
        /// </summary>
        /// <param name="capID4WS">Model for CapID </param>
        /// <param name="acaModel4WS">Module for ACA</param>
        /// <param name="notificationURL">notification URL</param>
        /// <param name="callerID">the public user id.</param>
        /// <returns>EMSE Result Base Model</returns>
        EMSEResultBaseModel4WS IOnlinePaymenBll.OnlinePaymentProcess(CapIDModel4WS capID4WS, ACAModel4WS acaModel4WS, string notificationURL, string callerID)
        {
            try
            {
                EMSEResultBaseModel4WS response = OnlinePaymentService.onlinePaymentProcess(capID4WS, acaModel4WS, notificationURL, callerID);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the merchant account by cap type.
        /// </summary>
        /// <param name="capTypeModel">The cap type, the agency code/group/type/sub-type/category are required.</param>
        /// <param name="needDecrypt">The merchant account need decrypt or not.</param>
        /// <returns>return the merchant account.</returns>
        RefMerchantAccountModel IOnlinePaymenBll.SearchMerchantAccount(CapTypeModel capTypeModel, bool needDecrypt)
        {
            try
            {
                RefMerchantAccountModel response = OnlinePaymentService.searchMerchantAccount(capTypeModel, needDecrypt);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update the transactions' status to Fail
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <param name="batchTransCode">The batch transaction number</param>
        /// <param name="errorMessage">error message from payment</param>
        void IOnlinePaymenBll.UpdateFailStatus4AllTransactions(string servProvCode, long batchTransCode, string errorMessage)
        {
            try
            {
                OnlinePaymentService.updateFailStatus4AllTransactions(servProvCode, batchTransCode, User.PublicUserId, errorMessage);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}