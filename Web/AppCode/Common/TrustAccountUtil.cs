#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TrustAccountUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TrustAccountUtil.cs 146779 2009-09-10 02:07:13Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// utility class for trust account.
    /// </summary>
    public static class TrustAccountUtil
    {
        #region Fields

        /// <summary>
        /// common for "active"
        /// </summary>
        private const string COMMON_ACTIVE = "Active";

        /// <summary>
        /// common for "closed"
        /// </summary>
        private const string COMMON_CLOSED = "Closed";

        #endregion Fields

        #region public Methods

        /// <summary>
        /// Convert status DB value to display value.
        /// </summary>
        /// <param name="status">trust account status value, "Active", "Closed"</param>
        /// <returns>trust account status label.</returns>
        public static string ConvertStatusField2Display(string status)
        {
            string resStatus = status;

            if (COMMON_ACTIVE.Equals(status, StringComparison.InvariantCultureIgnoreCase))
            {
                resStatus = BasePage.GetStaticTextByKey("aca_common_active");
            }
            else if (COMMON_CLOSED.Equals(status, StringComparison.InvariantCultureIgnoreCase))
            {
                resStatus = BasePage.GetStaticTextByKey("aca_common_close");
            }

            return resStatus;
        }

        /// <summary>
        /// Convert primary DB value to display value.
        /// </summary>
        /// <param name="isPrimary"> DB value</param>
        /// <returns>primary display value</returns>
        public static string ConvertPrimaryField2Display(bool isPrimary)
        {
            return isPrimary ? BasePage.GetStaticTextByKey("ACA_Common_Yes") : BasePage.GetStaticTextByKey("ACA_Common_No");
        }

        /// <summary>
        /// Get current cap and parent cap.
        /// </summary>
        /// <returns>The associated record list.</returns>
        public static IList<ListItem> GetAssociatedRecordList()
        {
            IList<ListItem> associatedTypesList = new List<ListItem>();
            CapIDModel4WS[] capIDs = AppSession.GetCapIDModelsFromSession();

            if (capIDs != null && capIDs.Length == 1)
            {
                string[] parentAccountIds = GetParentCapTrustAcountIDs(capIDs[0]);

                if (parentAccountIds != null && parentAccountIds.Length > 0)
                {
                    CapIDModel[] parentCapIDArray = GetParentCapIDList(capIDs[0]);

                    if (parentCapIDArray != null && parentCapIDArray.Length > 0)
                    {
                        foreach (CapIDModel parentCapID in parentCapIDArray)
                        {
                            string itemValue = string.Format("{0}{1}{2}", parentCapID.serviceProviderCode, ACAConstant.SPLIT_CHAR18, parentCapID.customID);
                            string itemText = string.Format("{0}{1}", BasePage.GetStaticTextByKey("aca_trustaccount_cap_parent_label"), parentCapID.customID);
                            associatedTypesList.Add(new ListItem(itemText, itemValue));
                        }
                    }
                }

                ITrustAccountBll trustAccountBll = (ITrustAccountBll)ObjectFactory.GetObject(typeof(ITrustAccountBll));
                string[] accountIds = trustAccountBll.GetTrustAccountIDsByCapID(capIDs[0]);
                if (accountIds != null && accountIds.Length > 0)
                {
                    string itemText = string.Empty;
                    string itemValue = string.Format("{0}{1}{2}", capIDs[0].serviceProviderCode, ACAConstant.SPLIT_CHAR18, capIDs[0].customID);
                   
                    if (parentAccountIds != null && parentAccountIds.Length > 0)
                    {
                        //if system can find parent cap id, we need to add "Child:" as prefix
                        itemText = string.Format("{0}{1}", BasePage.GetStaticTextByKey("aca_trustaccount_cap_child_label"), capIDs[0].customID);
                    }
                    else 
                    {
                        //if system can not find parent cap id, we only need to bing current cap id.
                        itemText = string.Format("{0}", capIDs[0].customID);
                    }
                    
                    associatedTypesList.Add(new ListItem(itemText, itemValue));
                }
            }

            return associatedTypesList;
        }

        /// <summary>
        /// get data source for <c>ddlTrustAccount</c> control.
        /// </summary>
        /// <param name="seqNbr">the sequence number.</param>
        /// <param name="associatedType">payment associated type.</param>
        /// <returns>The trust account list.</returns>
        public static IList<ListItem> GetTrustAccounts(string seqNbr, ACAConstant.PaymentAssociatedType associatedType)
        {
            IList<ListItem> trustAccountList = new List<ListItem>();
            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            // Trust account need to be supported only when all caps belong to one agency
            if (htAgencies.Count == 1)
            {
                string capAgency = htAgencies[0].ToString();

                ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

                string[] accountIds = null;

                switch (associatedType)
                {
                    case ACAConstant.PaymentAssociatedType.Licenses:
                    case ACAConstant.PaymentAssociatedType.Contacts:
                    case ACAConstant.PaymentAssociatedType.Addresses:
                    case ACAConstant.PaymentAssociatedType.Parcels:
                        accountIds = trustAccountBll.GetTrustAccountListBySeqNbr(ConfigManager.AgencyCode, seqNbr, capAgency, associatedType);
                        break;
                    case ACAConstant.PaymentAssociatedType.Record:
                        if (!string.IsNullOrEmpty(seqNbr))
                        {
                            string[] altId = seqNbr.Split(ACAConstant.SPLIT_CHAR18);
                            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                            CapIDModel4WS selectedCapId = capBll.GetCapIDByAltID(altId[0], altId[1]);

                            accountIds = trustAccountBll.GetTrustAccountIDsByCapID(selectedCapId);
                        }

                        break;
                }

                if (accountIds != null && accountIds.Length > 0)
                {
                    foreach (string id in accountIds)
                    {
                        trustAccountList.Add(new ListItem(id, id));
                    }
                }
            }

            return trustAccountList;
        }

        /// <summary>
        /// Convert reference address model to address model.
        /// </summary>
        /// <param name="refAddress">refAddress model.</param>
        /// <returns>a address model.</returns>
        public static AddressModel ConvertAddressModelbyReference(RefAddressModel refAddress)
        {
            AddressModel address = null;

            if (refAddress != null)
            {
                address = new AddressModel();
                address.houseNumberStart = StringUtil.ToInt(refAddress.houseNumberStart.HasValue ? refAddress.houseNumberStart.Value.ToString() : string.Empty);
                address.houseFractionStart = refAddress.houseFractionStart;
                address.houseNumberEnd = StringUtil.ToInt(refAddress.houseNumberEnd.HasValue ? refAddress.houseNumberEnd.Value.ToString() : string.Empty);
                address.houseFractionEnd = refAddress.houseFractionEnd;
                address.streetDirection = refAddress.streetDirection;
                address.resStreetDirection = refAddress.resStreetDirection;
                address.streetPrefix = refAddress.streetPrefix;
                address.streetName = refAddress.streetName;
                address.streetSuffix = refAddress.streetSuffix;
                address.resStreetSuffix = refAddress.resStreetSuffix;
                address.streetSuffixdirection = refAddress.streetSuffixdirection;
                address.resStreetSuffixdirection = refAddress.resStreetSuffixdirection;
                address.unitType = refAddress.unitType;
                address.resUnitType = refAddress.resUnitType;

                address.unitStart = refAddress.unitStart;
                address.unitEnd = refAddress.unitEnd;
                address.secondaryRoad = refAddress.secondaryRoad;
                address.secondaryRoadNumber = StringUtil.ToInt(refAddress.secondaryRoadNumber.HasValue ? refAddress.secondaryRoadNumber.Value.ToString() : string.Empty);
                address.neighborhoodPrefix = refAddress.neighborhoodPrefix;
                address.neighborhood = refAddress.neighborhood;
                address.addressDescription = refAddress.addressDescription;
                address.distance = refAddress.distance;

                address.inspectionDistrictPrefix = refAddress.inspectionDistrictPrefix;
                address.inspectionDistrict = refAddress.inspectionDistrict;
                address.city = refAddress.city;
                address.county = refAddress.county;
                address.state = refAddress.state;
                address.resState = refAddress.resState;
                address.zip = refAddress.zip;

                address.countryCode = refAddress.countryCode;
                address.resCountryCode = refAddress.resCountryCode;
                address.fullAddress = refAddress.fullAddress;
                address.addressLine1 = refAddress.addressLine1;
                address.addressLine2 = refAddress.addressLine2;

                address.levelPrefix = refAddress.levelPrefix;
                address.levelNumberStart = refAddress.levelNumberStart;
                address.levelNumberEnd = refAddress.levelNumberEnd;
                address.houseNumberAlphaStart = refAddress.houseNumberAlphaStart;
                address.houseNumberAlphaEnd = refAddress.houseNumberAlphaEnd;
            }

            return address;
        }
        
        /// <summary>
        /// Gets the associated trust account's model.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <returns>The trust account model.</returns>
        public static TrustAccountModel GetTrustAccountModel(string accountId)
        {
            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            if (htAgencies.Count != 1)
            {
                return null;
            }

            TrustAccountModel trustAccountModel = null;

            string capAgency = htAgencies[0].ToString();
            ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

            if (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent)
            {
                string userId = AppSession.User.IsAuthorizedAgent ? AppSession.User.UserID : AppSession.User.AuthAgentPublicUserID;
                TrustAccountModel[] trustAccountModels = trustAccountBll.GetTrustAccountByUserID4Agent(capAgency, userId);

                if (trustAccountModels != null && trustAccountModels.Length > 0)
                {
                    trustAccountModel = trustAccountModels.FirstOrDefault(o => ACAConstant.CAP_ACTIVE.Equals(o.acctStatus, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            else if (!string.IsNullOrEmpty(accountId))
            {
                trustAccountModel = trustAccountBll.GetTrustAccountByAccountID(accountId, capAgency);
            }

            return trustAccountModel;
        }

        /// <summary>
        /// Gets the total amount.
        /// </summary>
        /// <param name="trustAccountModel">The trust account model.</param>
        /// <returns>The total amount.</returns>
        public static double GetTotalAmount(TrustAccountModel trustAccountModel)
        {
            if (trustAccountModel == null)
            {
                return 0d;
            }

            double totalAmount = trustAccountModel.acctBalance == null ? 0 : trustAccountModel.acctBalance.Value;

            if (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent)
            {
                // for agent user, it has overdraft limited.
                if (trustAccountModel.overdraftLimit != null)
                {
                    totalAmount += trustAccountModel.overdraftLimit.Value;
                }
            }

            return totalAmount;
        }

        /// <summary>
        /// Get trust account IDs of parent Cap ID.
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <returns>account ids</returns>
        public static string[] GetParentCapTrustAcountIDs(CapIDModel4WS capID)
        {
            string[] parentAccountIds = null;
            List<string> trustAccountIDs = new List<string>();
            CapIDModel[] parentCapIDArray = GetParentCapIDList(capID);

            if (parentCapIDArray != null && parentCapIDArray.Length > 0)
            {
                foreach (CapIDModel parentCapID in parentCapIDArray)
                {
                    ITrustAccountBll trustAccountBll = (ITrustAccountBll)ObjectFactory.GetObject(typeof(ITrustAccountBll));
                    parentAccountIds = trustAccountBll.GetTrustAccountIDsByCapID(TempModelConvert.Add4WSForCapIDModel(parentCapID));
                    trustAccountIDs.AddRange(parentAccountIds);
                }
            }

            return trustAccountIDs.ToArray();
        }

        /// <summary>
        /// Determine whether we need to show the trust account option
        /// </summary>
        /// <returns>true or false.</returns>
        public static bool ShowTrustAccountOption()
        {
            ArrayList agencies = CapUtil.GetAgenciesFromCaps();

            bool hasMultipleAgencies = agencies.Count > 1;
            bool hasTrustAccount = HasLicenseTrustAccount() || HasContactTrustAccount() || HasParcelTrustAccount() || HasAddressTrustAccount() || HasRecordTrustAccount();

            return !hasMultipleAgencies && hasTrustAccount;
        }

        /// <summary>
        /// True: Has License's Trust Account.
        /// </summary>
        /// <returns>Indicate whether has the license's trust account.</returns>
        public static bool HasLicenseTrustAccount()
        {
            if (AppSession.User == null || AppSession.User.UserModel4WS == null)
            {
                return false;
            }

            ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            // Trust account need to be supported only when all caps belong to one agency
            if (htAgencies.Count == 1 && AppSession.User.UserModel4WS.licenseModel != null && AppSession.User.UserModel4WS.licenseModel.Length > 0)
            {
                string capAgency = htAgencies[0].ToString();

                foreach (LicenseModel4WS license in AppSession.User.UserModel4WS.licenseModel)
                {
                    string[] accountIds =
                        trustAccountBll.GetTrustAccountListBySeqNbr(ConfigManager.AgencyCode, license.licSeqNbr, capAgency, ACAConstant.PaymentAssociatedType.Licenses);

                    if (accountIds != null && accountIds.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// True: Has Contact's Trust Account.
        /// </summary>
        /// <returns>Indicate whether has contact's trust account.</returns>
        public static bool HasContactTrustAccount()
        {
            // if there is no professional account, it must have no trust account
            if (AppSession.User == null || AppSession.User.UserModel4WS == null)
            {
                return false;
            }

            ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            // Trust account need to be supported only when all caps belong to one agency
            if (htAgencies.Count == 1 && AppSession.User.UserModel4WS.peopleModel != null && AppSession.User.UserModel4WS.peopleModel.Length > 0)
            {
                string capAgency = htAgencies[0].ToString();

                foreach (PeopleModel4WS contact in AppSession.User.UserModel4WS.peopleModel)
                {
                    string[] accountIds =
                        trustAccountBll.GetTrustAccountListBySeqNbr(ConfigManager.AgencyCode, contact.contactSeqNumber, capAgency, ACAConstant.PaymentAssociatedType.Contacts);

                    if (accountIds != null && accountIds.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// True: Has Address's Trust Account.
        /// </summary>
        /// <returns>Indicate whether has address's trust account.</returns>
        public static bool HasAddressTrustAccount()
        {
            if (AppSession.User == null || AppSession.User.UserModel4WS == null)
            {
                return false;
            }

            ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            // Trust account need to be supported only when all caps belong to one agency
            if (htAgencies.Count == 1 && AppSession.User.UserModel4WS.addressList != null && AppSession.User.UserModel4WS.addressList.Length > 0)
            {
                string capAgencyCode = htAgencies[0].ToString();

                foreach (RefAddressModel address in AppSession.User.UserModel4WS.addressList)
                {
                    //External APO data.
                    if (!string.IsNullOrEmpty(address.UID))
                    {
                        continue;
                    }

                    string[] accountIds = trustAccountBll.GetTrustAccountListBySeqNbr(capAgencyCode, address.refAddressId.ToString(), string.Empty, ACAConstant.PaymentAssociatedType.Addresses);

                    if (accountIds != null && accountIds.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// True: Has Parcel's Trust Account.
        /// </summary>
        /// <returns>Indicate whether has parcel's trust account.</returns>
        public static bool HasParcelTrustAccount()
        {
            if (AppSession.User == null || AppSession.User.UserModel4WS == null)
            {
                return false;
            }

            ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            // Trust account need to be supported only when all caps belong to one agency
            if (htAgencies.Count == 1 && AppSession.User.UserModel4WS.parcelList != null && AppSession.User.UserModel4WS.parcelList.Length > 0)
            {
                string agencyCode = htAgencies[0].ToString();

                foreach (ParcelModel parcel in AppSession.User.UserModel4WS.parcelList)
                {
                    //External APO data.
                    if (!string.IsNullOrEmpty(parcel.UID))
                    {
                        continue;
                    }

                    string[] accountIds =
                        trustAccountBll.GetTrustAccountListBySeqNbr(agencyCode, parcel.parcelNumber, string.Empty, ACAConstant.PaymentAssociatedType.Parcels);

                    if (accountIds != null && accountIds.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// True: Has Record's Trust Account.
        /// </summary>
        /// <returns>Indicate whether has record's trust account.</returns>
        public static bool HasRecordTrustAccount()
        {
            ITrustAccountBll trustAccountBll = ObjectFactory.GetObject<ITrustAccountBll>();

            ArrayList htAgencies = CapUtil.GetAgenciesFromCaps();

            CapIDModel4WS[] capIDs = AppSession.GetCapIDModelsFromSession();

            // Trust account need to be supported only when all caps belong to one agency
            if (htAgencies.Count == 1 && capIDs.Length == 1)
            {
                string[] parentAccountIds = GetParentCapTrustAcountIDs(capIDs[0]);
                string[] accountIds = trustAccountBll.GetTrustAccountIDsByCapID(capIDs[0]);

                if ((accountIds != null && accountIds.Length > 0) || (parentAccountIds != null && parentAccountIds.Length > 0))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// get all associated type.
        /// </summary>
        /// <returns>The all associated type.</returns>
        public static IList<string> GetAllAssociatedTypes()
        {
            IList<string> allAssociatedTypes = new List<string>();

            foreach (string associatedType in Enum.GetNames(typeof(ACAConstant.PaymentAssociatedType)))
            {
                switch (EnumUtil<ACAConstant.PaymentAssociatedType>.Parse(associatedType))
                {
                    case ACAConstant.PaymentAssociatedType.Licenses:
                        if (!HasLicenseTrustAccount() && !AppSession.IsAdmin)
                        {
                            continue;
                        }

                        break;
                    case ACAConstant.PaymentAssociatedType.Contacts:
                        if (!HasContactTrustAccount() && !AppSession.IsAdmin)
                        {
                            continue;
                        }

                        break;
                    case ACAConstant.PaymentAssociatedType.Addresses:
                        if (!HasAddressTrustAccount() && !AppSession.IsAdmin)
                        {
                            continue;
                        }

                        break;
                    case ACAConstant.PaymentAssociatedType.Parcels:
                        if (!HasParcelTrustAccount() && !AppSession.IsAdmin)
                        {
                            continue;
                        }

                        break;
                    case ACAConstant.PaymentAssociatedType.Record:
                        if (!HasRecordTrustAccount() && !AppSession.IsAdmin)
                        {
                            continue;
                        }

                        break;
                }

                allAssociatedTypes.Add(associatedType);
            }

            return allAssociatedTypes;
        }

        /// <summary>
        /// Get available parent CapID list, include related cap, renewal process, amendment process and hazmat relationship.
        /// </summary>
        /// <param name="capID">Cap ID Model</param>
        /// <returns>The parent cap id list.</returns>
        private static CapIDModel[] GetParentCapIDList(CapIDModel4WS capID)
        {
            //Get all parent cap ids, include relationship R\Renewal\Amendment\AssoForm, exclude relationship EST.
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapIDModel[] parentCapIDArray = capBll.GetParentCapIDListByChildCapID(TempModelConvert.Trim4WSOfCapIDModel(capID), string.Empty, string.Empty);

            return parentCapIDArray;
        }
        
        #endregion public Methods
    }
}
