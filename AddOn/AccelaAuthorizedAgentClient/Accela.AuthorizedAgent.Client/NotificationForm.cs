#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: NotificationForm.cs
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
using System.Threading;
using System.Windows.Forms;
using Accela.AuthorizedAgent.Common.Common;
using Accela.AuthorizedAgent.Common.HttpServer;

namespace Accela.AuthorizedAgent.Client
{
    /// <summary>
    /// Notification Form
    /// </summary>
    public partial class NotificationForm : Form
    {
        #region Fields

        /// <summary>
        /// The HTTP server
        /// </summary>
        private HttpServer httpServer;

        /// <summary>
        /// The thread
        /// </summary>
        private Thread thread;

        /// <summary>
        /// The is proxy open
        /// </summary>
        private bool isProxyOpen = false;

        /// <summary>
        /// The is printer setting window open
        /// </summary>
        private bool isPrinterOpen = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the HTTP server.
        /// </summary>
        /// <value>
        /// The HTTP server.
        /// </value>
        public HttpServer HttpServer
        {
            get
            {
                return httpServer;
            }

            set
            {
                httpServer = value;
            }
        }

        /// <summary>
        /// Gets or sets the background thread.
        /// </summary>
        /// <value>
        /// The background thread.
        /// </value>
        public Thread BackgroundThread
        {
            get
            {
                return thread;
            }

            set
            {
                thread = value;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationForm"/> class.
        /// </summary>
        public NotificationForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (httpServer != null)
            {
                httpServer.onShowMessage += new HttpServer.ShowMessage(ShowMessage);
                httpServer.Listen();
            }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void ShowMessage(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HttpServer.ShowMessage(DisplayMessage), new object[] { message });
            }
        }

        /// <summary>
        /// Displays the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DisplayMessage(string message)
        {
            if (messageBox.Items.Count > 50)
            {
                messageBox.Items.Clear();
            }

            messageBox.Items.Add(message);

            Log.Instance.Write("Application Message", LogType.Error, message);
        }

        /// <summary>
        /// Get currect assembly version
        /// </summary>
        /// <returns>
        /// assembly version
        /// </returns>
        private string GetCurrentVer()
        {
            string ver = "V0.0.0.0";

            try
            {
                var version = this.GetType().Assembly.GetName().Version;
                ver = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
            catch (Exception exception)
            {
                this.ShowMessage(exception.Message);
            }

            return ver;
        }

        /// <summary>
        /// Handles the Resize event of the NotificationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void NotificationForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                // if the form has been minimised
                this.Hide(); // hide the form
                this.ntfIcon.Visible = true; // display the tray icon
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the NotificationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void NotificationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        /// <summary>
        /// Handles the Load event of the NotificationForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void NotificationForm_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.Text = "Accela Authorized Agent Client - " + GetCurrentVer();

            // Set the current log level
            if ("DEBUG".Equals(AppConfigUtil.LogLevel, StringComparison.OrdinalIgnoreCase))
            {
                this.menuEnableDebug.Checked = true;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuItem4 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuItem4_Click(object sender, EventArgs e)
        {
            try
            {
                this.httpServer.Close();
                this.ntfIcon.Visible = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Log.Instance.Write("Application Message", LogType.Error, ex);
            }
            finally
            {
                Environment.Exit(Environment.ExitCode);
            }
        }

        /// <summary>
        /// Handles the Click event of the menuProxySetting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuProxySetting_Click(object sender, EventArgs e)
        {
            if (!isProxyOpen)
            {
                ProxySetting proxySetting = new ProxySetting();
                proxySetting.Closed += new EventHandler(proxySetting_Closed);
                proxySetting.Show();
                isProxyOpen = true;
            }
        }

        /// <summary>
        /// Handles the Closed event of the proxySetting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void proxySetting_Closed(object sender, EventArgs e)
        {
            isProxyOpen = false;
        }

        /// <summary>
        /// Handles the Click event of the menuPrinterSetting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuPrinterSetting_Click(object sender, EventArgs e)
        {
            if (!isPrinterOpen)
            {
                PrinterSetting printerSetting = new PrinterSetting();
                printerSetting.Closed += new EventHandler(printerSetting_Closed);
                printerSetting.Show();
                isPrinterOpen = true;
            }
        }

        /// <summary>
        /// Handles the Closed event of the printerSetting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void printerSetting_Closed(object sender, EventArgs e)
        {
            isPrinterOpen = false;
        }

        /// <summary>
        /// Handles the Click event of the menuStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void menuStatus_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// Handles the Click event of the menuEnableDebug control
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">instance containing the event data.</param>
        private void menuEnableDebug_Click(object sender, EventArgs e)
        {
            if (!this.menuEnableDebug.Checked)
            {
                this.menuEnableDebug.Checked = true;
            }
            else
            {
                this.menuEnableDebug.Checked = false;
            }

            AppConfigUtil.EnableDebugMode(this.menuEnableDebug.Checked);
			
            MessageBox.Show("Please restart authorized agent client after enable or disable debug mode.", "Authorized Agent Client");
        }

        #endregion
    }
}
