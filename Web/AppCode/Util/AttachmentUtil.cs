#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: AttachmentUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Utility class for attachment operations.
*
*  Notes:
* $Id: AttachmentUtil.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 29, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;
using System.Xml;

using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// Utility class for attachment operations.
    /// </summary>
    public static class AttachmentUtil
    {
        #region Fields

        /// <summary>
        /// File unit K.
        /// </summary>
        private const string FILE_UNIT_K = "K";

        /// <summary>
        /// file unit M.
        /// </summary>
        private const string FILE_UNIT_M = "M";

        /// <summary>
        /// file unit KB.
        /// </summary>
        private const string FILE_UNIT_KB = "KB";

        /// <summary>
        /// file unit MB.
        /// </summary>
        private const string FILE_UNIT_MB = "MB";

        /// <summary>
        /// default file size.
        /// </summary>
        private const string DEFAULT_FILE_SIZE = "16";

        /// <summary>
        /// file unit bytes
        /// </summary>
        private const string FILE_UNIT_BYTES = "BYTES";

        /// <summary>
        /// Key of EDMS file size configuration.
        /// </summary>
        private const string EDMS_DOCUMENT_SIZE_MAX = "EDMS_DOCUMENT_SIZE_MAX";

        /// <summary>
        /// Key of EDMS file size configuration for ACA.
        /// </summary>
        private const string ACA_EDMS_DOCUMENT_SIZE_MAX = "ACA_EDMS_DOCUMENT_SIZE_MAX";

        /// <summary>
        /// upload the EDMS type file success' flag
        /// </summary>
        private const string UPLOAD_EDMS_SINGLE_FILE_SUCCESS = "TRUE";

        /// <summary>
        /// upload the EDMS type file failed with message's flag
        /// </summary>
        private const string UPLOAD_EDMS_SINGLE_FILE_FAILED_WITH_MSG = "ERROR_MSG";

        /// <summary>
        /// upload the EDMS type file fail flag
        /// </summary>
        private const string UPLOAD_EDMS_SINGLE_FILE_FAILED = "FALSE";

        /// <summary>
        /// validate file type failed
        /// </summary>
        private const string VALIDATE_FILE_TYPE_FAILED = "VALIDATE_FILE_TYPE_FAILED";

        /// <summary>
        /// validate file size failed
        /// </summary>
        private const string VALIDATE_FILE_SIZE_FAILED = "VALIDATE_FILE_SIZE_FAILED";

        #endregion Fields

        /// <summary>
        /// delete file from temp folder
        /// </summary>
        /// <param name="fileInfos">file upload info</param>
        public static void DeleteFileFromTempFolder(IEnumerable<FileUploadInfo> fileInfos)
        {
            if (fileInfos == null)
            {
                return;
            }

            var tempFolder = GetTempDirectory();
            const int MaxTryTimes = 3;

            foreach (var fileInfo in fileInfos)
            {
                string filePath = Path.Combine(tempFolder, fileInfo.FileName + fileInfo.FileId);

                if (File.Exists(filePath))
                {
                    int tryCounter = 1;

                    do
                    {
                        File.Delete(filePath);

                        if (!File.Exists(filePath))
                        {
                            break;
                        }

                        tryCounter++;
                        Thread.Sleep(10000);
                    }
                    while (tryCounter < MaxTryTimes);
                }
            }
        }

        /// <summary>
        /// Format the entity type for document according to the original entity type.
        /// </summary>
        /// <param name="originalEntityType">Original entity type</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>String of readable entity type.</returns>
        public static string FormatEntityType(string originalEntityType, string moduleName)
        {
            string result = string.Empty;

            if (originalEntityType != null)
            {
                switch (originalEntityType)
                {
                    case DocumentEntityType.Renewal:
                        result = LabelUtil.GetTextByKey("per_permitdetail_recordtype_renewrecord", moduleName);
                        break;
                    case DocumentEntityType.Related:
                        result = LabelUtil.GetTextByKey("per_permitdetail_recordtype_relatedrecord", moduleName);
                        break;
                    case DocumentEntityType.LP:
                        result = LabelUtil.GetTextByKey("aca_attachment_label_lpentitytype", moduleName);
                        break;
                    case DocumentEntityType.RefContact:
                        result = LabelUtil.GetTextByKey("aca_attachment_label_contactentitytype", moduleName);
                        break;
                    default:
                        result = LabelUtil.GetTextByKey("per_permitdetail_recordtype_record", moduleName);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Format file size as a string with unit.
        /// </summary>
        /// <param name="docSize">bytes count.</param>
        /// <returns>String of file size with unit.</returns>
        public static string FormatFileSize(double? docSize)
        {
            double size = docSize == null ? 0 : docSize.Value;

            string unit = "bytes";
            var lessThanOneKB = false;

            if (size / 1024 < 1)
            {
                lessThanOneKB = true;
            }
            else if (size / (1024 * 1024) < 1)
            {
                size = size / 1024;
                unit = FILE_UNIT_KB;
            }
            else
            {
                size = size / (1024 * 1024);
                unit = FILE_UNIT_MB;
            }

            string strSize;

            if (lessThanOneKB)
            {
                strSize = I18nNumberUtil.FormatNumberForUI(size) + " " + unit;
            }
            else
            {
                strSize = size.ToString("F") + " " + unit;
            }

            return strSize;
        }

        /// <summary>
        /// Get file size string 
        /// </summary>
        /// <param name="docSize">size of document in in bytes unit</param>
        /// <returns>return file size string in different unit, such as "bytes", "KB", "MB".</returns>
        public static string FormatFileSize(string docSize)
        {
            double size = 0;

            if (docSize != null)
            {
                if (ValidationUtil.IsNumber(docSize))
                {
                    size = I18nNumberUtil.ParseNumberFromWebService(docSize);
                }
            }

            return FormatFileSize(size);
        }

        /// <summary>
        /// Get and create ACA temporary folder.
        /// </summary>
        /// <returns>Full path of the ACA temporary folder.</returns>
        public static string GetTempDirectory()
        {
            string tempDir = ConfigManager.TempDirectory;

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            return tempDir;
        }

        /// <summary>
        /// Get virtualFolderGroup items by associated in cap type.
        /// </summary>
        /// <param name="virtualFolderGroup">virtual Folder Group</param>
        /// <returns>virtualFolderGroup items</returns>
        public static ListItem[] GetVirtualFolders(string virtualFolderGroup)
        {
            var listItems = new List<ListItem>();
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            var stdItems = cacheManager.GetBizDomainKeyValuePairs(ConfigManager.AgencyCode, virtualFolderGroup, I18nCultureUtil.UserPreferredCulture);

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (DictionaryEntry item in stdItems)
                {
                    var listItem = new ListItem();
                    listItem.Text = Convert.ToString(item.Value);
                    listItem.Value = Convert.ToString(item.Key);
                    listItems.Add(listItem);
                }
            }

            return listItems.OrderBy(i => i.Value).ToArray();
        }

        /// <summary>
        /// Get displayed text by selected value.
        /// </summary>
        /// <param name="virtualFolderGroup">virtualFolderGroup in stand choice</param>
        /// <param name="virtualFolders">Virtual Folder values</param>
        /// <returns>displayed text</returns>
        public static string[] GetVirtualFolderTextByValue(string virtualFolderGroup, string[] virtualFolders)
        {
            var virtualFolderText = new List<string>();
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            var stdItems = cacheManager.GetBizDomainKeyValuePairs(ConfigManager.AgencyCode, virtualFolderGroup, I18nCultureUtil.UserPreferredCulture);

            foreach (string virtualFolder in virtualFolders)
            {
                if (stdItems.ContainsKey(virtualFolder))
                {
                    virtualFolderText.Add(Convert.ToString(stdItems[virtualFolder]));
                }
            }

            return virtualFolderText.ToArray();
        }

        /// <summary>
        /// Check file owner permission
        /// </summary>
        /// <param name="fileOwnerPermission">permission object(1111000000)</param>
        /// <param name="actionName">action name</param>
        /// <returns>indicates whether it has permission or not.</returns>
        public static bool CheckFileOwnerPermission(string fileOwnerPermission, FileOwnerPermission actionName)
        {
            FileOwnerPermission permission = (FileOwnerPermission)Convert.ToInt32(fileOwnerPermission, 2);
            bool isHasPermission = (permission & actionName) == actionName;

            return isHasPermission;
        }

        /// <summary>
        /// Indicate current user is file owner or not.
        /// current associate LP and LP has attachment, current user is file owner.
        /// </summary>
        /// <param name="document">document model</param>
        /// <returns>indicate whether it is file owner or not.</returns>
        public static bool IsFileOwner(DocumentModel document)
        {
            var isFileOwner = false;

            switch (document.entityType)
            {
                case DocumentEntityType.LP:
                    var contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(document.serviceProviderCode, AppSession.User.UserSeqNum);

                    if (contractorLicenses != null && contractorLicenses.Length > 0)
                    {
                        foreach (var contractor in contractorLicenses)
                        {
                            if (contractor != null
                                && contractor.license != null
                                && document.entityID.Equals(contractor.license.licSeqNbr, StringComparison.InvariantCultureIgnoreCase))
                            {
                                isFileOwner = true;
                                break;
                            }
                        }
                    }

                    break;

                case DocumentEntityType.RefContact:
                    PeopleModel4WS[] contractorContacts = AppSession.User.UserModel4WS.peopleModel;

                    if (contractorContacts != null && contractorContacts.Length > 0)
                    {
                        foreach (PeopleModel4WS contractor in contractorContacts)
                        {
                            if (contractor != null
                                && document.entityID.Equals(contractor.contactSeqNumber, StringComparison.InvariantCultureIgnoreCase))
                            {
                                isFileOwner = true;
                                break;
                            }
                        }
                    }

                    break;
            }

            return isFileOwner;
        }

        /// <summary>
        /// Indicate current user is file owner or not.
        /// current associate LP and LP has attachment, current user is file owner.
        /// </summary>
        /// <param name="document">document model</param>
        /// <param name="capModel">The cap model.</param>
        /// <returns>indicate whether it is file owner or not.</returns>
        public static bool IsFileOwnerInCapDetail(DocumentModel document, CapModel4WS capModel)
        {
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

            if (capBll.IsCreateByDelegateUser(capModel))
            {
                return false;
            }

            switch (document.entityType)
            {
                case DocumentEntityType.LP:
                    var contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(document.serviceProviderCode, AppSession.User.UserSeqNum);
                    return IsLpFileOwner(capModel.licenseProfessionalList, contractorLicenses, document);

                case DocumentEntityType.RefContact:
                    PeopleModel4WS[] contractorContacts = AppSession.User.UserModel4WS.peopleModel;
                    List<CapContactModel4WS> dailyContacts = new List<CapContactModel4WS>();
                    if (capModel.contactsGroup != null)
                    {
                        dailyContacts.AddRange(capModel.contactsGroup);
                    }

                    if (capModel.applicantModel != null)
                    {
                        dailyContacts.Add(capModel.applicantModel);
                    }

                    return IsContactFileOwner(dailyContacts, contractorContacts, document);
            }

            return false;
        }

        /// <summary>
        /// Construct EntityModel based on the current user associated Licensed Professional information.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>EntityModel array</returns>
        public static EntityModel[] ConstructEntityModel(string agencyCode)
        {
            var entityList = new List<EntityModel>();
            var contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(agencyCode, AppSession.User.UserSeqNum);
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            
            if (contractorLicenses != null && contractorLicenses.Length > 0)
            {
                foreach (var contractor in contractorLicenses)
                {
                    if (contractor != null
                        && contractor.license != null
                        && ContractorLicenseStatus.Approved.Equals(contractor.status, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var entity = new EntityModel();
                        entity.entityType = DocumentEntityType.LP;
                        entity.entityID = contractor.license.licSeqNbr;
                        entity.customID = contractor.license.licSeqNbr;
                        entity.serviceProviderCode = contractor.license.serviceProviderCode;
                        entity.entity = I18nStringUtil.GetString(contractor.license.resLicenseType, contractor.license.licenseType) + ACAConstant.SPLITLINE + contractor.license.stateLicense;
                        entityList.Add(entity);
                    }
                }
            }

            if (AppSession.User.UserModel4WS.peopleModel != null && AppSession.User.UserModel4WS.peopleModel.Length > 0)
            {
                foreach (var people in AppSession.User.UserModel4WS.peopleModel)
                {
                    if (ContractorPeopleStatus.Approved.Equals(people.contractorPeopleStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var entity = new EntityModel();
                        entity.entityType = DocumentEntityType.RefContact;
                        entity.entityID = people.contactSeqNumber;
                        entity.customID = people.contactSeqNumber;
                        entity.serviceProviderCode = people.serviceProviderCode;
                        entity.entity = I18nStringUtil.GetString(people.contactType, people.contactType) + ACAConstant.SPLITLINE + peopleBll.GetContactUserName(people, true);
                        entityList.Add(entity);
                    }
                }
            }

            return entityList.ToArray();
        }

        /// <summary>
        /// Get specific entity information, like as entity-entityId.
        /// </summary>
        /// <param name="document">document model</param>
        /// <returns>string value</returns>
        public static string GetSpecificEntity(DocumentModel document)
        {
            var specificEntity = string.Empty;

            switch (document.entityType)
            {
                case DocumentEntityType.Cap:
                case DocumentEntityType.Related:
                case DocumentEntityType.Renewal:
                case DocumentEntityType.TMP_CAP:
                    specificEntity = document.capTypeAlias + ACAConstant.SPLITLINE + document.capID.customID;
                    break;
                case DocumentEntityType.RefContact:
                case DocumentEntityType.LP:
                    specificEntity = document.entity;
                    break;
            }

            return specificEntity;
        }

        /// <summary>
        /// Get all available licenses for people document upload.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>All available licenses.</returns>
        public static List<LicenseModel4WS> GetAvaliableLicense4PeopleDocument(string agencyCode)
        {
            var avaliableLicenses = new List<LicenseModel4WS>();
            var contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(agencyCode, AppSession.User.UserSeqNum);

            if (contractorLicenses != null && contractorLicenses.Length > 0)
            {
                foreach (var contractor in contractorLicenses)
                {
                    if (contractor != null && ContractorLicenseStatus.Approved.Equals(contractor.status, StringComparison.InvariantCultureIgnoreCase))
                    {
                        avaliableLicenses.Add(contractor.license);
                    }
                }
            }

            return avaliableLicenses;
        }

        /// <summary>
        /// Get all available contact for people document upload.
        /// </summary>
        /// <returns>All available contact.</returns>
        public static List<PeopleModel4WS> GetAvaliableContact4PeopleDocument()
        {
            List<PeopleModel4WS> avaliableContacts = new List<PeopleModel4WS>();
            PeopleModel4WS[] contractorContacts = AppSession.User.UserModel4WS.peopleModel;

            if (contractorContacts != null && contractorContacts.Length > 0)
            {
                foreach (PeopleModel4WS contractor in contractorContacts)
                {
                    if (contractor != null && ContractorPeopleStatus.Approved.Equals(contractor.contractorPeopleStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        avaliableContacts.Add(contractor);
                    }
                }
            }

            return avaliableContacts;
        }

        /// <summary>
        /// Gets file size configuration from EDMS configuration.
        /// </summary>
        /// <param name="moduleName">the module name.</param>
        /// <returns>String of file size.</returns>
        public static string GetMaxFileSizeWithUnit(string moduleName)
        {
            var capModel = AppSession.GetCapModelFromSession(moduleName);
            var capID = capModel == null ? null : capModel.capID;
            var edmsPolicy = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, moduleName, AppSession.User.PublicUserId, capID);

            if (edmsPolicy == null || string.IsNullOrEmpty(edmsPolicy.configuration))
            {
                return DataUtil.ConcatStringWithSplitChar(new string[] { DEFAULT_FILE_SIZE, FILE_UNIT_MB }, ACAConstant.BLANK);
            }

            var configs = edmsPolicy.configuration.Split(ACAConstant.SPLIT_CHAR_SEMICOLON);
            var fileSizeConfig = string.Empty;

            foreach (string config in configs)
            {
                //aca config document max size
                if (config.Contains(ACA_EDMS_DOCUMENT_SIZE_MAX))
                {
                    var result = config.Trim();
                    var start = ACA_EDMS_DOCUMENT_SIZE_MAX.Length + 1;
                    var length = result.Length - start;
                    fileSizeConfig = result.Substring(start, length);
                    break;
                }

                //aa config document max size
                if (config.Contains(EDMS_DOCUMENT_SIZE_MAX))
                {
                    var result = config.Trim();
                    var start = EDMS_DOCUMENT_SIZE_MAX.Length + 1;
                    var length = result.Length - start;
                    fileSizeConfig = result.Substring(start, length);
                }
            }

            fileSizeConfig = FormatFileSizeWithExtension(fileSizeConfig);

            return fileSizeConfig;
        }

        /// <summary>
        /// Get Disallowed file type configured in stand choice.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The disallowed file type.</returns>
        public static string GetDisallowedFileType(string moduleName)
        {
            //Disallowed file type configured in standchoice: "ACA_CONFIGS / DISALLOWED_FILE_TYPE".
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            var agencyCode = CapUtil.GetAgencyCode(moduleName);
            var disallowedFileType = bizBll.GetValueForACAConfig(agencyCode, BizDomainConstant.STD_ITEM_DISALLOWED_FILE_TYPES);

            return disallowedFileType;
        }

        /// <summary>
        /// Get bytes count from file size string.
        /// </summary>
        /// <param name="fileSizeString">File size string.</param>
        /// <returns>Number value of the bytes count. </returns>
        public static long ConvertToByteUnit(string fileSizeString)
        {
            long result = 0;

            try
            {
                if (fileSizeString.EndsWith(FILE_UNIT_BYTES, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = long.Parse(fileSizeString.Replace(FILE_UNIT_BYTES, string.Empty));
                }

                if (fileSizeString.EndsWith(FILE_UNIT_KB, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = long.Parse(fileSizeString.Replace(FILE_UNIT_KB, string.Empty)) * 1024;
                }

                if (fileSizeString.EndsWith(FILE_UNIT_K, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = long.Parse(fileSizeString.Replace(FILE_UNIT_K, string.Empty)) * 1024;
                }

                if (fileSizeString.EndsWith(FILE_UNIT_MB, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = long.Parse(fileSizeString.Replace(FILE_UNIT_MB, string.Empty)) * 1024 * 1024;
                }

                if (fileSizeString.EndsWith(FILE_UNIT_M, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = long.Parse(fileSizeString.Replace(FILE_UNIT_M, string.Empty)) * 1024 * 1024;
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Construct the document model.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="entityModel">The entity model.</param>
        /// <param name="capID">The cap ID model.</param>
        /// <returns>Return the DocumentModel.</returns>
        public static DocumentModel ConstructDocumentModel(FileUploadInfo fileInfo, EntityModel entityModel, CapIDModel4WS capID = null)
        {
            if (fileInfo == null || entityModel == null)
            {
                return null;
            }

            ITimeZoneBll timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
            string agencyCode = entityModel.serviceProviderCode;
            string userId = AppSession.User.PublicUserId;

            DateTime timeZoneDate = timeBll.GetAgencyCurrentDate(agencyCode);
            string subAgencyCode = capID != null ? capID.serviceProviderCode : agencyCode;

            DocumentContentModel documentContentModel = new DocumentContentModel
            {
                recDate = timeZoneDate,
                recFulName = userId,
                recStatus = ACAConstant.VALID_STATUS,
                servProvCode = subAgencyCode
            };

            // construct a document model for uploading
            DocumentModel documentModel = new DocumentModel();
            documentModel.docName = fileInfo.FileName;
            documentModel.fileName = fileInfo.FileName;
            documentModel.fileSize = fileInfo.FileSize;
            documentModel.recDate = timeZoneDate;
            documentModel.recFulNam = userId;
            documentModel.recStatus = ACAConstant.VALID_STATUS;
            documentModel.documentContent = documentContentModel;
            documentModel.docDate = timeZoneDate;
            documentModel.serviceProviderCode = subAgencyCode;
            documentModel.entityID = entityModel.entityID;
            documentModel.entityType = entityModel.entityType;
            documentModel.capID = capID != null ? TempModelConvert.Trim4WSOfCapIDModel(capID) : null;

            return documentModel;
        }

        /// <summary>
        /// Construct DocumentModel
        /// </summary>
        /// <param name="moduleName">Module Name.</param>
        /// <param name="fileInfo">File upload info model.</param>
        /// <param name="isSelectFromAccount">Is select from account.</param>
        /// <returns>return documentModel</returns>
        public static DocumentModel ConstructDocumentModel(string moduleName, FileUploadInfo fileInfo, bool isSelectFromAccount = false)
        {
            IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            DocumentModel documentModel = isSelectFromAccount
                                          ? edmsBll.ConstructFromAccountDocumentModel(capModel, AppSession.User.PublicUserId, ConfigManager.AgencyCode, fileInfo)
                                          : edmsBll.ConstructDocumentModel(capModel, AppSession.User.PublicUserId, ConfigManager.AgencyCode, fileInfo);

            documentModel = !string.IsNullOrEmpty(fileInfo.ParentDocumentNo) ? FillInfo4DocResubmit(documentModel, fileInfo) : documentModel;

            return documentModel;
        }

        /// <summary>
        /// Construct DocumentModel
        /// </summary>
        /// <param name="moduleName">Module Name.</param>
        /// <param name="fileInfo">File upload info model.</param>
        /// <param name="conditionNumber">Condition Number.</param>
        /// <returns>return documentModel</returns>
        public static DocumentModel ConstructDocumentModel(string moduleName, FileUploadInfo fileInfo, long conditionNumber)
        {
            DocumentModel documentModel = ConstructDocumentModel(moduleName, fileInfo);
            documentModel.docName = documentModel.fileName;
            documentModel.conditionNumber = conditionNumber;

            return documentModel;
        }

        /// <summary>
        /// delete condition document.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="conditionNumber">condition number</param>
        /// <returns>Not existing the attachment or delete the attachment successfully return true. Otherwise return false.</returns>
        public static bool DeleteConditionDocument(string moduleName, long conditionNumber)
        {
            try
            {
                DocumentModel document = FindConditionDocumentFromSource(moduleName, conditionNumber);

                if (document == null)
                {
                    return true;
                }

                bool isPartialCap = false;
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                if (capModel != null)
                {
                    isPartialCap = CapUtil.IsPartialCap(capModel.capClass);
                }

                IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                edmsBll.DoDelete(ConfigManager.AgencyCode, moduleName, document, AppSession.User.PublicUserId, isPartialCap);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Extract component name from client ID.
        /// </summary>
        /// <param name="clientID">The string value to be extracted.</param>
        /// <returns>The return value is the name of the component, if any. Otherwise, it is null.</returns>
        public static string ExtractComponentNameFromClientID(string clientID)
        {
            if (string.IsNullOrEmpty(clientID))
            {
                return null;
            }

            string componentName = null;

            /*
             * Below are some examples of the client id:
             *  
             *  A format that looks like "ctl00_PlaceHolderMain_Attachment_3188Edit_dlDocumentEdit" will come from the DataGrid.
             *  A format that looks like "ctl00_PlaceHolderMain_Attachment_3188Edit_iframeAttachmentList" will come from the CapEdit page. 
             *  A format that looks like "ctl00_PlaceHolderMain_Attachment_3188_iframeAttachmentList" will come from the CapConfirm page.
             *  
             * In these cases, only the string "Attachment_3188" is what we need.
             * It consists of two parts: a constant string "Attachment_", followed by the sequence number stored in DB.
             * 
             * The regular expression pattern Attachment_\d+ matches it.
             */
            string pattern = @"Attachment_\d+";
            Regex rgx = new Regex(pattern);
            Match m = rgx.Match(clientID);

            if (m.Success)
            {
                componentName = m.Value;
            }

            return componentName;
        }

        /// <summary>
        /// Check the EDMS server whether disabled
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="moduleName">The module name</param>
        /// <returns>true indicates the EDMS is disabled.</returns>
        public static bool IsDisabledEDMS(string agencyCode, string moduleName)
        {
            CapIDModel4WS capID = new CapIDModel4WS
            {
                serviceProviderCode = agencyCode
            };

            EdmsPolicyModel4WS edmsPolicy = CapUtil.GetEdmsPolicyModel4WS(agencyCode, moduleName, AppSession.User.PublicUserId, capID);

            return edmsPolicy == null;
        }

        /// <summary>
        /// Get failed upload file information.
        /// </summary>
        /// <param name="moduleName">Module Name.</param>
        /// <param name="fileUploadInfos">file Upload Information.</param>
        /// <returns>Return the error message.If exist error when upload file. Successfully return string.Empty</returns>
        public static string UploadFile(string moduleName, List<FileUploadInfo> fileUploadInfos)
        {
            bool isPartialCap = false;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (capModel != null)
            {
                isPartialCap = CapUtil.IsPartialCap(capModel.capClass);
            }

            int fileCount = fileUploadInfos.Count;
            string uploadFolder = GetTempDirectory();
            IList<string> failedFiles = new List<string>();
            var otherFailedMessages = new Dictionary<string, List<string>>();

            for (int i = 0; i < fileCount; i++)
            {
                var fileInfo = fileUploadInfos[i];
                var filePath = Path.Combine(uploadFolder, fileInfo.FileName + fileInfo.FileId);

                // Some document might be deleted by anti-virtus software.
                // The file is Select from account that sourceDocNbr is null.
                if (!File.Exists(filePath) && !IsSelectFromAccount(fileInfo.DocumentModel))
                {
                    failedFiles.Add(fileInfo.FileName);
                    continue;
                }

                string returnMessage = UploadSingleFile(moduleName, fileInfo.DocumentModel, filePath, isPartialCap);

                if (returnMessage.StartsWith(UPLOAD_EDMS_SINGLE_FILE_SUCCESS))
                {
                    continue;
                }

                // record the error information
                if (returnMessage.StartsWith(UPLOAD_EDMS_SINGLE_FILE_FAILED))
                {
                    failedFiles.Add(fileInfo.FileName);
                }
                else if (returnMessage.StartsWith(UPLOAD_EDMS_SINGLE_FILE_FAILED_WITH_MSG))
                {
                    returnMessage = returnMessage.Substring(UPLOAD_EDMS_SINGLE_FILE_FAILED_WITH_MSG.Length);

                    if (VALIDATE_FILE_SIZE_FAILED.Equals(returnMessage))
                    {
                        string fileSizeTip = string.Format(LabelUtil.GetTextByKey("per_attachment_label_filesize", moduleName), fileInfo.MaxFileSize);
                        failedFiles.Add(fileInfo.FileName + ": " + fileSizeTip);
                    }
                    else if (VALIDATE_FILE_TYPE_FAILED.Equals(returnMessage))
                    {
                        string fileTypeTip = LabelUtil.GetTextByKey("aca_fileuploadpage_checkfiletype_tip", moduleName);
                        failedFiles.Add(fileInfo.FileName + ": " + fileTypeTip);
                    }

                    if (otherFailedMessages.ContainsKey(returnMessage))
                    {
                        otherFailedMessages[returnMessage].Add(fileInfo.FileName);
                    }
                    else
                    {
                        List<string> tempList = new List<string>();
                        tempList.Add(fileInfo.FileName);
                        otherFailedMessages.Add(returnMessage, tempList);
                    }
                }
            }

            string errorMessage = string.Empty;

            if (failedFiles.Count > 0)
            {
                string failedNames = string.Empty;

                foreach (string failedName in failedFiles)
                {
                    failedNames += failedName + ACAConstant.HTML_BR;
                }

                failedNames = failedNames.Remove(failedNames.Length - ACAConstant.HTML_BR.Length);
                errorMessage = LabelUtil.GetTextByKey("per_permitdetail_message_uploadfailed", moduleName).Replace("'", "\\'");
                errorMessage = string.Format(errorMessage, failedFiles.Count, fileCount, failedNames);
            }

            // get the other failed message
            if (otherFailedMessages.Count > 0)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage += ACAConstant.HTML_BR + ACAConstant.HTML_BR;
                }

                foreach (string otherMsg in otherFailedMessages.Keys)
                {
                    errorMessage += otherMsg + ACAConstant.HTML_BR;
                    List<string> otherFailedName = otherFailedMessages[otherMsg];

                    foreach (string otherFile in otherFailedName)
                    {
                        errorMessage += otherFile + ACAConstant.HTML_BR;
                    }
                }

                errorMessage = errorMessage.Remove(errorMessage.Length - ACAConstant.HTML_BR.Length);
            }

            return errorMessage;
        }

        /// <summary>
        /// Has document type upload permission
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="componentName">document component name</param>
        /// <param name="needFilter">needFilter(is false when it is AccountManager Page or CapDetailPage)</param>
        /// <returns>Has permission return true, otherwise return false.</returns>
        public static bool HasDocumentTypeUploadPermission(CapModel4WS capModel, string moduleName, string componentName, bool needFilter)
        {
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

            CapIDModel4WS capID = capModel != null ? capModel.capID : null;
            UserRolePrivilegeModel userRole = userRoleBll.GetDefaultRole();

            if (capID == null)
            {
                return proxyUserRoleBll.HasPermission(capModel, userRole, ProxyPermissionType.MANAGE_DOCUMENTS);
            }

            List<XEntityPermissionModel> availableDocumentTypes = null;
            bool documentTypeConfiged;

            if (needFilter)
            {
                availableDocumentTypes = GetAvailableDocumentTypesInPageFlow(capModel, moduleName, componentName, out documentTypeConfiged);
            }

            // 1 It has not setting document type, it will use all document type by default.
            // 2 It has config no document type is associated in page flow, it should also show upload button.
            bool needDocumentTypeFilter = availableDocumentTypes != null && availableDocumentTypes.Any();

            IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            RefDocumentModel[] documentTypes = edmsBll.GetDocumentTypes(capID, AppSession.User.PublicUserId);

            // When no document type is returned, need to check if current user has permission to handle the current cap.
            // 1 No document group code is associated to CAP type.
            // 2 No document type is associated to document group.
            if (documentTypes != null && documentTypes.Length > 0)
            {
                bool hasRight = documentTypes.Any(item => HasUploadPrivilegeInPageFlow(capModel, availableDocumentTypes, item, needDocumentTypeFilter));
                return hasRight;
            }

            return proxyUserRoleBll.HasPermission(capModel, userRoleBll.GetDefaultRole(), ProxyPermissionType.MANAGE_DOCUMENTS);
        }

        /// <summary>
        /// Gets available documentTypes in page flow for every attachment.
        /// </summary>
        /// <param name="capModel">cap model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="componentName">document component name</param>
        /// <param name="documentTypeConfiged">out : whether is config Document Type in ACA admin </param>
        /// <returns>All available documentTypes</returns>
        public static List<XEntityPermissionModel> GetAvailableDocumentTypesInPageFlow(CapModel4WS capModel, string moduleName, string componentName, out bool documentTypeConfiged)
        {
            PageFlowGroupModel pfw = AppSession.GetPageflowGroupFromSession();

            if (pfw == null)
            {
                IPageflowBll pageFlowBll = ObjectFactory.GetObject<IPageflowBll>();
                pfw = pageFlowBll.GetPageflowGroupByCapType(capModel.capType);
            }

            string pageFlowGrpCode = pfw != null ? pfw.pageFlowGrpCode : string.Empty;

            List<XEntityPermissionModel> xEntityPermissionModels = null;

            // Get the CAP type assoicated document type list in current page flow.
            XEntityPermissionModel xentity = new XEntityPermissionModel();
            xentity.servProvCode = ConfigManager.AgencyCode;
            xentity.entityType = XEntityPermissionConstant.DOCUMENT_TYPE_OPTIONS;
            xentity.entityId = moduleName;
            xentity.entityId2 = pageFlowGrpCode;
            xentity.entityId3 = CAPHelper.GetCapTypeValue(capModel.capType);
            xentity.componentName = componentName ?? string.Empty;

            IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> result = xEntityPermissionBll.GetXEntityPermissions(xentity);
            xEntityPermissionModels = result != null ? result.ToList() : null;

            /* if xEntityPermissionModels not exists in DB, it indicates it has not config 
             * which document type is available in specified record type.
             * It is default value that all document types is available when not setting.
             */
            documentTypeConfiged = xEntityPermissionModels != null && xEntityPermissionModels.Any();
            List<XEntityPermissionModel> availableDocumentTypes = new List<XEntityPermissionModel>();

            if (documentTypeConfiged)
            {
                availableDocumentTypes = xEntityPermissionModels.Where(xep => ValidationUtil.IsYes(xep.permissionValue)).ToList();
            }

            return availableDocumentTypes;
        }

        /// <summary>
        /// Has Upload Privilege In Page Flow
        /// </summary>
        /// <param name="capModel">cap Model</param>
        /// <param name="availableDocumentTypes">available document types</param>
        /// <param name="documentType">document type model</param>
        /// <param name="needDocumentTypeFilter">need Document Type Filter</param>
        /// <returns>Has permission return true, otherwise return false.</returns>
        public static bool HasUploadPrivilegeInPageFlow(CapModel4WS capModel, List<XEntityPermissionModel> availableDocumentTypes, RefDocumentModel documentType, bool needDocumentTypeFilter)
        {
            // if isRestrictDocType4ACA is null, get upload permission by default role - LP,CAP Creator,associated Contacts/Owners;
            // if isRestrictDocType4ACA is 'Y', get upload permission by document type;
            // otherwise 'N', no permission. -- It's the same logic as view permission.
            //If only set contact permission,we must be consider the permission set in capcontact model in spear form.
            bool hasUploadPrivilege = false;

            if (ValidationUtil.IsYes(documentType.isRestrictDocType4ACA))
            {
                UserRolePrivilegeModel userRole = documentType.uploadRolePrivilegeModel;
                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                hasUploadPrivilege = proxyUserRoleBll.HasPermission(capModel, userRole, ProxyPermissionType.MANAGE_DOCUMENTS);
            }

            if (hasUploadPrivilege && needDocumentTypeFilter)
            {
                hasUploadPrivilege = availableDocumentTypes.Any(model => documentType.documentType.Equals(model.entityId4, StringComparison.InvariantCulture));
            }

            return hasUploadPrivilege;
        }

        /// <summary>
        /// Create the document data table.
        /// </summary>
        /// <returns>data table.</returns>
        public static DataTable CreateDocumentDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.DocNumber.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.Name.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.Description.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.Size.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.Date.ToString(), typeof(DateTime)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.Type.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.RowIndex.ToString(), typeof(int)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.ResType.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.ViewRole.ToString(), typeof(UserRolePrivilegeModel)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.DeleteRole.ToString(), typeof(UserRolePrivilegeModel)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.uploadRole.ToString(), typeof(UserRolePrivilegeModel)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.FileOwnerPermission.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.EntityType.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.Entity.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.RecordNumber.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.RecordType.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.EntityInfo.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.ViewRole4RealCAP.ToString(), typeof(bool)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.DocumentStatus.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.UploadDate.ToString(), typeof(DateTime)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.StatusDate.ToString(), typeof(DateTime)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.VirtualFolders.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.AgencyCode.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.RefAgencyCode.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.AllowActions.ToString(), typeof(string)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.DocumentModel.ToString(), typeof(DocumentModel)));
            dt.Columns.Add(new DataColumn(ColumnConstant.Attachment.ReviewStatus.ToString(), typeof(string[])));
            return dt;
        }

        /// <summary>
        /// Get Document DataRow from document model
        /// </summary>
        /// <param name="dtDocument">document data table</param>
        /// <param name="documentModel">document model</param>
        /// <param name="subAgencyCode">agency code</param>
        /// <returns>the data row</returns>
        public static DataRow GetDocumentDataRow(DataTable dtDocument, DocumentModel documentModel, string subAgencyCode)
        {
            string fileNmae = documentModel.fileName ?? string.Empty;
            string fileDescription = documentModel.docDescription ?? string.Empty;
            string fileSize = FormatFileSize(documentModel.fileSize);
            string documentNo = documentModel.documentNo != null ? documentModel.documentNo.ToString() : string.Empty;
            string fileKey = documentModel.fileKey ?? string.Empty;
            string customId = (documentModel.capID != null && DocumentEntityType.Cap.Equals(documentModel.entityType, StringComparison.InvariantCultureIgnoreCase)) ? documentModel.capID.customID : documentModel.entityID;

            DataRow dr = dtDocument.NewRow();
            dr[ColumnConstant.Attachment.DocumentModel.ToString()] = documentModel;
            dr[ColumnConstant.Attachment.Name.ToString()] = fileNmae;
            dr[ColumnConstant.Attachment.Description.ToString()] = fileDescription;
            dr[ColumnConstant.Attachment.Size.ToString()] = fileSize;
            dr[ColumnConstant.Attachment.Date.ToString()] = documentModel.recDate == null ? (object)DBNull.Value : documentModel.recDate.Value;
            dr[ColumnConstant.Attachment.Type.ToString()] = documentModel.docCategory;
            dr[ColumnConstant.Attachment.RowIndex.ToString()] = dtDocument.Rows.Count;
            dr[ColumnConstant.Attachment.ResType.ToString()] = documentModel.resDocCategory;
            dr[ColumnConstant.Attachment.ViewRole.ToString()] = documentModel.viewRoleModel;
            dr[ColumnConstant.Attachment.DeleteRole.ToString()] = documentModel.deleteRoleModel;
            dr[ColumnConstant.Attachment.uploadRole.ToString()] = documentModel.uploadRoleModel;
            dr[ColumnConstant.Attachment.FileOwnerPermission.ToString()] = documentModel.fileOwnerPermission;
            dr[ColumnConstant.Attachment.EntityType.ToString()] = documentModel.entityType;
            dr[ColumnConstant.Attachment.Entity.ToString()] = GetSpecificEntity(documentModel);
            dr[ColumnConstant.Attachment.RecordNumber.ToString()] = documentModel.altId;
            dr[ColumnConstant.Attachment.RecordType.ToString()] = documentModel.capTypeAlias;
            dr[ColumnConstant.Attachment.DocumentStatus.ToString()] = documentModel.docStatus;
            dr[ColumnConstant.Attachment.UploadDate.ToString()] = documentModel.fileUpLoadDate ?? (object)DBNull.Value;
            dr[ColumnConstant.Attachment.StatusDate.ToString()] = documentModel.docStatusDate ?? (object)DBNull.Value;
            dr[ColumnConstant.Attachment.VirtualFolders.ToString()] = documentModel.virtualFolders;
            dr[ColumnConstant.Attachment.AllowActions.ToString()] = documentModel.allowActions;
            dr[ColumnConstant.Attachment.RefAgencyCode.ToString()] = !string.IsNullOrEmpty(documentModel.refServProvCode) ? documentModel.refServProvCode : ConfigManager.AgencyCode;
            
            string[] entityInfo = { documentNo, fileKey, customId, documentModel.entityID, documentModel.entityType, subAgencyCode, documentModel.componentName };
            dr[ColumnConstant.Attachment.EntityInfo.ToString()] = DataUtil.ConcatStringWithSplitChar(entityInfo, ACAConstant.SPLIT_CHAR.ToString());
            dr[ColumnConstant.Attachment.AgencyCode.ToString()] = subAgencyCode;
            dr[ColumnConstant.Attachment.ReviewStatus.ToString()] = documentModel.reviewStatus;
            return dr;
        }

        /// <summary>
        /// add pending attachments to data table to show in grid view
        /// </summary>
        /// <param name="dtDocument">data table.</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="customID">the custom id.</param>
        /// <param name="attachmentIframeId">attachment iFrame Id</param>
        /// <param name="isPeopleDocument">is People Document</param>
        /// <returns>attachment list count.</returns>
        public static int LoadPendingAttachmentList(DataTable dtDocument, string moduleName, string customID, string attachmentIframeId, bool isPeopleDocument)
        {
            return LoadAttachmentList(dtDocument, moduleName, customID, "TempFiles", "*.*", ACAConstant.FILE_PENDING_DATE, attachmentIframeId, isPeopleDocument);
        }

        /// <summary>
        /// add failed attachments to data table to show in grid view
        /// </summary>
        /// <param name="dtDocument">data table.</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="customID">the custom id.</param>
        /// <param name="attachmentIframeId">attachment iFrame Id</param>
        /// <param name="isPeopleDocument">is People Document</param>
        public static void LoadFailedAttachmentList(DataTable dtDocument, string moduleName, string customID, string attachmentIframeId, bool isPeopleDocument)
        {
            LoadAttachmentList(dtDocument, moduleName, customID, "FailedFiles", "*.xml", ACAConstant.FILE_FAILED_DATE, attachmentIframeId, isPeopleDocument);
        }

        /// <summary>
        /// Upload file is by way of select from account.
        /// </summary>
        /// <param name="model">document model</param>
        /// <returns>Return true or false</returns>
        public static bool IsSelectFromAccount(DocumentModel model)
        {
            return model != null 
                   && model.sourceDocNbr != null 
                   && model.sourceDocNbr > 0;
        }

        /// <summary>
        /// Prepares the attachments for copying record.
        /// </summary>
        /// <param name="documentList">The document list to be checked.</param>
        /// <param name="capModel">The cap model.</param>
        /// <param name="moduleName">The module name.</param>
        public static void PrepareAttachments4FindComponent(List<DocumentModel> documentList, CapModel4WS capModel, string moduleName)
        {
            if (capModel == null || documentList == null)
            {
                return;
            }

            PageFlowGroupModel pageFlowGroup = PageFlowUtil.GetPageflowGroup(capModel);

            if (pageFlowGroup == null || pageFlowGroup.stepList == null)
            {
                return;
            }

            List<DocumentModel> needUpdateDocuments = new List<DocumentModel>();

            //The List of the name of components for document List Components are include in current page flow.
            List<string> pageFlowComponentNames = PageFlowUtil.GetAttachmentComponentName4PageFlow(pageFlowGroup);

            foreach (DocumentModel document in documentList)
            {
                //1. If find correspond component name, don't make any changes.
                if (pageFlowComponentNames.Contains(document.componentName))
                {
                    continue;
                }

                bool isFindComponent = false;

                //2. If not find correspond component name, find have available permissions of component, and change the document component name.
                foreach (string cptName in pageFlowComponentNames)
                {
                    List<string> documentTypes = GetAvailableDocumentTypeListInPageFlow(capModel, moduleName, cptName);

                    //If documenTypes is null or not any, it have all permission.
                    if (documentTypes == null || !documentTypes.Any() || documentTypes.Contains(document.docCategory))
                    {
                        isFindComponent = true;
                        document.componentName = cptName;
                        needUpdateDocuments.Add(document);
                        break;
                    }
                }

                //3. If can't find it, change the document component name to first component.
                if (!isFindComponent)
                {
                    string cptName = pageFlowComponentNames.First();
                    document.componentName = cptName; 
                    needUpdateDocuments.Add(document);
                }
            }

            if (needUpdateDocuments.Any())
            {
                IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                edmsBll.UpdateDocumentComponentNames(needUpdateDocuments.ToArray(), AppSession.User.PublicUserId);
            }
        }

        /// <summary>
        /// Prepares the attachments for copying record.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="componentName">The component name.</param>
        /// <returns>Return available document type list in page flow.</returns>
        public static List<string> GetAvailableDocumentTypeListInPageFlow(CapModel4WS capModel, string moduleName, string componentName)
        {
            List<string> availableDocumentType = new List<string>();
            bool documentTypeConfiged;
            List<XEntityPermissionModel> availableDocumentTypes = GetAvailableDocumentTypesInPageFlow(capModel, moduleName, componentName, out documentTypeConfiged);

            if (availableDocumentTypes == null || !availableDocumentTypes.Any())
            {
                return null;
            }

            foreach (XEntityPermissionModel entity in availableDocumentTypes)
            {
                availableDocumentType.Add(entity.entityId4);
            }

            return availableDocumentType;
        }

        /// <summary>
        /// Get document parent document model by parent document number that transfer from resubmit link.
        /// </summary>
        /// <param name="currentDocument">current uploading document model</param>
        /// <param name="currentFileUploadInfo">current uploading file info model</param>
        /// <returns>resubmit document model</returns>
        private static DocumentModel FillInfo4DocResubmit(DocumentModel currentDocument, FileUploadInfo currentFileUploadInfo)
        {
            IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            long parentDocNo = Convert.ToInt64(currentFileUploadInfo.ParentDocumentNo);
            DocumentModel parentDocument = edmsBll.GetDocumentByPk(currentFileUploadInfo.ParentAgencyCode, parentDocNo, false);

            /*
             * For document resubmit operation - should use the previous document's cap ID to construct the document model for resubmit:
             * 1. For current cap's document - the cap ID is current cap ID.
             * 2. For related cap's document - the cap ID is related cap's cap ID.
             */
            currentDocument.capID = parentDocument.capID;
            currentDocument.docGroup = parentDocument.docGroup;
            currentDocument.docCategory = parentDocument.docCategory;
            currentDocument.resDocCategory = parentDocument.resDocCategory;
            currentDocument.docDescription = parentDocument.docDescription;
            currentDocument.virtualFolders = parentDocument.virtualFolders;
            currentDocument.template = parentDocument.template;
            currentDocument.relatedID = parentDocument.relatedID;
            currentDocument.parentSeqNbr = Convert.ToInt64(currentFileUploadInfo.ParentDocumentNo);
            currentDocument.categoryByAction = ACAConstant.RESUBMIT;

            return currentDocument;
        }

        /// <summary>
        /// add attachments to data table to show in grid view
        /// </summary>
        /// <param name="dtDocument">data table.</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="customID">the custom id.</param>
        /// <param name="fileDir">the file directory</param>
        /// <param name="pattern">the pattern</param>
        /// <param name="documentStatus">load status.</param>
        /// <param name="attachmentIframeId">attachment iFrame Id</param>
        /// <param name="isPeopleDocument">is PeopleDocument</param>
        /// <returns>attachment list count.</returns>
        private static int LoadAttachmentList(DataTable dtDocument, string moduleName, string customID, string fileDir, string pattern, string documentStatus, string attachmentIframeId, bool isPeopleDocument)
        {
            int attachmentCount = 0;
            string dir = Path.Combine(ConfigurationManager.AppSettings["TempDirectory"], fileDir);

            if (!Directory.Exists(dir))
            {
                return attachmentCount;
            }

            string[] files = Directory.GetFiles(dir, pattern);

            foreach (string file in files)
            {
                if (fileDir == "FailedFiles" && DeleteFailedFile(file))
                {
                    continue;
                }

                LoadAttachmentList(dtDocument, moduleName, documentStatus, customID, file, attachmentIframeId, isPeopleDocument, ref attachmentCount);
            }

            return attachmentCount;
        }

        /// <summary>
        /// delete upload failed file
        /// </summary>
        /// <param name="fileName">the file name.</param>
        /// <returns>true or false.</returns>
        private static bool DeleteFailedFile(string fileName)
        {
            TimeSpan ts = DateTime.Now.Subtract(File.GetCreationTime(fileName));

            if (ts.Days >= ACAConstant.FILE_DELETE_DAYS)
            {
                File.Delete(fileName);
                fileName = fileName.Replace(ACAConstant.UPLOAD_FILE_DETAILINFO, string.Empty);

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the node.
        /// </summary>
        /// <param name="parent">parent xml node.</param>
        /// <param name="nodeName">the node name.</param>
        /// <returns>result for xml node</returns>
        private static XmlNode GetNode(XmlNode parent, string nodeName)
        {
            return parent.SelectSingleNode(nodeName);
        }

        /// <summary>
        /// Get Node Value.
        /// </summary>
        /// <param name="parent">parent xml node.</param>
        /// <param name="nodeName">the node name.</param>
        /// <returns>string for node value.</returns>
        private static string GetNodeValue(XmlNode parent, string nodeName)
        {
            XmlNode node = GetNode(parent, nodeName);

            return node == null ? string.Empty : node.InnerText;
        }

        /// <summary>
        /// add attachments to data table to show in grid view
        /// </summary>
        /// <param name="dtDocument">data table.</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="status">load status.</param>
        /// <param name="customID">the custom id.</param>
        /// <param name="filePath">the file path.</param>
        /// <param name="attachmentIFrameId"> attachment iFrame Id</param>
        /// <param name="isPeopleDocument">is People Document</param>
        /// <param name="attachmentCount">attachment Count</param>
        private static void LoadAttachmentList(DataTable dtDocument, string moduleName, string status, string customID, string filePath, string attachmentIFrameId, bool isPeopleDocument, ref int attachmentCount)
        {
            XmlDocument dom = new XmlDocument();
            dom.Load(filePath);
            XmlNode documentModelNode = dom.SelectSingleNode("AttachmentModel/DocumentModel");
            string callerID = GetNodeValue(documentModelNode, "recFulNam");
            string entityType = GetNodeValue(documentModelNode, "entityType");
            XmlNode capIDModelNode = GetNode(documentModelNode, "capID");
            string recordCustomID = capIDModelNode != null ? GetNodeValue(capIDModelNode, "customID") : string.Empty;
            string subAgencyCode = capIDModelNode != null ? GetNodeValue(capIDModelNode, "serviceProviderCode") : string.Empty;
            string componentName = GetNodeValue(documentModelNode, "componentName");

            //Only get current User's attachments or current Record's attachments.
            if ((isPeopleDocument
                    && (DocumentEntityType.LP.Equals(entityType, StringComparison.InvariantCultureIgnoreCase) || DocumentEntityType.RefContact.Equals(entityType, StringComparison.InvariantCultureIgnoreCase))
                    && AppSession.User.PublicUserId.Equals(callerID, StringComparison.InvariantCultureIgnoreCase))
                || (!string.IsNullOrEmpty(customID)
                    && customID.Equals(recordCustomID, StringComparison.InvariantCultureIgnoreCase)
                    && DocumentEntityType.Cap.Equals(entityType, StringComparison.InvariantCultureIgnoreCase)
                    && CapUtil.GetAgencyCode(moduleName).Equals(subAgencyCode, StringComparison.InvariantCultureIgnoreCase)
                    && (string.IsNullOrEmpty(componentName) || componentName.Equals(ExtractComponentNameFromClientID(attachmentIFrameId), StringComparison.InvariantCultureIgnoreCase))))
            {
                DataRow dr = dtDocument.NewRow();
                dr[ColumnConstant.Attachment.DocNumber.ToString()] = GetNodeValue(documentModelNode, "documentNo");
                dr[ColumnConstant.Attachment.Name.ToString()] = GetNodeValue(documentModelNode, "fileName");
                dr[ColumnConstant.Attachment.Description.ToString()] = GetNodeValue(documentModelNode, "docDescription");
                string fileSize = GetNodeValue(documentModelNode, "fileSize");

                if (!string.IsNullOrEmpty(fileSize))
                {
                    dr[ColumnConstant.Attachment.Size.ToString()] = FormatFileSize(fileSize);
                }

                dr[ColumnConstant.Attachment.Date.ToString()] = I18nDateTimeUtil.ParseFromWebService4DataTable(status);
                dr[ColumnConstant.Attachment.Type.ToString()] = GetNodeValue(documentModelNode, "docCategory");
                dr[ColumnConstant.Attachment.RowIndex.ToString()] = dtDocument.Rows.Count;
                dr[ColumnConstant.Attachment.ResType.ToString()] = GetNodeValue(documentModelNode, "resDocCategory");
                dr[ColumnConstant.Attachment.VirtualFolders.ToString()] = GetNodeValue(documentModelNode, "virtualFolders");
                dr[ColumnConstant.Attachment.EntityType.ToString()] = entityType;
                dtDocument.Rows.Add(dr);
                attachmentCount++;
            }
        }

        /// <summary>
        /// Format the file size configuration string.
        /// If configuration is valid return the configuration.
        /// If configuration is invalid return the default file size.
        /// </summary>
        /// <param name="sizeConfigString">File size configuration string.</param>
        /// <returns>String of file size.</returns>
        private static string FormatFileSizeWithExtension(string sizeConfigString)
        {
            string temp;
            var unitPart = FILE_UNIT_MB;
            var numberPart = DEFAULT_FILE_SIZE;

            if (sizeConfigString.EndsWith(FILE_UNIT_BYTES, StringComparison.InvariantCultureIgnoreCase))
            {
                temp = sizeConfigString.Substring(0, sizeConfigString.Length - FILE_UNIT_BYTES.Length);

                if (ValidationUtil.IsNumber(temp))
                {
                    numberPart = temp;
                    unitPart = FILE_UNIT_BYTES;
                }
            }
            else if (sizeConfigString.EndsWith(FILE_UNIT_KB, StringComparison.InvariantCultureIgnoreCase))
            {
                temp = sizeConfigString.Substring(0, sizeConfigString.Length - FILE_UNIT_KB.Length);

                if (ValidationUtil.IsNumber(temp))
                {
                    numberPart = temp;
                    unitPart = FILE_UNIT_KB;
                }
            }
            else if (sizeConfigString.EndsWith(FILE_UNIT_MB, StringComparison.InvariantCultureIgnoreCase))
            {
                temp = sizeConfigString.Substring(0, sizeConfigString.Length - FILE_UNIT_MB.Length);

                if (ValidationUtil.IsNumber(temp))
                {
                    numberPart = temp;
                    unitPart = FILE_UNIT_MB;
                }
            }
            else if (ValidationUtil.IsNumber(sizeConfigString))
            {
                numberPart = sizeConfigString;
            }

            var result = DataUtil.ConcatStringWithSplitChar(new[] { numberPart, unitPart }, ACAConstant.BLANK);

            return result;
        }

        /// <summary>
        /// upload a file
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <param name="documentModel">document model</param>
        /// <param name="filePath">full file path</param>
        /// <param name="isPartialCap">is partial cap</param>
        /// <returns>return message</returns>
        private static string UploadSingleFile(string moduleName, DocumentModel documentModel, string filePath, bool isPartialCap)
        {
            string returnMessage = UPLOAD_EDMS_SINGLE_FILE_SUCCESS;

            try
            {
                IEDMSDocumentBll edmsBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
                var result = edmsBll.DoUpload(moduleName, documentModel, filePath, isPartialCap);
                var resultMessage = string.Empty;

                if (result != null)
                {
                    resultMessage = ACAConstant.COMMON_ZERO.Equals(result.returnCode) ? string.Empty : result.returnMessage;
                }

                if (!string.IsNullOrEmpty(resultMessage))
                {
                    returnMessage = UPLOAD_EDMS_SINGLE_FILE_FAILED_WITH_MSG + resultMessage;
                }
            }
            catch (Exception)
            {
                returnMessage = UPLOAD_EDMS_SINGLE_FILE_FAILED;
            }

            return returnMessage;
        }

        /// <summary>
        /// Find condition document form source.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="conditionNumber">the condition number.</param>
        /// <returns>Document model.</returns>
        private static DocumentModel FindConditionDocumentFromSource(string moduleName, long conditionNumber)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            CapIDModel capId = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
            DocumentModel[] tempDocList = edmsDocBll.GetRecordDocumentList(capId.serviceProviderCode, moduleName, AppSession.User.PublicUserId, capId, true);
            DocumentModel documentModel = null;

            if (tempDocList != null && tempDocList.Any(t => conditionNumber == t.conditionNumber))
            {
                documentModel = tempDocList.FirstOrDefault(t => conditionNumber == t.conditionNumber);
            }

            return documentModel;
        }

        /// <summary>
        /// check filer owner permission for contact document.
        /// </summary>
        /// <param name="capContacts">contacts from cap</param>
        /// <param name="refPeoples">peoples which associated with public user</param>
        /// <param name="document">people document</param>
        /// <returns>return a boolean value to indicate public user is this people document's file owner</returns>
        private static bool IsContactFileOwner(List<CapContactModel4WS> capContacts, PeopleModel4WS[] refPeoples, DocumentModel document)
        {
            if (capContacts == null || capContacts.Count == 0 || refPeoples == null || refPeoples.Length == 0)
            {
                return false;
            }

            foreach (var refPeople in refPeoples)
            {
                foreach (var capContact in capContacts)
                {
                    if (refPeople != null
                        && capContact != null
                        && capContact.refContactNumber != null
                        && capContact.refContactNumber.Equals(refPeople.contactSeqNumber, StringComparison.InvariantCultureIgnoreCase)
                        && document.entityID.Equals(capContact.refContactNumber, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// check file owner permission for LP document. 
        /// </summary>
        /// <param name="lpListInCap">license professional from cap</param>
        /// <param name="refLicenses">license professional which associated with public user</param>
        /// <param name="document">people document</param>
        /// <returns>return a boolean value to indicate public user is this people document's file owner</returns>
        private static bool IsLpFileOwner(LicenseProfessionalModel4WS[] lpListInCap, ContractorLicenseModel4WS[] refLicenses, DocumentModel document)
        {
            if (lpListInCap == null || lpListInCap.Length == 0 || refLicenses == null || refLicenses.Length == 0)
            {
                return false;
            }

            foreach (var refLicense in refLicenses)
            {
                foreach (var lpModel in lpListInCap)
                {
                    if (refLicense.license != null && lpModel.licSeqNbr.Equals(refLicense.licenseSeqNBR, StringComparison.InvariantCultureIgnoreCase)
                        && lpModel.licSeqNbr.Equals(document.entityID, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #region Structs

        /// <summary>
        /// list item variable keys
        /// </summary>
        public struct FileUploadVariables
        {
            /// <summary>
            /// Maximum File Size
            /// </summary>
            public const string MaximumFileSize = "$$MaximumFileSize$$";

            /// <summary>
            /// Forbidden File Formats
            /// </summary>
            public const string ForbiddenFileFormats = "$$ForbiddenFileFormats$$";

            /// <summary>
            /// Required Document Types Formats
            /// </summary>
            public const string RequiredDocumentTypeFormats = "$$RequiredDocumentTypes$$";

            /// <summary>
            /// Upload button label.
            /// </summary>
            public const string UploadButtonLabel = "$$UploadButtonLabel$$";

            /// <summary>
            /// Condition Document Description.
            /// </summary>
            public const string ConditionDocDescription = "$$ConditionDocDescription$$";
        }

        #endregion Structs
    }
}