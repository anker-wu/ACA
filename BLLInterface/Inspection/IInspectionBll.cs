#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IInspectionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IInspectionBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// Defines method signs for Inspection BLL.
    /// </summary>
    public interface IInspectionBll
    {
        #region Methods

        /// <summary>
        /// batch schedule inspections
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="inspectionModelArray">inspection model of array</param>
        /// <param name="actMode">mode of action,include "Schedule" or "Reschedule" flag</param>
        /// <param name="inspector">The inspector.</param>
        /// <returns>
        /// array of inspection ids of the new inspections (not the old rescheduled ones).
        /// </returns>
        long[] BatchScheduleInspections(string serviceProviderCode, InspectionModel[] inspectionModelArray, string actMode, SysUserModel inspector);

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
        int CancelInspection(string serviceProviderCode, string callerID, InspectionModel[] inspections, SysUserModel actor);
                
        /// <summary>
        /// get confirm message when cancel inspection
        /// </summary>
        /// <param name="inspections">Array of InspectionModel</param>
        /// <param name="serviceProviderCode">the Service Provider Code (Agency) to which the inspections belong. 
        ///                                   This parameter cannot be null and it must contain a valid Agency code</param>
        /// <param name="callerID">a string representing the id of the invoker. This id is used for auditing purposes 
        ///                        and is usually the user id of the person logged into the system</param>
        /// <returns>confirm message </returns>
        string GetConfirmMessageWhenCancel(InspectionModel[] inspections, string serviceProviderCode, string callerID);

        /// <summary>
        /// Gets the inspection date range can be selected.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="userId">public user sequence number</param>
        /// <param name="module">module name</param>
        /// <param name="inspectionType">inspection type model</param>
        /// <returns>RangeDate Model</returns>
        RangeDateModel GetDatePermissionRangeByInType(string servProvCode, string userId, string module, InspectionTypeModel inspectionType);

        /// <summary>
        /// Get inspections by cap ID.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Array of InspectionModel.</returns>
        InspectionModel[] GetInspectionListByCapID(string moduleName, CapIDModel capID, QueryFormat queryFormat, string callerID);

        /// <summary>
        /// Validate inspection date time and get the available time result.
        /// </summary>
        /// <param name="serviceProviderCode">the serviceProviderCode value.</param>
        /// <param name="inspectionModel">the InspectionModel object.</param>
        /// <returns>get the available result.</returns>
        AvailableTimeResultModel ValidateInspectionDateTime(string serviceProviderCode, InspectionModel inspectionModel);

        /// <summary>
        /// Validate inspection date time and get the available time result.
        /// </summary>
        /// <param name="serviceProviderCode">the serviceProviderCode value.</param>
        /// <param name="inspectionModel">the InspectionModel object.</param>
        /// <returns>get the available result.</returns>
        AvailableTimeResultModel ValidateInspectionDateTimeWhileNotBlockSchedule(string serviceProviderCode, InspectionModel inspectionModel);

        /// <summary>
        /// Gets the inspection list for food facility.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="licenseType">The license type.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>the inspection list for food facility.</returns>
        List<InspectionDataModel> GetInspectionListForFoodFacility(CapModel4WS recordModel, string licenseType, QueryFormat queryFormat);

        /// <summary>
        /// Method to get all inspections
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="queryFormat">query format</param>
        /// <param name="callerID">caller id number.</param>
        /// <returns>Array inspection information </returns>
        InspectionModel[] GetInspections(string moduleName, string serviceProviderCode, QueryFormat queryFormat, string callerID);

        /// <summary>
        /// Get the inspections who have permission to result these.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="status">The status for get new inspection(NEW) or result inspection(RESULT).</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>The inspections that have permission to result inspection.</returns>
        SearchResultModel GetMyInspections(string agencyCode, string status, QueryFormat queryFormat);

        /// <summary>
        /// Get the CSV document stream that the inspections who have permission to result
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>The CSV document stream.</returns>
        byte[] GetMyInspectionsCSV(string agencyCode);

        /// <summary>
        /// Update the inspection according to the CSV file that contain the inspection result information.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="documentModel">The document model.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>Update the inspection success or failed.</returns>
        EMSEResultBaseModel4WS UpdateInspectionByCSV(string agencyCode, DocumentModel documentModel, string filePath);

        /// <summary>
        /// Validate the schedule operation according to inspection flow
        /// </summary>
        /// <param name="inspTypes">Array of inspection type</param>
        /// <param name="capId">The cap id</param>
        /// <returns>InspectionValidation Model</returns>
        InspectionValidationModel Validate4Schedule(string[] inspTypes, CapIDModel capId);

        /// <summary>
        /// This method to validate schedule date according to the inspection flow.
        /// </summary>
        /// <param name="capID">The cap id</param>
        /// <param name="inspection">inspection model</param>
        /// <returns> confirm message key of AA (the key is not used in ACA). In ACA, the message is re-defined. </returns>
        string ValidateScheduleDateByFlow(CapIDModel capID, InspectionModel inspection);

        /// <summary>
        /// Gets the inspection data models.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="isCapLockedOrHold">if set to <c>true</c> [is cap locked or hold].</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns>the inspection data models</returns>
        List<InspectionDataModel> GetDataModels(string moduleName, CapModel4WS recordModel, bool isCapLockedOrHold, PublicUserModel4WS currentUser);

        /// <summary>
        /// Get action mode.
        /// </summary>
        /// <param name="action">the type of InspectionAction</param>
        /// <returns>base on the InspectionAction</returns>
        string GetActionMode(InspectionAction action);

        /// <summary>
        /// get next actual status after action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>next actual status</returns>
        string GetStatusAfterAction(InspectionAction action);

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>the actual status</returns>
        InspectionStatus GetStatus(InspectionModel inspectionModel);

        /// <summary>
        /// Gets the res status string.
        /// </summary>
        /// <param name="inspectionStatus">The inspection status.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>the res status string.</returns>
        string GetResStatusString(InspectionStatus inspectionStatus, InspectionModel inspectionModel);

        /// <summary>
        /// get inspection tree.the parent level is stored in first tree node model's inspection model.
        /// The other level's inspect model store current inspect model.
        /// </summary>
        /// <param name="capID">CapID Model</param>
        /// <param name="inspectionID">inspection ID</param>
        /// <returns>Inspection Tree Node information</returns>
        InspectionTreeNodeModel GetRelatedInspections(CapIDModel capID, string inspectionID);

        /// <summary>
        /// get inspection status history list and result comment list are transformed by InspectionDataModel.
        /// </summary>
        /// <param name="inspectionID">inspection id</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordModel">The record model.</param>
        /// <param name="currentUser">The current user.</param>
        /// <returns>List Inspection Data information</returns>
        IList<InspectionDataModel> GetInspectionHistoryList(long inspectionID, string moduleName, CapModel4WS recordModel, PublicUserModel4WS currentUser);

        /// <summary>
        /// Gets the activity date.
        /// </summary>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <param name="timeOption">The time option.</param>
        /// <returns>the activity date with timeOption output.</returns>
        DateTime? GetActivityDate(InspectionModel inspectionModel, out InspectionTimeOption timeOption);

        #endregion Methods
    }
}