#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MessageUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *
 *  Notes:
 *      $Id: MessageUtil.cs 277836 2014-08-21 13:31:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Message utility class
    /// </summary>
    public static class MessageUtil
    {
        #region Methods

        /// <summary>
        /// Filter some special HTML char
        /// </summary>
        /// <param name="msg">content need be filtered.</param>
        /// <returns>content with filtered special chars</returns>
        public static string FilterQuotation(string msg)
        {
            string result = FilterQuotation(msg, false);
            return result;
        }

        /// <summary>
        /// Filters the quotation.
        /// </summary>
        /// <param name="msg">The message to show.</param>
        /// <param name="isForJavascript">if set to <c>true</c> [is for javascript].</param>
        /// <returns>the filtered message</returns>
        public static string FilterQuotation(string msg, bool isForJavascript)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return string.Empty;
            }

            //replace \ with "\"
            string result = msg.Replace("\\", "\\\\");

            // replace " with \"
            result = result.Replace("\"", "\\\"");

            // replace ' with \'
            result = result.Replace("'", "\\'");

            // replace enter with html mark "<br/>", "&nbsp;"
            if (isForJavascript)
            {
                result = Regex.Replace(result, "<br/>", "\\n", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "<br>", "\\n", RegexOptions.IgnoreCase);
                result = result.Replace("&nbsp;", " ");
            }
            else
            {
                result = result.Replace("\r\n", "<br/>");
                result = result.Replace("\n", "<br/>");
            }

            return result;
        }

        /// <summary>
        /// Gets the alert script.
        /// </summary>
        /// <param name="msg">The alert message.</param>
        /// <returns>the alert script.</returns>
        public static string GetAlertScript(string msg)
        {
            string result = FilterQuotation(msg, true);
            string jsPattern = "<script>alert(\"{0}\");</script>";
            result = string.Format(jsPattern, result);

            return result;
        }

        /// <summary>
        /// Show alert message.
        /// </summary>
        /// <param name="control">Control use to register script.</param>
        /// <param name="msg">alert message string.</param>
        public static void ShowAlertMessage(System.Web.UI.Control control, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                string result = FilterQuotation(msg, true);
                result = LabelUtil.RemoveHtmlFormat(result);
                string scriptStr = string.Format("alert('{0}');", result);
                ScriptManager.RegisterClientScriptBlock(control, control.GetType(), "showAlertMessage", scriptStr, true);
            }
        }

        /// <summary>
        /// Hide the message.
        /// </summary>
        /// <param name="control">for hiding the message in page header, it is Page; for others, it is the message span.</param>
        public static void HideMessage(System.Web.UI.Control control)
        {
            string controlClientID = (control is System.Web.UI.Page) ? "messageSpan" : control.ClientID;

            RegisterScript(control, string.Format("hideMessage('{0}');", controlClientID));
        }

        /// <summary>
        /// Hide the message.
        /// </summary>
        /// <param name="control">for hiding the message in page header, it is Page; for others, it is the message span.</param>
        public static void HideMessageByControl(System.Web.UI.Control control)
        {
            HideMessage(control);
        }

        /// <summary>
        /// Show a message in control 
        /// </summary>
        /// <param name="control">the message display in it.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be showed.</param>
        public static void ShowMessageByControl(System.Web.UI.Control control, MessageType msgType, string msg)
        {
            StringBuilder sbScript = new StringBuilder();
            string controlClientID = (control is System.Web.UI.Page) ? "messageSpan" : control.ClientID;

            string patt = "showMessage('{0}','{1}','{2}',true, 1);";
            sbScript.AppendFormat(patt, controlClientID, FilterQuotation(msg), msgType.ToString());

            RegisterScript(control, sbScript.ToString());
        }

        /// <summary>
        /// Delay show a message in control 
        /// </summary>
        /// <param name="control">the message display in it.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be showed.</param>
        public static void DelayShowMessageByControl(System.Web.UI.Control control, MessageType msgType, string msg)
        {
            StringBuilder sbScript = new StringBuilder();
            string controlClientID = (control is Page) ? "messageSpan" : control.ClientID;

            sbScript.AppendFormat(
                                "setTimeout(function () {{ showMessage('{0}','{1}','{2}',true, 1); }}, 500);", 
                                controlClientID, 
                                FilterQuotation(msg), 
                                msgType.ToString());

            RegisterScript(control, sbScript.ToString());
        }

        /// <summary>
        /// Show a message in control.
        /// </summary>
        /// <param name="control">the message display in it.</param>
        /// <param name="msgType">three type: error, notice, and success</param>       
        /// <param name="msg">which message need be showed.</param>
        /// <param name="canDelete">true - can be replace with other message. false - can't be replace with other message.</param>
        /// <param name="maxMsgCount">
        /// 1. less than 0: can add more message;
        /// 2. equal 0: clear all message(canDelete = true);
        /// 3. great than 0: it is the maximum number that message can be displayed.
        /// </param>
        public static void ShowMessageByControl(System.Web.UI.Control control, MessageType msgType, string msg, bool canDelete, int maxMsgCount)
        {
            StringBuilder sbScript = new StringBuilder();
            string controlClientID = (control is System.Web.UI.Page) ? "messageSpan" : control.ClientID;
            
            string patt = "showMessage('{0}','{1}','{2}',{3},{4});";
            sbScript.AppendFormat(patt, controlClientID, FilterQuotation(msg), msgType.ToString(), canDelete.ToString().ToLower(), maxMsgCount.ToString());

            RegisterScript(control, sbScript.ToString());
        }

        /// <summary>
        /// Show a message in page header.
        /// </summary>
        /// <param name="control">The control use to register the script.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be showed.</param>
        public static void ShowMessage(System.Web.UI.Control control, MessageType msgType, string msg)
        {
            StringBuilder sbScript = new StringBuilder();
            string controlClientID = (control is System.Web.UI.Page) ? "messageSpan" : control.ClientID;

            string patt = "showMessage('{0}', '{1}', '{2}', true, 1);";
            sbScript.AppendFormat(patt, controlClientID, FilterQuotation(msg), msgType.ToString());

            ShowMessage(control, sbScript.ToString());
        }

        /// <summary>
        /// show a message in control when the page is loading.
        /// </summary>
        /// <param name="control">the message display in it.</param>
        /// <param name="msgType">three type: error, notice, and success</param>      
        /// <param name="msg">which message need be showed.</param>
        /// <param name="canDelete">true - can be replace with other message. false - can't be replace with other message.</param>
        /// <param name="maxMsgCount">
        /// 1. less than 0: can add more message;
        /// 2. equal 0: clear all message(canDelete = true);
        /// 3. great than 0: it is the maximum number that message can be displayed.
        /// </param>
        public static void ShowMessage(System.Web.UI.Control control, MessageType msgType, string msg, bool canDelete, int maxMsgCount)
        {
            StringBuilder sbScript = new StringBuilder();
            string controlClientID = (control is System.Web.UI.Page) ? "messageSpan" : control.ClientID;

            string patt = "showMessage('{0}','{1}','{2}',{3},{4});";
            sbScript.AppendFormat(patt, controlClientID, FilterQuotation(msg), msgType.ToString(), canDelete.ToString().ToLower(), maxMsgCount.ToString());

            ShowMessage(control, sbScript.ToString());
        }

        /// <summary>
        /// Show a message in page header .
        /// </summary>
        /// <param name="page">Page use to register the script.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be showed.</param>
        public static void ShowMessageInParent(Page page, MessageType msgType, string msg)
        {
            StringBuilder sbScript = new StringBuilder();

            string patt = "parent.showMessage('messageSpan', '{0}', '{1}', true, 1);";
            sbScript.AppendFormat(patt, FilterQuotation(msg), msgType.ToString());

            RegisterScript(page, sbScript.ToString());
        }

        /// <summary>
        /// Show a message in page header .
        /// </summary>
        /// <param name="page">Page use to register the script.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be shown.</param>
        /// <param name="canDelete">
        /// true - The new messages will replace the existing message contents.
        /// false - The new messages will be append to the existing message contents.</param>
        /// <param name="maxMsgCount">
        /// 1. less than 0: can add more message;
        /// 2. equal 0: clear all message(canDelete = true);
        /// 3. great than 0: it is the maximum number that message can be displayed.
        /// </param>
        public static void ShowMessageInParent(Page page, MessageType msgType, string msg, bool canDelete, int maxMsgCount)
        {
            StringBuilder sbScript = new StringBuilder();
            string patt = "parent.showMessage('messageSpan', '{0}', '{1}', {2}, {3});";
            sbScript.AppendFormat(patt, FilterQuotation(msg), msgType.ToString(), canDelete.ToString().ToLower(), maxMsgCount.ToString());
            RegisterScript(page, sbScript.ToString());
        }

        /// <summary>
        /// Show a message in popup page header.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be showed.</param>
        public static void ShowMessageInPopup(System.Web.UI.Control control, MessageType msgType, string msg)
        {
            StringBuilder sbScript = new StringBuilder();
            string controlClientID = (control is System.Web.UI.Page) ? "messageSpan" : control.ClientID;

            string patt = "showMessage('{0}', '{1}', '{2}', true, 1, true);";
            sbScript.AppendFormat(patt, controlClientID, FilterQuotation(msg), msgType.ToString());

            string messageKey = "showMessage" + CommonUtil.GetRandomUniqueID().Substring(0, 6);

            ScriptManager.RegisterStartupScript(control, control.GetType(), messageKey, sbScript.ToString(), true);
        }

        /// <summary>
        /// Shows the message in popup scroll top.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="msgType">Type of the MSG.</param>
        /// <param name="msg">The MSG.</param>
        public static void ShowMessageInPopupScrollTop(System.Web.UI.Control control, MessageType msgType, string msg)
        {
            StringBuilder sbScript = new StringBuilder();

            string patt = "showMessage4Popup('{0}', '{1}');";
            sbScript.AppendFormat(patt, FilterQuotation(msg), msgType.ToString());

            string messageKey = "showMessage4Popup" + CommonUtil.GetRandomUniqueID().Substring(0, 6);

            ScriptManager.RegisterStartupScript(control, control.GetType(), messageKey, sbScript.ToString(), true);
        }

        /// <summary>
        /// Show a message in page header after page loaded.
        /// </summary>
        /// <param name="control">The control use to register the script.</param>
        /// <param name="msgType">three type: error, notice, and success</param>
        /// <param name="msg">which message need be showed.</param>
        public static void ShowMessageAfterPageLoad(System.Web.UI.Control control, MessageType msgType, string msg)
        {
            string controlClientID = (control is Page) ? "messageSpan" : control.ClientID;
            string script = string.Format(
                "$(document).ready(function(){{showMessage('{0}', '{1}', '{2}', true, 1);}});", 
                controlClientID,
                FilterQuotation(msg),
                msgType.ToString());

            ShowMessage(control, script);
        }

        /// <summary>
        /// Register scripts to web page.
        /// </summary>
        /// <param name="control">render scripts in the control</param>
        /// <param name="scripts">script contents.</param>
        private static void RegisterScript(System.Web.UI.Control control, string scripts)
        {
            string scriptKey = "message" + CommonUtil.GetRandomUniqueID().Substring(0, 6);
            ScriptManager.RegisterStartupScript(control, control.GetType(), scriptKey, scripts, true);
        }

        /// <summary>
        /// show message .
        /// </summary>
        /// <param name="control">show message in the control</param>
        /// <param name="message">message need be show</param>
        private static void ShowMessage(System.Web.UI.Control control, string message)
        {
            string messageKey = "showMessage" + CommonUtil.GetRandomUniqueID().Substring(0, 6);

            if (control is System.Web.UI.Page)
            {
                System.Web.UI.Page page = (System.Web.UI.Page)control;
                page.ClientScript.RegisterStartupScript(control.GetType(), messageKey, message, true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(control, control.GetType(), messageKey, message, true);
            }
        }

        #endregion Methods
    }
}
