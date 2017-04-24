using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project11
{
    class IrridescentReflectivePokeball : Shape
    {
        public IrridescentReflectivePokeball(Point center, double radius, Vector upVector, Vector outVector, ImageData texture, ImageData normalMapTop, ImageData normalMapBottom, List<Shape> shapes)
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
            Shapes = shapes;

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

        public double Phi(Point point)
        {
            Vector vector = point - Sphere.Center;
            Vector unitVector = vector.UnitVector();

            double phiCos = Function.DotProduct(unitVector, UpVector);
            double phi = Math.Acos(phiCos);

            return phi;
        }

        public Color GetColor(Point point)
        {
            return null;
        }

        public Color GetColor(Point point, Vector normalVector, Vector ray)
        {
            double phi = Phi(point);
            if (phi < Function.Degrees(90) + BlackAngle & phi > Function.Degrees(90) - BlackAngle)
                return _darkGray;
            else if (phi > Function.Degrees(90))
                return _pokeballBottomColor;

            return Reflection;
        }

        public void SetColor(Color color)
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

        public Color GetIrridescent(Vector normalVector, Vector ray)
        {
            normalVector = normalVector.UnitVector();
            ray = ray.UnitVector() * -1;
            double angle = Math.Acos(Function.DotProduct(normalVector, ray));
            if (angle > Function.Degrees(90))
                angle -= Function.Degrees(90);

            double red = 0;
            double green = 0;
            double blue = 0;
            if (angle < Function.Degrees(30))
            {
                red = angle / Function.Degrees(30);
                green = .5 - (angle) / (2 * Function.Degrees(30));
                blue = 1 - (angle) / (2 * Function.Degrees(30));
            }
            else if (angle < Function.Degrees(60))
            {
                red = 1 - (angle - Function.Degrees(30)) / (2 * Function.Degrees(30));
                green = (angle - Function.Degrees(30)) / Function.Degrees(30);
                blue = .5 - (angle - Function.Degrees(30)) / (2 * Function.Degrees(30));
            }
            else if (angle < Function.Degrees(90))
            {
                red = .5 - (angle - Function.Degrees(60)) / (2 * Function.Degrees(30));
                green = 1 - (angle - Function.Degrees(60)) / (2 * Function.Degrees(30));
                blue = (angle - Function.Degrees(60)) / Function.Degrees(30);
            }
            else
            {
                return Color.RedColor;
            }

            double max = Math.Max(red, Math.Max(green, blue)) * 3;
            return new Color(red / max, green / max, blue / max, .33);
        }

        private Vector GetNormalVector(Point point, Vector normalVector)
        {
            //if (NormalMap == null | UseNormalMap == false)
            //    return NormalVector;
            double phi = Phi(point);

            Point2D point2D = GetXY(point);
            Color color;
            if (phi < Function.Degrees(90))
                color = NormalMapTop.GetPixel(point2D.X, point2D.Y);
            else
                //return normalVector;
                color = NormalMapBottom.GetPixel(point2D.X, point2D.Y);
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
                Vector normalVector = GetNormalVector(sphereIntersection, sphereReturnData.NormalVector);
                if (Phi(sphereIntersection) > Function.Degrees(90) - BlackAngle)
                    returnData.Add(new ReturnData
                    {
                        Point = sphereIntersection,
                        Color = GetColor(sphereIntersection, normalVector, ray),
                        NormalVector = normalVector,
                        NonIntersectingShapes = new List<Shape> { Sphere }
                    });
                else
                {
                    ReturnData reflectReturnData = Function.Reflect(sphereIntersection, ray, normalVector, Shapes, new List<Shape> { this, Sphere });
                    if (reflectReturnData != null)
                    {
                        double factor = Math.Pow((sphereIntersection - reflectReturnData.Point).Length(), 1 / 3);
                        Color color = reflectReturnData.Color / factor;
                        color.Omega /= factor;
                        color += GetIrridescent(reflectReturnData.NormalVector, ray);
                        returnData.Add(new ReturnData
                        {
                            Point = sphereIntersection,
                            LightPoint = reflectReturnData.Point,
                            Color = color,
                            NormalVector = reflectReturnData.NormalVector,
                            NonIntersectingShapes = new List<Shape> { Sphere }
                        });
                    }
                    else
                        returnData.Add(new ReturnData
                        {
                            Point = sphereIntersection,
                            Color = GetColor(sphereIntersection, normalVector, ray),
                            NormalVector = normalVector,
                            NonIntersectingShapes = new List<Shape> { Sphere }
                        });
                }
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

            return null;
        }

        public Sphere Sphere { get; set; }
        public TruncatedCylinder Dot;

        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        private static readonly Color _pokeballTopColor = Color.RedColor;
        private static readonly Color _pokeballBottomColor = Color.WhiteColor;
        private static readonly Color _darkGray = Color.DarkGrayColor;

        public List<Shape> Shapes { get; set; }
        public ImageData Texture { get; set; }
        public ImageData NormalMapTop { get; set; }
        public ImageData NormalMapBottom { get; set; }
        private static int _totalRuns = 0;
        public static double BlackAngle = Function.Degrees(3);
        public static Color Reflection = new Color(.3, 0, 0, .3);
    }
}
