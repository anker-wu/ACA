#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ITimeZoneBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: ITimeZoneBll.cs 131464 2009-05-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Defines methods for time zone function.
    /// </summary>
    public interface ITimeZoneBll
    {
        #region Methods

        /// <summary>
        /// Gets a string of agency current date 
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code</param>
        /// <returns>agency current date</returns>
        DateTime GetAgencyCurrentDate(string serviceProviderCode);

        #endregion Methods
    }
}