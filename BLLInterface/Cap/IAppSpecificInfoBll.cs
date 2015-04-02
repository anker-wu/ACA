#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAppSpecificInfoBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  ACA Admin left tree.
 *
 *  Notes:
 * $Id: IAppSpecificInfoBll.cs 277225 2014-08-12 10:47:00Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// Defines methods for APP specific info.
    /// </summary>
    public interface IAppSpecificInfoBll
    {
        #region Methods

        /// <summary>
        /// create app spec info table group
        /// </summary>
        /// <param name="appSpecificTableGroup">app specific table group</param>
        /// <param name="callerID">caller ID number</param>
        /// <param name="capID">cap ID number</param>
        void CreateAppSpecificTableGroup(AppSpecificTableGroupModel4WS appSpecificTableGroup, string callerID, CapIDModel4WS capID);

        /// <summary>
        /// get App Spec Info table group by Cape ID.
        /// </summary>
        /// <param name="capID">cap ID number</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>ASIT Group information</returns>
        AppSpecificTableGroupModel4WS GetAppSpecificTableGroupModelByCapID(CapIDModel4WS capID, string callerID);

        /// <summary>
        /// Method to get reference application specific information group with application specific field by CAP Type.
        /// </summary>
        /// <param name="capType4WS">cap type object.</param>
        /// <param name="callerID">caller id number.</param>
        /// <returns>Array of Reference ASI group information</returns>
        Array GetRefAppSpecInfoListWithFiledsByCapType(CapTypeModel capType4WS, string callerID);

        /// <summary>
        ///  Returns relational searchable application specific info for ACA by Group/Type/SubType/Category of cap type.
        ///  Returns all searchable application specific info for ACA if cap type hasn't any value in
        /// </summary>
        /// <param name="capType4WS">capType CapTypeModel Group/Type/SubType/Category can be null, but serviceProviderCode must has value.</param>
        /// <returns>Array of Reference ASI group information</returns>
        RefAppSpecInfoGroupModel4WS[] GetRefSearchableAppSpecInfoFieldList(CapTypeModel capType4WS);

        /// <summary>
        ///  Returns searchable application specific table group for ACA by Group/Type/SubType/Category of cap type.
        ///  Returns all searchable application specific info for ACA if cap type hasn't any value in
        /// </summary>
        /// <param name="capType4WS">capType CapTypeModel Group/Type/SubType/Category can be null, but serviceProviderCode must has value.</param>
        /// <returns>Array of RefAppSpecInfoGroupModel4WS</returns>
        AppSpecificTableGroupModel4WS[] GetSearchableAppSpecTableFieldList(CapTypeModel capType4WS);

        /// <summary>
        /// Get initial drill down information for a ASI group.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="groupName">ASI group name</param>
        /// <returns>drill down information list</returns>
        ASITableDrillDownModel4WS[] GetASIDrillDown(string agencyCode, string groupName);

        /// <summary>
        /// get next drill down info by user selected ASI value.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="groupName">ASI group name</param>
        /// <param name="subGroupName">ASI sub group name</param>
        /// <param name="seriesIds">series ids which have the next ASI fields</param>
        /// <param name="selectedValue">user selected value</param>
        /// <returns>drill down information</returns>
        ASITableDrillDownModel4WS GetNextASIDrillDownData(string agencyCode, string groupName, string subGroupName, string[] seriesIds, string selectedValue);

        /// <summary>
        /// change ASI data source with drill down information.
        /// </summary>
        /// <param name="subASIGroupModel">model for sub ASI</param>
        /// <param name="drillDownModel">drill down model</param>
        /// <returns>return information for sub ASI</returns>
        AppSpecificInfoGroupModel4WS ChangeASIDataSourceByDrillD(AppSpecificInfoGroupModel4WS subASIGroupModel, ASITableDrillDownModel4WS drillDownModel);

        /// <summary>
        /// get the ASI group and sub group by agency code.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>ASI group and sub group</returns>
        GActivitySpecInfoGroupCodeModel[] GetASIGroups(string agencyCode);

        /// <summary>
        /// get the ASIT group and sub group by agency code.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>ASIT group and sub group</returns>
        GActivitySpecInfoGroupCodeModel[] GetASITGroups(string agencyCode);

        #endregion Methods
    }
}