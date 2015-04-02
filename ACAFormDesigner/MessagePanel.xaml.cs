/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MessagePanel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Accela.ACA.FormDesigner
{
	public partial class MessagePanel : UserControl
	{
        /// <summary>
        /// Construction method
        /// </summary>
		public MessagePanel()
		{
			// Required to initialize variables
			InitializeComponent();
            this.BtnYes.Click += new RoutedEventHandler(BtnYes_Click);
		}

        /// <summary>
        /// method for handle ok button logic
        /// </summary>
        /// <param name="sender">object of sender</param>
        /// <param name="e">RoutedEventArgs object</param>
        void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            this.TxtMessage.Text = string.Empty;
        }

        /// <summary>
        /// method for show message
        /// </summary>
        /// <param name="message">message string</param>
        public void ShowMessage(string message)
        {
            this.Loading.Visibility = System.Windows.Visibility.Collapsed;
            this.Loading.Show();
            this.BtnYes.Visibility = System.Windows.Visibility.Visible;
            this.TxtMessage.Text = message;
            this.MessageGrid.Visibility = System.Windows.Visibility.Visible;
            this.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// method for show waiting 
        /// </summary>
        public void ShowWaiting()
        {
            this.MessageGrid.Visibility = System.Windows.Visibility.Collapsed;
            this.Loading.Visibility = System.Windows.Visibility.Visible;
            this.Loading.Show();
            this.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// method for close message or waiting box.
        /// </summary>
        public void Close()
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            this.Loading.Close();
            this.TxtMessage.Text = string.Empty;
        }
	}
}