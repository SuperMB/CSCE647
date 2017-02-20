using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3
{
    class Plane : Shape
    {
        public Plane(Point point, Vector normalVector)
        {
            Point = point;
            NormalVector = normalVector.UnitVector();
            ColorMatrix = ColorMatrix.White();
            ColorMatrix.IgnoreSpecularIntensity = true;
            ColorMatrix.IgnoreSpotlightIntensity = true;
        }

        public bool Inside(Point point)
        {
            Vector vector = Function.GetVector(Point, point);
            return Function.DotProduct(vector, NormalVector) < 0;
        }
        public ColorMatrix GetColorMatrix(Point point)
        {
            return ColorMatrix;
        }
        public void SetColorMatrix(ColorMatrix colorMatrix)
        {
            ColorMatrix = colorMatrix;
            ColorMatrix.IgnoreSpecularIntensity = true;
            ColorMatrix.IgnoreSpotlightIntensity = true;
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
            return new ReturnData
            {
                Point = result,
                ColorMatrix = ColorMatrix,
                NormalVector = NormalVector 
            };
        }

        public Point Point { get; set; }
        public Vector NormalVector { get; set; }
        public ColorMatrix ColorMatrix { get; set; }
    }
}
