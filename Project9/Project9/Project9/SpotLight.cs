using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    class SpotLight : Light
    {
        public SpotLight()
        {
            Point = null;
            Direction = null;
            LightColor = null;
            Angle = 0;
            Buffer = 0;
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
            {
                double shadowIntensity = (shadow + alpha) / (1 + alpha);

                Color lightColor = LightColor.Clone();
                //lightColor.Red = 0;
                double omega = lightColor.Omega;
                lightColor /= omega;
                lightColor.Omega /= omega;
                return data.Color * new Color(1,1,1) * shadowIntensity;
            }

            bool notInSpotlight = false;
            double angleFromSpotlight = Math.Acos(Function.DotProduct(vectorToShape, Direction));
            if (angleFromSpotlight > Angle + Buffer)
                //notInSpotlight = true;
                return Color.None;

            double angleFromNormal = Math.Acos(Function.DotProduct(vectorToLight, data.NormalVector));
            if(angleFromNormal > Function.Degrees(90))
                //notInSpotlight = true;
                return Color.None;

            Color color = data.Color * LightColor;
            double intensity = Function.DotProduct(vectorToShape, Direction) * Function.DotProduct(vectorToLight, data.NormalVector);
            if (angleFromSpotlight > Angle)
            {
                //color.Red *= .1;
                intensity *= (Angle + Buffer - angleFromSpotlight) / (Buffer);
                //if (angleFromSpotlight > Angle + Buffer / 2)
                //    intensity *= (Angle + Buffer - angleFromSpotlight) / (Buffer);
            }

            color = color * intensity;
            color.Omega *= intensity;
            return color;
        }

        public Point Point { get; set; }
        public Vector Direction { get; set; }
        public Color LightColor { get; set; }
        public double Angle { get; set; }
        public double Buffer { get; set; }
    }
}
