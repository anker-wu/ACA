#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IEducationBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation daily side Education.
    /// </summary>
    public interface IEducationBll
    {
        #region Methods

        /// <summary>
        /// Add child model to education model if child model is null.
        /// </summary>
        /// <param name="educations">education models</param>
        void ContructChildModel2Education(EducationModel4WS[] educations);

        /// <summary>
        /// Get Ref education models by cap type.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <returns>ref education model array</returns>
        RefEducationModel4WS[] GetRefEducationModelsByCapType(CapTypeModel capType);

        /// <summary>
        /// Convert RefEducation model array to Education model array.
        /// </summary>
        /// <param name="refEducations">ref education model array</param>
        /// <param name="callId">The call id.</param>
        /// <returns>
        /// education model array
        /// </returns>
        EducationModel4WS[] ConvertRefEducations2Educations(RefEducationModel4WS[] refEducations, string callId);

        #endregion Methos
    }
}
