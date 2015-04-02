/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionTypeDataRowInfo.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: InspectionTypeDataRowInfo.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using Accela.ACA.Inspection;

namespace Accela.ACA.Web.AppCode.Common
{
    /// <summary>
    /// struct for inspection action code.
    /// </summary>
    public struct InspectionActionColumn
    {
        /// <summary>
        /// Inspection action permission agency
        /// </summary>
        public string InsActionPermissionAgency;

        /// <summary>
        /// Inspection action permission sequence number
        /// </summary>
        public long InsActionPermissionSeqNbr;

        /// <summary>
        /// Inspection action permission enabled.
        /// </summary>
        public bool Enabled;
    }

    /// <summary>
    /// The template row data in inspection action permission setting Grid Control,  key:  inspection type sequence number.
    /// </summary>
    public class InspectionTypeDataRowInfo
    {
        /// <summary>
        /// Gets or sets the Inspection Type sequence number
        /// </summary>
        /// <value>The Inspection Type sequence number.</value>
        public long InsTypeSeqNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Inspection Type
        /// </summary>
        /// <value>The Inspection Type.</value>
        public string InsType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inspection action permission for schedule..
        /// </summary>
        /// <value>The inspection action permission for schedule..</value>
        public InspectionActionColumn Schedule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inspection action permission for reSchedule
        /// </summary>
        /// <value>The inspection action permission for reSchedule.</value>
        public InspectionActionColumn ReSchedule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the inspection action permission for cancel..
        /// </summary>
        /// <value>The inspection action permission for cancel..</value>
        public InspectionActionColumn Cancel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Request or Schedule type.
        /// </summary>
        public InspectionAction RequestOrSchedule
        {
            get;
            set;
        }
    }
}
