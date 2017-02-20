using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3
{
    class DiffusedLight : Light
    {
        public DiffusedLight()
        {
            Point = null;
            LightColor = null;
        }

        public Color ShineOnShape(ReturnData data)
        {
            Point shapePoint = data.Point;
            Vector vectorToLight = (Point - shapePoint).UnitVector();
            double intensity = (Function.DotProduct(vectorToLight, data.NormalVector) + 1) / 2;

            Color color = data.ColorMatrix * LightColor;

            if (data.ColorMatrix.IgnoreDiffusedIntensity)
                return color;

            return color * intensity;
        }

        public Point Point { get; set; }
        public Color LightColor { get; set; }
    }
}
