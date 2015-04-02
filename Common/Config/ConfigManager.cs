#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConfigManager.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: ConfigManager.cs 183096 2010-10-27 01:49:43Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provider a tool to access web.config.
    /// </summary>
    public static class ConfigManager
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the agency code.
        /// </summary>
        public static string AgencyCode
        {
            get
            {
                string agencyCode = ConfigurationManager.AppSettings["ServProvCode"].ToUpper();

                if (HttpContext.Current != null)
                {
                    object isAdmin = HttpContext.Current.Session == null ? null : HttpContext.Current.Session[SessionConstant.SESSION_ADMIN_MODE];

                    if (isAdmin != null && ValidationUtil.IsYes(isAdmin.ToString()))
                    {
                        agencyCode = HttpContext.Current.Session[SessionConstant.SESSION_USER_PREFERRED_AGENCYCODE] == null ? agencyCode : HttpContext.Current.Session[SessionConstant.SESSION_USER_PREFERRED_AGENCYCODE].ToString();
                    }
                    else
                    {
                        HttpRequest currentRequest = HttpContext.Current.Request;
                        agencyCode = string.IsNullOrEmpty(currentRequest.QueryString[UrlConstant.AgencyCode]) ? agencyCode : ScriptFilter.EncodeHtmlEx(currentRequest.QueryString[UrlConstant.AgencyCode]);
                    }
                }

                return agencyCode;
            }
        }

        /// <summary>
        /// Gets Sup Agency Code.
        /// </summary>
        public static string SuperAgencyCode
        {
            get
            {
                return ConfigurationManager.AppSettings["ServProvCode"].ToUpper();
            }
        }

        /// <summary>
        /// Gets the HomePage file name.
        /// </summary>
        public static string HomePage
        {
            get
            {
                return ConfigurationManager.AppSettings["HomePage"];
            }
        }

        /// <summary>
        /// Gets the agency code.
        /// </summary>
        public static string MasterPage
        {
            get
            {
                return ConfigurationManager.AppSettings["MasterPageFile"];
            }
        }

        /// <summary>
        /// Gets the upload temp directory.
        /// </summary>
        public static string TempDirectory
        {
            get
            {
                string tempDir = ConfigurationManager.AppSettings["TempDirectory"];

                if (string.IsNullOrEmpty(tempDir) ||
                    tempDir.Trim() == string.Empty)
                {
                    tempDir = ACAConstant.DEFAULT_TEMP_DIRECTORY;
                }

                return tempDir;
            }
        }

        /// <summary>
        /// Gets the default customization directory
        /// </summary>
        public static string CustomizationDirectory
        {
            get
            {
                string tempDir = ConfigurationManager.AppSettings["CustomizationDirectory"];

                if (string.IsNullOrWhiteSpace(tempDir))
                {
                    tempDir = ACAConstant.DEFAULT_CUSTOMIZATION_DIRECTORY;
                }

                return tempDir + "/" + SuperAgencyCode;
            }
        }

        /// <summary>
        /// Gets the correct protocol to resolve one conflict protocol issue related to load balance env.
        /// </summary>
        public static string Protocol
        {
            get
            {
                if (ValidationUtil.IsTrue(ConfigurationManager.AppSettings["ReplaceToHTTPS"]))
                {
                    return "https";
                }

                return HttpContext.Current.Request.Url.Scheme;
            }
        }

        /// <summary>
        /// Gets LogWhenMethodExecuteTimeExceed settings.
        /// </summary>
        public static int MethodExecuteTimeMin
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["LogWhenMethodExecuteTimeExceed"]);
            }
        }

        /// <summary>
        /// Gets the facebook app id.
        /// </summary>
        /// <value>The facebook app id.</value>
        public static string FacebookAppId
        {
            get
            {
                return ConfigurationManager.AppSettings["FaceBookAppID"];
            }
        }

        /// <summary>
        /// Gets the face book app secret.
        /// </summary>
        /// <value>The face book app secret.</value>
        public static string FaceBookAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["FaceBookAppSecret"];
            }
        }

        /// <summary>
        /// Gets the Session State Cookie name.
        /// Default value is "ASP.NET_SessionId", the value can be customize in system.web/sessionState section.
        /// </summary>
        public static string SessionStateCookieName
        {
            get
            {
                SessionStateSection sessionStateSection =
                    ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;

                if (sessionStateSection != null)
                {
                    return sessionStateSection.CookieName;
                }

                return "ASP.NET_SessionId";
            }
        }

        /// <summary>
        /// Gets the Session State Session Timeout.
        /// The value(minute) can be customize in system.web/sessionState section.
        /// </summary>
        public static int SessionStateTimeout
        {
            get
            {
                SessionStateSection sessionStateSection = ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;

                if (sessionStateSection != null)
                {
                    return Convert.ToInt32(sessionStateSection.Timeout.TotalMinutes);
                }

                return 60;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the config "Default Support Accessibility" is set.
        /// </summary>
        public static bool DefaultSupportAccessibility
        {
            get
            {
                string defaultValue = ConfigurationManager.AppSettings["DefaultSupportAccessibility"];

                return ValidationUtil.IsYes(defaultValue);
            }
        }

        #endregion Properties
    }
}