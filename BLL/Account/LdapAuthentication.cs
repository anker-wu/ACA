#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: LdapAuthentication.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: A business class to support LDAP authentication.
*
*  Notes:
* $Id: LdapAuthentication.cs 217467 2012-04-18 10:16:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  April 18, 2012   Alan Hu      Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Accela.ACA.Common.Config;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// A business class to support LDAP authentication.
    /// </summary>
    public static class LdapAuthentication
    {
        #region Fields

        /// <summary>
        /// Constant string for LDAPS protocol.
        /// </summary>
        private const string LDAPS_PROTOCOL = "ldaps";

        /// <summary>
        /// LDAP DateTime format.
        /// </summary>
        private const string LDAPDataFormat = "yyyyMMddHHmmssZ";

        /// <summary>
        /// ILog instance.
        /// </summary>
        private static readonly ILog Loger = LogFactory.Instance.GetLogger(typeof(LdapAuthentication));

        /// <summary>
        /// Regular expression pattern for Distinguished Name.
        /// </summary>
        private static readonly Regex RegPattern = new Regex(@"^.+?=.+?(,.+?=.+?)*?$");

        /// <summary>
        /// LDAP DateTime format pattern.
        /// </summary>
        private static readonly Regex LdapDatePattern = new Regex(@"\d{14}Z", RegexOptions.IgnoreCase);

        /// <summary>
        /// Local instance of LDAP configuration analyzer.
        /// </summary>
        private static LdapConfiguration _ldapConfig;

        #endregion

        #region Properties

        /// <summary>
        /// Gets LDAP configuration.
        /// </summary>
        private static LdapConfiguration LdapConfig
        {
            get
            {
                if (_ldapConfig == null)
                {
                    try
                    {
                        //Specify the configuration file path.
                        ExeConfigurationFileMap configFile = new ExeConfigurationFileMap()
                        {
                            ExeConfigFilename = LdapConfiguration.ConfigFilePath
                        };
                        Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
                        _ldapConfig = (LdapConfiguration)config.GetSection(LdapConfiguration.ConfigSectionName);
                    }
                    catch (Exception e)
                    {
                        LogError(e);
                        throw new ConfigurationErrorsException("LDAP configuration error.", e);
                    }
                }

                return _ldapConfig;
            }
        }

        #endregion

        /// <summary>
        /// Validate the specified credential and retrieve the user information from LDAP server
        /// and fill the user information into a public user model.
        /// </summary>
        /// <param name="userId">User entered user ID.</param>
        /// <param name="password">User entered password.</param>
        /// <returns>Public user model with the user information retrieved from LDAP server.</returns>
        public static PublicUserModel4WS ValidateUser(string userId, string password)
        {
            string ldapServerUrl = LdapConfig.ServerUrl;

            try
            {
                Regex regServerUrl = new Regex(LdapConfiguration.ServerUrlPattern);
                Match matchServerUrl = regServerUrl.Match(ldapServerUrl);

                //Separate the server identifier and protocol from server url.
                string ldapServer = matchServerUrl.Result(LdapConfiguration.ServerVariable);
                string ldapProtocal = matchServerUrl.Result(LdapConfiguration.ProtocolVariable);

                LdapDirectoryIdentifier ldapIdentifier = new LdapDirectoryIdentifier(ldapServer);
                NetworkCredential loginCredential = new NetworkCredential();
                loginCredential.UserName = LdapConfig.BindUser;
                loginCredential.Password = LdapConfig.BindPassword;

                //Create LDAP connection.
                using (LdapConnection ldapConn = new LdapConnection(ldapIdentifier, loginCredential))
                {
                    ldapConn.SessionOptions.ProtocolVersion = 3;

                    //If use LDAPS connection, to initial the certificate verification logic.
                    if (LDAPS_PROTOCOL.Equals(ldapProtocal, StringComparison.OrdinalIgnoreCase))
                    {
                        ldapConn.SessionOptions.SecureSocketLayer = true;
                        ldapConn.SessionOptions.VerifyServerCertificate =
                            new VerifyServerCertificateCallback(VerifyServerCertificate);
                    }

                    /*
                     * If the login user is a distinguished name, needs change the authentication type to Basic.
                     * Default authentication type is Negotiate(Microsoft Negotiate authentication).
                     */
                    if (RegPattern.IsMatch(LdapConfig.BindUser))
                    {
                        ldapConn.AuthType = AuthType.Basic;
                    }

                    //Login  to LDAP server.
                    try
                    {
                        ldapConn.Bind(loginCredential);
                    }
                    catch (Exception e)
                    {
                        _ldapConfig = null;
                        throw new Exception("Failed to login to the LDAP server", e);
                    }

                    //Collect the attributes which are need synchronized from LDAP server.
                    List<string> syncAttributeNames = new List<string>();
                    List<UserMappingAttribute> syncAttributes = new List<UserMappingAttribute>();

                    foreach (UserMappingAttribute attribute in LdapConfig.AttributesMapping)
                    {
                        if (!string.IsNullOrWhiteSpace(attribute.ExternalName))
                        {
                            syncAttributeNames.Add(attribute.ExternalName);
                            syncAttributes.Add(attribute);
                        }
                    }

                    /*
                     * Search the LDAP user by user name and search filter.
                     * And only retrieve these user information which are need synchronized from the LDAP server.
                     */
                    string userSearchFilter = Regex.Replace(LdapConfig.UserFilterString, Regex.Escape(LdapConfiguration.UserIdVariable), userId, RegexOptions.IgnoreCase);
                    SearchRequest searchRequest = new SearchRequest(LdapConfig.BaseDN, userSearchFilter, LdapConfig.SearchScope, syncAttributeNames.ToArray());
                    SearchResponse searchResponse = (SearchResponse)ldapConn.SendRequest(searchRequest);

                    if (searchResponse.Entries.Count == 1)
                    {
                        /*
                         * User is found and search result is unique.
                         */

                        SearchResultEntry userEntry = searchResponse.Entries[0];
                        NetworkCredential userCredential = new NetworkCredential(userEntry.DistinguishedName, password);
                        
                        //Use the Base authentication for DN format credential.
                        ldapConn.AuthType = AuthType.Basic;
                        ldapConn.Bind(userCredential);

                        //Construct public user model and sync the user information based on the search result.
                        PublicUserModel4WS publicUser = BuildUserData(userId, userEntry, syncAttributes);

                        return publicUser;
                    }
                    else
                    {
                        /*
                        * If can not find the user or the search result is not unique means authentication is failed.
                        */
                        throw new Exception("Cannot find the user ID or user ID is not unique.");   
                    }
                }
            }
            catch (ConfigurationErrorsException configErr)
            {
                _ldapConfig = null;
                LogError(configErr);
                throw;
            }
            catch (Exception e)
            {
                LogError(e);
                throw new AuthenticationException(e.Message, e);
            }
        }

        /// <summary>
        /// Construct public user model and sync the user information based on the search result.
        /// </summary>
        /// <param name="userId">User name.</param>
        /// <param name="userEntry">Search result entry.</param>
        /// <param name="syncAttributes">A mapping attribute collection to indicates which attributes need to sync.</param>
        /// <returns>A public user model instance.</returns>
        private static PublicUserModel4WS BuildUserData(string userId, SearchResultEntry userEntry, List<UserMappingAttribute> syncAttributes)
        {
            PublicUserModel4WS publicUser = new PublicUserModel4WS { userID = userId };
            Type publicUserType = publicUser.GetType();

            foreach (UserMappingAttribute attribute in syncAttributes)
            {
                DirectoryAttribute directoryAttribute = userEntry.Attributes[attribute.ExternalName];

                if (directoryAttribute == null)
                {
                    continue;
                }

                object value = directoryAttribute[0];
                PropertyInfo property = publicUserType.GetProperty(attribute.UserModelPropertyName);

                try
                {
                    //PublicUserModel's property is writable.
                    if (property != null && property.CanWrite)
                    {
                        //Special logic for BirthDate field, field name defined in LdapConfiguration section.
                        if (attribute.Name.Equals("BirthDate"))
                        {
                            if (value is string && LdapDatePattern.IsMatch(value as string))
                            {
                                string strValue = (string)value;
                                DateTime dt;

                                if (DateTime.TryParseExact(strValue, LDAPDataFormat, null, DateTimeStyles.None, out dt))
                                {
                                    property.SetValue(publicUser, I18nDateTimeUtil.FormatToDateTimeStringForWebService(dt), null);
                                }
                            }
                        }
                        else if (property.PropertyType.Equals(value.GetType()))
                        {
                            //The data type are the same.
                            if (value is string)
                            {
                                string strValue = (string)value;

                                if (strValue.Length > attribute.MaxLength)
                                {
                                    value = strValue.Substring(0, attribute.MaxLength);
                                }
                            }

                            property.SetValue(publicUser, value, null);
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogError(exp);
                }
            }

            return publicUser;
        }

        /// <summary>
        /// Callback function for verify certificate for LDAPS connection.
        /// </summary>
        /// <param name="connection">LDAP connection.</param>
        /// <param name="certificate">Server side X509 certificate.</param>
        /// <returns>true means verify passed; otherwise, false.</returns>
        private static bool VerifyServerCertificate(LdapConnection connection, X509Certificate certificate)
        {
            try
            {
                X509Certificate expectedCert =
                    X509Certificate.CreateFromSignedFile(LdapConfig.CertificatePath);

                if (expectedCert.Equals(certificate))
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                LogError(e);
                return false;
            }
        }

        /// <summary>
        /// Log the exception and inner exception to error log file.
        /// </summary>
        /// <param name="exception">The exception object.</param>
        private static void LogError(Exception exception)
        {
            if (Loger.IsErrorEnabled)
            {
                Exception exp = exception;

                while (exp.InnerException != null)
                {
                    Loger.Error(exp.InnerException.Message, exp.InnerException);
                    exp = exp.InnerException;
                }

                Loger.Error(exception.Message, exception);
            }
        }
    }
}