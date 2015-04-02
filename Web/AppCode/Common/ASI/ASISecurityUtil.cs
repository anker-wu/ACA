#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: ASISecurityUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 * use it to define some common function for ASI table and ASI
 *  Notes:
 * $Id: ASISecurityUtil.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Text;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.AuthorizedAgent;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The utility class for ASISecurity
    /// </summary>
    public static class ASISecurityUtil
    {
        /// <summary>
        /// Disable control for ASI/ASIT/Generic Template security.
        /// </summary>
        /// <param name="control">The control for ASI/ASIT/Generic Template field.</param>
        /// <param name="fieldType">The field type of enumeration Accela.ACA.Common.FieldType</param>
        /// <param name="isReadonly">Indicates the field whether is readonly.</param>
        public static void DisableFieldForSecurity(WebControl control, FieldType fieldType, bool isReadonly)
        {
            if (!AppSession.IsAdmin && control != null && isReadonly)
            {
                if (control is CheckBox)
                {
                    CheckBox cb = (CheckBox)control;
                    cb.InputAttributes.Add("ValidateDisabledControl", ACAConstant.COMMON_Y);
                    cb.InputAttributes.Add("SecurityType", ACAConstant.ASISecurity.Read);
                }
                else
                {
                    control.Attributes.Add("ValidateDisabledControl", ACAConstant.COMMON_Y);
                    control.Attributes.Add("SecurityType", ACAConstant.ASISecurity.Read);
                }

                (control as IAccelaControl).DisableEdit();

                if (fieldType == FieldType.HTML_TEXTBOX_OF_DATE)
                {
                    ((AccelaCalendarText)control).ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// Gets ASI security for ASI group.
        /// </summary>
        /// <param name="appSpecInfoGroups">appSpecInfoGroups model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>ASI security string</returns>
        public static string GetASISecurity(AppSpecificInfoGroupModel4WS[] appSpecInfoGroups, string moduleName)
        {
            string asiGroupSecurity = ACAConstant.ASISecurity.Full;

            if (appSpecInfoGroups != null && appSpecInfoGroups.Length > 0)
            {
                string agencyCode = appSpecInfoGroups[0].capID != null ? appSpecInfoGroups[0].capID.serviceProviderCode : string.Empty;
                asiGroupSecurity = GetASISecurity(agencyCode, appSpecInfoGroups[0].groupCode, string.Empty, string.Empty, moduleName);
            }

            return asiGroupSecurity;
        }

        /// <summary>
        /// Gets ASI security, isSubGroup = false: gets ASI security for group, isSubGroup = true: gets ASI security for sub group.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="groupCode">ASI group name</param>
        /// <param name="subGroupCode">ASI sub group name</param>
        /// <param name="fieldName">ASI field name</param>
        /// <param name="moduleName">module name</param>
        /// <returns>ASI security string</returns>
        public static string GetASISecurity(string agencyCode, string groupCode, string subGroupCode, string fieldName, string moduleName)
        {
            string asiGroupSecurity = ACAConstant.ASISecurity.Full;

            string asiGroupKey = GetASISecurityKey(groupCode, subGroupCode, fieldName, ACAConstant.ASIType.ASI);

            asiGroupSecurity = GetASISecurity(agencyCode, groupCode, asiGroupKey, moduleName, ACAConstant.ASIType.ASI);

            return asiGroupSecurity;
        }

        /// <summary>
        /// Gets ASI security for ASI field.
        /// </summary>
        /// <param name="appSpecInfo">ASI fields' information</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>ASI security string</returns>
        public static string GetASISecurity(AppSpecificInfoModel4WS appSpecInfo, string moduleName)
        {
            string asiFieldSecurity = ACAConstant.ASISecurity.Full;

            if (appSpecInfo == null)
            {
                return asiFieldSecurity;
            }

            string asiFieldKey = GetASISecurityKey(appSpecInfo.groupCode, appSpecInfo.checkboxType, appSpecInfo.checkboxDesc, ACAConstant.ASIType.ASI);

            asiFieldSecurity = GetASISecurity(appSpecInfo.serviceProviderCode, appSpecInfo.groupCode, asiFieldKey, moduleName, ACAConstant.ASIType.ASI);

            return asiFieldSecurity;
        }

        /// <summary>
        /// Gets ASIT security for ASIT Group
        /// </summary>
        /// <param name="group">AppSpecificTableGroup Model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>ASIT security string</returns>
        public static string GetASITSecurity(AppSpecificTableGroupModel4WS group, string moduleName)
        {
            string asitGroupSecurity = ACAConstant.ASISecurity.Full;

            if (group == null)
            {
                return asitGroupSecurity;
            }

            string asitGroupKey = GetASISecurityKey(group.groupName, string.Empty, string.Empty, ACAConstant.ASIType.ASITable);
            string agencyCode = group.capIDModel != null ? group.capIDModel.serviceProviderCode : string.Empty;
            asitGroupSecurity = GetASISecurity(agencyCode, group.groupName, asitGroupKey, moduleName, ACAConstant.ASIType.ASITable);

            return asitGroupSecurity;
        }

        /// <summary>
        /// Gets ASIT security for Generic Template Table Group
        /// </summary>
        /// <param name="group">TemplateGroup Model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>ASIT security string</returns>
        public static string GetASITSecurity(TemplateGroup group, string moduleName)
        {
            string asitGroupSecurity = ACAConstant.ASISecurity.Full;

            if (group == null)
            {
                return asitGroupSecurity;
            }

            if (group.subgroups == null || group.subgroups.Length == 0 || group.subgroups[0].fields == null || group.subgroups[0].fields.Length == 0)
            {
                asitGroupSecurity = ACAConstant.ASISecurity.None;
            }
            else
            {
                string asitGroupKey = GetASISecurityKey(group.groupName, string.Empty, string.Empty, ACAConstant.ASIType.ASITable);
                string agencyCode = group.subgroups[0].fields[0].serviceProviderCode;
                asitGroupSecurity = GetASISecurity(agencyCode, group.groupName, asitGroupKey, moduleName, ACAConstant.ASIType.ASITable);
            }

            return asitGroupSecurity;
        }

        /// <summary>
        /// Gets ASIT security for ASIT sub group.
        /// </summary>
        /// <param name="subGroup">AppSpecificTable model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>ASIT security string</returns>
        public static string GetASITSecurity(AppSpecificTableModel4WS subGroup, string moduleName)
        {
            string asitSubGroupSecurity = ACAConstant.ASISecurity.Full;

            if (subGroup == null)
            {
                return asitSubGroupSecurity;
            }

            string asitSubGroupKey = GetASISecurityKey(subGroup.groupName, subGroup.tableName, string.Empty, ACAConstant.ASIType.ASITable);
            string agencyCode = subGroup.columns != null && subGroup.columns.Length > 0 ? subGroup.columns[0].servProvCode : string.Empty;
            asitSubGroupSecurity = GetASISecurity(agencyCode, subGroup.groupName, asitSubGroupKey, moduleName, ACAConstant.ASIType.ASITable);

            return asitSubGroupSecurity;
        }

        /// <summary>
        /// Gets ASIT security for generic template sub group.
        /// </summary>
        /// <param name="subGroup">TemplateSubgroup model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>ASIT security string</returns>
        public static string GetASITSecurity(TemplateSubgroup subGroup, string moduleName)
        {
            string asitSubGroupSecurity = ACAConstant.ASISecurity.Full;

            if (subGroup == null)
            {
                return asitSubGroupSecurity;
            }

            if (subGroup.fields == null || subGroup.fields.Length == 0)
            {
                asitSubGroupSecurity = ACAConstant.ASISecurity.None;
            }
            else
            {
                string groupName = subGroup.fields[0].groupName;
                string agencyCode = subGroup.fields[0].serviceProviderCode;
                string asitSubGroupKey = GetASISecurityKey(groupName, subGroup.subgroupName, string.Empty, ACAConstant.ASIType.ASITable);
                asitSubGroupSecurity = GetASISecurity(agencyCode, groupName, asitSubGroupKey, moduleName, ACAConstant.ASIType.ASITable);
            }

            return asitSubGroupSecurity;
        }

        /// <summary>
        /// Gets ASIT security for ASIT column.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="groupName">ASIT group name.</param>
        /// <param name="tableName">ASIT table(sub group) name.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>ASI security string</returns>
        public static string GetASITSecurity(string agencyCode, string groupName, string tableName, string columnName, string moduleName)
        {
            string asitFieldSecurity = ACAConstant.ASISecurity.Full;

            string asitFieldKey = GetASISecurityKey(groupName, tableName, columnName, ACAConstant.ASIType.ASITable);

            asitFieldSecurity = GetASISecurity(agencyCode, groupName, asitFieldKey, moduleName, ACAConstant.ASIType.ASITable);

            return asitFieldSecurity;
        }
       
        /// <summary>
        /// Gets a value indicating whether all column are hide by ASIT security or ACA display flag.
        /// </summary>
        /// <param name="templateSubgroup">Generic template sub group</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>Return true/false to indicate whether all columns are hide by ASIT security or ACA display flag.</returns>
        public static bool IsAllFieldsNoAccess(TemplateSubgroup templateSubgroup, string moduleName)
        {
            bool hasNoAccess = true;

            if (templateSubgroup.fields == null || templateSubgroup.fields.Length == 0)
            {
                return true;
            }

            foreach (GenericTemplateAttribute field in templateSubgroup.fields)
            {
                string asiTSecurity = GetASITSecurity(
                    field.serviceProviderCode, field.groupName, field.subgroupName, field.fieldName, moduleName);

                if (!ACAConstant.ASISecurity.None.Equals(asiTSecurity)
                    && field.acaTemplateConfig != null
                    && !ACAConstant.COMMON_N.Equals(field.acaTemplateConfig.acaDisplayFlag))
                {
                    hasNoAccess = false;
                    break;
                }
            }

            return hasNoAccess;
        }

        /// <summary>
        /// if there is column which is neither none security nor ACA display is no, the role is true.
        /// </summary>
        /// <param name="asitSubGroup">ASIT sub group</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>Return true/false to indicate the fields accessible.</returns>
        public static bool IsAllFieldsNoAccess(AppSpecificTableModel4WS asitSubGroup, string moduleName)
        {
            bool hasNoAccess = true; //if there is column which is neither none security nor ACA display is no, the role is true.

            if (asitSubGroup.columns == null || asitSubGroup.columns.Length < 1)
            {
                return true;
            }

            foreach (AppSpecificTableColumnModel4WS column in asitSubGroup.columns)
            {
                string asiTSecurity = GetASITSecurity(column.servProvCode, column.groupName, column.tableName, column.columnName, moduleName);

                if (!ACAConstant.ASISecurity.None.Equals(asiTSecurity)
                    && !ACAConstant.COMMON_N.Equals(column.vchDispFlag))
                {
                    hasNoAccess = false;
                    break;
                }
            }

            return hasNoAccess;
        }

        /// <summary>
        /// if there is column which is neither none security nor ACA display is no, the role is true.
        /// </summary>
        /// <param name="asiSubGroup">ASI sub group</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>Return true/false to indicate the fields accessible.</returns>
        public static bool IsAllFieldsNoAccess(AppSpecificInfoGroupModel4WS asiSubGroup, string moduleName)
        {
            bool hasNoAccess = true; //if there is column which is neither none security nor ACA display is no, the role is true.

            foreach (AppSpecificInfoModel4WS column in asiSubGroup.fields)
            {
                if (!ACAConstant.ASISecurity.None.Equals(GetASISecurity(column, moduleName))
                    && !ACAConstant.COMMON_N.Equals(column.vchDispFlag))
                {
                    hasNoAccess = false;
                    break;
                }
            }

            return hasNoAccess;
        }

        /// <summary>
        /// Gets ASI security
        /// </summary>
        /// <param name="agencyCode">Normal agency code</param>
        /// <param name="asiGroup">ASI group string</param>
        /// <param name="asiFieldSecurityKey">ASI field security key string</param>
        /// <param name="moduleName">current module name</param>
        /// <param name="asiType">ASI type</param>
        /// <returns>ASI security key string</returns>
        private static string GetASISecurity(string agencyCode, string asiGroup, string asiFieldSecurityKey, string moduleName, ACAConstant.ASIType asiType)
        {
            string asiFieldSecurity = ACAConstant.ASISecurity.Full;

            ASISecurityQueryParam4WS asiSecurityQueryParam = new ASISecurityQueryParam4WS();
            asiSecurityQueryParam.agency = !string.IsNullOrEmpty(agencyCode) ? agencyCode : ConfigManager.AgencyCode;
            PublicUserModel4WS publicUser = new PublicUserModel4WS();

            if (!string.IsNullOrEmpty(moduleName))
            {
                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                publicUser = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));
            }
            else
            {
                publicUser = AppSession.User.UserModel4WS;
            }

            string module = moduleName;

            if (publicUser != null)
            {
                if (CommonUtil.IsAuthorizedAgent(publicUser))
                {
                    AuthorizedServiceSettingModel authServiceSetting = AuthorizedAgentServiceUtil.GetAuthorizedServiceSetting();
                    asiSecurityQueryParam.userType = ACAUserType.AuthorizedAgent.ToString();
                    module = authServiceSetting != null ? authServiceSetting.ModuleName : string.Empty;
                }
                else if (CommonUtil.IsAgentClerk(publicUser))
                {
                    AuthorizedServiceSettingModel authServiceSetting = AuthorizedAgentServiceUtil.GetAuthorizedServiceSetting();
                    asiSecurityQueryParam.userType = ACAUserType.AuthorizedAgentClerk.ToString();
                    module = authServiceSetting != null ? authServiceSetting.ModuleName : string.Empty;
                }
                else if (publicUser.licenseModel != null && publicUser.licenseModel.Length > 0)
                {
                    asiSecurityQueryParam.userType = ACAUserType.LicenseProfessional.ToString();
                }
                else
                {
                    if (ACAConstant.ANONYMOUS_FLAG.Equals(publicUser.userSeqNum))
                    {
                        asiSecurityQueryParam.userType = ACAUserType.Anonymous.ToString();
                    }
                    else
                    {
                        asiSecurityQueryParam.userType = ACAUserType.Registered.ToString();
                    }
                }
            }
            else
            {
                asiSecurityQueryParam.userType = ACAUserType.Anonymous.ToString();
            }

            asiSecurityQueryParam.module = module;
            IServerConstantBll serverConstantBll = ObjectFactory.GetObject<IServerConstantBll>();
            asiSecurityQueryParam.userGroup = serverConstantBll.GetPublicUserGroup(module, asiSecurityQueryParam.agency, string.Empty);

            ASISecurityModel4WS asiSecurity = new ASISecurityModel4WS();
            XPolicyModel xPolicy = new XPolicyModel();
            xPolicy.data5 = asiGroup;
            xPolicy.data1 = asiType.ToString();
            asiSecurity.XPolicy = xPolicy;
            asiSecurityQueryParam.asiSecurityModel = asiSecurity;

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htSecurities = cacheManager.GetASISecuritiesFromCache(asiSecurityQueryParam, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ASISECURITIES));

            if (htSecurities != null && htSecurities.Count > 0)
            {
                if (htSecurities.Contains(asiFieldSecurityKey))
                {
                    asiFieldSecurity = htSecurities[asiFieldSecurityKey].ToString();
                }
            }

            return asiFieldSecurity;
        }

        /// <summary>
        /// Gets ASIT/ASI field security key.
        /// </summary>
        /// <param name="groupName">group name</param>
        /// <param name="subGroupName">sub group name</param>
        /// <param name="columnName">column name</param>
        /// <param name="asiType">ASI type.</param>
        /// <returns>ASIt field security key string</returns>
        private static string GetASISecurityKey(string groupName, string subGroupName, string columnName, ACAConstant.ASIType asiType)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return string.Empty;
            }

            StringBuilder fieldKeyBuilder = new StringBuilder();

            fieldKeyBuilder.Append(asiType.ToString()).Append(ACAConstant.SPLIT_DOUBLE_COLON);
            fieldKeyBuilder.Append(groupName).Append(ACAConstant.SPLIT_DOUBLE_COLON);

            subGroupName = string.IsNullOrEmpty(subGroupName) ? ACAConstant.NULL_STRING : subGroupName;

            fieldKeyBuilder.Append(subGroupName).Append(ACAConstant.SPLIT_DOUBLE_COLON);

            columnName = string.IsNullOrEmpty(columnName) ? ACAConstant.NULL_STRING : columnName;

            fieldKeyBuilder.Append(columnName);

            return fieldKeyBuilder.ToString();
        }
    }
}