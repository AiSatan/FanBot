using System.Collections.Generic;
using System.Drawing;

namespace FantasyBot.Context
{
    public interface IGPoint
    {
        /// <summary>
        /// Имя точки
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Координата точки
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// Все возможные стороны движения точки
        /// </summary>
        List<Direction> Directions { get; set; }

        /// <summary>
        /// Обратный путь к родительской точке
        /// </summary>
        Direction ParentPath { get; set; }

        /// <summary>
        /// Перемещает на шаг вперед и возвращает 
        /// новую точку с данным предком
        /// </summary>
        /// <returns></returns>
        CurrentPoint Move();

        /// <summary>
        /// Возвращаемся на шаг назад
        /// </summary>
        /// <returns>Родительская точка</returns>
        CurrentPoint Return();
    }
}