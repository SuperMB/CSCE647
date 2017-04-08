using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    class EnvironmentSphere : Shape
    {
        public EnvironmentSphere()
        {
            Center = null;
            Radius = 0;
            Color = null;
        }
        public EnvironmentSphere(Point center, double radius, Vector upVector, Vector outVector, ImageData texture)
        {
            Center = center;
            Radius = radius;
            Color = Color.BlueColor;
            Color.IgnoreEffects = true;

            OutVector = outVector.UnitVector();
            PerpendicularVector = Function.CrossProduct(OutVector, upVector);
            UpVector = Function.CrossProduct(PerpendicularVector, OutVector).UnitVector();
            Texture = texture;
            for (int i = 0; i < texture.Width; i++)
                for (int j = 0; j < texture.Height; j++)
                    Texture.TextureData[i][j].IgnoreEffects = true;

        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(point, Center);
            return vector.Length() - Radius < 0;
        }

        private Point2D GetXY(Point point)
        {
            Vector vector = point - Center;
            Vector unitVector = vector.UnitVector();

            double phiCos = Function.DotProduct(unitVector, UpVector);
            double phi = Math.Acos(phiCos);
            bool addUpVector = false;
            if (phi > Function.Degrees(90))
            {
                phi = Function.Degrees(180) - phi;
                phiCos = Math.Cos(phi);
                addUpVector = true;
            }

            Vector oppositeOutVector = OutVector * -1;
            int imageWidth = Texture.Width;
            int imageHeight = Texture.Height;

            Vector upProjection = phiCos * vector.Length() * UpVector;
            Vector equatorVector;
            if (addUpVector)
                equatorVector = vector + upProjection;
            else
                equatorVector = vector - upProjection;

            Vector equatorUnitVector = equatorVector.UnitVector();
            double theta = Math.Acos(Function.DotProduct(oppositeOutVector, equatorUnitVector));

            Vector perpendicularVector = Function.CrossProduct(oppositeOutVector, UpVector).UnitVector();
            double testTheta = Math.Acos(Function.DotProduct(perpendicularVector, equatorUnitVector));

            if (testTheta < Function.Degrees(90))
            {
                theta = 2 * Math.PI - theta;
            }

            int pixelX = 0;
            int pixelY = 0;
            double distance = phi / Function.Degrees(90);
            if (theta < Function.Degrees(90))
            {
                pixelX = (int)((imageWidth / 2) * (1 - Math.Sin(theta) * distance));
                pixelY = (int)((imageWidth / 2) * (1 - Math.Cos(theta) * distance));
            }
            else if (theta < Function.Degrees(180))
            {
                pixelX = (int)((imageWidth / 2) * (1 - Math.Sin(Function.Degrees(180) - theta) * distance));
                pixelY = (int)((imageWidth / 2) * (1 + Math.Cos(Function.Degrees(180) - theta) * distance));
            }
            else if (theta < Function.Degrees(270))
            {
                pixelX = (int)((imageWidth / 2) * (1 + Math.Sin(theta - Function.Degrees(180)) * distance));
                pixelY = (int)((imageWidth / 2) * (1 + Math.Cos(theta - Function.Degrees(180)) * distance));
            }
            else if (theta < Function.Degrees(360))
            {
                pixelX = (int)((imageWidth / 2) * (1 + Math.Sin(Function.Degrees(360) - theta) * distance));
                pixelY = (int)((imageWidth / 2) * (1 - Math.Cos(Function.Degrees(360) - theta) * distance));
            }

            return new Point2D(pixelX, pixelY);
        }

        public Color GetColor(Point point)
        {
            Point2D point2D = GetXY(point);

            Color color = Texture.GetPixel(point2D.X, point2D.Y);
            return color;
        }
        public void SetColor(Color color)
        {
            Color = color;
            Color.IgnoreEffects = true;
        }

        public ReturnData Outline(Point point, Vector ray)
        {
            Sphere sphere = new Sphere
            {
                Center = this.Center,
                Radius = this.Radius + 10,
                Color = Color.Outline
            };
            return sphere.Intersection(point, ray, false);
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            return Intersection(point, ray, true);
        }
        public ReturnData Intersection(Point point, Vector ray, bool tryOutlineIfFail)
        {
            //ray = ray.UnitVector();
            //double u = ray.X;
            //double v = ray.Y;
            //if (u < 1)
            //    u += 1;
            //if (v < 1)
            //    v += 1;

            //double u = Math.Acos(Math.Cos(ray.Z)) / Math.PI;
            //double v = Math.Acos(Math.Cos(ray.Y / Math.Sin(Math.PI * u))) / (2 * Math.PI);
            //if (ray.X > .5)
            //    v = 1 - v;

            //v += .5;
            //if (v > 1)
            //    v -= 1;

            Vector YZ = new Vector(0, ray.Y, ray.Z).UnitVector();
            double angleYZ = Math.Acos(Function.DotProduct(YZ, new Vector(0, 0, -1)));
            Vector XY = new Vector(ray.X, ray.Y, 0).UnitVector();
            double angleXY = Math.Acos(Function.DotProduct(XY, new Vector(-1, 0, 0)));

            double u = angleXY / Function.Degrees(180);
            double v = angleYZ / Function.Degrees(180);

            int uCoordinate = (int)(Texture.Width * u);
            int vCoordinate = (int)(Texture.Height * v);

            return new ReturnData
            {
                Point = new Point(100000, 1000000, 1000000),
                Color = Texture.GetPixel(uCoordinate, vCoordinate),
                NormalVector = -1 * ray,
                AngleDirection = AngleDirection.AngleIncreasing,
                NonIntersectingShapes = new List<Shape> { this }
            };

            //double b = Function.DotProduct(ray, point - Center);
            //double c = Function.DotProduct(Center - point, Center - point) - Math.Pow(Radius, 2);
            //double delta = Math.Pow(b, 2) - c;

            //if (b >= 0 & delta >= 0)
            //{
            //    double intersectDistance = b - Math.Sqrt(delta);
            //    Point intersectionPoint = point + ray * intersectDistance;
            //    Vector normalVector = (Center - intersectionPoint).UnitVector();
            //    return new ReturnData
            //    {
            //        Point = intersectionPoint,
            //        Color = GetColor(intersectionPoint),
            //        NormalVector = normalVector,
            //        AngleDirection = AngleDirection.AngleIncreasing,
            //        NonIntersectingShapes = new List<Shape> { this }
            //    };
            //}

            ////if (tryOutlineIfFail)
            ////    return Outline(point, ray);

            //return null;
        }

        public Point Center { get; set; }
        public double Radius { get; set; }
        public Color Color { get; set; }
        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        public Vector PerpendicularVector { get; set; }
        ImageData Texture { get; set; }

    }
}
