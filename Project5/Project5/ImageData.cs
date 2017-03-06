using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    class ImageData
    {
        public ImageData(string fileName)
        {
            FileName = fileName;
            Bitmap image;
            using (FileStream stream = new FileStream(FileName, FileMode.Open))
            {
                image = (Bitmap)Image.FromStream(stream);
            }
            Width = image.Width;
            Height = image.Height;
            _imageData = new Color[Width][];
            for (int i = 0; i < image.Width; i++)
            {
                _imageData[i] = new Color[Height];
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    _imageData[i][j] = new Color(
                        pixel.R / (double)255,
                        pixel.G / (double)255,
                        pixel.B / (double)255,
                        1
                        );
                }
            }
        }

        public void ReloadTexture()
        {
            Bitmap image;
            using (FileStream stream = new FileStream(FileName, FileMode.Open))
            {
                image = (Bitmap)Image.FromStream(stream);
            }
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    _imageData[i][j] = new Color(
                        pixel.R / (double)255,
                        pixel.G / (double)255,
                        pixel.B / (double)255,
                        1
                        );
                }
            }
        }

        public ColorMatrix GetPixel(int x, int y)
        {
            if (x < 0 | x > Width - 1 | y < 0 | y > Height - 1)
                return ColorMatrix.Blue();

            Color color = _imageData[x][y];
            ColorMatrix colorMatrix = new ColorMatrix(color.Red, color.Green, color.Blue);
            return colorMatrix;

            //try
            //{
            //    return _imageData[x - 1][y - 1];
            //}
            //catch
            //{
            //    return ColorMatrix.None;
            //}
        }

        Color [][] _imageData;
        public int Width { get; }
        public int Height { get; }
        public string FileName { get; set; }
    }
}
