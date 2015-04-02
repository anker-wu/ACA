#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IViewBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: IViewBll.cs 131464 2009-05-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

namespace Accela.ACA.BLL.View
{
    /// <summary>
    /// This interface's methods most reference label action.
    /// </summary>
    public interface IViewBll
    {
        #region Methods

        /// <summary>
        /// Get all label keys collection.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cultureName">language culture type</param>
        /// <returns>label keys collection</returns>
        Hashtable GetLabelKeys(string agencyCode, string cultureName);

        /// <summary>
        /// Get all label keys collection base.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>label keys collection</returns>
        Hashtable GetLabelKeys(string agencyCode);

        #endregion Methods
    }
}