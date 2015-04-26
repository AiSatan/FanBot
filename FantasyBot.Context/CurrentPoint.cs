using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace FantasyBot.Context
{
    public class CurrentPoint
    {
        public List<Directions> Directions { get; set; }
        public Point Location { get; set; }

        public string Name => Location.ToString();

        public Stack<Directions> Path { get; set; }
    }
} 