#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ITrustAccountBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ITrustAccountBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// Handle Trust Account Payment
    /// </summary>
    public interface ITrustAccountBll
    {
        #region Methods

        /// <summary>
        /// Gets trust account information by trust account's sequence number.
        /// </summary>
        /// <param name="accountSeq">Trust account's sequence number.</param>
        /// <param name="agencyCode">Agency code.</param>
        /// <returns>Trust Account information.</returns>
        TrustAccountModel GetTrustAccountByPK(long accountSeq, string agencyCode);

        /// <summary>
        /// Gets Trust Account List by cap ID.
        /// </summary>
        /// <param name="capID">Cap ID model for web services</param>
        /// <returns>Trust Account information list.</returns>
        IList<TrustAccountModel> GetTrustAccountListByCAPID(CapIDModel capID);

        /// <summary>
        /// Gets Trust Account List by public user sequence ID.
        /// </summary>
        /// <param name="userSeqNum">public user sequence Number</param>
        /// <returns>Trust Account information list.</returns>
        IList<TrustAccountModel> GetTrustAccountListByPublicUserID(string userSeqNum);

        /// <summary>
        /// Gets Trust Account peopleList by cap ID.
        /// </summary>
        /// <param name="accountSeqNumber">trust account id.</param>
        /// <returns>Trust account associate contact information list</returns>
        TrustAccountPeopleModel[] GetTrustAccountPeopleListByAccountNumber(string accountSeqNumber);

        /// <summary>
        /// Gets trust account and transaction information by account ID.
        /// </summary>
        /// <param name="accountID">Unique account ID.It is unique in agency level.It is alphanumeric.</param>
        /// <returns>Trust Account information.</returns>
        TrustAccountModel GetTrustAccountAndTransactionByAccountID(string accountID);

        /// <summary>
        /// Gets Trust Account List by license/contact/address/parcel NBR.
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="seqNbr">license/contact/address/parcel Sequence NBR</param>
        /// <param name="capAgency">the cap agency</param>
        /// <param name="asscociatedType">trust account associated type</param>
        /// <returns>Trust account ID list.</returns>
        string[] GetTrustAccountListBySeqNbr(string agencyCode, string seqNbr, string capAgency, ACAConstant.PaymentAssociatedType asscociatedType);

        /// <summary>
        /// Gets Trust Account List by cap id.
        /// </summary>
        /// <param name="capID">cap id</param>
        /// <returns>Trust account ID list</returns>
        string[] GetTrustAccountIDsByCapID(CapIDModel4WS capID);

        /// <summary>
        /// Get Current User's Trust Account List
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="license">user license.</param>
        /// <returns>Trust Account information list.</returns>
        TrustAccountModel[] GetTrustAccountList(string servProvCode, LicenseModel4WS license);

        /// <summary>
        /// Get Trust Account List by condition.
        /// </summary>
        /// <param name="trustAccountModel"> Trust AccountModel for web service</param>
        /// <param name="people4WS">Trust AccountPeopleModel for web service</param>
        /// <returns>Trust Account information list</returns>
        TrustAccountModel[] GetTrustAccountListByCondition(TrustAccountModel trustAccountModel, TrustAccountPeopleModel4WS people4WS);

        /// <summary>
        /// judge if current user has trust account
        /// </summary>
        /// <param name="servProvCode">agency code.</param>
        /// <param name="license">user license.</param>
        /// <returns>true if the current user has trust account,otherwise return false.</returns>
        bool UserHasTrustAccount(string servProvCode, LicenseModel4WS license);

        /// <summary>
        /// Gets trust account information by account ID.
        /// </summary>
        /// <param name="accountID">Unique account ID.It is unique in agency level.It is alphanumeric.</param>
        /// <param name="servProvCode">Service provider code.</param>
        /// <returns>Trust Account information.</returns>
        TrustAccountModel GetTrustAccountByAccountID(string accountID, string servProvCode);

        /// <summary>
        /// Gets the trust account by user Id for authorized agent.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns>Trust Account information list.</returns>
        TrustAccountModel[] GetTrustAccountByUserID4Agent(string servProvCode, string userID);

        #endregion Methods
    }
}
