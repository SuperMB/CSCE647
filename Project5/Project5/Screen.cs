using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    class Screen
    {
        public Screen(double width, double height, Point point, Vector xVector, double xScale, Vector yVector, double yScale)
        {
            Width = width;
            Height = height;
            Point = point;
            XVector = xVector.UnitVector();
            XScale = xScale;
            YVector = yVector.UnitVector();
            YScale = yScale;
            Pixels = new Color[(int)width][];
            for (int i = 0; i < Pixels.Length; i++)
            {
                Pixels[i] = new Color[(int)height];
                for (int j = 0; j < Pixels[i].Length; j++)
                    Pixels[i][j] = new Color();
            }

            PixelArray = new int[(int)(Width * Height)];
        }

        public void Set(Point point, Vector xVector, Vector yVector)
        {
            Point = point;
            XVector = xVector;
            YVector = yVector;
        }

        public Point GetPoint(double pixelX, double pixelY)
        {
            double vector0Percentage = pixelX / Width;
            double pixelYFlipped = Height - pixelY;
            double vector1Percentage = pixelYFlipped / Height;

            return new Point(
                Point.X + vector0Percentage * XVector.X * XScale + vector1Percentage * YVector.X * YScale,
                Point.Y + vector0Percentage * XVector.Y * XScale + vector1Percentage * YVector.Y * YScale,
                Point.Z + vector0Percentage * XVector.Z * XScale + vector1Percentage * YVector.Z * YScale
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
        public Point Point { get; set; }
        public Vector XVector { get; set; }
        public double XScale { get; set; }
        public Vector YVector { get; set; }
        public double YScale { get; set; }
        public Color [][] Pixels
        {
            get;
            set;
        }

        public void SetPixel(int x, int y, Color color)
        {
            Pixels[x - 1][y - 1].Set(color);
            PixelArray[(int)((y - 1) * Width + (x - 1))] = color.GetColorInt();
        }
        public void ErasePixel(int x, int y)
        {
            Pixels[x - 1][y - 1] = Color.None;
            PixelArray[(int)((y - 1) * Width + (x - 1))] = 0;
        }
        public int [] PixelArray { get; set; }
    }

}
