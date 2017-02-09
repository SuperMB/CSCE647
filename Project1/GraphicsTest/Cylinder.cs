using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
{
    class Cylinder : Shape
    {
        public Cylinder(Point point, Vector direction, Double radius)
        {
            Point = point;
            Direction = direction.UnitVector();
            Radius = radius;
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(Point, point);
            Double length = vector.Length();
            Double angle = Math.Acos(Function.DotProduct(vector, Direction) / (vector.Length() * Direction.Length()));
            Double distance = Math.Sin(angle) * length;
            return distance < Radius;
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
        public Vector Direction { get; set; }
        public Double Radius { get; set; }
        static public Color _color = new Color(.25, 0, 0, .25);
    }
}
