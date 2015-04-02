#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TrustAccoutBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  This class invokes web service TrustAccoutWebService.java all method.
 *  Provider business logic of trust account.
 *
 *  Notes:
 *  $Id: TrustAccoutBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// Handle Trust Account Payment
    /// </summary>
    public class TrustAccountBll : BaseBll, ITrustAccountBll
    {
        #region Fields

        /// <summary>
        /// account status active.
        /// </summary>
        private const string ACCOUNT_STATUS_ACTIVE = "Active";

        /// <summary>
        /// people type type license people.
        /// </summary>
        private const string PEOPLE_TYPE_LICENSED_PEOPLE = "Licensed People";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of TrustAccountService.
        /// </summary>
        private TrustAccountWebServiceService TrustAccountService
        {
            get
            {
                return WSFactory.Instance.GetWebService<TrustAccountWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the trust account by user Id for authorized agent.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns>Trust Account information list.</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCode, userID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TrustAccountModel[] GetTrustAccountByUserID4Agent(string servProvCode, string userID)
        {
            if (string.IsNullOrEmpty(servProvCode) || string.IsNullOrEmpty(userID))
            {
                throw new DataValidateException(new string[] { "servProvCode", "userID" });
            }

            try
            {
                TrustAccountModel[] responses = TrustAccountService.getTrustAccountByUserID4Agent(servProvCode, userID);

                return responses;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets trust account information by trust account's sequence number.
        /// </summary>
        /// <param name="accountSeq">Trust account's sequence number.</param>
        /// <param name="agencyCode">Agency code code.</param>
        /// <returns>Trust Account information.</returns>
        /// <exception cref="DataValidateException">{ <c>accountSeq</c>, agencyCode } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TrustAccountModel ITrustAccountBll.GetTrustAccountByPK(long accountSeq, string agencyCode)
        {
            if (accountSeq <= 0 || string.IsNullOrEmpty(agencyCode))
            {
                throw new DataValidateException(new string[] { "accountSeq", "agencyCode" });
            }

            try
            {
                TrustAccountModel response = TrustAccountService.getTrustAccountByPK(accountSeq, agencyCode);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Trust Account peopleList by cap ID.
        /// </summary>
        /// <param name="accountSeqNumber">trust account id.</param>
        /// <returns>Trust account associate contact information list</returns>
        /// <exception cref="DataValidateException">{ <c>accountSeqNumber</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TrustAccountPeopleModel[] ITrustAccountBll.GetTrustAccountPeopleListByAccountNumber(string accountSeqNumber)
        {
            if (accountSeqNumber == null)
            {
                throw new DataValidateException(new string[] { "accountSeqNumber" });
            }

            try
            {
                TrustAccountPeopleModel[] trustAccounts = TrustAccountService.getTrustAccountPeopleListByAccountNumber(AgencyCode, accountSeqNumber);

                return trustAccounts;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Trust Account List by public user sequence ID.
        /// </summary>
        /// <param name="userSeqNum">public user sequence Number</param>
        /// <returns>Trust Account information list.</returns>
        /// <exception cref="DataValidateException">{ <c>userSeqNum</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        IList<TrustAccountModel> ITrustAccountBll.GetTrustAccountListByPublicUserID(string userSeqNum)
        {
            if (userSeqNum == null)
            {
                throw new DataValidateException(new string[] { "userSeqNum" });
            }

            try
            {
                IList<TrustAccountModel> trustAccounts = TrustAccountService.getTrustAccountListByPublicUserID(AgencyCode, userSeqNum);

                return trustAccounts;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Trust Account List by cap ID.
        /// </summary>
        /// <param name="capID">Cap ID model for web services</param>
        /// <returns>Trust Account information list.</returns>
        /// <exception cref="DataValidateException">{ <c>CapIDModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        IList<TrustAccountModel> ITrustAccountBll.GetTrustAccountListByCAPID(CapIDModel capID)
        {
            if (capID == null)
            {
                throw new DataValidateException(new string[] { "CapIDModel" });
            }

            try
            {
                string callerID = User != null ? User.PublicUserId : string.Empty;

                IList<TrustAccountModel> trustAccounts = TrustAccountService.getTrustAccountListByCapID(capID, callerID, null);

                return trustAccounts;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets trust account and transaction information by account ID.
        /// </summary>
        /// <param name="accountID">Unique account ID.It is unique in agency level.It is alphanumeric.</param>
        /// <returns>Trust Account information.</returns>
        /// <exception cref="DataValidateException">{ <c>accountID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TrustAccountModel ITrustAccountBll.GetTrustAccountAndTransactionByAccountID(string accountID)
        {
            if (string.IsNullOrEmpty(accountID))
            {
                throw new DataValidateException(new string[] { "accountID" });
            }

            try
            {
                TrustAccountModel response = TrustAccountService.getTrustAccountAndTransactionByAccountID(accountID, AgencyCode);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Trust Account List by license/contact/address/parcel NBR.
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="seqNbr">license/contact/address/parcel Sequence NBR</param>
        /// <param name="capAgency">the cap agency</param>
        /// <param name="asscociatedType">trust account associated type</param>
        /// <returns>Trust account ID list.</returns>
        /// <exception cref="DataValidateException">{ <c>agency code or license is null</c> }</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        string[] ITrustAccountBll.GetTrustAccountListBySeqNbr(string agencyCode, string seqNbr, string capAgency, ACAConstant.PaymentAssociatedType asscociatedType)
        {
            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(seqNbr))
            {
                throw new DataValidateException(new string[] { "agency code or license is null" });
            }

            try
            {
                TrustAccountModel[] trustAccountModels = null;

                switch (asscociatedType)
                {
                    case ACAConstant.PaymentAssociatedType.Licenses:
                        trustAccountModels =
                        TrustAccountService.getTrustAccountListByLicenseSeqNbr(agencyCode, seqNbr);
                        break;
                    case ACAConstant.PaymentAssociatedType.Contacts:
                        trustAccountModels =
                            TrustAccountService.getTrustAccountListByContactSeqNbr(agencyCode, seqNbr);
                        break;
                    case ACAConstant.PaymentAssociatedType.Addresses:
                        trustAccountModels =
                            TrustAccountService.getTrustAccountListByAddressSeqNbr(agencyCode, seqNbr);
                        break;
                    case ACAConstant.PaymentAssociatedType.Parcels:
                        trustAccountModels =
                            TrustAccountService.getTrustAccountListByParcelSeqNbr(agencyCode, seqNbr);
                        break;
                }

                List<string> trustAccounts = new List<string>();

                if (trustAccountModels != null && trustAccountModels.Length > 0)
                {
                    trustAccounts.AddRange(trustAccountModels.Select(trustAccount => trustAccount.acctID));
                }

                return trustAccounts.ToArray();
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Trust Account List by cap id.
        /// </summary>
        /// <param name="capID">cap id object</param>
        /// <returns>Trust account ID list</returns>
        /// <exception cref="DataValidateException">{ <c>CAP ID is null</c> }</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        string[] ITrustAccountBll.GetTrustAccountIDsByCapID(CapIDModel4WS capID)
        {
            if (capID == null)
            {
                throw new DataValidateException(new string[] { "CAP ID is null" });
            }

            try
            {
                List<string> associatedTypes = new List<string>();
                associatedTypes.Add(ACAConstant.GET_INDIRECT_TRUST_ACCOUNT);
                associatedTypes.Add(ACAConstant.GET_DIRECT_TRUST_ACCOUNT);

                CapIDModel capIDModel = new CapIDModel();
                capIDModel.customID = capID.customID;
                capIDModel.ID1 = capID.id1;
                capIDModel.ID2 = capID.id2;
                capIDModel.ID3 = capID.id3;
                capIDModel.serviceProviderCode = capID.serviceProviderCode;
                capIDModel.trackingID = capID.trackingID;

                string callerID = User != null ? User.PublicUserId : string.Empty;

                TrustAccountModel[] trustAccountModels =
                    TrustAccountService.getTrustAccountListByCapID(capIDModel, callerID, associatedTypes.ToArray());

                List<string> trustAccounts = new List<string>();

                if (trustAccountModels != null && trustAccountModels.Length > 0)
                {
                    foreach (TrustAccountModel trustAccount in trustAccountModels)
                    {
                        trustAccounts.Add(trustAccount.acctID);
                    }
                }

                return trustAccounts.ToArray();
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Current User's Trust Account List
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="license">user license.</param>
        /// <returns>Trust Account information list.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TrustAccountModel[] ITrustAccountBll.GetTrustAccountList(string servProvCode, LicenseModel4WS license)
        {
            try
            {
                TrustAccountModel trustAccountModel = new TrustAccountModel();
                trustAccountModel.servProvCode = servProvCode;
                trustAccountModel.acctStatus = ACCOUNT_STATUS_ACTIVE;

                TrustAccountPeopleModel4WS people4WS = new TrustAccountPeopleModel4WS();
                people4WS.email = license.emailAddress;
                people4WS.firstName = license.contactFirstName;
                people4WS.lastName = license.contactLastName;
                people4WS.peopleType = PEOPLE_TYPE_LICENSED_PEOPLE;
                people4WS.licNbr = license.stateLicense;
                TrustAccountModel[] tams = TrustAccountService.getTrustAccountListByCondition(trustAccountModel, people4WS);

                return tams;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Trust Account List by condition.
        /// </summary>
        /// <param name="trustAccountModel">Trust AccountModel for web service</param>
        /// <param name="people4WS">Trust AccountPeopleModel for web service</param>
        /// <returns>Trust Account information list</returns>
        /// <exception cref="DataValidateException">{ <c>trustAccountModel, people4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TrustAccountModel[] ITrustAccountBll.GetTrustAccountListByCondition(TrustAccountModel trustAccountModel, TrustAccountPeopleModel4WS people4WS)
        {
            if (trustAccountModel == null || people4WS == null)
            {
                throw new DataValidateException(new string[] { "trustAccountModel", "people4WS" });
            }

            try
            {
                TrustAccountModel[] response = TrustAccountService.getTrustAccountListByCondition(trustAccountModel, people4WS);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// judge if current user has trust account
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="license">user license.</param>
        /// <returns>true if the current user has trust account,otherwise return false.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        bool ITrustAccountBll.UserHasTrustAccount(string servProvCode, LicenseModel4WS license)
        {
            try
            {
                if (license == null)
                {
                    return false;
                }

                TrustAccountModel[] tams = ((ITrustAccountBll)this).GetTrustAccountList(servProvCode, license);

                return tams != null && tams.Length > 0;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets trust account information by account ID.
        /// </summary>
        /// <param name="accountID">Unique account ID.It is unique in agency level.It is alphanumeric.</param>
        /// <param name="servProvCode">Service provider code.</param>
        /// <returns>Trust Account information.</returns>
        /// <exception cref="DataValidateException">{ <c>accountID, servProvCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TrustAccountModel ITrustAccountBll.GetTrustAccountByAccountID(string accountID, string servProvCode)
        {
            if (string.IsNullOrEmpty(accountID) || string.IsNullOrEmpty(servProvCode))
            {
                throw new DataValidateException(new string[] { "accountID", "servProvCode" });
            }

            try
            {
                TrustAccountModel response = TrustAccountService.getTrustAccountByAccountID(accountID, servProvCode);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}
