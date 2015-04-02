#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContinuingEducationList.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
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
    /// Display Continuing EducationList in daily side.
    /// </summary>
    public partial class ContinuingEducationList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// delete continuing education command name.
        /// </summary>
        protected const string DELETE_CONT_EDUCATION = "DeleteContEducation";

        /// <summary>
        /// The VIEW CON EDUCATION
        /// </summary>
        protected const string VIEW_CONT_EDUCATION = "ViewContEducation";

        /// <summary>
        /// select continuing education command name.
        /// </summary>
        protected const string SELECT_CONT_EDUCATION = "SelectContEducation";

        /// <summary>
        /// Column for required flag.
        /// </summary>
        private const string COLUMN_REQUIRED_FLAG = "requiredFlag";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ContinuingEducationList));
        
        /// <summary>
        /// selected event instance.
        /// </summary>
        public event CommonEventHandler ContEducationSelected;

        /// <summary>
        /// grid view row command event
        /// </summary>
        public event CommonEventHandler ContEducationsDeleted;

        /// <summary>
        /// Gets or sets grid view data source.
        /// </summary>
        public IList<ContinuingEducationModel4WS> GridViewDataSource
        {
            get
            {
                if (ViewState["ContEducationModels"] == null)
                {
                    ViewState["ContEducationModels"] = new List<ContinuingEducationModel4WS>();
                }

                return (List<ContinuingEducationModel4WS>)ViewState["ContEducationModels"];
            }

            set
            {
                ViewState["ContEducationModels"] = EducationUtil.AddRowIndex2ContEducationModel(value);
            }
        }

        /// <summary>
        /// Gets or sets the view id for con education list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvContEducationList.GridViewNumber;
            }

            set
            {
                gdvContEducationList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsEditable4ContEdu"]);
            }

            set
            {
                ViewState["IsEditable4ContEdu"] = value;
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
        public EducationOrExamSectionPosition ContEducationSectionPosition
        {
            get
            {
                return EnumUtil<EducationOrExamSectionPosition>.Parse(
                                    Convert.ToString(ViewState["ContEducationSectionPosition"]),
                                    EducationOrExamSectionPosition.None);
            }

            set
            {
                ViewState["ContEducationSectionPosition"] = value;

                switch (value)
                {
                    case EducationOrExamSectionPosition.AccountContactEdit:
                        tdExpandIcon.Visible = false;
                        GViewID = GviewID.RefContactContinuingEducationList;
                        break;
                    case EducationOrExamSectionPosition.CapEdit:
                    case EducationOrExamSectionPosition.CapConfirm:
                        GViewID = GviewID.SpearFormContinuingEducationList;
                        break;
                }

                GridViewBuildHelper.SetSimpleViewElements(gdvContEducationList, ModuleName, AppSession.IsAdmin);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact type has editable permission or not.
        /// </summary>
        private bool ContactTypePermission
        {
            get
            {
                if (!AppSession.IsAdmin && ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit)
                {
                    return ContactUtil.GetContactTypeEditablePermissionByContactSeqNbr(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
                }

                return true;
            }
        }

        #endregion Properties

        #region Public Method

        /// <summary>
        /// Bind continuing educations.
        /// </summary>
        public void BindContEducations()
        {
            gdvContEducationList.DataSource = AppSession.IsAdmin ? null : GridViewDataSource;
            gdvContEducationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvContEducationList.DataBind();
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
                lblActionNoticeAddSuccess.Text = msg;
                lblActionNoticeAddSuccess.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddSuccess, msg);
                }

                divImgSuccess.Visible = true;
            }
            else
            {
                lblActionNoticeAddFailed.Text = msg;
                lblActionNoticeAddFailed.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddFailed, msg);
                }

                divImgFailed.Visible = true;
            }

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Get continuing education model list and set it to session.
        /// </summary>
        /// <returns>continuing education model list.</returns>
        public ContinuingEducationModel4WS[] GetContEducationList()
        {
            return (GridViewDataSource as List<ContinuingEducationModel4WS>).ToArray();
        }

        /// <summary>
        /// Set the GridView required property
        /// </summary>
        /// <param name="isRequired">indicate if the GridView is required</param>
        public void SetGridViewRequired(bool isRequired)
        {
            gdvContEducationList.IsRequired = isRequired;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Hide required icon on cap detail page.
                if (ContEducationSectionPosition == EducationOrExamSectionPosition.CapDetail)
                {
                    gdvContEducationList.Columns[0].Visible = false;
                }
                else
                {
                    gdvContEducationList.Columns[1].Visible = false;
                }

                ControlBuildHelper.SetInstructionValue(this.lblContEducationTitle_sub_label, GetTextByKey("per_conteducationlist_title|sub"));

                if (!AppSession.IsAdmin && ContEducationSectionPosition == EducationOrExamSectionPosition.CapEdit)
                {
                    int rowIndex = 0;

                    if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_CONFIRMPAGE]) &&
                        PageFlowConstant.SECTION_NAME_CONT_EDUCATION.Equals(Request.QueryString[UrlConstant.SECTION_NAME], StringComparison.InvariantCultureIgnoreCase) &&
                        int.TryParse(Request.QueryString[UrlConstant.ROW_INDEX], out rowIndex))
                    {
                        if (rowIndex > gdvContEducationList.PageSize)
                        {
                            gdvContEducationList.SetPageIndex(rowIndex / gdvContEducationList.PageSize);
                            gdvContEducationList.DataBind();
                        }

                        if (AppSession.IsEditFromConfirmFlag)
                        {
                            var editRow = gdvContEducationList.Rows[rowIndex % gdvContEducationList.PageSize];
                            AccelaLinkButton lnkContEduName = (AccelaLinkButton)editRow.FindControl("btnContEducationName");
                            AccelaLinkButton lnkEdit = (AccelaLinkButton)editRow.FindControl("lnkEdit");
                            PopupActions actionMenu = (PopupActions)editRow.FindControl("actionMenu");

                            string script = string.Format("openEduExamEditDialog('{0}', '{1}', '{2}');", lnkContEduName.ClientID, lnkEdit.ClientID, actionMenu.ActionsLinkClientID);
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
        /// <param name="e">Event Argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ChangeLabelKey();
        }

        /// <summary>
        /// Continuing education row bind event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">Event Argument.</param>
        protected void ContEducationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            BuildActionMenu(e);

            AccelaLabel lblRequiredOptional = (AccelaLabel)e.Row.FindControl("lblRequiredOptional");
            AccelaDiv divImg = (AccelaDiv)e.Row.FindControl("divImg");
            AccelaDiv divLogo = (AccelaDiv)e.Row.FindControl("divLogo");
            HiddenField hdnGradingStyle = (HiddenField)e.Row.FindControl("hdnGradingStyle");
            AccelaLabel lblFinalScore = (AccelaLabel)e.Row.FindControl("lblFinalScore");
            AccelaLabel lblApproved = (AccelaLabel)e.Row.FindControl("lblApproved");
            AccelaLabel lblPassingScore = (AccelaLabel)e.Row.FindControl("lblPassingScore");

            ContinuingEducationModel4WS contEducation = (ContinuingEducationModel4WS)e.Row.DataItem;
            bool isCapDetailPage = ContEducationSectionPosition == EducationOrExamSectionPosition.CapDetail;
            bool isAccountContactEditPage = ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            bool isApprovedOrAssociated = ValidationUtil.IsYes(contEducation.approvedFlag) || contEducation.associatedContEduCount > 0;

            string requiredOptional = DataBinder.Eval(e.Row.DataItem, COLUMN_REQUIRED_FLAG).ToString();
            lblRequiredOptional.Text = EducationUtil.ConvertRequiredField2Display(requiredOptional);
            lblApproved.Text = ValidationUtil.IsYes(contEducation.approvedFlag) ? ACAConstant.COMMON_Yes : ACAConstant.COMMON_No;

            //validate comments is empty or null to hide logo.
            if (string.IsNullOrEmpty(contEducation.comments))
            {
                divLogo.Visible = false;
            }

            if (isCapDetailPage)
            {
                //Hide mark incomplete record logo in CAP detail page.
                divImg.Visible = false;
            }
            else
            {
                string refEduID = contEducation.RefConEduNbr;

                if (string.IsNullOrEmpty(refEduID) && !isAccountContactEditPage)
                {
                    RefContinuingEducationModel4WS refContEducation = EducationUtil.GetRefContinuingEducationModel(ConfigManager.AgencyCode, contEducation.contEduName);
                    refEduID = refContEducation == null ? string.Empty : refContEducation.refContEduNbr.ToString();
                }

                //Validate all of required fields have value for each row in Continuing Education list.
                string viewId = isAccountContactEditPage ? GviewID.RefContactContinuingEducationEdit : GviewID.ContinuingEducationEdit;
                GFilterScreenPermissionModel4WS permission = ControlBuildHelper.GetPermissionWithGenericTemplate(viewId, GViewConstant.SECTION_CONTINUING_EDUCATION, refEduID, contEducation.template);

                if (!isApprovedOrAssociated && IsEditable && (!RequiredValidationUtil.ValidateFields4Object(ModuleName, permission, viewId, contEducation)
                    || !RequiredValidationUtil.ValidateFields4GenericTemplate(contEducation.template, ModuleName)))
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

            var divComment = e.Row.FindControl("commentPanel");
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

                if (e.Row.RowIndex >= 0 && divLogo.Visible)
                {
                    dataCell4ExpandIcon.ContainingField.Visible = true;
                }
            }

            //format display final socre.
            if (hdnGradingStyle != null)
            {
                if (lblFinalScore != null)
                {
                    lblFinalScore.Text = EducationUtil.FormatScore(hdnGradingStyle.Value, lblFinalScore.Text);
                }

                if (lblPassingScore != null)
                {
                    lblPassingScore.Text = EducationUtil.FormatScore(hdnGradingStyle.Value, lblPassingScore.Text, true);
                }
            }
        }

        /// <summary>
        /// Continuing Education row command event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void ContEducationList_ActionCommand(object sender, EventArgs e)
        {
            AccelaLinkButton actionButton = (AccelaLinkButton)sender;
            int contEducationIndex = Convert.ToInt32(actionButton.CommandArgument);

            switch (actionButton.CommandName)
            {
                case SELECT_CONT_EDUCATION:
                    if (ContEducationSelected != null)
                    {
                        ContinuingEducationModel4WS selectedContEducation = GridViewDataSource.Single(o => o.RowIndex == Convert.ToInt32(contEducationIndex));

                        if (selectedContEducation != null)
                        {
                            object[] args = new object[] { contEducationIndex, selectedContEducation };
                            ContEducationSelected(sender, new CommonEventArgs(args));
                        }
                    }

                    break;
                case DELETE_CONT_EDUCATION:
                    DeleteContEducation(sender, contEducationIndex);

                    break;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Bind action menu.
        /// </summary>
        /// <param name="e">GridView Row Event Arguments</param>
        private void BuildActionMenu(GridViewRowEventArgs e)
        {
            ContinuingEducationModel4WS contEducation = (ContinuingEducationModel4WS)e.Row.DataItem;
            PopupActions actionMenu = e.Row.FindControl("actionMenu") as PopupActions;
            AccelaLinkButton lnkEdit = (AccelaLinkButton)e.Row.FindControl("lnkEdit");
            AccelaLinkButton lnkDelete = (AccelaLinkButton)e.Row.FindControl("lnkDelete");
            AccelaLinkButton lnkView = (AccelaLinkButton)e.Row.FindControl("lnkView");
            AccelaLinkButton lnkContEducationName = (AccelaLinkButton)e.Row.FindControl("btnContEducationName");
            AccelaLabel lblContEducationName = (AccelaLabel)e.Row.FindControl("lblContEducationName");

            ActionViewModel actionView;
            var actionList = new List<ActionViewModel>();

            bool isCapDetailPage = ContEducationSectionPosition == EducationOrExamSectionPosition.CapDetail;
            bool isCapEditPage = ContEducationSectionPosition == EducationOrExamSectionPosition.CapEdit;
            bool isAccountContactEditPage = ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit;
            bool isCapConfirmPage = ContEducationSectionPosition == EducationOrExamSectionPosition.CapConfirm;

            bool isApprovedOrAssociated = ValidationUtil.IsYes(contEducation.approvedFlag) || contEducation.associatedContEduCount > 0;
            bool isRefContEdu = (isCapEditPage || isCapConfirmPage) && !string.IsNullOrEmpty(contEducation.entityID);
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

            /* ReadOnly mode:
             * 1. Cap detail page.
             * 2. The continuing education has been approved or has been used by any cap.
             */
            if (isCapDetailPage || ((isAccountContactEditPage || isRefContEdu) && isApprovedOrAssociated) || !ContactTypePermission)
            {
                lnkEdit.Visible = false;
                lnkContEducationName.Visible = false;
                lblContEducationName.Visible = true;

                if (!isCapEditPage)
                {
                    lnkDelete.Visible = false;
                }

                if (isCapConfirmPage)
                {
                    lnkView.Visible = false;
                }
            }
            else
            {
                lnkView.Visible = false;

                // Hide delete link when required field is 'Y' or in cap confirm page, or this section is disable.
                if (ValidationUtil.IsYes(contEducation.requiredFlag) || isCapConfirmPage || !IsEditable || ContactIsFromExternal)
                {
                    lnkDelete.Visible = false;
                }
            }

            if (lnkView.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_cap_detail_contedulist_label_view");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
                actionView.ActionId = actionMenu.ClientID + "_ViewDetails";

                long? contEduNbr = contEducation.continuingEducationPKModel.contEduNbr;

                if (!contEduNbr.HasValue && !string.IsNullOrEmpty(contEducation.entityID))
                {
                    contEduNbr = long.Parse(contEducation.entityID);
                }

                string url = FileUtil.AppendApplicationRoot(string.Format(
                        "LicenseCertification/ContinuingEducationDetail.aspx?{0}={1}&conEduId={2}&{3}={4}",
                        UrlConstant.AgencyCode,
                        contEducation.continuingEducationPKModel.serviceProviderCode,
                        contEduNbr,
                        ACAConstant.MODULE_NAME,
                        ModuleName));

                if (isAccountContactEditPage)
                {
                    url += string.Format("&{0}={1}", UrlConstant.CONTACT_SEQ_NUMBER, contEducation.contactSeqNumber);
                }

                if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
                {
                    actionView.ClientEvent = string.Format("popUpDetailDialog('{0}','{1}')", url, actionView.ActionId);
                }
                else
                {
                    actionView.ClientEvent = string.Format("popUpDetailDialog('{0}','{1}')", url, actionMenu.ActionsLinkClientID);
                }

                actionList.Add(actionView);
            }

            if (lnkEdit.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_contuingeducationlist_label_lnkedit");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");
                actionView.ActionId = actionMenu.ClientID + "_Edit";

                string popupScript = string.Empty;

                if (isCapConfirmPage)
                {
                    popupScript = "CallPostBackFunction('{0}');SetNotAskForSPEAR();";
                    lnkContEducationName.OnClientClick = string.Format(popupScript, lnkContEducationName.UniqueID);
                    actionView.ClientEvent = string.Format(popupScript, lnkEdit.UniqueID);
                }
                else
                {
                    popupScript = string.Format(
                           "openContEducationFormDialog({{0}}, '{0}', '{1}', '{2}', '{3}' ,'{4}');return false;",
                           isAccountContactEditPage ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                           "edit",
                           contEducation.RowIndex,
                           IsEditable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                           ContactIsFromExternal ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);

                    string actionLinkObj = actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase) 
                                            ? actionView.ActionId 
                                            : "$get('" + actionMenu.ActionsLinkClientID + "')";

                    lnkEdit.OnClientClick = string.Format(popupScript, actionLinkObj);
                    actionView.ClientEvent = string.Format(popupScript, actionLinkObj);

                    lnkContEducationName.OnClientClick = string.Format(popupScript, "this");
                }

                actionList.Add(actionView);
            }

            if (lnkDelete.Visible)
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("per_conteducationlist_delete");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                actionView.ActionId = actionMenu.ClientID + "_Delete";

                actionView.ClientEvent = string.Format(
                                                       "return TriggerEventWithConfirm('{0}', '{1}');",
                                                       lnkDelete.UniqueID,
                                                       GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'"));

                actionList.Add(actionView);
            }

            if (actionList.Count > 0)
            {
                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_conteducationlist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        private void ChangeLabelKey()
        {
            if (GViewID == GviewID.RefContactContinuingEducationList)
            {
                GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvContEducationList);

                ((IAccelaNonInputControl)headerRow.FindControl("lnkContEducationName")).LabelKey = "aca_contact_continuing_education_list_label_name";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderName")).LabelKey = "aca_contact_continuing_education_list_label_provider_name";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkProviderNumber")).LabelKey = "aca_contact_continuing_education_list_label_provider_number";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkClass")).LabelKey = "aca_contact_continuing_education_list_label_class";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDateOfClass")).LabelKey = "aca_contact_continuing_education_list_label_date_class";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCompletedHours")).LabelKey = "aca_contact_continuing_education_list_label_completedhours";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFinalScore")).LabelKey = "aca_contact_continuing_education_list_label_final_score";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPassingScore")).LabelKey = "aca_contact_continuing_education_list_label_passing_score";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkApprovedHeader")).LabelKey = "aca_contact_continuing_education_list_label_approved";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRequiredOptional")).LabelKey = "aca_contact_continuing_education_list_label_required_option";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddress1")).LabelKey = "aca_contact_continuing_education_list_label_address1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddress2")).LabelKey = "aca_contact_continuing_education_list_label_address2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddress3")).LabelKey = "aca_contact_continuing_education_list_label_address3";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCity")).LabelKey = "aca_contact_continuing_education_list_label_city";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkState")).LabelKey = "aca_contact_continuing_education_list_label_state";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkZip")).LabelKey = "aca_contact_continuing_education_list_label_zip";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCountry")).LabelKey = "aca_contact_continuing_education_list_label_country";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPhoneNumber1")).LabelKey = "aca_contact_continuing_education_list_label_phone1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPhoneNumber2")).LabelKey = "aca_contact_continuing_education_list_label_phone2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFax")).LabelKey = "aca_contact_continuing_education_list_label_fax";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkEmail")).LabelKey = "aca_contact_continuing_education_list_label_email";
                ((IAccelaNonInputControl)headerRow.FindControl("lblActionHeader")).LabelKey = "aca_contact_continuing_education_list_label_action";
            }
        }

        /// <summary>
        /// Remove a education from education list and raise the educations changed event.
        /// </summary>
        /// <param name="sender">Education event object</param>
        /// <param name="dataItemIndex">Index for each education record in education list</param>
        private void DeleteContEducation(object sender, int dataItemIndex)
        {
            listPanel.FocusElement("lnkContEducationDetail");

            if (GridViewDataSource == null)
            {
                return;
            }

            try
            {
                ContinuingEducationModel4WS contEducation = GridViewDataSource.First(o => o.RowIndex == dataItemIndex);
                GridViewDataSource.Remove(contEducation);
                EducationUtil.AddRowIndex2ContEducationModel(GridViewDataSource);

                if (ContEducationSectionPosition == EducationOrExamSectionPosition.AccountContactEdit &&
                    !ValidationUtil.IsYes(contEducation.approvedFlag))
                {
                    ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
                    licenseCertificationBll.DeleteContinuingEducation(TempModelConvert.ConvertToContEducationModel(contEducation));
                }

                BindContEducations();

                if (ContEducationsDeleted != null)
                {
                    //execute delete event to delete ContinuingEducation.
                    ContEducationsDeleted(sender, new CommonEventArgs(contEducation));
                }

                DisplayDelActionNotice(true);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                DisplayDelActionNotice(false);
            }
        }

        #endregion Private Method
    }
}
