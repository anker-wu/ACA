#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IRefEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IRefEducationBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation reference side Education.
    /// </summary>
    public interface IRefEducationBll
    {
        #region Methods

        /// <summary>
        /// get reference Education list.
        /// </summary>
        /// <param name="refEducation">reference education model.</param>
        /// <returns>reference education model list.</returns>
        RefEducationModel4WS[] GetRefEducationList(RefEducationModel4WS refEducation);

        /// <summary>
        /// get reference Provider list.
        /// </summary>
        /// <param name="refEducation">reference education model</param>
        /// <returns>reference provider model list</returns>
        ProviderModel4WS[] GetProviderListByRefEducation(RefEducationModel4WS refEducation);

        /// <summary>
        /// Get RefEducation by refEducationPKModel.
        /// </summary>
        /// <param name="refEducationPKModel">a refEducationPKModel</param>
        /// <returns>A RefEducationModel.</returns>
        RefEducationModel4WS GetRefEducationByPK(RefEducationPKModel4WS refEducationPKModel);

        /// <summary>
        /// Get RefEducationList by Provider.
        /// </summary>
        /// <param name="providerModel">a providerModel</param>
        /// <param name="capTypeModel">a cap type model</param>
        /// <returns>refEducationModel array.</returns>
        RefEducationModel4WS[] GetRefEducationListByProvider(ProviderModel4WS providerModel, CapTypeModel capTypeModel);

        /// <summary>
        /// Get Reference Education name list By Name
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="educationName">education Name, pass null or empty will get all education names.</param>
        /// <returns>return key value pair of education list. Key is sequence number, value is education name.</returns>
        MapEntry4WS[] GetRefEducationListByName(string serviceProviderCode, string educationName);

        #endregion Methods
    }
}
