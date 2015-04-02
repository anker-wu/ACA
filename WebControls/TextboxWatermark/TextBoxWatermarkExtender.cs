/**
* <pre>
* 
*  Accela Citizen Access
*  File: TextBoxWatermarkExtender.cs
* 
*  Accela, Inc.
*  Copyright (C): 2010-2014
* 
*  Description:
* 
*  Notes:
* $Id: TextBoxWatermarkExtender.cs 171222 2010-04-23 17:32:00Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: WebResource("Accela.Web.Controls.TextboxWatermark.TextboxWatermark.ico", "image/x-icon")]
[assembly: WebResource("Accela.Web.Controls.TextboxWatermark.TextboxWatermark.js", "text/javascript")]

namespace AjaxControlToolkit
{
    /// <summary>
    /// Class TextBoxWatermarkExtender.
    /// </summary>
    [ClientScriptResource("AjaxControlToolkit.TextBoxWatermarkBehavior", "Accela.Web.Controls.TextboxWatermark.TextboxWatermark.js"), 
    TargetControlType(typeof(TextBox)), 
    Designer("AjaxControlToolkit.TextBoxWatermarkExtenderDesigner, AjaxControlToolkit"), 
    RequiredScript(typeof(CommonToolkitScripts)),
    ToolboxBitmap(typeof(TextBoxWatermarkExtender), "Accela.Web.Controls.TextboxWatermark.TextboxWatermark.ico")]
    public class TextBoxWatermarkExtender : ExtenderControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxWatermarkExtender"/> class.
        /// </summary>
        public TextBoxWatermarkExtender()
        {
            EnableClientState = true;
        }

        /// <summary>
        /// Gets or sets the watermark CSS class.
        /// </summary>
        /// <value>The watermark CSS class.</value>
        [DefaultValue(""), ExtenderControlProperty]
        public string WatermarkCssClass
        {
            get
            {
                return GetPropertyValue<string>("WatermarkCssClass", string.Empty);
            }

            set
            {
                SetPropertyValue<string>("WatermarkCssClass", value);
            }
        }

        /// <summary>
        /// Gets or sets the watermark text.
        /// </summary>
        /// <value>The watermark text.</value>
        [RequiredProperty, ExtenderControlProperty, DefaultValue("")]
        public string WatermarkText
        {
            get
            {
                return GetPropertyValue<string>("WatermarkText", string.Empty);
            }

            set
            {
                SetPropertyValue<string>("WatermarkText", value);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ScriptManager.RegisterOnSubmitStatement(this, typeof(TextBoxWatermarkExtender), "TextBoxWatermarkExtenderOnSubmit", "null;");
            ClientState = string.Compare(this.Page.Form.DefaultFocus, TargetControlID, StringComparison.OrdinalIgnoreCase) == 0 ? "Focused" : null;
        }
    }
}