#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FacebookUser.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
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
    /// Social Media Button Status
    /// </summary>
    public struct SocialMediaButtonStatus
    {
        /// <summary>
        /// All people can see the button
        /// </summary>
        public static string All = "All";

        /// <summary>
        /// record creator can see the button
        /// </summary>
        public static string Creator = "Creator";

        /// <summary>
        /// disable button
        /// </summary>
        public static string None = "None";
    }

    /// <summary>
    /// The facebook user information
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    [Serializable]
    public class FacebookUser
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
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [System.Runtime.Serialization.DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the first_name.
        /// </summary>
        /// <value>The first_name.</value>
        [System.Runtime.Serialization.DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle_name.
        /// </summary>
        /// <value>The middle_name.</value>
        [System.Runtime.Serialization.DataMember(Name = "middle_name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the last_name.
        /// </summary>
        /// <value>The last_name.</value>
        [System.Runtime.Serialization.DataMember(Name = "last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the birthday.
        /// </summary>
        /// <value>The birthday.</value>
        [System.Runtime.Serialization.DataMember(Name = "birthday")]
        public string Birthday { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        [System.Runtime.Serialization.DataMember(Name = "gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the update_time.
        /// </summary>
        /// <value>The update_time.</value>
        [System.Runtime.Serialization.DataMember(Name = "updated_time")]
        public string UpdatedTime { get; set; }
    }
}