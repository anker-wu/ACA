#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionSetting.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionSetting.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.AdminBLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Admin
{
    /// <summary>
    /// Module setting for ACA 
    /// </summary>
    public partial class InspectionSetting : AdminBasePage
    {
        #region Fields

        /// <summary>
        /// label key list for user role
        /// </summary>
        private string[] inspectionLabelKeyList = 
                                        {
                                            "admin_inspection_setting_label_schedule_all_aca_user",
                                            "admin_searchrole_gridtitle_registeredusers",
                                            "admin_inspection_setting_label_schedule_cap_creator",
                                            "admin_inspection_setting_label_schedule_cap_contact_user", 
                                            "admin_inspection_setting_label_schedule_cap_owner_user", 
                                            "admin_inspection_setting_label_schedule_associated"
                                        };

        /// <summary>
        /// label key list for user role
        /// </summary>
        private string[] contactTypeLabelKeyList = 
                                        {
                                            "admin_inspection_setting_label_schedule_all_aca_user",
                                            "admin_inspection_setting_label_schedule_cap_creator",
                                            "admin_inspection_setting_label_schedule_associated", 
                                            "admin_inspection_setting_label_schedule_cap_contact_user", 
                                            "admin_inspection_setting_label_schedule_cap_owner_user"
                                        };
        #endregion Fields

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
                chkDisplayMap4ShowObject.Enabled = HasGISSettings();
                chkDisplayMap4SelectObject.Enabled = chkDisplayMap4ShowObject.Enabled;
                this.DataBind();
                chkDisplayMap4ShowObject.Attributes.Add("onclick", "UpdateDataInfo();");
                chkDisplayMap4SelectObject.Attributes.Add("onclick", "UpdateDataInfo();");
                cblUserType.Attributes.Add("onclick", "UpdateDataInfo();");
                cblInputContactUserType.Attributes.Add("onclick", "UpdateDataInfo();");
                cblViewContactUserType.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoDisplayOptionYes.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoDisplayOptionNo.Attributes.Add("onclick", "UpdateDataInfo();");
                chkCMSearchModules.Attributes.Add("onclick", "UpdateDataInfo();");
                ddlAmendmentButtonName.Attributes.Add("onchange", "ChangeButtonSetting();");
                chkDisplayEmail.Attributes.Add("onclick", "UpdateDataInfo();");
                chkDisplayPayFeeLink.Attributes.Add("onclick", "UpdateDataInfo();");
                chkCloneRecord.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableSearchASI.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableSearchContactTemplate.Attributes.Add("onclick", "UpdateDataInfo();");
                ddlInsGroup.Attributes.Add("onchange", "UpdateInsGroupDataInfo();");
               
                txtSharedComments.Attributes.Add("onchange", "UpdateDataInfo()");

                InitData();
                rdoAllowMultipleYes.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoAllowMultipleNo.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoDisplayDefaultContact4InspectionYes.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoDisplayDefaultContact4InspectionNo.Attributes.Add("onclick", "UpdateDataInfo();");

                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "GetLabelKey", "<script type='text/javascript'>GetInspectionLabelKeyInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "GetSD", "<script type='text/javascript'>GetInspectionSDInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "GetDisplayEmailSDInfo", "<script type='text/javascript'>GetDisplayEmailSDInfo();</script>");
                BindContactTypeRoleData();
                BindReportRoleData();
                BindCapTypeRoleData();
                BindCrossModuleSettings();
                BindLicenseVerificationData();
                BindRecordDetailRoleData();
                BindInspectionGroupData();
                BindSocialMediaSettings();
            }
        }

        /// <summary>
        /// Bind inspection list.
        /// </summary>
        private void BindInspectionGroupData()
        {
            //1.Get inspection groups.
            IInspectionTypeBll insTyepBll = (IInspectionTypeBll)ObjectFactory.GetObject(typeof(IInspectionTypeBll));
            InspectionGroupModel[] inspectionGroups = insTyepBll.GetInspectionGroups();

            //2.bind inspection groups to drop down list.
            List<ListItem> listItems = new List<ListItem>();

            if (inspectionGroups != null && inspectionGroups.Length > 0)
            {
                foreach (InspectionGroupModel inspectionGroup in inspectionGroups)
                {
                    if (inspectionGroup != null)
                    {
                        string text = I18nStringUtil.GetString(inspectionGroup.resGroupNme, inspectionGroup.groupName);
                        listItems.Add(new ListItem(text, inspectionGroup.groupCode));
                    }
                }
            }

            //it need not default item and need sort this item list.
            DropDownListBindUtil.BindDDL(listItems, ddlInsGroup, true, true);
        }

        /// <summary>
        /// Bind Schedule list.
        /// </summary>
        private void BindAmendmentButton()
        {
            ddlAmendmentButtonName.Items.Add(new ListItem(GetTextByKey("admin_global_setting_label_SelectButton"), string.Empty));
            ddlAmendmentButtonName.Items.Add(new ListItem(GetTextByKey("admin_global_setting_label_CreateAmendmentButton"), ACAConstant.BUTTON_CREATE_AMENDMENT)); //1:select CreateAmendMent button
            ddlAmendmentButtonName.Items.Add(new ListItem(GetTextByKey("admin_global_setting_label_createdocumentdelete"), ACAConstant.BUTTON_CREATE_DOCUMENT_DELETE));
        }

        /// <summary>
        /// Bind contact type role data to grid
        /// </summary>
        private void BindContactTypeRoleData()
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> contactItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_CONTACT_TYPE, false);
            IDictionary<string, UserRolePrivilegeModel> contactTypeRoles = GetContactRoleData(contactItems);

            //Bind grid data
            StringBuilder roleDatas = new StringBuilder("var RoleDataList=[");

            foreach (ItemValue contactitem in contactItems)
            {
                string role = "," + contactTypeRoles[contactitem.Key].allAcaUserAllowed.ToString().ToLower().Trim() + "," + contactTypeRoles[contactitem.Key].capCreatorAllowed.ToString().ToLower().Trim() + ","
                              + contactTypeRoles[contactitem.Key].licensendProfessionalAllowed.ToString().ToLower().Trim() + "," + contactTypeRoles[contactitem.Key].contactAllowed.ToString().ToLower().Trim() + ","
                              + contactTypeRoles[contactitem.Key].ownerAllowed.ToString().ToLower().Trim();

                roleDatas.Append("[");
                roleDatas.Append("\"" + contactitem.Key.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"");
                roleDatas.Append("," + "\"" + (contactitem.Value == null ? string.Empty : contactitem.Value.ToString().Replace("\\", "\\\\").Replace("\"", "\\\"")) + "\"");
                roleDatas.Append(role);
                roleDatas.Append("],");
            }

            if (contactItems.Count > 0)
            {
                roleDatas.Remove(roleDatas.Length - 1, 1);
            }

            roleDatas.Append("];");

            //Composition column name list
            string[] roleItems = new string[contactTypeLabelKeyList.Length];
            for (int i = 0; i < roleItems.Length; i++)
            {
                roleItems[i] = GetTextByKey(contactTypeLabelKeyList[i]).Trim();
            }

            roleDatas.Append("var ContactTypeDisplayColumnNames=[\"Contact ID\",\"" + GetTextByKey("ACA_Contact_Type") + "\",");

            for (int i = 0; i < roleItems.Length; i++)
            {
                roleDatas.Append("\"" + roleItems[i].Replace("\\", "\\\\").Replace("\"", "\\\"") + "\",");
            }

            roleDatas.Remove(roleDatas.Length - 1, 1);
            roleDatas.Append("];");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ContactTypeRoleData", roleDatas.ToString(), true);
        }

        /// <summary>
        /// Get Report role data and output them to render.
        /// </summary>
        private void BindReportRoleData()
        {
            string moduleName = Request["moduleName"];

            IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();
            string roleDatas = reportBll.GetReportRoles(moduleName);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ReportRoles", roleDatas, true);
        }

        /// <summary>
        /// Bind License Verification data to UI.
        /// </summary>
        private void BindLicenseVerificationData()
        {
            string moduleName = Request["moduleName"];
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            CapTypeModel[] capTypes = capTypeBll.GetCapTypeList4ACAByModule(moduleName, null);

            string licenseVerificationData = BuildLicenseVerificationData(capTypes);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "LicenseVerification", licenseVerificationData, true);
        }

        /// <summary>
        /// Get Cap Type role data and output them to render.
        /// </summary>
        private void BindRecordDetailRoleData()
        {
            if (string.IsNullOrEmpty(Request["moduleName"]))
            {
                return;
            }

            string moduleName = Request["moduleName"];

            Dictionary<string, string> allSections = CapUtil.GetCapDetailSectionLabels(moduleName);

            StringBuilder sectionRoleData = new StringBuilder();
            sectionRoleData.Append("var SectionRoleList=[");

            foreach (KeyValuePair<string, string> kvp in allSections)
            {
                string sectionName = kvp.Key;
                string sectionAliasName = allSections[sectionName];
                sectionAliasName = Regex.Replace(sectionAliasName, @"<.+?>", string.Empty, RegexOptions.IgnoreCase);
                sectionRoleData.Append("[\"");
                sectionRoleData.Append(ScriptFilter.EncodeJson(sectionName));
                sectionRoleData.Append("\",\"");
                sectionRoleData.Append(ScriptFilter.EncodeJson(sectionAliasName));
                sectionRoleData.Append("\",");
                sectionRoleData.Append(GetUserRolePermission(sectionName, moduleName));
                sectionRoleData.Append("],");
            }

            sectionRoleData.Remove(sectionRoleData.Length - 1, 1);
            sectionRoleData.Append("];");

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "RecordDetailSectionRole", sectionRoleData.ToString(), true);
        }

        /// <summary>
        /// Get User Role by section name
        /// </summary>
        /// <param name="sectionName">the section name</param>
        /// <param name="moduleName">the module name</param>
        /// <returns>user roles.</returns>
        private string GetUserRolePermission(string sectionName, string moduleName)
        {
            string userRolePermission = string.Empty;

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] xpolicys = xPolicyBll.GetPolicyListByCategory(BizDomainConstant.STD_ITEM_CAPDETAIL_SECTIONROLES, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (xpolicys != null && xpolicys.Length > 0)
            {
                foreach (XPolicyModel xpolicy in xpolicys)
                {
                    if (sectionName.Equals(xpolicy.data4) && EntityType.GENERAL.ToString().Equals(xpolicy.data3))
                    {
                        userRolePermission = xpolicy.data2;
                    }
                }
            }

            if (string.IsNullOrEmpty(userRolePermission)
                && (sectionName.Equals(CapDetailSectionType.EDUCATION.ToString())
                || sectionName.Equals(CapDetailSectionType.CONTINUING_EDUCATION.ToString())
                || sectionName.Equals(CapDetailSectionType.EXAMINATION.ToString())
                || sectionName.Equals(CapDetailSectionType.ASSETS.ToString())))
            {
                userRolePermission = "0000000000";
            }

            return FormatUserRolePermission(userRolePermission);
        }

        /// <summary>
        /// Get The Formatted User Role .
        /// </summary>
        /// <param name="userRolePermission">the Original Role.</param>
        /// <returns>Formatted User Role.</returns>
        private string FormatUserRolePermission(string userRolePermission)
        {
            StringBuilder formattedUserRolePermission = new StringBuilder();

            if (string.IsNullOrEmpty(userRolePermission))
            {
                formattedUserRolePermission.Append("1,1,1,1,1,1,0,0,0,0");
            }
            else
            {
                char[] userRolePermissionArray = userRolePermission.ToCharArray();
                foreach (char rolePermission in userRolePermissionArray)
                {
                    formattedUserRolePermission.Append(rolePermission.ToString()).Append(",");
                }

                formattedUserRolePermission.Remove(formattedUserRolePermission.Length - 1, 1);
            }

            return formattedUserRolePermission.ToString();
        }

        /// <summary>
        /// Build License Verification Data.
        /// </summary>
        /// <param name="capTypes">the cap type list.</param>
        /// <returns>licenseVerification data.</returns>
        private string BuildLicenseVerificationData(CapTypeModel[] capTypes)
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("var LicenseVerificationData=[");

            if (capTypes != null && capTypes.Length > 0)
            {
                foreach (CapTypeModel capType in capTypes)
                {
                    string capTypeValue = ScriptFilter.EncodeJson(CAPHelper.FormatCapTypeValue(capType));
                    string displayText = ScriptFilter.EncodeJson(CAPHelper.GetAliasOrCapTypeLabel(capType));
                    buf.Append("[\"");
                    buf.Append(capTypeValue);
                    buf.Append("\",\"");
                    buf.Append(displayText);
                    buf.Append("\"");
                    buf.Append("],");
                }

                buf.Remove(buf.Length - 1, 1);
            }

            buf.Append("];");

            return buf.ToString();
        }

        /// <summary>
        /// Bind module names for " cross module search" except current module name.
        /// </summary>
        private void BindCrossModuleSettings()
        {
            if (string.IsNullOrEmpty(Request["moduleName"]))
            {
                return;
            }

            string moduleName = Request["moduleName"];
            string xPolicyValue = string.Empty;
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ACA_ENABLE_CROSS_MODULE_SEARCH);

            if (string.IsNullOrEmpty(xPolicyValue) || xPolicyValue.Equals(ACAConstant.COMMON_N))
            {
                chkCMSearchModules.Enabled = false;
            }

            chkCMSearchModules.DataSource = TabUtil.GetAllEnableModules(true);
            chkCMSearchModules.DataBind();
            int itemIndex = -1;
            ListItemCollection items = chkCMSearchModules.Items;

            for (int i = 0; i < items.Count; i++)
            {
                if (moduleName.Equals(items[i].Value))
                {
                    itemIndex = i;
                }

                items[i].Attributes.Add("mainLanguageValue", items[i].Value);
            }

            if (!itemIndex.Equals(-1))
            {
                items.RemoveAt(itemIndex);
            }
        }

        /// <summary>
        /// Bind social media setting.
        /// </summary>
        private void BindSocialMediaSettings()
        {
            if (string.IsNullOrEmpty(Request["moduleName"]))
            {
                return;
            }

            string moduleName = Request["moduleName"];
            string xPolicyValue = string.Empty;
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            xPolicyValue = xPolicyBll.GetValueByKey(XPolicyConstant.SOCIAL_MEDIA_SHARE_BUTTON_PERMISSION, moduleName);
            if (string.IsNullOrEmpty(xPolicyValue) || SocialMediaButtonStatus.None.Equals(xPolicyValue))
            {
                rdoShareButtonWithNone.Checked = true;
            }
            else if (SocialMediaButtonStatus.Creator.Equals(xPolicyValue))
            {
                rdoShareButtonWithOwner.Checked = true;
            }
            else if (SocialMediaButtonStatus.All.Equals(xPolicyValue))
            {
                rdoShareButtonWithAll.Checked = true;
            }
            
            txtSharedComments.Value = LabelUtil.GetTextByKey(ACAConstant.ACA_SOCIALMEDIA_LABEL_COMMENTS_PATTERN, moduleName);
            lblShareButtonWithAll.InnerText = LabelUtil.GetTextByKey("acaadmin_modulesettings_label_sharebuttonwithall", moduleName);
            lblShareButtonWithOwner.InnerText = LabelUtil.GetTextByKey("acaadmin_modulesettings_label_sharebuttonwithowner", moduleName);
            lblShareButtonWithNone.InnerText = LabelUtil.GetTextByKey("acaadmin_modulesettings_label_sharebuttonwithnone", moduleName);
        }

        /// <summary>
        /// Get Cap Type role data and output them to render.
        /// </summary>
        private void BindCapTypeRoleData()
        { 
            string moduleName = Request["moduleName"].ToString();
            IAdminCapTypePermissionBll capTypePermissionBll = (IAdminCapTypePermissionBll)ObjectFactory.GetObject(typeof(IAdminCapTypePermissionBll));

            IXPolicyWrapper xpolicyWrapper = (IXPolicyWrapper)ObjectFactory.GetObject(typeof(IXPolicyWrapper));
            XpolicyUserRolePrivilegeModel policy = xpolicyWrapper.GetPolicy(ConfigManager.AgencyCode, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            bool isModuleLevel = policy == null || !ACAConstant.COMMON_ONE.Equals(policy.data4);

            string roleDatas = capTypePermissionBll.GetCapTypeRoles(moduleName, policy);

            if (isModuleLevel)
            {
                rdoModuleLevel.Checked = true;
            }
            else
            {
                rdoEachCapTypeLevel.Checked = true;
            }

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "CapTypeRoles", roleDatas.ToString(), true);
        }

        /// <summary>
        /// Build data for checkbox list.
        /// </summary>
        /// <param name="cbl">AccelaCheckBoxList control</param>
        private void BuildCheckboxList(AccelaCheckBoxList cbl)
        {
            for (int i = 0; i < inspectionLabelKeyList.Length; i++)
            {
                cbl.Items.Add(GetTextByKey(inspectionLabelKeyList[i].ToString()));
            }

            if (cbl.Items != null && cbl.Items.Count > 0)
            {
                cbl.Items[0].Attributes.Add("onclick", "DisplayCblStatus('" + cbl.ID + "')");
                cbl.Items[cbl.Items.Count - 1].Attributes.Add("onclick", "EnableOrDisableLPButton('" + cbl.ID + "', this)");
            }
        }

        /// <summary>
        /// Build Contact User Type Check box List
        /// </summary>
        /// <param name="checkboxlist">check box list</param>
        private void BuildContactUserTypeCheckboxList(AccelaCheckBoxList checkboxlist)
        {
            string[] userTypes = new string[]
                                            {
                                                GetTextByKey("admin_searchrole_gridtitle_acausers"),
                                                GetTextByKey("admin_searchrole_gridtitle_registeredusers"),
                                                GetTextByKey("admin_searchrole_gridtitle_recordcreator"),
                                                GetTextByKey("admin_searchrole_gridtitle_contact"),
                                                GetTextByKey("admin_searchrole_gridtitle_owner"),
                                                GetTextByKey("admin_searchrole_gridtitle_licensedprofessional")
                                            };

            for (int i = 0; i < userTypes.Length; i++)
            {
                checkboxlist.Items.Add(userTypes[i]);
            }

            checkboxlist.Items[0].Attributes.Add("onclick", "DisplayCblStatus('" + checkboxlist.ID + "')");
            checkboxlist.Items[checkboxlist.Items.Count - 1].Attributes.Add("onclick", "EnableOrDisableLPButton('" + checkboxlist.ID + "', this)");
        }

        /// <summary>
        /// Get role list of contact types for appoint module name.
        /// </summary>
        /// <param name="contactItems">All contact types</param>
        /// <returns>Role list of contact types</returns>
        private IDictionary<string, UserRolePrivilegeModel> GetContactRoleData(IList<ItemValue> contactItems)
        {
            string moduleName = Request["moduleName"].ToString();
            IXPolicyWrapper xpolicyWrapper = ObjectFactory.GetObject(typeof(IXPolicyWrapper)) as IXPolicyWrapper;
            IDictionary<string, UserRolePrivilegeModel> contactRoles = xpolicyWrapper.GetSelectedContactRoles(ConfigManager.AgencyCode, moduleName, contactItems);
            return contactRoles;
        }

        /// <summary>
        /// Gets current GIS configuration is active or not.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool HasGISSettings()
        {
            bool isActive = false;
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            BizDomainModel4WS bizDomainModel = new BizDomainModel4WS();
            bizDomainModel.bizdomain = BizDomainConstant.STD_CAT_ACA_CONFIGS;
            bizDomainModel.serviceProviderCode = ConfigManager.AgencyCode;
            bizDomainModel.dispositionID = 0;
            bizDomainModel.bizdomainValue = BizDomainConstant.STD_ITEM_GIS_PORLET_URL;
            bizDomainModel = bizBll.GetBizDomainListByModel(bizDomainModel, "Admin", true);
            if (bizDomainModel != null)
            {
                if (bizDomainModel.auditStatus != null)
                {
                    isActive = bizDomainModel.auditStatus == "I" ? false : true;
                }
            }

            return isActive;
        }

        /// <summary>
        /// Initialize data.
        /// </summary>
        private void InitData()
        {
            //Build user type checkboxlist.
            BuildCheckboxList(cblUserType);

            // build input contact checkbox
            BuildContactUserTypeCheckboxList(cblInputContactUserType);

            // build view contact checkbox
            BuildContactUserTypeCheckboxList(cblViewContactUserType);

            BindAmendmentButton();
        }

        #endregion Methods
    }
}
