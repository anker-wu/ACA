#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaRecaptcha.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaRecaptcha.cs 271255 2014-05-30 09:51:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide a RECAPTCHA.
    /// </summary>
    public class AccelaRecaptcha : Recaptcha.RecaptchaControl
    {
        /// <summary>
        /// Render html text
        /// </summary>
        /// <param name="writer">html text writer</param>
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter tw = new StringWriter(sb);
            HtmlTextWriter output = new HtmlTextWriter(tw);
            base.Render(output);
            string outputStr = sb.ToString();

            // Add title and content for iframe between <noscript> and </noscript>.
            outputStr = Regex.Replace(
                outputStr,
                @"\<iframe([^\>\<]+?)(\>{1}?)",
                string.Format("<iframe$1 title='{0}'$2{1}", ToolTip, LabelConvertUtil.GetTextByKey("iframe_nonsrc_nonsupport_message", this)),
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);

            // Add title for textarea between <noscript> and </noscript>.
            outputStr = Regex.Replace(
                outputStr,
                @"\<textarea([^\>]+?)\>",
                string.Format("<textarea$1 title=\"{0}\">", LabelConvertUtil.GetTextByKey("aca_recaptcha_label_challenge_field|tip", this)),
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);

            writer.Write(outputStr);
        }
    }
}
