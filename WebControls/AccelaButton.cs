#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:This class is used for button display.
 *
 *  Notes:
 * $Id: AccelaButton.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using AccelaWebControlExtender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide a button
    /// </summary>
    public class AccelaButton : AccelaLinkButton
    {
        #region Fields 

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the button can configure URL in ACA Admin
        /// </summary>
        public string DivEnableCss
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the button can configure URL in ACA Admin
        /// </summary>
        public string DivDisableCss
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Displays the link button on the client.
        /// </summary>
        /// <param name="w">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter w)
        {
            if (DesignMode)
            {
                return;
            }

            foreach (Control control in Controls)
            {
                if (control is AccelaWebControlExtenderExtender || control is HelperExtender)
                {
                    control.RenderControl(w);
                    break;
                }
            }

            if (!string.IsNullOrEmpty(DivEnableCss) || !string.IsNullOrEmpty(DivDisableCss))
            {
                w.Write("<div id='" + this.ClientID + this.ClientIDSeparator + "container" + "'>");
            }

            if (Enabled)
            {
                if (!string.IsNullOrEmpty(DivEnableCss))
                {
                    w.Write("<div class='" + DivEnableCss + "'>");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(DivDisableCss))
                {
                    w.Write("<div class='" + DivDisableCss + "'>");
                }
            }

            RenderBeginTag(w);

            if (string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(LabelKey))
            {
                string text = LabelConvertUtil.GetTextByKey(LabelKey, GetModuleName(), this);

                if (text == LabelConvertUtil.GetGlobalTextByKey("aca_sys_default_module_name") && !string.IsNullOrEmpty(ModuleName))
                {
                    text = ModuleName;
                }

                Text = "<span>" + text + "</span>";
            }
            else if (!string.IsNullOrEmpty(Text))
            {
                Text = "<span>" + Text + "</span>";
            }

            RenderContents(w); 
            RenderEndTag(w);

            if (!string.IsNullOrEmpty(DivEnableCss) || !string.IsNullOrEmpty(DivDisableCss))
            {
                w.Write("</div></div>");
            }
        }

        #endregion Methods
    }
}