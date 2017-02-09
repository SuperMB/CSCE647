using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
{
    class Screen
    {
        public Screen(double width, double height, Point point0, Vector vector0, double scale0, Vector vector1, double scale1)
        {
            Width = width;
            Height = height;
            Point0 = point0;
            Vector0 = vector0.UnitVector();
            Scale0 = scale0;
            Vector1 = vector1.UnitVector();
            Scale1 = scale1;
            Pixels = new Color[(int)width][];
            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i] = new Color[(int)height];
                for (int j = 0; j < Pixels[i].Length; j++)
                    Pixels[i][j] = new Color();
            }
        }

        public Point GetPoint(double pixelX, double pixelY)
        {
            double vector0Percentage = pixelX / Width;
            double pixelYFlipped = Height - pixelY;
            double vector1Percentage = pixelYFlipped / Height;

            return new Point(
                Point0.X + vector0Percentage * Vector0.X * Scale0 + vector1Percentage * Vector1.X * Scale1,
                Point0.Y + vector0Percentage * Vector0.Y * Scale0 + vector1Percentage * Vector1.Y * Scale1,
                Point0.Z + vector0Percentage * Vector0.Z * Scale0 + vector1Percentage * Vector1.Z * Scale1
                );
        }

        public void Normalize()
        {
            double max = -1000;
            foreach (Color[] row in Pixels)
                foreach (Color pixel in row)
                    if (pixel.Omega > max)
                        max = pixel.Omega;

            foreach (Color[] row in Pixels)
                foreach (Color pixel in row)
                    pixel.Normalize(max);
        }

        public double Width { get; set; }
        public double Height { get; set; }
        public Point Point0 { get; set; }
        public Vector Vector0 { get; set; }
        public double Scale0 { get; set; }
        public Vector Vector1 { get; set; }
        public double Scale1 { get; set; }
        public Color [][] Pixels
        {
            get;
            set;
        }
    }

}
