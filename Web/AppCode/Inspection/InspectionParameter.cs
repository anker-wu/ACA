#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionParameter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionParameter.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// inspection parameter
    /// </summary>
    public class InspectionParameter
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
        /// Gets or sets the agency code.
        /// </summary>
        /// <value>The agency code.</value>
        [URLParameter(Keys.AgencyCode)]
        public string AgencyCode
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
        /// Gets or sets the schedule type.
        /// </summary>
        /// <value>the schedule type.</value>
        [URLParameter(Keys.ScheduledType)]
        public InspectionScheduleType ScheduleType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is required; otherwise, <c>false</c>.
        /// </value>
        [URLParameter(Keys.Required)]
        public bool? Required
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ready time enabled].
        /// </summary>
        /// <value><c>true</c> if [ready time enabled]; otherwise, <c>false</c>.</value>
        [URLParameter(Keys.ReadyTimeEnabled)]
        public bool? ReadyTimeEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        [URLParameter(Keys.Action)]
        public InspectionAction Action
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type ID.
        /// </summary>
        /// <value>The type ID.</value>
        [URLParameter(Keys.TypeID)]
        public string TypeID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The inspection type.</value>
        [URLParameter(Keys.Type)]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type text.
        /// </summary>
        /// <value>The type text.</value>
        [URLParameter(Keys.TypeText)]
        public string TypeText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The inspection group.</value>
        [URLParameter(Keys.Group)]
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scheduled date.
        /// </summary>
        /// <value>The scheduled date.</value>
        [URLParameter(Keys.ScheduledDateTime)]
        public DateTime? ScheduledDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end of the scheduled date.
        /// </summary>
        /// <value>The end of the scheduled date.</value>
        [URLParameter(Keys.EndScheduledDateTime)]
        public DateTime? EndScheduledDateTime
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [in advance].
        /// </summary>
        /// <value><c>true</c> if [in advance]; otherwise, <c>false</c>.</value>
        [URLParameter(Keys.InAdvance)]
        public bool? InAdvance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>The inspection ID.</value>
        [URLParameter(Keys.ID)]
        public string ID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the result group.
        /// </summary>
        /// <value>The result group.</value>
        [URLParameter(Keys.ResultGroup)]
        public string ResultGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        [URLParameter(Keys.Units)]
        public double? Units
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reschedule restriction settings.
        /// </summary>
        /// <value>The reschedule restriction settings.</value>
        [URLParameter(Keys.RescheduleRestrictionSettings)]
        public string RescheduleRestrictionSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cancel restriction settings.
        /// </summary>
        /// <value>The cancel restriction settings.</value>
        [URLParameter(Keys.CancelRestrictionSettings)]
        public string CancelRestrictionSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is from wizard page.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is from wizard page; otherwise, <c>false</c>.
        /// </value>
        [URLParameter(Keys.IsFromWizardPage)]
        public bool IsFromWizardPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [URLParameter(Keys.Catagory)]
        public string Catagory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show optional type].
        /// </summary>
        /// <value><c>true</c> if [show optional type]; otherwise, <c>false</c>.</value>
        [URLParameter(Keys.ShowOptionalType)]
        public bool? ShowOptionalType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Contact Visible
        /// </summary>
        /// <value>The contact visible.</value>
        [URLParameter(Keys.ContactVisible)]
        public bool? ContactVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Contact First Name
        /// </summary>
        /// <value>The first name of the contact.</value>
        [URLParameter(Keys.ContactFirstName)]
        public string ContactFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Contact Middle Name
        /// </summary>
        /// <value>The name of the contact middle.</value>
        [URLParameter(Keys.ContactMiddleName)]
        public string ContactMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Contact Last Name
        /// </summary>
        /// <value>The last name of the contact.</value>
        [URLParameter(Keys.ContactLastName)]
        public string ContactLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Contact Phone Number
        /// </summary>
        /// <value>The contact phone number.</value>
        [URLParameter(Keys.ContactPhoneNumber)]
        public string ContactPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Contact Phone IDD
        /// </summary>
        /// <value>The contact phone IDD.</value>
        [URLParameter(Keys.ContactPhoneIDD)]
        public string ContactPhoneIDD
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets contact country code
        /// </summary>
        [URLParameter(Keys.ContactCountryCode)]
        public string ContactCountryCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact full phone number.
        /// </summary>
        /// <value>The contact full phone number.</value>
        [URLParameter(Keys.ContactFullPhoneNumber)]
        public string ContactFullPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the request day.
        /// </summary>
        [URLParameter(Keys.RequestDayOption)]
        public InspectionRequestDayOption RequestDayOption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the request day.
        /// </summary>
        [URLParameter(Keys.ContactChangeOption)]
        public InspectionContactChangeOption ContactChangeOption
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
        /// Gets or sets the inspection status.
        /// </summary>
        [URLParameter(Keys.Status)]
        public InspectionStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the navigation begin page.
        /// </summary>
        /// <value>The navigation begin page.</value>
        [URLParameter(Keys.NavigationBeginPage)]
        public InspectionWizardPageName NavigationBeginPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the time option for the display of ready time/requested date/schedule date.
        /// </summary>
        /// <value>The time option.</value>
        [URLParameter(Keys.TimeOption)]
        public InspectionTimeOption TimeOption
        {
            get;
            set;
        }

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
            /// key of AgencyCode
            /// </summary>
            public const string AgencyCode = "agencyCode";

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
            /// key of action
            /// </summary>
            public const string Action = "Action";

            /// <summary>
            /// key of type id
            /// </summary>
            public const string TypeID = "TypeID";

            /// <summary>
            /// key of type
            /// </summary>
            public const string Type = "Type";

            /// <summary>
            /// key of type text
            /// </summary>
            public const string TypeText = "TypeText";

            /// <summary>
            /// key of group
            /// </summary>
            public const string Group = "Group";

            /// <summary>
            /// key of ScheduledDateTime
            /// </summary>
            public const string ScheduledDateTime = "ScheduledDateTime";

            /// <summary>
            /// key of end of ScheduledDateTime
            /// </summary>
            public const string EndScheduledDateTime = "EndScheduledDateTime";

            /// <summary>
            /// key of InAdvance
            /// </summary>
            public const string InAdvance = "InAdvance";

            /// <summary>
            /// key of inspection ID
            /// </summary>
            public const string ID = "ID";

            /// <summary>
            /// key of ResultGroup
            /// </summary>
            public const string ResultGroup = "ResultGroup";

            /// <summary>
            /// key of Units
            /// </summary>
            public const string Units = "Units";

            /// <summary>
            /// key of ScheduledType
            /// </summary>
            public const string ScheduledType = "ScheduledType";

            /// <summary>
            /// key of Required
            /// </summary>
            public const string Required = "Required";

            /// <summary>
            /// key of ReadyTimeEnabled
            /// </summary>
            public const string ReadyTimeEnabled = "ReadyTimeEnabled";

            /// <summary>
            /// reschedule restriction settings
            /// </summary>
            public const string RescheduleRestrictionSettings = "RescheduleRestrictionSettings";

            /// <summary>
            /// cancel restriction settings
            /// </summary>
            public const string CancelRestrictionSettings = "CancelRestrictionSettings";

            /// <summary>
            /// is from wizard page
            /// </summary>
            public const string IsFromWizardPage = "IsFromWizardPage";

            /// <summary>
            /// the inspection category
            /// </summary>
            public const string Catagory = "Catagory";

            /// <summary>
            /// show the optional type
            /// </summary>
            public const string ShowOptionalType = "ShowOptionalType";

            /// <summary>
            /// Contact Visible
            /// </summary>
            public const string ContactVisible = "ContactVisible";

            /// <summary>
            /// Contact First Name
            /// </summary>
            public const string ContactFirstName = "ContactFirstName";

            /// <summary>
            /// Contact Middle Name
            /// </summary>
            public const string ContactMiddleName = "ContactMiddleName";

            /// <summary>
            /// Contact Last Name
            /// </summary>
            public const string ContactLastName = "ContactLastName";

            /// <summary>
            /// Contact Phone Number
            /// </summary>
            public const string ContactPhoneNumber = "ContactPhoneNumber";

            /// <summary>
            /// Contact Phone IDD
            /// </summary>
            public const string ContactPhoneIDD = "ContactPhoneIDD";

            /// <summary>
            /// Contact Country Code
            /// </summary>
            public const string ContactCountryCode = "ContactCountryCode";

            /// <summary>
            /// Contact full Phone Number
            /// </summary>
            public const string ContactFullPhoneNumber = "ContactFullPhoneNumber";

            /// <summary>
            /// same day / next business day / next available day
            /// </summary>
            public const string RequestDayOption = "RequestDayOption";

            /// <summary>
            /// Contact change option
            /// </summary>
            public const string ContactChangeOption = "ContactChangeOption";

            /// <summary>
            /// The inspection status, like 'PendingByACA'
            /// </summary>
            public const string Status = "Status";

            /// <summary>
            /// The flag which indicates current page is popup or not.
            /// </summary>
            public const string IsPopupPage = UrlConstant.IS_POPUP_PAGE;

            /// <summary>
            /// Navigation begin page
            /// </summary>
            public const string NavigationBeginPage = "NavigationBeginPage";

            /// <summary>
            /// the time option, for time display, e.g. display all day/am/pm/none for request time/schedule time/ready time.
            /// </summary>
            public const string TimeOption = "TimeOption";
        }
    }
}
