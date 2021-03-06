﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7
{
    class DiffusedLight : Light
    {
        public DiffusedLight()
        {
            Point = null;
            LightColor = null;
        }

        public Color ShineOnShape(ReturnData data, double alpha, List<Shape> shapes)
        {
            //return data.ColorMatrix.Flatten();
            if (data.ColorMatrix.IgnoreDiffusedIntensity)
                return data.ColorMatrix.Flatten();

            Point shapePoint = data.Point;
            Vector vectorToLight = (Point - shapePoint).UnitVector();

            double cosTheta = Function.DotProduct(vectorToLight, data.NormalVector);
            double angle = Math.Acos(cosTheta);
            double intensity = (cosTheta + 1) / 2;
            
            double shadowFromAngle = Shadow(angle);
            double shadowFromIntersection = Function.IntersectionShadow(data, vectorToLight, shapes, _minAngle, _maxAngle);
            double shadow = Math.Min(shadowFromAngle, shadowFromIntersection);
            //shadow = 1;

            intensity *= (shadow + alpha) / (1 + alpha);
            if (intensity > _maxIntensity)
                _maxIntensity = intensity;

            Color color = data.ColorMatrix * LightColor * intensity;
            return color;
        }

        private double Shadow(double angle)
        {
            double shadow = 1;
            if (angle > _maxAngle)
                shadow = 0;
            else if (angle > _minAngle)
            {
                shadow = (1 - (angle - _minAngle) / (_maxAngle - _minAngle));
            }
            return shadow;
        }

        public Point Point { get; set; }
        public Color LightColor { get; set; }

        private double _minAngle = Function.Degrees(50);
        private double _maxAngle = Function.Degrees(95);
        static double _maxIntensity = -10000;
    }
}
