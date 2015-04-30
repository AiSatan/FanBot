using System;
using System.Collections.Generic;
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
        static Dictionary<string, CurrentPoint> Points { get; } = new Dictionary<string, CurrentPoint>();

        private CurrentPoint _activePoint;

        private void WcMainOnConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            try
            {
                var message = e.Message;

                if (message.StartsWith("Frame: "))
                {
                    if (_activePoint == null)
                    {
                        _activePoint = BaseLogic.CreatePoint(abUrl);
                    }
                    _activePoint = BaseLogic.UpdatePoint(_activePoint, message);

                    if (!Points.ContainsKey(_activePoint.Name))
                    {
                        Points.Add(_activePoint.Name, _activePoint);
                        _activePoint = _activePoint.Move();
                    }
                    else
                    {
                        _activePoint.Return();
                    }

                    //if (!Points.ContainsKey(point.Name))
                    //    Points.Add(point.Name, point);

                    /*                    foreach (var direction in point.Directions)
                                        {
                                            switch (direction)
                                            {
                                                case Directions.Up:
                                                {
                                                    var nPoint = new Point(point.Location.X, point.Location.Y - 1);
                                                    if (Points.ContainsKey(nPoint.ToString()))
                                                        continue;
                                                    break;
                                                }
                                                case Directions.Down:
                                                {
                                                    var nPoint = new Point(point.Location.X, point.Location.Y + 1);
                                                    if (Points.ContainsKey(nPoint.ToString()))
                                                        continue;
                                                    break;
                                                }
                                                case Directions.Left:
                                                {
                                                    var nPoint = new Point(point.Location.X - 1, point.Location.Y);
                                                    if (Points.ContainsKey(nPoint.ToString()))
                                                        continue;
                                                    break;
                                                }
                                                case Directions.Rigth:
                                                {
                                                    var nPoint = new Point(point.Location.X + 1, point.Location.Y);
                                                    if (Points.ContainsKey(nPoint.ToString()))
                                                        continue;
                                                    break;
                                                }
                                                default:
                                                    continue;
                                            }
                                            abUrl.RunJs("MoveTo(" + (int) direction + ");");
                                            return;
                                        }*/
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
    }
}
