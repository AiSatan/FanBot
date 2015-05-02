using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Awesomium.Core;
using FantasyBot.Context;

namespace FantasyBot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            abUrl.WebControl = wcMain;
        }

        public void SerToJson()
        {
            //            var obj = JsonConvert.SerializeObject(Points);
            //            File.WriteAllText(@"points.json", obj);
        }
        

        private CurrentPoint _activePoint;
        private MapForm _map;

        private void WcMainOnConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            try
            {
                var message = e.Message;

                if (message.StartsWith("Frame: "))
                {
                    Debug.WriteLine($"OnConsole: {_activePoint?.Name},{_activePoint?.ParentPath}, {_activePoint?.Directions?.Count}");
                    if (_activePoint == null)
                    {
                        //Если точка первая, создаем ее
                        _activePoint = BaseLogic.CreatePoint(abUrl);
                        CurrentPoint.OnMove += _map.OnMove;
                    }
                    //обновляем точку, не обновляем Directions если он уже есть
                    _activePoint = BaseLogic.UpdatePoint(_activePoint, message);

 /*                   //проверяем были ли мы на этой точке раньше
                    if (!CurrentPoint.Points.ContainsKey(_activePoint.Name))
                    {*/
                        Debug.WriteLine($"OnConsole: Not contains {_activePoint?.Name}");
                    if (!CurrentPoint.Points.ContainsKey(_activePoint.Name))
                    {

                        CurrentPoint.Points.Add(_activePoint.Name, _activePoint);
                    }

                    return;
                }

                if (message.StartsWith("Run: "))
                {
                    button10_Click(this, null);
                    return;
                }

                if (message.StartsWith("Injected jQuery:"))
                {
                    WriteToLog(e, message);

                    bRunJS.BackColor = Color.Lime;
                    return;
                }

                WriteToLog(e, message);
            }
            catch
            {
                // ignored
            }
        }

        private void WriteToLog(ConsoleMessageEventArgs e, string message)
        {
            var str = $"{e.LineNumber}: {message}, {e.Source}";
            tbLog.ThreadSafe(() => { tbLog.Text += str + "\r\n"; });
        }


        private void bLogin_Click(object sender, EventArgs e)
        {
            try
            {
                bRunJS.BackColor = Color.Red;

                abUrl.RunJs("Logon();");
            }
            catch (Exception)
            {
                // ignored
            }
        }


        private void buttonMove_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                if (btn == null)
                    return;
                abUrl.RunJs("MoveTo(" + btn.Text + ");");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void bRunJS_Click(object sender, EventArgs e)
        {
            try
            {
                abUrl.RunJs(tbJS.Text);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void InjectJQuery()
        {
            try
            {
                bRunJS.BackColor = Color.Lime;

            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            abUrl.RunJs("GetDirections();");
        }

        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
            if (!e.IsMainFrame && e.FrameId != 7)
                return;

            abUrl.RunJs("Main.js".GetJsCode());
            abUrl.RunJs("InjectJquery('https://code.jquery.com/jquery-2.1.3.js');");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _map = new MapForm();
            _map.Show();
        }
    }
}
