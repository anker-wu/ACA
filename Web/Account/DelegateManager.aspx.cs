#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DelegateManager.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DelegateManager.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services;

using Accela.ACA.BLL.ProxyUser;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// the delegate manger page.
    /// </summary>
    public partial class DelegateManager : PopupDialogBasePage
    {        
        /// <summary>
        /// Gets or sets a value the button status.
        /// </summary>
        private List<PublicUserModel4WS> ProxyUserList
        {
            get
            {
                if (ViewState["ProxyUserList"] == null)
                {
                    return null;
                }

                return (List<PublicUserModel4WS>)ViewState["ProxyUserList"];
            }

            set
            {
                ViewState["ProxyUserList"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value the button status.
        /// </summary>
        private List<PublicUserModel4WS> InitUserList
        {
            get
            {
                if (ViewState["InitUserList"] == null)
                {
                    return null;
                }

                return (List<PublicUserModel4WS>)ViewState["InitUserList"];
            }

            set
            {
                ViewState["InitUserList"] = value;
            }
        }

        /// <summary>
        /// Check unique Email address from Javascript of AccountEdit
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Validation Information</returns>
        [WebMethod(Description = "is existed email", EnableSession = true)]
        public static string ValidateProxyUserEmail(string emailAddress)
        {
            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;

            string agencyCode = ConfigManager.AgencyCode;
            string results = string.Empty;

            if (!Regex.IsMatch(emailAddress, I18nEmailUtil.EmailValidationExpression))
            {
                return LabelUtil.GetGlobalTextByKey("ACA_AccelaEmailText_ErrorMessage").Replace("'", "\\'");
            }

            PublicUserModel4WS model = AppSession.User.UserModel4WS;

            if (model.email.Equals(emailAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                return LabelUtil.GetGlobalTextByKey("aca_delegate_email_not_self").Replace("'", "\\'");
            }

            string existedEmailID = proxyUserBll.ValidateEmail(emailAddress, AppSession.User.PublicUserId);

            return existedEmailID.Replace("'", "\\'");
        }

        /// <summary>
        /// page load method
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handel.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            delegateManagement.EditPageTitleDelegateCommand += new CommonEventHandler(UpdatePageTitle);

            if (!IsPostBack)
            {
                InitDataSource();

                string pageType = Request.QueryString["proxyUserPageType"] == null ? string.Empty : Request.QueryString["proxyUserPageType"].ToString();
                string userSeqNum = Request.QueryString["userSeqNum"] == null ? string.Empty : Request.QueryString["userSeqNum"].ToString();
                string isProxyUser = Request.QueryString["isProxyUser"] == null ? string.Empty : Request.QueryString["isProxyUser"].ToString();
                delegateManagement.DelegateUserSeqNum = userSeqNum;

                switch (pageType)
                {
                    case ACAConstant.COMMON_ZERO:
                        delegateManagement.PageType = ProxyUserPageType.Create;

                        break;
                    case ACAConstant.COMMON_ONE:
                        delegateManagement.PageType = ProxyUserPageType.Edit;
                        delegateManagement.ProxyUserDataSource = isProxyUser == ACAConstant.COMMON_ONE ? ProxyUserList.Where(d => d.userSeqNum == userSeqNum).Single() : InitUserList.Where(d => d.userSeqNum == userSeqNum).Single();
                        break;
                    default:
                        delegateManagement.PageType = ProxyUserPageType.View;
                        delegateManagement.ProxyUserDataSource = isProxyUser == ACAConstant.COMMON_ONE ? ProxyUserList.Where(d => d.userSeqNum == userSeqNum).Single() : InitUserList.Where(d => d.userSeqNum == userSeqNum).Single();
                        break;
                }

                delegateManagement.BindData();
            }
        }

        /// <summary>
        /// update delegate event.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="arg">the argument.</param>
        private void UpdatePageTitle(object sender, CommonEventArgs arg)
        {
            SetPageTitleKey((string)arg.ArgObject);
        }

        /// <summary>
        /// initialize data source.
        /// </summary>
        private void InitDataSource()
        {
            List<PublicUserModel4WS> userList = new List<PublicUserModel4WS>();

            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;
            PublicUserModel4WS user = new PublicUserModel4WS();

            if (!AppSession.IsAdmin)
            {
                user = proxyUserBll.GetProxyUsers(ConfigManager.AgencyCode, long.Parse(AppSession.User.UserSeqNum));
            }

            if (user != null && user.proxyUsers != null && user.proxyUsers.Length > 0)
            {
                ProxyUserList = user.proxyUsers.ToList();
            }
            else
            {
                ProxyUserList = null;
            }

            if (user != null && user.initialUsers != null && user.initialUsers.Length > 0)
            {
                InitUserList = user.initialUsers.ToList();
            }
            else
            {
                InitUserList = null;
            }
        }
    }
}
