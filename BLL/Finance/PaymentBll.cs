#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
  *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operation payment.
    /// </summary>
    public class PaymentBll : BaseBll, IPaymentBll
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
        /// Complete payment
        /// </summary>
        /// <param name="creditCardModel4WS">CreditCard Model</param>
        /// <param name="checkModel4WS">Check Model.</param>
        /// <param name="capIDs">CapIDModel array.</param>
        /// <param name="parameters">parameters data.</param>
        /// <param name="adapterName">adapter Name</param>
        /// <param name="callerID">the public user id.</param>
        /// <returns>OnlinePaymentResult Model</returns>
        OnlinePaymentResultModel IPaymentBll.CompletePayment(CreditCardModel4WS creditCardModel4WS, CheckModel4WS checkModel4WS, CapIDModel[] capIDs, string parameters, string adapterName, string callerID)
        {
            try
            {
                return OnlinePaymentService.completePayment(creditCardModel4WS, checkModel4WS, capIDs, parameters, adapterName, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Complete payment for other thing, e.g. trust account.
        /// </summary>
        /// <param name="creditCardModel4WS">CreditCardModel4WS object.</param>
        /// <param name="checkModel4WS">Array of CheckModel4WS</param>
        /// <param name="entityID">payment for this account id</param>
        /// <param name="entityType">payment type, e.g. trust account</param>
        /// <param name="parameters">array of parameter</param>
        /// <param name="paymentProvider">adapter name</param>
        /// <param name="callerID">caller id number</param>
        /// <returns>OnlinePaymentResultModel object</returns>
        OnlinePaymentResultModel IPaymentBll.CompleteGenericPayment(CreditCardModel4WS creditCardModel4WS, CheckModel4WS checkModel4WS, string entityID, string entityType, string parameters, string paymentProvider, string callerID)
        {
            try
            {
                return OnlinePaymentService.completeGenericPayment(creditCardModel4WS, checkModel4WS, entityID, entityType, parameters, paymentProvider, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Initiate payment
        /// </summary>
        /// <param name="capIDModels">Array of CapIDModel4WS</param>
        /// <param name="parameters">parameters list.</param>        
        /// <param name="adapterName">adapter Name</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>OnlinePaymentResultModel object.</returns>
        OnlinePaymentResultModel IPaymentBll.InitiatePayment(CapIDModel[] capIDModels, string parameters, string adapterName, string callerID)
        {
            try
            {
                return OnlinePaymentService.initiatePayment(capIDModels, parameters, adapterName, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Verify payment
        /// </summary>
        /// <param name="parameters">parameter list.</param>
        /// <param name="adapterName">adapter name.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>OnlinePaymentResultModel object.</returns>
        OnlinePaymentResultModel IPaymentBll.VerifyPayment(string parameters, string adapterName, string callerID)
        {
            try
            {
                return OnlinePaymentService.verifyPayment(parameters, adapterName, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}