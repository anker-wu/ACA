#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionViewModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2012
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionViewModel.cs 223068 2012-06-25 08:30:54Z ACHIEVO\daly.zeng $.
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
    /// Inspection View Model
    /// </summary>
    [Serializable]
    public class InspectionViewModel
    {
        /// <summary>
        /// Gets or sets the inspection data model.
        /// </summary>
        /// <value>The inspection data model.</value>
        public InspectionDataModel InspectionDataModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection sequence number
        /// </summary>
        public long ID
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
        /// Gets or sets inspection group
        /// </summary>
        public string Group
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
        /// Gets or sets the status date time.
        /// </summary>
        /// <value>The status date time.</value>
        public DateTime? StatusDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status date time text.
        /// </summary>
        /// <value>The status date time text.</value>
        public string StatusDateTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status date text.
        /// </summary>
        /// <value>The status date text.</value>
        public string StatusDateText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the status time text.
        /// </summary>
        /// <value>The status time text.</value>
        public string StatusTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection request comments string
        /// </summary>
        public string RequestComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection result comments string
        /// </summary>
        public string ResultComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time option.
        /// </summary>
        /// <value>The time option.</value>
        public InspectionTimeOption TimeOption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requested date time.
        /// </summary>
        /// <value>The requested date time.</value>
        public DateTime? RequestedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requested date time text.
        /// </summary>
        /// <value>The requested date time text.</value>
        public string RequestedDateTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requested date text.
        /// </summary>
        /// <value>The requested date text.</value>
        public string RequestedDateText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requested time text.
        /// </summary>
        /// <value>The requested time text.</value>
         public string RequestedTimeText
        {
            get;
            set;
        }

         /// <summary>
         /// Gets or sets the scheduled date time.
         /// </summary>
         /// <value>The scheduled date time.</value>
         public DateTime? ScheduledDateTime
         {
             get;
             set;
         }

         /// <summary>
         /// Gets or sets the scheduled date time text.
         /// </summary>
         /// <value>The scheduled date time text.</value>
        public string ScheduledDateTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scheduled date text.
        /// </summary>
        /// <value>The scheduled date text.</value>
         public string ScheduledDateText
        {
            get;
            set;
        }

         /// <summary>
         /// Gets or sets the scheduled time text.
         /// </summary>
         /// <value>The scheduled time text.</value>
        public string ScheduledTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resulted date time.
        /// </summary>
        /// <value>The resulted date time.</value>
        public DateTime? ResultedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resulted date time text.
        /// </summary>
        /// <value>The resulted date time text.</value>
        public string ResultedDateTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resulted date text.
        /// </summary>
        /// <value>The resulted date text.</value>
        public string ResultedDateText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resulted time text.
        /// </summary>
        /// <value>The resulted time text.</value>
        public string ResultedTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ready time.
        /// </summary>
        /// <value>The ready time.</value>
        public DateTime? ReadyTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ready time date time text.
        /// </summary>
        /// <value>The ready time date time text.</value>
        public string ReadyTimeDateTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ready time date text.
        /// </summary>
        /// <value>The ready time date text.</value>
        public string ReadyTimeDateText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ready time time text.
        /// </summary>
        /// <value>The ready time time text.</value>
        public string ReadyTimeTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspector string
        /// </summary>
        public string Inspector
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether inspector is unassigned.
        /// </summary>
        public bool IsInspectorUnassigned
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
        /// Gets or sets a value indicating whether [contact visible].
        /// </summary>
        /// <value><c>true</c> if [contact visible]; otherwise, <c>false</c>.</value>
        public bool ContactVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of the contact.
        /// </summary>
        /// <value>The full name of the contact.</value>
        public string ContactFullName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name of the contact.
        /// </summary>
        /// <value>The first name of the contact.</value>
        public string ContactFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the contact middle.
        /// </summary>
        /// <value>The name of the contact middle.</value>
        public string ContactMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name of the contact.
        /// </summary>
        /// <value>The last name of the contact.</value>
        public string ContactLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact full phone number (with IDD).
        /// </summary>
        /// <value>The contact full phone number (with IDD).</value>
        public string ContactFullPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact phone IDD.
        /// </summary>
        /// <value>The contact phone IDD.</value>
        public string ContactPhoneIDD
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact phone number.
        /// </summary>
        /// <value>The contact phone number.</value>
        public string ContactPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated date time.
        /// </summary>
        /// <value>The last updated date time.</value>
        public DateTime LastUpdatedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated date time text.
        /// </summary>
        /// <value>The last updated date time text.</value>
        public string LastUpdatedDateTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated date text.
        /// </summary>
        /// <value>The last updated date text.</value>
        public string LastUpdatedDateText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated time text.
        /// </summary>
        /// <value>The last updated time text.</value>
        public string LastUpdatedTimeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last updated by (user name).
        /// </summary>
        /// <value>The last updated by.</value>
        public string LastUpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score.</value>
        public long? Score
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the score text.
        /// </summary>
        /// <value>The score text.</value>
        public string ScoreText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the major violations.
        /// </summary>
        /// <value>The major violations.</value>
        public Dictionary<string, string> MajorViolations
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the major violations text.
        /// </summary>
        /// <value>The major violations text.</value>
        public string MajorViolationsText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the grade image key.
        /// </summary>
        /// <value>The grade image key.</value>
        public string GradeImageKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the grade image description.
        /// </summary>
        /// <value>The grade image description.</value>
        public string GradeImageDescription
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the grade value.
        /// </summary>
        /// <value>The grade value</value>
        public string Grade
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Result value.
        /// </summary>
        /// <value>The Result value</value>
        public string Result
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name of the requestor.
        /// </summary>
        /// <value>The first name of the requestor.</value>
        public string RequestorFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the middle name of the requestor.
        /// </summary>
        /// <value>The middle name of the requestor.</value>
        public string RequestorMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name of the requestor.
        /// </summary>
        /// <value>The last name of the requestor.</value>
        public string RequestorLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requestor's phone number.
        /// </summary>
        /// <value>The requestor's phone number.</value>
        public string RequestorPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requestor's phone IDD.
        /// </summary>
        /// <value>The requestor's phone IDD.</value>
        public string RequestorPhoneIDD
        {
            get;
            set;
        }
    }
}
