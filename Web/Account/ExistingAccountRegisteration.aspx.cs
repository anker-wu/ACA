#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExistingAccountRegisteration.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ExistingAccountRegisteration.aspx.cs 269220 2014-05-26 10:03:58Z ACHIEVO\joy.duan $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.NewUI;
using Accela.ACA.Web.Util;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Page to forward to existing account registration procedure.
    /// </summary>
    public partial class ExistingAccountRegisteration : BasePage
    {
        #region Methods
        
        /// <summary>
        /// Cancel button click event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        /// <summary>
        /// Register button click event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            AppSession.SetContactSessionParameter(null);
            AppSession.SetRegisterContactSessionParameter(null);

            bool isLicenseRequired = StandardChoiceUtil.IsRequiredLicense();
            string url = string.Empty;
            string existingAccountRegisterationUserID = HttpUtility.UrlEncode(Request.QueryString[UrlConstant.USER_ID_OR_EMAIL]);

            if (isLicenseRequired)
            {
                url = string.Format(
                                    "~/Account/LicenseList.aspx?{0}={1}&{2}={3}&{4}={5}",
                                    UrlConstant.IS_REGISTER_LP_ACCOUNT,
                                    ACAConstant.COMMON_Y,
                                    UrlConstant.USER_ID_OR_EMAIL,
                                    existingAccountRegisterationUserID,
                                    UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                    ACAConstant.COMMON_Y);
            }
            else
            {
                url = string.Format(
                                    "~/Account/RegisterEdit.aspx?{0}={1}&{2}={3}",
                                    UrlConstant.USER_ID_OR_EMAIL,
                                    existingAccountRegisterationUserID,
                                    UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                    ACAConstant.COMMON_Y);
            }

            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                url = NewUiUtil.GetUrlByResource(url);
            }
            
            Response.Redirect(url);
        }

        #endregion
    }
}