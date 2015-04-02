#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSetting.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: GlobalSetting.aspx.cs 277850 2014-08-22 02:33:36Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Admin
{
    /// <summary>
    /// Global setting for ACA 
    /// </summary>
    public partial class GlobalSetting : AdminBasePage
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

                chkGISActivate.Attributes.Add("onclick", "EnableGis();");
                chkDocTypeActivate.Attributes.Add("onclick", "UpdateDataInfo();");
                chkCountryCodeActivate.Attributes.Add("onclick", "UpdateDataInfo();");
                chkCapListCheckBoxOption.Attributes.Add("onclick", "UpdateDataInfo();");
                txtGISPortletURL.Attributes.Add("onchange", "UpdateDataInfo();");
                txtNewGISPortletURL.Attributes.Add("onchange", "UpdateDataInfo();");
                chkExportActivate.Attributes.Add("onclick", "EnableExport();");
                chkShoppingCartActivate.Attributes.Add("onclick", "EnableShoppingCart();");
                cbxEnableProxyUser.Attributes.Add("onclick", "EnableProxyUser();");
                chkGlobalSearchSwitch.Attributes.Add("onclick", "UpdateGlobalSearchSwitch();");
                chkGlobalSearchCAPResultGroup.Attributes.Add("onclick", "UpdateGlobalSearchSubGroup(this);");
                chkGlobalSearchLPResultGroup.Attributes.Add("onclick", "UpdateGlobalSearchSubGroup(this);");
                chkGlobalSearchAPOResultGroup.Attributes.Add("onclick", "UpdateGlobalSearchSubGroup(this);");
                ddlTransactionType.Attributes.Add("onchange", "UpdateDataInfo();");
                txtSelectExpirationDay.Attributes.Add("onchange", "UpdateDataInfo();");
                txtSaveExpirationDay.Attributes.Add("onchange", "UpdateDataInfo();");

                ddlScriptName.Attributes.Add("onchange", "UpdateDataInfo();");

                txtOfficialWebSite.Attributes.Add("onchange", "UpdateDataInfo();");

                cbxUserInitialDisplay.Attributes.Add("onclick", "UpdateDataInfo();");

                chkPayFeeLinkActivate.Attributes.Add("onclick", "EnablePayFeeLink();");

                chkDecimalFeeItem.Attributes.Add("onclick", "UpdateDataInfo();");

                chkCrossModuleEnabled.Attributes.Add("onclick", "UpdateDataInfo();");
                chkSearchMyLicense.Attributes.Add("onclick", "UpdateDataInfo();");
                chkFeinMaskingActivate.Attributes.Add("onclick", "UpdateDataInfo();");
                chkAccessibilityActivate.Attributes.Add("onclick", "UpdateDataInfo();");
                chkLicenseStateActivate.Attributes.Add("onclick", "EnableLicenseState()");
                txtProxyUserExpiredDate.Attributes.Add("onchange", "UpdateDataInfo();");
                txtProxyUserExpiredRemoveDate.Attributes.Add("onchange", "UpdateDataInfo();");
                cbxEnableParcelGen.Attributes.Add("onclick", "UpdateDataInfo();");

                chkAnnouncementActivate.Attributes.Add("onclick", "EnableAnnouncement();");
                txtAnnouncementInterval.Attributes.Add("onchange", "UpdateDataInfo();");
                chkEnableAutoUpdate.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableAccountAttachment.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableContactAddressEdit.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableAccountEduExamCEInput.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoNewTemplate.Attributes.Add("onclick", "UpdateDataInfo();");
                rdoClassicTemplate.Attributes.Add("onclick", "UpdateDataInfo();");
                chkEnableContactCrossAgency.Attributes.Add("onclick", "UpdateDataInfo();");

                LoadDataInfo();
                BindReportRoleData();
                LoadCustomizedCSS();
                LoadPeopleSearchSettings();
                LoadContactSettings();

                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execGis", "<script type='text/javascript'>GetGisDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getScript", "<script type='text/javascript'>GetScriptNameInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execCountryCode", "<script type='text/javascript'>GetCountryCodeDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execOfficalWebSite", "GetOfficialWebSiteInfo();", true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execExport", "<script type='text/javascript'>GetExportDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execUserInitial", "GetUserInitialDisplayInfo();", true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execShoppingCart", "<script type='text/javascript'>GetShoppingCartDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execGlobalSearch", "<script type='text/javascript'>GetGlobalSearchDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execCheckBoxOption", "<script type='text/javascript'>GetCheckBoxOptionInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execPayFee", "<script type='text/javascript'>GetPayFeeLinkStatus();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execDecimalFeeItem", "<script type='text/javascript'>GetDecimalStatusForFeeItem();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getSwitchValueOfCrossModuleSearch", "GetSwitchValueOfCMSearch();", true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getSearchMyLicense", "GetSearchMyLicense();", true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getFeinMaskingSettings", "<script type='text/javascript'>GetFeinMaskingDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getAccessibilitySettings", "<script type='text/javascript'>GetAccessibilityDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getLicenseStateSettings", "<script type='text/javascript'>GetLicenseStateInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execProxyUser", "<script type='text/javascript'>GetProxyUserDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execParcelGenealogy", "<script type='text/javascript'>GetParcelGenealogyDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getAnnouncementDataInfo", "<script type='text/javascript'>GetAnnouncementDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execExaminationData", "<script type='text/javascript'>GetExaminationStatus();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "execUploadInspectionResultStatus", string.Format("<script type='text/javascript'>GetUploadInspectionResultStatus('{0}');</script>", XPolicyConstant.ENABLE_AUTO_UPDATE_INSPECTION_RESULT));
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getAccountAttachmentDataInfo", "<script type='text/javascript'>GetAccountAttachmentDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getEnableContactAddressEditDataInfo", "<script type='text/javascript'>GetEnableContactAddressEditDataInfo();</script>");
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getEnableContactAddressDeactivateInfo", string.Format("<script type='text/javascript'>GetEnableContactAddressDeactivateInfo('{0}');</script>", XPolicyConstant.ENABLE_CONTACT_ADDRESS_DEACTIVATE));
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getEnableAccountEduExamCEInputInfo", "GetEnableAccountEduExamCEInputInfo('" + XPolicyConstant.ENABLE_ACCOUNT_EDU_EXAM_CE_INPUT + "');", true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "getEnableNewTemplate", "GetEnableNewTemplate('" + XPolicyConstant.ENABLE_NEW_TEMPLATE + "');", true);
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "GetEnableContactCrossAgency", "GetEnableContactCrossAgency('" + XPolicyConstant.ENABLE_GET_CONTACT_FROM_OTHER_AGENCY + "');", true);
            }

            Page.Header.DataBind();
        }

        /// <summary>
        /// Get Report role data and output them to render.
        /// </summary>
        private void BindReportRoleData()
        {
            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

            // in global setting, pass the module name as empty.
            string roleDatas = reportBll.GetReportRoles(string.Empty);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "ReportRoles", roleDatas, true);
        }

        /// <summary>
        /// Bind shopping cart transaction type
        /// </summary>
        private void BindShoppingCartTransactionType()
        {
            ddlTransactionType.Items.Add(new ListItem(GetTextByKey("admin_global_setting_transaction_shoppingcart"), ((int)ShoppingCartTransactionType.TransactionPerCart).ToString()));
            ddlTransactionType.Items.Add(new ListItem(GetTextByKey("admin_global_setting_transaction_cap"), ((int)ShoppingCartTransactionType.TransactionPerRecord).ToString()));

            ddlTransactionType.DataBind();
        }

        /// <summary>
        /// This method is to get page data.
        /// </summary>
        private void LoadDataInfo()
        {
            this.BindShoppingCartTransactionType();
        }

        /// <summary>
        /// Check whether multiple language is supported or not
        /// </summary>
        /// <returns>true if multiple language is supported; otherwise, false.</returns>
        private bool MultiLanguageSupportEnable()
        {
            return I18nCultureUtil.IsMultiLanguageEnabled;
        }

        /// <summary>
        /// Load admin user customized CSS content
        /// </summary>
        private void LoadCustomizedCSS()
        {
            string cssText = LabelUtil.GetTextContentByKey("aca_css_customizedstyle", ConfigManager.AgencyCode);

            if (!string.IsNullOrEmpty(cssText))
            {
                this.txtCssEditor.Value = cssText;
            }
        }

        /// <summary>
        /// Load people search settings.
        /// </summary>
        private void LoadPeopleSearchSettings()
        {
            var xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));

            if (ValidationUtil.IsYes(xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_REFERENCE_CONTACT_SEARCH)))
            {
                chkRefContactSearchEnabled.Checked = true;
            }
            else
            {
                chkRefContactSearchEnabled.Checked = false;
            }

            if (ValidationUtil.IsYes(xPolicyBll.GetValueByKey(XPolicyConstant.ENABLE_REFERENCE_LP_SEARCH)))
            {
                chkRefLPSearchEnabled.Checked = true;
            }
            else
            {
                chkRefLPSearchEnabled.Checked = false;
            }
        }

        /// <summary>
        /// Load Contact Settings
        /// </summary>
        private void LoadContactSettings()
        {
            if (StandardChoiceUtil.IsEnabelManualContactAssociation())
            {
                chkEnableManualContactAssociation.Checked = true;
            }
            else
            {
                chkEnableManualContactAssociation.Checked = false;
                chkAutoActiveNewAssociatedContact.InputAttributes.Add("disabled", "true");
            }

            if (StandardChoiceUtil.IsAutoActivateNewAssociatedContact())
            {
                chkAutoActiveNewAssociatedContact.Checked = true;
            }
            else
            {
                chkAutoActiveNewAssociatedContact.Checked = false;
            }

            if (StandardChoiceUtil.IsEnableContactAddressMaintenance())
            {
                chkEnableContactAddressMaintenance.Checked = true;
            }
            else
            {
                chkEnableContactAddressMaintenance.Checked = false;
            }

            chkEnableManualContactAssociation.Attributes.Add("onclick", "UpdateDataInfo();");
            chkAutoActiveNewAssociatedContact.Attributes.Add("onclick", "UpdateDataInfo();");
            chkEnableContactAddressMaintenance.Attributes.Add("onclick", "UpdateDataInfo();");
            chkEnableContactAddressDeactivate.Attributes.Add("onclick", "UpdateDataInfo();");
        }

        #endregion Methods
    }
}
