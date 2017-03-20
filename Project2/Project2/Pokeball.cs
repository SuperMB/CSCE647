using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class Pokeball : Shape
    {
        public Pokeball(Point center, double radius, Vector upVector, Vector outVector)
        {
            _sphere = new Sphere(center, radius);
            UpVector = upVector;
            OutVector = outVector;
            _dot = new TruncatedCylinder(
                center + radius * OutVector,
                OutVector,
                radius / 5,
                radius / 50);
        }

        public Color Color(Point point)
        {
            Vector vector = point - _sphere.Center;
            //if (vector.Length() > Radius)
                //return new Color();

            double angle = Function.Angle(vector, UpVector);
            double blackAngle = Math.PI / 100;

            if (angle < Math.PI / 2 + blackAngle & angle > Math.PI / 2 - blackAngle)
                return DarkGray;
            else if (angle < Math.PI / 2)
                return PokeballTopColor;
            else
                return PokeballBottomColor;
        }

        public void SetColor(Color color)
        {

        }

        public bool Inside(Point point)
        {
            if (_sphere.Inside(point))
                return true;
            else
                return _dot.Inside(point);
        }

        public PointColor Intersection(Point point, Vector ray)
        {
            List<PointColor> pointColors = new List<PointColor>();

            PointColor pointColor = _sphere.Intersection(point, ray);
            if (pointColor != null)
            {
                Point sphereIntersection = pointColor.Point;
                pointColors.Add(new PointColor
                {
                    Point = sphereIntersection,
                    Color = Color(sphereIntersection)
                });
            }

            pointColor = _dot.Intersection(point, ray);
            if (pointColor != null)
                pointColors.Add(pointColor);

            double min = 1000000;
            int index = -1;
            for (int i = 0; i < pointColors.Count; i++)
            {
                double value = (pointColors[i].Point - point).Length();
                if (value < min)
                {
                    min = (pointColors[i].Point - point).Length();
                    index = i;
                }
            }

            if (index >= 0)
            {
                return pointColors[index];
            }
            

            return null;
        }

        private Sphere _sphere;
        private TruncatedCylinder _dot;

        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        public static readonly Color PokeballTopColor = new Color(1, 0, 0, 1);
        public static readonly Color PokeballBottomColor = new Color(1, 1, 1, 1);
        public static readonly Color DarkGray = new Color(.1, .1, .1);
    }
}
