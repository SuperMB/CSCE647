using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6
{
    public class PerlinNoiseTexture
    {
        //used in large from https://github.com/Zicore/NoiseTools/tree/master/NoiseTools
        //but adapted for use in sky

        public PerlinNoiseTexture()
        {
            this._noise = new PerlinNoise(new Random());
        }

        public PerlinNoiseTexture(PerlinNoise noise)
        {
            this._noise = noise;
        }

        PerlinNoise _noise;

        public PerlinNoise Noise
        {
            get { return _noise; }
            set { _noise = value; }
        }

        int octaves = 32;
        double lacunarity = 2.85;
        double gain = 0.45;
        double offset = 1.0;

        public int Octaves
        {
            get { return octaves; }
            set { octaves = value; }
        }

        public double Lacunarity
        {
            get { return lacunarity; }
            set { lacunarity = value; }
        }

        public double Gain
        {
            get { return gain; }
            set { gain = value; }
        }

        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private Color[][] PrepareColorArray(int x, int y)
        {
            var data = new Color[x][];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new Color[y];
            }
            return data;
        }

        public Color[][] FillRectangle(int width, int height, double offsetX, double offsetY, PerlinOptions options)
        {
            double red = options.RandomColor.Next(options.MinRed, options.MaxRed) / options.FactorRed;
            double green = options.RandomColor.Next(options.MinGreen, options.MaxGreen) / options.FactorGreen;
            double blue = options.RandomColor.Next(options.MinBlue, options.MaxBlue) / options.FactorBlue;

            Color[][] data = PrepareColorArray(width, height);
            double x = 0;
            double y = 0;
            double offX = x;


            for (int v = 0; v < width; v++)
            {
                y += options.PerlinNoiseStep;
                x = offX;

                for (int u = 0; u < height; u++)
                {
                    double noise = (double)Noise.RidgedMF(offsetX + x, offsetY + y, 0, options.Octaves, options.Lacunarity, options.Gain, options.Offset);
                    data[v][u] = ModifyColor(noise, options, red, red, blue);

                    x += options.PerlinNoiseStep;
                }
            }

            return data;
        }

        public Color ModifyColor(double noise, PerlinOptions options, double red, double green, double blue)
        {
            int colorRed = 0;
            int colorGreen = 0;
            int colorBlue = 0;

            colorRed = ColordoubleToInt(noise * red);
            if (options.UseCosineOnRed)
            {
                colorRed = ColordoubleToInt((double)Math.Cos(red / noise));
            }

            colorGreen = ColordoubleToInt(noise * green);
            if (options.UseCosineOnGreen)
            {
                colorGreen = ColordoubleToInt((double)Math.Cos(green / noise));
            }

            colorBlue = ColordoubleToInt(noise * blue);
            if (options.UseCosineOnBlue)
            {
                colorBlue = ColordoubleToInt((double)Math.Cos(blue / noise));
            }

            if (options.ReverseRed)
                colorRed = 255 - colorRed;

            if (options.ReverseGreen)
                colorGreen = 255 - colorGreen;

            if (options.ReverseBlue)
                colorBlue = 255 - colorBlue;

            double finalRed = colorRed / 255.0;
            double finalGreen = colorGreen / 255.0;
            double finalBlue = colorBlue / 255.0;
            double factor = (finalRed + finalGreen + finalBlue) / 3.0;
            double startValue = .4;
            return new Color(factor * (1- startValue) + startValue, factor * (1 - startValue) + startValue, 1);
        }

        private static int ColordoubleToInt(double value)
        {
            return Math.Min(Math.Max((int)(255 * value), 0), 255);
        }
    }

}
