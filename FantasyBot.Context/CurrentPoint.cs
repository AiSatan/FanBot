using System;
using System.Collections;
using System.Collections.Generic;
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
        public List<Direction> Directions { get; set; } = new List<Direction>();

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

        public CurrentPoint(CurrentPoint parentPoint, AddressBox control)
        {
            _control = control;
            _parentPoint = parentPoint;
        }

        public CurrentPoint(CurrentPoint parentPoint, AddressBox control, Direction parentPath) : this(parentPoint, control)
        {
            ParentPath = parentPath;
        }

        public CurrentPoint Move()
        {
            Directions.Remove(_path);

            var direction = Directions.FirstOrDefault();
            if (direction == default(Direction))
            {
                return Return();
            }
            Directions.Delete(direction);

            var currentPoint = new CurrentPoint(this, _control, direction);

            _control.RunJs("MoveTo(" + (int)direction + ");");
            return currentPoint;
        }

        public CurrentPoint Return()
        {
            _control.RunJs("MoveTo(" + (int)ParentPath + ");");
            return _parentPoint;
        }

        private Direction _path;
        private readonly CurrentPoint _parentPoint;
    }
} 