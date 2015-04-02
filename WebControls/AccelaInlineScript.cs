#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaInlineScript.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaInlineScript.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Text;
using System.Web.UI;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Accela inline script, used to contain inline script
    /// </summary>
    public class AccelaInlineScript : Control, IAccelaBaseControl
    {
        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"/> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the server control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                ScriptManager sm = ScriptManager.GetCurrent(Page);

                if (sm != null && sm.IsInAsyncPostBack)
                {
                    StringBuilder sb = new StringBuilder();
                    base.Render(new HtmlTextWriter(new StringWriter(sb)));
                    string script = sb.ToString();

                    if (!string.IsNullOrEmpty(script))
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(AccelaInlineScript), UniqueID, script, false);
                    }
                }
                else
                {
                    base.Render(writer);
                }
            }
        }
    }
}
