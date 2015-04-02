#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IRefExaminationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IRefExaminationBll.cs 140043 2009-07-21 06:09:54Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation daily side examination.
    /// </summary>
    public interface IRefExaminationBll
    {
        /// <summary>
        /// Get ProviderList by RefExamModel.
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        /// <returns>ProviderModel array</returns>
        ProviderModel4WS[] GetProviderListByRefExam(RefExaminationModel4WS refExaminationModel);

        /// <summary>
        /// Get refExaminationModel list by providerModel.
        /// </summary>
        /// <param name="providerModel">a providerModel</param>
        /// <param name="capTypeModel">a cap type model</param>
        /// <returns>refExaminationModel array</returns>
        RefExaminationModel4WS[] GetRefExaminationListByProvider(ProviderModel4WS providerModel, CapTypeModel capTypeModel);

        /// <summary>
        /// Get RefExamination by PK.
        /// </summary>
        /// <param name="refExaminationPKModel">a refExaminationPKModel</param>
        /// <returns>A RefExaminationModel</returns>
        RefExaminationModel4WS GetRefExaminationByPK(RefExaminationPKModel4WS refExaminationPKModel);

        /// <summary>
        /// Get RefExamination List.
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        /// <returns>RefExaminationModel array</returns>
        RefExaminationModel4WS[] GetRefExaminationList(RefExaminationModel4WS refExaminationModel);

        /// <summary>
        /// Get Ref Examination models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref Examination model array</returns>
        RefExaminationModel4WS[] GetRefExaminationList(CapTypeModel capType);

        /// <summary>
        /// Get RefExamination List, and filter by work flow restriction
        /// </summary>
        /// <param name="refExaminationModel">a refExaminationModel</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="from">where are request from.</param>
        /// <returns>RefExaminationModel array</returns>
        RefExaminationModel4WS[] GetRefExaminationListFilterByWFRestriction(RefExaminationModel4WS refExaminationModel,  CapIDModel capID, string from);
        
        /// <summary>
        /// Get Ref Examination models by examinationName , providerName, service provider code
        /// </summary>
        /// <param name="examinationName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="capType">cap type</param>
        /// <returns>ref examination model array</returns>
        RefExaminationModel4WS[] GetRefExaminationList(string examinationName, string providerName, string serviceProviderCode, CapTypeModel capType);

        /// <summary>
        /// Get Ref provider models by examinationName , providerName.service provider code
        /// </summary>
        /// <param name="examinationName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <returns>ref provider model array</returns>
        ProviderModel4WS[] GetRefProviderList(string examinationName, string providerName, string serviceProviderCode);

        /// <summary>
        /// Determines whether [is beyond allowance date] [the specified examination].
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="providerNo">The provider no.</param>
        /// <param name="examStartDate">The exam start date.</param>
        /// <param name="refExamNbr">The ref exam NBR.</param>
        /// <returns><c>true</c> if [is beyond allowance date] [the specified examination]; otherwise, <c>false</c>.</returns>
        bool IsBeyondAllowanceDate(string agencyCode, string providerNo, DateTime? examStartDate, long? refExamNbr);

        /// <summary>
        /// Get Reference examination provider model
        /// </summary>
        /// <param name="examName">examination name</param>
        /// <param name="providerName">provider name</param>
        /// <returns>Examination and provider relationship information</returns>
        XRefExaminationProviderModel GetRefExamProviderModel(string examName, string providerName);

        /// <summary>
        /// Get Reference Examination name List By Name.
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="examName">examination Name, pass null or empty will get all examination names.</param>
        /// <returns>return key value pair of examination list. Key is sequence number, value is examination name.</returns>
        MapEntry4WS[] GetRefExaminationListByName(string serviceProviderCode, string examName);
    }
}
