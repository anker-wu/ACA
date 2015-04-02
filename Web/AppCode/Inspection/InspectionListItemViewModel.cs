#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionListItemViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionListItemViewModel.cs 182945 2010-10-22 08:22:49Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// inspection list item view model
    /// </summary>
    [Serializable]
    public class InspectionListItemViewModel
    {
        /// <summary>
        /// Gets or sets inspection view model.
        /// </summary>
        /// <value>the inspection view model.</value>
        public InspectionViewModel InspectionViewModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is upcoming inspection.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is upcoming inspection; otherwise, <c>false</c>.
        /// </value>
        public bool IsUpcomingInspection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the combined info.
        /// </summary>
        /// <value>The combined info.</value>
        public string CombinedInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the available actions.
        /// </summary>
        /// <value>The available actions.</value>
        public InspectionActionViewModel[] AvailableActions
        {
            get;
            set;
        }
    }
}
