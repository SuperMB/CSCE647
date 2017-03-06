using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    class Function
    {
        public static double Degrees(double degrees)
        {
            return Math.PI / (180 / degrees);
        }
        public static double DotProduct(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public static Vector CrossProduct(Vector vector1, Vector vector2)
        {
            return new Vector(
                vector1.Y * vector2.Z - vector2.Y * vector1.Z,
                vector2.X * vector1.Z - vector1.X * vector2.Z,
                vector1.X * vector2.Y - vector2.X * vector1.Y
                );
        }

        public static double Angle(Vector vector1, Vector vector2)
        {
            double dotProduct = DotProduct(vector1, vector2);
            return Math.Acos(dotProduct / (vector1.Length() * vector2.Length()));
        }

        public static Vector GetVector(Point point1, Point point2)
        {
            return new Vector(
                point2.X - point1.X,
                point2.Y - point1.Y,
                point2.Z - point1.Z
                );
        }

        public static Point Add(Point point, Vector vector)
        {
            return new Point(
                point.X + vector.X,
                point.Y + vector.Y,
                point.Z + vector.Z
                );
        }
        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(
                vector1.X + vector2.X,
                vector1.Y + vector2.Y,
                vector1.Z + vector2.Z
                );
        }

        public static List<ReturnData> IntersectionPoints(Point point, Vector ray, List<Shape> shapes, List<Shape> originalShapes = null)
        {
            List<ReturnData> intersectionPoints = new List<ReturnData>();
            foreach (Shape shape in shapes)
            {
                bool assessShape = true;
                if (originalShapes != null)
                {
                    foreach (Shape originalShape in originalShapes)
                        if (originalShape == shape)
                            assessShape = false;
                }

                if (assessShape)
                {
                    ReturnData pointColor = shape.Intersection(point, ray);
                    if (pointColor != null)
                        intersectionPoints.Add(pointColor);
                }
            }

            return intersectionPoints;
        }

        public static List<ReturnData> IntersectionPoints2(Point point, Vector ray, List<Shape> shapes, List<Shape> originalShapes)
        {
            List<ReturnData> intersectionPoints = new List<ReturnData>();
            foreach (Shape shape in shapes)
            {
                bool assessShape = true;
                if (originalShapes != null)
                {
                    foreach (Shape originalShape in originalShapes)
                        if (originalShape == shape)
                            assessShape = false;
                }

                if (assessShape)
                {
                    ReturnData pointColor = shape.Intersection(point, ray);
                    if (pointColor != null)
                        intersectionPoints.Add(pointColor);
                }
            }

            return intersectionPoints;
        }

        private static double IntersectionShadowCalculation(double angle, AngleDirection angleDirection, double minAngle)
        {
            return 0;

            double shadow = 0;
            double val = Function.Degrees(90) - minAngle / 2;
            if (angleDirection == AngleDirection.AngleIncreasing)
            {
                if (angle > minAngle)
                    shadow = 1 - (Function.Degrees(90) - angle) / (Function.Degrees(90) - minAngle);
            }
            else
            {
                shadow = 0;
            }

            return shadow;
        }

        public static double IntersectionShadow(ReturnData data, Vector vectorToLight, List<Shape> shapes, double minAngle = 0, double maxAngle = 0)
        {
            double cosTheta = DotProduct(vectorToLight, data.NormalVector);
            double angle = Math.Acos(cosTheta);
            double shadowFromIntersection = 1;
            List<ReturnData> intersections = IntersectionPoints2(data.Point, vectorToLight, shapes, data.NonIntersectingShapes);
            if (intersections.Count > 0)
            {
                double largestAngle = 0;
                AngleDirection angleDirection = AngleDirection.AngleIncreasing;
                foreach (ReturnData intersection in intersections)
                {
                    double angleWithIntersection = Math.Acos(DotProduct(vectorToLight * -1, intersection.NormalVector));
                    if (angleWithIntersection > largestAngle)
                    {
                        largestAngle = angleWithIntersection;
                        angleDirection = intersection.AngleDirection;
                    }
                }
                shadowFromIntersection = IntersectionShadowCalculation(largestAngle, angleDirection, minAngle);
            }
            else
                return 1;

            return shadowFromIntersection;// Math.Min(shadowFromAngle, shadowFromIntersection);
        }

        public static double AngleShadow(double angle, double minAngle, double maxAngle)
        {
            double shadow = 1;
            if (angle > maxAngle)
                shadow = 0;
            else if (angle > minAngle)
            {
                shadow = (1 - (angle - minAngle) / (maxAngle - minAngle));
            }
            return shadow;
        }
    }
}
