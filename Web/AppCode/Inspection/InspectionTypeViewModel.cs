#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionTypeViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionTypeViewModel.cs 190825 2011-02-22 06:34:30Z ACHIEVO\daly.zeng $.
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
    /// Inspection type View Model
    /// </summary>
    [Serializable]
    public class InspectionTypeViewModel
    {
        /// <summary>
        /// Gets or sets the inspection type data model.
        /// </summary>
        /// <value>The inspection type data model.</value>
        public InspectionTypeDataModel InspectionTypeDataModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection type ID
        /// </summary>
        public long TypeID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection type string
        /// </summary>
        public string TypeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection group string
        /// </summary>
        public string GroupText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection Status string
        /// </summary>
        public string StatusText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets required or optional string
        /// </summary>
        public string RequiredOrOptional
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InspectionTypeViewModel"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InspectionTypeViewModel"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show flow completed icon].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show flow completed icon]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowFlowCompletedIcon
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show flow schedule active icon].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show flow schedule active icon]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowFlowScheduleActiveIcon
        {
            get;
            set;
        }
    }
}
