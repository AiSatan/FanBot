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
        public static Dictionary<string, CurrentPoint> Points { get; } = new Dictionary<string, CurrentPoint>();

        /// <summary>
        /// Создание новой точки
        /// </summary>
        /// <param name="parentPoint">точка предок</param>
        /// <param name="control">элемент управления</param>
        /// <param name="parentPath">направление по которому пришли к этой точке</param>
        public CurrentPoint(CurrentPoint parentPoint, WebControl control, Direction parentPath)
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
                Debug.WriteLine($"ParentPath, установлен путь к родителю для: {Name}, в направлении: {_path}");
            }
        }

        /// <summary>
        /// Перемещает на шаг вперед и возвращает 
        /// новую точку с данным предком.
        /// null - Если возможных направлении больше нет
        /// </summary>
        /// <returns></returns>
        public CurrentPoint Move()
        {
            Debug.WriteLine($"Move, Начинаем метод движения: {Name}");

            //поолучаем следующию точку
            var direction = GetNextPoint();
            if (direction == default(Direction))
            {
                Debug.WriteLine($"Move, Направлении не найдено для: {Name}");
                //Если возможных направлении больше нет
                return null;
            }
            Debug.WriteLine($"Move, Начинаем движение: {Name}, в сторону: {direction}");
            //движемся
            Move(direction);

            //удаляем это движение из возможных
            RemoveDirection(direction);
            //создаем новую точку
            var child = new CurrentPoint(this, _control, direction);
            return child;
        }

        /// <summary>
        /// Возвращаемся на шаг назад
        /// </summary>
        /// <returns>Родительская точка</returns>
        public CurrentPoint Return()
        {
            Debug.WriteLine($"Return, Делаем шаг назад из: {Name}, в сторону: {ParentPath}, к: {_parentPoint?.Name}");

            //возвращаемся назад
            Move(ParentPath);
            //отдаем старый поинт
            return _parentPoint;
        }
        #endregion

        /// <summary>
        /// Движение
        /// </summary>
        /// <param name="direction">Направление</param>
        private void Move(Direction direction)
        {
            Debug.WriteLine($"Move Func: this point: {this?.Name}, parent: {this?.ParentPath}, dir.Count: {this?.Directions?.Count}: {String.Join("-", this?.Directions)} moving to - {direction}");
            //вызываем событие
            OnMove?.Invoke(this, direction);
            var intValue = (int)direction;
            //двигаемся
            _control.ExecuteJavascript($"MoveTo({intValue});");
        }

        private void RemoveDirection(Direction direction)
        {
            Debug.WriteLine($"RemoveDirection, Удаляем из: {Name}, сторону: {direction}");
            //удаляем у данной точки это направление, оно нам более не нужно
            var result = Directions?.Remove(direction);
            Debug.WriteLine($"RemoveDirection, Удаление из: {Name}, стороны: {direction}, успешно: {result}");
        }

        /// <summary>
        /// Возвращает первое возможное направление
        /// </summary>
        /// <returns>Направление</returns>
        private Direction GetNextPoint()
        {
            Directions.Sort();
            //не смотрим предка
            foreach (var direction in Directions.Where(direction => direction != ParentPath).ToList())
            {
                string point;
                switch (direction)
                {
                    case Direction.Up:
                        point = new Point(Location.X, Location.Y + 1).GetName();
                        break;
                    case Direction.Down:
                        point = new Point(Location.X, Location.Y - 1).GetName();
                        break;
                    case Direction.Rigth:
                        point = new Point(Location.X + 1, Location.Y).GetName();
                        break;
                    case Direction.Left:
                        point = new Point(Location.X - 1, Location.Y).GetName();
                        break;
                    default:
                        continue;
                }
                //возвращаем направление если мы в нем еще небыли
                if (!Points.ContainsKey(point))
                {
                    Debug.WriteLine($"GetNextPoint, Найден путь у: {Name}, в сторону: {direction}, к: {point}");

                    return direction;
                }
                Debug.WriteLine($"GetNextPoint, Найден уже пройденный путь у: {Name}, в сторону: {direction}, к: {point}");
                RemoveDirection(direction);
            }

            Debug.WriteLine($"GetNextPoint, не найдены пути для: {Name}");

            return default(Direction);
        }

        public void PickUp(string value)
        {
            Debug.WriteLine($"PickUp: поднимаем item: {value}, в точке: {this?.Name}");
            _control.ExecuteJavascript($"PickUpItem({value});");
        }

        public void InvokeQuest(string action)
        {
            Debug.WriteLine($"InvokeQuest: выполняем action: {action}, в точке: {this?.Name}");
            _control.ExecuteJavascript($"InvokeAction({action});");
        }

        private Direction _path;
        private readonly CurrentPoint _parentPoint;
        private readonly WebControl _control;
        public static event MoveEventHandler OnMove = delegate { };
    }

    public delegate void MoveEventHandler(object sender, Direction direction);
}