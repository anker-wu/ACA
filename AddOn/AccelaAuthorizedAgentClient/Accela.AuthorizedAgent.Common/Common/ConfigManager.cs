#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConfigManager.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
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

using System.Configuration;

namespace Accela.AuthorizedAgent.Common.Common
{
    /// <summary>
    /// Get the config files
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <value>
        /// The IP address.
        /// </value>
        public static string IPAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["IP"];
            }
        }

        /// <summary>
        /// Gets the server port.
        /// </summary>
        /// <value>
        /// The server port.
        /// </value>
        public static string ServerPort
        {
            get
            {
                return ConfigurationManager.AppSettings["Port"];
            }
        }

        /// <summary>
        /// Gets or sets the name of the printer.
        /// </summary>
        /// <value>
        /// The name of the printer.
        /// </value>
        public static string PrinterName
        {
            get
            {
                return ConfigurationManager.AppSettings["PrinterName"];
            }

            set
            {
                bool isModified = false;

                foreach (string key in ConfigurationManager.AppSettings)
                {
                    if (key == "PrinterName")
                    {
                        isModified = true;
                    }
                }

                // Open App.Config of executable
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // You need to remove the old settings object before you can replace it
                if (isModified)
                {
                    config.AppSettings.Settings.Remove("PrinterName");
                }

                // Add an Application Setting.
                config.AppSettings.Settings.Add("PrinterName", value);

                // Save the changes in App.config file.
                config.Save(ConfigurationSaveMode.Modified);

                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
