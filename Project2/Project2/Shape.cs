using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    interface Shape
    {
        bool Inside(Point point);
        Color Color();
        void SetColor(Color color);
        Point Intersection(Point point, Vector ray);
    }
}
