using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Project5
{
    class Pokeball : Shape
    {
        public Pokeball(Point center, double radius, Vector upVector, Vector outVector, ImageData texture, ImageData normalMapTop, ImageData normalMapBottom, bool normalPokeball = false)
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

            Texture = texture;
            NormalMapTop = normalMapTop;
            NormalMapBottom = normalMapBottom;
            NormalPokeball = normalPokeball;

        }

        private Point2D GetXY(Point point)
        {
            Vector vector = point - Sphere.Center;
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

        private double Phi(Point point)
        {
            Vector vector = point - Sphere.Center;
            Vector unitVector = vector.UnitVector();

            double phiCos = Function.DotProduct(unitVector, UpVector);
            double phi = Math.Acos(phiCos);

            return phi;
        }

        public ColorMatrix GetColorMatrix(Point point)
        {
            double phi = Phi(point);
            if (phi < Function.Degrees(90) + _blackAngle & phi > Function.Degrees(90) - _blackAngle)
                return _darkGray;
            else if (phi > Function.Degrees(90))
                return _pokeballBottomColor;
            else if (NormalPokeball)
                return _pokeballTopColor;

            Point2D point2D = GetXY(point);

            ColorMatrix pixel = Texture.GetPixel(point2D.X, point2D.Y);
            return pixel;
        }

        public void Evaluate()
        {
            _totalRuns = _totalRuns;
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

        private Vector GetNormalVector(Point point, Vector normalVector)
        {
            //if (NormalMap == null | UseNormalMap == false)
            //    return NormalVector;

            double phi = Phi(point);

            Point2D point2D = GetXY(point);
            ColorMatrix pixel;
            if (phi < Function.Degrees(90))
                pixel = NormalMapTop.GetPixel(point2D.X, point2D.Y);
            else
                //return normalVector;
                pixel = NormalMapBottom.GetPixel(point2D.X, point2D.Y);
            Color color = pixel.Flatten();
            Vector addVector = new Vector(color.Red, color.Green, color.Blue);
            return (normalVector + addVector).UnitVector();
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            List<ReturnData> returnData = new List<ReturnData>();

            ReturnData sphereReturnData = Sphere.Intersection(point, ray, false);
            if (sphereReturnData != null)
            {
                Point sphereIntersection = sphereReturnData.Point;
                Point2D point2D = GetXY(sphereIntersection);
                returnData.Add(new ReturnData
                {
                    Point = sphereIntersection,
                    ColorMatrix = GetColorMatrix(sphereIntersection),
                    NormalVector = GetNormalVector(sphereIntersection, sphereReturnData.NormalVector),
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

            //ReturnData sphereOutline = Sphere.Outline(point, ray);
            //if (sphereOutline != null)
            //{
            //    return sphereOutline;
            //}

            return null;
        }

        public Sphere Sphere { get; set; }
        public TruncatedCylinder Dot;

        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        private static readonly ColorMatrix _pokeballTopColor = ColorMatrix.Red();
        private static readonly ColorMatrix _pokeballBottomColor = ColorMatrix.White();
        private static readonly ColorMatrix _darkGray = ColorMatrix.DarkGray();

        public bool NormalPokeball { get; set; }
        public ImageData Texture { get; set; }
        public ImageData NormalMapTop { get; set; }
        public ImageData NormalMapBottom { get; set; }
        private static int _totalRuns = 0;
        private static int _hitRuns = 0;
        private static int _missedRuns = 0;
        private static double _thetaMin = 10000;
        private static double _testThetaMin = 10000;
        private static double _thetaMax = -10000;
        private static double _testThetaMax = -10000;
        private static double _blackAngle = Function.Degrees(3);
    }
}
