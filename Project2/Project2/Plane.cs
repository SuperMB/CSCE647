using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Plane : Shape
    {
        public Plane(Point point, Vector normalVector)
        {
            Point = point;
            NormalVector = normalVector.UnitVector();
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(Point, point);
            //return Math.Abs(Function.DotProduct(vector, NormalVector)) < 1;
            return Function.DotProduct(vector, NormalVector) < 0;
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
            double denominator = Function.DotProduct(NormalVector, ray);
            if (denominator == 0)
                return null;

            double rayScaling = Function.DotProduct(NormalVector, point - Point) / denominator;
            if (rayScaling > 0)
                return null;

            return point + ray * (-1 * rayScaling);
        }

        public Point Point { get; set; }
        public Vector NormalVector { get; set; }
        public Color _color = new Color(0, 0, 2, 2);
    }
}
