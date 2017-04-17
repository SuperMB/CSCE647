using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    class Cube : Shape
    {
        public Cube(Point center, Vector upVector, Vector outVector, double distance)
        {
            OutVector = outVector.UnitVector();
            PerpendicularVector = Function.CrossProduct(upVector.UnitVector(), OutVector).UnitVector();
            UpVector = Function.CrossProduct(OutVector, PerpendicularVector).UnitVector();

            Point point0 = center + distance * ((-1 * OutVector) + UpVector + (-1 * PerpendicularVector)).UnitVector();
            Point point1 = center + distance * (OutVector + UpVector + (-1 * PerpendicularVector)).UnitVector();
            Point point2 = center + distance * (OutVector + UpVector + PerpendicularVector).UnitVector();
            Point point3 = center + distance * ((-1 * OutVector) + UpVector + PerpendicularVector).UnitVector();
            Point point4 = center + distance * ((-1 * OutVector) + (-1 * UpVector) + (-1 * PerpendicularVector)).UnitVector();
            Point point5 = center + distance * (OutVector + (-1 * UpVector) + (-1 * PerpendicularVector)).UnitVector();
            Point point6 = center + distance * (OutVector + (-1 * UpVector) + PerpendicularVector).UnitVector();
            Point point7 = center + distance * (OutVector + UpVector + (-1 * PerpendicularVector)).UnitVector();

            Center = center;

            Triangles = new List<Triangle>();
            Triangles.Add(new Triangle(point0, point1, point3));
            Triangles.Add(new Triangle(point1, point2, point3));
            Triangles.Add(new Triangle(point0, point4, point1));
            //Triangles[Triangles.Count - 1].ColorMatrix = ColorMatrix.Red();
            Triangles.Add(new Triangle(point4, point5, point1));
            //Triangles[Triangles.Count - 1].ColorMatrix = ColorMatrix.Red();
            Triangles.Add(new Triangle(point5, point2, point1));
            Triangles.Add(new Triangle(point5, point6, point2));
            Triangles.Add(new Triangle(point6, point3, point2));
            Triangles.Add(new Triangle(point6, point7, point3));
            Triangles.Add(new Triangle(point5, point4, point7));
            Triangles.Add(new Triangle(point5, point7, point6));
            Triangles.Add(new Triangle(point4, point0, point3));
            Triangles.Add(new Triangle(point4, point3, point7));
        }
        public Color GetColor(Point point)
        {
            return Triangles[0].Color;
        }
        public void SetColor(Color color)
        {
            foreach (Triangle triangle in Triangles)
                triangle.Color = color;
        }
        public ReturnData Intersection(Point point, Vector ray)
        {
            List<ReturnData> returnDatas = new List<ReturnData>();
            foreach (Triangle triangle in Triangles)
            {
                ReturnData data = triangle.Intersection(point, ray);
                //ReturnData data = Triangles[2].Intersection(point, ray);
                if (data != null)
                    returnDatas.Add(data);
            }

            if (returnDatas.Count == 0)
                return null;

            double closestDistance = 1000000;
            ReturnData returnData = null;
            foreach (ReturnData data2 in returnDatas)
            {
                Vector vectorToPoint = data2.Point - point;
                if (Function.DotProduct(vectorToPoint.UnitVector(), data2.NormalVector.UnitVector()) < 0)
                {
                    double distance = vectorToPoint.Length();
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        returnData = data2;
                    }
                }
            }

            return returnData;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public Point Center { get; set; }
        public List<Triangle> Triangles { get; set; }
        public Vector UpVector { get; set; }
        public Vector OutVector { get; set; }
        public Vector PerpendicularVector { get; set; }
    }
}
