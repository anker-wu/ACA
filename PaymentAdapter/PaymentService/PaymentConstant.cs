/**
 *  Accela Citizen Access
 *  File: PaymentConstant.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *   
 * 
 *  Notes:
 * $Id: PaymentConstant.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

namespace Accela.ACA.PaymentAdapter
{
    using System;

    /// <summary>
    /// payment constant
    /// </summary>
    public class PaymentConstant
    {
        /// <summary>
        /// application id
        /// </summary>
        public const string APPLICATION_ID = "ApplicationID";

        /// <summary>
        /// transaction id
        /// </summary>
        public const string TRANSACTION_ID = "TransactionID";

        /// <summary>
        /// payment amount
        /// </summary>
        public const string PAYMENT_AMOUNT = "PaymentAmount";

        /// <summary>
        /// convenience fee
        /// </summary>
        public const string CONVENIENCE_FEE = "ConvFee";

        /// <summary>
        /// postback url which used to communion with ACA
        /// </summary>
        public const string POSTBACK_URL = "PostbackURL";

        /// <summary>
        /// when Govolution complete process, it will redirect to this ACA page
        /// </summary>
        public const string REDIRECT_URL = "RedirectURL";

        /// <summary>
        /// the public user id
        /// </summary>
        public const string USER_ID = "UserID";

        /// <summary>
        /// the user full name
        /// </summary>
        public const string FULL_NAME = "FullName";

        /// <summary>
        /// the user email
        /// </summary>
        public const string USER_EMAIL = "UserEmail";

        /// <summary>
        /// return code
        /// </summary>
        public const string RETURN_CODE = "ReturnCode"; 

        /// <summary>
        /// the user message come from 3rd web site
        /// </summary>
        public const string USER_MESSAGE = "UserMessage";

        /// <summary>
        /// the payment type
        /// </summary>
        public const string PAYMENT_TYPE = "PaymentType";

        /// <summary>
        /// the card type
        /// </summary>
        public const string CC_TYPE = "CCType";

        /// <summary>
        /// the card number
        /// </summary>
        public const string CC_NUMBER = "CCNumber";

        /// <summary>
        /// the card auth code
        /// </summary>
        public const string CC_AuthCode = "CCAuthCode";

        /// <summary>
        /// the ACA transaction success status
        /// </summary>
        public const string ACA_TRANS_SUCCESS = "ACATransSuccess";

        /// <summary>
        /// the 3rd transaction id
        /// </summary>
        public const string PROC_TRANS_ID = "ProcTransactionID";

        /// <summary>
        /// the 3rd transaction status
        /// </summary>
        public const string PROC_TRANS_STATUS = "ProcTransStatus";

        /// <summary>
        /// the 3rd transaction type
        /// </summary>
        public const string PROC_TRANS_TYPE = "ProcTransactionType"; 

        /// <summary>
        /// success code
        /// </summary>
        public const string SUCCESS_CODE = "1";

        /// <summary>
        /// failure code
        /// </summary>
        public const string FAILURE_CODE = "-1";

        /// <summary>
        /// the customer cancel payment and exit the web site
        /// </summary>
        public const string EXIT_CODE = "0";

        /// <summary>
        /// pay method check 
        /// </summary>
        public const string PAY_METHOD_CHECK = "Check";

        /// <summary>
        /// pay method credit card 
        /// </summary>
        public const string PAY_METHOD_CREDIT_CARD = "Credit Card";

        /// <summary>
        /// the person who makes the payment
        /// </summary>
        public const string PAYEE = "Payee";

        /// <summary>
        /// the payee's address 
        /// </summary>
        public const string PAYEE_ADDRESS = "PayeeAdderss";

        /// <summary>
        /// the payee's phone 
        /// </summary>
        public const string PAYEE_PHONE = "PayeePhone";

        /// <summary>
        /// the payment comment
        /// </summary>
        public const string PAYMENT_COMMENT = "PaymentComment";

        public const string CUSTOM_BATCH_TRANS_NBR = "CustomBatchTransNbr";

        /// <summary>
        /// the agency code 
        /// </summary>
        public const string AGENCY_CODE = "AgencyCode";

        /// <summary>
        /// the merchant account id
        /// </summary>
        public const string MERCHANT_ACCOUNT_ID = "MerchantAccountID";
    }
}
