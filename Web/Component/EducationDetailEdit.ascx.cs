#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationDetailEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationDetailEdit.ascx.cs 277411 2014-08-15 01:56:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AppCode.LicenseCertification;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Add or Edit daily Education information.
    /// </summary>
    public partial class EducationDetailEdit : LicenseCertificationBasePage
    {
        #region Fields

        /// <summary>
        /// Method name for fill provider information to education.
        /// </summary>
        protected const string METHOD_NAME = "FillEducationProviderInfo";

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the EducationDetailEdit class
        /// </summary>
        public EducationDetailEdit()
            : base(GviewID.EducationEdit)
        {
        }

        #region properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is used in reference contact edit page.
        /// </summary>
        public bool IsFromRefContact
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsFromRefContact"]);
            }

            set
            {
                ViewState["IsFromRefContact"] = value;

                if (value)
                {
                    ViewId = GviewID.RefContactEducationEdit;
                }
            }
        }

        /// <summary>
        /// Gets or Sets Permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                if (base.Permission == null)
                {
                    base.Permission = new GFilterScreenPermissionModel4WS();
                    base.Permission.permissionLevel = GViewConstant.SECTION_EDUCATOIN;
                }

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets the ALL education string.
        /// </summary>
        /// <value>
        /// The ALL education string.
        /// </value>
        protected string RefEducationNameValueString
        {
            get
            {
                StringBuilder jsonString = new StringBuilder();

                if (!AppSession.IsAdmin)
                {
                    IRefEducationBll refEducationBLL = ObjectFactory.GetObject<IRefEducationBll>();
                    IEnumerable<MapEntry4WS> educations = refEducationBLL.GetRefEducationListByName(CapAgencyCode, null);

                    if (educations != null)
                    {
                        // Sort by Education name.
                        educations = educations.OrderBy(o => o.value);

                        foreach (MapEntry4WS education in educations)
                        {
                            if (!string.IsNullOrEmpty(jsonString.ToString()))
                            {
                                jsonString.Append(ACAConstant.SPLIT_CHAR);
                            }

                            jsonString.Append(ScriptFilter.EncodeJson((string)education.value));
                            jsonString.Append(ACAConstant.SPLIT_CHAR2);
                            jsonString.Append(Convert.ToString(education.key));
                        }
                    }
                }

                return jsonString.ToString().Replace("\f", "\\f").Replace("\b", "\\b");
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// clear value of controls in education edit form.
        /// </summary>
        public void ClearEducationForm()
        {
            ControlUtil.ClearValue(this, null);

            ControlUtil.ApplyRegionalSetting(false, false, true, true, ddlCountryCode);
            txtRequired.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
            DisplayGenericTemplate(string.Empty);

            hdnOrginalEduName.Value = string.Empty;
            hdnRefEducationSeq.Value = string.Empty;
            base.Permission.permissionValue = string.Empty;
            hdnEducationSeqNumber.Value = string.Empty;
        }

        /// <summary>
        /// Set all of controls in education form to disabled. 
        /// </summary>
        public void DisableEducationForm()
        {
            DisableEdit(this, null);
            genericTemplate.IsReadOnly = true;
        }

        /// <summary>
        /// All of controls in education form can editable.
        /// </summary>
        public void EnableEducationForm()
        {
            string[] filterControlIDs = null;

            filterControlIDs = genericTemplate.ReadOnlyControlIds != null && genericTemplate.ReadOnlyControlIds.Count > 0
                ? genericTemplate.ReadOnlyControlIds.ToArray()
                : null;

            EnableEdit(this, filterControlIDs);

            genericTemplate.IsReadOnly = false;
            txtRequired.Enabled = false;
            txtProviderNumber.Enabled = true;
        }

        /// <summary>
        /// Set education detail to education detail form.
        /// </summary>
        /// <param name="education">education model</param>
        public void SetEducationDetailInfo(EducationModel4WS education)
        {
            if (IsFromRefContact)
            {
                txtRequired.IsHidden = true;
            }

            if (education == null)
            {
                SetCurrentCityAndState();
                txtRequired.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
                txtApproved.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
                DisplayGenericTemplate(string.Empty);
                return;
            }

            txtWithImgProviderName.Text = education.providerName;
            txtProviderNumber.Text = education.providerNo;
            txtWithImgMajorName.Text = education.educationName;
            DropDownListBindUtil.SetSelectedValue(ddlDegree, education.degree);
            txtAttended.Text = education.yearAttended;
            txtGraduated.Text = education.yearGraduated;

            //here has been apply the regional setting, do not need apply in InitCountry method
            IsNeedInitRegional = false;

            if (education.providerDetailModel != null)
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountryCode, education.providerDetailModel.countryCode, false, true, false);
                txtAddress1.Text = education.providerDetailModel.address1;
                txtAddress2.Text = education.providerDetailModel.address2;
                txtAddress3.Text = education.providerDetailModel.address3;
                txtCity.Text = education.providerDetailModel.city;
                txtState.Text = education.providerDetailModel.state;

                txtZip.Text = ModelUIFormat.FormatZipShow(education.providerDetailModel.zip, education.providerDetailModel.countryCode, false);
                txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(education.providerDetailModel.phone1, education.providerDetailModel.countryCode);
                txtPhone1.CountryCodeText = education.providerDetailModel.phone1CountryCode;
                txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(education.providerDetailModel.phone2, education.providerDetailModel.countryCode);
                txtPhone2.CountryCodeText = education.providerDetailModel.phone2CountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(education.providerDetailModel.fax, education.providerDetailModel.countryCode);
                txtFax.CountryCodeText = education.providerDetailModel.faxCountryCode;
                txtEmail.Text = education.providerDetailModel.email;
            }
            else
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountryCode, string.Empty, false, true, false);
            }

            txtRequired.Text = EducationUtil.ConvertRequiredField2Display(education.requiredFlag);
            txtApproved.Text = ModelUIFormat.FormatYNLabel(education.approvedFlag, true);
            txtComments.Text = education.comments;
            hdnEducationSeqNumber.Value = education.educationPKModel.educationNbr.ToString();
            hdnOrginalEduName.Value = education.educationName;

            if (education.requiredFlag.Equals(ACAConstant.COMMON_Y))
            {
                txtWithImgMajorName.DisableEdit();
            }
            else
            {
                txtWithImgMajorName.EnableEdit();
            }

            txtRequired.Enabled = false;

            string refEduId = string.Empty;
            TemplateModel templateModel = education.template;
            genericTemplate.ResetControl();

            if (!string.IsNullOrEmpty(education.RefEduNbr))
            {
                refEduId = education.RefEduNbr;

                if (templateModel == null)
                {
                    DisplayGenericTemplate(refEduId);
                }
                else
                {
                    genericTemplate.Display(templateModel);
                }
            }
            else if (education.educationPKModel != null)
            {
                RefEducationModel4WS refEducation =
                    EducationUtil.GetRefEducationModel(
                        education.educationPKModel.serviceProviderCode, education.educationName);

                if (refEducation != null)
                {
                    refEduId = refEducation.refEducationNbr.ToString();
                }

                genericTemplate.Display(education.template);
            }

            SetPermissionValue(refEduId);
            hdnRefEducationSeq.Value = refEduId;
        }

        /// <summary>
        /// Get education model according to public input in education detail form.
        /// </summary>
        /// <returns>education model.</returns>
        public EducationModel4WS GetEducationModel()
        {
            EducationModel4WS educationModel = new EducationModel4WS();
            educationModel.educationPKModel = new EducationPKModel4WS();

            if (!string.IsNullOrEmpty(hdnEducationSeqNumber.Value))
            {
                educationModel.educationPKModel.educationNbr = long.Parse(hdnEducationSeqNumber.Value);
            }

            educationModel.educationName = txtWithImgMajorName.Text;
            educationModel.yearAttended = txtAttended.Text;
            educationModel.yearGraduated = txtGraduated.Text;
            educationModel.comments = txtComments.Text;
            educationModel.degree = ddlDegree.SelectedValue;
            educationModel.requiredFlag = EducationUtil.ConvertRequiredFeild2Save(txtRequired.Text.Trim());
            educationModel.educationPKModel.serviceProviderCode = CapAgencyCode;
            educationModel.providerNo = txtProviderNumber.Text;
            educationModel.providerName = txtWithImgProviderName.Text;
            educationModel.FromCapAssociate = false;

            ProviderDetailModel4WS providerDetail = new ProviderDetailModel4WS();
            AuditModel4WS auditModel = new AuditModel4WS();
            auditModel.auditID = AppSession.User.PublicUserId;
            auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            auditModel.auditStatus = ACAConstant.VALID_STATUS;

            providerDetail.countryCode = ddlCountryCode.SelectedValue;
            providerDetail.address1 = txtAddress1.Text;
            providerDetail.address2 = txtAddress2.Text;
            providerDetail.address3 = txtAddress3.Text;
            providerDetail.city = txtCity.Text;
            providerDetail.state = txtState.Text;

            providerDetail.zip = txtZip.GetZip(ddlCountryCode.SelectedValue);
            providerDetail.phone1 = txtPhone1.GetPhone(ddlCountryCode.SelectedValue);
            providerDetail.phone1CountryCode = txtPhone1.CountryCodeText;
            providerDetail.phone2 = txtPhone2.GetPhone(ddlCountryCode.SelectedValue);
            providerDetail.phone2CountryCode = txtPhone2.CountryCodeText;
            providerDetail.fax = txtFax.GetPhone(ddlCountryCode.SelectedValue);
            providerDetail.faxCountryCode = txtFax.CountryCodeText;
            providerDetail.email = txtEmail.Text;

            educationModel.providerDetailModel = providerDetail;
            educationModel.auditModel = auditModel;
            educationModel.template = genericTemplate.GetTemplateModel(true);
            educationModel.RefEduNbr = hdnRefEducationSeq.Value;

            return educationModel;
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        public void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        /// <summary>
        /// on initial event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
            {
                //Bind degree type dropdown list.
                DropDownListBindUtil.BindDegreeType(ddlDegree);
            }

            ddlCountryCode.BindItems();
            ddlCountryCode.SetCountryControls(txtZip, txtState, txtFax, txtPhone1, txtPhone2);

            AccelaFormDesignerPlaceHolder = phContent;
            GenericTemplateEditControl = genericTemplate;
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                txtWithImgProviderName.ImageEnabled = false;
                txtWithImgMajorName.ImageEnabled = false;
                DisplayGenericTemplate(string.Empty);
            }

            InitCountry(IsPostBack, ddlCountryCode, txtZip, txtState, txtFax, txtPhone1, txtPhone2);

            txtWithImgProviderName.ImageUrl = ImageUtil.GetImageURL("Search.gif");
            txtWithImgMajorName.ImageUrl = ImageUtil.GetImageURL("Search.gif");

            if (IsFromRefContact)
            {
                txtApproved.Attributes.Add("IsDBRequired", "true");
            }

            if (IsPostBack)
            {
                string eduRefId = string.Empty;

                if ((updatePanel.UniqueID + POST_BACK_BY_CHANGE_NAME).Equals(Request.Form[Page.postEventSourceID]))
                {
                    eduRefId = Request.Form[Page.postEventArgumentID];

                    if (string.IsNullOrEmpty(eduRefId))
                    {
                        string eduName = txtWithImgMajorName.Text.Trim();
                        RefEducationModel4WS education = EducationUtil.GetRefEducationModel(CapAgencyCode, eduName);

                        if (education != null)
                        {
                            eduRefId = education.refEducationNbr.ToString();
                            txtWithImgMajorName.Text = education.refEducationName;
                            DropDownListBindUtil.SetSelectedValue(ddlDegree, education.degree);
                            txtRequired.Text = EducationUtil.GetEducationRequireValue(education, ModuleName);
                            hdnOrginalEduName.Value = education.refEducationName;
                        }
                        else
                        {
                            txtRequired.Text = GetTextByKey("ACA_Common_No");
                        }
                    }

                    DisplayGenericTemplate(eduRefId);
                    hdnRefEducationSeq.Value = eduRefId;
                    Page.FocusElement(txtWithImgMajorName.ClientID);
                }
                else
                {
                    eduRefId = hdnRefEducationSeq.Value;

                    if ((updatePanel.UniqueID + POST_BACK_BY_COUNTRY).Equals(Request.Form[Page.postEventSourceID]))
                    {
                        Page.FocusElement(txtWithImgProviderName.ClientID);
                    }
                }

                SetPermissionValue(eduRefId);
            }
        }

        /// <summary>
        /// Run this event before render.
        /// </summary>
        /// <param name="e">PreRender event argument</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Add asterisk for required fields according to configuration in ACA admin.
            ControlBuildHelper.AddValidationForStandardFields(ViewId, ModuleName, Permission, Controls);
            InitFormDesignerPlaceHolder(phContent);
        }

        #endregion Methods
    }
}
