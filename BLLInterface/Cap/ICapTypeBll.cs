#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICapTypeBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  ICapTypeBll
 *
 *  Notes:
 * $Id: ICapTypeBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Web.UI.WebControls;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{    
    /// <summary>
    /// Defines methods for Cap Type BLL logic.
    /// </summary>
    public interface ICapTypeBll
    {
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
        List<CapTypeModel> GetBoardTypes(string moduleName, string filterName, string vchType, string userID, bool containAsChildOnly);

        /// <summary>
        /// Get Cap type by Cap ID.
        /// </summary>
        /// <param name="capIDModel">capIDModel entity</param>
        /// <returns>Cap Type object</returns>
        CapTypeModel GetCapTypeByCapID(CapIDModel4WS capIDModel);

        /// <summary>
        /// Get CAP type to indicates what page flow should be used for current cap. For super agency, it returns the parent's CAP type and for normal agency, it returns current CAP type.
        /// </summary>
        /// <param name="capIDModel">CapIDModel entity</param>
        /// <returns>Cap Type object</returns>
        CapTypeModel GetCapTypeByCapIDForShoppingCart(CapIDModel4WS capIDModel);

        /// <summary>
        /// Gets cap detail type by cap type model
        /// </summary>
        /// <param name="capTypeModel4WS">CAP type model</param>
        /// <returns>CapTypeDetailModel4WS object</returns>
        CapTypeDetailModel GetCapTypeByPK(CapTypeModel capTypeModel4WS);

        /// <summary>
        /// Gets cap type list by cap type models with cap type PK information
        /// </summary>
        /// <param name="capTypeModel">CAP type models</param>
        /// <returns>Cap Type object</returns>
        CapTypeModel[] GetCapTypeListByPKs(CapTypeModel[] capTypeModel);

        /// <summary>
        /// Gets cap detail type list by module.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="queryFormat4ws">query format</param>
        /// <returns>Array of CapTypeDetailModel4WS</returns>
        CapTypeDetailModel[] GetCapTypeDetailListByModule(string serviceProviderCode, string moduleName, QueryFormat queryFormat4ws);

        /// <summary>
        /// Get Cap Type Items.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="vchType">VCH Type value.</param>
        /// <returns>List of  cap type items.</returns>
        List<ListItem> GetCapTypeItems(string moduleName, string vchType);

        /// <summary>
        /// Gets cap type for search page, if a cap type was unmarked via admin, it should be throw.
        /// </summary>
        /// <param name="servProvCode">agency id value.</param>
        /// <param name="moduleName">module name</param>
        /// <param name="queryFormat4ws">query Format entity</param>
        /// <returns>Array of Cap Type</returns>
        CapTypeModel[] GetCapTypeList(string servProvCode, string moduleName, QueryFormat queryFormat4ws);

        /// <summary>
        /// Get Cap type list, if get all modules' cap type, set the "moduleName" null or empty.
        /// </summary>
        /// <param name="moduleName">module Name.</param>
        /// <param name="vchType">VCH Type value.</param>
        /// <param name="queryFormat4ws">queryFormat value.</param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetCapTypeList4ACA(string moduleName, string vchType, QueryFormat queryFormat4ws);

        /// <summary>
        /// Method to get CAP type list by module.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="queryFormat">query format model</param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetCapTypeList4ACAByModule(string moduleName, QueryFormat queryFormat);

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
        CapTypeModel[] GetCapTypeListByFilter(string moduleName, string filterName, string vchType, string userID);

        /// <summary>
        /// Judge the cap type accessible.
        /// </summary>
        /// <param name="capType">Cap Type model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter name</param>
        /// <param name="vchType">VCH type.</param>
        /// <param name="userID">user id.</param>
        /// <returns>cap type whether accessible.</returns>
        bool IsCapTypeAccessible(CapTypeModel capType, string moduleName, string filterName, string vchType, string userID);

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
        /// null/empty--ALL VCH type.
        /// </param>
        /// <param name="queryFormat4ws">query format model</param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetCapTypeListByModule(string serviceProviderCode, string moduleName, string vchType, QueryFormat queryFormat4ws);

        /// <summary>
        /// Method to get CAP type list by module.
        /// </summary>
        /// <param name="groupCode">the page flow group code</param>
        /// <param name="moduleName">module name</param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetCapTypeListByPageflowGroupCode(string groupCode, string moduleName);

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
        ///  it will be filtered again.
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
        /// null/empty--ALL VCH type.
        /// </param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetFilteredCapTypesByPublicUser(string servProvCode, string module, string userSeqNbr, string vchType);

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
        CapTypeModel[] GetGeneralCapTypeList(string moduleName, string filterName, string vchType, string userID, bool containAsChildOnly = false);

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
        CapTypeModel[] GetGeneralCapTypeList(string moduleName, string filterName, string vchType, string boardType, string userID, bool containAsChildOnly = false);

        /// <summary>
        /// Method to check.
        /// </summary>        
        /// <param name="capTypeModule">cap Type Alias Name</param>
        /// <param name="moduleName">module name</param>
        /// <param name="filterName">filter Name</param>
        /// <returns>true(this cap is trade license cap) or false</returns>
        bool IsMatchTheFilter(CapTypeModel capTypeModule, string moduleName, string filterName);

        /// <summary>
        /// Gets all cap type list by filter which it associate with available cap type.
        /// </summary>
        /// <param name="capType">The cap type model</param>
        /// <returns>Array of AppStatusGroupModel4WS</returns>
        AppStatusGroupModel4WS[] GetAppStatusByCapType(CapTypeModel capType);

        /// <summary>
        /// Gets the dependent cap type list.
        /// </summary>
        /// <param name="servProvCode">agency name</param>
        /// <param name="moduleName">module name</param>
        /// <param name="baseCapIDModel">the base capID model</param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetDependentCapTypeList(string servProvCode, string moduleName, CapIDModel baseCapIDModel);

        /// <summary>
        /// Gets the prerequisites CapTypeModel list from the owner permit list
        /// </summary>
        /// <param name="servProvCode">agency name</param>
        /// <param name="unPaidCapIDList">the unpaid capID model list without current</param>
        /// <param name="currentCapID">current capID model</param>
        /// <returns>Array of CapTypeModel</returns>
        CapTypeModel[] GetPrerequisitesCapTypeList(string servProvCode, CapIDModel[] unPaidCapIDList, CapIDModel currentCapID);

        #endregion Methods
    }
}