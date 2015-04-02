#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionDataModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2012
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionDataModel.cs 223068 2012-06-25 08:30:54Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Inspection
{
    /// <summary>
    /// inspection data model
    /// </summary>
    [Serializable]
    public class InspectionDataModel : InspectionTypeDataModel
    {
        /// <summary>
        /// Gets or sets the original model.
        /// </summary>
        /// <value>The inspection model.</value>
        public InspectionModel InspectionModel
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
        /// Gets or sets inspection request comments
        /// </summary>
        public string RequestComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspection result comments
        /// </summary>
        public string ResultComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets requested date time
        /// </summary>
        public DateTime? RequestedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets scheduled date time
        /// </summary>
        public DateTime? ScheduledDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets resulted date time
        /// </summary>
        public DateTime? ResultedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ready time
        /// </summary>
        public DateTime? ReadyTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether ready time is available ("ready time enabled" + "current status is request pending")).
        /// </summary>
        /// <value><c>true</c> if ready time is available; otherwise, <c>false</c>.</value>
        public bool ReadyTimeAvailable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets inspector
        /// </summary>
        public SysUserModel Inspector
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>The score of inspection.</value>
        public long ? Score
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the guide sheet models.
        /// </summary>
        /// <value>The guide sheet models.</value>
        public GGuideSheetModel[] GuideSheetModels
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
        /// Gets or sets the grade image key.
        /// </summary>
        /// <value>The grade image key.</value>
        public string GradeImageKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [allow view result comments].
        /// </summary>
        /// <value>
        /// <c>true</c> if [allow view result comments]; otherwise, <c>false</c>.
        /// </value>
        public bool AllowViewResultComments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether allow display current inspection in ACA.
        /// </summary>
        /// <value><c>true</c> if allow display current inspection in ACA; otherwise, <c>false</c>.</value>
        public bool AllowDisplayInACA
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
        /// Gets or sets the status string.
        /// get value with priority: 
        /// 1.has resStatus
        /// 2.if is result status and is user defined status
        /// </summary>
        /// <value>The status string.</value>
        public string StatusString
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
        /// Gets or sets the contact phone number.
        /// </summary>
        /// <value>The contact phone number.</value>
        public string ContactPhoneNumber
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
        /// Gets or sets the grade value
        /// </summary>
        ///<value> the grade value</value>
        public string Grade
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
        /// Gets or sets the requestor phone number.
        /// </summary>
        /// <value>The requestor phone number.</value>
        public string RequestorPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the requestor phone IDD.
        /// </summary>
        /// <value>The requestor phone IDD.</value>
        public string RequestorPhoneIDD
        {
            get;
            set;
        }
    }
}
