using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class PointColor
    {
        public PointColor()
        {
            Point = null;
            Color = null;
        }

        public PointColor(Point point, Color color)
        {
            Point = point;
            Color = color;
        }

        public Point Point { get; set; }
        public Color Color { get; set; }
    }
}
