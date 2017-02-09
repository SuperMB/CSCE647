using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
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

        public Point Intersection(Point point, Vector ray)
        {
            double b = Function.DotProduct(ray, Center - point);
            double c = Function.DotProduct(Center - point, Center - point) - Math.Pow(Radius, 2);
            double delta = Math.Pow(b, 2) - c;

            if (b >= 0 & delta >= 0)
            {
                double intersectDistance = b - Math.Sqrt(delta);
                return point + ray * intersectDistance;
            }

            return null;
        }

        public Point Center { get; set; }
        public double Radius { get; set; }
        public Color _color = new Color(0, .2, 0, .2);
    }
}
