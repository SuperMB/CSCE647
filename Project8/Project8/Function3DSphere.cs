using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    class Function3DSphere : Shape
    {
        public Function3DSphere(Sphere sphere)
        {
            Sphere = sphere;
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
            double value = 20;
            return new Color(Math.Abs((point.X % value) / value), Math.Abs((point.Y % value) / value), Math.Abs((point.Z % value)) / value);
        }
        
        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData returnData = Sphere.Intersection(point, ray, false);
            if (returnData != null)
            {
                returnData.Color = GetColor(returnData.Point);
                return returnData;
            }
            return null;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public Sphere Sphere { get; set; }
    }
}
