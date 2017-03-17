using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6
{
    class Sky : Shape
    {
        public Sky(Point point, Vector normalVector)
        {
            Plane = new Plane(point, normalVector,new Vector(0,1,0),null, null);
            MakeSky();
            ShowSky = true;
        }
        
        public void ToggleSky()
        {
            ShowSky = !ShowSky;
        }

        public void MakeSky()
        {
            Perlin = new PerlinNoiseTexture();
            PerlinColor = Perlin.FillRectangle(1000, 1000, 0, 0, new PerlinOptions());
        }
        public bool Inside(Point point)
        {
            return Plane.Inside(point);
        }

        private Point2D GetXY(Point point)
        {

            Point clonePoint = new Point(point.X, point.Y, point.Z);
            clonePoint.X /= 8;
            clonePoint.Y /= 8;

            Vector vector = (clonePoint - Plane.Point);
            Vector unitVector = vector.UnitVector();
            double cosTheta = Function.DotProduct(unitVector, Plane.UpVector);
            double theta = Math.Acos(cosTheta);
            Vector perpendicularVector = Function.CrossProduct(Plane.UpVector, Plane.NormalVector);
            double testTheta = Math.Acos(Function.DotProduct(unitVector, perpendicularVector));
            if (testTheta < Function.Degrees(90))
                theta = Function.Degrees(360) - theta;

            int pixelX = 0;
            int pixelY = 0;
            double distance = vector.Length();
            int imageWidth = 1000;
            int imageHeight = 1000;
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

        public ColorMatrix GetColorMatrix(Point point)
        {
            if (!ShowSky)
                return ColorMatrix.Blue();

            Point2D point2D = GetXY(point);
            Color color = PerlinColor[point2D.X][point2D.Y];

            ColorMatrix colorMatrix = new ColorMatrix(
                color.Red,
                color.Green,
                color.Blue,
                1
                );
            colorMatrix.IgnoreDiffusedIntensity = true;
            return colorMatrix;
        }
        public void SetColorMatrix(ColorMatrix colorMatrix)
        {

        }
        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData returnData = Plane.Intersection(point, ray);
            returnData.ColorMatrix = GetColorMatrix(returnData.Point);
            return returnData;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return Plane.Outline(point, ray);
        }

        public Plane Plane { get; set; }
        public PerlinNoiseTexture Perlin { get; set; }
        public Color[][] PerlinColor { get; set; }
        private static Color _blue = new Color(0, 0, 0);
        private static Color _white = new Color(1,1,1);

        private static double _min = 10000;
        private static double _max = -10000;
        public bool ShowSky { get; set; }
    }
}
