#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationRelationSearchForm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationRelationSearchForm.ascx.cs 142550 2009-08-10 02:13:47Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for EducationRelationSearchForm
    /// </summary>
    public partial class EducationRelationSearchForm : FormDesignerBaseControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the EducationRelationSearchForm class.
        /// </summary>
        public EducationRelationSearchForm()
            : base(GviewID.SearchForProvider)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the search type with provider or examinations
        /// </summary>
        public GeneralInformationSearchType SearchType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets contact client id prefix.
        /// </summary>
        public string ContactIDPrefix
        {
            get
            {
                return phContent.ClientID.Replace(phContent.ID, string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets Permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                
                if (SearchType == GeneralInformationSearchType.Search4Provider)
                {
                    base.Permission.permissionLevel = "SEARCH_FOR_PROVIDER";
                }
                else
                {
                    base.Permission.permissionLevel = "SEARCH_FOR_EDUCATIONANDEXAM";
                }

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtPhone1.ClientID);
                sbControls.Append(",").Append(txtPhone2.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);

                return sbControls.ToString();
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// show provider info.
        /// </summary>
        /// <param name="educationSearchModel">The educationSearchModel</param>
        public void ShowProviderInfo(EducationSearchModel educationSearchModel)
        {
            if (educationSearchModel != null && educationSearchModel.providerModel4WS != null)
            {
                //provider search form.
                ProviderModel4WS providerModel4WS = educationSearchModel.providerModel4WS;
                if (providerModel4WS.offerEducation == ACAConstant.COMMON_Y)
                {
                    lblProviderType.SelectedValue = ProviderType.OfferEducation.ToString();
                }
                else if (providerModel4WS.offerContinuing == ACAConstant.COMMON_Y)
                {
                    lblProviderType.SelectedValue = ProviderType.OfferContinuing.ToString();
                }
                else if (providerModel4WS.offerExamination == ACAConstant.COMMON_Y)
                {
                    lblProviderType.SelectedValue = ProviderType.OfferExamination.ToString();
                }

                txtProviderName.Text = providerModel4WS.providerName;
                txtProviderNumber.Text = providerModel4WS.providerNo;

                if (providerModel4WS.refLicenseProfessionalModel != null)
                {
                    DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, providerModel4WS.refLicenseProfessionalModel.countyCode, false, true, true);

                    txtLicenseNbr.Text = providerModel4WS.refLicenseProfessionalModel.stateLicense;
                    txtAddress1.Text = providerModel4WS.refLicenseProfessionalModel.address1;
                    txtAddress2.Text = providerModel4WS.refLicenseProfessionalModel.address2;
                    txtAddress3.Text = providerModel4WS.refLicenseProfessionalModel.address3;
                    txtCity.Text = providerModel4WS.refLicenseProfessionalModel.city;
                    txtState.Text = providerModel4WS.refLicenseProfessionalModel.state;
                    txtZip.Text = providerModel4WS.refLicenseProfessionalModel.zip;
                    txtFax.Text = providerModel4WS.refLicenseProfessionalModel.fax;
                    txtFax.CountryCodeText = providerModel4WS.refLicenseProfessionalModel.faxCountryCode;
                    txtPhone1.Text = providerModel4WS.refLicenseProfessionalModel.phone1;
                    txtPhone1.CountryCodeText = providerModel4WS.refLicenseProfessionalModel.phone1CountryCode;
                    txtPhone2.Text = providerModel4WS.refLicenseProfessionalModel.phone2;
                    txtPhone2.CountryCodeText = providerModel4WS.refLicenseProfessionalModel.phone2CountryCode;
                }

                //education search form.
                if (providerModel4WS.refEduModel != null && providerModel4WS.refEduModel.Length > 0)
                {
                    txtName.Text = providerModel4WS.refEduModel[0].refEducationName;
                    txtDegree.Text = providerModel4WS.refEduModel[0].degree;
                }

                //continuing education search form
                if (providerModel4WS.refContEducations != null && providerModel4WS.refContEducations.Length > 0)
                {
                    txtCourseName.Text = providerModel4WS.refContEducations[0].contEduName;
                    XRefContinuingEducationAppTypeModel4WS[] xRefContEducationAppTypes = providerModel4WS.refContEducations[0].refContEduAppTypeModels;
                    
                    if (xRefContEducationAppTypes != null && xRefContEducationAppTypes.Length > 0)
                    {
                        txtTotalHours.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(providerModel4WS.refContEducations[0].refContEduAppTypeModels[0].requiredHours);
                    }
                }

                //examination search form
                if (providerModel4WS.refExaminations != null && providerModel4WS.refExaminations.Length > 0)
                {
                    txtExaminationName.Text = providerModel4WS.refExaminations[0].examName;
                }
            }

            if (educationSearchModel != null && educationSearchModel.capTypeModel != null)
            {
                ddlModuleSearchEdu.SelectedValue = educationSearchModel.capTypeModel.moduleName;
                DropDownListBindUtil.BindDDL(CapUtil.GetPermitTypesByModuleName(GetSelectedModule()), ddlRecordTypeSearchEdu);
                ddlRecordTypeSearchEdu.SelectedValue = CAPHelper.GetCapTypeValue(educationSearchModel.capTypeModel);
            }
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        public void ApplyRegionalSetting()
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, false, ddlCountry);
        }

        /// <summary>
        /// Initial search by education..
        /// </summary>
        public void InitProviderForm()
        {
            DropDownListBindUtil.BindProviderType(lblProviderType);
            DropDownListBindUtil.BindModules(ddlModuleSearchEdu, false);
            DropDownListBindUtil.BindDDL(null, ddlRecordTypeSearchEdu, true);
            txtLicenseNbr.Text = string.Empty;
            txtProviderName.Text = string.Empty;
            txtProviderNumber.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtAddress3.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZip.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtFax.CountryCodeText = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone1.CountryCodeText = string.Empty;
            txtPhone2.Text = string.Empty;
            txtPhone2.CountryCodeText = string.Empty;
            txtName.Text = string.Empty;
            txtDegree.Text = string.Empty;
            txtCourseName.Text = string.Empty;
            txtTotalHours.Text = string.Empty;
            txtExaminationName.Text = string.Empty;

            ControlUtil.ClearRegionalSetting(ddlCountry, true, string.Empty, null, string.Empty);
            ddlCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCountry.RegisterScripts();
        }

        /// <summary>
        /// Hide cap type selection section
        /// </summary>
        public void HideCAPType()
        {
            ddlModuleSearchEdu.ClearSelection();
            ddlRecordTypeSearchEdu.ClearSelection();            
        }

        /// <summary>
        /// Get provider model from page.
        /// </summary>
        /// <returns>a ProviderModel4WS</returns>
        public EducationSearchModel GetProviderModel()
        {
            EducationSearchModel educationSearchModel = new EducationSearchModel();

            //provider model.
            ProviderModel4WS providerModel = new ProviderModel4WS();
            providerModel.serviceProviderCode = ConfigManager.AgencyCode;
            providerModel.offerEducation = lblProviderType.SelectedValue == ProviderType.OfferEducation.ToString() ? ACAConstant.COMMON_Y : string.Empty;
            providerModel.offerContinuing = lblProviderType.SelectedValue == ProviderType.OfferContinuing.ToString() ? ACAConstant.COMMON_Y : string.Empty;
            providerModel.offerExamination = lblProviderType.SelectedValue == ProviderType.OfferExamination.ToString() ? ACAConstant.COMMON_Y : string.Empty;
            providerModel.providerName = txtProviderName.Text.Trim();
            providerModel.providerNo = txtProviderNumber.Text.Trim();

            RefLicenseProfessionalModel4WS refLicenseProfessionalModel = new RefLicenseProfessionalModel4WS();
            providerModel.refLicenseProfessionalModel = refLicenseProfessionalModel;

            refLicenseProfessionalModel.stateLicense = txtLicenseNbr.Text.Trim();
            refLicenseProfessionalModel.address1 = txtAddress1.Text.Trim();
            refLicenseProfessionalModel.address2 = txtAddress2.Text.Trim();
            refLicenseProfessionalModel.address3 = txtAddress3.Text.Trim();
            refLicenseProfessionalModel.countyCode = ddlCountry.SelectedValue;
            refLicenseProfessionalModel.city = txtCity.Text.Trim();
            refLicenseProfessionalModel.state = txtState.Text.Trim();
            refLicenseProfessionalModel.zip = txtZip.GetZip(ddlCountry.SelectedValue.Trim());
            refLicenseProfessionalModel.phone1 = txtPhone1.GetPhone(ddlCountry.SelectedValue.Trim());
            refLicenseProfessionalModel.phone1CountryCode = txtPhone1.CountryCodeText.Trim();
            refLicenseProfessionalModel.phone2 = txtPhone2.GetPhone(ddlCountry.SelectedValue.Trim());
            refLicenseProfessionalModel.phone2CountryCode = txtPhone2.CountryCodeText.Trim();
            refLicenseProfessionalModel.fax = txtFax.GetPhone(ddlCountry.SelectedValue.Trim());
            refLicenseProfessionalModel.faxCountryCode = txtFax.CountryCodeText.Trim();

            //education model
            RefEducationModel4WS refEduModel = new RefEducationModel4WS();
            refEduModel.degree = txtDegree.Text.Trim();
            refEduModel.refEducationName = txtName.Text.Trim();
            providerModel.refEduModel = new RefEducationModel4WS[] { refEduModel };

            //continucation education model
            RefContinuingEducationModel4WS refContEduModel = new RefContinuingEducationModel4WS();
            refContEduModel.serviceProviderCode = ConfigManager.AgencyCode;
            refContEduModel.contEduName = txtCourseName.Text.Trim();
            XRefContinuingEducationAppTypeModel4WS xRefContinuingEducationAppType = new XRefContinuingEducationAppTypeModel4WS();
            xRefContinuingEducationAppType.requiredHours = txtTotalHours.GetInvariantDoubleText().Trim();
            refContEduModel.refContEduAppTypeModels = new XRefContinuingEducationAppTypeModel4WS[] { xRefContinuingEducationAppType };
            providerModel.refContEducations = new RefContinuingEducationModel4WS[] { refContEduModel };

            //Examination model.
            RefExaminationModel4WS refExamination = new RefExaminationModel4WS();
            refExamination.serviceProviderCode = ConfigManager.AgencyCode;
            refExamination.examName = txtExaminationName.Text.Trim();
            providerModel.refExaminations = new RefExaminationModel4WS[] { refExamination };

            educationSearchModel.providerModel4WS = providerModel;
            
            // Get selected CAP Type 
            educationSearchModel.capTypeModel = CapUtil.GetCAPTypeModelByString(GetSelectedModule(), ddlRecordTypeSearchEdu.SelectedValue, ddlRecordTypeSearchEdu.SelectedItem.Text);
            
            return educationSearchModel;
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        public void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Update Permit Type List when Module Name is changed.
        /// </summary>
        /// <param name="sender">Module Dropdown List Control</param>
        /// <param name="e">Event Argument</param>
        protected void Module_IndexChanged(object sender, EventArgs e)
        {
            ddlRecordTypeSearchEdu.Items.Clear();
            DropDownListBindUtil.BindDDL(CapUtil.GetPermitTypesByModuleName(GetSelectedModule()), ddlRecordTypeSearchEdu);

            if (ddlModuleSearchEdu.SelectedIndex != 0)
            {
                ddlRecordTypeSearchEdu.Required = true;
            }

            Page.FocusElement(ddlModuleSearchEdu.ClientID);
        }

        /// <summary>
        /// Raises the initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // set the validation
            SetViewID();

            ddlCountry.BindItems();
            ddlCountry.SetCountryControls(txtZip, txtState, txtPhone1, txtPhone2, txtFax);

            ControlBuildHelper.AddValidationForStandardFields(ViewId, ModuleName, Permission, Controls);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            foreach (Control ctrl in phContent.Controls)
            {
                ctrl.Visible = true;
            }

            ApplyRegionalSetting();

            if (AppSession.IsAdmin)
            {
                ddlModuleSearchEdu.AutoPostBack = false;
            }

            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// Render event method.
        /// </summary>
        /// <param name="writer">writer string.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsPostBack && !AppSession.IsAdmin)
            {
                SetCurrentCityAndState();
            }

            base.Render(writer);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Gets Selected Module.
        /// </summary>
        /// <returns>Selected Module</returns>
        private string GetSelectedModule()
        {
            string selectedModule = string.Empty;

            if (ddlModuleSearchEdu.SelectedItem != null)
            {
                selectedModule = ddlModuleSearchEdu.SelectedItem.Value;
            }

            return selectedModule;
        }

         /// <summary>
        /// Set current view id.
        /// </summary>
        private void SetViewID()
        {
            if (SearchType == GeneralInformationSearchType.Search4Provider)
            {
                ViewId = GviewID.SearchForProvider;
            }
            else
            {
                ViewId = GviewID.SearchForEducationAndExam;
            }
        }

        #endregion Private Methods
    }
}