using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project11
{
    class Tetrahedron : Shape
    {
        public Tetrahedron(Point center, double distance)
        {
            Point point0 = center + distance * Vector0;
            Point point1 = center + distance * Vector1;
            Point point2 = center + distance * Vector2;
            Point point3 = center + distance * Vector3;

            Center = center;
            
            Triangles = new Triangle[4];
            Triangles[0] = new Triangle(point0, point1, point2);
            Triangles[1] = new Triangle(point0, point1, point3);
            Triangles[2] = new Triangle(point0, point2, point3);
            Triangles[3] = new Triangle(point1, point2, point3);
        }
        public Color GetColor(Point point)
        {
            return Triangles[0].Color;
        }
        public void SetColor(Color color)
        {
            foreach(Triangle triangle in Triangles)
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
            foreach(ReturnData data2 in returnDatas)
            {
                double distance = (data2.Point - point).Length();
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    returnData = data2;
                }
            }

            return returnData;
        }
        public ReturnData Outline(Point point, Vector ray)
        {
            return null;
        }

        public Point Center { get; set; }
        public Triangle[] Triangles { get; set; }

        public static readonly Vector Vector0 = new Vector(1, 1, 1);
        public static readonly Vector Vector1 = new Vector(1, -1, -1);
        public static readonly Vector Vector2 = new Vector(-1, 1, -1);
        public static readonly Vector Vector3 = new Vector(-1, -1, 1);
    }
}
