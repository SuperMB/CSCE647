using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class ReflectiveSquare : Shape
    {
        public ReflectiveSquare(Point topLeft, Point bottomLeft, Point bottomRight, Point topRight, List<Shape> shapes)
        {
            Triangles = new List<Triangle>();
            Triangles.Add(new Triangle(
                topLeft,
                bottomLeft,
                bottomRight
                ));
            Triangles.Add(new Triangle(
                bottomRight,
                topRight,
                topLeft
                ));
            Shapes = shapes;
            SetColor(Reflection);
        }
        
        public Color GetColor(Point point)
        {
            return Reflection;
        }
        public void SetColor(Color color)
        {
            foreach (Triangle triangle in Triangles)
                triangle.SetColor(color);
        }

        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public ReturnData Intersection(Point point, Vector ray)
        {
            ReturnData triangleReturnData = null;
            foreach (Triangle triangle in Triangles)
            {
                ReturnData testReturnData = triangle.Intersection(point, ray);
                if (testReturnData != null)
                    triangleReturnData = testReturnData;
            }
            if (triangleReturnData == null)
                return null;
            //return triangleReturnData;

            triangleReturnData.NonIntersectingShapes = new List<Shape> { this };
            ReturnData returnData = Function.Reflect(triangleReturnData.Point, ray, triangleReturnData.NormalVector, Shapes, this);
            if (returnData != null)
            {
                double factor = Math.Pow((triangleReturnData.Point - returnData.Point).Length(), 1 / 3);
                Color color = returnData.Color / factor;
                color.Omega /= factor;
                triangleReturnData.Color += color; // Function.Illuminate(returnData);
                triangleReturnData.LightPoint = returnData.Point;
                triangleReturnData.NormalVector = returnData.NormalVector;
                return triangleReturnData;
            }
            else if (triangleReturnData != null)
            {
                return triangleReturnData;
            }


            return null;
        }

        public List<Triangle> Triangles { get; set; }
        public List<Shape> Shapes { get; set; }
        public Color Reflection = new Color(.7, .7, .7);
    }
}
