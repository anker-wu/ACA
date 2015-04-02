#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IGlobalSearchBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IGlobalSearchBll.cs 131464 2009-08-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.GlobalSearch
{
    /// <summary>
    /// Global search business interface
    /// </summary>
    public interface IGlobalSearchBll
    {
        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="searchParam">the parameters for global search</param>
        /// <param name="viewElementNames">hidden element name array</param>
        /// <returns>the global search result</returns>
        GlobalSearchResult4WS ExecuteQuery(GlobalSearchParam4WS searchParam, string[] viewElementNames);
    }
}
