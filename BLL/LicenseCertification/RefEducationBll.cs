#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RefEducationBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
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
    /// This interface provide the ability to operation reference side Education.
    /// </summary>
    public class RefEducationBll : BaseBll, IRefEducationBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of RefEducationService.
        /// </summary>
        private RefEducationWebServiceService RefEducationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<RefEducationWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// get reference side education list.
        /// </summary>
        /// <param name="refEducation">reference side education model.</param>
        /// <returns>reference education model list.</returns>
        RefEducationModel4WS[] IRefEducationBll.GetRefEducationList(RefEducationModel4WS refEducation)
        {
            return RefEducationService.getRefEducationList(refEducation);
        }

        /// <summary>
        /// get reference Provider list.
        /// </summary>
        /// <param name="refEducation">reference education model</param>
        /// <returns>reference provider model list</returns>
        ProviderModel4WS[] IRefEducationBll.GetProviderListByRefEducation(RefEducationModel4WS refEducation)
        {
            return RefEducationService.getProviderListByRefEducation(refEducation);
        }

        /// <summary>
        /// Get RefEducation by refEducationPKModel.
        /// </summary>
        /// <param name="refEducationPKModel">a refEducationPKModel</param>
        /// <returns>A RefEducationModel.</returns>
        RefEducationModel4WS IRefEducationBll.GetRefEducationByPK(RefEducationPKModel4WS refEducationPKModel)
        {
            return RefEducationService.getRefEducationByPK(refEducationPKModel);
        }

        /// <summary>
        /// Get RefEducationList by Provider.
        /// </summary>
        /// <param name="providerModel">a providerModel</param>
        /// <param name="capTypeModel">a cap type model</param>
        /// <returns>refEducationModel array.</returns>
        RefEducationModel4WS[] IRefEducationBll.GetRefEducationListByProvider(ProviderModel4WS providerModel, CapTypeModel capTypeModel)
        {
            return RefEducationService.getRefEducationListByProvider(providerModel, capTypeModel);
        }

        /// <summary>
        /// Get Reference Education name list By Name
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="educationName">education Name, pass null or empty will get all education names.</param>
        /// <returns>return key value pair of education list. Key is sequence number, value is education name.</returns>
        MapEntry4WS[] IRefEducationBll.GetRefEducationListByName(string serviceProviderCode, string educationName)
        {
            return RefEducationService.getRefEducationListByName(serviceProviderCode, educationName);
        }

        #endregion Methods
    }
}
