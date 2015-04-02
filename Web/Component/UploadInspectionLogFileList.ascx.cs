#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UploadInspectionLogFileList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: The upload inspection log file list.
 *
 *  Notes:
 *      $Id: UploadInspectionLogFileList.ascx.cs 252704 2013-06-25 10:05:17Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provides the upload inspection log file list.
    /// </summary>
    public partial class UploadInspectionLogFileList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The command for download.
        /// </summary>
        protected const string COMMAND_DOWNLOAD = "Download";

        /// <summary>
        /// The Inspection Result Log.
        /// </summary>
        private const string DOCUMENT_FLAG_LOG_FILE = "INSPECTION_RESULT_LOG";

        /// <summary>
        /// The export file name for log file.
        /// </summary>
        private const string EXPORT_FILENAME_LOG_FILE = "LogFile";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets license list data source.
        /// </summary>
        private DataTable GridViewDataSource
        {
            get
            {
                if (ViewState["GridViewDataSource"] == null)
                {
                    ViewState["GridViewDataSource"] = CreateDataTable4LogFile();
                }

                return (DataTable)ViewState["GridViewDataSource"];
            }

            set
            {
                ViewState["GridViewDataSource"] = value;
            }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvLogFileList, string.Empty, AppSession.IsAdmin);

            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvLogFileList.ShowExportLink = true;
                gdvLogFileList.ExportFileName = EXPORT_FILENAME_LOG_FILE;
            }
            else
            {
                gdvLogFileList.ShowExportLink = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Page load event method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // When export CSV form request, it need Re-bind grid view.
            if ((AppSession.IsAdmin && !IsPostBack)
                || (IsPostBack && Request.Form[Page.postEventSourceID] != null && Request.Form[Page.postEventSourceID].IndexOf("btnExport") > -1))
            {
                gdvLogFileList.DataSource = GridViewDataSource;
                gdvLogFileList.DataBind();

                return;
            }

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                GridViewDataSource = GetLogFileDataSource();

                gdvLogFileList.DataSource = GridViewDataSource;
                gdvLogFileList.DataBind();
            }
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LogFileList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            GridViewDataSource.DefaultView.Sort = e.GridViewSortExpression;
            GridViewDataSource = GridViewDataSource.DefaultView.ToTable();
        }

        /// <summary>
        /// GridView row command event method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">A GridViewCommandEventArgs object containing the event data.</param>
        protected void LogFileList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string entityInfo = (string)e.CommandArgument;
            string[] entityInfoList = entityInfo.Split(ACAConstant.SPLIT_CHAR);

            if (COMMAND_DOWNLOAD.Equals(e.CommandName) && entityInfoList.Length >= 5)
            {
                string documentNo = entityInfoList[0];
                string fileKey = entityInfoList[1];
                string entityId = entityInfoList[2];
                string entityType = entityInfoList[3];
                string agencyCode = entityInfoList[4];

                try
                {
                    // Download document
                    string downloadUrl = string.Format(
                        "Handlers/FileHandler.ashx?action=Download&agency={0}&entityId={1}entityType={2}&{3}={4}&fileKey={5}", agencyCode, entityId, entityType, UrlConstant.DOCUMENT_NO, documentNo, fileKey);

                    Response.Redirect(FileUtil.AppendApplicationRoot(downloadUrl));
                }
                catch (ACAException)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_common_technical_difficulty"), false, -1);
                }
            }

            gdvLogFileList.DataSource = GridViewDataSource;
            gdvLogFileList.DataBind();
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Get the log file data source.
        /// </summary>
        /// <returns>LogFile Data Table</returns>
        private DataTable GetLogFileDataSource()
        {
            if (AppSession.IsAdmin)
            {
                return null;
            }

            DocumentModel[] logFileDocs = GetLogFileDocumentList();

            if (logFileDocs == null || logFileDocs.Length <= 0)
            {
                return null;
            }

            DataTable dt = CreateDataTable4LogFile();

            foreach (DocumentModel model in logFileDocs)
            {
                string[] entityInfo = new[]
                                     {
                                         model.documentNo != null ? model.documentNo.ToString() : string.Empty,
                                         model.fileKey,
                                         model.entityID,
                                         model.entityType,
                                         model.serviceProviderCode
                                     };

                DataRow dr = dt.NewRow();
                dr["DocumentNo"] = model.documentNo;
                dr["FileName"] = model.fileName;
                dr["DocDate"] = model.recDate;
                dr["EntityInfo"] = DataUtil.ConcatStringWithSplitChar(entityInfo, ACAConstant.SPLIT_CHAR.ToString());
                dt.Rows.Add(dr);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "DocDate DESC";
            dt = dv.ToTable();

            return dt;
        }

        /// <summary>
        /// Create data table construct for log file.
        /// </summary>
        /// <returns>Data table</returns>
        private DataTable CreateDataTable4LogFile()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DocumentNo", typeof(long));
            dt.Columns.Add("FileName", typeof(string));
            dt.Columns.Add("DocDate", typeof(DateTime));
            dt.Columns.Add("EntityInfo", typeof(string));

            return dt;
        }

        /// <summary>
        /// Get the log file document list.
        /// </summary>
        /// <returns>Document Model</returns>
        private DocumentModel[] GetLogFileDocumentList()
        {
            IEDMSDocumentBll documentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            EntityModel[] entityList = AttachmentUtil.ConstructEntityModel(ConfigManager.AgencyCode);

            if (entityList == null || entityList.Length == 0)
            {
                return null;
            }

            List<DocumentEntityAssociationModel> documentEntityList = new List<DocumentEntityAssociationModel>();

            foreach (EntityModel entity in entityList)
            {
                DocumentEntityAssociationModel model = new DocumentEntityAssociationModel
                                                           {
                                                               serviceProviderCode = ConfigManager.AgencyCode,
                                                               entityID = entity.entityID,
                                                               entityType = entity.entityType,
                                                               entityID1 = DOCUMENT_FLAG_LOG_FILE,
                                                               entityID2 = int.Parse(AppSession.User.UserSeqNum)
                                                           };
                documentEntityList.Add(model);
            }

            DocumentModel[] result = documentBll.GetDocumentList(ConfigManager.AgencyCode, documentEntityList.ToArray(), null, AppSession.User.PublicUserId);

            return result;
        }

        #endregion Private Methods
    }
}