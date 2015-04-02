#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: StandardChoiceUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  LabelUtil for getting label by label key.
 *  UI should call this class if need to get text in .cs.
 *
 *  Notes:
 *      $Id: StandardChoiceUtil.cs 279328 2014-10-20 01:52:54Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Standard Choice Utility
    /// </summary>
    public static class StandardChoiceUtil
    {
        #region Methods

        /// <summary>
        /// check if multiple trade name supported
        /// </summary>
        /// <returns>return true if multi trade name supported. Otherwise returns false.</returns>
        public static bool EnaleMultiTradeName()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string forbiden = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.MULTIPLE_TRADE_NAME_FORBIDDEN, ACAConstant.COMMON_N);

            // if it is configurated with 'Yes' or 'Y',forbide mutiple trade name,the defalut vale is 'N'
            if (forbiden.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase) ||
                forbiden.Equals(ACAConstant.COMMON_Yes, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// get the CAP 's status after apply a new permit.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>CAP status</returns>
        public static string GetCapStatusAfterApply(string moduleName)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string str = BizDomainConstant.PA_CAP_STATUS_AFTER_APPLY + "_" + moduleName.ToUpperInvariant();
            string[] capStatusList = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, str);
            string capStatus = string.Empty;

            if (capStatusList != null &&
                capStatusList.Length > 0)
            {
                capStatus = capStatusList[0];
            }

            return capStatus;
        }

        /// <summary>
        /// Get contact type by key for support multi-language.
        /// </summary>
        /// <param name="contactTypeKey">contact type key</param>
        /// <returns>contact type</returns>
        public static string GetContactTypeByKey(string contactTypeKey)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> contactTypes = bizBll.GetContactTypeList(ConfigManager.AgencyCode, true, ContactTypeSource.All);
            ItemValue contactType = contactTypes == null ? null : contactTypes.FirstOrDefault(o => o.Key.Equals(contactTypeKey));

            return contactType == null ? string.Empty : contactType.Value.ToString();
        }

        /// <summary>
        /// Check if ACA Document Permission is enabled for all ACA user
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="moduleName">Module Name</param>
        /// <param name="documentRight">"Download", "Delete", "View"</param>
        /// <returns>True - Ignore ACA Document Permission Rule</returns>
        public static bool IsEnabledAllUserDocumentPermission(string servProvCode, string moduleName, string documentRight)
        {
            bool isEnabledDocumentPermission = false;

            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> items = bizBll.GetBizDomainList(servProvCode, BizDomainConstant.STD_ALL_USER_DOCUMENT_PERMISSION, false);

            if (items != null || items.Count > 0)
            {
                foreach (ItemValue item in items)
                {
                    if (documentRight.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (item.Value != null && ValidationUtil.IsYes(item.Value.ToString()))
                        {
                            isEnabledDocumentPermission = true;
                        }

                        break;
                    }
                }
            }

            return isEnabledDocumentPermission;
        }

        /// <summary>
        /// Check the Admin Features setting for "Upload Inspection Result" whether is Enabled.
        /// </summary>
        /// <returns>Is Enabled return true else return false</returns>
        public static bool IsEnabledUploadInspectionResult()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            IList<ItemValue> lstLinks = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS, false);

            var query = from link in lstLinks
                        where link.Key.Equals(BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS_UPLOAD_INSPECTION, StringComparison.OrdinalIgnoreCase)
                        select link;

            ItemValue lnk = query.SingleOrDefault<ItemValue>();
            return lnk != null;
        }

        /// <summary>
        /// Get Fee Quantity Accuracy
        /// </summary>
        /// <returns>The fee quantity accuracy.</returns>
        public static int GetFeeQuantityAccuracy()
        {
            int feeQuantityAccuracy = 2;
            bool isEnanbleDecimalQuality = true;

            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string value = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS, BizDomainConstant.STD_ITEM_SUPPORT_DECIMAL_QUANTITY);
            if (!string.IsNullOrEmpty(value))
            {
                isEnanbleDecimalQuality = ValidationUtil.IsYes(value) ? true : false;
            }

            if (isEnanbleDecimalQuality)
            {
                string[] bizdomainValueList = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_FEE_QUANTITY_ACCURACY);

                if (bizdomainValueList != null && bizdomainValueList.Length > 0)
                {
                    int tmpValue = 2;
                    foreach (string bizdomainValue in bizdomainValueList)
                    {
                        if (int.TryParse(bizdomainValue, out tmpValue))
                        {
                            feeQuantityAccuracy = tmpValue;
                            break;
                        }
                    }
                }
            }
            else
            {
                feeQuantityAccuracy = 0;
            }

            return feeQuantityAccuracy;
        }

        /// <summary>
        /// Get country by key for support multi-language.
        /// </summary>
        /// <param name="countryKey">country Key</param>
        /// <returns>country text for display.</returns>
        public static string GetCountryByKey(string countryKey)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string country = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_COUNTRY, countryKey);

            return country;
        }

        /// <summary>
        /// Get default country from STD
        /// </summary>
        /// <returns>default country</returns>
        public static string GetDefaultCountry()
        {
            IBizdomainProvider bizProvider = ObjectFactory.GetObject(typeof(IBizdomainProvider)) as IBizdomainProvider;
            IList<ItemValue> countryDefalut = bizProvider.GetBizDomainList(BizDomainConstant.STD_COUNTRY_DEFAULT_VALUE);
            return countryDefalut != null && countryDefalut.Count > 0
                                        ? countryDefalut[0].Key
                                        : string.Empty;
        }

        /// <summary>
        /// Get default job value in stand choice item
        /// </summary>
        /// <returns>return default job value</returns>
        public static string GetDefaultJobValue()
        {
            string returnValue = string.Empty;
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            ArrayList jobList = new ArrayList();
            string[] jobValues = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_DEFAULT_JOB_VALUE);

            if (jobValues != null &&
                jobValues.Length > 0)
            {
                foreach (string jobValue in jobValues)
                {
                    jobList.Add(jobValue);
                }

                jobList.Sort();
                returnValue = jobList[0].ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// Get default regional modifier value in stand choice item
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>return regional modifier value</returns>
        public static string GetRegionalModifierValue(string agencyCode)
        {
            string returnValue = string.Empty;
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            ArrayList regModifierList = new ArrayList();
            string[] regModifiers = bizBll.GetBizDomainValueList(agencyCode, BizDomainConstant.STD_REGIONAL_MODIFIER);

            if (regModifiers != null &&
                regModifiers.Length > 0)
            {
                foreach (string regModifier in regModifiers)
                {
                    regModifierList.Add(regModifier);
                }

                regModifierList.Sort();
                returnValue = regModifierList[0].ToString();
            }

            if (!ValidationUtil.IsNumber(returnValue))
            {
                returnValue = "1";
            }

            return returnValue;
        }

        /// <summary>
        /// Get EPayment adapter type
        /// </summary>
        /// <returns>EPayment adapter type</returns>
        public static string GetEPaymentAdapterType()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            return bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_EPAYMENT_CONFIG, "ACAAdapterType");
        }

        /// <summary>
        /// Indicates if the account manage links should be enabled or not.
        /// </summary>
        /// <returns>return true if the link is enabled or didn't be configured. Otherwise returns false.</returns>
        public static bool IsAccountManagementEnabled()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string accountManager = bizBll.GetValueForACAConfig(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_ITEM_ACCOUNT_MANAGEMENT_ENABLED);

            return !ValidationUtil.IsNo(accountManager);
        }

        /// <summary>
        /// Indicates the expression alert whether disabled
        /// </summary>
        /// <returns>return true if the value is YES or Y</returns>
        public static bool IsDisableExpessionAlert()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string[] values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_DISABLE_EXPRESSION_ALERT);

            if (values != null && values.Length > 0)
            {
                foreach (string val in values)
                {
                    if (ValidationUtil.IsYes(val))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Indicates whether display select supervisor in Spear form page.
        /// </summary>
        /// <returns>If Yes select supervisor button will display in ACA daily side, No won't display</returns>
        public static bool IsDisplaySupervisor()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isDisplaySupervisor = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_TEMPLATE_EMSE_DROPDOWN, BizDomainConstant.STD_ITEM_TEMPLATE_EMSE_ENABLE);

            // if the value is 'Yes' or 'Y', display the user initial, otherwise display the user name.
            return ValidationUtil.IsYes(isDisplaySupervisor);
        }

        /// <summary>
        /// Indicates whether the option Display Request Trade License shows in App Status Filter section or not
        /// </summary>
        /// <returns>Whether the option Display Request Trade License shows in App Status Filter section or not</returns>
        public static bool IsEnableRequestTradeLicenseFilter()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isDisplayRequestTradeLicenseFilter = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_DISPLAY_REQUEST_TRADE_LICENSE_FILTER);

            return ValidationUtil.IsYes(isDisplayRequestTradeLicenseFilter);
        }

        /// <summary>
        /// Get gender by key(F/M) for support multi-language.
        /// </summary>
        /// <param name="genderKey">Gender Key</param>
        /// <returns>Gender text for display.</returns>
        public static string GetGenderByKey(string genderKey)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string gender = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_GENDER, genderKey);

            return gender;
        }

        /// <summary>
        /// Get URL of GIS mini viewer
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Url of GIS</returns>
        public static string GetGisAddressLocatorURL(string agencyCode)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string url = bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_GIS_PORLET_URL).Trim();

            return url;
        }

        /// <summary>
        /// Get ASI Label Width(PX)
        /// </summary>
        /// <returns>Label Width(PX)</returns>
        public static string GetASILabelWidth()
        {
            int labelWidth = 0;

            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string charWidth = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_ASI_LABEL_WIDTH);

            if (!string.IsNullOrEmpty(charWidth))
            {
                charWidth = charWidth.Trim();
            }

            if (ValidationUtil.IsInt(charWidth) && Convert.ToInt32(charWidth) > 0)
            {
                // for current css font the control width should be char width * 7.
                labelWidth = Convert.ToInt32(charWidth) * 7;
            }
            else
            {
                labelWidth = ACAConstant.ASI_LABEL_WIDTH_DEFAULT * 5;
            }

            return labelWidth.ToString() + "px";
        }

        /// <summary>
        /// get require license identifier  from registration setting.
        /// </summary>
        /// <returns>return true if require license to register. Otherwise returns false.</returns>
        public static string GetOfficialWebSite()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string officialSite = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_OFFICIAL_WEBSITE_URL);

            return officialSite;
        }

        /// <summary>
        /// Get standard choice's Description by key for support multi-language.
        /// </summary>
        /// <param name="bizDomainName">biz domain name</param>
        /// <param name="bizDomainValue">the biz domain value</param>
        /// <returns>the item value</returns>
        public static string GetStandardChoiceValueByKey(string bizDomainName, string bizDomainValue)
        {
            string bizDomainDes = string.Empty;

            if (string.IsNullOrEmpty(bizDomainValue) ||
                string.IsNullOrEmpty(bizDomainName))
            {
                return bizDomainDes;
            }

            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            bizDomainDes = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, bizDomainName, bizDomainValue);

            return bizDomainDes;
        }

        /// <summary>
        /// get task disposition comment
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>task disposition comment</returns>
        public static string GetTaskDispositionComment(string moduleName)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string comment_BizName = ACAConstant.PA_WORKFLOW_TASK_APPLY_COMMENT + "_" + moduleName.ToUpperInvariant();
            string comment = string.Empty;
            string[] commentList = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, comment_BizName);

            if (commentList != null &&
                commentList.Length > 0)
            {
                comment = commentList[0];
            }

            return comment;
        }

        /// <summary>
        /// get work flow tasks for ACAModel
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>work flow task list</returns>
        public static BizDomainModel4WS[] GetWorkflowTasks(string moduleName)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            string sb = ACAConstant.PA_WORKFLOW_TASK_AFTER_APPLY + "_" + moduleName.ToUpperInvariant();

            string[] paWorkflowTaskList = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, sb);

            if (paWorkflowTaskList != null &&
                paWorkflowTaskList.Length > 0)
            {
                BizDomainModel4WS[] bizDomainModels = new BizDomainModel4WS[paWorkflowTaskList.Length];
                int i = 0;
                foreach (string s in paWorkflowTaskList)
                {
                    BizDomainModel4WS model = new BizDomainModel4WS();
                    model.bizdomainValue = s;
                    bizDomainModels[i] = model;
                    i++;
                }

                return bizDomainModels;
            }

            return null;
        }

        /// <summary>
        /// Indicates whether the country code textbox enabled. the default value should be disable.
        /// </summary>
        /// <returns>true-country code textbox enabled, false - disabled</returns>
        public static bool IsCountryCodeEnabled()
        {
            //return false;
            IBizdomainProvider bizProvider = ObjectFactory.GetObject(typeof(IBizdomainProvider)) as IBizdomainProvider;
            IList<ItemValue> items = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_PHONE_NUMBER_IDD_ENABLE);

            if (items != null && items.Count > 0 &&
                ValidationUtil.IsYes(items[0].Key))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// indicates if currency symbol shows behind ASI field's label
        /// </summary>
        /// <returns>true or null --- display, false -- not display</returns>
        public static bool IsDisplayCurrencySymbol()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string enabled = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_DISPLAY_CURRENCY_SYMBOL_FOR_ASI);

            return !ValidationUtil.IsNo(enabled);
        }

        /// <summary>
        /// Indicates whether the users' initials display in ACA. The default value is false.
        /// </summary>
        /// <returns>true means to show User Initial, else not</returns>
        public static bool IsDisplayUserInitial()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isDisplayUserInitial = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_DISPLAY_USER_INITIALS);

            // if the value is 'Yes' or 'Y', display the user initial, otherwise display the user name.
            return ValidationUtil.IsYes(isDisplayUserInitial);
        }

        /// <summary>
        /// Indicates whether the export to CSV function enabled. the default value should be disable.
        /// </summary>
        /// <returns>true-export to CSV function enabled, false - disabled.</returns>
        public static bool IsEnableExport2CSV()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isEnableExport = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_ALLOW_EXPORTING_TO_CSV, ACAConstant.COMMON_N);

            return ValidationUtil.IsYes(isEnableExport);
        }

        /// <summary>
        /// Judge whether the anonymous user is allowed to print permit or receipt.
        /// </summary>
        /// <returns>
        ///     return true if the current user is not anonymous user
        ///     return true if the current user is anonymous user and is configured ENABLE_ANONYMOUS_REPORT in ACA_CONFIGS as "Y"
        /// </returns>
        public static bool IsEnableReportForAnonymousUser()
        {
            if (!AppSession.User.IsAnonymous)
            {
                return true;
            }

            bool isEnableAnonymousReport = false;
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string enableAnonymousReport = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ENABLE_ANONYMOUS_REPORT);
            if (enableAnonymousReport != null &&
                (ACAConstant.COMMON_Y.Equals(enableAnonymousReport, StringComparison.InvariantCultureIgnoreCase) || ACAConstant.COMMON_YES.Equals(enableAnonymousReport, StringComparison.InvariantCultureIgnoreCase)))
            {
                isEnableAnonymousReport = true;
            }

            return isEnableAnonymousReport;
        }

        /// <summary>
        /// Indicates whether enable shopping cart.
        /// </summary>
        /// <returns>true means Shopping Cart is enabled, else not</returns>
        public static bool IsEnableShoppingCart()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableShoppingCart = policyBll.GetSuperAgencyValueByKey(XPolicyConstant.ACA_ENABLE_SHOPPING_CART);

            // if the value is 'Yes' or 'Y', the shopping cart function is on , otherwise is off.
            return ValidationUtil.IsYes(isEnableShoppingCart);
        }

        /// <summary>
        /// Indicating whether enable document type filter.
        /// </summary>
        /// <returns>Whether enable document type filter or not.</returns>
        public static bool IsEnableDocumentType()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableDocType = policyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_DOCUMENT_TYPE_FILTER);
            return ValidationUtil.IsYes(isEnableDocType);
        }

        /// <summary>
        /// Indicating whether auto active new associated contact.
        /// </summary>
        /// <returns> Whether auto activate new associated contact.</returns>
        public static bool IsAutoActivateNewAssociatedContact()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isAutoActivateNewAssociatedContact = policyBll.GetValueByKey(XPolicyConstant.AUTO_ACTIVATE_NEW_ASSOCIATED_CONTACT);
            return ValidationUtil.IsYes(isAutoActivateNewAssociatedContact);
        }

        /// <summary>
        /// Indicating whether Enable Manual Contact Association.
        /// </summary>
        /// <returns>Whether Enable Manual Contact Association.</returns>
        public static bool IsEnabelManualContactAssociation()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnabelManualContactAssociation = policyBll.GetValueByKey(XPolicyConstant.ENABEL_MANUAL_CONTACT_ASSOCIATION);

            return ValidationUtil.IsYes(isEnabelManualContactAssociation);
        }

        /// <summary>
        /// Indicating whether enable Manual contact address maintenance.
        /// </summary>
        /// <returns>Whether enable Manual contact address maintenance.</returns>
        public static bool IsEnableContactAddressMaintenance()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string enableContactAddressMaintenance = policyBll.GetValueByKey(XPolicyConstant.ENABLE_CONTACT_ADDRESS_MAINTENANCE);
            return !ValidationUtil.IsNo(enableContactAddressMaintenance);
        }

        /// <summary>
        /// Indicates whether enable to display license state.
        /// </summary>
        /// <returns>true means License State is enabled, else not</returns>
        public static bool IsDisplayLicenseState()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableLicenseState = policyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_LICENSESTATE);

            // if the value is 'Y', the license state function is on , otherwise is off.
            return ValidationUtil.IsYes(isEnableLicenseState);
        }

        /// <summary>
        /// Indicates whether enable to use announcement.
        /// </summary>
        /// <returns>true means announcement function is enabled, else not</returns>
        public static bool IsUseAnnouncement()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableAnnouncement = policyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_ANNOUNCEMENT);

            // if the value is 'Y', the license state function is on , otherwise is off.
            return ValidationUtil.IsYes(isEnableAnnouncement);
        }

        /// <summary>
        /// Get the announcement interval from server
        /// </summary>
        /// <returns>interval time</returns>
        public static string GetAnnouncementInterval()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string announcementInterval = policyBll.GetSuperAgencyValueByKey(XPolicyConstant.ANNOUNCEMENT_INTERVAL);
            return announcementInterval;
        }

        /// <summary>
        /// Indicates whether enable proxy user.
        /// </summary>
        /// <returns>true means proxy user is enabled, else not</returns>
        public static bool IsEnableProxyUser()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableProxyUser = policyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_PROXYUSER);

            // if the value is 'Yes' or 'Y',the proxy user function is on, otherwise is off.
            return ValidationUtil.IsYes(isEnableProxyUser);
        }

        /// <summary>
        /// Indicates whether enable announcement.
        /// </summary>
        /// <returns>true means announcement is enabled, else not</returns>
        public static bool IsEnableAnnouncement()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableAnnouncement = policyBll.GetSuperAgencyValueByKey(XPolicyConstant.ACA_ENABLE_ANNOUNCEMENT);

            // if the value is 'Yes' or 'Y',the annnouncement function is on, otherwise is off.
            return ValidationUtil.IsYes(isEnableAnnouncement);
        }

        /// <summary>
        /// Indicates whether the LDAP authentication is enabled.
        /// </summary>
        /// <returns>true means the LDAP authentication is enabled, else not</returns>
        public static bool IsEnableLdapAuthentication()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string enableLdap = bizBll.GetValueForACAConfig(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_ITEM_ENABLE_LDAP_AUTHENTICATION);
            return ValidationUtil.IsYes(enableLdap);
        }

        /// <summary>
        /// Indicates whether enable Parcel Genealogy.
        /// </summary>
        /// <returns>true means Parcel Genealogy is enabled, else not</returns>
        public static bool IsEnableParcelGenealogy()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableParcelGenealogy = policyBll.GetValueByKey(ACAConstant.ACA_ENABLE_PARCEL_GENEALOGY);

            return ValidationUtil.IsYes(isEnableParcelGenealogy);
        }

        /// <summary>
        /// Indicates whether enable for data 4 as key.
        /// </summary>
        /// <param name="policyName">policy Name</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="data4">data 4.</param>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableForData4AsKey(string policyName, string moduleName, string data4)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;
            string levelData = moduleName;

            if (string.IsNullOrEmpty(moduleName))
            {
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
                levelData = ConfigManager.AgencyCode;
            }

            bool isEnableForData4AsKey = false;
            IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string policyValue = xpolicyBll.GetPolicyValueForData4AsKey(policyName, levelType, levelData, data4);

            if (ValidationUtil.IsYes(policyValue))
            {
                isEnableForData4AsKey = true;
            }

            return isEnableForData4AsKey;
        }

        /// <summary>
        /// Indicates whether need to limit the inspection comment length when cancel or schedule a inspection.
        /// </summary>
        /// <returns>true-don't need to limit comment length, false - Limited comment length.</returns>
        public static bool IsInspectionCommentLengthUnlimited()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isUnLimit = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_INSPECTION_CONFIGS, BizDomainConstant.STD_ITEM_ENABLE_UNLIMIT_COMMENT_LENGTH);

            return ValidationUtil.IsYes(isUnLimit);
        }

        /// <summary>
        /// Indicates if the login links should be enabled or not.
        /// </summary>
        /// <returns>return true if the link is enabled or didn't be configured. Otherwise returns false.</returns>
        public static bool IsLoginEnabled()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string enabled = bizBll.GetValueForACAConfig(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_ITEM_LOGON_ENABLED, ACAConstant.COMMON_YES);

            // if it is configurated with 'No' or 'N', disable the login link, the default value is enabled
            //if (enabled.ToUpper() == ACAConstant.COMMON_NO || enabled.ToUpper() == ACAConstant.COMMON_N)
            if (enabled.Equals(ACAConstant.COMMON_NO, StringComparison.InvariantCultureIgnoreCase) ||
                enabled.Equals(ACAConstant.COMMON_N, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Indicates if the logo is visible.
        /// </summary>
        /// <returns>return true if the logo is visible. Otherwise returns false.</returns>
        public static bool IsLogoVisible()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string enabled = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_ACA_LOG_VISIBLE, ACAConstant.COMMON_YES);

            // if it is configurated with 'No' or 'N', disable the login link, the default value is enabled
            //if (enabled.ToUpper() == ACAConstant.COMMON_NO || enabled.ToUpper() == ACAConstant.COMMON_N)
            if (enabled.Equals(ACAConstant.COMMON_NO, StringComparison.InvariantCultureIgnoreCase) ||
                enabled.Equals(ACAConstant.COMMON_N, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Indicates if the registration links should be enabled or not.
        /// </summary>
        /// <returns>return true if the link is enabled or didn't be configured. Otherwise returns false.</returns>
        public static bool IsRegistrationEnabled()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string enabled = bizBll.GetValueForACAConfig(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_ITEM_REGISTRATION_ENABLED, ACAConstant.COMMON_YES);

            // if it is configurated with 'No' or 'N', disable the registration link, the default value is enabled
            //if (enabled.ToUpper() == ACAConstant.COMMON_NO || enabled.ToUpper() == ACAConstant.COMMON_N)
            if (enabled.Equals(ACAConstant.COMMON_NO, StringComparison.InvariantCultureIgnoreCase) ||
                enabled.Equals(ACAConstant.COMMON_N, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// If true, ACA can not support online payment
        /// and we should remove PAY FEE DUE link from my permit list
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>true or false</returns>
        public static bool IsRemovePayFee(string agencyCode)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isRemovePayFee = bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_REMOVE_PAY_FEE, ACAConstant.COMMON_No);

            return ValidationUtil.IsYes(isRemovePayFee);
        }

        /// <summary>
        /// get require license identifier  from registration setting.
        /// </summary>
        /// <returns>return true if require license to register. Otherwise returns false.</returns>
        public static bool IsRequiredLicense()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isRequest = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_REGISTRATION_LICENSE_ENABLED, ACAConstant.COMMON_No);

            return ValidationUtil.IsYes(isRequest);
        }

        /// <summary>
        /// Indicates if the Add LP links should be enabled or not.
        /// </summary>
        /// <returns>return true if the Add link is enabled. Otherwise returns false.</returns>
        public static bool DisabledAddLicense()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string optionValue = policyBll.GetValueByKey(ACAConstant.ACA_DISABLE_REGISTRATION_ADD_LICENSE);

            //intial option is empty. so that it should go old logic.
            return ValidationUtil.IsYes(optionValue);
        }

        /// <summary>
        /// Indicates if the Remove LP links should be enabled or not.
        /// </summary>
        /// <returns>return true if the Remove link is enabled. Otherwise returns false.</returns>
        public static bool DisabledRemoveLicense()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string optionValue = policyBll.GetValueByKey(XPolicyConstant.ACA_DISABLE_REGISTRATION_REMOVE_LICENSE);

            //intial option is empty. so that it should go old logic.
            return ValidationUtil.IsYes(optionValue);
        }

        /// <summary>
        /// Indicates whether need to turn on the "Show on Map" function.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <returns>true-turn on to show Map, false-turn off.</returns>
        public static bool IsShowMap4ShowObject(string moduleName)
        {
            return IsShowMap(moduleName, BizDomainConstant.STD_DISPLAY_MAP_FOR_SHOWOBJECT);
        }
            
        /// <summary>
        /// Indicates whether need to turn on the "Use map to select work location" function.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <returns>true-turn on to show Map, false-turn off.</returns>
        public static bool IsShowMap4SelectObject(string moduleName)
        {
            return IsShowMap(moduleName, BizDomainConstant.STD_DISPLAY_MAP_FOR_SELECTOBJECT);
        }

        /// <summary>
        /// Gets current GIS configured is active or not.
        /// </summary>
        /// <returns>true or false.</returns>
        public static bool HasGISSettings()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string activeGIS = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_GIS_PORLET_URL);

            return !string.IsNullOrEmpty(activeGIS);
        }

        /// <summary>
        /// if enable contact type filtering by module.
        /// </summary>
        /// <returns>true or false.</returns>
        public static bool IsEnableContactTypeFilteringByModule()
        {
            IBizdomainProvider bizProvider = ObjectFactory.GetObject(typeof(IBizdomainProvider)) as IBizdomainProvider;
            IList<ItemValue> items = bizProvider.GetBizDomainList(BizDomainConstant.STD_ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE);

            if (items != null && items.Count > 0)
            {
                foreach (ItemValue itm in items)
                {
                    if (ValidationUtil.IsNo(itm.Key))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether follow the super agency's logic based in the Standard Choice and the Url parameters.
        /// </summary>
        /// <param name="checkSuperAgencyOnAdmin">
        /// Indicating whether need the check the Super Agency environment in ACA Admin, default is false.
        /// </param>
        /// <returns>true - Super Agency, false - Normal Agency.</returns>
        public static bool IsSuperAgency(bool checkSuperAgencyOnAdmin = false)
        {
            /*
             * In Super agency environment, if the url paramter has "isSubAgencyCap" and value is yes, follow the normal/sub agency's logic.
             * For example: In super agency site select One service which has page flow to create CAP, the logic is similar to the normal agency.
             */
            return IsSuperAgencyIgnoreSubAgency(checkSuperAgencyOnAdmin)
                && !ValidationUtil.IsYes(HttpContext.Current.Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]);
        }

        /// <summary>
        /// Gets a value indicating whether follow the super agency's logic based in the Standard Choice.
        /// </summary>
        /// <param name="checkSuperAgencyOnAdmin">
        /// Indicating whether need the check the Super Agency environment in ACA Admin, default is false.
        /// </param>
        /// <returns>true - Super Agency, false - Normal Agency.</returns>
        public static bool IsSuperAgencyIgnoreSubAgency(bool checkSuperAgencyOnAdmin = false)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string isSuperAgency = bizBll.GetValueForStandardChoice(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_CAT_MULTI_SERVICE_SETTINGS, BizDomainConstant.STD_ITEM_IS_SUPER_AGENCY);

            // in Admin,Don't need to check the super agency.
            if (AppSession.IsAdmin)
            {
                if (checkSuperAgencyOnAdmin)
                {
                    return ValidationUtil.IsYes(isSuperAgency);
                }
                else
                {
                    return false;
                }
            }

            return ValidationUtil.IsYes(isSuperAgency);
        }

        /// <summary>
        /// Allow inspection schedule for scheduling an inspection 1 business day in advance, default is false;
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsUseNextBusinessDayCalendar()
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string onlyScheduleOneBusinessDay = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.ONLY_SCHEDULE_ONE_BUSINESS_DAY, ACAConstant.COMMON_NO);

            if (ValidationUtil.IsYes(onlyScheduleOneBusinessDay))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Setup I18n Settings
        /// </summary>
        public static void SetupI18nInitialSettings()
        {
            I18nCultureUtil.UserPreferredCulture = I18nCultureUtil.GetUserPreferredCulture();
        }

        /// <summary>
        /// Display 'Pay Fee' link to all ACA user in Fee Section of CAP detail page.
        /// True - Allow display 'Pay Fee' link to all ACA user.
        /// False - Forbid display 'Pay Fee' link to all ACA user.
        /// </summary>
        /// <param name="moduleName">moduleName value</param>
        /// <returns>true or false</returns>
        public static bool IsDisplayPayFeeLink(string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isDispPayFeeLink = policyBll.GetValueByKey(XPolicyConstant.PAY_FEE_LINK_DISP, moduleName);

            return ValidationUtil.IsTrue(isDispPayFeeLink);
        }

        /// <summary>
        /// Allow public user to clone record?
        /// True - public user can clone record.
        /// False - public user can't clone record.
        /// </summary>
        /// <param name="moduleName">moduleName value</param>
        /// <returns>true or false</returns>
        public static bool IsEnableCloneRecord(string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isCloneSwitchOn = policyBll.GetValueByKey(XPolicyConstant.ENABLE_CLONE_RECORD, moduleName);

            return ValidationUtil.IsTrue(isCloneSwitchOn);
        }

        /// <summary>
        /// Gets a value to indicating whether the contact address is enabled.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableContactAddress()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string[] values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_ENABLE_CONTACT_ADDRESS);

            if (values != null && values.Length > 0)
            {
                foreach (string val in values)
                {
                    if (ValidationUtil.IsYes(val))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value to indicating whether required primary contact address.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsPrimaryContactAddressRequired()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string[] values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_PRIMARY_CONTACT_ADDRESS_REQUIRED);

            if (values != null && values.Length > 0)
            {
                return ValidationUtil.IsYes(values[0]);
            }

            return false;
        }

        /// <summary>
        /// Is Licensing Board Required.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsLicensingBoardRequired()
        {
            IXPolicyBll xPolicy = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            string isLicensingBoardRequired = xPolicy.GetValueByKey(XPolicyConstant.ENABLE_LICENSINGBOARD_REQUIRED, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);

            return ValidationUtil.IsYes(isLicensingBoardRequired);
        }

        /// <summary>
        /// Display Search by ASI additional criteria link.
        /// True - Show search by asi additional criteria link.
        /// False - Hidden search by asi additional criteria link.
        /// </summary>
        /// <param name="moduleName">moduleName value</param>
        /// <returns>true or false</returns>
        public static bool IsDisplayASICriteria(string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isDisplayASICriteria = policyBll.GetValueByKey(XPolicyConstant.ENABLE_SEARCHASI_ADDITIONALCRITERIA, moduleName);

            return string.IsNullOrEmpty(isDisplayASICriteria) || ACAConstant.COMMON_TRUE.Equals(isDisplayASICriteria, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Display Search by Contact template additional criteria link.
        /// True - Show search by asi additional criteria link.
        /// False - Hidden search by asi additional criteria link.
        /// </summary>
        /// <param name="moduleName">moduleName value</param>
        /// <returns>true or false</returns>
        public static bool IsDisplayContactTemplateCriteria(string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isDisplayContactCriteria = policyBll.GetValueByKey(XPolicyConstant.ENABLE_SEARCHCONTACT_ADDITIONALCRITERIA, moduleName);

            return string.IsNullOrEmpty(isDisplayContactCriteria) || ACAConstant.COMMON_TRUE.Equals(isDisplayContactCriteria, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Indicates whether display conditions of approval
        /// </summary>
        /// <returns>Return true if it setting display condition of approval</returns>
        public static bool IsDisplayConditionsOfApproval()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string[] valueList = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_CONDITIONS_OF_APPROVALS);

            if (valueList != null)
            {
                foreach (string item in valueList)
                {
                    return ValidationUtil.IsYes(item);
                }
            }

            return false;
        }

        /// <summary>
        /// Indicates whether enable fein masking.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableFeinMasking()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableFeinMasking = policyBll.GetValueByKey(XPolicyConstant.ITEM_ENABLE_FEIN_MASKING);

            // if the value is 'Yes' or 'Y','Y' enable fein mask, otherwise disable it.
            return ValidationUtil.IsYes(isEnableFeinMasking);
        }

        /// <summary>
        /// Get password security configuration by password security.
        /// </summary>
        /// <param name="passwordKey">password security key</param>
        /// <returns>the description</returns>
        public static string GetPasswordSecurityConfig(string passwordKey)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string passwordValue = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PASSWORD_POLICY_SETTINGS, passwordKey);

            return passwordValue;
        }

        /// <summary>
        /// Indicating whether the account attachment is enabled.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="peopleType">The people type.</param>
        /// <returns>true means enabled, else not</returns>
        public static bool AutoSyncPeople(string moduleName, PeopleType peopleType)
        {
            bool isAutoSyncPeople = false;
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> autoSyncPeopleList = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_AUTO_SYNC_PEOPLE, false);

            if (autoSyncPeopleList == null || autoSyncPeopleList.Count() == 0
                || autoSyncPeopleList.Count(p => moduleName.Equals(p.Key, StringComparison.InvariantCulture)) == 0)
            {
                return isAutoSyncPeople;
            }

            List<object> values = autoSyncPeopleList.Where(p => moduleName.Equals(p.Key, StringComparison.InvariantCulture)).Select(v => v.Value).ToList();

            if (values != null && values.Count > 0)
            {
                string value = (string)values[0];
                string[] splitSetting = value.Split(';');

                if (string.IsNullOrEmpty(value) || splitSetting.Length != 2)
                {
                    return isAutoSyncPeople;
                }

                string contactSetting = splitSetting[0];
                string lpSetting = splitSetting[1];
                string strAutoSyncPeople = string.Empty;

                strAutoSyncPeople = PeopleType.Contact.Equals(peopleType) ? contactSetting.Split('=')[1] : lpSetting.Split('=')[1];
                isAutoSyncPeople = ValidationUtil.IsYes(strAutoSyncPeople);
            }

            return isAutoSyncPeople;
        }

        /// <summary>
        /// Gets a flag indicating whether Board Type Selection mode is enabled for the specified module.
        /// Feature:09ACC-08040_Board_Type_Selection.
        /// Note:By default, the value is false if administrator does not set up this setting item be compatible with old versions.
        /// </summary>
        /// <param name="moduleName">The name of the module whose Board Type Selection mode are going to be get.</param>
        /// <returns>true or false.</returns>
        public static bool IsEnableBoardTypeSelection(string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            return ValidationUtil.IsYes(policyBll.GetValueByKey(XPolicyConstant.ENABLE_BOARD_TYPE_SELECTION, moduleName));
        }

        /// <summary>
        /// Indicates whether enable accessibility switch.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool AccessibilitySwitchEnabled()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isAccessibilitySwitchEnabled = policyBll.GetSuperAgencyValueByKey(XPolicyConstant.ACA_ENABLE_ACCESSIBILITY);

            // if the value is 'Yes' or 'Y', display the user initial, otherwise display the user name.
            return ValidationUtil.IsYes(isAccessibilitySwitchEnabled);
        }

        /// <summary>
        /// Indicates whether enable to reference contact search.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableRefContactSearch()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableRefContactSearch = policyBll.GetValueByKey(XPolicyConstant.ENABLE_REFERENCE_CONTACT_SEARCH);

            // if the value is 'Y', the search reference contact function is on , otherwise is off.
            return ValidationUtil.IsYes(isEnableRefContactSearch);
        }

        /// <summary>
        /// Indicates whether enable to reference license professional search.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableRefLPSearch()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableRefLPSearch = policyBll.GetValueByKey(XPolicyConstant.ENABLE_REFERENCE_LP_SEARCH);

            // if the value is 'Y', the reference license search function is on , otherwise is off.
            return ValidationUtil.IsYes(isEnableRefLPSearch);
        }

        /// <summary>
        /// Indicating whether the registration captcha is enabled.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableCaptchaForRegistration()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableCaptcha = policyBll.GetValueByKey(XPolicyConstant.ENABLE_CAPTCHA_FOR_REGISTRATION);

            return ValidationUtil.IsYes(isEnableCaptcha);
        }

        /// <summary>
        /// Indicating whether the login captcha is enabled.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableCaptchaForLogin()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableCaptcha = policyBll.GetValueByKey(XPolicyConstant.ENABLE_CAPTCHA_FOR_LOGIN);

            return ValidationUtil.IsYes(isEnableCaptcha);
        }

        /// <summary>
        /// Indicating whether the account attachment is enabled.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableAccountAttachment()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableAccountAttachment = policyBll.GetValueByKey(XPolicyConstant.ENABLE_ACCOUNT_ATTACHMENT);

            return ValidationUtil.IsYes(isEnableAccountAttachment);
        }

        /// <summary>
        /// Indicating whether the contact address is editable.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnableContactAddressEdit()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableContactAddressEdit = policyBll.GetValueByKey(XPolicyConstant.ENABLE_CONTACT_ADDRESS_EDIT);

            return !ValidationUtil.IsNo(isEnableContactAddressEdit);
        }

        /// <summary>
        /// Gets a value to indicating whether the contact address validation is enabled.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableContactAddressValidation()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string[] values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_ENABLE_CONTACT_ADDRESS_VALIDATION);

            if (values != null && values.Length > 0)
            {
                foreach (string val in values)
                {
                    if (ValidationUtil.IsYes(val))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value  indicating whether only show auto invoiced fee items.
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="moduleName">current module name</param>
        /// <returns>enable or not</returns>
        public static bool IsOnlyShowAutoInvoiceFeeItems(string agencyCode, string moduleName)
        {
            /*
             * ENABLE_AUTO_INVOICE ACA_CONFIG     AUTO_INVOICE_MODULE STANDARD CHOICE       Showed Fee Item with autoInvoiceFlag 
             * Y/NULL                                               Include                                                            ALL
             * Y/NULL                                               Exclude                                                            ALL
             * N                                                       Include                                                            ALL
             * N                                                       Exclude                                                            Y
             */

            var isEnableAutoInvoiceModule = false;
            var bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            //Get auto invoice flag in ACA_CONFIG.
            var isEnableAutoInvoice = !ValidationUtil.IsNo(bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_ENABLE_AUTO_INVOICE, ACAConstant.COMMON_Yes));

            //Get auto invoice module setting in standard choice control aa logic
            var moduleLsit = bizBll.GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_AUTO_INVOICE_MODULE, false);

            if (moduleLsit != null && moduleLsit.Count > 0)
            {
                if (moduleLsit.Any(moduleItem => moduleItem.Key.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isEnableAutoInvoiceModule = true;
                }
            }

            return !isEnableAutoInvoice && !isEnableAutoInvoiceModule;
        }

        /// <summary>
        /// Gets a value to indicating whether enable old upload behavior.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableBasicUploadProcess()
        {
            var basicUpload = false;

            // Always use Basic upload behavior if section 508 enabled.
            if (AccessibilityUtil.AccessibilityEnabled)
            {
                return true;
            }

            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            if (bizBll != null)
            {
                var values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_FILE_UPLOAD_BEHAVIOR);

                if (values != null && values.Length > 0)
                {
                    if (values.Any(val => "Basic".Equals(val, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        basicUpload = true;
                    }
                }
                else
                {
                    basicUpload = true;
                }
            }

            return basicUpload;
        }

        /// <summary>
        /// Gets a value to indicating the way to upload file.
        /// </summary>
        /// <returns>true or false</returns>
        public static FileUploadBehavior GetFileUploadBehavior()
        {
            var behavior = FileUploadBehavior.Basic;
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            if (bizBll != null)
            {
                var values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_FILE_UPLOAD_BEHAVIOR);

                if (values != null && values.Length > 0)
                {
                    // If select multiple options, use html5 first, then basic, advanced at last.
                    if (values.Any(val => BizDomainConstant.STD_FILE_UPLOAD_BEHAVIOR_ITEM_HTML5.Equals(val, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        behavior = FileUploadBehavior.Html5;
                    }
                    else if (values.Any(val => BizDomainConstant.STD_FILE_UPLOAD_BEHAVIOR_ITEM_BASIC.Equals(val, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        behavior = FileUploadBehavior.Basic;
                    }
                    else if (values.Any(val => BizDomainConstant.STD_FILE_UPLOAD_BEHAVIOR_ITEM_ADVANCED.Equals(val, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        behavior = FileUploadBehavior.Advanced;
                    }
                }
            }

            // The default upload behavior should be the agency¡¯s choice, if not, always use Basic upload behavior if section 508 enabled.
            if (AccessibilityUtil.AccessibilityEnabled && behavior == FileUploadBehavior.Advanced)
            {
                return FileUploadBehavior.Basic;
            }

            return behavior;
        }

        /// <summary>
        /// Indicating whether the customer detail is editable for clerk.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsCustomerDetailEditable()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string isEditable = policyBll.GetValueByKey(XPolicyConstant.AUTH_AGENT_CUSTOMER_EDITABLE);

            return !ValidationUtil.IsNo(isEditable);
        }

        /// <summary>
        /// Gets a value to indicating whether the browser detect is enabled.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableBrowserDetect()
        {
            var bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            var enableBrowserDetect = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_ENABLE_BROWSER_DETECT);

            return ValidationUtil.IsYes(enableBrowserDetect);
        }

        /// <summary>
        /// Gets create application model.
        /// </summary>
        /// <returns>NormalModel: Save record normal, SaveDataInConfirmPageModel: Save record in confirm page.</returns>
        public static string CreateApplicationModel()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string createApplicationModel = bizBll.GetValueForACAConfig(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_ITEM_CREATE_APPLICATION_MODEL);

            return createApplicationModel;
        }

        /// <summary>
        /// Pay At Counter button control by "DEFER_PAYMENT_ENABLED" standard choice of "ACA_CONFIGS"
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableDeferPayment()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string stdDeferPaymentFlag = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_DEFER_PAYMENT_ENABLED);
            return ValidationUtil.IsYes(stdDeferPaymentFlag);
        }

        /// <summary>
        /// Get a value indicating whether enable expand/collapse ability for review page
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <returns>Whether enable expand/collapse ability for review page</returns>
        public static bool IsEnableExpandReviewSection(string agencyCode)
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            //Get enable expand review page flag in ACA_CONFIG.
            return ValidationUtil.IsYes(bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_ENABLE_EXPAND_REVIEW, ACAConstant.COMMON_No));
        }

        /// <summary>
        /// Get a value indicating whether display owner section
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsDisplayOwnerSection()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            //Get enable owner in ACA_CONFIG, if standard choice not configure set default value is "Y".
            return ValidationUtil.IsYes(bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_DISPLAY_OWNER_INFORMATION, ACAConstant.COMMON_Y));
        }

        /// <summary>
        /// If it need auto update inspection result.
        /// </summary>
        /// <returns>Return true if is auto-update inspection result.</returns>
        public static bool IsAutoUpdateInspectionResult()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string isAutoUpdate = policyBll.GetValueByKey(XPolicyConstant.ENABLE_AUTO_UPDATE_INSPECTION_RESULT);

            return ValidationUtil.IsYes(isAutoUpdate);
        }

        /// <summary>
        /// Indicates whether enable contact address "Deactivate" action.
        /// </summary>
        /// <returns>true means remove, else not</returns>
        public static bool IsEnableContactAddressDeactivate()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string isEnableContactAddressDeactivate = policyBll.GetValueByKey(XPolicyConstant.ENABLE_CONTACT_ADDRESS_DEACTIVATE);

            // if the value is 'Yes' or 'Y', the "Deactivate" menu is Enable , otherwise is Disenable.
            return ValidationUtil.IsYes(isEnableContactAddressDeactivate);
        }

        /// <summary>
        /// Get a value indicating whether enable account education, examination and continuing education input.
        /// </summary>
        /// <returns>Whether enable account education, examination and continuing education input.</returns>
        public static bool IsEnableAccountEduExamCEInput()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            return ValidationUtil.IsYes(policyBll.GetValueByKey(XPolicyConstant.ENABLE_ACCOUNT_EDU_EXAM_CE_INPUT));
        }

        /// <summary>
        /// Get Pop Action Item Style
        /// </summary>
        /// <returns>The pop action item style.</returns>
        public static string GetPopActionItemStyle()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            return bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_POP_ACTION_ITEM_STYLE, "LIST");
        }

        /// <summary>
        /// Indicates whether the users' setting combine the reset password info in one page.
        /// </summary>
        /// <returns>true means to show in one page, else not</returns>
        public static bool IsEnableResetPasswordOnCombine()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            return ValidationUtil.IsYes(policyBll.GetValueByKey(XPolicyConstant.ENABLE_RESETPASSWORD_ON_COMBINE));
        }

        /// <summary>
        /// Get RegistrationInfo ExpireTime to Indicates whether the user information is Expired.
        /// </summary>
        /// <returns>The registration information's expire time.</returns>
        public static XPolicyModel[] GetRegistrationInfoExpireTime()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            return policyBll.GetPolicyListByCategory(XPolicyConstant.ENABLE_LOGIN_ON_REGISTRATION, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);
        }

        /// <summary>
        /// Indicates whether the users' setting enabled login on registration while registration successful.
        /// </summary>
        /// <returns>true means enabled, else not</returns>
        public static bool IsEnabledLoginOnRegistration()
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            return ValidationUtil.IsYes(policyBll.GetValueByKey(XPolicyConstant.ENABLE_LOGIN_ON_REGISTRATION));
        }

        /// <summary>
        /// Get a value indicating whether expanding all steps of BreadcrumbsBar or not
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Indicate whether is expend breadcrumb bar.</returns>
        public static bool IsExpandBreadcrumbBar(string agencyCode)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            //Get display quick query flag in ACA_CONFIG.
            return ValidationUtil.IsYes(bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_EXPAND_BREADCRUMB_BAR, ACAConstant.COMMON_No));
        }
        
        /// <summary>
        /// Gets a value indicating whether to enable customization per page or not
        /// </summary>
        /// <returns>enable or not</returns>
        public static bool IsEnableCustomizationPerPage()
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            return ValidationUtil.IsYes(bizBll.GetValueForACAConfig(ConfigManager.SuperAgencyCode, BizDomainConstant.STD_ITEM_ENABLE_CUSTOMIZATION_PER_PAGE, ACAConstant.COMMON_No));
        }

        /// <summary>
        /// Indicates whether display conditions of approval
        /// </summary>
        /// <returns>Return true if it setting display condition of approval</returns>
        public static string[] GetConditionTypeFilter()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            return bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.DOCUMENT_CONDITION_TYPE_FILTER);
        }

        /// <summary>
        /// Get a value indicating whether show Quick Query dropdownlist
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <returns>Whether show Quick Query dropdownlist.</returns>
        public static bool IsDisplayQuickQuery(string agencyCode)
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            //Get display quick query flag in ACA_CONFIG.
            return ValidationUtil.IsYes(bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_DISPLAY_QUICK_QUERY, ACAConstant.COMMON_No));
        }

        /// <summary>
        /// Get a value which indicates whether block or continue when no inspector was found.
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <returns>Whether block or continue when no inspector was found.</returns>
        public static bool IsBlockedWhenNoInspectorFound(string agencyCode)
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string stdItemValue = bizBll.GetValueForStandardChoice(agencyCode, BizDomainConstant.STD_ACA_AUTO_ASSIGN_INSPECTOR, BizDomainConstant.STD_ITEM_BLOCK_SCHEDULE_WHEN_NO_INSPECTOR_FOUND);

            if (string.IsNullOrEmpty(stdItemValue))
            {
                return true;
            }

            return ValidationUtil.IsYes(stdItemValue);
        }

        /// <summary>
        /// Indicates whether to enable url reference checking
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableUrlRefererCheck()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string isEnableUrlRefererCheck = bizBll.GetValueForStandardChoice(
                                                                ConfigManager.AgencyCode,
                                                                BizDomainConstant.STD_CAT_SECURITY_SETTING,
                                                                BizDomainConstant.STD_ITEM_SECURITY_SETTING_ENABLE_URL_REFERER_CHECK);

            return ValidationUtil.IsYes(isEnableUrlRefererCheck);
        }

        /// <summary>
        /// Get the trusted external site
        /// </summary>
        /// <returns>trusted site list</returns>
        public static string[] GetTrustedSiteUrls()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string trustedSites = bizBll.GetValueForStandardChoice(
                                                        ConfigManager.AgencyCode,
                                                        BizDomainConstant.STD_CAT_SECURITY_SETTING,
                                                        BizDomainConstant.STD_ITEM_SECURITY_SETTING_TRUSTED_SITES);

            if (!string.IsNullOrEmpty(trustedSites))
            {
                return trustedSites.Split(ACAConstant.COMMA_CHAR);
            }

            return null;
        }

        /// <summary>
        /// Is enable display ISLAMIC calendar
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableDisplayISLAMICCalendar()
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            var values = bizBll.GetBizDomainValueList(ConfigManager.AgencyCode, BizDomainConstant.STD_ENABLE_DISPLAY_ISLAMIC_CALENDAR);
            
            if (values != null)
            {
                return ValidationUtil.IsYes(values.First());
            }

            return false;
        }

        /// <summary>
        /// Indicates whether current address source is external or not.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsExternalAddressSource()
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            var value = bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, BizDomainConstant.STD_EXTERNAL_ADDRESS_SOURCE, BizDomainConstant.STD_EXTERNAL_SOURCE_LOCATION);

            return BizDomainConstant.STD_EXTERNAL_SOURCE_LOCATION_VALUE.Equals(value);
        }

        /// <summary>
        /// Indicates whether current parcel source is external or not.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsExternalParcelSource()
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            var value = bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, BizDomainConstant.STD_EXTERNAL_PARCEL_SOURCE, BizDomainConstant.STD_EXTERNAL_SOURCE_LOCATION);

            return BizDomainConstant.STD_EXTERNAL_SOURCE_LOCATION_VALUE.Equals(value);
        }

        /// <summary>
        /// Indicates whether current owner source is external or not.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsExternalOwnerSource()
        {
            var bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            var value = bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, BizDomainConstant.STD_EXTERNAL_OWNER_SOURCE, BizDomainConstant.STD_EXTERNAL_SOURCE_LOCATION);

            return BizDomainConstant.STD_EXTERNAL_SOURCE_LOCATION_VALUE.Equals(value);
        }

        /// <summary>
        /// Indicates to use new template or classic template.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool IsEnableNewTemplate()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableNewTemplate = policyBll.GetValueByKey(XPolicyConstant.ENABLE_NEW_TEMPLATE);

            return ValidationUtil.IsTrue(isEnableNewTemplate);
        }

        /// <summary>
        /// Get Url of GIS in new template.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>Url of GIS</returns>
        public static string GetNewGISServerURL(string agencyCode)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string url = bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_NEW_GIS_PORLET_URL).Trim();

            return url;
        }

        /// <summary>
        /// Indicates whether need to turn on the Map feature based on module.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="key">Standard choice key.</param>
        /// <returns>true-turn on to show Map, false-turn off.</returns>
        private static bool IsShowMap(string moduleName, string key)
        {
            bool isShowMap = false;

            if (HasGISSettings())
            {
                if (!string.IsNullOrEmpty(moduleName))
                {
                    IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

                    // the default value should be set to NO for backward compatible.
                    string value = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, moduleName + "_" + key, ACAConstant.COMMON_NO);

                    // if the value is No - don't show map.
                    isShowMap = !ValidationUtil.IsNo(value);
                }
                else
                {
                    isShowMap = true;
                }

                if (isShowMap)
                {
                    isShowMap = !IsEnableNewTemplate();
                }
            }

            return isShowMap;
        }

        #endregion Methods
    }
}