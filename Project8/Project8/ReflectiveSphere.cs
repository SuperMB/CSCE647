using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    class ReflectiveSphere : Shape
    {
        public ReflectiveSphere(Point center, double radius, List<Shape> shapes)
        {
            Sphere = new Sphere(
                center,
                radius
                );
            Sphere.Color = Color.WhiteColor;
            Shapes = shapes;
        }

        public bool Inside(Point point)
        {
            return Sphere.Inside(point);
        }

        public Color GetColor(Point point)
        {
            return Sphere.Color;
        }
        public void SetColor(Color color)
        {
            Sphere.Color = color;
        }

        public ReturnData Outline(Point point, Vector ray)
        {
            return Sphere.Outline(point, ray);
        }
        
        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData sphereReturnData = Sphere.Intersection(point, ray);
            if (sphereReturnData == null)
                return null;

            ReturnData returnData = Function.Reflect(sphereReturnData.Point, ray, sphereReturnData.NormalVector, Shapes, this);
            if (returnData != null)
            {
                sphereReturnData.NonIntersectingShapes = new List<Shape> { this };
                double factor = Math.Pow((sphereReturnData.Point - returnData.Point).Length(), 1/3);
                Color color = returnData.Color / factor;
                color.Omega /= factor;
                sphereReturnData.Color += color; // Function.Illuminate(returnData);
                return sphereReturnData;
            }
            else if(sphereReturnData != null)
            {
                return sphereReturnData;
            }
           

            return null;
        }

        public Sphere Sphere{ get; set; }
        public List<Shape> Shapes { get; set; }
        public Color Reflection = new Color(0, 1, 0);
    }
}
