using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    class Pokeball : Shape
    {
        public Pokeball(Point center, double radius, Vector upVector, Vector outVector)
        {
            Sphere = new Sphere(center, radius);
            OutVector = outVector.UnitVector();
            Vector perpendicularVector = Function.CrossProduct(OutVector, upVector);
            UpVector = Function.CrossProduct(perpendicularVector, OutVector).UnitVector();

            //UpVector = upVector.UnitVector();
            Dot = new TruncatedCylinder(
                center + radius * OutVector,
                OutVector,
                radius / 5,
                radius / 50);
        }

        public ColorMatrix GetColorMatrix(Point point)
        {
            Vector vector = point - Sphere.Center;
            //if (vector.Length() > Radius)
                //return new Color();

            double angle = Function.Angle(vector, UpVector);
            double blackAngle = Function.Degrees(3);

            if (angle < Math.PI / 2 + blackAngle & angle > Math.PI / 2 - blackAngle)
                return _darkGray;
            else if (angle < Math.PI / 2)
                return _pokeballTopColor;
            else
                return _pokeballBottomColor;
        }

        public void SetColorMatrix(ColorMatrix color)
        {

        }

        public bool Inside(Point point)
        {
            if (Sphere.Inside(point))
                return true;
            else
                return Dot.Inside(point);
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            List<ReturnData> returnData = new List<ReturnData>();

            ReturnData sphereReturnData = Sphere.Intersection(point, ray, false);
            if (sphereReturnData != null)
            {
                Point sphereIntersection = sphereReturnData.Point;
                returnData.Add(new ReturnData
                {
                    Point = sphereIntersection,
                    ColorMatrix = GetColorMatrix(sphereIntersection),
                    NormalVector = sphereReturnData.NormalVector,
                    NonIntersectingShapes = new List<Shape> { Sphere }
                });
            }

            ReturnData dotReturnData = Dot.Intersection(point, ray);
            if (dotReturnData != null)
            {
                dotReturnData.NonIntersectingShapes.Add(Sphere);
                returnData.Add(dotReturnData);
            }

            double min = 1000000;
            int index = -1;
            for (int i = 0; i < returnData.Count; i++)
            {
                double value = (returnData[i].Point - point).Length();
                if (value < min)
                {
                    min = (returnData[i].Point - point).Length();
                    index = i;
                }
            }

            if (index >= 0)
            {
                return returnData[index];
            }

            ReturnData sphereOutline = Sphere.Outline(point, ray);
            if (sphereOutline != null)
            {
                return sphereOutline;
            }

            return null;
        }

        public Sphere Sphere { get; set; }
        public TruncatedCylinder Dot;

        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        private static readonly ColorMatrix _pokeballTopColor = ColorMatrix.Red();
        private static readonly ColorMatrix _pokeballBottomColor = ColorMatrix.White();
        private static readonly ColorMatrix _darkGray = ColorMatrix.DarkGray();
    }
}
