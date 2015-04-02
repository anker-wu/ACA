#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaHeightSeparate.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaHeightSeparate.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a class for define separate div.
    /// </summary>
    public class AccelaHeightSeparate : Control
    {
        /// <summary>
        /// Gets or sets height.
        /// </summary>
        public int Height
        {
            get; 
            set;
        }

        /// <summary>
        /// Displays the div on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            string style = string.Format("height:{0}px;clear:both;width:100%;", Height);
            writer.Write("<div id=\"{0}\" style=\"{1}\">", ClientID, style);
            writer.Write("&nbsp;");
            writer.Write("</div>");
        }
    }
}
