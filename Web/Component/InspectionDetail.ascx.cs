#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionDetail.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// inspection detail user control.
    /// </summary>
    public partial class InspectionDetail : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The parameter for inspection type.
        /// </summary>
        protected const string PARAMETER_INSPECTION_TYPE = "$$InspectionType$$";

        /// <summary>
        /// The parameter for inspection id.
        /// </summary>
        protected const string PARAMETER_INSPECTION_ID = "$$InspectionID$$";

        /// <summary>
        /// The parameter for inspection required status.
        /// </summary>
        protected const string PARAMETER_INSPECTION_REQUIRED_STATUS = "$$InspectionRequiredStatus$$";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the service provider code
        /// </summary>
        public string ServiceProviderCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets cap model.
        /// </summary>
        /// <value>The cap model.</value>
        public CapModel4WS Cap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user model.
        /// </summary>
        /// <value>The current user.</value>
        public User CurrentUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection sequence number.
        /// </summary>
        /// <value>The inspection ID.</value>
        public string InspectionID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is admin.
        /// </summary>
        /// <value><c>true</c> if this instance is admin; otherwise, <c>false</c>.</value>
        public bool IsAdmin
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether hide the action button or not.
        /// </summary>
        private bool IsHideActionButton
        {
            get
            {
                if (!string.IsNullOrEmpty(Request[UrlConstant.HIDE_ACTION_BUTTON]))
                {
                    return ValidationUtil.IsYes(Request[UrlConstant.HIDE_ACTION_BUTTON]);
                }

                return false;
            }
        }

        #endregion Properties

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InspectionViewModel currentInspectionViewModel = null;

                if (IsAdmin)
                {
                    InitAdminSideUI();

                    divInspectionField.Visible = true;
                    lblInspectionName.LabelType = LabelType.BodyText;
                }
                else
                {
                    currentInspectionViewModel = GetInspectionViewModel();
                    InitDailySideUI(currentInspectionViewModel);

                    lblInspectionName.IsNeedEncode = false;
                    lblInspectionName.LabelType = LabelType.LabelText;

                    if (currentInspectionViewModel != null)
                    {
                        string inspectionNamePattern = LabelUtil.GetTextByKey("aca_inspectiondetail_label_inspectiontype_pattern", ModuleName);

                        lblInspectionName.Text = inspectionNamePattern.Replace(PARAMETER_INSPECTION_TYPE, currentInspectionViewModel.TypeText)
                                                                      .Replace(PARAMETER_INSPECTION_ID, currentInspectionViewModel.ID.ToString())
                                                                      .Replace(PARAMETER_INSPECTION_REQUIRED_STATUS, currentInspectionViewModel.RequiredOrOptional);
                    }
                }

                BindStatusHistoryAndCommentList(currentInspectionViewModel);
                BindRelatedInspectionList();
                SetContactByPermission();
            }
        }

        /// <summary>
        /// Initializes the admin side UI.
        /// </summary>
        private void InitAdminSideUI()
        {
        }

        /// <summary>
        /// Gets the inspection view model.
        /// </summary>
        /// <returns>the inspection view model.</returns>
        private InspectionViewModel GetInspectionViewModel()
        {
            InspectionViewModel result = null;
            var inspectionID = Convert.ToInt32(InspectionID);
            result = InspectionViewUtil.GetViewModelByIDFromCache(inspectionID, ModuleName);

            if (result == null)
            {
                var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
                bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(Cap.capID, CurrentUser.UserSeqNum);
                var allInspectionDataModels = inspectionBll.GetDataModels(ModuleName, Cap, isCapLockedOrHold, CurrentUser.UserModel4WS);
                var inspectionDataModel =
                    allInspectionDataModels == null ? null :
                    (from i in allInspectionDataModels
                     where i.ID == Convert.ToInt64(InspectionID)
                     select i).FirstOrDefault();
                result = InspectionViewUtil.BuildViewModel(ModuleName, inspectionDataModel);
            }

            return result;
        }

        /// <summary>
        /// Initializes the daily side UI.
        /// </summary>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        private void InitDailySideUI(InspectionViewModel inspectionViewModel)
        {
            var inspectionDataModel = inspectionViewModel == null ? null : inspectionViewModel.InspectionDataModel;

            if (IsHideActionButton)
            {
                divInspectionButton.Visible = false;
            }
            else
            {
                bool readOnly = Cap.IsForRenew;
                CapIDModel recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(Cap.capID);
                InspectionAction mainAction = inspectionDataModel == null ? InspectionAction.None : inspectionDataModel.MainAction;
                InspectionAction cancelAction = inspectionDataModel == null ? InspectionAction.None : inspectionDataModel.CancelAction;
                InspectionActionViewModel mainActionViewModel = InspectionActionViewUtil.BuildViewModel(ServiceProviderCode, recordIDModel, ModuleName, CurrentUser.PublicUserId, inspectionViewModel, mainAction);
                InspectionActionViewModel cancellActionViewModel = InspectionActionViewUtil.BuildViewModel(ServiceProviderCode, recordIDModel, ModuleName, CurrentUser.PublicUserId, inspectionViewModel, cancelAction);
                InitActions(mainActionViewModel, cancellActionViewModel, readOnly);
            }

            AddressText.Display(Cap.addressModel, false);
            lblStatus.Text = inspectionViewModel.StatusText;
            lblStatusDate.Text = GetStatusDate(inspectionViewModel);

            lblLastUpdatedBy.Text = string.IsNullOrEmpty(inspectionViewModel.InspectionDataModel.LastUpdatedBy)
                                        ? LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName)
                                        : inspectionViewModel.InspectionDataModel.LastUpdatedBy;
            lblLastUpdated.Text = inspectionViewModel.LastUpdatedDateTimeText;
            lblRecordAltID.Text = Cap.altID;
            lblRecordType.Text = CAPHelper.GetAliasOrCapTypeLabel(Cap);

            // set the contact name and phone
            string firstName = inspectionViewModel.ContactFirstName;
            string middleName = inspectionViewModel.ContactMiddleName;
            string lastName = inspectionViewModel.ContactLastName;
            string phoneIDD = inspectionViewModel.ContactPhoneIDD;
            string phoneNumber = inspectionViewModel.ContactPhoneNumber;

            if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(middleName) || !string.IsNullOrEmpty(lastName))
            {
                lblContactName.Text = UserUtil.FormatToFullName(firstName, middleName, lastName);
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = ModelUIFormat.FormatPhoneShow(phoneIDD, phoneNumber, string.Empty);
                lblContactPhoneNumber.Text = LabelUtil.RemoveHtmlFormat(phoneNumber);
            }

            // set the requestor name and phone
            string reqFirstName = inspectionViewModel.RequestorFirstName;
            string reqMiddleName = inspectionViewModel.RequestorMiddleName;
            string reqLastName = inspectionViewModel.RequestorLastName;
            string reqPhoneNumber = inspectionViewModel.RequestorPhoneNumber;
            string reqPhoneIDD = inspectionViewModel.RequestorPhoneIDD;

            if (!string.IsNullOrEmpty(reqFirstName) || !string.IsNullOrEmpty(reqMiddleName) || !string.IsNullOrEmpty(reqLastName))
            {
                lblRequestorName.Text = UserUtil.FormatToFullName(reqFirstName, reqMiddleName, reqLastName);
            }

            if (!string.IsNullOrEmpty(reqPhoneNumber))
            {
                reqPhoneNumber = ModelUIFormat.FormatPhoneShow(reqPhoneIDD, reqPhoneNumber, string.Empty);
                lblRequestorPhoneNumber.Text = LabelUtil.RemoveHtmlFormat(reqPhoneNumber);
            }

            if (!AppSession.IsAdmin)
            {
                ShowEstimatedArrivalTime(inspectionDataModel);
                ShowDesiredDate(inspectionDataModel);
            }

            divStatusHistory.Attributes.Add("class", "ACA_Hide");
            divResultComments.Attributes.Add("class", "ACA_Hide");
        }

        /// <summary>
        /// Shows the estimated arrival time.
        /// </summary>
        /// <param name="inspectionDataModel">The inspection data model.</param>
        private void ShowEstimatedArrivalTime(InspectionDataModel inspectionDataModel)
        {
            if (inspectionDataModel != null && !inspectionDataModel.IsUpcomingInspection)
            {
                divEstimatedArrivalTime.Visible = false;
            }
            else
            {
                string estStartTime = string.Empty;
                string estEndTime = string.Empty;

                if (inspectionDataModel != null && inspectionDataModel.InspectionModel != null &&
                    inspectionDataModel.InspectionModel.activity != null)
                {
                    estStartTime = inspectionDataModel.InspectionModel.activity.estimatedStartTime;
                    estEndTime = inspectionDataModel.InspectionModel.activity.estimatedEndTime;
                }

                lblEstimatedArrivalTimeValue.Text = InspectionViewUtil.GetEstimatedArrivateTime(estStartTime, estEndTime, ModuleName);
            }
        }

        /// <summary>
        /// Shows the desired date.
        /// </summary>
        /// <param name="inspectionDataModel">The inspection data model.</param>
        private void ShowDesiredDate(InspectionDataModel inspectionDataModel)
        {
            InspectionTimeOption timeOption = InspectionTimeOption.Unknow;
            DateTime? desiredDate = InspectionViewUtil.GetDesiredDate(inspectionDataModel, out timeOption);

            if (desiredDate.HasValue)
            {
               lblDesiredDateValue.Text = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(desiredDate.Value);
            }
            else
            {
                lblDesiredDateValue.Text = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName);
            }
        }

        /// <summary>
        /// Initializes the actions.
        /// </summary>
        /// <param name="mainActionViewModel">The main action view model.</param>
        /// <param name="cancellActionViewModel">The cancel action view model.</param>
        /// <param name="readOnly">is readonly.</param>
        private void InitActions(InspectionActionViewModel mainActionViewModel, InspectionActionViewModel cancellActionViewModel, bool readOnly)
        {
            AccelaButton btnMainAction = null;

            switch (mainActionViewModel.Action)
            {
                case InspectionAction.Request:
                    btnMainAction = btnRequest;
                    break;
                case InspectionAction.Schedule:
                    btnMainAction = btnSchedule;
                    break;
                case InspectionAction.Reschedule:
                    btnMainAction = btnReschedule;
                    break;
                default:
                    btnMainAction = null;
                    break;
            }

            btnRequest.Visible = false;
            btnSchedule.Visible = false;
            btnReschedule.Visible = false;

            if (btnMainAction != null && !readOnly)
            {
                btnMainAction.Visible = mainActionViewModel.Visible && FunctionTable.IsEnableScheduleInspection();
                btnMainAction.Enabled = mainActionViewModel.Enabled;
                btnMainAction.PostBackUrl = mainActionViewModel.TargetURL;
            }

            divMainActionSpacer.Visible = btnMainAction != null && btnMainAction.Visible;
            btnCancel.Visible = !readOnly && cancellActionViewModel.Visible && FunctionTable.IsEnableScheduleInspection();
            btnCancel.Enabled = cancellActionViewModel.Enabled;
            btnCancel.PostBackUrl = cancellActionViewModel.TargetURL;
        }

        /// <summary>
        /// bind status history and result comment list.
        /// </summary>
        /// <param name="currentInspectionViewModel">The current inspection view model.</param>
        private void BindStatusHistoryAndCommentList(InspectionViewModel currentInspectionViewModel)
        {
            IList<InspectionViewModel> inspectionHistoryViewModels = null;
            bool allowViewResultComments = true;

            if (!IsAdmin)
            {
                var currentScheduleType = currentInspectionViewModel.InspectionDataModel.ScheduleType;
                allowViewResultComments = currentInspectionViewModel.InspectionDataModel.AllowViewResultComments;
                var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
                long inspectionID = long.Parse(InspectionID);
                var inspectionHistoryDataModels = inspectionBll.GetInspectionHistoryList(inspectionID, ModuleName, Cap, CurrentUser.UserModel4WS);

                if (inspectionHistoryDataModels != null)
                {
                    inspectionHistoryViewModels = new List<InspectionViewModel>();
                    foreach (var dataModel in inspectionHistoryDataModels)
                    {
                        var tempViewModel = InspectionViewUtil.BuildViewModel(ModuleName, dataModel);
                        tempViewModel.ResultComments = allowViewResultComments ? tempViewModel.ResultComments : string.Empty;
                        UpdateStatusDateTime4History(tempViewModel, currentScheduleType);
                        UpdateStatusText4History(tempViewModel);
                        
                        inspectionHistoryViewModels.Add(tempViewModel);
                    }
                }
            }

            //because status history and result comment has the same data source ,so only request web service one time. 
            StatusHistoryList.DataSource = inspectionHistoryViewModels;
            StatusHistoryList.Bind();

            List<InspectionViewModel> resultCommentViewModels = null;

            if (!IsAdmin && allowViewResultComments)
            {
                resultCommentViewModels = inspectionHistoryViewModels == null ? null : inspectionHistoryViewModels.Where(o => !string.IsNullOrEmpty(o.ResultComments)).ToList();

                //always show the latest result comments.
                if (!IsAdmin && (resultCommentViewModels == null || resultCommentViewModels.Count == 0) && currentInspectionViewModel != null && !string.IsNullOrEmpty(currentInspectionViewModel.ResultComments))
                {
                    resultCommentViewModels = new List<InspectionViewModel>() { currentInspectionViewModel };
                }
            }

            ResultCommentList.DataSource = resultCommentViewModels;
            ResultCommentList.Bind();
        }

        /// <summary>
        /// update status date time again as inspection history (audit log) can't provide enough info to present the real inspection status.
        /// </summary>
        /// <param name="inspectionViewModel">inspectionViewModel object</param>
        /// <param name="currentScheduleType">Type of the current schedule.</param>
        private void UpdateStatusDateTime4History(InspectionViewModel inspectionViewModel, InspectionScheduleType currentScheduleType)
        {
            DateTime? tempStatusDateTime = null;
            var inspectionModel = inspectionViewModel.InspectionDataModel.InspectionModel;
            var isRescheduled = ACAConstant.INSPECTION_STATUS_KEY_RESCHEDULED.Equals(inspectionModel.activity.status, StringComparison.OrdinalIgnoreCase);
            var isCancelled = ACAConstant.INSPECTION_STATUS_KEY_CANCELED.Equals(inspectionModel.activity.status, StringComparison.OrdinalIgnoreCase);
            var isRequestPendingWithDateTime = false;
            var needTimeOption = false;
            string textTBD = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", ModuleName);
            var theDateTimeString = textTBD;
            var theDateString = textTBD;
            var theTimeString = textTBD;

            if (isRescheduled || isCancelled)
            {
                tempStatusDateTime = inspectionViewModel.LastUpdatedDateTime;
            }
            else if (ACAConstant.INSPECTION_STATUS_KEY_PENDING.Equals(inspectionModel.activity.status, StringComparison.OrdinalIgnoreCase)
                && inspectionViewModel.ResultedDateTime == null 
                && inspectionViewModel.ScheduledDateTime != null)
            {
                isRequestPendingWithDateTime = true;
                needTimeOption = true;
                tempStatusDateTime = inspectionModel.activity.activityDate;
            }
            else if (inspectionViewModel.ResultedDateTime != null)
            {
                tempStatusDateTime = inspectionViewModel.ResultedDateTime.Value;
            }
            else if (inspectionViewModel.ScheduledDateTime != null || inspectionViewModel.RequestedDateTime != null)
            {
                needTimeOption = true;
                tempStatusDateTime = inspectionModel.activity.activityDate;
            }

            inspectionViewModel.StatusDateTime = tempStatusDateTime;

            if (tempStatusDateTime != null)
            {
                theDateTimeString = needTimeOption ? InspectionViewUtil.BuildDateTimeText(tempStatusDateTime.Value, InspectionTimeOption.SpecificTime, ModuleName) : I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(tempStatusDateTime.Value);
                theDateString = I18nDateTimeUtil.FormatToDateStringForUI(tempStatusDateTime.Value);
                theTimeString = needTimeOption ? InspectionViewUtil.BuildTimeText(tempStatusDateTime.Value, InspectionTimeOption.SpecificTime, ModuleName) : I18nDateTimeUtil.FormatToTimeStringForUI(tempStatusDateTime.Value, true);
            }

            if (isCancelled || (isRequestPendingWithDateTime && currentScheduleType == InspectionScheduleType.RequestSameDayNextDay))
            {
                inspectionViewModel.StatusDateTimeText = theDateString;
            }
            else
            {
                inspectionViewModel.StatusDateTimeText = theDateTimeString;
            }

            inspectionViewModel.StatusDateText = theDateTimeString;
            inspectionViewModel.StatusTimeText = theTimeString;
        }

        /// <summary>
        /// Updates the status text4 history.
        /// </summary>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        private void UpdateStatusText4History(InspectionViewModel inspectionViewModel)
        {
            const string SCHEDULED = "Scheduled";
            const string CANCELLED = "Cancelled";
            const string PENDING = "Pending";
            const string RESCHEDULED = "Rescheduled";
            string newText = string.Empty;

            if (inspectionViewModel != null && inspectionViewModel.InspectionDataModel != null && inspectionViewModel.InspectionDataModel.Status == InspectionStatus.Unknown)
            {
                var status = InspectionStatus.Unknown;

                switch (inspectionViewModel.StatusText)
                {
                    case SCHEDULED:
                        status = InspectionStatus.Scheduled;
                        break;
                    case CANCELLED:
                        status = InspectionStatus.Canceled;
                        break;
                    case PENDING:
                        status = InspectionStatus.PendingByACA;
                        break;
                    case RESCHEDULED:
                        status = InspectionStatus.Rescheduled;
                        break;
                    default:
                        status = InspectionStatus.Unknown;
                        break;
                }

                string newTextKey = InspectionViewUtil.GetLabelKey(status);
                newText = string.IsNullOrEmpty(newTextKey) ? string.Empty : LabelUtil.GetTextByKey(newTextKey, ModuleName);

                if (!string.IsNullOrEmpty(newText))
                {
                    inspectionViewModel.StatusText = newText;
                }
            }
        }

        /// <summary>
        /// bind related inspection list.
        /// </summary>
        private void BindRelatedInspectionList()
        {
            InspectionTreeNodeModel fullTree = null;

            if (!IsAdmin)
            {
                IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));
                fullTree = inspectionBll.GetRelatedInspections(TempModelConvert.Trim4WSOfCapIDModel(Cap.capID), InspectionID);
            }

            RelatedInspections.IsAdmin = IsAdmin;
            RelatedInspections.ServiceProviderCode = ServiceProviderCode;
            RelatedInspections.Cap = Cap;
            RelatedInspections.CurrentUser = CurrentUser;
            RelatedInspections.DataSource = fullTree;
            RelatedInspections.Bind();
        }

        /// <summary>
        /// Set contact by permission
        /// </summary>
        private void SetContactByPermission()
        {
            if (!IsAdmin)
            {
                InspectionParameter parameters = InspectionParameterUtil.BuildModelFromURL();

                var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();
                bool right = inspectionPermissionBll.CheckContactRight(Cap, parameters.AgencyCode, ModuleName, ACAConstant.USER_ROLE_POLICY_FOR_INSPECTION_VIEW_CONTACT);
                
                divPrimaryContact.Visible = right;               
            }
        }

        /// <summary>
        /// Get the status date.
        /// </summary>
        /// <param name="inspectionViewModel">the InspectionViewModel</param>
        /// <returns>Return the status date.</returns>
        private string GetStatusDate(InspectionViewModel inspectionViewModel)
        {
            string result = string.Empty;

            if (inspectionViewModel != null && inspectionViewModel.InspectionDataModel != null)
            {
                InspectionDataModel inspectionDataModel = inspectionViewModel.InspectionDataModel;

                // ready time is available (enabled + request pending)
                if (inspectionDataModel.ReadyTimeAvailable)
                {
                    string readytimePattern = "{0}: {1}";

                    if (I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft)
                    {
                        readytimePattern = "{1} :{0}";
                    }

                    result = string.Format(readytimePattern, LabelUtil.GetTextByKey("ins_inspectionlist_label_readytime", ModuleName), inspectionViewModel.StatusDateTimeText);
                }
                else
                {
                    result = inspectionViewModel.StatusDateTimeText;
                }
            }

            return result;
        }
    }
}