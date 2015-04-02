#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RegistrationSetting.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RegistrationSetting.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common.Util;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Admin
{
    /// <summary>
    /// Register setting page
    /// </summary>
    public partial class RegistrationSetting : AdminBasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (MultiLanguageSupportEnable())
                {
                    hdfFlag.Value = "Yes";
                }

                chkIntervalActivate.Attributes.Add("onclick", "EnableInterval();");
                chkAddLicense.Attributes.Add("onclick", "UpdateDataInfo();");
                chkRemoveLicense.Attributes.Add("onclick", "EnableInterval();");
                txtIntervalDay.Attributes.Add("onchange", "UpdateDataInfo();");
                chkPasswordExpiration.Attributes.Add("onclick", "EnablePasswordExpriation();");
                chkPasswordFailedAttempts.Attributes.Add("onclick", "EnablePasswordFailedAttempts();");
                txtPasswordExpirationDays.Attributes.Add("onchange", "UpdateDataInfo();");
                txtPasswordFailedTimes.Attributes.Add("onchange", "UpdateDataInfo();");
                txtPasswordFailedDurations.Attributes.Add("onchange", "UpdateDataInfo();");
                chkEnableLoginOnRegistration.Attributes.Add("onclick", "EnableLoginExpriation();");
                txtLoginExpireTime.Attributes.Add("onchange", "UpdateDataInfo();");
                chkEnableResetPassordOnCombine.Attributes.Add("onclick", "UpdateDataInfo();");

                AccelaCheckBox chkRL = updatePanel1.FindControl("chkRequireLicense") as AccelaCheckBox;

                if (chkRL != null)
                {
                    chkRL.Attributes.Add("onclick", "UpdateDataInfo();");
                }
                
                chkEnableRecaptchaForRegistration.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableRecaptchaForLogin.Attributes.Add("onclick", "UpdateDataInfo();");

                chkEnableAutoActivation.Attributes.Add("onclick", "UpdateDataInfo();");

                Page.ClientScript.RegisterStartupScript(this.GetType(), "getRegistrationDataInfo", "<script type='text/javascript'>GetRegistrationDataInfo();</script>");
                txtLoginExpireTime.ToolTip = GetTextByKey("acaadmin_registrationsetting_label_loginexpiretime");

                chkSecurityQuestion.Attributes.Add("onclick", "EnableSecurityQuestionQuantity();");
                txtSecurityQuestionQuantity.Attributes.Add("onchange", "UpdateDataInfo();");
                txtSecurityQuestionQuantity.ToolTip = GetTextByKey("acaadmin_registrationsetting_label_authbysecurityquestion_tooltip");
                Page.ClientScript.RegisterStartupScript(GetType(), "getAuthenticationBySecurityQuestionInfo", "GetAuthBySecurityQuestionInfo();", true);
            }
        }

        /// <summary>
        /// Check whether multiple language is supported or not
        /// </summary>
        /// <returns>true if multiple language is supported; otherwise, false.</returns>
        private bool MultiLanguageSupportEnable()
        {
            return I18nCultureUtil.IsMultiLanguageEnabled;
        }

        #endregion
    }
}