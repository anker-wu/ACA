#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapTypeBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CapTypeBll.cs 278415 2014-09-03 08:45:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

using log4net;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation CAP type.
    /// </summary>
    public class CapTypeBll : BaseBll, ICapTypeBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapTypeBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of CapTypeService.
        /// </summary>
        private CapTypeWebServiceService CapTypeService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypeWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of CapTypeService.
        /// </summary>
        private AppStatusGroupWebServiceService AppStatusGroupWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AppStatusGroupWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// This method is general method for getting board types, the values of the second level if CAP types.
        /// 1. firstly invoke GetCapTypeListByFilter when filter name isn't null or string.empty.
        /// 2. secondly invoke GetFilteredCapTypesByPublicUser when flag be turn on in standard choice "STD_CAT_ACA_FILTER_CAP_BY_LICENSE".
        /// 3. third invoke default getting cap type list method GetCapTypeListByModule.
        /// 3. finally extracts the second level of value form the CAP types matching the conditions above steps.
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="filterName">Cap type filter name which it associates with button or link.</param>
        /// <param name="vchType">VHAPP--Only Application is available;
        /// VHAI--Application and Issuance are available;
        /// EST--Fee Estimation Only;
        /// VHSP--Application and Estimate;
        /// VHCP--Application, Issuance and Estimation;
        /// APNAVHSP--ALL, allowed all above;
        /// NA or N/A--Not available for ACA;
        /// null/empty--ALL VCH type.</param>
        /// <param name="userID">The user unique identifier.</param>
        /// <param name="containAsChildOnly">whether need as child only cap type</param>
        /// <returns>board types in the array of CapTypeModel type. returns empty list instance If no cap type</returns>
        public List<CapTypeModel> GetBoardTypes(string moduleName, string filterName, string vchType, string userID, bool containAsChildOnly)
        {
            List<CapTypeModel> boardTypes = new List<CapTypeModel>();

            CapTypeModel[] capTypes = GetGeneralCapTypeList(moduleName, filterName, vchType, userID, containAsChildOnly);
            if (capTypes != null)
            {
                foreach (CapTypeModel item in capTypes)
                {
                    if (item != null && 
                        !string.IsNullOrEmpty(item.type) &&
                        !boardTypes.Exists(tmp => item.type == tmp.type))
                    {
                        boardTypes.Add(item);
                    }
                }
            }

            return boardTypes;
        }

        /// <summary>
        /// Get Cap type by Cap ID.
        /// </summary>
        /// <param name="capIDModel">capIDModel entity</param>
        /// <returns>Cap Type object</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel GetCapTypeByCapID(CapIDModel4WS capIDModel)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeByCapID()");
            }

            try
            {
                CapTypeModel capType = CapTypeService.getCapTypeByCapID(capIDModel);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeByCapID()");
                }

                return capType;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get CAP type to indicates what page flow should be used for current cap. For super agency, it returns the parent's CAP type and for normal agency, it returns current CAP type.
        /// </summary>
        /// <param name="capIDModel">CapIDModel entity</param>
        /// <returns>Cap Type object</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel GetCapTypeByCapIDForShoppingCart(CapIDModel4WS capIDModel)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeByCapIDForShoppingCart()");
            }

            try
            {
                CapTypeModel capType = CapTypeService.getCapTypeByCapIDForShoppingCart(capIDModel);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeByCapIDForShoppingCart()");
                }

                return capType;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets cap detail type by cap type model
        /// </summary>
        /// <param name="capTypeModel4WS">CAP type model</param>
        /// <returns>CapTypeDetailModel4WS object</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeDetailModel GetCapTypeByPK(CapTypeModel capTypeModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.getCapTypeByPK()");
            }

            try
            {
                CapTypeDetailModel response = new CapTypeDetailModel();
                if (!string.IsNullOrEmpty(capTypeModel4WS.group) && !string.IsNullOrEmpty(capTypeModel4WS.type) && !string.IsNullOrEmpty(capTypeModel4WS.subType) && !string.IsNullOrEmpty(capTypeModel4WS.category))
                {
                    response = CapTypeService.getCapTypeByPK(capTypeModel4WS);
                }
                else
                {
                    response = CapTypeService.getCapTypeByAlias(AgencyCode, capTypeModel4WS.alias);
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.getCapTypeByPK()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets cap detail type list by module.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="queryFormat4ws">query format</param>
        /// <returns>Array of CapTypeDetailModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeDetailModel[] GetCapTypeDetailListByModule(string serviceProviderCode, string moduleName, QueryFormat queryFormat4ws)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.getCapTypeDetailListByModule()");
            }

            try
            {
                CapTypeDetailModel[] response = CapTypeService.getCapTypeDetailListByModule(serviceProviderCode, moduleName, queryFormat4ws);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.getCapTypeDetailListByModule()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Cap Type Items.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="vchType">VCH Type value.</param>
        /// <returns>List of  cap type items.</returns>
        public List<ListItem> GetCapTypeItems(string moduleName, string vchType)
        {
            CapTypeModel[] capTypes = GetCapTypeList4ACA(moduleName, vchType, null);

            if (capTypes == null || capTypes.Length == 0)
            {
                return null;
            }

            List<ListItem> listItems = new List<ListItem>();

            if (string.IsNullOrEmpty(moduleName))
            {
                listItems = GetAllCapTypeItems(capTypes);
            }
            else
            {
                // get cap type items for the specific module
                foreach (CapTypeModel model in capTypes)
                {
                    if (model == null)
                    {
                        continue;
                    }

                    ListItem item = new ListItem();
                    item.Text = CAPHelper.GetAliasOrCapTypeLabel(model);
                    item.Value = CAPHelper.GetCapTypeValue(model);
                    listItems.Add(item);
                }
            }

            return listItems;
        }

        /// <summary>
        /// Gets cap type for search page, if a cap type was unmarked via admin, it should be throw.
        /// </summary>
        /// <param name="servProvCode">agency id value.</param>
        /// <param name="moduleName">module name</param>
        /// <param name="queryFormat4ws">query Format entity</param>
        /// <returns>Array of Cap Type</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetCapTypeList(string servProvCode, string moduleName, QueryFormat queryFormat4ws)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeList()");
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeList(servProvCode, moduleName, queryFormat4ws);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeList()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets cap type list by cap type models with cap type PK information
        /// </summary>
        /// <param name="capTypeModel">CAP type models</param>
        /// <returns>Cap Type object</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetCapTypeListByPKs(CapTypeModel[] capTypeModel)
        {
            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeListByPKs(capTypeModel);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Cap type list, if get all modules' cap type, set the "moduleName" null or empty.
        /// </summary>
        /// <param name="moduleName">module Name.</param>
        /// <param name="vchType">VCH Type value.</param>
        /// <param name="queryFormat4ws">queryFormat value.</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetCapTypeList4ACA(string moduleName, string vchType, QueryFormat queryFormat4ws)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeList4ACA()");
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeList4ACA(AgencyCode, moduleName, vchType, queryFormat4ws);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeList4ACA()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get CAP type list by module.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="queryFormat">query format model</param>
        /// <returns>Array of CapTypeModel</returns>
        public CapTypeModel[] GetCapTypeList4ACAByModule(string moduleName, QueryFormat queryFormat)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeList4ACAByModule()");
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeList4ACAByModule(AgencyCode, moduleName, null); //(AgencyCode, groupCode, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeList4ACAByModule()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets all cap type list by filter which it associate with available cap type.
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="filterName">The filter name which it associate with available cap types.</param>
        /// <param name="vchType">VHAPP--Only Application is available;
        /// VHAI--Application and Issuance are available;
        /// EST--Fee Estimation Only;
        /// VHSP--Application and Estimate;
        /// VHCP--Application, Issuance and Estimation;
        /// APNAVHSP--ALL, allowed all above;
        /// NA or N/A--Not available for ACA;
        /// null/empty--ALL VCH type.</param>
        /// <param name="userID">The user unique identifier.</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetCapTypeListByFilter(string moduleName, string filterName, string vchType, string userID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeListByFilter()");
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeListByFilter(AgencyCode, moduleName, filterName, vchType, userID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeListByFilter()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judge the cap type accessible.
        /// </summary>
        /// <param name="capType">Cap Type model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter name</param>
        /// <param name="vchType">VCH type.</param>
        /// <param name="userID">user id.</param>
        /// <returns>cap type whether accessible.</returns>
        public bool IsCapTypeAccessible(CapTypeModel capType, string moduleName, string filterName, string vchType, string userID)
        {
            bool isAccesssible = false;

            CapTypeModel[] capTypes = GetGeneralCapTypeList(moduleName, filterName, vchType, userID);

            if (capTypes != null && capTypes.Length > 0)
            {
                foreach (CapTypeModel item in capTypes)
                {
                    if (item != null && !string.IsNullOrEmpty(item.group) && item.group.Equals(capType.group)
                        && !string.IsNullOrEmpty(item.type) && item.type.Equals(capType.type)
                        && !string.IsNullOrEmpty(item.subType) && item.subType.Equals(capType.subType)
                        && !string.IsNullOrEmpty(item.category) && item.category.Equals(capType.category))
                    {
                        isAccesssible = true;
                        break;
                    }
                }
            }

            return isAccesssible;
        }

        /// <summary>
        /// Method to get CAP type list by module.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="vchType">VHAPP--Only Application is available;
        /// VHAI--Application and Issuance are available;
        /// EST--Fee Estimation Only;
        /// VHSP--Application and Estimate;
        /// VHCP--Application, Issuance and Estimation;
        /// APNAVHSP--ALL, allowed all above;
        /// NA or N/A--Not available for ACA;
        /// null/empty--ALL VCH type.</param>
        /// <param name="queryFormat4ws">query format model</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetCapTypeListByModule(string serviceProviderCode, string moduleName, string vchType, QueryFormat queryFormat4ws)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.getCapTypeListByModule()");
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeListByModule(serviceProviderCode, moduleName, vchType, queryFormat4ws);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.getCapTypeListByModule()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get CAP type list by module.
        /// </summary>
        /// <param name="groupCode">the page flow group code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="DataValidateException">{ <c>pageflow group code</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetCapTypeListByPageflowGroupCode(string groupCode, string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetCapTypeListByPageflowGroupCode()");
            }

            if (string.IsNullOrEmpty(groupCode))
            {
                throw new DataValidateException(new string[] { "pageflow group code" });
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getCapTypeList4ACAPageFlowGroup(AgencyCode, moduleName, groupCode, "ACA ADMIN"); //(AgencyCode, groupCode, User.PublicUserId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetCapTypeListByPageflowGroupCode()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets all filtered CAP types according to license types of public user¡¯s valid licenses for module.
        /// If not enable filter for module, method will return all CAP types filtered by VCH type.
        /// If the public user has invalid licenses, method will filter the CAP types related license types of the invalid license.
        /// If a license type of valid licenses is valid, method will match its corresponding CAP types according to
        /// Standard Choice. Method will filter the CAP types are not existed in CAP type list which is external parameter.
        /// The left CAP types can be as valid CAP types to be applied by this public user.
        /// If license Type is not empty, that means end user selected a specific license.
        /// Method will filter the specific license type not all license types of this user.
        /// If there are excluded CAP types in Standard Choice configuration for module, method will ignore to filter
        /// these CAP types. This means license professionals can apply for these CAP types with or without a valid license.
        /// All filtered CAP types matches the CAP types by VCH type. If the filtered CAP type is not existed in CAP types by VCH type,
        /// it will be filtered again.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="module">module name</param>
        /// <param name="userSeqNbr">public user sequence number</param>
        /// <param name="vchType">VHAPP--Only Application is available;
        /// VHAI--Application and Issuance are available;
        /// EST--Fee Estimation Only;
        /// VHSP--Application and Estimate;
        /// VHCP--Application, Issuance and Estimation;
        /// APNAVHSP--ALL, allowed all above;
        /// NA or N/A--Not available for ACA;
        /// null/empty--ALL VCH type.</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCode, module, userSeqNbr</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetFilteredCapTypesByPublicUser(string servProvCode, string module, string userSeqNbr, string vchType)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.getFilteredCapTypesByPublicUser()");
            }

            if (string.IsNullOrEmpty(servProvCode) || string.IsNullOrEmpty(module) || string.IsNullOrEmpty(userSeqNbr))
            {
                throw new DataValidateException(new string[] { "servProvCode", "module", "userSeqNbr" });
            }

            try
            {
                CapTypeModel[] response = CapTypeService.getFilteredCapTypesByPublicUser(servProvCode, module, userSeqNbr, vchType);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.getFilteredCapTypesByPublicUser()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// This method is general method for getting cap type list.
        /// 1. firstly invoke GetCapTypeListByFilter when filter name isn't null or string.empty.
        /// 2. secondly invoke GetFilteredCapTypesByPublicUser when flag be turn on in standard choice "STD_CAT_ACA_FILTER_CAP_BY_LICENSE".
        /// 3. third invoke default getting cap type list method GetCapTypeListByModule.
        /// 3. finally filter CAP types according by the given board type if Board Type Selection mode is enabled.
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="filterName">Cap type filter name which it associates with button or link.</param>
        /// <param name="vchType">VHAPP--Only Application is available;
        /// VHAI--Application and Issuance are available;
        /// EST--Fee Estimation Only;
        /// VHSP--Application and Estimate;
        /// VHCP--Application, Issuance and Estimation;
        /// APNAVHSP--ALL, allowed all above;
        /// NA or N/A--Not available for ACA;
        /// null/empty--ALL VCH type.</param>
        /// <param name="boardType">The board type used to filter CAP types:
        /// if Board Type Selection mode is:
        /// 1. Enabled:
        /// (1) if boardType is not equal to null or string.empty, returns all available CAP types under the specified board type;
        /// (2) otherwise, returns no CAP types
        /// 2. Disable:
        /// (1) throws ACAException with an InvalidInvokeException internal exception if boardType is not equal to null or string.empty;
        /// (2) otherwise, returns all available CAP types</param>
        /// <param name="userID">The user unique identifier.</param>
        /// <param name="containAsChildOnly">whether need as child only cap type</param>
        /// <returns>Cap Type List</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public CapTypeModel[] GetGeneralCapTypeList(string moduleName, string filterName, string vchType, string boardType, string userID, bool containAsChildOnly = false)
        {
            List<CapTypeModel> availableCapTypes = new List<CapTypeModel>();

            CapTypeModel[] capTypes = GetGeneralCapTypeList(moduleName, filterName, vchType, userID, containAsChildOnly);
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            bool isEnabledBoardTypeSelection = ValidationUtil.IsYes(policyBll.GetValueByKey(XPolicyConstant.ENABLE_BOARD_TYPE_SELECTION, moduleName));

            if (isEnabledBoardTypeSelection)
            {
                if (!string.IsNullOrEmpty(boardType) &&
                    capTypes != null)
                {
                    foreach (CapTypeModel item in capTypes)
                    {
                        if (item != null && item.type == boardType)
                        {
                            availableCapTypes.Add(item);
                        }
                    }                    
                }
            }
            else
            {
                if (string.IsNullOrEmpty(boardType) == false)
                {
                    throw new ACAException(new InvalidOperationException());
                }

                if (capTypes != null)
                {
                    availableCapTypes.AddRange(capTypes);
                }
            }

            return availableCapTypes.ToArray<CapTypeModel>();
        }

        /// <summary>
        /// This method is general method for getting cap type list.
        /// 1. firstly invoke GetCapTypeListByFilter when filter name isn't null or string.empty.
        /// 2. secondly invoke GetFilteredCapTypesByPublicUser when flag be turn on in standard choice "STD_CAT_ACA_FILTER_CAP_BY_LICENSE".
        /// 3. finally invoke default getting cap type list method GetCapTypeListByModule.
        /// </summary>
        /// <param name="moduleName">The module this group belongs to</param>
        /// <param name="filterName">Cap type filter name which it associates with button or link.</param>
        /// <param name="vchType">VHAPP--Only Application is available;
        /// VHAI--Application and Issuance are available;
        /// EST--Fee Estimation Only;
        /// VHSP--Application and Estimate;
        /// VHCP--Application, Issuance and Estimation;
        /// APNAVHSP--ALL, allowed all above;
        /// NA or N/A--Not available for ACA;
        /// null/empty--ALL VCH type.</param>
        /// <param name="userID">The user unique identifier.</param>
        /// <param name="containAsChildOnly">whether need as child only cap type</param>
        /// <returns>Cap Type List</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetGeneralCapTypeList(string moduleName, string filterName, string vchType, string userID, bool containAsChildOnly = false)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.GetGeneralCapTypeList()");
            }

            try
            {
                CapTypeModel[] capTypeList;

                if (!string.IsNullOrEmpty(filterName))
                {
                    capTypeList = GetCapTypeListByFilter(moduleName, filterName, vchType, userID);
                }
                else
                {
                    IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                    string flag = bizBll.GetValueForStandardChoice(AgencyCode, BizDomainConstant.STD_CAT_ACA_FILTER_CAP_BY_LICENSE, BizDomainConstant.STD_ITEM_ENABLE_FILTER_BY_REFTYPE + moduleName);

                    if (flag != null && flag.Equals(ACAConstant.COMMON_YES, StringComparison.InvariantCultureIgnoreCase))
                    {
                        capTypeList = GetFilteredCapTypesByPublicUser(AgencyCode, moduleName, User.UserSeqNum, vchType);
                    }
                    else
                    {
                        capTypeList = GetCapTypeListByModule(AgencyCode, moduleName, vchType, null);
                    }
                }

                if (capTypeList != null && !containAsChildOnly)
                {
                    capTypeList = capTypeList.Where(capType => !ValidationUtil.IsYes(capType.asChildOnly)).ToArray();
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.GetGeneralCapTypeList()");
                }

                return capTypeList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to check.
        /// </summary>
        /// <param name="capTypeModule">cap Type Alias Name</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter Name</param>
        /// <returns>true(this cap is trade license cap) or false</returns>
        /// <exception cref="DataValidateException">{ <c>capTypeModule, filterName</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool IsMatchTheFilter(CapTypeModel capTypeModule, string moduleName, string filterName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin CapTypeBll.IsMatchTheFilter()");
            }

            if (capTypeModule == null || string.IsNullOrEmpty(filterName))
            {
                throw new DataValidateException(new string[] { "capTypeModule", "filterName" });
            }

            try
            {
                bool isMatchTheFilter = CapTypeService.isMatchTheFilter(AgencyCode, capTypeModule, moduleName, filterName);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End CapTypeBll.IsMatchTheFilter()");
                }

                return isMatchTheFilter;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets all cap type list by filter which it associate with available cap type.
        /// </summary>
        /// <param name="capType">The cap type model</param>
        /// <returns>Array of AppStatusGroupModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public AppStatusGroupModel4WS[] GetAppStatusByCapType(CapTypeModel capType) 
        {
            try
            {
                return AppStatusGroupWebService.getAppStatusByCapType(AgencyCode, capType, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the dependent cap type list.
        /// </summary>
        /// <param name="servProvCode">agency name</param>
        /// <param name="moduleName">module name</param>
        /// <param name="baseCapIDModel">the base capID model</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetDependentCapTypeList(string servProvCode, string moduleName, CapIDModel baseCapIDModel)
        {
            try
            {
                return CapTypeService.getDependentCapTypeList(servProvCode, moduleName, baseCapIDModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the prerequisites CapTypeModel list from the owner permit list
        /// </summary>
        /// <param name="servProvCode">agency name</param>
        /// <param name="unPaidCapIDList">the unpaid capID model list without current</param>
        /// <param name="currentCapID">current capID model</param>
        /// <returns>Array of CapTypeModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypeModel[] GetPrerequisitesCapTypeList(string servProvCode, CapIDModel[] unPaidCapIDList, CapIDModel currentCapID)
        {
            try
            {
                return CapTypeService.getPrerequisitesCapTypeList(servProvCode, unPaidCapIDList, currentCapID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }       
        }

        /// <summary>
        /// Get all modules' cap type items
        /// </summary>
        /// <param name="capTypes">cap type model</param>
        /// <returns>List items.</returns>
        private List<ListItem> GetAllCapTypeItems(CapTypeModel[] capTypes)
        {
            if (capTypes == null || capTypes.Length == 0)
            {
                return null;
            }

            var types = from c in capTypes group c by c.dispGroup into g orderby g.Key select new { Module = g.Key, CapTypeModel = g };

            List<ListItem> listItems = new List<ListItem>();

            foreach (var g in types)
            {
                if (g.CapTypeModel == null)
                {
                    continue;
                }

                ListItem moduleItem = new ListItem();
                moduleItem.Text = "--" + g.Module + "--";
                moduleItem.Value = string.Empty;
                moduleItem.Attributes.Add("style", "color:Gray;");

                listItems.Add(moduleItem);

                List<ListItem> moduleItems = new List<ListItem>();

                foreach (var model in g.CapTypeModel)
                {
                    ListItem item = new ListItem();
                    item.Text = CAPHelper.GetAliasOrCapTypeLabel(model);
                    item.Value = CAPHelper.GetCapTypeValue(model);

                    moduleItems.Add(item);
                }

                var orderItems = moduleItems.OrderBy(m => m.Text);

                listItems.AddRange(orderItems);
            }

            return listItems;
        }

        #endregion Methods
    }
}