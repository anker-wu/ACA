/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: DownloadResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: DownloadResultModel.cs 130107 2012-06-20 12:23:56Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.Data;

namespace Accela.ACA.Web.AppCode.Common
{
    /// <summary>
    /// download result model
    /// </summary>
    public class DownloadResultModel
    {
        /// <summary>
        /// Gets or sets the download data source
        /// </summary>
        public DataTable DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start DB row
        /// </summary>
        public int StartDBRow 
        {
            get; 
            set; 
        }
    }
}
