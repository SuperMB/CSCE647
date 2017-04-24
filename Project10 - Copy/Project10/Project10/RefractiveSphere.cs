using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class RefractiveSphere : Shape
    {
        public RefractiveSphere(Point center, double radius, Vector upVector, Vector outVector, ImageData normalMapTop, ImageData normalMapBottom, ImageData refactiveIndexData, List<Shape> shapes)
        {
            Sphere = new Sphere(
                center,
                radius
                );
            Sphere.Color = Color.WhiteColor;
            Shapes = shapes;
            UpVector = upVector;
            OutVector = outVector;
            NormalMapTop = normalMapTop;
            NormalMapBottom = normalMapBottom;
            RefractiveIndexData = refactiveIndexData;

            if (NormalMapTop != null)
            {
                ImageHeight = NormalMapTop.Height;
                ImageWidth = NormalMapTop.Width;
            }
            else if (NormalMapBottom != null)
            {
                ImageHeight = NormalMapBottom.Height;
                ImageWidth = NormalMapBottom.Width;
            }
            else if (RefractiveIndexData != null)
            {
                ImageHeight = RefractiveIndexData.Height;
                ImageWidth = RefractiveIndexData.Width;
            }
            else
            {
                ImageHeight = 0;
                ImageWidth = 0;
            }
        }

        public bool Inside(Point point)
        {
            return Sphere.Inside(point);
        }

        public Color GetColor(Point point)
        {
            return Sphere.Color;
        }
        public void SetColor(Color color)
        {
            Sphere.Color = color;
        }

        public ReturnData Outline(Point point, Vector ray)
        {
            return Sphere.Outline(point, ray);
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
            int imageWidth = ImageWidth;
            int imageHeight = ImageHeight;

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

        private double GetRefractiveIndex(Point point)
        {            
            Point2D point2D = GetXY(point);
            Color color = RefractiveIndexData.GetPixel(point2D.X, point2D.Y);
            return (color.Red + color.Blue + color.Green) / 3;
        }


        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData sphereReturnData = Sphere.Intersection(point, ray);
            if (sphereReturnData == null)
                return null;


            double refractiveIndex = .8;
            if(RefractiveIndexData != null)
                refractiveIndex = GetRefractiveIndex(sphereReturnData.Point);

            Vector originalNormal = sphereReturnData.NormalVector;
            ReturnData returnData;
            if (NormalMapBottom == null || NormalMapTop == null)
                returnData = Function.Refract(refractiveIndex, sphereReturnData.Point, ray, sphereReturnData.NormalVector, Shapes, new List<Shape> { this });
            else
                returnData = Function.RefractNormalMap(refractiveIndex, sphereReturnData.Point, ray, originalNormal, GetNormalVector(sphereReturnData.Point, sphereReturnData.NormalVector), Shapes, new List<Shape> { this });
            if (returnData != null)
            {
                return returnData;

                sphereReturnData.NonIntersectingShapes = new List<Shape> { this };
                double factor = Math.Pow((sphereReturnData.Point - returnData.Point).Length(), 1/3);
                Color color = returnData.Color / factor;
                color.Omega /= factor;
                sphereReturnData.Color += color; // Function.Illuminate(returnData);
                return sphereReturnData;
            }
            else if(sphereReturnData != null)
            {
                return null; 
                sphereReturnData.Color = Color.RedColor;
                return sphereReturnData;
            }
           

            return null;
        }

        public Sphere Sphere{ get; set; }
        public List<Shape> Shapes { get; set; }
        public Color Reflection = new Color(0, 1, 0);
        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        public ImageData NormalMapTop { get; set; }
        public ImageData NormalMapBottom { get; set; }
        public ImageData RefractiveIndexData { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
    }
}
