#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IEDMSDocumentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IEDMSDocumentBll.cs 278983 2014-09-23 10:06:41Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.UI.Model;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL.Attachment
{
    /// <summary>
    /// Defines methods for EDMS Document.
    /// </summary>
    public interface IEDMSDocumentBll
    {
        #region Methods

        /// <summary>
        /// construct a  document model for uploading.
        /// </summary>
        /// <param name="capModel">cap module</param>
        /// <param name="userId">user id number.</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="fileInfo">FileUploadInfo model</param>
        /// <returns>Document model.</returns>
        DocumentModel ConstructDocumentModel(CapModel4WS capModel, string userId, string agencyCode, FileUploadInfo fileInfo);

        /// <summary>
        /// construct a  document model for select from account.
        /// </summary>
        /// <param name="capModel">cap module</param>
        /// <param name="userId">user id number.</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="fileInfo">FileUploadInfo model</param>
        /// <returns>Document model.</returns>
        DocumentModel ConstructFromAccountDocumentModel(CapModel4WS capModel, string userId, string agencyCode, FileUploadInfo fileInfo);

        /// <summary>
        /// To delete document of selected by documentID or fileKey.
        /// </summary>
        /// <param name="serviceProviderCode">Agency Code</param>
        /// <param name="module">module name of page</param>
        /// <param name="documentModel">document Model</param>
        /// <param name="callerID">Caller ID number.</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <returns>return true as successful.false as failed.</returns>
        bool DoDelete(string serviceProviderCode, string module, DocumentModel documentModel, string callerID, bool isPartialCap);

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
        DocumentModel DoDownload(string serviceProviderCode, string module, string callerID, EntityModel enity, EdmsPolicyModel policy, long documentID, string fileKey, bool isPartialCap);

        /// <summary>
        /// Uploads attached document for CAP
        /// </summary>
        /// <param name="module">module name of page</param>
        /// <param name="documentModel4WS">model of attach document</param>
        /// <param name="filePath">file path URL.</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <param name="category4EMSE">Document category for distinguish what function to upload document, DocumentUploadAfter event will use this value to handle some logic.</param>
        /// <returns>return true as successful,false as failed</returns>
        EMSEResultBaseModel4WS DoUpload(string module, DocumentModel documentModel4WS, string filePath, bool isPartialCap, string category4EMSE = null);

        /// <summary>
        /// Synchronous upload file.
        /// </summary>
        /// <param name="module">The module</param>
        /// <param name="documentModel">The document model</param>
        /// <param name="filePath">The file path</param>
        /// <param name="isPartialCap">Is partial cap or not.</param>
        /// <returns>Return true as successful,false as failed</returns>
        EMSEResultBaseModel4WS DoSynchronousUpload(string module, DocumentModel documentModel, string filePath, bool isPartialCap);

        /// <summary>
        /// Gets document model by document key, not need right.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="documentSeqNo">Document sequence number.</param>
        /// <param name="isCapDocument">get cap document or people document</param>
        /// <returns>Document Model</returns>
        DocumentModel GetDocumentByPk(string agencyCode, long documentSeqNo, bool isCapDocument);

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
        DocumentModel GetDocumentByPk(string agencyCode, long documentSeqNo, CapIDModel capIdModel, bool needRight, bool isCapDocument, string publicUserId);

        /// <summary>
        /// Gets document type list by CAP ID.
        /// </summary>
        /// <param name="capIDModel4WS">Cap ID model.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>Reference document information list</returns>
        RefDocumentModel[] GetDocumentTypes(CapIDModel4WS capIDModel4WS, string callerID);

        /// <summary>
        /// Gets document type list by agency code.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capTypeModel">capType model. The model can be null</param>
        /// <returns>return specify cap type of reference document models. If cap type is null return all reference document models</returns>
        RefDocumentModel[] GetAllDocumentTypes(string agencyCode, CapTypeModel capTypeModel = null);

        /// <summary>
        /// Gets required document type list by required document information
        /// </summary>
        /// <param name="searchModel">Required Document information</param>
        /// <returns>RefRequiredDocumentModel[] of required document type array</returns>
        RefRequiredDocumentModel[] GetRequiredDocumentList(RefRequiredDocumentModel searchModel);

        /// <summary>
        /// Gets document list
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="module">module name of page</param>
        /// <param name="callerID">caller ID number.</param>
        /// <param name="capID">model of cap ID for WS</param>
        /// <param name="isPartialCap">Is partial CAP for current cap</param>
        /// <returns>Document model information list</returns>
        DocumentModel[] GetRecordDocumentList(string serviceProviderCode, string module, string callerID, CapIDModel capID, bool isPartialCap);

        /// <summary>
        /// Get Document List by GIS Object.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="gisObjects">GIS Object array</param>
        /// <param name="moduleList">Module list</param>
        /// <param name="callerId">call Id number</param>
        /// <returns>Document model information list </returns>
        DocumentModel[] GetDocumentListByGisObject(string agencyCode, GISObjectModel[] gisObjects, string[] moduleList, string callerId);

        /// <summary>
        /// Gets EDMS Security Policy Model
        /// </summary>
        /// <param name="serviceProviderCode">Agency Code</param>
        /// <param name="module">module name of page</param>
        /// <param name="callerID">Caller ID number.</param>
        /// <param name="capID">Cap ID number.</param>
        /// <returns>Model of EDMS Policy</returns>
        EdmsPolicyModel4WS GetSecurityPolicy(string serviceProviderCode, string module, string callerID, CapIDModel4WS capID);

        /// <summary>
        /// Gets document type list by public user associated people.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="entityType">LICENSEPROFESSIONAL or REFCONTACT</param>
        /// <param name="peopleType">associated people LP type Contact type</param>
        /// <returns>string[] of document type array</returns>
        RefDocumentModel[] GetDocTypeByPeopleType(string serviceProviderCode, string entityType, string peopleType);

        /// <summary>
        /// Get people document list.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="callerID">public user id</param>
        /// <param name="entityModelList">entity Model List</param>
        /// <returns>return document result model</returns>
        DocumentResultModel GetEntityDocumentList(string serviceProviderCode, string callerID, EntityModel[] entityModelList);

        /// <summary>
        /// Get people document list by Cap ID model.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="capIDModel">cap ID model</param>
        /// <param name="moduleName"> module name</param>
        /// <param name="callerID">public user id</param>
        /// <returns>return document result model</returns>
        DocumentResultModel GetPeopleDocumentByCapID(string serviceProviderCode, CapIDModel capIDModel, string moduleName, string callerID);

        /// <summary>
        /// Get the document list
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="documentEntityList">The document entity model list.</param>
        /// <param name="moduleNames">The module name list, it uses to find the EDMS.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Return the document list.</returns>
        DocumentModel[] GetDocumentList(string agencyCode, DocumentEntityAssociationModel[] documentEntityList, string[] moduleNames, string callerId);

        /// <summary>
        /// Get the Associated Document Agency
        /// </summary>
        /// <param name="sourceServProvCode">the source Server Provide Code </param>
        /// <param name="sourceDocSeqNbr">the document source Sequence NBR</param>
        /// <returns>Associated Document Agency code list</returns>
        string[] GetAssociatedDocumentAgency(string sourceServProvCode, long sourceDocSeqNbr);

        /// <summary>
        /// Start async upload timer.
        /// </summary>
        void StartAsyncUploadTimer();

        /// <summary>
        /// Stop async upload timer.
        /// </summary>
        void StopAsyncUploadTimer();

        /// <summary>
        /// Update document component names. Update condition is Agency and document Sequence Number
        /// </summary>
        /// <param name="documentModels">Document models.</param>
        /// <param name="publicUserId">Current public user id.</param>
        void UpdateDocumentComponentNames(DocumentModel[] documentModels, string publicUserId);

        /// <summary>
        /// Update Document SourceInfo
        /// </summary>
        /// <param name="documentModels">Document models.</param>
        void UpdateDocumentSourceInfo(DocumentModel[] documentModels);

        #endregion Methods
    }
}