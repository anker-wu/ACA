#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BizDomainBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ShoppingCart.aspx.cs 278785 2014-09-13 09:51:14Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.ProxyUser;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.Web.ShoppingCart
{
    /// <summary>
    /// Page to handler shopping cart
    /// </summary>
    public partial class ShoppingCart : BasePage
    {
        #region Fields

        /// <summary>
        /// Whether the cap has negative fee
        /// </summary>
        private bool _isOneCapHasNagativeFee = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the unavailable examine fee item.
        /// </summary>
        public F4FeeItemModel4WS[] InavailableExamFeeItems
        {
            get
            {
                return ViewState["InavailableExamFee"] as F4FeeItemModel4WS[];
            }

            set
            {
                ViewState["InavailableExamFee"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!StandardChoiceUtil.IsEnableShoppingCart() && !AppSession.IsAdmin)
            {
                RedirectToHomePage();
            }

            ShoppingCartListSaved.ShoppingCartUpdateCommand += new EventHandler(ShoppingCartUpdateCommand);
            ShoppingCartListSelected.ShoppingCartUpdateCommand += new EventHandler(ShoppingCartUpdateCommand);
            ShoppingCartListSelected.ShoppingCartRemoveCommand += new EventHandler(ShoppingCartRemoveCommand);
            ShoppingCartListSaved.ShoppingCartRemoveCommand += new EventHandler(ShoppingCartRemoveCommand);

            ShoppingCartUtil.SetCartItemNumber();

            if (!AppSession.IsAdmin)
            {
                BreadCrumpShoppingCart.PageFlow = BreadCrumpUtil.GetShoppingCartFlowConfig();
            }
            else
            {
                divNoRecord.Visible = true;
                divNoAddressSavedSetting.Visible = true;
                divNoAddressSelectedSetting.Visible = true;
            }

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                ShowFeeChangeMessage();
                Hashtable htAllCartItems = GetShoppingCart();
                GetInavailableExamFeeList(htAllCartItems);
                BindShoppingCart(htAllCartItems);
                BindTotalFee(htAllCartItems);
                
                //the defaul module session used for payment log.
                Session[SessionConstant.SESSION_DEFAULT_MODULE] = null;
            }

            Common.Control.ControlBuildHelper.SetInstructionValue(this.lblTitleSelected_sub_label, GetTextByKey("per_shoppingcart_label_paynow|sub"));
        }

        /// <summary>
        /// Raises the check out event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void CheckOutButton_Click(object sender, EventArgs e)
        {
            Hashtable htAllCartItems = GetShoppingCart();
            ShoppingCartItemModel4WS[] shoppingCartItems = GetShoppingCartItems(htAllCartItems);

            // Check the prerequisites
            if (!ValidatePrerequisities(shoppingCartItems))
            {
                return;
            }

            CapIDModel4WS[] capIDs = GetPositiveCapIDs(shoppingCartItems);

            //set pay now cap id to session.
            if (_isOneCapHasNagativeFee)
            {
                MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("per_permitFee_message_havenegativefee"));
                return;
            }

            if (!HasPermissionToAvailableCaps(shoppingCartItems))
            {
                return;
            }

            if (capIDs != null && capIDs.Length > 0)
            {
                //Check all the related records whether be added to the pay now list.
                IList<CapIDModel> notInCartRelatedRecords;
                var partialCapIDs = from cartItem in shoppingCartItems
                                    where CapUtil.IsPartialCap(cartItem.capClass)
                                    select TempModelConvert.Trim4WSOfCapIDModel(cartItem.capID);

                if (partialCapIDs != null && partialCapIDs.Count() > 0 &&
                    !CheckRelatedRecords(partialCapIDs.ToArray(), out notInCartRelatedRecords))
                {
                    string msg = GetTextByKey("aca_shoppingcart_msg_relatedrecordsnotadded");

                    foreach (CapIDModel capId in notInCartRelatedRecords)
                    {
                        msg += "<br />" + capId.customID;
                    }

                    MessageUtil.ShowMessage(Page, MessageType.Notice, msg);
                    return;
                }

                //Check all the records' status.
                IList<CapIDModel4WS> notCompletedRecords;

                if (!CheckRecordsStatus(shoppingCartItems, out notCompletedRecords))
                {
                    string msg = GetTextByKey("aca_shoppingcart_msg_somerecordsnotcompleted");

                    foreach (CapIDModel4WS capId in notCompletedRecords)
                    {
                        msg += "<br />" + capId.customID;
                    }

                    MessageUtil.ShowMessage(Page, MessageType.Notice, msg);
                    return;
                }

                if (!CheckLicense4Caps(htAllCartItems))
                {
                    return;
                }

                double totalFeeOfShoppingCart = ShoppingCartUtil.GetTotalFeeForShoppingCart(capIDs);
                string alertMessageLabelKey = string.Empty;
                bool isValidPerAgencyPayment = PaymentHelper.IsValidPerAgencyPayment(capIDs, totalFeeOfShoppingCart, out alertMessageLabelKey);

                if (!isValidPerAgencyPayment)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey(alertMessageLabelKey));
                    return;
                }

                if (totalFeeOfShoppingCart > 0)
                {
                    if (!ValidateMerchantAccount(shoppingCartItems, capIDs))
                    {
                        return;
                    }

                    if (!ValidateEMSECheckout(shoppingCartItems, totalFeeOfShoppingCart))
                    {
                        return;
                    }

                    AppSession.SetCapIDModelsToSession(capIDs);

                    string url = "../Cap/CapPayment.aspx?stepNumber=3";
                    Response.Redirect(url);
                }
                else
                {
                    AppSession.SetOnlinePaymentResultModelToSession(ShoppingCartUtil.CreateZeroFeeCAPs(capIDs));

                    string url = "../Cap/CapCompletions.aspx?stepNumber=4";
                    Response.Redirect(url);
                }
            }
            else
            {
                ShowFeeChangeMessage();
                BindShoppingCart(htAllCartItems);
                BindTotalFee(htAllCartItems);
                btnCheckOut.Enabled = false;
            }
        }

        /// <summary>
        /// no url configuration in aca admin, go to welcome page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void CreateAnotherApplicationButton_Click(object sender, EventArgs e)
        {
            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "redirectToNewUIHome", "<script>window.parent.redirectToNewUIHome();</script>");
                return;
            }

            string tabName = string.Empty;

            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string url = bizBll.GetConfiguredUrlFromXPolicy();

            if (string.IsNullOrEmpty(url))
            {
                url = ACAConstant.URL_WELCOME_PAGE;
            }
            else
            {
                tabName = GetTabName(url);
            }

            url = FileUtil.AppendApplicationRoot(url + tabName);
            Response.Redirect(url);
        }

        /// <summary>
        /// Get shopping cart all cap's in-available exam fee item
        /// </summary>
        /// <param name="htAllCartItems">shopping cart items</param>
        private void GetInavailableExamFeeList(Hashtable htAllCartItems)
        {
            if (htAllCartItems == null || htAllCartItems.Count == 0)
            {
                return;
            }

            var shoppingCartItems = new List<ShoppingCartItemModel4WS>();

            foreach (ShoppingCartModel4WS item in htAllCartItems.Values)
            {
                if (item != null && item.shoppingCartItems != null && item.shoppingCartItems.Length > 0)
                {
                    shoppingCartItems.AddRange(item.shoppingCartItems);
                }
            }

            if (shoppingCartItems.Count > 0)
            {
                var capIds = GetPositiveCapIDs(shoppingCartItems);

                if (capIds == null && capIds.Length == 0)
                {
                    return;
                }

                var examBll = ObjectFactory.GetObject<IExaminationBll>();
                var feeBll = ObjectFactory.GetObject<IFeeBll>();
                ExaminationModel[] inavailableExams = examBll.GetInavailableExamListByCapIds(capIds);

                if (inavailableExams != null && inavailableExams.Length > 0)
                {
                    F4FeeItemModel4WS[] inavailableExamFees = feeBll.GetNoPaidExamFeeItemsByExams(inavailableExams);

                    if (inavailableExamFees != null && inavailableExamFees.Length > 0)
                    {
                        InavailableExamFeeItems = inavailableExamFees;
                    }
                }
            }
        }

        /// <summary>
        /// Update CapID list to Session
        /// </summary>
        /// <param name="htAllCartItems">Shopping cart item hashtable</param>
        private void UpdateCapIDSession(Hashtable htAllCartItems)
        {
            if (AppSession.GetCapIDModelsFromSession() != null)
            {
                ShoppingCartItemModel4WS[] shoppingCartItems = GetShoppingCartItems(htAllCartItems);
                CapIDModel4WS[] capIDs = GetPositiveCapIDs(shoppingCartItems);
                AppSession.SetCapIDModelsToSession(capIDs);
            }
        }

        /// <summary>
        /// Validate the EMSE for shopping cart check out before event.
        /// </summary>
        /// <param name="payNowCartItems">The pay now shopping cart items.</param>
        /// <param name="payAmount">The pay amount</param>
        /// <returns>Return true or false to indicate whether validation or not.</returns>
        private bool ValidateEMSECheckout(ShoppingCartItemModel4WS[] payNowCartItems, double payAmount)
        {
            CapTypeModel[] capTypeModels = GetDistinctCapType4PayNowItems(payNowCartItems);

            if (capTypeModels != null && capTypeModels.Length > 0)
            {
                EMSEResultModel4WS emseResult = EmseUtil.RunEMSEScript4ShoppingCart(ConfigManager.AgencyCode, ACAConstant.EMSE_SHOPPINGCART_CHECKOUT_BEFORE, capTypeModels, payAmount);

                if (emseResult != null && EmseUtil.RETURN_CODE_FOR_DISPLAY_MESSAGE.Equals(emseResult.errorCode))
                {
                    MessageUtil.ShowMessageByControl(Page, MessageType.Error, emseResult.errorMessage);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get the distinct cap type form pay now cart items.
        /// </summary>
        /// <param name="payNowCartItems">The pay now cart items.</param>
        /// <returns>Return the distinct cap type list.</returns>
        private CapTypeModel[] GetDistinctCapType4PayNowItems(ShoppingCartItemModel4WS[] payNowCartItems)
        {
            if (payNowCartItems == null || payNowCartItems.Length == 0)
            {
                return null;
            }

            Dictionary<string, CapTypeModel> dictCapType = new Dictionary<string, CapTypeModel>();

            foreach (ShoppingCartItemModel4WS item in payNowCartItems)
            {
                if (item.capID != null && item.capType != null)
                {
                    // assign the agency code to cap type, because the cap type not associated it from shopping cart.
                    item.capType.serviceProviderCode = item.capID.serviceProviderCode;
                    string capTypeKey = string.Format("{0}/{1}", item.capType.serviceProviderCode, CAPHelper.GetCapTypeValue(item.capType));
                    
                    if (!dictCapType.Keys.Contains(capTypeKey))
                    {
                        dictCapType.Add(capTypeKey, item.capType);
                    }
                }
            }

            return dictCapType.Values.ToArray();
        }

        /// <summary>
        /// Validate the prerequisites.
        /// </summary>
        /// <param name="shoppingCartItems">The shopping cart item.</param>
        /// <returns>Return true or false to indicate whether validation or not.</returns>
        private bool ValidatePrerequisities(ShoppingCartItemModel4WS[] shoppingCartItems)
        {
            Hashtable htPrerequisites = GetPrerequisites(shoppingCartItems);

            if (htPrerequisites != null && htPrerequisites.Count > 0)
            {
                StringBuilder msg = new StringBuilder();

                foreach (string currentCapType in htPrerequisites.Keys)
                {
                    string pattern = GetTextByKey("aca_label_shoppingcart_prerequisitesstart").Replace("\\n", "<BR/>");
                    string prerequisitesCapType = string.Join(", ", (IList<string>)htPrerequisites[currentCapType]);
                    msg.AppendFormat(pattern, prerequisitesCapType, currentCapType);
                }

                msg.Append(GetTextByKey("aca_label_shoppingcart_prerequisitesend"));
                MessageUtil.ShowMessage(Page, MessageType.Notice, msg.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate the merchant account, ensure that only one merchant account can check out once.
        /// </summary>
        /// <param name="payNowCartItems">The shopping cart item that want to pay now.</param>
        /// <param name="capIds">The CAP ID list.</param>
        /// <returns>Return true or false to indicate whether validation or not.</returns>
        private bool ValidateMerchantAccount(ShoppingCartItemModel4WS[] payNowCartItems, CapIDModel4WS[] capIds)
        {
            // authorized agent only use trust account to pay fee, so need not validate the merchant account.
            if (payNowCartItems == null || payNowCartItems.Length <= 1 || AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent)
            {
                return true;
            }

            // if only config the trust account payment method, need not validate
            if (TrustAccountUtil.ShowTrustAccountOption())
            {
                CapIDModel[] tempCapIds = TempModelConvert.Trim4WSOfCapIDModels(capIds);
                bool isSamePaymentTypes;

                var paymentTypes = ShoppingCartUtil.GetAvailablePaymentTypes(tempCapIds, ModuleName, out isSamePaymentTypes);

                // if only exists trust account options need NOT validate merchant account.
                string trustAccountOptionValue = ((int)PaymentMethod.TrustAccount).ToString();

                if (paymentTypes != null && paymentTypes.Count == 1 && paymentTypes[0].Value.IndexOf(trustAccountOptionValue) != -1)
                {
                    return true;
                }
            }

            PaymentAdapterType paymentAdapterType = PaymentHelper.GetPaymentAdapterType();

            // validate merchant account function only actived in External Redirect Mode
            if (paymentAdapterType != PaymentAdapterType.ExternalAdapter)
            {
                return true;
            }
            
            // key: merchant account, value: cap id list. 
            Dictionary<string, List<CapIDModel4WS>> dictMerchantAccount = new Dictionary<string, List<CapIDModel4WS>>();

            // key: the string "agencyCode/group/type/sub-type/category" indicates a record type, value: merchant account
            Dictionary<string, string> dictCapType2MerchantAccount = new Dictionary<string, string>();
            
            foreach (ShoppingCartItemModel4WS item in payNowCartItems)
            {
                if (item.capID != null && item.capType != null)
                {
                    // assign the agency code to cap type, because the cap type not associated it from shopping cart.
                    item.capType.serviceProviderCode = item.capID.serviceProviderCode;
                    string capTypeKey = string.Format("{0}/{1}", item.capType.serviceProviderCode, CAPHelper.GetCapTypeValue(item.capType));

                    // 1. Getting merchant account, for performance, use local cache that has already obtained.
                    string account = string.Empty;

                    if (dictCapType2MerchantAccount.Keys.Contains(capTypeKey))
                    {
                        account = dictCapType2MerchantAccount[capTypeKey];
                    }
                    else
                    {
                        account = GetEncryptMerchantAccount(item.capType);
                        dictCapType2MerchantAccount.Add(capTypeKey, account);
                    }

                    // 2. Assemble an mapping that merchant account with an record list.
                    if (dictMerchantAccount.Keys.Contains(account))
                    {
                        List<CapIDModel4WS> capIDModels = dictMerchantAccount[account];
                        capIDModels.Add(item.capID);

                        dictMerchantAccount[account] = capIDModels;
                    }
                    else
                    {
                        dictMerchantAccount.Add(account, new List<CapIDModel4WS> { item.capID });
                    }
                }
            }

            // if exists multiply merachant accounts, show message.
            if (dictMerchantAccount.Count > 1)
            {
                string labelDescItem = GetTextByKey("aca_shoppingcart_msg_singlemerchantaccount_item");

                StringBuilder sbMessage = new StringBuilder();
                sbMessage.Append(GetTextByKey("aca_shoppingcart_msg_singlemerchantaccount"));
                sbMessage.Append("<br/>");

                int i = 0;
                foreach (string account in dictMerchantAccount.Keys)
                {
                    sbMessage.AppendFormat(labelDescItem, ++i);

                    List<CapIDModel4WS> capIDModels = dictMerchantAccount[account];
                    foreach (CapIDModel4WS capIDModel in capIDModels)
                    {
                        sbMessage.AppendFormat("{0}, ", capIDModel.customID);
                    }

                    sbMessage.Remove(sbMessage.Length - 2, 2);
                    sbMessage.Append("<br/>");
                }

                MessageUtil.ShowMessage(Page, MessageType.Notice, sbMessage.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the encrypt merchant account by cap type.
        /// </summary>
        /// <param name="capTypeModel">The cap type.</param>
        /// <returns>Return the merchant account that has encrypt.</returns>
        private string GetEncryptMerchantAccount(CapTypeModel capTypeModel)
        {
            if (string.IsNullOrEmpty(capTypeModel.serviceProviderCode))
            {
                throw new ACAException("Invalid cap type, the agency code is null.");
            }

            IOnlinePaymenBll onlinePaymenBll = ObjectFactory.GetObject<IOnlinePaymenBll>();
            RefMerchantAccountModel accountModel = onlinePaymenBll.SearchMerchantAccount(capTypeModel, false);

            if (accountModel != null)
            {
                /* use the account name and the principal/convenience account info to indicate an unique account.
                 * 1. Single Agency: use account name can specificate the unique account.
                 * 2. Multiple Agencies: if account name is the same, but the principal/convenience is different, it also not the unique. 
                 * 3. Account Name is case-insensitive.
                 */
                return string.Format(
                    "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}",
                    ACAConstant.SPLIT_CHAR,
                    accountModel.accountName.ToUpper(),
                    accountModel.principalAccNumber,
                    accountModel.principalAccSettleNumber,
                    accountModel.principalAccPassword,
                    accountModel.convenienceAccNumber,
                    accountModel.convenienceAccSettleNumber,
                    accountModel.convenienceAccPassword);
            }

            return string.Empty;
        }

        /// <summary>
        /// Check the prerequisites for shopping cart items
        /// </summary>
        /// <param name="payNowCartItems">pay now shopping cart items</param>
        /// <returns>return the missing prerequisites</returns>
        private Hashtable GetPrerequisites(ShoppingCartItemModel4WS[] payNowCartItems)
        {
            if (payNowCartItems == null)
            {
                return null;
            }

            Hashtable htPrerequisitesMapping = new Hashtable();
            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();

            foreach (ShoppingCartItemModel4WS payNowCartItem in payNowCartItems)
            {
                if (payNowCartItem == null)
                {
                    continue;
                }

                string currentCapType = CAPHelper.GetAliasOrCapTypeLabel(payNowCartItem.capType);

                if (htPrerequisitesMapping.ContainsKey(currentCapType))
                {
                    continue;
                }

                CapIDModel currentCapIDModel = TempModelConvert.Trim4WSOfCapIDModel(payNowCartItem.capID);
                List<CapIDModel> shopCartCapIDModels = new List<CapIDModel>();

                // get the shopcart items that except the current item 
                if (payNowCartItems.Length > 1)
                {
                    IEnumerable<ShoppingCartItemModel4WS> payNowExpceptCurrent = payNowCartItems.Except(new[] { payNowCartItem });

                    foreach (ShoppingCartItemModel4WS item in payNowExpceptCurrent)
                    {
                        shopCartCapIDModels.Add(TempModelConvert.Trim4WSOfCapIDModel(item.capID));
                    }
                }

                // get the prerequisites cap type
                CapTypeModel[] capTypeModels = capTypeBll.GetPrerequisitesCapTypeList(currentCapIDModel.serviceProviderCode, shopCartCapIDModels.ToArray(), currentCapIDModel);

                if (capTypeModels != null && capTypeModels.Length > 0)
                {
                    List<string> capTypesPrerequisites = new List<string>();

                    foreach (CapTypeModel item in capTypeModels)
                    {
                        capTypesPrerequisites.Add(CAPHelper.GetAliasOrCapTypeLabel(item));
                    }

                    htPrerequisitesMapping.Add(currentCapType, capTypesPrerequisites);
                }
            }

            return htPrerequisitesMapping;
        }

        /// <summary>
        /// Show no delegate permission caps.
        /// </summary>
        /// <param name="shoppingCartItems">the pay now shopping cart items.</param>
        /// <returns>true or false.</returns>
        private bool HasPermissionToAvailableCaps(ShoppingCartItemModel4WS[] shoppingCartItems)
        {
            bool hasPermission = true;

            if (!StandardChoiceUtil.IsEnableProxyUser() || shoppingCartItems == null || shoppingCartItems.Length == 0
               || AppSession.User.UserModel4WS.initialUsers == null || AppSession.User.UserModel4WS.initialUsers.Length == 0)
            {
                return hasPermission;
            }

            CapIDModel[] capIDs = TempModelConvert.Trim4WSOfCapIDModels(shoppingCartItems.Select(p => p.capID).ToArray());
            var proxyUserBll = ObjectFactory.GetObject<IProxyUserBll>();
            CapIDModel[] noPermissionCapIDs = proxyUserBll.ValidateProxyUserPaymentPermission(capIDs, AppSession.User.PublicUserId);

            if (noPermissionCapIDs == null || noPermissionCapIDs.Length == 0)
            {
                return true;
            }

            ShoppingCartItemModel4WS[] noPermissionCartIems = shoppingCartItems.Where(s => TempModelConvert.Add4WSForCapIDModels(noPermissionCapIDs).Contains(s.capID)).ToArray();

            StringBuilder noticeMessage = new StringBuilder();

            foreach (ShoppingCartItemModel4WS item in noPermissionCartIems)
            {
                hasPermission = false;
                noticeMessage.Append("<li>");

                if (string.Equals(ACAConstant.CAP_RENEWAL, item.processType, StringComparison.InvariantCulture))
                {
                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                    CapIDModel parentCapIdModel = capBll.GetParentCapIDByChildCapID(TempModelConvert.Trim4WSOfCapIDModel(item.capID), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);

                    if (parentCapIdModel != null && !string.IsNullOrEmpty(parentCapIdModel.customID))
                    {
                        noticeMessage.Append(parentCapIdModel.customID);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.capID.customID))
                    {
                        noticeMessage.Append(item.capID.customID);
                    }
                }

                noticeMessage.Append("</li>");
            }

            if (!hasPermission)
            {
                MessageUtil.ShowMessage(Page, MessageType.Notice, GetNoPermissionMessage(noticeMessage.ToString()));
            }

            return hasPermission;
        }

        /// <summary>
        /// Get no permission message.
        /// </summary>
        /// <param name="message">the message.</param>
        /// <returns>the no permission message.</returns>
        private string GetNoPermissionMessage(string message)
        {
            StringBuilder noticeMessage = new StringBuilder();
            noticeMessage.Append(GetTextByKey("aca_delegate_no_permission_message"));
            noticeMessage.Append("<ul class='ACA_NoticeChangedFee'>");

            noticeMessage.Append(message);

            noticeMessage.Append("</ul>");
            noticeMessage.Append("<div class='ACA_NoticeEndChangedFee'>");
            noticeMessage.Append(GetTextByKey("aca_delegate_to_savelater"));
            noticeMessage.Append("</div>");

            return noticeMessage.ToString();
        }

        /// <summary>
        /// Check all the caps in pay now list whether have been completed.
        /// </summary>
        /// <param name="shoppingCartItems">Shopping cart pay now list</param>
        /// <param name="notCompletedRecords">output the not completed caps</param>
        /// <returns>true - means all the caps is valid.</returns>
        private bool CheckRecordsStatus(ShoppingCartItemModel4WS[] shoppingCartItems, out IList<CapIDModel4WS> notCompletedRecords)
        {
            bool result = true;
            notCompletedRecords = new List<CapIDModel4WS>();

            if (shoppingCartItems != null && shoppingCartItems.Length > 0)
            {
                foreach (ShoppingCartItemModel4WS cartItem in shoppingCartItems)
                {
                    if (CapUtil.IsPartialCap(cartItem.capClass) && !ACAConstant.INCOMPLETE_EST.Equals(cartItem.capClass, StringComparison.OrdinalIgnoreCase))
                    {
                        notCompletedRecords.Add(cartItem.capID);
                        result = false;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check the related caps for Associated Forms,to prevent the cap be paid by individual.
        /// </summary>
        /// <param name="capIds">the CAP id list</param>
        /// <param name="notInCartRelatedRecords">missed related cap ids<see cref="capIds"/></param>
        /// <returns>true - means all the related caps already existed in the CAP list.</returns>
        private bool CheckRelatedRecords(CapIDModel[] capIds, out IList<CapIDModel> notInCartRelatedRecords)
        {
            bool result = true;
            notInCartRelatedRecords = new List<CapIDModel>();

            if (capIds != null && capIds.Length > 0)
            {
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                notInCartRelatedRecords = capBll.GetMissingRecordIDs4ShoppingCart(capIds, ACAConstant.CAP_RELATIONSHIP_ASSOFORM);

                if (notInCartRelatedRecords != null && notInCartRelatedRecords.Count() > 0)
                {
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Bind total fee
        /// </summary>
        /// <param name="htAllCartItems">all shopping cart items.</param>
        private void BindTotalFee(Hashtable htAllCartItems)
        {
            CapIDModel4WS[] capIDs = GetCapIDsFromShoppingCart(htAllCartItems);
            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();
            double totalFee = shoppingCartBll.GetTotalFeeForShoppingCart(ConfigManager.AgencyCode, capIDs, AppSession.User.PublicUserId);

            lblFeeAmount.Text = I18nNumberUtil.FormatMoneyForUI(totalFee);
        }

        /// <summary>
        /// Bind Data to Shopping cart.
        /// </summary>
        /// <param name="htAllCartItems">all shopping cart items.</param>
        private void BindShoppingCart(Hashtable htAllCartItems)
        {
            ShoppingCartModel4WS selectedShoppingCart = FilterShoppingCart(htAllCartItems, true);
            bool isAgencyDisabledPayment = !FunctionTable.IsEnableMakePayment();

            if (isAgencyDisabledPayment || selectedShoppingCart == null || selectedShoppingCart.shoppingCartItems == null || selectedShoppingCart.shoppingCartItems.Length == 0)
            {
                btnCheckOut.Enabled = false;
                divNoRecord.Visible = true;
            }
            else
            {
                btnCheckOut.Enabled = true;
                divNoRecord.Visible = false;
            }

            ShoppingCartListSelected.Visible = true;
            ShoppingCartListSelected.InavailableExamFeeItems = InavailableExamFeeItems;
            ShoppingCartListSelected.ShowShoppingCart(selectedShoppingCart);
            btnCheckOut.Enabled = ShoppingCartListSelected.HasIntersectPaymentType && !isAgencyDisabledPayment;

            if (IsInavailaleExamCapSelected(selectedShoppingCart))
            {
                string msg = GetTextByKey("aca_exam_msg_inavailable2cartpayfee");
                MessageUtil.ShowMessage(Page, MessageType.Error, msg);
                btnCheckOut.Enabled = false;
            }

            if (!ShoppingCartListSelected.HasIntersectPaymentType)
            {
                AppSession.ShoppingCartBreadcrumbParams.LastIndex = 1;
            }

            ShoppingCartModel4WS savedShoppingCart = FilterShoppingCart(htAllCartItems, false);

            if (savedShoppingCart == null || savedShoppingCart.shoppingCartItems == null || savedShoppingCart.shoppingCartItems.Length == 0)
            {
                ShoppingCartListSaved.Visible = false;
                divTitleSaved.Visible = false;
            }
            else
            {
                divTitleSaved.Visible = true;
                ShoppingCartListSaved.Visible = true;
                ShoppingCartListSaved.InavailableExamFeeItems = null;
                ShoppingCartListSaved.ShowShoppingCart(savedShoppingCart);
            }
        }

        /// <summary>
        /// Judge is there in-available exam existing in selected to payment caps.
        /// </summary>
        /// <param name="selectedShoppingCart">the selected shopping cart</param>
        /// <returns>return true if in-available exam existing in the selected </returns>
        private bool IsInavailaleExamCapSelected(ShoppingCartModel4WS selectedShoppingCart)
        {
            if (InavailableExamFeeItems == null || selectedShoppingCart == null || selectedShoppingCart.shoppingCartItems == null || selectedShoppingCart.shoppingCartItems.Length == 0)
            {
                return false;
            }

            foreach (var item in selectedShoppingCart.shoppingCartItems)
            {
                if (item.feeItems == null || item.feeItems.Length == 0)
                {
                    continue;
                }

                foreach (var fee in item.feeItems)
                {
                    if (InavailableExamFeeItems.Any(feeItem => feeItem.capID.Equals(fee.capID) && feeItem.feeSeqNbr == fee.feeSeqNbr))
                    {
                        return true;
                    }    
                }
            }

            return false;
        }

        /// <summary>
        /// splits shopping cart items with pay now and pay later.
        /// </summary>
        /// <param name="htShoppingFilterModel">hash table.</param>
        /// <param name="isSelected">is selected or not</param>
        /// <returns>ShoppingCartModel4WS object</returns>
        private ShoppingCartModel4WS FilterShoppingCart(Hashtable htShoppingFilterModel, bool isSelected)
        {
            ShoppingCartModel4WS shoppingCartFiltered = null;

            if (htShoppingFilterModel == null)
            {
                return null;
            }

            if (isSelected)
            {
                shoppingCartFiltered = (ShoppingCartModel4WS)htShoppingFilterModel[0];
            }
            else
            {
                shoppingCartFiltered = (ShoppingCartModel4WS)htShoppingFilterModel[1];
            }

            return shoppingCartFiltered;
        }

        /// <summary>
        /// Get pay now CapID Models from dataSource.
        /// </summary>
        /// <returns>CapIDModel4WS array</returns>
        /// <param name="htAllCartItems">all shopping cart items.</param>
        private CapIDModel4WS[] GetCapIDsFromShoppingCart(Hashtable htAllCartItems)
        {
            ShoppingCartModel4WS selectedShoppingCart = FilterShoppingCart(htAllCartItems, true);

            if (selectedShoppingCart == null || selectedShoppingCart.shoppingCartItems == null || selectedShoppingCart.shoppingCartItems.Length <= 0)
            {
                return null;
            }

            ShoppingCartItemModel4WS[] shoppingCartItems = selectedShoppingCart.shoppingCartItems;
            int itemLength = shoppingCartItems.Length;
            CapIDModel4WS[] capIDs = new CapIDModel4WS[itemLength];

            for (int i = 0; i < itemLength; i++)
            {
                capIDs[i] = shoppingCartItems[i].capID;
            }

            return capIDs;
        }

        /// <summary>
        /// if one cap total fee negative , return false.
        /// </summary>
        /// <param name="shoppingCartItems">The shopping cart item list.</param>
        /// <returns>CapIDModel4WS array</returns>
        private CapIDModel4WS[] GetPositiveCapIDs(IList<ShoppingCartItemModel4WS> shoppingCartItems)
        {
            CapIDModel4WS[] capIDs = null;

            if (shoppingCartItems != null && shoppingCartItems.Count > 0)
            {
                if (shoppingCartItems[0].capType != null && shoppingCartItems[0].capType.moduleName != null)
                {
                    Session[SessionConstant.SESSION_DEFAULT_MODULE] = shoppingCartItems[0].capType.moduleName;
                }

                int itemLength = shoppingCartItems.Count;
                capIDs = new CapIDModel4WS[itemLength];

                for (int i = 0; i < itemLength; i++)
                {
                    double capTotalFee = GetTotalFee(shoppingCartItems[i]);

                    if (capTotalFee < 0)
                    {
                        _isOneCapHasNagativeFee = true;
                        return null;
                    }

                    capIDs[i] = shoppingCartItems[i].capID;
                }
            }

            return capIDs;
        }

        /// <summary>
        /// Get positive shopping cart items.
        /// </summary>
        /// <param name="htAllShoppingCart">The all shopping cart.</param>
        /// <returns>ShoppingCartItemModel4WS array.</returns>
        private ShoppingCartItemModel4WS[] GetShoppingCartItems(Hashtable htAllShoppingCart)
        {
            ShoppingCartModel4WS selectedShoppingCart = FilterShoppingCart(htAllShoppingCart, true);

            if (selectedShoppingCart == null)
            {
                return null;
            }

            return selectedShoppingCart.shoppingCartItems;
        }

        /// <summary>
        /// get all shopping cart items, include pay now and pay later.
        /// </summary>
        /// <returns>all shopping cart items</returns>
        private Hashtable GetShoppingCart()
        {
            Hashtable htShoppingCartItems = null;

            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();

            if (AppSession.User.IsAnonymous)
            {
                return null;
            }

            htShoppingCartItems = shoppingCartBll.GetShoppingCart(ConfigManager.AgencyCode, long.Parse(AppSession.User.UserSeqNum), AppSession.User.PublicUserId);

            return htShoppingCartItems;
        }

        /// <summary>
        /// tab name is same as module name
        /// </summary>
        /// <param name="url">string url</param>
        /// <returns>the tab name.</returns>
        private string GetTabName(string url)
        {
            string moduleName = string.Empty;
            int start = url.IndexOf("module=");

            if (start > 0)
            {
                moduleName = url.Substring(start + 7);
            }
            else
            {
                return string.Empty;
            }

            return "&tabName=" + moduleName;
        }

        /// <summary>
        /// get cap total fee.
        /// </summary>
        /// <param name="shoppingCartItem">ShoppingCartItemModel4WS object</param>
        /// <returns>cap total fee</returns>
        private double GetTotalFee(ShoppingCartItemModel4WS shoppingCartItem)
        {
            double fee = 0;

            if (shoppingCartItem.feeItems == null)
            {
                return 0;
            }

            F4FeeItemModel4WS[] f4FeeItems = shoppingCartItem.feeItems;

            foreach (F4FeeItemModel4WS f4FeeItem in f4FeeItems)
            {
                fee += f4FeeItem.fee;
            }

            return fee;
        }

        /// <summary>
        /// Redirect to home page
        /// </summary>
        private void RedirectToHomePage()
        {
            string homePageUrl = ACAConstant.URL_WELCOME_PAGE;
            homePageUrl = FileUtil.AppendApplicationRoot(homePageUrl);
            Response.Redirect(homePageUrl);
        }

        /// <summary>
        /// Remove shopping cart 
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        private void ShoppingCartRemoveCommand(object sender, EventArgs e)
        {
            Hashtable htAllCartItems = GetShoppingCart();
            ShowFeeChangeMessage();
            BindShoppingCart(htAllCartItems);
            BindTotalFee(htAllCartItems);
            UpdateCapIDSession(htAllCartItems);
            ShoppingCartUtil.SetCartItemNumber();
        }

        /// <summary>
        /// Update shopping cart
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        private void ShoppingCartUpdateCommand(object sender, EventArgs e)
        {
            Hashtable htAllCartItems = GetShoppingCart();
            ShowFeeChangeMessage();
            BindShoppingCart(htAllCartItems);
            BindTotalFee(htAllCartItems);
            UpdateCapIDSession(htAllCartItems);
            ShoppingCartUtil.SetCartItemNumber();
        }

        /// <summary>
        /// Show Fee Change note if the fee item changed in AA.
        /// </summary>
        private void ShowFeeChangeMessage()
        {
            IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();
            Hashtable htCartItemWithCustomID = shoppingCartBll.GetCartItemWithCustomID();
            CapIDModel4WS[] allCapIds = GetCapIDsFromHashTable(htCartItemWithCustomID);

            CapIDModel4WS[] feeChangedCapIds = CapUtil.GetFeeChangedCapIdList(allCapIds);

            if (feeChangedCapIds != null && feeChangedCapIds.Length > 0)
            {
                StringBuilder noticeMessage = new StringBuilder();
                noticeMessage.Append(GetTextByKey("per_shoppingcart_message_feeschedulebeginchangenote"));
                noticeMessage.Append("<ul class='ACA_NoticeChangedFee'>");

                foreach (CapIDModel4WS capID in feeChangedCapIds)
                {
                    string customerID = ConstructCustomID(capID);
                    noticeMessage.Append("<li>");

                    ShoppingCartItemModel4WS cartItem = (ShoppingCartItemModel4WS)htCartItemWithCustomID[customerID];

                    if (htCartItemWithCustomID.Contains(customerID) &&
                        string.Equals(ACAConstant.CAP_RENEWAL, cartItem.processType, StringComparison.InvariantCulture))
                    {
                        ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                        CapIDModel parentCapIdModel = capBll.GetParentCapIDByChildCapID(TempModelConvert.Trim4WSOfCapIDModel(cartItem.capID), ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);

                        if (parentCapIdModel != null && !string.IsNullOrEmpty(parentCapIdModel.customID))
                        {
                            noticeMessage.Append(parentCapIdModel.customID);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(capID.customID))
                        {
                            noticeMessage.Append(capID.customID);
                        }
                    }

                    noticeMessage.Append("</li>");
                }

                noticeMessage.Append("</ul>");
                noticeMessage.Append("<div class='ACA_NoticeEndChangedFee'>");
                noticeMessage.Append(GetTextByKey("per_shoppingcart_message_feescheduleendchangenote"));
                noticeMessage.Append("</div>");

                CapUtil.UpdateFeeScheduleOrFeeItems(allCapIds);
                MessageUtil.ShowMessage(Page, MessageType.Notice, noticeMessage.ToString());
            }
        }
 
        /// <summary>
        /// Get Cap ID model list from hashTable
        /// </summary>
        /// <param name="htCartItemWithCustomID">hast table contains cap id and status.</param>
        /// <returns>cap id model list</returns>
        private CapIDModel4WS[] GetCapIDsFromHashTable(Hashtable htCartItemWithCustomID)
        {
            CapIDModel4WS[] capIDs = new CapIDModel4WS[htCartItemWithCustomID.Count];
            int index = 0;

            foreach (DictionaryEntry entry in htCartItemWithCustomID)
            {
                capIDs[index] = ((ShoppingCartItemModel4WS)entry.Value).capID;
                index++;
            }

            return capIDs;
        }
        
        /// <summary>
        /// Construct cap id as "09EST-00000-09123"
        /// </summary>
        /// <param name="capID">cap id model</param>
        /// <returns>format cap id string.</returns>
        private string ConstructCustomID(CapIDModel4WS capID)
        {
            string customID = string.Empty;

            if (capID != null)
            {
                customID = string.Format("{0}-{1}-{2}", capID.id1, capID.id2, capID.id3);
            }

            return customID;
        }

        /// <summary>
        /// Get positive CAP models.
        /// </summary>
        /// <param name="htAllShoppingCart">The all shopping cart.</param>
        /// <returns>CapModel4WS array.</returns>
        private CapModel4WS[] GetCapsFromShoppingCart(Hashtable htAllShoppingCart)
        {
            ShoppingCartItemModel4WS[] shoppingCartItems = GetShoppingCartItems(htAllShoppingCart);

            if (shoppingCartItems == null || shoppingCartItems.Length < 1)
            {
                return null;
            }

            IList<CapModel4WS> capModelList = new List<CapModel4WS>();

            foreach (ShoppingCartItemModel4WS shoppingCartItem in shoppingCartItems)
            {
                if (shoppingCartItem != null)
                {
                    CapModel4WS capModel = new CapModel4WS();
                    capModel.capID = shoppingCartItem.capID;
                    capModel.capType = shoppingCartItem.capType;
                    capModel.capClass = shoppingCartItem.capClass;
                    capModelList.Add(capModel);
                }
            }

            CapModel4WS[] capModels = new CapModel4WS[capModelList.Count];
            capModelList.CopyTo(capModels, 0);

            return capModels;
        }

        /// <summary>
        /// Check the license of the permits. show error message if some permits have unavailable licenses.
        /// </summary>
        /// <param name="htAllShoppingCart">The all shopping cart.</param>
        /// <returns>true all permits are valid;otherwise,false.</returns>
        private bool CheckLicense4Caps(Hashtable htAllShoppingCart)
        {
            bool isValidCap = true;

            if (!LicenseUtil.EnableExpiredLicense())
            {
                return true;
            }

            CapModel4WS[] capModels = GetCapsFromShoppingCart(htAllShoppingCart);

            string invalidCapIDs = LicenseUtil.GetInvalidCapIDsByCheckLicense(capModels);

            if (!string.IsNullOrEmpty(invalidCapIDs))
            {
                string errorMsg = GetTextByKey("per_shoppingcart_error_unavailablelicense");
                MessageUtil.ShowMessage(Page, MessageType.Error, DataUtil.StringFormat(errorMsg, invalidCapIDs));
                isValidCap = false;
            }

            return isValidCap;
        }

        #endregion Methods
    }
}