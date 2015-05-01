﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FantasyBot.Context;

namespace FantasyBot
{
    public partial class MapForm : Form
    {
        private Bitmap _map;

        public MapForm()
        {
            InitializeComponent();
        }

        private void Map_Load(object sender, EventArgs e)
        {
            _map = new Bitmap(500, 500);
            pbMap.Image = _map;
        }

        public void UpdateMap()
        {
            pbMap.ThreadSafe(() => pbMap.Image = _map);
        }

        public void WritePoint(Point location, List<Direction> directs)
        {
            var x = location.X * 4;
            var y = location.Y * 4;
            Color color;
            switch (directs.Count)
            {
                case 0:
                    color = Color.Black;
                    break;
                case 1:
                    color = Color.Green;
                    break;
                case 2:
                    color = Color.DarkOrange;
                    break;
                case 3:
                    color = Color.Red;
                    break;

                default:
                    return;
            }
            for (var mx = 0; mx < 4; mx++)
            {
                for (var my = 0; my < 4; my++)
                {
                    _map.SetPixel(x - mx, y - my, color);
                }
            }
            _map.SetPixel(x, y, color);
            _map.SetPixel(x, y, color);
            UpdateMap();
        }

        public void OnMove(object sender, Direction direction)
        {
            var point = sender as CurrentPoint;
            if (point == null)
                return;
            WritePoint(point.Location, point.Directions);
        }
    }
}