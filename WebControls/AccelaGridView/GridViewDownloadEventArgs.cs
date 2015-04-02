/**
 * <pre>
 *
 *  Accela
 *  File: GridViewDownloadEventArgs.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GridViewDownloadEventArgs.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/10/2007    daly.zeng    Initial.
 * </pre>
 */

using System;
using System.Collections.Generic;

namespace Accela.Web.Controls
{
    /// <summary>
    /// grid view download event argument
    /// </summary>
    public class GridViewDownloadEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the export parameters.
        /// </summary>
        /// <value>The export parameters.</value>
        public List<ExportParameter> ExportParameters
        {
            get;
            set;
        }
    }
}
