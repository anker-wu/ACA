/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LoadingRunner.cs
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
using System.Windows.Threading;

namespace Accela.ACA.FormDesigner
{
	public partial class LoadingRunner : UserControl
	{
        private DispatcherTimer timer = null;
        /// <summary>
        /// Construction method
        /// </summary>
		public LoadingRunner()
		{
			// Required to initialize variables
			InitializeComponent();
            if (timer == null)
            {
                timer = new DispatcherTimer();
            }

            timer.Interval = new TimeSpan(10);
            timer.Tick += new EventHandler(timer_Tick);
		}

        /// <summary>
        /// Timer tick event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (pbBar.Value >= 100)
            {
                pbBar.Value = 0;
            }
            else
            {
                pbBar.Value += 1;
            }
        }

        /// <summary>
        /// Show Progress bar.
        /// </summary>
        public void Show()
        {
            pbBar.Value = 0;
            timer.Start();
        }

        /// <summary>
        /// close Progress bar.
        /// </summary>
        public void Close()
        {
            pbBar.Value = 100;
            timer.Stop();
        }
        
	}
}