#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FacebookFriend.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

namespace Accela.ACA.Web.SocialMedia
{
    /// <summary>
    /// The Facebook friend information
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class FacebookFriend
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
        /// Gets or sets a value indicating whether this <see cref="FacebookFriend"/> is installed.
        /// </summary>
        /// <value><c>true</c> if installed; otherwise, <c>false</c>.</value>
        [System.Runtime.Serialization.DataMember(Name = "installed")]
        public bool Installed { get; set; }
    }
}