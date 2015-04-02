#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IRefContinuingEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IRefContinuingEducationBll.cs 140043 2009-07-21 06:09:54Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation daily side continuing education.
    /// </summary>
    public interface IRefContinuingEducationBll
    {
        /// <summary>
        /// Get Continuing Education by PK.
        /// </summary>
        /// <param name="refContinuingEducationPKModel">a refContinuingEducationPKModel</param>
        /// <returns>a RefContinuingEducationModel</returns>
        RefContinuingEducationModel4WS GetRefContinuingEducationByPK(RefContinuingEducationPKModel4WS refContinuingEducationPKModel);

        /// <summary>
        /// Get Continuing Education model list by provider.
        /// </summary>
        /// <param name="providerModel">a provider information</param>
        /// <param name="capTypeModel">a cap type model</param>
        /// <returns>Continuing Education array</returns>
        RefContinuingEducationModel4WS[] GetRefContEducationListByProvider(ProviderModel4WS providerModel, CapTypeModel capTypeModel);

        /// <summary>
        /// Get ProviderList by Continuing Education.
        /// </summary>
        /// <param name="refContinuingEducationModel">a Continuing Education</param>
        /// <returns>Provider information array </returns>
        ProviderModel4WS[] GetProviderListByRefContEducation(RefContinuingEducationModel4WS refContinuingEducationModel);

        /// <summary>
        /// Get Continuing Education List.
        /// </summary>
        /// <param name="refContinuingEducationModel">a Continuing Education</param>
        /// <returns>Continuing Education array </returns>
        RefContinuingEducationModel4WS[] GetRefContEducationList(RefContinuingEducationModel4WS refContinuingEducationModel);

        /// <summary>
        /// Get Ref continuing education models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref continuing education models</returns>
        RefContinuingEducationModel4WS[] GetRefContEducationsByCapType(CapTypeModel capType);

        /// <summary>
        /// Get Reference continuing education name list By Name
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="contEduName">continuing education Name, pass null or empty will get all continuing education names.</param>
        /// <returns>return key value pair of continuing education list. Key is sequence number, value is continuing education name.</returns>
        MapEntry4WS[] GetRefContEducationListByName(string serviceProviderCode, string contEduName);
    }
}
