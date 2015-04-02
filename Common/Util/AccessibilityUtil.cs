#region Header

/**
 *  Accela Citizen Access
 *  File: AccessibilityUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *   Provide accessibility function.
 *
 *  Notes:
 * $Id: AccessibilityUtil.cs 279179 2014-10-13 08:59:15Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// The accessibility utility class
    /// </summary>
    public static class AccessibilityUtil
    {
        /// <summary>
        /// Gets or sets a value indicating whether support accessibility or not.
        /// </summary>
        /// <returns>bool true or false</returns>
        public static bool AccessibilityEnabled
        {
            get
            {
                bool isEnableAccessibility = ConfigManager.DefaultSupportAccessibility;
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(CookieConstant.SUPPORT_ACCESSSIBILITY);
                
                if (cookie != null && cookie.Value != null)
                {
                    cookie.HttpOnly = true;
                    bool.TryParse(ScriptFilter.AntiXssHtmlEncode(cookie.Value), out isEnableAccessibility);
                }

                return isEnableAccessibility;
            }

            set
            {
                HttpCookie cookie = new HttpCookie(CookieConstant.SUPPORT_ACCESSSIBILITY, Convert.ToString(value));
                cookie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Response.Cookies[CookieConstant.SUPPORT_ACCESSSIBILITY].Expires = DateTime.Now.AddYears(50);
            }
        }

        /// <summary>
        /// Gets the access key.
        /// </summary>
        /// <param name="keyType">Type of the access key.</param>
        /// <returns>the access key</returns>
        public static string GetAccessKey(AccessKeyType keyType)
        {
            string result = string.Empty;

            switch (keyType)
            {
                case AccessKeyType.Help:
                    result = "h";
                    break;
                case AccessKeyType.SkipNavigation:
                    result = "0";
                    break;
                case AccessKeyType.SkipToolBar:
                    result = "1";
                    break;
                case AccessKeyType.AccessibilitySetup:
                    result = "2";
                    break;
                case AccessKeyType.SubmitForm:
                    result = "3";
                    break;
                case AccessKeyType.HomePage:
                    result = "4";
                    break;
                case AccessKeyType.ValidationResults:
                    result = "9";
                    break;
            }

            return result;
        }

        /// <summary>
        /// Focus UI element by javascript.
        /// </summary>
        /// <param name="elemntClientId">the element client id for focus.</param>
        public static void FocusElement(string elemntClientId)
        {
            Page currentPage = HttpContext.Current.Handler as Page;
            FocusElement(currentPage, elemntClientId);
        }

        /// <summary>
        /// Focus UI element by javascript with special control  
        /// </summary>
        /// <param name="control">the special control use to Register javascript</param>
        /// <param name="elementClientId">the element client id for focus.</param>
        public static void FocusElement(this Control control, string elementClientId)
        {
            if (control != null && !string.IsNullOrEmpty(elementClientId))
            {
                /*
                 * In the IE browser, set the focus will lead to other script execution is very slow, so introduce a delay logic to solve this problem.
                 * 
                 * Using AntiXssHtmlEncode to encode the client ID to avoid the cross-site scripting injection.
                 * The client ID is used in the javascript as a string, use AntiXssJavaScriptEncode will generate similar '\x7d' such characters and cause js error.
                 * So use AntiXssHtmlEncode to encode the client ID.
                 */
                string focusScripts = string.Format(
                                                    @"setTimeout(function () {{
                                                        $('#{0}').focus();
                                                        $lastFocus = $('#__LASTFOCUS_ID');

                                                        if ($lastFocus.val() == '{0}') {{
                                                            $($lastFocus).val('');
                                                        }}
                                                    }}, 0);",
                                                    ScriptFilter.AntiXssHtmlEncode(elementClientId));

                ScriptManager.RegisterStartupScript(control, control.GetType(), "Focus" + elementClientId + CommonUtil.GetRandomUniqueID().Substring(0, 6), focusScripts, true);
            }
        }

        /// <summary>
        /// Gets the GridViewCommandEventArgs object included active row.
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        /// <returns>
        /// If have value return value, else return null
        /// </returns>
        public static GridViewRow GetRow(this GridViewCommandEventArgs e)
        {
            if (e != null)
            {
                Type gVCEAType = e.GetType();
                PropertyInfo pi = gVCEAType.GetProperty("Row", BindingFlags.NonPublic | BindingFlags.Instance);
                if (pi != null)
                {
                    return pi.GetValue(e, null) as GridViewRow;
                }
            }

            return null;
        }

        /// <summary>
        /// Focuses the name of the row cell by.
        /// </summary>
        /// <param name="e">The instance containing the event data.</param>
        /// <param name="controlName">Name of the control.</param>
        public static void FocusRowCellByName(this GridViewCommandEventArgs e, string controlName)
        {
            GridViewRow row = e.GetRow();
            if (row != null)
            {
                Control control = row.FindControl(controlName);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }
    }
}
