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
 *      $Id: DelegateEdit.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
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
    public partial class DelegateEdit : BaseUserControl
    {
        /// <summary>
        /// all categories label.
        /// </summary>
        private static readonly string ALLUFFIXAL = LabelUtil.GetTextByKey("aca_delegate_all_categories", string.Empty);

        /// <summary>
        /// selected categories label.
        /// </summary>
        private static readonly string SELECTEDSUFFIXAL = LabelUtil.GetTextByKey("aca_delegate_selected_categories", string.Empty);

        /// <summary>
        /// click save button event.
        /// </summary>
        public event CommonEventHandler UpdateDelegateCommand;

        /// <summary>
        /// Gets or sets the delegate user sequence number.
        /// </summary>
        public string DelegateUserSeqNum
        {
            get
            {
                return (string)ViewState["DelegateUserSeqNumEdit"];
            }

            set
            {
                ViewState["DelegateUserSeqNumEdit"] = value;
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
        /// Gets or sets the data source.
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

        /// <summary>
        /// Bind Data.
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

            InitUI();
        }

        /// <summary>
        /// Focus beginning element.
        /// </summary>
        public void FocusBeginning()
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "FocusBegginning", "focusBegginning();", true);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cblModuleList.Attributes.Add("onclick", "handleButtonDelegateEdit(this)");
                RegisterHotKey();
            }
        }

        /// <summary>
        /// save module list.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void SaveModuleButton_OnClick(object sender, EventArgs e)
        {
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

            ProxyPermissionType permissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnEditProxyUserRoleType.Value);

            PermissionModules[permissionType] = moduleList;

            if (permissionType == ProxyPermissionType.VIEW_RECORD)
            {
                foreach (ProxyPermissionType p in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
                {
                    List<string> allModule = (List<string>)PermissionModules[p];
                    PermissionModules[p] = allModule.Where(f => moduleList.Contains(f.ToString())).ToList();

                    //When View Records contains module list doesn't in sub group module list,then remove the sub group checked state. 
                    if (allModule.Count > 0 && ((List<string>)PermissionModules[p]).Count == 0)
                    {
                        GetCkeckBoxByPermissionType(p).Checked = false;
                    }
                }
            }

            RefreshUI();
            KeepFocus();
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void SaveChangesButton_OnClick(object sender, EventArgs e)
        {
            List<XProxyUserPermissionModel> proxyUserPermissionModels = GetPermissionModels();

            IProxyUserBll proxyUserBll = ObjectFactory.GetObject(typeof(IProxyUserBll)) as IProxyUserBll;

            if (proxyUserPermissionModels == null || proxyUserPermissionModels.Count > 0)
            {
                proxyUserBll.UpdatePermissions(proxyUserPermissionModels.ToArray(), AppSession.User.PublicUserId);
            }

            if (UpdateDelegateCommand != null)
            {
                UpdateDelegateCommand(sender, null);
            }

            ScriptManager.RegisterStartupScript(this.Page, GetType(), "CloseDelegeteWindow", "CloseDelegeteWindow();", true);
        }

        /// <summary>
        /// view record permission click.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void ViewRecordPermissionButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.VIEW_RECORD, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// create application click.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void CreateApplicationButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.CREATE_APPLICATION, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// manage document click
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void ManageDocumentsButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.MANAGE_DOCUMENTS, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// make payment click
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void MakePaymentsLink_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.MAKE_PAYMENTS, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// the amendment link click. 
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void AmendmentLink_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.AMENDMENT, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// manage inspection click.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void ManageInspectionsButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.MANAGE_INSPECTIONS, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// renew click.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void RenewButton_OnClick(object sender, EventArgs e)
        {
            BindModuleListByRoleType(ProxyPermissionType.RENEW_RECORD, ((AccelaLinkButton)sender).ClientID);
        }

        /// <summary>
        /// Keep focus.
        /// </summary>
        private void KeepFocus()
        {
            if (string.IsNullOrEmpty(hdnEditProxyUserRoleType.Value))
            {
                return;
            }

            ProxyPermissionType permissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnEditProxyUserRoleType.Value);

            switch (permissionType)
            {
                case ProxyPermissionType.VIEW_RECORD:
                    Page.FocusElement(btnViewRecordPermission.ClientID);
                    break;
                case ProxyPermissionType.CREATE_APPLICATION:
                    Page.FocusElement(btnCreateApplication.ClientID);
                    break;
                case ProxyPermissionType.RENEW_RECORD:
                    Page.FocusElement(btnRenew.ClientID);
                    break;
                case ProxyPermissionType.MANAGE_INSPECTIONS:
                    Page.FocusElement(btnManageInspections.ClientID);
                    break;
                case ProxyPermissionType.MANAGE_DOCUMENTS:
                    Page.FocusElement(btnManageDocuments.ClientID);
                    break;
                case ProxyPermissionType.MAKE_PAYMENTS:
                    Page.FocusElement(btnMakePayments.ClientID);
                    break;
                case ProxyPermissionType.AMENDMENT:
                    Page.FocusElement(btnAmendment.ClientID);
                    break;
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
            if (!string.IsNullOrEmpty(hdnEditProxyUserRoleType.Value))
            {
                ProxyPermissionType editPermissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnEditProxyUserRoleType.Value);

                //when click View Records in selected categories,the sub category should keep current state.
                if (!ProxyPermissionType.VIEW_RECORD.Equals(editPermissionType))
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
        ///  Register hot key to cancel button
        /// </summary>
        private void RegisterHotKey()
        {
            const string JSFormat = "OverrideTabKey(event, {0}, '{1}')";
            btnCancelPopup.Attributes.Add("onkeydown", string.Format(JSFormat, "false", hlBeginFocus.ClientID));
        }

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitUI()
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            hdnEditProxyUserRoleType.Value = string.Empty;
            InitDataSource();
            RefreshUI();

            lblDelegateUserName.Text = ScriptFilter.EncodeHtml(ProxyUserDataSource.proxyUserModel.nickName);
            lblProxyUserEmailAddress.Text = ProxyUserDataSource.email;

            if (ProxyUserDataSource.proxyUserModel.proxyStatus == ProxyUserStatus.P)
            {
                lblLastAccessedTime.Visible = false;
                lblAddedTime.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_invitation_sent_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(ProxyUserDataSource.proxyUserModel.createdDate));
            }
            else
            {
                lblLastAccessedTime.Visible = true;
                lblLastAccessedTime.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_delegate_last_accessed_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(ProxyUserDataSource.proxyUserModel.accessDate));
                lblAddedTime.Text = string.Format("{0}{1}{2}", LabelUtil.GetTextByKey("aca_delegate_add_on", string.Empty), ACAConstant.BLANK, I18nDateTimeUtil.FormatToDateStringForUI(ProxyUserDataSource.proxyUserModel.createdDate));
            }
        }

        /// <summary>
        /// Initializes Data Source.
        /// </summary>
        private void InitDataSource()
        {
            PermissionModules = new Hashtable();

            if (ProxyUserDataSource == null)
            {
                return;
            }

            List<XProxyUserPermissionModel> allPermissions = ProxyUserDataSource.proxyUserModel.XProxyUserPermissionModels.ToList();
            allPermissions = allPermissions.Where(f => ModuleList.Contains(f.levelData)).ToList();
            List<XProxyUserPermissionModel> permissions = allPermissions.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE).ToList();

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
                permissionModel.proxyUserSeqNbr = long.Parse(DelegateUserSeqNum);
                permissionModel.levelType = ACAConstant.LEVEL_TYPE_MODULE;
                permissionModel.levelData = module;
                permissionModel.permission = permissions;
                permissionModels.Add(permissionModel);
            }

            return permissionModels;
        }

        /// <summary>
        /// Refresh UI.
        /// </summary>
        private void RefreshUI()
        {
            foreach (ProxyPermissionType permissionType in (ProxyPermissionType[])Enum.GetValues(typeof(ProxyPermissionType)))
            {
                if (!string.IsNullOrEmpty(hdnEditProxyUserRoleType.Value))
                {
                    ProxyPermissionType editPermissionType = (ProxyPermissionType)Enum.Parse(typeof(ProxyPermissionType), hdnEditProxyUserRoleType.Value);

                    if (editPermissionType != ProxyPermissionType.VIEW_RECORD && editPermissionType != permissionType)
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

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "RefreshDialogHeader", "$(getParentDocument()).find('#ACA_Dialog_Header_Mask').addClass('ACA_Hide');", true);
        }

        /// <summary>
        /// Bind module list.
        /// </summary>
        /// <param name="permissionType">The permission type.</param>
        /// <param name="focusBackCtrolID">The FocusBackControlID.</param>
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

                cblModuleList.Items.Add(li);
            }

            ScriptManager.RegisterStartupScript(this.Page, GetType(), permissionType.ToString(), "ShowEditModuleList('" + permissionType.ToString() + "','" + hdnEditProxyUserRoleType.ClientID + "','" + hlBeginFocus.ClientID + "','" + focusBackCtrolID + "');", true);
        }

        /// <summary>
        /// Get check box according to permission type.
        /// </summary>
        /// <param name="permissionType">ENUM ProxyPermissionType</param>
        /// <returns>the check box.</returns>
        private AccelaCheckBox GetCkeckBoxByPermissionType(ProxyPermissionType permissionType)
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
    }
}