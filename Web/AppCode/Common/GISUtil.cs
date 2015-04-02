#region Header
/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GISUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  LabelUtil for getting label by label key.
 *  UI should call this class if need to get text in .cs.
 *
 *  Notes:
 *      $Id: GISUtil.cs 146779 2009-09-10 02:07:13Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Web.SessionState;
using System.Web.UI;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// This class
    /// </summary>
    public class GISUtil
    {
        /// <summary>
        /// Create an ACAGISModel
        /// </summary>
        /// <returns>return ACAGISModel object</returns>
        public static ACAGISModel CreateACAGISModel()
        {
            ACAGISModel model = new ACAGISModel();
            model.UserID = AppSession.User.PublicUserId;
            model.Language = I18nCultureUtil.UserPreferredCulture;
            return model;
        }

        /// <summary>
        /// set ACAGISModel's post url
        /// </summary>
        /// <param name="page">current page</param>
        /// <param name="mode">ACAGISModel object</param>
        public static void SetPostUrl(Page page, ACAGISModel mode)
        {
            if (page.Request.Url.Port == 80)
            {
                mode.PostUrl = string.Format("{0}://{1}{2}", page.Request.Url.Scheme, page.Request.Url.Host, page.ResolveUrl("../GIS/AGISPostBack.aspx"));
            }
            else
            {
                mode.PostUrl = string.Format("{0}://{1}:{2}{3}", page.Request.Url.Scheme, page.Request.Url.Host, page.Request.Url.Port, page.ResolveUrl("../GIS/AGISPostBack.aspx"));
            }
        }

        /// <summary>
        /// Remove ACAGISModel from session created when applying record from the Map control in current module. 
        /// </summary>
        /// <param name="moduleName">module name.</param>
        public static void RemoveACAGISModelFromSession(string moduleName)
        {
            HttpSessionState session = System.Web.HttpContext.Current.Session;
            if (session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] != null)
            {
                ACAGISModel model = session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] as ACAGISModel;
                if (string.Equals(model.ModuleName, moduleName))
                {
                    session.Remove(SessionConstant.SESSION_CREATE_RECORD_BY_MAP);
                }
            }
        }
        
        /// <summary>
        /// Get agency by AGISModel.
        /// </summary>
        /// <param name="model">The ACAGISModel.</param>
        /// <returns>The agency list.</returns>
        public static List<string> GetAgencies(ACAGISModel model)
        {
            List<string> agencyList = new List<string>();
            if (model.CapIDModels != null)
            {
                foreach (CapIDModel item in model.CapIDModels)
                {
                    if (!agencyList.Contains(item.serviceProviderCode))
                    {
                        agencyList.Add(item.serviceProviderCode);
                    }
                }
            }
            else
            {
                List<long> sourceNumberList = null;

                if (model.AddressInfoModels != null)
                {
                    sourceNumberList = GetSourceNumberList(model.AddressInfoModels);
                }
                else if (model.ParcelInfoModels != null)
                {
                    sourceNumberList = GetSourceNumberList(model.ParcelInfoModels);
                }
                else if (model.ParcelModels != null)
                {
                    sourceNumberList = GetSourceNumberList(model.ParcelModels);
                }
                else if (model.RefAddressModels != null)
                {
                    sourceNumberList = GetSourceNumberList(model.RefAddressModels);
                }

                IServiceProviderBll provider = ObjectFactory.GetObject(typeof(IServiceProviderBll)) as IServiceProviderBll;

                if (sourceNumberList != null)
                {
                    foreach (long sourceNumber in sourceNumberList)
                    {
                        ServiceProviderModel[] agencies = provider.GetServiceProviderBySourceSeqNumber(sourceNumber);

                        if (agencies == null)
                        {
                            continue;
                        }

                        foreach (ServiceProviderModel agency in agencies)
                        {
                            if (!agencyList.Contains(agency.serviceProviderCode))
                            {
                                agencyList.Add(agency.serviceProviderCode);
                            }
                        }
                    }
                }
            }

            return agencyList;
        }

        /// <summary>
        /// get parcel information model by ACAGISModel
        /// </summary>
        /// <param name="pageInfo">Pagination Model</param>
        /// <param name="gisModel">The ACAGISModel</param>
        /// <returns>ParcelInfoModel array</returns>
        public static ParcelInfoModel[] GetAPOListByGISModel(PaginationModel pageInfo, ACAGISModel gisModel)
        {
            IAPOBll apoBll = (IAPOBll)ObjectFactory.GetObject(typeof(IAPOBll));
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            SearchResultModel result = new SearchResultModel();

            if (gisModel.RefAddressModels != null && gisModel.RefAddressModels.Length == 1)
            {
                result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, gisModel.RefAddressModels[0], queryFormat, true);
            }
            else if (gisModel.GisObjects != null && gisModel.GisObjects.Length == 1)
            {
                ParcelModel parcelModel = new ParcelModel();
                parcelModel.gisObjectList = gisModel.GisObjects;
                parcelModel.parcelStatus = ACAConstant.VALID_STATUS;
                parcelModel.auditStatus = ACAConstant.VALID_STATUS;
                result = apoBll.GetParcelInfoByParcel(ConfigManager.AgencyCode, parcelModel, queryFormat, true);
            }
            else if (gisModel.ParcelModels != null && gisModel.ParcelModels.Length == 1)
            {
                result = apoBll.GetParcelInfoByParcel(ConfigManager.AgencyCode, gisModel.ParcelModels[0], queryFormat, true);
            }

            pageInfo.StartDBRow = result.startRow;
            ParcelInfoModel[] apoList = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

            return apoList;
        }

        /// <summary>
        /// Get address list by GIS
        /// </summary>
        /// <param name="pageInfo">A PaginationModel</param>
        /// <param name="gisModel">An ACAGISModel</param>
        /// <returns>A RefAddressModel array</returns>
        public static RefAddressModel[] GetRefAddressListByGISModel(PaginationModel pageInfo, ACAGISModel gisModel)
        {
            RefAddressModel[] refAddressList = null;
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();

            if (gisModel.RefAddressModels != null && gisModel.RefAddressModels.Length == 1)
            {
                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                SearchResultModel result = apoBll.GetRefAddressList(ConfigManager.AgencyCode, gisModel.RefAddressModels[0], queryFormat);

                pageInfo.StartDBRow = result.startRow;
                refAddressList = ObjectConvertUtil.ConvertObjectArray2EntityArray<RefAddressModel>(result.resultList);
            }
            else if (gisModel.GisObjects != null && gisModel.GisObjects.Length == 1)
            {
                ParcelModel parcelModel = new ParcelModel();
                parcelModel.gisObjectList = gisModel.GisObjects;
                parcelModel.parcelStatus = ACAConstant.VALID_STATUS;
                parcelModel.auditStatus = ACAConstant.VALID_STATUS;

                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                SearchResultModel result = apoBll.GetRefAddressListByParcel(ConfigManager.AgencyCode, parcelModel, queryFormat);
                pageInfo.StartDBRow = result.startRow;
                refAddressList = ObjectConvertUtil.ConvertObjectArray2EntityArray<RefAddressModel>(result.resultList);
            }
            else if (gisModel.ParcelModels != null && gisModel.ParcelModels.Length == 1)
            {
                refAddressList = apoBll.GetRefAddressListByParcelPK(ConfigManager.AgencyCode, gisModel.ParcelModels[0], true);
            }

            return refAddressList;
        }

        /// <summary>
        /// Get parcel list by GIS
        /// </summary>
        /// <param name="pageInfo">A PaginationModel</param>
        /// <param name="gisModel">An ACAGISModel</param>
        /// <returns>A ParcelModel array</returns>
        public static ParcelModel[] GetParcelListByGISModel(PaginationModel pageInfo, ACAGISModel gisModel)
        {
            ParcelModel parcelModel = null;
            ParcelModel[] parcelList = null;

            if (gisModel.GisObjects != null && gisModel.GisObjects.Length == 1)
            {
                parcelModel = new ParcelModel();
                parcelModel.gisObjectList = gisModel.GisObjects;
                parcelModel.parcelStatus = ACAConstant.VALID_STATUS;
                parcelModel.auditStatus = ACAConstant.VALID_STATUS;
            }
            else if (gisModel.ParcelModels != null && gisModel.ParcelModels.Length == 1)
            {
                parcelModel = gisModel.ParcelModels[0];
            }

            if (parcelModel != null)
            {
                IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                SearchResultModel result = apoBll.GetRefParcelList(ConfigManager.AgencyCode, parcelModel, queryFormat);

                pageInfo.StartDBRow = result.startRow;
                parcelList = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelModel>(result.resultList);
            }

            return parcelList;
        }

        /// <summary>
        /// Get agency list by RefAddressModel
        /// </summary>
        /// <param name="refAddressModel">The reference address model.</param>
        /// <returns>return source number array</returns>
        private static List<long> GetSourceNumberList(RefAddressModel[] refAddressModel)
        {
            List<long> sourceNumberList = new List<long>();
            if (refAddressModel == null)
            {
                return sourceNumberList;
            }

            foreach (RefAddressModel item in refAddressModel)
            {
                long sourceNumber = 0;

                if (item.sourceNumber.HasValue)
                {
                    sourceNumber = (long)item.sourceNumber.Value;
                }

                if (sourceNumber != 0 && !sourceNumberList.Contains(sourceNumber))
                {
                    sourceNumberList.Add(sourceNumber);
                }

                if (item.duplicatedAPOKeys != null && item.duplicatedAPOKeys.Length > 0)
                {
                    DuplicatedAPOKeyModel[] apoKeys = item.duplicatedAPOKeys;
                    GetSourceNumberList(ref sourceNumberList, apoKeys);
                }
            }

            return sourceNumberList;
        }

        /// <summary>
        /// Get Agency list by ParcelModel.
        /// </summary>
        /// <param name="parcelModels">The parcel model list.</param>
        /// <returns>return source number array</returns>
        private static List<long> GetSourceNumberList(ParcelModel[] parcelModels)
        {
            List<long> sourceNumberList = new List<long>();

            if (parcelModels == null)
            {
                return sourceNumberList;
            }

            foreach (ParcelModel item in parcelModels)
            {
                long sourceNumber = 0;
                
                if (item.sourceSeqNumber.HasValue)
                {
                    sourceNumber = (long)item.sourceSeqNumber.Value;
                }

                if (sourceNumber != 0 && !sourceNumberList.Contains(sourceNumber))
                {
                    sourceNumberList.Add(sourceNumber);
                }

                if (item.duplicatedAPOKeys != null && item.duplicatedAPOKeys.Length > 0)
                {
                    DuplicatedAPOKeyModel[] apoKeys = item.duplicatedAPOKeys;
                    GetSourceNumberList(ref sourceNumberList, apoKeys);
                }               
            }

            return sourceNumberList;
        }

        /// <summary>
        /// Get Agency list by APO list.
        /// </summary>
        /// <param name="parcelInfos">ParcelInfoModel array</param>
        /// <returns>return source number array</returns>
        private static List<long> GetSourceNumberList(ParcelInfoModel[] parcelInfos)
        {
            List<long> sourceNumberList = new List<long>();
            if (parcelInfos == null)
            {
                return sourceNumberList;
            }
            
            foreach (ParcelInfoModel item in parcelInfos)
            {
                long sourceNumber = 0;
                if (item.RAddressModel != null && item.RAddressModel.sourceNumber.HasValue)
                {
                    sourceNumber = (long)item.RAddressModel.sourceNumber.Value;
                }
                else if (item.parcelModel != null && item.parcelModel.sourceSeqNumber.HasValue)
                {
                    sourceNumber = (long)item.parcelModel.sourceSeqNumber.Value;
                }

                if (sourceNumber != 0 && !sourceNumberList.Contains(sourceNumber))
                {
                    sourceNumberList.Add(sourceNumber);
                }

                // do with dulicated apo 
                if (item.RAddressModel != null && item.RAddressModel.duplicatedAPOKeys != null && item.RAddressModel.duplicatedAPOKeys.Length > 0)
                {
                    DuplicatedAPOKeyModel[] apoKeys = item.RAddressModel.duplicatedAPOKeys;
                    GetSourceNumberList(ref sourceNumberList, apoKeys);
                }

                if (item.parcelModel != null && item.parcelModel.duplicatedAPOKeys != null && item.parcelModel.duplicatedAPOKeys.Length > 0)
                {
                    DuplicatedAPOKeyModel[] apoKeys = item.parcelModel.duplicatedAPOKeys;
                    GetSourceNumberList(ref sourceNumberList, apoKeys);
                }
            }

            return sourceNumberList;
        }

        /// <summary>
        /// Get sourceNumber list
        /// </summary>
        /// <param name="sourceNumberList">source number list.</param>
        /// <param name="apoKeys">DuplicatedAPOKeyModel array.</param>
        private static void GetSourceNumberList(ref List<long> sourceNumberList, DuplicatedAPOKeyModel[] apoKeys)
        {
            foreach (DuplicatedAPOKeyModel apokey in apoKeys)
            {
                if (!string.Equals(apokey.sourceNumber, ACAConstant.COMMON_ZERO) && !sourceNumberList.Contains(long.Parse(apokey.sourceNumber)))
                {
                    sourceNumberList.Add(long.Parse(apokey.sourceNumber));
                }
            }
        }
    }
}
