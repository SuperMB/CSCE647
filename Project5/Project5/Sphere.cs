using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    class Sphere : Shape
    {
        public Sphere()
        {
            Center = null;
            Radius = 0;
            ColorMatrix = null;
        }
        public Sphere(Point center, double radius)
        {
            Center = center;
            Radius = radius;
            ColorMatrix = ColorMatrix.Red();
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(point, Center);
            return vector.Length() - Radius < 0;
        }

        public ColorMatrix GetColorMatrix(Point point)
        {
            return ColorMatrix;
        }
        public void SetColorMatrix(ColorMatrix colorMatrix)
        {
            ColorMatrix = colorMatrix;
        }

        public ReturnData Outline(Point point, Vector ray)
        {
            Sphere sphere = new Sphere
            {
                Center = this.Center,
                Radius = this.Radius + 10,
                ColorMatrix = ColorMatrix.Highlight()
            };
            return sphere.Intersection(point, ray, false);
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            return Intersection(point, ray, true);
        }
        public ReturnData Intersection(Point point, Vector ray, bool tryOutlineIfFail)
        {
            double b = Function.DotProduct(ray, Center - point);
            double c = Function.DotProduct(Center - point, Center - point) - Math.Pow(Radius, 2);
            double delta = Math.Pow(b, 2) - c;

            if (b >= 0 & delta >= 0)
            {
                double intersectDistance = b - Math.Sqrt(delta);
                Point intersectionPoint = point + ray * intersectDistance;
                Vector normalVector = (intersectionPoint - Center).UnitVector();
                return new ReturnData
                {
                    Point = intersectionPoint,
                    ColorMatrix = ColorMatrix,
                    NormalVector = normalVector,
                    AngleDirection = AngleDirection.AngleIncreasing,
                    NonIntersectingShapes = new List<Shape> { this }
                };
            }

            if (tryOutlineIfFail)
                return Outline(point, ray);

            return null;
        }

        public Point Center { get; set; }
        public double Radius { get; set; }
        public ColorMatrix ColorMatrix { get; set; }
    }
}
