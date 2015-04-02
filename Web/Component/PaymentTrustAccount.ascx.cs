/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentTrustAccount.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PaymentTrustAccount.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// PaymentTrustAccount Control
    /// </summary>
    public partial class PaymentTrustAccount : BaseUserControl
    {
        /// <summary>
        /// Associated Type Label Key Prefix
        /// </summary>
        private const string ASSOCIATED_TYPE_LABEL_KEY_PREFIX = "aca_payment_trustaccount_associatedtype_";

        /// <summary>
        /// Gets or sets the trust account id.
        /// </summary>
        public string TrustAccountID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets it to 'N' when money not enough or charge amount out of limited.
        /// </summary>
        public PaymentMessageType PaymentMessageType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the true to selected the all.
        /// </summary>
        public string NeedDefaultSelected
        {
            get;
            set;
        }

        #region public function begin

        /// <summary>
        /// Initializes TrustAccount
        /// </summary>
        public void InitTrustAccount()
        {
            divTrustAccountDetail.Visible = false;
            rdlAssociatedType.SelectedIndex = -1;
            ddlTrustAccount.SelectedValue = null;
            TrustAccountID = null;
        }

        /// <summary>
        /// Construct a TrustAccountModel4WS from user's input
        /// </summary>
        /// <returns>a TrustAccountModel4WS</returns>
        public TrustAccountModel GetTrustAccountModel()
        {
            TrustAccountModel trustAccount = new TrustAccountModel();
            trustAccount.acctID = string.IsNullOrEmpty(TrustAccountID) ? ddlTrustAccount.SelectedValue : TrustAccountID;
            trustAccount.servProvCode = ConfigManager.AgencyCode;

            // reset the account ID
            TrustAccountID = string.Empty;

            return trustAccount;
        }
        #endregion public function end

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindAssociatedTypeList();

                if (ValidationUtil.IsYes(NeedDefaultSelected))
                {
                    rdlAssociatedType.SelectedIndex = 0;
                    AssociatedTypeDropdown_IndexChanged(null, null);
                }
            }

            rdlAssociatedType.AddRequiredValidator();

            if (!string.IsNullOrEmpty(rdlAssociatedType.SelectedValue))
            {
                ddlAssociatedType.LabelKey = (ASSOCIATED_TYPE_LABEL_KEY_PREFIX + rdlAssociatedType.SelectedValue).ToLower();
            }

            DisplayTrustAccountInfo();

            InitAdmin();
        }

        /// <summary>
        /// Dropdown list command
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AssociatedTypeDropdown_IndexChanged(object sender, EventArgs e)
        {
            MessageUtil.HideMessageByControl(Page);
            InitAllValue();

            //Focus the each related dropdown field after selected specific associated type.
            Page.FocusElement(ddlAssociatedType.ClientID);
        }

        /// <summary>
        /// AssociatedType Dropdown List SelectedIndexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AssociatedTypeDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string eventTarget = Request["__EVENTTARGET"];

            if ((!string.IsNullOrEmpty(eventTarget) && (eventTarget.IndexOf("ddlAssociatedType") > -1 || eventTarget.IndexOf("rdlAssociatedType") > -1)) || PaymentMessageType != PaymentMessageType.None)
            {
                BindTrustAccount(string.Empty);

                // if license professional drop down with one avaiable item been selected, and trust account drop down selected value, it needn't re-bind item for trust account drop down.
                if (ddlAssociatedType.Required && IsOneAvaiableItem(ddlAssociatedType.Items) && !string.IsNullOrEmpty(ddlAssociatedType.SelectedValue) && !string.IsNullOrEmpty(ddlTrustAccount.SelectedValue))
                {
                    return;
                }

                bool isEntityLocked = false;

                if (ddlAssociatedType.Required && IsOneAvaiableItem(ddlAssociatedType.Items))
                {
                    isEntityLocked = IsEntityLocked(ddlAssociatedType.Items[1].Value);

                    if (!isEntityLocked)
                    {
                        BindTrustAccount(ddlAssociatedType.Items[1].Value);
                    }
                }
                else
                {
                    isEntityLocked = IsEntityLocked(ddlAssociatedType.SelectedValue);

                    if (!isEntityLocked)
                    {
                        BindTrustAccount(ddlAssociatedType.SelectedValue);
                    }
                }

                //Auto trigger drop down select event.
                if (!isEntityLocked)
                {
                    TriggerTrustNameOnChange();
                }
            }
        }

        #region private function begin

        /// <summary>
        /// Judges the entity is locked or not.
        /// </summary>
        /// <param name="seqNbr">Entity sequence number.</param>
        /// <returns>IsLock Flag</returns>
        private bool IsEntityLocked(string seqNbr)
        {
            if (string.IsNullOrEmpty(seqNbr) && ACAConstant.PaymentAssociatedType.Record.ToString().CompareTo(rdlAssociatedType.SelectedValue) != 0)
            {
                return false;
            }

            bool isLocked = false;
            string messageLabelKey = string.Empty;
            IConditionBll conditionBll = ObjectFactory.GetObject<IConditionBll>();

            switch ((ACAConstant.PaymentAssociatedType)ACAConstant.PaymentAssociatedType.Parse(typeof(ACAConstant.PaymentAssociatedType), rdlAssociatedType.SelectedValue))
            {
                case ACAConstant.PaymentAssociatedType.Licenses:
                    isLocked = conditionBll.IsLicenseLocked(ConfigManager.AgencyCode, long.Parse(seqNbr), AppSession.User.UserID);
                    messageLabelKey = "aca_trustaccount_message_licenselocked";
                    break;
                case ACAConstant.PaymentAssociatedType.Contacts:
                    isLocked = conditionBll.IsContactLocked(ConfigManager.AgencyCode, long.Parse(seqNbr), AppSession.User.UserID);
                    messageLabelKey = "aca_trustaccount_message_contactlocked";
                    break;
                case ACAConstant.PaymentAssociatedType.Addresses:
                    RefAddressModel addressModel = new RefAddressModel();
                    addressModel.refAddressId = long.Parse(seqNbr);
                    isLocked = conditionBll.IsAddressLocked(ConfigManager.AgencyCode, addressModel, AppSession.User.UserID);
                    messageLabelKey = "aca_trustaccount_message_addresslocked";
                    break;
                case ACAConstant.PaymentAssociatedType.Parcels:
                    ParcelModel parcelModel = new ParcelModel();
                    parcelModel.parcelNumber = seqNbr;
                    isLocked = conditionBll.IsParcelLocked(ConfigManager.AgencyCode, parcelModel, AppSession.User.UserID);
                    messageLabelKey = "aca_trustaccount_message_parcellocked";
                    break;
                case ACAConstant.PaymentAssociatedType.Record:
                    if (!string.IsNullOrEmpty(seqNbr))
                    {
                        string[] altId = seqNbr.Split(ACAConstant.SPLIT_CHAR18);
                        ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                        CapIDModel4WS selectedCapId = capBll.GetCapIDByAltID(altId[0], altId[1]);
                        isLocked = conditionBll.IsCapLocked(TempModelConvert.Trim4WSOfCapIDModel(selectedCapId));
                        messageLabelKey = "aca_trustaccount_message_recordlocked";
                    }

                    break;
            }

            if (isLocked)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey(messageLabelKey));
            }

            return isLocked;
        }

        /// <summary>
        /// bind trust account drop down value
        /// </summary>
        /// <param name="seqNbr">sequence number.</param>
        private void BindTrustAccount(string seqNbr)
        {
            if (string.IsNullOrEmpty(seqNbr) && rdlAssociatedType.SelectedValue.CompareTo(ACAConstant.PaymentAssociatedType.Record.ToString()) != 0)
            {
                ddlTrustAccount.Items.Clear();
                ddlTrustAccount.Items.Add(DropDownListBindUtil.DefaultListItem);

                return;
            }

            ACAConstant.PaymentAssociatedType paymentAssociatedType = EnumUtil<ACAConstant.PaymentAssociatedType>.Parse(rdlAssociatedType.SelectedValue);

            IList<ListItem> trustAccountList = TrustAccountUtil.GetTrustAccounts(seqNbr, paymentAssociatedType);

            if (trustAccountList != null && trustAccountList.Count > 0)
            {
                DropDownListBindUtil.BindDDL(trustAccountList, ddlTrustAccount);
            }
            else
            {
                ddlTrustAccount.Items.Clear();
                ddlTrustAccount.Items.Add(DropDownListBindUtil.DefaultListItem);
            }
        }

        /// <summary>
        /// Trig trust name dropdown list on change event
        /// </summary>
        private void TriggerTrustNameOnChange()
        {
            if (ddlTrustAccount.Required && IsOneAvaiableItem(ddlTrustAccount.Items))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "trustAccount_onchange", "trustAccount_onchange();", true);
            }
        }

        /// <summary>
        /// Flag for whether include one available item.
        /// </summary>
        /// <param name="listItem">list item collection for dropdown list</param>
        /// <returns>true or false</returns>
        private bool IsOneAvaiableItem(ListItemCollection listItem)
        {
            ListItem defaultItem = new ListItem(WebConstant.DropDownDefaultText, string.Empty);

            if (listItem != null && listItem.Count == 2 && listItem.IndexOf(defaultItem) != -1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Bind the associated type list.
        /// </summary>
        private void BindAssociatedTypeList()
        {
            IList<string> allAssociatedTypes = TrustAccountUtil.GetAllAssociatedTypes();

            if (allAssociatedTypes != null && allAssociatedTypes.Count > 0)
            {
                foreach (var associatedType in allAssociatedTypes)
                {
                    rdlAssociatedType.Items.Add(
                        new ListItem(GetTextByKey((ASSOCIATED_TYPE_LABEL_KEY_PREFIX + associatedType).ToLower()), associatedType));
                }
            }
        }

        /// <summary>
        /// Clear form data when switch payment method.
        /// </summary>
        private void InitAllValue()
        {
            string selectedAssociatedType = rdlAssociatedType.SelectedValue;

            divTrustAccountDetail.Visible = true;
            ddlAssociatedType.LabelKey = (ASSOCIATED_TYPE_LABEL_KEY_PREFIX + selectedAssociatedType).ToLower();

            if (!AppSession.IsAdmin)
            {
                //reset Trust Account
                DropDownListBindUtil.BindTrustAccountValidAssociatedTypes(EnumUtil<ACAConstant.PaymentAssociatedType>.Parse(rdlAssociatedType.SelectedValue), ddlAssociatedType, false);

                if (ddlAssociatedType.Items.Count <= 0)
                {
                    divTrustAccountDetail.Visible = false;
                }
                else
                {
                    DropDownListBindUtil.BindDDL(null, ddlTrustAccount, true);

                    DropDownListBindUtil.SetSelectedToFirstItem(ddlTrustAccount);

                    //Auto trigger license id drop down onchange mothed.
                    AssociatedTypeDropdown_SelectedIndexChanged(null, null);
                }
            }
        }

        /// <summary>
        /// Initializes for admin
        /// </summary>
        private void InitAdmin()
        {
            if (AppSession.IsAdmin)
            {
                rdlAssociatedType.AutoPostBack = false;
                divTrustAccountDetail.Visible = true;
                ddlAssociatedType.Visible = true;
                ddlContacts.Visible = true;
                ddlAddresses.Visible = true;
                ddlParcels.Visible = true;
                ddlRecord.Visible = true;

                rdlAssociatedType.Attributes.Add("onclick", "ChangeAssociatedType()");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "associatedTypeChange", "ChangeAssociatedType();", true);
            }
        }

        /// <summary>
        /// Display the trust account's information.
        /// </summary>
        private void DisplayTrustAccountInfo()
        {
            TrustAccountModel trustAccountModel = TrustAccountUtil.GetTrustAccountModel(GetTrustAccountModel().acctID);

            if (trustAccountModel != null)
            {
                string name = I18nStringUtil.GetString(trustAccountModel.resDescription, trustAccountModel.description);
                double balance = TrustAccountUtil.GetTotalAmount(trustAccountModel);

                if (!string.IsNullOrEmpty(name))
                {
                    divTrustAccountInfo.InnerHtml = string.Format(
                                                                  @"<br/>
                                                                    <table role='presentation'>
                                                                        <tr><td>{0}</td><td>{1}</td></tr>
                                                                        <tr><td>{2}</td><td>{3}</td></tr>
                                                                    </table>",
                                                                    GetTextByKey("ACA_CapPayment_NameOnTrustAccount").Replace("'", "\\'"),
                                                                    name,
                                                                    GetTextByKey("ACA_CapPayment_AmountAvailable").Replace("'", "\\'"),
                                                                    I18nNumberUtil.FormatMoneyForUI(balance));
                }
            }
            else
            {
                divTrustAccountInfo.InnerHtml = string.Empty;
            }
        }

        #endregion private function end
    }
}