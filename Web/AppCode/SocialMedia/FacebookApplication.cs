#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FacebookUser.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;

namespace Accela.ACA.Web.SocialMedia
{
    /// <summary>
    /// The Facebook Application
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    [Serializable]
    public class FacebookApplication
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        [System.Runtime.Serialization.DataMember(Name = "id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [System.Runtime.Serialization.DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        [System.Runtime.Serialization.DataMember(Name = "category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the subcategory.
        /// </summary>
        /// <value>The subcategory.</value>
        [System.Runtime.Serialization.DataMember(Name = "subcategory")]
        public string Subcategory { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        [System.Runtime.Serialization.DataMember(Name = "link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        [System.Runtime.Serialization.DataMember(Name = "namespace")]
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the icon_url.
        /// </summary>
        /// <value>The icon_url.</value>
        [System.Runtime.Serialization.DataMember(Name = "icon_url")]
        public string IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the logo_url.
        /// </summary>
        /// <value>The logo_url.</value>
        [System.Runtime.Serialization.DataMember(Name = "logo_url")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// Gets or sets the weekly_active_users.
        /// </summary>
        /// <value>The weekly_active_users.</value>
        [System.Runtime.Serialization.DataMember(Name = "weekly_active_users")]
        public string WeeklyActiveUsers { get; set; }

        /// <summary>
        /// Gets or sets the monthly_active_users.
        /// </summary>
        /// <value>The monthly_active_users.</value>
        [System.Runtime.Serialization.DataMember(Name = "monthly_active_users")]
        public string MonthlyActiveUsers { get; set; }
    }
}