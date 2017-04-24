using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project11
{
    class Matrix
    {
        public Matrix()
        {
            matrix = new double[3][];
            for (int i = 0; i < matrix.Count(); i++)
                matrix[i] = new double[3];
        }

        public static Matrix RotationXMatrix(double theta)
        {
            Matrix matrix = new Matrix();
            matrix[0][0] = 1;
            matrix[0][1] = 0;
            matrix[0][2] = 0;

            matrix[1][0] = 0;
            matrix[1][1] = Math.Cos(theta);
            matrix[1][2] = - Math.Sin(theta);

            matrix[2][0] = 0;
            matrix[2][1] = Math.Sin(theta);
            matrix[2][2] = Math.Cos(theta);

            return matrix;
        }
        public static Matrix RotationYMatrix(double theta)
        {
            Matrix matrix = new Matrix();
            matrix[0][0] = Math.Cos(theta);
            matrix[0][1] = 0;
            matrix[0][2] = Math.Sin(theta);

            matrix[1][0] = 0;
            matrix[1][1] = 1;
            matrix[1][2] = 0;

            matrix[2][0] = - Math.Sin(theta);
            matrix[2][1] = 0;
            matrix[2][2] = Math.Cos(theta);

            return matrix;
        }
        public static Matrix RotationZMatrix(double theta)
        {
            Matrix matrix = new Matrix();
            matrix[0][0] = Math.Cos(theta);
            matrix[0][1] = - Math.Sin(theta);
            matrix[0][2] = 0;

            matrix[1][0] = Math.Sin(theta);
            matrix[1][1] = Math.Cos(theta);
            matrix[1][2] = 0;

            matrix[2][0] = 0;
            matrix[2][1] = 0;
            matrix[2][2] = 1;

            return matrix;
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            return new Vector(
                matrix[0][0] * vector.X
                + matrix[0][1] * vector.Y
                + matrix[0][2] * vector.Z,

                matrix[1][0] * vector.X
                + matrix[1][1] * vector.Y
                + matrix[1][2] * vector.Z,

                matrix[2][0] * vector.X
                + matrix[2][1] * vector.Y
                + matrix[2][2] * vector.Z
                );
        }

        public double[] this[int i]
        {
            get { return matrix[i]; }
        }

        private double[][] matrix;
    }
}
