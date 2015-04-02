#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: FileUploadBase.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description:
*  The base user control for file upload.
*
*  Notes:
* $Id: FileUploadBase.cs $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2014      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Util;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The base user control for file upload.
    /// </summary>
    public abstract class FileUploadBase : BaseUserControl
    {
        /// <summary>
        /// Upload handler URL.
        /// </summary>
        protected readonly string UPLOAD_HANDLER_URL = FileUtil.AppendApplicationRoot("Handlers/FileHandler.ashx");

        /// <summary>
        /// Gets or sets a value to indicating whether support multiple file select.
        /// </summary>
        public string IsMultipleUpload
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value to transfer document number for resubmit link auto fill document information.
        /// </summary>
        public string ParentDocumentNo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value to store current agency code  for resubmit link auto fill document information.
        /// </summary>
        public string ParentAgencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public FileUploadBehavior CurrentFileUploadBehavior
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
        /// Gets long value of max file size by byte.
        /// </summary>
        protected long MaxFileSizeByByte
        {
            get
            {
                return AttachmentUtil.ConvertToByteUnit(MaxFileSizeWithUnit);
            }
        }

        /// <summary>
        /// Gets  initial parameter for upload control.
        /// </summary>
        protected string InitParams
        {
            get
            {
                StringBuilder paramString = new StringBuilder();
                paramString.AppendFormat("uploadOnSelect={0}", ACAConstant.COMMON_N);
                paramString.Append(ACAConstant.COMMA);

                paramString.AppendFormat("uploadURL={0}://{1}?action=Add", ConfigManager.Protocol, FileUtil.CombineWebPath(Request.Url.Authority, UPLOAD_HANDLER_URL));
                paramString.Append(ACAConstant.COMMA);

                // Add the SilverLight button's CSS style
                paramString.AppendFormat("FontFamily=Arial{0}FontSize=10{0}FontColor=#FF003366{0}FontWeight=SemiBold{0}Width={0}Height=24{0}Cursor=Hand", ACAConstant.COMMA);
                paramString.Append(ACAConstant.COMMA);

                //Chunk Size is stream size per upload to service, ACA set Chunk Size = 1MB. 
                paramString.AppendFormat("ChunkSize={0}", 1024 * 1024);

                return paramString.ToString();
            }
        }

        /// <summary>
        /// Gets the Windowless parameter for Silverlight object. 
        /// </summary>
        protected string Windowless
        {
            get
            {
                /*
                 * If section 508 enabled, disable Windowless param in IE browser let JAWS could read the Silverlight upload button.
                 * You need use composite key "Insert + Z" to disable “Virtual Cursor” function in JAWS.
                 */
                if (AccessibilityUtil.AccessibilityEnabled
                    && (Request.Browser.Browser.Equals("IE", StringComparison.OrdinalIgnoreCase)
                        || (Request.UserAgent != null && Regex.IsMatch(Request.UserAgent, @"Trident/7.*rv:11", RegexOptions.IgnoreCase))))
                {
                    return ACAConstant.COMMON_FALSE;
                }

                return ACAConstant.COMMON_TRUE;
            }
        }

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
        /// Gets upload handler url.
        /// </summary>
        protected string UploadUrl
        {
            get
            {
                string handlerUrl = string.Format(
                    "{0}://{1}?action=Add&ChunkSize={2}&uploadOnSelect={3}",
                    ConfigManager.Protocol,
                    FileUtil.CombineWebPath(Request.Url.Authority, UPLOAD_HANDLER_URL),
                    1024 * 1024,
                    ACAConstant.COMMON_Y);

                return handlerUrl;
            }
        }
    }
}