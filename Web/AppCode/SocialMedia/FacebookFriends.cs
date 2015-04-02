#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FacebookFriends.cs
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
    /// The Facebook Friend List
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class FacebookFriends
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        [System.Runtime.Serialization.DataMember(Name = "data")]
        public FacebookFriend[] Data { get; set; }

        /// <summary>
        /// Gets or sets the paging.
        /// </summary>
        /// <value>The paging.</value>
        [System.Runtime.Serialization.DataMember(Name = "paging")]
        public FacebookPaging Paging { get; set; }
    }
}