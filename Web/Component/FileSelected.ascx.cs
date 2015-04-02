#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FileSelected.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *      $Id: FileSelected.ascx.cs 196284 2011-05-13 10:27:27Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Display Grade Style Information in Education,Examination Form
    /// </summary>
    public partial class FileSelected : FileUploadBase
    {
        /// <summary>
        /// Gets or sets the Condition number.
        /// </summary>
        public long ConditionNumber
        {
            get
            {
                if (ViewState["ConditionNumber"] == null)
                {
                    return 0;
                }

                return (long)ViewState["ConditionNumber"];
            }

            set
            {
                ViewState["ConditionNumber"] = value;
            }
        }
        
        /// <summary>
        /// Initial button display.
        /// </summary>
        /// <param name="fileName">the file name.</param>
        /// <param name="title">part of the title value. the value is condition document description</param>
        public void InitFileNameControl(string fileName, string title)
        {
            txtFileName.Text = fileName;

            string templateToolTip = GetTextByKey("aca_conditiondocument_filename|tip");
            templateToolTip = templateToolTip.Replace(AttachmentUtil.FileUploadVariables.UploadButtonLabel, GetTextByKey("aca_attachment_label_browsefile"));
            templateToolTip = templateToolTip.Replace(AttachmentUtil.FileUploadVariables.ConditionDocDescription, title);
            txtFileName.ToolTip = templateToolTip;

            CurrentFileUploadBehavior = StandardChoiceUtil.GetFileUploadBehavior();
        }

        /// <summary>
        /// Update file info.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void UpdateFileInfo_Click(object sender, EventArgs e)
        {
            string senderArgs = Request.Form[Page.postEventArgumentID];
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            FileUploadInfo[] fileInfoList = jsSerializer.Deserialize<FileUploadInfo[]>(senderArgs);

            if (fileInfoList == null || fileInfoList.Length == 0)
            {
                return;
            }

            // Start file upload if some file do not upload complete.
            if (!AppSession.IsAdmin)
            {
                var fileInfo = fileInfoList[0];
                txtFileName.Text = fileInfo.FileName;
                lblFileName.Visible = true;
                lblFileName.IsNeedProgress = true;
                lblFileName.ProgressParams = GetProgressBarParams(fileInfo.FileId, fileInfo.StateString);
                lblFileName.FileUploadBehavior = CurrentFileUploadBehavior;

                string btnClientID = fileInfo.FileSelectedID;
                string script;

                if (CurrentFileUploadBehavior != FileUploadBehavior.Html5)
                {
                    script = string.Format("StartUpload_{0}('{1}','{2}');", divFileSelect.ClientID, fileInfo.FileId, btnClientID);
                }
                else
                {
                    btnClientID = divHtml5Upload.ClientID;
                    script = string.Format("upload_{0}.startUpload('{1}','{2}', '{3}_table');", ClientID, fileInfo.FileId, UploadUrl, lblFileName.ClientID);
                }

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "StartUpload" + fileInfo.FileId, script, true);

                RegisterRemoveSingleFileEvent(fileInfo.FileId, btnClientID);
            }
        }

        /// <summary>
        /// Remove document event.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void RemoveDocumentButton_Click(object sender, EventArgs e)
        {
            try
            {
                AttachmentUtil.DeleteFileFromTempFolder(new[] { new FileUploadInfo() { FileId = hdFileId.Value, FileName = hdFileName.Value } });

                if (AttachmentUtil.DeleteConditionDocument(ModuleName, ConditionNumber))
                {
                    RefreshAttatchmentPanel();
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Control prepare render.
        /// </summary>
        /// <param name="e">Control on prepare render event</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AttachmentUtil.IsDisabledEDMS(ConfigManager.AgencyCode, ModuleName))
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapIDModel4WS capID = capModel != null ? capModel.capID : null;
                EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, ModuleName, AppSession.User.PublicUserId, capID);

                if (edmsPolicyModel != null)
                {
                    bool hasEdmsUploadRight = Convert.ToBoolean(edmsPolicyModel.uploadRight);
                    bool hasEdmsDeleteRight = Convert.ToBoolean(edmsPolicyModel.deleteRight);

                    btnUploadDocument.Visible = hasEdmsUploadRight && CurrentFileUploadBehavior != FileUploadBehavior.Html5;
                    divHtml5Upload.Visible = hasEdmsUploadRight && CurrentFileUploadBehavior == FileUploadBehavior.Html5;

                    btnRemoveDocument.Visible = hasEdmsDeleteRight;
                    btnRemoveDocument.ToolTip = GetTextByKey("aca_fileselected_label_remove");
                }
            }
            else
            {
                btnRemoveDocument.Visible = false;
                btnUploadDocument.Visible = false;
                divHtml5Upload.Visible = false;
            }
        }

        /// <summary>
        /// Gets progress bar parameter
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="state">The state</param>
        /// <returns>the progress bar parameter string</returns>
        private string GetProgressBarParams(string fileId, string state)
        {
            StringBuilder paramString = new StringBuilder();
            paramString.AppendFormat("fileId={0}", fileId);
            paramString.Append(ACAConstant.COMMA);
            paramString.AppendFormat("fileState={0}", state);

            return paramString.ToString();
        }

        /// <summary>
        /// Refresh attachment panel.
        /// </summary>
        private void RefreshAttatchmentPanel()
        {
            lblFileName.Text = string.Empty;
            txtFileName.Text = string.Empty;
            attatchmentPanel.Update();
        }

        /// <summary>
        /// Register remove single file event.
        /// </summary>
        /// <param name="fileId">file id</param>
        /// <param name="fileSelectedId">file selected id</param>
        private void RegisterRemoveSingleFileEvent(string fileId, string fileSelectedId)
        {
            string removeSingleFileScript = CurrentFileUploadBehavior != FileUploadBehavior.Html5 ?
                                            string.Format("RemoveSingleFile_{0}('{1}','{2}');", divFileSelect.ClientID, fileId, fileSelectedId) :
                                            string.Format("RemoveSingleFileByHTML5_{0}('{1}');", ClientID, fileId);

            btnRemoveDocument.Attributes.Add("onclick", removeSingleFileScript);
            removeButtonPanel.Update();
        }
    }
}
