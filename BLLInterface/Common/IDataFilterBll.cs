#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IDataFilterBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: UserRoleBll.cs 247358 2014-02-17 03:05:45Z ACHIEVO\eric.he $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Interface Data Filter 
    /// </summary>
    public interface IDataFilterBll
    {
        /// <summary>
        /// get the XDataFilterModel collection by View Id.
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="viewId">View Id</param>
        /// <param name="module">Module Name</param>
        /// <param name="datafilterType">the data filter's type, it's "DATAFILTER" or "QUICKQUERY"</param>
        /// <param name="callerId">The caller id</param>
        /// <returns>data filter objects</returns>
        XDataFilterModel[] GetXDataFilterByViewId(string servProvCode, long viewId, string module, string datafilterType, string callerId);

        /// <summary>
        /// get the XDataFilterElementModel collection by data filter's id.
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="viewId">View Id</param>
        /// <param name="dataFilterId">the data filter's id.</param>
        /// <param name="callerId">The caller id</param>
        /// <returns>data filter element objects</returns>
        XDataFilterElementModel[] GetXDataFilterElementByDataFilterId(string servProvCode, long viewId, long dataFilterId, string callerId);
    }
}
