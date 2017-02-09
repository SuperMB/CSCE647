using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
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
        
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Omega { get; set; }
    }
}
