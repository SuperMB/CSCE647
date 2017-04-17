using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    class ImageData
    {
        public ImageData()
        {

        }

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
            TextureData = new Color[Width][];
            for (int i = 0; i < image.Width; i++)
            {
                TextureData[i] = new Color[Height];
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    TextureData[i][j] = new Color(
                        pixel.R / (double)255,
                        pixel.G / (double)255,
                        pixel.B / (double)255,
                        1
                        );
                }
            }
        }

        public void MakeSky()
        {
            PerlinNoiseTexture perlin = new PerlinNoiseTexture();
            TextureData = perlin.FillRectangle(1000, 1000, 0, 0, new PerlinOptions());
            Width = 1000;
            Height = 1000;
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
                    TextureData[i][j] = new Color(
                        pixel.R / (double)255,
                        pixel.G / (double)255,
                        pixel.B / (double)255,
                        1
                        );
                }
            }
        }

        public Color GetPixel(int x, int y)
        {
            if (x < 0 | x > Width - 1 | y < 0 | y > Height - 1)
                return Color.BlueColor;

            Color color = TextureData[x][y];
            return color;

            //try
            //{
            //    return _imageData[x - 1][y - 1];
            //}
            //catch
            //{
            //    return ColorMatrix.None;
            //}
        }

        public Color[][] TextureData { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string FileName { get; set; }
    }
}
