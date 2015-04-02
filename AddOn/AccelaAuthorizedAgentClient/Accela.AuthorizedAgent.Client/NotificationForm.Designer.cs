namespace Accela.AuthorizedAgent.Client
{
    using System;

    partial class NotificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationForm));
            this.ntfIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmToolMenu = new System.Windows.Forms.ContextMenu();
            this.menuStatus = new System.Windows.Forms.MenuItem();
            this.menuPrinterSetting = new System.Windows.Forms.MenuItem();
            this.menuProxySetting = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuEnableDebug = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.messageBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ntfIcon
            // 
            this.ntfIcon.ContextMenu = this.cmToolMenu;
            this.ntfIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfIcon.Icon")));
            this.ntfIcon.Text = "Accela Authorized Agent Client";
            this.ntfIcon.Visible = true;
            // 
            // cmToolMenu
            // 
            this.cmToolMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuStatus,
            this.menuPrinterSetting,
            this.menuProxySetting,
            this.menuItem3,
            this.menuEnableDebug,
            this.menuExit});
            // 
            // menuStatus
            // 
            this.menuStatus.Index = 0;
            this.menuStatus.Text = "Status";
            this.menuStatus.Click += new System.EventHandler(this.menuStatus_Click);
            // 
            // menuPrinterSetting
            // 
            this.menuPrinterSetting.Index = 1;
            this.menuPrinterSetting.Text = "Printer Setting";
            this.menuPrinterSetting.Click += new System.EventHandler(this.menuPrinterSetting_Click);
            // 
            // menuProxySetting
            // 
            this.menuProxySetting.Index = 2;
            this.menuProxySetting.Text = "Proxy Setting";
            this.menuProxySetting.Click += new System.EventHandler(this.menuProxySetting_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "-";
            // 
            // menuEnableDebug
            // 
            this.menuEnableDebug.Index = 4;
            this.menuEnableDebug.Text = "Debug Mode";
            this.menuEnableDebug.Click += new System.EventHandler(this.menuEnableDebug_Click);
            // 
            // menuExit
            // 
            this.menuExit.Index = 5;
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // messageBox
            // 
            this.messageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageBox.FormattingEnabled = true;
            this.messageBox.Location = new System.Drawing.Point(12, 32);
            this.messageBox.Name = "messageBox";
            this.messageBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.messageBox.Size = new System.Drawing.Size(402, 121);
            this.messageBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Status";
            // 
            // NotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 165);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.messageBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NotificationForm";
            this.ShowInTaskbar = false;
            this.Text = "Status";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NotificationForm_FormClosing);
            this.Load += new System.EventHandler(this.NotificationForm_Load);
            this.Resize += new System.EventHandler(this.NotificationForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ntfIcon;
        private System.Windows.Forms.ContextMenu cmToolMenu;
        private System.Windows.Forms.MenuItem menuPrinterSetting;
        private System.Windows.Forms.MenuItem menuProxySetting;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuExit;
        private System.Windows.Forms.ListBox messageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuItem menuStatus;
        private System.Windows.Forms.MenuItem menuEnableDebug;
    }
}

