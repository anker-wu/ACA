#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionViewController.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionViewUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// Inspection View Utility
    /// </summary>
    public static class InspectionViewUtil
    {
        /// <summary>
        /// Gets the inspection view model by ID from cache.
        /// </summary>
        /// <param name="inspectionID">The inspection ID.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>the inspection view model by ID from cache.</returns>
        public static InspectionViewModel GetViewModelByIDFromCache(int inspectionID, string moduleName)
        {
            InspectionViewModel result = null;
            var inspectionListItemViewModels = AppSession.InspectionData;

            if (inspectionListItemViewModels != null && inspectionListItemViewModels.Count > 0)
            {
                var inspectionDataModel =
                       (from i in inspectionListItemViewModels
                        where i.InspectionViewModel.InspectionDataModel.ID == inspectionID
                        select i.InspectionViewModel.InspectionDataModel).FirstOrDefault();
                result = inspectionDataModel == null ? null : BuildViewModel(moduleName, inspectionDataModel);
            }

            return result;
        }

        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="dataModel">The data model.</param>
        /// <returns>the view model.</returns>
        public static InspectionViewModel BuildViewModel(string moduleName, InspectionDataModel dataModel)
        {
            var result = new InspectionViewModel();

            if (dataModel != null)
            {
                string textTBD = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", moduleName);
                string textUnassigned = LabelUtil.GetTextByKey("aca_inspection_unassigned", moduleName);
                result.ID = dataModel.ID;
                result.TypeID = dataModel.TypeID;
                result.InspectionDataModel = dataModel;
                InspectionModel inspectionModel = dataModel.InspectionModel;
                InspectionTypeModel inspectionTypeModel = dataModel.InspectionTypeModel;
                result.StatusText = GetStatusText(dataModel.StatusString, inspectionModel.activity.status, dataModel.Status, moduleName);
                result.Group = dataModel.Group;
                result.Result = GetStatusText(dataModel.StatusString, inspectionModel.activity.status, dataModel.Status, moduleName);
                result.Grade = dataModel.Grade;

                if (inspectionTypeModel != null)
                {
                    result.GroupText = I18nStringUtil.GetString(inspectionTypeModel.resGroupCodeName, inspectionTypeModel.groupCodeName, inspectionTypeModel.resGroupCode, inspectionTypeModel.groupCode);
                }
                else
                {
                    result.GroupText = inspectionModel.activity.activityGroup;
                }

                string inspector = UserUtil.GetUserName(dataModel.Inspector);
                var isInspectorUnassigned = string.IsNullOrEmpty(inspector);
                result.IsInspectorUnassigned = isInspectorUnassigned;
                result.Inspector = isInspectorUnassigned ? textUnassigned : inspector.Trim();
                result.RequiredOrOptional = dataModel.Required ? LabelUtil.GetTextByKey("ACA_Inspection_Status_Required", moduleName) : LabelUtil.GetTextByKey("ACA_Inspection_Status_Optional", moduleName);
                result.TimeOption = dataModel.TimeOption;

                DateTime? readyTimeDateTime = dataModel.ReadyTimeAvailable ? dataModel.ScheduledDateTime : (DateTime?)null;
                result.ReadyTime = readyTimeDateTime;
                result.ReadyTimeDateTimeText = readyTimeDateTime != null ? BuildDateTimeText(readyTimeDateTime, dataModel.TimeOption, moduleName) : textTBD;
                result.ReadyTimeDateText = readyTimeDateTime != null ? I18nDateTimeUtil.FormatToDateStringForUI(readyTimeDateTime) : textTBD;
                result.ReadyTimeTimeText = readyTimeDateTime != null ? BuildTimeText(readyTimeDateTime, dataModel.TimeOption, moduleName) : textTBD;

                result.RequestedDateTime = dataModel.RequestedDateTime;
                result.RequestedDateText = dataModel.RequestedDateTime != null ? I18nDateTimeUtil.FormatToDateStringForUI(dataModel.RequestedDateTime.Value) : textTBD;

                if (result.InspectionDataModel.Status == InspectionStatus.PendingByACA && result.InspectionDataModel.ScheduleType == InspectionScheduleType.RequestSameDayNextDay)
                {
                    result.RequestedDateTimeText = result.RequestedDateText;
                    result.RequestedTimeText = textTBD;
                }
                else
                {
                    result.RequestedDateTimeText = dataModel.RequestedDateTime != null ? BuildDateTimeText(dataModel.RequestedDateTime.Value, dataModel.TimeOption, moduleName) : textTBD;
                    result.RequestedTimeText = dataModel.RequestedDateTime != null ? BuildTimeText(dataModel.RequestedDateTime.Value, dataModel.TimeOption, moduleName) : textTBD;
                }
                
                result.ResultedDateTime = dataModel.ResultedDateTime;
                result.ResultedDateTimeText = dataModel.ResultedDateTime != null ? I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(dataModel.ResultedDateTime.Value) : textTBD;
                result.ResultedDateText = dataModel.ResultedDateTime != null ? I18nDateTimeUtil.FormatToDateStringForUI(dataModel.ResultedDateTime.Value) : textTBD;
                result.ResultedTimeText = dataModel.ResultedDateTime != null ? I18nDateTimeUtil.FormatToTimeStringForUI(dataModel.ResultedDateTime.Value, true) : textTBD;
                result.ScheduledDateTime = dataModel.ScheduledDateTime;
                result.ScheduledDateTimeText = dataModel.ScheduledDateTime != null ? BuildDateTimeText(dataModel.ScheduledDateTime.Value, dataModel.TimeOption, moduleName) : textTBD;
                result.ScheduledDateText = dataModel.ScheduledDateTime != null ? I18nDateTimeUtil.FormatToDateStringForUI(dataModel.ScheduledDateTime.Value) : textTBD;
                result.ScheduledTimeText = dataModel.ScheduledDateTime != null ? BuildTimeText(dataModel.ScheduledDateTime.Value, dataModel.TimeOption, moduleName) : textTBD;
                result.RequestComments = dataModel.RequestComments;
                result.ResultComments = dataModel.ResultComments;
                result.TypeText = I18nStringUtil.GetString(inspectionModel.activity.resActivityType, inspectionModel.activity.activityType);
                result.ContactFirstName = dataModel.ContactFirstName;
                result.ContactMiddleName = dataModel.ContactMiddleName;
                result.ContactLastName = dataModel.ContactLastName;
                result.ContactFullName = dataModel.ContactVisible ? UserUtil.FormatToFullName(dataModel.ContactFirstName, dataModel.ContactMiddleName, dataModel.ContactLastName) : string.Empty;
                result.ContactPhoneIDD = dataModel.ContactPhoneIDD;
                result.ContactPhoneNumber = dataModel.ContactPhoneNumber;
                result.ContactFullPhoneNumber = dataModel.ContactVisible ? ModelUIFormat.FormatPhoneShow(dataModel.ContactPhoneIDD, dataModel.ContactPhoneNumber, string.Empty) : string.Empty;
                result.RequestorFirstName = dataModel.RequestorFirstName;
                result.RequestorMiddleName = dataModel.RequestorMiddleName;
                result.RequestorLastName = dataModel.RequestorLastName;
                result.RequestorPhoneNumber = dataModel.RequestorPhoneNumber;
                result.RequestorPhoneIDD = dataModel.RequestorPhoneIDD;
                result.LastUpdatedDateTime = dataModel.LastUpdated;
                result.LastUpdatedDateTimeText = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(dataModel.LastUpdated);
                result.LastUpdatedDateText = I18nDateTimeUtil.FormatToDateStringForUI(dataModel.LastUpdated);
                result.LastUpdatedTimeText = I18nDateTimeUtil.FormatToTimeStringForUI(dataModel.LastUpdated, true);
                result.LastUpdatedBy = !string.IsNullOrEmpty(dataModel.LastUpdatedBy) ? dataModel.LastUpdatedBy : textTBD;

                //get status date according to each date time.
                DateTime? statusDateTime = null;
                string statusDateTimeText = string.Empty;
                string statusDateText = string.Empty;
                string statusTimeText = string.Empty;
                GetStatusDateTime(result, out statusDateTime, out statusDateTimeText, out statusDateText, out statusTimeText);
                result.StatusDateTime = statusDateTime;
                result.StatusDateTimeText = statusDateTimeText;
                result.StatusDateText = statusDateText;
                result.StatusTimeText = statusTimeText;

                result.Score = dataModel.Score;
                result.ScoreText = dataModel.Score == null ? string.Empty : dataModel.Score.Value.ToString();

                var majorViolations = GetMajorViolations(dataModel.GuideSheetModels);
                result.MajorViolations = majorViolations;
                result.MajorViolationsText = GetMajorViolationsText(result, string.Empty, string.Empty);
                result.GradeImageKey = dataModel.GradeImageKey;
                result.GradeImageDescription = LabelUtil.GetTextByKey("aca_foodfacilitydetail_label_gradeimagealt", moduleName);
            }

            return result;
        }

        /// <summary>
        /// Builds the view models.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="inspectionDataModels">The inspection data models.</param>
        /// <returns>the view models.</returns>
        public static List<InspectionViewModel> BuildViewModels(string moduleName, List<InspectionDataModel> inspectionDataModels)
        {
            var result = new List<InspectionViewModel>();

            if (inspectionDataModels != null)
            {
                foreach (var inspectionDataModel in inspectionDataModels)
                {
                    var viewModel = BuildViewModel(moduleName, inspectionDataModel);

                    if (viewModel != null)
                    {
                        result.Add(viewModel);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether this instance [can show schedule link] the specified agency code.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="currentUser">The current user.</param>
        /// <param name="recordModel">The record model.</param>
        /// <returns>
        /// <c>true</c> if this instance [can show schedule link] the specified agency code; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanShowScheduleLink(string agencyCode, string moduleName, User currentUser, CapModel4WS recordModel)
        {
            bool result = false;
            bool isCurrentUserAnonymous = currentUser != null ? currentUser.IsAnonymous : false;

            bool isCapLockedOrHold = ConditionsUtil.IsConditionLockedOrHold(recordModel.capID, currentUser.UserSeqNum);

            var inspectionPermissionBll = ObjectFactory.GetObject<IInspectionPermissionBll>();
            bool allowSchedule = inspectionPermissionBll.AllowSchedule(agencyCode, moduleName, isCurrentUserAnonymous, recordModel);

            result = !isCapLockedOrHold && allowSchedule;
            return result;
        }

        /// <summary>
        /// Gets the status text.
        /// </summary>
        /// <param name="resStatusString">The res status string.</param>
        /// <param name="statusString">The status string.</param>
        /// <param name="inspectionStatus">The inspection status.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>the status text.</returns>
        public static string GetStatusText(string resStatusString, string statusString, InspectionStatus inspectionStatus, string moduleName)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(resStatusString))
            {
                string statusLabelKey = GetLabelKey(inspectionStatus);
                result = string.IsNullOrEmpty(statusLabelKey) ? string.Empty : LabelUtil.GetTextByKey(statusLabelKey, moduleName);
                result = I18nStringUtil.GetString(result, statusString);
            }
            else
            {
                result = resStatusString;
            }

            return result;
        }

        /// <summary>
        /// Validate inspection date time and get the available time result.
        /// </summary>
        /// <param name="selectedDate">the select date</param>
        /// <param name="capIDModel">the CapIDModel object</param>
        /// <param name="inspectionParameter">the InspectionParameter object</param>
        /// <param name="isBlockedWhenNoInspectorFound">is blocked when no inspector found.</param>
        /// <returns>get the available result.</returns>
        public static AvailableTimeResultModel GetAvailableTimeResult(DateTime selectedDate, CapIDModel capIDModel, InspectionParameter inspectionParameter, bool isBlockedWhenNoInspectorFound = true)
        {
            var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();

            InspectionAction inspectionAction = inspectionParameter.Action;

            //prepare actMode
            string statusAfterAction = inspectionBll.GetStatusAfterAction(inspectionAction);

            string time1String = string.Empty;
            string time2String = string.Empty;

            if (selectedDate != DateTime.MinValue)
            {
                InspectionScheduleType scheduleType = inspectionParameter.ScheduleType;
                var readyTimeEnabled = inspectionParameter.ReadyTimeEnabled != null ? inspectionParameter.ReadyTimeEnabled.Value : false;

                if (scheduleType == InspectionScheduleType.ScheduleUsingCalendar
                    || scheduleType == InspectionScheduleType.RequestSameDayNextDay
                    || (scheduleType == InspectionScheduleType.RequestOnlyPending && readyTimeEnabled)
                    || scheduleType == InspectionScheduleType.Unknown)
                {
                    BuildTimeValue(selectedDate, inspectionParameter.TimeOption, out time1String, out time2String);
                }
            }

            InspectionModel inspectionModel = new InspectionModel();

            ActivityModel activityModel = new ActivityModel();
            activityModel.capID = capIDModel;
            activityModel.activityDate = selectedDate;
            activityModel.time1 = time1String;
            activityModel.time2 = time2String;
            activityModel.auditID = AppSession.User.PublicUserId;
            activityModel.auditDate = DateTime.Now;
            activityModel.auditStatus = ACAConstant.VALID_STATUS;
            activityModel.activityType = inspectionParameter.Type;
            activityModel.serviceProviderCode = inspectionParameter.AgencyCode;
            activityModel.inAdvanceFlag = inspectionParameter.InAdvance == true ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            activityModel.requestorUserID = AppSession.User.UserID;

            //prepare inspectionId
            long inspectionId = 0;
            long.TryParse(inspectionParameter.ID, out inspectionId);

            activityModel.idNumber = inspectionId;

            long sequenceNumber = 0;
            long.TryParse(inspectionParameter.TypeID, out sequenceNumber);

            activityModel.inspSequenceNumber = sequenceNumber;
            activityModel.status = statusAfterAction;
            activityModel.activityDescription = inspectionParameter.Type;
            activityModel.requiredInspection = inspectionParameter.Required == true ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            inspectionModel.activity = activityModel;
            
            AvailableTimeResultModel availableTimeResultModel = null;

            if (isBlockedWhenNoInspectorFound)
            {
                availableTimeResultModel = inspectionBll.ValidateInspectionDateTime(inspectionParameter.AgencyCode, inspectionModel);
            }
            else
            {
                availableTimeResultModel = inspectionBll.ValidateInspectionDateTimeWhileNotBlockSchedule(inspectionParameter.AgencyCode, inspectionModel);
            }

            return availableTimeResultModel;
        }

        /// <summary>
        /// Gets the encoded major violations.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="baseClientID">The base client ID.</param>
        /// <param name="majorViolationPattern">The major violation pattern.</param>
        /// <returns>the encoded major violations.</returns>
        public static string GetMajorViolationsText(InspectionViewModel viewModel, string baseClientID, string majorViolationPattern)
        {
            string result = string.Empty;

            if (viewModel.MajorViolations != null && viewModel.MajorViolations.Count > 0)
            {
                List<string> resultList = new List<string>();
                int index = 0;

                foreach (string majorViolation in viewModel.MajorViolations.Keys)
                {
                    if (!string.IsNullOrEmpty(majorViolation))
                    {
                        string encodedMajorViolation = HttpUtility.HtmlEncode(majorViolation);
                        string majorViolationDescription = viewModel.MajorViolations[majorViolation];
                        string containerID = !string.IsNullOrEmpty(baseClientID) ? baseClientID + index.ToString() : string.Empty;
                        Random rd = new Random();
                        string lnkMajorViolationID = Convert.ToString(rd.Next());
                        index++;

                        string text = !string.IsNullOrEmpty(majorViolationPattern) ? string.Format(majorViolationPattern, containerID, majorViolationDescription, encodedMajorViolation, lnkMajorViolationID) : encodedMajorViolation;
                        resultList.Add(text);
                    }
                }

                result = string.Join("<br>", resultList.ToArray());
            }
            else
            {
                result = LabelUtil.GetTextByKey("aca_foodfacilitydetail_label_nomajorviolations", null);
            }

            return result;
        }

        /// <summary>
        /// Determines whether [has valid result image] [the specified agency code].
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="logoType">Type of the logo.</param>
        /// <returns>
        /// <c>true</c> if [has valid result image] [the specified agency code]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValidResultImage(string agencyCode, string logoType)
        {
            bool result = false;

            var logo = ObjectFactory.GetObject<ILogoBll>();
            LogoModel logoModel = logo.GetAgencyLogoByType(agencyCode, logoType);

            if (logoModel != null)
            {
                byte[] datas = logoModel.docContent;

                result = datas.Length > 0;
            }

            return result;
        }

        /// <summary>
        /// Builds the time value.
        /// </summary>
        /// <param name="theDateTime">The date time.</param>
        /// <param name="timeOption">The time option.</param>
        /// <param name="amOrPmValue">The am or pm value.</param>
        /// <param name="specificTimeValue">The specific time value.</param>
        public static void BuildTimeValue(DateTime? theDateTime, InspectionTimeOption timeOption, out string amOrPmValue, out string specificTimeValue)
        {
            amOrPmValue = string.Empty;
            specificTimeValue = string.Empty;

            if (theDateTime != null)
            {
                switch (timeOption)
                {
                    case InspectionTimeOption.AllDay:
                        amOrPmValue = string.Empty;
                        specificTimeValue = string.Empty;
                        break;
                    case InspectionTimeOption.AM:
                        amOrPmValue = "AM";
                        specificTimeValue = string.Empty;
                        break;
                    case InspectionTimeOption.PM:
                        amOrPmValue = "PM";
                        specificTimeValue = string.Empty;
                        break;
                    case InspectionTimeOption.SpecificTime:
                    case InspectionTimeOption.Unknow:
                    default:
                        amOrPmValue = theDateTime.Value.Hour < 12 ? "AM" : "PM";
                        specificTimeValue = theDateTime.Value.ToString(ACAConstant.HOUR12_MINTUTE_TIME_FORMAT);
                        break;
                }
            }
        }

        /// <summary>
        /// Builds the date time text.
        /// </summary>
        /// <param name="theDateTime">The date time.</param>
        /// <param name="timeOption">The time option.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>the date time text.</returns>
        public static string BuildDateTimeText(DateTime? theDateTime, InspectionTimeOption timeOption, string moduleName)
        {
            string result = string.Empty;

            if (theDateTime != null)
            {
                string theDateString = string.Empty;
                string theTimeString = string.Empty;

                switch (timeOption)
                {
                    case InspectionTimeOption.AllDay:
                        result = I18nDateTimeUtil.FormatToDateStringForUI(theDateTime.Value);
                        break;
                    case InspectionTimeOption.AM:
                    case InspectionTimeOption.PM:
                        theDateString = I18nDateTimeUtil.FormatToDateStringForUI(theDateTime.Value);
                        theTimeString = BuildTimeText(theDateTime, timeOption, moduleName);
                        result = string.Format("{0} {1}", theDateString, theTimeString);
                        break;
                    case InspectionTimeOption.SpecificTime:
                    case InspectionTimeOption.Unknow:
                    default:
                        result = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(theDateTime.Value);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Builds the time text.
        /// </summary>
        /// <param name="theDateTime">The date time.</param>
        /// <param name="timeOption">The time option.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>the time text.</returns>
        public static string BuildTimeText(DateTime? theDateTime, InspectionTimeOption timeOption, string moduleName)
        {
            string result = string.Empty;

            if (theDateTime != null)
            {
                switch (timeOption)
                {
                    case InspectionTimeOption.AM:
                        result = LabelUtil.GetTextByKey("aca_calendar_morning", moduleName);
                        break;
                    case InspectionTimeOption.PM:
                        result = LabelUtil.GetTextByKey("aca_calendar_afternoon", moduleName);
                        break;
                    case InspectionTimeOption.SpecificTime:
                        result = I18nDateTimeUtil.FormatToTimeStringForUI(theDateTime.Value, true);
                        break;
                    case InspectionTimeOption.AllDay:
                        result = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", moduleName);
                        break;
                    case InspectionTimeOption.Unknow:
                    default:
                        result = string.Empty;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get status label key
        /// </summary>
        /// <param name="actualStatus">the instance of InspectionStatus</param>
        /// <returns>status label key</returns>
        public static string GetLabelKey(InspectionStatus actualStatus)
        {
            string result = string.Empty;

            switch (actualStatus)
            {
                case InspectionStatus.Canceled:
                    result = "ins_inspectionStatus_label_Cancelled";
                    break;

                case InspectionStatus.InitialRequired:
                    result = "ACA_Inspection_Status_Required";
                    break;

                case InspectionStatus.InitialOptional:
                    result = "ACA_Inspection_Status_Optional";
                    break;

                case InspectionStatus.PendingByACA:
                case InspectionStatus.PendingByV360:
                case InspectionStatus.ResultPending:
                    result = "ins_inspectionStatus_label_Pending";
                    break;

                case InspectionStatus.Rescheduled:
                    result = "ins_inspectionStatus_label_Rescheduled";
                    break;

                case InspectionStatus.Scheduled:
                    result = "ins_inspectionStatus_label_Scheduled";
                    break;

                case InspectionStatus.ResultApproved:
                case InspectionStatus.ResultDenied:
                case InspectionStatus.FlowCompleted:
                case InspectionStatus.FlowCompletedButActive:
                case InspectionStatus.FlowPrerequisiteNotMet:
                case InspectionStatus.Unknown:
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets the estimated arrival time.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>Return the estimated arrival time.</returns>
        public static string GetEstimatedArrivateTime(string startTime, string endTime, string moduleName)
        {
            if (string.Equals(startTime, InspectionTimeOption.AM.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return LabelUtil.GetTextByKey("aca_calendar_morning", moduleName);
            }

            if (string.Equals(startTime, InspectionTimeOption.PM.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                return LabelUtil.GetTextByKey("aca_calendar_afternoon", moduleName);
            }

            string result = LabelUtil.GetTextByKey("ACA_Inspection_BlankInspector_TBD", moduleName);
            startTime = !string.IsNullOrEmpty(startTime) ? I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(startTime, true) : string.Empty;
            endTime = !string.IsNullOrEmpty(endTime) ? I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(endTime, true) : string.Empty;

            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
            {
                result = string.Format("{0} - {1}", startTime, endTime);
            }
            else if (!string.IsNullOrEmpty(startTime))
            {
                result = startTime;
            }
            else if (!string.IsNullOrEmpty(endTime))
            {
                result = endTime;
            }

            return result;
        }

        /// <summary>
        /// Gets the desired date.
        /// </summary>
        /// <param name="inspectionDataModel">The inspectionDataModel model.</param>
        /// <param name="timeOption">The inspection time option.</param>
        /// <returns>Return the desired date.</returns>
        public static DateTime? GetDesiredDate(InspectionDataModel inspectionDataModel, out InspectionTimeOption timeOption)
        {
            timeOption = InspectionTimeOption.Unknow;

            if (inspectionDataModel == null)
            {
                return null;
            }

            DateTime? result = null;
            InspectionModel inspectionModel = inspectionDataModel.InspectionModel;
            
            if (inspectionModel != null && inspectionModel.activity != null && inspectionModel.activity.desiredDate.HasValue)
            {
                DateTime? date = inspectionModel.activity.desiredDate;
                string time = inspectionModel.activity.desiredTime2;
                string ampm = inspectionModel.activity.desiredTime;

                result = I18nDateTimeUtil.GetInspectionDate(date, time, ampm, out timeOption);
            }

            return result;
        }

        /// <summary>
        /// Gets the status date.
        /// </summary>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        /// <param name="statusDateTime">The status date time.</param>
        /// <param name="statusDateTimeText">The status date time text.</param>
        /// <param name="statusDateText">The status date text.</param>
        /// <param name="statusTimeText">The status time text.</param>
        private static void GetStatusDateTime(InspectionViewModel inspectionViewModel, out DateTime? statusDateTime, out string statusDateTimeText, out string statusDateText, out string statusTimeText)
        {
            InspectionDataModel dataModel = inspectionViewModel.InspectionDataModel;

            switch (dataModel.Status)
            {
                case InspectionStatus.PendingByACA:
                case InspectionStatus.PendingByV360:
                    if (dataModel.ReadyTimeAvailable)
                    {
                        statusDateTime = inspectionViewModel.ReadyTime;
                        statusDateTimeText = inspectionViewModel.ReadyTimeDateTimeText;
                        statusDateText = inspectionViewModel.ReadyTimeDateText;
                        statusTimeText = inspectionViewModel.ReadyTimeTimeText;
                    }
                    else
                    {
                        statusDateTime = inspectionViewModel.RequestedDateTime;
                        statusDateTimeText = inspectionViewModel.RequestedDateTimeText;
                        statusDateText = inspectionViewModel.RequestedDateText;
                        statusTimeText = inspectionViewModel.RequestedTimeText;
                    }

                    break;
                case InspectionStatus.Scheduled:
                    statusDateTime = inspectionViewModel.ScheduledDateTime;
                    statusDateTimeText = inspectionViewModel.ScheduledDateTimeText;
                    statusDateText = inspectionViewModel.ScheduledDateText;
                    statusTimeText = inspectionViewModel.ScheduledTimeText;
                    break;
                case InspectionStatus.FlowCompleted:
                case InspectionStatus.FlowCompletedButActive:
                case InspectionStatus.ResultApproved:
                case InspectionStatus.ResultDenied:
                case InspectionStatus.ResultPending:
                    statusDateTime = inspectionViewModel.ResultedDateTime;
                    statusDateTimeText = inspectionViewModel.ResultedDateTimeText;
                    statusDateText = inspectionViewModel.ResultedDateText;
                    statusTimeText = inspectionViewModel.ResultedTimeText;
                    break;
                case InspectionStatus.Canceled:
                    statusDateTime = inspectionViewModel.LastUpdatedDateTime;
                    statusDateTimeText = inspectionViewModel.LastUpdatedDateText;
                    statusDateText = inspectionViewModel.LastUpdatedDateText;
                    statusTimeText = inspectionViewModel.LastUpdatedTimeText;
                    break;
                case InspectionStatus.Rescheduled:
                case InspectionStatus.FlowPrerequisiteNotMet:
                case InspectionStatus.InitialOptional:
                case InspectionStatus.InitialRequired:
                case InspectionStatus.Unknown:
                default:
                    statusDateTime = inspectionViewModel.LastUpdatedDateTime;
                    statusDateTimeText = inspectionViewModel.LastUpdatedDateTimeText;
                    statusDateText = inspectionViewModel.LastUpdatedDateText;
                    statusTimeText = inspectionViewModel.LastUpdatedTimeText;
                    break;
            }
        }
        
        /// <summary>
        /// Gets the major violations.
        /// </summary>
        /// <param name="guideSheetModels">The guide sheet models.</param>
        /// <returns>the major violations</returns>
        private static Dictionary<string, string> GetMajorViolations(GGuideSheetModel[] guideSheetModels)
        {
            Dictionary<string, string> result = null;

            if (guideSheetModels != null)
            {
                result = (from gs in guideSheetModels
                          where gs != null
                              && gs.gGuideSheetItemModels != null
                          from gsi in gs.gGuideSheetItemModels
                          where !string.IsNullOrEmpty(gsi.guideItemText)
                          select new
                          {
                              ItemText = I18nStringUtil.GetString(gsi.resGuideItemText, gsi.guideItemText),
                              ItemDescription = I18nStringUtil.GetString(gsi.resGuideItemComment, gsi.guideItemComment)
                          }).ToDictionary(gi => gi.ItemText, gi => gi.ItemDescription);
            }

            return result;
        }
    }
}
