using System;
using System.IO;
using System.Windows.Forms;
using Awesomium.Windows.Forms;

namespace FantasyBot.Context
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

        public static void RunJs(this AddressBox abUrl, string jsCode)
        {
            try
            {
                abUrl.ThreadSafe(() => { abUrl.URL = new Uri($"javascript:{jsCode}"); });
            }
            catch
            {
                // ignored
            }
        }

        public static void ThreadSafe(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}