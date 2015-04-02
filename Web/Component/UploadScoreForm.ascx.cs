#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UploadScoreForm.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: UploadScoreForm.ascx.cs 123865 2009-03-16 09:40:24Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// examination upload scores control.
    /// </summary>
    public partial class UploadScoreForm : FileUploadBase
    {
        /// <summary>
        /// Upload handler URL.
        /// </summary>
        protected readonly string UPLOAD_HANDLER_URL = FileUtil.AppendApplicationRoot("Handlers/FileHandler.ashx");

        /// <summary>
        /// default Examination Score Error Message Count for Check.
        /// </summary>
        private const int EXAMINATION_SCORE_ERROR_MESSAGE_COUNT = 10;

        /// <summary>
        /// CSV extension validate
        /// </summary>
        private const string EXTENSION_CSV = ".CSV";

        /// <summary>
        /// Gets or sets DocumentSource
        /// </summary>
        protected FileUploadInfo DocumentSource
        {
            get
            {
                if (ViewState["DocumentSource"] == null)
                {
                    ViewState["DocumentSource"] = new FileUploadInfo();
                }

                return ViewState["DocumentSource"] as FileUploadInfo;
            }

            set
            {
                ViewState["DocumentSource"] = value;
            }
        }

        /// <summary>
        /// Gets Initial Parameters for Upload control.
        /// </summary>
        protected string InitParams
        {
            get
            {
                var paramString = new StringBuilder();
                paramString.Append("fileId=");
                paramString.Append(DocumentSource.FileId);
                paramString.Append(ACAConstant.COMMA);
                paramString.Append("fileState=");
                paramString.Append(DocumentSource.StateString);

                return paramString.ToString();
            }
        }

        /// <summary>
        /// page load event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event  argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DialogUtil.RegisterScriptForDialog(this.Page);
                DropDownListBindUtil.BindProviderByUserSeqNbr(ddlLicenseID);
            }

            if (!AppSession.IsAdmin)
            {
                fileSelect.FunctionRunInSelectCompleted = string.Format("{0}_FillDocEditForm", ClientID);
                CurrentFileUploadBehavior = StandardChoiceUtil.GetFileUploadBehavior();
            }
        }
            
        /// <summary>
        /// Save upload scores file.
        /// </summary>
        /// <param name="sender">Save button sender</param>
        /// <param name="e">Click event</param>
        protected void Save(object sender, EventArgs e)
        {
            string providerName = string.Empty;
            string providerNumber = string.Empty;
            string licenseId = string.Empty;

            var fileInfo = DocumentSource;
            string uploadFolder = AttachmentUtil.GetTempDirectory();
            string fileExtName = Path.GetExtension(fileInfo.FileName);
            var filePath = Path.Combine(uploadFolder, fileInfo.FileName + fileInfo.FileId);

            if (ddlLicenseID.SelectedItem != null && !string.IsNullOrEmpty(ddlLicenseID.SelectedItem.Text))
            {
                string[] providerInfo = ddlLicenseID.SelectedItem.Text.Split(ACAConstant.SPLIT_CHAR);

                if (providerInfo != null && providerInfo.Length == 2)
                {
                    providerName = providerInfo[0];
                    providerNumber = providerInfo[1];
                    licenseId = ddlLicenseID.SelectedItem.Value;
                }
            }

            try
            {
                //do csv extension validation.if passing extension validation,will do csv format validation. 
                bool formatValidation = EXTENSION_CSV.Equals(fileExtName, StringComparison.OrdinalIgnoreCase);
                string checkMessage = formatValidation ?
                    CSVUtil.CheckCSVFormat(filePath, BizDomainConstant.STD_EXAM_CSV_FORMAT, EXAMINATION_SCORE_ERROR_MESSAGE_COUNT, providerName, providerNumber) :
                    LabelUtil.GetTextByKey("aca_fileuploadpage_checkfiletype_tip", null);

                if (string.IsNullOrEmpty(checkMessage))
                {
                    //Build Attachment
                    AttachmentModel attachmentModel = new AttachmentModel();
                    attachmentModel.attachmentContent = File.ReadAllBytes(filePath);
                    attachmentModel.auditID = AppSession.User.PublicUserId;
                    attachmentModel.serviceProviderCode = ConfigManager.AgencyCode;
                    attachmentModel.fileName = fileInfo.FileName;
                    attachmentModel.scoreFileFlag = ACAConstant.COMMON_Y;

                    //Build Provider
                    ProviderPKModel providerSelect = new ProviderPKModel();
                    providerSelect.providerNbr = Convert.ToInt64(licenseId);
                    providerSelect.serviceProviderCode = ConfigManager.AgencyCode;

                    //Update ExamCSV
                    IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                    EMSEResultBaseModel4WS result = examinationBll.UpdateExamListByCSV(attachmentModel, providerSelect);

                    if (result != null && ValidationUtil.IsNumber(result.returnCode))
                    {
                        int returnCode = Convert.ToInt32(result.returnCode);

                        switch (returnCode)
                        {
                            case (int)UploadExamReturnCode.UPLOAD_SUCCESSFUL:
                                MessageUtil.ShowMessageByControl(Page, MessageType.Success, LabelUtil.GetTextByKey("csv_parse_upload_success", null));
                                break;

                            case (int)UploadExamReturnCode.SUCCESSFUL:
                                MessageUtil.ShowMessageByControl(Page, MessageType.Success, LabelUtil.GetTextByKey("csv_parse_upload_autoupdate_success", null));
                                break;

                            case (int)UploadExamReturnCode.EMPTY_LIST:
                                MessageUtil.ShowMessageByControl(Page, MessageType.Error, LabelUtil.GetTextByKey("csv_parse_upload_empty_exam_list", null));
                                break;

                            case (int)UploadExamReturnCode.CONFIG_ERROR:
                                MessageUtil.ShowMessageByControl(Page, MessageType.Error, LabelUtil.GetTextByKey("csv_parse_upload_configuration_error", null));
                                break;

                            case (int)UploadExamReturnCode.AUTO_UPDATE_EXAM_FAILED:
                                MessageUtil.ShowMessageByControl(Page, MessageType.Error, LabelUtil.GetTextByKey("csv_parse_autoupdate_exam_failed", null));
                                break;

                            default:
                                MessageUtil.ShowMessageByControl(Page, MessageType.Error, result.returnMessage);
                                break;
                        }
                    }
                }
                else
                {
                    checkMessage = LabelUtil.GetTextByKey("csv_parse_error_message_title", null) + ACAConstant.HTML_BR + ACAConstant.HTML_BR + checkMessage;
                    MessageUtil.ShowMessageByControl(Page, MessageType.Error, checkMessage);
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
            }
            catch (Exception ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message.Replace("\n", "<br>").Replace("\r", string.Empty));
            }
            finally
            {
                FileUtil.DeleteFile(filePath);
                DocumentSource = null;

                //Focus on "Browse" button after document info saved.
                Page.FocusElement(btnBrowse.ClientID);
            }
        }

        /// <summary>
        /// Clear all document.
        /// </summary>
        /// <param name="sender">Clear all button</param>
        /// <param name="e">Click event</param>
        protected void RemoveAll(object sender, EventArgs e)
        {
            //Remove related files from temp folder.
            AttachmentUtil.DeleteFileFromTempFolder(new[] { DocumentSource });
            DocumentSource = null;

            //Focus on "Browse" button after document edit section been cleared.
            string btnClientId;
            switch (CurrentFileUploadBehavior)
            {
                case FileUploadBehavior.Basic:
                    btnClientId = divFileSelect.ClientID;
                    Page.FocusElement(btnClientId);
                    break;
                case FileUploadBehavior.Advanced:
                    btnClientId = btnBrowse.ClientID;
                    Page.FocusElement(btnClientId);
                    break;
                case FileUploadBehavior.Html5:
                    btnClientId = divFileBrowser.ClientID;
                    string script = string.Format("setTimeout(function () {{ $('#{0}').find('a').focus();}}, 0);", btnClientId);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), btnClientId + CommonUtil.GetRandomUniqueID().Substring(0, 6), script, true);
                    break;
            }
        }

        /// <summary>
        /// Fill filename to form.
        /// </summary>
        /// <param name="sender">Button document edit</param>
        /// <param name="e">Click event</param>
        protected void FillDocEditForm(object sender, EventArgs e)
        {
            string senderArgs = Request.Form[System.Web.UI.Page.postEventArgumentID];
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            FileUploadInfo[] fileInfos = jsSerializer.Deserialize<FileUploadInfo[]>(senderArgs);

            if (fileInfos != null && fileInfos.Length > 0)
            {
                DocumentSource = fileInfos[0];
                lblFileName.Value = DocumentSource.FileName;
                lblFileName.IsNeedProgress = !AppSession.IsAdmin && CurrentFileUploadBehavior != FileUploadBehavior.Advanced;
                lblFileName.FileUploadBehavior = CurrentFileUploadBehavior;

                // Start file upload if some file do not upload complete.
                if (!AppSession.IsAdmin && !ACAConstant.FINISHED_STATUS.Equals(DocumentSource.StateString, StringComparison.InvariantCultureIgnoreCase))
                {
                    string script = string.Empty;

                    if (CurrentFileUploadBehavior == FileUploadBehavior.Html5)
                    {
                        script = string.Format("upload_{0}.startUpload('{1}','{2}', '{3}_table');", ClientID, fileInfos[0].FileId, UploadUrl, lblFileName.ClientID);
                    }
                    else if (CurrentFileUploadBehavior == FileUploadBehavior.Basic)
                    {
                        script = fileSelect.ClientID + "_StartUpload('" + DocumentSource.FileId + "');";
                    }

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowProgressBar" + DocumentSource.FileId, script, true);
                }
            }

            // Focus on "Save" button after user click "Finish" button in file upload dialog.
            Page.FocusElement(btnSave.ClientID);
        }

        /// <summary>
        /// Overwrite PreRender.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                string maxFileSize = AttachmentUtil.GetMaxFileSizeWithUnit(ModuleName);
                lblSizeIntroduction.Visible = false;

                if (!string.IsNullOrEmpty(maxFileSize))
                {
                    lblSizeIntroduction.Visible = true;
                    lblSizeIntroduction.Text = GetTextByKey("aca_fileupload_label_sizelimitation").Replace(AttachmentUtil.FileUploadVariables.MaximumFileSize, maxFileSize);
                }
            }

            base.OnPreRender(e);
            RegisterInitButtonScript();
            lblFileName.ProgressParams = InitParams;
            divAttachmentField.Visible = AppSession.IsAdmin;
        }

        /// <summary>
        /// Register Initial Button Script
        /// </summary>
        private void RegisterInitButtonScript()
        {
            var isShow = !string.IsNullOrEmpty(DocumentSource.FileId) || AppSession.IsAdmin;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InitButtons", "if(typeof (InitButtons) != 'undefined'){InitButtons(" + isShow.ToString().ToLower() + ");}", true);
        }
    }
}