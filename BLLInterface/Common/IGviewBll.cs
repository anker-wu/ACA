#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IGviewBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IGviewBll.cs 155305 2009-11-24 06:10:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approach to get generic view element.
    /// </summary>
    public interface IGviewBll
    {
        /// <summary>
        /// Get generic view elements
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="viewID">string view id.</param>
        /// <returns>generic view element array.</returns>
        SimpleViewElementModel4WS[] GetSimpleViewElementModel(string moduleName, string viewID);

        /// <summary>
        /// Get generic view elements
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="permission">GFilterScreenPermissionModel4WS object</param>
        /// <param name="viewID">string view id.</param>
        /// <param name="callerId">The caller unique identifier.</param>
        /// <returns>generic view element array.</returns>
        SimpleViewElementModel4WS[] GetSimpleViewElementModel(string moduleName, GFilterScreenPermissionModel4WS permission, string viewID, string callerId);

        /// <summary>
        /// Get Section Standard Fields with JSON format
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">module name</param>
        /// <param name="viewID">section id</param>
        /// <param name="controPrefix">the prefix of control's client id</param>
        /// <param name="permissionLevel">The permission level.</param>
        /// <param name="permissionValue">The permission value.</param>
        /// <param name="callerId">The caller unique identifier.</param>
        /// <returns>generic view and element to json string</returns>
        string GetSimpleViewElementByJsonFormat(string agencyCode, string moduleName, string viewID, string controPrefix, string permissionLevel, string permissionValue, string callerId);

        /// <summary>
        /// Gets the generic view.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="viewID">The view ID.</param>
        /// <param name="permissionLevel">The permission level.</param>
        /// <param name="permissionValue">The permission value.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>generic view object</returns>
        SimpleViewModel4WS GetSimpleViewModel(string agencyCode, string moduleName, string viewID, string permissionLevel, string permissionValue, string callerId);

        /// <summary>
        /// Get field visible from simple view element.
        /// </summary>
        /// <param name="models">the simple view element model</param>
        /// <param name="fieldName">the field name</param>
        /// <returns>true or false.</returns>
        bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName);

        /// <summary>
        /// Get field visible by view id. 
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="permission">the generic filter screen permission model</param>
        /// <param name="viewId">the view id.</param>
        /// <param name="fieldName">the field name</param>
        /// <param name="callerID">the caller id.</param>
        /// <returns>true or false</returns>
        bool IsFieldVisible(string moduleName, GFilterScreenPermissionModel4WS permission, string viewId, string fieldName, string callerID);
    }
}
