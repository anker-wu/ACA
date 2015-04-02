#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminGviewBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminGviewBll.cs 155305 2009-11-24 06:10:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is provided to get generic view elements in ACA admin 
    /// </summary>
    public class AdminGviewBll : BaseBll, IGviewBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of GFilterViewService.
        /// </summary>
        private GFilterViewWebServiceService GFilterViewService
        {
            get
            {
                return WSFactory.Instance.GetWebService<GFilterViewWebServiceService>();
            }
        }

        #endregion Properties

        /// <summary>
        /// Get generic view elements
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="viewID">string view id.</param>
        /// <returns>generic view element array.</returns>
        public SimpleViewElementModel4WS[] GetSimpleViewElementModel(string moduleName, string viewID)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;

            if (string.IsNullOrEmpty(moduleName) || moduleName == AgencyCode)
            {
                moduleName = AgencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            SimpleViewElementModel4WS[] simpleViewElements = GFilterViewService.getSimpleViewElementModel(AgencyCode, levelType, moduleName, viewID);

            return simpleViewElements;
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
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;

            if (string.IsNullOrEmpty(moduleName) || moduleName == AgencyCode)
            {
                moduleName = AgencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            SimpleViewElementModel4WS[] simpleViewElements = null;
            SimpleViewModel4WS simpleView = GFilterViewService.getFilterScreenView(AgencyCode, levelType, moduleName, viewID, permission, callerId);
            if (simpleView != null)
            {
                simpleViewElements = simpleView.simpleViewElements;
            }

            return simpleViewElements;
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

                if (models != null && models.Length > 0)
                {
                    models = models.OrderBy(m => !ValidationUtil.IsInt(m.displayOrder) ? 0 : Convert.ToInt32(m.displayOrder)).ToArray();
                }
            }
            else
            {
                SimpleViewModel4WS simpleViewModel = GetSimpleViewModel(agencyCode, moduleName, viewID, permissionLevel, permissionValue, callerId);
                List<string> transContactTypes = GetContactTypes(ContactTypeSource.Transaction);
                List<string> refContactTypes = GetContactTypes(ContactTypeSource.Reference);
                models = GViewUtil.FilterPeopleTempate(simpleViewModel.simpleViewElements, viewID, transContactTypes, refContactTypes);

                /*
                 * In list view, merge the template fields if some fields have the same label.
                 * List templete fields need merge, Form templete fields don't need merge.
                 */
                if (ACAConstant.TEMPLATE_GENUS_LEVEL_TYPE.Equals(permissionLevel, StringComparison.InvariantCultureIgnoreCase))
                {
                    models = TemplateUtil.MergeTemplateSimpleViewElement(models);
                }
            }

            if (models == null || models.Length == 0)
            {
                return string.Empty;
            }

            int index = 0;
            StringBuilder sb = new StringBuilder("[");

            foreach (SimpleViewElementModel4WS model in models)
            {
                index++;
                bool isStandard = !ValidationUtil.IsNo(model.standard);

                string visible = ACAConstant.VALID_STATUS.Equals(model.recStatus, StringComparison.InvariantCulture).ToString().ToLowerInvariant();
                string required = ACAConstant.COMMON_Y.Equals(model.required, StringComparison.InvariantCulture).ToString().ToLowerInvariant();
                string viewElementName = isStandard ? model.viewElementName : TemplateUtil.GetTemplateControlID(model.viewElementName);
                string editable = (!string.Equals(model.isEditable, ACAConstant.COMMON_N)).ToString().ToLowerInvariant();
                sb.Append("{");

                // standard/template fields both use the [OriginalElementName] as viewElementName
                sb.AppendFormat("\"OriginalElementName\":\"{0}{1}\",", controPrefix, ScriptFilter.EncodeJson(model.viewElementName));
                sb.AppendFormat("\"ElementName\":\"{0}{1}\",", controPrefix, viewElementName);
                sb.AppendFormat("\"Visible\":\"{0}\",", visible);
                sb.AppendFormat("\"Label\":\"{0}\",", ScriptFilter.EncodeJson(model.labelValue));
                sb.AppendFormat("\"Required\":\"{0}\",", required);
                sb.AppendFormat("\"Editable\":\"{0}\",", editable);
                sb.AppendFormat("\"Order\":\"{0}\",", model.displayOrder);

                if (visible.Equals(ACAConstant.COMMON_FALSE))
                {
                    sb.AppendFormat("\"Left\":\"{0}\",", 0);
                    sb.AppendFormat("\"Top\":\"{0}\",", 10000 + index);
                }
                else
                {
                    sb.AppendFormat("\"Left\":\"{0}\",", model.elementLeft);
                    sb.AppendFormat("\"Top\":\"{0}\",", model.elementTop);
                }

                sb.AppendFormat("\"Width\":\"{0}\",", model.elementWidth);
                sb.AppendFormat("\"InputWidth\":\"{0}\",", model.inputWidth);
                sb.AppendFormat("\"LabelWidth\":\"{0}\",", model.labelWidth);
                sb.AppendFormat("\"UnitWidth\":\"{0}\",", model.unitWidth);
                sb.AppendFormat("\"Height\":\"{0}\",", model.elementHeight);
                sb.AppendFormat("\"ViewElementId\":\"{0}\",", model.viewElementId);
                sb.AppendFormat("\"ControlPrefix\":\"{0}\",", controPrefix);
                sb.AppendFormat("\"ElementType\":\"{0}\",", model.elementType);
                sb.AppendFormat("\"Standard\":\"{0}\",", model.standard);
                sb.AppendFormat("\"LabelId\":\"{0}\",", ScriptFilter.EncodeJson(model.labelId));
                sb.AppendFormat("\"OldStatus\":\"{0}\"", model.recStatus);

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
        /// <param name="viewID">The generic view ID.</param>
        /// <param name="permissionLevel">The permission level.</param>
        /// <param name="permissionValue">The permission value.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>generic view object</returns>
        public SimpleViewModel4WS GetSimpleViewModel(string agencyCode, string moduleName, string viewID, string permissionLevel, string permissionValue, string callerId)
        {
            IAdminBll admin = ObjectFactory.GetObject(typeof(IAdminBll)) as IAdminBll;

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

            string levelType = ACAConstant.LEVEL_TYPE_MODULE;

            if (string.IsNullOrEmpty(moduleName) || moduleName == agencyCode)
            {
                moduleName = agencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            SimpleViewModel4WS simpleViewModel = admin.GetSimpleViewMode(agencyCode, levelType, moduleName, viewID, screenPermission, callerId);

            return simpleViewModel;
        }

        /// <summary>
        /// Get field visible from generic view element.
        /// </summary>
        /// <param name="models">the generic view element model</param>
        /// <param name="fieldName">the field name</param>
        /// <returns>true or false.</returns>
        public bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
        {
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
            return true;
        }

        /// <summary>
        /// Get Contact type
        /// </summary>
        /// <param name="contactTypeSource">the contact type source.</param>
        /// <returns>contact type list.</returns>
        private static List<string> GetContactTypes(string contactTypeSource)
        {
            List<string> contactTypes = new List<string>();
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, contactTypeSource);

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue itemValue in stdItems)
                {
                    contactTypes.Add(itemValue.Key);
                }
            }

            return contactTypes;
        }
    }
}
