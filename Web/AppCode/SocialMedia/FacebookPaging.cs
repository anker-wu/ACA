#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FacebookPaging.cs
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
    /// The Facebook page link
    /// </summary>
    public class FacebookPaging
    {
        /// <summary>
        /// Gets or sets the next.
        /// </summary>
        /// <value>The next.</value>
        [System.Runtime.Serialization.DataMember(Name = "next")]
        public string Next { get; set; }

        /// <summary>
        /// Gets or sets the previous.
        /// </summary>
        /// <value>The previous.</value>
        [System.Runtime.Serialization.DataMember(Name = "previous")]
        public string Previous { get; set; }
    }
}