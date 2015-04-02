#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProxySetting.cs
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

using System;
using System.Windows.Forms;
using Accela.AuthorizedAgent.Common.Common;
using Accela.AuthorizedAgent.Common.Setting;

namespace Accela.AuthorizedAgent.Client
{
    /// <summary>
    /// Proxy Setting
    /// </summary>
    public partial class ProxySetting : Form
    {
        /// <summary>
        /// The setting path
        /// </summary>
        private static string settingPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "AccelaDocProxySetting.xml";

        /// <summary>
        /// The server setting
        /// </summary>
        private ProxyServerSetting serverSetting = SerializationUtil.ConfigureProxySetting();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxySetting"/> class.
        /// </summary>
        public ProxySetting()
        {
            InitializeComponent();
            initializeSettings();
        }

        /// <summary>
        /// load proxy setting from setting configuration xml
        /// </summary>
        private void initializeSettings()
        {
            serverIP.Text = serverSetting.ServerIP;
            port.Text = serverSetting.Port;
            userName.Text = serverSetting.UserName;
            password.Text = serverSetting.Password;
            domain.Text = serverSetting.Domain;
            auth_chk.Checked = serverSetting.IsNeedAuthorized;
            byPass_chk.Checked = serverSetting.IsByPassLocalAddr;
            if (!serverSetting.IsUsingProxy)
            {
                disable(serverSetting);
                directConn.Checked = true;
            }
            else
            {
                proxyConn.Checked = true;
                enable(serverSetting);
            }
        }

        /// <summary>
        /// Disables the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        private void disable(ProxyServerSetting setting)
        {
            lblProxy.Enabled = false;
            lblServerIP.Enabled = false;
            lblPort.Enabled = false;
            lblUserName.Enabled = false;
            lblPwd.Enabled = false;
            serverIP.Enabled = false;
            port.Enabled = false;
            userName.Enabled = false;
            password.Enabled = false;
            lblDirect.Enabled = true;
            domain.Enabled = false;
            lblDomain.Enabled = false;
            byPass_chk.Enabled = false;
            auth_chk.Enabled = false;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the proxyRadio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void proxyRadio_CheckedChanged(object sender, EventArgs e)
        {
            enable(serverSetting);
        }

        /// <summary>
        /// Enables the specified setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        private void enable(ProxyServerSetting setting)
        {
            lblDirect.Enabled = false;
            lblProxy.Enabled = true;
            lblServerIP.Enabled = true;
            lblPort.Enabled = true;
            lblUserName.Enabled = true;
            lblPwd.Enabled = true;
            serverIP.Enabled = true;
            port.Enabled = true;
            auth_chk.Enabled = true;
            byPass_chk.Enabled = true;
            changeAuthStatus();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the directConn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void directConn_CheckedChanged(object sender, EventArgs e)
        {
            disable(serverSetting);
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        /// <summary>
        /// save the setting and serialize to AccelaDocProxySetting.xml then dispose the form
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            ProxyServerSetting serverSetting = SerializationUtil.ConfigureProxySetting();
            string strServerIP = serverIP.Text;
            string strPort = port.Text;
            if (!directConn.Checked)
            {
                if (validate(strServerIP, strPort))
                {
                    serverSetting.ServerIP = strServerIP;
                    serverSetting.Port = strPort;
                    serverSetting.UserName = userName.Text;
                    serverSetting.Password = password.Text;
                    serverSetting.IsUsingProxy = true;
                    serverSetting.Domain = domain.Text;
                    serverSetting.IsNeedAuthorized = auth_chk.Checked;
                    serverSetting.IsByPassLocalAddr = byPass_chk.Checked;
                    SerializationUtil.XmlSerializeToFile(serverSetting, settingPath);
                    MessageBox.Show("Saved successfully.");
                    base.Close();
                }
            }
            else
            {
                serverSetting.IsUsingProxy = false;
                SerializationUtil.XmlSerializeToFile(serverSetting, settingPath);
                MessageBox.Show("Saved successfully.");
                base.Close();
            }
        }

        /// <summary>
        /// validate the proxy server setting for field serverip and port
        /// </summary>
        /// <param name="serverIP">The server IP.</param>
        /// <param name="port">The port.</param>
        /// <returns>
        /// validate resulst
        /// </returns>
        private bool validate(string serverIP, string port)
        {
            string ipPattrn = "^(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(serverIP, ipPattrn))
            {
                MessageBox.Show("IP format error.");
                return false;
            }
            string portPattern = @"^[-]?\d+[.]?\d*$";
            if (System.Text.RegularExpressions.Regex.IsMatch(port, portPattern))
            {
                if (!(int.Parse(port) > 0) || !(int.Parse(port) < 65535))
                {
                    MessageBox.Show("Port format error.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Port format error.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// change the status of the authorization
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void auth_chk_CheckedChanged(object sender, EventArgs e)
        {
            changeAuthStatus();
        }

        /// <summary>
        /// Changes the authorized status.
        /// </summary>
        private void changeAuthStatus()
        {
            bool isNeedAtuh = auth_chk.Checked;
            if (isNeedAtuh)
            {
                lblDomain.Enabled = true;
                lblUserName.Enabled = true;
                lblPwd.Enabled = true;
                userName.Enabled = true;
                password.Enabled = true;
                domain.Enabled = true;
            }
            else
            {
                lblDomain.Enabled = false;
                lblUserName.Enabled = false;
                lblPwd.Enabled = false;
                userName.Enabled = false;
                password.Enabled = false;
                domain.Enabled = false;
            }
        }
    }
}
