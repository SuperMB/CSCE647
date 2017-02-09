using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
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

        public Point Point { get; set; }
        public Vector NormalVector { get; set; }
        static public Color _color = new Color(0, 0, 2, 2);
    }
}
