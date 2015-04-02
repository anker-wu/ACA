#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOResultModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: APOResultModel.cs 130988 2014-07-15  16:26:01Z ACHIEVO\canon.wu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Web
{
    /// <summary>
    /// APO Result model
    /// </summary>
    public class APOResultModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether check the APO data whether it only has address.
        /// </summary>
        public bool IsOnlyAddress
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether check the APO data whether it only has parcel.
        /// </summary>
        public bool IsOnlyParcel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether check the APO data whether it only has owner.
        /// </summary>
        public bool IsOnlyOwner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether check the APO data whether it only has address or parcel or owner.
        /// </summary>
        public bool IsSingleAPOData
        {
            get;
            set;
        }
    }
}
