#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SelectedContactLicenseCertificationModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  Contact Utilities.
 *
 *  Notes:
 *      $Id: SelectedContactLicenseCertificationModel.cs 144292 2013-11-05 14:09:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy.UI
{
    /// <summary>
    /// Selected Contact License Certification Model
    /// </summary>
    [Serializable]
    public class SelectedContactLicenseCertificationModel
    {
        /// <summary>
        /// Gets or sets the selected educations.
        /// </summary>
        /// <value>
        /// The selected educations.
        /// </value>
        public IList<EducationModel4WS> SelectedEducations { get; set; }

        /// <summary>
        /// Gets or sets the selected examinations.
        /// </summary>
        /// <value>
        /// The selected examinations.
        /// </value>
        public IList<ExaminationModel> SelectedExaminations { get; set; }

        /// <summary>
        /// Gets or sets the selected cont EDU.
        /// </summary>
        /// <value>
        /// The selected cont EDU.
        /// </value>
        public IList<ContinuingEducationModel4WS> SelectedContEdus { get; set; }
    }
}
