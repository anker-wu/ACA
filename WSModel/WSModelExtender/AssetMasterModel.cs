#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetMasterModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AssetMasterModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System.Collections.Generic;

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// Asset master model extender
    /// </summary>
    public partial class AssetMasterModel
    {
        /// <summary>
        /// Gets the county.
        /// </summary>
        /// <value>
        /// The county.
        /// </value>
        public string county
        {
            get
            {
                if (refAddressModel != null)
                {
                    return refAddressModel.county;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the street start.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        public string streetStart
        {
            get
            {
                if (refAddressModel != null)
                {
                    return refAddressModel.houseNumberStart.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the street end.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        public string streetEnd
        {
            get
            {
                if (refAddressModel != null)
                {
                    return refAddressModel.houseNumberEnd.ToString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the country Code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string countryCode
        {
            get
            {
                if (refAddressModel != null)
                {
                    return refAddressModel.countryCode;
                }

                return string.Empty;
            }
        }

        #region Nested Class

        /// <summary>
        /// This class provider the Asset Master Model comparer.
        /// </summary>
        public class Comparer : IEqualityComparer<AssetMasterModel>
        {
            /// <summary>
            /// Define the equal method for comparer.
            /// </summary>
            /// <param name="model1">The Asset Master Model 1.</param>
            /// <param name="model2">The Asset Master Model 2.</param>
            /// <returns>Return true if model1 equal model2.</returns>
            public bool Equals(AssetMasterModel model1, AssetMasterModel model2)
            {
                if (model1.g1AssetID == model2.g1AssetID && model1.g1AssetSequenceNumber == model2.g1AssetSequenceNumber)
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Get hash code.
            /// </summary>
            /// <param name="assetMasterModel">The Asset Master Model.</param>
            /// <returns>The hash code value.</returns>
            public int GetHashCode(AssetMasterModel assetMasterModel)
            {
                return assetMasterModel.GetHashCode();
            }
        }

        #endregion Nested Class
    }
}
