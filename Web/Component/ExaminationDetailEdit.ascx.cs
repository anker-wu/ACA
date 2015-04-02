#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationDetailEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExaminationDetailEdit.ascx.cs 140449 2009-07-23 05:50:15Z ACHIEVO\jackie.yu $.
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
using Accela.Web.Controls;
using AjaxControlToolkit;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Add or Edit Examination Info
    /// </summary>
    public partial class ExaminationDetailEdit : LicenseCertificationBasePage
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ExaminationDetailEdit class
        /// </summary>
        public ExaminationDetailEdit()
            : base(GviewID.ExaminationEdit)
        {
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// Examination Event.
        /// </summary>
        public event CommonEventHandler DataChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Examination Model
        /// </summary>
        public ExaminationModel DataSource
        {
            get
            {
                return (ExaminationModel)ViewState["ExaminationDataSource"];
            }

            set
            {
                ViewState["ExaminationDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsEditable"]);
            }

            set
            {
                ViewState["IsEditable"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact from external.
        /// </summary>
        public bool ContactIsFromExternal
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether used for cap detail or spear form.
        /// </summary>
        public bool ForCAPDetail
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets the URL whether for examination register.
        /// </summary>
        public long ExamNum
        {
            get
            {
                return Convert.ToInt32(ViewState["ExamNum"]);
            }

            set
            {
                ViewState["ExamNum"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the examination section position.
        /// </summary>
        public EducationOrExamSectionPosition ExaminationSectionPosition
        {
            get
            {
                return (EducationOrExamSectionPosition)ViewState["ExamSectionPosition"];
            }

            set
            {
                ViewState["ExamSectionPosition"] = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        ViewId = GviewID.RefContactExaminationEdit;
                        btnSave.LabelKey = "aca_contact_examination_edit_label_save_and_close";
                        btnCancel.LabelKey = "aca_contact_examination_edit_label_cancel";
                        btnCancel.OnClientClick = "parent.ACADialog.close();return false;";
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                        btnSave.LabelKey = "aca_examination_edit_label_save_and_close";
                        btnCancel.LabelKey = "examination_detail_cancel";
                        btnCancel.OnClientClick = "parent.ACADialog.close();return false;";
                        break;
                    case EducationOrExamSectionPosition.CapDetail:
                        btnSave.LabelKey = "aca_examination_edit_label_save_and_close";
                        btnCancel.LabelKey = "examination_detail_cancel";
                        break;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this control is used by account contact edit page.
        /// </summary>
        public bool IsFromAccountContactEdit
        {
            get
            {
                return ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            }
        }

        /// <summary>
        /// Gets the ALL examination string.
        /// </summary>
        /// <value>
        /// The ALL examination string.
        /// </value>
        protected string RefExaminationNameValueString
        {
            get
            {
                StringBuilder jsonString = new StringBuilder();

                if (!AppSession.IsAdmin)
                {
                    IRefExaminationBll refExaminationBll = ObjectFactory.GetObject<IRefExaminationBll>();
                    IEnumerable<MapEntry4WS> educations = refExaminationBll.GetRefExaminationListByName(CapAgencyCode, null);

                    if (educations != null)
                    {
                        // Sort by Education name.
                        educations = educations.OrderBy(o => o.value);

                        foreach (MapEntry4WS examination in educations)
                        {
                            if (!string.IsNullOrEmpty(jsonString.ToString()))
                            {
                                jsonString.Append(ACAConstant.SPLIT_CHAR);
                            }

                            jsonString.Append(ScriptFilter.EncodeJson((string)examination.value));
                            jsonString.Append(ACAConstant.SPLIT_CHAR2);
                            jsonString.Append(Convert.ToString(examination.key));
                        }
                    }
                }

                return jsonString.ToString().Replace("\f", "\\f").Replace("\b", "\\b");
            }
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                if (base.Permission == null)
                {
                    base.Permission = new GFilterScreenPermissionModel4WS();
                    base.Permission.permissionLevel = GViewConstant.SECTION_EXAMINATION;
                }

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        #endregion       

        #region Methods

        /// <summary>
        ///  Fill Examination Detail Filed by Examination Model
        /// </summary>
        public void Bind()
        {
            if (DataSource != null)
            {
                txtWithImgExaminationName.Text = DataSource.examName;
                txtWithImgProviderName.Text = DataSource.providerName;
                txtProviderNumber.Text = DataSource.providerNo;
                txtFinalScore.DisplayTypeString = DataSource.gradingStyle;
                txtFinalScore.Value = I18nNumberUtil.ConvertNumberToInvariantString(DataSource.finalScore);
                hdnPassingScore.Value = DataSource.passingScore == null ? string.Empty : DataSource.passingScore.ToString();
                txtComments.Text = DataSource.comments;
                hdnExamNbr.Value = DataSource.refExamSeq == null ? string.Empty : DataSource.refExamSeq.ToString();
                txtRequired.Text = EducationUtil.ConvertRequiredField2Display(DataSource.requiredFlag);
                txtRequired.Enabled = false;
                txtApproved.Text = ModelUIFormat.FormatYNLabel(DataSource.approvedFlag, true);
                actExaminationDate.Text = I18nDateTimeUtil.FormatToDateStringForUI(DataSource.examDate);
                hdnOrginalExamName.Value = DataSource.examName;

                if (!ForCAPDetail && !ACAConstant.COMMON_Y.Equals(DataSource.requiredFlag, StringComparison.OrdinalIgnoreCase))
                {
                    txtWithImgExaminationName.EnableEdit();
                }
                else
                {
                    txtWithImgExaminationName.DisableEdit();
                }

                if (ContactIsFromExternal)
                {
                    txtWithImgExaminationName.DisableEdit();
                }

                if (DataSource.startTime != null && DataSource.startTime.Value != null)
                {
                    startTime.TimeText = I18nDateTimeUtil.FormatToTimeStringForUI((DateTime)DataSource.startTime, false);
                }
                else
                {
                    startTime.TimeText = string.Empty;
                }

                if (DataSource.endTime != null && DataSource.endTime.Value != null)
                {
                    endTime.TimeText = I18nDateTimeUtil.FormatToTimeStringForUI((DateTime)DataSource.endTime, false);
                }
                else
                {
                    endTime.TimeText = string.Empty;
                }

                //here has been apply the regional setting, do not need apply in InitCountry method
                IsNeedInitRegional = false;

                //Fill Provider
                if (DataSource.examProviderDetailModel != null)
                {
                    DropDownListBindUtil.SetCountrySelectedValue(ddlCountryCode, DataSource.examProviderDetailModel.countryCode, false, true, false);
                    txtAddress1.Text = DataSource.examProviderDetailModel.address1;
                    txtAddress2.Text = DataSource.examProviderDetailModel.address2;
                    txtAddress3.Text = DataSource.examProviderDetailModel.address3;

                    txtZip.Text = ModelUIFormat.FormatZipShow(DataSource.examProviderDetailModel.zip, DataSource.examProviderDetailModel.countryCode, false);
                    txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(DataSource.examProviderDetailModel.phone1, DataSource.examProviderDetailModel.countryCode);
                    txtPhone1.CountryCodeText = DataSource.examProviderDetailModel.phone1CountryCode;
                    txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(DataSource.examProviderDetailModel.phone2, DataSource.examProviderDetailModel.countryCode);
                    txtPhone2.CountryCodeText = DataSource.examProviderDetailModel.phone2CountryCode;
                    txtFax.Text = ModelUIFormat.FormatPhone4EditPage(DataSource.examProviderDetailModel.fax, DataSource.examProviderDetailModel.countryCode);
                    txtFax.CountryCodeText = DataSource.examProviderDetailModel.faxCountryCode;
                    txtEmail.Text = DataSource.examProviderDetailModel.email;
                }
                else
                {
                    DropDownListBindUtil.SetCountrySelectedValue(ddlCountryCode, string.Empty, false, true, false);
                }

                txtCity.Text = DataSource.examProviderDetailModel == null ? string.Empty : DataSource.examProviderDetailModel.city;
                txtState.Text = DataSource.examProviderDetailModel == null ? string.Empty : DataSource.examProviderDetailModel.state;

                txtRosterID.Text = DataSource.userExamID;
                string examStatus = DataSource.examStatus;

                if (ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(DataSource.examStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    examStatus = ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING;
                }

                ddlExaminationStatus.SetValue(examStatus);

                string refExamId = hdnExamNbr.Value;
                SetPermissionValue(refExamId);
                genericTemplate.ResetControl();

                if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(DataSource.examStatus, StringComparison.InvariantCultureIgnoreCase) 
                    || (ExaminationSectionPosition == EducationOrExamSectionPosition.CapDetail && !IsEditable))
                {
                    genericTemplate.NeedValidate = false;
                }

                if (DataSource.template == null
                    && !string.IsNullOrEmpty(refExamId))
                {
                    DisplayGenericTemplate(refExamId);
                }
                else
                {
                    genericTemplate.Display(DataSource.template); 
                }

                if ((!IsEditable && !AppSession.IsAdmin) || ContactIsFromExternal)
                {
                    //Disable All Control in Examinatin Form
                    DisableExamination();
                }
            }
            else
            {
                txtApproved.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
                DisplayGenericTemplate(string.Empty);
            }

            if (IsFromAccountContactEdit)
            {
                txtRequired.IsHidden = true;
                ScriptManager.RegisterStartupScript(Page, GetType(), "disableStatus", "disableStatus();", true);
            }
        }

        /// <summary>
        /// set all value of controls in examination Detail form to empty.
        /// </summary>
        public void ClearExamination()
        {
            ControlUtil.ClearValue(this, null);

            ControlUtil.ApplyRegionalSetting(false, false, true, true, ddlCountryCode);

            txtRequired.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
            txtFinalScore.DisplayType = GradingStyle.GradingStyleType.none;
            txtWithImgExaminationName.Enabled = true;
            SetCurrentCityAndState();
            DataSource = null;
            
            DisplayGenericTemplate(string.Empty);
            hdnOrginalExamName.Value = string.Empty;
            hdnExamNbr.Value = string.Empty;
            base.Permission.permissionValue = string.Empty;
        }

        /// <summary>
        /// Set all of controls in examination form to disabled. 
        /// </summary>
        public void DisableExamination()
        {
            DisableEdit(this, null);

            genericTemplate.IsReadOnly = true;
            DisableExaminationButton();
        }

        /// <summary>
        /// Set All of controls in examination form to editable.
        /// </summary>
        public void EnableExamination()
        {
            genericTemplate.IsReadOnly = false;

            string[] filterControlIDs = null;

            filterControlIDs = genericTemplate.ReadOnlyControlIds != null && genericTemplate.ReadOnlyControlIds.Count > 0
                ? genericTemplate.ReadOnlyControlIds.ToArray()
                : null;

            EnableEdit(this, filterControlIDs);
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
        /// Page On Initial Event
        /// </summary>
        /// <param name="e">Event Args</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlCountryCode.BindItems();
            ddlCountryCode.SetCountryControls(txtZip, txtState, txtFax, txtPhone1, txtPhone2);

            //Set the time text mask. 
            DropDownListBindUtil.BindExaminationStatus(ddlExaminationStatus);

            AccelaFormDesignerPlaceHolder = phContent;
            GenericTemplateEditControl = genericTemplate;

            ddlExaminationStatus.AutoPostBack = !AppSession.IsAdmin;

            if (IsPostBack)
            {
                if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(Request.Form[ddlExaminationStatus.UniqueID], StringComparison.InvariantCultureIgnoreCase))
                {
                    genericTemplate.NeedValidate = false;
                }
            }
        }

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">sender Object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitCountry(IsPostBack, ddlCountryCode, txtZip, txtState, txtFax, txtPhone1, txtPhone2);
            
            //Bind ImageUrl of ImageControl
            txtWithImgExaminationName.ImageUrl = ImageUtil.GetImageURL("Search.gif");
            txtWithImgProviderName.ImageUrl = ImageUtil.GetImageURL("Search.gif");
            txtWithImgExaminationName.Validate = "required";
            ddlExaminationStatus.Required = true;
            actExaminationDate.Attributes.Add("onchange", "verifyExaminationDate();");

            // Add validate final score field.
            txtFinalScore.Permission = Permission;
            txtFinalScore.SectionId = ViewId;

            if (IsFromAccountContactEdit)
            {
                txtApproved.Attributes.Add("IsDBRequired", "true");
            }

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    // auto fill city and state.
                    SetCurrentCityAndState();

                    txtRequired.Text = BasePage.GetStaticTextByKey("ACA_Common_No");
                }
                else
                {
                    txtWithImgExaminationName.Enabled = false;
                    txtWithImgProviderName.Enabled = false;
                }

                Bind();
            }

            if ((!IsEditable && !AppSession.IsAdmin) || ContactIsFromExternal)
            {
                //Disable All Control in Examinatin Form
                DisableExamination();
            }

            if (IsPostBack)
            {
                string refExamId = string.Empty;
                string examName = txtWithImgExaminationName.Text.Trim();

                if ((detailPanel.UniqueID + EXAMINATION_POSTBACK_STRING).Equals(Request.Form[Page.postEventSourceID]))
                {
                    refExamId = Request.Form[Page.postEventArgumentID];

                    if (string.IsNullOrEmpty(refExamId))
                    {
                        IRefExaminationBll refExaminationBll = ObjectFactory.GetObject<IRefExaminationBll>();
                        RefExaminationModel4WS[] examinations =
                            refExaminationBll.GetRefExaminationList(
                                new RefExaminationModel4WS() { serviceProviderCode = CapAgencyCode, examName = examName });

                        RefExaminationModel4WS examination = examinations != null && examinations.Length > 0
                                                                 ? examinations.FirstOrDefault(
                                                                     o =>
                                                                     examName.Equals(
                                                                         o.examName,
                                                                         StringComparison.InvariantCultureIgnoreCase))
                                                                 : null;

                        if (examination != null)
                        {
                            refExamId = examination.refExamNbr.ToString();
                            txtWithImgExaminationName.Text = examination.examName;
                            txtRequired.Text = ExaminationUtil.GetExamationRequiredStatus(examination, ModuleName);
                            hdnPassingScore.Value = examination.passingScore;
                            hdnOrginalExamName.Value = examination.examName;
                            txtFinalScore.DisplayTypeString = examination.gradingStyle;

                            ChangeGradingStyleByProvider(examination.examName);
                        }
                        else
                        {
                            txtRequired.Text = GetTextByKey("ACA_Common_No");
                            txtFinalScore.DisplayTypeString = GradingStyle.GradingStyleType.none.ToString();
                        }
                    }

                    DisplayGenericTemplate(refExamId);
                    hdnExamNbr.Value = refExamId;
                    Page.FocusElement(txtWithImgExaminationName.ClientID);
                }
                else if ((detailPanel.UniqueID + POST_BACK_BY_COUNTRY).Equals(Request.Form[Page.postEventSourceID])) 
                {
                    //if country changed, and do the post back. update the grade style in here.
                    //but if country not change upate the grade style in ajax method.
                    refExamId = hdnExamNbr.Value;

                    if (!string.IsNullOrEmpty(examName) && !string.IsNullOrEmpty(refExamId))
                    {
                        ChangeGradingStyleByProvider(examName);
                    }

                    Page.FocusElement(txtWithImgProviderName.ClientID);
                }
                else
                {
                    refExamId = hdnExamNbr.Value;
                }

                SetPermissionValue(refExamId);
            }

            SetDisableFinalScore();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //Add asterisk for required fields according to configuration in ACA admin.
            ControlBuildHelper.AddValidationForStandardFields(ViewId, ModuleName, Permission, Controls);
            
            //remove the require flag base examination status.
            SetRequiredFiled();
            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// Dropdown list command
        /// </summary>
        /// <param name="sender">an object that contains the event sender.</param>
        /// <param name="e">object that contains the event data</param>
        protected void ExaminationStatus_IndexChanged(object sender, EventArgs e)
        {    
            if (!AppSession.IsAdmin)
            {
                SetPermissionValue(hdnExamNbr.Value);

                if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(ddlExaminationStatus.SelectedValue, StringComparison.InvariantCultureIgnoreCase)
                    && DataSource != null && DataSource.finalScore != null)
                {   
                    txtFinalScore.Value = DataSource.finalScore.ToString();
                }
            }
        }

        /// <summary>
        /// Save Examination Detail Info
        /// </summary>
        /// <param name="sender">sender Object</param>
        /// <param name="e">Event Args</param>
        protected void Examination_Saved(object sender, EventArgs e)
        {
            if (DataSource == null)
            {
                DataSource = new ExaminationModel();
                DataSource.RowIndex = -1;
                DataSource.examinationPKModel = new ExaminationPKModel();
                DataSource.examinationPKModel.serviceProviderCode = CapAgencyCode;
            }

            if (!string.IsNullOrEmpty(hdnExamNbr.Value))
            {
                DataSource.refExamSeq = Convert.ToInt64(hdnExamNbr.Value);
            }
            else
            {
                DataSource.refExamSeq = null;
            }

            //Fill ExaminationModel
            DataSource.examName = txtWithImgExaminationName.Text;
            DataSource.gradingStyle = txtFinalScore.DisplayTypeString;
            DataSource.providerName = txtWithImgProviderName.Text;
            DataSource.providerNo = txtProviderNumber.Text;

            if (!string.IsNullOrEmpty(actExaminationDate.Text))
            {
                DataSource.examDate = I18nDateTimeUtil.ParseFromUI(actExaminationDate.Text);
            }
            else
            {
                DataSource.examDate = null;
            }

            DataSource.comments = txtComments.Text;
            DataSource.requiredFlag = EducationUtil.ConvertRequiredFeild2Save(txtRequired.Text.Trim());
            DataSource.approvedFlag = ValidationUtil.IsYes(DataSource.approvedFlag) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            if (!string.IsNullOrEmpty(txtFinalScore.Value))
            {
                DataSource.finalScore = I18nNumberUtil.ParseNumberFromWebService(txtFinalScore.Value);
            }
            else
            {
                DataSource.finalScore = null;
            }

            double passScore = 0;

            if (I18nNumberUtil.TryParseNumberFromInput(hdnPassingScore.Value, out passScore))
            {
                DataSource.passingScore = passScore;
            }

            //Fill ProviderDetailModel
            if (DataSource.examProviderDetailModel == null)
            {
                DataSource.examProviderDetailModel = new ExamProviderDetailModel();
            }
            
            DataSource.examProviderDetailModel.address1 = txtAddress1.Text;
            DataSource.examProviderDetailModel.address2 = txtAddress2.Text;
            DataSource.examProviderDetailModel.address3 = txtAddress3.Text;
            DataSource.examProviderDetailModel.city = txtCity.Text;
            DataSource.examProviderDetailModel.state = txtState.Text;
            DataSource.FromCapAssociate = false;

            DataSource.examProviderDetailModel.zip = txtZip.GetZip(ddlCountryCode.SelectedValue);
            DataSource.examProviderDetailModel.phone1 = txtPhone1.GetPhone(ddlCountryCode.SelectedValue);
            DataSource.examProviderDetailModel.phone1CountryCode = txtPhone1.CountryCodeText;
            DataSource.examProviderDetailModel.phone2 = txtPhone2.GetPhone(ddlCountryCode.SelectedValue);
            DataSource.examProviderDetailModel.phone2CountryCode = txtPhone2.CountryCodeText;
            DataSource.examProviderDetailModel.fax = txtFax.GetPhone(ddlCountryCode.SelectedValue);
            DataSource.examProviderDetailModel.faxCountryCode = txtFax.CountryCodeText;
            DataSource.examProviderDetailModel.email = txtEmail.Text;
            DataSource.examProviderDetailModel.countryCode = ddlCountryCode.SelectedValue;

            //Fill AuditModel
            if (DataSource.auditModel == null)
            {
                DataSource.auditModel = new SimpleAuditModel();
            }

            DataSource.auditModel.auditID = AppSession.User.PublicUserId;
            DataSource.auditModel.auditStatus = ACAConstant.VALID_STATUS;
            
            if (!string.IsNullOrEmpty(endTime.TimeText))
            {
                if (DataSource.examDate != null && DataSource.examDate.Value != null)
                {
                    DataSource.endTime = I18nDateTimeUtil.ParseFromUI(actExaminationDate.Text + ACAConstant.BLANK + endTime.TimeText);
                }
                else
                {
                    DataSource.endTime = I18nDateTimeUtil.ParseFromUI(System.DateTime.Now.ToShortDateString() + ACAConstant.BLANK + endTime.TimeText);
                }
            }
            else
            {
                DataSource.endTime = null;
            }

            if (!string.IsNullOrEmpty(startTime.TimeText))
            {
                if (DataSource.examDate != null && DataSource.examDate.Value != null)
                {
                    DataSource.startTime = I18nDateTimeUtil.ParseFromUI(actExaminationDate.Text + ACAConstant.BLANK + startTime.TimeText);
                }
                else
                {
                    DataSource.startTime = I18nDateTimeUtil.ParseFromUI(System.DateTime.Now.ToShortDateString() + ACAConstant.BLANK + startTime.TimeText);
                }
            }
            else
            {
                DataSource.startTime = null;
            }

            DataSource.examStatus = ddlExaminationStatus.SelectedValue;
            DataSource.userExamID = txtRosterID.Text;
            DataSource.template = genericTemplate.GetTemplateModel(true);

            //Send Examination Save Event
            DataChanged(sender, new CommonEventArgs(DataSource));
        }

        /// <summary>
        /// Clear Examination Detail Info
        /// </summary>
        /// <param name="sender">sender Object</param>
        /// <param name="e">Event Args</param>
        protected void Examination_Clear(object sender, EventArgs e)
        {
            if (!ForCAPDetail)
            {
                //Clear Examination Field Value
                ClearExamination();

                //Send Examination Save Event
                DataChanged(sender, new CommonEventArgs(DataSource));

                Page.SetFocus("lnkExaminationAdd");
            }
            else
            {
                ScriptManager.RegisterStartupScript(detailPanel, GetType(), "SaveExamination" + ClientID, "ClosePopup();", true);
            }
        }

        /// <summary>
        /// Set required fields according to logical
        /// </summary>
        private void SetRequiredFiled()
        {
            if (ddlExaminationStatus.Items.Count < 1)
            {
                return;
            }

            bool isPending = ACAConstant.EXAMINATION_STATUS_PENDING.Equals(ddlExaminationStatus.SelectedValue, StringComparison.InvariantCultureIgnoreCase);

            if (isPending)
            {
                RemoveReqiured();
            }

            SetGenericTemplateRquiredFlag(isPending);

            SetDisableFinalScore();
        }

        /// <summary>
        /// Set required flag for generic template fields when examination status is pending.
        /// And there is a special logic for AccelaRadioButtonList
        ///     - need set all validator status because these validator created in control creation stage instead of render state.
        /// </summary>
        /// <param name="isPending">if set to <c>true</c> [is pending].</param>
        private void SetGenericTemplateRquiredFlag(bool isPending)
        {
            if (genericTemplate.Controls.Count > 0)
            {
                foreach (var control in genericTemplate.Controls)
                {
                    if (isPending)
                    {
                        if (control is AccelaCheckBox)
                        {
                            var checkBox = control as AccelaCheckBox;
                            checkBox.Required = false;
                        }
                        else if (control is AccelaDropDownList)
                        {
                            var ddl = control as AccelaDropDownList;
                            ddl.Required = false;
                        }
                        else if (control is AccelaTextBox)
                        {
                            var textBox = control as AccelaTextBox;
                            textBox.Validate = textBox.Validate.Replace("required", string.Empty);
                        }
                    }

                    //when status change and the radio field is require, restore the validator enable.
                    if (control is AccelaRadioButtonList)
                    {
                        var radio = control as AccelaRadioButtonList;
                        var reqValidator = genericTemplate.FindControl(radio.ID + "_req");
                        var reqValidatorExt = genericTemplate.FindControl(radio.ID + "_req_ext");

                        if (isPending)
                        {
                            radio.Required = false;
                        }

                        if (reqValidator is RadioButtonListRequiredFieldValidator)
                        {
                            (reqValidator as RadioButtonListRequiredFieldValidator).Enabled = radio.Required;
                        }

                        if (reqValidatorExt is ValidatorCallbackExtender)
                        {
                            (reqValidatorExt as ValidatorCallbackExtender).Enabled = radio.Required;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set final score disable or not
        /// </summary>
        private void SetDisableFinalScore()
        {
            if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(ddlExaminationStatus.SelectedValue, StringComparison.InvariantCultureIgnoreCase))
            {
                txtFinalScore.Disable = false;
            }
            else
            {
                txtFinalScore.Disable = true;
            }
        }

        /// <summary>
        /// Remove required property value of all the fields.
        /// </summary>
        private void RemoveReqiured()
        { 
            txtProviderNumber.Validate = txtProviderNumber.Validate.Replace("required", string.Empty);
            txtWithImgProviderName.Validate = txtWithImgProviderName.Validate.Replace("required", string.Empty);
            txtAddress1.Validate = txtAddress1.Validate.Replace("required", string.Empty);
            txtAddress2.Validate = txtAddress2.Validate.Replace("required", string.Empty);
            txtAddress3.Validate = txtAddress3.Validate.Replace("required", string.Empty);
            txtCity.Validate = txtCity.Validate.Replace("required", string.Empty);
            txtComments.Validate = txtComments.Validate.Replace("required", string.Empty);
            txtEmail.Validate = txtEmail.Validate.Replace("required", string.Empty);
            txtFax.Validate = txtFax.Validate.Replace("required", string.Empty);
            txtPhone1.Validate = txtPhone1.Validate.Replace("required", string.Empty);
            txtPhone2.Validate = txtPhone2.Validate.Replace("required", string.Empty);
            txtRequired.Validate = txtRequired.Validate.Replace("required", string.Empty);
            txtRosterID.Validate = txtRosterID.Validate.Replace("required", string.Empty);
            txtState.Validate = txtState.Validate.Replace("required", string.Empty);
            txtZip.Validate = txtZip.Validate.Replace("required", string.Empty);
            startTime.Validate = startTime.Validate.Replace("required", string.Empty);
            endTime.Validate = endTime.Validate.Replace("required", string.Empty);
            actExaminationDate.Validate = actExaminationDate.Validate.Replace("required", string.Empty);
            ddlCountryCode.Required = false;
        }

        /// <summary>
        /// Set Button Controls in Examination Detail form to disable
        /// </summary>
        private void DisableExaminationButton()
        {
            // Add
            btnSave.Enabled = false;
        }

        /// <summary>
        /// Change the grading style by provider name.
        /// </summary>
        /// <param name="examName">Examination name.</param>
        private void ChangeGradingStyleByProvider(string examName)
        {
            string providerName = txtWithImgProviderName.Text.Trim();

            if (!string.IsNullOrWhiteSpace(providerName))
            {
                IRefExaminationBll refExaminationBLL = ObjectFactory.GetObject<IRefExaminationBll>();
                XRefExaminationProviderModel examinationProvider = refExaminationBLL.GetRefExamProviderModel(examName, providerName);

                if (examinationProvider != null)
                {
                    bool isPassfail = ACA.Common.GradingStyle.Passfail.ToString().Equals(examinationProvider.gradingStyle, StringComparison.OrdinalIgnoreCase);

                    hdnPassingScore.Value = examinationProvider.passingScore == null || isPassfail
                        ? string.Empty
                        : examinationProvider.passingScore.ToString();

                    txtFinalScore.DisplayTypeString = examinationProvider.gradingStyle;
                }
            }
        }

        #endregion 
    }
}
