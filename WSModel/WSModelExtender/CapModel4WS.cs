#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapModel4WS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapModel4WS.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// APO Condition Type
    /// </summary>
    public enum APOConditionType
    {
        /// <summary>
        /// Address condition
        /// </summary>
        Address,

        /// <summary>
        /// Parcel condition
        /// </summary>
        Parcel
    }

    /// <summary>
    /// Extends the CapModel4WS.
    /// </summary>
    public partial class CapModel4WS
    {
        /// <summary>
        /// Gets or sets a value indicating whether the contacts have been checked for copying record.
        /// </summary>
        public bool IsContactsChecked4Record { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the licenses have been checked for copying record.
        /// </summary>
        public bool IsLicensesChecked4Record { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cap is renewal
        /// </summary>
        public bool IsForRenew { get; set; }

        /// <summary>
        /// Gets or sets a Condition Type for APO.
        /// </summary>
        public APOConditionType APOConditionType { get; set; }
    }
}