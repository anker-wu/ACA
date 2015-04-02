#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseProfessionalWrapperModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseProfessionalWrapperModel.cs 249895 2013-05-22 09:11:21Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.ComponentService.Model
{
    /// <summary>
    /// This class provides one LicenseProfessionalWrapper model for custom component
    /// </summary>
    public class LicenseProfessionalWrapperModel
    {
        /// <summary>
        /// license Professional model
        /// </summary>
        private LicenseProfessionalModel licenseProfessional;

        /// <summary>
        /// Initializes a new instance of the LicenseProfessionalWrapperModel class
        /// </summary>
        /// <param name="licenseProfessional">license Professional</param>
        internal LicenseProfessionalWrapperModel(LicenseProfessionalModel licenseProfessional)
        {
            this.licenseProfessional = licenseProfessional;
        }

        /// <summary>
        /// Gets LicenseNumber
        /// </summary>
        public string LicenseNumber
        {
            get
            {
                return licenseProfessional.licenseNbr;
            }
        }

        /// <summary>
        /// Gets LicenseType
        /// </summary>
        public string LicenseType
        {
            get
            {
                return licenseProfessional.licenseType;
            }
        }
    }
}
