using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7
{
    class ObjReader
    {
        public static List<Triangle> ReadObjFile(string fileName)
        {
            List<Point> points = new List<Point>();
            List<Triangle> triangles = new List<Triangle>();
            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines)
            {
                Console.Out.WriteLine(line);
                string[] split = line.Split(' ');
                if (split[0] == "v")
                {
                    double[] doubles = new double[3];
                    for (int i = 1; i < split.Count(); i++)
                        doubles[i - 1] = double.Parse(split[i]);
                    points.Add(new Point(
                        doubles[0],
                        doubles[1],
                        doubles[2]
                        ));
                }
                else if(split[0] == "f")
                {
                    int[] ints = new int[3];
                    for (int i = 1; i < split.Count(); i++)
                        ints[i - 1] = int.Parse(split[i]);
                    triangles.Add(new Triangle(
                        points[ints[0] - 1],
                        points[ints[1] - 1],
                        points[ints[2] - 1]
                        ));
                }
            }

            return triangles;
        }
    }
}
