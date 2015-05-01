using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Awesomium.Windows.Forms;

namespace FantasyBot.Context
{
    public class CurrentPoint
    {
        private readonly AddressBox _control;

        /// <summary>
        /// Имя точки
        /// </summary>
        public string Name => $"{Location.X}&{Location.Y}";

        /// <summary>
        /// Координата точки
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Все возможные стороны движения точки
        /// </summary>
        public List<Direction> Directions { get; set; } = null;

        /// <summary>
        /// Обратный путь к родительской точке
        /// </summary>
        public Direction ParentPath
        {
            get { return _path; }
            set
            {
                _path = value.Invert();
            }
        }

        public CurrentPoint(CurrentPoint parentPoint, AddressBox control, Direction parentPath)
        {
            Debug.WriteLine($"CurrentPoint constructor: {parentPath},{parentPoint?.Name}");

            ParentPath = parentPath;
            _control = control;
            _parentPoint = parentPoint;
        }

        public CurrentPoint Move()
        {
            Debug.WriteLine($"OnMove: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count}");

            if (Directions == null)
                return Return();

            Directions.Remove(_path);
            Debug.WriteLine($"OnMove: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count} - remove - {_path}");

            var direction = GetNextPoint();
            if (direction == default(Direction))
            {
                Debug.WriteLine($"OnMove: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count} - {string.Join(";", this?.Directions)} - direction not found");
                return Return();
            }
            Debug.WriteLine($"OnMove: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count}: {string.Join(";", this?.Directions)} - direction found - {direction}");

            var currentPoint = new CurrentPoint(this, _control, direction);
            Debug.WriteLine($"OnMove: new point: {currentPoint?.Name},{currentPoint?.ParentPath}, {currentPoint?.Directions?.Count} - move to - {direction}");

            Move(direction);
            return currentPoint;
        }

        public Direction GetNextPoint()
        {
            Directions.Sort();
            foreach (var direction in Directions)
            {

                string point = null;
                switch (direction)
                {
                    case Direction.Up:
                        point = new Point(Location.X + 1, Location.Y).GetName();
                        break;
                    case Direction.Down:
                        point = new Point(Location.X - 1, Location.Y).GetName();
                        break;
                    case Direction.Rigth:
                        point = new Point(Location.X, Location.Y + 1).GetName();
                        break;
                    case Direction.Left:
                        point = new Point(Location.X, Location.Y - 1).GetName();
                        break;
                }
                if (string.IsNullOrEmpty(point))
                    continue;
                if (!Points.ContainsKey(point))
                {
                    return direction;
                }
            }
            return default(Direction);
        }

        public CurrentPoint Return()
        {
            Debug.WriteLine($"OnReturn: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count}");
            if (Directions == null)
                return this;
            var direction = Directions.FirstOrDefault();
            if (direction == default(Direction))
            {
                Debug.WriteLine($"OnReturn: Direction Not Found: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count}: {String.Join(";", this?.Directions)}");
                Move(ParentPath);
                return _parentPoint;
            }
            //Directions.Delete(direction);
            Debug.WriteLine($"OnReturn: Direction Found: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count}: {String.Join(";", this?.Directions)}");
            return this;
        }

        private void Move(Direction direction)
        {
            Debug.WriteLine($"Move Func: {this?.Name},{this?.ParentPath}, {this?.Directions?.Count}: {String.Join(";", this?.Directions)} to - {direction}");
            Directions?.Remove(direction);

            OnMove?.Invoke(this, direction);
            _control.RunJs("MoveTo(" + (int)direction + ");");
        }

        public static event MoveEventHandler OnMove = delegate { };
        private Direction _path;
        private readonly CurrentPoint _parentPoint;
        public static Dictionary<string, CurrentPoint> Points { get; } = new Dictionary<string, CurrentPoint>();
    }

    public delegate void MoveEventHandler(object sender, Direction direction);
}