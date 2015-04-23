using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Awesomium.Core;

namespace FantasyBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            wcMain.DocumentReady += OnDocumentReady;
        }

        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender,
            Awesomium.Core.FrameEventArgs e)
        {
            if (e.IsMainFrame)
            {
                
            }
        }

        private void OnDocumentReady(object sender, DocumentReadyEventArgs e)
        {
            bLogin.Text = "Login";
        }

        private void bLogin_Click(object sender, EventArgs e)
        {
            if (!wcMain.IsDocumentReady)
            {
                wcMain.Reload(false);
                bLogin.Text = "NO";
                return;
            }
            //$('input[name="login"]').prop('value', 'AiTest');
            wcMain.ExecuteJavascript("document.getElementsByName('login')[0].value = 'AiTest'");
            //$('input[name="password"]').prop('value', 'admin');
            wcMain.ExecuteJavascript("document.getElementsByName('password')[0].value = 'admin'");
            wcMain.ExecuteJavascript("Login();");
        }
        

        private void buttonMove_Click(object sender, EventArgs e)
        {
            if (!wcMain.IsDocumentReady)
            {
                wcMain.Reload(false);
                bLogin.Text = "NO";
                return;
            }
            var n = 0;
            var btn = sender as Button;
            if (btn == null)
                return;
            switch (btn.Text)
            {
                case "1":
                    wcMain.ExecuteJavascript("goTo(1);", "//iframe[@name='no_combat']");
                    break;
                case "2":
                    wcMain.ExecuteJavascript("goTo(2);", "//iframe[@name='no_combat']");
                    break;
                case "3":
                    wcMain.ExecuteJavascript("goTo(3);", "//iframe[@name='no_combat']");
                    break;
                case "4":
                    wcMain.ExecuteJavascript("goTo(4);", "//iframe[@name='no_combat']");
                    break;
                case "5":
                    wcMain.ExecuteJavascript("goTo(5);", "//iframe[@name='no_combat']");
                    break;
                case "6":
                    wcMain.ExecuteJavascript("goTo(6);", "//iframe[@name='no_combat']");
                    break;
                case "7":
                    wcMain.ExecuteJavascript("goTo(7);", "//iframe[@name='no_combat']");
                    break;
                case "8":
                    wcMain.ExecuteJavascript("goTo(8);", "//iframe[@name='no_combat']");
                    break;
                case "9":
                    wcMain.ExecuteJavascript("goTo(9);", "//iframe[@name='no_combat']");
                    break;
                default:
                    return;
            }
            var js = @"var x = frames['loc'];
                       var y = x.frames['no_combat'];
                       var btn = y.document.getElementsByName('d"+ btn.Text + @"')[0];
alert(btn.firstChild.src);
                       btn.firstChild.click();
                       alert('hell'); ";
            wcMain.ExecuteJavascript(js);
        }

        private void bRunJS_Click(object sender, EventArgs e)
        {
            if (!wcMain.IsDocumentReady)
            {
                wcMain.Reload(false);
                bLogin.Text = "NO";
                return;
            }
            wcMain.ExecuteJavascript(tbJS.Text, tbXPath.Text);
        }

        public void InjectJQuery(String script)
        {
            // Get jQueryUrl
            string jquery = "http://code.jquery.com/jquery-2.1.3.min.js";

            String injectCode =
@"function InjectJquery(urlJQuery)
{
  if (typeof jQuery != 'undefined')
    return;

  var scriptTag = document.createElement('script');
  scriptTag.setAttribute('type', 'text/javascript');
  scriptTag.setAttribute('src', urlJQuery);
  scriptTag.onload = scriptTag.onreadystatechange = function ()
  {"
     + script +
  @"    
  };
  document.head.appendChild(scriptTag);
}
InjectJquery('" + jquery + "');";

            wcMain.ExecuteJavascript(injectCode);
        }

        private void Awesomium_Windows_Forms_WebControl_JavascriptMessage(object sender, JavascriptMessageEventArgs e)
        {

        }

        private void Awesomium_Windows_Forms_WebControl_JavascriptRequest(object sender, JavascriptRequestEventArgs e)
        {

        }

        private void bInjectJQ_Click(object sender, EventArgs e)
        {
            if (!wcMain.IsDocumentReady)
            {
                wcMain.Reload(false);
                bLogin.Text = "NO";
                return;
            }
            InjectJQuery("alert('injected jq');");
        }
    }
}
