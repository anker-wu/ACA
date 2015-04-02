#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAdmin.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: IAdminBll.cs 276976 2014-08-08 10:10:01Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Admin
{
    /// <summary>
    /// interface of Admin business
    /// </summary>
    public interface IAdminBll
    {
        #region Methods

        /// <summary>
        /// Method to save application status model.
        /// </summary>
        /// <param name="appStatusGroupModel4WS">AppStatusGroupModel4WS array</param>
        void EditAppStatusGroup(AppStatusGroupModel4WS[] appStatusGroupModel4WS);

        /// <summary>
        /// Method to save sub tree node model
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="wsModelList">tree model</param>
        void EditSubTreeNode(string servProvCode, AcaAdminTreeNodeModel4WS[] wsModelList);

        /// <summary>
        ///  Get all ACA page list which is predefined in aca admin tree.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Aca Admin Tree information list</returns>
        AcaAdminTreeNodeModel4WS[] GetACAPageList(string agencyCode);

        /// <summary>
        /// Method to get application status model.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>application status data table</returns>
        DataTable GetAppStatusGroup(string servProvCode, string moduleName);

        /// <summary>
        /// Method to get application status model.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>application status information list</returns>
        AppStatusGroupModel4WS[] GetAppStatusGroupBySPC(string servProvCode, string moduleName);

        /// <summary>
        /// Get page id by url
        /// </summary>
        /// <param name="url">string page url</param>
        /// <returns>page id string</returns>
        string GetPageIDbyUrl(string url);

        /// <summary>
        /// Method to get sub tree node model
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="callerID">public user id</param>
        /// <returns>Data Table</returns>
        DataTable GetSubTreeNode(string agencyCode, string callerID);

        /// <summary>
        /// Method to get sub tree node model
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="callerID">public user id</param>
        /// <returns>Aca Admin Tree information list</returns>
        AcaAdminTreeNodeModel4WS[] GetSubTreeNodeModel(string agencyCode, string callerID);

        /// <summary>
        /// Get Admin Left Tree Nodes
        /// </summary>
        /// <returns>Admin tree node table</returns>
        DataTable GetTreeNodes();

        /// <summary>
        /// Method to get SimpleViewMode 
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="levelType">level type</param>
        /// <param name="moduleName">module name</param>
        /// <param name="viewID">current view id</param>
        /// <param name="screenPermission">GFilterScreenPermissionModel4WS object</param>
        /// <param name="callerid">call user id</param>
        /// <returns>Generic View information</returns>
        SimpleViewModel4WS GetSimpleViewMode(string servProvCode, string levelType, string moduleName, string viewID, GFilterScreenPermissionModel4WS screenPermission, string callerid);

        /// <summary>
        /// Method for save Generic View
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="levelType">level type</param>
        /// <param name="moduleName">module name</param>
        /// <param name="simpleViewMode">SimpleViewModel4WS object</param>
        /// <param name="callerid">call user id</param>
        void SaveSimpleViewModel(string servProvCode, string levelType, string moduleName, SimpleViewModel4WS simpleViewMode, string callerid);

        #endregion Methods
    }
}