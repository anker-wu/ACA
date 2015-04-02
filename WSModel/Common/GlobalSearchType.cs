#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SearchType.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchType.cs 148017 2009-09-16 08:31:21Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.Common
{
    /// <summary>
    /// Search Type,these enum names can't be changed in order to make consistent with AA's enum.
    /// </summary>
    public enum GlobalSearchType
    {
        /// <summary>
        /// The license professional
        /// </summary>
        LP = 1,

        /// <summary>
        /// The application
        /// </summary>
        CAP = 2,

        /// <summary>
        /// The parcel
        /// </summary>
        PARCEL = 3,

        /// <summary>
        /// the APO address
        /// </summary>
        ADDRESS = 4
    }
}