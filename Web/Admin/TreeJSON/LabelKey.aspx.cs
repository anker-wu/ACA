#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LabelKey.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2010
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LabelKey.aspx.cs 184803 2010-11-18 08:29:14Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Admin
{
    /// <summary>
    /// Get label key for tree nodes
    /// </summary>
    public partial class JSONLabelKey : System.Web.UI.Page
    {
        #region Methods

        /// <summary>
        /// Get text by label key
        /// </summary>
        /// <param name="key">label key.</param>
        /// <returns>text of the key</returns>
        public string GetLabelKey(string key)
        {
            return LabelUtil.GetAdminUITextByKey(key);
        }

        /// <summary>
        /// Get I18N text by label key
        /// </summary>
        /// <param name="key">label key.</param>
        /// <returns>text of the key.</returns>
        public string GetTextByKey(string key)
        {
            return LabelUtil.GetGlobalTextByKey(key);
        }

        /// <summary>
        /// Get text in default language by label key.
        /// </summary>
        /// <param name="key">label key.</param>
        /// <returns>text for default language.</returns>
        public string GetDefaultLanguageTextByKey(string key)
        {
            return LabelUtil.GetDefaultLanguageGlobalTextByKey(key);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                Response.Redirect("../login.aspx");
            }
        }

        #endregion Methods
    }
}