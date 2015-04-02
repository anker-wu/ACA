#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TabUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TabUtil.cs 154424 2009-11-03 07:23:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Tab Utility class.
    /// </summary>
    public static class TabUtil
    {
        /// <summary>
        /// provider license session is null.
        /// </summary>
        public const int NONE_PROVIDER_LICENSE = -1;

        /// <summary>
        /// no existing provider license.
        /// </summary>
        private const int NO_EXIST_PROVIDER_LICENSE = 0;

        /// <summary>
        /// existing provider license.
        /// </summary>
        private const int EXIST_PROVIDER_LICENSE = 1;

        /// <summary>
        /// Get all modules' name and value except "Module Home".
        /// </summary>
        /// <param name="isAdminModeRegistered">Is admin model registered.</param>
        /// <returns>all enable modules.</returns>
        public static Dictionary<string, string> GetAllEnableModules(bool isAdminModeRegistered)
        {
            Dictionary<string, string> moduleKeyValues = new Dictionary<string, string>();

            IList<TabItem> tabList = GetTabList(isAdminModeRegistered);

            foreach (TabItem item in tabList)
            {
                if (item == null || (!item.TabVisible && !item.BlockVisible) || string.IsNullOrEmpty(item.Label) ||
                        "aca_sys_default_home".Equals(item.Label) || "aca_sys_apo_search".Equals(item.Label))
                {
                    continue;
                }

                string name = LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey(item.Label, item.Module));

                if (name == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
                {
                    name = DataUtil.AddBlankToString(item.Module);
                }

                if (!moduleKeyValues.ContainsKey(item.Module))
                {
                    moduleKeyValues.Add(item.Module, name);
                }
            }

            return moduleKeyValues;
        }

        /// <summary>
        /// Get all modules' name and value except "Module Home".
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>The all enabled modules.</returns>
        public static Dictionary<string, string> GetAllEnableModules(string agencyCode)
        {
            Dictionary<string, string> moduleKeyValues = new Dictionary<string, string>();

            IList<TabItem> tabList = GetTabList(agencyCode, false);

            foreach (TabItem item in tabList)
            {
                if (item == null || !item.TabVisible || string.IsNullOrEmpty(item.Label) ||
                        "aca_sys_default_home".Equals(item.Label) || "aca_sys_apo_search".Equals(item.Label))
                {
                    continue;
                }

                string name = LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey(item.Label, item.Module));

                if (name == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
                {
                    name = DataUtil.AddBlankToString(item.Module);
                }

                if (!moduleKeyValues.ContainsKey(item.Module))
                {
                    moduleKeyValues.Add(item.Module, name);
                }
            }

            return moduleKeyValues;
        }

        /// <summary>
        /// Generate cross module list in data table structure.
        /// </summary>
        /// <param name="moduleName">current module name</param>        
        /// <returns>crossed modules</returns>
        public static IList<string> GetCrossModules(string moduleName)
        {
            IList<string> moduleNames = new List<string>();

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            List<string> mainLanuageModuleNames = xPolicyBll.GetCrossModulesFromXPolicy(moduleName);

            IList<TabItem> tabList = GetTabList();

            //Compare modules in xpolicy and ACA_CONFIGS_TABS to get active modules configured in aca admin
            foreach (TabItem item in tabList)
            {
                if (item == null || moduleName.Equals(item.Key) ||
                        !item.TabVisible || string.IsNullOrEmpty(item.Label) ||
                        "aca_sys_default_home".Equals(item.Label) || "aca_sys_apo_search".Equals(item.Label))
                {
                    continue;
                }

                foreach (string mainLanuageValue in mainLanuageModuleNames)
                {
                    if (mainLanuageValue.ToUpper().Equals(item.Module.ToUpper()))
                    {
                        if (!moduleNames.Contains(mainLanuageValue))
                        {
                            moduleNames.Add(mainLanuageValue);
                        }
                    }
                }
            }

            return moduleNames;
        }

        /// <summary>
        /// Gets the tab list including corresponding links.
        /// </summary>
        /// <returns>Tab list with sub links.</returns>
        public static IList<TabItem> GetTabList()
        {
            return GetTabList(false);
        }

        /// <summary>
        /// Get creation Link list.
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="fromAllModule">a value indicating whether get data from all modules.</param>
        /// <returns>return creation link list.</returns>
        public static List<LinkItem> GetCreationLinkItemList(string module, bool fromAllModule)
        {
            List<LinkItem> list = new List<LinkItem>();
            string moduleCreationItemName = module + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_KEY_TAIL;
            string moduleCreationByServiceName = module + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_BY_SERVICE_KEY_TAIL;
            IList<TabItem> tabList = null;

            if (fromAllModule)
            {
                tabList = GetTabListAll();
            }
            else
            {
                tabList = GetTabList();
            }

            foreach (TabItem item in tabList)
            {
                if (item == null || !item.BlockVisible || string.IsNullOrEmpty(item.Label) || item.Children.Count <= 0 ||
                        "aca_sys_default_home".Equals(item.Label) || "aca_sys_apo_search".Equals(item.Label) || !module.Equals(item.Module, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                foreach (LinkItem subLink in item.Children)
                {
                    if (subLink != null && !string.IsNullOrEmpty(subLink.Label)
                        && (moduleCreationItemName.Equals(subLink.Key, StringComparison.OrdinalIgnoreCase)
                            || moduleCreationByServiceName.Equals(subLink.Key, StringComparison.OrdinalIgnoreCase)))
                    {
                        list.Add(subLink);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Get A Tab's Creation Item
        /// </summary>
        /// <param name="module">A module name.</param>
        /// <returns>The Tab's Creation Item.</returns>
        public static TabItem GetTabItemWithModuleName(string module)
        {
            IList<TabItem> tabList = GetTabListAll();
            TabItem tabItem = null;
            
            foreach (TabItem item in tabList)
            {
                if (item != null && item.Module != null && module.ToLower().Equals(item.Module.ToLower()))
                {
                    tabItem = item;
                    break;
                }
            }

            return tabItem;
        }

        /// <summary>
        /// Gets the tab list including corresponding links.
        /// </summary>
        /// <returns>Tab list with sub links.</returns>
        public static IList<TabItem> GetTabListAll()
        {
            bool hasProviderLicense = HasProviderLicense();
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            // get all defined tabs.
            string agencyCode = ConfigManager.AgencyCode;
            ACAUserType userType = ACAUserType.AllUser;
            if (!AppSession.IsAdmin)
            {
                agencyCode = ConfigManager.SuperAgencyCode;
            }

            // get all defined tabs.
            IList<TabItem> tabsList = bizBll.GetTabsList(ConfigManager.AgencyCode, userType, hasProviderLicense);
            return tabsList;
        }

        /// <summary>
        /// Gets the tab list including corresponding links.
        /// </summary>
        /// <param name="isAdminModeRegistered">if current page is welcome register page and opened in aca admin</param>
        /// <returns>Tab list with sub links.</returns>
        public static IList<TabItem> GetTabList(bool isAdminModeRegistered)
        {
            bool isAnonymous = false;
            bool hasProviderLicense = false;
            ACAUserType userType = ACAUserType.Registered;
            if ((AppSession.User == null || AppSession.User.IsAnonymous) && !isAdminModeRegistered)
            {
                isAnonymous = true;
                userType = ACAUserType.Anonymous;
            }

            if (!isAnonymous)
            {
                hasProviderLicense = HasProviderLicense();
            }

            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string agencyCode = ConfigManager.AgencyCode;

            if (!AppSession.IsAdmin)
            {
                agencyCode = ConfigManager.SuperAgencyCode;
            }

            // get all defined tabs.
            IList<TabItem> tabsList = bizBll.GetTabsList(agencyCode, userType, hasProviderLicense);

            return tabsList;
        }

        /// <summary>
        /// Gets Tab List by AgencyCode.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="isAdminModeRegistered">a value indicating whether user is administrator or register.</param>
        /// <returns>return Tab List.</returns>
        public static IList<TabItem> GetTabList(string agencyCode, bool isAdminModeRegistered)
        {
            bool isAnonymous = false;
            bool hasProviderLicense = false;
            ACAUserType userType = ACAUserType.Registered;
            if ((AppSession.User == null || AppSession.User.IsAnonymous) && !isAdminModeRegistered)
            {
                isAnonymous = true;
                userType = ACAUserType.Anonymous;
            }

            if (!isAnonymous)
            {
                hasProviderLicense = HasProviderLicense();
            }

            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            // get all defined tabs.
            IList<TabItem> tabsList = bizBll.GetTabsList(agencyCode, userType, hasProviderLicense);

            return tabsList;
        }

        /// <summary>
        /// Build a full url.
        /// </summary>
        /// <param name="partialUrl">partial url.</param>
        /// <param name="tabName">string tab name.</param>
        /// <param name="filterName">string filter name.</param>
        /// <returns>string full url.</returns>
        public static string RebuildUrl(string partialUrl, string tabName, string filterName)
        {
            if (string.IsNullOrEmpty(partialUrl))
            {
                return string.Empty;
            }

            string url = partialUrl;

            // if there is no parameter in url, append "?", otherwise append "&"
            if (url.IndexOf("?", StringComparison.InvariantCulture) == -1)
            {
                url += "?";
            }
            else
            {
                url += "&";
            }

            url += "TabName=" + tabName;

            if (!string.IsNullOrEmpty(filterName))
            {
                url += "&FilterName=" + HttpUtility.UrlEncode(filterName);
            }

            return url;
        }

        /// <summary>
        /// Judge the permission according the agency defined
        /// </summary>
        /// <param name="linkItem">The link item.</param>
        /// <returns>Return true or false according the permission.</returns>
        public static bool HasPermission(LinkItem linkItem)
        {
            if (AppSession.IsAdmin)
            {
                return true;
            }

            string perfix = linkItem.Module + ACAConstant.SPLIT_CHAR5;

            bool isCreateApplicationItem = linkItem.Key.Equals(perfix + ACAConstant.MODULE_CREATION_KEY_TAIL, StringComparison.InvariantCultureIgnoreCase);
            bool isFeeEstimateItem = linkItem.Key.Equals(perfix + ACAConstant.MODULE_FEE_ESTIMATE_KEY_TAIL, StringComparison.InvariantCultureIgnoreCase);
            bool isScheduleItem = linkItem.Key.Equals(perfix + ACAConstant.MODULE_SCHEDULE_KEY_TAIL, StringComparison.InvariantCultureIgnoreCase);

            if ((isCreateApplicationItem && !FunctionTable.IsEnableCreateApplication())
                || (isFeeEstimateItem && !FunctionTable.IsEnableObtainFeeEstimate())
                || (isScheduleItem && !FunctionTable.IsEnableScheduleInspection()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indicate current user whether has Provider License.
        /// </summary>
        /// <returns>
        /// True indicate current user has provider license; 
        /// False indicate current user has not provider license.
        /// </returns>
        private static bool HasProviderLicense()
        {
            bool hasProviderLicense = false;

            // Provider License session is null.
            if (AppSession.GetHasProviderLicenseFlagFromSession() == NONE_PROVIDER_LICENSE)
            {
                hasProviderLicense = EducationUtil.HasProviderLicense();

                if (hasProviderLicense)
                {
                    AppSession.SetHasProviderLicenseFlagToSession(EXIST_PROVIDER_LICENSE);
                }
                else
                {
                    AppSession.SetHasProviderLicenseFlagToSession(NO_EXIST_PROVIDER_LICENSE);
                }
            }
            else
            {
                if (AppSession.GetHasProviderLicenseFlagFromSession() == NO_EXIST_PROVIDER_LICENSE)
                {
                    hasProviderLicense = false;
                }
                else if (AppSession.GetHasProviderLicenseFlagFromSession() == EXIST_PROVIDER_LICENSE)
                {
                    hasProviderLicense = true;
                }
            }

            return hasProviderLicense;
        }
    }
}
