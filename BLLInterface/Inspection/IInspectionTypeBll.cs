#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IInspectionTypeBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IInspectionTypeBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 * $Id: IInspectionTypeBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
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
    /// Defines method sign for Inspection Type.
    /// </summary>
    public interface IInspectionTypeBll
    {
        #region Methods

        /// <summary>
        /// Gets inspection result by inspection result group name.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="groupName">the inspection result group name</param>
        /// <param name="resultCategory">The result category.</param>
        /// <returns>Array of InspectionResultModel.</returns>
        InspectionResultModel[] GetInspectionResultByGroupName(string servProvCode, string groupName, string resultCategory);

        /// <summary>
        /// Gets the inspection types by cap ID.
        /// </summary>
        /// <param name="capID">The cap ID.</param>
        /// <param name="callerID">The caller ID.</param>
        /// <returns>the inspection types</returns>
        InspectionTypeModel[] GetInspectionTypesByCapID(CapIDModel capID, string callerID);

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
        InspectionTypeModel[] GetInspectionTypesByCapType(string moduleName, CapTypeModel capType, CapIDModel capID, bool includeOptionalInspections, QueryFormat queryFormat, string callerID);

        /// <summary>
        /// Get inspection groups
        /// </summary>
        /// <returns>Inspection group model</returns>
        InspectionGroupModel[] GetInspectionGroups();

        /// <summary>
        /// Get inspection types by inspection group code
        /// </summary>
        /// <param name="insGroupCode">inspection group code</param>
        /// <returns>array for inspection type model</returns>
        InspectionTypeModel[] GetInspectionTypesByGroupCode(string insGroupCode);

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
        /// <returns>
        /// the type of the available inspection types by cap type
        /// </returns>
        List<InspectionTypeDataModel> GetAvailableInspectionTypes(string moduleName, CapModel4WS recordModel, bool isCapLockedOrHold, PublicUserModel4WS currentUser, bool includeOptionalInspections, QueryFormat queryFormat, string callerID);

        /// <summary>
        /// Gets the inspection categories by cap ID.
        /// </summary>
        /// <param name="recordIDModel">The record ID model.</param>
        /// <returns>the inspection categories.</returns>
        List<InspectionCategoryDataModel> GetInspectionCategoriesByCapID(CapIDModel recordIDModel);
        
        #endregion Methods
    }
}