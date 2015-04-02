#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: LdapConfiguration.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: Represents the LdapConfiguration section within a configuration file.
*
*  Notes:
* $Id: LdapConfiguration.cs 217467 2012-04-18 10:51:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  April 18, 2012   Alan Hu      Initial.
* </pre>
*/

#endregion

using System;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Web;

namespace Accela.ACA.Common.Config
{
    /// <summary>
    /// Represents the LDAP Configuration section within a configuration file.
    /// </summary>
    public class LdapConfiguration : ConfigurationSection
    {
        #region Fields

        /// <summary>
        /// A constant string to indicates the section name of the LDAP configuration.
        /// </summary>
        public const string ConfigSectionName = "LdapConfiguration";

        /// <summary>
        /// A constant string to indicates the variable name for the LDAP protocol in the server url regular expression.
        /// </summary>
        public const string ProtocolVariable = "${Protocol}";

        /// <summary>
        /// A constant string to indicates the variable name for the LDAP server identifier in the server url regular expression.
        /// </summary>
        public const string ServerVariable = "${Server}";

        /// <summary>
        /// A constant string to defines the regular expression for the LDAP server url.
        /// </summary>
        public const string ServerUrlPattern = @"^(?<Protocol>(L|l)(D|d)(A|a)(P|p)(S|s)?)://((?<Server>[a-zA-Z0-9\-\.]*(:\d{1,5})?)/?|/)$";

        /// <summary>
        /// A constant string to indicates the user ID variable in the user filter string in the LDAP configuration file.
        /// </summary>
        public const string UserIdVariable = "$$UserID$$";

        /// <summary>
        /// The full path of the LDAP configuration file.
        /// </summary>
        public static readonly string ConfigFilePath = HttpContext.Current.Server.MapPath("~/config/LdapConfiguration.config");

        #endregion

        /// <summary>
        /// Gets the LDAP server url.
        /// </summary>
        [ConfigurationProperty("ServerUrl", DefaultValue = "LDAP:///", IsRequired = true)]
        [RegexStringValidator(ServerUrlPattern)]
        public string ServerUrl
        {
            get
            {
                return (string)this["ServerUrl"];
            }
        }

        /// <summary>
        /// Gets the based distinguished name for user search.
        /// </summary>
        [ConfigurationProperty("BaseDN", IsRequired = true)]
        public string BaseDN
        {
            get
            {
                return (string)this["BaseDN"];
            }
        }

        /// <summary>
        /// Gets the user search scope.
        /// </summary>
        [ConfigurationProperty("SearchScope", DefaultValue = SearchScope.Subtree)]
        public SearchScope SearchScope
        {
            get
            {
                return (SearchScope)this["SearchScope"];
            }
        }

        /// <summary>
        /// Gets the LDAP user filter string.
        /// </summary>
        [ConfigurationProperty("UserFilterString", IsRequired = true)]
        public string UserFilterString
        {
            get
            {
                string filterString = (string)this["UserFilterString"];

                if (filterString.IndexOf(UserIdVariable, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    throw new ConfigurationErrorsException(
                        string.Format("The UserFilterString must include {0}.", UserIdVariable), ConfigFilePath, -1);
                }

                return filterString;
            }
        }

        /// <summary>
        /// Gets the Windows account name or a distinguished name used to bind to the LDAP server before any LDAP operation.
        /// </summary>
        [ConfigurationProperty("BindUser", IsRequired = true)]
        public string BindUser
        {
            get
            {
                return (string)this["BindUser"];
            }
        }

        /// <summary>
        /// Gets the password of the bind user.
        /// </summary>
        [ConfigurationProperty("BindPassword", IsRequired = true)]
        public string BindPassword
        {
            get
            {
                return (string)this["BindPassword"];
            }
        }

        /// <summary>
        /// Gets the full path of the X509 certificate. The certificate used to create encrypted connection with LDAP server.
        /// </summary>
        [ConfigurationProperty("CertificatePath")]
        public string CertificatePath
        {
            get
            {
                return (string)this["CertificatePath"];
            }
        }

        /// <summary>
        /// Gets the mapping attributes for user information synchronization.
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