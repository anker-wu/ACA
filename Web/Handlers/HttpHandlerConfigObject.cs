#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlRoutingConfig.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Url routing config element.
*
*  Notes:
* $Id: UrlRoutingConfig.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// the type of handler.
    /// </summary>
    public enum ServiceType
    {
        /// <summary>
        /// Default type
        /// </summary>
        Default = 0,

        /// <summary>
        /// Url Routing Handler
        /// </summary>
        UrlRoutingHandler = 1000,

        /// <summary>
        /// File Handler
        /// </summary>
        FileHandler = 1001
    }

    /// <summary>
    /// url routing config element.
    /// </summary>
    public class HttpHandlerConfigObject
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The service type.</value>
        public ServiceType Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the reflection.
        /// </summary>
        /// <value>The type of the reflection.</value>
        public string ReflectionType { get; set; }

        /// <summary>
        /// Gets or sets the service Url.
        /// </summary>
        /// <value>The service page.</value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the service parameters.
        /// </summary>
        /// <value>The service parameters.</value>
        public string Params { get; set; }
    }
}