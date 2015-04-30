using System;
using System.Collections.Generic;
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

        public static void Delete(this List<Direction> directions, Direction item)
        {
            if (!directions.Remove(item))
                throw new InvalidOperationException();
        }


        /// <summary>
        /// Инвертирует путь
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Direction Invert(this Direction value)
        {
            switch (value)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Rigth;
                case Direction.Rigth:
                    return Direction.Left;
            }
            //TODO: refactoring this
            return Direction.Refresh;
        }
    }
}