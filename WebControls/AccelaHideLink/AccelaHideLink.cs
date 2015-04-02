#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaHideLink.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaHideLink.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Common;

[assembly: WebResource("Accela.Web.Controls.AccelaHideLink.onepixel.gif", "image/gif")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a class for define hide link.
    /// </summary>
    public class AccelaHideLink : WebControl
    {
        /// <summary>
        /// browser name IE.
        /// </summary>
        private const string BROWSER_NAME_SAFARI = "Safari";

        /// <summary>
        /// Gets or sets label key for alt.
        /// </summary>
        public string AltKey
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether current link is skipping to an anchor.
        /// </summary>
        /// <value>
        /// <c>true</c> if current link is skipping to an anchor; otherwise, <c>false</c>.
        /// </value>
        public bool IsSkippingToAnchor
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is skipping to parent control.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is skipping to parent control; otherwise, <c>false</c>.
        /// </value>
        public bool IsSkippingToParentControl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the hide link text.
        /// </summary>
        /// <value>The hide link text.</value>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets script string for client click event
        /// </summary>
        public string OnClientClick
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets focus next control id.
        /// </summary>
        public string NextControlID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets focus next control id.
        /// </summary>
        public string NextControlClientID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets script string for On Key Down
        /// </summary>
        public string OnKeyDown
        {
            get;
            set;
        }

        /// <summary>
        /// render element "a" to web.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            int imgWidthOrHeight = this.Page.Request.UserAgent.Contains(BROWSER_NAME_SAFARI) ? 1 : 0;
            string altKey = string.IsNullOrEmpty(AltKey) ? string.Empty : AltKey;
            string src = Page.ClientScript.GetWebResourceUrl(typeof(AccelaHideLink), "Accela.Web.Controls.AccelaHideLink.onepixel.gif");
            string tabIndexString = this.TabIndex != 0 ? string.Concat("tabIndex=\"", this.TabIndex.ToString(), "\"") : string.Empty;
            string hrefValue = IsSkippingToAnchor ? "#" + this.GetClientID() : "#";
            string defaultOnClientClickValue = string.Format("if(typeof(SetNotAsk)!='undefined') SetNotAsk();var obj = document.getElementById('{0}'); if (obj) obj.focus();", this.GetClientID());
            string onClientClickString = !string.IsNullOrEmpty(this.OnClientClick) ? string.Format("onclick=\"{0}\"", this.OnClientClick) : IsSkippingToAnchor ? string.Empty : string.Format("onclick=\"{0}\"", defaultOnClientClickValue);
            string onKeyDown = !string.IsNullOrEmpty(OnKeyDown) ? string.Format("onkeydown=\"{0}\"", OnKeyDown) : string.Empty;

            StringBuilder sbHtml = new StringBuilder();
            string altText = string.IsNullOrEmpty(this.Text) ? LabelConvertUtil.GetGlobalTextByKey(altKey) : this.Text;
            string titleText = ScriptFilter.AntiXssHtmlEncode(LabelConvertUtil.RemoveHtmlFormat(altText));

            sbHtml.AppendFormat("<DIV style=\"WIDTH: {0}px; HEIGHT: {0}px;\" id=\"div{1}\" class=\"{2}\">", imgWidthOrHeight, ClientID, CssClass);
            sbHtml.AppendFormat("<a id=\"{0}\" accesskey=\"{1}\" href=\"{2}\" {3} {4} title=\"{5}\" {6} class=\"NotShowLoading\">", ClientID, this.AccessKey, hrefValue, tabIndexString, onClientClickString, titleText, onKeyDown);
            sbHtml.AppendFormat("<img alt=\"{0}\" src=\"{1}\" width='{2}px' height='{2}px' style='border-width:0px;'/></a>", altText, src, imgWidthOrHeight);
            sbHtml.Append("</DIV>");

            writer.Write(sbHtml.ToString());
        }

        /// <summary>
        /// Get focus next control client id.
        /// </summary>
        /// <returns>the client ID</returns>
        private string GetClientID()
        {
            //At first, to get focus control client ID, it is the height Level.
            if (!string.IsNullOrEmpty(NextControlClientID))
            {
                return NextControlClientID;
            }
            else
            {
                string prefix = string.Empty;

                //if next control is in parent page.
                if (IsSkippingToParentControl)
                {
                    if (Parent != null && Parent.Parent != null)
                    {
                        prefix = Parent.Parent.ClientID;
                    }

                    return string.Format("{0}_{1}", prefix, NextControlID);
                }
                else
                {
                    //the focus next control in local page.
                    prefix = ClientID.Replace(ID, string.Empty);
                    return string.Format("{0}{1}", prefix, NextControlID);
                }
            }
        }
    }
}
