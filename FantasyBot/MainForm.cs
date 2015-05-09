using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Awesomium.Core;
using Awesomium.Windows.Forms;
using FantasyBot.Context;
using Newtonsoft.Json;

namespace FantasyBot
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
            nudStepTime.Value = Convert.ToDecimal(_setting.MoveSpeed);
            abUrl.WebControl = wcMain;
        }

        public void SerToJson()
        {
            //            var obj = JsonConvert.SerializeObject(Points);
            //            File.WriteAllText(@"points.json", obj);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _map = new MapForm();
            _map.Show();
            EventHandler();
            Text = $"FantasyBot: {Application.ProductVersion}, Context: {BaseLogic.GetAssembleVersion()}";
        }

        private WebSessionProvider _wspSession;
        private void LoadSettings()
        {
            try
            {
                const string fname = "appSetting.json";
                if (File.Exists(fname))
                {
                    var content = File.ReadAllText(fname);
                    _setting = JsonConvert.DeserializeObject<AppSetting>(content);
                }
                else
                {
                    _setting = new AppSetting();
                    var content = JsonConvert.SerializeObject(_setting);
                    File.WriteAllText(fname, content);
                }
                _wspSession = new WebSessionProvider();
                if (!string.IsNullOrEmpty(_setting.Proxy))
                {
                    var preference = _wspSession.Preferences;
                    preference.ProxyConfig = _setting.Proxy;
                    _wspSession.Preferences = preference;
                }
                var path = Path.Combine(Application.StartupPath, "temp");
                _wspSession.DataPath = path;
                //wcMain.WebSession = WebCore.CreateWebSession(path, _wspSession.Preferences);
                _wspSession.Views.Add(this.wcMain);
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private void EventHandler()
        {
            //подписываемся на события
            CurrentPoint.OnMove += _map.OnMove;
            BaseLogic.OnNeedCaptcha += BaseLogicOnOnNeedCaptcha;
            BaseLogic.OnStatusChange += BaseLogicOnOnStatusChange;
            BaseLogic.OnException += BaseLogicOnOnException;
            BaseLogic.OnPickUp += BaseLogic_OnPickUp;
        }

        private void tStep_Tick(object sender, EventArgs e)
        {//socks4://127.0.0.1:6080
            var statusMsg = BaseLogic.GetStatus(wcMain);

            var status = BaseLogic.GetStatus(statusMsg);
            if (status > 5)
            {
                var jval = BaseLogic.Step(wcMain);

                if (BaseLogic.NeedCode(jval))
                {
                    Debug.WriteLine("WcMainOnConsoleMessage, Найдена капча");
                    return;
                }
                IsRepit = false;
                if (_activePoint == null)
                {
                    Debug.WriteLine("WcMainOnConsoleMessage, Создание точки.");

                    //Если точка первая, создаем ее
                    _activePoint = BaseLogic.CreatePoint(wcMain);
                }
                Debug.WriteLine($"WcMainOnConsoleMessage, Обновление точки: {_activePoint?.Name}");

                //обновляем точку, не обновляем Directions если он уже есть
                _activePoint = BaseLogic.UpdatePoint(_activePoint, jval);
                Debug.WriteLine(
                    _activePoint?.Directions != null
                        ? $"WcMainOnConsoleMessage, Обновление точки: {_activePoint?.Name}, {_activePoint?.Directions?.Count}: {string.Join("-", _activePoint?.Directions)}"
                        : $"WcMainOnConsoleMessage, Обновление точки: {_activePoint?.Name}, {_activePoint?.Directions?.Count}.");

                if (_activePoint == null)
                    return;

                //find items
                var item = BaseLogic.FindItem(jval);
                if (!string.IsNullOrEmpty(item))
                {
                    _activePoint.PickUp(item);
                    return;
                }

                //find quest actions
                var action = BaseLogic.FindQuestAction(jval);
                if (!string.IsNullOrEmpty(action))
                {
                    if (BaseLogic.AllowedAction(action))
                    {
                        _activePoint.InvokeQuest(action);
                        return;
                    }
                }

                _map.WritePoint(_activePoint.Location, _activePoint.Directions);
                //проверяем были ли мы на этой точке раньше
                if (!CurrentPoint.Points.ContainsKey(_activePoint.Name))
                {
                    //eсли нет, добавляем
                    CurrentPoint.Points.Add(_activePoint.Name, _activePoint);
                }
                else
                {
                    Debug.WriteLine(
                        _activePoint?.Directions != null
                            ? $"WcMainOnConsoleMessage, повторное прибывание в данной точке: {_activePoint.Name}, {_activePoint.Directions.Count}: {string.Join("-", _activePoint.Directions)}"
                            : $"WcMainOnConsoleMessage, повторное прибывание в данной точке: {_activePoint.Name}");
                }
                //идем если есть куда идти, иначе возвращаемся
                _activePoint = _activePoint.Move() ?? _activePoint.Return();
                return;
            }
            return;
        }

        private void WcMainOnConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            try
            {
                var message = e.Message;

                if (message.StartsWith("Frame: "))
                {
                }

                if (message.StartsWith("PickUpItem: "))
                {
                    WriteToLog(message);
                    return;
                }

                if (message.StartsWith("InvokeAction: "))
                {
                    WriteToLog(message);
                    return;
                }

                if (message.StartsWith("Status: "))
                {
                    
                }

                if (message.StartsWith("Injected jQuery:"))
                {
                    WriteToLog(message, e);

                    bRunJS.BackColor = Color.Lime;
                    return;
                }

                WriteToLog(message, e);
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private void BaseLogicOnOnStatusChange(object sender, string htlmContent)
        {
            lStatus.ThreadSafe(() => lStatus.Text = $"Status: {htlmContent}%");
        }
        //TODO: Hot Fix
        private static bool IsRepit = false;
        private static void BaseLogicOnOnNeedCaptcha(object sender, string htlmContent)
        {
            if (IsRepit)
                return;
            IsRepit = true;
            Debug.WriteLine("BaseLogicOnOnAlarm, Найдена капча");
            MessageBox.Show("captcha found");
        }

        private void WriteToLog(string message, ConsoleMessageEventArgs consoleMessageEventArgs = null)
        {
            if (consoleMessageEventArgs != null)
            {
                var str = $"{consoleMessageEventArgs.LineNumber}: {message}, {consoleMessageEventArgs.Source}";
                tbLog.ThreadSafe(() => { tbLog.Text += $"{str}\r\n"; });
            }
            tbLog.ThreadSafe(() => { tbLog.Text += $"{message}\r\n"; });
        }


        private void bLogin_Click(object sender, EventArgs e)
        {
            try
            {
                bRunJS.BackColor = Color.Red;

                wcMain.ExecuteJavascript($"Logon('{_setting.Login}', '{_setting.Password}');");
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }


        private void buttonMove_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                if (btn == null)
                    return;
                wcMain.ExecuteJavascript($"MoveTo({btn.Text});");
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private void bRunJS_Click(object sender, EventArgs e)
        {
            //socks4://127.0.0.1:6080
            try
            {
                wcMain.ExecuteJavascript(tbJS.Text);
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        public void InjectJQuery()
        {
            try
            {
                bRunJS.BackColor = Color.Lime;
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
            if (!e.IsMainFrame && e.FrameId != 7)
                return;

            wcMain.ExecuteJavascript("Main.js".GetJsCode());
            wcMain.ExecuteJavascript("InjectJquery('https://code.jquery.com/jquery-2.1.3.js');");
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            WriteToLog("Бот запущен.");
            tStep.Enabled = true;
            bStart.Enabled = false;
        }

        private void BaseLogicOnOnException(object sender, string exceptionMessage)
        {
            WriteToLog($"Ошибка: {exceptionMessage}");
        }

        private void BaseLogic_OnPickUp(object sender, string htlmContent)
        {
            WriteToLog($"Поднято: {htlmContent}");
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                const string fname = "appSetting.json";
                if (_setting == null)
                {
                    _setting = new AppSetting();
                }
                _setting.MoveSpeed = nudStepTime.Value.ToString(CultureInfo.InvariantCulture);

                var content = JsonConvert.SerializeObject(_setting);
                File.WriteAllText(fname, content);
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private void bSetSpeed_Click(object sender, EventArgs e)
        {
            try
            {
                var speed = (int)nudStepTime.Value;
                tStep.Interval = speed;
                lMoveSpeed.ThreadSafe(() => lMoveSpeed.Text = $"Ходить раз в {speed / 1000} секунд");
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private CurrentPoint _activePoint;
        private MapForm _map;
        private AppSetting _setting;

        private void Awesomium_Windows_Forms_WebControl_DocumentReady(object sender, DocumentReadyEventArgs e)
        {
           /* wcMain.ca*/
        }
    }
}
