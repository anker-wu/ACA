/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ActionRequiredController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:ActioncenterController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Common.RestAPI;
using Accela.ACA.Web.Common;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Web API for Action Required
    /// </summary>
    public class ActionRequiredController : ApiController
    {
        #region public methods
        
        /// <summary>
        /// Get Action Required record list
        /// </summary>
        /// <returns>Action Required record list</returns>
        public HttpResponseMessage GetActionRequiredInfo()
        {
            string cacheKey = "ActionRequired" + AppSession.User.UserToken;
            string result = string.Empty;
            string actionRequired = string.Empty;

            if (string.IsNullOrEmpty(result))
            {
                if (!string.IsNullOrEmpty(AppSession.User.UserToken))
                {
                    string url = WSConfiguration.GetConfig().WebSites[0].Url;
                    Uri bastUri = new Uri(url);
                    string[] splitUrl = url.Split(new string[] { bastUri.Authority }, StringSplitOptions.RemoveEmptyEntries);

                    string modules = GetModules();

                    url = splitUrl[0] + bastUri.Authority + "/apis/v4/records/mine/tasks?token=" + AppSession.User.UserToken + "&limit=500&expirationDay=30&modules=" + modules;

                    actionRequired = ApiUtil.Get(url, 30000);
                }

                ResponseModel responseModel = (ResponseModel)JsonConvert.DeserializeObject(actionRequired, typeof(ResponseModel));

                if (responseModel != null && HttpStatusCode.OK.Equals(responseModel.Status) && responseModel.Result != null)
                {
                    ApiUtil.AddCache(actionRequired, cacheKey);
                    result = actionRequired;
                }
            }

            return new HttpResponseMessage { Content = new StringContent(result) };
        }

        /// <summary>
        /// Get Is Super Agency value
        /// </summary>
        /// <returns>Is Super Agency value</returns>
        public HttpResponseMessage GetIsSuperAgency()
        {
            string isSubAgencyCap = StandardChoiceUtil.IsSuperAgency() ? ACAConstant.COMMON_Y : string.Empty;

            return new HttpResponseMessage { Content = new StringContent(isSubAgencyCap) };
        }

        #endregion

        #region private method

        /// <summary>
        /// Get all modules name
        /// </summary>
        /// <returns>all modules name</returns>
        public string GetModules()
        {
            string modules = string.Empty;

            // get all defined tabs and links.
            IList<TabItem> tabsList = TabUtil.GetTabList(false);

            foreach (TabItem tab in tabsList)
            {
                // if the tab needn't to be showed in home page as link block
                if (!tab.BlockVisible || string.IsNullOrEmpty(tab.Label) || tab.Key == "APO")
                {
                    continue;
                }

                string label = LabelUtil.GetSuperAgencyTextByKey(tab.Label, tab.Module);

                if (label == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
                {
                    label = DataUtil.AddBlankToString(tab.Module);
                }

                tab.Title = LabelUtil.RemoveHtmlFormat(label);

                // found the block links in home page.
                if (tab.Children.Count > 0)
                {
                    modules += tab.Module + ",";
                }
            }

            return modules;
        }
        #endregion
    }
}