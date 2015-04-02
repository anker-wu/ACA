#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionListUpcoming.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Provide the upcoming inspection list.
 *
 *  Notes:
 *      $Id: InspectionListUpcoming.ascx.cs 178037 2010-09-06 06:25:12Z ACHIEVO\daly.zeng $.
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

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the upcoming inspection list. 
    /// </summary>
    public partial class InspectionList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(InspectionList));

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data source of all list.
        /// </summary>
        public List<InspectionListItemViewModel> AllListGridViewDataSource
        {
            get
            {
                return AppSession.InspectionData;
            }

            set
            {
                AppSession.InspectionData = value;
            }
        }

        /// <summary>
        /// Gets or sets the Cap ID1
        /// </summary>
        public string CapID1
        {
            get
            {
                if (ViewState["CapID1"] != null)
                {
                    return ViewState["CapID1"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["CapID1"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Cap ID2
        /// </summary>
        public string CapID2
        {
            get
            {
                if (ViewState["CapID2"] != null)
                {
                    return ViewState["CapID2"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["CapID2"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Cap ID3
        /// </summary>
        public string CapID3
        {
            get
            {
                if (ViewState["CapID3"] != null)
                {
                    return ViewState["CapID3"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["CapID3"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the service provider code
        /// </summary>
        public string ServiceProviderCode
        {
            get
            {
                if (ViewState["ServiceProviderCode"] != null)
                {
                    return ViewState["ServiceProviderCode"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["ServiceProviderCode"] = value;
            }
        }

        /// <summary>
        /// Gets or sets current user.
        /// </summary>
        /// <value>The current user.</value>
        public User CurrentUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                if (ViewState["IsEditable"] == null)
                {
                    return true;
                }

                return Convert.ToBoolean(ViewState["IsEditable"]);
            }

            set
            {
                ViewState["IsEditable"] = value;
            }
        }

        #endregion

        /// <summary>
        /// Initial inspection list
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        public void InitList(CapModel4WS recordModel)
        {
            if (recordModel != null && recordModel.capID != null)
            {
                // set the cap id model to viewstate
                CapID1 = recordModel.capID.id1;
                CapID2 = recordModel.capID.id2;
                CapID3 = recordModel.capID.id3;
                ServiceProviderCode = recordModel.capID.serviceProviderCode;
            }
        }

        /// <summary>
        /// Bind the inspection list.
        /// </summary>
        /// <param name="bindEmpty">if set to <c>true</c> [bind empty].</param>
        public void BindInspectionList(bool bindEmpty)
        {
            if (!bindEmpty)
            {
                CapModel4WS recordModel = GetCapModel();
                AllListGridViewDataSource = GetListItemViewModels(recordModel);
            }

            BindUpcomingList(bindEmpty);
            BindCompletedList(bindEmpty);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (AppSession.IsAdmin)
                {
                    divUpcomingComboField.Visible = true;
                    divCompletedComboField.Visible = true;
                }

                if (!Page.IsPostBack && this.Visible)
                {
                    BindInspectionList(true);
                    SetlnspectionScheduleLinkVisible();
                }
            }
            catch (Exception ex)
            {
                HandleExecption(ex);
            }
        }

        /// <summary>
        /// GridView ListUpcoming RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs object</param>
        protected void ListUpcoming_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (inspectionUpdatePanel.Visible)
            {
                try
                {
                    BindListAction(e, false);
                }
                catch (Exception ex)
                {
                    HandleExecption(ex);
                }
            }
        }

        /// <summary>
        /// GridView ListCompleted RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs object</param>
        protected void ListCompleted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (inspectionUpdatePanel.Visible)
            {
                try
                {
                    BindListAction(e, true);
                }
                catch (Exception ex)
                {
                    HandleExecption(ex);
                }
            }
        }

        /// <summary>
        /// RefreshGridView Button Click event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs object</param>
        protected void RefreshGridViewButton_Click(object sender, EventArgs e)
        {
            if (inspectionUpdatePanel.Visible)
            {
                try
                {
                    BindInspectionList(false);
                }
                catch (Exception ex)
                {
                    HandleExecption(ex);
                }
            }
        }

        /// <summary>
        /// Gets the schedule or request url used by UI page.
        /// </summary>
        /// <returns>Return the schedule or request url used by UI page.</returns>
        protected string GetScheduleOrRequestUrl()
        {
            InspectionParameter parameter = new InspectionParameter();
            parameter.ModuleName = ModuleName;
            parameter.AgencyCode = ServiceProviderCode;
            parameter.RecordID1 = CapID1;
            parameter.RecordID2 = CapID2;
            parameter.RecordID3 = CapID3;
            parameter.IsPopupPage = ACAConstant.COMMON_Y;

            string urlQueryString = InspectionParameterUtil.BuildQueryString(parameter);

            return string.Format("{0}?{1}", FileUtil.AppendApplicationRoot("Inspection/InspectionWizardInputType.aspx"), urlQueryString);
        }

        /// <summary>
        /// Set Inspections the schedule link visible.
        /// </summary>
        private void SetlnspectionScheduleLinkVisible()
        {
            if (!AppSession.IsAdmin)
            {
                if (!FunctionTable.IsEnableScheduleInspection() || !IsEditable)
                {
                    divInspectionScheduleLink.Visible = false;
                    return;
                }

                var recordModel = GetCapModel();
                divInspectionScheduleLink.Visible = InspectionViewUtil.CanShowScheduleLink(ServiceProviderCode, ModuleName, AppSession.User, recordModel);
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The exception object.</param>
        private void HandleExecption(Exception ex)
        {
            inspectionUpdatePanel.Visible = false;
            Logger.Error(ex);
            MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            string errorMessage4JavaScript = MessageUtil.FilterQuotation(ex.Message, true);
            string scriptToHideLoading = string.Format("HandleExeception('{0}');", errorMessage4JavaScript);
            string keyofScriptToHideLoading = this.ID + "keyofScriptToHideLoading";
            ScriptManager.RegisterStartupScript(this, this.GetType(), keyofScriptToHideLoading, scriptToHideLoading, true);
        }

        /// <summary>
        /// Get Cap model.
        /// </summary>
        /// <returns>Return the Cap model.</returns>
        private CapModel4WS GetCapModel()
        {
            CapModel4WS capModel = null;

            // get the cap id model
            CapIDModel4WS capIDModel = new CapIDModel4WS();
            capIDModel.id1 = CapID1;
            capIDModel.id2 = CapID2;
            capIDModel.id3 = CapID3;
            capIDModel.serviceProviderCode = ServiceProviderCode;

            if (capIDModel != null)
            {
                CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capIDModel, AppSession.User.UserSeqNum, true);

                if (capWithConditionModel != null)
                {
                    capModel = capWithConditionModel.capModel;
                }
            }

            return capModel;
        }

        /// <summary>
        /// Bind the list action control.
        /// </summary>
        /// <param name="e">GridViewRowEventArgs object</param>
        /// <param name="isForCompletedList">if set to <c>true</c> [is for completed list].</param>
        private void BindListAction(GridViewRowEventArgs e, bool isForCompletedList)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var availableActions = ((InspectionListItemViewModel)e.Row.DataItem).AvailableActions;

                if (isForCompletedList)
                {
                    AccelaLabel lblOnlyOneAction = e.Row.FindControl("lblOnlyOneAction") as AccelaLabel;

                    var availableAction =
                        availableActions == null 
                        ? (InspectionActionViewModel)null 
                        : (from a in availableActions
                           where a.Action == InspectionAction.ViewDetails
                           select a).FirstOrDefault();

                    bool readyToDisplayViewDetailsOnly = lblOnlyOneAction != null && availableAction != null;

                    if (readyToDisplayViewDetailsOnly)
                    {
                        //preserve such code for enhancement.
                        Random rd = new Random();
                        string lnkOnlyOneAction = Convert.ToString(rd.Next());

                        string cssClassName = availableAction.Action == InspectionAction.ViewDetails 
                                                                        ? "InspectionDetailsPageWidth" 
                                                                        : "InspectionWizardPageWidth";

                        string targetURL = string.Format(
                                                         "showInspectionPopupDialog('{0}','{1}','{2}')", 
                                                         availableAction.TargetURL, 
                                                         cssClassName, 
                                                         lnkOnlyOneAction);

                        lblOnlyOneAction.Text = string.Format(
                                                              "<a id='{0}' href='javascript:void(0)' onclick=\"{1}\" title='{2}'>{2}</a>", 
                                                              lnkOnlyOneAction, 
                                                              targetURL, 
                                                              availableAction.ActionLabel);
                    }
                    else if (lblOnlyOneAction != null)
                    {
                        //set invisible when not ready to display view details only
                        lblOnlyOneAction.Visible = false;
                    }
                }
                else
                {
                    var actionMenu = e.Row.FindControl("paActionMenu") as PopupActions;
                    BuildActionMenu(actionMenu, availableActions);
                }
            }
        }

        /// <summary>
        /// Builds the action menu.
        /// </summary>
        /// <param name="actionMenu">The action menu.</param>
        /// <param name="actionViewModelArray">The action view model array.</param>
        private void BuildActionMenu(PopupActions actionMenu, InspectionActionViewModel[] actionViewModelArray)
        {
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

            if (actionViewModelArray != null)
            {
                var actionViewModelList = new List<ActionViewModel>();
                foreach (InspectionActionViewModel actionModel in actionViewModelArray)
                {
                    if (!IsEditable && actionModel.Action != InspectionAction.ViewDetails)
                    {
                        continue;
                    }

                    bool canSchedule = FunctionTable.IsEnableScheduleInspection();
                    bool isViewDetailsAction = actionModel.Action == InspectionAction.ViewDetails;

                    if (actionModel.Visible && (canSchedule || isViewDetailsAction))
                    {
                        var actinoViewModel = new ActionViewModel();
                        string objectTargetId = string.Empty;

                        actinoViewModel.ActionLabel = actionModel.ActionLabel;
                        actinoViewModel.ActionId = actionMenu.ActionsLinkClientID + "_" + CommonUtil.GetRandomUniqueID();
                        actinoViewModel.IcoUrl = GetIcoUrlByInspectionAction(actionModel.Action);
                        string cssClassName = isViewDetailsAction ? "InspectionDetailsPageWidth" : "InspectionWizardPageWidth";

                        if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
                        {
                            objectTargetId = actinoViewModel.ActionId;
                        }
                        else
                        {
                            objectTargetId = InspectionAction.Cancel.Equals(actionModel.Action) ? "lnkInspectionSchedule" : actionMenu.ActionsLinkClientID;
                        }

                        actinoViewModel.ClientEvent = string.Format("showInspectionPopupDialog('{0}','{1}','{2}')", actionModel.TargetURL, cssClassName, objectTargetId);
                        actionViewModelList.Add(actinoViewModel);
                    }
                }

                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_inspection_actions_link";
                actionMenu.AvailableActions = actionViewModelList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Get the icon url by inspection action
        /// </summary>
        /// <param name="action">action value</param>
        /// <returns>icon url</returns>
        private string GetIcoUrlByInspectionAction(InspectionAction action)
        {
            string iconUrl = string.Empty;

            switch (action)
            {
                case InspectionAction.ViewDetails:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_view.png");
                        break;
                    }

                case InspectionAction.Reschedule:
                case InspectionAction.RestrictedReschedule:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_reschedule.png");
                        break;
                    }

                case InspectionAction.Cancel:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_cancel.png");
                        break;
                    }

                case InspectionAction.Schedule:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_schedule.png");
                        break;
                    }

                case InspectionAction.Request:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_request.png");
                        break;
                    }

                case InspectionAction.DoComplete:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_do_complete.png");
                        break;
                    }

                case InspectionAction.PrintDetails:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_print_details.png");
                        break;
                    }

                case InspectionAction.RestrictedCancel:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_retricted_cancel.png");
                        break;
                    }

                case InspectionAction.DoPrerequisiteNotMet:
                    {
                        iconUrl = ImageUtil.GetImageURL("popaction_do_perequisite_not_met.png");
                        break;
                    }
            }

            return iconUrl;
        }

        /// <summary>
        /// Bind Upcoming list control.
        /// </summary>
        /// <param name="bindEmpty">if set to <c>true</c> [bind empty].</param>
        private void BindUpcomingList(bool bindEmpty)
        {
            List<InspectionListItemViewModel> upcomingGridViewDataSource = null;

            if (!AppSession.IsAdmin && !bindEmpty)
            {
                upcomingGridViewDataSource = InspectionListItemViewUtil.GetUpcomingViewModels(AllListGridViewDataSource);
            }

            if (!AppSession.IsAdmin)
            {
                // set the title
                string labelUpcoming = GetTextByKey("aca_inspection_upcoming_label");

                if (upcomingGridViewDataSource != null && upcomingGridViewDataSource.Count > 0)
                {
                    lblInspectionUpcoming.Text = string.Format(GetTextByKey("aca_inspection_title_with_number_label"), labelUpcoming, upcomingGridViewDataSource.Count);
                }
                else
                {
                    lblInspectionUpcoming.Text = labelUpcoming;
                }
            }

            // bind data source
            gvListUpcoming.DataSource = upcomingGridViewDataSource;
            gvListUpcoming.DataBind();
        }

        /// <summary>
        /// Bind completed list control.
        /// </summary>
        /// <param name="bindEmpty">if set to <c>true</c> [bind empty].</param>
        private void BindCompletedList(bool bindEmpty)
        {
            List<InspectionListItemViewModel> completedGridViewDataSource = null;

            if (!AppSession.IsAdmin && !bindEmpty)
            {
                completedGridViewDataSource = InspectionListItemViewUtil.GetCompletedViewModels(AllListGridViewDataSource);
            }

            // set the title
            string labelCompleted = GetTextByKey("aca_inspection_completed_label");

            if (completedGridViewDataSource != null && completedGridViewDataSource.Count > 0)
            {
                lblInspectionCompleted.Text = string.Format(GetTextByKey("aca_inspection_title_with_number_label"), labelCompleted, completedGridViewDataSource.Count);

                // set the staus record count list
                Dictionary<string, int> dict = InspectionListItemViewUtil.GetResultedStatusStatistics(completedGridViewDataSource);
                string statusRecordCount = string.Empty;

                foreach (string status in dict.Keys)
                {
                    statusRecordCount += string.Format("{0} - {1}; ", status, dict[status]);
                }

                if (!string.IsNullOrEmpty(statusRecordCount))
                {
                    lblInspectionStatusRecordCount.Text = statusRecordCount.Substring(0, statusRecordCount.Length - 2);
                }

                divInspectionStatusRecordCount.Visible = true;
            }
            else
            {
                if (!AppSession.IsAdmin)
                {
                    lblInspectionCompleted.Text = labelCompleted;
                }

                divInspectionStatusRecordCount.Visible = false;
            }

            // bind data source
            gvListCompleted.DataSource = completedGridViewDataSource;
            gvListCompleted.DataBind();
        }

        /// <summary>
        /// Gets the inspection list item view models.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <returns>the data source</returns>
        private List<InspectionListItemViewModel> GetListItemViewModels(CapModel4WS recordModel)
        {
            List<InspectionListItemViewModel> result = new List<InspectionListItemViewModel>();

            if (recordModel != null && !AppSession.IsAdmin)
            {
                var viewModels = GetInspectionViewModels(ModuleName, recordModel);
                CapIDModel recordIDModel = recordModel == null ? null : TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);

                result = InspectionListItemViewUtil.BuildViewModels(ServiceProviderCode, recordIDModel, ModuleName, CurrentUser.PublicUserId, viewModels);
            }

            return result;
        }

        /// <summary>
        /// Gets the inspection view models.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <returns>the view models.</returns>
        private List<InspectionViewModel> GetInspectionViewModels(string moduleName, CapModel4WS recordModel)
        {
            var results = new List<InspectionViewModel>();
            var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();

            if (recordModel != null && !AppSession.IsAdmin)
            {
                // get inspection list base on Cap ID
                bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(recordModel.capID, CurrentUser.UserSeqNum);

                var currentUser = CurrentUser.UserModel4WS;
                var dataModelList = inspectionBll.GetDataModels(moduleName, recordModel, isCapLockedOrHold, currentUser);

                if (dataModelList != null)
                {
                    foreach (InspectionDataModel dataModel in dataModelList)
                    {
                        var viewModel = InspectionViewUtil.BuildViewModel(moduleName, dataModel);
                        results.Add(viewModel);
                    }
                }
            }

            return results;
        }
    }
}