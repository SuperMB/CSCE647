using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6
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
        public void SetColorMatrix(ColorMatrix colorMatrix)
        {

        }
        public ColorMatrix GetColorMatrix(Point point)
        {
            return new ColorMatrix(point.X % 255, point.Y % 255, point.Z % 255);
        }
        
        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData returnData = Sphere.Intersection(point, ray, false);
            if (returnData != null)
            {
                returnData.ColorMatrix = GetColorMatrix(returnData.Point);
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
