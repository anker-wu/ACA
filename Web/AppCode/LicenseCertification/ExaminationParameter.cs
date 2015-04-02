#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationParameter.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination Parameter
    /// </summary>
    public class ExaminationParameter
    {
        /// <summary>
        /// Gets or sets the parameter store key.
        /// </summary>
        /// <value>
        /// The parameter store key.
        /// </value>
        [URLParameter(Keys.ParameterStoreKey)]
        public string ParameterStoreKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record agency code.
        /// </summary>
        /// <value>The record agency code.</value>
        [URLParameter(Keys.RecordAgencyCode)]
        public string RecordAgencyCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the has primary contact.
        /// </summary>
        /// <value>The has primary contact.</value>
        [URLParameter(Keys.HasPrimaryContact)]
        public string HasPrimaryContact
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record contact E-mail.
        /// </summary>
        /// <value>The record contact E mail.</value>
        [URLParameter(Keys.RecordContactEMail)]
        public string RecordContactEMail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first name of the record contact.
        /// </summary>
        /// <value>The first name of the record contact.</value>
        [URLParameter(Keys.RecordContactFirstName)]
        public string RecordContactFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the record contact middle.
        /// </summary>
        /// <value>The name of the record contact middle.</value>
        [URLParameter(Keys.RecordContactMiddleName)]
        public string RecordContactMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last name of the record contact.
        /// </summary>
        /// <value>The last name of the record contact.</value>
        [URLParameter(Keys.RecordContactLastName)]
        public string RecordContactLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of the record contact.
        /// </summary>
        /// <value>The full name of the record contact.</value>
        [URLParameter(Keys.RecordContactFullName)]
        public string RecordContactFullName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record Alt ID (custom ID)
        /// </summary>
        [URLParameter(Keys.RecordAltID)]
        public string RecordAltID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record ID1
        /// </summary>
        [URLParameter(Keys.RecordID1)]
        public string RecordID1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record ID2
        /// </summary>
        [URLParameter(Keys.RecordID2)]
        public string RecordID2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the record ID3
        /// </summary>
        [URLParameter(Keys.RecordID3)]
        public string RecordID3
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        [URLParameter(Keys.ModuleName)]
        public string ModuleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the public user ID.
        /// </summary>
        /// <value>The public user ID.</value>
        [URLParameter(Keys.PublicUserID)]
        public string PublicUserID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the public user ID.
        /// </summary>
        /// <value>The public user ID.</value>
        [URLParameter(Keys.ExaminationName)]
        public string ExaminationName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ref examination NBR.
        /// </summary>
        /// <value>The examination NBR.</value>
        [URLParameter(Keys.ExaminationNbr)]
        public string ExaminationNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the schedule ID.
        /// </summary>
        /// <value>The schedule ID.</value>
        [URLParameter(Keys.ScheduleID)]
        public string ScheduleID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the exam schedule.
        /// </summary>
        /// <value>The name of the exam schedule.</value>
        [URLParameter(Keys.ExamScheduleName)]
        public string ExamScheduleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule provider NBR.
        /// </summary>
        /// <value>The exam schedule provider NBR.</value>
        [URLParameter(Keys.ExamScheduleProviderNbr)]
        public string ExamScheduleProviderNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the exam schedule provider.
        /// </summary>
        /// <value>The name of the exam schedule provider.</value>
        [URLParameter(Keys.ExamScheduleProviderName)]
        public string ExamScheduleProviderName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule provider no. 
        /// it is to Save the provider No. of the reschedule or cancel flow's initialization.
        /// </summary>
        /// <value>The exam schedule provider no.</value>
        [URLParameter(Keys.ExamReScheduleProviderNo)]
        public string ExamReScheduleProviderNo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule provider no.
        /// it is to Save the provider name of the reschedule or cancel flow's initialization.
        /// </summary>
        /// <value>The exam schedule provider no.</value>
        [URLParameter(Keys.ExamReScheduleProviderName)]
        public string ExamReScheduleProviderName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule date.
        /// </summary>
        /// <value>The exam schedule date.</value>
        [URLParameter(Keys.ExamScheduleDate)]
        public string ExamScheduleDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule start time.
        /// </summary>
        /// <value>The exam schedule start time.</value>
        [URLParameter(Keys.ExamScheduleStartTime)]
        public string ExamScheduleStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule end time.
        /// </summary>
        /// <value>The exam schedule end time.</value>
        [URLParameter(Keys.ExamScheduleEndTime)]
        public string ExamScheduleEndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule lang.
        /// </summary>
        /// <value>The exam schedule lang.</value>
        public string ExamScheduleLang
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule location.
        /// </summary>
        /// <value>The exam schedule location.</value>
        [URLParameter(Keys.ExamScheduleLocation)]
        public string ExamScheduleLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule seats.
        /// </summary>
        /// <value>The exam schedule seats.</value>
        [URLParameter(Keys.ExamScheduleSeats)]
        public string ExamScheduleSeats
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule handle cap.
        /// </summary>
        /// <value>The exam schedule handle cap.</value>
        [URLParameter(Keys.ExamScheduleHandlecap)]
        public string ExamScheduleHandlecap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule total fee.
        /// </summary>
        /// <value>The exam schedule total fee.</value>
        [URLParameter(Keys.ExamScheduleTotalFee)]
        public string ExamScheduleTotalFee
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule week day.
        /// </summary>
        /// <value>The exam schedule week day.</value>
        [URLParameter(Keys.ExamScheduleCalendarId)]
        public string ExamScheduleCalendarId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule schedule id.
        /// </summary>
        /// <value>The exam schedule schedule id.</value>
        [URLParameter(Keys.ExamScheduleScheduleId)]
        public string ExamScheduleScheduleId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule location id.
        /// </summary>
        /// <value>The exam schedule location id.</value>
        [URLParameter(Keys.ExamScheduleLocationId)]
        public string ExamScheduleLocationId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the exam schedule week day.
        /// </summary>
        /// <value>The exam schedule week day.</value>
        [URLParameter(Keys.ExamScheduleWeekDay)]
        public string ExamScheduleWeekDay
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is online.
        /// </summary>
        /// <value>The is online.</value>
        [URLParameter(Keys.IsOnline)]
        public string IsOnline
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is online.
        /// </summary>
        /// <value>The is online.</value>
        [URLParameter(Keys.IsExternal)]
        public string IsExternal
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Daily Examination number.
        /// </summary>
        /// <value>The Daily Examination number.</value>
        [URLParameter(Keys.DailyExaminationNbr)]
        public string DailyExaminationNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is reschedule.
        /// </summary>
        /// <value>The is reschedule.</value>
        [URLParameter(Keys.IsReschedule)]
        public string IsReschedule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the accessibility description.
        /// </summary>
        /// <value>The accessibility description.</value>
        public string AccessiblityDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the driving description.
        /// </summary>
        /// <value>The driving description.</value>
        public string DrivingDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Exam Instructions.
        /// </summary>
        /// <value>The driving description.</value>
        public string ExamInstructions
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag which indicates current page is popup or not.
        /// </summary>
        [URLParameter(Keys.IsPopupPage)]
        public string IsPopupPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the is allowed beyond date.
        /// </summary>
        /// <value>The is allowed beyond date.</value>
        [URLParameter(Keys.IsAllowedBeyondDate)]
        public string IsAllowedBeyondDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reason ID.
        /// </summary>
        /// <value>The reason ID.</value>
        [URLParameter(Keys.ReasonID)]
        public string ReasonID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition provider NBR.
        /// </summary>
        /// <value>The condition provider NBR.</value>
        [URLParameter(Keys.ConditionProviderNbr)]
        public string ConditionProviderNbr
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition city.
        /// </summary>
        /// <value>The condition city.</value>
        [URLParameter(Keys.ConditionCity)]
        public string ConditionCity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state of the condition.
        /// </summary>
        /// <value>The state of the condition.</value>
        [URLParameter(Keys.ConditionState)]
        public string ConditionState
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition start time.
        /// </summary>
        /// <value>The condition start time.</value>
        [URLParameter(Keys.ConditionStartTime)]
        public string ConditionStartTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the condition end time.
        /// </summary>
        /// <value>The condition end time.</value>
        [URLParameter(Keys.ConditionEndTime)]
        public string ConditionEndTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ready to schedule status.
        /// </summary>
        /// <value>
        /// The ready to schedule status.
        /// </value>
        [URLParameter(Keys.ReadyToScheduleStatus)]
        public string ReadyToScheduleStatus { get; set; }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        [URLParameter(Keys.Template)]
        public TemplateModel Template { get; set; }

        /// <summary>
        /// inspection queryString keys
        /// </summary>
        public struct Keys
        {
            /// <summary>
            /// parameter store key
            /// </summary>
            public const string ParameterStoreKey = "ParameterStoreKey";

            /// <summary>
            /// key of record agency code
            /// </summary>
            public const string RecordAgencyCode = "agencyCode";

            /// <summary>
            /// HasPrimaryContact true or false
            /// </summary>
            public const string HasPrimaryContact = "HasPrimaryContact";

            /// <summary>
            /// the Record Contact E-Mail (primary contact)
            /// </summary>
            public const string RecordContactEMail = "RecordContactEMail";

            /// <summary>
            /// the Record Contact First Name (primary contact)
            /// </summary>
            public const string RecordContactFirstName = "RecordContactFirstName";

            /// <summary>
            /// the Record Contact last Name (primary contact)
            /// </summary>
            public const string RecordContactLastName = "RecordContactLastName";

            /// <summary>
            /// the Record Contact Full Name (primary contact)
            /// </summary>
            public const string RecordContactFullName = "RecordContactFullName";

            /// <summary>
            /// The record contact middle name (primary contact)
            /// </summary>
            public const string RecordContactMiddleName = "RecordContactMiddleName";

            /// <summary>
            /// the record Alt ID (custom ID)
            /// </summary>
            public const string RecordAltID = "RecordAltID";

            /// <summary>
            /// the record ID1
            /// </summary>
            public const string RecordID1 = "RecordID1";

            /// <summary>
            /// the record ID2
            /// </summary>
            public const string RecordID2 = "RecordID2";

            /// <summary>
            /// the record ID3
            /// </summary>
            public const string RecordID3 = "RecordID3";

            /// <summary>
            /// key of module name
            /// </summary>
            public const string ModuleName = "Module";

            /// <summary>
            /// public user ID
            /// </summary>
            public const string PublicUserID = "PublicUserID";

            /// <summary>
            /// Examination Name
            /// </summary>
            public const string ExaminationName = "ExaminationName";

            /// <summary>
            /// Schedule ID
            /// </summary>
            public const string ScheduleID = "ScheduleID";

            /// <summary>
            /// Exam Schedule Name
            /// </summary>
            public const string ExamScheduleName = "ExamScheduleName";

            /// <summary>
            /// Exam Schedule number
            /// </summary>
            public const string ExaminationNbr = "ExaminationNbr";

            /// <summary>
            /// Exam Schedule Provider number
            /// </summary>
            public const string ExamScheduleProviderNbr = "ExamScheduleProviderNbr";

            /// <summary>
            /// Exam Schedule Provider Name
            /// </summary>
            public const string ExamScheduleProviderName = "ExamScheduleProviderName";

            /// <summary>
            /// Exam Schedule Provider No
            /// </summary>
            public const string ExamReScheduleProviderNo = "ExamReScheduleProviderNo";

            /// <summary>
            /// Exam Schedule Provider Name
            /// </summary>
            public const string ExamReScheduleProviderName = "ExamReScheduleProviderName";

            /// <summary>
            /// Exam Schedule Date
            /// </summary>
            public const string ExamScheduleDate = "ExamScheduleDate";

            /// <summary>
            /// Exam ScheduleStart Time
            /// </summary>
            public const string ExamScheduleStartTime = "ExamScheduleStartTime";

            /// <summary>
            /// Exam Schedule EndTime
            /// </summary>
            public const string ExamScheduleEndTime = "ExamScheduleEndTime";

            /// <summary>
            /// Exam Schedule Location
            /// </summary>
            public const string ExamScheduleLocation = "ExamScheduleLocation";

            /// <summary>
            /// Exam Schedule Seats
            /// </summary>
            public const string ExamScheduleSeats = "ExamScheduleSeats";

            /// <summary>
            /// Exam Schedule Handle cap
            /// </summary>
            public const string ExamScheduleHandlecap = "ExamScheduleHandlecap";

            /// <summary>
            /// Exam Schedule TotalFee
            /// </summary>
            public const string ExamScheduleTotalFee = "ExamSchTotalFeeeduleHandlecap";

            /// <summary>
            /// Exam Schedule Week Day
            /// </summary>
            public const string ExamScheduleWeekDay = "ExamScheduleWeekDay";

            /// <summary>
            /// Exam Schedule ScheduleId
            /// </summary>
            public const string ExamScheduleScheduleId = "ExamScheduleScheduleId";

            /// <summary>
            /// Exam Schedule LocationId
            /// </summary>
            public const string ExamScheduleLocationId = "ExamScheduleLocationId";

            /// <summary>
            /// Exam Schedule CalendarId
            /// </summary>
            public const string ExamScheduleCalendarId = "ExamScheduleCalendarId";

            /// <summary>
            /// Is External Exam online
            /// </summary>
            public const string IsOnline = "IsOnline";

            /// <summary>
            /// Is Online Exam
            /// </summary>
            public const string IsExternal = "IsExternal";

            /// <summary>
            /// on ReSchedule , need existing Daily Examination number
            /// </summary>
            public const string DailyExaminationNbr = "DailyExaminationNbr";

            /// <summary>
            /// Is reschedule?
            /// </summary>
            public const string IsReschedule = "IsReschedule";

            /// <summary>
            /// The flag which indicates current page is popup or not.
            /// </summary>
            public const string IsPopupPage = UrlConstant.IS_POPUP_PAGE;

            /// <summary>
            /// the is allowed beyond date.
            /// </summary>
            public const string IsAllowedBeyondDate = "IsAllowedBeyondDate";

            /// <summary>
            /// the reason id
            /// </summary>
            public const string ReasonID = "ReasonID";

            /// <summary>
            /// the search Condition provider number
            /// </summary>
            public const string ConditionProviderNbr = "ConditionProviderNbr";

            /// <summary>
            /// the search Condition city.
            /// </summary>
            public const string ConditionCity = "ConditionCity";

            /// <summary>
            /// the search Condition state.
            /// </summary>
            public const string ConditionState = "ConditionState";

            /// <summary>
            /// the search Condition start Time.
            /// </summary>
            public const string ConditionStartTime = "ConditionStartTime";

            /// <summary>
            /// the search Condition EndTime
            /// </summary>
            public const string ConditionEndTime = "ConditionEndTime";

            /// <summary>
            /// The Generic template
            /// </summary>
            public const string Template = "Template";

            /// <summary>
            /// The ready to schedule status
            /// </summary>
            public const string ReadyToScheduleStatus = "ReadyToScheduleStatus";
        }
    }
}