using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project11
{
    class Triangle : Shape
    {
        public Triangle(Point point0, Point point1, Point point2)
        {
            Point0 = point0;
            Point1 = point1;
            Point2 = point2;
            Color = Color.BlueColor;
            Vector normalVector = Function.CrossProduct(Point1 - Point0, Point2 - Point0).UnitVector();
            Plane = new Plane(Point0, normalVector);
        }
        public Color GetColor(Point point)
        {
            return Color;
        }
        public void SetColor(Color color)
        {
            Color = color;
        }
        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData returnData = Plane.Intersection(point, ray);

            if (returnData == null)
                return null;
            if (!(InsideTriangle(returnData.Point)))
                return null;

            returnData.Color = Color;
            returnData.NonIntersectingShapes = new List<Shape> { this };
            return returnData;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public bool InsideTriangle(Point point)
        {
            if (point.X < 500 & point.X > 0 & point.Y < 500 & point.Y > 0)
                point = point; 

            Vector area = Function.CrossProduct(Point1 - Point0, Point2 - Point0);

            Vector area2 = Function.CrossProduct(Point0 - point, Point1 - point);
            Vector area0 = Function.CrossProduct(Point1 - point, Point2 - point);
            Vector area1 = Function.CrossProduct(Point2 - point, Point0 - point);

            //if(area0.Length() + area1.Length() + area2.Length() < 1.01 * area.Length()
            //    && area0.Length() + area1.Length() + area2.Length() > .99 * area.Length())
            //    return true;

            //return false;

            if (Math.Abs(area.X) > Math.Abs(area.Y)
                && Math.Abs(area.X) > Math.Abs(area.Z))
            {
                return (area0.X / area.X > 0 || (area.X == 0))
                    && (area1.X / area.X > 0 || (area.X == 0))
                    && (area2.X / area.X > 0 || (area.X == 0));
            }
            else if (Math.Abs(area.Y) > Math.Abs(area.Z))
            {
                return ((area0.Y / area.Y > 0) || (area.Y == 0))
                    && ((area1.Y / area.Y > 0) || (area.Y == 0))
                    && ((area2.Y / area.Y > 0) || (area.Y == 0));

            }
            else
            {
                return ((area0.Z / area.Z > 0) || (area.Z == 0))
                    && ((area1.Z / area.Z > 0) || (area.Z == 0))
                    && ((area2.Z / area.Z > 0) || (area.Z == 0));
            }


            //double d1 = area0.X / area.X;
            //double d2 = area1.X / area.X;
            //double d3 = area2.X / area.X;
            //double d4 = area0.Y / area.Y;
            //double d5 = area1.Y / area.Y;
            //double d6 = area2.Y / area.Y;
            //double d7 = area0.Z / area.Z;
            //double d8 = area1.Z / area.Z;
            //double d9 = area2.Z / area.Z;

            //return ((area0.X / area.X > 0) || (area.X == 0))
            //    && ((area1.X / area.X > 0) || (area.X == 0))
            //    && ((area2.X / area.X > 0) || (area.X == 0))
            //    && ((area0.Y / area.Y > 0) || (area.Y == 0))
            //    && ((area1.Y / area.Y > 0) || (area.Y == 0))
            //    && ((area2.Y / area.Y > 0) || (area.Y == 0))
            //    && ((area0.Z / area.Z > 0) || (area.Z == 0))
            //    && ((area1.Z / area.Z > 0) || (area.Z == 0))
            //    && ((area2.Z / area.Z > 0) || (area.Z == 0));
        }

        public Point GetUV(Point point)
        {
            Vector area = Function.CrossProduct(Point1 - Point0, Point2 - Point0);

            Vector area1 = Function.CrossProduct(Point2 - point, Point0 - point);
            Vector area2 = Function.CrossProduct(Point0 - point, Point1 - point);

            return new Point(
                area1.Length() / area.Length(),
                area2.Length() / area.Length(),
                0
                );
        }

        public Point Point0 { get; set; }
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Plane Plane { get; set; }
        public Color Color { get; set; }
    }
}
