#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: OAMConfiguration.cs
*
*  Accela, Inc.
*  Copyright (C): 2012
*
*  Description: Represents the OAMConfiguration section within a configuration file.
*
* </pre>
*/

#endregion

using System.Configuration;
using Accela.ACA.Common.Config;

namespace Accela.ACA.OAMAccessGate
{
    /// <summary>
    /// Represents the OAMConfiguration section within a configuration file.
    /// </summary>
    public class OAMConfiguration : ConfigurationSection
    {
        /// <summary>
        /// Gets the Access Manager SDK install directory without the "/" or "\" suffix.
        /// </summary>
        [ConfigurationProperty("ASDKInstallDir", IsRequired = true)]
        public string ASDKInstallDir
        {
            get
            {
                return (string)this["ASDKInstallDir"];
            }
        }

        /// <summary>
        /// Gets the Host Identifier defined in OAM server to indicates the HTTP resources.
        /// </summary>
        [ConfigurationProperty("HostIdentifier", IsRequired = true)]
        public string HostIdentifier
        {
            get
            {
                return (string)this["HostIdentifier"];
            }
        }

        /// <summary>
        /// Gets the challenge URL of the OAM server. It's should point to the "obrareq.cgi" in OAM server.
        /// </summary>
        [ConfigurationProperty("ChallengeURL", IsRequired = true)]
        public string ChallengeUrl
        {
            get
            {
                return (string)this["ChallengeURL"];
            }
        }

        /// <summary>
        /// Gets the register URL.
        /// </summary>
        [ConfigurationProperty("RegisterURL")]
        public string RegisterUrl
        {
            get
            {
                return (string)this["RegisterURL"];
            }
        }
        
        /// <summary>
        /// Gets the logout URL.
        /// </summary>
        [ConfigurationProperty("LogoutURL")]
        public string LogoutUrl
        {
            get
            {
                return (string)this["LogoutURL"];
            }
        }

        /// <summary>
        /// Gets the mappping attributes for user information synchronization.
        /// </summary>
        [ConfigurationProperty("AttributesMapping", IsRequired = true)]
        public UserMappingAttributeCollection AttributesMapping
        {
            get
            {
                return (UserMappingAttributeCollection)this["AttributesMapping"];
            }
        }
    }
}