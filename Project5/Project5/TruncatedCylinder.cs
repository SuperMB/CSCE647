using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    class TruncatedCylinder : Shape
    {
        public TruncatedCylinder(Point point, Vector direction, double radius, double height)
        {
            Point = point;
            Direction = direction.UnitVector();
            Radius = radius;
            Height = height;
            _sideColorMatrix = ColorMatrix.DarkGray();
            _bottomColorMatrix = ColorMatrix.White();
            _topColorMatrix = ColorMatrix.White();
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(Point, point);
            double length = vector.Length();
            double angle = Math.Acos(Function.DotProduct(vector, Direction) / (vector.Length() * Direction.Length()));
            double distance = Math.Sin(angle) * length;
            return distance < Radius;
        }
        public ColorMatrix GetColorMatrix(Point point)
        {
            //Vector vector = point - Point;
            //if (Function.DotProduct(vector, Direction) == 0)
            //    return new ColorMatrix(1, 0, 0);

            return _sideColorMatrix;
        }

        public void SetColorMatrix(ColorMatrix colorMatrix)
        {
            _sideColorMatrix = colorMatrix;
            _bottomColorMatrix = colorMatrix;
            _topColorMatrix = colorMatrix;
        }


        public ReturnData IntersectBottom(Point point, Vector ray)
        {
            Plane bottomPlane = new Plane(Point, Direction);
            ReturnData bottomReturnData = bottomPlane.Intersection(point, ray);
            if (bottomReturnData != null)
            {
                Point planeIntersection = bottomReturnData.Point;
                if ((planeIntersection - Point).Length() < Radius)
                    return new ReturnData
                    {
                        Point = planeIntersection,
                        ColorMatrix = _bottomColorMatrix,
                        NormalVector = -1 * Direction,
                        AngleDirection = AngleDirection.AngleDecreasing,
                        NonIntersectingShapes = new List<Shape> { this }
                    };
            }

            return null;
        }
        public ReturnData IntersectTop(Point point, Vector ray)
        {
            Point topCenter = Point + Direction * Height;
            Plane topPlane = new Plane(topCenter, Direction);
            ReturnData topReturnData = topPlane.Intersection(point, ray);
            if (topReturnData != null)
            {
                Point planeIntersection2 = topReturnData.Point;
                if ((planeIntersection2 - topCenter).Length() < Radius)
                {
                    ColorMatrix color = _topColorMatrix;
                    double distanceOut = (planeIntersection2 - topCenter).Length();
                    if (distanceOut  > Radius * .8
                        | (distanceOut < Radius * .65 & distanceOut > Radius * .6)
                        )
                        color = _sideColorMatrix;

                    return new ReturnData
                    {
                        Point = planeIntersection2,
                        ColorMatrix = color,
                        NormalVector = Direction,
                        AngleDirection = AngleDirection.AngleDecreasing,
                        NonIntersectingShapes = new List<Shape> { this }
                    };
                }
            }

            return null;
        }
        public ReturnData IntersectSide(Point point, Vector ray)
        {
            Vector v1 = Function.CrossProduct(ray, Direction);
            Vector v2 = Function.CrossProduct(point - Point, Direction);
            double a = Math.Pow(v1.Length(), 2);
            double b = 2 * Function.DotProduct(v1, v2);
            double c = Math.Pow(v2.Length(), 2) - Radius * Radius;


            if (b * b - 4 * a * c > 0)
            {
                bool addIntersectionPoint = true;
                double delta = Math.Sqrt(b * b - 4 * a * c);
                double a2 = (2 * a);
                double t0 = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double t1 = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                double t = Math.Min((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a), (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a));
                Point intersectionPoint = point + (ray * t);
                Vector intersectionVector = intersectionPoint - Point;

                Vector intersectionDirection = intersectionVector.UnitVector();
                if (Function.DotProduct(Direction, intersectionDirection) < 0)
                    addIntersectionPoint = false;

                double value = Function.DotProduct(intersectionVector, Direction);
                if (value > Height)
                    addIntersectionPoint = false;

                if (addIntersectionPoint)
                {
                    double directionScale = Function.DotProduct(Direction, intersectionVector);
                    Vector scaledDirection = Direction * directionScale;
                    Vector normalVector = (intersectionVector - scaledDirection).UnitVector();
                    return new ReturnData
                    {
                        Point = intersectionPoint,
                        ColorMatrix = _sideColorMatrix,
                        NormalVector = normalVector,
                        AngleDirection = AngleDirection.AngleDecreasing,
                        NonIntersectingShapes = new List<Shape> { this }
                    };
                }
            }

            return null;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            List<ReturnData> returnData = new List<ReturnData>();

            ReturnData returnDataBottom = IntersectBottom(point, ray);
            if (returnDataBottom != null)
                returnData.Add(returnDataBottom);

            ReturnData returnDataTop = IntersectTop(point, ray);
            if (returnDataTop != null)
                returnData.Add(returnDataTop);

            ReturnData returnDataSide = IntersectSide(point, ray);
            if (returnDataSide != null)
                returnData.Add(returnDataSide);

            double min = 1000000;
            int index = -1;
            for (int i = 0; i < returnData.Count; i++)
            {
                double value = (returnData[i].Point - point).Length();
                if(returnData.Count > 1)
                    value = (returnData[i].Point - point).Length();

                if ( value < min)
                {
                    min = (returnData[i].Point - point).Length();
                    index = i;
                }
            }

            if (index >= 0)
            {
                return returnData[index];
            }            

            return null;
        }


        public Point Point { get; set; }
        public Vector Direction { get; set; }
        public double Radius { get; set; }
        public double Height { get; set; }

        private ColorMatrix _sideColorMatrix;
        private ColorMatrix _topColorMatrix;
        private ColorMatrix _bottomColorMatrix;
    }
}
