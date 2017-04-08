using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    class SolidTextureSphere : Shape
    {
        public SolidTextureSphere(Sphere sphere, Point planePoint, Vector planeUpVector, ImageData imageData)
        {
            Sphere = sphere;
            sphere.SetColor(Color.WhiteColor);
            Plane = new Plane(planePoint, (Sphere.Center - planePoint).UnitVector());
            PlaneUpVector = planeUpVector.UnitVector();
            ImageData = imageData;
        }

        public bool Inside(Point point)
        {
            return false;
        }
        public void SetColor(Color color)
        {

        }
        public Color GetColor(Point point)
        {
            return null;
        }
        public Color GetColorMatrix(Point2D point)
        {
            Color color = ImageData.GetPixel(point.X, point.Y);
            return color;
        }
        private Point2D GetXY(Point point)
        {
            Vector vector = (point - Plane.Point);
            Vector unitVector = vector.UnitVector();
            double cosTheta = Function.DotProduct(unitVector, PlaneUpVector);
            double theta = Math.Acos(cosTheta);
            Vector perpendicularVector = Function.CrossProduct(PlaneUpVector, Plane.NormalVector);
            double testTheta = Math.Acos(Function.DotProduct(unitVector, perpendicularVector));
            if (testTheta < Function.Degrees(90))
                theta = Function.Degrees(360) - theta;

            int pixelX = 0;
            int pixelY = 0;
            int imageWidth = ImageData.Width;
            int imageHeight = ImageData.Height;
            double distance = vector.Length() * imageWidth / (2 * Sphere.Radius);
            //double distance = vector.Length();
            if (theta < Function.Degrees(90))
            {
                pixelX = (int)((imageWidth / 2 - Math.Sin(theta) * distance) % imageWidth);
                pixelY = (int)((imageHeight / 2 - Math.Cos(theta) * distance) % imageHeight);
            }
            else if (theta < Function.Degrees(180))
            {
                pixelX = (int)((imageWidth / 2 - (Math.Sin(Function.Degrees(180) - theta) * distance) % imageWidth));
                pixelY = (int)((imageHeight / 2 + (Math.Cos(Function.Degrees(180) - theta) * distance) % imageHeight));
            }
            else if (theta < Function.Degrees(270))
            {
                pixelX = (int)((imageWidth / 2 + (Math.Sin(theta - Function.Degrees(180)) * distance) % imageWidth));
                pixelY = (int)((imageHeight / 2 + (Math.Cos(theta - Function.Degrees(180)) * distance) % imageHeight));
            }
            else if (theta < Function.Degrees(360))
            {
                pixelX = (int)((imageWidth / 2 + (Math.Sin(Function.Degrees(360) - theta) * distance) % imageWidth));
                pixelY = (int)((imageHeight / 2 - (Math.Cos(Function.Degrees(360) - theta) * distance) % imageHeight));
            }
            //if (pixelX < 0 | pixelY < 0 | pixelX > imageWidth | pixelY > imageHeight)
            //    return ColorMatrix;

            //return ColorMatrix.None;
            int pixelXOriginal = pixelX;
            int pixelYOriginal = pixelY;
            pixelX %= imageWidth;
            pixelY %= imageHeight;
            if (pixelX < 0)
                pixelX += imageWidth;
            if (pixelY < 0)
                pixelY += imageHeight;

            return new Point2D(pixelX, pixelY);
        }
        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData returnData = Sphere.Intersection(point, ray, false);
            Point2D imageXY = null;
            if (returnData != null)
            {
                Vector direction = Plane.NormalVector * -1;
                ReturnData planeIntersection = Plane.Intersection(returnData.Point, direction);
                imageXY = GetXY(planeIntersection.Point);
                returnData.Color = GetColorMatrix(imageXY);

                return returnData;
            }
            return null;
            //returnData = Plane.Intersection(point, ray);
            //if (returnData == null)
            //    return null;

            //imageXY = GetXY(returnData.Point);
            //returnData.ColorMatrix = GetColorMatrix(imageXY);
            //return returnData;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }
        
        public Sphere Sphere { get; set; }
        public Plane Plane { get; set; }
        public Vector PlaneUpVector { get; set; }
        public ImageData ImageData { get; set; }
    }
}
