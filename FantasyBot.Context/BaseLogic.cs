using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Awesomium.Windows.Forms;

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
            catch
            {
                // ignored
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
                    if(res == Direction.Refresh)
                        continue;
                    if (res == Direction.Exit)
                        continue;
                    point.Directions.Add(res);
                }
                Debug.WriteLine($"OnGetDirections: {point?.Name},{point?.ParentPath}, {point?.Directions?.Count}");
            }
            catch
            {
                // ignored
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
            if (point?.Directions == null)
                GetDirections(message, ref point);
            GetLocation(message, ref point);
            return point;
        }
    }
}
