using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Awesomium.Windows.Forms;
using MSHTML;
using HtmlElement = System.Web.UI.HtmlControls.HtmlElement;

namespace FantasyBot.Logics
{
    public static class Helpers
    {
        public static string GetJsCode(this string fileName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "js", fileName);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "alert('file: " + fileName+ " - not found')";
        }

        public static object ExecuteJavascriptIE(this WebBrowser webBrowser, string methodName, string code, params object[] args)
        {
            if (webBrowser.Document == null)
                return null;
            var head = webBrowser.Document.GetElementsByTagName("head")[0];
            var scriptEl = webBrowser.Document.CreateElement("script");
            if (scriptEl == null)
                return null;
            var element = (IHTMLScriptElement)scriptEl.DomElement;
            element.text = code;
            head.AppendChild(scriptEl); 
            return args.Length > 0 ? webBrowser.Document.InvokeScript(methodName, args) : webBrowser.Document.InvokeScript(methodName);
        }

        public static void RunJS(this AddressBox abUrl, string jsCode)
        {
            abUrl.URL = new Uri($"javascript:{jsCode}");
        }
    }
}