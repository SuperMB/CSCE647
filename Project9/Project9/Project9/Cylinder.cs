using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
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
        public ColorMatrix ColorMatrix(Point point)
        {
            return _color;
        }

        public void SetColorMatrix(ColorMatrix color)
        {
            _color = color;
        }
        public ReturnData Intersection(Point point, Vector ray)
        {
            return null;
        }


        public Point Point { get; set; }
        public Vector Direction { get; set; }
        public Double Radius { get; set; }
        public ColorMatrix _color = new ColorMatrix(.25, 0, 0, .25);
    }
}
