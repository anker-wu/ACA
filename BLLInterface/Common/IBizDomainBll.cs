#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IBizDomainBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IBizDomainBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approach to get standard choice value.
    /// </summary>
    public interface IBizDomainBll
    {
        #region Methods

        /// <summary>
        /// Define Standard Choice,Create and Edit Biz Domain Model Array.
        /// </summary>
        /// <param name="bizDomainModel4WSArray">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        bool CreateAndEditBizDomain(BizDomainModel4WS[] bizDomainModel4WSArray);

        /// <summary>
        /// Define Standard Choice,Create and Edit Biz Domain Model Array.
        /// </summary>
        /// <param name="bizDomainModel4WSArray">BizDomainMode for web service</param>
        /// <param name="ignoreLanguage">ignoring language flag</param>
        /// <returns>true successful, false failure.</returns>
        bool CreateAndEditBizDomain(BizDomainModel4WS[] bizDomainModel4WSArray, bool ignoreLanguage);

        /// <summary>
        /// Define Standard Choice and Create Biz Domain to save.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        bool CreateBizDomain(BizDomainModel4WS bizDomainModel4WS);

        /// <summary>
        /// Define Standard Choice and Update Biz Domain.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        bool EditBizDomain(BizDomainModel4WS bizDomainModel4WS);

        /// <summary>
        /// Method to get contact type in biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bizName">biz domain name</param>
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <returns>ItemValue list.</returns>
        IList<ItemValue> GetBizDomainList(string agencyCode, string bizName, bool includeInactivity);

        /// <summary>
        /// Method to get contact type in biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <param name="contactTypeSource">BIZ domain description</param>
        /// <returns>
        /// Contact type list.
        /// </returns>
        IList<ItemValue> GetContactTypeList(string agencyCode, bool includeInactivity, string contactTypeSource);

        /// <summary>
        /// Method to get biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bizName">biz domain name</param>
        /// <param name="includeInactivity">whether need to show inactivity record</param>
        /// <param name="showType">indicates what value is used for standard choice, value,value description or both</param>
        /// <returns>Standard choice item list.</returns>
        IList<ItemValue> GetBizDomainList(string agencyCode, string bizName, bool includeInactivity, int showType);

        /// <summary>
        /// Method to get biz domain(standard choice) object model
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="callerID">callerID number.</param>
        /// <param name="ignoreLanguage">ignoring language flag</param>
        /// <returns>Biz Domain Model</returns>
        BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string callerID, bool ignoreLanguage);

        /// <summary>
        /// Method to get biz domain(standard choice) object model.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="callerID">callerID number.</param>
        /// <returns>Biz Domain Model</returns>
        BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string callerID);

        /// <summary>
        /// Method to get biz domain(standard choice) object model.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="key">key values.</param>
        /// <param name="callerID">callerID  number.</param>
        /// <returns>Biz Domain Model</returns>
        BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string key, string callerID);

        /// <summary>
        /// Method to get biz domain(standard choice) values.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bizName">biz domain name</param>
        /// <returns>Array of String</returns>
        string[] GetBizDomainValueList(string agencyCode, string bizName);

        /// <summary>
        /// This Configured Url of link button.
        /// </summary>
        /// <returns>string for configured url</returns>
        string GetConfiguredUrlFromXPolicy();

        /// <summary>
        /// This method gets the Tabs from the DB and sorts them in the right order.
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="userType">ACA user type</param>
        /// <param name="hasProviderLicense">if set to <c>true</c> [has provider license].</param>
        /// <returns>Sorted List of tabs</returns>
        IList<TabItem> GetTabsList(string agencyCode, ACAUserType userType, bool hasProviderLicense);

        /// <summary>
        /// Gets value by standard choice of ACA_CONFIG category.
        /// if the key doesn't exist, it returns the defaultValue.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="key">standard choice key in ACA_CONFIG category.</param>
        /// <param name="defaultValue">if the key doesn't exist or empty, it returns defaultValue passed by caller.</param>
        /// <returns>standard choice value, return default value if the key can't be found from admin configuration(AA).</returns>
        string GetValueForACAConfig(string agencyCode, string key, string defaultValue);

        /// <summary>
        /// Get Condition Group Name for I18N
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="conditionGroup">Condition Group</param>
        /// <returns>Condition Group for I18N</returns>
        string GetConditionGroupFor18N(string agencyCode, string conditionGroup);

        /// <summary>
        /// Gets value by standard choice of ACA_CONFIG category.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="key">standard choice key in ACA_CONFIG category.</param>
        /// <returns>standard choice value, return string.Empty</returns>
        string GetValueForACAConfig(string agencyCode, string key);

        /// <summary>
        /// Gets value by standard choice by given category and key.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="category">standard choice category</param>
        /// <param name="key">standard choice key in category.</param>
        /// <returns>standard choice value, return string.Empty</returns>
        string GetValueForStandardChoice(string agencyCode, string category, string key);

        /// <summary>
        /// Indicates whether only allow selection one service.
        /// </summary>
        /// <param name="module">module name</param>
        /// <returns>true - single service selection only,false - allow select multiple services.</returns>
        bool IsSingleServiceOnly(string module);

        /// <summary>
        /// Indicates whether the url need to force login.
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="url">url,only file name such as Cap Home page</param>
        /// <param name="urlParameter">url parameter that identify the key with url. Most, it is null.</param>
        /// <returns>true-need force login,false-needn't force login</returns>
        bool IsForceLogin(string module, string url, string urlParameter);

        /// <summary>
        /// The standard choice item value of the biz domain.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainItemKey">The standard choice item key</param>
        /// <returns>The standard choice item description</returns>
        string GetBizDomainItemDesc(string agencyCode, string bizDomain, string bizDomainItemKey);

        /// <summary>
        /// Judge whether the biz domain is existed or not.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainValues">biz domain value</param>
        /// <returns>message for exist biz domain value</returns>
        string IsExistBizDomainValue(string servProvCode, string bizDomain, string[] bizDomainValues);

        /// <summary>
        /// Gets the biz domain value.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="category">The category.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <param name="ignoreLanguage">if set to <c>true</c> [ignore language].</param>
        /// <param name="culture">The culture.</param>
        /// <returns>standard choice list</returns>
        BizDomainModel4WS[] GetBizDomainValue(
            string agencyCode,
            string category,
            QueryFormat4WS queryFormat,
            bool ignoreLanguage,
            string culture);

        #endregion Methods
    }
}
