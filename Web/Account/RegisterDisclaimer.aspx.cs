#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RegisterDisclaimer.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RegisterDisclaimer.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.NewUI;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// the class for RegisterDisclaimer.
    /// </summary>
    public partial class RegisterDisclaimer : BasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check user whether can enter the registration process.
            AccountUtil.CheckRegistrationPermission();

            AppSession.SetContactSessionParameter(null);
            AppSession.SetRegisterContactSessionParameter(null);

            termAccept.Attributes["title"] = LabelUtil.RemoveHtmlFormat(GetTextByKey("acc_regHome_label_acceptTerms"));
        }

        /// <summary>
        /// Register button click event.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs e.</param>
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            bool isRegisterLPAccount = StandardChoiceUtil.IsRequiredLicense();
            string url = isRegisterLPAccount ? "RegisterLicense.aspx?isRegisterLPAccount=Y" : "RegisterEdit.aspx";
            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                url = NewUiUtil.GetUrlByResource(url);
            }

            UrlHelper.KeepReturnUrlAndRedirect(url);
        }

        #endregion Methods
    }
}