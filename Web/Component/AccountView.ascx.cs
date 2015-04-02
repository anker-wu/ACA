#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AccountView.ascx.cs 278148 2014-08-28 07:30:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Text;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AccountView
    /// </summary>
    public partial class AccountView : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The security questions string builder.
        /// </summary>
        private readonly StringBuilder _securityQuestiontBuilder = new StringBuilder();

        /// <summary>
        /// Gets the security question builder.
        /// </summary>
        /// <value>The security question builder.</value>
        protected StringBuilder SecurityQuestionBuilder
        {
            get
            {
                return _securityQuestiontBuilder;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Display the account information
        /// </summary>
        /// <param name="model">Account model for displaying</param>
        public void Display(PublicUserModel4WS model)
        {
            if (model != null)
            {
                lblUser.Text = model.userID;
                lblEmail.Text = model.email;

                /* 
                 * If current authentication isn't internal authentication adapter or the LDAP authentication is enabled,
                 * the password and security question information will be hidden.
                 */
                if (!AuthenticationUtil.IsInternalAuthAdapter || StandardChoiceUtil.IsEnableLdapAuthentication())
                {
                    passwordInfo.Visible = false;
                }
                else
                {
                    passwordInfo.Visible = true;
                    lblPassword.Text = "******";
                    string questionLabel = GetTextByKey("acc_userInfoDisplay_label_securityQuestion");

                    if (model.questions != null && model.questions.Length != 0)
                    {
                        foreach (PublicUserQuestionModel item in model.questions.OrderBy(f => f.sortOrder))
                        {
                            _securityQuestiontBuilder.AppendFormat(
                                "<tr><td class='ACA_FLeft ACA_Page ACA_MLonger'>{0}</td><td class='ACA_FLeft ACA_Page'>{1}</td></tr>",
                                questionLabel,
                                ScriptFilter.EncodeHtml(item.questionValue));
                        }
                    }
                    else if (!AppSession.IsAdmin)
                    {
                        _securityQuestiontBuilder.AppendFormat(
                            "<tr><td class='ACA_FLeft ACA_Page ACA_MLonger'>{0}</td><td class='ACA_FLeft ACA_Page'></td></tr>",
                            questionLabel);
                    }
                }

                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS();
                permission.permissionLevel = GViewConstant.PERMISSION_USERREGISTRATION;

                SimpleViewElementModel4WS[] fields = gviewBll.GetSimpleViewElementModel(ModuleName, permission, GviewID.UserRegistration, AppSession.User.PublicUserId);

                if (fields.Count(f => string.Equals(f.viewElementName, "txbMobilePhone", StringComparison.InvariantCultureIgnoreCase) && ACAConstant.VALID_STATUS.Equals(f.recStatus, StringComparison.InvariantCultureIgnoreCase)) == 0)
                {
                    mobile.Visible = false;
                }
                else
                {
                    lblCellPhone.Text = ModelUIFormat.FormatPhoneShow(model.cellPhoneCountryCode, model.cellPhone, StandardChoiceUtil.GetDefaultCountry());
                }

                if (fields.Count(f => string.Equals(f.viewElementName, "cbReceiveSMS", StringComparison.InvariantCultureIgnoreCase) && ACAConstant.VALID_STATUS.Equals(f.recStatus, StringComparison.InvariantCultureIgnoreCase)) == 0)
                {
                    receiveSMS.Visible = false;
                }
                else
                {
                    lblReceiveSMS.Text = ValidationUtil.IsYes(model.receiveSMS) ? ACAConstant.COMMON_YES : ACAConstant.COMMON_NO;
                }
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && AppSession.IsAdmin)
            {
                secQuestionInfo.Visible = true;
            }
        }

        /// <summary>
        /// Post back this page.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void PostbackButton_Click(object sender, EventArgs e)
        {
            Display(AppSession.User.UserModel4WS);
        }

        #endregion Methods
    }
}