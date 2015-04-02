#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IProviderBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IProviderBll.cs 131477 2009-05-20 02:41:19Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// Interface define for Provider
    /// </summary>
    public interface IProviderBll
    {
        #region Methods

        /// <summary>
        /// Get Provider List
        /// </summary>
        /// <param name="providerModel">A ProviderModel4WS</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>Provider Array.</returns>
        ProviderModel4WS[] GetProviderList(ProviderModel4WS providerModel, QueryFormat queryFormat);

        /// <summary>
        /// Get Provider by providerPKModel
        /// </summary>
        /// <param name="providerPKModel4WS">a providerPKModel4WS</param>
        /// <returns>A ProviderModel4WS.</returns>
        ProviderModel4WS GetProviderByPK(ProviderPKModel4WS providerPKModel4WS);

        /// <summary>
        /// Get Provider List By User Sequence Number
        /// </summary>
        /// <param name="servProvCode">Server Provider Code</param>
        /// <param name="userSeqNbr">User Sequence Number</param>
        /// <returns>ProviderModel4WS Array.</returns>
        ProviderModel4WS[] GetProviderListByUserSeqNbr(string servProvCode, string userSeqNbr);

        /// <summary>
        /// Gets the provider fee items.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="entityID">The entity ID.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="providerNum">The provider number.</param>
        /// <returns>Fee DSEC array</returns>
        RefFeeDsecVO[] GetProviderFeeItems(string servProvCode, string entityID, string entityType, string providerNum);

        /// <summary>
        /// Determines whether [is external exam provider] [the specified agency code].
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="providerNbr">The provider NBR.</param>
        /// <returns><c>true</c> if [is external exam provider] [the specified agency code]; otherwise, <c>false</c>. </returns>
        bool IsExternalExamProvider(string servProvCode, string providerNbr);

        /// <summary>
        /// Gets the providers by exam NBR.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="examNbr">The exam NBR.</param>
        /// <returns>Provider information</returns>
        ProviderModel[] GetProvidersByExamNbr(string servProvCode, string examNbr);

        #endregion Methods
    }
}
