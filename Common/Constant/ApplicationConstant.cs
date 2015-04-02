#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ApplicationConstant.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: A static class to define Application constant variable keys.
*
* </pre>
*/

#endregion

namespace Accela.ACA.Common
{
    /// <summary>
    /// A static class to define Application constant variable keys.
    /// </summary>
    public static class ApplicationConstant
    {
        /// <summary>
        /// Define the application name to cache the UrlRoutingHandlerConfig.
        /// </summary>
        public const string URLROUTING_HANDLER_CONFIG = "UrlRoutingHandlerConfig";

        /// <summary>
        /// The multiple service data from deep link for multiple records creation.
        /// </summary>
        public const string SERVICE_DATA_FROM_RECORDS_CREATION_DEEPLINK = "ServiceDataFromRecordsCreationDeepLink";

        /// <summary>
        /// The Authorized Agent Config
        /// </summary>
        public const string AUTHORIZED_AGENT_CONFIG = "AuthorizedAgentConfig";
    }
}