#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GeoDocumentList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: GeoDocumentList.ascx.cs 172830 2010-05-15 09:17:46Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web.Component
{
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Accela.ACA.BLL.Attachment;
    using Accela.ACA.Common;
    using Accela.ACA.Web.Common;
    using Accela.ACA.Web.Common.Control;
    using Accela.ACA.WSProxy;
    using Accela.Web.Controls;

    /// <summary>
    /// Geographic document list component
    /// </summary>
    public partial class GeoDocumentList : BaseUserControl
    {
        #region Events

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets DataSource
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["DocumentDataSource"] != null)
                {
                    return (DataTable)ViewState["DocumentDataSource"];
                }

                return null;
            }

            set
            {
                ViewState["DocumentDataSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind DocumentList
        /// </summary>
        public void BindDocumentList()
        {
            gdvDocumentList.DataSource = DataSource;
            gdvDocumentList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event handler
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvDocumentList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// GridView DocumentList GridViewSort event
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        protected void DocumentList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                DataSource.DefaultView.Sort = e.GridViewSortExpression;
                DataSource = DataSource.DefaultView.ToTable();
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// GridView DocumentList PageIndex Changing event
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        protected void DocumentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// GridView DocumentList RowCommand event
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        protected void DocumentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "download")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());
                DataTable dt = DataSource;
                DataRow row = dt.Rows[rowIndex];

                EdmsPolicyModel policy = row["EdmsPolicy"] as EdmsPolicyModel;
                EntityModel entity = new EntityModel();
                entity.entityType = row["EntityType"].ToString();
                entity.serviceProviderCode = row["AgencyCode"].ToString();
                entity.entityID = row["EntityId"].ToString();
                string documentNo = row["DocumentID"].ToString();
                string fileKey = (string)row["FileKey"];
                string file = (string)row["Name"];
                DownloadAttachment(entity, policy, documentNo, fileKey, file);
            }
            else
            {
                BindDocumentList();
            }
        }

        /// <summary>
        /// DocumentList Row Bound event handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments</param>
        protected void DocumentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                AccelaLabel lblVirtualFolder = (AccelaLabel)e.Row.FindControl("lblVirtualFolders");
                LinkButton lnk = e.Row.FindControl("lnkFileName") as LinkButton;
                ((ScriptManager)this.Page.Master.FindControl("ScriptManager1")).RegisterPostBackControl(lnk);

                //display virtual folder
                string virtualFolderValue = Convert.ToString(rowView["VirtualFolders"]);

                if (!string.IsNullOrEmpty(virtualFolderValue))
                {
                    string[] virtualFolderTexts = virtualFolderValue.Split(ACAConstant.SPLIT_CHAR_SEMICOLON);
                    lblVirtualFolder.Text = DataUtil.ConcatStringWithSplitChar(virtualFolderTexts, ACAConstant.HTML_BR);
                }
            }
        }

        /// <summary>
        /// download a file
        /// </summary>
        /// <param name="entity">Entity Model.</param>
        /// <param name="policy">EDMS Policy Model.</param>
        /// <param name="documentNo">Document number.</param>
        /// <param name="fileKey">The field key.</param>
        /// <param name="file">The field name.</param>        /// 
        private void DownloadAttachment(EntityModel entity, EdmsPolicyModel policy, string documentNo, string fileKey, string file)
        {
            string callerID = AppSession.User.PublicUserId;
            string agencyCode = ConfigManager.AgencyCode;

            //invoke the EDMS's to get a file.
            IEDMSDocumentBll edmsBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
            try
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(file).Replace("+", "%20")); //resolve the filename with empty space char
                Response.ContentType = "application/octet-stream";
                DocumentModel documentModel = edmsBll.DoDownload(agencyCode, null, callerID, entity, policy, long.Parse(documentNo), fileKey, false);
                if (documentModel != null)
                {
                    DocumentContentModel docContent = documentModel.documentContent;
                    byte[] buffer = docContent.docContentStream;
                    string fileName = documentModel.fileName;
                    int fileSize = buffer.Length;

                    if (buffer.Length > 0)
                    {
                        Response.OutputStream.Write(buffer, 0, fileSize);
                    }
                }

                Response.Flush();
                Response.Close();
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageInParent(Page, MessageType.Error, GetTextByKey("aca_common_technical_difficulty"));
            }
        }

        #endregion Methods
    }
}
