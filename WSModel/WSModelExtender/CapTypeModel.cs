#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapTypeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapTypeModel.cs 238264 2012-11-20 08:31:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// This is the external for CapTypeModel
    /// </summary>
    public partial class CapTypeModel
    {
        /// <summary>
        /// Get the cap type's key
        /// </summary>
        public string ToKey()
        {
            return string.Format("{0}-{1}-{2}-{3}-{4}", serviceProviderCode, group, type, subType, category);
        }
    }
}