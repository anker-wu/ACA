#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationList.ascx.cs 139167 2009-07-15 06:20:30Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display Examination Information in List Table
    /// </summary>
    public partial class ExaminationList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Row Selected
        /// </summary>
        protected const string EXAMINATION_SELECTED = "ExaminationSelected";

        /// <summary>
        /// Row Deleted
        /// </summary>
        protected const string EXAMINATION_DELECTED = "ExaminationDelected";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ExaminationList));

        #endregion

        #region Properties

        /// <summary>
        /// Examination Item Select Event.
        /// </summary>
        public event CommonEventHandler ExaminationSelected;

        /// <summary>
        /// Examination Delete Event
        /// </summary>
        public event CommonEventHandler ExaminationDeleted;
        
        /// <summary>
        /// ENUM for Examination Display type.
        /// </summary>
        public enum ExaminationDisplayType
        {
            /// <summary>
            /// Spear Page
            /// </summary>
            CapEdit,

            /// <summary>
            /// Cap Home Page
            /// </summary>
            CapDetail
        }

        /// <summary>
        /// Gets or sets Examinations, Examination Collection
        /// </summary>
        public IList<ExaminationModel> DataSource
        {
            get
            {
                if (ViewState["ExaminationDataSource"] == null)
                {
                    ViewState["ExaminationDataSource"] = new List<ExaminationModel>();
                }

                return (List<ExaminationModel>)ViewState["ExaminationDataSource"];
            }

            set
            {
                ViewState["ExaminationDataSource"] = ExaminationUtil.AddRowIndex(value);
            }
        }

        /// <summary>
        /// Gets or sets the view id for examination list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvExaminationList.GridViewNumber;
            }

            set
            {
                gdvExaminationList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether GridView required property
        /// </summary>
        public bool IsRequired
        {
            get 
            {
                return gdvExaminationList.IsRequired;
            }

            set
            {
                gdvExaminationList.IsRequired = value;
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
        /// Gets or sets the education section position.
        /// </summary>
        public EducationOrExamSectionPosition ExaminationSectionPosition
        {
            get
            {
                return EnumUtil<EducationOrExamSectionPosition>.Parse(
                                    Convert.ToString(ViewState["ExaminationSectionPosition"]),
                                    EducationOrExamSectionPosition.None);
            }

            set
            {
                ViewState["ExaminationSectionPosition"] = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        GViewID = GviewID.RefContactExaminationList;
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                    case EducationOrExamSectionPosition.CapConfirm:
                        GViewID = GviewID.SpearFormExaminationList;
                        break;
                    case EducationOrExamSectionPosition.CapDetail:
                        GViewID = GviewID.LicenseDetailExaminationList;
                        break;
                }

                GridViewBuildHelper.SetSimpleViewElements(gdvExaminationList, ModuleName, AppSession.IsAdmin);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact type editable settings or not.
        /// </summary>
        private bool ContactTypePermission
        {
            get
            {
                if (!AppSession.IsAdmin && ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
                {
                    return ContactUtil.GetContactTypeEditablePermissionByContactSeqNbr(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
                }

                return true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Examination Control Re-Bind with DataSource
        /// </summary>
        public void Bind()
        {
            this.gdvExaminationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            this.gdvExaminationList.DataSource = DataSource;
            this.gdvExaminationList.DataBind();
        }

        /// <summary>
        /// Display delete action notice
        /// </summary>
        /// <param name="isSuccessfully">True when deleting record successfully; false otherwise.</param>
        public void DisplayDelActionNotice(bool isSuccessfully)
        {
            if (isSuccessfully)
            {
                this.lblActionNoticeDeleteSuccess.Text = GetTextByKey(lblActionNoticeDeleteSuccess.LabelKey);
                lblActionNoticeDeleteSuccess.Visible = true;
                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeDeleteSuccess, GetTextByKey(lblActionNoticeDeleteSuccess.LabelKey));
                }

                divImgSuccess.Visible = true;
            }
            else
            {
                this.lblActionNoticeDeleteFailed.Text = GetTextByKey(lblActionNoticeDeleteFailed.LabelKey);
                this.lblActionNoticeDeleteFailed.Visible = true;
                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeDeleteFailed, GetTextByKey(lblActionNoticeDeleteFailed.LabelKey));
                }

                divImgFailed.Visible = true;
            }

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Display add action notice
        /// </summary>
        /// <param name="isSuccessfully">True when adding record successfully; false otherwise.</param>
        public void DisplayAddActionNotice(bool isSuccessfully)
        {
            if (isSuccessfully)
            {
                this.lblActionNoticeAddSuccess.Text = GetTextByKey(lblActionNoticeAddSuccess.LabelKey);
                this.lblActionNoticeAddSuccess.Visible = true;
                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddSuccess, GetTextByKey(lblActionNoticeAddSuccess.LabelKey));
                }

                divImgSuccess.Visible = true;
            }
            else
            {
                this.lblActionNoticeAddFailed.Text = GetTextByKey(lblActionNoticeAddFailed.LabelKey);
                this.lblActionNoticeAddFailed.Visible = true;
                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddFailed, GetTextByKey(lblActionNoticeAddFailed.LabelKey));
                }

                divImgFailed.Visible = true;
            }

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Display Update action notice
        /// </summary>
        /// <param name="isSuccessfully">True when updating record successfully; false otherwise.</param>
        public void DisplayUpdateActionNotice(bool isSuccessfully)
        {
            if (isSuccessfully)
            {
                this.lblActionNoticeEditSuccess.Text = GetTextByKey(lblActionNoticeEditSuccess.LabelKey);
                this.lblActionNoticeEditSuccess.Visible = true;
                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeEditSuccess, GetTextByKey(lblActionNoticeEditSuccess.LabelKey));
                }

                divImgSuccess.Visible = true;
            }
            else
            {
                this.lblActionNoticeEditFailed.Text = GetTextByKey(lblActionNoticeEditFailed.LabelKey);
                this.lblActionNoticeEditFailed.Visible = true;
                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeEditFailed, GetTextByKey(lblActionNoticeEditFailed.LabelKey));
                }

                divImgFailed.Visible = true;
            }

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Display add from saved notice.
        /// </summary>
        /// <param name="isSuccessfully">True when adding record successfully; false otherwise.</param>
        /// <param name="msg">The message.</param>
        public void DisplayAddFromSavedNotice(bool isSuccessfully, string msg)
        {
            if (isSuccessfully)
            {
                this.lblActionNoticeAddSuccess.Text = msg;
                this.lblActionNoticeAddSuccess.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddSuccess, msg);
                }

                divImgSuccess.Visible = true;
            }
            else
            {
                this.lblActionNoticeAddFailed.Text = msg;
                this.lblActionNoticeAddFailed.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddFailed, msg);
                }

                divImgFailed.Visible = true;
            }

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">Event Args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ExaminationSectionPosition == EducationOrExamSectionPosition.CapEdit)
                {
                    //Hidden Common Icon Column
                    gdvExaminationList.Columns[0].Visible = false;

                    if (AppSession.IsAdmin)
                    {
                        //Hidden ValidateRequire Icon Column
                        gdvExaminationList.Columns[1].Visible = false;
                    }
                }
                else if (ExaminationSectionPosition == EducationOrExamSectionPosition.CapDetail)
                {
                    divExaminationInfo.Visible = false;

                    if (AppSession.IsAdmin)
                    {
                        //Hidden Common Icon Column
                        gdvExaminationList.Columns[0].Visible = false;
                    }

                    ////Hidden ValidateRequire Icon Column
                    gdvExaminationList.Columns[1].Visible = false;
                }

                if (!AppSession.IsAdmin && ExaminationSectionPosition == EducationOrExamSectionPosition.CapEdit)
                {
                    int rowIndex = 0;

                    if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_CONFIRMPAGE]) &&
                        PageFlowConstant.SECTION_NAME_EXAMINATION.Equals(Request.QueryString[UrlConstant.SECTION_NAME], StringComparison.InvariantCultureIgnoreCase) &&
                        int.TryParse(Request.QueryString[UrlConstant.ROW_INDEX], out rowIndex))
                    {
                        if (rowIndex > gdvExaminationList.PageSize)
                        {
                            gdvExaminationList.SetPageIndex(rowIndex / gdvExaminationList.PageSize);
                            gdvExaminationList.DataBind();
                        }

                        if (AppSession.IsEditFromConfirmFlag)
                        {
                            var editRow = gdvExaminationList.Rows[rowIndex % gdvExaminationList.PageSize];
                            AccelaLinkButton lnkExamName = (AccelaLinkButton)editRow.FindControl("ExaminationName");
                            AccelaLinkButton lnkEdit = (AccelaLinkButton)editRow.FindControl("lnkEditExamination");
                            PopupActions actionMenu = (PopupActions)editRow.FindControl("actionMenu");

                            string script = string.Format("openEduExamEditDialog('{0}', '{1}', '{2}');", lnkExamName.ClientID, lnkEdit.ClientID, actionMenu.ActionsLinkClientID);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "openExamEditDialog", script, true);

                            AppSession.IsEditFromConfirmFlag = false;
                        }
                    }
                }
            }

            if (!AppSession.IsAdmin)
            {
                divActionNotice.Visible = false;
                lblActionNoticeAddFailed.Visible = false;
                lblActionNoticeAddSuccess.Visible = false;
                lblActionNoticeDeleteFailed.Visible = false;
                lblActionNoticeDeleteSuccess.Visible = false;
                lblActionNoticeEditFailed.Visible = false;
                lblActionNoticeEditSuccess.Visible = false;
                divImgFailed.Visible = false;
                divImgSuccess.Visible = false;
            }
            else
            {
                divActionNotice.Visible = true;
                lblActionNoticeAddFailed.Visible = true;
                lblActionNoticeAddSuccess.Visible = true;
                lblActionNoticeDeleteFailed.Visible = true;
                lblActionNoticeDeleteSuccess.Visible = true;
                lblActionNoticeEditFailed.Visible = true;
                lblActionNoticeEditSuccess.Visible = true;
            }
        }

        /// <summary>
        /// Override OnPreRender event to change the label key.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ChangeLabelKey();
        }

        /// <summary>
        /// Exam row command event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event arguments.</param>
        protected void ExamList_ActionCommand(object sender, EventArgs e)
        {
            AccelaLinkButton actionButton = (AccelaLinkButton)sender;
            int examIndex = Convert.ToInt32(actionButton.CommandArgument);

            switch (actionButton.CommandName)
            {
                case EXAMINATION_SELECTED:
                    if (ExaminationSelected != null)
                    {
                        ExaminationModel selectedExamination = DataSource.Single(o => o.RowIndex == Convert.ToInt64(examIndex));

                        if (selectedExamination != null)
                        {
                            object[] arg = new object[] { examIndex, selectedExamination };
                            ExaminationSelected(sender, new CommonEventArgs(arg));
                        }
                    }

                    break;
                case EXAMINATION_DELECTED:
                    try
                    {
                        ExaminationModel removeExam = DataSource.FirstOrDefault(o => o.RowIndex == Convert.ToInt64(examIndex));
                        DataSource.Remove(removeExam);
                        Bind();

                        if (ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit && !ValidationUtil.IsYes(removeExam.approvedFlag))
                        {
                            IExaminationBll licenseCertificationBll = ObjectFactory.GetObject<IExaminationBll>();

                            if (removeExam.auditModel == null || string.IsNullOrEmpty(removeExam.auditModel.auditID))
                            {
                                removeExam.auditModel = new SimpleAuditModel { auditID = AppSession.User.PublicUserId };    
                            }

                            licenseCertificationBll.DeleteExam(removeExam);
                        }

                        if (ExaminationDeleted != null)
                        {
                            ExaminationDeleted(sender, new CommonEventArgs(examIndex));
                        }

                        DisplayDelActionNotice(true);
                    }
                    catch (ACAException ex)
                    {
                        Logger.Error(ex);
                        DisplayDelActionNotice(false);
                    }

                    break;
            }
        }

        /// <summary>
        /// Examination Row bind event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event arguments.</param>
        protected void ExaminationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e == null || e.Row == null || e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            BuildActionMenu(e);

            ExaminationModel exam = (ExaminationModel)e.Row.DataItem;

            //Require Row can't be deleted
            AccelaLabel lblRequired = (AccelaLabel)e.Row.FindControl("lblRequired");
            AccelaLabel lblApproved = (AccelaLabel)e.Row.FindControl("lblApproved");
            lblApproved.Text = ValidationUtil.IsYes(exam.approvedFlag) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;                          

            bool isAccountContactEditPage = ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            bool isApprovedOrAssociated = ValidationUtil.IsYes(exam.approvedFlag) || exam.associatedExamCount > 0;
            
            lblRequired.Text = EducationUtil.ConvertRequiredField2Display(lblRequired.Text);

            //Require Field confirm
            AccelaDiv divImg = (AccelaDiv)e.Row.FindControl("divImg");
            string viewId = isAccountContactEditPage ? GviewID.RefContactExaminationEdit : GviewID.ExaminationEdit;
            GFilterScreenPermissionModel4WS permission = ControlBuildHelper.GetPermissionWithGenericTemplate(viewId, GViewConstant.SECTION_EXAMINATION, exam.refExamSeq == null ? string.Empty : exam.refExamSeq.ToString(), exam.template);

            if (!isApprovedOrAssociated && IsEditable && !ACAConstant.EXAMINATION_STATUS_PENDING.Equals(exam.examStatus, StringComparison.InvariantCultureIgnoreCase)
                && (!RequiredValidationUtil.ValidateFields4Object(ModuleName, permission, viewId, exam)
                    || !RequiredValidationUtil.ValidateFields4GenericTemplate(exam.template, this.ModuleName)))
            {
                divImg.Visible = true;
            }
            else
            {
                divImg.Visible = false;
            }

            //format examinationdate
            AccelaLabel lblExaminationDate = (AccelaLabel)e.Row.FindControl("lblExaminationDate");

            if (lblExaminationDate != null && exam.examDate != null)
            {
                lblExaminationDate.Text = I18nDateTimeUtil.FormatToDateStringForUI(exam.examDate.Value);
            }

            //format examination start time
            AccelaLabel lblExaminationStartTime = (AccelaLabel)e.Row.FindControl("lblExaminationStartTime");
            if (lblExaminationStartTime != null && exam.startTime != null)
            {
                lblExaminationStartTime.Text = I18nDateTimeUtil.FormatToTimeStringForUI(exam.startTime.Value, false);
            }

            //format examination end time
            AccelaLabel lblExaminationEndTime = (AccelaLabel)e.Row.FindControl("lblExaminationEndTime");
            if (lblExaminationEndTime != null && exam.endTime != null)
            {
                lblExaminationEndTime.Text = I18nDateTimeUtil.FormatToTimeStringForUI(exam.endTime.Value, false);
            }

            //format display final socre. 
            AccelaLabel lblFinalScore = (AccelaLabel)e.Row.FindControl("lblFinalScore");
                
            if (lblFinalScore != null)
            {
                lblFinalScore.Text = EducationUtil.FormatScore(exam.gradingStyle, exam.finalScore == null ? string.Empty : I18nNumberUtil.FormatNumberForWebService(exam.finalScore.Value));
            }

            //format display passing score.
            AccelaLabel lblPassingScore = (AccelaLabel)e.Row.FindControl("lblPassingScore");
                
            if (lblPassingScore != null)
            {
                lblPassingScore.Text = EducationUtil.FormatScore(exam.gradingStyle, lblPassingScore.Text, true);
            }

            //format display passing score.
            AccelaLabel lblExamStatus = (AccelaLabel)e.Row.FindControl("lblExamStatus");

            if (lblExamStatus != null)
            {
                lblExamStatus.Text = ExaminationUtil.GetExamStatusLabel(exam.examStatus, this.ModuleName);
            }

            AccelaDiv imgExpand = (AccelaDiv)e.Row.FindControl("divLogo");

            //Display or Hidden Common Column/the comment cell container
            if (string.IsNullOrEmpty(exam.comments))
            {
                if (imgExpand != null)
                {
                    imgExpand.Visible = false;
                }

                var lblCommonsValue = e.Row.FindControl("lblCommonsValue");
                var dataCell4Comment = lblCommonsValue == null ? null : lblCommonsValue.Parent as DataControlFieldCell;

                //if Expand icon is invisible, set the cell container to invisible also for section 508
                if (dataCell4Comment != null && imgExpand != null && imgExpand.Visible == false)
                {
                    dataCell4Comment.Visible = false;
                }
            }

            //set Expand column visibile for section 508
            AccelaGridView currentGridView = sender as AccelaGridView;
            var dataCell4ExpandIcon = imgExpand == null ? null : imgExpand.Parent as DataControlFieldCell;

            if (currentGridView != null && dataCell4ExpandIcon != null && e.Row.RowIndex >= 0 && AccessibilityUtil.AccessibilityEnabled)
            {
                if (e.Row.RowIndex == 0)
                {
                    dataCell4ExpandIcon.ContainingField.Visible = false;
                }

                if (e.Row.RowIndex >= 0 && imgExpand.Visible == true)
                {
                    dataCell4ExpandIcon.ContainingField.Visible = true;
                }
            }
        }

        /// <summary>
        /// Bind action menu.
        /// </summary>
        /// <param name="e">GridView Row Event Arguments</param>
        private void BuildActionMenu(GridViewRowEventArgs e)
        {
            ExaminationModel exam = (ExaminationModel)e.Row.DataItem;
            PopupActions actionMenu = e.Row.FindControl("actionMenu") as PopupActions;
            AccelaLinkButton lnkDelete = (AccelaLinkButton)e.Row.FindControl("lnkDeleteExamination");
            AccelaLinkButton lnkView = (AccelaLinkButton)e.Row.FindControl("lnkViewExamination");
            AccelaLinkButton lnkEdit = (AccelaLinkButton)e.Row.FindControl("lnkEditExamination");
            AccelaLinkButton lnkExamName = (AccelaLinkButton)e.Row.FindControl("ExaminationName");
            AccelaLabel lblExamName = (AccelaLabel)e.Row.FindControl("lblExamName");
            
            ActionViewModel actionView;
            var actionList = new List<ActionViewModel>();
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

            bool isAccountContactEditPage = ExaminationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            bool isCapEdiPage = ExaminationSectionPosition == EducationOrExamSectionPosition.CapEdit;
            bool isCapConfimPage = ExaminationSectionPosition == EducationOrExamSectionPosition.CapConfirm;
            bool isCapDetailPage = ExaminationSectionPosition == EducationOrExamSectionPosition.CapDetail;

            bool isApprovedOrAssociated = ValidationUtil.IsYes(exam.approvedFlag) || exam.associatedExamCount > 0;
            bool isRefExam = (isCapEdiPage || isCapConfimPage) && exam.entityID.HasValue;

            /* ReadOnly mode:
             *  1. Cap detail page.
             *  2. The examination has been approved or has been used by any cap.
             */
            if (isCapDetailPage || ((isAccountContactEditPage || isRefExam) && isApprovedOrAssociated) || !ContactTypePermission)
            {
                lnkEdit.Visible = false;
                lnkExamName.Visible = false;
                lblExamName.Visible = true;

                if (!isCapEdiPage)
                {
                    lnkDelete.Visible = false;
                }

                if (isCapConfimPage)
                {
                    lnkView.Visible = false;
                }
            }
            else
            {
                lnkView.Visible = false;

                // Hide delete link when required field is 'Y' or in cap confirm page, or this section is disable.
                if (ValidationUtil.IsYes(exam.requiredFlag) || isCapConfimPage || !IsEditable || ContactIsFromExternal)
                {
                    lnkDelete.Visible = false;
                }
            }

            if (lnkView.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_examination_list_label_view");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");

                if (actionMenu != null)
                {
                    actionView.ActionId = actionMenu.ClientID + "_ViewDetails";
                }
                
                string url = string.Empty;
                long? examNbr = exam.examinationPKModel.examNbr ?? exam.entityID;

                if (ACAConstant.EXAMINATION_STATUS_COMPLETED_SCHEDULE.Equals(exam.examStatus, StringComparison.InvariantCultureIgnoreCase)
                    || ACAConstant.EXAMINATION_STATUS_SCHEDULE.Equals(exam.examStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    url = FileUtil.AppendApplicationRoot(
                        "Examination/ExaminationScheduleView.aspx?" + UrlConstant.AgencyCode + "="
                        + exam.examinationPKModel.serviceProviderCode + "&ExaminationNum="
                        + examNbr + "&" + ACAConstant.MODULE_NAME + "="
                        + this.ModuleName);
                }
                else
                {
                    url = FileUtil.AppendApplicationRoot(
                        "LicenseCertification/ExaminationDetail.aspx?" + UrlConstant.AgencyCode + "="
                        + exam.examinationPKModel.serviceProviderCode + "&ExaminationNum="
                        + examNbr + "&" + ACAConstant.MODULE_NAME + "="
                        + this.ModuleName + "&fromlicese=fromlicese");

                    if (isAccountContactEditPage)
                    {
                        url += string.Format("&{0}={1}", UrlConstant.CONTACT_SEQ_NUMBER, exam.contactSeqNumber);
                    }
                }

                if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
                {
                    actionView.ClientEvent = string.Format("popUpDetailDialog('{0}', '{1}');return false;", url, actionView.ActionId);
                }
                else
                {
                    actionView.ClientEvent = string.Format("popUpDetailDialog('{0}', '{1}');return false;", url, actionMenu.ActionsLinkClientID);
                }

                actionList.Add(actionView);
            }

            if (lnkEdit.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_examinationlist_label_lnkedit");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");

                if (actionMenu != null)
                {
                    actionView.ActionId = actionMenu.ClientID + "_Edit";
                }

                string popupScript = string.Empty;

                if (isCapConfimPage)
                {
                    popupScript = "CallPostBackFunction('{0}');SetNotAskForSPEAR();";
                    lnkExamName.OnClientClick = string.Format(popupScript, lnkExamName.UniqueID);

                    actionView.ClientEvent = string.Format(popupScript, lnkEdit.UniqueID);
                }
                else
                {
                    popupScript = string.Format(
                       "openExamFormDialog({{0}}, '{0}', '{1}', '{2}', '{3}');return false;",
                       isAccountContactEditPage ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                       exam.RowIndex,
                       IsEditable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                       ContactIsFromExternal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
                    string actionLinkObj = "$get('" + actionMenu.ActionsLinkClientID + "')";
                    lnkEdit.OnClientClick = string.Format(popupScript, actionLinkObj);

                    if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
                    {
                        actionView.ClientEvent = string.Format(popupScript, actionView.ActionId);
                    }
                    else
                    {
                        actionView.ClientEvent = string.Format(popupScript, actionLinkObj);
                    }

                    lnkExamName.OnClientClick = string.Format(popupScript, "this");
                }

                actionList.Add(actionView);
            }

            if (lnkDelete.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("examination_list_delete");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");

                if (actionMenu != null)
                {
                    actionView.ActionId = actionMenu.ClientID + "_Delete";
                }

                if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
                {
                    actionView.ClientEvent = string.Format(
                        "return TriggerEventWithConfirm('{0}', '{1}');",
                        lnkDelete.UniqueID,
                        GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'"));
                }
                else
                {
                    actionView.ClientEvent = string.Format(
                        "return TriggerEventWithConfirm('{0}', '{1}');",
                        lnkDelete.UniqueID,
                        GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'"));
                }

                actionList.Add(actionView);
            }

            if (actionList.Count > 0)
            {
                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_examlist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        private void ChangeLabelKey()
        {
            if (GViewID == GviewID.RefContactExaminationList)
            {
                GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvExaminationList);

                ((IAccelaNonInputControl)headerRow.FindControl("lnkExaminationName")).LabelKey = "aca_contact_examination_list_label_exam_name";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderName")).LabelKey = "aca_contact_examination_list_label_provider_name";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderNumber")).LabelKey = "aca_contact_examination_list_label_provider_number";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkExaminationDate")).LabelKey = "aca_contact_examination_list_label_exam_date";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkExaminationStartTime")).LabelKey = "aca_contact_examination_list_label_start_time";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkExaminationEndTime")).LabelKey = "aca_contact_examination_list_label_end_time";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFinalScore")).LabelKey = "aca_contact_examination_list_label_final_score";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPassingScore")).LabelKey = "aca_contact_examinationlist_label_passingscore";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkUserExamID")).LabelKey = "aca_contact_examination_list_label_user_examid";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkExamStatus")).LabelKey = "aca_contact_examination_list_label_exam_status";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkApprovedHeader")).LabelKey = "aca_contact_examination_list_label_approved";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRequested")).LabelKey = "aca_contact_examination_list_label_required";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderAddress1")).LabelKey = "aca_contact_examination_list_label_address1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderAddress2")).LabelKey = "aca_contact_examination_list_label_address2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderAddress3")).LabelKey = "aca_contact_examination_list_label_address3";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderCity")).LabelKey = "aca_contact_examination_list_label_city";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderState")).LabelKey = "aca_contact_examination_list_label_state";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCountry")).LabelKey = "aca_contact_examination_list_label_country";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderZipCode")).LabelKey = "aca_contact_examination_list_label_zipcode";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderPhoneNumber1")).LabelKey = "aca_contact_examination_list_label_phonenumber1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderPhoneNumber2")).LabelKey = "aca_contact_examination_list_label_phonenumber2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderFax")).LabelKey = "aca_contact_examination_list_label_fax";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderEmail")).LabelKey = "aca_contact_examination_list_label_email";
                ((IAccelaNonInputControl)headerRow.FindControl("lblActionHeader")).LabelKey = "aca_contact_examination_list_label_action";
            }
        }

        #endregion
    }
}
