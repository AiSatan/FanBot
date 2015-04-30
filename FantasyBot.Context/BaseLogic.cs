using System;
using System.Collections.Generic;
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
                    point.Directions.Add(res);
                }
            }
            catch
            {
                // ignored
            }
        }

        public static CurrentPoint CreatePoint(AddressBox control)
        {
            var point = new CurrentPoint(null, control)
            {
                ParentPath = Direction.Refresh
            };
            return point;
        }

        public static CurrentPoint UpdatePoint(CurrentPoint point, string message)
        {
            GetDirections(message, ref point);
            GetLocation(message, ref point);
            return point;
        }
    }
}
