using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    class TranslucentSquare : Shape
    {
        public TranslucentSquare(Point topLeft, Point bottomLeft, Point bottomRight, Point topRight, List<Shape> shapes)
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
            SetColor(TranslucentColor);
        }

        public Color GetColor(Point point)
        {
            return TranslucentColor;
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
            ReturnData returnData = Function.Translucent(.3, triangleReturnData.Point, ray, Shapes, new List<Shape> { this });
            if (returnData != null)
            {
                returnData.Color += TranslucentColor;
                return returnData;

                double factor = 1;// Math.Pow((triangleReturnData.Point - returnData.Point).Length(), 1 / 3);
                Color color = returnData.Color / factor;
                color.Omega /= factor;
                triangleReturnData.Color = Function.Illuminate(returnData);
                triangleReturnData.LightPoint = returnData.Point;
                triangleReturnData.NormalVector = returnData.NormalVector;
                triangleReturnData.IgnoreShadow = true;
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
        public Color TranslucentColor = new Color(3, 3, 3, 3);
    }
}
