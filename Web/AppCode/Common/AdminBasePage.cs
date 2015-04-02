#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminBasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AdminBasePage.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Admin BasePage, all of admin page should inherit this class.
    /// </summary>
    public class AdminBasePage : Page, IPage
    {
        #region Properties

        /// <summary>
        /// Gets admin root, ends with /
        /// </summary>
        public static string AdminRoot
        {
            get
            {
                return FileUtil.ApplicationRoot + "Admin/";
            }
        }

        /// <summary>
        /// Gets a value indicating whether the controls in current page whether need to be rendered as admin mode.
        /// true - All controls will be rendered as daily mode.
        /// false- All controls will be rendered as admin mode
        /// </summary>
        public bool IsControlRenderAsAdmin
        {
            get
            {
                // in order to present the web control as daily mode, it should return false.
                return false;
            }
        }

        /// <summary>
        /// Gets current page's element id
        /// </summary>
        public string PageID
        {
            get
            {
                return string.Empty;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        public string GetTextByKey(string key)
        {
            return LabelUtil.GetAdminUITextByKey(key);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        public string GetTextByKey(string key, string moduleName)
        {
            return GetTextByKey(key);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        public string GetSuperAgencyTextByKey(string key)
        {
            return LabelUtil.GetSuperAgencyTextByKey(key, string.Empty);
        }

        /// <summary>
        /// Construct title value with image alt value with blank. 
        /// </summary>
        /// <param name="alt">the label key</param>
        /// <param name="title">the label key after encode.</param>
        /// <returns>the title value</returns>
        public string GetTitleByKey(string alt, string title)
        {
            string[] newTitle = { GetTextByKey(alt), GetTextByKey(title) };
            return ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(DataUtil.ConcatStringWithSplitChar(newTitle, ACAConstant.BLANK)));
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module,returns String.Empty.
        /// </summary>
        /// <returns>module name.</returns>
        string IPage.GetModuleName()
        {
            // in admin mode, the module name is disabled.
            return string.Empty;
        }

        /// <summary>
        /// Change theme
        /// </summary>
        protected void ChangeTheme()
        {
            Page.Theme = "Default";
        }

        /// <summary>
        /// Raise <c>OnPreInit</c> event
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            ProductLicenseValidation.ValidateProductLicense(true);

            if (!AppSession.IsAdmin && string.IsNullOrEmpty(Request["sessionid"]))
            {
                Response.Redirect(AdminRoot + "login.aspx");
            }

            ChangeTheme();
        }

        #endregion Methods
    }
}