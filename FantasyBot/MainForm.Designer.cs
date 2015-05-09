using System;
using System.IO;

namespace FantasyBot
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bLogin = new System.Windows.Forms.Button();
            this.tbJS = new System.Windows.Forms.TextBox();
            this.bRunJS = new System.Windows.Forms.Button();
            this.wcMain = new Awesomium.Windows.Forms.WebControl(this.components);
            this.abUrl = new Awesomium.Windows.Forms.AddressBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.lStatus = new System.Windows.Forms.Label();
            this.tStep = new System.Windows.Forms.Timer(this.components);
            this.bStart = new System.Windows.Forms.Button();
            this.nudStepTime = new System.Windows.Forms.NumericUpDown();
            this.lMoveSpeed = new System.Windows.Forms.Label();
            this.bSetSpeed = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepTime)).BeginInit();
            this.SuspendLayout();
            // 
            // bLogin
            // 
            this.bLogin.Location = new System.Drawing.Point(12, 484);
            this.bLogin.Name = "bLogin";
            this.bLogin.Size = new System.Drawing.Size(85, 41);
            this.bLogin.TabIndex = 1;
            this.bLogin.Text = "Login";
            this.bLogin.UseVisualStyleBackColor = true;
            this.bLogin.Click += new System.EventHandler(this.bLogin_Click);
            // 
            // tbJS
            // 
            this.tbJS.Location = new System.Drawing.Point(194, 484);
            this.tbJS.Multiline = true;
            this.tbJS.Name = "tbJS";
            this.tbJS.Size = new System.Drawing.Size(399, 41);
            this.tbJS.TabIndex = 11;
            this.tbJS.Text = "GetDirections();";
            // 
            // bRunJS
            // 
            this.bRunJS.Location = new System.Drawing.Point(599, 484);
            this.bRunJS.Name = "bRunJS";
            this.bRunJS.Size = new System.Drawing.Size(131, 41);
            this.bRunJS.TabIndex = 12;
            this.bRunJS.Text = "<< Run current \r\nSCRIPT";
            this.bRunJS.UseVisualStyleBackColor = true;
            this.bRunJS.Click += new System.EventHandler(this.bRunJS_Click);
            // 
            // wcMain
            // 
            this.wcMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wcMain.Location = new System.Drawing.Point(12, 12);
            this.wcMain.NavigationInfo = Awesomium.Core.NavigationInfo.None;
            this.wcMain.Size = new System.Drawing.Size(718, 461);
            this.wcMain.Source = new System.Uri("http://www.fantasyland.ru/main.php", System.UriKind.Absolute);
            this.wcMain.TabIndex = 16;
            this.wcMain.ConsoleMessage += new Awesomium.Core.ConsoleMessageEventHandler(this.WcMainOnConsoleMessage);
            this.wcMain.DocumentReady += new Awesomium.Core.DocumentReadyEventHandler(this.Awesomium_Windows_Forms_WebControl_DocumentReady);
            this.wcMain.LoadingFrameComplete += new Awesomium.Core.FrameEventHandler(this.Awesomium_Windows_Forms_WebControl_LoadingFrameComplete);
            // 
            // abUrl
            // 
            this.abUrl.AcceptsReturn = true;
            this.abUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.abUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.abUrl.Location = new System.Drawing.Point(567, 453);
            this.abUrl.Name = "abUrl";
            this.abUrl.ReadOnly = true;
            this.abUrl.Size = new System.Drawing.Size(163, 20);
            this.abUrl.TabIndex = 17;
            this.abUrl.URL = null;
            this.abUrl.Visible = false;
            this.abUrl.WebControl = null;
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 599);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.Size = new System.Drawing.Size(716, 111);
            this.tbLog.TabIndex = 20;
            // 
            // lStatus
            // 
            this.lStatus.AutoSize = true;
            this.lStatus.Location = new System.Drawing.Point(12, 528);
            this.lStatus.Name = "lStatus";
            this.lStatus.Size = new System.Drawing.Size(57, 13);
            this.lStatus.TabIndex = 21;
            this.lStatus.Text = "Status: 0%";
            // 
            // tStep
            // 
            this.tStep.Interval = 5000;
            this.tStep.Tick += new System.EventHandler(this.tStep_Tick);
            // 
            // bStart
            // 
            this.bStart.Location = new System.Drawing.Point(103, 484);
            this.bStart.Name = "bStart";
            this.bStart.Size = new System.Drawing.Size(85, 41);
            this.bStart.TabIndex = 22;
            this.bStart.Text = "Start Bot";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // nudStepTime
            // 
            this.nudStepTime.Location = new System.Drawing.Point(15, 565);
            this.nudStepTime.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.nudStepTime.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudStepTime.Name = "nudStepTime";
            this.nudStepTime.Size = new System.Drawing.Size(125, 20);
            this.nudStepTime.TabIndex = 23;
            this.nudStepTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudStepTime.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // lMoveSpeed
            // 
            this.lMoveSpeed.AutoSize = true;
            this.lMoveSpeed.Location = new System.Drawing.Point(12, 549);
            this.lMoveSpeed.Name = "lMoveSpeed";
            this.lMoveSpeed.Size = new System.Drawing.Size(127, 13);
            this.lMoveSpeed.TabIndex = 24;
            this.lMoveSpeed.Text = "Ходить раз в %s секунд";
            // 
            // bSetSpeed
            // 
            this.bSetSpeed.Location = new System.Drawing.Point(145, 562);
            this.bSetSpeed.Name = "bSetSpeed";
            this.bSetSpeed.Size = new System.Drawing.Size(75, 23);
            this.bSetSpeed.TabIndex = 25;
            this.bSetSpeed.Text = "Применить";
            this.bSetSpeed.UseVisualStyleBackColor = true;
            this.bSetSpeed.Click += new System.EventHandler(this.bSetSpeed_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 722);
            this.Controls.Add(this.bSetSpeed);
            this.Controls.Add(this.lMoveSpeed);
            this.Controls.Add(this.nudStepTime);
            this.Controls.Add(this.bStart);
            this.Controls.Add(this.lStatus);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.abUrl);
            this.Controls.Add(this.wcMain);
            this.Controls.Add(this.bRunJS);
            this.Controls.Add(this.tbJS);
            this.Controls.Add(this.bLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "FantasyBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudStepTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bLogin;
        private System.Windows.Forms.TextBox tbJS;
        private System.Windows.Forms.Button bRunJS;
        private Awesomium.Windows.Forms.WebControl wcMain;
        private Awesomium.Windows.Forms.AddressBox abUrl;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Timer tStep;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.NumericUpDown nudStepTime;
        private System.Windows.Forms.Label lMoveSpeed;
        private System.Windows.Forms.Button bSetSpeed;
    }
}

