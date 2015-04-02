#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PrinterSetting.cs
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
using System.Collections.Generic;
using System.Management;
using System.Windows.Forms;
using Accela.AuthorizedAgent.Common.Common;

namespace Accela.AuthorizedAgent.Client
{
    /// <summary>
    /// Printer Setting
    /// </summary>
    public partial class PrinterSetting : Form
    {
        #region Fields

        /// <summary>
        /// The query string for printer
        /// </summary>
        private static string QUERY_PRINTER = "Select * from Win32_Printer";

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterSetting"/> class.
        /// </summary>
        public PrinterSetting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the PrinterSetting control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void PrinterSetting_Load(object sender, EventArgs e)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(QUERY_PRINTER);
            ManagementObjectCollection printerList = searcher.Get();
            Dictionary<string, string> items = new Dictionary<string, string>();
            items.Add(string.Empty, string.Empty);

            foreach (ManagementObject printer in printerList)
            {
                string name = printer.Properties["Name"].Value.ToString();
                string driverName = printer.Properties["DriverName"].Value.ToString();

                items.Add(name, driverName);
            }

            cmbPrinters.DataSource = new BindingSource(items, null);
            cmbPrinters.DisplayMember = "Key";
            cmbPrinters.ValueMember = "Value";

            foreach (var item in cmbPrinters.Items)
            {
                KeyValuePair<string, string> keyValuePair = (KeyValuePair<string, string>)item;

                if (keyValuePair.Key.Equals(ConfigManager.PrinterName) || keyValuePair.Value.Equals(ConfigManager.PrinterName))
                {
                    cmbPrinters.SelectedItem = item;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>();

            if (cmbPrinters.SelectedItem is KeyValuePair<string, string>)
            {
                keyValuePair = (KeyValuePair<string, string>)cmbPrinters.SelectedItem;
            }
            
            ConfigManager.PrinterName = keyValuePair.Key;
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
