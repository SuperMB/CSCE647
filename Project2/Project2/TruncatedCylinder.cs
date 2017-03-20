using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    class TruncatedCylinder : Shape
    {
        public TruncatedCylinder(Point point, Vector direction, double radius, double height)
        {
            Point = point;
            Direction = direction.UnitVector();
            Radius = radius;
            Height = height;
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(Point, point);
            double length = vector.Length();
            double angle = Math.Acos(Function.DotProduct(vector, Direction) / (vector.Length() * Direction.Length()));
            double distance = Math.Sin(angle) * length;
            return distance < Radius;
        }
        public Color Color(Point point)
        {
            Vector vector = point - Point;
            if (Function.DotProduct(vector, Direction) == 0)
                return new Project2.Color(1, 0, 0);

            //Plane plane = new Plane(Point, Direction);
            //Point planeIntersection = plane.Intersection(point, ray);
            //if (planeIntersection != null)
            //{
            //    if ((planeIntersection - Point).Length() < Radius)
            //        return planeIntersection;
            //}

            //Point topCenter = Point + Direction * Height;
            //plane = new Plane(topCenter, Direction);
            //planeIntersection = plane.Intersection(point, ray);
            //if (planeIntersection != null)
            //{
            //    if ((planeIntersection - topCenter).Length() < Radius)
            //        return planeIntersection;
            //}

            return _color;
        }

        public void SetColor(Color color)
        {
            _color = color;
        }
        public PointColor Intersection(Point point, Vector ray)
        {
            List<PointColor> pointColors = new List<PointColor>();

            Plane plane = new Plane(Point, Direction);
            PointColor pointColor = plane.Intersection(point, ray);
            if (pointColor != null)
            {
                Point planeIntersection = pointColor.Point;
                if (planeIntersection != null)
                {
                    if ((planeIntersection - Point).Length() < Radius)
                        pointColors.Add(new PointColor
                        {
                            Point = planeIntersection,
                            Color = _bottomColor
                        });
                }
            }

            Point topCenter = Point + Direction * Height;
            plane = new Plane(topCenter, Direction);
            pointColor = plane.Intersection(point, ray);
            if (pointColor != null)
            {
                Point planeIntersection2 = pointColor.Point;
                if (planeIntersection2 != null)
                {
                    if ((planeIntersection2 - topCenter).Length() < Radius)
                    {
                        Color color = _topColor;
                        if ((planeIntersection2 - topCenter).Length() > Radius * .8)
                            color = _color;

                        pointColors.Add(new PointColor
                        {
                            Point = planeIntersection2,
                            Color = color
                        });
                    }
                }
            }

            
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
                Vector intersectionDirection = intersectionPoint - Point;

                //if (Function.CrossProduct(intersectionDirection, Direction).Length() > Radius)
                //    addIntersectionPoint = false;



                Vector unitIntersectionDirection = intersectionDirection.UnitVector();
                if (Function.DotProduct(Direction, unitIntersectionDirection) < 0)
                    addIntersectionPoint = false;

                double value = Function.DotProduct(intersectionDirection, Direction);
                if (value > Height)
                    addIntersectionPoint = false;

                if (addIntersectionPoint)
                    pointColors.Add(new PointColor
                    {
                        Point = intersectionPoint,
                        Color = _color
                    });
            }

            double min = 1000000;
            int index = -1;
            for (int i = 0; i < pointColors.Count; i++)
            {
                double value = (pointColors[i].Point - point).Length();
                if(pointColors.Count > 1)
                    value = (pointColors[i].Point - point).Length();

                if ( value < min)// & pointColors.Count == 2)
                {
                    min = (pointColors[i].Point - point).Length();
                    index = i;
                }
            }

            if (index >= 0)
            {
                //if ((pointColors[index].Point - Point).Length() < 30)
                //    pointColors[index].Color = new Color(0, 1, 0);
                return pointColors[index];
            }            

            return null;
        }


        public Point Point { get; set; }
        public Vector Direction { get; set; }
        public double Radius { get; set; }
        public double Height { get; set; }

        public Color _color = new Color(.1, .1, .1);
        private static Color _topColor = new Color(1, 1, 1);
        private static Color _bottomColor = new Color(1, 1, 1);
    }
}
