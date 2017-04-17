using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class SpecularHighlight : Light
    {
        public SpecularHighlight()
        {
            Point = null;
            LightColor = null;
            Minimum = 0;
            Maximum = 0;
        }

        public Color ShineOnShape(ReturnData data, double alpha, List<Shape> shapes)
        {
            //if (data.ColorMatrix.IgnoreSpotlightIntensity)
            //    return Color.None;

            //alpha /= 2;

            Point shapePoint = data.Point;
            Vector vectorToLight = (Point - shapePoint).UnitVector();
            Vector normalVector = data.NormalVector;
            Vector reversedLightVector = 2 * Function.DotProduct(normalVector, vectorToLight) * normalVector - vectorToLight;
            Vector vectorToEye = (EyePoint - shapePoint).UnitVector();
            double cosPhi = Function.DotProduct(reversedLightVector, vectorToEye);

            double intensity = 0;
            if (cosPhi < Minimum)
            {
                //return None;
                double shadow = Function.IntersectionShadow(data, vectorToLight, shapes, _minAngle, _maxAngle);// Math.Min(shadowFromAngle, shadowFromIntersection);
                if (shadow == 1)
                    return None;

                //return new Color(0, 1, 1);

                intensity = (shadow + alpha) / (1 + alpha);

                Color lightColor = LightColor.Clone();
                //lightColor.Red = 0;
                double omega = lightColor.Omega;
                lightColor /= omega;
                lightColor.Omega /= omega;
                return data.Color * lightColor * intensity;
            }
            else if (cosPhi > Maximum)
                return LightColor;
            
            double value = (1 / LightColor.Omega) + ((Math.Acos(Minimum) - Math.Acos(cosPhi)) / (Math.Acos(Minimum) - Math.Acos(Maximum))) * ((LightColor.Omega - 1) / LightColor.Omega);
            intensity = value;

            Color color = LightColor * intensity;
            color.Omega = color.Omega * intensity;

            return color;
        }
        
        public Point Point { get; set; }
        public Point EyePoint { get; set; }
        public Color LightColor { get; set; }

        private static readonly Color None = new Color(0, 0, 0, 0);

        public double Minimum;// = Math.Cos(Math.PI / (180 / 45));
        public double Maximum;// = Math.Cos(Math.PI / (180 / 10));
        private static readonly double fraction = .8;


        private double _minAngle = Function.Degrees(65);
        private double _maxAngle = Function.Degrees(95);
        static double _minAngleChecked = 10000;
        static double _maxAngleChecked = -10000;
        static int _lessThan0 = 0;
    }
}
