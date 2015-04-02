#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProviderBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ProviderBll.cs 141673 2009-07-31 07:27:31Z ACHIEVO\weiky.chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This class provide the ability to operation daily side provider.
    /// </summary>
    public class ProviderBll : BaseBll, IProviderBll
    {
        #region Fields
   
        #endregion Fields

        /// <summary>
        /// Gets an instance of ProviderService.
        /// </summary>
        private ProviderWebServiceService ProviderService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ProviderWebServiceService>();
            }
        }

        #region Method

        /// <summary>
        /// Get Provider by providerPKModel
        /// </summary>
        /// <param name="providerPKModel">a providerPKModel</param>
        /// <returns>A ProviderModel.</returns>
        ProviderModel4WS IProviderBll.GetProviderByPK(ProviderPKModel4WS providerPKModel)
        {
            return ProviderService.getProviderByPK(providerPKModel);
        }

        /// <summary>
        /// Get Provider List
        /// </summary>
        /// <param name="providerModel">A ProviderModel4WS</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>Provider Array.</returns>
        ProviderModel4WS[] IProviderBll.GetProviderList(ProviderModel4WS providerModel, QueryFormat queryFormat)
        {
            return ProviderService.getProviderList(providerModel, queryFormat);
        }

        /// <summary>
        /// Get Provider List By User Sequence Number
        /// </summary>
        /// <param name="servProvCode">Server Provider Code</param>
        /// <param name="userSeqNbr">User Sequence Number</param>
        /// <returns>ProviderModel4WS Array.</returns>
        ProviderModel4WS[] IProviderBll.GetProviderListByUserSeqNbr(string servProvCode, string userSeqNbr)
        {
            return ProviderService.getProviderListByUserSeqNbr(servProvCode, userSeqNbr);
        }

        /// <summary>
        /// Gets the provider fee items.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="entityID">The entity ID.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="providerNum">The provider number.</param>
        /// <returns>Fee DSEC array</returns>
        RefFeeDsecVO[] IProviderBll.GetProviderFeeItems(string servProvCode, string entityID, string entityType, string providerNum)
        {
            return ProviderService.getProviderFeeItems(servProvCode, entityID, entityType, providerNum);
        }

        /// <summary>
        /// Determines whether [is external exam provider] [the specified agency code].
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="providerNbr">The provider NBR.</param>
        /// <returns><c>true</c> if [is external exam provider] [the specified agency code]; otherwise, <c>false</c>.</returns>
        bool IProviderBll.IsExternalExamProvider(string servProvCode, string providerNbr)
        {
            return ProviderService.isExternalExamProvider(servProvCode, providerNbr);
        }

        /// <summary>
        /// Gets the providers by exam NBR.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="examNbr">The exam NBR.</param>
        /// <returns>Provider information</returns>
        ProviderModel[] IProviderBll.GetProvidersByExamNbr(string servProvCode, string examNbr)
        {
            return ProviderService.getProvidersByExamNbr(examNbr, servProvCode);
        }

        #endregion Method
    }
}
