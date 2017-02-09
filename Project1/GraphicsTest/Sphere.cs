using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
{
    class Sphere : Shape
    {
        public Sphere(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(point, Center);
            return vector.Length() - Radius < 0;
        }

        public Color Color()
        {
            return _color;
        }
        public void SetColor(Color color)
        {
            _color = color;
        }

        public Point Center { get; set; }
        public double Radius { get; set; }
        static public Color _color = new Color(0, .2, 0, .2);
    }
}
