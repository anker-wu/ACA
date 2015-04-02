#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExamList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description: Provide the examination list.
 *
 *  Notes:  
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Exam List Component
    /// </summary>
    public partial class ExamList : BaseUserControl
    {
        #region Events

        /// <summary>
        /// delegate for refresh contact
        /// </summary>
        public delegate void RefreshContact();

        /// <summary>
        /// Occurs when [refresh cap contact].
        /// </summary>
        public event RefreshContact RefreshCapContact;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the data source of all list.
        /// </summary>
        public List<ExaminationListItemViewModel> GridViewDataSource
        {
            get
            {
                return (List<ExaminationListItemViewModel>)ViewState["ExaminationListItemViewModel"];
            }

            set
            {
                ViewState["ExaminationListItemViewModel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has primary contact.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has primary contact; otherwise, <c>false</c>.
        /// </value>
        public bool HasPrimaryContact
        {
            get
            {
                if (ViewState["HasPrimaryContact"] == null)
                {
                    return false;
                }

                return bool.Parse(ViewState["HasPrimaryContact"].ToString());
            }

            set
            {
                ViewState["HasPrimaryContact"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the record primary contact.
        /// </summary>
        /// <value>The record primary contact.</value>
        public PeopleModel4WS RecordPrimaryContact
        {
            get
            {
                return ViewState["RecordPrimaryContact"] as PeopleModel4WS;
            }

            set
            {
                ViewState["RecordPrimaryContact"] = value;
            }
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

        #region Method

        /// <summary>
        /// Bind the examination list.
        /// </summary>
        public void BindExaminationList()
        {
            if (!AppSession.IsAdmin)
            {
                CapModel4WS recordModel = AppSession.GetCapModelFromSession(ModuleName);

                CapContactModel4WS contactModel = ExaminationScheduleUtil.GetCapPrimaryContact(recordModel);

                if (contactModel != null && contactModel.people != null)
                {
                    HasPrimaryContact = true;
                }
                else
                {
                    HasPrimaryContact = false;
                }

                RecordPrimaryContact = contactModel != null ? contactModel.people : null;
                GridViewDataSource = GetListItemViewModels(recordModel);
            }

            BindPendingList();
            BindScheduledList();
            BindCompletedList();
            BindReadyToScheduleList();
        }

        /// <summary>
        /// Page initial on loading
        /// </summary>
        /// <param name="sender">object sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (AppSession.IsAdmin)
                {
                    divScheduledComboField.Visible = true;
                    divCompletedComboField.Visible = true;
                    divPendingComboField.Visible = true;
                    divReadyToScheduleComboField.Visible = true;
                }

                if (!Page.IsPostBack && !AppSession.IsAdmin)
                {
                    var recordModel = AppSession.GetCapModelFromSession(ModuleName);         
                }

                if (!IsEditable)
                {
                    divExaminationScheduleLink.Visible = false;
                }
            }
            catch (Exception ex)
            {
                HandleExecption(ex);
            }
        }

        /// <summary>
        /// Grid view scheduled examination row data bound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs object</param>
        protected void ListScheduled_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (examinationUpdatePanel.Visible)
            {
                BindListAction(e);
            }
        }

        /// <summary>
        /// GridView ListCompleted RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs object</param>
        protected void ListCompleted_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (examinationUpdatePanel.Visible)
            {
                BindListAction(e);
            }
        }

        /// <summary>
        /// Gets the schedule or request url used by UI page.
        /// </summary>
        /// <returns>Return the schedule or request url used by UI page.</returns>
        protected string GetScheduleOrRequestUrl()
        {
            string urlQueryString = GetUrlQueryString(null, false);

            return string.Format("{0}?{1}", FileUtil.AppendApplicationRoot("Examination/AvailableExaminationList.aspx"), urlQueryString);
        }

        /// <summary>
        /// RefreshGridView Button Click event handler
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs object</param>
        protected void RefreshExamGVButton_Click(object sender, EventArgs e)
        {
            if (examinationUpdatePanel.Visible)
            {
                try
                {
                    UpdateCapExaminationInSession();
                    BindExaminationList();

                    if (RefreshCapContact != null)
                    {
                        RefreshCapContact();
                    }
                }
                catch (Exception ex)
                {
                    HandleExecption(ex);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the RedirectPage button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void RedirctPageButton_Click(object sender, EventArgs e)
        {
            string url = string.Format(
                "~/Cap/CapFees.aspx?permitType=PayFees&Module={0}&stepNumber=0&isPay4ExistingCap={1}&{2}={3}", 
                ModuleName,
                ACAConstant.COMMON_Y,
                UrlConstant.IS_FROM_EXAM_SCHEDULE,
                ACAConstant.COMMON_Y);

            Response.Redirect(url);
        }

        /// <summary>
        /// Bind the list action control.
        /// </summary>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        private void BindListAction(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var examinationListItem = (ExaminationListItemViewModel)e.Row.DataItem;
                var availableActions = examinationListItem.AvailableActions;
                var actionMenu = e.Row.FindControl("paActionMenu") as PopupActions;
                BuildActionMenu(actionMenu, availableActions, examinationListItem);
            }
        }

        /// <summary>
        /// Bind the list action control.
        /// </summary>
        /// <param name="actionMenu">action Menu</param>
        /// <param name="actionViewModelArray">action View Model Array</param>
        /// <param name="examinationListItem">The examination list item.</param>
        private void BuildActionMenu(PopupActions actionMenu, ExaminationActionViewModel[] actionViewModelArray, ExaminationListItemViewModel examinationListItem)
        {
            if (actionViewModelArray != null)
            {
                var actionViewModelList = new List<ActionViewModel>();
                string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();

                foreach (ExaminationActionViewModel actionModel in actionViewModelArray)
                {
                    var actinoViewModel = new ActionViewModel();
                    
                    string lnkActionId = actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase) 
                                         ? actionMenu.ClientID + CommonUtil.GetRandomUniqueID()
                                         : actionMenu.ClientID + "_lnkActions";

                    actinoViewModel.ActionLabel = actionModel.ActionLabel;
                    actinoViewModel.ActionId = lnkActionId;

                    if (actionModel.Action == ExaminationAction.Schedule)
                    {
                        string url = string.Format(
                                                    "{0}?{1}", 
                                                    actionModel.TargetURL.Split('?')[0], 
                                                    GetUrlQueryString(examinationListItem.ExaminationViewModel, false));

                        actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_schedule.png");

                        actinoViewModel.ClientEvent = string.Format(
                                                                    "ShowExaminationPopupDialog('{0}','{1}','{2}','{3}','{4}','{5}')", 
                                                                    url, 
                                                                    lnkActionId, 
                                                                    examinationListItem.ExaminationViewModel.examinationPKModel.examNbr,
                                                                    ModuleName, 
                                                                    examinationListItem.ExaminationViewModel.examStatus, 
                                                                    actionModel.Action);
                        actionViewModelList.Add(actinoViewModel);
                    }
                    else if (actionModel.Action == ExaminationAction.Cancel || actionModel.Action == ExaminationAction.Reschedule)
                    {
                        string urlQueryString = GetUrlQueryString(
                                                                  examinationListItem.ExaminationViewModel,
                                                                  actionModel.Action == ExaminationAction.Reschedule);

                        string url = string.Format("{0}?{1}", actionModel.TargetURL.Split('?')[0], urlQueryString);

                        actinoViewModel.IcoUrl = actionModel.Action == ExaminationAction.Reschedule ? ImageUtil.GetImageURL("popaction_reschedule.png") : ImageUtil.GetImageURL("popaction_cancel.png");

                        string showExamPopUpDialogString = string.Format(
                                                                          "ShowExaminationPopupDialog('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                                          url,
                                                                          lnkActionId,
                                                                          examinationListItem.ExaminationViewModel.examinationPKModel.examNbr,
                                                                          ModuleName,
                                                                          examinationListItem.ExaminationViewModel.examStatus,
                                                                          actionModel.Action);

                        string showExamWithNoCheckWorkFlowString = string.Format(
                                                                                 "ShowExamPopupDialogNoCheckWorkFlow('{0}','{1}')",
                                                                                 url,
                                                                                 lnkActionId);

                        actinoViewModel.ClientEvent = actionModel.Action == ExaminationAction.Reschedule ? showExamPopUpDialogString : showExamWithNoCheckWorkFlowString;

                        actionViewModelList.Add(actinoViewModel);
                    }
                    else if (actionModel.Action == ExaminationAction.Delete)
                    {
                        actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                        actinoViewModel.ClientEvent = string.Format(
                                                                    "DeleteExamination('{0}','{1}','{2}','{3}','{4}')", 
                                                                    examinationListItem.ExaminationViewModel.examinationPKModel.serviceProviderCode,
                                                                    examinationListItem.ExaminationViewModel.b1PerId1,
                                                                    examinationListItem.ExaminationViewModel.b1PerId2,
                                                                    examinationListItem.ExaminationViewModel.b1PerId3,
                                                                    examinationListItem.ExaminationViewModel.examinationPKModel.examNbr);
                                                                    actionViewModelList.Add(actinoViewModel);
                    }
                    else if (actionModel.Action == ExaminationAction.Edit)
                    {
                        actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");
                        actinoViewModel.ClientEvent = string.Format(
                                                                    "ShowExaminationPopupDialog('{0}','{1}','{2}','{3}','{4}','{5}')", 
                                                                    actionModel.TargetURL, 
                                                                    lnkActionId, 
                                                                    examinationListItem.ExaminationViewModel.examinationPKModel.examNbr,
                                                                    ModuleName, 
                                                                    examinationListItem.ExaminationViewModel.examStatus, 
                                                                    actionModel.Action);
                        actionViewModelList.Add(actinoViewModel);
                    }
                    else if (actionModel.Action == ExaminationAction.TakeExamination)
                    {
                        actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_take_examination.png");
                        actinoViewModel.ClientEvent = actionModel.TargetURL;
                        actinoViewModel.IsHyperLink = true;
                        actionViewModelList.Add(actinoViewModel);
                    }
                    else if (actionModel.Action == ExaminationAction.RetrySchedule)
                    {
                        string url = string.Format(
                                                   "{0}?{1}", 
                                                   actionModel.TargetURL.Split('?')[0], 
                                                   GetUrlReadyToScheduleQueryString(examinationListItem.ExaminationViewModel));

                        actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_reschedule.png");
                        actinoViewModel.ClientEvent = string.Format(
                                                                    "ShowExaminationPopupDialog('{0}','{1}','{2}','{3}','{4}','{5}')", 
                                                                    url, 
                                                                    lnkActionId, 
                                                                    examinationListItem.ExaminationViewModel.examinationPKModel.examNbr,
                                                                    ModuleName, 
                                                                    examinationListItem.ExaminationViewModel.examStatus, 
                                                                    actionModel.Action);
                        actionViewModelList.Add(actinoViewModel);
                    }
                    else if (actionModel.Action == ExaminationAction.Reset)
                    {
                        if (!ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID.Equals(examinationListItem.ExaminationViewModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                             && !ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID.Equals(examinationListItem.ExaminationViewModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
                        {
                            actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_reset.png");
                            actinoViewModel.ClientEvent = "resetExamination('" + examinationListItem.ExaminationViewModel.examinationPKModel.examNbr + "', '" + this.ModuleName + "');return false;";
                            actionViewModelList.Add(actinoViewModel);
                        }
                    }
                    else
                    {
                        actinoViewModel.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
                        actinoViewModel.ClientEvent = ACAConstant.EXAMINATION_STATUS_SCHEDULE.Equals(examinationListItem.ExaminationViewModel.examStatus, StringComparison.InvariantCultureIgnoreCase) 
                            ? string.Format("ShowExamPopupDialogNoCheckWorkFlow('{0}','{1}')", actionModel.TargetURL, lnkActionId)
                            : string.Format(
                                            "ShowExaminationPopupDialog('{0}','{1}','{2}','{3}','{4}','{5}')",
                                            actionModel.TargetURL, 
                                            lnkActionId, 
                                            examinationListItem.ExaminationViewModel.examinationPKModel.examNbr, 
                                            ModuleName, 
                                            examinationListItem.ExaminationViewModel.examStatus, 
                                            actionModel.Action);

                        actionViewModelList.Add(actinoViewModel);
                    }
                }

                actionMenu.Visible = true;
                actionMenu.ActionLableKey = string.Empty;
                actionMenu.AvailableActions = actionViewModelList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The exception object.</param>
        private void HandleExecption(Exception ex)
        {
            MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            string errorMessage4JavaScript = MessageUtil.FilterQuotation(ex.Message, true);
            string scriptToHideLoading = string.Format("HandleExeception('{0}');", errorMessage4JavaScript);
            string keyofScriptToHideLoading = this.ID + "keyofScriptToHideLoading";
            ScriptManager.RegisterStartupScript(this, this.GetType(), keyofScriptToHideLoading, scriptToHideLoading, true);
        }

        /// <summary>
        /// Bind pending list control.
        /// </summary>
        private void BindPendingList()
        {
            List<ExaminationListItemViewModel> pendingGridViewDataSource = null;

            if (!AppSession.IsAdmin)
            {
                pendingGridViewDataSource = ExaminationUtil.GetExaminationsByStatus(GridViewDataSource, ACAConstant.EXAMINATION_STATUS_PENDING);

                //// set the title
                string lblPending = GetTextByKey("aca_examination_pending_label");

                if (pendingGridViewDataSource != null && pendingGridViewDataSource.Count > 0)
                {
                    lblExaminationPending.Text = string.Format("{0} ({1})", lblPending, pendingGridViewDataSource.Count);
                }
                else
                {
                    lblExaminationPending.Text = lblPending;
                }
            }

            //// bind data source
            gvListPending.DataSource = pendingGridViewDataSource;
            gvListPending.DataBind();
        } 

        /// <summary>
        /// Bind completed list control.
        /// </summary>
        private void BindCompletedList()
        {
            List<ExaminationListItemViewModel> completedGridViewDataSource = null;

            if (!AppSession.IsAdmin)
            {
                completedGridViewDataSource = ExaminationUtil.GetExaminationsByStatus(
                                                                                      GridViewDataSource, 
                                                                                      ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING,
                                                                                      ACAConstant.EXAMINATION_STATUS_COMPLETED_SCHEDULE,
                                                                                      ACAConstant.EXAMINATION_STATUS_COMPLETED);

                string labelCompleted = GetTextByKey("aca_examination_completed_label");

                if (completedGridViewDataSource != null && completedGridViewDataSource.Count > 0)
                {
                    lblExaminationCompleted.Text = string.Format("{0} ({1})", labelCompleted, completedGridViewDataSource.Count);
                }
                else
                {
                    lblExaminationCompleted.Text = labelCompleted;
                }
            }

            //// bind data source
            gvListCompleted.DataSource = completedGridViewDataSource;
            gvListCompleted.DataBind();
        }

        /// <summary>
        /// Bind scheduled list control.
        /// </summary>
        private void BindScheduledList()
        {
            List<ExaminationListItemViewModel> scheduledGridViewDataSource = null;

            if (!AppSession.IsAdmin)
            {
                scheduledGridViewDataSource = ExaminationUtil.GetExaminationsByStatus(GridViewDataSource, ACAConstant.EXAMINATION_STATUS_SCHEDULE);

                // set the title
                string labelScheduled = GetTextByKey("aca_examination_scheduled_label");

                if (scheduledGridViewDataSource != null && scheduledGridViewDataSource.Count > 0)
                {
                    lblExaminationScheduled.Text = string.Format("{0} ({1})", labelScheduled, scheduledGridViewDataSource.Count);
                }
                else
                {
                    lblExaminationScheduled.Text = labelScheduled;
                }
            }

            // bind data source
            gvListScheduled.DataSource = scheduledGridViewDataSource;
            gvListScheduled.DataBind();
        }

        /// <summary>
        /// Binds the ready to schedule list.
        /// </summary>
        private void BindReadyToScheduleList()
        {
            List<ExaminationListItemViewModel> readyGridViewDataSource = null;

            if (!AppSession.IsAdmin)
            {
                readyGridViewDataSource = ExaminationUtil.GetExaminationsByStatus(GridViewDataSource, ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_UNPAID, ACAConstant.EXAMINATION_STATUS_READY_TO_SCHEDULE_PAID);
                
                // set the title
                string labelScheduled = GetTextByKey("aca_examlist_label_readytoschedule_title");

                if (readyGridViewDataSource != null && readyGridViewDataSource.Count > 0)
                {
                    lblReadyToSchedule.Text = string.Format("{0} ({1})", labelScheduled, readyGridViewDataSource.Count);
                }
                else
                {
                    lblReadyToSchedule.Text = labelScheduled;
                }
            }

            // bind data source
            gvReadyToSchedule.DataSource = readyGridViewDataSource;
            gvReadyToSchedule.DataBind();
        }

        /// <summary>
        /// Gets the examination list item view models. 
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <returns>Examination ListItem View Model</returns>
        private List<ExaminationListItemViewModel> GetListItemViewModels(CapModel4WS recordModel)
        {
            List<ExaminationListItemViewModel> result = new List<ExaminationListItemViewModel>();

            if (recordModel != null && !AppSession.IsAdmin)
            {
                CapIDModel recordIDModel = recordModel == null ? null : TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);

                if (recordModel != null && recordModel.examinationList != null)
                {
                    bool isCurrentUser = false;

                    if (HasPrimaryContact && AppSession.User != null && !string.IsNullOrEmpty(AppSession.User.Email)
                        && !string.IsNullOrEmpty(RecordPrimaryContact.email))
                    {
                        isCurrentUser = AppSession.User.Email.Equals(RecordPrimaryContact.email, StringComparison.OrdinalIgnoreCase);
                    }

                    result = ExaminationUtil.BuildExaminationView(recordModel.examinationList.ToList<ExaminationModel>(), ModuleName, isCurrentUser, !IsEditable);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the cap examination in session.
        /// </summary>
        private void UpdateCapExaminationInSession()
        {
            IExaminationBll examinationBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
            CapModel4WS recordModel = AppSession.GetCapModelFromSession(ModuleName);         

            if (recordModel != null && recordModel.capID != null)
            {
                CapIDModel capIdModel = new CapIDModel()
                                            {
                                                customID = recordModel.capID.customID,
                                                ID1 = recordModel.capID.id1,
                                                ID2 = recordModel.capID.id2,
                                                ID3 = recordModel.capID.id3,
                                                serviceProviderCode = recordModel.capID.serviceProviderCode,
                                                trackingID = recordModel.capID.trackingID
                                            };
                recordModel.examinationList = examinationBll.GetExamListByCapID(capIdModel);
            }
        }

        /// <summary>
        /// Gets the URL query string.
        /// </summary>
        /// <param name="examinationViewModel">The examination view model.</param>
        /// <param name="isReschedule">if set to <c>true</c> [is reschedule].</param>
        /// <returns>string of url address.</returns>
        private string GetUrlQueryString(ExaminationModel examinationViewModel, bool isReschedule)
        {
            var recordModel = AppSession.GetCapModelFromSession(ModuleName);         
            ExaminationParameter parameter = new ExaminationParameter();
            parameter.ModuleName = ModuleName;
            parameter.RecordAgencyCode = recordModel.capID.serviceProviderCode;
            parameter.RecordID1 = recordModel.capID.id1;
            parameter.RecordID2 = recordModel.capID.id2;
            parameter.RecordID3 = recordModel.capID.id3;
            parameter.HasPrimaryContact = HasPrimaryContact.ToString();
            parameter.IsAllowedBeyondDate = "False";
            parameter.IsReschedule = isReschedule.ToString();

            if (HasPrimaryContact)
            {
                parameter.RecordContactEMail = RecordPrimaryContact.email;
                parameter.RecordContactFirstName = RecordPrimaryContact.firstName;
                parameter.RecordContactMiddleName = RecordPrimaryContact.middleName;
                parameter.RecordContactFullName = RecordPrimaryContact.fullName;
                parameter.RecordContactLastName = RecordPrimaryContact.lastName;
            }

            if (examinationViewModel != null)
            {
                parameter.ExaminationName = examinationViewModel.examName;
                parameter.ExaminationNbr = examinationViewModel.refExamSeq == null ? string.Empty : examinationViewModel.refExamSeq.ToString();
                parameter.DailyExaminationNbr = examinationViewModel.examinationPKModel.examNbr.ToString();
                parameter.ExamReScheduleProviderNo = examinationViewModel.providerNo;
                parameter.ExamReScheduleProviderName = examinationViewModel.providerName;
            }

            return ExaminationParameterUtil.BuildQueryString(parameter);
        }

        /// <summary>
        /// Gets the URL ready to schedule query string.
        /// </summary>
        /// <param name="examinationViewModel">The examination view model.</param>
        /// <returns>Parameter Collection</returns>
        private string GetUrlReadyToScheduleQueryString(ExaminationModel examinationViewModel)
        {
            var recordModel = AppSession.GetCapModelFromSession(ModuleName);
            ExaminationParameter parameter = new ExaminationParameter();
            parameter.ModuleName = ModuleName;
            parameter.RecordAgencyCode = recordModel.capID.serviceProviderCode;
            parameter.RecordID1 = recordModel.capID.id1;
            parameter.RecordID2 = recordModel.capID.id2;
            parameter.RecordID3 = recordModel.capID.id3;
            parameter.HasPrimaryContact = HasPrimaryContact.ToString();

            if (HasPrimaryContact)
            {
                parameter.RecordContactEMail = RecordPrimaryContact.email;
                parameter.RecordContactFirstName = RecordPrimaryContact.firstName;
                parameter.RecordContactMiddleName = RecordPrimaryContact.middleName;
                parameter.RecordContactFullName = RecordPrimaryContact.fullName;
                parameter.RecordContactLastName = RecordPrimaryContact.lastName;
            }

            if (examinationViewModel != null)
            {
                IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                IRefExaminationBll refExaminationBll = ObjectFactory.GetObject<IRefExaminationBll>();
                ProviderModel4WS[] providerList =
                    refExaminationBll.GetRefProviderList(
                        examinationViewModel.examName, examinationViewModel.providerName, recordModel.capID.serviceProviderCode);

                ProviderModel4WS provider = providerList[0];
                bool isExternal = !string.IsNullOrEmpty(provider.externalExamURL);
                parameter.IsExternal = isExternal.ToString();
                parameter.IsOnline = "true";
                parameter.ExaminationName = examinationViewModel.examName;
                parameter.ExaminationNbr = examinationViewModel.refExamSeq == null ? string.Empty : examinationViewModel.refExamSeq.ToString();
                parameter.DailyExaminationNbr = examinationViewModel.examinationPKModel.examNbr.ToString();
                parameter.ExamReScheduleProviderNo = examinationViewModel.providerNo;
                parameter.ExamReScheduleProviderName = examinationViewModel.providerName;
                parameter.ExamScheduleLocationId = examinationViewModel.locationID == null ? string.Empty : examinationViewModel.locationID.Value.ToString();
                parameter.ExamScheduleProviderNbr = provider.providerNbr.ToString();

                parameter.ExamScheduleProviderName = examinationViewModel.providerName;
                parameter.ExamScheduleLang = examinationViewModel.examProviderDetailModel.supportedLanguages;
                
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                parameter.ExamScheduleLocation = addressBuilderBll.BuildAddress4Provider(examinationViewModel.examProviderDetailModel);
                parameter.ExamScheduleCalendarId = examinationViewModel.calendarID == null ? string.Empty : examinationViewModel.calendarID.Value.ToString();
                parameter.ExamScheduleScheduleId = examinationViewModel.scheduleID == null ? string.Empty : examinationViewModel.scheduleID.Value.ToString();

                parameter.ExamScheduleSeats = examinationBll.GetExamScheduleAvailableSeats(examinationViewModel.examinationPKModel).ToString();

                parameter.ExamScheduleHandlecap = examinationViewModel.examProviderDetailModel.isHandicapAccessible;
                parameter.ExamScheduleDate = I18nDateTimeUtil.FormatToDateStringForUI(examinationViewModel.examDate);
                parameter.ExamScheduleWeekDay = examinationViewModel.examDate == null
                                                    ? string.Empty
                                                    : I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.DayNames[(int)examinationViewModel.examDate.Value.DayOfWeek];

                if (examinationViewModel.startTime != null)
                {
                    parameter.ExamScheduleStartTime = I18nDateTimeUtil.FormatToTimeStringForUI(examinationViewModel.startTime.Value, false);   
                }

                if (examinationViewModel.endTime != null)
                {
                    parameter.ExamScheduleEndTime = I18nDateTimeUtil.FormatToTimeStringForUI(examinationViewModel.endTime.Value, false);
                }

                parameter.IsReschedule = "false";
                parameter.AccessiblityDesc = examinationViewModel.examProviderDetailModel.handicapAccessibleDes;
                parameter.ExamInstructions = examinationViewModel.comments;
                parameter.DrivingDesc = examinationViewModel.examProviderDetailModel.drivingDirections;
                parameter.ReadyToScheduleStatus = examinationViewModel.examStatus;

                if (!isExternal)
                {
                    bool isBeyondAllowanceDate = refExaminationBll.IsBeyondAllowanceDate(recordModel.capID.serviceProviderCode, examinationViewModel.providerNo, examinationViewModel.startTime, examinationViewModel.refExamSeq);
                    parameter.IsAllowedBeyondDate = isBeyondAllowanceDate.ToString();
                }
            }

            return ExaminationParameterUtil.BuildQueryString(parameter);
        }

        #endregion
    }
}