using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7
{
    class Point
    {
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point(double x, double y, double z, double omega)
        {
            X = x;
            Y = y;
            Z = z;
            Omega = omega;
        }
        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y, point.Z + vector.Z);
        }
        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);
        }
        public static Vector operator -(Point point1, Point point2)
        {
            return new Vector(point1.X - point2.X, point1.Y - point2.Y, point1.Z - point2.Z);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Omega { get; set; }
    }
}
