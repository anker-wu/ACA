/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Default.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: Default.aspx.cs 277475 2014-08-15 08:21:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

/// <summary>
/// the class for default page.
/// </summary>
public partial class Default : Page
{
    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string errorMessage = string.Empty;
        string defaultPageVirtualPath = string.Empty;
        bool languageChanged = false;
        try
        {
            string defaultPage = ConfigurationManager.AppSettings["DefaultPageFile"];
            defaultPageVirtualPath = FileUtil.AppendApplicationRoot(defaultPage);
            errorMessage = HandleLanguageSwitch(errorMessage, ref languageChanged);
            errorMessage = HandleGoingToHomePageAction(errorMessage);
            errorMessage = HandleCheckingDefaultPageSetting(defaultPage, defaultPageVirtualPath, errorMessage);
        }
        catch (Exception ex)
        {
            errorMessage = ExceptionUtil.GetErrorMessage(ex);
        }

        //determine if has error message.
        if (!string.IsNullOrEmpty(errorMessage))
        {
            lblmsg.Text = errorMessage;
            lblmsg.Visible = true;
            lblmsg.BackColor = Color.Yellow;
            lblmsg.ForeColor = AccessibilityUtil.AccessibilityEnabled ? Color.FromArgb(255, 229, 0, 0) : Color.Red;
        }
        else
        {
            if (languageChanged)
            {
                Response.Redirect(FileUtil.ApplicationRoot, true);
            }

            string queryString = Request.QueryString.ToString() == string.Empty ? string.Empty : "?" + Request.QueryString.ToString();

            if (StandardChoiceUtil.IsEnableNewTemplate())
            {
                Response.Redirect(FileUtil.AppendApplicationRoot(ACAConstant.URL_NEW_UI) + queryString);
            }
            else
            {
                Server.Execute(defaultPageVirtualPath + queryString);
            }
        }
    }

    /// <summary>
    /// handle language switch, if parameter previousErrorMessage is not null or empty ,return it, otherwise return handled result.
    /// </summary>
    /// <param name="previousErrorMessage">previous error message</param>
    /// <param name="languageChanged">is language changed.</param>
    /// <returns>string for handle language switch.</returns>
    private string HandleLanguageSwitch(string previousErrorMessage, ref bool languageChanged)
    {
        if (!I18nCultureUtil.IsMultiLanguageEnabled)
        {
            return string.Empty;
        }

        if (!string.IsNullOrEmpty(previousErrorMessage))
        {
            return previousErrorMessage;
        }
        else
        {
            if (!Page.IsPostBack)
            {
                languageChanged = HandleLanguageSwitch();
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// handle going to the home page, if parameter previousErrorMessage is not null or empty ,return it, otherwise return handled result.
    /// </summary>
    /// <param name="previousErrorMessage">previous error message</param>
    /// <returns>string for handle going to home page action.</returns>
    private string HandleGoingToHomePageAction(string previousErrorMessage)
    {
        if (!string.IsNullOrEmpty(previousErrorMessage))
        {
            return previousErrorMessage;
        }

        string resetUrl = Request.QueryString["resetUrl"];

        if (null != resetUrl && resetUrl.Equals(ACAConstant.COMMON_YES, StringComparison.InvariantCultureIgnoreCase))
        {
            Session[ACAConstant.ACTIVE_TAB_NAME] = null;
            Session[ACAConstant.CURRENT_URL] = null;
        }

        return string.Empty;
    }

    /// <summary>
    /// handle Checking Default Page Setting, if parameter previousErrorMessage is not null or empty ,return it, otherwise return handled result.
    /// </summary>
    /// <param name="defaultPage">default page.</param>
    /// <param name="defaultPageVirtualPath">default page virtual path</param>
    /// <param name="previousErrorMessage">previous error message</param>
    /// <returns>string for handle checking default page setting.</returns>
    private string HandleCheckingDefaultPageSetting(string defaultPage, string defaultPageVirtualPath, string previousErrorMessage)
    {
        if (!string.IsNullOrEmpty(previousErrorMessage))
        {
            return previousErrorMessage;
        }

        string errorMessage = string.Empty;

        if (defaultPage == null || defaultPage == string.Empty)
        {
            errorMessage = GetErrorMessageOfConfigDefaultPageFile();
        }
        else if (!File.Exists(Server.MapPath(defaultPageVirtualPath)))
        {
            errorMessage = GetErrorMessageOfConfirmFileExists(defaultPage);
        }

        return errorMessage;
    }

    /// <summary>
    /// Get error message of "config default page file".
    /// </summary>
    /// <returns>string error message.</returns>
    private string GetErrorMessageOfConfigDefaultPageFile()
    {
        Dictionary<string, string> supportedLanguageList = I18nCultureUtil.GetSupportedLanguageList();
        List<string> messageList = new List<string>();
        StringBuilder sb = new StringBuilder();

        foreach (string culture in supportedLanguageList.Keys)
        {
            if (sb.Length > 0)
            {
                sb.Append("<br>");
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            string message = LabelUtil.GetGlobalTextByKey("aca_home_error_configdefaultpagefile");

            if (!messageList.Contains(message))
            {
                messageList.Add(message);
                sb.Append(message); //(*****I18nStamp:Resources*****)
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Get error message of "Confirm File Exists".
    /// </summary>
    /// <param name="defaultPage">default page.</param>
    /// <returns>string for error message.</returns>
    private string GetErrorMessageOfConfirmFileExists(string defaultPage)
    {
        Dictionary<string, string> supportedLanguageList = I18nCultureUtil.GetSupportedLanguageList();
        List<string> messageList = new List<string>();
        StringBuilder sb = new StringBuilder();

        foreach (string culture in supportedLanguageList.Keys)
        {
            if (sb.Length > 0)
            {
                sb.Append("<br>");
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            string message = string.Format(LabelUtil.GetGlobalTextByKey("aca_home_error_confirmfileexists"), "\"" + defaultPage + "\"");

            if (!messageList.Contains(message))
            {
                messageList.Add(message);
                sb.Append(message); //(*****I18nStamp:Resources*****)
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// handle language switch, if <c>I18nCultureUtil.UserPreferredCulture</c> changed, return true, otherwise, return false
    /// </summary>
    /// <returns>true or false.</returns>
    private bool HandleLanguageSwitch()
    {
        bool isLanguageChanged = false;
        string culture = System.Web.HttpContext.Current.Request.QueryString["culture"];

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
