using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project5
{
    public class Color
    {
        public Color()
        {
            Red = 0;
            Blue = 0;
            Green = 0;
            Omega = 0;
        }

        public Color(double intensity)
        {
            Red = intensity;
            Blue = intensity;
            Green = intensity;
            Omega = intensity;
        }
        public Color(double red, double green, double blue)
        {
            Red = red;
            Blue = blue;
            Green = green;
            Omega = 1;
        }
        public Color(double red, double green, double blue, double omega)
        {
            Red = red;
            Blue = blue;
            Green = green;
            Omega = omega;
        }
        
        public Color Clone()
        {
            return new Color(Red, Green, Blue, Omega);
        }

        public byte[] GetColor()
        {

            byte[] bytes = { (byte)((int)((Blue / 1) * 255)), (byte)((int)((Green / 1) * 255)), (byte)((int)((Red / 1) * 255)), 0 };
            return bytes;
        }

        public int GetColorInt()
        {
            if(Omega != 1)
                return (int)((Red * 255) / Omega) << 16 | (int)((Green * 255) / Omega) << 8 | (int)((Blue * 255) / Omega);

            return (int)((Red * 255)) << 16 | (int)((Green * 255)) << 8 | (int)((Blue * 255));
        }

        public void Normalize(double value)
        {
            Red /= value;
            Green /= value;
            Blue /= value;
        }

        public void Set(Color color)
        {
            Red = color.Red;
            Green = color.Green;
            Blue = color.Blue;
            Omega = color.Omega;
        }
        
        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Omega { get; set; }

        public static Color None = new Color(0, 0, 0, 0);

        public static Color operator +(Color color1, Color color2)
        {
            //return new Color(
            //    (color1.Red/color1.Omega + color2.Red / color2.Omega), 
            //    color1.Green / color1.Omega + color2.Green / color2.Omega, 
            //    color1.Blue / color1.Omega + color2.Blue / color2.Omega, 1);
            return new Color((color1.Red + color2.Red), (color1.Green + color2.Green), (color1.Blue + color2.Blue), (color1.Omega + color2.Omega));
        }
        public static Color operator /(Color color1, double value)
        {
            //return new Color(
            //    (color1.Red/color1.Omega + color2.Red / color2.Omega), 
            //    color1.Green / color1.Omega + color2.Green / color2.Omega, 
            //    color1.Blue / color1.Omega + color2.Blue / color2.Omega, 1);
            return new Color(color1.Red / value, color1.Green / value, color1.Blue / value, color1.Omega);
        }
        public static Color operator *(Color color1, double value)
        {
            //return new Color(
            //    (color1.Red/color1.Omega + color2.Red / color2.Omega), 
            //    color1.Green / color1.Omega + color2.Green / color2.Omega, 
            //    color1.Blue / color1.Omega + color2.Blue / color2.Omega, 1);
            return new Color(color1.Red * value, color1.Green * value, color1.Blue * value, color1.Omega);
        }
    }
}
