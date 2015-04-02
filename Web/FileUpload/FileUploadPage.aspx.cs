#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileUploadPage.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FileUploadPage.aspx.cs 278108 2014-08-27 09:55:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Attachment
{
    /// <summary>
    /// Silverlight upload control container.
    /// </summary>
    public partial class FileUploadPage : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// Upload handler URL.
        /// </summary>
        protected readonly string UPLOAD_HANDLER_URL = FileUtil.AppendApplicationRoot("Handlers/FileHandler.ashx");

        #endregion

        #region Parameters for Upload control

        /// <summary>
        /// Gets disallowed file types.
        /// </summary>
        protected string DisallowedFileTypes
        {
            get
            {
                return AttachmentUtil.GetDisallowedFileType(ModuleName);
            }
        }

        /// <summary>
        /// Gets prefix for file ID.
        /// </summary>
        protected string FileKeyPrefix
        {
            get
            {
                return Session.SessionID;
            }
        }

        /// <summary>
        /// Gets Initial Parameters for Upload control.
        /// </summary>
        protected string InitParams
        {
            get
            {
                string handlerUrl = string.Format("{0}://{1}", ConfigManager.Protocol, FileUtil.CombineWebPath(Request.Url.Authority, UPLOAD_HANDLER_URL));
                StringBuilder paramString = new StringBuilder();
                paramString.Append("uploadOnSelect=");
                paramString.Append(ACAConstant.COMMON_Y);
                paramString.Append(ACAConstant.COMMA);
                paramString.Append("uploadURL=");
                paramString.Append(handlerUrl + "?action=Add");
                paramString.Append(ACAConstant.COMMA);

                //Chunk Size is strem size per upload to service, ACA set Chunk Size = 1MB. 
                paramString.Append("ChunkSize=");
                paramString.Append(1024 * 1024);

                return paramString.ToString();
            }
        }

        /// <summary>
        /// Gets a value to indicating whether support multiple file select.
        /// </summary>
        protected string IsMultipleUpload
        {
            get
            {
                bool isMultipleUpload = ACAConstant.COMMON_Y.Equals(Request.QueryString["MultipleUpload"], StringComparison.OrdinalIgnoreCase);
                return isMultipleUpload ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            }
        }

        /// <summary>
        /// Gets current language code for upload control.
        /// </summary>
        protected string LanguageCode
        {
            get
            {
                var languageCode = I18nCultureUtil.GetLanguageCodeForSoapHandler();
                var regionalCode = I18nCultureUtil.GetRegionalCodeForSoapHandler();
                languageCode = string.Format("{0}_{1}", languageCode, regionalCode);

                return languageCode;
            }
        }

        /// <summary>
        /// Gets a value to transfer document number.
        /// </summary>
        protected string ParentDocumentNo
        {
            get
            {
                return Convert.ToString(Request.QueryString["documentNo"]);
            }
        }

        /// <summary>
        /// Gets a value to store current agency code.
        /// </summary>
        protected string ParentAgencyCode
        {
            get
            {
                return Convert.ToString(ScriptFilter.EncodeHtmlEx(Request.QueryString[UrlConstant.AgencyCode]));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        protected FileUploadBehavior CurrentFileUploadBehavior
        {
            get
            {
                if (ViewState["CurrentFileUploadBehavior"] == null)
                {
                    return FileUploadBehavior.Basic;
                }

                FileUploadBehavior behavior = FileUploadBehavior.Basic;
                FileUploadBehavior.TryParse(ViewState["CurrentFileUploadBehavior"].ToString(), true, out behavior);

                return behavior;
            }

            set
            {
                ViewState["CurrentFileUploadBehavior"] = value;
            }
        }

        /// <summary>
        /// Gets upload handler url.
        /// </summary>
        protected string UploadUrl
        {
            get
            {
                string handlerUrl = string.Format(
                                                "{1}://{2}?action=Add{0}ChunkSize={3}{0}uploadOnSelect={4}",
                                                ACAConstant.AMPERSAND,
                                                ConfigManager.Protocol,
                                                FileUtil.CombineWebPath(Request.Url.Authority, UPLOAD_HANDLER_URL),
                                                2 * 1024 * 1024,
                                                ACAConstant.COMMON_Y);

                return handlerUrl;
            }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets max file size string with unit.
        /// </summary>
        protected string MaxFileSizeWithUnit
        {
            get
            {
                return AttachmentUtil.GetMaxFileSizeWithUnit(ModuleName);
            }
        }

        /// <summary>
        /// Gets long value of max file size by byte.
        /// </summary>
        protected long MaxFileSizeByByte
        {
            get
            {
                return AttachmentUtil.ConvertToByteUnit(MaxFileSizeWithUnit);
            }
        }

        #endregion

        /// <summary>
        /// Handle page load event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Set pop up page title.
            SetPageTitleKey("ACA_FileUploadPage_PageTitle");

            if (!Page.IsPostBack)
            {
                List<LicenseModel4WS> lpList = AttachmentUtil.GetAvaliableLicense4PeopleDocument(ConfigManager.AgencyCode);
                List<ListItem> listItems = new List<ListItem>();

                divSelectLp.Visible = lpList != null && lpList.Count > 1 && Convert.ToBoolean(Request.QueryString["isLpUpload"]);

                foreach (LicenseModel4WS lpModel in lpList)
                {
                    ListItem listItem = new ListItem();
                    string currentUserName = string.Empty;

                    if (ContactType4License.Organization.ToString().Equals(lpModel.typeFlag, StringComparison.OrdinalIgnoreCase))
                    {
                        currentUserName = ScriptFilter.EncodeHtmlEx(lpModel.businessName);
                    }
                    else
                    {
                        currentUserName = UserUtil.FormatToFullName(
                            ScriptFilter.EncodeHtmlEx(lpModel.contactFirstName),
                            ScriptFilter.EncodeHtmlEx(lpModel.contactMiddleName),
                            ScriptFilter.EncodeHtmlEx(lpModel.contactLastName));
                    }

                    listItem.Text = currentUserName;
                    listItem.Value = lpModel.licSeqNbr;
                    listItems.Add(listItem);
                }

                DropDownListBindUtil.BindDDL(listItems, ddlProfessional);
            }

            //Display introduction for type and size
            string maxFileSize = AttachmentUtil.GetMaxFileSizeWithUnit(ModuleName);
            string sizeLimitation = GetTextByKey("aca_fileupload_label_sizelimitation");
            
            if (!string.IsNullOrEmpty(maxFileSize))
            {
                lblSizeIntroduction.Visible = true;
                lblSizeIntroduction.Text = sizeLimitation.Replace(AttachmentUtil.FileUploadVariables.MaximumFileSize, maxFileSize);
            }

            string disAllowedFileTypes = DisallowedFileTypes;
            string typeLimitation = GetTextByKey("aca_fileupload_label_typelimitation");
            
            if (!string.IsNullOrEmpty(disAllowedFileTypes))
            {
                lblTypeIntroduction.Visible = true;
                lblTypeIntroduction.Text = typeLimitation.Replace(AttachmentUtil.FileUploadVariables.ForbiddenFileFormats, disAllowedFileTypes);
            }

            CurrentFileUploadBehavior = StandardChoiceUtil.GetFileUploadBehavior();
        }

        #region Methods

        #endregion
    }
}