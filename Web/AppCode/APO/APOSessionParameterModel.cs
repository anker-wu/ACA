#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOSessionParameterModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Provide a class save the parameters of APO in session.
 *
 *  Notes:
 *      $Id: APOSessionParameterModel.cs 170366 2014-06-19 05:34:25Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Provide a class save the parameters of APO in session.
    /// </summary>
    [Serializable]
    public class APOSessionParameterModel
    {
        /// <summary>
        /// Gets or sets callback function name callback to refresh base form when success to operate.
        /// </summary>
        public string CallbackFunctionName { get; set; }

        /// <summary>
        /// Gets or sets the object such as RefAddressModel of search criteria.
        /// </summary>
        public object SearchCriterias { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate for address
        /// </summary>
        public bool IsValidate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether address list is from map.
        /// </summary>
        public bool IsFromMap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether create cap from map or not
        /// </summary>
        public bool IsCreateCapFromGIS { get; set; }

        /// <summary>
        /// Gets or sets selected address information.
        /// </summary>
        public AddressModel SelectedAddress { get; set; }

        /// <summary>
        /// Gets or sets selected parcel information.
        /// </summary>
        public ParcelModel SelectedParcel { get; set; }

        /// <summary>
        /// Gets or sets selected owner information.
        /// </summary>
        public OwnerModel SelectedOwner { get; set; }

        /// <summary>
        /// Gets or sets external Owner for Super Agency
        /// </summary>
        public RefOwnerModel ExternalOwnerForSuperAgency { get; set; }

        /// <summary>
        /// Gets or sets External Owner for Super Agency
        /// </summary>
        public ParcelModel ExternalParcelForSuperAgency { get; set; }

        /// <summary>
        /// Gets or sets the information such as RefAddressModel/ParcelModel with condition to show the condition.
        /// </summary>
        public object ConditionInfo { get; set; }

        /// <summary>
        /// Gets or sets error message when page load.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
