using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using Awesomium.Windows.Forms;
using Newtonsoft.Json;

namespace FantasyBot.Context
{
    public class BaseLogic
    {
        private static void GetLocation(string message, ref CurrentPoint point)
        {
            try
            {
                var reg = new Regex("\\d+, \\d+\\)</b>");
                var mathces = reg.Matches(message);
                foreach (var dw in from Match mathc in mathces
                                   select mathc.Value.Trim().Replace(")</b>", "").Split(','))
                {
                    point.Location = new Point(Convert.ToInt32(dw[0].Trim()), Convert.ToInt32(dw[1].Trim()));
                }
                Debug.WriteLine($"OnGetLocation: {point?.Name},{point?.ParentPath}, {point?.Directions?.Count}");
            }
            catch (Exception ex)
            {
                ExceptionCatch(ex);
            }
        }

        private static void GetDirections(string message, ref CurrentPoint point)
        {
            try
            {
                point.Directions = new List<Direction>();
                var reg = new Regex("goTo\\(([\\d])\\)");
                var mathces = reg.Matches(message);
                foreach (var activeBtn in from Match mathc in mathces
                                          select mathc.Groups[1].Value)
                {
                    Direction res;
                    if (!Enum.TryParse(activeBtn, out res))
                        continue;
                    //ignore refresh button
                    if (res == Direction.Refresh)
                        continue;
                    if (res == Direction.Exit)
                        continue;
                    if (res == point.ParentPath)
                        continue;
                    point.Directions.Add(res);
                }
                Debug.WriteLine($"OnGetDirections: {point?.Name},{point?.ParentPath}, {point?.Directions?.Count}");
            }
            catch (Exception ex)
            {
                BaseLogic.ExceptionCatch(ex);
            }
        }

        public static CurrentPoint CreatePoint(AddressBox control)
        {
            var point = new CurrentPoint(null, control, Direction.Refresh);
            Debug.WriteLine($"OnCreatePoint: {point.Name},{point.ParentPath}, {point.Directions?.Count}");
            return point;
        }

        public static CurrentPoint UpdatePoint(CurrentPoint point, string message)
        {
            Debug.WriteLine($"OnUpdate: {point?.Name},{point?.ParentPath}, {point?.Directions?.Count}");
            GetLocation(message, ref point);
            if (point != null && !CurrentPoint.Points.ContainsKey(point.Name))
            {
                GetDirections(message, ref point);
            }

            return point;
        }

        public static void GetStatus(AddressBox control)
        {
            Debug.WriteLine($"GetStatus");
            control.RunJs($"GetStatus();");
        }

        public static int GetStatus(string message)
        {
            const string pattern = "(?<status>\\d+)";
            var reg = new Regex(pattern);
            var mathces = reg.Matches(message);
            foreach (var status in from Match mathc in mathces
                                   select mathc.Groups["status"].Value)
            {
                int res;
                if (!int.TryParse(status, out res))
                    continue;
                OnStatusChange?.Invoke(null, res.ToString());
                return res;
            }
            return -1;
        }


        public static bool NeedCode(string message)
        {
            const string pattern = "id=\"cod\" style=\"display: block";
            var result = message.Contains(pattern);
            if (result)
            {
                OnNeedCaptcha?.Invoke(null, message);
            }
            return result;
        }

        public static string FindItem(string message)
        {
            const string pattern = "javascript:pickUp\\((?<id>\\d{3,})";
            var reg = new Regex(pattern);
            var mathces = reg.Matches(message);
            foreach (var pickUp in from Match mathc in mathces
                                   select mathc.Groups["id"].Value)
            {
                OnPickUp?.Invoke(null, pickUp);
                return pickUp;
            }
            return string.Empty;
        }

        public static string FindQuestAction(string message)
        {
            const string pattern = "javascript:doQuestAction\\(([\\d+])\\)";
            var reg = new Regex(pattern);
            var mathces = reg.Matches(message);
            foreach (var action in from Match mathc in mathces
                                   select mathc.Groups[1].Value)
            {
                OnQuestAction?.Invoke(null, action);
                return action;
            }
            return string.Empty;
        }

        public static string GetAssembleVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        public static void Step(AddressBox control)
        {
            control.RunJs("GetDirections();");
        }

        public static bool AllowedAction(string action)
        {
            try
            {
                if (AllowedItems == null)
                {
                    const string fname = "allowedActions.json";
                    if (File.Exists(fname))
                    {
                        var content = File.ReadAllText(fname);
                        AllowedItems = JsonConvert.DeserializeObject<Dictionary<string, bool>>(content);
                    }
                    else
                    {
                        AllowedItems = new Dictionary<string, bool>
                        {
                            {"-1", true},
                            {"-2", false}
                        };
                        var content = JsonConvert.SerializeObject(AllowedItems);
                        File.WriteAllText(fname, content);
                    }
                }
                var status = false;
                if (AllowedItems.ContainsKey(action))
                {
                    status = AllowedItems.FirstOrDefault(pair => pair.Key == action).Value;
                }
                return status;
            }
            catch (Exception ex)
            {
                ExceptionCatch(ex);
            }
            return false;
        }

        public static void ExceptionCatch(Exception ex)
        {
            OnException?.Invoke(ex, ex.Message);
        }

        private static Dictionary<string, bool> AllowedItems = null;
        public static event AlarmEventHandler OnNeedCaptcha = delegate { };
        public static event AlarmEventHandler OnPickUp = delegate { };
        public static event AlarmEventHandler OnQuestAction = delegate { };
        public static event AlarmEventHandler OnStatusChange = delegate { };
        public static event AlarmEventHandler OnException = delegate { };
    }

    public delegate void AlarmEventHandler(object sender, string htlmContent);
}
