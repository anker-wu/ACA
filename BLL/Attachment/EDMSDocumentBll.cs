#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EDMSDocumentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  This class invokes web service EDMSDocumentWebService.java all method.
 *
 *  Notes:
 * $Id: EDMSDocumentBll.cs 278983 2014-09-23 10:06:41Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.IO;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Attachment
{
    /// <summary>
    /// This class provide the ability to operation EDMS document.
    /// </summary>
    public class EDMSDocumentBll : BaseBll, IEDMSDocumentBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EDMSDocumentBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of EDMSDocumentService.EDMSDocumentUploadWebServiceService
        /// </summary>
        private EDMSDocumentWebServiceService EDMSDocumentService
        {
            get
            {
                return WSFactory.Instance.GetWebService<EDMSDocumentWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of EDMSDocumentUploadService.
        /// </summary>
        private EDMSDocumentUploadWebServiceService EDMSDocumentUploadService
        {
            get
            {
                return WSFactory.Instance.GetWebService3<EDMSDocumentUploadWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// construct a  document model for uploading.
        /// </summary>
        /// <param name="capModel">cap module</param>
        /// <param name="userId">user id number.</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="fileInfo">FileUploadInfo model</param>
        /// <returns>Document model.</returns>
        public DocumentModel ConstructDocumentModel(CapModel4WS capModel, string userId, string agencyCode, FileUploadInfo fileInfo)
        {
            ITimeZoneBll timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
            DateTime timeZoneDate = timeBll.GetAgencyCurrentDate(agencyCode);
            CapIDModel4WS capID = capModel != null ? capModel.capID : null;
            string subAgencyCode = capID != null ? capID.serviceProviderCode : agencyCode;
            string docName = null;

            if (fileInfo != null && fileInfo.DocumentModel != null)
            {
                docName = fileInfo.DocumentModel.docName;
            }
            else if (fileInfo != null)
            {
                docName = fileInfo.FileName;
            }

            DocumentContentModel documentContentModel = new DocumentContentModel();
            documentContentModel.recDate = timeZoneDate;
            documentContentModel.recFulName = userId;
            documentContentModel.recStatus = ACAConstant.VALID_STATUS;
            documentContentModel.servProvCode = subAgencyCode;

            //construct a document model for uploading
            DocumentModel documentModel = new DocumentModel();
            documentModel.capID = capID != null ? TempModelConvert.Trim4WSOfCapIDModel(capID) : null;
            documentModel.docName = docName;
            documentModel.recDate = timeZoneDate;
            documentModel.recFulNam = userId;
            documentModel.recStatus = ACAConstant.VALID_STATUS;
            documentModel.documentContent = documentContentModel;
            documentModel.docDate = timeZoneDate;
            documentModel.serviceProviderCode = subAgencyCode;
            documentModel.entityID = capID != null ? DataUtil.ConcatStringWithSplitChar(new string[] { capID.id1, capID.id2, capID.id3 }, ACAConstant.SPLIT_CHAR4) : string.Empty;
            documentModel.entityType = DocumentEntityType.Cap;

            if (fileInfo != null)
            {
                documentModel.FileId = fileInfo.FileId;
                documentModel.fileName = fileInfo.FileName;
                documentModel.fileSize = fileInfo.FileSize;
                documentModel.FileState = fileInfo.StateString;

                if (fileInfo.DocumentModel != null)
                {
                    if (fileInfo.DocumentModel.sourceDocNbr != null)
                    {
                        documentModel.FileId = string.Empty;
                    }

                    documentModel.sourceSpc = fileInfo.DocumentModel.sourceSpc;
                    documentModel.sourceRecfulnam = fileInfo.DocumentModel.sourceRecfulnam;
                    documentModel.sourceDocNbr = fileInfo.DocumentModel.sourceDocNbr;
                    documentModel.sourceEntityType = fileInfo.DocumentModel.sourceEntityType;
                    documentModel.sourceEntityID = fileInfo.DocumentModel.sourceEntityID;
                }
            }

            return documentModel;
        }

        /// <summary>
        /// construct a  document model for select from account.
        /// </summary>
        /// <param name="capModel">cap module</param>
        /// <param name="userId">user id number.</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="fileInfo">FileUploadInfo model</param>
        /// <returns>Document model.</returns>
        public DocumentModel ConstructFromAccountDocumentModel(CapModel4WS capModel, string userId, string agencyCode, FileUploadInfo fileInfo)
        {
            if (fileInfo == null || fileInfo.DocumentModel == null)
            {
                return null;
            }

            CapIDModel4WS capID = capModel != null ? capModel.capID : null;
            string subAgencyCode = capID != null ? capID.serviceProviderCode : agencyCode;
            
            //construct a document model for select from account.
            DocumentModel documentModel = new DocumentModel();
            documentModel = fileInfo.DocumentModel;
            documentModel.capID = capID != null ? TempModelConvert.Trim4WSOfCapIDModel(capID) : null;
            documentModel.recFulNam = userId;
            documentModel.recStatus = ACAConstant.VALID_STATUS;
            documentModel.serviceProviderCode = subAgencyCode;
            documentModel.entityID = capID != null ? DataUtil.ConcatStringWithSplitChar(new string[] { capID.id1, capID.id2, capID.id3 }, ACAConstant.SPLIT_CHAR4) : string.Empty;
            documentModel.entityType = DocumentEntityType.Cap;
            documentModel.FileId = string.Empty;

            if (fileInfo.DocumentModel != null)
            {
                documentModel.recDate = fileInfo.DocumentModel.recDate;
                documentModel.docDate = fileInfo.DocumentModel.docDate;
                documentModel.sourceSpc = fileInfo.DocumentModel.sourceSpc;
                documentModel.sourceRecfulnam = fileInfo.DocumentModel.sourceRecfulnam;
                documentModel.sourceDocNbr = fileInfo.DocumentModel.sourceDocNbr;
                documentModel.sourceEntityType = fileInfo.DocumentModel.sourceEntityType;
                documentModel.sourceEntityID = fileInfo.DocumentModel.sourceEntityID;
            }

            return documentModel;
        }

        /// <summary>
        /// To delete document of selected by documentID or fileKey.
        /// </summary>
        /// <param name="serviceProviderCode">Agency Code</param>
        /// <param name="module">module name of page</param>
        /// <param name="documentModel">document Model</param>
        /// <param name="callerID">Caller ID number.</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <returns>return true as successful.false as failed.</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID, documentModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool DoDelete(string serviceProviderCode, string module, DocumentModel documentModel, string callerID, bool isPartialCap)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID) || documentModel == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID", "documentModel" });
            }

            try
            {
                bool response = false;
                CapIDModel4WS capID = TempModelConvert.Add4WSForCapIDModel(documentModel.capID);

                if (isPartialCap)
                {
                    response = EDMSDocumentService.doDelete4PartialCap(serviceProviderCode, module, callerID, capID, long.Parse(documentModel.documentNo.ToString()), documentModel.fileKey);
                }
                else
                {
                    response = EDMSDocumentService.doDelete(serviceProviderCode, module, documentModel, callerID);
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// To download document of selected by documentID or fileKey.
        /// </summary>
        /// <param name="serviceProviderCode">Agency Code</param>
        /// <param name="module">module name of page</param>
        /// <param name="callerID">Caller ID number.</param>
        /// <param name="enity">Entity Model</param>
        /// <param name="policy">EDMS Policy Model</param>
        /// <param name="documentID">Document Mark1</param>
        /// <param name="fileKey">Document Mark2</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <returns>return document model for web service.</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID, enityModel, documentID , fileKey</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentModel DoDownload(string serviceProviderCode, string module, string callerID, EntityModel enity, EdmsPolicyModel policy, long documentID, string fileKey, bool isPartialCap)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID) || enity == null || (documentID <= 0 && string.IsNullOrEmpty(fileKey)))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID", "enityModel", "documentID & fileKey" });
            }

            try
            {
                DocumentModel response = null;

                if (isPartialCap)
                {
                    response = EDMSDocumentUploadService.doDownload4PartialCap(serviceProviderCode, module, callerID, enity, documentID, fileKey);
                }
                else
                {
                    response = EDMSDocumentUploadService.doDownload(serviceProviderCode, module, callerID, enity, policy, documentID, fileKey);
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Uploads attached document for CAP
        /// </summary>
        /// <param name="module">module name of page</param>
        /// <param name="documentModel4WS">model of attach document</param>
        /// <param name="filePath">file path URL.</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <param name="category4EMSE">Document category for distinguish what function to upload document, DocumentUploadAfter event will use this value to handle some logic.</param>
        /// <returns>return true as successful,false as failed</returns>
        /// <exception cref="DataValidateException">{ <c>documentModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public EMSEResultBaseModel4WS DoUpload(string module, DocumentModel documentModel4WS, string filePath, bool isPartialCap, string category4EMSE = null)
        {
            if (documentModel4WS == null)
            {
                throw new DataValidateException(new string[] { "documentModel4WS" });
            }

            try
            {
                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                string stdSycFlag = bizBll.GetValueForACAConfig(ACAConstant.AgencyCode, ACAConstant.ENABLE_SYNCHRONOUS_UPLOAD);

                bool isSyc = ACAConstant.COMMON_YES.Equals(stdSycFlag.ToUpper()) || ACAConstant.COMMON_Y.Equals(stdSycFlag.ToUpper());
                EMSEResultBaseModel4WS eMSEResultBaseModel4WS = new EMSEResultBaseModel4WS();

                if (!isSyc)
                {
                    AsynchronousUploadFile(module, documentModel4WS, filePath, isPartialCap, category4EMSE);
                }
                else
                {
                    eMSEResultBaseModel4WS = SynchronousUploadFile(module, documentModel4WS, filePath, isPartialCap, category4EMSE);
                }

                return eMSEResultBaseModel4WS;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Synchronous upload file.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="documentModel">The document model</param>
        /// <param name="filePath">The file path</param>
        /// <param name="isPartialCap">Is partial cap or not.</param>
        /// <returns>Return true as successful,false as failed</returns>
        /// <exception cref="DataValidateException">{ <c>documentModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public EMSEResultBaseModel4WS DoSynchronousUpload(string module, DocumentModel documentModel, string filePath, bool isPartialCap)
        {
            if (documentModel == null)
            {
                throw new DataValidateException(new[] { "documentModel" });
            }

            try
            {
                return SynchronousUploadFile(module, documentModel, filePath, isPartialCap, null);
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets document model by document key, not need right.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="documentSeqNo">Document sequence number.</param>
        /// <param name="isCapDocument">get cap document or people document</param>
        /// <returns>Document Model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentModel GetDocumentByPk(string agencyCode, long documentSeqNo, bool isCapDocument)
        {
            try
            {
                return EDMSDocumentService.getDocumentByPk(agencyCode, documentSeqNo, null, false, isCapDocument, string.Empty);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the document by PK.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="documentSeqNo">The document sequence no.</param>
        /// <param name="capIdModel">The cap id model.</param>
        /// <param name="needRight">if set to <c>true</c> [need right].</param>
        /// <param name="isCapDocument">get cap document or people document</param>
        /// <param name="publicUserId">The public user id.</param>
        /// <returns>Document Model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentModel GetDocumentByPk(string agencyCode, long documentSeqNo, CapIDModel capIdModel, bool needRight, bool isCapDocument, string publicUserId)
        {
            try
            {
                return EDMSDocumentService.getDocumentByPk(agencyCode, documentSeqNo, capIdModel, needRight, isCapDocument, publicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets document type list by CAP ID.
        /// </summary>
        /// <param name="capIDModel4WS">Cap ID model.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Reference document information list</returns>
        /// <exception cref="DataValidateException">{ <c>capIDModel4WS, capIDModel4WS.serviceProviderCode, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefDocumentModel[] GetDocumentTypes(CapIDModel4WS capIDModel4WS, string callerID)
        {
            if (capIDModel4WS == null || string.IsNullOrEmpty(capIDModel4WS.serviceProviderCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "capIDModel4WS", "capIDModel4WS.serviceProviderCode", "callerID" });
            }

            try
            {
                RefDocumentModel[] response = EDMSDocumentService.getDocCategoryList(capIDModel4WS, callerID);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets document type list by agency code.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capTypeModel">capType model. The model can be null</param>
        /// <returns>return specify cap type of reference document models. If cap type is null return all reference document models</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefDocumentModel[] GetAllDocumentTypes(string agencyCode, CapTypeModel capTypeModel)
        {
            try
            {
                RefDocumentModel[] response = EDMSDocumentService.getAllDocCategoryList(agencyCode, capTypeModel);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets document list
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="module">module name of page</param>
        /// <param name="callerID">caller ID number.</param>
        /// <param name="capID">model of cap ID for WS</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <returns>Document model information list</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, module, callerID, capID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentModel[] GetRecordDocumentList(string serviceProviderCode, string module, string callerID, CapIDModel capID, bool isPartialCap)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(module) || string.IsNullOrEmpty(callerID) || capID == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "module", "callerID", "capID" });
            }

            try
            {
                DocumentModel[] response = null;
                if (isPartialCap)
                {
                    response = EDMSDocumentService.getDocumentList4PartialCap(serviceProviderCode, module, callerID, capID);
                }
                else
                {
                    response = EDMSDocumentService.getRecordDocumentList(serviceProviderCode, module, callerID, capID);
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets required document type list by required document information
        /// </summary>
        /// <param name="searchModel">Required Document information</param>
        /// <returns>RefRequiredDocumentModel[] of required document type array</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefRequiredDocumentModel[] GetRequiredDocumentList(RefRequiredDocumentModel searchModel)
        {
            if (searchModel == null)
            {
                return null;
            }

            try
            {
                if (IsAdmin)
                {
                    return EDMSDocumentService.getRequiredDocumentList(searchModel);
                }

                string cacheKey = string.Format(
                    "{1}{0}{2}{0}{3}{0}{4}",
                    ACAConstant.SPLIT_CHAR,
                    searchModel.group,
                    searchModel.type,
                    searchModel.subType,
                    searchModel.category);

                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                Hashtable reqDocumentSetting = cacheManager.GetRequiredDocumentTypes(searchModel, cacheKey);
                return reqDocumentSetting[cacheKey] as RefRequiredDocumentModel[];
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Document List by GIS Object.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="gisObjects">GIS Object array</param>
        /// <param name="moduleList">Module list</param>
        /// <param name="callerId">call Id number</param>
        /// <returns>Document model information list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentModel[] GetDocumentListByGisObject(string agencyCode, GISObjectModel[] gisObjects, string[] moduleList, string callerId)
        {
            try
            {
                DocumentModel[] result = EDMSDocumentService.getDocumentListByGISObject(agencyCode, gisObjects, moduleList, callerId);
                return result;
            }
            catch (Exception ex)
            {
                throw new ACAException(ex);
            }
        }

        /// <summary>
        /// Gets EDMS Security Policy Model
        /// </summary>
        /// <param name="serviceProviderCode">Agency Code</param>
        /// <param name="module">module name of page</param>
        /// <param name="callerID">Caller ID number.</param>
        /// <param name="capID">Cap ID number.</param>
        /// <returns>Model of EDMS Policy</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public EdmsPolicyModel4WS GetSecurityPolicy(string serviceProviderCode, string module, string callerID, CapIDModel4WS capID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID" });
            }

            try
            {
                EdmsPolicyModel4WS response = EDMSDocumentService.getSecurityPolicy(serviceProviderCode, module, callerID, capID);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets document type list by public user associated people.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="entityType">LICENSEPROFESSIONAL or REFCONTACT</param>
        /// <param name="peopleType">associated people LP type Contact type</param>
        /// <returns>string[] of document type array</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefDocumentModel[] GetDocTypeByPeopleType(string serviceProviderCode, string entityType, string peopleType)
        {
            try
            {
                RefDocumentModel[] response = EDMSDocumentService.getDocTypeByPeopleType(serviceProviderCode, entityType, peopleType);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get people document list.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="callerID">public user id</param>
        /// <param name="entityModelList">entity Model List</param>
        /// <returns>DocumentModel list</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID, entityModelList</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentResultModel GetEntityDocumentList(string serviceProviderCode, string callerID, EntityModel[] entityModelList)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID) || entityModelList == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID", "entityModelList" });
            }

            try
            {
                return EDMSDocumentService.getPeopleDocument(serviceProviderCode, entityModelList, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get people document list by Cap ID model.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capIDModel">cap ID model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="callerID">public user id</param>
        /// <returns>Document model information list</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, callerID, capIDModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DocumentResultModel GetPeopleDocumentByCapID(string serviceProviderCode, CapIDModel capIDModel, string moduleName, string callerID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(callerID) || capIDModel == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "callerID", "capIDModel" });
            }

            try
            {
                return EDMSDocumentService.getPeopleDocumentByCapID(serviceProviderCode, capIDModel, moduleName, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the Associated Document Agency
        /// </summary>
        /// <param name="sourceServProvCode">the source Server Provide Code</param>
        /// <param name="sourceDocSeqNbr">the document source Sequence NBR</param>
        /// <returns>Associated Document Agency code list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetAssociatedDocumentAgency(string sourceServProvCode, long sourceDocSeqNbr)
        {
            try
            {
                return EDMSDocumentService.getAssociatedDocumentAgency(sourceServProvCode, sourceDocSeqNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the document list
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="documentEntityList">The document entity model list.</param>
        /// <param name="moduleNames">The module name list, it uses to find the EDMS.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Return the document list.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        DocumentModel[] IEDMSDocumentBll.GetDocumentList(string agencyCode, DocumentEntityAssociationModel[] documentEntityList, string[] moduleNames, string callerId)
        {
            try
            {
                return EDMSDocumentService.getDocumentList(agencyCode, documentEntityList, moduleNames, callerId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Start async upload timer.
        /// </summary>
        public void StartAsyncUploadTimer()
        {
            AsynchronousUpload.Instance.Start();
        }

        /// <summary>
        /// Stop async upload timer.
        /// </summary>
        public void StopAsyncUploadTimer()
        {
            AsynchronousUpload.Instance.Stop();
        }

        /// <summary>
        /// Update document component names. Update condition is Agency and document Sequence Number.
        /// </summary>
        /// <param name="documentModels">Document models.</param>
        /// <param name="publicUserId">Current public user id.</param>
        public void UpdateDocumentComponentNames(DocumentModel[] documentModels, string publicUserId)
        {
            if (documentModels == null || string.IsNullOrEmpty(publicUserId))
            {
                throw new DataValidateException(new string[] { "documentModels", "publicUserId" });
            }

            try
            {
                EDMSDocumentService.updateDocumentComponentNames(documentModels, publicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update Document SourceInfo.
        /// </summary>
        /// <param name="documentModels">Document models.</param>
        public void UpdateDocumentSourceInfo(DocumentModel[] documentModels)
        {
            try
            {
                EDMSDocumentService.updateDocumentSourceInfo(documentModels);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Asynchronous upload file
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="documentModel4WS">documentModel4WS object.</param>
        /// <param name="filePath">file path to be uploaded.</param>
        /// <param name="isPartialCap">Is partial cap.</param>
        /// <param name="category4EMSE">Document category for distinguish what function to upload document, DocumentUploadAfter event will use this value to handle some logic.</param>
        private void AsynchronousUploadFile(string module, DocumentModel documentModel4WS, string filePath, bool isPartialCap, string category4EMSE)
        {
            // general upload
            if (File.Exists(filePath))
            {
                AttachmentModel um = new AttachmentModel(module, documentModel4WS);
                um.IsPartialCap = isPartialCap;
                um.Category4EMSE = category4EMSE;

                string xml = SerializationUtil.XmlSerialize(um);
                string infoFileName = AsynchronousUpload.GetTempInfoFileName(filePath);

                using (StreamWriter sw = new StreamWriter(infoFileName))
                {
                    sw.Write(xml);
                    sw.Flush();
                    sw.Close();
                }

                if (Logger.IsDebugEnabled)
                {
                    string serviceProviderCode = (um.DocumentModel != null && !string.IsNullOrEmpty(um.DocumentModel.serviceProviderCode)) ? um.DocumentModel.serviceProviderCode : ConfigManager.AgencyCode;
                    string entityID = (um.DocumentModel != null && um.DocumentModel.entityID != null) ? um.DocumentModel.entityID : string.Empty;
                    string entityType = (um.DocumentModel != null && um.DocumentModel.entityType != null) ? um.DocumentModel.entityType : string.Empty;

                    Logger.DebugFormat(
                        "Upload document asynchronously. \nEntityID: {0}\nEntityType: {1}\nInfo File Name:{2}\nDoc Name:{3}\nFile Name:{4}",
                        serviceProviderCode + "-" + entityID,
                        entityType,
                        infoFileName,
                        um.DocumentModel.docName,
                        um.DocumentModel.fileName);
                }

                AsynchronousUpload.Instance.Start();
            }
            else if (IsSelectFromAccount(documentModel4WS))
            {
                if (isPartialCap)
                {
                    EDMSDocumentUploadService.doUpload4PartialCap(ConfigManager.AgencyCode, module, documentModel4WS.recFulNam, TempModelConvert.Add4WSForCapIDModel(documentModel4WS.capID), documentModel4WS);
                }
                else
                {
                    EDMSDocumentUploadService.doUpload(ConfigManager.AgencyCode, module, documentModel4WS.recFulNam, documentModel4WS, category4EMSE);
                }
            }
        }

        /// <summary>
        /// Upload file is by way of select from account.
        /// </summary>
        /// <param name="model">document model</param>
        /// <returns>Return true or false</returns>
        private bool IsSelectFromAccount(DocumentModel model)
        {
            return model != null
                   && model.sourceDocNbr != null
                   && model.sourceDocNbr > 0;
        }

        /// <summary>
        /// Synchronous upload file
        /// </summary>
        /// <param name="module">module name.</param>
        /// <param name="documentModel4WS">document model.</param>
        /// <param name="filePath">the file path.</param>
        /// <param name="isPartialCap">Is partial cap.</param>
        /// <param name="category4EMSE">Document category for distinguish what function to upload document, DocumentUploadAfter event will use this value to handle some logic.</param>
        /// <returns>EMSEResultBase Model</returns>
        private EMSEResultBaseModel4WS SynchronousUploadFile(string module, DocumentModel documentModel4WS, string filePath, bool isPartialCap, string category4EMSE)
        {
            // general upload
            if (!IsSelectFromAccount(documentModel4WS))
            {
                documentModel4WS.documentContent.docContentStream = File.ReadAllBytes(filePath);
            }

            EMSEResultBaseModel4WS eMSEResultBaseModel;

            CapIDModel4WS capID = TempModelConvert.Add4WSForCapIDModel(documentModel4WS.capID);

            if (isPartialCap)
            {
                eMSEResultBaseModel = EDMSDocumentUploadService.doUpload4PartialCap(AgencyCode, module, documentModel4WS.recFulNam, capID, documentModel4WS);
            }
            else
            {
                eMSEResultBaseModel = EDMSDocumentUploadService.doUpload(AgencyCode, module, documentModel4WS.recFulNam, documentModel4WS, category4EMSE);
            }

            File.Delete(filePath);

            return eMSEResultBaseModel;
        }

        #endregion Methods
    }
}