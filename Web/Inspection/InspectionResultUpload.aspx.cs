#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionResultUpload.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: Upload inspeciton result
 *
 *  Notes:
 *      $Id: InspectionResultUpload.aspx.cs 183850 2013-07-18 15:43:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// upload inspection result page
    /// </summary>
    public partial class InspectionResultUpload : BasePage
    {
        #region Fields

        /// <summary>
        /// The command for download.
        /// </summary>
        protected const string COMMAND_DOWNLOAD = "Download";

        /// <summary>
        /// Upload handler URL.
        /// </summary>
        protected readonly string UPLOAD_HANDLER_URL = FileUtil.AppendApplicationRoot("Handlers/FileHandler.ashx");

        /// <summary>
        /// Document category for distinguish what function to upload document, DocumentUploadAfter event will use this value to handle some logic.
        /// </summary>
        private const string INSPECTIONRESULT_DOCUPLOAD_CATEGORY_FOR_EMSE = "InspectionResult";

        /// <summary>
        /// The standard choice's description for inspection result format
        /// </summary>
        private const string INSPECTION_RESULT_FORMAT_COLUMN = "COLUMN";

        /// <summary>
        /// The standard choice's description for inspection result format
        /// </summary>
        private const string INSPECTION_RESULT_FORMAT_REQUIRED = "REQUIRED";

        /// <summary>
        /// inspection sequence number key
        /// </summary>
        private const string INSPECTION_SEQUENCE_NUMBER = "INSPECTION SEQUENCE NUMBER";

        /// <summary>
        /// inspection date
        /// </summary>
        private const string INSPECTION_DATE = "INSPECTION DATE";

        /// <summary>
        /// inspection status
        /// </summary>
        private const string INSPECTION_STATUS = "STATUS";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the LP total count
        /// </summary>
        protected int LPTotalCount
        {
            get
            {
                if (ViewState["LPTotalCount"] != null)
                {
                    return (int)ViewState["LPTotalCount"];
                }

                return 0;
            }

            set
            {
                ViewState["LPTotalCount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the license sequence number.
        /// </summary>
        private string LicSeqNbr
        {
            get
            {
                return ViewState["LicSeqNbr"] as string;
            }

            set 
            {
                ViewState["LicSeqNbr"] = value;
            }
        }

        #endregion Properties

        #region Web Methods

        /// <summary>
        /// Upload inspection attachment.
        /// </summary>
        /// <param name="inspectionEntity">The inspection entity that contain agency code, cap id, inspection sequence number.</param>
        /// <param name="jsFileInfos">The file information array.</param>
        /// <returns>Return the error message.</returns>
        [WebMethod(Description = "UploadInspectionAttachment", EnableSession = true)]
        public static string UploadInspectionAttachment(string inspectionEntity, string jsFileInfos)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            FileUploadInfo[] fileInfos = jsSerializer.Deserialize<FileUploadInfo[]>(jsFileInfos);

            if (fileInfos == null || fileInfos.Length <= 0)
            {
                return string.Empty;
            }

            string uploadFolder = AttachmentUtil.GetTempDirectory();

            // get the inspection entity information
            string[] arrInspectionEntity = null;

            if (!string.IsNullOrEmpty(inspectionEntity))
            {
                arrInspectionEntity = inspectionEntity.Split(ACAConstant.SPLIT_CHAR);
            }

            if (arrInspectionEntity == null || arrInspectionEntity.Length < 7)
            {
                return string.Empty;
            }

            string moduleName = arrInspectionEntity[5];

            if (AttachmentUtil.IsDisabledEDMS(arrInspectionEntity[0], moduleName))
            {
                return LabelUtil.GetGUITextByKey("aca.error.edms.datasourcenotfound");
            }

            EntityModel entityModel = new EntityModel
            {
                entityID = arrInspectionEntity[4],
                entityType = DocumentEntityType.Inspection,
                serviceProviderCode = ConfigManager.AgencyCode
            };

            CapIDModel4WS capID = new CapIDModel4WS
                                      {
                                          serviceProviderCode = arrInspectionEntity[0],
                                          id1 = arrInspectionEntity[1],
                                          id2 = arrInspectionEntity[2],
                                          id3 = arrInspectionEntity[3]
                                      };

            foreach (FileUploadInfo fileInfo in fileInfos)
            {
                DocumentModel documentModel = AttachmentUtil.ConstructDocumentModel(fileInfo, entityModel, capID);
                string filePath = Path.Combine(uploadFolder, fileInfo.FileName + fileInfo.FileId);

                if (!File.Exists(filePath) || documentModel == null)
                {
                    return LabelUtil.GetTextByKey("aca_uploadinspresult_msg_uploadfailed", string.Empty);
                }

                // upload inspection attachment
                EMSEResultBaseModel4WS emseResult = null;
                string errorMessage = string.Empty;

                try
                {
                    IEDMSDocumentBll edmsDocumentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                    emseResult = edmsDocumentBll.DoSynchronousUpload(moduleName, documentModel, filePath, false);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                if (emseResult != null)
                {
                    errorMessage = "0".Equals(emseResult.returnCode) ? string.Empty : emseResult.returnMessage;
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return errorMessage;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// The web method for get inspection attachments
        /// </summary>
        /// <param name="inspectionEntity">The inspection entity that contain agency code, cap id, inspection sequence number.</param>
        /// <returns>The inspection attachment list.</returns>
        [WebMethod(Description = "GetInspectionAttachments", EnableSession = true)]
        public static ArrayList GetInspectionAttachments(string inspectionEntity)
        {
            string[] arrInspectionEntity = null;

            if (!string.IsNullOrEmpty(inspectionEntity))
            {
                arrInspectionEntity = inspectionEntity.Split(ACAConstant.SPLIT_CHAR);
            }

            if (arrInspectionEntity == null || arrInspectionEntity.Length < 7)
            {
                return null;
            }

            IEDMSDocumentBll edmsDocumentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();

            DocumentEntityAssociationModel documentEntityModel = new DocumentEntityAssociationModel
            {
                serviceProviderCode = arrInspectionEntity[0],
                entityID = arrInspectionEntity[4],
                entityType = DocumentEntityType.Inspection,
                ID1 = arrInspectionEntity[1],
                ID2 = arrInspectionEntity[2],
                ID3 = arrInspectionEntity[3]
            };

            ArrayList list = new ArrayList();
            string moduleName = arrInspectionEntity[5];

            DocumentModel[] documents = edmsDocumentBll.GetDocumentList(ConfigManager.AgencyCode, new[] { documentEntityModel }, new[] { moduleName }, AppSession.User.PublicUserId);

            if (documents != null && documents.Length > 0)
            {
                foreach (DocumentModel model in documents)
                {
                    string documentRights = GetDocumentRights(model, moduleName);

                    list.Add(string.Format(
                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}", 
                        ACAConstant.SPLIT_CHAR, 
                        model.serviceProviderCode,
                        model.documentNo,
                        model.fileKey,
                        model.entityID,
                        model.entityType,
                        model.fileName,
                        moduleName,
                        documentRights));
                }
            }

            return list;
        }

        /// <summary>
        /// The web method to delete the attachments.
        /// </summary>
        /// <param name="documentValue">The document information.</param>
        [WebMethod(Description = "DeleteAttachments", EnableSession = true)]
        public static void DeleteAttachments(string documentValue)
        {
            if (string.IsNullOrEmpty(documentValue))
            {
                return;
            }

            string[] arrDocumentValue = documentValue.Split(ACAConstant.SPLIT_CHAR);

            if (arrDocumentValue != null && arrDocumentValue.Length >= 5)
            {
                string agencyCode = arrDocumentValue[0];
                string documentNo = arrDocumentValue[1];
                string fileKey = arrDocumentValue[2];
                string entityId = arrDocumentValue[3];
                string entityType = arrDocumentValue[4];
                string moduleName = arrDocumentValue[5];

                //string agencyCode, string documentNo, string fileKey, string entityId, string entityType
                DocumentModel document = new DocumentModel
                                            {
                                                serviceProviderCode = agencyCode,
                                                documentNo = long.Parse(documentNo),
                                                fileKey = fileKey,
                                                entityID = entityId,
                                                entityType = entityType
                                            };

                try
                {
                    //invoke the EDMS's to remove a file.
                    IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();

                    edmsBll.DoDelete(ConfigManager.AgencyCode, moduleName, document, AppSession.User.PublicUserId, false);
                }
                catch (ACAException)
                {
                }
            }
        }

        #endregion Web Methods

        #region Protected Methods

        /// <summary>
        /// Page load event method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Register lnkUploadFileCallback to void postback operation perform twice when refresh page
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(lnkUploadFileCallback);

            if (!AppSession.IsAdmin)
            {
                if (AppSession.User == null
                    || !AppSession.User.IsInspector
                    || !StandardChoiceUtil.IsEnabledUploadInspectionResult()
                    || AppSession.User.Licenses == null || AppSession.User.Licenses.Length == 0)
                {
                    Response.Redirect(ACAConstant.URL_DEFAULT);
                }

                DialogUtil.RegisterScriptForDialog(Page);
                MessageUtil.HideMessage(Page);
            }

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                // if LP more than 1, then show a popup to select a LP
                List<LicenseModel4WS> lpModelList = AttachmentUtil.GetAvaliableLicense4PeopleDocument(ConfigManager.AgencyCode);
                LPTotalCount = lpModelList != null ? lpModelList.Count : 0;

                if (lpModelList != null && lpModelList.Count == 1)
                {
                    LicSeqNbr = lpModelList[0].licSeqNbr;
                }
            }
        }

        /// <summary>
        /// Download new inspection event method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void DownloadNewInspectionsButton_Click(object sender, EventArgs e)
        {
            try
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                BizDomainModel4WS[] bizList = bizBll.GetBizDomainValue(ConfigManager.AgencyCode, BizDomainConstant.STD_INSPECTION_RESULT_CSV_FORMAT, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);
                if (bizList == null || bizList.Length == 0)
                {
                    MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, GetTextByKey("aca_uploadinspresult_msg_downloadfile_configundefined"));
                    return;
                }

                string fileName = string.Format("NewInspectionList{0}.csv", DateTime.Now.ToString("yyyyMMdd"));

                string downloadUrl = string.Format(
                    "Handlers/FileHandler.ashx?action=DownloadNewInspection&agency={0}&fileName={1}", ConfigManager.AgencyCode, fileName);

                Response.Redirect(FileUtil.AppendApplicationRoot(downloadUrl));
            }
            catch (ACAException)
            {
                MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, GetTextByKey("aca_common_technical_difficulty"), false, -1);
            }
        }

        /// <summary>
        /// Upload file's callback, update document info to database.
        /// </summary>
        /// <param name="sender">button document edit</param>
        /// <param name="e">click event</param>
        protected void UploadFileCallback(object sender, EventArgs e)
        {
            string senderArgs = Request.Form[Page.postEventArgumentID];
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            FileUploadInfo[] fileInfos = jsSerializer.Deserialize<FileUploadInfo[]>(senderArgs);
            
            if (fileInfos == null || fileInfos.Length <= 0)
            {
                return;
            }

            if (AttachmentUtil.IsDisabledEDMS(ConfigManager.AgencyCode, string.Empty))
            {
                 MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, GetTextByKey("aca.error.edms.datasourcenotfound"));
                 return;
            }
            
            if (DocumentEntityType.LP.Equals(hdnEntityType.Value, StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (FileUploadInfo fileInfo in fileInfos)
                {
                    if (!fileInfo.FileName.EndsWith(".CSV", StringComparison.InvariantCultureIgnoreCase))
                    {
                        MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, GetTextByKey("aca_uploadinspresult_msg_uploadfile_filetypeincorrect"));
                        return;
                    }
                }

                if (!ValidateUploadFile(fileInfos))
                {
                    return;
                }

                bool isAutoUpdateInspectionResult = StandardChoiceUtil.IsAutoUpdateInspectionResult();
                string uploadFolder = AttachmentUtil.GetTempDirectory();
  
                EntityModel entityModel = new EntityModel
                                              {
                                                  entityID = LPTotalCount == 1 ? LicSeqNbr : hdnEntity.Value,
                                                  entityType = DocumentEntityType.LP,
                                                  serviceProviderCode = ConfigManager.AgencyCode
                                              };

                foreach (FileUploadInfo fileInfo in fileInfos)
                {
                    DocumentModel documentModel = AttachmentUtil.ConstructDocumentModel(fileInfo, entityModel);
                    documentModel.fileOwnerPermission = ACAConstant.DEFAULT_FILEOWNERPERMISSION;
                    string filePath = Path.Combine(uploadFolder, fileInfo.FileName + fileInfo.FileId);

                    // upload inspection result
                    string errorMessage = UploadInspectionResult(documentModel, filePath, isAutoUpdateInspectionResult);

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, errorMessage);
                        return;
                    }
                }

                string successMsg = isAutoUpdateInspectionResult
                                        ? GetTextByKey("aca_uploadinspresult_msg_uploadresult_autoupdate_success")
                                        : GetTextByKey("aca_uploadinspresult_msg_uploadresult_success");

                MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Notice, successMsg);
            }
        }
       
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Get the document rights, view, download, delete rights.
        /// </summary>
        /// <param name="document">The document model.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The document rights.</returns>
        private static string GetDocumentRights(DocumentModel document, string moduleName)
        {
            // get the EDMS model for controlling the permission.
            string callerID = AppSession.User.PublicUserId;
            CapIDModel4WS capID = TempModelConvert.Add4WSForCapIDModel(document.capID);

            EdmsPolicyModel4WS edmsPolicy = CapUtil.GetEdmsPolicyModel4WS(document.serviceProviderCode, moduleName, callerID, capID);
            bool downloadRight = false;
            bool deleteRight = false;

            if (edmsPolicy != null)
            {
                downloadRight = string.IsNullOrEmpty(edmsPolicy.downloadRight) || ValidationUtil.IsTrue(edmsPolicy.downloadRight);
                deleteRight = string.IsNullOrEmpty(edmsPolicy.deleteRight) || ValidationUtil.IsTrue(edmsPolicy.deleteRight);
            }

            /*
             * Format: viewRight\fdownloadRight\fdeleteRight
             * In ACA, the viewRight is always true, even the view right of the edms is unchecked.
             */
            return string.Format(
                "{1}{0}{2}{0}{3}",
                ACAConstant.SPLIT_CHAR,
                ACAConstant.COMMON_Y,
                downloadRight ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                deleteRight ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        }

        /// <summary>
        /// Upload the inspection result
        /// </summary>
        /// <param name="documentModel">The document model.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="isAutoUpdateInspectionResult">Is auto-update inspection result or not.</param>
        /// <returns>Return error message if upload failed.</returns>
        private string UploadInspectionResult(DocumentModel documentModel, string filePath, bool isAutoUpdateInspectionResult)
        {
            // Some document might be deleted by anti-virtus software.
            if (!File.Exists(filePath) || documentModel == null)
            {
                return GetTextByKey("aca_uploadinspresult_msg_uploadfailed");
            }

            EMSEResultBaseModel4WS emseResult = null;
            string errorMessage = string.Empty;

            try
            {
                // Save upload file to DB and update the inspection result
                if (isAutoUpdateInspectionResult)
                {
                    IInspectionBll inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
                    emseResult = inspectionBll.UpdateInspectionByCSV(ConfigManager.AgencyCode, documentModel, filePath);

                    File.Delete(filePath);
                }
                else
                {
                    IEDMSDocumentBll edmsDocumentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                    emseResult = edmsDocumentBll.DoUpload(string.Empty, documentModel, filePath, false, INSPECTIONRESULT_DOCUPLOAD_CATEGORY_FOR_EMSE);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            if (emseResult != null)
            {
                errorMessage = "0".Equals(emseResult.returnCode) ? string.Empty : emseResult.returnMessage;
            }

            return errorMessage;
        }

        /// <summary>
        /// Validate the upload file's header, the required column need display in CSV.
        /// </summary>
        /// <param name="fileInfos">the file info</param>
        /// <returns>Return true if the upload file's header is validate.</returns>
        private bool ValidateUploadFile(FileUploadInfo[] fileInfos)
        {
            if (fileInfos == null || fileInfos.Length == 0)
            {
                return false;
            }

            string uploadDirectory = AttachmentUtil.GetTempDirectory();

            IList<string> requiredColumns = new List<string>();
            bool isDBRequiredColumnConfiged = GetRequiredFields4Upload(ref requiredColumns);

            if (!isDBRequiredColumnConfiged)
            {
                return false;
            }

            // loop the upload file to check the CSV header
            foreach (FileUploadInfo info in fileInfos)
            {
                if (info == null)
                {
                    continue;
                }

                string filePath = Path.Combine(uploadDirectory, info.FileName + info.FileId);
                string headerLine;

                if (!File.Exists(filePath))
                {
                    continue;
                }

                bool isContentEmpty = true;

                using (StreamReader sr = new StreamReader(filePath))
                {
                    headerLine = sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        string contentLine = sr.ReadLine();

                        if (contentLine != null && !string.IsNullOrEmpty(contentLine.TrimEnd(ACAConstant.CultureInfoSplitChar.ToCharArray())))
                        {
                            isContentEmpty = false;
                            break;
                        }
                    }

                    sr.Close();
                }

                if (string.IsNullOrEmpty(headerLine) || isContentEmpty)
                {
                    MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, GetTextByKey("aca_uploadinspresult_msg_uploadfile_contentempty"));
                    return false;
                }

                string[] arrHeaderLine = headerLine.Split(ACAConstant.CultureInfoSplitChar.ToCharArray());

                // check the header line to contain the required columns
                foreach (string requiredColumn in requiredColumns)
                {
                    bool isContainRequired = arrHeaderLine.Any(headerColumn => requiredColumn.Equals(headerColumn.Trim('"'), StringComparison.InvariantCultureIgnoreCase));

                    if (!isContainRequired)
                    {
                        MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, string.Format(GetTextByKey("aca_uploadinspresult_msg_uploadfile_requiredcolumnmiss"), requiredColumn));
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Get the required fields for upload.
        /// </summary>
        /// <param name="requiredColumns">the required columns</param>
        /// <returns>Return the success to get the required columns.</returns>
        private bool GetRequiredFields4Upload(ref IList<string> requiredColumns)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string dbRequiredMessage = GetTextByKey("aca_uploadinspresult_msg_uploadfile_dbrequiredcolumnmiss");

            BizDomainModel4WS[] bizList = bizBll.GetBizDomainValue(
               ConfigManager.AgencyCode, BizDomainConstant.STD_INSPECTION_RESULT_CSV_FORMAT, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);

            // if standard choice not config or disable, show message to config the item.
            if (bizList == null || bizList.Length == 0)
            {
                string message = string.Format(
                    dbRequiredMessage,
                    string.Format("{1}{0}{2}{0}{3}", ACAConstant.COMMA_BLANK, INSPECTION_SEQUENCE_NUMBER, INSPECTION_DATE, INSPECTION_STATUS));

                MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, message);
                return false;
            }

            // The DB required column which need when upload inspection result.
            Dictionary<string, bool> dbRequiredColumns = new Dictionary<string, bool>();
            dbRequiredColumns.Add(INSPECTION_SEQUENCE_NUMBER, false);
            dbRequiredColumns.Add(INSPECTION_DATE, false);
            dbRequiredColumns.Add(INSPECTION_STATUS, false);

            foreach (BizDomainModel4WS bizDomain in bizList)
            {
                string columnName = string.Empty;
                bool isRequired = false;
                string[] descItems = bizDomain.description.Split(';');

                // Get the required columns, contain the DB required and standard choice configed required.
                foreach (string descItem in descItems)
                {
                    string[] pieces = descItem.Split('=');

                    if (pieces.Length == 2)
                    {
                        if (INSPECTION_RESULT_FORMAT_COLUMN.Equals(pieces[0].Trim(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            columnName = pieces[1].Trim();
                        }

                        if (dbRequiredColumns.ContainsKey(bizDomain.bizdomainValue.ToUpper())
                            || (INSPECTION_RESULT_FORMAT_REQUIRED.Equals(pieces[0].Trim(), StringComparison.InvariantCultureIgnoreCase) && ValidationUtil.IsYes(pieces[1].Trim())))
                        {
                            isRequired = true;
                        }
                    }
                }

                if (isRequired)
                {
                    requiredColumns.Add(columnName);
                }

                if (dbRequiredColumns.ContainsKey(bizDomain.bizdomainValue.ToUpper()))
                {
                    dbRequiredColumns[bizDomain.bizdomainValue.ToUpper()] = true;
                }
            }

            string dbReqiredMissColumn = string.Empty;

            // If miss required columns, show message.
            foreach (var requiredColumn in dbRequiredColumns)
            {
                if (!requiredColumn.Value)
                {
                    dbReqiredMissColumn += requiredColumn.Key + ACAConstant.COMMA_BLANK;
                }
            }

            if (!string.IsNullOrEmpty(dbReqiredMissColumn))
            {
                MessageUtil.ShowMessage(msgUpdatePanel, MessageType.Error, string.Format(dbRequiredMessage, dbReqiredMissColumn.TrimEnd(ACAConstant.COMMA_BLANK.ToCharArray())));
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}