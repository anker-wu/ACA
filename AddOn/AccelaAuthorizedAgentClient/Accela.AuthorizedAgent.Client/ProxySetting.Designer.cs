namespace Accela.AuthorizedAgent.Client
{
    partial class ProxySetting
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
            this.auth_chk = new System.Windows.Forms.CheckBox();
            this.byPass_chk = new System.Windows.Forms.CheckBox();
            this.domain = new System.Windows.Forms.TextBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.lblPwd = new System.Windows.Forms.Label();
            this.userName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.serverIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.lblProxy = new System.Windows.Forms.Label();
            this.proxyConn = new System.Windows.Forms.RadioButton();
            this.lblDirect = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.directConn = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // auth_chk
            // 
            this.auth_chk.AutoSize = true;
            this.auth_chk.Location = new System.Drawing.Point(15, 188);
            this.auth_chk.Name = "auth_chk";
            this.auth_chk.Size = new System.Drawing.Size(162, 17);
            this.auth_chk.TabIndex = 29;
            this.auth_chk.Text = "Use authorization information";
            this.auth_chk.UseVisualStyleBackColor = true;
            this.auth_chk.CheckedChanged += new System.EventHandler(this.auth_chk_CheckedChanged);
            // 
            // byPass_chk
            // 
            this.byPass_chk.AutoSize = true;
            this.byPass_chk.Location = new System.Drawing.Point(15, 164);
            this.byPass_chk.Name = "byPass_chk";
            this.byPass_chk.Size = new System.Drawing.Size(151, 17);
            this.byPass_chk.TabIndex = 28;
            this.byPass_chk.Text = "Bypass for local addresses";
            this.byPass_chk.UseVisualStyleBackColor = true;
            // 
            // domain
            // 
            this.domain.BackColor = System.Drawing.Color.White;
            this.domain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.domain.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domain.Location = new System.Drawing.Point(74, 281);
            this.domain.Name = "domain";
            this.domain.Size = new System.Drawing.Size(240, 22);
            this.domain.TabIndex = 32;
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(27, 285);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(46, 13);
            this.lblDomain.TabIndex = 37;
            this.lblDomain.Text = "Domain:";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnOK.Location = new System.Drawing.Point(82, 312);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 33;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCancel.Location = new System.Drawing.Point(177, 312);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // password
            // 
            this.password.BackColor = System.Drawing.Color.White;
            this.password.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.password.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(74, 245);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(240, 22);
            this.password.TabIndex = 31;
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(17, 249);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(56, 13);
            this.lblPwd.TabIndex = 36;
            this.lblPwd.Text = "Password:";
            // 
            // userName
            // 
            this.userName.BackColor = System.Drawing.Color.White;
            this.userName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.userName.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userName.Location = new System.Drawing.Point(74, 210);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(240, 22);
            this.userName.TabIndex = 30;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(15, 214);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(58, 13);
            this.lblUserName.TabIndex = 35;
            this.lblUserName.Text = "Username:";
            // 
            // port
            // 
            this.port.BackColor = System.Drawing.Color.White;
            this.port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.port.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port.Location = new System.Drawing.Point(281, 127);
            this.port.MaxLength = 5;
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(43, 22);
            this.port.TabIndex = 27;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(251, 130);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 26;
            this.lblPort.Text = "Port:";
            // 
            // serverIP
            // 
            this.serverIP.BackColor = System.Drawing.SystemColors.Window;
            this.serverIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverIP.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverIP.Location = new System.Drawing.Point(71, 127);
            this.serverIP.Name = "serverIP";
            this.serverIP.Size = new System.Drawing.Size(172, 22);
            this.serverIP.TabIndex = 25;
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(22, 131);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(48, 13);
            this.lblServerIP.TabIndex = 24;
            this.lblServerIP.Text = "Address:";
            // 
            // lblProxy
            // 
            this.lblProxy.AutoSize = true;
            this.lblProxy.Location = new System.Drawing.Point(12, 94);
            this.lblProxy.Name = "lblProxy";
            this.lblProxy.Size = new System.Drawing.Size(303, 26);
            this.lblProxy.TabIndex = 23;
            this.lblProxy.Text = "Use this option if you are behind a firewall and know your proxy\r\nserver setting." +
    "";
            // 
            // proxyConn
            // 
            this.proxyConn.AutoSize = true;
            this.proxyConn.Location = new System.Drawing.Point(7, 72);
            this.proxyConn.Name = "proxyConn";
            this.proxyConn.Size = new System.Drawing.Size(183, 17);
            this.proxyConn.TabIndex = 22;
            this.proxyConn.TabStop = true;
            this.proxyConn.Text = "Connect via a HTTP proxy server";
            this.proxyConn.UseVisualStyleBackColor = true;
            this.proxyConn.CheckedChanged += new System.EventHandler(this.proxyRadio_CheckedChanged);
            // 
            // lblDirect
            // 
            this.lblDirect.AutoSize = true;
            this.lblDirect.Location = new System.Drawing.Point(12, 43);
            this.lblDirect.Name = "lblDirect";
            this.lblDirect.Size = new System.Drawing.Size(309, 26);
            this.lblDirect.TabIndex = 21;
            this.lblDirect.Text = "This is the recommend setting if you have never had connection\r\nproblems.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Connection";
            // 
            // directConn
            // 
            this.directConn.AutoSize = true;
            this.directConn.Location = new System.Drawing.Point(8, 22);
            this.directConn.Name = "directConn";
            this.directConn.Size = new System.Drawing.Size(187, 17);
            this.directConn.TabIndex = 19;
            this.directConn.TabStop = true;
            this.directConn.Text = "Connect directly to the ACA server";
            this.directConn.UseVisualStyleBackColor = true;
            this.directConn.CheckedChanged += new System.EventHandler(this.directConn_CheckedChanged);
            // 
            // ProxySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 344);
            this.Controls.Add(this.auth_chk);
            this.Controls.Add(this.byPass_chk);
            this.Controls.Add(this.domain);
            this.Controls.Add(this.lblDomain);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.password);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.port);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.serverIP);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.lblProxy);
            this.Controls.Add(this.proxyConn);
            this.Controls.Add(this.lblDirect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.directConn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProxySetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proxy Setting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox auth_chk;
        private System.Windows.Forms.CheckBox byPass_chk;
        private System.Windows.Forms.TextBox domain;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox serverIP;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblProxy;
        private System.Windows.Forms.RadioButton proxyConn;
        private System.Windows.Forms.Label lblDirect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton directConn;
    }
}