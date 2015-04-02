#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/09/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provide the ability to operation inspection.
    /// </summary>
    public class InspectionBll : BaseBll, IInspectionBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of InspectionService.
        /// </summary>
        private InspectionWebServiceService InspectionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<InspectionWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of InspectionRelationWebService.
        /// </summary>
        private InspectionRelationWebServiceService InspectionRelationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<InspectionRelationWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of EDMSDocumentUploadService.
        /// </summary>
        private EDMSDocumentUploadWebServiceService EDMSDocumentUploadService
        {
            get
            {
                return WSFactory.Instance.GetWebService3<EDMSDocumentUploadWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// batch schedule inspections
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="inspectionModelArray">inspection model of array</param>
        /// <param name="actMode">mode of action,include "Schedule" or "Reschedule" flag</param>
        /// <param name="inspector">The inspector.</param>
        /// <returns>array of inspection ids of the new inspections (not the old rescheduled ones).</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, inspectionModelArray, actMode, inspector</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public long[] BatchScheduleInspections(string serviceProviderCode, InspectionModel[] inspectionModelArray, string actMode, SysUserModel inspector)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || inspectionModelArray == null || inspectionModelArray.Length == 0 || string.IsNullOrEmpty(actMode) || inspector == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "inspectionModelArray", "actMode", "inspector" });
            }

            try
            {
                string[] response = InspectionService.batchScheduleInspections(serviceProviderCode, inspectionModelArray, actMode, inspector);

                long[] returnValues = new long[response.Length];
                int i = 0;

                foreach (string s in response)
                {
                    returnValues[i++] = long.Parse(s, CultureInfo.InvariantCulture);
                }

                return returnValues;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// cancel schedule inspections
        /// </summary>
        /// <param name="serviceProviderCode">the Service Provider Code (Agency) to which the inspections belong.
        /// This parameter cannot be null and it must contain a valid Agency code</param>
        /// <param name="callerID">a string representing the id of the invoker. This id is used for auditing purposes
        /// and is usually the user id of the person logged into the system</param>
        /// <param name="inspections">Array of InspectionModel</param>
        /// <param name="actor">a model of SysUserModel</param>
        /// <returns>count of Success</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID, inspections, actor</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public int CancelInspection(string serviceProviderCode, string callerID, InspectionModel[] inspections, SysUserModel actor)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID) || inspections == null || inspections.Length == 0 || actor == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID", "inspections", "actor" });
            }

            try
            {
                int response = InspectionService.cancelInspection(serviceProviderCode, callerID, inspections, actor);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get confirm message when cancel inspection
        /// </summary>
        /// <param name="inspections">Array of InspectionModel</param>
        /// <param name="serviceProviderCode">the Service Provider Code (Agency) to which the inspections belong.
        /// This parameter cannot be null and it must contain a valid Agency code</param>
        /// <param name="callerID">a string representing the id of the invoker. This id is used for auditing purposes
        /// and is usually the user id of the person logged into the system</param>
        /// <returns>confirm message</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID, inspections, actor</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetConfirmMessageWhenCancel(InspectionModel[] inspections, string serviceProviderCode, string callerID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID) || inspections == null || inspections.Length == 0)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID", "inspections", "actor" });
            }

            try
            {
                string message = InspectionService.getConfirmMessageWhenCancel(inspections, serviceProviderCode, callerID);

                // java return the \\\n, need to remove the \n
                message = (message != null && message.Length > 3 && message.EndsWith("\\\n")) ? message.Substring(0, message.Length - 2) : message;

                return message;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the inspection date range can be selected.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="userId">public user sequence number</param>
        /// <param name="module">module name</param>
        /// <param name="inspectionType">inspection type model</param>
        /// <returns>RangeDate Model</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, publicUserSeqNum, module, inspectionType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RangeDateModel GetDatePermissionRangeByInType(string servProvCode, string userId, string module, InspectionTypeModel inspectionType)
        {
            if (string.IsNullOrEmpty(servProvCode) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(module) || inspectionType == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "publicUserSeqNum", "module", "inspectionType" });
            }

            try
            {
                RangeDateModel response = InspectionService.getDatePermissionRangeByInType(servProvCode, userId, module, new InspectionTypeModel[1] { inspectionType });

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get inspections by cap ID.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Array of InspectionModel.</returns>
        /// <exception cref="DataValidateException">{ <c>capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public InspectionModel[] GetInspectionListByCapID(string moduleName, CapIDModel capID, QueryFormat queryFormat, string callerID)
        {
            if (capID == null)
            {
                throw new DataValidateException(new string[] { "capID" });
            }

            try
            {
                InspectionModel[] response = InspectionService.getInspectionListByCapID(capID, queryFormat, callerID);

                if (!string.IsNullOrEmpty(moduleName))
                {
                    response = FilterInspections(AgencyCode, moduleName, response);
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate inspection date time and get the available time result.
        /// </summary>
        /// <param name="serviceProviderCode">the serviceProviderCode value.</param>
        /// <param name="inspectionModel">the InspectionModel object.</param>
        /// <returns>get the available result.</returns>
        public AvailableTimeResultModel ValidateInspectionDateTime(string serviceProviderCode, InspectionModel inspectionModel)
        {
            if (inspectionModel == null)
            {
                throw new DataValidateException(new string[] { "inspectionModel" });
            }

            try
            {
                return InspectionService.validateInspectionDateTime(serviceProviderCode, inspectionModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate inspection date time and get the available time result.
        /// </summary>
        /// <param name="serviceProviderCode">the serviceProviderCode value.</param>
        /// <param name="inspectionModel">the InspectionModel object.</param>
        /// <returns>get the available result.</returns>
        public AvailableTimeResultModel ValidateInspectionDateTimeWhileNotBlockSchedule(string serviceProviderCode, InspectionModel inspectionModel)
        {
            if (inspectionModel == null)
            {
                throw new DataValidateException(new string[] { "inspectionModel" });
            }

            try
            {
                return InspectionService.validateInspectionDateTimeWhileNotBlockSchedule(serviceProviderCode, inspectionModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the inspection list for food facility.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="licenseType">The license type.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>the inspection list for food facility.</returns>
        public List<InspectionDataModel> GetInspectionListForFoodFacility(CapModel4WS recordModel, string licenseType, QueryFormat queryFormat)
        {
            if (recordModel == null)
            {
                throw new DataValidateException(new string[] { "recordModel" });
            }

            try
            {
                var result = new List<InspectionDataModel>();
                var recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);
                string callerID = User == null ? string.Empty : User.PublicUserId;

                InspectionModel[] inspectionModels = InspectionService.getInspectionListForFoodFacility(recordIDModel, licenseType, queryFormat, callerID);
                var inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();
                InspectionTypeModel[] inspectionTypes = inspectionTypeBll.GetInspectionTypesByCapID(recordIDModel, User.PublicUserId);

                if (inspectionModels != null && inspectionTypes != null)
                {
                    bool allowSchedule = false;

                    foreach (InspectionModel inspectionModel in inspectionModels)
                    {
                        InspectionDataModel dataModel = null;

                        InspectionTypeModel inspectionTypeModel = null;

                        if (inspectionModel.activity != null)
                        {
                            var inspectionTypeQueryResult = from t in inspectionTypes
                                                            where t.sequenceNumber == inspectionModel.activity.inspSequenceNumber
                                                            select t;
                            inspectionTypeModel = inspectionTypeQueryResult.FirstOrDefault();
                        }

                        if (inspectionTypeModel != null)
                        {
                            dataModel = BuildDataModel(null, recordModel, inspectionModel, inspectionTypeModel, false, allowSchedule, User.UserModel4WS);
                        }

                        if (dataModel != null && dataModel.AllowDisplayInACA)
                        {
                            result.Add(dataModel);
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get all inspections
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="callerID">caller id number.</param>
        /// <returns>Array inspection information</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public InspectionModel[] GetInspections(string moduleName, string serviceProviderCode, QueryFormat queryFormat, string callerID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                InspectionModel[] response = InspectionService.getInspections(serviceProviderCode, queryFormat, callerID);

                response = FilterInspections(serviceProviderCode, moduleName, response);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the inspections who have permission to result these.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="status">The status for get new inspection(NEW) or result inspection(RESULT).</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>The inspections that have permission to result inspection.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        SearchResultModel IInspectionBll.GetMyInspections(string agencyCode, string status, QueryFormat queryFormat)
        {
            try
            {
                return InspectionService.getMyInspections(agencyCode, status, queryFormat, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the CSV document stream that the inspections who have permission to result
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>The CSV document stream.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        byte[] IInspectionBll.GetMyInspectionsCSV(string agencyCode)
        {
            try
            {
                return InspectionService.getMyInspectionsCSV(agencyCode, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update the inspection according to the CSV file that contain the inspection result information.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="documentModel">The document model.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>Update the inspection success or failed.</returns>
        EMSEResultBaseModel4WS IInspectionBll.UpdateInspectionByCSV(string agencyCode, DocumentModel documentModel, string filePath)
        {
            try
            {
                documentModel.documentContent.docContentStream = File.ReadAllBytes(filePath);

                return EDMSDocumentUploadService.updateInspectionByCSV(agencyCode, documentModel, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate the schedule operation according to inspection flow
        /// </summary>
        /// <param name="inspTypes">Array of inspection type</param>
        /// <param name="capId">The cap id</param>
        /// <returns>InspectionValidation Model</returns>
        public InspectionValidationModel Validate4Schedule(string[] inspTypes, CapIDModel capId)
        {
            if (capId == null || inspTypes == null || inspTypes.Length == 0)
            {
                throw new DataValidateException(new string[] { "inspTypes", "capId" });
            }

            try
            {
                InspectionValidationModel result = InspectionService.validate4Schedule(inspTypes, capId);

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// This method to validate schedule date according to the inspection flow.
        /// </summary>
        /// <param name="capID">The cap id</param>
        /// <param name="inspection">inspection model</param>
        /// <returns>confirm message key of AA (the key is not used in ACA). In ACA, the message is re-defined.</returns>
        /// <exception cref="DataValidateException">{ <c>capID, inspection</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string ValidateScheduleDateByFlow(CapIDModel capID, InspectionModel inspection)
        {
            if (capID == null || inspection == null)
            {
                throw new DataValidateException(new string[] { "capID", "inspection" });
            }

            try
            {
                string messageKey = InspectionService.validateScheduleDateByFlow(capID, inspection);

                return messageKey;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the inspection data models.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="isCapLockedOrHold">if set to <c>true</c> [is cap locked or hold].</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns>the inspection data models</returns>
        /// <exception cref="DataValidateException">{ <c>moduleName, recordModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public List<InspectionDataModel> GetDataModels(string moduleName, CapModel4WS recordModel, bool isCapLockedOrHold, PublicUserModel4WS currentUser)
        {
            if (string.IsNullOrEmpty(moduleName) || recordModel == null)
            {
                throw new DataValidateException(new string[] { "moduleName", "recordModel" });
            }

            try
            {
                var result = new List<InspectionDataModel>();
                var inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();

                InspectionModel[] inspectionModels = GetInspectionListByCapID(moduleName, TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID), null, User.PublicUserId);
                InspectionTypeModel[] inspectionTypes = inspectionTypeBll.GetInspectionTypesByCapID(TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID), User.PublicUserId);

                if (inspectionModels != null)
                {
                    bool allowSchedule = InspectionPermissionUtil.AllowSchedule(AgencyCode, moduleName, User.IsAnonymous, recordModel);

                    foreach (InspectionModel inspectionModel in inspectionModels)
                    {
                        InspectionDataModel dataModel = null;

                        InspectionTypeModel inspectionTypeModel = null;

                        if (inspectionModel.activity != null && inspectionTypes != null)
                        {
                            var inspectionTypeQueryResult = from t in inspectionTypes
                                                            where t.sequenceNumber == inspectionModel.activity.inspSequenceNumber
                                                            select t;
                            inspectionTypeModel = inspectionTypeQueryResult.FirstOrDefault();
                        }
                        
                        dataModel = BuildDataModel(moduleName, recordModel, inspectionModel, inspectionTypeModel, isCapLockedOrHold, allowSchedule, currentUser);

                        if (dataModel != null && dataModel.AllowDisplayInACA)
                        {
                            result.Add(dataModel);
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get action mode.
        /// </summary>
        /// <param name="action">the type of InspectionAction</param>
        /// <returns>base on the InspectionAction</returns>
        public string GetActionMode(InspectionAction action)
        {
            return InspectionActionUtil.GetActionMode(action);
        }

        /// <summary>
        /// get next actual status after action.
        /// </summary>
        /// <param name="action">Inspection Action</param>
        /// <returns>next actual status</returns>
        public string GetStatusAfterAction(InspectionAction action)
        {
            return InspectionStatusUtil.GetNextSystemDefinedStatus(action);
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>the actual status</returns>
        public InspectionStatus GetStatus(InspectionModel inspectionModel)
        {
            return InspectionStatusUtil.GetStatus(inspectionModel);
        }

        /// <summary>
        /// Gets the res status string.
        /// </summary>
        /// <param name="inspectionStatus">The inspection status.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>the res status string.</returns>
        public string GetResStatusString(InspectionStatus inspectionStatus, InspectionModel inspectionModel)
        {
            return InspectionStatusUtil.GetResStatusString(inspectionStatus, inspectionModel);
        }

        /// <summary>
        /// get inspection tree.the parent level is stored in first tree node model's inspection model.
        /// The other level's inspect model store current inspect model.
        /// </summary>
        /// <param name="capID">CapID Model</param>
        /// <param name="inspectionID">inspection ID</param>
        /// <returns>InspectionTreeNodeModel model</returns>
        public InspectionTreeNodeModel GetRelatedInspections(CapIDModel capID, string inspectionID)
        {
            return InspectionRelationService.getRelatedInspections(capID, Convert.ToInt64(inspectionID));
        }

        /// <summary>
        /// get inspection status history list and result comment list are transformed by InspectionDataModel.
        /// </summary>
        /// <param name="inspectionID">inspection id</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns>IList InspectionDataModel</returns>
        public IList<InspectionDataModel> GetInspectionHistoryList(long inspectionID, string moduleName, CapModel4WS recordModel, PublicUserModel4WS currentUser)
        {
            var results = new List<InspectionDataModel>();
            var recordIDModel = recordModel != null ? TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID) : null;
            var inspectionModels = ObjectConvertUtil.ConvertArrayToList(InspectionService.getInspectionHistoryList(recordIDModel, inspectionID));

            if (inspectionModels != null)
            {
                foreach (var inspectionModel in inspectionModels)
                {
                    var tempDataModel = BuildDataModel(moduleName, recordModel, inspectionModel, null, true, false, currentUser);
                    results.Add(tempDataModel);
                }
            }

            return results;
        }

        /// <summary>
        /// Gets the activity date.
        /// </summary>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <param name="timeOption">The time option.</param>
        /// <returns>the activity date with timeOption output.</returns>
        public DateTime? GetActivityDate(InspectionModel inspectionModel, out InspectionTimeOption timeOption)
        {
            return InspectionUtil.GetActivityDate(inspectionModel, out timeOption);
        }

        /// <summary>
        /// Builds the data model.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <param name="isCapLockedOrHold">if set to <c>true</c> [is cap locked or hold].</param>
        /// <param name="allowSchedule">if set to <c>true</c> [allow schedule].</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns>the data model</returns>
        private InspectionDataModel BuildDataModel(string moduleName, CapModel4WS recordModel, InspectionModel inspectionModel, InspectionTypeModel inspectionTypeModel, bool isCapLockedOrHold, bool allowSchedule, PublicUserModel4WS currentUser)
        {
            InspectionDataModel result = null;

            if (inspectionModel.activity != null)
            {
                var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();

                // assignment prepration
                var inspectionStatus = inspectionTypeModel == null ? InspectionStatusUtil.GetStatus(inspectionModel) : InspectionStatusUtil.GetStatus(inspectionTypeModel, inspectionModel);
               
                //get inspection status
                string resStatus = string.Empty;

                if (!IsUpcomingInspection(inspectionStatus))
                {
                    resStatus = InspectionStatusUtil.GetResStatusString(inspectionStatus, inspectionModel);
                }

                var requiredInspection = ACAConstant.COMMON_Y.Equals(inspectionModel.activity.requiredInspection, StringComparison.OrdinalIgnoreCase) ? true : false;
                var scheduleType = InspectionScheduleTypeUtil.GetScheduleType(inspectionTypeModel);
                var readyTimeEnabled = inspectionTypeModel == null ? false : InspectionPermissionUtil.IsReadyTimeEnabled(inspectionTypeModel);
                var readyTimeAvailable = InspectionPermissionUtil.IsReadyTimeAvailable(readyTimeEnabled, inspectionStatus);
                var allowDisplayInACA = string.IsNullOrEmpty(inspectionModel.activity.displayInACA) || ValidationUtil.IsYes(inspectionModel.activity.displayInACA);
                var allowMultiple = inspectionTypeModel == null ? false : InspectionPermissionUtil.AllowMultipleInspections(moduleName, inspectionTypeModel.allowMultiInsp);
                var allowViewResultComments = inspectionModel.activity.isRestrictView4ACA == null || (ValidationUtil.IsYes(inspectionModel.activity.isRestrictView4ACA) && userRoleBll.HasReadOnlyPermission(recordModel, inspectionModel.activity.userRolePrivilegeModel, currentUser));
                long inspectionTypeID = inspectionModel != null && inspectionModel.activity != null && inspectionModel.activity.inspSequenceNumber != null ? inspectionModel.activity.inspSequenceNumber.Value : -1;
                var timeOption = InspectionTimeOption.Unknow;
                var activityDateTime = InspectionUtil.GetActivityDate(inspectionModel, out timeOption);

                List<InspectionAction> availableOperations = new List<InspectionAction>();
                InspectionAction mainAction = InspectionAction.None;
                InspectionAction cancelAction = InspectionAction.None;

                if (allowSchedule && !isCapLockedOrHold)
                {
                    mainAction = inspectionTypeModel == null ? InspectionAction.None : InspectionActionUtil.GetAction(this.AgencyCode, moduleName, recordModel.statusGroupCode, recordModel.capStatus, inspectionTypeModel, inspectionModel);
                    bool isMainActionableByAppStatus = inspectionTypeModel == null ? true : InspectionPermissionUtil.IsActionableByAppStatus(recordModel.statusGroupCode, recordModel.capStatus, inspectionTypeModel.groupCode, inspectionTypeModel.type, mainAction);

                    if (!isMainActionableByAppStatus)
                    {
                        mainAction = InspectionAction.None;
                    }

                    cancelAction = inspectionTypeModel == null ? InspectionAction.None : InspectionActionUtil.GetCancelAction(this.AgencyCode, moduleName, recordModel.statusGroupCode, recordModel.capStatus, inspectionTypeModel, inspectionModel);

                    bool isCancelActionableByAppStatus = inspectionTypeModel == null ? true : InspectionPermissionUtil.IsActionableByAppStatus(recordModel.statusGroupCode, recordModel.capStatus, inspectionTypeModel.groupCode, inspectionTypeModel.type, cancelAction);

                    if (!isCancelActionableByAppStatus)
                    {
                        cancelAction = InspectionAction.None;
                    }

                    if (!availableOperations.Contains(mainAction) && mainAction != InspectionAction.None)
                    {
                        availableOperations.Add(mainAction);
                    }

                    if (!availableOperations.Contains(cancelAction) && cancelAction != InspectionAction.None)
                    {
                        availableOperations.Add(cancelAction);
                    }
                }

                InspectionAction viewDetailOperation = InspectionAction.ViewDetails;

                if (!availableOperations.Contains(viewDetailOperation) && viewDetailOperation != InspectionAction.None)
                {
                    availableOperations.Add(viewDetailOperation);
                }

                //begin assigning for data model
                InspectionDataModel dataModel = new InspectionDataModel();
                dataModel.InspectionModel = inspectionModel;

                //basic properties
                dataModel.ID = inspectionModel.activity.idNumber;
                dataModel.Inspector = inspectionModel.activity.sysUser;
                dataModel.Type = inspectionModel.activity.activityType;
                dataModel.TypeID = inspectionTypeID;
                dataModel.TimeOption = timeOption;
                dataModel.ScheduledDateTime = activityDateTime;
                dataModel.RequestComments = inspectionModel.requestComment != null ? inspectionModel.requestComment.text : string.Empty;
                dataModel.RequestedDateTime = activityDateTime;
                dataModel.ResultedDateTime = inspectionModel.activity.completionDate;
                dataModel.ResultComments = allowViewResultComments ? inspectionModel.resultComment : string.Empty;

                dataModel.ContactFirstName = inspectionModel.activity.contactFname;
                dataModel.ContactMiddleName = inspectionModel.activity.contactMname;
                dataModel.ContactLastName = inspectionModel.activity.contactLname;
                dataModel.ContactPhoneNumber = inspectionModel.activity.contactPhoneNum;
                dataModel.ContactPhoneIDD = inspectionModel.activity.contactPhoneNumIDD;

                dataModel.RequestorFirstName = inspectionModel.activity.requestorFname;
                dataModel.RequestorMiddleName = inspectionModel.activity.requestorMname;
                dataModel.RequestorLastName = inspectionModel.activity.requestorLname;
                dataModel.RequestorPhoneNumber = inspectionModel.activity.reqPhoneNum;
                dataModel.RequestorPhoneIDD = inspectionModel.activity.reqPhoneNumIDD;

                string lastUpdateBy = string.Empty;

                if (!string.IsNullOrEmpty(inspectionModel.activity.auditID))
                {
                    //set last update peopel value by user contact
                    IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                    PublicUserModel4WS user = null;

                    try
                    {
                        //if user status is inactive or disabled, there throw a exception.
                        user = accountBll.GetPublicUserByEmailOrUserId(ConfigManager.AgencyCode, inspectionModel.activity.auditID);
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(GetType()).Error(ex);
                    }

                    if (user != null)
                    {
                        IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                        lastUpdateBy = peopleBll.GetContactUserName(user, true);
                    }

                    /*
                     * When lastUpdateBy not set value, there has 3 conditions:
                     * 1. auditID is sysuserid
                     * 2. auditID is public user contact name format in Java side.
                     * 3. User inactive or disable
                     * We need display lastUdpateBy that using the inspectionModel.activity.auditID.
                     * */
                    if (string.IsNullOrEmpty(lastUpdateBy))
                    {
                        lastUpdateBy = inspectionModel.activity.auditID;
                    }
                }

                dataModel.LastUpdated = inspectionModel.activity.auditDate.Value;
                dataModel.LastUpdatedBy = lastUpdateBy;
                dataModel.Score = inspectionModel.activity.totalScore;
                dataModel.Grade = inspectionModel.activity.grade != null ? inspectionModel.activity.grade : string.Empty;

                string gradeGroup = inspectionTypeModel != null ? inspectionTypeModel.grade : string.Empty;
                dataModel.GradeImageKey = GetGradeImageKey(gradeGroup, inspectionModel.activity.grade);
                dataModel.GuideSheetModels = inspectionModel.gGuideSheetModels;

                //permission related.
                dataModel.ScheduleType = scheduleType;
                dataModel.Status = inspectionStatus;
                dataModel.StatusString = resStatus;
                dataModel.AllowDisplayInACA = allowDisplayInACA;
                dataModel.AllowMultiple = allowMultiple;
                dataModel.AllowViewResultComments = allowViewResultComments;
                dataModel.AvailableOperations = availableOperations.ToArray();
                dataModel.ReadyTimeEnabled = readyTimeEnabled;
                dataModel.ReadyTimeAvailable = readyTimeAvailable;
                dataModel.Required = requiredInspection;
                dataModel.ReadyTime = readyTimeAvailable ? activityDateTime : null;
                dataModel.IsUpcomingInspection = IsUpcomingInspection(inspectionStatus);
                dataModel.MainAction = mainAction;
                dataModel.CancelAction = cancelAction;

                //properties depending on inspection type
                if (inspectionTypeModel != null)
                {
                    dataModel.InspectionTypeModel = inspectionTypeModel;
                    dataModel.Group = inspectionTypeModel.groupCode;
                    dataModel.InAdvance = inspectionTypeModel.isInAdvance;
                    dataModel.RestrictionSettings4Cancel = this.WrapCancellationRestrictionSettings(inspectionTypeModel);
                    dataModel.RestrictionSettings4Reschedule = this.WrapRescheduleRestrictionSettings(inspectionTypeModel);
                    dataModel.ResultGroup = inspectionTypeModel.resultGroup != null ? inspectionTypeModel.resultGroup.groupName : string.Empty;
                    dataModel.Units = inspectionTypeModel.inspUnits == null ? 0 : inspectionTypeModel.inspUnits.Value;
                }

                result = dataModel;
            }

            return result;
        }

        /// <summary>
        /// Gets the grade image key.
        /// </summary>
        /// <param name="gradeGroup">The inspection grade group.</param>
        /// <param name="grade">The inspection grade.</param>
        /// <returns>the grade image key.</returns>
        private string GetGradeImageKey(string gradeGroup, string grade)
        {
            if (string.IsNullOrEmpty(gradeGroup) || string.IsNullOrEmpty(grade))
            {
                return string.Empty;
            }

            string result = string.Empty;
            IInspectionTypeBll inspectionTypeBll = ObjectFactory.GetObject<IInspectionTypeBll>();
            InspectionResultModel[] gradeList = inspectionTypeBll.GetInspectionResultByGroupName(AgencyCode, gradeGroup, ACAConstant.INSPECTION_RESULT_CATEGORY_GRADE);

            if (gradeList != null)
            {
                foreach (InspectionResultModel model in gradeList)
                {
                    if (grade.Equals(model.resultValue, StringComparison.OrdinalIgnoreCase))
                    {
                        result = model.resultImage;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether is upcoming inspection.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>
        /// <c>true</c> if is upcoming inspection; otherwise, <c>false</c>.
        /// </returns>
        private bool IsUpcomingInspection(InspectionStatus status)
        {
            bool result = status == InspectionStatus.Scheduled
                          || status == InspectionStatus.InitialOptional
                          || status == InspectionStatus.InitialRequired
                          || status == InspectionStatus.PendingByACA
                          || status == InspectionStatus.PendingByV360
                          || status == InspectionStatus.FlowPrerequisiteNotMet;

            return result;
        }

        /// <summary>
        /// Wraps the reschedule restriction settings.
        /// </summary>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>
        /// the reschedule restriction settings wrapped.
        /// </returns>
        private string WrapRescheduleRestrictionSettings(InspectionTypeModel inspectionType)
        {
            StringBuilder resultBuilder = new StringBuilder();

            if (inspectionType != null)
            {
                resultBuilder.Append(inspectionType.reScheduleOptionInACA);
                resultBuilder.Append(ACAConstant.SPLIT_CHAR4URL1);
                resultBuilder.Append(inspectionType.reScheduleDaysInACA);
                resultBuilder.Append(ACAConstant.SPLIT_CHAR4URL1);
                resultBuilder.Append(inspectionType.reScheduleHoursInACA);
                resultBuilder.Append(ACAConstant.SPLIT_CHAR4URL1);
                resultBuilder.Append(inspectionType.reScheduleTimeInACA);
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Wraps the cancellation restriction settings.
        /// </summary>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>
        /// the cancellation restriction settings wrapped.
        /// </returns>
        private string WrapCancellationRestrictionSettings(InspectionTypeModel inspectionType)
        {
            StringBuilder resultBuilder = new StringBuilder();

            if (inspectionType != null)
            {
                resultBuilder.Append(inspectionType.cancelOptionInACA);
                resultBuilder.Append(ACAConstant.SPLIT_CHAR4URL1);
                resultBuilder.Append(inspectionType.cancelDaysInACA);
                resultBuilder.Append(ACAConstant.SPLIT_CHAR4URL1);
                resultBuilder.Append(inspectionType.cancelHoursInACA);
                resultBuilder.Append(ACAConstant.SPLIT_CHAR4URL1);
                resultBuilder.Append(inspectionType.cancelTimeInACA);
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Filters the inspections.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="inspections">The inspections.</param>
        /// <returns>the filtered  inspections.</returns>
        private InspectionModel[] FilterInspections(string agencyCode, string moduleName, InspectionModel[] inspections)
        {
            InspectionModel[] results = inspections;

            if (inspections != null)
            {
                bool allowDisplayOfOptionalInspections = InspectionPermissionUtil.AllowDisplayOfOptionalInspections(agencyCode, moduleName);

                var searchResults = from i in inspections
                                    where i.activity != null
                                          && (ValidationUtil.IsYes(i.activity.requiredInspection) || allowDisplayOfOptionalInspections)
                                    select i;

                results = searchResults.ToArray();
            }

            return results;
        }

        #endregion Methods
    }
}