#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DelegateUsersView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DelegateUsersView.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.People;
using Accela.ACA.BLL.ProxyUser;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation UserLicenseView.
    /// </summary>
    public partial class DelegateUsersView : BaseUserControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the button status is disabled.
        /// </summary>
        private bool BtnStatus
        {
            get
            {
                if (ViewState["BtnStatus"] == null)
                {
                    return true;
                }

                return (bool)ViewState["BtnStatus"];
            }

            set
            {
                ViewState["BtnStatus"] = value;
            }
        }

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
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                divHiddenInitExpiredItems.Visible = true;
                divHiddenRejectProxyItems.Visible = true;
            }

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    btnAddDelegateUser.Attributes["onclick"] = "showPopupDialog('DelegateManager.aspx?proxyUserPageType=0','" + LabelUtil.GetTextByKey("aca_add_delegate", string.Empty) + "',this.id);";
                }

                hdnShowRejectExpiredProxyUsers.Value = ACAConstant.COMMON_ZERO;
                hdnShowRejectExpiredInitUsers.Value = ACAConstant.COMMON_ZERO;
                InitDataSource();
                BindAvailableData();
            }
        }

        /// <summary>
        /// Post back page.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void PostbackButton_Click(object sender, EventArgs e)
        {
            InitDataSource();
            BindAvailableData();
        }
        
        /// <summary>
        /// Update Delegate.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object.</param>
        protected void UpdateDelegate(object sender, CommonEventArgs arg)
        {
            InitDataSource();
            BindAvailableData();
        } 

        /// <summary>
        /// Bind Address to UI.
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">the data list event args.</param>
        protected void MyDelegateUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            PublicUserModel4WS delegateUser = (PublicUserModel4WS)e.Item.DataItem;
            LinkButton btnViewInvitation = (LinkButton)e.Item.FindControl("btnViewInvitation");
            AccelaLabel lblProxyUserActionDate = (AccelaLabel)e.Item.FindControl("lblProxyUserActionDate");
            AccelaLabel lblUserName = (AccelaLabel)e.Item.FindControl("lblUserName");
            HtmlContainerControl divDelegateUserViewInvition = (HtmlContainerControl)e.Item.FindControl("divDelegateUserViewInvition");
            lblUserName.Text = ScriptFilter.EncodeHtml(LabelUtil.BreakWord(30, delegateUser.proxyUserModel.nickName));

            if (!BtnStatus)
            {
                btnViewInvitation.Enabled = false;
            }

            btnViewInvitation.Attributes["onclick"] = "showPopupDialog('DelegateManager.aspx?proxyUserPageType=1&isProxyUser=1&userSeqNum=" + delegateUser.userSeqNum + "','" + LabelUtil.GetTextByKey("aca_add_delegate", string.Empty) + "','" + btnViewInvitation.ClientID + "');return false;";

            HtmlContainerControl divProxyUserInfromation = (HtmlContainerControl)e.Item.FindControl("divProxyUserInfromation");
            HtmlContainerControl divProxyUserActionDate = (HtmlContainerControl)e.Item.FindControl("divProxyUserActionDate");
            ProxyUserStatus? proxyUserStatus = delegateUser.proxyUserModel.proxyStatus;

            switch (proxyUserStatus)
            {
                case ProxyUserStatus.P:
                    lblProxyUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_invitation_sent_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    divDelegateUserViewInvition.Visible = true;
                    break;
                case ProxyUserStatus.A:
                    lblProxyUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_last_accessed_account_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    PopupActions proxyUserActions = (PopupActions)e.Item.FindControl("PopupActions");
                    proxyUserActions.ActionLableKey = "aca_delegate_actions";
                    proxyUserActions.Visible = true;
                    proxyUserActions.AvailableActions = GetActions(proxyUserActions, delegateUser.userSeqNum, true);
                    proxyUserActions.BindListAction();
                    break;
                case ProxyUserStatus.E:
                    divProxyUserInfromation.Attributes["class"] = "ACA_TabRow_Italic";
                    divProxyUserActionDate.Attributes["class"] = "ACA_Magin_Top_Negative5 ACA_TabRow_Italic";
                    lblProxyUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_expired_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    break;
                case ProxyUserStatus.R:
                    divProxyUserInfromation.Attributes["class"] = "ACA_TabRow_Italic";
                    divProxyUserActionDate.Attributes["class"] = "ACA_Magin_Top_Negative5 ACA_TabRow_Italic";
                    lblProxyUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_rejected_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    break;
            }
        }

        /// <summary>
        /// Bind Address to UI.
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">the data list event args.</param>
        protected void MyInitUser_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            PublicUserModel4WS delegateUser = (PublicUserModel4WS)e.Item.DataItem;
            LinkButton btnAccept = (LinkButton)e.Item.FindControl("btnAccept");
            LinkButton btnReject = (LinkButton)e.Item.FindControl("btnReject");
            AccelaLabel lblInitUserName = (AccelaLabel)e.Item.FindControl("lblInitUserName");
            Label lblInitUserActionDate = (Label)e.Item.FindControl("lblInitUserActionDate");
            HtmlContainerControl divAccept = (HtmlContainerControl)e.Item.FindControl("divAccept");
            HtmlContainerControl divReject = (HtmlContainerControl)e.Item.FindControl("divReject");
            HtmlContainerControl divChange = (HtmlContainerControl)e.Item.FindControl("divChange");
            IPeopleBll peopleBll = ObjectFactory.GetObject(typeof(IPeopleBll)) as IPeopleBll;

            lblInitUserName.Text = peopleBll.GetContactUserName(delegateUser);

            if (BtnStatus)
            {
                divReject.Attributes["class"] = "ACA_LinkButton";
                divAccept.Attributes["class"] = "ACA_LinkButton ACA_DivMargin6";
            }
            else
            {
                divReject.Attributes["class"] = "ACA_LinkButtonDisable";
                divAccept.Attributes["class"] = "ACA_LinkButtonDisable ACA_DivMargin6";
                btnAccept.Enabled = false;
                btnReject.Enabled = false;
            }

            HtmlContainerControl divInitUserInfromation = (HtmlContainerControl)e.Item.FindControl("divInitUserInfromation");
            HtmlContainerControl divInitUserActionDate = (HtmlContainerControl)e.Item.FindControl("divInitUserActionDate");
            ProxyUserStatus? proxyUserStatus = delegateUser.proxyUserModel.proxyStatus;

            switch (proxyUserStatus)
            {
                case ProxyUserStatus.P:
                    divAccept.Visible = true;
                    divReject.Visible = true;
                    lblInitUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_invitation_received_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    break;
                case ProxyUserStatus.A:
                    lblInitUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_last_accessed_account_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    PopupActions initUserActions = (PopupActions)e.Item.FindControl("InitUserActions");
                    initUserActions.Visible = true;
                    initUserActions.ActionLableKey = "aca_delegate_actions";

                    initUserActions.AvailableActions = GetActions(initUserActions, delegateUser.userSeqNum, false);
                    initUserActions.BindListAction();
                    break;
                case ProxyUserStatus.E:
                    divInitUserInfromation.Attributes["class"] = "ACA_TabRow_Italic";
                    divInitUserActionDate.Attributes["class"] = "ACA_Magin_Top_Negative5 ACA_TabRow_Italic";
                    divAccept.Visible = true;
                    lblInitUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_expired_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    break;
                case ProxyUserStatus.R:
                    divInitUserInfromation.Attributes["class"] = "ACA_TabRow_Italic";
                    divInitUserActionDate.Attributes["class"] = "ACA_Magin_Top_Negative5 ACA_TabRow_Italic";
                    divAccept.Visible = true;
                    lblInitUserActionDate.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_view_rejected_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(delegateUser.proxyUserModel.accessDate));
                    break;
            }
        }

        /// <summary>
        /// Initializes user item command.
        /// </summary>
        /// <param name="source">System Sender.</param>
        /// <param name="e">the data list event args.</param>
        protected void MyInitUser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string hdnInitUserSeqNum = ((HiddenField)e.Item.FindControl("hdnInitUserSeqNum")).Value;

            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;
            XProxyUserModel xProxyUser = new XProxyUserModel();
            xProxyUser.serviceProviderCode = ConfigManager.AgencyCode;
            xProxyUser.proxyUserSeqNbr = long.Parse(AppSession.User.UserSeqNum);
            xProxyUser.userSeqNbr = long.Parse(hdnInitUserSeqNum);

            if (e.CommandName == "Accept")
            {
                xProxyUser.proxyStatus = ProxyUserStatus.A;
            }
            else if (e.CommandName == "Reject")
            {
                xProxyUser.proxyStatus = ProxyUserStatus.R;    
            }

            proxyUserBll.UpdateProxyStatus(xProxyUser, AppSession.User.PublicUserId);

            if (e.CommandName == "Accept")
            {
                UpdateSession();
            }

            InitDataSource();
            BindAvailableData();
        }

        /// <summary>
        /// Show Reject Expired proxy users.
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">the data list event args.</param>
        protected void ShowRejectExpiredProxyUsersButton_OnClick(object sender, EventArgs e)
        {
            if (ACAConstant.COMMON_ZERO.Equals(hdnShowRejectExpiredProxyUsers.Value))
            {
                btnShowRejectExpiredProxyUsers.Text = LabelUtil.GetTextByKey("aca_hide_expired_items", string.Empty);
                hdnShowRejectExpiredProxyUsers.Value = ACAConstant.COMMON_ONE;
            }
            else
            {
                btnShowRejectExpiredProxyUsers.Text = LabelUtil.GetTextByKey("aca_show_expired_items", string.Empty);
                hdnShowRejectExpiredProxyUsers.Value = ACAConstant.COMMON_ZERO;
            }

            BindAvailableData();

            ScriptManager.RegisterStartupScript(this.Page, GetType(), "focusBack", "SetFocusBack();", true);
        }

        /// <summary>
        /// Show Reject Expired initial users.
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">the data list event args.</param>
        protected void ShowRejectExpiredInitUsersButton_OnClick(object sender, EventArgs e)
        {
            if (ACAConstant.COMMON_ZERO.Equals(hdnShowRejectExpiredInitUsers.Value))
            {
                btnShowRejectExpiredInitUsers.Text = LabelUtil.GetTextByKey("aca_hide_expired_items", string.Empty);
                hdnShowRejectExpiredInitUsers.Value = ACAConstant.COMMON_ONE;
            }
            else
            {
                btnShowRejectExpiredInitUsers.Text = LabelUtil.GetTextByKey("aca_show_expired_items", string.Empty);
                hdnShowRejectExpiredInitUsers.Value = ACAConstant.COMMON_ZERO;
            }

            BindAvailableData();
        }

        /// <summary>
        /// Add Delegate User Click
        /// </summary>
        /// <param name="sender">System sender.</param>
        /// <param name="e">the data list event args.</param>
        protected void RemoveAccountLink_OnClick(object sender, EventArgs e)
        {
            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;
            XProxyUserModel xProxyUser = new XProxyUserModel();
            xProxyUser.serviceProviderCode = ConfigManager.AgencyCode;

            if (hdnIsProxyUser.Value.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase))
            {
                xProxyUser.proxyUserSeqNbr = long.Parse(hdnUserSeqNum.Value);
                xProxyUser.userSeqNbr = long.Parse(AppSession.User.UserSeqNum);
            }
            else
            {
                xProxyUser.proxyUserSeqNbr = long.Parse(AppSession.User.UserSeqNum);
                xProxyUser.userSeqNbr = long.Parse(hdnUserSeqNum.Value);
            }

            proxyUserBll.DeleteProxyUser(xProxyUser, AppSession.User.PublicUserId);

            if (!hdnIsProxyUser.Value.Equals(ACAConstant.COMMON_TRUE, StringComparison.InvariantCultureIgnoreCase))
            {
                UpdateSession();
            }

            InitDataSource();

            BindAvailableData();
        }

        /// <summary>
        /// Update session.
        /// </summary>
        private void UpdateSession()
        {
            IAccountBll accountBll = ObjectFactory.GetObject(typeof(IAccountBll)) as IAccountBll;
            AppSession.User.UserModel4WS = accountBll.GetPublicUser(AppSession.User.UserSeqNum);
        }

        /// <summary>
        /// Get Actions(Edit and view).
        /// </summary>
        /// <param name="popupAction">Pop up action.</param>
        /// <param name="userSeqNum">the user sequence number.</param>
        /// <param name="isForProxyUser">is for proxy user.</param>
        /// <returns>action view model.</returns>
        private ActionViewModel[] GetActions(PopupActions popupAction, string userSeqNum, bool isForProxyUser)
        {
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();
            string cssClassName = "InspectionWizardPageWidth";
            string isProxyUser = isForProxyUser ? "&isProxyUser=1" : string.Empty;
            string lnkActionID = popupAction.ClientID + "_lnkActions";
            List<ActionViewModel> actions = new List<ActionViewModel>();
            ActionViewModel actionView = new ActionViewModel();
            string url = "DelegateManager.aspx?proxyUserPageType=2&userSeqNum=" + userSeqNum + isProxyUser;
            actionView.ActionLabel = LabelUtil.GetTextByKey("aca_delegate_view_permission", string.Empty);
            actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
            actionView.ActionId = popupAction.ClientID + "_ViewPermissions";

            if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
            {
                actionView.ClientEvent = string.Format("showPopupDialog('{0}','{1}')", url, actionView.ActionId);
            }
            else
            {
                actionView.ClientEvent = string.Format("showPopupDialog('{0}','{1}')", url, lnkActionID);
            }

            actions.Add(actionView);

            if (isForProxyUser)
            {
                ActionViewModel actionEdit = new ActionViewModel();
                url = "DelegateManager.aspx?proxyUserPageType=1&userSeqNum=" + userSeqNum + isProxyUser;
                actionEdit.ActionLabel = LabelUtil.GetTextByKey("aca_delegate_edit_permission", string.Empty);
                actionEdit.ActionId = popupAction.ClientID + "_EditPermissions";
                actionEdit.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");

                if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
                {
                    actionEdit.ClientEvent = string.Format("showPopupDialog('{0}','{1}')", url, actionEdit.ActionId);
                }
                else
                {
                    actionEdit.ClientEvent = string.Format("showPopupDialog('{0}','{1}')", url, lnkActionID);
                }

                actions.Add(actionEdit);
            }

            ActionViewModel actionLine = new ActionViewModel();
            actionLine.SeparateLine = true;
            actions.Add(actionLine);

            ActionViewModel actionRemove = new ActionViewModel();
            actionRemove.ActionLabel = LabelUtil.GetTextByKey("aca_delegate_remove", string.Empty);
            actionRemove.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
            actionRemove.ActionId = popupAction.ClientID + "_Remove";
            string isProxyUserString = isForProxyUser ? ACAConstant.COMMON_TRUE : ACAConstant.COMMON_FALSE;

            if (actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase))
            {
                actionRemove.ClientEvent = string.Format("ConfirmRemoveProxyUser('{0}','{1}','{2}');return false;", userSeqNum, isProxyUserString, actionRemove.ActionId);
            }
            else
            {
                actionRemove.ClientEvent = string.Format("ConfirmRemoveProxyUser('{0}','{1}','{2}');return false;", userSeqNum, isProxyUserString, lnkActionID);
            }

            actions.Add(actionRemove);

            return actions.ToArray();
        }

        /// <summary>
        /// Bind Available Data.
        /// </summary>
        private void BindAvailableData()
        {
            List<PublicUserModel4WS> proxyDataSource = new List<PublicUserModel4WS>();
            List<PublicUserModel4WS> initDataSource = new List<PublicUserModel4WS>();
            List<PublicUserModel4WS> proxyExpriedItems = new List<PublicUserModel4WS>();

            if (ProxyUserList != null)
            {
                proxyExpriedItems = ProxyUserList.Where(p => p.proxyUserModel.proxyStatus == ProxyUserStatus.E || p.proxyUserModel.proxyStatus == ProxyUserStatus.R).ToList();

                if (ACAConstant.COMMON_ONE.Equals(hdnShowRejectExpiredProxyUsers.Value))
                {
                    List<PublicUserModel4WS> proxySortedList = new List<PublicUserModel4WS>();
                    proxySortedList.AddRange(ProxyUserList.Where(p => p.proxyUserModel.proxyStatus != ProxyUserStatus.E && p.proxyUserModel.proxyStatus != ProxyUserStatus.R).ToList());
                    proxySortedList.AddRange(proxyExpriedItems);
                    proxyDataSource = proxySortedList;
                }
                else
                {
                    proxyDataSource = ProxyUserList.Where(p => p.proxyUserModel.proxyStatus != ProxyUserStatus.E && p.proxyUserModel.proxyStatus != ProxyUserStatus.R).ToList();
                }
            }

            divShowRejectExpiredProxyUsers.Visible = proxyExpriedItems.Count != 0 || AppSession.IsAdmin;
            List<PublicUserModel4WS> initExpriedItems = new List<PublicUserModel4WS>();

            if (InitUserList != null)
            {
                initExpriedItems = InitUserList.Where(p => p.proxyUserModel.proxyStatus == ProxyUserStatus.E || p.proxyUserModel.proxyStatus == ProxyUserStatus.R).ToList();

                if (ACAConstant.COMMON_ONE.Equals(hdnShowRejectExpiredInitUsers.Value))
                {
                    List<PublicUserModel4WS> initSortedList = new List<PublicUserModel4WS>();
                    initSortedList.AddRange(InitUserList.Where(p => p.proxyUserModel.proxyStatus != ProxyUserStatus.E && p.proxyUserModel.proxyStatus != ProxyUserStatus.R).ToList());
                    initSortedList.AddRange(initExpriedItems);
                    initDataSource = initSortedList;
                }
                else
                {
                    initDataSource = InitUserList.Where(p => p.proxyUserModel.proxyStatus != ProxyUserStatus.E && p.proxyUserModel.proxyStatus != ProxyUserStatus.R).ToList();
                }
            }

            divShowRejectExpiredInitUsers.Visible = initExpriedItems.Count != 0 || AppSession.IsAdmin;

            lblDelegateUserNone.Visible = proxyDataSource == null || proxyDataSource.Count == 0;
            divAddDelegateUser.Visible = !lblDelegateUserNone.Visible;
            lblInitUserNone.Visible = initDataSource == null || initDataSource.Count == 0;

            myDelegateUserPanel.Update();

            BindData(proxyDataSource, initDataSource);
        }

        /// <summary>
        /// bind data.
        /// </summary>
        /// <param name="proxyDataSource">the proxy user data source</param>
        /// <param name="initDataSource">the initial user data source</param>
        private void BindData(List<PublicUserModel4WS> proxyDataSource, List<PublicUserModel4WS> initDataSource)
        {
            rptMyDelegateUser.DataSource = proxyDataSource;
            rptMyDelegateUser.DataBind();
            rptMyInitUser.DataSource = initDataSource;
            rptMyInitUser.DataBind();
        }

        /// <summary>
        /// initial data source.
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