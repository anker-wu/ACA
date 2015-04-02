#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AttachmentList.ascx.cs 279224 2014-10-15 08:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AttachmentList
    /// </summary>
    public partial class AttachmentList : FileUploadBase
    {
        #region Fields

        /// <summary>
        /// Download command name.
        /// </summary>
        protected const string COMMAND_DOWNLOAD = "DownLoad";

        /// <summary>
        /// Remove command name.
        /// </summary>
        protected const string COMMAND_REMOVE = "Remove";

        /// <summary>
        /// Document Permission Category: View
        /// Configured in Standard Choice item: ALL_USER_DOCUMENT_PERMISSION
        /// </summary>
        private const string VIEW_RIGHT = "View";

        /// <summary>
        /// Document Permission Category: Download
        /// Configured in Standard Choice item: ALL_USER_DOCUMENT_PERMISSION
        /// </summary>
        private const string DOWNLOAD_RIGHT = "Download";

        /// <summary>
        /// Document Permission Category: Delete
        /// Configured in Standard Choice item: ALL_USER_DOCUMENT_PERMISSION
        /// </summary>
        private const string DELETE_FRIGHT = "Delete";

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(AttachmentList));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets Pending attachment count.
        /// </summary>
        public int PendingAttachmentCount
        {
            get
            {
                return (int)ViewState["PendingAttachmentCount"];
            }

            set
            {
                ViewState["PendingAttachmentCount"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether current document list is people document list.
        /// </summary>
        protected bool IsPeopleDocument
        {
            get
            {
                if (IsAccountManagerPage)
                {
                    return true;
                }

                return ValidationUtil.IsTrue(Request.QueryString["isPeopleDocument"]);
            }
        }

        /// <summary>
        /// Gets Maintain the EDMS PolicyModel
        /// </summary>
        private EdmsPolicyModel4WS EdmsPolicyModel
        {
            get
            {
                // get the EDMS model for controlling the permission of attachment section.
                string callerID = AppSession.User.PublicUserId;
                string moduleName = !IsPeopleDocument ? ModuleName : string.Empty;
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
                CapIDModel4WS capID = capModel != null ? capModel.capID : null;
                EdmsPolicyModel4WS edmsPolicy = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, moduleName, callerID, capID);

                return edmsPolicy;
            }
        }

        /// <summary>
        /// Gets FILE FAILED
        /// </summary>
        private string FILE_FAILED
        {
            get
            {
                return GetTextByKey("per_attachmentList_file_failed");
            }
        }

        /// <summary>
        /// Gets file pending.
        /// </summary>
        private string FILE_PENDING
        {
            get
            {
                return GetTextByKey("per_attachmentList_file_pending");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cap has lock condition
        /// </summary>
        private bool HasLockCondition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether partial cap.
        /// </summary>
        private bool IsPartialCap
        {
            get
            {
                bool isPartialCap = false;
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && !string.IsNullOrEmpty(capModel.capClass))
                {
                    isPartialCap = !capModel.capClass.Equals(ACAConstant.COMPLETED);
                }

                return isPartialCap;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Attachment is deletable by Application status
        /// </summary>
        private bool IsDocDeletableByAppStatus
        {
            get
            {
                if (IsPartialCap)
                {
                    // if partial cap, don't need to judge the Cap status.
                    ViewState["IsDocDeletableByAppStatus"] = true;
                }
                else
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                    if (ViewState["IsDocDeletableByAppStatus"] == null)
                    {
                        ICapTypePermissionBll capTypePermissionBll = ObjectFactory.GetObject(typeof(ICapTypePermissionBll)) as ICapTypePermissionBll;
                        ViewState["IsDocDeletableByAppStatus"] =
                            capTypePermissionBll.IsDeletableDocumentByAppStatus(capModel.capStatus, ACAConstant.BUTTON_CREATE_DOCUMENT_DELETE, capModel.capType);
                    }
                }

                return Convert.ToBoolean(ViewState["IsDocDeletableByAppStatus"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page is cap detail
        /// </summary>
        private bool IsDetailPage
        {
            get
            {
                return ValidationUtil.IsTrue(Request["isdetail"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page is license detail page.
        /// </summary>
        private bool IsLicenseeDetailPage
        {
            get
            {
                return ValidationUtil.IsTrue(Request["isLicenseeDetailPage"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page is cap confirm page.
        /// </summary>
        private bool IsConfirmPage
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isInConfirm"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether current page is account manager.
        /// </summary>
        private bool IsAccountManagerPage
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isaccountmanager"]);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// in cap detail page, set the grid view number to 60062
        /// </summary>
        public void ChangeGridViewNumber()
        {
            //cap edit page
            gdvAttachmentList.GridViewNumber = GviewID.CapEditAttachmentList;

            if (IsPeopleDocument || IsAccountManagerPage)
            {
                //account manager page || cap detail page's people document list
                gdvAttachmentList.GridViewNumber = GviewID.AccountManagerAttachmentList;
            }
            else if (IsDetailPage)
            {
                //cap detail page
                gdvAttachmentList.GridViewNumber = GviewID.CapDetailAttachmentList;
            }
            else
            {
                //Hide [Record Number | Record Type | Entity Type] in spear form.
                foreach (var col in gdvAttachmentList.Columns)
                {
                    var accelaCol = col as AccelaTemplateField;

                    if (accelaCol != null)
                    {
                        if ("lnkRecordNumberHeader".Equals(accelaCol.AttributeName, StringComparison.OrdinalIgnoreCase) ||
                            "lnkRecordTypeHeader".Equals(accelaCol.AttributeName, StringComparison.OrdinalIgnoreCase) ||
                            "lnkEntityTypeHeader".Equals(accelaCol.AttributeName, StringComparison.OrdinalIgnoreCase))
                        {
                            accelaCol.Visible = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Show attachment list
        /// </summary>
        /// <param name="componentName">Component Name</param>
        /// <returns>display attachment count.</returns>
        public int DisplayAttachment(string componentName = null)
        {
            CapIDModel capID = null;
            string customId = string.Empty;
            DocumentModel[] tempDocList = null;
            string serviceProviderCode = string.Empty;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null)
            {
                capID = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
                serviceProviderCode = capID.serviceProviderCode;
                customId = capID.customID;
            }

            try
            {
                // invoke the EMDS interface to get AttachmentList
                if (!AppSession.IsAdmin)
                {
                    CurrentFileUploadBehavior = StandardChoiceUtil.GetFileUploadBehavior();
                    IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();

                    if (IsAccountManagerPage
                        && StandardChoiceUtil.IsEnableAccountAttachment())
                    {
                        //get people document list by reference LP
                        EntityModel[] entityList = AttachmentUtil.ConstructEntityModel(ConfigManager.AgencyCode);
                        DocumentResultModel resultModel = entityList.Length > 0 ? edmsDocBll.GetEntityDocumentList(ConfigManager.AgencyCode, AppSession.User.PublicUserId, entityList) : null;
                        
                        resultModel = resultModel ?? new DocumentResultModel();
                        tempDocList = resultModel.documentList;

                        if (!string.IsNullOrEmpty(resultModel.errorMessage))
                        {
                            MessageUtil.ShowMessageInParent(Page, MessageType.Error, resultModel.errorMessage, true, -1);    
                        }
                    }
                    else if (IsPeopleDocument)
                    {
                        //get people document list by cap id after clicking view people attachments link
                        DocumentResultModel resultModel  = edmsDocBll.GetPeopleDocumentByCapID(serviceProviderCode, capID, string.Empty, AppSession.User.PublicUserId);
                        
                        resultModel = resultModel ?? new DocumentResultModel();
                        tempDocList = resultModel.documentList;

                        if (!string.IsNullOrEmpty(resultModel.errorMessage))
                        {
                            MessageUtil.ShowMessageInParent(Page, MessageType.Error, resultModel.errorMessage, true, -1);    
                        }
                    }
                    else
                    {
                        //get record document list by cap id after clicking view record attachments link
                        tempDocList = edmsDocBll.GetRecordDocumentList(ConfigManager.AgencyCode, ModuleName, AppSession.User.PublicUserId, capID, IsPartialCap);
                    }
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageInParent(Page, MessageType.Error, ex.Message, true, -1);
            }

            /* 
             * If there is no component name, which means the request may be made by the page of Record Detail, Account Management, and/or People Document.
             * Since these pages are not handled by page flow, so just simply show all documents for them.
             * Otherwise, we need to firstly figure out which document belongs to which attachment list, and then show them.
             */
            if (string.IsNullOrEmpty(componentName))
            {
                PendingAttachmentCount = BindAttachmentList(tempDocList, customId);

                return PendingAttachmentCount;
            }

            List<DocumentModel> documentList4DB = tempDocList != null ? new List<DocumentModel>(tempDocList) : new List<DocumentModel>();
            List<DocumentModel> documentsBelongToComponent = new List<DocumentModel>();

            AttachmentUtil.PrepareAttachments4FindComponent(documentList4DB, capModel, ModuleName);

            foreach (DocumentModel document in documentList4DB)
            {
                if (document != null && componentName.Equals(document.componentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    documentsBelongToComponent.Add(document);
                }
            }

            PendingAttachmentCount = BindAttachmentList(documentsBelongToComponent.ToArray(), customId);

            return PendingAttachmentCount;
        }

        /// <summary>
        /// OnPreRender Event.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            HideGridViewColumns();
            base.OnPreRender(e);
        }

        /// <summary>
        /// override the on initialization event
        /// </summary>
        /// <param name="e">event argument.</param>
        protected override void OnInit(EventArgs e)
        {
            //People document list only get Agency level view layout.
            //licensee deail page need get agency level view layout.
            bool isAgencyLevel = IsLicenseeDetailPage || IsPeopleDocument;
            string moduleName = !isAgencyLevel ? ModuleName : string.Empty;
            GridViewBuildHelper.SetSimpleViewElements(gdvAttachmentList, moduleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Page load event.
        /// </summary>
        /// <param name="sender">event object.</param>
        /// <param name="e">event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                // Display notice message for user to edit.
                divActionNotice.Visible = true;
                lblActionNoticeDeleteSuccess.Visible = true;
                lblActionNoticeDeleteFailed.Visible = true;
            }
            else
            {
                divActionNotice.Visible = false;

                if (!IsPostBack)
                {
                    //Resolve the Tab index issue in Opera browser.
                    bool isOpera = Request.UserAgent.IndexOf("opera", 0, StringComparison.OrdinalIgnoreCase) != -1;
                    lnkAttachmentEnd.Visible = isOpera;
                }
            }
        }

        /// <summary>
        /// download or delete command.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AttachmentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string entityInfo = (string)e.CommandArgument;
            string[] temps = entityInfo.Split(ACAConstant.SPLIT_CHAR);
            string componentName;

            if (temps.Length < 6)
            {
                // Other events, for example: sorting or page index change event.
                string iframeId = Request[UrlConstant.IFRAME_ID];
                componentName = AttachmentUtil.ExtractComponentNameFromClientID(iframeId);
                DisplayAttachment(componentName);

                return;
            }

            string documentNo = temps[0];
            string fileKey = temps[1];
            string customId = temps[2];
            string entityId = temps[3];
            string entityType = temps[4];
            string serviceProviderCode = temps[5];

            // In record detail page attachment section's component name is null. When page flow exist multiple attachment section.
            componentName = temps.Length > 6 ? temps[6] : null;

            if (COMMAND_DOWNLOAD.Equals(e.CommandName))
            {
                EntityModel entity = new EntityModel();
                entity.entityID = entityId;
                entity.customID = customId;
                entity.entityType = entityType;
                entity.serviceProviderCode = serviceProviderCode;

                DownloadAttachment(entity, documentNo, fileKey);
            }
            else if (COMMAND_REMOVE.Equals(e.CommandName))
            {
                DocumentModel document = new DocumentModel();
                document.altId = customId;
                document.fileKey = fileKey;
                document.entityID = entityId;
                document.entityType = entityType;
                document.documentNo = long.Parse(documentNo);
                document.serviceProviderCode = serviceProviderCode;

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapIDModel4WS capID = capModel != null ? capModel.capID : null;
                document.capID = capID != null ? TempModelConvert.Trim4WSOfCapIDModel(capID) : null;
                document.componentName = componentName;

                DeleteAttachment(document);

                //Focus on hidden anchor before the attachment list after single item deleted.
                Page.FocusElement(lnkFocusAnchor.ClientID);
            }
        }

        /// <summary>
        /// GridView AttachmentList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void AttachmentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaDateLabel lblDate = (AccelaDateLabel)e.Row.FindControl("lblDate");
                AccelaLabel lblEntityType = (AccelaLabel)e.Row.FindControl("lblEntityType");
                AccelaLabel lblRecordType = (AccelaLabel)e.Row.FindControl("lblRecordType");
                AccelaLabel lblRecordNumber = (AccelaLabel)e.Row.FindControl("lblRecordNumber");
                AccelaLabel lblVirtualFolder = (AccelaLabel)e.Row.FindControl("lblVirtualFolders");
                AccelaLabel lblReviewStatus = (AccelaLabel)e.Row.FindControl("lblReviewStatus");
                AccelaLabel lblDescription = (AccelaLabel)e.Row.FindControl("lblDescription");
                lblDescription.EllipsisContainerID = Request[UrlConstant.IFRAME_ID] + "_iframeAttachmentList";

                string enStyleDate = I18nDateTimeUtil.FormatToDateStringForWebService(lblDate.Text2);

                if (EdmsPolicyModel != null)
                {
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    EdmsPolicyModel4WS edmsPolicyModel = EdmsPolicyModel;
                    Dictionary<object, object> documentActions = new Dictionary<object, object>();
                    string edmsDownloadRight = edmsPolicyModel.downloadRight;
                    string edmsDeleteRight = edmsPolicyModel.deleteRight;
                    string edmsUploadRight = edmsPolicyModel.uploadRight;
                    object entityType = rowView[ColumnConstant.Attachment.EntityType.ToString()];
                    string recordType = CAPHelper.GetAliasOrCapTypeLabel(capModel);
                    string altId = rowView[ColumnConstant.Attachment.RecordNumber.ToString()].ToString();
                    DocumentModel document = rowView[ColumnConstant.Attachment.DocumentModel.ToString()] as DocumentModel;

                    if (document != null)
                    {
                        string[] reviewStatusArray = document.reviewStatus;

                        if (reviewStatusArray != null)
                        {
                            string reviewStatusStr = string.Empty;
                            foreach (string reviewStatus in reviewStatusArray)
                            {
                                reviewStatusStr += reviewStatus + ACAConstant.HTML_BR;
                            }

                            lblReviewStatus.Text = reviewStatusStr;
                        }
                    }

                    if (enStyleDate.Equals(ACAConstant.FILE_PENDING_DATE, StringComparison.InvariantCulture)
                        || (!(entityType is DBNull) && entityType.ToString() == DocumentEntityType.TMP_CAP && !IsPartialCap))
                    {
                        //Pending
                        lblDate.Text = FILE_PENDING;
                        lblEntityType.Text = AttachmentUtil.FormatEntityType(entityType.ToString(), ModuleName);
                        lblRecordNumber.Text = capModel != null ? capModel.altID : string.Empty;
                        lblRecordType.Text = recordType;
                    }
                    else if (enStyleDate.Equals(ACAConstant.FILE_FAILED_DATE, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //Failed 
                        lblDate.Text = FILE_FAILED;  
                        lblEntityType.Text = AttachmentUtil.FormatEntityType(entityType.ToString(), ModuleName);
                        lblRecordNumber.Text = capModel != null ? capModel.altID : string.Empty;
                        lblRecordType.Text = recordType;
                    }
                    else
                    {
                        if (!(entityType is DBNull))
                        {
                            lblEntityType.Text = AttachmentUtil.FormatEntityType(entityType.ToString(), ModuleName);
                        }

                        //Generate the actions based on the permissions.
                        UserRolePrivilegeModel viewRole = rowView[ColumnConstant.Attachment.ViewRole.ToString()] as UserRolePrivilegeModel;
                        UserRolePrivilegeModel deleteRole = rowView[ColumnConstant.Attachment.DeleteRole.ToString()] as UserRolePrivilegeModel;
                        UserRolePrivilegeModel uploadRole = rowView[ColumnConstant.Attachment.uploadRole.ToString()] as UserRolePrivilegeModel;

                        var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

                        /*
                         * Download permission.
                         * If view role is not NULL and have view record or manage document permission, public user can download the document.
                         */
                        bool hasDownloadRight = false;
                        bool hasEDMSDownloadRight = string.IsNullOrEmpty(edmsDownloadRight) || ACAConstant.COMMON_TRUE.Equals(edmsDownloadRight, StringComparison.InvariantCulture);

                        if (IsPartialCap)
                        {
                            if (viewRole != null)
                            {
                                hasDownloadRight = proxyUserRoleBll.HasPermission(capModel, viewRole, ProxyPermissionType.VIEW_RECORD);

                                if (!hasDownloadRight)
                                {
                                    hasDownloadRight = proxyUserRoleBll.HasPermission(capModel, viewRole, ProxyPermissionType.MANAGE_DOCUMENTS);
                                }

                                hasDownloadRight = (AttachmentUtil.IsFileOwner(document) || hasDownloadRight) && hasEDMSDownloadRight;
                            }
                        }
                        else
                        {
                            hasDownloadRight = (bool)rowView[ColumnConstant.Attachment.ViewRole4RealCAP.ToString()] && hasEDMSDownloadRight;
                        }

                        //Crate download link and Actions menu.
                        if (hasDownloadRight)
                        {
                            DisplayDownLink(e);
                        }

                        documentActions.Add(DocumentActions.Download, hasDownloadRight);

                        if (!IsConfirmPage)
                        {
                            //Delete permission.
                            bool hasDeleteRight = false;

                            if (!HasLockCondition || AttachmentUtil.IsFileOwner(document))
                            {
                                bool isAltIdMatched = altId == string.Empty || altId == capModel.altID;

                                if (StandardChoiceUtil.IsEnabledAllUserDocumentPermission(ConfigManager.AgencyCode, ModuleName, DELETE_FRIGHT))
                                {
                                    hasDeleteRight = true;
                                }
                                else
                                {
                                    if (deleteRole != null)
                                    {
                                        // if current attachment list is people document list, will ignore contact access level right.
                                        // contact access level permission controls cap documents not control people(reference LP,Contact) documents.
                                        hasDeleteRight = proxyUserRoleBll.HasPermission(capModel, deleteRole, ProxyPermissionType.MANAGE_DOCUMENTS, IsPeopleDocument);
                                    }
                                }

                                hasDeleteRight = (AttachmentUtil.IsFileOwner(document) || (hasDeleteRight && !IsReadOnly() && isAltIdMatched && IsDocDeletableByAppStatus))
                                                 && (string.IsNullOrEmpty(edmsDeleteRight) || ACAConstant.COMMON_TRUE.Equals(edmsDeleteRight, StringComparison.InvariantCulture));

                                if (IsPeopleDocument && hasDeleteRight)
                                {
                                    bool isFileOwner = IsDetailPage ? AttachmentUtil.IsFileOwnerInCapDetail(document, capModel) : AttachmentUtil.IsFileOwner(document);

                                    if (isFileOwner)
                                    {
                                        if (!string.IsNullOrEmpty(document.fileOwnerPermission))
                                        {
                                            hasDeleteRight = AttachmentUtil.CheckFileOwnerPermission(document.fileOwnerPermission, FileOwnerPermission.Deleteable);    
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(document.docCategory))
                                        {
                                            hasDeleteRight = false;
                                        }
                                    }
                                }
                            }

                            documentActions.Add(DocumentActions.Delete, hasDeleteRight);

                            //If allow resubmit.
                            string allowActions = Convert.ToString(rowView[ColumnConstant.Attachment.AllowActions.ToString()]);
                            bool allowResubmit = allowActions.Contains(ACAConstant.ACA_RESUBMIT) && !IsReadOnly();
                            documentActions.Add(DocumentActions.Resubmit, allowResubmit);

                            //Upload permission.
                            if (string.IsNullOrEmpty(document.docCategory))
                            {
                                IUserRoleBll userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                                uploadRole = userRoleBll.GetDefaultRole();
                            }

                            bool hasUploadRight = proxyUserRoleBll.HasPermission(capModel, uploadRole, ProxyPermissionType.MANAGE_DOCUMENTS, IsPeopleDocument);
                            
                            hasUploadRight = (AttachmentUtil.IsFileOwner(document) || hasUploadRight)
                                              && (string.IsNullOrEmpty(edmsUploadRight) || ACAConstant.COMMON_TRUE.Equals(edmsUploadRight, StringComparison.InvariantCulture));
                            
                            documentActions.Add(DocumentActions.Upload, hasUploadRight);
                        }

                        string agencyCode = rowView[ColumnConstant.Attachment.AgencyCode.ToString()].ToString();
                        string refAgencyCode = rowView[ColumnConstant.Attachment.RefAgencyCode.ToString()].ToString();

                        bool readOnly = capModel != null && capModel.IsForRenew;
                        BuildActionsMenu(e, agencyCode, document.documentNo.ToString(), documentActions, document, readOnly);

                        //display virtual folder
                        string virtualFolderValue = Convert.ToString(rowView[Convert.ToString(ColumnConstant.Attachment.VirtualFolders)]);

                        if (!string.IsNullOrEmpty(virtualFolderValue))
                        {
                            string[] virtualFolders = virtualFolderValue.Split(ACAConstant.SPLIT_CHAR_SEMICOLON);

                            //In super agency environment, can not get the correct I18n value for virtual folders.
                            bool isDocCreatedInSuperAgency = !string.IsNullOrEmpty(refAgencyCode)
                                && !string.Equals(agencyCode, refAgencyCode, StringComparison.OrdinalIgnoreCase);

                            if (capModel != null
                                && capModel.capType != null
                                && !string.IsNullOrEmpty(capModel.capType.virtualFolderGroup)
                                && !isDocCreatedInSuperAgency)
                            {
                                virtualFolders = AttachmentUtil.GetVirtualFolderTextByValue(capModel.capType.virtualFolderGroup, virtualFolders);
                            }

                            lblVirtualFolder.Text = DataUtil.ConcatStringWithSplitChar(virtualFolders, ACAConstant.HTML_BR);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// display document view button.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        private static void DisplayDownLink(GridViewRowEventArgs e)
        {
            LinkButton lnkFileName = (LinkButton)e.Row.FindControl("lnkFileName");
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            lnkFileName.Visible = true;
            lblFileName.Visible = false;
        }

        /// <summary>
        /// Build actions menu.
        /// </summary>
        /// <param name="eventArgs">GridView row event argument.</param>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="documentNo">Document sequence number.</param>
        /// <param name="documentActions">allowed document actions.</param>
        /// <param name="document">Document model to pass through the additional parameters.</param>
        /// <param name="readOnly">is readonly.</param>
        private void BuildActionsMenu(GridViewRowEventArgs eventArgs, string agencyCode, string documentNo, Dictionary<object, object> documentActions, DocumentModel document, bool readOnly)
        {
            PopupActions actionMenu = eventArgs.Row.FindControl("actionMenu") as PopupActions;
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();
            bool isIcoStyle = actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase);

            if (actionMenu == null)
            {
                return;
            }

            var actionList = new List<ActionViewModel>();
            ActionViewModel actionView;

            //View details action
            if (!string.IsNullOrEmpty(agencyCode) && !string.IsNullOrEmpty(documentNo))
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_attachmentlist_label_action_viewdetails");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
                actionView.ActionId = actionMenu.ClientID + "_ViewDetails";

                if (isIcoStyle)
                {
                    actionView.ClientEvent = string.Format(
                                                           "return ViewDocumentDetails('{0}','{1}','{2}','{3}');",
                                                           agencyCode,
                                                           documentNo,
                                                           HttpUtility.UrlEncode(document.entity),
                                                           actionView.ActionId);
                }
                else
                {
                    actionView.ClientEvent = string.Format(
                                                           "return ViewDocumentDetails('{0}','{1}','{2}','{3}');",
                                                           agencyCode,
                                                           documentNo,
                                                           HttpUtility.UrlEncode(document.entity),
                                                           actionMenu.ActionsLinkClientID);
                }

                actionList.Add(actionView);
            }

            if (!readOnly)
            {
                //Delete action
                if (documentActions.ContainsKey(DocumentActions.Delete)
                    && Convert.ToBoolean(documentActions[DocumentActions.Delete])
                    && ConfigManager.AgencyCode.Equals(document.serviceProviderCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    LinkButton lnkRemove = eventArgs.Row.FindControl("lnkRemove") as LinkButton;

                    if (lnkRemove != null)
                    {
                        actionView = new ActionViewModel();
                        actionView.ActionLabel = GetTextByKey("ACA_AttachmentList_Label_Delete");
                        actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                        actionView.ActionId = actionMenu.ClientID + "_Delete";
                        actionView.ClientEvent = string.Format("return RemoveDocument('{0}');", lnkRemove.UniqueID);
                        actionList.Add(actionView);
                    }
                }

                //Resubmit action
                if (documentActions.ContainsKey(DocumentActions.Upload) && Convert.ToBoolean(documentActions[DocumentActions.Upload]) &&
                    documentActions.ContainsKey(DocumentActions.Resubmit) && Convert.ToBoolean(documentActions[DocumentActions.Resubmit]))
                {
                    var lnkAdvanceResubmit = eventArgs.Row.FindControl("lnkAdvanceResubmit") as AccelaLinkButton;

                    if (CurrentFileUploadBehavior == FileUploadBehavior.Basic)
                    {
                        FileUpload silverlightLink = eventArgs.Row.FindControl("resubmitLink") as FileUpload;
                        silverlightLink.Visible = true;
                        silverlightLink.IsDisplayResubmitIconButton = isIcoStyle;
                        silverlightLink.IsDisplayLinkButton = !isIcoStyle;
                        silverlightLink.ParentAgencyCode = agencyCode;
                        silverlightLink.ParentDocumentNo = documentNo;
                        string iframeId = Request[UrlConstant.IFRAME_ID];
                        silverlightLink.ErrorMsgContainerId = iframeId + "_errorMessageLabel";
                        silverlightLink.FunctionRunInSelectCompleted = string.Format("parent.{0}_FillDocEditForm", iframeId);
                        silverlightLink.FunctionRunInStartUpload = string.Format("parent.{0}_DisableSaveAttachmentButton", iframeId);
                        silverlightLink.FunctionRunInAllFilesFinished = string.Format("parent.{0}_DisableSaveAttachmentButton", iframeId);
                        silverlightLink.AdvanceButtonWidth = "$('#" + lnkAdvanceResubmit.ClientID + "').width()";
                    }
                    else if (CurrentFileUploadBehavior == FileUploadBehavior.Html5)
                    {
                        HtmlGenericControl divFileBrowser = eventArgs.Row.FindControl("divFileBrowser") as HtmlGenericControl;

                        RegisterScript4Html5Upload(divFileBrowser.ClientID, agencyCode, documentNo);

                        if (isIcoStyle)
                        {
                            actionView = new ActionViewModel();
                            actionView.ActionLabel = GetTextByKey("aca_attachmentlist_label_action_resubmit");
                            actionView.IcoUrl = ImageUtil.GetImageURL("popaction_resubmit.png");
                            actionView.ActionId = actionMenu.ClientID + "_Resubmit";
                            string clientEvent = BulidClickEvent(divFileBrowser.ClientID, actionView.ActionId);
                            actionView.ClientEvent = clientEvent;
                            actionList.Add(actionView);
                        }
                        else
                        {
                            divFileBrowser.Attributes["class"] = string.Empty;
                        }
                    }
                    else
                    {
                        if (isIcoStyle)
                        {
                            actionView = new ActionViewModel();
                            actionView.ActionLabel = GetTextByKey("aca_attachmentlist_label_action_resubmit");
                            actionView.IcoUrl = ImageUtil.GetImageURL("popaction_resubmit.png");
                            actionView.ActionId = actionMenu.ClientID + "_Resubmit";
                            string clientEvent = string.Format("return BrowseFile('{0}','{1}','{2}');", agencyCode, documentNo, actionView.ActionId);
                            actionView.ClientEvent = clientEvent;
                            actionList.Add(actionView);
                        }
                        else
                        {
                            lnkAdvanceResubmit.CssClass = lnkAdvanceResubmit.CssClass.Replace("ACA_Hide", string.Empty);
                            var clientEventForLink = string.Format("return BrowseFile('{0}','{1}','{2}');", agencyCode, documentNo, lnkAdvanceResubmit.ClientID);
                            lnkAdvanceResubmit.Attributes.Add("onclick", clientEventForLink);
                        }
                    }
                }
            }

            if (actionList.Count > 0)
            {
                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_attachmentlist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Register script to bind the html5 upload control
        /// </summary>
        /// <param name="divBrowserId">div container</param>
        /// <param name="agency">agency code</param>
        /// <param name="docNo">document no</param>
        private void RegisterScript4Html5Upload(string divBrowserId, string agency, string docNo)
        {
            string iframeId = Request[UrlConstant.IFRAME_ID];
            StringBuilder sbScript = new StringBuilder();

            sbScript.AppendFormat(
                                "var upload_{0} = new FileUploader(); $(document).ready(function () {{ upload_{0}.register4UploadController('{0}','{1}',{2},'ACA_LinkButton html5FileUploadResubmit'); upload_{0}.initFileUploadControl('{3}_errorMessageLabel', '{4}', {5},'{6}', '{7}','{8}', '{9}', '{10}', '{11}', '{12}');",
                                divBrowserId,
                                GetTextByKey("aca_attachmentlist_label_action_resubmit").Replace("'", "\\'"),
                                "true",
                                iframeId,
                                DisallowedFileTypes,
                                MaxFileSizeByByte,
                                GetTextByKey("aca_fileupload_msg_typeerror").Replace("'", "\\'"),
                                GetTextByKey("aca_fileupload_msg_sizeerror").Replace("'", "\\'"),
                                GetTextByKey("aca_fileupload_msg_zerosizeerror").Replace("'", "\\'"),
                                agency,
                                docNo,
                                GetTextByKey("aca_html5upload_msg_notsupport").Replace("'", "\\'"),
                                GetTextByKey("aca_html5upload_msg_uploaderror").Replace("'", "\\'"));

            string enableBtnFunc = string.Format("function (){{ parent.{0}_DisableSaveAttachmentButton(false); }}", iframeId);
            string disableBtnFunc = string.Format("function (){{ parent.{0}_DisableSaveAttachmentButton(true); }}", iframeId);
            sbScript.AppendFormat(
                            "upload_{0}.selectCompletedCallBack = function(files, uploaderName) {{ parent.{1}_FillDocEditForm(files, null, uploaderName); }}; upload_{0}.startUploadCallBack = {2}; upload_{0}.allFilesFinishedCallBack = {3}; upload_{0}.removeAllCallBack = {3};",
                            divBrowserId,
                            iframeId,
                            disableBtnFunc,
                            enableBtnFunc);

            sbScript.AppendFormat("upload_{0}.singleFinishCallBack = function(fileInfo) {{ parent.{1}_FunctionRunInSingleFinish(fileInfo); }};", divBrowserId, iframeId);
            sbScript.Append("});");

            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Register4Html5ResubmitButton" + divBrowserId, sbScript.ToString(), true);
        }

        /// <summary>
        /// Resubmit click event for html5upload
        /// </summary>
        /// <param name="divBrowserId">div container</param>
        /// <param name="focusObjId">need focus object id</param>
        /// <returns>Click event</returns>
        private string BulidClickEvent(string divBrowserId, string focusObjId)
        {
            return string.Format("invokeClick($('#' + upload_{0}.fileInputObjId)[0]);return false;", divBrowserId);
        }

        /// <summary>
        /// create data source for attachment list
        /// </summary>
        /// <param name="documentList"> a list of documents</param>
        /// <param name="customID"> the custom ID</param>
        /// <param name="pendingAttachmentCount"> pending attachment count</param>
        /// <returns>data table for document.</returns>
        private DataTable CreateDataSource(DocumentModel[] documentList, string customID, out int pendingAttachmentCount)
        {
            DataTable dtDocument = AttachmentUtil.CreateDocumentDataTable();
            string attachmentIframeId = Request[UrlConstant.IFRAME_ID];
            pendingAttachmentCount = AttachmentUtil.LoadPendingAttachmentList(dtDocument, ModuleName, customID, attachmentIframeId, IsPeopleDocument);
            AttachmentUtil.LoadFailedAttachmentList(dtDocument, ModuleName, customID, attachmentIframeId, IsPeopleDocument);
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (documentList == null || documentList.Length == 0)
            {
                return dtDocument;
            }

            bool isDownloadableForAllUser = StandardChoiceUtil.IsEnabledAllUserDocumentPermission(ConfigManager.AgencyCode, ModuleName, DOWNLOAD_RIGHT);
            bool isViewableForAllUser = StandardChoiceUtil.IsEnabledAllUserDocumentPermission(ConfigManager.AgencyCode, ModuleName, VIEW_RIGHT);

            foreach (DocumentModel document in documentList)
            {
                if (document == null)
                {
                    continue;
                }

                bool isDisplayDocTitleLink = false;
                bool isDisplayDownLink = false;

                if (IsPartialCap)
                {
                    isDisplayDocTitleLink = CheckDocumentTypePermission(capModel, document);
                }
                else
                {
                    isDisplayDocTitleLink = isViewableForAllUser || CheckDocumentTypePermission(capModel, document);
                    isDisplayDownLink = isDownloadableForAllUser || document.viewable;

                    if (IsPeopleDocument)
                    {
                        // ACA Permission In V360
                        bool isFileOwner = IsDetailPage ? AttachmentUtil.IsFileOwnerInCapDetail(document, capModel) : AttachmentUtil.IsFileOwner(document);

                        if (isFileOwner)
                        {
                            if (!string.IsNullOrEmpty(document.fileOwnerPermission))
                            {
                                isDisplayDocTitleLink = AttachmentUtil.CheckFileOwnerPermission(document.fileOwnerPermission, FileOwnerPermission.TitleViewable);
                                isDisplayDownLink = AttachmentUtil.CheckFileOwnerPermission(document.fileOwnerPermission, FileOwnerPermission.Downloadable);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(document.docCategory))
                            {
                                isDisplayDownLink = false;
                            }
                        }
                    }
                }

                /*
                 * the maximal PRI=downRight, hasPrivilegeToHandleCap and IsLocked.
                 * secondly PRI= isDisplayDownLink
                 * whether display down link by the maximal PRI and secondly PRI
                 */ 
                if (!isDisplayDocTitleLink)
                {
                    continue;
                }
                
                DataRow dr = AttachmentUtil.GetDocumentDataRow(dtDocument, document, document.serviceProviderCode);

                if (document.entityType == DocumentEntityType.TMP_CAP && !IsPartialCap)
                {
                    pendingAttachmentCount++;
                }

                dr[ColumnConstant.Attachment.ViewRole4RealCAP.ToString()] = isDisplayDownLink;

                dtDocument.Rows.Add(dr);
            }

            return dtDocument;
        }

        /// <summary>
        /// If view title role is not NULL and have view record or manage document permission, public user can view the document
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="document">document model</param>
        /// <returns>The document type permission for display.</returns>
        private bool CheckDocumentTypePermission(CapModel4WS capModel, DocumentModel document)
        {
            bool isDisplayDocTitleLink = false;

            // Document Type Permission In AA Classic
            // If view title role is not NULL and have view record or manage document permission, public user can view the document
            if (document.viewTitleRoleModel != null)
            {
                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                UserRolePrivilegeModel userRole = document.viewTitleRoleModel;
                isDisplayDocTitleLink = proxyUserRoleBll.HasPermission(capModel, userRole, ProxyPermissionType.VIEW_RECORD);

                if (!isDisplayDocTitleLink)
                {
                    isDisplayDocTitleLink = proxyUserRoleBll.HasPermission(capModel, userRole, ProxyPermissionType.MANAGE_DOCUMENTS);
                }
            }

            return isDisplayDocTitleLink;
        }

        /// <summary>
        /// bind data source
        /// </summary>
        /// <param name="documentList"> document List</param>
        /// <param name="customID"> the custom ID</param>
        /// <returns>bind attachment list count.</returns>
        private int BindAttachmentList(DocumentModel[] documentList, string customID)
        {
            if (IsDetailPage)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapIDModel4WS capID = capModel.capID;
                CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capID, AppSession.User.UserSeqNum, true);

                if (capWithConditionModel != null && capWithConditionModel.conditionModel != null
                    && ACAConstant.LOCK_CONDITION.Equals(capWithConditionModel.conditionModel.impactCode, StringComparison.OrdinalIgnoreCase))
                {
                    HasLockCondition = true;
                }
            }

            int pendingAttachmentCount;
            DataTable dtSource = CreateDataSource(documentList, customID, out pendingAttachmentCount);
            gdvAttachmentList.DataSource = dtSource;
            gdvAttachmentList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("ins_attachmentList_message_noRecord");
            gdvAttachmentList.DataBind();

            if (IsDetailPage)
            {
                HttpContext.Current.Session[SessionConstant.DOCUMENT_STATUS_LIST] = dtSource;

                Page.ClientScript.RegisterStartupScript(GetType(), "BindDocumentStatusList", "BindDocumentList();", true);
            }

            return pendingAttachmentCount;
        }

        /// <summary>
        /// delete document.
        /// </summary>
        /// <param name="document">document model</param>
        private void DeleteAttachment(DocumentModel document)
        {
            string callerID = AppSession.User.PublicUserId;
            string agencyCode = document.serviceProviderCode;

            IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            string moduleName = !IsPeopleDocument ? ModuleName : string.Empty;

            try
            {
                bool canDelete = true;

                // People document need to check associate relation.
                if (IsPeopleDocument)
                {
                    string[] agency = edmsBll.GetAssociatedDocumentAgency(agencyCode, (long)document.documentNo);
                    canDelete = agency == null || agency.Length == 0;
                    
                    if (!canDelete)
                    {
                        //check the document relationship mapping
                        DisplayDelActionNotice(DeleteAttachmentResult.Related);
                    }
                }

                if (canDelete)
                {
                    //invoke the EDMS's to remove a file.
                    bool result = edmsBll.DoDelete(agencyCode, moduleName, document, callerID, IsPartialCap);

                    DisplayDelActionNotice(result ? DeleteAttachmentResult.Successfull : DeleteAttachmentResult.Failed);
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                DisplayDelActionNotice(DeleteAttachmentResult.Failed);
            }
            finally
            {
                /*
                 * Only need to refresh the data of the specified document list indicated by component name.
                 * But in cap detail page do not need indicated by component name.
                 */
                DisplayAttachment(IsDetailPage || IsAccountManagerPage ? null : document.componentName);
            }
        }

        /// <summary>
        /// Display delete action notice
        /// </summary>
        /// <param name="result">Delete attachment result ENUM</param>
        private void DisplayDelActionNotice(DeleteAttachmentResult result)
        {
            switch (result)
            {
                case DeleteAttachmentResult.Successfull:
                    {
                        lblActionNoticeDeleteSuccess.Text = GetTextByKey(lblActionNoticeDeleteSuccess.LabelKey);
                        lblActionNoticeDeleteSuccess.Visible = true;

                        if (AccessibilityUtil.AccessibilityEnabled)
                        {
                            MessageUtil.ShowAlertMessage(lblActionNoticeDeleteSuccess, GetTextByKey(lblActionNoticeDeleteSuccess.LabelKey));
                        }

                        divImgSuccess.Visible = true;
                        break;
                    }

                case DeleteAttachmentResult.Failed:
                    {
                        lblActionNoticeDeleteFailed.Text = GetTextByKey(lblActionNoticeDeleteFailed.LabelKey);
                        lblActionNoticeDeleteFailed.Visible = true;

                        if (AccessibilityUtil.AccessibilityEnabled)
                        {
                            MessageUtil.ShowAlertMessage(lblActionNoticeDeleteFailed, GetTextByKey(lblActionNoticeDeleteFailed.LabelKey));
                        }

                        divImgFailed.Visible = true;
                        break;
                    }

                case DeleteAttachmentResult.Related:
                    {
                        lblActionNoticeDeleteCheckResult.Text = GetTextByKey(lblActionNoticeDeleteCheckResult.LabelKey);
                        lblActionNoticeDeleteCheckResult.Visible = true;

                        if (AccessibilityUtil.AccessibilityEnabled)
                        {
                            MessageUtil.ShowAlertMessage(lblActionNoticeDeleteCheckResult, GetTextByKey(lblActionNoticeDeleteCheckResult.LabelKey));
                        }

                        divImgFailed.Visible = true;
                        break;
                    }
            }

            divActionNotice.Visible = true;
        }
        
        /// <summary>
        /// download a file
        /// </summary>
        /// <param name="entityModel">cap id model.</param>
        /// <param name="documentNo">document number</param>
        /// <param name="fileKey">the field key</param>
        private void DownloadAttachment(EntityModel entityModel, string documentNo, string fileKey)
        {
            string callerID = AppSession.User.PublicUserId;
            string agencyCode = ConfigManager.AgencyCode;

            //invoke the EDMS's to get a file.
            IEDMSDocumentBll edmsBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
            string moduleName = IsPeopleDocument ? string.Empty : ModuleName;

            try
            {
                //invoke the EDMS's to remove a file.
                DocumentModel documentModel = edmsBll.DoDownload(agencyCode, moduleName, callerID, entityModel, null, long.Parse(documentNo), fileKey, IsPartialCap);

                if (documentModel == null)
                {
                    return;
                }

                DocumentContentModel docContent = documentModel.documentContent;
                byte[] buffer = docContent.docContentStream;
                string fileName = documentModel.fileName;
                int fileSize = buffer.Length;

                if (buffer.Length > 0)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20")); //resolve the filename with empty space char
                    Response.ContentType = "application/octet-stream";
                    Response.OutputStream.Write(buffer, 0, fileSize);
                    Response.End();
                }
            }
            catch (ACAException)
            {
                DisplayAttachment();
                MessageUtil.ShowMessageInParent(Page, MessageType.Error, GetTextByKey("aca_common_technical_difficulty"));
            }
        }

        /// <summary>
        /// Hide grid's column again.This grid require to set "AccessibleHeaderText" of template field. It is unique.
        /// </summary>
        private void HideGridViewColumns()
        {
            //People document list only get Agency level view layout.
            //licensee deail page need get agency level view layout.
            bool isAgencyLevel = IsLicenseeDetailPage || IsPeopleDocument;
            string moduleName = !isAgencyLevel ? ModuleName : string.Empty;
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] smpViewElementModels = gviewBll.GetSimpleViewElementModel(moduleName, gdvAttachmentList.GridViewNumber);

            if (smpViewElementModels == null || smpViewElementModels.Length == 0 || AppSession.IsAdmin)
            {
                return;
            }

            for (int i = 0; i < gdvAttachmentList.Columns.Count; i++)
            {
                DataControlField column = gdvAttachmentList.Columns[i];

                for (int j = 0; j < smpViewElementModels.Length; j++)
                {
                    string viewElementName = smpViewElementModels[j].viewElementName;

                    if (viewElementName.Equals(column.AccessibleHeaderText))
                    {
                        column.Visible = smpViewElementModels[j].recStatus == ACAConstant.INVALID_STATUS ? false : true;
                    }
                }
            }
        }

        /// <summary>
        /// Attachments can only be viewable.
        /// </summary>
        /// <returns>Is ReadOnly Flag</returns>
        private bool IsReadOnly()
        {
            return ACAConstant.COMMON_TRUE.Equals(Request["readonly"], StringComparison.OrdinalIgnoreCase);
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// comparer the date order.
        /// </summary>
        public class DateComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">document model x.</param>
            /// <param name="y">document model y.</param>
            /// <returns>compare for result.</returns>
            int IComparer.Compare(object x, object y)
            {
                DocumentModel document1 = (DocumentModel)x;
                DocumentModel document2 = (DocumentModel)y;

                if (document1 == null || document2 == null || document1.recDate == null || document2.recDate == null)
                {
                    return 0;
                }

                DateTime recDate1 = document1.recDate.Value;
                DateTime recDate2 = document2.recDate.Value;

                return recDate2.CompareTo(recDate1);
            }

            #endregion Methods
        }

        /// <summary>
        /// comparer the Size ASC order.
        /// </summary>
        public class SizeASCComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">document model x.</param>
            /// <param name="y">document model y.</param>
            /// <returns>compare for result.</returns>
            int IComparer.Compare(object x, object y)
            {
                DocumentModel document1 = (DocumentModel)x;
                DocumentModel document2 = (DocumentModel)y;

                if (document1 == null || document2 == null || document1.fileSize == null || document2.fileSize == null)
                {
                    return 0;
                }

                double fileSize1 = document1.fileSize.Value;
                double fileSize2 = document2.fileSize.Value;

                return fileSize2.CompareTo(fileSize1);
            }

            #endregion Methods
        }

        /// <summary>
        /// comparer the Size DESC order.
        /// </summary>
        public class SizeDESCComparer : IComparer
        {
            #region Methods

            /// <summary>
            /// Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            /// </summary>
            /// <param name="x">document model x.</param>
            /// <param name="y">document model y.</param>
            /// <returns>compare for result.</returns>
            int IComparer.Compare(object x, object y)
            {
                DocumentModel document1 = (DocumentModel)x;
                DocumentModel document2 = (DocumentModel)y;

                if (document1 == null || document2 == null || document1.fileSize == null || document2.fileSize == null)
                {
                    return 0;
                }

                double fileSize1 = document1.fileSize.Value;
                double fileSize2 = document2.fileSize.Value;

                return fileSize1.CompareTo(fileSize2);
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}