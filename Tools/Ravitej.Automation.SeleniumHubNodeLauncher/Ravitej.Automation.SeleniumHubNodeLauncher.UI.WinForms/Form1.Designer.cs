namespace Ravitej.Automation.SeleniumHubNodeLauncher.UI.WinForms
{
    partial class FrmLauncherMain
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
            this.btnGo = new System.Windows.Forms.Button();
            this.chkStartHub = new System.Windows.Forms.CheckBox();
            this.chkStartNode = new System.Windows.Forms.CheckBox();
            this.txtHubParams = new System.Windows.Forms.TextBox();
            this.lblHubParams = new System.Windows.Forms.Label();
            this.chkCustomNode = new System.Windows.Forms.CheckBox();
            this.lblBrowsers = new System.Windows.Forms.Label();
            this.lblMaxSessions = new System.Windows.Forms.Label();
            this.cboMaxSessions = new System.Windows.Forms.ComboBox();
            this.lblHub = new System.Windows.Forms.Label();
            this.txtHub = new System.Windows.Forms.TextBox();
            this.chkIE = new System.Windows.Forms.CheckBox();
            this.chkFirefox = new System.Windows.Forms.CheckBox();
            this.chkChrome = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAltPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAltUsername = new System.Windows.Forms.TextBox();
            this.txtAltDomain = new System.Windows.Forms.TextBox();
            this.txtAltPath = new System.Windows.Forms.TextBox();
            this.chkDontShowConsole = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGo
            // 
            this.btnGo.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(25, 511);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(112, 48);
            this.btnGo.TabIndex = 0;
            this.btnGo.Text = "Launch";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.EnabledChanged += new System.EventHandler(this.btnGo_EnabledChanged);
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // chkStartHub
            // 
            this.chkStartHub.AutoSize = true;
            this.chkStartHub.Location = new System.Drawing.Point(9, 21);
            this.chkStartHub.Name = "chkStartHub";
            this.chkStartHub.Size = new System.Drawing.Size(89, 20);
            this.chkStartHub.TabIndex = 1;
            this.chkStartHub.Text = "Start Hub?";
            this.chkStartHub.UseVisualStyleBackColor = true;
            this.chkStartHub.CheckedChanged += new System.EventHandler(this.chkStartHub_CheckedChanged);
            // 
            // chkStartNode
            // 
            this.chkStartNode.AutoSize = true;
            this.chkStartNode.Location = new System.Drawing.Point(6, 19);
            this.chkStartNode.Name = "chkStartNode";
            this.chkStartNode.Size = new System.Drawing.Size(98, 20);
            this.chkStartNode.TabIndex = 2;
            this.chkStartNode.Text = "Start Node?";
            this.chkStartNode.UseVisualStyleBackColor = true;
            this.chkStartNode.CheckedChanged += new System.EventHandler(this.chkStartNode_CheckedChanged);
            // 
            // txtHubParams
            // 
            this.txtHubParams.Location = new System.Drawing.Point(6, 72);
            this.txtHubParams.Name = "txtHubParams";
            this.txtHubParams.Size = new System.Drawing.Size(278, 22);
            this.txtHubParams.TabIndex = 3;
            // 
            // lblHubParams
            // 
            this.lblHubParams.AutoSize = true;
            this.lblHubParams.Location = new System.Drawing.Point(6, 50);
            this.lblHubParams.Name = "lblHubParams";
            this.lblHubParams.Size = new System.Drawing.Size(146, 16);
            this.lblHubParams.TabIndex = 4;
            this.lblHubParams.Text = "Additional Hub Params";
            // 
            // chkCustomNode
            // 
            this.chkCustomNode.AutoSize = true;
            this.chkCustomNode.Location = new System.Drawing.Point(10, 24);
            this.chkCustomNode.Name = "chkCustomNode";
            this.chkCustomNode.Size = new System.Drawing.Size(116, 20);
            this.chkCustomNode.TabIndex = 5;
            this.chkCustomNode.Text = "Custom Node?";
            this.chkCustomNode.UseVisualStyleBackColor = true;
            this.chkCustomNode.CheckedChanged += new System.EventHandler(this.chkCustomNode_CheckedChanged);
            // 
            // lblBrowsers
            // 
            this.lblBrowsers.AutoSize = true;
            this.lblBrowsers.Location = new System.Drawing.Point(7, 125);
            this.lblBrowsers.Name = "lblBrowsers";
            this.lblBrowsers.Size = new System.Drawing.Size(235, 16);
            this.lblBrowsers.TabIndex = 7;
            this.lblBrowsers.Text = "Choose browsers (select one or more)";
            // 
            // lblMaxSessions
            // 
            this.lblMaxSessions.AutoSize = true;
            this.lblMaxSessions.Location = new System.Drawing.Point(7, 189);
            this.lblMaxSessions.Name = "lblMaxSessions";
            this.lblMaxSessions.Size = new System.Drawing.Size(224, 16);
            this.lblMaxSessions.TabIndex = 8;
            this.lblMaxSessions.Text = "How many max sessions in parallel?";
            // 
            // cboMaxSessions
            // 
            this.cboMaxSessions.FormattingEnabled = true;
            this.cboMaxSessions.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboMaxSessions.Location = new System.Drawing.Point(10, 210);
            this.cboMaxSessions.Name = "cboMaxSessions";
            this.cboMaxSessions.Size = new System.Drawing.Size(121, 24);
            this.cboMaxSessions.TabIndex = 9;
            // 
            // lblHub
            // 
            this.lblHub.AutoSize = true;
            this.lblHub.Location = new System.Drawing.Point(6, 59);
            this.lblHub.Name = "lblHub";
            this.lblHub.Size = new System.Drawing.Size(210, 16);
            this.lblHub.TabIndex = 10;
            this.lblHub.Text = "Which hub you want to connect to?";
            // 
            // txtHub
            // 
            this.txtHub.Location = new System.Drawing.Point(10, 82);
            this.txtHub.Name = "txtHub";
            this.txtHub.Size = new System.Drawing.Size(264, 22);
            this.txtHub.TabIndex = 11;
            // 
            // chkIE
            // 
            this.chkIE.AutoSize = true;
            this.chkIE.Location = new System.Drawing.Point(155, 146);
            this.chkIE.Name = "chkIE";
            this.chkIE.Size = new System.Drawing.Size(123, 20);
            this.chkIE.TabIndex = 14;
            this.chkIE.Text = "Internet Explorer";
            this.chkIE.UseVisualStyleBackColor = true;
            this.chkIE.CheckedChanged += new System.EventHandler(this.chkIE_CheckedChanged);
            // 
            // chkFirefox
            // 
            this.chkFirefox.AutoSize = true;
            this.chkFirefox.Location = new System.Drawing.Point(86, 146);
            this.chkFirefox.Name = "chkFirefox";
            this.chkFirefox.Size = new System.Drawing.Size(67, 20);
            this.chkFirefox.TabIndex = 13;
            this.chkFirefox.Text = "Firefox";
            this.chkFirefox.UseVisualStyleBackColor = true;
            this.chkFirefox.CheckedChanged += new System.EventHandler(this.chkFirefox_CheckedChanged);
            // 
            // chkChrome
            // 
            this.chkChrome.AutoSize = true;
            this.chkChrome.Location = new System.Drawing.Point(10, 146);
            this.chkChrome.Name = "chkChrome";
            this.chkChrome.Size = new System.Drawing.Size(74, 20);
            this.chkChrome.TabIndex = 12;
            this.chkChrome.Text = "Chrome";
            this.chkChrome.UseVisualStyleBackColor = true;
            this.chkChrome.CheckedChanged += new System.EventHandler(this.chkChrome_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(231, 601);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 33);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "Exit";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkStartHub);
            this.groupBox1.Controls.Add(this.txtHubParams);
            this.groupBox1.Controls.Add(this.lblHubParams);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(25, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(292, 107);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hub";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.chkStartNode);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(25, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(292, 309);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Node";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkCustomNode);
            this.groupBox3.Controls.Add(this.lblHub);
            this.groupBox3.Controls.Add(this.txtHub);
            this.groupBox3.Controls.Add(this.lblBrowsers);
            this.groupBox3.Controls.Add(this.chkChrome);
            this.groupBox3.Controls.Add(this.chkIE);
            this.groupBox3.Controls.Add(this.cboMaxSessions);
            this.groupBox3.Controls.Add(this.lblMaxSessions);
            this.groupBox3.Controls.Add(this.chkFirefox);
            this.groupBox3.Location = new System.Drawing.Point(6, 55);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 248);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Custom Node";
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(336, 36);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(505, 598);
            this.txtOutput.TabIndex = 18;
            // 
            // btnShutdown
            // 
            this.btnShutdown.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShutdown.Location = new System.Drawing.Point(205, 511);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(112, 48);
            this.btnShutdown.TabIndex = 19;
            this.btnShutdown.Text = "Shutdown";
            this.btnShutdown.UseVisualStyleBackColor = true;
            this.btnShutdown.Click += new System.EventHandler(this.butShutdown_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // groupBox4
            // 
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox4.Location = new System.Drawing.Point(25, 653);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(816, 83);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Alternate Path or Credentials";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.txtAltPassword);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtAltUsername);
            this.groupBox5.Controls.Add(this.txtAltDomain);
            this.groupBox5.Controls.Add(this.txtAltPath);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox5.Location = new System.Drawing.Point(25, 653);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(816, 83);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Alternate Path or Credentials";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(312, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 27;
            this.label1.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 26;
            this.label2.Text = "Username";
            // 
            // txtAltPassword
            // 
            this.txtAltPassword.Location = new System.Drawing.Point(386, 53);
            this.txtAltPassword.Name = "txtAltPassword";
            this.txtAltPassword.PasswordChar = '.';
            this.txtAltPassword.Size = new System.Drawing.Size(189, 22);
            this.txtAltPassword.TabIndex = 22;
            this.txtAltPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(603, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Domain";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(262, 16);
            this.label4.TabIndex = 24;
            this.label4.Text = "Selenium Hub and Node Executables Path";
            // 
            // txtAltUsername
            // 
            this.txtAltUsername.Location = new System.Drawing.Point(83, 53);
            this.txtAltUsername.Name = "txtAltUsername";
            this.txtAltUsername.Size = new System.Drawing.Size(185, 22);
            this.txtAltUsername.TabIndex = 21;
            // 
            // txtAltDomain
            // 
            this.txtAltDomain.Location = new System.Drawing.Point(664, 53);
            this.txtAltDomain.Name = "txtAltDomain";
            this.txtAltDomain.Size = new System.Drawing.Size(126, 22);
            this.txtAltDomain.TabIndex = 23;
            // 
            // txtAltPath
            // 
            this.txtAltPath.Location = new System.Drawing.Point(274, 19);
            this.txtAltPath.Name = "txtAltPath";
            this.txtAltPath.Size = new System.Drawing.Size(516, 22);
            this.txtAltPath.TabIndex = 20;
            // 
            // chkDontShowConsole
            // 
            this.chkDontShowConsole.AutoSize = true;
            this.chkDontShowConsole.Location = new System.Drawing.Point(25, 476);
            this.chkDontShowConsole.Name = "chkDontShowConsole";
            this.chkDontShowConsole.Size = new System.Drawing.Size(144, 17);
            this.chkDontShowConsole.TabIndex = 19;
            this.chkDontShowConsole.Text = "Don\'t Show Grid Console";
            this.chkDontShowConsole.UseVisualStyleBackColor = true;
            this.chkDontShowConsole.CheckedChanged += new System.EventHandler(this.chkShowConsole_CheckedChanged);
            // 
            // FrmLauncherMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(867, 748);
            this.Controls.Add(this.chkDontShowConsole);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnShutdown);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Name = "FrmLauncherMain";
            this.Text = "Selenium Hub Node Launcher";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox chkStartHub;
        private System.Windows.Forms.CheckBox chkStartNode;
        private System.Windows.Forms.TextBox txtHubParams;
        private System.Windows.Forms.Label lblHubParams;
        private System.Windows.Forms.CheckBox chkCustomNode;
        private System.Windows.Forms.Label lblBrowsers;
        private System.Windows.Forms.Label lblMaxSessions;
        private System.Windows.Forms.ComboBox cboMaxSessions;
        private System.Windows.Forms.Label lblHub;
        private System.Windows.Forms.TextBox txtHub;
        private System.Windows.Forms.CheckBox chkIE;
        private System.Windows.Forms.CheckBox chkFirefox;
        private System.Windows.Forms.CheckBox chkChrome;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAltPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAltUsername;
        private System.Windows.Forms.TextBox txtAltDomain;
        private System.Windows.Forms.TextBox txtAltPath;
        private System.Windows.Forms.CheckBox chkDontShowConsole;
    }
}

