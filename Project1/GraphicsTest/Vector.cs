using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
{
    class Vector
    {
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector UnitVector()
        {
            double length = Length();
            return new Vector(X / length, Y / length, Z / length);
        }

        public void Scale(double factor)
        {
            X = X * factor;
            Y = Y * factor;
            Z = Z * factor;
        }

        public Vector Clone()
        {
            return new Vector(X, Y, Z);
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
