using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3
{
    class Function
    {
        public static double Degrees(double degrees)
        {
            return Math.PI / (180 / degrees);
        }
        public static double DotProduct(Vector vector1, Vector vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public static Vector CrossProduct(Vector vector1, Vector vector2)
        {
            return new Vector(
                vector1.Y * vector2.Z - vector2.Y * vector1.Z,
                vector2.X * vector1.Z - vector1.X * vector2.Z,
                vector1.X * vector2.Y - vector2.X * vector1.Y
                );
        }

        public static double Angle(Vector vector1, Vector vector2)
        {
            double dotProduct = DotProduct(vector1, vector2);
            return Math.Acos(dotProduct / (vector1.Length() * vector2.Length()));
        }

        public static Vector GetVector(Point point1, Point point2)
        {
            return new Vector(
                point2.X - point1.X,
                point2.Y - point1.Y,
                point2.Z - point1.Z
                );
        }

        public static Point Add(Point point, Vector vector)
        {
            return new Point(
                point.X + vector.X,
                point.Y + vector.Y,
                point.Z + vector.Z
                );
        }
        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector(
                vector1.X + vector2.X,
                vector1.Y + vector2.Y,
                vector1.Z + vector2.Z
                );
        }
    }
}
