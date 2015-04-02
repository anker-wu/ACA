#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: DocumentDetail.aspx.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: A web form to show document details.
*
*  Notes:
* $Id: DocumentDetail.aspx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 20, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Attachment
{
    /// <summary>
    /// A web form to show document details
    /// </summary>
    public partial class DocumentDetail : PopupDialogBasePage
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether current document list is people document list.
        /// </summary>
        private bool IsPeopleDocument
        {
            get
            {
                return ValidationUtil.IsTrue(Request.QueryString["isPeopleDocument"]);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handle the Page_Load event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_documentdetail_label_title");
            SetDialogMaxHeight("500");

            if (!AppSession.IsAdmin)
            {
                string agencyCode = Request.QueryString[UrlConstant.AgencyCode];
                string documentNo = Request.QueryString["documentNo"];
                string specificEntity = Request.QueryString["specificEntity"];

                IEDMSDocumentBll edmsBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
                DocumentModel document = edmsBll.GetDocumentByPk(agencyCode, long.Parse(documentNo), !IsPeopleDocument);
                document.entity = specificEntity;
                DisplayDocDetails(document);
            }
        }

        /// <summary>
        /// Display document model to UI.
        /// </summary>
        /// <param name="document">Document model.</param>
        private void DisplayDocDetails(DocumentModel document)
        {
            lblFileName.Value = document.fileName;
            lblFileSize.Value = AttachmentUtil.FormatFileSize(document.fileSize);
            lblDocType.Value = I18nStringUtil.GetString(document.resDocCategory, document.docCategory);
            lblDescription.Value = document.docDescription;
            lblEntityType.Value = AttachmentUtil.FormatEntityType(document.entityType, ModuleName);
            lblEntity.Value = AttachmentUtil.GetSpecificEntity(document);
            lblDocumentStatus.Value = document.docStatus;
            lblUploadDate.Value = I18nDateTimeUtil.FormatToDateStringForUI(document.fileUpLoadDate);
            lblStatusDate.Value = I18nDateTimeUtil.FormatToDateStringForUI(document.docStatusDate);
            lblLatestUpdateDate.Value = I18nDateTimeUtil.FormatToDateStringForUI(document.recDate);

            if (IsPeopleDocument)
            {
                lblRecordType.Visible = false;
                lblVirtualFolders.Visible = false;
                lblRecordNumber.Visible = false;
            }
            else
            {
                lblRecordNumber.Value = document.altId;
                lblRecordType.Value = document.capTypeAlias;

                if (!string.IsNullOrEmpty(document.virtualFolders))
                {
                    string[] virtualFolders = document.virtualFolders.Split(ACAConstant.SPLIT_CHAR_SEMICOLON);

                    //get virtual folder by cap type
                    CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                    //In super agency environment, can not get the correct I18n value for virtual folders.
                    bool isDocCreatedInSuperAgency = !string.IsNullOrEmpty(document.refServProvCode)
                        && !string.Equals(document.serviceProviderCode, document.refServProvCode, StringComparison.OrdinalIgnoreCase);

                    if (cap != null
                        && cap.capType != null
                        && !string.IsNullOrEmpty(cap.capType.virtualFolderGroup)
                        && !isDocCreatedInSuperAgency)
                    {
                        virtualFolders = AttachmentUtil.GetVirtualFolderTextByValue(cap.capType.virtualFolderGroup, virtualFolders);
                    }

                    lblVirtualFolders.Value = DataUtil.ConcatStringWithSplitChar(virtualFolders, ACAConstant.SPLIT_CHAR_SEMICOLON.ToString() + ACAConstant.BLANK);
                }
            }

            genericTemplate.Display(document.template);
        }

        #endregion Methods
    }
}