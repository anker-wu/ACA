/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ACAFormDesigner.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Common.Util;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;

/// <summary>
/// the class for default ACAFormDesigner.
/// </summary>
public partial class ACAFormDesigner : AdminBasePage
{
    #region Properties

    /// <summary>
    /// Gets or sets FormDesigner Source.
    /// </summary>
    public string FormDesignerSource
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the Initial Parameters for Form designer.
    /// </summary>
    public string InitParams
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the form name
    /// </summary>
    protected string FormName
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets TypeLabel
    /// </summary>
    protected string TypeLabel
    {
        get
        {
            string t = (string)ViewState["TypeLabel"];
            if (string.IsNullOrEmpty(t))
            {
                t = string.Empty;
            }

            return t;
        }

        set
        {
            ViewState["TypeLabel"] = value;
        }
    }

    #endregion Properties

    /// <summary>
    /// method for handle type change
    /// </summary>
    /// <param name="sender">object of sender</param>
    /// <param name="e">EventArgs object</param>
    public void Types_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadFormDesigner();
    }

    /// <summary>
    /// Initial event method
    /// </summary>
    /// <param name="e">EventArgs e</param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        string version = Application["Version"] as string;
        if (version == null)
        {
            string path = this.Server.MapPath("../ClientBin/ACAFormDesigner.xap");
            System.IO.FileInfo fileinfo = new System.IO.FileInfo(path);
            string fdDate = fileinfo.CreationTime.ToString();
            Application["Version"] = fdDate;
        }

        if (!IsPostBack)
        {
            this.ddlEntityType.Attributes.Add("onchange", "return ChangeTypeHandle()");
            string permissionValue = Request["permissionValue"];
            string viewId = Request.QueryString["viewId"];
            string moduleName = Request.QueryString["module"];

            switch (viewId)
            {
                case GviewID.LicenseEdit:
                case GviewID.LicenseeDetail:
                    DropDownListBindUtil.BindLicenseType(ddlEntityType);
                    break;

                case GviewID.ContactEdit:
                    DropDownListBindUtil.BindContactType(ddlEntityType, moduleName, ContactTypeSource.Transaction);
                    break;
                case GviewID.RegistrationContactForm:
                case GviewID.AddReferenceContactForm:
                case GviewID.ModifyReferenceContactForm:
                case GviewID.AuthAgentCustomerDetail:
                case GviewID.AuthAgentNewClerkContactForm:
                case GviewID.AuthAgentEditClerkContactForm:
                    DropDownListBindUtil.BindContactType(ddlEntityType, moduleName, ContactTypeSource.Reference);
                    break;

                case GviewID.Attachment:
                    DropDownListBindUtil.BindAllDocumentTypes(ddlEntityType, false);
                    break;

                case GviewID.PeopleAttachment:
                    DropDownListBindUtil.BindAllDocumentTypes(ddlEntityType, true);
                    break;
            }

            DropDownListBindUtil.SetSelectedValue(ddlEntityType, permissionValue);
        }
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
            LoadFormDesigner();
        }

        string viewId = Request.QueryString["viewId"];

        if (Request.QueryString["permissionLevel"] == GViewConstant.PERMISSION_PEOPLE
            || viewId == GviewID.Attachment
            || viewId == GviewID.PeopleAttachment)
        {
            switch (viewId)
            {
                case GviewID.LicenseEdit:
                case GviewID.LicenseeDetail:
                    TypeLabel = "License Type";
                    break;

                case GviewID.ContactEdit:
                case GviewID.RegistrationContactForm:
                case GviewID.AddReferenceContactForm:
                case GviewID.ModifyReferenceContactForm:
                case GviewID.AuthAgentCustomerDetail:
                case GviewID.AuthAgentNewClerkContactForm:
                case GviewID.AuthAgentEditClerkContactForm:
                    TypeLabel = "Contact Type";
                    break;

                case GviewID.Attachment:
                case GviewID.PeopleAttachment:
                    TypeLabel = "Document Type";
                    break;

                default:
                    divType.Visible = false;
                    break;
            }
        }
        else
        {
            divType.Visible = false;
        }
    }

    /// <summary>
    /// method for load FormDesigner
    /// </summary>
    protected void LoadFormDesigner()
    {
        string version = string.Empty;
        string state = Application["Version"] as string;
        if (!string.IsNullOrEmpty(state))
        {
            version = state;
        }
        else
        {
            version = System.DateTime.Now.ToString();
        }

        InitParams = GetBusinessParam();
        FormDesignerSource = ResolveUrl("~/ClientBin/ACAFormDesigner.xap?") + version;
    }

    /// <summary>
    /// Get the business parameter form url query string.
    /// </summary>
    /// <returns>business parameter string format as key=value,key=value</returns>
    private string GetBusinessParam()
    {
        StringBuilder paramString = new StringBuilder();
        string tmpPermissionValue = string.Empty;
        if (Request.QueryString.Count > 0)
        {
            if (!string.IsNullOrEmpty(ConfigManager.AgencyCode))
            {
                paramString.Append("servProvCode=");
                paramString.Append(ConfigManager.AgencyCode);
                paramString.Append(",");
            }

            string levelType = ACAConstant.LEVEL_TYPE_MODULE;
            string moduleName = Request.QueryString["module"];

            if (string.IsNullOrEmpty(moduleName) || string.Equals(moduleName, ConfigManager.AgencyCode, StringComparison.InvariantCultureIgnoreCase))
            {
                moduleName = ConfigManager.AgencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            paramString.Append("levelType=");
            paramString.Append(levelType);
            paramString.Append(",");

            paramString.Append("levelName=");
            paramString.Append(moduleName);
            paramString.Append(",");

            paramString.Append("callerId=");
            paramString.Append(ACAConstant.ADMIN_CALLER_ID);
            paramString.Append(",");

            if (!string.IsNullOrEmpty(Request.QueryString["viewId"]))
            {
                paramString.Append("viewId=");
                paramString.Append(Request.QueryString["viewId"]);
                paramString.Append(",");
            }

            if (!string.IsNullOrEmpty(Request.QueryString["permissionLevel"]))
            {
                paramString.Append("permissionLevel=");
                paramString.Append(Request.QueryString["permissionLevel"]);
                paramString.Append(",");
            }

            if (!string.IsNullOrEmpty(Request.QueryString["permissionValue"]))
            {
                tmpPermissionValue = Request.QueryString["permissionValue"];
            }

            if (Request.QueryString["permissionLevel"] == ACAConstant.PEOPLE 
                || Request.QueryString["viewId"] == GviewID.Attachment
                || Request.QueryString["viewId"] == GviewID.PeopleAttachment)
            {
                tmpPermissionValue = this.ddlEntityType.SelectedValue.Trim().Replace(ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL, string.Empty);
                this.LastSelectIndex.Value = this.ddlEntityType.SelectedIndex.ToString();
            }

            if (!string.IsNullOrEmpty(tmpPermissionValue))
            {
                paramString.Append("permissionValue=");
                paramString.Append(tmpPermissionValue);
                paramString.Append(",");
            }

            this.FormName = string.IsNullOrEmpty(Request.QueryString[UrlConstant.SECTION_NAME]) ? string.Empty : Request.QueryString[UrlConstant.SECTION_NAME];

            paramString.Append("serviceUrl=");
            paramString.Append(WebServiceConfig.GetDefaultConfigParameter().Url);
            paramString.Append(",");

            string loadingErrorMsg = LabelUtil.GetAdminUITextByKey("aca_form_desinger_load_error");
            string savingErrorMsg = LabelUtil.GetAdminUITextByKey("aca_form_desinger_save_error");

            if (!string.IsNullOrEmpty(loadingErrorMsg))
            {
                paramString.Append("loadingErrorMsg=");
                paramString.Append(loadingErrorMsg);
                paramString.Append(",");
            }

            if (!string.IsNullOrEmpty(savingErrorMsg))
            {
                paramString.Append("saveErrorMsg=");
                paramString.Append(savingErrorMsg);
                paramString.Append(",");
            }

            string languageCode = string.Empty;
            string regionalCode = string.Empty;

            languageCode = I18nCultureUtil.GetLanguageCodeForSoapHandler();
            regionalCode = I18nCultureUtil.GetRegionalCodeForSoapHandler();
            languageCode = string.Format("{0}_{1}", languageCode, regionalCode);

            paramString.Append("countryCode=");
            paramString.Append(regionalCode);
            paramString.Append(",");
            paramString.Append("langCode=");
            paramString.Append(languageCode);
            paramString.Append(",");
        }

        return paramString.ToString();
    }
}