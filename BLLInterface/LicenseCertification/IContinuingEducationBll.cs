#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IContinuingEducationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IContinuingEducationBll.cs 139167 2009-07-15 06:20:30Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Data;

using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Education;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This interface provide the ability to operation daily side Continuing Education.
    /// </summary>
    public interface IContinuingEducationBll
    {
        #region Methods

        /// <summary>
        /// Get continuing education summary list.
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <param name="contEducations">continuing education model array</param>
        /// <returns>List for continuing education information.</returns>
        IList<ContinuingEducationSummary> GetContEducationSummaryList(CapTypeModel capType, ContinuingEducationModel4WS[] contEducations);

        /// <summary>
        /// Get continue education result passing or not
        /// </summary>
        /// <param name="gradingStyle">grading style</param>
        /// <param name="finalScore">final score</param>
        /// <param name="passingScore">passing score</param>
        /// <returns>Continuing education whether passed</returns>
        bool IsPassedContEdu(string gradingStyle, string finalScore, string passingScore);

        #endregion Methos
    }
}
