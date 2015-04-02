#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DataFilterBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: DataFilterBll.cs 247358 2014-02-17 03:05:45Z ACHIEVO\eric.he $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using ACAWebService.DataFilterWebService;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to support data filter.
    /// </summary>
    public class DataFilterBll : IDataFilterBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of DataFilterWebServiceService.
        /// </summary>
        /// <value>The data filter web service.</value>
        private DataFilterWebServiceService DataFilterWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<DataFilterWebServiceService>();
            }
        }

        #endregion Properties

        /// <summary>
        /// get the XDataFilterModel collection by View Id.
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="viewId">View Id</param>
        /// <param name="module">Module Name</param>
        /// <param name="datafilterType">the data filter's type, it's "DATAFILTER" or "QUICKQUERY"</param>
        /// <param name="callerId">The caller id</param>
        /// <returns>data filter objects</returns>
        public XDataFilterModel[] GetXDataFilterByViewId(string servProvCode, long viewId, string module, string datafilterType, string callerId)
        {
            return DataFilterWebService.getXDataFilterByViewId(servProvCode, viewId, module, datafilterType, callerId);
        }

        /// <summary>
        /// get the XDataFilterElementModel collection by data filter's id.
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="viewId">View Id</param>
        /// <param name="dataFilterId">the data filter's id.</param>
        /// <param name="callerId">The caller id</param>
        /// <returns>data filter element objects</returns>
        public XDataFilterElementModel[] GetXDataFilterElementByDataFilterId(string servProvCode, long viewId, long dataFilterId, string callerId)
        {
            return DataFilterWebService.getXDataFilterElementByDataFilterId(servProvCode, viewId, dataFilterId, callerId);
        }
    }
}