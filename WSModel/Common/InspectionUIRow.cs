#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionUIRow.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionUIRow.cs 209458 2011-12-12 06:03:07Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Html.Inspection
{
    /// <summary>
    /// The entity of inspection data
    /// </summary>
    [Serializable]
    public class InspectionUIRow
    {
        /// <summary>
        /// Inspection history
        /// </summary>
        private List<InspectionUIRow> _inspectionHistory;

        /// <summary>
        /// Gets or sets inspection type
        /// </summary>
        public string InspType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection type label
        /// </summary>
        public string InspTypeLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection group
        /// </summary>
        public string InspGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection status
        /// </summary>
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection Status label
        /// </summary>
        public string StatusLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection comments
        /// </summary>
        public string Comments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets scheduled date
        /// </summary>
        public string Date
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets scheduled date label
        /// </summary>
        public string DateLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether can be updated
        /// </summary>
        public bool CanBeUpdated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspector
        /// </summary>
        public string Inspector
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the switch for cancel operation
        /// </summary>
        public string Switch4CancelAction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether require inspection or not
        /// </summary>
        public string IsRequiredInspection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the schedule type
        /// </summary>
        public string SchedulingMnaner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets operation
        /// </summary>
        public string Operation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second operation
        /// </summary>
        public string SecondOperation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets id number
        /// </summary>
        public string IDNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection time
        /// </summary>
        public string Time
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time label.
        /// </summary>
        /// <value>The time label.</value>
        public string TimeLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets in advance in inspection flow
        /// </summary>
        public string InAdvance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets if in inspection flow
        /// </summary>
        public string InFlow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Iinspection sequence number
        /// </summary>
        public string InspectionSeqNbr
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
        /// Gets or sets is restrict role
        /// </summary>
        public string IsRestrictRole
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection unit
        /// </summary>
        public string InspectUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets user role privilege
        /// </summary>
        public UserRolePrivilegeModel UserRolePrivilege
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the inspections in history can show operation link, if allow multiple inspection
        /// </summary>
        public bool EnabledOperationLink
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets row index
        /// </summary>
        public int RowIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection history
        /// </summary>
        public List<InspectionUIRow> InspectionHistory
        {
            get
            {
                if (_inspectionHistory == null)
                {
                    _inspectionHistory = new List<InspectionUIRow>();
                }

                return _inspectionHistory;
            }

            set
            {
                _inspectionHistory = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether multiple inspections is enabled
        /// </summary>
        public bool MultipleInspectionsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets update time
        /// </summary>
        public System.DateTime UpdateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspectoin result
        /// </summary>
        public string InspectionResult
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets document description
        /// </summary>
        public string DocumentDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets total inspections
        /// </summary>
        public int TotalInspections
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reschedule restriction settings.
        /// </summary>
        public string RescheduleRestrictionSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cancellation restriction settings.
        /// </summary>
        public string CancellationRestrictionSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready time enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ready time enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyTimeEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is ready time visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ready time visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadyTimeVisible
        {
            get;
            set;
        }
    }
}