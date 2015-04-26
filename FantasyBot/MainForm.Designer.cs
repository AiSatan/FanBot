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
            this.bLogin = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.tbJS = new System.Windows.Forms.TextBox();
            this.bRunJS = new System.Windows.Forms.Button();
            this.wcMain = new Awesomium.Windows.Forms.WebControl(this.components);
            this.webSessionProvider1 = new Awesomium.Windows.Forms.WebSessionProvider(this.components);
            this.abUrl = new Awesomium.Windows.Forms.AddressBox();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // bLogin
            // 
            this.bLogin.Location = new System.Drawing.Point(12, 482);
            this.bLogin.Name = "bLogin";
            this.bLogin.Size = new System.Drawing.Size(85, 78);
            this.bLogin.TabIndex = 1;
            this.bLogin.Text = "Login";
            this.bLogin.UseVisualStyleBackColor = true;
            this.bLogin.Click += new System.EventHandler(this.bLogin_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(619, 479);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(658, 479);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(697, 479);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(619, 509);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(33, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(658, 509);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(33, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(697, 508);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(33, 23);
            this.button6.TabIndex = 7;
            this.button6.Text = "6";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(697, 537);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(33, 23);
            this.button9.TabIndex = 10;
            this.button9.Text = "9";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(658, 538);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(33, 23);
            this.button8.TabIndex = 9;
            this.button8.Text = "8";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(619, 538);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(33, 23);
            this.button7.TabIndex = 8;
            this.button7.Text = "7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // tbJS
            // 
            this.tbJS.Location = new System.Drawing.Point(103, 484);
            this.tbJS.Multiline = true;
            this.tbJS.Name = "tbJS";
            this.tbJS.Size = new System.Drawing.Size(373, 76);
            this.tbJS.TabIndex = 11;
            this.tbJS.Text = "GetDirections();";
            // 
            // bRunJS
            // 
            this.bRunJS.Location = new System.Drawing.Point(482, 484);
            this.bRunJS.Name = "bRunJS";
            this.bRunJS.Size = new System.Drawing.Size(131, 76);
            this.bRunJS.TabIndex = 12;
            this.bRunJS.Text = "Run";
            this.bRunJS.UseVisualStyleBackColor = true;
            this.bRunJS.Click += new System.EventHandler(this.bRunJS_Click);
            // 
            // wcMain
            // 
            this.wcMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wcMain.Location = new System.Drawing.Point(12, 12);
            this.wcMain.NavigationInfo = Awesomium.Core.NavigationInfo.Normal;
            this.wcMain.Size = new System.Drawing.Size(718, 461);
            this.wcMain.Source = new System.Uri("http://fantasyland.ru/", System.UriKind.Absolute);
            this.wcMain.TabIndex = 16;
            this.wcMain.ViewType = Awesomium.Core.WebViewType.Offscreen;
            this.wcMain.ConsoleMessage += new Awesomium.Core.ConsoleMessageEventHandler(this.WcMainOnConsoleMessage);
            this.wcMain.LoadingFrameComplete += new Awesomium.Core.FrameEventHandler(this.Awesomium_Windows_Forms_WebControl_LoadingFrameComplete);
            // 
            // webSessionProvider1
            // 
            this.webSessionProvider1.Views.Add(this.wcMain);
            // 
            // abUrl
            // 
            this.abUrl.AcceptsReturn = true;
            this.abUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.abUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.abUrl.Location = new System.Drawing.Point(12, 569);
            this.abUrl.Name = "abUrl";
            this.abUrl.Size = new System.Drawing.Size(163, 20);
            this.abUrl.TabIndex = 17;
            this.abUrl.URL = null;
            this.abUrl.WebControl = null;
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 595);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(716, 101);
            this.tbLog.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 708);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.abUrl);
            this.Controls.Add(this.wcMain);
            this.Controls.Add(this.bRunJS);
            this.Controls.Add(this.tbJS);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bLogin);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bLogin;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox tbJS;
        private System.Windows.Forms.Button bRunJS;
        private Awesomium.Windows.Forms.WebControl wcMain;
        private Awesomium.Windows.Forms.WebSessionProvider webSessionProvider1;
        private Awesomium.Windows.Forms.AddressBox abUrl;
        private System.Windows.Forms.TextBox tbLog;
    }
}

