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
 * $Id: InspectionTypeBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/09/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provides the ability to operation inspection type.
    /// </summary>
    public class InspectionTypeBll : BaseBll, IInspectionTypeBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of InspectionTypeService.
        /// </summary>
        private InspectionTypeWebServiceService InspectionTypeService
        {
            get
            {
                return WSFactory.Instance.GetWebService<InspectionTypeWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets inspection result by inspection result group name.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="groupName">the inspection result group name</param>
        /// <param name="resultCategory">The result category.</param>
        /// <returns>InspectionResultModel array.</returns>
        public InspectionResultModel[] GetInspectionResultByGroupName(string servProvCode, string groupName, string resultCategory)
        {
            try
            {
                if (string.IsNullOrEmpty(servProvCode) || string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(resultCategory))
                {
                    throw new DataValidateException(new string[] { "serviceProviderCode", "groupName", "resultCategory" });
                }

                InspectionResultModel[] inspectionResults = InspectionTypeService.getInspectionResultByGroupName(servProvCode, groupName, resultCategory);

                return inspectionResults;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the inspection types by cap ID.
        /// </summary>
        /// <param name="capID">The cap ID.</param>
        /// <param name="callerID">The caller ID.</param>
        /// <returns>the inspection types</returns>
        public InspectionTypeModel[] GetInspectionTypesByCapID(CapIDModel capID, string callerID)
        {
            try
            {
                if (capID == null || string.IsNullOrEmpty(callerID))
                {
                    throw new DataValidateException(new string[] { "capID", "callerID" });
                }

                InspectionTypeModel[] results = InspectionTypeService.getInspectionTypesByCapID(capID, callerID);

                return results;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets inspection types by cap type,
        /// this method returns all inspection types without filtered by any ACA permission.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capType">the record type</param>
        /// <param name="capID">the record id</param>
        /// <param name="includeOptionalInspections">if set to <c>true</c> [include optional inspection].</param>
        /// <param name="queryFormat">queryFormat model</param>
        /// <param name="callerID">the public user id.</param>
        /// <returns>Inspection Type Array</returns>
        /// <exception cref="DataValidateException">{ <c>capType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public InspectionTypeModel[] GetInspectionTypesByCapType(string moduleName, CapTypeModel capType, CapIDModel capID, bool includeOptionalInspections, QueryFormat queryFormat, string callerID)
        {
            if (capType == null)
            {
                throw new DataValidateException(new string[] { "capType" });
            }

            try
            {
                InspectionTypeModel[] response = InspectionTypeService.getInspectionTypesByCapType(capType, capID, queryFormat, callerID);

                response = FilterInspectionTypes(AgencyCode, moduleName, response, includeOptionalInspections);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the type of the available inspection types by cap type.
        /// this method returns available inspection types filtered by any ACA permission.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="isCapLockedOrHold">if set to <c>true</c> [is cap locked or hold].</param>
        /// <param name="currentUser">The current user.</param>
        /// <param name="includeOptionalInspections">if set to <c>true</c> [include optional inspections].</param>
        /// <param name="queryFormat">The query format.</param>
        /// <param name="callerID">The caller ID.</param>
        /// <returns>the type of the available inspection types by cap type</returns>
        /// <exception cref="DataValidateException">{ <c>moduleName, recordModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public List<InspectionTypeDataModel> GetAvailableInspectionTypes(string moduleName, CapModel4WS recordModel, bool isCapLockedOrHold, PublicUserModel4WS currentUser, bool includeOptionalInspections, QueryFormat queryFormat, string callerID)
        {
            if (string.IsNullOrEmpty(moduleName) || recordModel == null)
            {
                throw new DataValidateException(new string[] { "moduleName", "recordModel" });
            }

            try
            {
                var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
                var result = new List<InspectionTypeDataModel>();

                InspectionModel[] inspectionModels = inspectionBll.GetInspectionListByCapID(moduleName, TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID), queryFormat, User.PublicUserId);
                InspectionTypeModel[] inspectionTypeModels = GetInspectionTypesByCapType(moduleName, recordModel.capType, TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID), includeOptionalInspections, queryFormat, User.PublicUserId);

                if (inspectionTypeModels != null)
                {
                    var inspectionAndTypes =
                        from t in inspectionTypeModels
                        select new
                        {
                            InspectionType = t,
                            Inspections = inspectionModels == null ? (IEnumerable<InspectionModel>)null
                                :
                                from i in inspectionModels
                                where t.sequenceNumber == (i.activity == null ? -1 : i.activity.inspSequenceNumber)
                                select i
                        };

                    if (inspectionAndTypes != null)
                    {
                        var recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);
                        var availableCategories = GetInspectionCategoriesByCapID(recordIDModel);
                        var relationships = GetRelationshipOfCategoryAndInspectionType(recordIDModel);

                        foreach (var ti in inspectionAndTypes)
                        {
                            var inspectionTypeModel = ti.InspectionType;
                            long inspecitonTypeID = inspectionTypeModel == null ? -1 : inspectionTypeModel.sequenceNumber;
                            var filteredCategories = FilterInspectionCategories(availableCategories, relationships, inspecitonTypeID);
                            var categoryArray = filteredCategories == null ? null : filteredCategories.ToArray();
                            var dataModel = BuildDataModel(moduleName, recordModel, ti.InspectionType, ti.Inspections, isCapLockedOrHold, categoryArray);
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
        /// Get inspection groups
        /// </summary>
        /// <returns>Inspection group model</returns>
        public InspectionGroupModel[] GetInspectionGroups()
        {
            return InspectionTypeService.getInspectionGroups(AgencyCode);
        }

        /// <summary>
        /// Get inspection types by inspection group code
        /// </summary>
        /// <param name="insGroupCode">inspection group code</param>
        /// <returns>array for inspection type model</returns>
        public InspectionTypeModel[] GetInspectionTypesByGroupCode(string insGroupCode)
        {
            return InspectionTypeService.getInspectionTypesByGroupCode(AgencyCode, insGroupCode);
        }

        /// <summary>
        /// Gets the inspection categories by cap ID.
        /// </summary>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <returns>the inspection categories.</returns>
        public List<InspectionCategoryDataModel> GetInspectionCategoriesByCapID(CapIDModel recordIDModel)
        {
            List<InspectionCategoryDataModel> result = null;
            var bizDomainModels = InspectionTypeService.getInspectionTypeCategoriesByCapID(recordIDModel);

            if (bizDomainModels != null)
            {
                result = new List<InspectionCategoryDataModel>();

                foreach (var bizDomainModel in bizDomainModels)
                {
                    var category = new InspectionCategoryDataModel();
                    category.ID = bizDomainModel.bizdomainValue;
                    category.Category = I18nStringUtil.GetString(bizDomainModel.resBizdomainValue, bizDomainModel.bizdomainValue);
                    category.DisplayOrder = bizDomainModel.sortOrder;
                    result.Add(category);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the relationship of category and inspection.
        /// </summary>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <returns>
        /// the relationship of category and inspection
        /// </returns>
        private List<XInspectionTypeCategoryModel> GetRelationshipOfCategoryAndInspectionType(CapIDModel recordIDModel)
        {
            var result = new List<XInspectionTypeCategoryModel>();
            var relationships = InspectionTypeService.getRelationshipOfCategoryAndInspectionType(recordIDModel);

            if (relationships != null)
            {
                result.AddRange(relationships);
            }

            return result;
        }

        /// <summary>
        /// Filters the inspection categories.
        /// </summary>
        /// <param name="availableCategories">The available categories.</param>
        /// <param name="relationships">The relationships.</param>
        /// <param name="inspectionTypeID">The inspection type ID.</param>
        /// <returns>the inspection categories.</returns>
        private List<InspectionCategoryDataModel> FilterInspectionCategories(List<InspectionCategoryDataModel> availableCategories, List<XInspectionTypeCategoryModel> relationships, long inspectionTypeID)
        {
            var result = availableCategories;

            if (relationships != null && availableCategories != null)
            {
                result = (from r in relationships
                          from c in availableCategories
                          where r.refInspectionTypeCategoryPKModel != null
                              && !string.IsNullOrEmpty(r.refInspectionTypeCategoryPKModel.categoryName)
                              && !string.IsNullOrEmpty(c.ID)
                              && r.refInspectionTypeCategoryPKModel.categoryName.Equals(c.ID, StringComparison.OrdinalIgnoreCase)
                              && r.refInspectionTypeCategoryPKModel.sequenceNumber == inspectionTypeID
                          select c).ToList();
            }

            return result;
        }

        /// <summary>
        /// Builds the data model.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <param name="inspectionModels">The inspection models.</param>
        /// <param name="isCapLockedOrHold">if set to <c>true</c> [is cap locked or hold].</param>
        /// <param name="categories">The categories.</param>
        /// <returns>the built data model.</returns>
        private InspectionTypeDataModel BuildDataModel(string moduleName, CapModel4WS recordModel, InspectionTypeModel inspectionTypeModel, IEnumerable<InspectionModel> inspectionModels, bool isCapLockedOrHold, InspectionCategoryDataModel[] categories)
        {
            var dataModel = new InspectionTypeDataModel();

            dataModel.InspectionTypeModel = inspectionTypeModel;
            dataModel.Status = InspectionStatusUtil.GetStatus(inspectionTypeModel);
            bool allowMultiple = InspectionPermissionUtil.AllowMultipleInspections(moduleName, inspectionTypeModel.allowMultiInsp);
            dataModel.AllowMultiple = allowMultiple;
            dataModel.Group = inspectionTypeModel.groupCode;
            dataModel.InAdvance = inspectionTypeModel.isInAdvance;
            dataModel.ReadyTimeEnabled = InspectionPermissionUtil.IsReadyTimeEnabled(inspectionTypeModel);
            dataModel.Required = ACAConstant.COMMON_Y.Equals(inspectionTypeModel.requiredInspection, StringComparison.OrdinalIgnoreCase) ? true : false;
            dataModel.RestrictionSettings4Cancel = this.WrapCancellationRestrictionSettings(inspectionTypeModel);
            dataModel.RestrictionSettings4Reschedule = this.WrapRescheduleRestrictionSettings(inspectionTypeModel);
            dataModel.ResultGroup = inspectionTypeModel.resultGroup != null ? inspectionTypeModel.resultGroup.groupName : string.Empty;
            dataModel.ScheduleType = InspectionScheduleTypeUtil.GetScheduleType(inspectionTypeModel);
            dataModel.Type = inspectionTypeModel.type;
            dataModel.TypeID = inspectionTypeModel.sequenceNumber;
            dataModel.Units = inspectionTypeModel.inspUnits == null ? 0 : inspectionTypeModel.inspUnits.Value;
            dataModel.LastUpdated = inspectionTypeModel.auditDate.Value;

            List<InspectionAction> availableActions = GetAvailableActions(moduleName, recordModel, isCapLockedOrHold, inspectionTypeModel, inspectionModels, allowMultiple);
            dataModel.AvailableOperations = availableActions.ToArray();
            dataModel.MainAction = dataModel.AvailableOperations.Length > 0 ? dataModel.AvailableOperations[0] : InspectionAction.None;
            dataModel.CancelAction = InspectionAction.None;
            dataModel.Actionable = IsTypeActionable(availableActions);
            dataModel.Categories = categories;

            return dataModel;
        }

        /// <summary>
        /// Determines whether [is type actionable] [the specified available actions].
        /// </summary>
        /// <param name="availableActions">The available actions.</param>
        /// <returns>
        /// <c>true</c> if [is type actionable] [the specified available actions]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsTypeActionable(List<InspectionAction> availableActions)
        {
            bool result = false;

            if (availableActions != null)
            {
                foreach (InspectionAction action in availableActions)
                {
                    result = InspectionPermissionUtil.CanBeOperated(action);

                    if (result == true)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the available actions.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="isCapLockedOrHold">if set to <c>true</c> [is cap locked or hold].</param>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <param name="inspectionModels">The inspection models.</param>
        /// <param name="allowMultipleInspection">if set to <c>true</c> [allow multiple inspection].</param>
        /// <returns>the available actions.</returns>
        private List<InspectionAction> GetAvailableActions(string moduleName, CapModel4WS recordModel, bool isCapLockedOrHold, InspectionTypeModel inspectionTypeModel, IEnumerable<InspectionModel> inspectionModels, bool allowMultipleInspection)
        {
            List<InspectionAction> availableActions = new List<InspectionAction>();
            bool allowSchedule = InspectionPermissionUtil.AllowSchedule(AgencyCode, moduleName, User.IsAnonymous, recordModel);
            allowSchedule = allowSchedule && !isCapLockedOrHold && inspectionTypeModel != null;

            if (allowSchedule)
            {
                //rules:
                //1. flow with any status only allows one inspection instance existing.
                //2. flow with any status allows do request/schedule action if no existing any one requested/scheduled inspection.
                bool onlyAllowOneInstance = inspectionTypeModel.isConfiguredInInspFlow || !allowMultipleInspection;
                InspectionAction mainAction4InspecitonType = InspectionActionUtil.GetAction(this.AgencyCode, moduleName, recordModel.statusGroupCode, recordModel.capStatus, inspectionTypeModel);

                bool isActionableByAppStatus = inspectionTypeModel == null ? true : InspectionPermissionUtil.IsActionableByAppStatus(recordModel.statusGroupCode, recordModel.capStatus, inspectionTypeModel.groupCode, inspectionTypeModel.type, mainAction4InspecitonType);

                if (!isActionableByAppStatus)
                {
                    mainAction4InspecitonType = InspectionAction.None;
                }

                bool isActionable4InspecitonType = mainAction4InspecitonType == InspectionAction.Request || mainAction4InspecitonType == InspectionAction.Schedule;

                bool existUncompletedInstance = false;
                bool existApprovedInstance = false;

                if (isActionable4InspecitonType && onlyAllowOneInstance && inspectionModels != null)
                {
                    //check if exist uncompleted inspection
                    existUncompletedInstance =
                        (from i in inspectionModels
                            let isProcessingStatus = InspectionStatusUtil.IsProcessingStatus(InspectionStatusUtil.GetStatus(inspectionTypeModel, i))
                            where isProcessingStatus == true
                            select i).Any();

                    if (!existUncompletedInstance)
                    {
                        existApprovedInstance =
                            (from i in inspectionModels
                                let currentStatus = InspectionStatusUtil.GetStatus(inspectionTypeModel, i)
                                where currentStatus == InspectionStatus.ResultApproved
                                select i).Any();
                    }
                }

                if (existUncompletedInstance || existApprovedInstance)
                {
                    mainAction4InspecitonType = InspectionAction.None;
                }

                if (!availableActions.Contains(mainAction4InspecitonType) && mainAction4InspecitonType != InspectionAction.None)
                {
                    availableActions.Add(mainAction4InspecitonType);
                }
            }

            return availableActions;
        }

        /// <summary>
        /// Filters the inspection types.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="inspectionTypes">The inspection types.</param>
        /// <param name="includeOptionalInspections">if set to <c>true</c> [include optional inspections].</param>
        /// <returns>the filtered inspection types.</returns>
        private InspectionTypeModel[] FilterInspectionTypes(string agencyCode, string moduleName, InspectionTypeModel[] inspectionTypes, bool includeOptionalInspections)
        {
            InspectionTypeModel[] results = inspectionTypes;

            if (inspectionTypes != null)
            {
                bool allowDisplayOfOptionalInspections = InspectionPermissionUtil.AllowDisplayOfOptionalInspections(agencyCode, moduleName);

                var searchResults = from t in inspectionTypes
                                    where ValidationUtil.IsYes(t.requiredInspection)
                                          || (allowDisplayOfOptionalInspections && includeOptionalInspections)
                                    select t;

                results = searchResults.ToArray();
            }

            return results;
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

        #endregion Methods
    }
}
