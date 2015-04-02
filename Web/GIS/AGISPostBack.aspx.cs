#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AGISPostBack.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AGISPostBack.aspx.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Accela.ACA.BLL;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.GIS
{
    /// <summary>
    /// the class to process data from AGIS post back
    /// </summary>
    [SuppressCsrfCheck]
    public partial class AGISPostBack : System.Web.UI.Page
    {
        /// <summary>
        /// context for parcel detail spear form
        /// </summary>
        private const string CONTEXT_PARCEL_DETAIL_SPEAR = "ParcelDetailSpear";

        /// <summary>
        /// The logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AGISPostBack));

        #region Methods

        /// <summary>
        /// Page Load method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strModel = Request["GovXMLRequest"];

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug(strModel);
                }

                if (strModel == null)
                {
                    body.Attributes.Remove("onload");
                    return;
                }

                ACAGISModel model = SerializationUtil.XmlDeserialize(strModel, typeof(ACAGISModel)) as ACAGISModel;
                if (model == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(model.Agency))
                {
                    model.Agency = ConfigManager.AgencyCode;
                }

                if (model.RefAddressModels != null && model.RefAddressModels.Length == 1)
                {
                    model.RefAddressModels[0].primaryFlag = null;
                }
                
                ProcessGISCommand(model);
            }
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

        /// <summary>
        /// process create cap command post back.
        /// </summary>
        /// <param name="model">ACAGISModel Object</param>
        private void CreateCap(ACAGISModel model)
        {
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
            ParcelInfoModel[] parcelInfos = null;

            if (model.CapIDModels != null && model.CapIDModels.Length == 1)
            {
                if (IsLock(model.CapIDModels[0]))
                {
                    this.body.Attributes.Add("onload", "ShowRecordLockMessage()");
                    return;
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

                CapWithConditionModel4WS capCondition = capBll.GetCapViewBySingle(capId, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());
                CapUtil.FillCapModelTemplateValue(capCondition.capModel);

                if (capCondition != null && capCondition.capModel != null)
                {
                    List<ParcelInfoModel> list = new List<ParcelInfoModel>();
                    ParcelInfoModel parcelInfo = new ParcelInfoModel();
                    if (capCondition.capModel.addressModel != null)
                    {
                        parcelInfo.RAddressModel = CapUtil.ConvertAddressModel2RefAddressModel(capCondition.capModel.addressModel);
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
                    this.body.Attributes.Add("onload", "ShowNoRecordLinkGISObjectMessage()");
                    return;
                }
                else if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    this.body.Attributes.Add("onload", "ShowParcelLockMessageWithCreateRecord()");
                    return;
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
                    this.body.Attributes.Add("onload", "ShowAddressLockMessageWithCreateRecord()");
                    return;
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
                    this.body.Attributes.Add("onload", "ShowDisableParcelMessage()");
                    return;
                }

                if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    this.body.Attributes.Add("onload", "ShowParcelLockMessageWithCreateRecord()");
                    return;
                }
            }

            if (parcelInfos == null || parcelInfos.Length == 0)
            {
                this.body.Attributes.Add("onload", "ShowErrorMessage()");
                return;
            }

            this.ClientScript.RegisterHiddenField("Module", model.ModuleName);
            this.ClientScript.RegisterHiddenField("AgencyCode", model.Agency);
            form1.Method = "post";
            body.Attributes.Add("onload", "CreateRecord()");

            string url = GetCreateNewRecordUrl(model.ModuleName);
            url = this.ResolveUrl(url);
            this.ClientScript.RegisterHiddenField("Url", url);
            model.ParcelInfoModels = parcelInfos;
            Session[ACAConstant.CURRENT_URL] = url;
            Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] = model;
        }

        /// <summary>
        /// Get Create New Record Url
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>The new record url.</returns>
        private string GetCreateNewRecordUrl(string moduleName)
        {
            string applylinkKey = moduleName + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_KEY_TAIL;
            List<LinkItem> links = TabUtil.GetCreationLinkItemList(moduleName, false);
            LinkItem link = null;

            // if exist apply by location, get it, otherwise use apply by select service.
            if (links.Exists(l => string.Equals(applylinkKey, l.Key, StringComparison.InvariantCultureIgnoreCase)))
            {
                link = links.Find(l => string.Equals(applylinkKey, l.Key, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                link = links[0];
            }

            ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, moduleName, link.Label);
            string url = TabUtil.RebuildUrl(link.Url, moduleName, filterName);
            url += "&From=GIS";
            if (url.StartsWith("/"))
            {
                url = "~" + url;
            }

            return url;
        }
       
        /// <summary>
        /// Get ParcelInfoModel list by parcel Id.
        /// </summary>
        /// <param name="parcel">ParcelModel object</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>return ParcelInfoModel array</returns>
        private DataTable GetAPOByParcelID(ParcelModel parcel, string agencyCode)
        {
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
            SearchResultModel result = apoBll.GetAPOListByParcel(agencyCode, parcel, null, false);
            DataTable dt = APOUtil.BuildAPODataTable(result.resultList);

            return dt;
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
        /// Process GIS command from the Map.
        /// </summary>
        /// <param name="model">ACAGISModel Object from the Map</param>
        private void ProcessGISCommand(ACAGISModel model)
        {
            switch (model.CommandName.ToUpper())
            {
                case ACAConstant.AGIS_COMMAND_SHOW_ACCELA_RECORD:
                    ShowRecord(model);
                    break;

                case ACAConstant.AGIS_COMMAND_CREATE_CAP:
                    CreateCap(model);
                    break;

                case ACAConstant.AGIS_COMMAND_SEND_FEATURES:
                    if ((model.GisObjects != null && model.GisObjects.Length == 1) || (model.ParcelModels != null && model.ParcelModels.Length == 1))
                    {
                        if (model.Context.EndsWith(CONTEXT_PARCEL_DETAIL_SPEAR, StringComparison.InvariantCultureIgnoreCase))
                        {
                            FillParcel(model);
                        }
                        else
                        {
                            FillAddress(model);
                        }
                    }
                    
                    break;
                case ACAConstant.AGIS_COMMAND_SEND_ADDRESS:
                    if (model.RefAddressModels != null && model.RefAddressModels.Length == 1)
                    {
                        FillAddress(model);
                    }

                    break;

                case ACAConstant.AGIS_COMMAND_SCHEDULE_INSPECTION:
                    //process Create Inspection command.navigate to cap detail page.
                    ScheduleInspection(model);
                    break;
                case ACAConstant.AGIS_COMMAND_RESUME:
                    ResumeApplication(model);
                    break;
                case ACAConstant.AGIS_COMMAND_SHOW_GEODOCUMENT:
                    ShowDocuments(model);
                    break;
                case ACAConstant.AGIS_COMMAND_SEND_ASSET:
                    SendAsset(model);
                    break;
            }
        }

        /// <summary>
        /// Show Geo documents
        /// </summary>
        /// <param name="model">ACAGISModel Object which is used by ACA Integration with GIS.</param>
        private void ShowDocuments(ACAGISModel model)
        {
            if (model.GisObjects != null && model.GisObjects.Length > 0)
            {
                string url = "ShowGeoDocuments.aspx";
                url = this.ResolveUrl(url);
                form1.Attributes.Add("action", url);
                form1.Target = "_blank";
                form1.Attributes.Add("target", "_blank");
                Session["ShowDocuments"] = model;
                body.Attributes.Add("onload", "ShowGeoDocuments()");
            }
        }

        /// <summary>
        /// Sends the asset.
        /// </summary>
        /// <param name="model">The ACAGISModel object.</param>
        private void SendAsset(ACAGISModel model)
        {
            this.ClientScript.RegisterHiddenField("xml", SerializationUtil.XmlSerialize(model));
            body.Attributes.Add("onload", "SendAsset()");
        }

        /// <summary>
        /// create url for resume application
        /// </summary>
        /// <param name="capModel">Cap model for ACA.</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>string for URL.</returns>
        private string CreateUrlForResume(CapModel4WS capModel, string moduleName)
        {
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            bool isTradeLicenseCap = capTypeBll.IsMatchTheFilter(capModel.capType, moduleName, ACAConstant.REQUEST_PARMETER_TRADE_LICENSE);

            string requestStr = "../Cap/CapEdit.aspx?permitType=resume&Module={0}&stepNumber=2&pageNumber=1&isFeeEstimator={1}";

            if (isTradeLicenseCap)
            {
                requestStr = requestStr + "&FilterName=" + ACAConstant.REQUEST_PARMETER_TRADE_LICENSE;
            }
            else
            {
                bool isTradeNameCap = capTypeBll.IsMatchTheFilter(capModel.capType, moduleName, ACAConstant.REQUEST_PARMETER_TRADE_NAME);

                if (isTradeNameCap)
                {
                    requestStr = requestStr + "&FilterName=" + ACAConstant.REQUEST_PARMETER_TRADE_NAME;
                }
            }

            return string.Format(requestStr, moduleName, capModel.capClass == ACAConstant.INCOMPLETE ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
        }

        /// <summary>
        /// Resume Application
        /// </summary>
        /// <param name="model">ACAGISModel Model</param>
        private void ResumeApplication(ACAGISModel model)
        {
            if (model.CapIDModels != null)
            {
                CapIDModel capId = model.CapIDModels[0];
                CapIDModel4WS capIdModel = new CapIDModel4WS();
                capIdModel.id1 = capId.ID1;
                capIdModel.id2 = capId.ID2;
                capIdModel.id3 = capId.ID3;
                capIdModel.serviceProviderCode = capId.serviceProviderCode;
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                bool isSuperCap = CapUtil.IsSuperCAP(capIdModel);
                CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capIdModel, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, isSuperCap);
                CapModel4WS capModel = capWithConditionModel.capModel;

                string url = CreateUrlForResume(capModel, capModel.moduleName);
                CapUtil.FillCapModelTemplateValue(capModel);

                CapUtil.SetCapInfoToAppSession(capModel, capModel.capType, capModel.moduleName);

                PageFlowGroupModel pageFlowGroup = AppSession.GetPageflowGroupFromSession();

                if (pageFlowGroup != null)
                {
                    url += "&" + UrlConstant.AgencyCode + "=" + pageFlowGroup.serviceProviderCode;
                }

                if (StandardChoiceUtil.IsSuperAgency() && !isSuperCap)
                {
                    url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
                }

                url = this.ResolveUrl(url);
                this.ClientScript.RegisterHiddenField("Url", url);
                Session[ACAConstant.CURRENT_URL] = url;
                body.Attributes.Add("onload", "ResumeApplication()");
            }
        }

        /// <summary>
        /// process scheduleInspection command post back
        /// </summary>
        /// <param name="model">ACAGISModel Object</param>
        private void ScheduleInspection(ACAGISModel model)
        {
            if (model.CapIDModels != null)
            {
                CapIDModel capId = model.CapIDModels[0];
                string urlInpsection = string.Format("../Cap/CapDetail.aspx?Module={0}&TabName={1}&capID1={2}&capID2={3}&capID3={4}&{5}={6}&Command={7}", HttpUtility.UrlEncode(model.ModuleName), model.ModuleName, capId.ID1, capId.ID2, capId.ID3, UrlConstant.AgencyCode, model.Agency, ACAConstant.AGIS_COMMAND_SCHEDULE_INSPECTION);
                urlInpsection = this.ResolveUrl(urlInpsection);
                this.ClientScript.RegisterHiddenField("Url", urlInpsection);
                Session[ACAConstant.CURRENT_URL] = urlInpsection;
                body.Attributes.Add("onload", "ScheduleInspection()");
            }
        }

        /// <summary>
        /// Fill Address Spear Form.
        /// </summary>
        /// <param name="model">ACAGISModel Object</param>
        private void FillAddress(ACAGISModel model)
        {
            if (model.RefAddressModels != null && model.RefAddressModels.Length == 1)
            {
                model.RefAddressModels[0].XCoordinator = null;
                model.RefAddressModels[0].YCoordinator = null;

                IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
                RefAddressModel refAddress = model.RefAddressModels[0];
                refAddress.auditStatus = ACAConstant.VALID_STATUS;
                SearchResultModel result = apoBll.GetParcelInfoByAddress(model.Agency, refAddress, null, true);
                ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    this.body.Attributes.Add("onload", "ShowAddressLockMessage()");
                    return;
                }
            }
            else if ((model.GisObjects != null && model.GisObjects.Length == 1) || (model.ParcelModels != null && model.ParcelModels.Length == 1))
            {
                IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
                ParcelModel parcel = new ParcelModel();
                if (model.ParcelModels != null && model.ParcelModels.Length == 1)
                {
                    parcel = model.ParcelModels[0];
                }
                else if (model.GisObjects != null && model.GisObjects.Length == 1)
                {
                    parcel.gisObjectList = model.GisObjects;
                }

                parcel.auditStatus = ACAConstant.VALID_STATUS;
                SearchResultModel result = apoBll.GetParcelInfoByParcel(model.Agency, parcel, null, true);
                ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    this.body.Attributes.Add("onload", "ShowParcelLockMessage()");
                    return;
                }
            }

            this.ClientScript.RegisterHiddenField("xml", SerializationUtil.XmlSerialize(model));
            body.Attributes.Add("onload", "SendAddress()");
        }

        /// <summary>
        /// Fill Parcel Spear Form.
        /// </summary>
        /// <param name="model">ACAGISModel Object</param>
        private void FillParcel(ACAGISModel model)
        {
            if ((model.ParcelModels != null && model.ParcelModels.Length == 1) || (model.GisObjects != null && model.GisObjects.Length == 1))
            {
                IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
                ParcelModel parcel = new ParcelModel();
                if (model.ParcelModels != null && model.ParcelModels.Length == 1)
                {
                    parcel = model.ParcelModels[0];
                }
                else if (model.GisObjects != null && model.GisObjects.Length == 1)
                {
                    parcel.gisObjectList = model.GisObjects;
                }

                parcel.auditStatus = ACAConstant.VALID_STATUS;
                SearchResultModel result = apoBll.GetParcelInfoByParcel(model.Agency, parcel, null, true);
                ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (parcelInfos != null && parcelInfos.Length == 1 && IsLock(parcelInfos[0], model.Agency))
                {
                    this.body.Attributes.Add("onload", "ShowParcelLockMessage()");
                    return;
                }

                this.ClientScript.RegisterHiddenField("xml", SerializationUtil.XmlSerialize(model));
                body.Attributes.Add("onload", "SendFeature()");
            }
        }

        /// <summary>
        /// process ShowRecord command post back
        /// </summary>
        /// <param name="model">ACAGISModel Object</param>
        private void ShowRecord(ACAGISModel model)
        {
            if (model.CapIDModels != null)
            {
                ScheduleInspection(model);
            }
            else if (model.GisObjects != null)
            {
                ShowRecordByGisObject(model);
            }
            else if (model.ParcelModels != null && model.ParcelModels.Length == 1)
            {
                //// select parcel from map
                ParcelModel parcel = model.ParcelModels[0];
                DataTable dt = GetAPOByParcelID(parcel, model.Agency);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER, row["ParcelNumber"].ToString());
                    this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE, row["ParcelSequenceNumber"].ToString());
                    this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_REFADDRESS_ID, row["AddressID"].ToString());
                    this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE, row["AddressSequenceNumber"].ToString());
                    this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_PARCEL_UID, row["ParcelUID"].ToString());
                    this.ClientScript.RegisterHiddenField("AgencyCode", model.Agency);
                    this.ClientScript.RegisterHiddenField("HideHeader", "True");
                    form1.Method = "get";
                    string urlParcelDetail = "../APO/ParcelDetail.aspx";
                    urlParcelDetail = this.ResolveUrl(urlParcelDetail);
                    form1.Attributes.Add("action", urlParcelDetail);
                    form1.Attributes.Add("target", "_blank");
                    body.Attributes.Add("onload", "ShowParcel()");
                }
            }
            else if (model.RefAddressModels != null && model.RefAddressModels.Length == 1)
            {
                RefAddressModel addressModel = model.RefAddressModels[0];
                IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
                SearchResultModel result = apoBll.GetAPOListByAddress(model.Agency, addressModel, null, false);
                DataTable dt = APOUtil.BuildAPODataTable(result.resultList);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.ClientScript.RegisterHiddenField("AddressID", row["AddressID"].ToString());
                    this.ClientScript.RegisterHiddenField("AddressSeq", row["AddressSequenceNumber"].ToString());
                    this.ClientScript.RegisterHiddenField("ParcelNum", row["ParcelNumber"].ToString());
                    this.ClientScript.RegisterHiddenField("AddressUID", row["AddressUID"].ToString());
                    this.ClientScript.RegisterHiddenField("AgencyCode", model.Agency);
                    form1.Method = "get";
                    string urlAddressDetail = "../APO/AddressDetail.aspx";
                    urlAddressDetail = this.ResolveUrl(urlAddressDetail);
                    form1.Attributes.Add("action", urlAddressDetail);
                    body.Attributes.Add("onload", "ShowAddress()");
                }
                else
                {
                    this.body.Attributes.Add("onload", "ShowErrorMessage()");
                }
            }
        }

        /// <summary>
        /// select a gis object to show record.
        /// </summary>
        /// <param name="model">The ACAGISModel</param>
        private void ShowRecordByGisObject(ACAGISModel model)
        {
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
            ParcelModel parcelmodel = new ParcelModel();
            parcelmodel.gisObjectList = model.GisObjects;
            SearchResultModel result = apoBll.GetAPOListByParcel(model.Agency, parcelmodel, null, false);
            DataTable dt = APOUtil.BuildAPODataTable(result.resultList);

            int parcelCount = 0;
            int capCount = 0;
            List<SimpleCapModel> caplist = new List<SimpleCapModel>();

            if (dt != null && dt.Rows.Count > 0)
            {
                parcelCount = dt.Rows.Count;
            }
            
            if (parcelCount <= 1)
            {
                ICapBll capBll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
                CapModel4WS capmodel = null;
                List<string> moduleNames = null;
                if (string.IsNullOrEmpty(model.ModuleName))
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
                    moduleNames.Add(model.ModuleName);
                    if (CapUtil.EnableCrossModuleSearch())
                    {
                        IList<string> crossModules = TabUtil.GetCrossModules(model.ModuleName);
                        if (crossModules.Count > 0)
                        {
                            moduleNames.AddRange(crossModules.ToArray());
                        }
                    }
                }

                if (parcelCount == 1)
                {
                    capmodel = new CapModel4WS();
                    capmodel.moduleName = model.ModuleName;
                    capmodel.parcelModel = new CapParcelModel();
                    capmodel.parcelModel.parcelModel = new ParcelModel();
                    capmodel.parcelModel.parcelModel.gisObjectList = model.GisObjects;
                    SearchResultModel searchResult = capBll.GetCapList4ACA(capmodel, null, AppSession.User.UserSeqNum, null, false, moduleNames, false, null);
                    if (searchResult != null && searchResult.resultList != null && searchResult.resultList.Length > 0)
                    {
                        capCount = searchResult.resultList.Length;
                    }
                }

                if (capCount == 0)
                {
                    capmodel = new CapModel4WS();
                    capmodel.moduleName = model.ModuleName;
                    capmodel.gisObjects = model.GisObjects;
                    SearchResultModel searchGisResult = capBll.GetCapList4ACA(capmodel, null, AppSession.User.UserSeqNum, null, false, moduleNames, false, null);

                    if (searchGisResult.resultList != null && searchGisResult.resultList.Length > 0)
                    {
                        capCount = searchGisResult.resultList.Length;
                        SimpleCapModel[] simpleCaps = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchGisResult.resultList);
                        caplist.AddRange(simpleCaps);
                    }
                }
            }

            if (parcelCount == 0 && capCount == 0)
            {
                //show message.
                this.body.Attributes.Add("onload", "ShowErrorMessage()");
            }
            else if (parcelCount == 1 && capCount == 0)
            {
                //show parcel detail.
                DataRow row = dt.Rows[0];
                this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER, row["ParcelNumber"].ToString());
                this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE, row["ParcelSequenceNumber"].ToString());
                this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_REFADDRESS_ID, row["AddressID"].ToString());
                this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE, row["AddressSequenceNumber"].ToString());
                this.ClientScript.RegisterHiddenField(ACAConstant.REQUEST_PARMETER_PARCEL_UID, row["ParcelUID"].ToString());
                this.ClientScript.RegisterHiddenField("AgencyCode", model.Agency);
                this.ClientScript.RegisterHiddenField("HideHeader", "True");
                form1.Method = "get";
                string urlParcelDetail = "../APO/ParcelDetail.aspx";
                urlParcelDetail = this.ResolveUrl(urlParcelDetail);
                form1.Attributes.Add("action", urlParcelDetail);
                form1.Attributes.Add("target", "_blank");
                body.Attributes.Add("onload", "ShowParcel()");
            }
            else if (parcelCount == 0 && capCount == 1)
            {
                //show cap detail
                CapIDModel capId = caplist[0].capID;
                string urlInpsection = string.Format("../Cap/CapDetail.aspx?Module={0}&TabName={1}&capID1={2}&capID2={3}&capID3={4}&{5}={6}", HttpUtility.UrlEncode(caplist[0].moduleName), model.ModuleName, capId.ID1, capId.ID2, capId.ID3, UrlConstant.AgencyCode, capId.serviceProviderCode);
                urlInpsection = this.ResolveUrl(urlInpsection);
                this.ClientScript.RegisterHiddenField("Url", urlInpsection);
                Session[ACAConstant.CURRENT_URL] = urlInpsection;
                body.Attributes.Add("onload", "ScheduleInspection()");
            }
            else
            {
                //show record list.
                this.ClientScript.RegisterHiddenField("GisId", model.GisObjects[0].gisId);
                this.ClientScript.RegisterHiddenField("LayerId", model.GisObjects[0].layerId);
                this.ClientScript.RegisterHiddenField("ServiceId", model.GisObjects[0].serviceID);
                this.ClientScript.RegisterHiddenField("AgencyCode", model.Agency);
                this.ClientScript.RegisterHiddenField("ModuleName", model.ModuleName);
                form1.Method = "post";
                string urlRecordList = "../GIS/RecordByGISObject.aspx";
                urlRecordList = this.ResolveUrl(urlRecordList);
                form1.Attributes.Add("action", urlRecordList);
                form1.Attributes.Add("target", "_blank");
                body.Attributes.Add("onload", "ShowRecordList()");
            }
        }

        #endregion Methods
    }
}