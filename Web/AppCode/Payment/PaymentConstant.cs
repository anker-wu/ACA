#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentConstant.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: PaymentConstant.cs 130988 2009-10-16  17:50:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Payment
{
    #region Enumerations

    /// <summary>
    /// Payment Method Constant.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Credit cart Payment.
        /// </summary>
        CreditCard = 0,

        /// <summary>
        /// Trust Account Payment.
        /// </summary>
        TrustAccount = 1,

        /// <summary>
        /// Check Payment.
        /// </summary>
        Check = 2,
    }

    /// <summary>
    /// Payment adapter type
    /// </summary>
    public enum PaymentAdapterType
    {
        /// <summary>
        /// unknown payment type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// API payment service
        /// </summary>
        APIPayment = 1,

        /// <summary>
        /// CoBrandPlus payment service
        /// </summary>
        CoBrandPlus = 2,

        /// <summary>
        /// ETISALAT payment service
        /// </summary>
        Etisalat = 3,

        /// <summary>
        /// GOVOLUTION payment service
        /// </summary>
        Govolution = 4,

        /// <summary>
        /// FirstData payment service
        /// </summary>
        FirstData = 5,

        /// <summary>
        /// External payment service
        /// </summary>
        ExternalAdapter = 6
    }

    /// <summary>
    /// payment result message type.
    /// </summary>
    public enum PaymentMessageType
    {
        /// <summary>
        /// the none
        /// </summary>
        None,

        /// <summary>
        /// money is not enough.
        /// </summary>
        NotEnoughMoney,

        /// <summary>
        /// charge amount out of the max amount limited.
        /// </summary>
        OutOfMaxAmount,

        /// <summary>
        /// The others payment error message type.
        /// </summary>
        Others
    }

    /// <summary>
    /// the payment method to be selected.
    /// </summary>
    public enum PaymentSelectedOption
    {
        /// <summary>
        /// not selected.
        /// </summary>
        None,

        /// <summary>
        /// selected default item.
        /// </summary>
        Default,

        /// <summary>
        /// selected trust account.
        /// </summary>
        TrustAccount
    }

    #endregion Enumerations

    /// <summary>
    /// This class provide the payment constant.
    /// </summary>
    public static class PaymentConstant
    {
        #region Request Parameters from ACA to Adapter
        
        /// <summary>
        /// the agency code 
        /// </summary>
        public const string ACA2ADAPTER_AGENCY_CODE = "AgencyCode";

        /// <summary>
        /// the AA application id
        /// </summary>
        public const string ACA2ADAPTER_APPLICATION_ID = "ApplicationID";

        /// <summary>
        /// the AA transaction id
        /// </summary>
        public const string ACA2ADAPTER_TRANSACTION_ID = "TransactionID";

        /// <summary>
        /// URL parameter for payment method
        /// </summary>
        public const string ACA2ADAPTER_PAYMENT_METHOD = "PaymentType";

        /// <summary>
        /// URL parameter for merchant account id.
        /// </summary>
        public const string ACA2ADAPTER_MERCHANT_ACCOUNTID = "MerchantAccountID";

        /// <summary>
        /// payment amount
        /// </summary>
        public const string ACA2ADAPTER_PAYMENT_AMOUNT = "PaymentAmount";

        /// <summary>
        /// convenience fee
        /// </summary>
        public const string ACA2ADAPTER_CONVENIENCE_FEE = "ConvFee";

        /// <summary>
        /// post back url that the adapter used to do completion process
        /// </summary>
        public const string ACA2ADAPTER_POSTBACK_URL = "PostbackURL";

        /// <summary>
        /// redirect url that the adapter will redirect to when finishing payment
        /// </summary>
        public const string ACA2ADAPTER_REDIRECT_URL = "RedirectURL";

        /// <summary>
        /// the public user id
        /// </summary>
        public const string ACA2ADAPTER_USER_ID = "UserID";

        /// <summary>
        /// user full name
        /// </summary>
        public const string ACA2ADAPTER_FULL_NAME = "FullName";

        /// <summary>
        /// user email
        /// </summary>
        public const string ACA2ADAPTER_USER_EMAIL = "UserEmail";

        /// <summary>
        /// the language
        /// </summary>
        public const string ACA2ADAPTER_LANG_FLAG = "Lang";

        #endregion Request Parameters from ACA to Adapter

        #region Postback Parameters from Adapter to ACA

        /// <summary>
        /// AA transaction id
        /// </summary>
        public const string ADAPTER2ACA_TRANSACTION_ID = "TransactionID";

        /// <summary>
        /// return code
        /// </summary>
        public const string ADAPTER2ACA_RETURN_CODE = "ReturnCode";

        /// <summary>
        /// payment amount
        /// </summary>
        public const string ADAPTER2ACA_PAYMENT_AMOUNT = "PaymentAmount";

        /// <summary>
        /// convenience fee
        /// </summary>
        public const string ADAPTER2ACA_CONVENIENCE_FEE = "ConvFee";

        /// <summary>
        /// the 3rd transaction id
        /// </summary>
        public const string ADAPTER2ACA_PROC_TRANSACTION_ID = "ProcTransactionID";

        /// <summary>
        /// Check/Credit Card
        /// </summary>
        public const string ADAPTER2ACA_PROC_TRANSACTION_TYPE = "ProcTransactionType";

        /// <summary>
        /// Visa/ Discover/ MasterCard
        /// </summary>
        public const string ADAPTER2ACA_CC_TYPE = "CCType";

        /// <summary>
        /// the card <c>AuthCode</c> from 3rd
        /// </summary>
        public const string ADAPTER2ACA_CC_AUTH_CODE = "CCAuthCode";

        /// <summary>
        /// credit cart number
        /// </summary>
        public const string ADAPTER2ACA_CC_NUMBER = "CCNumber";

        /// <summary>
        /// the person who makes the payment
        /// </summary>
        public const string ADAPTER2ACA_PAYEE = "Payee";

        /// <summary>
        /// the payee's address 
        /// </summary>
        public const string ADAPTER2ACA_PAYEE_ADDRESS = "PayeeAdderss";

        /// <summary>
        /// the payee's phone 
        /// </summary>
        public const string ADAPTER2ACA_PAYEE_PHONE = "PayeePhone";

        /// <summary>
        /// the payment's comment 
        /// </summary>
        public const string ADAPTER2ACA_PAYMENT_COMMENT = "PaymentComment";

        /// <summary>
        /// the custom batch transaction number
        /// </summary>
        public const string ADAPTER2ACA_CUSTOM_BATCH_TRANS_NBR = "CustomBatchTransNbr";

        /// <summary>
        /// the user message come from 3rd web site
        /// </summary>
        public const string ADAPTER2ACA_USER_MESSAGE = "UserMessage";

        #endregion Postback Parameters from Adapter to ACA
        
        /// <summary>
        /// payment method pass from ACA to adapter, Credit Card
        /// </summary>
        public const string PAYMENT_TYPE_CREDITCARD = "CC";

        /// <summary>
        /// payment method pass from ACA to adapter, e-check
        /// </summary>
        public const string PAYMENT_TYPE_CHECK = "EC";

        /// <summary>
        /// URL parameter for customized, current used in redirect payment.
        /// </summary>
        public const string CUSTOMIZED_URL_PARAMETER = "CustomizedUrlParameter";
    }
}
