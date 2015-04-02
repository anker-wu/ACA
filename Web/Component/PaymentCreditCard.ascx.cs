#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentCreditCard.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: PaymentCreditCard Control
 *
 *  Notes:
 *      $Id: PaymentCreditCard.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;

using AjaxControlToolkit;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// PaymentCreditCard Control.
    /// </summary>
    public partial class PaymentCreditCard : BaseUserControl
    {
        #region property begin

        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtCCPhone.ClientID);
                return sbControls.ToString();
            }
        }

        #endregion property end

        #region public function begin
        /// <summary>
        /// Construct CreditCardModel.
        /// </summary>
        /// <returns>a CreditCardModel4WS</returns>
        public CreditCardModel4WS GetCreditCardModel()
        {
            //construct an credit card model
            CreditCardModel4WS creditCardModel = new CreditCardModel4WS();
            creditCardModel.servProvCode = ConfigManager.AgencyCode;
            creditCardModel.callerID = AppSession.User.PublicUserId;
            creditCardModel.cardType = this.ddlCardType.Text;
            creditCardModel.holderName = this.txtCardName.Text;
            creditCardModel.expirationDate = GetExpirationDate();
            creditCardModel.expirationMonth = this.ddlExpMonth.Text;
            creditCardModel.expirationYear = this.ddlExpYear.Text;
            creditCardModel.streetAddress = this.txtCCStreetAdd1.Text;
            creditCardModel.city = this.txtCCCity.Text;
            creditCardModel.state = this.ddlCCState.Text;
            creditCardModel.postalCode = this.txtCCZip.GetZip(this.ddlCCCountry.SelectedValue.Trim());
            creditCardModel.email = this.txtCCEmail.Text;
            creditCardModel.pos = false;
            creditCardModel.accountNumber = this.txtCardNumber.Text;
            creditCardModel.securityCode = this.txtCCV.Text;
            creditCardModel.telephone = this.txtCCPhone.GetPhone(this.ddlCCCountry.SelectedValue.Trim());
            creditCardModel.telephoneCountryCode = this.txtCCPhone.CountryCodeText;
            creditCardModel.countryCD = this.ddlCCCountry.SelectedValue.Trim();

            if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.APIPayment)
            {
                creditCardModel.firstName = txtFirstName.Text;
                creditCardModel.middleName = txtMiddleName.Text;
                creditCardModel.lastName = txtLastName.Text;
                creditCardModel.billingPostalCD = creditCardModel.postalCode;
                creditCardModel.streetAddress2 = this.txtCCStreetAdd2.Text;

                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                string country = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_ONLINE_PAYMENT_WEBSERVICE, ACAConstant.ONLINE_PAYMENT_COUNTRY_CODE);

                creditCardModel.countryCD = country;
            }

            string moduleName = ModuleName;

            if (string.IsNullOrEmpty(moduleName) && Session[SessionConstant.SESSION_DEFAULT_MODULE] != null)
            {
                moduleName = Session[SessionConstant.SESSION_DEFAULT_MODULE].ToString();
            }

            creditCardModel.additionInfo = PaymentHelper.BuildAdditionInfoForPayment(ConfigManager.AgencyCode, moduleName, AppSession.User.UserID, ACAConstant.PUBLIC_USER_NAME, string.Empty);

            return creditCardModel;
        }

        /// <summary>
        /// Get expiration date.
        /// </summary>
        /// <returns>the expirationDate</returns>
        public string GetExpirationDate()
        {
            DateTime expirationDateTime = new DateTime(int.Parse(ddlExpYear.Text), int.Parse(ddlExpMonth.Text), 1);
            return I18nDateTimeUtil.FormatToDateTimeStringForWebService(expirationDateTime);
        }

        /// <summary>
        /// Clear form data when switch payment method.
        /// </summary>
        public void InitAllValue()
        {
            ControlUtil.ApplyRegionalSetting(false, false, true, !IsPostBack, ddlCCCountry);

            //reset Credit Card
            DropDownListBindUtil.SetSelectedToFirstItem(ddlCardType);

            //bind country
            ddlCCCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCCCountry.RegisterScripts();

            DropDownListBindUtil.SetSelectedToFirstItem(ddlExpMonth);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlExpYear);
            txtCCEmail.Text = string.Empty;
            txtCCZip.Text = string.Empty;
            txtCCV.Text = string.Empty;
            txtCCCity.Text = string.Empty;
            txtCardName.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
            txtCCPhone.Text = string.Empty;
            txtCCPhone.CountryCodeText = string.Empty;
            txtCCStreetAdd1.Text = string.Empty;

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                ddlAutoFill.Enabled = false;
            }

            chkAutoFill.Attributes.Add("title", GetTextByKey(chkAutoFill.LabelKey));
            ddlCCState.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtCCStreetAdd2.Text = string.Empty;
        }

        /// <summary>
        /// Initializes Credit card for STP
        /// first_name    60 letters, numbers, and ,-.'
        /// middle_name 60 letters, numbers, and ,-.'
        /// last_name 60    letters, numbers, and ,-.'
        /// address 60 letters, numbers, and ,-.'#
        /// city_name 60    letters, numbers, and ,-.'
        /// state_code 60 letters, numbers, and ,-.'
        /// postal_code 32 letters and numbers
        /// telephone 32    letters and numbers
        /// </summary>
        public void InitCreditCardForSTP()
        {
            FilterTypes filterTypes = FilterTypes.Custom | FilterTypes.LowercaseLetters | FilterTypes.UppercaseLetters | FilterTypes.Numbers;
            string valiCharsForSTP = " ,-.'";
            string addressValiCharsForSTP = valiCharsForSTP + "#";

            txtFirstName.FilterType = filterTypes;
            txtFirstName.ValidChars = valiCharsForSTP;
            txtMiddleName.FilterType = filterTypes;
            txtMiddleName.ValidChars = valiCharsForSTP;
            txtLastName.FilterType = filterTypes;
            txtLastName.ValidChars = valiCharsForSTP;
            txtCCStreetAdd1.FilterType = filterTypes;
            txtCCStreetAdd1.ValidChars = addressValiCharsForSTP;
            txtCCStreetAdd2.FilterType = filterTypes;
            txtCCStreetAdd2.ValidChars = addressValiCharsForSTP;
            txtCCCity.FilterType = filterTypes;
            txtCCCity.ValidChars = valiCharsForSTP;

            divAccountName.Visible = true;  //cc
            divAddress2.Visible = true;           //cc

            liCardName.Visible = false;  //cc
        }

        /// <summary>
        /// Initializes API UI.
        /// </summary>
        public void InitAPIUI()
        {
            liCardName.Visible = true;
            divAccountName.Visible = false;
            divAddress2.Visible = false;
        }

        /// <summary>
        /// Register Scripts.
        /// </summary>
        public void RegisterScripts()
        {
            ddlCCCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCCCountry.RegisterScripts();
        }

        #endregion public function end

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlCCCountry.BindItems();
            ddlCCCountry.SetCountryControls(txtCCZip, ddlCCState, txtCCPhone);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, false, true, !IsPostBack, ddlCCCountry);

            if (!IsPostBack)
            {
                InitUI();
                BindConactList();
            }
            else
            {
                if (!AppSession.IsAdmin)
                {
                    ddlAutoFill.Enabled = chkAutoFill.Checked;
                }
            }
        }

        #region private function begin

        /// <summary>
        /// Set current City and State
        /// </summary>
        private void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCCCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(ddlCCState, ModuleName);
        }

        /// <summary>
        /// Hide bill information check box if current user is Anonymous user.
        /// </summary>
        private void HideChkBillInformation()
        {
            if (AppSession.User.IsAnonymous)
            {
                dvAutoFill.Visible = false;
            }
        }

        /// <summary>
        /// Bind Contact list for auto fill.
        /// </summary>
        private void BindConactList()
        {
            if (!AppSession.IsAdmin)
            {
                ddlAutoFill.Attributes.Add("onchange", "creditcard_contactchanged()");
                chkAutoFill.Attributes.Add("onclick", "creditcard_chkAutoFillclick(this)");

                IList<ListItem> items = DropDownListBindUtil.ConvertPeopleToListItem(AppSession.User.ApprovedContacts);

                DropDownListBindUtil.BindDDL(items, this.ddlAutoFill, false, false);
            }
        }

        /// <summary>
        /// Initializes UI method.
        /// </summary>
        private void InitUI()
        {
            this.InitCreditCard();
            HideChkBillInformation();
        }

        /// <summary>
        /// Initializes Credit Card Payment UI.
        /// </summary>
        private void InitCreditCard()
        {
            ddlCCCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCCCountry.RegisterScripts();
            DropDownListBindUtil.BindStandardChoise(ddlCardType, BizDomainConstant.STD_CAT_PAYMENT_CREDITCARD_TYPE);
            DropDownListBindUtil.BindMonth(this.ddlExpMonth);
            DropDownListBindUtil.BindYear(this.ddlExpYear);
            string expDate = GetTextByKey("per_permitPayFee_label_expAndDate");
            string expYear = GetTextByKey("aca_payment_year");
            string expMonth = GetTextByKey("aca_payment_month");
            ddlExpMonth.ToolTip = string.Format("{0} {1}", expDate, expMonth);
            ddlExpYear.ToolTip = string.Format("{0} {1}", expDate, expYear);
            ddlCardType.MaxValueLength = StandardChoiceMaxLength.MAX_LENGTH_PAYMENT_CREDITCARD_TYPE;
        }

        #endregion private function end
    }
}