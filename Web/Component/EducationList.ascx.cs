#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationList.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
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
    /// Display Educations which from ref Education or manually add in daily side.
    /// </summary>
    public partial class EducationList : BaseUserControl
    {
        #region Properties 

        /// <summary>
        /// select education.
        /// </summary>
        protected const string SELECT_EDUCATION = "SelectEducation";

        /// <summary>
        /// delete education.
        /// </summary>
        protected const string DELETE_EDUCATION = "DeleteEducation";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EducationList));

        /// <summary>
        /// Education selected event instance.
        /// </summary>
        public event CommonEventHandler EducationSelected;

        /// <summary>
        /// grid view row command event
        /// </summary>
        public event CommonEventHandler EducationsDeleted;

        /// <summary>
        /// Gets or sets Education data source.
        /// </summary>
        public IList<EducationModel4WS> GridViewDataSource
        {
            get
            {
                if (ViewState["EducationModels"] == null)
                {
                    ViewState["EducationModels"] = new List<EducationModel4WS>();
                }

                return ViewState["EducationModels"] as IList<EducationModel4WS>;
            }

            set
            {
                ViewState["EducationModels"] = EducationUtil.AddRowIndex2EducationModel(value);
            }
        }

        /// <summary>
        /// Gets or sets the view id for education list
        /// </summary>
        public string GViewID
        {
            get
            {
                return this.gdvEducationList.GridViewNumber;
            }

            set
            {
                this.gdvEducationList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets the Row index.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return ViewState["SelectedIndex"] == null ? -1 : Convert.ToInt32(ViewState["SelectedIndex"]);
            }

            set
            {
                ViewState["SelectedIndex"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsEditable4Edu"]);
            }

            set
            {
                ViewState["IsEditable4Edu"] = value;
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
        /// Gets or sets the contact sequence number.
        /// </summary>
        public string ContactSeqNbr
        {
            get
            {
                return ViewState["ContactSeqNbr"] as string;
            }
            
            set
            {
                ViewState["ContactSeqNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the education section position.
        /// </summary>
        public EducationOrExamSectionPosition EducationSectionPosition
        {
            get
            {
                return EnumUtil<EducationOrExamSectionPosition>.Parse(
                                    Convert.ToString(ViewState["EducationSectionPosition"]), 
                                    EducationOrExamSectionPosition.None);
            }

            set
            {
                ViewState["EducationSectionPosition"] = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        GViewID = GviewID.RefContactEducationList;
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                    case EducationOrExamSectionPosition.CapConfirm:
                        GViewID = GviewID.SpearFormEducationList;
                        break;
                }

                GridViewBuildHelper.SetSimpleViewElements(gdvEducationList, ModuleName, AppSession.IsAdmin);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact type editable settings or not.
        /// </summary>
        private bool ContactTypePermission
        {
            get
            {
                if (!AppSession.IsAdmin && EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
                {
                    return ContactUtil.GetContactTypeEditablePermissionByContactSeqNbr(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
                }

                return true;
            }
        }

        #endregion Properties

        /// <summary>
        /// Bind education with data source.
        /// </summary>
        /// <param name="needRecalculateRowIndex">NeedRecalculateRowIndex Flag</param>
        public void BindEducations(bool needRecalculateRowIndex = false)
        {
            if (needRecalculateRowIndex)
            {
                EducationUtil.AddRowIndex2EducationModel(GridViewDataSource);
            }

            gdvEducationList.DataSource = GridViewDataSource;
            gdvEducationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvEducationList.DataBind();
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
        /// Display add from saved notice.
        /// </summary>
        /// <param name="isSuccessfully">True when adding record successfully; false otherwise.</param>
        /// <param name="msg">The Message.</param>
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
        /// set the GridView required property
        /// </summary>
        /// <param name="isRequired">indicate if the GridView is required</param>
        public void SetGridViewRequired(bool isRequired)
        {
            gdvEducationList.IsRequired = isRequired;
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Hide Education instruction for Education list in CAP detail page.
                if (EducationSectionPosition == EducationOrExamSectionPosition.CapDetail)
                {
                    divEducationInfo.Visible = false;
                    gdvEducationList.Columns[0].Visible = false;
                }
                else
                {
                    gdvEducationList.Columns[1].Visible = false;
                }

                if (!AppSession.IsAdmin && EducationSectionPosition == EducationOrExamSectionPosition.CapEdit)
                {
                    int rowIndex = 0;

                    if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_CONFIRMPAGE]) &&
                        PageFlowConstant.SECTION_NAME_EDUCATION.Equals(Request.QueryString[UrlConstant.SECTION_NAME], StringComparison.InvariantCultureIgnoreCase) &&
                        int.TryParse(Request.QueryString[UrlConstant.ROW_INDEX], out rowIndex))
                    {
                        if (rowIndex > gdvEducationList.PageSize)
                        {
                            gdvEducationList.SetPageIndex(rowIndex / gdvEducationList.PageSize);
                            gdvEducationList.DataBind();
                        }

                        if (AppSession.IsEditFromConfirmFlag)
                        {
                            var editRow = gdvEducationList.Rows[rowIndex % gdvEducationList.PageSize];
                            AccelaLinkButton lnkEduName = (AccelaLinkButton)editRow.FindControl("lnkMajorDiscipine");
                            AccelaLinkButton lnkEdit = (AccelaLinkButton)editRow.FindControl("lnkEdit");
                            PopupActions actionMenu = (PopupActions)editRow.FindControl("actionMenu");

                            string script = string.Format("openEduExamEditDialog('{0}', '{1}', '{2}');", lnkEduName.ClientID, lnkEdit.ClientID, actionMenu.ActionsLinkClientID);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "openEduEditDialog", script, true);

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
        /// Education row bind event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event arguments.</param>
        protected void EducationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            BuildActionMenu(e);

            AccelaLabel lblRequired = (AccelaLabel)e.Row.FindControl("lblRequired");
            AccelaDiv divImg = (AccelaDiv)e.Row.FindControl("divImg");
            AccelaDiv divLogo = (AccelaDiv)e.Row.FindControl("divLogo");
            AccelaLabel lblApproved = (AccelaLabel)e.Row.FindControl("lblApproved");

            EducationModel4WS education = e.Row.DataItem as EducationModel4WS;

            string required = education.requiredFlag;
            lblRequired.Text = EducationUtil.ConvertRequiredField2Display(required);
            lblApproved.Text = ValidationUtil.IsYes(education.approvedFlag) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;

            bool isAccountContactEditPage = EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            bool isCapDetailPage = EducationSectionPosition == EducationOrExamSectionPosition.CapDetail;

            bool isApprovedOrAssociated = ValidationUtil.IsYes(education.approvedFlag) || education.associatedEduCount > 0;
            var divComment = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("commentPanel");

            //Education comment.
            string educationComment = education.comments;

            //validate comments is empty or null to hide logo.
            if (string.IsNullOrEmpty(educationComment))
            {
                divLogo.Visible = false;
            }

            if (isCapDetailPage)
            {
                divImg.Visible = false;
                divEducationInfo.Visible = false;
            }
            else
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);
                string refEduID = education.RefEduNbr;

                if (string.IsNullOrEmpty(refEduID) && !isAccountContactEditPage)
                {
                    RefEducationModel4WS refEducation =
                        EducationUtil.GetRefEducationModel(
                            capModel.capType.serviceProviderCode, education.educationName);

                    refEduID = refEducation == null ? string.Empty : refEducation.refEducationNbr.ToString();
                }

                //validate all of required fields have value for each row in Education list.
                string viewId = isAccountContactEditPage ? GviewID.RefContactEducationEdit : GviewID.EducationEdit;
                GFilterScreenPermissionModel4WS permission = ControlBuildHelper.GetPermissionWithGenericTemplate(viewId, GViewConstant.SECTION_EDUCATOIN, refEduID, education.template);

                if (!isApprovedOrAssociated && IsEditable && (!RequiredValidationUtil.ValidateFields4Object(ModuleName, permission, viewId, education)
                                                              || !RequiredValidationUtil.ValidateFields4GenericTemplate(education.template, this.ModuleName)))
                {
                    divImg.Visible = true;
                }
                else
                {
                    divImg.Visible = false;
                }

                //Spear form and Confirm page need hide arrow logo.
                divLogo.Visible = false;
            }

            divComment.Visible = divLogo.Visible;

            var dataCell4Comment = divComment == null ? null : divComment.Parent as DataControlFieldCell;

            //if Expand icon is invisible, set the cell container to invisible also for section 508
            if (dataCell4Comment != null && divLogo != null && divLogo.Visible == false)
            {
                dataCell4Comment.Visible = false;
            }

            //set Expand column visibile for section 508
            AccelaGridView currentGridView = sender as AccelaGridView;
            var dataCell4ExpandIcon = divLogo == null ? null : divLogo.Parent as DataControlFieldCell;

            if (currentGridView != null && dataCell4ExpandIcon != null && e.Row.RowIndex >= 0 && AccessibilityUtil.AccessibilityEnabled)
            {
                if (e.Row.RowIndex == 0)
                {
                    dataCell4ExpandIcon.ContainingField.Visible = false;
                }

                if (e.Row.RowIndex >= 0 && divLogo.Visible == true)
                {
                    dataCell4ExpandIcon.ContainingField.Visible = true;
                }
            }
        }

        /// <summary>
        /// Education row command event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event arguments.</param>
        protected void EducationList_ActionCommand(object sender, EventArgs e)
        {
            AccelaLinkButton actionButton = (AccelaLinkButton)sender;
            int dataItemIndex = Convert.ToInt32(actionButton.CommandArgument);

            switch (actionButton.CommandName)
            {
                case SELECT_EDUCATION:
                    SelectEducation(sender, dataItemIndex);
                    break;
                case DELETE_EDUCATION:
                    DeleteEducation(sender, dataItemIndex);
                    break;
                default:
                    gdvEducationList.DataSource = GridViewDataSource;
                    gdvEducationList.DataBind();
                    break;
            }
        }

        /// <summary>
        /// select Education event.
        /// </summary>
        /// <param name="sender">Education event object</param>
        /// <param name="dataItemIndex">Index for each education record in education list</param>
        private void SelectEducation(object sender, int dataItemIndex)
        {
            //Raise EducationSelected event
            if (EducationSelected != null)
            {
                EducationModel4WS educationModel = GridViewDataSource[dataItemIndex];
                SelectedIndex = Convert.ToInt32(educationModel.RowIndex);

                //execute selected event to populate Education information.
                object[] args = new object[] { SelectedIndex, educationModel };
                EducationSelected(sender, new CommonEventArgs(args));
            }
        }

        /// <summary>
        /// Remove a education from education list and raise the educations changed event.
        /// </summary>
        /// <param name="sender">Education event object</param>
        /// <param name="dataItemIndex">Index for each education record in education list</param>
        private void DeleteEducation(object sender, int dataItemIndex)
        {
            listPanel.FocusElement("lnkEducationDetail");

            if (GridViewDataSource != null)
            {
                try
                {
                    EducationModel4WS education = GridViewDataSource.First(o => o.RowIndex == dataItemIndex);
                    GridViewDataSource.Remove(education);

                    if (EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
                    {
                        if (!ValidationUtil.IsYes(education.approvedFlag))
                        {
                            ILicenseCertificationBll licenseCertificationBll =
                                ObjectFactory.GetObject<ILicenseCertificationBll>();
                            licenseCertificationBll.DeleteEducation(TempModelConvert.ConvertToEducationModel(education));
                        }
                    }

                    BindEducations(true);

                    if (EducationsDeleted != null)
                    {
                        //execute delete event to delete Education.
                        EducationsDeleted(sender, new CommonEventArgs(education));
                    }

                    DisplayDelActionNotice(true);
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex);
                    DisplayDelActionNotice(false);
                }
            }
        }

        /// <summary>
        /// Bind action menu.
        /// </summary>
        /// <param name="eventArgs">GridView Row Event Arguments</param>
        private void BuildActionMenu(GridViewRowEventArgs eventArgs)
        {
            EducationModel4WS education = eventArgs.Row.DataItem as EducationModel4WS;
            PopupActions actionMenu = eventArgs.Row.FindControl("actionMenu") as PopupActions;
            AccelaLinkButton lnkView = eventArgs.Row.FindControl("lnkView") as AccelaLinkButton;
            AccelaLinkButton lnkEdit = eventArgs.Row.FindControl("lnkEdit") as AccelaLinkButton;
            AccelaLinkButton lnkDelete = eventArgs.Row.FindControl("lnkDelete") as AccelaLinkButton;
            AccelaLinkButton lnkMajorDiscipline = eventArgs.Row.FindControl("lnkMajorDiscipine") as AccelaLinkButton;
            AccelaLabel lblRefEducationName = eventArgs.Row.FindControl("lblRefEducationName") as AccelaLabel;

            ActionViewModel actionView;
            var actionList = new List<ActionViewModel>();
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

            bool isAccountContactEditPage = EducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            bool isCapEdiPage = EducationSectionPosition == EducationOrExamSectionPosition.CapEdit;
            bool isCapConfimPage = EducationSectionPosition == EducationOrExamSectionPosition.CapConfirm;
            bool isCapDetailPage = EducationSectionPosition == EducationOrExamSectionPosition.CapDetail;

            bool isApprovedOrAssociated = ValidationUtil.IsYes(education.approvedFlag) || education.associatedEduCount > 0;
            bool isRefEdu = (isCapEdiPage || isCapConfimPage) && education.entityID.HasValue;

            /* ReadOnly mode:
             * 1. Cap detail page.
             * 2. The education has been approved or has been used by any cap.
             */
            if (isCapDetailPage || ((isAccountContactEditPage || isRefEdu) && isApprovedOrAssociated) || !ContactTypePermission)
            {
                lnkEdit.Visible = false;

                if (!isCapEdiPage)
                {
                    lnkDelete.Visible = false;
                }

                if (isCapConfimPage)
                {
                    lnkView.Visible = false;
                }

                lblRefEducationName.Visible = true;
                lnkMajorDiscipline.Visible = false;
            }
            else
            {
                lnkView.Visible = false;

                // Hide delete link when required field is 'Y' or in cap confirm page, or this section is disable.
                if (ValidationUtil.IsYes(education.requiredFlag) || isCapConfimPage || !IsEditable || ContactIsFromExternal)
                {
                    lnkDelete.Visible = false;
                }
            }

            if (lnkView.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_cap_detail_educationlist_label_view");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
                actionView.ActionId = actionMenu.ClientID + "_ViewDetails";

                long? eduId = education.educationPKModel.educationNbr ?? education.entityID;
                string url = FileUtil.AppendApplicationRoot(
                    "LicenseCertification/EducationDetail.aspx?" + UrlConstant.AgencyCode + "="
                    + education.educationPKModel.serviceProviderCode + "&eduId="
                    + eduId + "&" + ACAConstant.MODULE_NAME + "=" + this.ModuleName);

                if (isAccountContactEditPage)
                {
                    url += string.Format("&{0}={1}", UrlConstant.CONTACT_SEQ_NUMBER, education.contactSeqNumber);
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
                actionView.ActionLabel = GetTextByKey("aca_educationlist_label_lnkedit");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");
                actionView.ActionId = actionMenu.ClientID + "_Edit";

                string popupScript = string.Empty;

                if (isCapConfimPage)
                {
                    popupScript = "CallPostBackFunction('{0}');SetNotAskForSPEAR();";
                    lnkMajorDiscipline.OnClientClick = string.Format(popupScript, lnkMajorDiscipline.UniqueID);

                    actionView.ClientEvent = string.Format(popupScript, lnkEdit.UniqueID);
                }
                else
                {
                    popupScript = string.Format(
                        "openEducationFormDialog({{0}}, '{0}', '{1}', '{2}','{3}');return false;",
                        isAccountContactEditPage ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                        education.RowIndex,
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

                    lnkMajorDiscipline.OnClientClick = string.Format(popupScript, "this");
                }

                actionList.Add(actionView);
            }

            if (lnkDelete.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("education_delete");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                actionView.ActionId = actionMenu.ClientID + "_Delete";

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
                actionMenu.ActionLableKey = "aca_educationlist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        private void ChangeLabelKey()
        {
            if (GViewID == GviewID.RefContactEducationList)
            {
                GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvEducationList);

                ((IAccelaNonInputControl)headerRow.FindControl("lnkMajorDisciplineHeader")).LabelKey = "aca_contact_education_list_label_major_discipline";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderNameHeader")).LabelKey = "aca_contact_education_list_label_provider_name";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderNumberHeader")).LabelKey = "aca_contact_education_list_label_provider_number";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDegreeHeader")).LabelKey = "aca_contact_education_list_label_degree";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkYearAttendedHeader")).LabelKey = "aca_contact_education_list_label_attended";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkYearGraduatedHeader")).LabelKey = "aca_contact_education_list_label_graduateded";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkApprovedHeader")).LabelKey = "aca_contact_education_list_label_approved";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderAddress1Header")).LabelKey = "aca_contact_education_list_label_address1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderAddress2Header")).LabelKey = "aca_contact_education_list_label_address2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderAddress3Header")).LabelKey = "aca_contact_education_list_label_address3";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderCityHeader")).LabelKey = "aca_contact_education_list_label_city";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderStateHeader")).LabelKey = "aca_contact_education_list_label_state";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCountry")).LabelKey = "aca_contact_education_list_label_label_country";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderZipCodeHeader")).LabelKey = "aca_contact_education_list_label_zip";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderPhoneNumberHeader")).LabelKey = "aca_contact_education_list_label_phone1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkMobilePhoneHeader")).LabelKey = "aca_contact_education_list_label_phone2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFaxHeader")).LabelKey = "aca_contact_education_list_label_fax";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkEmailHeader")).LabelKey = "aca_contact_education_list_label_email";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRequiredHeader")).LabelKey = "aca_contact_education_list_label_required";
                ((IAccelaNonInputControl)headerRow.FindControl("lblActionHeader")).LabelKey = "aca_contact_education_list_label_action";
            }
        }
    }
}
