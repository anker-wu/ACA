#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentCheckProcessing.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: PaymentCheckProcessing Control
 *
 *  Notes:
 *      $Id: PaymentCheckProcessing.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
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
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;

using AjaxControlToolkit;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// PaymentCheckProcessing Control
    /// </summary>
    public partial class PaymentCheckProcessing : BaseUserControl
    {
        #region fields begin
        /// <summary>
        /// account type personal.
        /// </summary>
        private const string ACCT_TYPE_PERSONAL = "Personal";

        /// <summary>
        /// account type business.
        /// </summary>
        private const string ACCT_TYPE_BUSINESS = "Business";

        /// <summary>
        /// check method electronic check.
        /// </summary>
        private const string CHK_METHOD_ELECTRONIC_CHECK = "Electronic Check";

        /// <summary>
        /// check method account debit.
        /// </summary>
        private const string CHK_METHOD_ACCOUNT_DEBIT = "Account Debit";

        #endregion fields end

        #region property begin
        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtChkInfoPhoneNbr.ClientID);
                return sbControls.ToString();
            }
        }
        #endregion property end

        #region public function begin

        /// <summary>
        /// Clear form data when switch payment method.
        /// </summary>
        public void InitAllValue()
        {
            ControlUtil.ApplyRegionalSetting(false, false, true, !IsPostBack, ddlChkInfoCountry);

            DropDownListBindUtil.SetSelectedToFirstItem(this.ddlProcessingMethod);
            ChangeProcessingMethod();
            DropDownListBindUtil.SetSelectedToFirstItem(this.ddlAccountType);
            ChangeAccountType();

            ddlChkInfoCountry.RelevantControlIDs = RelevantControlIDs;
            ddlChkInfoCountry.RegisterScripts();

            txtChkInfoEmail.Text = string.Empty;
            txtChkInfoPhoneNbr.Text = string.Empty;
            txtChkInfoPhoneNbr.CountryCodeText = string.Empty;
            txtChkInfoZip.Text = string.Empty;
            txtChkInfoCity.Text = string.Empty;
            txtChkInfoStreetAddr.Text = string.Empty;
            txtChkInfoAccountName.Text = string.Empty;
            txtCheckNbr.Text = string.Empty;
            txtAccountNbr.Text = string.Empty;
            txtRoutingNbr.Text = string.Empty;
            txtDriverLicNbr.Text = string.Empty;
            txtFederalTaxNbr.Text = string.Empty;

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                ddlAutoFill.Enabled = false;
            }

            chkAutoFill.Attributes.Add("title", GetTextByKey(chkAutoFill.LabelKey));
            ddlChkInfoState.Text = string.Empty;
            txtChkInfoFirstName.Text = string.Empty;
            txtChkInfoMiddleName.Text = string.Empty;
            txtChkInfoLastName.Text = string.Empty;
            txtChkInfoStreetAddr2.Text = string.Empty;
        }

        /// <summary>
        /// Construct CheckModel.
        /// </summary>
        /// <returns>a CheckModel4WS</returns>
        public CheckModel4WS GetCheckModel()
        {
            CheckModel4WS checkModel = new CheckModel4WS();
            checkModel.module = ModuleName;
            checkModel.callerID = AppSession.User.PublicUserId;
            checkModel.servProvCode = ConfigManager.AgencyCode;
            checkModel.pos = false;
            checkModel.accountNbr = txtAccountNbr.Text;
            checkModel.checkNbr = txtCheckNbr.Text;
            checkModel.routingNbr = txtRoutingNbr.Text;
            checkModel.city = txtChkInfoCity.Text;
            checkModel.name = txtChkInfoAccountName.Text;
            checkModel.streetAddress = txtChkInfoStreetAddr.Text;
            checkModel.phoneNbr = txtChkInfoPhoneNbr.GetPhone(this.ddlChkInfoCountry.SelectedValue.Trim());
            checkModel.phoneNbrCountryCode = txtChkInfoPhoneNbr.CountryCodeText;
            checkModel.state = ddlChkInfoState.Text;
            checkModel.countryCD = ddlChkInfoCountry.SelectedValue.Trim();

            checkModel.email = txtChkInfoEmail.Text;
            checkModel.postalCode = this.txtChkInfoZip.GetZip(ddlChkInfoCountry.SelectedValue.Trim());
            checkModel.comment = "ACA Check Payment"; // only a comment,not special meaning
            checkModel.licenseNbr = this.txtDriverLicNbr.Text; //Driver's License Number for account type is personal.
            checkModel.ssNbr = this.txtFederalTaxNbr.Text; //Federal Tax ID Number for account type is business.

            string moduleName = ModuleName;

            if (string.IsNullOrEmpty(moduleName) && Session[SessionConstant.SESSION_DEFAULT_MODULE] != null)
            {
                moduleName = Session[SessionConstant.SESSION_DEFAULT_MODULE].ToString();
            }

            checkModel.additionInfo = PaymentHelper.BuildAdditionInfoForPayment(ConfigManager.AgencyCode, moduleName, AppSession.User.UserID, ACAConstant.PUBLIC_USER_NAME, checkModel.comment);

            if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.APIPayment)
            {
                //the display of Account type in AA is disply as Check Type, this is differ from ACA.
                checkModel.checkType = ddlAccountType.SelectedValue;

                checkModel.firstName = txtChkInfoFirstName.Text;
                checkModel.middleName = txtChkInfoMiddleName.Text;
                checkModel.lastName = txtChkInfoLastName.Text;
                checkModel.streetAddress2 = txtChkInfoStreetAddr2.Text;

                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                string country = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_ONLINE_PAYMENT_WEBSERVICE, ACAConstant.ONLINE_PAYMENT_COUNTRY_CODE);

                checkModel.countryCD = country;
            }
            else
            {
                checkModel.checkType = ddlProcessingMethod.SelectedValue;
            }

            return checkModel;
        }

        /// <summary>
        /// Register Scripts.
        /// </summary>
        public void RegisterScripts()
        {
            ddlChkInfoCountry.RelevantControlIDs = RelevantControlIDs;
            ddlChkInfoCountry.RegisterScripts();
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        public void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtChkInfoCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(ddlChkInfoState, ModuleName);
        }

        /// <summary>
        /// Initializes Electronic Check for STP
        /// first_name    60 letters, numbers, and ,-.'
        /// middle_name 60 letters, numbers, and ,-.'
        /// last_name 60    letters, numbers, and ,-.'
        /// address 60 letters, numbers, and ,-.'#
        /// city_name 60    letters, numbers, and ,-.'
        /// state_code 60 letters, numbers, and ,-.'
        /// postal_code 32 letters and numbers
        /// telephone 32    letters and numbers
        /// </summary>
        public void InitCheckForSTP()
        {
            FilterTypes filterTypes = FilterTypes.Custom | FilterTypes.LowercaseLetters | FilterTypes.UppercaseLetters | FilterTypes.Numbers;
            string valiCharsForSTP = " ,-.'";
            string addressValiCharsForSTP = valiCharsForSTP + "#";

            txtChkInfoFirstName.FilterType = filterTypes;
            txtChkInfoFirstName.ValidChars = valiCharsForSTP;
            txtChkInfoMiddleName.FilterType = filterTypes;
            txtChkInfoMiddleName.ValidChars = valiCharsForSTP;
            txtChkInfoLastName.FilterType = filterTypes;
            txtChkInfoLastName.ValidChars = valiCharsForSTP;
            txtChkInfoStreetAddr.FilterType = filterTypes;
            txtChkInfoStreetAddr.ValidChars = addressValiCharsForSTP;
            txtChkInfoStreetAddr2.FilterType = filterTypes;
            txtChkInfoStreetAddr2.ValidChars = addressValiCharsForSTP;
            txtChkInfoCity.FilterType = filterTypes;
            txtChkInfoCity.ValidChars = valiCharsForSTP;

            divChkInfoAccountName.Visible = true; //cp
            divChkInfoAccountFullName.Visible = false; //cp
            divCheckInfoAddress.Visible = true;   //cp

            divProvide.Visible = false;  //cp
            txtCheckNbr.Visible = false;  //cp
            divProcessingMethod.Visible = false;  //cp
        }

        /// <summary>
        /// Initializes API UI.
        /// </summary>
        public void InitAPIUI()
        {
            divChkInfoAccountName.Visible = false;
            divChkInfoAccountFullName.Visible = true;
            divCheckInfoAddress.Visible = false;

            divProcessingMethod.Visible = true;
            divProvide.Visible = true;
        }
        #endregion public function end

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlChkInfoCountry.BindItems();
            ddlChkInfoCountry.SetCountryControls(txtChkInfoZip, ddlChkInfoState, txtChkInfoPhoneNbr);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, false, true, !IsPostBack, ddlChkInfoCountry);

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

        /// <summary>
        /// ProcessingMethod Dropdown List indexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ProcessingMethodDropdown_indexChanged(object sender, EventArgs e)
        {
            ChangeProcessingMethod();
        }

        /// <summary>
        /// AccountType Dropdown List indexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AccountTypeDropdown_indexChanged(object sender, EventArgs e)
        {
            ChangeAccountType();
        }

        #region private function begin

        /// <summary>
        /// Bind Contact list for auto fill.
        /// </summary>
        private void BindConactList()
        {
            if (!AppSession.IsAdmin)
            {
                ddlAutoFill.Attributes.Add("onchange", "contact_changed()");
                chkAutoFill.Attributes.Add("onclick", "chkAutoFill_click(this)");

                IList<ListItem> items = DropDownListBindUtil.ConvertPeopleToListItem(AppSession.User.ApprovedContacts);

                DropDownListBindUtil.BindDDL(items, this.ddlAutoFill, false, false);
            }
        }

        /// <summary>
        /// Bind Check Payment Processing Method.
        /// </summary>
        private void BindProcessingMethod()
        {
            DropDownListBindUtil.BindStandardChoise(ddlProcessingMethod, BizDomainConstant.STD_CAT_PAYMENT_CHECK_TYPE);
            ddlProcessingMethod.Items.Remove(DropDownListBindUtil.DefaultListItem);
            ddlProcessingMethod.MaxValueLength = StandardChoiceMaxLength.MAX_LENGTH_PAYMENT_CHECK_TYPE;
            ChangeProcessingMethod();
        }

        /// <summary>
        /// Bind Check Account Type.
        /// </summary>
        private void BindAccountType()
        {
            if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.APIPayment || IsCustomAdapter())
            {
                DropDownListBindUtil.BindStandardChoise(ddlAccountType, BizDomainConstant.STD_CAT_PAYMENT_CHECK_ACCOUNT_TYPE);
            }
            else
            {
                this.ddlAccountType.Items.Add(new ListItem(LabelUtil.GetTextByKey("per_permitPayFee_AcctType_Personal", string.Empty), ACCT_TYPE_PERSONAL));
                this.ddlAccountType.Items.Add(new ListItem(LabelUtil.GetTextByKey("per_permitPayFee_AcctType_Business", string.Empty), ACCT_TYPE_BUSINESS));
                ChangeAccountType();
            }
        }

        /// <summary>
        /// Change Account Type
        /// </summary>
        private void ChangeAccountType()
        {
            //if (ACCT_TYPE_PERSONAL.Equals(this.ddlAccountType.SelectedValue))
            if (ACCT_TYPE_PERSONAL.Equals(this.ddlAccountType.SelectedValue, StringComparison.InvariantCulture))
            {
                this.txtDriverLicNbr.Visible = true;
                this.txtFederalTaxNbr.Visible = false;
            }
            else if (ACCT_TYPE_BUSINESS.Equals(this.ddlAccountType.SelectedValue, StringComparison.InvariantCulture))
            {
                this.txtDriverLicNbr.Visible = false;
                this.txtFederalTaxNbr.Visible = true;
            }
        }

        /// <summary>
        /// Change Processing
        /// </summary>
        private void ChangeProcessingMethod()
        {
            if (CHK_METHOD_ACCOUNT_DEBIT.Equals(ddlProcessingMethod.SelectedValue, StringComparison.InvariantCulture))
            {
                this.txtCheckNbr.Visible = false;
            }
            else if (CHK_METHOD_ELECTRONIC_CHECK.Equals(ddlProcessingMethod.SelectedValue, StringComparison.InvariantCulture))
            {
                this.txtCheckNbr.Visible = true;
            }
        }

        /// <summary>
        /// Indicates whether the adapter is named by customer. 
        /// </summary>
        /// <returns>true - it is custom adapter. false - named by system.</returns>
        private bool IsCustomAdapter()
        {
            string[] adapterNames = { "PAYPAL", "PAYPAL43", "VIRTUALMERCHANT", "OPSTP", "OPCoBrandPlus", "Etisalat", "Govolution", "FirstData" };

            bool isCustom = true;

            string adapter = StandardChoiceUtil.GetEPaymentAdapterType();

            foreach (string adapterName in adapterNames)
            {
                if (adapter.StartsWith(adapterName + "_", StringComparison.InvariantCultureIgnoreCase))
                {
                    isCustom = false;
                    break;
                }
            }

            return isCustom;
        }

        /// <summary>
        /// Initial UI method.
        /// </summary>
        private void InitUI()
        {
            InitCheckProcessing();
            HideChkBillInformation();
            if (!AppSession.IsAdmin)
            {
                SetCurrentCityAndState();
            }

            if (AppSession.IsAdmin)
            {
                InitAdminUI();
            }
        }

        /// <summary>
        /// Initializes Check Payment UI
        /// </summary>
        private void InitCheckProcessing()
        {
            BindProcessingMethod();
            BindAccountType();
            ddlChkInfoCountry.RelevantControlIDs = RelevantControlIDs;
            ddlChkInfoCountry.RegisterScripts();
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
        /// Initializes Admin UI.
        /// </summary>
        private void InitAdminUI()
        {
            ddlProcessingMethod.AutoPostBack = false;
            txtCheckNbr.Visible = true;
            ddlProcessingMethod.Attributes.Add("onchange", "ChangeProcessingMethod(this)");
            ddlAccountType.AutoPostBack = false;
            txtDriverLicNbr.Visible = true;
            txtFederalTaxNbr.Visible = true;
            ddlAccountType.Attributes.Add("onchange", "ChangeAccountType(this)");
        }
        #endregion private function end
    }
}