using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    class Plane : Shape
    {
        public Plane(Point point, Vector normalVector, Vector upVector, ImageData wallpaper, ImageData normalMap)
        {
            Point = point;
            NormalVector = normalVector.UnitVector();
            Vector perpendicularVector = Function.CrossProduct(NormalVector, upVector);
            UpVector = Function.CrossProduct(perpendicularVector, NormalVector).UnitVector();

            Color = Color.WhiteColor;
            Wallpaper = wallpaper;
            NormalMap = normalMap;
            UseNormalMap = true;
        }
        public Plane(Point point, Vector normalVector)
        {
            Point = point;
            NormalVector = normalVector.UnitVector();
            UpVector = null;

            Color = Color.WhiteColor;
            Wallpaper = null;
            NormalMap = null;
            UseNormalMap = false;
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(Point, point);
            return Function.DotProduct(vector, NormalVector) < 0;
        }
        private Point2D GetXY(Point point)
        {
            if (NormalMap == null)
                return null;

            Vector vector = (point - Point);
            Vector unitVector = vector.UnitVector();
            double cosTheta = Function.DotProduct(unitVector, UpVector);
            double theta = Math.Acos(cosTheta);
            Vector perpendicularVector = Function.CrossProduct(UpVector, NormalVector);
            double testTheta = Math.Acos(Function.DotProduct(unitVector, perpendicularVector));
            if (testTheta < Function.Degrees(90))
                theta = Function.Degrees(360) - theta;

            int pixelX = 0;
            int pixelY = 0;
            double distance = vector.Length();
            int imageWidth = Wallpaper.Width;
            int imageHeight = Wallpaper.Height;
            if (theta < Function.Degrees(90))
            {
                pixelX = (int)((imageWidth / 2 - Math.Sin(theta) * distance) % imageWidth);
                pixelY = (int)((imageHeight / 2 - Math.Cos(theta) * distance) % imageHeight);
                //return ColorMatrix.Red();
            }
            else if (theta < Function.Degrees(180))
            {
                pixelX = (int)((imageWidth / 2 - (Math.Sin(Function.Degrees(180) - theta) * distance) % imageWidth));
                pixelY = (int)((imageHeight / 2 + (Math.Cos(Function.Degrees(180) - theta) * distance) % imageHeight));
                //return ColorMatrix.White();
            }
            else if (theta < Function.Degrees(270))
            {
                pixelX = (int)((imageWidth / 2 + (Math.Sin(theta - Function.Degrees(180)) * distance) % imageWidth));
                pixelY = (int)((imageHeight / 2 + (Math.Cos(theta - Function.Degrees(180)) * distance) % imageHeight));
                //return ColorMatrix.Green();
            }
            else if (theta < Function.Degrees(360))
            {
                pixelX = (int)((imageWidth / 2 + (Math.Sin(Function.Degrees(360) - theta) * distance) % imageWidth));
                pixelY = (int)((imageHeight / 2 - (Math.Cos(Function.Degrees(360) - theta) * distance) % imageHeight));
                //return ColorMatrix.Blue();
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
        public Color GetColor(Point point)
        {
            return Color;
        }
        public Color GetColorMatrix(Point2D point)
        {
            if (Wallpaper == null)
                return Color;

            Color pixel = Wallpaper.GetPixel(point.X, point.Y);
            return pixel;
        }
        public void ToggleNormalMap()
        {
            UseNormalMap = !UseNormalMap;
        }
        private Vector GetNormalVector(Point2D point)
        {
            if (NormalMap == null | UseNormalMap == false)
                return NormalVector;

            Color color = NormalMap.GetPixel(point.X, point.Y);
            Vector addVector = new Vector(color.Red, color.Green, color.Blue);
            return (NormalVector + addVector).UnitVector();
        }
        public void SetColor(Color color)
        {
            Color = color;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }
        public ReturnData Intersection(Point point, Vector ray)
        {
            double denominator = Function.DotProduct(NormalVector, ray);
            if (denominator == 0)
                return null;

            double rayScaling = Function.DotProduct(NormalVector, point - Point) / denominator;
            if (rayScaling > 0)
                return null;

            Point result = point + ray * (-1 * rayScaling);
            Point2D point2D = GetXY(result);
            return new ReturnData
            {
                Point = result,
                Color = GetColorMatrix(point2D),
                NormalVector = GetNormalVector(point2D),
                AngleDirection = AngleDirection.AngleIncreasing,
                NonIntersectingShapes = new List<Shape> { this }
            };
        }

        public Point Point { get; set; }
        public Vector NormalVector { get; set; }
        public Vector UpVector { get; set; }
        public Color Color { get; set; } 
        public ImageData Wallpaper { get; set; }
        public ImageData NormalMap { get; set; }
        private bool UseNormalMap { get; set; }
    }
}
