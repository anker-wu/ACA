#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionListItemViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionRelatedItemViewModel.cs 193521 2011-03-23 06:52:00Z ACHIEVO\hans.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// related inspection item view model.
    /// </summary>
    [Serializable]
    public class InspectionRelatedItemViewModel
    {
        /// <summary>
        /// Gets or sets inspection sequence number.
        /// </summary>
        /// <value>The inspection ID.</value>
        public string InspectionID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection name.
        /// </summary>
        /// <value>The name of the inspection.</value>
        public string InspectionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets relationship with other inspection.
        /// </summary>
        /// <value>The relation ship.</value>
        public string RelationShip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection status text.
        /// </summary>
        /// <value>The status.</value>
        public string StatusText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target URL.
        /// </summary>
        /// <value>The target URL.</value>
        public string TargetURL
        {
            get;
            set;
        }
    }
}
