#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EducationModel4WS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EducationModel4WS.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// The external for education model
    /// </summary>
    public partial class EducationModel4WS
    {
        /// <summary>
        /// Row Index
        /// </summary>
        public int RowIndex
        {
            get;
            set;
        }

        public string RefEduNbr
        {
            get;
            set;
        }

        /// <summary>
        /// provider detail address1
        /// </summary>
        public string address1
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.address1;
            }
        }

        /// <summary>
        /// provider detail address2
        /// </summary>
        public string address2
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.address2;
            }
        }

        /// <summary>
        /// provider detail address3
        /// </summary>
        public string address3
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.address3;
            }
        }

        /// <summary>
        /// provider detail city
        /// </summary>
        public string city
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.city;
            }
        }

        /// <summary>
        /// provider detail state
        /// </summary>
        public string state
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.state;
            }
        }

        /// <summary>
        /// provider detail zip
        /// </summary>
        public string zip
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.zip;
            }
        }

        /// <summary>
        /// provider detail phone1
        /// </summary>
        public string phone1
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.phone1;
            }
        }

        /// <summary>
        /// provider detail phone1CountryCode
        /// </summary>
        public string phone1CountryCode
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.phone1CountryCode;
            }
        }

        /// <summary>
        /// provider detail phone2
        /// </summary>
        public string phone2
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.phone2;
            }
        }

        /// <summary>
        /// provider detail phone2CountryCode
        /// </summary>
        public string phone2CountryCode
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.phone2CountryCode;
            }
        }

        /// <summary>
        /// provider detail fax
        /// </summary>
        public string fax
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.fax;
            }
        }

        /// <summary>
        /// provider detail faxCountryCode
        /// </summary>
        public string faxCountryCode
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.faxCountryCode;
            }
        }

        /// <summary>
        /// provider detail email
        /// </summary>
        public string email
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.email;
            }
        }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string countryCode
        {
            get
            {
                return providerDetailModel == null ? null : providerDetailModel.countryCode;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [from cap associate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [from cap associate]; otherwise, <c>false</c>.
        /// </value>
        public bool FromCapAssociate { get; set; }
    }
}