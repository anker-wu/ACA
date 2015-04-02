#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PropertyInformationListItem.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: PropertyInformationListItem.cs 130988 2009-8-20  11:06:01Z ACHIEVO\jackie.yu $.
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
    /// Property Information List Item
    /// </summary>
    [Serializable]
    public class APOView4UI
    {
        #region Private Fields

        /// <summary>
        /// Parcel number
        /// </summary>
        private string _parcelNumber;

        /// <summary>
        /// Parcel sequence number
        /// </summary>
        private string _parcelSeqNbr;

        /// <summary>
        /// The owner number
        /// </summary>
        private string _ownerNumber;

        /// <summary>
        /// The owner source number
        /// </summary>
        private string _ownerSeqNumber;

        /// <summary>
        /// The owner name
        /// </summary>
        private string _ownerName;

        /// <summary>
        /// The address number
        /// </summary>
        private string _addressNumber;

        /// <summary>
        /// The address source number
        /// </summary>
        private string _addressSeqNumber;

        /// <summary>
        /// The address full name
        /// </summary>
        private string _addressDesc;

        #endregion

        #region Public Properties

        #endregion

        /// <summary>
        /// Gets or sets the parcel number
        /// </summary>
        [FieldMapping(GviewElementId = "lnkParcelNumberHeader", JavaPropertyName = "parcelNumber", SearchbyByParcelOrder = 1, SearchbyByAddressOrder = 2)]
        public string ParcelNumber
        {
            get { return _parcelNumber; }
            set { _parcelNumber = value; }
        }

        /// <summary>
        /// Gets or sets the parcel sequence number
        /// </summary>
        public string ParcelSeqNbr
        {
            get { return _parcelSeqNbr; }
            set { _parcelSeqNbr = value; }
        }

        /// <summary>
        /// Gets or sets the owner number
        /// </summary>
        public string OwnerSourceNumber
        {
            get { return _ownerNumber; }
            set { _ownerNumber = value; }
        }

        /// <summary>
        /// Gets or sets the owner source number
        /// </summary>
        public string OwnerSeqNumber
        {
            get { return _ownerSeqNumber; }
            set { _ownerSeqNumber = value; }
        }

        /// <summary>
        /// Gets or sets the owner name
        /// </summary>
        [FieldMapping(GviewElementId = "lnkOwnerHeader", JavaPropertyName = "refOwnerName", SearchbyByParcelOrder = 2, SearchbyByAddressOrder = 3)]
        public string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }

        /// <summary>
        /// Gets or sets the address number
        /// </summary>
        public string AddressSourceNumber
        {
            get { return _addressNumber; }
            set { _addressNumber = value; }
        }

        /// <summary>
        /// Gets or sets the address source number
        /// </summary>
        public string AddressSeqNumber
        {
            get { return _addressSeqNumber; }
            set { _addressSeqNumber = value; }
        }

        /// <summary>
        /// Gets or sets the address full name
        /// </summary>
        [FieldMapping(GviewElementId = "lnkAddressHeader", JavaPropertyName = "fullAddress", SearchbyByParcelOrder = 3, SearchbyByAddressOrder = 1)]
        public string AddressDescription
        {
            get { return _addressDesc; }
            set { _addressDesc = value; }
        }
    }
}