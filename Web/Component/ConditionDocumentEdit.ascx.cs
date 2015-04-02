#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConditionDocumentEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ConditionDocumentEdit.ascx.cs 123865 2009-03-16 09:40:24Z ACHIEVO\meit.mei $.
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
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AttachmentEdit.
    /// </summary>
    public partial class ConditionDocumentEdit : FileUploadBase
    {
        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !AppSession.IsAdmin)
            {
                BindRequiredDocumentSource();
                CurrentFileUploadBehavior = StandardChoiceUtil.GetFileUploadBehavior();
            }
        }

        /// <summary>
        /// OnPreRender Event.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            divAttachmentField4Admin.Visible = AppSession.IsAdmin;

            if (!AppSession.IsAdmin)
            {
                string maxFileSize = AttachmentUtil.GetMaxFileSizeWithUnit(ModuleName);
                string disAllowedFileTypes = AttachmentUtil.GetDisallowedFileType(ModuleName);
                
                lblSizeIntroduction.Visible = false;
                lblTypeIntroduction.Visible = false;

                if (!string.IsNullOrEmpty(maxFileSize))
                {
                    lblSizeIntroduction.Visible = true;
                    lblSizeIntroduction.Text = GetTextByKey("aca_conditiondocument_label_sizelimitation").Replace(AttachmentUtil.FileUploadVariables.MaximumFileSize, maxFileSize);
                }

                if (!string.IsNullOrEmpty(disAllowedFileTypes))
                {
                    lblTypeIntroduction.Visible = true;
                    lblTypeIntroduction.Text = GetTextByKey("aca_conditiondocument_label_typelimitation").Replace(AttachmentUtil.FileUploadVariables.ForbiddenFileFormats, disAllowedFileTypes);
                }
            }
        }

        /// <summary>
        /// AttachmentList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void RequiredDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                FileSelected fileSelected = e.Row.FindControl("fileSelected") as FileSelected;
                fileSelected.ConditionNumber = long.Parse(rowView["ConditionNumber"].ToString());
                fileSelected.IsMultipleUpload = ACAConstant.COMMON_N;
                fileSelected.ParentDocumentNo = Request[UrlConstant.DOCUMENT_NO];
                fileSelected.ParentAgencyCode = Request[UrlConstant.AgencyCode];
                string fileName = rowView["FileName"].ToString();
                string description = rowView["DispConditionDescription"].ToString();
                fileSelected.InitFileNameControl(fileName, description);
            }
        }

        /// <summary>
        /// download or delete command.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RequiredDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Page")
            {
                BindRequiredDocumentSource();
            }
        }

        /// <summary>
        /// Bind document data source.
        /// </summary>
        private void BindRequiredDocumentSource()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel == null || capModel.capID == null)
            {
                return;
            }
            
            DataTable table = new DataTable();
            table.Columns.Add("DocumentIndex");
            table.Columns.Add("conditionGroup");
            table.Columns.Add("dispConditionDescription");
            table.Columns.Add("ConditionNumber");
            table.Columns.Add("FileName");

            DocumentModel[] docList = GetRecordDocumentList(capModel);
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
            List<CapConditionModel4WS> conditions = ConditionsUtil.GetFilteredCapConditions(capModel.capID);

            if (conditions != null && conditions.Count > 0)
            {
                for (int index = 0; index < conditions.Count; index++)
                {
                    CapConditionModel4WS condition = conditions[index];
                    DocumentModel documentModel = GetDocumentModel(condition.conditionNumber, docList);

                    DataRow dr = table.NewRow();
                    dr["DocumentIndex"] = (index + 1) + ACAConstant.SPOT_CHAR;
                    dr["conditionGroup"] = bizDomainBll.GetConditionGroupFor18N(capModel.capID.serviceProviderCode, condition.conditionGroup);
                    dr["dispConditionDescription"] = string.IsNullOrEmpty(condition.resConditionDescription) ? condition.conditionDescription : condition.resConditionDescription;
                    dr["ConditionNumber"] = condition.conditionNumber;
                    dr["FileName"] = documentModel == null ? null : documentModel.fileName;

                    table.Rows.Add(dr);
                }
            }

            if (table.Rows.Count == 0)
            {
                divInstruction.Visible = false;
                gdvRequiredDocument.EmptyDataText = LabelUtil.GetTextByKey("aca_requireddocument_label_norequireddocument", ModuleName);
            }

            gdvRequiredDocument.DataSource = table;
            gdvRequiredDocument.DataBind();
        }

        /// <summary>
        /// The record document list.
        /// </summary>
        /// <param name="capModel">the cap model.</param>
        /// <returns>Record document list.</returns>
        private DocumentModel[] GetRecordDocumentList(CapModel4WS capModel)
        {
            try
            {
                IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                CapIDModel capId = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
                DocumentModel[] docList = edmsDocBll.GetRecordDocumentList(capModel.capID.serviceProviderCode, ModuleName, AppSession.User.PublicUserId, capId, true);
               
                return docList;
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }

            return null;
        }

        /// <summary>
        /// Get document model
        /// </summary>
        /// <param name="conditionNumber">the condition number.</param>
        /// <param name="docList">The document List.</param>
        /// <returns>the document model.</returns>
        private DocumentModel GetDocumentModel(long conditionNumber, DocumentModel[] docList)
        {
            DocumentModel docModel = null;

            if (docList != null && docList.Any(t => conditionNumber == t.conditionNumber))
            {
                docModel = docList.FirstOrDefault(t => conditionNumber == t.conditionNumber);
            }

            return docModel;
        }
    }
}
