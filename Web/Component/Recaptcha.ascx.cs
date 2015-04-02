/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Recaptcha.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 * 
 *  Description: 
 * 
 *  Notes:
 *      $Id: Recaptcha.ascx.cs 215354 2012-03-09 07:46:34Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Configuration;
using Accela.ACA.Common.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the RECAPTCHA class.
    /// </summary>
    public partial class Recaptcha : BaseUserControl
    {
        /// <summary>
        /// default PublicKey
        /// </summary>
        private const string DEFAULTPUBLICKEY = "6LeMLL8SAAAAAIYgHs0VrUWMqrtozUXmq7LnvvJQ";

        /// <summary>
        /// default PrivateKey
        /// </summary>
        private const string DEFAULTPRIVATEKEY = "6LeMLL8SAAAAAEC49ft9PTXvw7tSatH1CU9H8apm";

        /// <summary>
        /// RECAPTCHA validate method
        /// </summary>
        /// <returns>true or false</returns>
        public bool Validate()
        {
            bool isValid = recaptcha.IsValid;

            if (!isValid)
            {
                lblErrorMessage.Text = LabelUtil.GetTextByKey("aca_captcha_error_message", string.Empty);
            }

            return isValid;
        }

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            recaptcha.PublicKey = ConfigurationManager.AppSettings["RecaptchaPublicKey"] ?? DEFAULTPUBLICKEY;
            recaptcha.PrivateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"] ?? DEFAULTPRIVATEKEY;
            recaptcha.OverrideSecureMode = "https".Equals(ConfigManager.Protocol, StringComparison.OrdinalIgnoreCase);

            string tooltip = ScriptFilter.RemoveHTMLTag(GetTextByKey("aca_capcha_description"));
            recaptcha.ToolTip = tooltip;
            recaptcha_response_field.ToolTip = tooltip;
        }
    }
}
