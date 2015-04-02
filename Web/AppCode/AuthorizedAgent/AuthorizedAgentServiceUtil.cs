#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AuthorizedAgentServiceUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;
using System.Linq;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.AuthorizedAgent
{
    /// <summary>
    /// Authorized Agent Service Utility
    /// </summary>
    public static class AuthorizedAgentServiceUtil
    {
        /// <summary>
        /// Determines whether [has authorized service].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has authorized service]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAuthorizedServiceConfig()
        {
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
            BizDomainModel4WS[] bizList = bizDomainBll.GetBizDomainValue(
               ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);

            return bizList != null && bizList.Any(o => BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_FISHING_AND_HUNTING_LICENSE_SALES.Equals(o.bizdomainValue, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the authorized service setting.
        /// </summary>
        /// <returns>Return the authorized service setting.</returns>
        public static AuthorizedServiceSettingModel GetAuthorizedServiceSetting()
        {
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
            AuthorizedServiceSettingModel authorizedServiceSetting = new AuthorizedServiceSettingModel();

            authorizedServiceSetting.ModuleName = bizDomainBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_MODULE);
            authorizedServiceSetting.CapTypeFilterName = bizDomainBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_CAPTYPEFILTER);

            return authorizedServiceSetting;
        }

        /// <summary>
        /// Gets the printer config.
        /// </summary>
        /// <returns>Return the printer config.</returns>
        public static string GetPrinterConfig()
        {
            string result = string.Empty;
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();

            BizDomainModel4WS[] bizList = bizDomainBll.GetBizDomainValue(
                ConfigManager.AgencyCode, BizDomainConstant.STD_AUTHORIZED_SERVICE, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);

            if (bizList != null && bizList.Length > 0)
            {
                BizDomainModel4WS printersItem = bizList.FirstOrDefault(o => o.bizdomainValue.Equals(BizDomainConstant.STD_ITEM_AUTHORIZED_SERVICE_PRINTERS));
                result = printersItem == null ? string.Empty : printersItem.description;
            }

            return ScriptFilter.FilterJSChar(result);
        }
    }
}