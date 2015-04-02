#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchConditions.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchConditions.ascx.cs 130988 2009-8-20  18:05:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;

using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.GlobalSearch;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Global search condition control
    /// </summary>
    public partial class GlobalSearchConditions : BaseUserControl
    {
        /// <summary>
        /// Page OnLoad event
        /// </summary>
        /// <param name="sender">the event object</param>
        /// <param name="e">the event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin && GlobalSearchUtil.IsGlobalSearchEnabled() && Visible)
            {
                string script = GlobalSearchManager.GetHistoryScript();
                if (!Page.ClientScript.IsStartupScriptRegistered(GetType(), "global_search"))
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "global_search", script, true);
                }
            }

            if (!Page.IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    lblSearchCondition.Visible = true;
                }
                else
                {
                    lblSearchCondition.Visible = false;
                }
            }
        }

        /// <summary>
        /// Get watermark
        /// </summary>
        /// <returns>watermark label</returns>
        protected string GetWaterMark()
        {
            string waterMark = GetTextByKey("per_globalsearch_label_search");

            return LabelUtil.RemoveHtmlFormat(waterMark).Replace("'", "&#39;").Replace("\"", "&quot;");
        }
    }
}