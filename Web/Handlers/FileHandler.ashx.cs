#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: FileHandler.ashx.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Http handler for files.
*
*  Notes:
* $Id: FileUploadHandler.ashx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// Http handler for files uploading.
    /// </summary>
    public class FileHandler : IHttpHandler, IHttpHandlerCommon, IRequiresSessionState
    {
        #region Fields

        /// <summary>
        /// Log instance.
        /// </summary>
        protected static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(FileHandler));

        /// <summary>
        /// Download action.
        /// </summary>
        private const string DOWNLOAD = "Download";

        /// <summary>
        /// Download for new inspection action.
        /// </summary>
        private const string DOWNLOAD_NewInspection = "DownloadNewInspection";

        /// <summary>
        /// Delete action.
        /// </summary>
        private const string DELETE = "Delete";

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the handler can be reused.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the config section.
        /// </summary>
        /// <value>The config section.</value>
        public HttpHandlerConfigObject ConfigObject 
        { 
            get; 
            
            set;
        }

        /// <summary>
        /// Gets the agency code.
        /// </summary>
        /// <value>The agency code.</value>
        private string AgencyCode
        {
            get
            {
                return HttpContext.Current.Request.QueryString["agency"];
            }
        }

        /// <summary>
        /// Gets the cap id1.
        /// </summary>
        /// <value>The cap id1.</value>
        private string CapId1
        {
            get
            {
                return HttpContext.Current.Request.QueryString["ID1"];
            }
        }

        /// <summary>
        /// Gets the cap id2.
        /// </summary>
        /// <value>The cap id2.</value>
        private string CapId2
        {
            get
            {
                return HttpContext.Current.Request.QueryString["ID2"];
            }
        }

        /// <summary>
        /// Gets the cap id3.
        /// </summary>
        /// <value>The cap id3.</value>
        private string CapId3
        {
            get
            {
                return HttpContext.Current.Request.QueryString["ID3"];
            }
        }

        /// <summary>
        /// Gets the document sequence.
        /// </summary>
        /// <value>The document sequence.</value>
        private string DocumentSeqNo
        {
            get
            {
                return HttpContext.Current.Request.QueryString["SeqNo"];
            }
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        private string Action
        {
            get
            {
                string action = string.Empty;

                if (ConfigObject != null)
                {
                    action = ConfigObject.Params;
                }
                else
                {
                    action = HttpContext.Current.Request.QueryString["action"];
                }

                return action;
            }
        }

        #endregion

        /// <summary>
        /// Tries the validation.
        /// </summary>
        /// <param name="type">The handler type.</param>
        /// <returns>validation parameters successful</returns>
        public bool ValidateParams(ServiceType type)
        {
            bool isValidation = !string.IsNullOrEmpty(Action);

            if (isValidation && DOWNLOAD.Equals(Action, StringComparison.InvariantCultureIgnoreCase))
            {
                isValidation = !string.IsNullOrEmpty(AgencyCode) 
                                        && !string.IsNullOrEmpty(CapId1) 
                                        && !string.IsNullOrEmpty(CapId2)
                                        && !string.IsNullOrEmpty(CapId3) 
                                        && !string.IsNullOrEmpty(DocumentSeqNo);
            }

            return isValidation;
        }

        /// <summary>
        /// To process http request.
        /// </summary>
        /// <param name="context">Http context.</param>
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(Action) && DELETE.Equals(Action, StringComparison.InvariantCultureIgnoreCase))
            {
                //delete action triggled by cancel or close upload page.
                DeleteFile(context);
            }
            else if (!string.IsNullOrEmpty(Action) && DOWNLOAD.Equals(Action, StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.IsNullOrEmpty(context.Request["entityId"]))
                {
                    DownloadFile(context);
                }
                else
                {
                    DownloadFileByEntity(context);
                }
            }
            else if (DOWNLOAD_NewInspection.Equals(Action, StringComparison.InvariantCultureIgnoreCase))
            {
                DownloadFile4NewInspection(context);
            }
            else
            {
                ProcessFileUpload(context);
            }
        }

        /// <summary>
        /// Delete file.
        /// </summary>
        /// <param name="context">Http context.</param>
        private void DeleteFile(HttpContext context)
        {
            try
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                FileUploadInfo[] fileList = jsSerializer.Deserialize<FileUploadInfo[]>(context.Request.Params["FileList"]);
                AttachmentUtil.DeleteFileFromTempFolder(fileList);
            }
            catch (Exception e)
            {
                Logger.Error("Delete file operation failed.", e);
            }
        }

        /// <summary>
        /// To process file upload.
        /// </summary>
        /// <param name="context">Http context.</param>
        private void ProcessFileUpload(HttpContext context)
        {
            string fileName = context.Request.QueryString["fileName"];
            string fileID = context.Request.QueryString["fileId"];
            string firstChunk = context.Request.QueryString["isFirstChunk"];
            string lastChunk = context.Request.QueryString["isLastChunk"];
            bool isFirstChunk = true;
            bool isLastChunk = true;

            if (context.Request.InputStream.Length == 0 || string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileID))
            {
                throw new ArgumentException("No file input.");
            }

            if (!string.IsNullOrEmpty(firstChunk))
            {
                bool.TryParse(firstChunk, out isFirstChunk);
            }

            if (!string.IsNullOrEmpty(lastChunk))
            {
                bool.TryParse(lastChunk, out isLastChunk);
            }

            string uploadFolder = AttachmentUtil.GetTempDirectory();
            string diskFileName = fileName + fileID;
            string diskFilePath = Path.Combine(uploadFolder, diskFileName);

            if (isFirstChunk && File.Exists(diskFilePath))
            {
                //Delete existing file if is first chunk.
                File.Delete(diskFilePath);
            }

            SaveFile(context.Request.InputStream, diskFilePath);

            if (isLastChunk)
            {
                //TODO:File upload finished.
            }
        }

        /// <summary>
        /// Write stream to file.
        /// </summary>
        /// <param name="stream">Data stream.</param>
        /// <param name="filePath">Full file path.</param>
        private void SaveFile(Stream stream, string filePath)
        {
            byte[] buffer = new byte[65536];

            using (FileStream fs = File.Open(filePath, FileMode.Append))
            {
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    fs.Write(buffer, 0, bytesRead);
                }

                stream.Close();
                fs.Close();
                fs.Dispose();
                GC.Collect();
            }
        }

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="context">The context.</param>
        private void DownloadFile(HttpContext context)
        {
            if (AuthenticationUtil.IsAuthenticated && AppSession.User != null && !AppSession.User.IsAnonymous)
            {
                var capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                var edmsDocumentBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
                
                var capIdmodel4WS = new CapIDModel4WS() { id1 = CapId1, id2 = CapId2, id3 = CapId3, serviceProviderCode = AgencyCode };
                var capModel4WS = capBll.GetCapByPK(capIdmodel4WS);
                var capIdModel = TempModelConvert.Trim4WSOfCapIDModel(capIdmodel4WS);
                var documentModel = edmsDocumentBll.GetDocumentByPk(AgencyCode, long.Parse(DocumentSeqNo), capIdModel, true, true, AppSession.User.PublicUserId);
                var edmsPolicyModel = edmsDocumentBll.GetSecurityPolicy(AgencyCode, capModel4WS.moduleName, AppSession.User.PublicUserId, capIdmodel4WS);

                if (capModel4WS != null && documentModel != null)
                {
                    bool hasEdmsDownloadRight = edmsPolicyModel == null || ACAConstant.COMMON_TRUE.Equals(edmsPolicyModel.downloadRight, StringComparison.InvariantCultureIgnoreCase);
                    var hasDownloadRight = documentModel.viewable && hasEdmsDownloadRight;

                    if (hasDownloadRight)
                    {
                        string downloadState = string.IsNullOrEmpty(context.Request.QueryString["DownloadState"]) ? DOWNLOAD : context.Request.QueryString["DownloadState"];

                        if (DOWNLOAD.Equals(downloadState, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ResponseContent4CapDocument(capModel4WS, documentModel, context);
                        }
                        else
                        {
                            string downloadUrl = context.Request.Url.AbsoluteUri.Replace("WaitDownload", DOWNLOAD);
                            context.Response.Write("<script>");
                            context.Response.Write(" window.location.href='" + downloadUrl + "'");
                            context.Response.Write("</script>");
                        }
                    }
                    else
                    {
                        throw new Exception(LabelUtil.GetTextByKey("aca_label_message_download_nopermission", capModel4WS.moduleName));
                    }
                }
            }
            else
            {
                AuthenticationUtil.RedirectToLoginPage(context.Request.Url.AbsoluteUri + "&DownloadState=WaitDownload", null);
            }
        }

        /// <summary>
        /// Download file by entity.
        /// </summary>
        /// <param name="context">The HttpContext.</param>
        private void DownloadFileByEntity(HttpContext context)
        {
            string entityId = context.Request["entityId"];
            string entityType = context.Request["entityType"];
            string documentNo = context.Request["documentNo"];
            string fileKey = context.Request["fileKey"];
            string moduleName = context.Request[ACAConstant.MODULE_NAME];

            try
            {
                EntityModel entity = new EntityModel
                {
                    entityID = entityId,
                    entityType = entityType,
                    customID = entityId,
                    serviceProviderCode = AgencyCode
                };

                ResponseContent(context, entity, moduleName, long.Parse(documentNo), fileKey);
            }
            catch (ACAException)
            {
                throw new Exception(LabelUtil.GetTextByKey("aca_label_message_download_nopermission", string.Empty));
            }
        }

        /// <summary>
        /// Download file for new inspection.
        /// </summary>
        /// <param name="context">The context.</param>
        private void DownloadFile4NewInspection(HttpContext context)
        {
            IInspectionBll inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
            string fileName = context.Request["fileName"];

            byte[] buffer = inspectionBll.GetMyInspectionsCSV(AgencyCode);
            int fileSize = buffer.Length;

            if (buffer.Length > 0)
            {
                context.Response.Clear();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20"));

                //resolve the filename with empty space char
                context.Response.ContentType = "application/octet-stream";
                context.Response.OutputStream.Write(buffer, 0, fileSize);
                context.Response.Flush();
                context.Response.Close();
            }
        }

        /// <summary>
        /// Responses the content.
        /// </summary>
        /// <param name="capModel4WS">The cap model.</param>
        /// <param name="documentModel">The document model.</param>
        /// <param name="context">The context.</param>
        private void ResponseContent4CapDocument(CapModel4WS capModel4WS, DocumentModel documentModel, HttpContext context)
        {
            var entity = new EntityModel();
            entity.entityType = DocumentEntityType.Cap;
            entity.customID = capModel4WS.capID.customID;
            entity.serviceProviderCode = capModel4WS.capID.serviceProviderCode;
            entity.entityID = DataUtil.ConcatStringWithSplitChar(new string[] { capModel4WS.capID.id1, capModel4WS.capID.id2, capModel4WS.capID.id3 }, ACAConstant.SPLIT_CHAR4);

            ResponseContent(context, entity, capModel4WS.moduleName, long.Parse(DocumentSeqNo), documentModel.fileKey);
        }

        /// <summary>
        /// Response the content
        /// </summary>
        /// <param name="context">The HttpContext.</param>
        /// <param name="entity">The entity model.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="documentNo">The document number.</param>
        /// <param name="fileKey">The file key.</param>
        private void ResponseContent(HttpContext context, EntityModel entity, string moduleName, long documentNo, string fileKey)
        {
            IEDMSDocumentBll edmsDocumentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            DocumentModel doDownload = edmsDocumentBll.DoDownload(AgencyCode, moduleName, AppSession.User.PublicUserId, entity, null, documentNo, fileKey, false);

            if (doDownload != null)
            {
                DocumentContentModel docContent = doDownload.documentContent;
                byte[] buffer = docContent.docContentStream;
                string fileName = doDownload.fileName;
                int fileSize = buffer.Length;

                if (buffer.Length > 0)
                {
                    context.Response.Clear();
                    context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20"));

                    //resolve the filename with empty space char
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.OutputStream.Write(buffer, 0, fileSize);
                    context.Response.Flush();
                    context.Response.Close();
                }
            }
        }
    }
}