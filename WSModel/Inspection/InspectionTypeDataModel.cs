#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionTypeDataModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionTypeDataModel.cs 194095 2011-03-29 12:17:11Z ACHIEVO\hans.shi $.
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

namespace Accela.ACA.Inspection
{
    /// <summary>
    /// inspection type data model
    /// </summary>
    [Serializable]
    public class InspectionTypeDataModel
    {
        /// <summary>
        /// Gets or sets the inspection type model.
        /// </summary>
        /// <value>The inspection type model.</value>
        public InspectionTypeModel InspectionTypeModel
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
        /// Gets or sets inspection type
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection group
        /// </summary>
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection type's units
        /// </summary>
        public double Units
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection status
        /// </summary>
        public InspectionStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether required inspection or not
        /// </summary>
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the schedule type
        /// </summary>
        public InspectionScheduleType ScheduleType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets available operations
        /// </summary>
        public InspectionAction[] AvailableOperations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the main action.
        /// </summary>
        /// <value>The main action.</value>
        public InspectionAction MainAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cancel action.
        /// </summary>
        /// <value>The cancel action.</value>
        public InspectionAction CancelAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is in advance.
        /// </summary>
        /// <value><c>true</c> if [in advance]; otherwise, <c>false</c>.</value>
        public bool InAdvance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets result group
        /// </summary>
        public string ResultGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether multiple inspections is enabled
        /// </summary>
        public bool AllowMultiple
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets last updated time
        /// </summary>
        public DateTime LastUpdated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets last updated by (user)
        /// </summary>
        public string LastUpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reschedule restriction settings.
        /// </summary>
        public string RestrictionSettings4Reschedule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cancellation restriction settings.
        /// </summary>
        public string RestrictionSettings4Cancel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready time enabled ("request only pending" + "ready time enabled is set to 'yes'").
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ready time enabled; otherwise, <c>false</c>.
        /// </value>
        public bool ReadyTimeEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is actionable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is actionable; otherwise, <c>false</c>.
        /// </value>
        public bool Actionable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The categories.</value>
        public InspectionCategoryDataModel[] Categories
        {
            get;
            set;
        }
    }
}
