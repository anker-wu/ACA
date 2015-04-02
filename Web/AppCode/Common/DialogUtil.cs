#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DialogUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *  File: DialogUtil.cs  *      $Id: .cs 142445 2009-08-07 08:19:22Z ACHIEVO\kale.huang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Some Common methods for popup dialog.
    /// </summary>
    public static class DialogUtil
    {
        /// <summary>
        /// Register related resource to a page.
        /// </summary>
        /// <param name="page">Represents an <c>.aspx</c> file, also known as a Web Forms page, requested from a server that hosts an ASP.NET Web application.</param>
        public static void RegisterScriptForDialog(Page page)
        {
            string scriptFile = FileUtil.AppendApplicationRoot("Scripts/dialog.js");   
            ScriptManager.RegisterClientScriptInclude(page, page.GetType(), "ACADialog", scriptFile);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogCloseImageUrl", "ACADialog.close_image_url='" + ImageUtil.GetImageURL("closepopup.png") + "';", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogSpacerImageUrl", "ACADialog.spacer_image_url='" + ImageUtil.GetImageURL("spacer.gif") + "';", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogBeginAlt", "ACADialog.begin_alt='" + LabelUtil.GetGUITextByKey("img_alt_form_begin").Replace("'", "\\'") + "';", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogEndAlt", "ACADialog.end_alt='" + LabelUtil.GetGUITextByKey("img_alt_form_end").Replace("'", "\\'") + "';", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogCloseImage", "ACADialog.close_alt='" + LabelUtil.GetGUITextByKey("aca_common_close").Replace("'", "\\'") + "';", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogIframeTitle", "ACADialog.iframe_title='" + LabelUtil.GetGUITextByKey("aca_dialog_title").Replace("'", "\\'") + "';", true);
            ScriptManager.RegisterStartupScript(page, page.GetType(), "ACADialogAgencyCode", "ACADialog.agencyCode='" + ConfigManager.AgencyCode + "';", true);
        }
    }
}
