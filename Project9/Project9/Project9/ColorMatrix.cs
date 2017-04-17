using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    class ColorMatrix
    {
        public ColorMatrix()
        {
            Matrix = new double[4][];
            for (int i = 0; i < Matrix.Count(); i++)
            {
                Matrix[i] = new double[4];
                for (int j = 0; j < Matrix[i].Count(); j++)
                    Matrix[i][j] = 0;
            }
            IsHighlight = false;
            IgnoreDiffusedIntensity = false;
            IgnoreSpecularIntensity = false;
            IgnoreSpotlightIntensity = false;
        }

        public ColorMatrix(double red, double green, double blue)
            : this()
        {
            Red(red);
            Green(green);
            Blue(blue);
            Omega(1);
        }
        public ColorMatrix(double red, double green, double blue, double omega)
            : this()
        {
            Red(red);
            Green(green);
            Blue(blue);
            Omega(omega);
        }

        public void Red(double value)
        {
            Matrix[0][0] = value;
        }
        public void Green(double value)
        {
            Matrix[1][1] = value;
        }
        public void Blue(double value)
        {
            Matrix[2][2] = value;
        }
        public void Omega(double value)
        {
            Matrix[3][3] = value;
        }

        public static ColorMatrix White()
        {
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.Red(1);
            colorMatrix.Green(1);
            colorMatrix.Blue(1);
            colorMatrix.Omega(1);
            return colorMatrix;
        }
        public static ColorMatrix Red()
        {
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.Red(1);
            colorMatrix.Omega(1);
            return colorMatrix;
        }
        public static ColorMatrix Green()
        {
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.Green(1);
            colorMatrix.Omega(1);
            return colorMatrix;
        }
        public static ColorMatrix Blue()
        {
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.Blue(1);
            colorMatrix.Omega(1);
            return colorMatrix;
        }
        public static ColorMatrix DarkGray()
        {
            ColorMatrix colorMatrix = new ColorMatrix();
            double value = .2;
            colorMatrix.Red(value);
            colorMatrix.Green(value);
            colorMatrix.Blue(value);
            colorMatrix.Omega(1);
            return colorMatrix;
        }

        public static ColorMatrix Highlight()
        {
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.Green(0);
            colorMatrix.Blue(0);
            colorMatrix.Omega(0);
            colorMatrix.IsHighlight = true;
            return colorMatrix;
        }

        public Color Flatten()
        {
            return new Color(Matrix[0][0], Matrix[1][1], Matrix[2][2], Matrix[3][3]);
        }

        public static Color operator *(ColorMatrix colorMatrix, Color color)
        {
            double red =
                colorMatrix.Matrix[0][0] * color.Red +
                colorMatrix.Matrix[0][1] * color.Green +
                colorMatrix.Matrix[0][2] * color.Blue +
                colorMatrix.Matrix[0][3] * color.Omega;

            double green =
                colorMatrix.Matrix[1][0] * color.Red +
                colorMatrix.Matrix[1][1] * color.Green +
                colorMatrix.Matrix[1][2] * color.Blue +
                colorMatrix.Matrix[1][3] * color.Omega;

            double blue =
                colorMatrix.Matrix[2][0] * color.Red +
                colorMatrix.Matrix[2][1] * color.Green +
                colorMatrix.Matrix[2][2] * color.Blue +
                colorMatrix.Matrix[2][3] * color.Omega;

            double omega =
                colorMatrix.Matrix[3][0] * color.Red +
                colorMatrix.Matrix[3][1] * color.Green +
                colorMatrix.Matrix[3][2] * color.Blue +
                colorMatrix.Matrix[3][3] * color.Omega;

            return new Color(red, green, blue, omega);
        }
        public static ColorMatrix operator +(ColorMatrix colorMatrix1, ColorMatrix colorMatrix2)
        {
            ColorMatrix colorMatrix = new ColorMatrix();

            colorMatrix.Matrix[0][0] = colorMatrix1.Matrix[0][0] + colorMatrix2.Matrix[0][0];
            colorMatrix.Matrix[0][1] = colorMatrix1.Matrix[0][1] + colorMatrix2.Matrix[0][1];
            colorMatrix.Matrix[0][2] = colorMatrix1.Matrix[0][2] + colorMatrix2.Matrix[0][2];
            colorMatrix.Matrix[0][3] = colorMatrix1.Matrix[0][3] + colorMatrix2.Matrix[0][3];

            colorMatrix.Matrix[1][0] = colorMatrix1.Matrix[1][0] + colorMatrix2.Matrix[1][0];
            colorMatrix.Matrix[1][1] = colorMatrix1.Matrix[1][1] + colorMatrix2.Matrix[1][1];
            colorMatrix.Matrix[1][2] = colorMatrix1.Matrix[1][2] + colorMatrix2.Matrix[1][2];
            colorMatrix.Matrix[1][3] = colorMatrix1.Matrix[1][3] + colorMatrix2.Matrix[1][3];
        
            colorMatrix.Matrix[2][0] = colorMatrix1.Matrix[2][0] + colorMatrix2.Matrix[2][0];
            colorMatrix.Matrix[2][1] = colorMatrix1.Matrix[2][1] + colorMatrix2.Matrix[2][1];
            colorMatrix.Matrix[2][2] = colorMatrix1.Matrix[2][2] + colorMatrix2.Matrix[2][2];
            colorMatrix.Matrix[2][3] = colorMatrix1.Matrix[2][3] + colorMatrix2.Matrix[2][3];

            colorMatrix.Matrix[3][0] = colorMatrix1.Matrix[3][0] + colorMatrix2.Matrix[3][0];
            colorMatrix.Matrix[3][1] = colorMatrix1.Matrix[3][1] + colorMatrix2.Matrix[3][1];
            colorMatrix.Matrix[3][2] = colorMatrix1.Matrix[3][2] + colorMatrix2.Matrix[3][2];
            colorMatrix.Matrix[3][3] = colorMatrix1.Matrix[3][3] + colorMatrix2.Matrix[3][3];


            return colorMatrix;
        }

        public double [][] Matrix { get; set; }
        public bool IsHighlight { get; set; }
        public bool IgnoreDiffusedIntensity { get; set; }
        public bool IgnoreSpecularIntensity { get; set; }
        public bool IgnoreSpotlightIntensity { get; set; }
        public static ColorMatrix None = new ColorMatrix(0, 0, 0, 0);
    }
}
