using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project11
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
            double closestDistance = 10000000000000;
            bool valid = true;
            foreach (Shape shape in shapes)
            {
                bool assessShape = true;
                if (originalShapes != null)
                {
                    foreach (Shape originalShape in originalShapes)
                        if (originalShape == shape)
                            assessShape = false;
                }

                //if (assessShape)
                //{
                ReturnData pointColor = shape.Intersection(point, ray);
                if (pointColor != null)
                {
                    double distance = (pointColor.Point - point).Length();
                    if (distance < closestDistance)
                    {
                        valid = assessShape;
                        closestDistance = distance;
                    }
                    intersectionPoints.Add(pointColor);
                }
                //}
            }
            if (!valid)
                return new List<ReturnData>();

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
            Point point = data.Point;
            if (data.LightPoint != null)
                point = data.LightPoint;
            List<ReturnData> intersections = IntersectionPoints(point, vectorToLight, shapes, data.NonIntersectingShapes);
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

        public static int Closest(List<ReturnData> intersectionPoints, Point point)
        {
            int index = -1;

            if (intersectionPoints.Count < 1)
                return index;

            double closestDistance = 1000000000000;
            for (int k = 0; k < intersectionPoints.Count; k++)
            {
                ReturnData pointColor = intersectionPoints[k];
                double distance = (pointColor.Point - point).Length();
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    index = k;
                }
            }

            return index;
        }

        public static ReturnData Reflect(Point point, Vector ray, Vector normalVector, List<Shape> shapes, Shape shape)
        {
            Vector reflectedVector = (ray - 2 * Function.DotProduct(ray, normalVector) * normalVector).UnitVector();

            List<ReturnData> intersectionPoints = Function.IntersectionPoints(point, reflectedVector, shapes, new List<Shape> { shape });

            int closestIndex = Function.Closest(intersectionPoints, point);
            if (closestIndex >= 0)
            {
                ReturnData returnData = intersectionPoints[closestIndex];
                //returnData.LightPoint = returnData.Point;
                return returnData;
            }

            return null;
        }
        public static ReturnData Reflect(Point point, Vector ray, Vector normalVector, List<Shape> shapes, List<Shape> originalShapes)
        {
            Vector reflectedVector = (ray - 2 * Function.DotProduct(ray, normalVector) * normalVector).UnitVector();

            List<ReturnData> intersectionPoints = Function.IntersectionPoints(point, reflectedVector, shapes, originalShapes);

            int closestIndex = Function.Closest(intersectionPoints, point);
            if (closestIndex >= 0)
            {
                ReturnData returnData = intersectionPoints[closestIndex];
                //returnData.LightPoint = returnData.Point;
                return returnData;
            }

            return null;
        }
        
        public static ReturnData RefractNormalMap(double refractiveIndex, Point point, Vector ray, Vector originalNormal, Vector normalVector, List<Shape> shapes, List<Shape> originalShapes)
        {
            ray = (ray * -1).UnitVector();
            originalNormal = originalNormal.UnitVector();
            normalVector = normalVector.UnitVector();
            double C = Function.DotProduct(ray, originalNormal);
            double squareRootFactor = (C * C - 1) / (refractiveIndex * refractiveIndex) + 1;
            Vector transmitted = null;

            //while (squareRootFactor <= 0)
            //{
            //    refractiveIndex += .1;
            //    squareRootFactor = (C * C - 1) / (refractiveIndex * refractiveIndex) + 1;
            //}
            //if (squareRootFactor > 1)
            //    squareRootFactor = 1;

            if (squareRootFactor > 0)
            {
                C = Function.DotProduct(ray, normalVector);
                squareRootFactor = (C * C - 1) / (refractiveIndex * refractiveIndex) + 1;
                if (squareRootFactor < 0)
                {
                    C = Function.DotProduct(ray, originalNormal);
                    squareRootFactor = (C * C - 1) / (refractiveIndex * refractiveIndex) + 1;
                    transmitted = (-1 / refractiveIndex) * ray + ((C / refractiveIndex) - Math.Sqrt(squareRootFactor)) * originalNormal;
                }
                else
                {
                    transmitted = (-1 / refractiveIndex) * ray + ((C / refractiveIndex) - Math.Sqrt(squareRootFactor)) * normalVector;
                }
            }
            else
            {
                return null;
                ReturnData rd = Reflect(point, ray, normalVector, shapes, originalShapes);
                if (rd != null)
                    rd.Color += new Color(.1, .1, .1, 3);
                return rd;
            }

            List<ReturnData> intersectionPoints = Function.IntersectionPoints(point, transmitted, shapes, originalShapes);

            int closestIndex = Function.Closest(intersectionPoints, point);
            if (closestIndex >= 0)
            {
                ReturnData returnData = intersectionPoints[closestIndex];
                //returnData.LightPoint = returnData.Point;
                return returnData;
            }

            return null;
        }
        public static ReturnData Refract(double refractiveIndex, Point point, Vector ray, Vector normalVector, List<Shape> shapes, List<Shape> originalShapes)
        {
            ray = (ray * -1).UnitVector();
            normalVector = normalVector.UnitVector();
            double C = Function.DotProduct(ray, normalVector);
            double squareRootFactor = (C * C - 1) / (refractiveIndex * refractiveIndex) + 1;
            Vector transmitted = null;
            if (squareRootFactor > 0)
                transmitted = (-1 / refractiveIndex) * ray + ((C / refractiveIndex) - Math.Sqrt(squareRootFactor)) * normalVector;
            else
            {
                return null;
                ReturnData rd = Reflect(point, ray, normalVector, shapes, originalShapes);
                if (rd != null)
                    rd.Color += new Color(.1, .1, .1, 3);
                return rd;
            }

            List<ReturnData> intersectionPoints = Function.IntersectionPoints(point, transmitted, shapes, originalShapes);

            int closestIndex = Function.Closest(intersectionPoints, point);
            if (closestIndex >= 0)
            {
                ReturnData returnData = intersectionPoints[closestIndex];
                //returnData.LightPoint = returnData.Point;
                return returnData;
            }

            return null;
        }

        public static ReturnData Gloss(double glossAmount, Point point, Vector ray, Vector normalVector, List<Shape> shapes, List<Shape> originalShapes)
        {
            Vector reflectedVector = (ray - 2 * Function.DotProduct(ray, normalVector) * normalVector).UnitVector();
            return Translucent(.2, point, reflectedVector, shapes, originalShapes, null);
        }

        public static ReturnData Translucent(double translucentAmount, Point point, Vector ray, List<Shape> shapes, List<Shape> originalShapes, Vector direction)
        {
            translucentAmount = .2;
            Color color = new Color(0, 0, 0, 0);
            int numberOfRuns = 150;
            ReturnData returnData = null;
            //Vector transmitted = (ray).UnitVector();

            int matchedRuns = 0;

            for (int i = 0; i < numberOfRuns; i++)
            {
                Vector transmitted = (ray + ((new Vector(_random.NextDouble(), _random.NextDouble(), _random.NextDouble())).UnitVector() * translucentAmount)).UnitVector();
                List<ReturnData> intersectionPoints = Function.IntersectionPoints(point, transmitted, shapes, originalShapes);

                int closestIndex = Function.Closest(intersectionPoints, point);
                if (closestIndex >= 0)
                {
                    returnData = intersectionPoints[closestIndex];
                    color = color + returnData.Color;
                    matchedRuns++;
                }
            }

            if (returnData != null)
            {
                color /= matchedRuns;
                //color.Omega = 1;
                color.Omega /= matchedRuns;

                color.IgnoreEffects = returnData.Color.IgnoreEffects;
                returnData.Color = color;
                return returnData;
            }
            else
                return null;
        }

        public static Color Illuminate(ReturnData returnData)
        {
            if (!_illuminate)
                return returnData.Color;

            Color color = new Color(0, 0, 0, 0);
            foreach (Light light in _lights)
                color += light.ShineOnShape(returnData, _alpha, _intersectingShapes);

            return color;
        }

        public static Vector RotateX(Vector vector, double theta)
        {
            Matrix rotationMatrix = Matrix.RotationXMatrix(theta);
            return rotationMatrix * vector;
        }
        public static Vector RotateY(Vector vector, double theta)
        {
            Matrix rotationMatrix = Matrix.RotationYMatrix(theta);
            return rotationMatrix * vector;
        }
        public static Vector RotateZ(Vector vector, double theta)
        {
            Matrix rotationMatrix = Matrix.RotationZMatrix(theta);
            return rotationMatrix * vector;
        }

        public static Vector Rotate(Vector rotateVector, Vector aroundVector, double theta)
        {
            Vector rotateUnit = rotateVector.UnitVector();
            Vector aroundUnit = aroundVector.UnitVector();

            Vector x = aroundUnit;
            Vector z = CrossProduct(aroundUnit, rotateUnit).UnitVector();
            Vector y = CrossProduct(z, x).UnitVector();

            Vector rotateX = DotProduct(rotateUnit, x) * x;
            Vector rotateY = DotProduct(rotateUnit, y) * y;
            double rotateYLength = rotateY.Length();

            Vector remainingX = rotateX;
            Vector remainingY = rotateY * Math.Cos(theta);
            Vector remainingZ = z * rotateY.Length() * Math.Sin(theta);

            return remainingX + remainingY + remainingZ;
        }

        static public bool _illuminate { get; set; }
        static public List<Light> _lights { get; set; }
        static public double _alpha { get; set; }
        static public List<Shape> _intersectingShapes { get; set; }
        static public Random _random = new Random();
    }
}
