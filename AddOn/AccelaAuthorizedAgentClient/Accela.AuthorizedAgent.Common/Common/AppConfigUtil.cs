#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppConfigUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: 
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Xml;

namespace Accela.AuthorizedAgent.Common.Common
{
    public class AppConfigUtil
    {
        /// <summary>
        /// The log level setting
        /// </summary>
        public static string LogLevel
        {
            get
            {
                string logLevel = string.Empty;

                try
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                    XmlNode logLevel4NetNodel = xmlDoc.SelectSingleNode("/configuration/log4net/root/level");

                    if (logLevel4NetNodel != null && logLevel4NetNodel.Attributes["value"] != null)
                    {
                        logLevel = ((logLevel4NetNodel).Attributes["value"]).Value;
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Write("Loading the setting for the log level", LogType.Error, "The detailed info: " + ex);
                }

                return logLevel;
            }
        }

        /// <summary>
        /// Enble or not Debug Level for troubleshooting
        /// </summary>
        /// <param name="debug"></param>
        public static void EnableDebugMode(bool debug)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            XmlNode logLevel4NetNodel = xmlDoc.SelectSingleNode("/configuration/log4net/root/level");

            if (logLevel4NetNodel != null && logLevel4NetNodel.Attributes["value"] != null)
            {
                if (debug)
                {
                    ((logLevel4NetNodel).Attributes["value"]).Value = "DEBUG";
                }
                else
                {
                    ((logLevel4NetNodel).Attributes["value"]).Value = "INFO";
                }
            }

            xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        }
    }
}
