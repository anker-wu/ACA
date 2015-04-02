#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationDetailEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationDetailEdit.ascx.cs 277411 2014-08-15 01:56:26Z ACHIEVO\james.shi $.
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
    /// It providers add or edit continuing Education information in daily side.
    /// </summary>
    public partial class ContinuingEducationDetailEdit : LicenseCertificationBasePage
    {
        #region Fields

        /// <summary>
        /// Indicate the default index value when no continuing education is selected 
        /// in continuing education list.
        /// </summary>
        private const int CONTEDUCATION_ROWINDEX_DEFAULT_VALUE = -1;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuingEducationDetailEdit" /> class.
        /// </summary>
        public ContinuingEducationDetailEdit()
            : base(GviewID.ContinuingEducationEdit)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this page is open from reference contact edit page.
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
                    ViewId = GviewID.RefContactContinuingEducationEdit;
                }
            }
        }

        /// <summary>
        /// Gets the ALL continuing education string.
        /// </summary>
        /// <value>
        /// The ALL continuing education string.
        /// </value>
        protected string RefContinuingEducationNameValueString
        {
            get
            {
                StringBuilder jsonString = new StringBuilder();

                if (!AppSession.IsAdmin)
                {
                    IRefContinuingEducationBll refContEducationBLL = ObjectFactory.GetObject<IRefContinuingEducationBll>();
                    IEnumerable<MapEntry4WS> educations = refContEducationBLL.GetRefContEducationListByName(CapAgencyCode, null);

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
                    base.Permission.permissionLevel = GViewConstant.SECTION_CONTINUING_EDUCATION;
                }

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected contact record index.
        /// </summary>
        private int SelectedIndex
        {
            get
            {
                return ViewState["SelectedIndex"] == null ? CONTEDUCATION_ROWINDEX_DEFAULT_VALUE : Convert.ToInt32(ViewState["SelectedIndex"]);
            }

            set
            {
                ViewState["SelectedIndex"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear value in continuing education form.
        /// </summary>
        public void ClearContEducationForm()
        {
            ControlUtil.ClearValue(this, null);
            
            ControlUtil.ApplyRegionalSetting(false, false, true, true, ddlCountryCode);
            txtCompletedDate.Text2 = null;
            txtRequired.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
            txtFinalScore.DisplayType = GradingStyle.GradingStyleType.none;

            //reset rowIndex to -1
            SelectedIndex = CONTEDUCATION_ROWINDEX_DEFAULT_VALUE;

            //let continuing education field can edit.
            txtContEducationName.Enabled = true;
            txtContEducationName.ImageEnabled = true;

            DisplayGenericTemplate(string.Empty);
            hdnOrginalEduName.Value = string.Empty;
            hdnRefContEducationSeqNumber.Value = string.Empty;
            base.Permission.permissionValue = string.Empty;
            hdnContEducationSeqNumber.Value = string.Empty;
        }

        /// <summary>
        /// Let continuing education form enabled.
        /// </summary>
        public void EnableContEducationForm()
        {
            string[] filterControlIDs = null;

            filterControlIDs = genericTemplate.ReadOnlyControlIds != null && genericTemplate.ReadOnlyControlIds.Count > 0
                ? genericTemplate.ReadOnlyControlIds.ToArray()
                : null;

            EnableEdit(this, filterControlIDs);

            genericTemplate.IsReadOnly = false;
        }

        /// <summary>
        /// Let continuing education form disabled.
        /// </summary>
        public void DisableContEducationForm()
        {
            DisableEdit(this, null);
            genericTemplate.IsReadOnly = true;
        }

        /// <summary>
        /// Fill continuing education detail form.
        /// </summary>
        /// <param name="contEducation">continuing education</param>
        public void FillContEducation(ContinuingEducationModel4WS contEducation)
        {
            if (IsFromRefContact)
            {
                txtRequired.IsHidden = true;
            }

            if (contEducation == null)
            {
                txtRequired.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
                txtApproved.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
                SetCurrentCityAndState();
                DisplayGenericTemplate(string.Empty);
                return;
            }

            txtContEducationName.Text = contEducation.contEduName;
            txtProviderName.Text = contEducation.providerName;
            txtProviderNumber.Text = contEducation.providerNo;
            txtClass.Text = contEducation.className;
            txtCompletedDate.Text2 = contEducation.dateOfClass;
            txtClassHours.DoubleValue = contEducation.hoursCompleted;
            txtFinalScore.DisplayTypeString = contEducation.gradingStyle;
            txtFinalScore.Value = contEducation.finalScore == null ? null : contEducation.finalScore.ToString();
            txtComments.Text = contEducation.comments;
            txtRequired.Text = EducationUtil.ConvertRequiredField2Display(contEducation.requiredFlag);
            txtApproved.Text = ModelUIFormat.FormatYNLabel(contEducation.approvedFlag, true);
            hdnContEducationSeqNumber.Value = contEducation.continuingEducationPKModel.contEduNbr.ToString();
            hdnOrginalEduName.Value = contEducation.contEduName;
            hdnPassingScore.Value = contEducation.passingScore;

            SelectedIndex = contEducation.RowIndex;

            //here has been apply the regional setting, do not need apply in InitCountry method
            IsNeedInitRegional = false;

            if (contEducation.providerDetailModel != null)
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountryCode, contEducation.providerDetailModel.countryCode, false, true, false);
                txtAddress1.Text = contEducation.providerDetailModel.address1;
                txtAddress2.Text = contEducation.providerDetailModel.address2;
                txtAddress3.Text = contEducation.providerDetailModel.address3;

                txtZip.Text = ModelUIFormat.FormatZipShow(contEducation.providerDetailModel.zip, contEducation.providerDetailModel.countryCode, false);
                txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(contEducation.providerDetailModel.phone1, contEducation.providerDetailModel.countryCode);
                txtPhone1.CountryCodeText = contEducation.providerDetailModel.phone1CountryCode;
                txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(contEducation.providerDetailModel.phone2, contEducation.providerDetailModel.countryCode);
                txtPhone2.CountryCodeText = contEducation.providerDetailModel.phone2CountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(contEducation.providerDetailModel.fax, contEducation.providerDetailModel.countryCode);
                txtFax.CountryCodeText = contEducation.providerDetailModel.faxCountryCode;
                txtEmail.Text = contEducation.providerDetailModel.email;
            }
            else
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountryCode, string.Empty, false, true, false);
            }

            txtCity.Text = contEducation.providerDetailModel == null ? string.Empty : contEducation.providerDetailModel.city;
            txtState.Text = contEducation.providerDetailModel == null ? string.Empty : contEducation.providerDetailModel.state;

            // if continuing education is auto loaded from admin side, continuing education name can't be modified.
            if (contEducation.requiredFlag.Equals(ACAConstant.COMMON_Y))
            {
                txtContEducationName.DisableEdit();
            }
            else
            {
                txtContEducationName.EnableEdit();
            }

            string refContEduId = string.Empty;
            TemplateModel templateModel = contEducation.template;
            genericTemplate.ResetControl();

            if (!string.IsNullOrEmpty(contEducation.RefConEduNbr))
            {
                refContEduId = contEducation.RefConEduNbr;

                if (templateModel == null)
                {
                    //when entity is reference cont education, need sent name to get generic template.
                    DisplayGenericTemplate(refContEduId, string.IsNullOrEmpty(refContEduId) ? null : txtContEducationName.Text);
                }
                else
                {
                    genericTemplate.Display(templateModel);
                }
            }
            else if (contEducation.continuingEducationPKModel != null)
            {
                RefContinuingEducationModel4WS refContEducation =
                    EducationUtil.GetRefContinuingEducationModel(
                        contEducation.continuingEducationPKModel.serviceProviderCode, contEducation.contEduName);

                if (refContEducation != null)
                {
                    refContEduId = refContEducation.refContEduNbr.ToString();
                }

                genericTemplate.Display(templateModel);
            }

            SetPermissionValue(refContEduId);
            hdnRefContEducationSeqNumber.Value = refContEduId;
        }

        /// <summary>
        /// Get continuing education model for save.
        /// </summary>
        /// <returns>continuing education model</returns>
        public ContinuingEducationModel4WS GetContEducation()
        {
            ContinuingEducationModel4WS contEducation = new ContinuingEducationModel4WS();
            contEducation.continuingEducationPKModel = new ContinuingEducationPKModel4WS();

            if (!string.IsNullOrEmpty(hdnContEducationSeqNumber.Value))
            {
                contEducation.continuingEducationPKModel.contEduNbr = long.Parse(hdnContEducationSeqNumber.Value);
            }

            contEducation.contEduName = txtContEducationName.Text.Trim();
            contEducation.providerName = txtProviderName.Text.Trim();
            contEducation.providerNo = txtProviderNumber.Text.Trim();
            contEducation.className = txtClass.Text.Trim();
            string completeDate = txtCompletedDate.Text.Trim();

            if (!string.IsNullOrEmpty(completeDate))
            {
                contEducation.dateOfClass = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(completeDate);
            }
            else
            {
                contEducation.dateOfClass = null;
            }

            double? classHours = txtClassHours.DoubleValue;

            if (classHours != null)
            {
                contEducation.hoursCompleted = classHours.Value;
            }

            if (!string.IsNullOrEmpty(txtFinalScore.Value))
            {
                contEducation.finalScore = txtFinalScore.Value;
            }

            if (!string.IsNullOrEmpty(hdnPassingScore.Value))
            {
                contEducation.passingScore = hdnPassingScore.Value;
            }

            contEducation.gradingStyle = txtFinalScore.DisplayTypeString;
            contEducation.comments = txtComments.Text.Trim();

            contEducation.requiredFlag = EducationUtil.ConvertRequiredFeild2Save(txtRequired.Text.Trim()); 

            contEducation.RowIndex = SelectedIndex;

            contEducation.continuingEducationPKModel.serviceProviderCode = ConfigManager.AgencyCode;
            ProviderDetailModel4WS providerDetail = new ProviderDetailModel4WS();

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

            contEducation.providerDetailModel = providerDetail;
            AuditModel4WS auditModel = new AuditModel4WS();
            auditModel.auditID = AppSession.User.PublicUserId;
            auditModel.auditDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(DateTime.Now);
            auditModel.auditStatus = ACAConstant.VALID_STATUS;
            contEducation.FromCapAssociate = false;
            contEducation.auditModel = auditModel;

            contEducation.RefConEduNbr = hdnRefContEducationSeqNumber.Value;
            contEducation.template = genericTemplate.GetTemplateModel(true);

            return contEducation;
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
        /// Initialize control event.
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ddlCountryCode.BindItems();
            ddlCountryCode.SetCountryControls(txtZip, txtState, txtFax, txtPhone1, txtPhone2);

            // Add validate final score field.
            txtFinalScore.SectionId = ViewId;
            txtFinalScore.Permission = Permission;

            AccelaFormDesignerPlaceHolder = phContent;
            GenericTemplateEditControl = genericTemplate;
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object</param>
        /// <param name="e">event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            IsFromRefContact = Request.QueryString.AllKeys.Contains("contactSeqNbr");

            if (AppSession.IsAdmin)
            {
                txtProviderName.ImageEnabled = false;
                txtContEducationName.ImageEnabled = false;
                DisplayGenericTemplate(string.Empty);
            }

            InitCountry(IsPostBack, ddlCountryCode, txtZip, txtState, txtFax, txtPhone1, txtPhone2);

            // Get image logo.
            txtProviderName.ImageUrl = ImageUtil.GetImageURL("Search.gif");
            txtContEducationName.ImageUrl = ImageUtil.GetImageURL("Search.gif");

            if (IsFromRefContact)
            {
                txtApproved.Attributes.Add("IsDBRequired", "true");
            }

            if (IsPostBack)
            {
                string refContEduId = string.Empty;

                if ((updatePanel.UniqueID + POST_BACK_BY_CHANGE_CONTEDU).Equals(Request.Form[Page.postEventSourceID]))
                {
                    refContEduId = Request.Form[Page.postEventArgumentID];

                    if (string.IsNullOrEmpty(refContEduId))
                    {
                        string contEducationName = txtContEducationName.Text.Trim();

                        RefContinuingEducationModel4WS contEducation =
                            EducationUtil.GetRefContinuingEducationModel(CapAgencyCode, contEducationName);

                        if (contEducation != null)
                        {
                            refContEduId = contEducation.refContEduNbr.ToString();
                            txtContEducationName.Text = contEducation.contEduName;
                            txtRequired.Text = EducationUtil.GetContinuingEducationRequireValue(contEducation, ModuleName);
                            hdnPassingScore.Value = contEducation.passingScore;
                            hdnOrginalEduName.Value = contEducation.contEduName;
                        }
                        else
                        {
                            txtRequired.Text = GetTextByKey("ACA_Common_No");
                            txtFinalScore.DisplayTypeString = GradingStyle.GradingStyleType.none.ToString();
                        }
                    }

                    //when entity is reference cont education, need sent name to get generic template.
                    DisplayGenericTemplate(refContEduId, string.IsNullOrEmpty(refContEduId) ? null : txtContEducationName.Text);
                    hdnRefContEducationSeqNumber.Value = refContEduId;
                    Page.FocusElement(txtContEducationName.ClientID);
                }
                else
                {
                    refContEduId = hdnRefContEducationSeqNumber.Value;

                    if ((updatePanel.UniqueID + POST_BACK_BY_COUNTRY).Equals(Request.Form[Page.postEventSourceID]))
                    {
                        Page.FocusElement(txtProviderName.ClientID);
                    }
                }

                SetPermissionValue(refContEduId);
            }
        }

        /// <summary>
        /// Previous render event.
        /// </summary>
        /// <param name="e">previous render event argument</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Add asterisk for required fields according to configuration in ACA admin. 
            ControlBuildHelper.AddValidationForStandardFields(ViewId, ModuleName, Permission, Controls);
            InitFormDesignerPlaceHolder(phContent);
        }

        #endregion Methods
    }
}
