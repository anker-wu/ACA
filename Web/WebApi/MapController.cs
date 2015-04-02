#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MapController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:MapController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\Reid.wang.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Xml;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.WebApi.Entity;
using Accela.ACA.Web.WebApi.Entity.Adapter;
using Accela.ACA.Web.WebApi.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Web API for ESRI map 
    /// </summary>
    public class MapController : ApiController
    {
        /// <summary>
        /// get address for display in permit list
        /// </summary>
        /// <param name="capMode">cap model for aca.</param>
        /// <returns>string for permit address.</returns>
        [NonAction]
        public static string GetPermitAddress(SimpleCapModel capMode)
        {
            string result = string.Empty;

            if (capMode != null && capMode.addressModel != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                result = addressBuilderBll.BuildAddressByFormatType(capMode.addressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return result;
        }

        /// <summary>
        /// Get Gis config information.
        /// </summary>
        /// <returns>GIS Server Url</returns>
        [ActionName("gisServerUrl")]
        public HttpResponseMessage GetGisServerUrl()
        {
            var url = StandardChoiceUtil.GetNewGISServerURL(ConfigManager.AgencyCode);

            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { gisApiUrl = url, agencyCode = ConfigManager.AgencyCode }))
            };
        }

        /// <summary>
        /// GET locate INFO
        /// </summary>
        /// <param name="capIds">cap Id</param>
        /// <returns>locate INFO</returns>
        [ActionName("locate")]
        public HttpResponseMessage GetlocateInfo(string capIds)
        {
            string result = string.Empty;
            result = GetSingleLocateInfo(capIds);

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"records\": [{ " + result + " }]}")
            };
        }

        /// <summary>
        /// Get All locate INFO
        /// </summary>
        /// <param name="ckCapIds">CAP ID</param>
        /// <returns>All locate INFO</returns>
        [ActionName("locate-All-Record")]
        public HttpResponseMessage GetAlllocateInfo(string ckCapIds)
        {
            string result = string.Empty;
            string[] capids = ckCapIds.Split(new string[] { @"," }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < capids.Length; i++)
            {
                result += "{" + GetSingleLocateInfo(capids[i]) + "},";
            }

            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"records\": [ " + result + " ]}")
            };
        }

        /// <summary>
        /// get search records and parcel information
        /// </summary>
        /// <param name="gisId">GIS ID</param>
        /// <param name="layerId">layer ID</param>
        /// <param name="serviceId">service ID</param>
        /// <returns>Search Result</returns>
        [ActionName("SearchResult4Map")]
        public HttpResponseMessage GetSearchResult(string gisId, string layerId, string serviceId)
        {
            List<GISObjectModel> gISModelList = GetGISModelList(gisId, layerId, serviceId);

            string aPOByGIS = GetAPOByGIS(0, null, gISModelList);
            string capByGIS = GetCapByGIS(0, null, gISModelList);

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"searchResult\":{" + aPOByGIS + "," + capByGIS + "}}")
            };
        }

        /// <summary>
        /// get records or parcel detail information
        /// </summary>
        /// <param name="itemNumber">item Number</param>
        /// <returns>Cap Detail</returns>
        [ActionName("CapDetailInfo")]
        public HttpResponseMessage GetCapDetailInfo(string itemNumber)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\"Record\":[");
            string singleRecordJson = GetSingleCapJson(itemNumber);
            sb.Append(singleRecordJson);
            sb.Append("]");

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"capDetail\":{" + sb.ToString() + "}}")
            };
        }

        /// <summary>
        /// get parcel detail information
        /// </summary>
        /// <param name="itemNumber">item Number</param>
        /// <param name="itemAddress">item Address</param>
        /// <param name="owner">owner information</param>
        /// <returns>Parcel Detail</returns>
        [ActionName("ParcelDetailInfo")]
        public HttpResponseMessage GetParcelDetailInfo(string itemNumber, string itemAddress, string owner)
        {
            StringBuilder sb = new StringBuilder();

            ParcelModel parcelModel = GetSingleParcelInfoModel(itemNumber);
            sb.Append("\"Parcel\":[");
            sb.Append("{");

            if (parcelModel != null)
            {
                sb.Append("\"ParcelNumber\":\"" + GetRealValue(parcelModel.parcelNumber) + "\",");
                sb.Append("\"Owner\":\"" + GetRealValue(owner) + "\",");
                sb.Append("\"Address\":\"" + GetRealValue(itemAddress) + "\"");
            }

            sb.Append("}");
            sb.Append("]");

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"parcelDetail\":{" + sb.ToString() + "}}")
            };
        }

        /// <summary>
        /// Get APO location information
        /// </summary>
        /// <param name="addressNumber">address source number</param>
        /// <param name="parcelNumber">parcel number</param>
        /// <returns>location data json format</returns>
        [ActionName("APOLocateInfo")]
        public HttpResponseMessage GetApoLocateInfo(string addressNumber, string parcelNumber)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(parcelNumber))
            {
                result = GetParcelLocateInfo(parcelNumber);
            }
            else
            {
                if (!string.IsNullOrEmpty(addressNumber))
                {
                    result = "{\"addresses\":[" + GetSingleAddress(addressNumber) + "]}";
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "{}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Get APO location information
        /// </summary>
        /// <param name="addressNumbers">address source number</param>
        /// <param name="parcelNumbers">parcel number</param>
        /// <param name="capIds">cap ID</param>
        /// <returns>location data json format</returns>
        [ActionName("ApoOrCapInfo")]
        public HttpResponseMessage GetApoOrCapInfo(string addressNumbers, string parcelNumbers, string capIds)
        {
            string result = string.Empty;
            string parcelResult = string.Empty;
            string addressResult = string.Empty;
            string recordResult = string.Empty;

            if (!string.IsNullOrEmpty(parcelNumbers))
            {
                StringBuilder sb = new StringBuilder();
                string[] pNumbers = parcelNumbers.Split(new string[] { @"," }, StringSplitOptions.RemoveEmptyEntries);

                sb.Append("\"ServiceProviderCode\": \"" + ConfigManager.AgencyCode + "\",");
                sb.Append("\"language\": \"" + I18nCultureUtil.DefaultCulture + "\",");
                sb.Append("\"context\": \"\",");
                sb.Append("\"parcels\": [");

                if (pNumbers.Length > 0)
                {
                    for (int i = 0; i < pNumbers.Length; i++)
                    {
                        sb.Append(" {");
                        sb.Append("\"parcelNumber\": \"" + pNumbers[i] + "\",");
                        sb.Append("\"unmaskedParcelNumber\": \"\"");
                        sb.Append("},");
                    }
                }

                sb = GetRealString(sb);
                sb.Append("],");
                parcelResult = sb.ToString();
                result += parcelResult;
            }

            if (!string.IsNullOrEmpty(addressNumbers))
            {
                StringBuilder sb = new StringBuilder();
                string[] aNumbers = addressNumbers.Split(new string[] { @"|" }, StringSplitOptions.RemoveEmptyEntries);

                sb.Append("\"addresses\":[");

                if (aNumbers.Length > 0)
                {
                    for (int i = 0; i < aNumbers.Length; i++)
                    {
                        string address = GetSingleAddress(aNumbers[i]);
                        if (!string.IsNullOrEmpty(address))
                        {
                            address = address + ",";
                        }

                        sb.Append(address);
                    }
                }

                sb = GetRealString(sb);
                sb.Append("],");
                addressResult = sb.ToString();
                result += addressResult;
            }

            if (!string.IsNullOrEmpty(capIds))
            {
                StringBuilder sb = new StringBuilder();
                string[] ids = capIds.Split(new string[] { @"," }, StringSplitOptions.RemoveEmptyEntries);

                sb.Append("\"records\":[");

                for (int i = 0; i < ids.Length; i++)
                {
                    string singleRecordJson = "{" + GetSingleLocateInfo(ids[i]) + "}";
                    sb.Append(singleRecordJson + ",");
                }

                sb = GetRealString(sb);
                sb.Append("]");
                recordResult = sb.ToString();
                result += recordResult;
            }

            if (string.IsNullOrEmpty(result))
            {
                result = "{}";
            }
            else
            {
                if (result.EndsWith(","))
                {
                    result = result.Substring(0, result.Length - 1);
                }

                result = "{" + result + "}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Link to Apply
        /// </summary>
        /// <param name="parcelNumber">parcel Number</param>
        /// <param name="addressDescription">address Description</param>
        /// <param name="moduleName">module Name</param>
        /// /// <returns>Apo Apply</returns>
        [ActionName("ApoApplyInfo")]
        public HttpResponseMessage GetApoApply(string parcelNumber, string addressDescription, string moduleName)
        {
            HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] = null;

            ACAGISModel acaGisModel = GISUtil.CreateACAGISModel();
            acaGisModel.Agency = ConfigManager.AgencyCode;

            PaginationModel pageInfo = new PaginationModel();
            pageInfo.SortExpression = null;
            pageInfo.CurrentPageIndex = 0;
            pageInfo.CustomPageSize = 100;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;

            //Fill the data into the model
            acaGisModel.CommandName = ACAConstant.AGIS_COMMAND_CREATE_CAP;
            acaGisModel.ModuleName = moduleName;
            acaGisModel.IsHideSendAddress = false;
            acaGisModel.IsHideSendFeatures = false;
            acaGisModel.IsMiniMap = false;
            acaGisModel.Windowless = false;

            if (!string.IsNullOrEmpty(parcelNumber))
            {
                ParcelModel parcel = new ParcelModel();
                parcel.parcelNumber = parcelNumber;
                acaGisModel.ParcelModels = new[] { parcel };

                SearchResultModel result = apoBll.GetParcelInfoByParcel(ConfigManager.AgencyCode, parcel, queryFormat, false);
                ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
                acaGisModel.ParcelInfoModels = parcelInfos;
            }
            else if (!string.IsNullOrEmpty(addressDescription))
            {
                RefAddressModel addressModel = new RefAddressModel();
                addressModel.fullAddress = addressDescription;
                acaGisModel.RefAddressModels = new[] { addressModel };

                SearchResultModel result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, addressModel, queryFormat, false);
                ParcelInfoModel[] address = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
                acaGisModel.ParcelInfoModels = address;
            }

            HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] = acaGisModel;

            return new HttpResponseMessage
            {
                Content = new StringContent("{}")
            };
        }

        /// <summary>
        /// Get documents 
        /// </summary>
        /// <param name="capID">Cap ID</param>
        /// <param name="agencyCode">module Name</param>
        /// <returns>Get document</returns>
        [ActionName("CapDocuments")]
        public HttpResponseMessage GetCapDocuments(string capID, string agencyCode)
        {
            DocumentModel[] documentModels = new DocumentModel[0];
            string message = string.Empty;
            string capClass = string.Empty;
            string moduleName = string.Empty;
            CapModel4WS capModel = null;
            try
            {
                string[] capIds = capID.Split('-');
                CapIDModel4WS capIDModel = new CapIDModel4WS
                {
                    id1 = capIds[0],
                    id2 = capIds[1],
                    id3 = capIds[2],
                    serviceProviderCode = agencyCode
                };

                capModel = ApiUtil.GetCapModel4Ws(capIDModel);
                documentModels = ApiUtil.GetDocumentModels(capModel, agencyCode);
                capClass = capModel.capClass;
                moduleName = capModel.moduleName;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            List<object> documentList = new List<object>();
            foreach (var item in documentModels)
            {
                Dictionary<object, object> dictionary = new Dictionary<object, object>();
                dictionary.Add("fileName", item.fileName);
                dictionary.Add("postfix", item.fileName.Substring(item.fileName.LastIndexOf(".") + 1).ToLower());
                dictionary.Add("documentNo", item.documentNo);
                dictionary.Add("fileKey", item.fileKey);
                dictionary.Add("entityID", item.entityID);
                dictionary.Add("entityType", item.entityType);
                dictionary.Add("altId", item.altId);
                dictionary.Add("capClass", capClass ?? string.Empty);
                dictionary.Add("agencyCode", agencyCode);
                dictionary.Add("moduleName", moduleName);
                dictionary.Add("isDownload", ApiUtil.DownloadPermissions(item, capModel));
                documentList.Add(dictionary);
            }

            string jsonData = "{\"isOk\": " + (message == string.Empty ? "true" : "false") + ", \"message\": \"" + message + "\", \"data\": " + Newtonsoft.Json.JsonConvert.SerializeObject(documentList) + " }";

            return new HttpResponseMessage
            {
                Content = new StringContent(jsonData)
            };
        }

        /// <summary>
        /// Get LP documents 
        /// </summary>
        /// <param name="resLicenseType">resLicense Type</param>
        /// <param name="licenseType">license Type</param>
        /// <param name="licenseNumber">license Number</param>
        /// <returns>Get Documents</returns>
        [ActionName("LpDocuments")]
        public HttpResponseMessage GetLPDocuments(string resLicenseType, string licenseType, string licenseNumber)
        {
            DocumentModel[] documentModels = new DocumentModel[0];
            string message = string.Empty;
            string capClass = string.Empty;
            string moduleName = string.Empty;
            string agencyCode = ConfigManager.SuperAgencyCode;

            LicenseModel4WS licenseModel = new LicenseModel4WS();
            licenseModel.licenseType = licenseType;
            licenseModel.stateLicense = licenseNumber;
            licenseModel.serviceProviderCode = agencyCode;

            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel4WS licenseModel4Ws = licenseBll.GetLicenseByStateLicNbr(licenseModel);

            try
            {
                if (licenseModel4Ws != null)
                {
                    string licSeqNbr = licenseModel4Ws.licSeqNbr;
                    var entity = new EntityModel();
                    entity.entityType = DocumentEntityType.LP;
                    entity.entityID = licSeqNbr;
                    entity.customID = licSeqNbr;
                    entity.serviceProviderCode = agencyCode;
                    entity.entity = I18nStringUtil.GetString(resLicenseType, resLicenseType) + ACAConstant.SPLITLINE +
                                    licenseNumber;

                    if (!licenseModel4Ws.licExpired && ACAConstant.VALID_STATUS.Contains(licenseModel4Ws.auditStatus))
                    {
                        documentModels = ApiUtil.GetDocumentModels(null, entity.serviceProviderCode, true, true, false, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            documentModels = documentModels ?? new DocumentModel[0];

            List<object> documentList = new List<object>();
            foreach (var item in documentModels)
            {
                Dictionary<object, object> dictionary = new Dictionary<object, object>();
                dictionary.Add("fileName", item.fileName);
                dictionary.Add("postfix", item.fileName.Substring(item.fileName.LastIndexOf(".") + 1).ToLower());
                dictionary.Add("documentNo", item.documentNo);
                dictionary.Add("fileKey", item.fileKey);
                dictionary.Add("entityID", item.entityID);
                dictionary.Add("entityType", item.entityType);
                dictionary.Add("altId", item.altId);
                dictionary.Add("capClass", capClass);
                dictionary.Add("moduleName", moduleName);
                dictionary.Add("agencyCode", agencyCode);
                dictionary.Add("isDownload", ApiUtil.DownloadPermissions(item, null, false, true));
                documentList.Add(dictionary);
            }

            string jsonData = "{\"isOk\": " + (message == string.Empty ? "true" : "false") + ", \"message\": \"" + message + "\", \"data\": " + Newtonsoft.Json.JsonConvert.SerializeObject(documentList) + " }";

            return new HttpResponseMessage
            {
                Content = new StringContent(jsonData)
            };
        }

        /// <summary>
        /// Get Download Document
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="capClass">cap Class</param>
        /// <param name="documentNo">document No</param>
        /// <param name="fileKey">file Key</param>
        /// <param name="entityID">entity ID</param>
        /// <param name="entityType">entity Type</param>
        /// <param name="altId">alt Id</param>
        /// <returns>Download Document</returns>
        [ActionName("DownloadDocument")]
        public HttpResponseMessage GetDownloadDocument(
            string agencyCode,
            string moduleName,
            string capClass,
            string documentNo,
            string fileKey,
            string entityID,
            string entityType,
            string altId)
        {
            EntityModel entity = new EntityModel();
            entity.entityID = entityID;
            entity.customID = altId;
            entity.entityType = entityType;
            entity.serviceProviderCode = agencyCode;
            entity.moduleName = moduleName;
            capClass = capClass == "null" ? null : capClass;
            DocumentModel document = ApiUtil.DownloadAttachment(entity, documentNo, fileKey, capClass);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            if (document != null)
            {
                byte[] buffer = document.documentContent.docContentStream;
                string fileName = document.fileName;

                if (buffer.Length > 0)
                {
                    ByteArrayContent byteArrayContent = new ByteArrayContent(buffer);
                    byteArrayContent.Headers.Add("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20"));
                    byteArrayContent.Headers.Add("Content-type", "application/octet-stream");
                    httpResponseMessage = new HttpResponseMessage() { Content = byteArrayContent };
                }
            }

            return httpResponseMessage;
        }

        #region GisConfiguration

        /// <summary>
        /// Set gis session module name
        /// </summary>
        /// <param name="moduleName">module name</param>
        [NonAction]
        public void SetGisSessionModuleName(string moduleName)
        {
            if (HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] != null)
            {
                ACAGISModel model = (ACAGISModel)HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP];

                if (!string.IsNullOrEmpty(moduleName))
                {
                    model.ModuleName = moduleName;
                }

                HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] = model;
            }
        }

        /// <summary>
        /// Set gis information to session
        /// </summary>
        /// <param name="mapData">map data</param>
        /// <returns>Gis message</returns>
        [ActionName("GisSession")]
        [HttpGet]
        public HttpResponseMessage SetGisSession(string mapData)
        {
            string message = string.Empty;
            dynamic mapJson = JsonConvert.DeserializeObject(mapData);
            ACAGISModel model = GISUtil.CreateACAGISModel();

            if (string.IsNullOrEmpty(model.Agency))
            {
                model.Agency = ConfigManager.AgencyCode;
            }

            //Fill the data into the model
            model.CommandName = ACAConstant.AGIS_COMMAND_CREATE_CAP;
            model.IsHideSendAddress = false;
            model.IsHideSendFeatures = false;

            try
            {
                var isUseLocation = false;
                string type = string.Empty;

                try
                {
                    type = mapJson.gISObjects[0].type.ToString();
                }
                catch (Exception)
                {
                }

                try
                {
                    isUseLocation = Convert.ToBoolean(mapJson.isUseLocation.ToString());
                }
                catch (Exception)
                {
                }

                if (isUseLocation)
                {
                    RefAddressModel rAddressModel = GetAddressModel(mapJson, "UseLocation");

                    model.ParcelInfoModels = new ParcelInfoModel[]
                    {
                        new ParcelInfoModel
                        {
                            RAddressModel = rAddressModel
                        }
                    };

                    message = CreateCap(model, model.ParcelInfoModels);

                    return new HttpResponseMessage
                    {
                        Content = new StringContent("{\"message\":\"" + message + "\"}")
                    };
                }
                else if (!string.IsNullOrEmpty(type) && type.ToLower() == "address")
                {
                    RefAddressModel rAddressModel = GetAddressModel(mapJson, string.Empty);

                    model.ParcelInfoModels = new ParcelInfoModel[]
                    {
                        new ParcelInfoModel
                        {
                            RAddressModel = rAddressModel
                        }
                    };

                    if (model.RefAddressModels != null && model.RefAddressModels.Length == 1)
                    {
                        model.RefAddressModels[0].primaryFlag = null;
                    }

                    message = CreateCap(model, model.ParcelInfoModels);

                    return new HttpResponseMessage
                    {
                        Content = new StringContent("{\"message\":\"" + message + "\"}")
                    };
                }
                else
                {
                    model.GisObjects = new[]
                    {
                        new GISObjectModel
                        {
                            layerId = mapJson.gISObjects[0].type.ToString(),
                            gisId = mapJson.gISObjects[0].key.ToString(),
                            serviceID = mapJson.mapServiceId.ToString()
                        }
                    };
                }

                model.IsMiniMap = false;
                model.Windowless = false;
            }
            catch (Exception)
            {
            }

            message = CreateCap(model, null);

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"message\":\"" + message + "\"}")
            };
        }

        /// <summary>
        /// process create cap command post back.
        /// </summary>
        /// <param name="model">ACA GISModel</param>
        /// <param name="parcelInfos">ParcelInfo Models</param>
        /// <returns>Gis message</returns>
        [NonAction]
        private string CreateCap(ACAGISModel model, ParcelInfoModel[] parcelInfos = null)
        {
            string message = string.Empty;
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;

            if (model.CapIDModels != null && model.CapIDModels.Length == 1)
            {
                if (IsLock(model.CapIDModels[0]))
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_gis_message_recordlocked").Replace("'", "\\'");
                }

                ICapBll capBll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
                CapIDModel capIdModel = model.CapIDModels[0];
                CapIDModel4WS capId = new CapIDModel4WS()
                {
                    id1 = capIdModel.ID1,
                    id2 = capIdModel.ID2,
                    id3 = capIdModel.ID3,
                    serviceProviderCode = capIdModel.serviceProviderCode
                };

                CapWithConditionModel4WS capCondition = capBll.GetCapViewBySingle(
                    capId, AppSession.User.PublicUserId, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());
                CapUtil.FillCapModelTemplateValue(capCondition.capModel);

                if (capCondition.capModel != null)
                {
                    List<ParcelInfoModel> list = new List<ParcelInfoModel>();
                    ParcelInfoModel parcelInfo = new ParcelInfoModel();
                    if (capCondition.capModel.addressModel != null)
                    {
                        parcelInfo.RAddressModel =
                            CapUtil.ConvertAddressModel2RefAddressModel(capCondition.capModel.addressModel);
                    }

                    if (capCondition.capModel.parcelModel != null)
                    {
                        parcelInfo.parcelModel = capCondition.capModel.parcelModel.parcelModel;
                    }

                    if (capCondition.capModel.ownerModel != null)
                    {
                        parcelInfo.ownerModel = CapUtil.ConvertRefOwnerModel2OwnerModel(capCondition.capModel.ownerModel);
                    }

                    list.Add(parcelInfo);
                    parcelInfos = list.ToArray();
                }
            }
            else if (model.GisObjects != null && model.GisObjects.Length == 1)
            {
                ParcelModel parcelModel = new ParcelModel();
                parcelModel.gisObjectList = model.GisObjects;
                parcelModel.parcelStatus = ACAConstant.VALID_STATUS;
                parcelModel.auditStatus = ACAConstant.VALID_STATUS;
                SearchResultModel result = apoBll.GetParcelInfoByParcel(model.Agency, parcelModel, null, true);
                parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (parcelInfos == null)
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_gis_msg_norecord_link_gis_object").Replace("'", "\\'");
                }
                else if (parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_gis_message_parcellockedwithcreaterecord")
                        .Replace("'", "\\'");
                }
            }
            else if (model.RefAddressModels != null && model.RefAddressModels.Length == 1)
            {
                model.RefAddressModels[0].addressStatus = ACAConstant.VALID_STATUS;
                model.RefAddressModels[0].auditStatus = ACAConstant.VALID_STATUS;
                SearchResultModel result = apoBll.GetParcelInfoByAddress(model.Agency, model.RefAddressModels[0], null, true);
                parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_gis_message_addresslockedwithcreaterecord")
                        .Replace("'", "\\'");
                }
            }
            else if (model.ParcelModels != null && model.ParcelModels.Length == 1)
            {
                model.ParcelModels[0].parcelStatus = ACAConstant.VALID_STATUS;
                model.ParcelModels[0].auditStatus = ACAConstant.VALID_STATUS;
                SearchResultModel result = apoBll.GetParcelInfoByParcel(model.Agency, model.ParcelModels[0], null, true);
                parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (parcelInfos == null)
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_gis_msg_disable_parcel").Replace("'", "\\'");
                }

                if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    message = LabelUtil.GetGlobalTextByKey("aca_gis_message_parcellockedwithcreaterecord")
                        .Replace("'", "\\'");
                }
            }

            //if the gisobject have associated ParcelInfoModels then set it to session
            if (parcelInfos != null && parcelInfos.Length > 0)
            {
                model.ParcelInfoModels = parcelInfos;
                HttpContext.Current.Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] = model;
            }
            else
            {
                message = LabelUtil.GetGlobalTextByKey("aca_gis_no_record_message").Replace("'", "\\'");
            }

            return message;
        }

        /// <summary>
        /// Set RefAddressModel by Address
        /// </summary>
        /// <param name="mapJson">map data json</param>
        /// <param name="type">type:UseLocation or other</param>
        /// <returns>Ref AddressModel</returns>
        [NonAction]
        private RefAddressModel GetAddressModel(dynamic mapJson, string type)
        {
            RefAddressModel rAddressModel = new RefAddressModel();
            dynamic address;

            if (type == "UseLocation")
            {
                address = mapJson.gISObjects.address;
                rAddressModel.XCoordinator = mapJson.gISObjects.x;
                rAddressModel.YCoordinator = mapJson.gISObjects.y;
                rAddressModel.distance = GetObjectValue(address.Distance) != null ? address.Distance : null;
                rAddressModel.unitStart = GetObjectValue(address.Unit);
                rAddressModel.unitEnd = GetObjectValue(address.UnitEnd);
                rAddressModel.houseNumberStartFrom = GetObjectValue(address.HouseNumberRangeStartFrom);
                rAddressModel.houseNumberStartTo = GetObjectValue(address.HouseNumberRangeStartTo);
                rAddressModel.houseNumberEndFrom = GetObjectValue(address.HouseNumberRangeEndFrom);
                rAddressModel.houseNumberEndTo = GetObjectValue(address.HouseNumberRangeEndTo);
                rAddressModel.houseNumberEnd = GetObjectValue(address.HouseNumberEnd);
                rAddressModel.houseFractionStart = GetObjectValue(address.houseNumberFraction);
                rAddressModel.streetPrefix = GetObjectValue(address.StreetPrefix);
                rAddressModel.country = GetObjectValue(address.Country);
                rAddressModel.fullAddress = GetObjectValue(address.Fulladdress);
                rAddressModel.streetSuffixdirection = GetObjectValue(address.StreetSuffixdirection);
                rAddressModel.addressLine1 = GetObjectValue(address.Line1);
                rAddressModel.addressLine2 = GetObjectValue(address.Line2);
                rAddressModel.neighborhoodPrefix = GetObjectValue(address.NeighberhoodPrefix);
                rAddressModel.neighborhood = GetObjectValue(address.NeighberhoodPrefix);
            }
            else
            {
                address = mapJson.gISObjects[0].key;
            }

            int? houseNumber = null;

            if (!string.IsNullOrEmpty(GetObjectValue(address.HouseNumber)))
            {
                houseNumber = GetObjectValue(address.HouseNumber) != null
                    ? Convert.ToInt32(GetObjectValue(address.HouseNumber)) : null;
            }

            if (!string.IsNullOrEmpty(GetObjectValue(address.HouseNumber)))
            {
                rAddressModel.houseNumberStart = houseNumber;
            }

            rAddressModel.zip = GetObjectValue(address.Postal);
            rAddressModel.streetDirection = GetObjectValue(address.StreetDirection);
            rAddressModel.streetName = GetObjectValue(address.StreetName);
            rAddressModel.streetSuffix = GetObjectValue(address.StreetSuffix);
            rAddressModel.city = GetObjectValue(address.City);
            rAddressModel.state = GetObjectValue(address.State);

            return rAddressModel;
        }

        #endregion

        #region Locate information
        /// <summary>
        /// get Single LocateInfo By CapIds 
        /// </summary>
        /// <param name="capIds">cap Ids</param>
        /// <returns>Single LocateInfo</returns>
        [NonAction]
        private string GetSingleLocateInfo(string capIds)
        {
            string result = string.Empty;

            try
            {
                CapIDModel[] modelArray = new CapIDModel[]
                {
                    new CapIDModel()
                    {
                        ID1 = capIds.Split('-')[0],
                        ID2 = capIds.Split('-')[1],
                        ID3 = capIds.Split('-')[2],
                        serviceProviderCode = ConfigManager.AgencyCode
                    }
                };

                GISWebServiceService gis = WSFactory.Instance.GetWebService<GISWebServiceService>();
                SimpleCapModel[] capModels = gis.getGisSimpleCapModelByCapIDs(
                    ConfigManager.AgencyCode,
                    modelArray,
                    AppSession.User.PublicUserId);

                if (capModels != null && capModels.Length > 0)
                {
                    SimpleCapModel cap = capModels[0];
                    string altId = GetRealValue(cap.altID);
                    string capId = cap.capID.ID1 + "-" + cap.capID.ID2 + "-" + cap.capID.ID3;
                    string id = "\"id\":\"" + altId + "," + capId + "\"";

                    result = id + "," + GetSystem() + "," + GetGisObject(cap) + "," + GetAddress(cap) + "," + GetParcelInfo(cap);
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// get system information
        /// </summary>
        /// <returns>System Result</returns>
        [NonAction]
        private string GetSystem()
        {
            SystemEntity sys = new SystemEntity();
            sys.ServiceProviderCode = ConfigManager.AgencyCode;
            //// not necessary
            sys.MapServiceId = string.Empty;
            string system = "\"system\":" + Newtonsoft.Json.JsonConvert.SerializeObject(sys);

            return system;
        }

        /// <summary>
        /// get GIS information
        /// </summary>
        /// <param name="myCapModel">Cap Model</param>
        /// <returns>Gis Object</returns>
        [NonAction]
        private string GetGisObject(SimpleCapModel myCapModel)
        {
            StringBuilder sbGisObjects = new StringBuilder();
            GISObjectModel[] gisObjects = myCapModel.gisObjects;
            sbGisObjects.Append("\"record_by_GISObject\": [");

            if (gisObjects != null && gisObjects.Count() > 0)
            {
                foreach (var item in gisObjects)
                {
                    sbGisObjects.Append("{");

                    sbGisObjects.Append("\"Layer\": \"" + GetRealValue(item.layerId) + "\",");
                    sbGisObjects.Append("\"GISID\": \"" + GetRealValue(item.gisId) + "\",");
                    sbGisObjects.Append("\"contextType\": \"" + "CAP" + "\",");

                    sbGisObjects.Append("\"record\": {");
                    sbGisObjects.Append("\"object\":{");

                    string capid1 = string.Empty;
                    string capid2 = string.Empty;
                    string capid3 = string.Empty;
                    string customID = string.Empty;

                    if (item.capID != null)
                    {
                        capid1 = GetRealValue(item.capID.ID1);
                        capid2 = GetRealValue(item.capID.ID2);
                        capid3 = GetRealValue(item.capID.ID3);
                        customID = GetRealValue(item.capID.customID);
                    }

                    sbGisObjects.Append("\"key1\": \"" + capid1 + "\",");
                    sbGisObjects.Append("\"key2\": \"" + capid2 + "\",");
                    sbGisObjects.Append("\"key3\": \"" + capid3 + "\"");
                    sbGisObjects.Append("},");
                    sbGisObjects.Append("\"IdentifierDisplay\": \"" + customID + "\"");
                    sbGisObjects.Append("}");

                    sbGisObjects.Append("},");
                }

                sbGisObjects = GetRealString(sbGisObjects);
            }

            sbGisObjects.Append("]");

            return sbGisObjects.ToString();
        }

        /// <summary>
        /// get addressModel information
        /// </summary>
        /// <param name="myCapModel">Cap Model</param>
        /// <returns>Get Address</returns>
        [NonAction]
        private string GetAddress(SimpleCapModel myCapModel)
        {
            StringBuilder sbAdressObjects = new StringBuilder();
            AddressModel[] addressModels = myCapModel.addressModels;

            if (addressModels == null && myCapModel.addressModel != null)
            {
                addressModels = new AddressModel[] { myCapModel.addressModel };
            }

            sbAdressObjects.Append("\"record_by_address\": [");

            if (addressModels != null && addressModels.Count() > 0)
            {
                foreach (var item in addressModels)
                {
                    sbAdressObjects.Append("{");

                    sbAdressObjects.Append("\"address\": {");
                    sbAdressObjects.Append("\"Address\": \"" + GetRealValue(item.displayAddress) + "\",");
                    sbAdressObjects.Append("\"Neighborhood\": \"" + GetRealValue(item.neighborhood) + "\",");
                    sbAdressObjects.Append("\"City\": \"" + GetRealValue(item.city) + "\",");
                    sbAdressObjects.Append("\"StreetName\":\"" + GetRealValue(item.streetName) + "\",");
                    sbAdressObjects.Append("\"Postal\": \"" + GetRealValue(item.zip) + "\",");
                    sbAdressObjects.Append("\"StreetNumber\":\"" + GetRealValue(item.houseNumberStart) + "\",");
                    sbAdressObjects.Append("\"CountryCode\": \"" + GetRealValue(item.countryCode) + "\",");
                    sbAdressObjects.Append("\"StreetSuffix\":\"" + GetRealValue(item.streetSuffix) + "\",");
                    sbAdressObjects.Append("\"Direction\": \"" + GetRealValue(item.streetDirection) + "\"");
                    sbAdressObjects.Append("},");

                    sbAdressObjects.Append("\"record\": {");
                    sbAdressObjects.Append("\"object\":{");

                    string capid1 = string.Empty;
                    string capid2 = string.Empty;
                    string capid3 = string.Empty;
                    string customID = string.Empty;

                    if (item.capID != null)
                    {
                        capid1 = GetRealValue(item.capID.ID1);
                        capid2 = GetRealValue(item.capID.ID2);
                        capid3 = GetRealValue(item.capID.ID3);
                        customID = GetRealValue(item.capID.customID);
                    }

                    sbAdressObjects.Append("\"key1\": \"" + capid1 + "\",");
                    sbAdressObjects.Append("\"key2\": \"" + capid2 + "\",");
                    sbAdressObjects.Append("\"key3\": \"" + capid3 + "\"");
                    sbAdressObjects.Append("},");
                    sbAdressObjects.Append("\"IdentifierDisplay\": \"" + customID + "\"");
                    sbAdressObjects.Append("},");

                    sbAdressObjects.Append("\"contextType\": \"" + "CAP" + "\"");

                    sbAdressObjects.Append("},");
                }

                sbAdressObjects = GetRealString(sbAdressObjects);
            }

            sbAdressObjects.Append("]");

            return sbAdressObjects.ToString();
        }

        /// <summary>
        /// get parcel information
        /// </summary>
        /// <param name="myCapModel">my Cap Model</param>
        /// <returns>Parcel Info</returns>
        [NonAction]
        private string GetParcelInfo(SimpleCapModel myCapModel)
        {
            StringBuilder sbParcelObjects = new StringBuilder();
            StringBuilder capParcelObjects = new StringBuilder();
            StringBuilder capbParcelAdress = new StringBuilder();
            ParcelInfoModel[] parcelModels = myCapModel.parcelModels;

            sbParcelObjects.Append("\"record_by_pacerl\": [");
            capParcelObjects.Append("\"CAP_Parcel_Item\": [");
            capbParcelAdress.Append("\"CAP_Parcel_Item_address\": [");

            if (parcelModels != null && parcelModels.Count() > 0)
            {
                foreach (var item in parcelModels)
                {
                    string singlePacelJson = GetRecordByPacerlJson(item).ToString();
                    string singleCapJson = GetRecordByPacerlJson(item).ToString();
                    string singleAdressJson = GetRecordByPacerlJson(item).ToString();

                    if (!string.IsNullOrEmpty(singlePacelJson))
                    {
                        sbParcelObjects.Append(singlePacelJson + ",");
                    }

                    if (!string.IsNullOrEmpty(singleCapJson))
                    {
                        capParcelObjects.Append(singleCapJson + ",");
                    }

                    if (!string.IsNullOrEmpty(singlePacelJson))
                    {
                        capbParcelAdress.Append(singleAdressJson + ",");
                    }
                }
            }

            capbParcelAdress = GetRealString(capbParcelAdress);
            capParcelObjects = GetRealString(capParcelObjects);
            sbParcelObjects = GetRealString(sbParcelObjects);

            capbParcelAdress.Append("]");
            capParcelObjects.Append("]");
            sbParcelObjects.Append("]");

            return sbParcelObjects.ToString() + "," + capParcelObjects.ToString() + "," + capbParcelAdress.ToString();
        }

        /// <summary>
        /// Get record by parcel
        /// </summary>
        /// <param name="item">parcel INFO model</param>
        /// <returns>Record by Parcel</returns>
        [NonAction]
        private StringBuilder GetRecordByPacerlJson(ParcelInfoModel item)
        {
            StringBuilder sbParcelObjects = new StringBuilder();

            if (item.parcelModel.gisObjectList == null)
            {
                return sbParcelObjects;
            }

            //record_by_pacerl
            foreach (var gisItem in item.parcelModel.gisObjectList)
            {
                sbParcelObjects.Append("{");
                sbParcelObjects.Append("\"Layer\": \"" + GetRealValue(gisItem.layerId) + "\",");
                sbParcelObjects.Append("\"GISID\": \"" + GetRealValue(gisItem.gisId) + "\",");
                sbParcelObjects.Append("\"contextType\": \"" + "CAP-Parcel" + "\",");
                sbParcelObjects.Append("\"record\": {");
                sbParcelObjects.Append("\"object\":{");

                string capid1 = string.Empty;
                string capid2 = string.Empty;
                string capid3 = string.Empty;

                if (gisItem.capID != null)
                {
                    capid1 = GetRealValue(gisItem.capID.ID1);
                    capid2 = GetRealValue(gisItem.capID.ID2);
                    capid3 = GetRealValue(gisItem.capID.ID3);
                }

                sbParcelObjects.Append("\"key1\": \"" + capid1 + "\",");
                sbParcelObjects.Append("\"key2\": \"" + capid2 + "\",");
                sbParcelObjects.Append("\"key3\": \"" + capid3 + "\"");

                sbParcelObjects.Append("},");
                sbParcelObjects.Append("\"IdentifierDisplay\": \"" + GetRealValue(gisItem.gisId) + "\"");
                sbParcelObjects.Append("}");
                sbParcelObjects.Append("},");
            }

            sbParcelObjects = GetRealString(sbParcelObjects);

            return sbParcelObjects;
        }

        /// <summary>
        /// Get CAP Parcel
        /// </summary>
        /// <param name="item">Parcel INFO</param>
        /// <returns>CAP Parcel</returns>
        [NonAction]
        private StringBuilder GetCAPParcelItemJson(ParcelInfoModel item)
        {
            StringBuilder capParcelObjects = new StringBuilder();

            if (item.parcelModel.gisObjectList == null)
            {
                return capParcelObjects;
            }

            //CAP_Parcel_Item
            foreach (var parcelItem in item.parcelModel.gisObjectList)
            {
                capParcelObjects.Append("{");
                capParcelObjects.Append("\"Layer\": \"" + GetRealValue(parcelItem.layerId) + "\",");
                capParcelObjects.Append("\"GISID\": \"" + GetRealValue(parcelItem.gisId) + "\",");
                capParcelObjects.Append("\"contextType\": \"" + "CAP-Parcel" + "\",");
                capParcelObjects.Append("\"pacerlKey\": \"" + GetRealValue(parcelItem.gisId) + "\",");
                capParcelObjects.Append("\"IdentifierDisplay\": \"" + GetRealValue(parcelItem.gisId) + "\"");
                capParcelObjects.Append("},");
            }

            capParcelObjects = GetRealString(capParcelObjects);

            return capParcelObjects;
        }

        /// <summary>
        /// Get CAP_Parcel_Item_address
        /// </summary>
        /// <param name="item">Parcel INFO</param>
        /// <returns>CAP Parcel address</returns>
        [NonAction]
        private StringBuilder GetCAPParcelItemaddressJson(ParcelInfoModel item)
        {
            StringBuilder capbParcelAdress = new StringBuilder();

            if (item.parcelModel.gisObjectList == null)
            {
                return capbParcelAdress;
            }

            // CAP_Parcel_Item_address
            foreach (var parcelItem in item.parcelModel.gisObjectList)
            {
                capbParcelAdress.Append("{");
                capbParcelAdress.Append("\"addresses\": [");

                foreach (var addressItem in item.addressLists)
                {
                    capbParcelAdress.Append("{");
                    capbParcelAdress.Append("\"address\": {");
                    capbParcelAdress.Append("\"Address\": \"" + GetRealValue(addressItem.fullAddress) + "\",");
                    capbParcelAdress.Append("\"Neighborhood\": \"" + GetRealValue(addressItem.neighborhood) + "\",");
                    capbParcelAdress.Append("\"City\": \"" + GetRealValue(addressItem.city) + "\",");
                    capbParcelAdress.Append("\"StreetName\":\"" + GetRealValue(addressItem.streetName) + "\",");
                    capbParcelAdress.Append("\"Postal\": \"" + GetRealValue(addressItem.zip) + "\",");
                    capbParcelAdress.Append("\"StreetNumber\":\"" + GetRealValue(addressItem.streetPrefix) + "\",");
                    capbParcelAdress.Append("\"CountryCode\": \"" + GetRealValue(addressItem.countryCode) + "\",");
                    capbParcelAdress.Append("\"StreetSuffix\":\"" + GetRealValue(addressItem.streetSuffix) + "\",");
                    capbParcelAdress.Append("\"Direction\": \"" + GetRealValue(addressItem.streetDirection) + "\"");
                    capbParcelAdress.Append("}");
                    capbParcelAdress.Append("},");
                }

                capbParcelAdress = GetRealString(capbParcelAdress);
                capbParcelAdress.Append("],");

                capbParcelAdress.Append("\"GISID\": \"" + GetRealValue(parcelItem.gisId) + "\",");
                capbParcelAdress.Append("\"IdentifierDisplay\": \"" + GetRealValue(parcelItem.gisId) + "\",");
                capbParcelAdress.Append("\"contextType\": \"" + "CAP-Parcel-Item" + "\"");
                capbParcelAdress.Append("}");
            }

            capbParcelAdress = GetRealString(capbParcelAdress);

            return capbParcelAdress;
        }

        /// <summary>
        /// delete the "," when the StringBuilder is end with ","
        /// </summary>
        /// <param name="result">string result</param>
        /// <returns>Real String</returns>
        [NonAction]
        private StringBuilder GetRealString(StringBuilder result)
        {
            if (!string.IsNullOrEmpty(result.ToString()) && result.Length > 1 && result.ToString().EndsWith(","))
            {
                result.Length -= 1;
            }

            return result;
        }

        /// <summary>
        /// get real value
        /// </summary>
        /// <param name="value">Object Value</param>
        /// <returns>Real Value</returns>
        [NonAction]
        private string GetRealValue(object value)
        {
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }

                return string.IsNullOrEmpty(value.ToString()) ? string.Empty : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// get object value
        /// </summary>
        /// <param name="value">Object Value</param>
        /// <returns>Real Value</returns>
        [NonAction]
        private object GetObjectValue(object value)
        {
            try
            {
                if (value == null)
                {
                    return null;
                }

                return string.IsNullOrEmpty(value.ToString()) ? string.Empty : value.ToString();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Search result locate
        /// <summary>
        /// get single cap json information
        /// </summary>
        /// <param name="number">cap number</param>
        /// <returns>Single Cap Json</returns>
        [NonAction]
        private string GetSingleCapJson(string number)
        {
            StringBuilder sb = new StringBuilder();
            SimpleCapModel capModel = GetSingleCapModel(number);
            string permitType = CAPHelper.GetAliasOrCapTypeLabel(capModel);
            string permitNumber = capModel.altID != null ? capModel.altID : string.Empty;
            string address = GetPermitAddress(capModel);
            string agencyCode = capModel.capID.serviceProviderCode ?? ConfigManager.AgencyCode;
            string status = I18nStringUtil.GetString(capModel.resCapStatus, capModel.capStatus);

            //whether or not shown Add to cart button
            bool isShowAddToCart = !AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart();

            // check whether the cap can be used for pay fee
            bool canPayFeeDue = false;
            bool canPayFeeDue4Renew = false;
            bool isPartialCap = CapUtil.IsPartialCap(capModel.capClass);
            bool hasNopaidFee = capModel.noPaidFeeFlag;
            string renewalStatus = capModel.renewalStatus ?? string.Empty;
            bool hasPrivilegeToHandleCap = capModel.hasPrivilegeToHandleCap;

            string defaultViewUrl = ApiUtil.ConstructRecordDetailUrl(
                    capModel.moduleName,
                    capModel.capID.ID1,
                    capModel.capID.ID2,
                    capModel.capID.ID3,
                    agencyCode);

            bool displayInspcetion = ActionButtonController.DisplayInspcetion(TempModelConvert.Add4WSForCapIDModel(capModel.capID), capModel.moduleName);

            if (!isPartialCap && !StandardChoiceUtil.IsRemovePayFee(ConfigManager.AgencyCode))
            {
                if (hasNopaidFee && FunctionTable.IsEnableMakePayment())
                {
                    canPayFeeDue = true;
                }
                else if (FunctionTable.IsEnableRenewRecord() &&
                         ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCulture))
                {
                    canPayFeeDue4Renew = true;
                }
            }

            string payfeesUrl = string.Empty;
            if (canPayFeeDue)
            {
                payfeesUrl = ApiUtil.ConstructPayFeesDeepLink(
                    capModel.moduleName,
                    capModel.capID.ID1,
                    capModel.capID.ID2,
                    capModel.capID.ID3,
                    agencyCode);
            }
            else if (canPayFeeDue4Renew)
            {
                payfeesUrl = ApiUtil.ConstructPayFeeDue4RenewDeepLink(
                    capModel.moduleName,
                    capModel.capID.ID1,
                    capModel.capID.ID2,
                    capModel.capID.ID3,
                    agencyCode);
            }

            //resume link
            string resumeUrl = string.Empty;

            if (hasPrivilegeToHandleCap && isPartialCap)
            {
                resumeUrl = ApiUtil.ConstructResumeDeepLink(
                    capModel.moduleName,
                    capModel.capID.ID1,
                    capModel.capID.ID2,
                    capModel.capID.ID3,
                    agencyCode,
                    capModel.capClass,
                    ApiUtil.GetFilterNameForResume(capModel.capType, capModel.moduleName));
            }

            // Display\hide clone record button
            bool isShowCopyRecord = false;

            if (CloneRecordUtil.IsDisplayCloneButton(capModel.capType, capModel.capID, capModel.moduleName, true)
                && FunctionTable.IsEnableCloneRecord())
            {
                isShowCopyRecord = true;
            }

            bool isShowUpload = !isPartialCap && ApiUtil.IsShowUploadButtonInDetailPage(capModel.moduleName, TempModelConvert.Add4WSForCapIDModel(capModel.capID));

            // to show ReportButton or not.   
            CapIDModel4WS capIDModel = new CapIDModel4WS
            {
                id1 = capModel.capID.ID1,
                id2 = capModel.capID.ID2,
                id3 = capModel.capID.ID3,
                serviceProviderCode = capModel.capID.serviceProviderCode
            };
            CustomCapView4Ui customCapView4Ui = NewUiReportUtil.DisplayReportButton(capIDModel, capModel.moduleName);

            sb.Append("{");
            sb.Append("\"created\":\"" + capModel.auditDate + "\",");
            sb.Append("\"expDate\":\"" + capModel.expDate + "\",");
            sb.Append("\"permitType\":\"" + permitType + "\",");
            sb.Append("\"permitNumber\":\"" + permitNumber + "\",");
            sb.Append("\"capID\":\"" + (capModel.capID != null ? (capModel.capID.ID1 + ACAConstant.SPLIT_CHAR4 + capModel.capID.ID2 + ACAConstant.SPLIT_CHAR4 + capModel.capID.ID3) : string.Empty) + "\",");
            sb.Append("\"capClass\":\"" + capModel.capClass + "\",");
            sb.Append("\"hasNoPaidFees\":\"" + capModel.noPaidFeeFlag + "\",");
            sb.Append("\"renewalStatus\":\"" + capModel.renewalStatus + "\",");
            sb.Append("\"defaultViewUrl\":\"" + defaultViewUrl + "\",");
            sb.Append("\"displayInspcetion\":\"" + displayInspcetion + "\",");
            sb.Append("\"isShowAddToCart\":\"" + isShowAddToCart + "\",");
            sb.Append("\"isShowCopyRecord\":\"" + isShowCopyRecord + "\",");
            sb.Append("\"payfeesUrl\":\"" + payfeesUrl + "\",");
            sb.Append("\"resumeUrl\":\"" + resumeUrl + "\",");
            sb.Append("\"agencyCode\":\"" + agencyCode + "\",");
            sb.Append("\"moduleName\":\"" + capModel.moduleName + "\",");
            sb.Append("\"status\":\"" + status + "\",");
            sb.Append("\"Address\":\"" + address + "\",");
            sb.Append("\"IsShowPrintPermit\":\"" + customCapView4Ui.IsShowPrintPermit + "\",");
            sb.Append("\"PrintPermitReportId\":\"" + customCapView4Ui.PrintPermitReportId + "\",");
            sb.Append("\"PrintPermitReportName\":\"" + customCapView4Ui.PrintPermitReportName + "\",");
            sb.Append("\"PrintPermitReportType\":\"" + customCapView4Ui.PrintPermitReportType + "\",");
            sb.Append("\"IsShowPrintSummary\":\"" + customCapView4Ui.IsShowPrintSummary + "\",");
            sb.Append("\"PrintSummaryReportId\":\"" + customCapView4Ui.PrintSummaryReportId + "\",");
            sb.Append("\"PrintSummaryReportName\":\"" + customCapView4Ui.PrintSummaryReportName + "\",");
            sb.Append("\"PrintSummaryReportType\":\"" + customCapView4Ui.PrintSummaryReportType + "\",");
            sb.Append("\"IsShowPrintReceipt\":\"" + customCapView4Ui.IsShowPrintReceipt + "\",");
            sb.Append("\"PrintReceiptReportId\":\"" + customCapView4Ui.PrintReceiptReportId + "\",");
            sb.Append("\"PrintReceiptReportName\":\"" + customCapView4Ui.PrintReceiptReportName + "\",");
            sb.Append("\"PrintReceiptReportType\":\"" + customCapView4Ui.PrintReceiptReportType + "\",");

            sb.Append("\"isShowUpload\":\"" + isShowUpload + "\"");
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Get APO By GIS
        /// </summary>
        /// <param name="currentPageIndex">current Page Index</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="gISModelList">GIS Model List</param>
        /// <returns>APO By GIS</returns>
        [NonAction]
        private string GetAPOByGIS(int currentPageIndex, string sortExpression, List<GISObjectModel> gISModelList)
        {
            StringBuilder sb = new StringBuilder();
            ParcelModel parcelmodel = new ParcelModel();
            parcelmodel.gisObjectList = gISModelList.ToArray();
            PaginationModel pageInfo = new PaginationModel();
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = 100;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
            SearchResultModel result = apoBll.GetAPOListByParcel(ConfigManager.AgencyCode, parcelmodel, queryFormat, false);
            pageInfo.StartDBRow = result.startRow;
            ParcelInfoModel[] parcelsModels = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
            sb.Append("\"Parcel\":[");

            if (parcelsModels != null && parcelsModels.Length > 0)
            {
                foreach (var model in parcelsModels)
                {
                    sb.Append("{");

                    if (model.RAddressModel != null)
                    {
                        RefAddressModel addressModel = model.RAddressModel;
                        var addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                        AddressFormatType addressFormatType = AddressFormatType.SHORT_ADDRESS_NO_FORMAT;
                        string address = addressBuilderBll.BuildAddressByFormatType(addressModel, null, addressFormatType);

                        sb.Append("\"Address\":\"" + GetRealValue(address) + "\",");
                    }

                    if (model.parcelModel != null)
                    {
                        ParcelModel parcelModel = model.parcelModel;
                        sb.Append("\"ParcelUID\":\"" + GetRealValue(parcelModel.UID) + "\",");
                        sb.Append("\"ParcelNumber\":\"" + GetRealValue(parcelModel.parcelNumber) + "\",");
                        sb.Append("\"sourceSeqNumber\":\"" + GetRealValue(parcelModel.sourceSeqNumber) + "\",");
                    }

                    if (model.ownerModel != null)
                    {
                        OwnerModel ownerModel = model.ownerModel;
                        sb.Append("\"Owner\":\"" + Microsoft.JScript.GlobalObject.escape(GetRealValue(ownerModel.ownerFullName)) + "\"");
                    }

                    sb = GetRealString(sb);
                    sb.Append("},");
                }

                sb = GetRealString(sb);
            }

            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Get Cap By GIS
        /// </summary>
        /// <param name="currentPageIndex">current Page Index</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="gISModelList">GIS Model List</param>
        /// <param name="module">module name</param>
        /// <returns>Cap By GIS</returns>
        [NonAction]
        private string GetCapByGIS(int currentPageIndex, string sortExpression, List<GISObjectModel> gISModelList, string module = "")
        {
            StringBuilder sb = new StringBuilder();
            CapModel4WS capmodel = new CapModel4WS();
            capmodel.moduleName = string.Empty;
            capmodel.gisObjects = gISModelList.ToArray();
            PaginationModel pageInfo = new PaginationModel();
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = 100;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            List<string> moduleNames = null;
            if (string.IsNullOrEmpty(module))
            {
                Dictionary<string, string> allModules = TabUtil.GetAllEnableModules(false);
                if (allModules != null && allModules.Count > 0)
                {
                    moduleNames = allModules.Keys.ToList();
                }
            }
            else
            {
                moduleNames = new List<string>();
                moduleNames.Add(module);
                if (CapUtil.EnableCrossModuleSearch())
                {
                    IList<string> crossModules = TabUtil.GetCrossModules(module);
                    if (crossModules.Count > 0)
                    {
                        moduleNames.AddRange(crossModules.ToArray());
                    }
                }
            }

            /*
             * Here are two request to search record:
             * 1. Search records by GIS object.
             * 2. Search records by parcel object.
             * 3. Merge records list.
             */

            //Search record by GIS object.
            ICapBll capBll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
            SearchResultModel searchGisResult = capBll.GetCapList4ACA(capmodel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNames, false, null);
            SimpleCapModel[] simpleCaps = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchGisResult.resultList);
            List<SimpleCapModel> list = new List<SimpleCapModel>();

            if (simpleCaps != null && simpleCaps.Length > 0)
            {
                list.AddRange(simpleCaps);
            }

            //Search record by parcel object.
            capmodel = new CapModel4WS();
            capmodel.moduleName = module;
            capmodel.parcelModel = new CapParcelModel();
            capmodel.parcelModel.parcelModel = new ParcelModel();
            capmodel.parcelModel.parcelModel.gisObjectList = gISModelList.ToArray();
            SearchResultModel searchResult = capBll.GetCapList4ACA(capmodel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNames, false, null);
            SimpleCapModel[] simpleCaps1 = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchResult.resultList);

            if (simpleCaps1 != null && simpleCaps1.Length > 0)
            {
                foreach (SimpleCapModel item in simpleCaps1)
                {
                    if (!IsContains(item, list))
                    {
                        list.Add(item);
                    }
                }
            }

            // Sort record list by creation date DESC.
            list.Sort(new Comparison<SimpleCapModel>(this.CompareSimpleCapModel));

            SimpleCapModel[] caplist = list.ToArray();

            sb.Append("\"Record\":[");

            if (caplist != null && caplist.Length > 0)
            {
                foreach (var item in caplist)
                {
                    string permitType = CAPHelper.GetAliasOrCapTypeLabel(item);
                    string permitNumber = item.altID != null ? item.altID : string.Empty;
                    string status = I18nStringUtil.GetString(item.resCapStatus, item.capStatus);
                    string address = GetPermitAddress(item);
                    string capId = item.capID.ID1 + "-" + item.capID.ID2 + "-" + item.capID.ID3;

                    sb.Append("{");
                    sb.Append("\"permitType\":\"" + permitType + "\",");
                    sb.Append("\"moduleName\":\"" + item.moduleName + "\",");
                    sb.Append("\"permitNumber\":\"" + permitNumber + "\",");
                    sb.Append("\"capClass\":\"" + item.capClass + "\",");
                    sb.Append("\"capId\":\"" + capId + "\",");
                    sb.Append("\"status\":\"" + status + "\",");
                    sb.Append("\"Address\":\"" + address + "\"");
                    sb.Append("},");
                }

                sb = GetRealString(sb);
            }

            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// get GISObjectModel
        /// </summary>
        /// <param name="gisId">GIS ID</param>
        /// <param name="layerId">layer ID</param>
        /// <param name="serviceId">service ID</param>
        /// <returns>GIS Model List</returns>
        [NonAction]
        private List<GISObjectModel> GetGISModelList(string gisId, string layerId, string serviceId)
        {
            GISObjectModel gisObject = new GISObjectModel();
            List<GISObjectModel> gisObjects = new List<GISObjectModel>();
            gisObject.gisId = gisId;
            gisObject.layerId = layerId;
            gisObject.serviceID = serviceId;
            gisObjects.Add(gisObject);

            return gisObjects;
        }

        /// <summary>
        /// contains an simple cap model.
        /// </summary>
        /// <param name="capModel">SimpleCapModel Object.</param>
        /// <param name="list">SimpleCapModel array</param>
        /// <returns>return true if it contains.</returns>
        [NonAction]
        private bool IsContains(SimpleCapModel capModel, List<SimpleCapModel> list)
        {
            if (capModel == null || list == null)
            {
                return false;
            }

            bool isExist = false;

            foreach (SimpleCapModel item in list)
            {
                if (string.Equals(item.altID, capModel.altID))
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        /// <summary>
        /// Compares the simple cap model.
        /// </summary>
        /// <param name="x">First compare record object.</param>
        /// <param name="y">Second compare record object.</param>
        /// <returns>
        /// A signed number indicating the relative values of x and y. Value Type Condition
        ///     Less than zero x is earlier than y. Zero x is the same as y. Greater
        ///     than zero x is later than y.
        /// </returns>
        [NonAction]
        private int CompareSimpleCapModel(SimpleCapModel x, SimpleCapModel y)
        {
            DateTime? fileDateX = x == null ? DateTime.MinValue : x.fileDate;
            DateTime? fileDateY = y == null ? DateTime.MinValue : y.fileDate;

            if (fileDateX != null && fileDateY != null)
            {
                return DateTime.Compare(fileDateY.Value, fileDateX.Value);
            }
            else if (fileDateX == null && fileDateY != null)
            {
                return 1;
            }
            else if (fileDateX != null && fileDateY == null)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// get ParcelInfoModel by address
        /// </summary>
        /// <param name="addressNumber">address Number</param>
        /// <returns>Single Address</returns>
        [NonAction]
        private string GetSingleAddress(string addressNumber)
        {
            string addressSourceNumber = addressNumber.Split('&')[0];
            string addressDescription = addressNumber.Split('&')[1];

            if (string.IsNullOrEmpty(addressSourceNumber) || string.IsNullOrEmpty(addressDescription))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            string serviceProviderCode = ConfigManager.AgencyCode;
            RefAddressModel addressModel = new RefAddressModel();

            if (!string.IsNullOrEmpty(addressSourceNumber))
            {
                addressModel.sourceNumber = Convert.ToInt32(addressSourceNumber);
            }

            if (!string.IsNullOrEmpty(addressDescription))
            {
                addressModel.addressDescription = addressDescription;
            }

            IAPOBll apoBll = (IAPOBll)ObjectFactory.GetObject(typeof(IAPOBll));
            PaginationModel pageInfo = new PaginationModel();
            pageInfo.CurrentPageIndex = 0;
            pageInfo.CustomPageSize = 1;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            SearchResultModel resultModel = apoBll.GetParcelInfoByAddress(serviceProviderCode, addressModel, queryFormat, true);
            ParcelInfoModel[] parcelInfoModel = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(resultModel.resultList);

            if (parcelInfoModel != null && parcelInfoModel.Length > 0)
            {
                foreach (var parcelInfo in parcelInfoModel)
                {
                    RefAddressModel[] refAddressModels = parcelInfo.addressLists;

                    if (refAddressModels != null && refAddressModels.Length > 0)
                    {
                        foreach (RefAddressModel item in refAddressModels)
                        {
                            sb.Append("{");
                            sb.Append("\"location\": {");
                            sb.Append("\"x\": " + item.XCoordinator + ",");
                            sb.Append("\"y\": " + item.XCoordinator + ",");
                            sb.Append("\"spatialReference\": { \"wkid\": 4326 }");
                            sb.Append("},");
                            sb.Append("\"attributes\": {");
                            sb.Append("\"Address\": \"" + GetRealValue(item.fullAddress) + "\",");
                            sb.Append("\"Neighborhood\": \"" + GetRealValue(item.neighborhood) + "\",");
                            sb.Append("\"City\": \"" + GetRealValue(item.city) + "\",");
                            sb.Append("\"StreetName\":\"" + GetRealValue(item.streetName) + "\",");
                            sb.Append("\"Postal\": \"" + GetRealValue(item.zip) + "\",");
                            sb.Append("\"StreetNumber\":\"" + GetRealValue(item.streetPrefix) + "\",");
                            sb.Append("\"CountryCode\": \"" + GetRealValue(item.countryCode) + "\",");
                            sb.Append("\"StreetSuffix\":\"" + GetRealValue(item.streetSuffix) + "\",");
                            sb.Append("\"Direction\": \"" + GetRealValue(item.streetDirection) + "\"");
                            sb.Append("}");
                            sb.Append("},");
                        }
                    }
                }
            }

            sb = GetRealString(sb);

            return sb.ToString();
        }

        /// <summary>
        /// get ParcelInfoModel by parcelNumber from session
        /// </summary>
        /// <param name="number">string number</param>
        /// <returns>Parcel Model</returns>
        [NonAction]
        private ParcelModel GetSingleParcelInfoModel(string number)
        {
            string parcelUID = number.Split('&')[0];
            string parcelNum = number.Split('&')[1];
            string sourceSeqNbr = number.Split('&')[2];
            ParcelModel parcelPK = GetParcelModel(parcelUID, parcelNum, sourceSeqNbr);

            return parcelPK;
        }

        /// <summary>
        /// get SimpleCapModel by capNumber from session
        /// </summary>
        /// <param name="capNumber">cap Number</param>
        /// <returns>Single Cap Model</returns>
        [NonAction]
        private SimpleCapModel GetSingleCapModel(string capNumber)
        {
            // set the page info and query format
            PaginationModel pageInfo = new PaginationModel();
            //// pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = 0;
            pageInfo.CustomPageSize = 1;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            // set the search criteria from the general search model
            CapModel4WS capModel = new CapModel4WS();
            capModel.altID = capNumber;

            // 2. get module name list
            List<string> moduleNameList = null;
            if (string.IsNullOrEmpty(capModel.moduleName))
            {
                Dictionary<string, string> allModules = TabUtil.GetAllEnableModules(false);
                if (allModules != null && allModules.Count > 0)
                {
                    moduleNameList = allModules.Keys.ToList();
                }
            }

            string[] hiddenViewEltNames = null;

            // search records
            ICapBll _capBll = ObjectFactory.GetObject<ICapBll>();
            SearchResultModel capResult = _capBll.GetCapList4ACA(capModel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNameList, true, hiddenViewEltNames);

            SimpleCapModel simpleCapModel = capResult.resultList[0] as SimpleCapModel;

            return simpleCapModel;
        }

        /// <summary>
        /// Gets the parcel model.
        /// </summary>
        /// <param name="parcelUID">parcel UID</param>
        /// <param name="parcelNum">parcel Number</param>
        /// <param name="sourceSeqNbr">source Sequence</param>
        /// <returns>Parcel Model</returns>
        private ParcelModel GetParcelModel(string parcelUID, string parcelNum, string sourceSeqNbr)
        {
            ParcelModel _parcelPK = null;
            ParcelModel result = null;
            IParcelBll parcelBll = (IParcelBll)ObjectFactory.GetObject(typeof(IParcelBll));
            _parcelPK = GetParcelPKModel(parcelUID, parcelNum, sourceSeqNbr);
            result = parcelBll.GetParcelByPK(ConfigManager.AgencyCode, _parcelPK);

            return result;
        }

        /// <summary>
        /// Get Parcel PK model.
        /// </summary>
        /// <param name="parcelUID">parcel UID</param>
        /// <param name="parcelNum">parcel Number</param>
        /// <param name="sourceSeqNbr">source Sequence</param>
        /// <returns>Parcel Model</returns>
        private ParcelModel GetParcelPKModel(string parcelUID, string parcelNum, string sourceSeqNbr)
        {
            ParcelModel parcelPK = new ParcelModel();
            parcelPK.UID = parcelUID;
            parcelPK.parcelNumber = parcelNum;
            parcelPK.sourceSeqNumber = StringUtil.ToLong(sourceSeqNbr);

            return parcelPK;
        }

        /// <summary>
        /// Get Parcel Locate json by parcelNumber
        /// </summary>
        /// <param name="parcelNumber">parcel Number</param>
        /// <returns>Parcel Locate INFO</returns>
        [NonAction]
        private string GetParcelLocateInfo(string parcelNumber)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"ServiceProviderCode\": \"" + ConfigManager.AgencyCode + "\",");
            sb.Append("\"language\": \"" + I18nCultureUtil.DefaultCulture + "\",");
            sb.Append("\"context\": \"\",");
            sb.Append("\"parcels\": [");
            sb.Append(" {");
            sb.Append("\"parcelNumber\": \"" + parcelNumber + "\",");
            sb.Append("\"unmaskedParcelNumber\": \"\"");
            sb.Append("}");
            sb.Append("]");
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// indicate whether address or parcel is locked.
        /// </summary>
        /// <param name="parcelInfo">ParcelInfo Model</param>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>return true if the Parcel or Address is locked, otherwise return false.</returns>
        private bool IsLock(ParcelInfoModel parcelInfo, string agencyCode)
        {
            bool isLocked = false;
            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));

            if (parcelInfo.parcelModel != null && conditionBll.IsParcelLocked(agencyCode, parcelInfo.parcelModel, AppSession.User.UserID))
            {
                isLocked = true;
            }
            else if (parcelInfo.RAddressModel != null && conditionBll.IsAddressLocked(agencyCode, parcelInfo.RAddressModel, AppSession.User.UserID))
            {
                isLocked = true;
            }
            else if (parcelInfo.ownerModel != null && conditionBll.IsOwnerLocked(agencyCode, parcelInfo.ownerModel, AppSession.User.UserID))
            {
                isLocked = true;
            }

            return isLocked;
        }

        /// <summary>
        /// Indicating whether the Cap is locked or not.
        /// </summary>
        /// <param name="capId">The cap id model.</param>
        /// <returns>Return true if the cap is locked.</returns>
        private bool IsLock(CapIDModel capId)
        {
            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
            return conditionBll.IsCapLocked(capId);
        }
        #endregion
    }
}