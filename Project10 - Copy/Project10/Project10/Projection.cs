using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class Projection : Light
    {
        public Projection(Point point, Vector direction, Vector upVector, ImageData imageData, double angle, double distance)
        {
            Point = point;
            Direction = direction.UnitVector();
            ImageData = imageData;
            Angle = angle;
            Distance = distance;

            PerpendicularVector = Function.CrossProduct(Direction, upVector).UnitVector();
            UpVector = Function.CrossProduct(PerpendicularVector, Direction).UnitVector();
        }

        public Color ShineOnShape(ReturnData data, double alpha, List<Shape> shapes)
        {
            //if (data.ColorMatrix.IgnoreSpotlightIntensity)
            //    return Color.None;

            Point shapePoint = data.Point;
            Vector vectorToLight = (Point - shapePoint).UnitVector();
            Vector vectorToShape = -1 * vectorToLight;

            double shadow = Function.IntersectionShadow(data, vectorToLight, shapes);// Math.Min(shadowFromAngle, shadowFromIntersection);
            if (shadow != 1)
                return Color.None;
            double angleFromSpotlight = Math.Acos(Function.DotProduct(vectorToShape, Direction));
            if (angleFromSpotlight > Angle)
                return Color.None;
            double angleFromNormal = Math.Acos(Function.DotProduct(vectorToLight, data.NormalVector));
            if (angleFromNormal > Function.Degrees(90))
                return Color.None;

            double hypotenous = Distance / Math.Cos(angleFromSpotlight);
            Vector vectorToImage = vectorToShape * hypotenous;
            double imageMax = Distance * Math.Tan(Angle);

            Vector projectionY = Function.DotProduct(UpVector, vectorToImage) * UpVector;
            int yPixel = 0;
            if (Function.DotProduct(projectionY, UpVector) < 0)
                yPixel = (int)((ImageData.Height / 2) * (1 + projectionY.Length() / imageMax));
            else
                yPixel = (int)((ImageData.Height / 2) * (1 - projectionY.Length() / imageMax));

            Vector projectionX = Function.DotProduct(PerpendicularVector, vectorToImage) * PerpendicularVector;
            int xPixel = 0;
            if (Function.DotProduct(projectionX, PerpendicularVector) < 0)
                xPixel = (int)((ImageData.Width / 2) * (1 - projectionX.Length() / imageMax));
            else
                xPixel = (int)((ImageData.Width / 2) * (1 + projectionX.Length() / imageMax));


            Color color = ImageData.GetPixel(xPixel, yPixel);//.Flatten();// data.ColorMatrix * LightColor;
            //double intensity = Function.DotProduct(vectorToShape, Direction) * Function.DotProduct(vectorToLight, data.NormalVector);
            //if (angleFromSpotlight > Angle)
            //{
            //    //color.Red *= .1;
            //    intensity *= (Angle + Buffer - angleFromSpotlight) / (Buffer);
            //    //if (angleFromSpotlight > Angle + Buffer / 2)
            //    //    intensity *= (Angle + Buffer - angleFromSpotlight) / (Buffer);
            //}

            //color = color * intensity;
            //color.Omega *= intensity;
            return color;
        }

        public Point Point { get; set; }
        public Vector Direction { get; set; }
        public Vector UpVector { get; set; }
        public Vector PerpendicularVector { get; set; }
        public double Angle { get; set; }
        public double Distance { get; set; }
        public ImageData ImageData { get; set; }
    }
}
