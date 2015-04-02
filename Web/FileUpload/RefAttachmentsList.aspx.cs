#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAttachmentsList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAttachmentsList.aspx.cs 266805 2014-06-04 14:01:29Z ACHIEVO\zehon.cai $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.FileUpload
{
    /// <summary>
    /// The reference attachment list.
    /// </summary>
    public partial class RefAttachmentsList : PopupDialogBasePage
    {
        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogUtil.RegisterScriptForDialog(Page);
            SetPageTitleKey("aca_refattachmentlist_label_title");

            if (!IsPostBack)
            {
                refAttachmentList.DisplayAttachment();
            }
            else
            {
                refAttachmentList.BindAttachmentList();
            }
        }

        /// <summary>
        /// Save license professional information into licensed professional list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BtnContinueClick(object sender, EventArgs e)
        {
            List<DocumentModel> attachmentList = new List<DocumentModel>();

            if (refAttachmentList.SelectedItems != null && refAttachmentList.SelectedItems.Length > 0)
            {
                attachmentList.AddRange(refAttachmentList.SelectedItems);
            }

            List<FileUploadInfo> fileInfos = new List<FileUploadInfo>();

            foreach (DocumentModel item in attachmentList)
            {
                FileUploadInfo fileInfo = new FileUploadInfo();
                fileInfo.FileId = Guid.NewGuid().ToString();
                fileInfo.StateString = ACAConstant.FINISHED_STATUS;
                fileInfo.FileName = item.fileName;

                if (fileInfo.DocumentModel == null)
                {
                    fileInfo.DocumentModel = new DocumentModel();
                }

                fileInfo.DocumentModel = item;
                fileInfo.DocumentModel.componentName = Request[UrlConstant.SECTION_NAME];
                fileInfo.DocumentModel.SelectFromAccountTemplate = item.template;
                fileInfo.DocumentModel.docCategory = string.Empty;
                fileInfo.DocumentModel.virtualFolders = string.Empty;
                fileInfo.DocumentModel.scoreFileFlag = string.Empty;
                fileInfo.DocumentModel.documentContent = null;
                fileInfo.DocumentModel.FileId = fileInfo.FileId;
                fileInfo.DocumentModel.FileState = fileInfo.StateString;
                fileInfo.DocumentModel.sourceSpc = item.serviceProviderCode;
                fileInfo.DocumentModel.sourceRecfulnam = item.recFulNam;
                fileInfo.DocumentModel.sourceDocNbr = item.documentNo;
                fileInfo.DocumentModel.sourceEntityType = item.entityType;
                fileInfo.DocumentModel.sourceEntityID = item.entityID;
                
                fileInfos.Add(fileInfo);
            }

            string result = JsonConvert.SerializeObject(fileInfos);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ClosePopup", "ClosePopup(" + result + ");", true);
        }

        #endregion Methods
    }
}