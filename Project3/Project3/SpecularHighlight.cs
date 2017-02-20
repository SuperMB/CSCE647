using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3
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

        public Color ShineOnShape(ReturnData data)
        {
            if (data.ColorMatrix.IgnoreSpotlightIntensity)
                return Color.None;

            Point shapePoint = data.Point;
            Vector vectorToLight = (Point - shapePoint).UnitVector();
            Vector normalVector = data.NormalVector;
            Vector reversedLightVector = 2 * Function.DotProduct(normalVector, vectorToLight) * normalVector - vectorToLight;
            Vector vectorToEye = (EyePoint - shapePoint).UnitVector();
            double cosPhi = Function.DotProduct(reversedLightVector, vectorToEye);

            if (cosPhi < Minimum)
                return None;
            else if (cosPhi > Maximum)
                return LightColor;
            
            double value = ((Math.Acos(Minimum) - Math.Acos(cosPhi)) / (Math.Acos(Minimum) - Math.Acos(Maximum)));
            double intensity = value;
            Color color = LightColor * intensity;
            //color.Red *= .1;
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
    }
}
