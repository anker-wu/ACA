#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AttachmentEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AttachmentEdit.ascx.cs 123865 2009-03-16 09:40:24Z ACHIEVO\weiky.chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AttachmentEdit.
    /// </summary>
    public partial class AttachmentEdit : FileUploadBase
    {
        #region Fields

        /// <summary>
        /// remove command
        /// </summary>
        private const string REMOVE_COMMAND = "Remove";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this control is in cap confirm page or not.
        /// </summary>
        public bool IsInConfirmPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether attachmentEdit is in cap detail page.
        /// </summary>
        public bool IsDetailPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether attachmentEdit is in account manage page.
        /// </summary>
        public bool IsAccountManagerPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether attachmentEdit is in spear form.
        /// </summary>
        public bool IsInSpearForm
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether attachmentEdit for condition document.
        /// </summary>
        public bool IsForConditionDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the component name to indicate the correct Attachment Component because it can set multiple Attachment Components in the same page.
        /// </summary>
        public string ComponentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                if (ViewState["IsEditable"] == null)
                {
                    return true;
                }

                return Convert.ToBoolean(ViewState["IsEditable"]);
            }

            set
            {
                ViewState["IsEditable"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the url parameter indicating the upload whether from new UI or not.
        /// </summary>
        public bool IsUpLoadFromNewUI
        {
            get 
            { 
                return ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_UPLOAD_FROM_NEW_UI]);
            }
        }

        /// <summary>
        /// Gets or sets DocumentSource
        /// </summary>
        protected List<FileUploadInfo> DocumentSource
        {
            get
            {
                if (ViewState["DocumentSource"] == null)
                {
                    List<FileUploadInfo> list = new List<FileUploadInfo>();

                    if (AppSession.IsAdmin)
                    {
                        var fileInfo = new FileUploadInfo();
                        fileInfo.DocumentModel = new DocumentModel();
                        list.Add(fileInfo);
                    }
                    
                    ViewState["DocumentSource"] = list;
                }

                return ViewState["DocumentSource"] as List<FileUploadInfo>;
            }

            set
            {
                ViewState["DocumentSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handle <c>OnInit</c> event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnInit(EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);

            if (sm != null)
            {
                //Get special timeout configuration for DocumentUpload web service.
                //If can not get the configuration of DocumentUpload web service, then get default timeout of all web service.
                WebServiceParameter wsp = WebServiceConfig.GetWebServiceParameter(typeof(EDMSDocumentUploadWebServiceService));

                //Set the timeout for async postback (by UploadPanel) to avoid the javascript timeout error.
                //The unit of the WebServiceParameter.Timeout is millisecond.
                sm.AsyncPostBackTimeout = wsp.Timeout / 1000;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Clear all document.
        /// </summary>
        /// <param name="sender">clear all button</param>
        /// <param name="e">click event</param>
        protected void ClearAll(object sender, EventArgs e)
        {
            RegisterScript4ResubmitRemoveFile(DocumentSource);

            //Remove related files from temp folder.
            AttachmentUtil.DeleteFileFromTempFolder(DocumentSource);
            DocumentSource.Clear();
            BindDocumentSource();

            //Focus on "Browse" button after document edit section been cleared.
            FocusOnBrowserBtn();
        }

        /// <summary>
        /// document list data bound
        /// </summary>
        /// <param name="sender">data list document</param>
        /// <param name="e">data bind event</param>
        protected void DocumentEdit_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var fileInfo = e.Item.DataItem as FileUploadInfo;

                if (fileInfo == null)
                {
                    return;
                }

                var btnRemove = e.Item.FindControl("btnRemove") as AccelaButton;

                // Remove file info from file collection in javascript side.
                if (btnRemove != null && !string.IsNullOrEmpty(fileInfo.FileId))
                {
                    string script = string.Format("{0}_RemoveSingleFile('{1}')", ClientID, fileInfo.FileId);
                    btnRemove.Attributes.Add("onclick", script);
                }

                var docEdit = e.Item.FindControl("documentEdit") as DocumentEdit;

                if (docEdit != null && fileInfo.DocumentModel != null)
                {
                    fileInfo.DocumentModel.FileId = fileInfo.FileId;

                    /*
                     * Make sure each document has been associated with the corresponding attachment list.
                     * Since there may be multiple attachment lists on the same page, we need to find a way to distinguish each other.
                     * This is what the component name does.
                     */
                    DataList tmpDL = (DataList)sender;

                    string componentName = AttachmentUtil.ExtractComponentNameFromClientID(tmpDL.ClientID);

                    if (!string.IsNullOrEmpty(componentName) && string.IsNullOrEmpty(fileInfo.DocumentModel.componentName))
                    {
                        fileInfo.DocumentModel.componentName = componentName;
                    }
                    
                    docEdit.Display(fileInfo.DocumentModel, true);

                    var lblFileName = docEdit.FindControl("lblFileName") as AccelaNameValueLabel;

                    // Start file upload if some file do not upload complete.
                    if (!AppSession.IsAdmin && !ACAConstant.FINISHED_STATUS.Equals(fileInfo.DocumentModel.FileState, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (FileUploadBehavior.Basic == CurrentFileUploadBehavior)
                        {
                            var script = fileSelect.ClientID + "_StartUpload('" + fileInfo.FileId + "');";
                            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowProgressBar" + fileInfo.FileId, script, true);
                        }
                        else if (FileUploadBehavior.Html5 == CurrentFileUploadBehavior && !ACAConstant.UPLOADING_STATUS.Equals(fileInfo.DocumentModel.FileState, StringComparison.InvariantCultureIgnoreCase))
                        {
                            var script = string.Format(
                                "upload_{0}.startUpload('{1}', '{2}', '{3}_table');",
                                ClientID,
                                fileInfo.FileId,
                                UploadUrl,
                                lblFileName.ClientID);

                            if (!string.IsNullOrEmpty(fileInfo.UploaderName) && ACAConstant.RESUBMIT.Equals(fileInfo.DocumentModel.categoryByAction, StringComparison.InvariantCultureIgnoreCase))
                            {
                                script = string.Format(
                                    "$get('{0}_iframeAttachmentList').contentWindow.{1}.startUpload('{2}', '{3}', '{4}_table');",
                                    ClientID, 
                                    fileInfo.UploaderName, 
                                    fileInfo.FileId, 
                                    UploadUrl, 
                                    lblFileName.ClientID);
                            }

                            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ShowProgressBar" + fileInfo.FileId, script, true);
                            fileInfo.DocumentModel.FileState = ACAConstant.UPLOADING_STATUS;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// dlDocumentEdit Item command event handler.
        /// </summary>
        /// <param name="source">data list document</param>
        /// <param name="e">item command event</param>
        protected void DocumentEdit_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (REMOVE_COMMAND.Equals(e.CommandName, StringComparison.InvariantCultureIgnoreCase))
            {
                RefreshDocumentData();
                var fileInfo = DocumentSource[e.Item.ItemIndex];
                AttachmentUtil.DeleteFileFromTempFolder(new[] { fileInfo });
                DocumentSource.Remove(fileInfo);
                BindDocumentSource();
                
                //remove single file for resubmit
                RegisterScript4ResubmitRemoveFile(new List<FileUploadInfo>() { fileInfo });
                
                //Focus on "Browse" button after a document edit section been removed.
                FocusOnBrowserBtn();
            }
        }

        /// <summary>
        /// Save document information
        /// </summary>
        /// <param name="sender">save button</param>
        /// <param name="e">click event</param>
        protected void Save(object sender, EventArgs e)
        {
            string errorMessage = AttachmentUtil.UploadFile(ModuleName, DocumentSource);

            DocumentSource.Clear();
            BindDocumentSource();

            //Register a js function to refresh iframe that contains attachment list.
            //To resolve Async upload.
            string scriptKey = string.Format("{0}_AfterAttachmentUpload", ClientID);
            string script = string.Format("{0}_AfterAttachmentUpload();", ClientID);
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), scriptKey, script, true);

            //Focus on "Browse" button after document info saved.
            FocusOnBrowserBtn();
            
            //delay the error message show, so that it can scroll to the error message after set focus on browser button.
            string msg = string.IsNullOrEmpty(errorMessage) ? GetTextByKey("per_permitDetail_message_asyUploadSuccess") : errorMessage;
            MessageUtil.DelayShowMessageByControl(Page, string.IsNullOrEmpty(errorMessage) ? MessageType.Success : MessageType.Error, msg);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                btnBrowse.Attributes.Add("onclick", string.Format("return {0}_BrowseFile();", ClientID));

                fileSelect.ErrorMsgContainerId = ClientID + "_errorMessageLabel";
                fileSelect.FunctionRunInSelectCompleted = string.Format("{0}_FillDocEditForm", ClientID);
                fileSelect.FunctionRunInStartUpload = string.Format("{0}_DisableSaveAttachmentButton", ClientID);
                fileSelect.FunctionRunInAllFilesFinished = string.Format("{0}_DisableSaveAttachmentButton", ClientID);

                if (!IsEditable)
                {
                    divUploadButton.Visible = false;
                }

                CurrentFileUploadBehavior = IsUpLoadFromNewUI ? FileUploadBehavior.Html5 : StandardChoiceUtil.GetFileUploadBehavior();
            }

            txtValidateSaveAction.CheckControlValueValidateFunction = ClientID + "_CheckRequired4Document";
            DialogUtil.RegisterScriptForDialog(this.Page);

            if (!IsPostBack)
            {
                BindDocumentSource();

                //Resolve the Tab index issue in Opera browser.
                bool isOpera = Request.UserAgent != null && Request.UserAgent.IndexOf("opera", 0, StringComparison.OrdinalIgnoreCase) != -1;
                hlAttachmentListBegin.Visible = isOpera;
                hlAttachmentListEnd.Visible = isOpera;
                divAttachmentField.Visible = AppSession.IsAdmin;
                btnSelectFromAccountContainer.Visible = IsShowSelectFromAccountButton();

                btnSave.OnClientClick = string.Format("{0}_SaveDocument();", ClientID);
                btnClearAll.OnClientClick = string.Format("{0}_RemoveAllFiles();", ClientID);
                btnSelectFromAccount.OnClientClick = string.Format("{0}_btnSelectFromAccount_OnClientClick(this.id); return false;", ClientID);
            }

            RefreshDocumentData();
        }

        /// <summary>
        /// OnPreRender Event.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (AppSession.IsAdmin && IsInSpearForm)
            {
                lblRequiredDocumentTypesIntroduction.Visible = true;
                spanRequiredDocumentTypesIntroduction.Visible = true;
                return;
            }

            if (!AppSession.IsAdmin)
            {
                string maxFileSize = AttachmentUtil.GetMaxFileSizeWithUnit(ModuleName);
                string disAllowedFileTypes = AttachmentUtil.GetDisallowedFileType(ModuleName);
                string sizeIntroductionLabelKey = IsForConditionDocument ? "aca_conditiondocument_label_sizelimitation" : "aca_fileupload_label_sizelimitation";
                string typeIntroductionLabelKey = IsForConditionDocument ? "aca_conditiondocument_label_typelimitation" : "aca_fileupload_label_typelimitation";

                lblSizeIntroduction.Visible = false;
                lblTypeIntroduction.Visible = false;

                if (!string.IsNullOrEmpty(maxFileSize))
                {
                    lblSizeIntroduction.Visible = true;
                    lblSizeIntroduction.Text = GetTextByKey(sizeIntroductionLabelKey).Replace(AttachmentUtil.FileUploadVariables.MaximumFileSize, maxFileSize);
                }

                if (!string.IsNullOrEmpty(disAllowedFileTypes))
                {
                    lblTypeIntroduction.Visible = true;
                    lblTypeIntroduction.Text = GetTextByKey(typeIntroductionLabelKey).Replace(AttachmentUtil.FileUploadVariables.ForbiddenFileFormats, disAllowedFileTypes);
                }

                if (DocumentSource != null && DocumentSource.Count > 0)
                {
                    bool existUploadingFile = DocumentSource.Any(file => !ACAConstant.FINISHED_STATUS.Equals(file.DocumentModel.FileState));

                    string setSaveButtonStateScriptKey = string.Format("{0}_DisableSaveAttachmentButton", ClientID);
                    string setSaveButtonStateScript = string.Format("if(typeof ({0}_DisableSaveAttachmentButton) != 'undefined'){{{0}_DisableSaveAttachmentButton({1});}}", ClientID, existUploadingFile.ToString().ToLower());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), setSaveButtonStateScriptKey, setSaveButtonStateScript, true);
                }
            }

            if (!IsInSpearForm && !IsInConfirmPage)
            {
                return;
            }

            // Display the document types that this application requires for sumbission.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            Dictionary<string, string> dicRequiedDocumentTypes = CapUtil.GetRequiredDocumentTypeList(capModel.capType);

            if (dicRequiedDocumentTypes == null || dicRequiedDocumentTypes.Count == 0)
            {
                return;
            }

            StringBuilder documentTypes = new StringBuilder();

            foreach (KeyValuePair<string, string> item in dicRequiedDocumentTypes)
            {
                documentTypes.Append(item.Value);
                documentTypes.Append(ACAConstant.COMMA_BLANK);
            }

            if (documentTypes.Length > ACAConstant.COMMA_BLANK.Length)
            {
                documentTypes.Remove(documentTypes.Length - ACAConstant.COMMA_BLANK.Length, ACAConstant.COMMA_BLANK.Length);

                lblRequiredDocumentTypesIntroduction.Visible = true;
                spanRequiredDocumentTypesIntroduction.Visible = true;
                string requiredDocumentTypesInstruction = GetTextByKey("aca_fileupload_label_required_document_types");
                requiredDocumentTypesInstruction = requiredDocumentTypesInstruction.Replace(AttachmentUtil.FileUploadVariables.RequiredDocumentTypeFormats, documentTypes.ToString());
                lblRequiredDocumentTypesIntroduction.Text = requiredDocumentTypesInstruction;
            }
        }

        /// <summary>
        /// Fill document edit form.
        /// </summary>
        /// <param name="sender">button fill document form</param>
        /// <param name="e">click event</param>
        protected void FillDocEditForm(object sender, EventArgs e)
        {
            string senderArgs = Request.Form[Page.postEventArgumentID];
            string[] args = senderArgs.Split(ACAConstant.SPLIT_CHAR4URL1);

            if (args.Length < 2)
            {
                return;
            }

            FileUploadInfo[] fileInfoList = JsonConvert.DeserializeObject<FileUploadInfo[]>(args[0]);

            RefreshDocumentData();
            
            if (fileInfoList != null && fileInfoList.Any())
            {
                foreach (FileUploadInfo fileInfo in fileInfoList)
                {
                    bool isSelectFromAccount = AttachmentUtil.IsSelectFromAccount(fileInfo.DocumentModel);
                    fileInfo.DocumentModel = AttachmentUtil.ConstructDocumentModel(ModuleName, fileInfo, isSelectFromAccount);

                    //only for html5 upload resubmit
                    if (CurrentFileUploadBehavior == FileUploadBehavior.Html5 && ACAConstant.RESUBMIT.Equals(fileInfo.DocumentModel.categoryByAction, StringComparison.InvariantCultureIgnoreCase))
                    {
                        fileInfo.UploaderName = args[1].Trim();
                    }

                    DocumentSource.Add(fileInfo);
                }
            }

            BindDocumentSource();

            //Focus on the start of document edit section.
            Page.FocusElement(lnkFocusAnchor.ClientID);
        }

        /// <summary>
        /// Gets a value to indicating whether displaying the select from account button.
        /// </summary>
        /// <returns>Is Show SelectFromAccount Button flag</returns>
        protected bool IsShowSelectFromAccountButton()
        {
            if (AppSession.IsAdmin 
                && (IsInSpearForm || IsDetailPage))
            {
                return true;
            }

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            
            // if in confirm page or agency-defined disabled then hide the link.
            if (IsInConfirmPage
                || IsAccountManagerPage
                || !FunctionTable.IsEnableUploadDocument()
                || !StandardChoiceUtil.IsEnableAccountAttachment()
                || capBll.IsCreateByDelegateUser(capModel)
                || (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk))
            {
                return false;
            }

            CapIDModel4WS capID = capModel != null ? capModel.capID : null;
            EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, ModuleName, AppSession.User.PublicUserId, capID);

            if (edmsPolicyModel != null)
            {
                bool hasUploadPermission = AttachmentUtil.HasDocumentTypeUploadPermission(capModel, ModuleName, ComponentName, !IsAccountManagerPage && !IsDetailPage);
                return Convert.ToBoolean(edmsPolicyModel.uploadRight) && hasUploadPermission;
            }

            return false;
        }

        /// <summary>
        /// Gets a value to indicating whether the current user has the Upload permission and the the upload button.
        /// </summary>
        /// <returns>Is Show Upload Button Flag</returns>
        protected bool IsShowUploadButton()
        {
            if (AppSession.IsAdmin)
            {
                return true;
            }

            // if in confirm page or agency-defined disabled then hide the link.
            if (IsInConfirmPage
                || !FunctionTable.IsEnableUploadDocument()
                || (IsAccountManagerPage
                    && AttachmentUtil.GetAvaliableLicense4PeopleDocument(ConfigManager.SuperAgencyCode).Count < 1
                    && AttachmentUtil.GetAvaliableContact4PeopleDocument().Count < 1))
            {
                return false;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CapIDModel4WS capID = capModel != null ? capModel.capID : null;
            EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, ModuleName, AppSession.User.PublicUserId, capID);

            if (edmsPolicyModel != null)
            {
                // get the EDMS upload right
                bool hasEDMSUploadRight = Convert.ToBoolean(edmsPolicyModel.uploadRight);

                //Upload attachment to people will ignore proxyUserRole.
                if (IsAccountManagerPage)
                {
                    return hasEDMSUploadRight;
                }

                bool hasUploadPermission = AttachmentUtil.HasDocumentTypeUploadPermission(capModel, ModuleName, ComponentName, !IsAccountManagerPage && !IsDetailPage);
                return hasEDMSUploadRight && hasUploadPermission;
            }

            return false;
        }

        /// <summary>
        /// Bind document data source.
        /// </summary>
        private void BindDocumentSource()
        {
            if (IsDetailPage || IsInSpearForm)
            {
                string key = ModuleName + "|" + UniqueID;

                Dictionary<string, List<FileUploadInfo>> htFileUploadInfos = AppSession.GetUploadFileInfoFromSession(ModuleName);

                if (htFileUploadInfos != null)
                {
                    if (htFileUploadInfos.ContainsKey(key))
                    {
                        htFileUploadInfos[key] = DocumentSource;
                    }
                    else
                    {
                        htFileUploadInfos.Add(key, DocumentSource);
                    }
                }
                else
                {
                    htFileUploadInfos = new Dictionary<string, List<FileUploadInfo>>();
                    htFileUploadInfos.Add(key, DocumentSource);
                }

                AppSession.SetUploadFileInfoToSession(ModuleName, htFileUploadInfos);
            }
            else
            {
                AppSession.SetUploadFileInfoToSession(ModuleName, null);
            }

            dlDocumentEdit.DataSource = DocumentSource;
            dlDocumentEdit.DataBind();

            // each times refresh the editing fileid dataset's value.
            hdfEditingFileIds.Value = string.Empty;

            if (DocumentSource != null && DocumentSource.Count > 0)
            {
                List<string> fileIds = new List<string>();

                foreach (var item in DocumentSource)
                {
                    if (!AttachmentUtil.IsSelectFromAccount(item.DocumentModel))
                    {
                        fileIds.Add(item.FileId);    
                    }
                }

                hdfEditingFileIds.Value = new JavaScriptSerializer().Serialize(fileIds);
            }
            
            var isShow = (DocumentSource != null && DocumentSource.Count > 0) || AppSession.IsAdmin;

            string scriptKey = string.Format("{0}_InitButtons", ClientID);
            string script = string.Format("if(typeof ({0}_InitButtons) != 'undefined'){{{0}_InitButtons({1});}}", ClientID, isShow.ToString().ToLower());
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), scriptKey, script, true);
        }

        /// <summary>
        /// Keeping UI data by user inputted.
        /// </summary>
        private void RefreshDocumentData()
        {
            for (int i = 0; i < DocumentSource.Count; i++)
            {
                var docEdit = dlDocumentEdit.Items[i].FindControl("documentEdit") as DocumentEdit;
                DocumentSource[i].DocumentModel = CurrentFileUploadBehavior == FileUploadBehavior.Html5 ? UpdateFileState(docEdit.DataModel) : fileSelect.UpdateFileState(docEdit.DataModel);
            }
        }

        /// <summary>
        /// Update document state when post back.
        /// </summary>
        /// <param name="document">document model</param>
        /// <returns>Document Model</returns>
        private DocumentModel UpdateFileState(DocumentModel document)
        {
            if (!string.IsNullOrEmpty(hdAllFinishedFileArray.Value) && document != null)
            {
                var jsSerializer = new JavaScriptSerializer();
                var fileInfoList = jsSerializer.Deserialize<FileUploadInfo[]>(hdAllFinishedFileArray.Value);

                foreach (var uploadInfo in fileInfoList)
                {
                    if (document.FileId.Equals(uploadInfo.FileId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        document.FileState = uploadInfo.StateString;
                    }
                }
            }

            return document;
        }

        /// <summary>
        /// Focus on "Browse" button.
        /// </summary>
        private void FocusOnBrowserBtn()
        {
            string btnClientId;
            switch (CurrentFileUploadBehavior)
            {
                case FileUploadBehavior.Basic:
                    btnClientId = divFileSelect.ClientID;

                    //Focus on "Browse" button after document info saved.
                    Page.FocusElement(btnClientId);
                    break;
                case FileUploadBehavior.Advanced:
                    btnClientId = btnBrowse.ClientID;

                    //Focus on "Browse" button after document info saved.
                    Page.FocusElement(btnClientId);
                    break;
                case FileUploadBehavior.Html5:
                    btnClientId = divHtml5Upload.ClientID;
                    string script = string.Format("setTimeout(function () {{ $('#{0}').find('a').focus();}}, 0);", btnClientId);
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), btnClientId + CommonUtil.GetRandomUniqueID().Substring(0, 6), script, true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Remove file for html5 resubmit upload
        /// </summary>
        /// <param name="fileUploadInfos">fileUploadInfo list</param>
        private void RegisterScript4ResubmitRemoveFile(List<FileUploadInfo> fileUploadInfos)
        {
            if (fileUploadInfos == null || fileUploadInfos.Count == 0 || CurrentFileUploadBehavior != FileUploadBehavior.Html5)
            {
                return;
            }

            foreach (var fileInfo in fileUploadInfos)
            {
                if (string.IsNullOrEmpty(fileInfo.UploaderName)
                    || !ACAConstant.RESUBMIT.Equals(fileInfo.DocumentModel.categoryByAction, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                string script = string.Format(
                    "$get('{0}_iframeAttachmentList').contentWindow.{1}.removeSingleFile('{2}');",
                    ClientID,
                    fileInfo.UploaderName,
                    fileInfo.FileId);

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "RemoveSingleFile" + fileInfo.FileId, script, true);
            }
        }
        #endregion Methods
    }
}
