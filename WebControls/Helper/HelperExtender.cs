#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: HelperExtender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: HelperExtender.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Globalization;
using System.Web.UI;
using Accela.Web.Controls;
using AjaxControlToolkit;

[assembly: WebResource("Accela.Web.Controls.Helper.help_icon.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-bl.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-bm.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-br.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-ml.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-mr.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-pl.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-pr.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-tl.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-tm.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-bg-tr.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.help-close.png", "image/png")]
[assembly: WebResource("Accela.Web.Controls.Helper.helper.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("Accela.Web.Controls.Helper.HelperBehavior.js", "application/x-javascript", PerformSubstitution = true)]

namespace AccelaWebControlExtender
{
    /// <summary>
    /// Class HelperExtender.
    /// </summary>
    [TargetControlType(typeof(Accela.Web.Controls.IAccelaNonInputControl))]
    [TargetControlType(typeof(System.Web.UI.HtmlControls.HtmlInputControl))]
    [ClientCssResource("Accela.Web.Controls.Helper.helper.css")]
    [ClientScriptResource("AccelaWebControlExtender.HelperBehavior", "Accela.Web.Controls.Helper.HelperBehavior.js")]
    [RequiredScript(typeof(CommonToolkitScripts))]
    public class HelperExtender : ExtenderControlBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Is right to left
        /// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("isRTL")]
        public virtual bool IsRTL
        {
            get
            {
                bool isRTL = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
                return GetPropertyValue("IsRTL", isRTL);
            }

            set
            {
                SetPropertyValue("IsRTL", value);
            }
        }

        /// <summary>
        /// Gets or sets Help window title
        /// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("title")]
        public virtual string Title
        {
            get
            {
                string title = GetPropertyValue("Title", LabelConvertUtil.GetDefaultLanguageGlobalTextByKey("aca_field_helpwindow_title"));
                if (!string.IsNullOrEmpty(title))
                {
                    return title;
                }
                else
                {
                    return "Help";
                }
            }

            set
            {
                SetPropertyValue("Title", value);
            }
        }

        /// <summary>
        /// Gets or sets Close Title.
        /// </summary>
        [ExtenderControlProperty]
        [ClientPropertyName("closeTitle")]
        public virtual string CloseTitle
        {
            get
            {
                return GetPropertyValue("closeTitle", LabelConvertUtil.GetGUITextByKey("aca_common_close"));
            }

            set
            {
                SetPropertyValue("closeTitle", value);
            }
        }

        #endregion
    }
}