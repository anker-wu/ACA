#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DelegateManagement.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DelegateManagement.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.ProxyUser;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation UserLicenseView.
    /// </summary>
    public partial class DelegateManagement : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// all categories
        /// </summary>
        private static readonly string ALLUFFIXAL = LabelUtil.GetTextByKey("aca_delegate_all_categories", string.Empty);

        /// <summary>
        /// selected categories
        /// </summary>
        private static readonly string SELECTEDSUFFIXAL = LabelUtil.GetTextByKey("aca_delegate_selected_categories", string.Empty);

        #endregion Fields

        /// <summary>
        /// click save button event.
        /// </summary>
        public event CommonEventHandler UpdateDelegateCommand;

        /// <summary>
        /// click Edit Permission button event.
        /// </summary>
        public event CommonEventHandler EditPageTitleDelegateCommand;

        #region Properties
        /// <summary>
        /// Gets or sets the Delegate user sequence number.
        /// </summary>
        public string DelegateUserSeqNum
        {
            get
            {
                return (string)ViewState["DelegateUserSeqNum"];
            }

            set
            {
                ViewState["DelegateUserSeqNum"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the page type.
        /// </summary>
        public ProxyUserPageType PageType
        {
            get
            {
                return (ProxyUserPageType)ViewState["PageType"];
            }

            set
            {
                ViewState["PageType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value the button status.
        /// </summary>
        public PublicUserModel4WS ProxyUserDataSource
        {
            get
            {
                if (ViewState["ProxyUserDataSource"] == null)
                {
                    return null;
                }

                return (PublicUserModel4WS)ViewState["ProxyUserDataSource"];
            }

            set
            {
                ViewState["ProxyUserDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the module mapping.
        /// </summary>
        private Dictionary<string, string> ModuleMapping
        {
            get
            {
                return (Dictionary<string, string>)ViewState["ModuleMapping"];
            }

            set
            {
                ViewState["ModuleMapping"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the module list.
        /// </summary>
        private List<string> ModuleList
        {
            get
            {
                return (List<string>)ViewState["ModuleList"];
            }

            set
            {
                ViewState["ModuleList"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the permission modules.
        /// </summary>
        private Hashtable PermissionModules
        {
            get
            {
                return (Hashtable)ViewState["PermissionModules"];
            }

            set
            {
                ViewState["PermissionModules"] = value;
            }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// bind data.
        /// </summary>
        public void BindData()
        {
            ModuleMapping = TabUtil.GetAllEnableModules(false);
            List<string> tempModuleList = new List<string>();

            if (ModuleMapping != null && ModuleMapping.Count > 0)
            {
                foreach (KeyValuePair<string, string> module in ModuleMapping)
                {
                    tempModuleList.Add(module.Key);
                }
            }

            ModuleList = tempModuleList;

            switch (PageType)
            {
                case ProxyUserPageType.Create:
                    hdnExpand.Value = ACAConstant.COMMON_ZERO;
                    ShowOrHiddenPersonNote();
                    divUserLabel.Visible = false;
                    divCreateDelegte.Visible = true;
                    ViewDelegate.Visible = false;
                    divEditDelegate.Visible = false;
                    InitCreateUI();
                    break;
                case ProxyUserPageType.Edit:
                    divUserLabel.Visible = true;
                    divCreateDelegte.Visible = false;
                    divEditDelegate.Visible = true;
                    ViewDelegate.Visible = false;
                    delegateEdit.DelegateUserSeqNum = DelegateUserSeqNum;
                    delegateEdit.ProxyUserDataSource = ProxyUserDataSource;
                    delegateEdit.BindData();
                    delegateEdit.FocusBeginning();
                    break;
                case ProxyUserPageType.View:
                    divUserLabel.Visible = true;
                    divCreateDelegte.Visible = false;
                    divEditDelegate.Visible = false;
                    ViewDelegate.Visible = true;
                    InitViewUI();
                    HandButtons();
                    break;
            }
        }

        /// <summary>
        /// Invite delegate
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void InviteDelegateButton_OnClick(object sender, EventArgs e)
        {
            if (!reCaptcha.Validate())
            {
                tbEmailAddress.Validate = "required;email;customvalidation";
                tbNickName.Validate = "required;MaxLength";
                return;
            }

            List<XProxyUserPermissionModel> proxyUserPermissionModels = GetPermissionModels();

            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;
            XProxyUserModel xProxyUser = new XProxyUserModel();
            xProxyUser.serviceProviderCode = ConfigManager.AgencyCode;
            xProxyUser.XProxyUserPermissionModels = proxyUserPermissionModels.ToArray();
            xProxyUser.userSeqNbr = long.Parse(AppSession.User.UserSeqNum);
            xProxyUser.nickName = tbNickName.Text.Trim();
            xProxyUser.invitationMessage = tbPersonNote.Text.Trim();

            try
            {
                proxyUserBll.CreateProxyUser(xProxyUser, tbEmailAddress.Text, AppSession.User.PublicUserId);

                ScriptManager.RegisterStartupScript(Page, GetType(), "InviteDelegate", "RefreshParent();", true);

                if (UpdateDelegateCommand != null)
                {
                    UpdateDelegateCommand(sender, null);
                }
            }
            catch (Exception ex)
            {
                MessageUtil.ShowAlertMessage(Page, ex.Message);
            }
        }

        /// <summary>
        /// View Record permission click.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle</param>
        protected void ViewRecordPermissionButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.VIEW_RECORD, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// create application
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void CreateApplicationButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.CREATE_APPLICATION, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// manage document click.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void ManageDocumentsButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.MANAGE_DOCUMENTS, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// make payment click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void MakePaymentsLink_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.MAKE_PAYMENTS, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// amendment record click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void AmendmentLink_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.AMENDMENT, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// manage inspection click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void ManageInspectionsButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.MANAGE_INSPECTIONS, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// the renew record click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void RenewButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.RENEW_RECORD, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// edit permission click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void EditPermissionsLink_OnClick(object sender, EventArgs e)
        {
            PageType = ProxyUserPageType.Edit;
            delegateEdit.DelegateUserSeqNum = DelegateUserSeqNum;
            BindData();
        }

        /// <summary>
        /// remove account click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void RemoveAccountButton_OnClick(object sender, EventArgs e)
        {
            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;
            XProxyUserModel xProxyUser = new XProxyUserModel();
            xProxyUser.serviceProviderCode = ConfigManager.AgencyCode;

            string isProxyUser = Request.QueryString["isProxyUser"] == null ? string.Empty : Request.QueryString["isProxyUser"].ToString();

            if (isProxyUser.Equals(ACAConstant.COMMON_ONE, StringComparison.InvariantCultureIgnoreCase))
            {
                xProxyUser.proxyUserSeqNbr = long.Parse(DelegateUserSeqNum);
                xProxyUser.userSeqNbr = long.Parse(AppSession.User.UserSeqNum);
            }
            else
            {
                AppSession.ReloadPublicUserSession();
                xProxyUser.proxyUserSeqNbr = long.Parse(AppSession.User.UserSeqNum);
                xProxyUser.userSeqNbr = long.Parse(DelegateUserSeqNum);
            }

            proxyUserBll.DeleteProxyUser(xProxyUser, AppSession.User.PublicUserId);

            if (UpdateDelegateCommand != null)
            {
                UpdateDelegateCommand(sender, null);
            }

            ScriptManager.RegisterStartupScript(Page, GetType(), "RemoveAccount", "RefreshParent();", true);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            delegateEdit.UpdateDelegateCommand += UpdateDelegate;

            if (IsPostBack)
            {
                ShowOrHiddenPersonNote();
            }
            else
            {
                RegisterHotKey();
            }

            string labelKey = PageType == ProxyUserPageType.Create ? "aca_add_delegate_section_title" : "aca_delegate_manage_section_title";
            CommonEventArgs arg = new CommonEventArgs(labelKey);
            EditPageTitleDelegateCommand(sender, arg);

            if (PageType == ProxyUserPageType.View)
            {
                btnCancelPopup.Attributes["onclick"] = "CloseModuleListWindow();return false";
            }
        }
        
        /// <summary>
        /// save module click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void SaveModuleButton_OnClick(object sender, EventArgs e)
        {
            tbEmailAddress.Validate = "required;email;customvalidation";
            tbNickName.Validate = "required;MaxLength";
            List<string> moduleList = new List<string>();

            foreach (ListItem li in cblModuleList.Items)
            {
                if (li.Selected)
                {
                    moduleList.Add(li.Value);
                }
            }

            if (moduleList == null || moduleList.Count == 0)
            {
                return;
            }

            ProxyPermissionType permissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnAddProxyUserRoleType.Value);

            PermissionModules[permissionType] = moduleList;

            if (permissionType == ProxyPermissionType.VIEW_RECORD)
            {
                foreach (ProxyPermissionType p in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
                {
                    List<string> allModule = (List<string>)PermissionModules[p];

                    if (allModule == null)
                    {
                        continue;
                    }

                    PermissionModules[p] = allModule.Where(f => moduleList.Contains(f.ToString())).ToList();

                    //When View Records contains module list doesn't in sub group module list,then remove the sub group checked state. 
                    if (allModule.Count > 0 && ((List<string>)PermissionModules[p]).Count == 0)
                    {
                        GetCheckBoxByPermissionType(p).Checked = false;
                    }
                }
            }

            RefreshUI();
            string scriptKey = permissionType.ToString() + "_Close";
            ScriptManager.RegisterStartupScript(Page, GetType(), scriptKey, "CloseModuleListWindow();", true);
        }

        /// <summary>
        /// save module click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void CancelModuleSaveButton_OnClick(object sender, EventArgs e)
        {
            tbEmailAddress.Validate = "required;email;customvalidation";
            tbNickName.Validate = "required;MaxLength";

            ScriptManager.RegisterStartupScript(Page, GetType(), "CloseModuleListWindow", "CloseModuleListWindow();", true);
        }

        /// <summary>
        ///   Register hot key to cancel button
        /// </summary>
        private void RegisterHotKey()
        {
            const string JSFormat = "OverrideTabKey(event, {0}, '{1}')";
            btnCancelPopup.Attributes.Add("onkeydown", string.Format(JSFormat, "false", hlBegin.ClientID));
        }

        /// <summary>
        /// hand buttons.
        /// </summary>
        private void HandButtons()
        {
            string isProxyUser = Request.QueryString["isProxyUser"] == null ? string.Empty : Request.QueryString["isProxyUser"].ToString();

            if (isProxyUser.Equals(ACAConstant.COMMON_ONE, StringComparison.InvariantCultureIgnoreCase))
            {
                btnRemoveAccount.Text = LabelUtil.GetTextByKey("aca_remove_delegate", string.Empty);
                lnkEditPermissions.Visible = true;
            }
            else
            {
                btnRemoveAccount.Text = LabelUtil.GetTextByKey("aca_remove_inviter", string.Empty);
                lnkEditPermissions.Visible = false;
            }
        }

        /// <summary>
        /// Initializes create UI.
        /// </summary>
        private void InitCreateUI()
        {
            ClearControlValue();
            chkCreateApplication.Checked = false;
            chkRenewRecord.Checked = false;
            chkManageInspections.Checked = false;
            chkManageDocuments.Checked = false;
            chkMakePayments.Checked = false;
            chkAmendment.Checked = false;
            tbEmailAddress.Visible = true;
            tbEmailAddress.Validate = "required;email;customvalidation";
            tbNickName.Validate = "required;MaxLength";
            hdnAddProxyUserRoleType.Value = string.Empty;

            InitPermissionModules();
            RefreshUI();
        }

        /// <summary>
        /// Initializes view UI.
        /// </summary>
        private void InitViewUI()
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            InitDataSource();

            lblViewRecord_ModelSuffixal.Visible = true;
            lnkViewRecord.Visible = true;

            liCreateRecord.Visible = true;
            lblCreateRecord_ModelSuffixal.Visible = true;
            lnkCreateRecord.Visible = true;

            liRenewRecord.Visible = true;
            lblRenewRecord_ModelSuffixal.Visible = true;
            lnkRenewRecord.Visible = true;

            liManageInspections.Visible = true;
            lblManageInspections_ModelSuffixal.Visible = true;
            lnkManageInspections.Visible = true;

            liManageDocuments.Visible = true;
            lblManageDocuments_ModelSuffixal.Visible = true;
            lnkManageDocuments.Visible = true;

            liMakePayments.Visible = true;
            lblMakePayments_ModelSuffixal.Visible = true;
            lnkMakePayments.Visible = true;

            liAmendment.Visible = true;
            lblAmendment_ModelSuffixal.Visible = true;
            lnkAmendment.Visible = true;

            foreach (ProxyPermissionType permissionType in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
            {
                List<string> modules = (List<string>)PermissionModules[permissionType];

                int modulesCount = modules.Count();

                switch (permissionType)
                {
                    case ProxyPermissionType.VIEW_RECORD:
                        if (modulesCount == 1)
                        {
                            lnkViewRecord.Visible = false;
                            lblViewRecord_ModelSuffixal.Text = HttpUtility.HtmlDecode(ModuleMapping[modules[0]]);
                        }
                        else if (modulesCount == ModuleList.Count || modulesCount == 0)
                        {
                            lnkViewRecord.Visible = false;
                            lblViewRecord_ModelSuffixal.Text = ALLUFFIXAL;
                        }
                        else if (modulesCount < ModuleList.Count)
                        {
                            lblViewRecord_ModelSuffixal.Visible = false;
                            lnkViewRecord.Text = SELECTEDSUFFIXAL;
                        }

                        break;
                    case ProxyPermissionType.CREATE_APPLICATION:
                        SettingViewUI(modules, liCreateRecord, lnkCreateRecord, lblCreateRecord_ModelSuffixal);
                        break;
                    case ProxyPermissionType.RENEW_RECORD:
                        SettingViewUI(modules, liRenewRecord, lnkRenewRecord, lblRenewRecord_ModelSuffixal);
                        break;
                    case ProxyPermissionType.MANAGE_INSPECTIONS:
                        SettingViewUI(modules, liManageInspections, lnkManageInspections, lblManageInspections_ModelSuffixal);
                        break;
                    case ProxyPermissionType.MANAGE_DOCUMENTS:
                        SettingViewUI(modules, liManageDocuments, lnkManageDocuments, lblManageDocuments_ModelSuffixal);
                        break;
                    case ProxyPermissionType.MAKE_PAYMENTS:
                        SettingViewUI(modules, liMakePayments, lnkMakePayments, lblMakePayments_ModelSuffixal);
                        break;
                    case ProxyPermissionType.AMENDMENT:
                        SettingViewUI(modules, liAmendment, lnkAmendment, lblAmendment_ModelSuffixal);
                        break;
                }
            }

            lblDelegateEmailAddress.Text = ProxyUserDataSource.email;
            lblAddedTime.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_delegate_add_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(ProxyUserDataSource.proxyUserModel.createdDate));
            string isProxyUser = Request.QueryString["isProxyUser"] == null ? string.Empty : Request.QueryString["isProxyUser"].ToString();

            if (isProxyUser.Equals(ACAConstant.COMMON_ONE, StringComparison.InvariantCultureIgnoreCase))
            {
                lblDelegateUserName.Text = ScriptFilter.EncodeHtml(ProxyUserDataSource.proxyUserModel == null ? string.Format("{0}{1}{2}", ProxyUserDataSource.firstName, ACAConstant.BLANK, ProxyUserDataSource.lastName) : ProxyUserDataSource.proxyUserModel.nickName);
                lblLastAccessedTime.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_delegate_last_accessed_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(ProxyUserDataSource.proxyUserModel.accessDate));
                lblViewStruction.Text = LabelUtil.GetTextByKey("aca_delegate_view_struction", string.Empty);
            }
            else
            {
                lblDelegateUserName.Text = ScriptFilter.EncodeHtml(string.Format("{0}{1}{2}", ProxyUserDataSource.firstName, ACAConstant.BLANK, ProxyUserDataSource.lastName));
                lblLastAccessedTime.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_last_accessed_on_delegate", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(ProxyUserDataSource.proxyUserModel.accessDate));
                lblViewStruction.Text = LabelUtil.GetTextByKey("aca_delegate_view_struction_delegate", string.Empty);
            }
        }

        /// <summary>
        /// Setting View UI
        /// </summary>
        /// <param name="modules">the module list.</param>
        /// <param name="li">li control.</param>
        /// <param name="linkButton">the link button.</param>
        /// <param name="label">the label.</param>
        private void SettingViewUI(List<string> modules, HtmlGenericControl li, AccelaLinkButton linkButton, AccelaLabel label)
        {
            int modulesCount = modules.Count();

            if (modulesCount == 0)
            {
                li.Visible = false;
            }
            else if (modulesCount == 1)
            {
                linkButton.Visible = false;
                label.Text = HttpUtility.HtmlDecode(ModuleMapping[modules[0]]);
            }
            else if (modulesCount == ModuleList.Count)
            {
                linkButton.Visible = false;
                label.Text = ALLUFFIXAL;
            }
            else if (modulesCount < ModuleList.Count)
            {
                label.Visible = false;
                linkButton.Text = SELECTEDSUFFIXAL;
            }
        }

        /// <summary>
        /// Setting Permission Check box.
        /// </summary>
        /// <param name="permissionType">the permission type.</param>
        /// <param name="moduleSuffixal">the module suffix.</param>
        /// <param name="isChecked">is check box checked.</param>
        /// <param name="checkBox">the check box control.</param>
        /// <param name="labelKey">the label key.</param>
        private void SettingPermissionCheckBox(ProxyPermissionType permissionType, string moduleSuffixal, bool isChecked, AccelaCheckBox checkBox, string labelKey)
        {
            if (PermissionModules[permissionType] != null && ((List<string>)PermissionModules[permissionType]).Count != 0)
            {
                checkBox.Text = string.Format(LabelUtil.GetTextByKey(labelKey, string.Empty), moduleSuffixal);
            }

            //when view invitation,the hiden field is null;
            //when manage delegate the hiden field will be fill.
            if (!string.IsNullOrEmpty(hdnAddProxyUserRoleType.Value))
            {
                ProxyPermissionType addPermissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnAddProxyUserRoleType.Value);

                //when click View Records in selected categories,the sub category should keep current state.
                if (!ProxyPermissionType.VIEW_RECORD.Equals(addPermissionType))
                {
                    checkBox.Checked = isChecked;
                }
            }
            else
            {
                checkBox.Checked = isChecked;
            }
        }

        /// <summary>
        /// Invite delegate
        /// </summary>
        /// <returns>the proxy user permission model list.</returns>
        private List<XProxyUserPermissionModel> GetPermissionModels()
        {
            List<XProxyUserPermissionModel> permissionModels = new List<XProxyUserPermissionModel>();

            foreach (string module in ModuleList)
            {
                ProxyUserRolePrivilegeModel4WS rolePrivilege = new ProxyUserRolePrivilegeModel4WS();
                XProxyUserPermissionModel permissionModel = new XProxyUserPermissionModel();
                string permissions = ACAConstant.ROLE_PROXYUSER_NOPERMISSION;

                if (((List<string>)PermissionModules[ProxyPermissionType.VIEW_RECORD]).Contains(module))
                {
                    rolePrivilege.viewRecordAllowed = ((List<string>)PermissionModules[ProxyPermissionType.VIEW_RECORD]).Contains(module);
                    rolePrivilege.createApplicationAllowed = chkCreateApplication.Checked
                        && (PermissionModules[ProxyPermissionType.CREATE_APPLICATION] == null || ((List<string>)PermissionModules[ProxyPermissionType.CREATE_APPLICATION]).Count == 0 || ((List<string>)PermissionModules[ProxyPermissionType.CREATE_APPLICATION]).Contains(module));
                    rolePrivilege.renewRecordAllowed = chkRenewRecord.Checked
                        && (PermissionModules[ProxyPermissionType.RENEW_RECORD] == null || ((List<string>)PermissionModules[ProxyPermissionType.RENEW_RECORD]).Count == 0 || ((List<string>)PermissionModules[ProxyPermissionType.RENEW_RECORD]).Contains(module));
                    rolePrivilege.manageInspectionsAllowed = chkManageInspections.Checked
                        && (PermissionModules[ProxyPermissionType.MANAGE_INSPECTIONS] == null || ((List<string>)PermissionModules[ProxyPermissionType.MANAGE_INSPECTIONS]).Count == 0 || ((List<string>)PermissionModules[ProxyPermissionType.MANAGE_INSPECTIONS]).Contains(module));
                    rolePrivilege.manageDocumentsAllowed = chkManageDocuments.Checked
                        && (PermissionModules[ProxyPermissionType.MANAGE_DOCUMENTS] == null || ((List<string>)PermissionModules[ProxyPermissionType.MANAGE_DOCUMENTS]).Count == 0 || ((List<string>)PermissionModules[ProxyPermissionType.MANAGE_DOCUMENTS]).Contains(module));
                    rolePrivilege.makePaymentsAllowed = chkMakePayments.Checked
                        && (PermissionModules[ProxyPermissionType.MAKE_PAYMENTS] == null || ((List<string>)PermissionModules[ProxyPermissionType.MAKE_PAYMENTS]).Count == 0 || ((List<string>)PermissionModules[ProxyPermissionType.MAKE_PAYMENTS]).Contains(module));
                    rolePrivilege.amendmentAllowed = chkAmendment.Checked
                        && (PermissionModules[ProxyPermissionType.AMENDMENT] == null || ((List<string>)PermissionModules[ProxyPermissionType.AMENDMENT]).Count == 0 || ((List<string>)PermissionModules[ProxyPermissionType.AMENDMENT]).Contains(module));
                    var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                    permissions = proxyUserRoleBll.ConvertToUserRoleString(rolePrivilege).PadRight(15, '0');
                }

                permissionModel.serviceProviderCode = ConfigManager.AgencyCode;
                permissionModel.userSeqNbr = long.Parse(AppSession.User.UserSeqNum);

                permissionModel.levelType = ACAConstant.LEVEL_TYPE_MODULE;
                permissionModel.levelData = module;
                permissionModel.permission = permissions;
                permissionModels.Add(permissionModel);
            }

            return permissionModels;
        }

        /// <summary>
        /// Initializes data source.
        /// </summary>
        private void InitDataSource()
        {
            PermissionModules = new Hashtable();
            List<XProxyUserPermissionModel> allPermissions = ProxyUserDataSource.proxyUserModel.XProxyUserPermissionModels.ToList();
            List<XProxyUserPermissionModel> permissions = new List<XProxyUserPermissionModel>();
            allPermissions = allPermissions.Where(f => ModuleList.Contains(f.levelData)).ToList();
            string isProxyUser = Request.QueryString["isProxyUser"] == null ? string.Empty : Request.QueryString["isProxyUser"].ToString();

            if (isProxyUser.Equals(ACAConstant.COMMON_ONE, StringComparison.InvariantCultureIgnoreCase))
            {
                permissions = allPermissions.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.proxyUserSeqNbr == long.Parse(DelegateUserSeqNum)).ToList();
            }
            else
            {
                permissions = allPermissions.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.userSeqNbr == long.Parse(DelegateUserSeqNum)).ToList();
            }

            foreach (ProxyPermissionType c in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
            {
                List<string> moduleList = new List<string>();

                foreach (XProxyUserPermissionModel permission in permissions)
                {
                    var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

                    if (proxyUserRoleBll.IsPermissionTypeHasPermission(c, permission.permission))
                    {
                        moduleList.Add(permission.levelData);
                    }
                }

                PermissionModules[c] = moduleList;
            }
        }

        /// <summary>
        /// Initializes permission modules.
        /// </summary>
        private void InitPermissionModules()
        {
            PermissionModules = new Hashtable();

            foreach (ProxyPermissionType c in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
            {
                if (ProxyPermissionType.VIEW_RECORD == c)
                {
                    PermissionModules.Add(c, ModuleList);
                }
                else
                {
                    PermissionModules.Add(c, null);
                }
            }
        }

        /// <summary>
        /// Refresh UI.
        /// </summary>
        private void RefreshUI()
        {
            foreach (ProxyPermissionType permissionType in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
            {
                if (!string.IsNullOrEmpty(hdnAddProxyUserRoleType.Value))
                {
                    ProxyPermissionType addPermissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnAddProxyUserRoleType.Value);

                    if (addPermissionType != ProxyPermissionType.VIEW_RECORD && addPermissionType != permissionType)
                    {
                        continue;
                    }
                }

                bool hasModules = true;
                List<string> modules = (List<string>)PermissionModules[permissionType];
                string moduleSuffixal = ALLUFFIXAL;

                if (modules != null && modules.Count != 0)
                {
                    if (modules.Count == 1)
                    {
                        moduleSuffixal = HttpUtility.HtmlDecode(ModuleMapping[modules[0]]);
                    }
                    else if (modules.Count < ModuleList.Count)
                    {
                        moduleSuffixal = SELECTEDSUFFIXAL;
                    }
                }
                else
                {
                    hasModules = false;
                }

                switch (permissionType)
                {
                    case ProxyPermissionType.VIEW_RECORD:
                        lblViewRecordRoleTitle.Text = string.Format(LabelUtil.GetTextByKey("aca_view_record_description", string.Empty), moduleSuffixal);
                        chkCreateApplication.Text = string.Format(LabelUtil.GetTextByKey("aca_create_application_description", string.Empty), moduleSuffixal);
                        chkRenewRecord.Text = string.Format(LabelUtil.GetTextByKey("aca_renew_record_description", string.Empty), moduleSuffixal);
                        chkManageInspections.Text = string.Format(LabelUtil.GetTextByKey("aca_manage_inspections_description", string.Empty), moduleSuffixal);
                        chkManageDocuments.Text = string.Format(LabelUtil.GetTextByKey("aca_manage_document_description", string.Empty), moduleSuffixal);
                        chkMakePayments.Text = string.Format(LabelUtil.GetTextByKey("aca_make_paments_description", string.Empty), moduleSuffixal);
                        chkAmendment.Text = string.Format(LabelUtil.GetTextByKey("aca_amend_record", string.Empty), moduleSuffixal);
                        break;
                    case ProxyPermissionType.CREATE_APPLICATION:
                        SettingPermissionCheckBox(permissionType, moduleSuffixal, hasModules, chkCreateApplication, "aca_create_application_description");
                        break;
                    case ProxyPermissionType.RENEW_RECORD:
                        SettingPermissionCheckBox(permissionType, moduleSuffixal, hasModules, chkRenewRecord, "aca_renew_record_description");
                        break;
                    case ProxyPermissionType.MANAGE_INSPECTIONS:
                        SettingPermissionCheckBox(permissionType, moduleSuffixal, hasModules, chkManageInspections, "aca_manage_inspections_description");
                        break;
                    case ProxyPermissionType.MANAGE_DOCUMENTS:
                        SettingPermissionCheckBox(permissionType, moduleSuffixal, hasModules, chkManageDocuments, "aca_manage_document_description");
                        break;
                    case ProxyPermissionType.MAKE_PAYMENTS:
                        SettingPermissionCheckBox(permissionType, moduleSuffixal, hasModules, chkMakePayments, "aca_make_paments_description");
                        break;
                    case ProxyPermissionType.AMENDMENT:
                        SettingPermissionCheckBox(permissionType, moduleSuffixal, hasModules, chkAmendment, "aca_amend_record");
                        break;
                }
            }
        }

        /// <summary>
        /// Show or hidden person note.
        /// </summary>
        private void ShowOrHiddenPersonNote()
        {
            if (ACAConstant.COMMON_ONE == hdnExpand.Value)
            {
                divPersonNoteInput.Attributes["class"] = "ACA_Show";
                lnkRemoveNote.Attributes["class"] = "ACA_Show";
                lnkAddNote.Attributes["class"] = "ACA_Hide";
            }
            else
            {
                divPersonNoteInput.Attributes["class"] = "ACA_Hide";
                lnkRemoveNote.Attributes["class"] = "ACA_Hide";
                lnkAddNote.Attributes["class"] = "ACA_Show";
            }
        }

        /// <summary>
        /// update delegate event.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="arg">the argument.</param>
        private void UpdateDelegate(object sender, CommonEventArgs arg)
        {
            if (UpdateDelegateCommand != null)
            {
                UpdateDelegateCommand(sender, null);
            }
        }

        /// <summary>
        /// clear control value.
        /// </summary>
        private void ClearControlValue()
        {
            tbNickName.Text = string.Empty;
            tbEmailAddress.Text = string.Empty;
        }

        /// <summary>
        /// bind module list.
        /// </summary>
        /// <param name="permissionType">the permission type.</param>
        /// <param name="focusBackCtrolID">the focusBackControlID.</param>
        private void BindModuleListByRoleType(ProxyPermissionType permissionType, string focusBackCtrolID)
        {
            List<string> viewRecordModules = (List<string>)PermissionModules[ProxyPermissionType.VIEW_RECORD];
            List<string> modules = (List<string>)PermissionModules[permissionType];

            cblModuleList.Items.Clear();

            foreach (string module in ModuleList)
            {
                ListItem li = new ListItem(HttpUtility.HtmlDecode(ModuleMapping[module]), module);

                if (permissionType != ProxyPermissionType.VIEW_RECORD)
                {
                    li.Selected = ((modules == null || modules.Count == 0) && viewRecordModules.Contains(module)) 
                        || ((modules != null && modules.Count != 0) && modules.Contains(module) && viewRecordModules.Contains(module));
                    li.Enabled = viewRecordModules.Contains(module);
                }
                else
                {
                    li.Selected = modules.Contains(module);
                }

                if (PageType == ProxyUserPageType.View)
                {
                    li.Enabled = false;
                }

                cblModuleList.Items.Add(li);
            }

            btnSaveModule.Visible = !(PageType == ProxyUserPageType.View);
            divBlank.Visible = PageType == ProxyUserPageType.View;

            ScriptManager.RegisterStartupScript(Page, GetType(), permissionType.ToString(), "ShowModuleList('" + permissionType.ToString() + "','" + focusBackCtrolID + "');", true);
        }

        /// <summary>
        /// Get check box according to permission type.
        /// </summary>
        /// <param name="permissionType">ENUM ProxyPermissionType</param>
        /// <returns>the check box.</returns>
        private AccelaCheckBox GetCheckBoxByPermissionType(ProxyPermissionType permissionType)
        {
            AccelaCheckBox currentCheckBox = new AccelaCheckBox();

            switch (permissionType)
            {
                case ProxyPermissionType.CREATE_APPLICATION:
                    currentCheckBox = chkCreateApplication;
                    break;
                case ProxyPermissionType.RENEW_RECORD:
                    currentCheckBox = chkRenewRecord;
                    break;
                case ProxyPermissionType.MANAGE_INSPECTIONS:
                    currentCheckBox = chkManageInspections;
                    break;
                case ProxyPermissionType.MANAGE_DOCUMENTS:
                    currentCheckBox = chkManageDocuments;
                    break;
                case ProxyPermissionType.MAKE_PAYMENTS:
                    currentCheckBox = chkMakePayments;
                    break;
                case ProxyPermissionType.AMENDMENT:
                    currentCheckBox = chkAmendment;
                    break;
            }

            return currentCheckBox;
        }

        #endregion Methods
    }
}