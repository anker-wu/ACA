#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Program.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
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
using System.Diagnostics;
using System.Windows.Forms;
using Accela.AuthorizedAgent.Common.Common;
using Accela.AuthorizedAgent.Common.HttpServer;

namespace Accela.AuthorizedAgent.Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            if (!IsAlreadyRunning())
            {
                Log.Instance.LogConfig();
                Log.Instance.Write("Main Program", LogType.Info, "Authorized Agent Client is running, and the current log level is " + AppConfigUtil.LogLevel);

                HttpServer httpServer;

                if (args.GetLength(0) > 0)
                {
                    httpServer = new PrinterServer(Convert.ToInt16(args[0]));
                }
                else
                {
                    // the default tcp port
                    httpServer = new PrinterServer(int.Parse(ConfigManager.ServerPort));
                }
                try
                {
                    Application.SetCompatibleTextRenderingDefault(false);

                    NotificationForm main = new NotificationForm();
                    main.HttpServer = httpServer;
                    main.Start();
                    Application.EnableVisualStyles();
                    Application.Run(main);
                }
                catch (Exception e)
                {
                    Log.Instance.Write("Main Program", LogType.Error, "Application Start failed, the deailed error information: " + e);
                    MessageBox.Show("Application Start failed, the deailed error information: " + e.Message, "Authorized Agent Client");
                }
                finally
                {
                    httpServer.Close();
                }
            }
            else
            {
                MessageBox.Show("Another Authorized Agent Client is running.");
            }
        }

        /// <summary>
        /// Determines whether [is already running].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is already running]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsAlreadyRunning()
        {
            bool isExist = false;
            Process[] process = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);

            if (process.Length > 1)
            {
                isExist = true;
            }

            return isExist;
        }    
    }
}
