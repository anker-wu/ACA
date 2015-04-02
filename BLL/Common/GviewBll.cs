#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GviewBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GviewBll.cs 155305 2009-11-24 06:10:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class is provided to get generic view elements in ACA daily 
    /// </summary>
    public class GviewBll : BaseBll, IGviewBll
    {
        /// <summary>
        /// Get generic view elements
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="viewID">string view id.</param>
        /// <returns>generic view element array.</returns>
        public SimpleViewElementModel4WS[] GetSimpleViewElementModel(string moduleName, string viewID)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            return cacheManager.GetGviewElements(moduleName, viewID);
        }

        /// <summary>
        /// Get generic view elements
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="permission">GFilterScreenPermissionModel4WS object</param>
        /// <param name="viewID">string view id.</param>
        /// <param name="callerId">The caller unique identifier.</param>
        /// <returns>generic view element array.</returns>
        public SimpleViewElementModel4WS[] GetSimpleViewElementModel(string moduleName, GFilterScreenPermissionModel4WS permission, string viewID, string callerId)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            return cacheManager.GetGviewElements(moduleName, permission, viewID, callerId);
        }

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
        public string GetSimpleViewElementByJsonFormat(string agencyCode, string moduleName, string viewID, string controPrefix, string permissionLevel, string permissionValue, string callerId)
        {
            string returnValue = string.Empty;
            SimpleViewElementModel4WS[] models = null;
            if (string.IsNullOrEmpty(permissionLevel))
            {
                models = GetSimpleViewElementModel(moduleName, viewID);
            }
            else
            {
                SimpleViewModel4WS simpleViewModel = GetSimpleViewModel(agencyCode, moduleName, viewID, permissionLevel, permissionValue, callerId);
                models = simpleViewModel.simpleViewElements;
            }

            if (models == null || models.Length == 0)
            {
                return string.Empty;
            }

            SimpleViewElementModel4WS[] lstModel = models.OrderBy(m => !ValidationUtil.IsInt(m.displayOrder) ? 0 : Convert.ToInt32(m.displayOrder)).ToArray();

            StringBuilder sb = new StringBuilder("[");

            foreach (SimpleViewElementModel4WS model in lstModel)
            {
                bool isVisible = ACAConstant.VALID_STATUS.Equals(model.recStatus, StringComparison.InvariantCulture);
                bool isStandard = !ValidationUtil.IsNo(model.standard);
                bool isLineControl = ControlType.Line.ToString().Equals(model.elementType, StringComparison.OrdinalIgnoreCase);

                if (!isStandard || !isVisible || isLineControl)
                {
                    continue;
                }
                
                string required = ACAConstant.COMMON_Y.Equals(model.required, StringComparison.InvariantCulture).ToString().ToLowerInvariant();
                string viewElementName = model.viewElementName;

                if (viewElementName == "txtAppState" || viewElementName == "ddlBirthplaceState" || viewElementName == "ddlDriverLicenseState")
                {
                    viewElementName += "_State1";
                }

                sb.Append("{");
                sb.AppendFormat("\"ControlId\":\"{0}{1}\",", controPrefix, ScriptFilter.EncodeJson(viewElementName));
                sb.AppendFormat("\"Required\":\"{0}\"", required);
                sb.Append("},");
            }

            if (sb.Length > 1)
            {
                sb.Length -= 1;
                sb.Append("]");

                returnValue = sb.ToString();
            }

            return returnValue;
        }

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
        public SimpleViewModel4WS GetSimpleViewModel(string agencyCode, string moduleName, string viewID, string permissionLevel, string permissionValue, string callerId)
        {
            GFilterScreenPermissionModel4WS screenPermission = new GFilterScreenPermissionModel4WS();

            if (string.IsNullOrEmpty(permissionLevel))
            {
                screenPermission = null;
            }
            else
            {
                screenPermission.permissionLevel = permissionLevel;
                screenPermission.permissionValue = permissionValue;
                screenPermission.recFulName = string.Empty;
                screenPermission.recStatus = string.Empty;
                screenPermission.servProvCode = agencyCode;
            }

            if (string.IsNullOrEmpty(moduleName) || moduleName == agencyCode)
            {
                moduleName = agencyCode;
            }

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            return cacheManager.GetGview(agencyCode, moduleName, screenPermission, viewID, callerId);
        }

        /// <summary>
        /// Get field visible from simple view element.
        /// </summary>
        /// <param name="models">the simple view element model</param>
        /// <param name="fieldName">the field name</param>
        /// <returns>true or false.</returns>
        public bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
        {
            if (models == null || models.Length == 0)
            {
                return true;
            }

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (model.viewElementName == fieldName)
                {
                    return model.recStatus == ACAConstant.VALID_STATUS;
                }
            }

            return true;
        }

        /// <summary>
        /// Get field visible by view id.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <param name="permission">the generic filter screen permission model</param>
        /// <param name="viewId">the view id.</param>
        /// <param name="fieldName">the field name</param>
        /// <param name="callerID">the caller id.</param>
        /// <returns>true or false</returns>
        public bool IsFieldVisible(string moduleName, GFilterScreenPermissionModel4WS permission, string viewId, string fieldName, string callerID)
        {
            SimpleViewElementModel4WS[] models = GetSimpleViewElementModel(moduleName, permission, viewId, callerID);

            return IsFieldVisible(models, fieldName);
        }
    }
}
