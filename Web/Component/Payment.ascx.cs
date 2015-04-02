#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Payment.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Display work location information
 *
 *  Notes:
 *      $Id: Payment.ascx.cs 277973 2014-08-25 06:20:14Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.ProxyUser;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Payment Control
    /// </summary>
    public partial class Payment : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The constant string for fee formula product, both(AA and ACA).
        /// </summary>
        private const string FEE_FORMULA_PRODUCT_BOTH = "Both";

        /// <summary>
        /// The constant string for fee formula product to ACA.
        /// </summary>
        private const string FEE_FORMULA_PRODUCT_ACA = "ACA";

        /// <summary>
        /// The constant string for fee formula's all credit card type.
        /// </summary>
        private const string FEE_FORMULA_CARD_TYPE_ALL = "All";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Payment));

        /// <summary>
        /// The trust account.
        /// </summary>
        private TrustAccountModel _trustAccountModel;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Pay4ExistingCap or not.
        /// </summary>
        public bool Pay4ExistingCap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMsg
        {
            get; 
            set; 
        }

        /// <summary>
        /// Gets the trust account id.
        /// </summary>
        public string TrustAccountID
        {
            get
            {
                string trustAccountID = Request.QueryString["accountID"];
                return string.IsNullOrEmpty(trustAccountID) ? string.Empty : trustAccountID;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Trust Account Deposit or not.(True: Trust Account Deposit; False: other payment, such as record.)
        /// </summary>
        private bool IsTrustAccountDeposit
        {
            get
            {
                return Request.Url.ToString().IndexOf("TrustAccountDeposit.aspx") >= 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether that exist convenience fee formula in XPolicy or not.
        /// </summary>
        private bool ExistConvenienceFeeFormulaInXPolicy
        {
            get
            {
                if (!IsTrustAccountDeposit)
                {
                    // judge all CAPs' agencies' convenience fee config
                    CapIDModel4WS[] capIds = AppSession.GetCapIDModelsFromSession();
                    List<string> agencies = new List<string>();

                    foreach (CapIDModel4WS capIdModel in capIds)
                    {
                        if (!agencies.Contains(capIdModel.serviceProviderCode))
                        {
                            agencies.Add(capIdModel.serviceProviderCode);
                        }
                    }

                    return EPaymentConfig.ExistConvenienceFeeFormula(agencies);
                }

                return EPaymentConfig.ExistConvenienceFeeFormula(null);
            }
        }

        /// <summary>
        /// Gets a value indicating whether that exist convenience fee formula or not.
        /// </summary>
        private bool ExistConvenienceFeeFormula
        {
            get
            {
                if (AppSession.IsAdmin)
                {
                    return true;
                }

                PaymentMethod enumPaymentMethod = GetPaymentMethod();

                // if pay for trust account, it need not calculate the convenience fee
                if (enumPaymentMethod == PaymentMethod.TrustAccount)
                {
                    return false;
                }

                List<RefMerchantAccountModel> merchantAccounts = PaymentHelper.GetMerchantAccount();

                if (merchantAccounts == null)
                {
                    // judge convenience fee formula in XPolicy
                    return ExistConvenienceFeeFormulaInXPolicy;
                }

                foreach (RefMerchantAccountModel merchantAccount in merchantAccounts)
                {
                    if (merchantAccount == null || merchantAccount.formulas == null || merchantAccount.formulas.Length == 0)
                    {
                        continue;
                    }

                    // judge convenience fee formula in Merchant Account
                    string paymentMethod = enumPaymentMethod == PaymentMethod.Check ? ACAConstant.PAY_METHOD_CHECK : ACAConstant.PAY_METHOD_CREDIT_CARD;

                    foreach (var formula in merchantAccount.formulas)
                    {
                        if (formula == null || 
                            (!FEE_FORMULA_PRODUCT_BOTH.Equals(formula.product, StringComparison.InvariantCultureIgnoreCase) && !FEE_FORMULA_PRODUCT_ACA.Equals(formula.product, StringComparison.InvariantCultureIgnoreCase)) ||
                            !paymentMethod.Equals(formula.paymentMethod, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        if ((ACAConstant.PAY_METHOD_CREDIT_CARD.Equals(paymentMethod, StringComparison.InvariantCultureIgnoreCase) && FEE_FORMULA_CARD_TYPE_ALL.Equals(formula.cardType, StringComparison.InvariantCultureIgnoreCase)) ||
                            ACAConstant.PAY_METHOD_CHECK.Equals(paymentMethod, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        #endregion Properties

        #region function
        /// <summary>
        /// Construct CheckModel.
        /// </summary>
        /// <returns>a CheckModel4WS</returns>
        public CheckModel4WS GetCheckModel()
        {
            CheckModel4WS checkModel = CheckProcessing.GetCheckModel();
            checkModel.balance = I18nNumberUtil.ConvertMoneyToInvariantString(this.GetChargedAmount());
            checkModel.convFee = I18nNumberUtil.ConvertMoneyToInvariantString(this.GetConvenienceFee());
            
            return checkModel;
        }

        /// <summary>
        /// Construct CreditCardModel.
        /// </summary>
        /// <returns>a CreditCardModel4WS</returns>
        public CreditCardModel4WS GetCreditCardModel()
        {
            CreditCardModel4WS creditCardModel = CreditCard.GetCreditCardModel();
            creditCardModel.balance = I18nNumberUtil.ConvertMoneyToInvariantString(this.GetChargedAmount());
            creditCardModel.convFee = I18nNumberUtil.ConvertMoneyToInvariantString(this.GetConvenienceFee());

            return creditCardModel;
        }

        /// <summary>
        /// Construct a TrustAccountModel4WS from user's input
        /// </summary>
        /// <returns>a TrustAccountModel4WS</returns>
        public TrustAccountModel GetTrustAccountModel()
        {
            TrustAccountModel trustAccount = null;

            if ((AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent) 
                && _trustAccountModel != null)
            {
                trustAccount = _trustAccountModel;
            }
            else
            {
                trustAccount = TrustAccount.GetTrustAccountModel();
            }

            trustAccount.acctBalance = GetChargedAmount();

            return trustAccount;
        }

        /// <summary>
        /// Get payment method.
        /// </summary>
        /// <returns>a PaymentMethod</returns>
        public PaymentMethod GetPaymentMethod()
        {
            PaymentMethod pm;

            // if agent/clerk user then only use trust account, use TrustAccount method.
            if ((AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent) && _trustAccountModel != null)
            {
                pm = PaymentMethod.TrustAccount;
            }
            else
            {
                string selectedValue = rdlPaymentMethod.SelectedValue;

                if (string.IsNullOrEmpty(selectedValue))
                {
                    return PaymentMethod.CreditCard;
                }

                if (selectedValue.IndexOf("||") != -1)
                {
                    selectedValue = selectedValue.Substring(0, selectedValue.IndexOf("||")); // remove bizDomainValue
                }

                pm = (PaymentMethod)int.Parse(selectedValue.Replace("-", string.Empty));
            }

            return pm;
        }

        /// <summary>
        /// Get Total Fee that include charged amount and convenience fee.
        /// </summary>
        /// <returns>double for total fee.</returns>
        public double GetTotalFee()
        {
            //1. Add Charged Amount of Cap or TrustAccount
            double chargedAmount = this.GetChargedAmount();

            //2. Add Convenience Fee of Cap or TrustAccount, if any.
            double convFee = this.GetConvenienceFee();

            return chargedAmount + convFee;
        }

        /// <summary>
        /// Get amount to be charged.
        /// </summary>
        /// <returns>double for amount</returns>
        public double GetChargedAmount()
        {
            double chargedAmount = 0;
            double tempParsingAmount = 0;
            string originalAmountString = IsTrustAccountDeposit ? this.txtDepositAmount.GetInvariantDoubleText() : this.hdnChargedAmount.Value;

            bool parsingResult = I18nNumberUtil.TryParseMoneyFromWebService(originalAmountString, out tempParsingAmount);

            if (parsingResult)
            {
                chargedAmount = tempParsingAmount;
            }
            else
            {
                Logger.DebugFormat("Error occurred when converting this.hdnChargedAmount.Value,,\n original value={0}", originalAmountString);
            }

            return chargedAmount;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _trustAccountModel = TrustAccountUtil.GetTrustAccountModel(string.Empty);

                BindPaymentMethodList();
                InitUI();

                if (!AppSession.IsAdmin)
                {
                    // The agent user use default trust account to payment the fee(s).
                    if ((AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent) && _trustAccountModel != null)
                    {
                        TrustAccount.TrustAccountID = _trustAccountModel.acctID;
                        DoPayment();
                    }
                    else if (IsDirectToPayment(GetPaymentMethod()))
                    {
                        // only one record and not trust account deposit, redirect to payfee directly.
                        DoPayment();
                    }
                }
            }
        }

        /// <summary>
        /// Dropdown list command
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PaymentMethodDropdown_IndexChanged(object sender, EventArgs e)
        {
            string eventTarget = Request["__EVENTTARGET"];
            AccelaRadioButtonList targetObj = sender as AccelaRadioButtonList;

            if ((!string.IsNullOrEmpty(eventTarget) && eventTarget.IndexOf("rdlPaymentMethod") > -1)
                || (targetObj != null && targetObj.ID.IndexOf("rdlPaymentMethod") > -1))
            {
                ChangePaymentMethod();

                if (!AppSession.IsAdmin)
                {
                    this.CheckProcessing.SetCurrentCityAndState();

                    // change the convenience fee by the payment method
                    InitConvenienceFeeUI();
                }
            }
        }

        /// <summary>
        /// SubmitPaymentPage Link Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SubmitPaymentPageLink_Click(object sender, EventArgs e)
        {
            DoPayment();
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ErrorMsg);
                RegisterScripts();
            }
        }

        /// <summary>
        /// textbox (deposit amount) text changed event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void DepositAmount_TextChanged(object sender, EventArgs e)
        {
            if (ExistConvenienceFeeFormula && IsTrustAccountDeposit)
            {
                double? depositAmount = txtDepositAmount.DoubleValue;
                double totalAmount = depositAmount == null ? 0 : depositAmount.Value;
                double convFee = GetConvenienceFee();
                lblConveninenceFee.Text = I18nNumberUtil.FormatMoneyForUI(convFee);
                lblTotalAmount.Text = I18nNumberUtil.FormatMoneyForUI(totalAmount + convFee);
            }
        }

        /// <summary>
        /// if the trust account has only one item.
        /// </summary>
        /// <param name="trustAccountID">trust account ID</param>
        /// <returns>HasSingleTrustAccount Flag</returns>
        private static bool IsSingleTrustAccount(ref string trustAccountID)
        {
            bool hasSingleTrustAccount = false;

            IList<string> allAssociatedTypes = TrustAccountUtil.GetAllAssociatedTypes();

            if (allAssociatedTypes != null && allAssociatedTypes.Count == 1)
            {
                ACAConstant.PaymentAssociatedType paymentAssociatedType = EnumUtil<ACAConstant.PaymentAssociatedType>.Parse(allAssociatedTypes[0]);
                IList<ListItem> associatedTypes = DropDownListBindUtil.GetTrustAccountValidAssociatedTypes(paymentAssociatedType);

                // if only one associated type.
                if (associatedTypes != null && associatedTypes.Count == 1)
                {
                    string seqNbr = associatedTypes[0].Value;
                    IList<ListItem> trustAccounts = TrustAccountUtil.GetTrustAccounts(seqNbr, paymentAssociatedType);

                    // only one trust account.
                    hasSingleTrustAccount = trustAccounts != null && trustAccounts.Count == 1;

                    if (hasSingleTrustAccount)
                    {
                        trustAccountID = trustAccounts[0].Value;
                    }
                }
            }

            return hasSingleTrustAccount;
        }

        /// <summary>
        /// Do payment.
        /// </summary>
        private void DoPayment()
        {
            CapIDModel[] capIDs = null;

            if (!IsTrustAccountDeposit)
            {
                capIDs = TempModelConvert.Trim4WSOfCapIDModels(AppSession.GetCapIDModelsFromSession());
            }

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

            //the ModuleName is not empty only if shopping cart disabled.
            if (proxyUserRoleBll.IsEnableProxyUser() && !string.IsNullOrEmpty(ModuleName))
            {
                var proxyUserBll = ObjectFactory.GetObject<IProxyUserBll>();
                CapIDModel[] noPermissionCaps = proxyUserBll.ValidateProxyUserPaymentPermission(capIDs, AppSession.User.PublicUserId);

                if (noPermissionCaps != null && noPermissionCaps.Length > 0)
                {
                    ErrorMsg = GetTextByKey("aca_payment_no_permission_message");
                    return;
                }
            }

            double maxAmount = 0d;
            PaymentMethod paymentMethod = GetPaymentMethod();

            if (IsGreaterThanMaxAmount(paymentMethod, out maxAmount))
            {
                TrustAccount.PaymentMessageType = PaymentMessageType.OutOfMaxAmount;
                ErrorMsg = string.Format(GetTextByKey("aca_excceed_payment_amount_limit_message"), I18nNumberUtil.FormatMoneyForUI(maxAmount));
                return;
            }

            try
            {
                //Check Payment Group for credit card or bank check payment
                if (paymentMethod == PaymentMethod.CreditCard || paymentMethod == PaymentMethod.Check)
                {
                    if (CapUtil.HasDifferentPaymentGroup(capIDs))
                    {
                        ErrorMsg = GetTextByKey("aca_payment_different_payment_group_msg");
                        return;
                    }
                }

                double chargedAmount = GetChargedAmount();

                //check chagedAmount, if chagedAmount<=0, show warning message,and return.
                if (chargedAmount <= 0)
                {
                    ErrorMsg = GetTextByKey("per_permitpayfee_error_invalidamount");
                    return;
                }

                bool isUsingTrustAccount = paymentMethod == PaymentMethod.TrustAccount;
                PaymentAdapterType paymentAdapterType = PaymentHelper.GetPaymentAdapterType();
                bool isUnknownPaymentAdapter = paymentAdapterType == PaymentAdapterType.Unknown;

                if (!isUsingTrustAccount && paymentAdapterType == PaymentAdapterType.Etisalat)
                {
                    RegisterEtisalatOnlinePayment();
                }
                else if ((paymentMethod == PaymentMethod.CreditCard || paymentMethod == PaymentMethod.Check) && paymentAdapterType == PaymentAdapterType.ExternalAdapter)
                {
                    CapIDModel4WS[] capIDModels = null;

                    if (!IsTrustAccountDeposit)
                    {
                        capIDModels = AppSession.GetCapIDModelsFromSession();
                    }

                    string alertMessageLabelKey = string.Empty;
                    bool isValidPerAgencyPayment = PaymentHelper.IsValidPerAgencyPayment(capIDModels, chargedAmount, out alertMessageLabelKey);

                    if (!isValidPerAgencyPayment)
                    {
                        ErrorMsg = GetTextByKey(alertMessageLabelKey);
                        return;
                    }

                    PaymentHelper.InitiatePayment(this.Page);
                }
                else
                {
                    IPayment paymentProcessor = null;

                    if (isUsingTrustAccount)
                    {
                        TrustAccountModel trustAccountModel = _trustAccountModel ?? TrustAccountUtil.GetTrustAccountModel(GetTrustAccountModel().acctID);
                        double totalAmount = TrustAccountUtil.GetTotalAmount(trustAccountModel);

                        if (totalAmount < chargedAmount)
                        {
                            TrustAccount.PaymentMessageType = PaymentMessageType.NotEnoughMoney;
                            ErrorMsg = GetTextByKey("per_permitPayFee_Tip_NotEnough").Replace("'", "\\'");
                            return;
                        }

                        TrustAccount.TrustAccountID = trustAccountModel.acctID;
                        paymentProcessor = new InternalPayment();
                    }
                    else if (isUnknownPaymentAdapter)
                    {
                        paymentProcessor = new InternalPayment();
                    }
                    else if (!string.IsNullOrEmpty(PaymentHelper.GetAdapterName()))
                    {
                        paymentProcessor = PaymentProcessorFactory.CreateProcessor();
                    }

                    if (paymentProcessor == null)
                    {
                        TrustAccount.PaymentMessageType = PaymentMessageType.Others;
                        ErrorMsg = LabelUtil.GetTextByKey("aca_shoppingcart_no_paymentprovider", ModuleName);
                        return;
                    }

                    OnlinePaymentResultModel result = paymentProcessor.InitiatePayment(this.Page);
                    ErrorMsg = BuildPaymentErrorMessage(result);

                    TrustAccount.PaymentMessageType = string.IsNullOrEmpty(ErrorMsg)
                                                          ? PaymentMessageType.None
                                                          : PaymentMessageType.Others;
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                ErrorMsg = ex.Message;
            }
        }

        /// <summary>
        /// Register Scripts.
        /// </summary>
        private void RegisterScripts()
        {
            CreditCard.RegisterScripts();
            CheckProcessing.RegisterScripts();
        }

        /// <summary>
        /// Bind the payment method list.
        /// </summary>
        private void BindPaymentMethodList()
        {
            rdlPaymentMethod.Items.Clear();

            if (!AppSession.IsAdmin && !FunctionTable.IsEnableMakePayment())
            {
                lnkSubmitPaymentPage.Enabled = false;
            }

            if (AppSession.IsAdmin || IsTrustAccountDeposit)
            {
                List<ListItem> paymentItems = ShoppingCartUtil.GetPaymentTypesByModule(ModuleName);
                paymentItems = FilterPaymentTypes(paymentItems);
                rdlPaymentMethod.Items.AddRange(paymentItems.ToArray());
                return;
            }

            CapIDModel[] capIds = TempModelConvert.Trim4WSOfCapIDModels(AppSession.GetCapIDModelsFromSession());
            bool isSamePaymentTypes;

            var paymentTypes = ShoppingCartUtil.GetAvailablePaymentTypes(capIds, ModuleName, out isSamePaymentTypes);
            paymentTypes = FilterPaymentTypes(paymentTypes);
            
            // validate that different CAP have different payment type.
            if (!isSamePaymentTypes)
            {
                MessageType messageType = paymentTypes.Count > 0 ? MessageType.Notice : MessageType.Error;
                string msg = ShoppingCartUtil.GetMessageWithDifferencePaymentTypes(capIds);
                MessageUtil.ShowMessage(Page, messageType, msg);
            }

            if (paymentTypes.Count > 0)
            {
                rdlPaymentMethod.Items.AddRange(paymentTypes.ToArray());
            }
            else
            {
                lnkSubmitPaymentPage.Enabled = false;
                divCreditCard.Visible = false;
            }
        }

        /// <summary>
        /// Filter Payment Types.
        /// </summary>
        /// <param name="paymentTypes">Payment Types</param>
        /// <returns>Payment Types List</returns>
        private List<ListItem> FilterPaymentTypes(List<ListItem> paymentTypes)
        {
            List<ListItem> list = new List<ListItem>();

            foreach (var paymentItem in paymentTypes)
            {
                string value = paymentItem.Value;
                int charIndex = value.IndexOf("||");

                if (charIndex > -1)
                {
                    value = value.Substring(0, charIndex); // remove bizDomainValue
                }

                value = value.Replace("-", string.Empty);

                if (value.Equals(((int)PaymentMethod.TrustAccount).ToString(), StringComparison.InvariantCultureIgnoreCase) &&
                    (IsTrustAccountDeposit || (!AppSession.IsAdmin && _trustAccountModel == null && !TrustAccountUtil.ShowTrustAccountOption())))
                {
                    continue;
                }

                list.Add(paymentItem);
            }

            return list;
        }
        
        /// <summary>
        /// Build Payment Error Message
        /// </summary>
        /// <param name="result">a OnlinePaymentResultModel</param>
        /// <returns>Error Message Information.</returns>
        private string BuildPaymentErrorMessage(OnlinePaymentResultModel result)
        {
            if (result == null)
            {
                return string.Empty;
            }

            StringBuilder errorMessage = new StringBuilder();

            if (result.exceptionMsg != null && result.exceptionMsg.Length > 0)
            {
                foreach (string message in result.exceptionMsg)
                {
                    string tempMsg = GetTextByKey(message);
                    if (string.IsNullOrEmpty(tempMsg))
                    {
                        tempMsg = message;
                    }

                    errorMessage.Append(tempMsg);
                    errorMessage.Append("\n");
                }
            }

            return errorMessage.ToString();
        }

        /// <summary>
        /// Change Payment
        /// </summary>
        private void ChangePaymentMethod()
        {
            InitAllValue();

            if (rdlPaymentMethod.Items.Count == 0)
            {
                return;
            }

            PaymentMethod pm = GetPaymentMethod();
            PaymentAdapterType paymentAdapterType = PaymentHelper.GetPaymentAdapterType();

            switch (pm)
            {
                case PaymentMethod.CreditCard:

                    if (paymentAdapterType == PaymentAdapterType.CoBrandPlus || paymentAdapterType == PaymentAdapterType.Etisalat || paymentAdapterType == PaymentAdapterType.ExternalAdapter)
                    {
                        divCreditCard.Visible = false;
                    }
                    else if (paymentAdapterType == PaymentAdapterType.Govolution || paymentAdapterType == PaymentAdapterType.FirstData)
                    {
                        divCreditCard.Visible = false;
                        divCheckProcessing.Visible = false;
                    }
                    else
                    {
                        divCreditCard.Visible = true;
                    }

                    break;
                case PaymentMethod.TrustAccount:
                    PaymentSelectedOption defaultPaymentMethod = SelectedPaymentMethod();

                    //Display when has "trust account" payment method.
                    divTrustAccount.Visible = defaultPaymentMethod != PaymentSelectedOption.None;
                    divConveninenceFee.Visible = false;
                    break;
                case PaymentMethod.Check:
                    if (paymentAdapterType == PaymentAdapterType.CoBrandPlus || paymentAdapterType == PaymentAdapterType.ExternalAdapter)
                    {
                        divCreditCard.Visible = false;
                    }
                    else
                    {
                        divCheckProcessing.Visible = true;
                    }

                    break;
            }

            ListItemCollection payMethodItems = rdlPaymentMethod.Items;
            ListItem selectedItem = rdlPaymentMethod.SelectedItem;
            if (payMethodItems != null && payMethodItems.Count > 0 && selectedItem != null && !string.IsNullOrEmpty(selectedItem.Text))
            {
                for (int i = 0; i < payMethodItems.Count; i++)
                {
                    if (payMethodItems[i].Text == selectedItem.Text)
                    {
                        updatePanel.FocusElement(string.Concat(rdlPaymentMethod.ClientID, ACAConstant.SPLIT_CHAR5, i));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get convenience fee based on charged amount, if the payment adapter has configured convenience fee formulary.
        /// </summary>
        /// <returns>double value of convenience fee</returns>
        private double GetConvenienceFee()
        {
            double convFee = 0;

            if (!AppSession.IsAdmin && ExistConvenienceFeeFormula)
            {
                double chargeAmount = GetChargedAmount();
                CreditCardModel4WS creditCardModel = null;
                string merchantAccountID = null;
                string paymentMethod = GetPaymentMethod() == PaymentMethod.Check ? ACAConstant.PAY_METHOD_CHECK : ACAConstant.PAY_METHOD_CREDIT_CARD;

                // get merchant account id.
                List<RefMerchantAccountModel> merchantAccounts = PaymentHelper.GetMerchantAccount();

                if (merchantAccounts != null && merchantAccounts.Count > 0)
                {
                    RefMerchantAccountModel merchantAccount = merchantAccounts[0];

                    if (merchantAccount != null && merchantAccount.merchantAccountPKModel != null)
                    {
                        merchantAccountID = merchantAccount.merchantAccountPKModel.resId.ToString();
                    }
                }

                IOnlinePaymenBll onlinePaymenBll = (IOnlinePaymenBll)ObjectFactory.GetObject(typeof(IOnlinePaymenBll));

                if (StandardChoiceUtil.IsEnableShoppingCart() && !IsTrustAccountDeposit)
                {
                    string adapterName = PaymentHelper.GetAdapterName();
                    CapIDModel4WS[] capIds = AppSession.GetCapIDModelsFromSession();

                    creditCardModel = onlinePaymenBll.CalculateConvenienceFeeForShoppingCart(ConfigManager.AgencyCode, adapterName, TempModelConvert.Trim4WSOfCapIDModels(capIds), paymentMethod, merchantAccountID);
                }
                else if (chargeAmount > 0)
                {
                    string agencyCode = ConfigManager.AgencyCode;

                    if (!IsTrustAccountDeposit)
                    {
                        CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                        agencyCode = capModel.capID.serviceProviderCode;
                    }

                    creditCardModel = onlinePaymenBll.CalculateConvenienceFee(agencyCode, chargeAmount, paymentMethod, merchantAccountID);
                }

                if (creditCardModel != null && creditCardModel.convFee != null)
                {
                    convFee = I18nNumberUtil.ParseMoneyFromWebService(creditCardModel.convFee);
                }
            }

            return convFee;
        }

        /// <summary>
        /// Get Total Fee for Shopping Cart
        /// </summary>
        /// <returns>double for fee.</returns>
        private double GetTotalFeeForShoppingCart()
        {
            IShoppingCartBll shoppingCartBll = (IShoppingCartBll)ObjectFactory.GetObject(typeof(IShoppingCartBll));
            double totalFee = shoppingCartBll.GetTotalFeeForShoppingCart(ConfigManager.AgencyCode, AppSession.GetCapIDModelsFromSession(), AppSession.User.PublicUserId);

            return totalFee;
        }

        /// <summary>
        /// Initializes General Payment UI.
        /// Included Payment Adapter Type: PayPal/PayPal43, VirtualMerchant, Any Customized E-Payment Adapters
        /// </summary>
        private void InitGeneralPaymentUI()
        {
            Logger.DebugFormat("Begin else() \n");

            CreditCard.InitAPIUI();
            CheckProcessing.InitAPIUI();
        }

        /// <summary>
        /// Clear form data when switch payment method.
        /// </summary>
        private void InitAllValue()
        {
            if (IsPostBack)
            {
                MessageUtil.HideMessageByControl(Page);
            }

            divCreditCard.Visible = false;
            divTrustAccount.Visible = false;
            divCheckProcessing.Visible = false;

            //reset Credit Card.
            CreditCard.InitAllValue();

            //reset Trust Account
            TrustAccount.InitTrustAccount();

            //reset Check
            CheckProcessing.InitAllValue();

            //init Convenience Fee UI
            if (ExistConvenienceFeeFormula)
            {
                divConveninenceFee.Visible = true;
            }
        }

        /// <summary>
        /// Initializes Credit card and Electronic Check for STP
        /// first_name    60 letters, numbers, and ,-.'
        /// middle_name 60 letters, numbers, and ,-.'
        /// last_name 60    letters, numbers, and ,-.'
        /// address 60 letters, numbers, and ,-.'#
        /// city_name 60    letters, numbers, and ,-.'
        /// state_code 60 letters, numbers, and ,-.'
        /// postal_code 32 letters and numbers
        /// telephone 32    letters and numbers
        /// </summary>
        private void InitCreditCardAndCheckForSTP()
        {
            CreditCard.InitCreditCardForSTP();
            CheckProcessing.InitCheckForSTP();
        }

        /// <summary>
        /// Initial Official STP API Payment Gateway UI
        /// </summary>
        private void InitOPSTPPaymentUI()
        {
            Logger.DebugFormat("Begin IsUseAPIPaymentWebservice() \n");

            InitCreditCardAndCheckForSTP();
        }

        /// <summary>
        /// initial UI method.
        /// </summary>
        private void InitUI()
        {
            PaymentAdapterType paymentAdapterType = PaymentHelper.GetPaymentAdapterType();

            if (IsTrustAccountDeposit)
            {
                lblTrustAccountValue.Text = TrustAccountID;
                tbTrustAccountAmount.Visible = true;
                divPermitAmount.Visible = false;
            }
            else
            {
                tbTrustAccountAmount.Visible = false;
                divPermitAmount.Visible = true;
                double totalFee = this.GetTotalFeeForShoppingCart();
                hdnChargedAmount.Value = totalFee.ToString(CultureInfo.InvariantCulture);
                lblChargedAmount.Text = I18nNumberUtil.FormatMoneyForUI(totalFee);
            }

            if (paymentAdapterType == PaymentAdapterType.APIPayment)
            {
                InitOPSTPPaymentUI();
            }
            else if (paymentAdapterType == PaymentAdapterType.ExternalAdapter
                || paymentAdapterType == PaymentAdapterType.CoBrandPlus
                || paymentAdapterType == PaymentAdapterType.Etisalat
                || paymentAdapterType == PaymentAdapterType.Govolution
                || paymentAdapterType == PaymentAdapterType.FirstData)
            {
                InitRedirectedPaymentUI();
            }
            else
            {
                InitGeneralPaymentUI();
            }

            InitConvenienceFeeUI();

            SelectedDefaultPaymentMethod();

            if (AppSession.IsAdmin)
            {
                InitAdminUI();
            }
        }

        /// <summary>
        /// Initializes Admin UI.
        /// </summary>
        private void InitAdminUI()
        {
            divCreditCard.Visible = true;
            divTrustAccount.Visible = true;
            divCheckProcessing.Visible = true;
            rdlPaymentMethod.AutoPostBack = false;
            rdlPaymentMethod.Attributes.Add("onclick", "ChangeType()");
        }

        /// <summary>
        /// Is multi CAP
        /// </summary>
        /// <returns>true or false.</returns>
        private bool IsMultiCAP()
        {
            return !Pay4ExistingCap && StandardChoiceUtil.IsSuperAgency();
        }

        /// <summary>
        /// Redirect the page after payment
        /// </summary>
        /// <param name="receiptNbr">payment No</param>
        private void RedirectAfterEtisalatPayment(string receiptNbr)
        {
            // get the step number for parameter.
            string preStepNumber = Request.QueryString["stepNumber"];
            bool isNumber = ValidationUtil.IsNumber(preStepNumber);
            int stepNumber = 0;

            if (isNumber)
            {
                stepNumber = int.Parse(preStepNumber) + 1;
            }

            //whether existing cap to pay fee
            string isPay4ExistingCap = Pay4ExistingCap ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            string isRenewalFlag = ACAConstant.COMMON_Y == Request["isRenewal"] ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            Response.Redirect("../Cap/CapCompletion.aspx?Module=" + ModuleName + "&receiptNbr=" + receiptNbr + "&stepNumber=" + stepNumber + "&isPay4ExistingCap=" + isPay4ExistingCap + "&isRenewal=" + isRenewalFlag);
        }

        /// <summary>
        /// Register ETISALAT online payment.
        /// </summary>
        private void RegisterEtisalatOnlinePayment()
        {
            EtisalatRegistrationInput input = new EtisalatRegistrationInput();
            input.ModuleName = ModuleName;
            input.IsMultiCAP = IsMultiCAP();
            input.IsPay4ExistingCap = Pay4ExistingCap;
            EtisalatRegistrationOutput output = EtisalatAdapter.RegisterPayment(input);

            if (output.Status == EtisalatStatus.Registered || output.Status == EtisalatStatus.Paid)
            {
                Response.Redirect(output.RedirectionURL);
            }
            else if (output.Status == EtisalatStatus.RegistrationSucceeded)
            {
                RedirectAfterEtisalatPayment(output.ReceiptNbr);
            }
            else
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, output == null ? string.Empty : output.ErrorMessage);
            }
        }

        /// <summary>
        /// Set Default option for Search Type.
        /// </summary>
        private void SelectedDefaultPaymentMethod()
        {
            if (rdlPaymentMethod.Items.Count > 0)
            {
                PaymentSelectedOption defaultPaymentMethod = SelectedPaymentMethod();

                switch (defaultPaymentMethod)
                {
                    case PaymentSelectedOption.None:
                        rdlPaymentMethod.SelectedIndex = -1;
                        break;
                    case PaymentSelectedOption.Default:
                        rdlPaymentMethod.SelectedIndex = 0;
                        break;
                    case PaymentSelectedOption.TrustAccount:
                        rdlPaymentMethod.SelectedValue = ((int)PaymentMethod.TrustAccount).ToString();
                        break;
                }

                ChangePaymentMethod();
            }
            else
            {
                lnkSubmitPaymentPage.Enabled = false;
            }
        }

        /// <summary>
        /// Gets the payment selected option
        /// </summary>
        /// <returns>Selected Option</returns>
        private PaymentSelectedOption SelectedPaymentMethod()
        {
           PaymentSelectedOption selectedOption = PaymentSelectedOption.Default;

            // for agent user
            if (!AppSession.IsAdmin && (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent) && _trustAccountModel != null)
            {
                // if has configed "Trust Account" payment option, selected it. otherwise, not selected.
                if (rdlPaymentMethod.Items.FindByValue(((int)PaymentMethod.TrustAccount).ToString()) != null)
                {
                     selectedOption = PaymentSelectedOption.TrustAccount;
                }
                else
                {
                    selectedOption = PaymentSelectedOption.None;
                }
            }

            return selectedOption;
        }

        /// <summary>
        /// Initializes Redirect Online Payment UI
        /// Included Payment Adapter Type: <c>CoBrandPlus, Etisalat, Govolution, FirstData, ExternalAdapter</c>.
        /// </summary>
        private void InitRedirectedPaymentUI()
        {
            Logger.DebugFormat("Begin IsUseExternalAdapterPayment \n");
            divCreditCard.Visible = false;
            divCheckProcessing.Visible = false;
        }

        /// <summary>
        /// Initialize the convenience fee UI
        /// </summary>
        private void InitConvenienceFeeUI()
        {
            if (ExistConvenienceFeeFormula)
            {
                double convFee = GetConvenienceFee();
                double chargedAmount = GetChargedAmount();
                double totalAmount = convFee + chargedAmount;

                lblConveninenceFee.Text = I18nNumberUtil.FormatMoneyForUI(convFee);
                lblTotalAmount.Text = I18nNumberUtil.FormatMoneyForUI(totalAmount);

                divConveninenceFee.Visible = true;
            }
            else
            {
                divConveninenceFee.Visible = false;
            }
        }

        /// <summary>
        /// If the total fee greater than the max amount,that return true and get the error message.
        /// </summary>
        /// <param name="paymentMethod">the payment method </param>
        /// <param name="maxAmount">max amount limited</param>
        /// <returns>return true or false</returns>
        private bool IsGreaterThanMaxAmount(PaymentMethod paymentMethod, out double maxAmount)
        {
            string key = ModuleName + ACAConstant.SPLIT_CHAR5 + paymentMethod.ToString();
            string moneyString = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_ONLINE_PAYMENT_MAX_AMOUNT, key);

            if (string.IsNullOrEmpty(moneyString))
            {
                moneyString = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_ONLINE_PAYMENT_MAX_AMOUNT, paymentMethod.ToString());
            }

            if (!I18nNumberUtil.TryParseMoneyFromWebService(moneyString, out maxAmount))
            {
                return false;
            }

            double totalFee = I18nNumberUtil.ParseMoneyFromWebService(hdnChargedAmount.Value);

            bool result = totalFee > Math.Round(maxAmount, 2);

            return result;
        }

        /// <summary>
        /// only one record and not trust account deposit, redirect to pay fee directly.
        /// </summary>
        /// <param name="paymentMethod">the payment method.</param>
        /// <returns>IsSingleTrustAccount Flag</returns>
        private bool IsDirectToPayment(PaymentMethod paymentMethod)
        {
            // for agent user, default to use trust account payment method.
            bool isSingle = !AppSession.IsAdmin && rdlPaymentMethod.Items.Count == 1;

            // when trust account deposit, or has multiply record, not skip.
            if (IsTrustAccountDeposit || !isSingle)
            {
                return false;
            }
            
            if (paymentMethod == PaymentMethod.CreditCard || paymentMethod == PaymentMethod.Check)
            {
                // use CreditCard/E-Check and judge if it redirects to 3rd payment provider.
                return PaymentHelper.IsRedirectToPaymentProvider();
            }

            if (paymentMethod == PaymentMethod.TrustAccount)
            {
                // use trust account
                string trustAccountID = string.Empty;
                bool isSingleTrustAccount = IsSingleTrustAccount(ref trustAccountID);

                // payment information has only one.
                if (isSingleTrustAccount)
                {
                    PaymentSelectedOption defaultPaymentMethod = SelectedPaymentMethod();

                    if (defaultPaymentMethod != PaymentSelectedOption.None)
                    {
                        // it need to default selected when has trust account payment method.
                        TrustAccount.NeedDefaultSelected = ACAConstant.COMMON_Y;
                        PaymentMethodDropdown_IndexChanged(rdlPaymentMethod, null);
                    }

                    TrustAccount.TrustAccountID = trustAccountID;
                }

                return isSingleTrustAccount;
            }

            return false;
        }

        #endregion function
    }
}