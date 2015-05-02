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
    public class CurrentPoint : IGPoint
    {
        /// <summary>
        /// Создание новой точки
        /// </summary>
        /// <param name="parentPoint">точка предок</param>
        /// <param name="control">элемент управления</param>
        /// <param name="parentPath">направление по которому пришли к этой точке</param>
        public CurrentPoint(CurrentPoint parentPoint, AddressBox control, Direction parentPath)
        {
            Debug.WriteLine($"CurrentPoint, создаем точку, parent {parentPoint?.Name}");

            ParentPath = parentPath;
            _control = control;
            _parentPoint = parentPoint;
        }

        #region IGPoint
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

        /// <summary>
        /// Перемещает на шаг вперед и возвращает 
        /// новую точку с данным предком
        /// </summary>
        /// <returns></returns>
        public CurrentPoint Move()
        {
            //создаем новую точку
            var child = new CurrentPoint(this, _control, Direction.Refresh);
            //поолучаем следующию точку
            var direction = GetNextPoint();
            //устанавливаем
            child.ParentPath = direction;
            Move(direction);
            RemoveDirection(direction);
            return child;
        }

        /// <summary>
        /// Возвращаемся на шаг назад
        /// </summary>
        /// <returns>Родительская точка</returns>
        public CurrentPoint Return()
        {
            Move(ParentPath);
            return _parentPoint;
        }
        #endregion

        /// <summary>
        /// Движение
        /// </summary>
        /// <param name="direction">Направление</param>
        private void Move(Direction direction)
        {
            Debug.WriteLine($"Move Func: this point: {this?.Name}, parent: {this?.ParentPath}, dir.Count: {this?.Directions?.Count}: {String.Join("; ", this?.Directions)} moving to - {direction}");
            //вызываем событие
            OnMove?.Invoke(this, direction);
            //двигаемся
            _control.RunJs("MoveTo(" + (int)direction + ");");
        }

        private void RemoveDirection(Direction direction)
        {
            //удаляем у данной точки это направление, оно нам более не нужно
            Directions?.Remove(direction);
        }

        /// <summary>
        /// Возвращает первое возможное направление
        /// </summary>
        /// <returns>Направление</returns>
        private Direction GetNextPoint()
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

        public static event MoveEventHandler OnMove = delegate { };
        private Direction _path;
        private readonly CurrentPoint _parentPoint;
        public static Dictionary<string, CurrentPoint> Points { get; } = new Dictionary<string, CurrentPoint>();
        private readonly AddressBox _control;
    }

    public delegate void MoveEventHandler(object sender, Direction direction);
}