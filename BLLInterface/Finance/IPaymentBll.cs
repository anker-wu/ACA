#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPaymentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  IPaymentBll interface..
 *
 *  Notes:
 * $Id: IPaymentBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// Defines method sign for Payment BLL.
    /// </summary>
    public interface IPaymentBll
    {
        #region Methods

        /// <summary>
        /// Complete payment
        /// </summary>
        /// <param name="creditCardModel4WS">CreditCardModel4WS object.</param>
        /// <param name="checkModel4WS">Array of CheckModel4WS</param>
        /// <param name="capIDs">Array of CapIDModel</param>
        /// <param name="parameters">array of parameter</param>
        /// <param name="adapterName">adapter Name</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Online Payment Result object.</returns>
        OnlinePaymentResultModel CompletePayment(CreditCardModel4WS creditCardModel4WS, CheckModel4WS checkModel4WS, CapIDModel[] capIDs, string parameters, string adapterName, string callerID);

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
        /// <returns>Online Payment Result object</returns>
        OnlinePaymentResultModel CompleteGenericPayment(CreditCardModel4WS creditCardModel4WS, CheckModel4WS checkModel4WS, string entityID, string entityType, string parameters, string paymentProvider, string callerID);

        /// <summary>
        /// Initiate payment
        /// </summary>
        /// <param name="capIDModels">Array of CapIDModel</param>
        /// <param name="parameters">parameters list.</param>        
        /// <param name="adapterName">adapter Name</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Online Payment Result object.</returns>
        OnlinePaymentResultModel InitiatePayment(CapIDModel[] capIDModels, string parameters, string adapterName, string callerID);

        /// <summary>
        /// Verify payment
        /// </summary>
        /// <param name="parameters">parameter list.</param>
        /// <param name="adapterName">adapter name.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Online Payment Result object.</returns>
        OnlinePaymentResultModel VerifyPayment(string parameters, string adapterName, string callerID);

        #endregion Methods
    }
}