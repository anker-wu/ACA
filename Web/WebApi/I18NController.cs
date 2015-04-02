#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: I18NController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:I18NController.cs 77905 2014-08-27 12:49:28Z ACHIEVO\Reid.wang.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Get I18N language culture
    /// </summary>
    public class I18NController : ApiController
    {
        /// <summary>
        /// Get the Culture Language
        /// </summary>
        /// <param name="isAdmin">is Admin</param>
        /// <returns>Y or N</returns>
        [ActionName("Culture-Language")]
        public HttpResponseMessage GetCulturelanguage(bool isAdmin)
        {
            string result = ApiUtil.GetLanguages();

            // When login admin site, all daily pages must contain isAdmin in url. Login admin then enter daily directly.
            if (AppSession.IsAdmin && !isAdmin)
            {
                // in daily mode forece session back to daily mode from admin
                AppSession.IsAdmin = false;
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Change the Culture Language
        /// </summary>
        /// <param name="culture">culture parameter</param>
        /// <returns>Y or N</returns>
        [ActionName("Change-Language")]
        [HttpGet]
        public HttpResponseMessage ChangeCulturelanguage(string culture)
        {
            StringBuilder result = new StringBuilder();
            bool isLanguageChanged = HandleLanguageSwitch(culture);

            if (isLanguageChanged)
            {
                result.Append("{\"changed\":\"success\"}");
            }
            else
            {
                result.Append("{\"changed\":\"error\"}");
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result.ToString())
            };
        }

        /// <summary>
        /// handle language switch, if <c>I18nCultureUtil.UserPreferredCulture</c> changed, return true, otherwise, return false
        /// </summary>
        /// <param name="culture">culture parameter</param>
        /// <returns>true or false.</returns>
        [NonAction]
        private bool HandleLanguageSwitch(string culture)
        {
            bool isLanguageChanged = false;

            if (!string.IsNullOrEmpty(culture))
            {
                Dictionary<string, string> supportedCultureList = I18nCultureUtil.GetSupportedLanguageList();
                foreach (string key in supportedCultureList.Keys)
                {
                    if (key.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                    {
                        I18nCultureUtil.UserPreferredCulture = culture;
                        isLanguageChanged = true;
                        break;
                    }
                }
            }

            if (isLanguageChanged && AppSession.User != null && AppSession.User.UserModel4WS != null && !string.IsNullOrEmpty(AppSession.User.UserSeqNum))
            {
                IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
                PeopleModel4WS[] peoples = peopleBll.GetAssociatedContactsByUserId(ConfigManager.AgencyCode, AppSession.User.UserSeqNum);

                if (peoples != null && peoples.Length > 0 && AppSession.User.UserModel4WS.peopleModel != null)
                {
                    AppSession.User.UserModel4WS.peopleModel = peoples;
                }

                /*
                if lang changed,updated the User session's template fields.
                because User session cache the User's template fields
                */
                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                var templatefields = templateBll.GetRefPeopleTemplateAttributes(
                                                                                ACAConstant.PUBLIC_USER,
                                                                                AppSession.User.UserSeqNum,
                                                                                ConfigManager.AgencyCode,
                                                                                AppSession.User.UserID);

                if (templatefields != null && templatefields.Length > 0)
                {
                    AppSession.User.UserModel4WS.templateAttributes = templatefields;
                }
            }

            // clear the general view cache when languge changed
            if (isLanguageChanged)
            {
                string cacheKey = ConfigManager.AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_GVIEW_ELEMENT;
                HttpRuntime.Cache.Remove(cacheKey);
            }

            return isLanguageChanged;
        }
    }
}