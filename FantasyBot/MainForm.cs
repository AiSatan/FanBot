using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Awesomium.Core;
using FantasyBot.Context;
using Newtonsoft.Json;

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

        private void MainForm_Load(object sender, EventArgs e)
        {
            _map = new MapForm();
            _map.Show();
            EventHandler();
            Text = $"FantasyBot: {Application.ProductVersion}, Context: {BaseLogic.GetAssembleVersion()}";

            LoadSettings();
        }

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
                nudStepTime.Value = Convert.ToDecimal(_setting.MoveSpeed);
                /*nudStepTime_ValueChanged(this, null);*/
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
        {
            BaseLogic.GetStatus(abUrl);
        }

        private void WcMainOnConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            try
            {
                var message = e.Message;

                if (message.StartsWith("Frame: "))
                {
                    if (BaseLogic.NeedCode(message))
                    {
                        Debug.WriteLine("WcMainOnConsoleMessage, Найдена капча");
                        return;
                    }
                    if (_activePoint == null)
                    {
                        Debug.WriteLine("WcMainOnConsoleMessage, Создание точки.");

                        //Если точка первая, создаем ее
                        _activePoint = BaseLogic.CreatePoint(abUrl);
                    }
                    Debug.WriteLine($"WcMainOnConsoleMessage, Обновление точки: {_activePoint?.Name}");

                    //обновляем точку, не обновляем Directions если он уже есть
                    _activePoint = BaseLogic.UpdatePoint(_activePoint, message);
                    Debug.WriteLine(
                        _activePoint?.Directions != null
                            ? $"WcMainOnConsoleMessage, Обновление точки: {_activePoint?.Name}, {_activePoint?.Directions?.Count}: {string.Join("-", _activePoint?.Directions)}"
                            : $"WcMainOnConsoleMessage, Обновление точки: {_activePoint?.Name}, {_activePoint?.Directions?.Count}.");

                    if (_activePoint == null)
                        return;

                    //find items
                    var item = BaseLogic.FindItem(message);
                    if (!string.IsNullOrEmpty(item))
                    {
                        _activePoint.PickUp(item);
                        return;
                    }

                    //find quest actions
                    var action = BaseLogic.FindQuestAction(message);
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
                    var status = BaseLogic.GetStatus(message);
                    if (status > 5)
                    {
                        BaseLogic.Step(abUrl);
                    }
                    return;
                }

                if (message.StartsWith("Run: "))
                {
                    BaseLogic.Step(abUrl);
                    return;
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

        private static void BaseLogicOnOnNeedCaptcha(object sender, string htlmContent)
        {
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

                abUrl.RunJs($"Logon('{_setting.Login}', '{_setting.Password}');");
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
                abUrl.RunJs($"MoveTo({btn.Text});");
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        private void bRunJS_Click(object sender, EventArgs e)
        {
            try
            {
                abUrl.RunJs(tbJS.Text);
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

            abUrl.RunJs("Main.js".GetJsCode());
            abUrl.RunJs("InjectJquery('https://code.jquery.com/jquery-2.1.3.js');");
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
    }
}
