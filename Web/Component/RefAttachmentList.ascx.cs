#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAttachmentList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAttachmentList.ascx.cs 269428 2014-06-04 11:12:54Z ACHIEVO\zehon.cai $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
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
    /// RefAttachmentList Control.
    /// </summary>
    public partial class RefAttachmentList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Document Permission Category: View
        /// Configured in Standard Choice item: ALL_USER_DOCUMENT_PERMISSION
        /// </summary>
        private const string VIEW_RIGHT = "View";

        /// <summary>
        /// HasCheckedItems Flag.
        /// </summary>
        private bool _hasCheckedItems;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether has checked item or not.
        /// </summary>
        public bool HasCheckedItems
        {
            get
            {
                return _hasCheckedItems
                       || (gdvAttachmentList.GetSelectedRowIndexes() != null
                           && gdvAttachmentList.GetSelectedRowIndexes().Count > 0);
            }

            set
            {
                _hasCheckedItems = value;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["AttachmentDataSource"];
            }

            set
            {
                ViewState["AttachmentDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the selected item
        /// </summary>
        public DocumentModel[] SelectedItems
        {
            get
            {
                if (SelectedRowIndexes == null || SelectedRowIndexes.Count == 0 || GridViewDataSource == null)
                {
                    return null;
                }

                IList<DocumentModel> selectedAttachtments = new List<DocumentModel>();
                DataTable datasource = gdvAttachmentList.GetSelectedData(GridViewDataSource);

                if (datasource != null && datasource.Rows.Count > 0)
                {
                    foreach (DataRow row in datasource.Rows)
                    {
                        DocumentModel model = (DocumentModel)row["DocumentModel"];

                        if (model != null)
                        {
                            selectedAttachtments.Add(model);
                        }
                    }
                }

                return selectedAttachtments.ToArray();
            }
        }

        /// <summary>
        /// Gets the selected row Index
        /// </summary>
        public List<int> SelectedRowIndexes
        {
            get
            {
                return gdvAttachmentList.GetSelectedRowIndexes();
            }
        }

        /// <summary>
        /// Gets Maintain the EDMSPolicyModel
        /// </summary>
        private EdmsPolicyModel4WS EdmsPolicyModel
        {
            get
            {
                // get the EDMS model for controlling the permission of attachment section.
                string callerID = AppSession.User.PublicUserId;
                string moduleName = ModuleName;
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
                CapIDModel4WS capID = capModel != null ? capModel.capID : null;
                EdmsPolicyModel4WS edmsPolicy = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, moduleName, callerID, capID);

                return edmsPolicy;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Show attachment list
        /// </summary>
        public void DisplayAttachment()
        {
            // invoke the EMDS interface to get AttachmentList
            if (!AppSession.IsAdmin)
            {
                IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();

                EntityModel[] entityList = AttachmentUtil.ConstructEntityModel(ConfigManager.SuperAgencyCode);
                DocumentResultModel resultModel = entityList.Length > 0 ? edmsDocBll.GetEntityDocumentList(ConfigManager.AgencyCode, AppSession.User.PublicUserId, entityList) : null;
                resultModel = resultModel ?? new DocumentResultModel();

                if (!string.IsNullOrWhiteSpace(resultModel.errorMessage))
                {
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, resultModel.errorMessage);
                }

                DocumentModel[] recordDocumentList = GetRecordDocumentList();
                GridViewDataSource = CreateDataSource(resultModel.documentList, recordDocumentList);
                BindAttachmentList();
            }
        }

        /// <summary>
        /// bind data source
        /// </summary>
        public void BindAttachmentList()
        {
            gdvAttachmentList.DataSource = GridViewDataSource;
            gdvAttachmentList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("ins_attachmentList_message_noRecord");
            gdvAttachmentList.DataBind();
        }

        #endregion Public Methods

        #region Protected Methods

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
        /// override the on initial event
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvAttachmentList, string.Empty, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Document List Grid View Sort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">The Event Args object containing the event data.</param>
        protected void AttachmentList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            GridViewDataSource.DefaultView.Sort = e.GridViewSortExpression;
            GridViewDataSource = GridViewDataSource.DefaultView.ToTable();
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
                AccelaLabel lblEntityType = (AccelaLabel)e.Row.FindControl("lblEntityType");
                AccelaLabel lblVirtualFolder = (AccelaLabel)e.Row.FindControl("lblVirtualFolders");
                AccelaLabel lblDescription = (AccelaLabel)e.Row.FindControl("lblDescription");

                lblDescription.EllipsisContainerID = "ACADialogFrame";

                if (EdmsPolicyModel != null)
                {
                    DataRowView rowView = (DataRowView)e.Row.DataItem;
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    object entityType = rowView[ColumnConstant.Attachment.EntityType.ToString()];

                    if (!(entityType is DBNull))
                    {
                        lblEntityType.Text = AttachmentUtil.FormatEntityType(entityType.ToString(), ModuleName);
                    }

                    string agencyCode = rowView[ColumnConstant.Attachment.AgencyCode.ToString()].ToString();
                    string refAgencyCode = rowView[ColumnConstant.Attachment.RefAgencyCode.ToString()].ToString();

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

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Hide grid's column again.This grid require to set "AccessibleHeaderText" of template field. It is unique.
        /// </summary>
        private void HideGridViewColumns()
        {
            //People document list only get Agency level view layout.
            //licensee deail page need get agency level view layout.
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
            SimpleViewElementModel4WS[] smpViewElementModels = gviewBll.GetSimpleViewElementModel(string.Empty, gdvAttachmentList.GridViewNumber);

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
                        column.Visible = smpViewElementModels[j].recStatus != ACAConstant.INVALID_STATUS;
                    }
                }
            }
        }

        /// <summary>
        /// create data source for attachment list
        /// </summary>
        /// <param name="documentList"> a list of documents</param>
        /// <param name="recordDocumentList">the current record document list</param>
        /// <returns>data table for document.</returns>
        private DataTable CreateDataSource(DocumentModel[] documentList, DocumentModel[] recordDocumentList)
        {
            DataTable dtDocument = AttachmentUtil.CreateDocumentDataTable();

            if (documentList == null || documentList.Length == 0)
            {
                return dtDocument;
            }

            List<FileUploadInfo> fileUploadInfos = new List<FileUploadInfo>();
            Dictionary<string, List<FileUploadInfo>> htFileUploadInfos = new Dictionary<string, List<FileUploadInfo>>();

            IProxyUserRoleBll proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            
            //Get selected document list from "select from account"
            htFileUploadInfos = AppSession.GetUploadFileInfoFromSession(ModuleName);

            if (htFileUploadInfos != null && htFileUploadInfos.Count > 0)
            {
                foreach (KeyValuePair<string, List<FileUploadInfo>> kvp in htFileUploadInfos)
                {
                    fileUploadInfos.AddRange(kvp.Value);
                }
            }

            bool isViewableForAllUser = StandardChoiceUtil.IsEnabledAllUserDocumentPermission(ConfigManager.AgencyCode, ModuleName, VIEW_RIGHT);

            // Check the document if need to showing
            foreach (DocumentModel document in documentList)
            {
                if (document == null)
                {
                    continue;
                }

                bool isDisplayDocTitleLink;

                // 1.If view title role is not NULL and have view record or manage document permission, public user can view the document
                if (document.viewTitleRoleModel != null)
                {
                    UserRolePrivilegeModel userRole = document.viewTitleRoleModel;
                    isDisplayDocTitleLink = proxyUserRoleBll.HasPermission(AppSession.GetCapModelFromSession(ModuleName), userRole, ProxyPermissionType.VIEW_RECORD);

                    if (!isDisplayDocTitleLink)
                    {
                        isDisplayDocTitleLink = proxyUserRoleBll.HasPermission(AppSession.GetCapModelFromSession(ModuleName), userRole, ProxyPermissionType.MANAGE_DOCUMENTS);
                    }
                }
                else
                {
                    // 2.ACA Permission In V360
                    isDisplayDocTitleLink = document.viewTitleable
                                            || AttachmentUtil.IsFileOwner(document)
                                            || isViewableForAllUser;

                    // 3.File Owner permission In V360
                    if (!string.IsNullOrEmpty(document.fileOwnerPermission))
                    {
                        isDisplayDocTitleLink = AttachmentUtil.CheckFileOwnerPermission(document.fileOwnerPermission, FileOwnerPermission.TitleViewable);
                    }
                }

                if (!isDisplayDocTitleLink)
                {
                    continue;
                }

                bool isSkip = false;

                // 4.If the document have contains in the record then should not showing this document in select from account list
                if (recordDocumentList != null)
                {
                    if (recordDocumentList.Any(documentModel => documentModel.sourceDocNbr.Equals(document.documentNo)))
                    {
                        isSkip = true;
                    }
                }

                // 5.If the document have been selected in the record then should not showing this document in select from account list
                if (!isSkip && fileUploadInfos != null)
                {
                    if (fileUploadInfos.Any(documentModel => documentModel.DocumentModel.sourceDocNbr.Equals(document.documentNo)))
                    {
                        isSkip = true;
                    }
                }

                // 6.If the document have not upload finished(Pending) then should not showing this document in select from account list
                if (document.recDate != null)
                {
                    if (!isSkip && (I18nDateTimeUtil.FormatToDateStringForWebService(document.recDate.Value).Equals(ACAConstant.FILE_PENDING_DATE, StringComparison.InvariantCulture)
                                   || I18nDateTimeUtil.FormatToDateStringForWebService(document.recDate.Value).Equals(ACAConstant.FILE_FAILED_DATE, StringComparison.InvariantCulture)))
                    {
                        isSkip = true;
                    }
                }

                if (isSkip)
                {
                    continue;
                }

                string subAgencyCode = document.capID != null ? document.capID.serviceProviderCode : ConfigManager.AgencyCode;

                DataRow dr = AttachmentUtil.GetDocumentDataRow(dtDocument, document, subAgencyCode);

                dtDocument.Rows.Add(dr);
            }

            return dtDocument;
        }

        /// <summary>
        /// Get current record document list
        /// </summary>
        /// <returns>Return the current record document list, If existing EDMS Service error return null.</returns>
        private DocumentModel[] GetRecordDocumentList()
        {
            try
            {
                DocumentModel[] recordDocumentList = null;
                IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && capModel.capID != null)
                {
                    CapIDModel capID = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);

                    //Get record document list
                    recordDocumentList = edmsDocBll.GetRecordDocumentList(ConfigManager.AgencyCode, ModuleName, AppSession.User.PublicUserId, capID, CapUtil.IsPartialCap(capModel.capClass));
                }

                return recordDocumentList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Private Methods
    }
}