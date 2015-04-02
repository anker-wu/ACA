#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PeopleModel4WS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PeopleModel4WS.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// The external for people model
    /// </summary>
    public partial class PeopleModel4WS
    {
        /// <summary>
        /// Gets or sets a JSON string indicating the special parameters of the self or the other object 
        /// such as <see cref="PeopleModel"/>, <see cref="OwnerModel"/>, <see cref="LicenseModel4WS"/>. 
        /// Used in Contact edit for conditions.
        /// </summary>
        public string ConditionParameters
        {
            get;
            set;
        }

        public int? RowIndex
        {
            get;
            set;
        }
    }
}
