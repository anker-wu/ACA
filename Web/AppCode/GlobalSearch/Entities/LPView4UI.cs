#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicensedProfessionalListItem.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: LicensedProfessionalListItem.cs 130988 2009-8-20  10:59:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;

namespace Accela.ACA.Web.Common.GlobalSearch
{
    /// <summary>
    /// Licensed Professional List Item
    /// </summary>
    [Serializable]
    public class LPView4UI
    {
        #region Private Fields

        /// <summary>
        /// Agency code
        /// </summary>
        private string _agencyCode;

        /// <summary>
        /// License number
        /// </summary>
        private string _licenseNumber;

        /// <summary>
        /// License type
        /// </summary>
        private string _licenseType;

        /// <summary>
        /// Res License type
        /// </summary>
        private string _reslicenseType;

        /// <summary>
        /// License profession name
        /// </summary>
        private string _licensedProfessionalName;

        /// <summary>
        /// Business name
        /// </summary>
        private string _businessName;

        #endregion

        #region Public Properties

        #endregion

        /// <summary>
        /// Gets or sets the agency code
        /// </summary>
        public string AgencyCode
        {
            get { return _agencyCode; }
            set { _agencyCode = value; }
        }

        /// <summary>
        /// Gets or sets the license number
        /// </summary>
        [FieldMapping(GviewElementId = "lnkLicenseProHeader", JavaPropertyName = "licenseNumber", Order = 1)]
        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set { _licenseNumber = value; }
        }

        /// <summary>
        /// Gets or sets the res license type
        /// </summary>
        [FieldMapping(GviewElementId = "lnkLicenseTypeHeader", JavaPropertyName = "licenseType", Order = 2)]
        public string ResLicenseType
        {
            get { return _reslicenseType; }
            set { _reslicenseType = value; }
        }

        /// <summary>
        /// Gets or sets the license type
        /// </summary>       
        public string LicenseType
        {
            get { return _licenseType; }
            set { _licenseType = value; }
        }

        /// <summary>
        /// Gets or sets the license profession name
        /// </summary>
        [FieldMapping(GviewElementId = "lnkLicenseNameHeader", JavaPropertyName = "contact", Order = 3)]
        public string LicensedProfessionalName
        {
            get { return _licensedProfessionalName; }
            set { _licensedProfessionalName = value; }
        }

        /// <summary>
        /// Gets or sets the business name
        /// </summary>
        [FieldMapping(GviewElementId = "lnkBusinessNameHeader", JavaPropertyName = "businessName", Order = 4)]
        public string BusinessName
        {
            get { return _businessName; }
            set { _businessName = value; }
        }
    }
}