#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ShowGeoDocuments.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  This class invokes web service EDMSDocumentWebService.java all method.
 *
 *  Notes:
 * $Id: ShowGeoDocuments.cs 179651 2010-08-24 06:28:29Z ACHIEVO\grady.lu $.
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
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.GIS
{
    /// <summary>
    /// Show Geo Documents
    /// </summary>
    public partial class ShowGeoDocuments : PopupDialogBasePage
    {
        #region Fields

        /// <summary>
        /// The constant for NO_DOC_TYPE
        /// </summary>
        private const string NO_DOC_TYPE = "NO\fDOC\fTYPE";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets Document list.
        /// </summary>
        protected DocumentModel[] DocumentList
        {
            get
            {
                if (ViewState["DocumentList"] != null)
                {
                    return (DocumentModel[])ViewState["DocumentList"];
                }

                return null;
            }

            set
            {
                ViewState["DocumentList"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Page Load method
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_showgeodocuments_label_pagetitle");
            SetDialogMaxHeight("800");
            
            if (!IsPostBack && !AppSession.IsAdmin)
            {
                AccelaHeightSeparate2.Visible = false;
                DocumentList = GetAllDocumentList();
                if (DocumentList != null && DocumentList.Length > 0)
                {
                    Dictionary<string, string> docTypes = GetAllDocumentTypes();
                    if (StandardChoiceUtil.IsEnableDocumentType() && docTypes.Count > 0)
                    {
                        BindDocumentTypes(docTypes);
                    }
                    else
                    {
                        dvDocType.Visible = false;
                    }
                }
                else
                {
                    dvCondition.Visible = false;
                    noDataMessageDocList.Show(MessageType.Notice, "per_permitList_Error_noResult", MessageSeperationType.Both);
                }
            }
            else if (AppSession.IsAdmin)
            {
                ckbALL.AutoPostBack = false;
                ckbDocType.AutoPostBack = false;
                ckbDocType.Visible = false;
                docList.DataSource = CreateTable();
                docList.BindDocumentList();
            }
        }

        /// <summary>
        /// Search document
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            DocumentModel[] docs = GetDocumentsByHistory(ckbIsHistorical.Checked);
            if (ckbALL.Checked || dvDocType.Visible == false)
            {
                dt = CreateDataSource(docs);
            }
            else
            {
                List<string> docTypes = GetSelectDocTypes();
                if (docTypes.Count > 0)
                {
                    List<DocumentModel> documents = new List<DocumentModel>();
                    foreach (DocumentModel doc in docs)
                    {
                        if (docTypes.Contains(doc.docCategory) || (docTypes.Contains(NO_DOC_TYPE) && string.IsNullOrEmpty(doc.docCategory)))
                        {
                            documents.Add(doc);
                        }
                    }

                    dt = CreateDataSource(documents.ToArray());
                }
            }

            dvCondition.Visible = false;
            if (dt == null || dt.Rows.Count == 0)
            {
                noDataMessageDocList.Show(MessageType.Notice, "per_permitList_Error_noResult", MessageSeperationType.Both);
            }
            else
            {
                docList.DataSource = dt;
                docList.BindDocumentList();
            }
        }

        /// <summary>
        /// CheckBox ALL change event handler.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void CheckALL_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem item in ckbDocType.Items)
            {
                item.Selected = ((Accela.Web.Controls.AccelaCheckBox)sender).Checked;
            }
        }

        /// <summary>
        /// CheckBox selected index changed.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void DocTypeCheckBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isALL = true;
            foreach (ListItem item in ckbDocType.Items)
            {
                if (!item.Selected)
                {
                    isALL = false;
                    break;
                }
            }

            ckbALL.Checked = isALL;
        }

        /// <summary>
        /// Bind document type.
        /// </summary>
        /// <param name="docTypes">The document type list.</param>
        private void BindDocumentTypes(Dictionary<string, string> docTypes)
        {
            foreach (KeyValuePair<string, string> docType in docTypes)
            {
                ListItem item = new ListItem();
                item.Text = docType.Value;
                item.Value = docType.Key;
                item.Selected = true;
                ckbDocType.Items.Add(item);
            }

            ckbALL.Checked = true;
        }

        /// <summary>
        /// Create DataSource from document list.
        /// </summary>
        /// <param name="documentList">The document list.</param>
        /// <returns>DataTable of document information.</returns>
        private DataTable CreateDataSource(DocumentModel[] documentList)
        {
            DataTable dt = CreateTable();
            if (documentList != null)
            {
                int rowIndex = 0;
                foreach (DocumentModel item in documentList)
                {
                    DataRow row = dt.NewRow();
                    row["Name"] = item.fileName;
                    row["Description"] = item.docDescription;
                    row["ParcelNumber"] = item.parcelNumber;

                    row["RecordNumber"] = item.altId;
                    row["RecordType"] = item.capTypeAlias;
                    row["EntityType"] = GetEntityType(item.entityType);

                    row["DocType"] = string.IsNullOrEmpty(item.resDocCategory) ? item.docCategory : item.resDocCategory;
                    row["Size"] = AttachmentUtil.FormatFileSize(item.fileSize);
                    row["Date"] = item.recDate == null ? (object)DBNull.Value : item.recDate.Value;

                    row["DownloadRight"] = item.viewable;
                    row["EdmsPolicy"] = item.policy;
                    row["DocumentID"] = item.documentNo;

                    row["FileKey"] = item.fileKey;
                    row["AgencyCode"] = item.serviceProviderCode;
                    row["EntityId"] = item.entityID;

                    row["ReadOnly"] = !item.viewable;
                    row["Id"] = rowIndex;
                    row["Agency"] = item.serviceProviderCode;

                    row["DocumentStatus"] = item.docStatus == null ? (object)DBNull.Value : item.docStatus;
                    row["UploadDate"] = item.fileUpLoadDate == null ? (object)DBNull.Value : item.fileUpLoadDate;
                    row["StatusDate"] = item.docStatusDate == null ? (object)DBNull.Value : item.docStatusDate;
                    row["VirtualFolders"] = item.virtualFolders;

                    rowIndex++;
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// Create DataTable to document List.
        /// </summary>
        /// <returns>DataTable of document list.</returns>
        private DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("ParcelNumber", typeof(string)));

            dt.Columns.Add(new DataColumn("RecordNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("RecordType", typeof(string)));
            dt.Columns.Add(new DataColumn("EntityType", typeof(string)));

            dt.Columns.Add(new DataColumn("DocType", typeof(string)));
            dt.Columns.Add(new DataColumn("Size", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));

            dt.Columns.Add(new DataColumn("DownloadRight", typeof(bool)));
            dt.Columns.Add(new DataColumn("EdmsPolicy", typeof(EdmsPolicyModel)));
            dt.Columns.Add(new DataColumn("DocumentID", typeof(string)));

            dt.Columns.Add(new DataColumn("FileKey", typeof(string)));
            dt.Columns.Add(new DataColumn("AgencyCode", typeof(string)));
            dt.Columns.Add(new DataColumn("EntityId", typeof(string)));

            dt.Columns.Add(new DataColumn("ReadOnly", typeof(bool)));
            dt.Columns.Add(new DataColumn("Id", typeof(int)));
            dt.Columns.Add(new DataColumn("Agency", typeof(string)));

            dt.Columns.Add(new DataColumn("DocumentStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("UploadDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("StatusDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("VirtualFolders", typeof(string)));

            return dt;
        }

        /// <summary>
        /// Get All Document list.
        /// </summary>
        /// <returns>List of all relative document.</returns>
        private DocumentModel[] GetAllDocumentList()
        {
            ////List<DocumentModel> documentlist = new List<DocumentModel>();
            DocumentModel[] documentList = null;
            IEDMSDocumentBll documentBll = ObjectFactory.GetObject(typeof(IEDMSDocumentBll)) as IEDMSDocumentBll;
            ACAGISModel model = Session["ShowDocuments"] as ACAGISModel;
            if (model != null)
            {
                Dictionary<string, string> modules = TabUtil.GetAllEnableModules(false);
                List<string> moduelist = new List<string>();

                foreach (KeyValuePair<string, string> item in modules)
                {
                    moduelist.Add(item.Key);
                }

                documentList = documentBll.GetDocumentListByGisObject(ConfigManager.AgencyCode, model.GisObjects, moduelist.ToArray(), AppSession.User.PublicUserId);
                if (documentList != null && documentList.Length > 0)
                {
                    documentList = documentList.Where(d => d.viewTitleable == true).ToArray();
                }
            }

            return documentList;
        }

        /// <summary>
        /// Get all document types.
        /// </summary>
        /// <returns>return all document types.</returns>
        private Dictionary<string, string> GetAllDocumentTypes()
        {
            Dictionary<string, string> docTypelist = new Dictionary<string, string>();
            Dictionary<string, string> sortedDocTypelist = new Dictionary<string, string>();

            if (DocumentList != null && DocumentList.Length > 0)
            {
                bool hasEmptyDocType = false;

                // Get all the selected document type.
                foreach (DocumentModel doc in DocumentList)
                {
                    if (!string.IsNullOrEmpty(doc.docCategory) && !docTypelist.ContainsKey(doc.docCategory))
                    {
                        docTypelist.Add(doc.docCategory, doc.resDocCategory);
                    }
                    else if (string.IsNullOrEmpty(doc.docCategory))
                    {
                        hasEmptyDocType = true;
                    }
                }

                // Sort the document types.
                var sortedList = docTypelist.OrderBy(entry => entry.Value);
                foreach (KeyValuePair<string, string> k in sortedList)
                {
                    sortedDocTypelist.Add(k.Key, k.Value);
                }

                // If No Document Type exists, add it to the end of the list.
                if (hasEmptyDocType)
                {
                    string noDocType = GetTextByKey("aca_showgeodocuments_label_nodocumenttype");
                    sortedDocTypelist.Add(NO_DOC_TYPE, noDocType);
                }
            }

            return sortedDocTypelist;
        }

        /// <summary>
        /// Get DocumentModel Array by History
        /// </summary>
        /// <param name="isIncludeHistory">a value indicating whether document is historical</param>
        /// <returns>DocumentModel Array</returns>
        private DocumentModel[] GetDocumentsByHistory(bool isIncludeHistory)
        {
            List<DocumentModel> list = new List<DocumentModel>();
            if (DocumentList != null)
            {
                foreach (DocumentModel item in DocumentList)
                {
                    if (isIncludeHistory)
                    {
                        list.Add(item);
                    }
                    else if (!item.history)
                    {
                        list.Add(item);
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Gets Entity Type label 
        /// </summary>
        /// <param name="entityType">entity type</param>
        /// <returns>return entity type label</returns>
        private string GetEntityType(string entityType)
        {
            string strLabelType = string.Empty;
            switch (entityType)
            {
                case "CAP":
                    strLabelType = LabelUtil.GetGlobalTextByKey("aca_showgeodocuments_label_recordtype_record");
                    break;
                case "PARCEL":
                    strLabelType = LabelUtil.GetGlobalTextByKey("aca_showgeodocuments_label_recordtype_parcel");
                    break;
            }

            return strLabelType;
        }

        /// <summary>
        /// Get Selected Document types.
        /// </summary>
        /// <returns>Selected document types.</returns>
        private List<string> GetSelectDocTypes()
        {
            List<string> docTypes = new List<string>();
            if (ckbDocType.Items.Count > 0)
            {
                foreach (ListItem item in ckbDocType.Items)
                {
                    if (item.Selected)
                    {
                        docTypes.Add(item.Value);
                    }
                }
            }

            return docTypes;
        }

        #endregion Methods
    }
}