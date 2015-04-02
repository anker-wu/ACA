#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICapTypeFilterBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Interface define for ICapTypeFilterBll.
 *
 *  Notes:
 * $Id: ICapTypeFilterBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// Defines many methods for Cap type.
    /// </summary>
    public interface ICapTypeFilterBll
    {
        #region Methods

        /// <summary>
        /// Edit filter for button agency level.
        /// </summary>
        /// <param name="xBtnFilter4WS">this model contains the label key of a control and its filter name</param>
        /// <param name="callerId">The caller unique identifier.</param>
        void EditFilter4ButtonAgencyLevel(XButtonFilterModel4WS xBtnFilter4WS, string callerId);

        /// <summary>
        /// Create Cap Type Filter
        /// </summary>
        /// <param name="filterModel">filter model</param>
        /// <param name="callerId">caller Id number</param>
        void CreateCapTypeFilter(CapTypeFilterModel4WS filterModel, string callerId);

        /// <summary>
        /// Create or edit filter 4 button
        /// </summary>
        /// <param name="xBtnFilter4WS">button filter entity</param>
        /// <param name="callerID">The caller unique identifier.</param>
        void CreateOrEditFilter4Button(XButtonFilterModel4WS xBtnFilter4WS, string callerID);

        /// <summary>
        /// Delete cap type filter
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter name</param>
        /// <param name="callerId">caller Id number</param>
        void DeleteCapTypeFilter(string agencyCode, string moduleName, string filterName, string callerId);

        /// <summary>
        /// Edit cap type filter
        /// </summary>
        /// <param name="filterModel">filter Model</param>
        /// <param name="callerId">caller Id number</param>
        void EditCapTypeFilter(CapTypeFilterModel4WS filterModel, string callerId);

        /// <summary>
        /// Get all relation on button2 filter
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>all relations</returns>
        Hashtable GetAllRelationOnButton2Filter(string agencyCode, string callerId);

        /// <summary>
        /// Get cap type filter by label key
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="labelKey">label's key</param>
        /// <returns>cap type filter</returns>
        string GetCapTypeFilterByLabelKey(string agencyCode, string moduleName, string labelKey);
        
        /// <summary>
        /// Get cap type filter list
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>cap type filter list</returns>
        ArrayList GetCapTypeFilterList(string agencyCode, string callerId);

        /// <summary>
        /// Get cap type filter list by module
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>filter list</returns>
        string[] GetCapTypeFilterListByModule(string agencyCode, string moduleName, string callerId);

        /// <summary>
        /// This function gets the detail information of a cap type filter
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter name</param>
        /// <param name="callerId">caller ID number</param>
        /// <returns>Cap Type Filter entity</returns>
        CapTypeFilterModel4WS GetCapTypeFilterModel(string agencyCode, string moduleName, string filterName, string callerId);

        /// <summary>
        /// Get filter 4 button
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="labelKey">label's key</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>Filter 4 button</returns>
        string GetFilter4Button(string agencyCode, string moduleName, string labelKey, string callerId);

        /// <summary>
        /// Get filter 4 button
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="labelKey">label's key</param>
        /// <param name="callerId">caller Id number</param>
        /// <returns>Filter 4 button</returns>
        XButtonFilterModel4WS GetFilter4ButtonModel(string agencyCode, string moduleName, string labelKey, string callerId);

        /// <summary>
        /// Get filter button list
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="filterName">filter Name</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>Array of Button Filter</returns>
        XButtonFilterModel4WS[] GetFilter4ButtonListByFilterName(string servProvCode, string moduleName, string filterName, string callerID);

        #endregion Methods
    }
}