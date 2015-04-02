/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AnnouncementModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * </pre>
 */

using System;

namespace Accela.ACA.UI.Model
{
    /// <summary>
    /// Announcement Model for Daily
    /// </summary>
    [Serializable]
    public class AnnouncementModel
    {
        /// <summary>
        /// Gets or sets start date for this announcement
        /// </summary>
        public System.Nullable<System.DateTime> StartDate
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets end date for this announcement
        /// </summary>
        public System.Nullable<System.DateTime> EndDate
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets record date
        /// </summary>
        public System.Nullable<System.DateTime> RecDate
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets a value indicating whether the read flag is true or not for this announcement
        /// </summary>
        public bool IsRead
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets audit ID
        /// </summary>
        public long AuditID
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets announcement content title
        /// </summary>
        public string AnnouncementContentTitle
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets announcement agency code
        /// </summary>
        public string AnnouncementAgencyCode
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets announcement content full
        /// </summary>
        public string AnnouncementContentFull
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets announcement content part
        /// </summary>
        public string AnnouncementContentPart
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets announcement content part for list
        /// </summary>
        public string AnnouncementContentPartForList
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets user ID
        /// </summary>
        public string UserID
        {
            get;
            set;
        }
    }
}
