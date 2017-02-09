using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
{
    class Color
    {
        public Color()
        {
            Red = 0;
            Blue = 0;
            Green = 0;
            Omega = 0;
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

        public byte[] GetColor()
        {

            byte[] bytes = { (byte)((int)((Blue / 1) * 255)), (byte)((int)((Green / 1) * 255)), (byte)((int)((Red / 1) * 255)), 0 };
            return bytes;
        }

        public void Normalize(double value)
        {
            Red /= value;
            Green /= value;
            Blue /= value;
        }



        public double Red { get; set; }
        public double Green { get; set; }
        public double Blue { get; set; }
        public double Omega { get; set; }

        public static Color operator +(Color color1, Color color2)
        {
            //return new Color(
            //    (color1.Red/color1.Omega + color2.Red / color2.Omega), 
            //    color1.Green / color1.Omega + color2.Green / color2.Omega, 
            //    color1.Blue / color1.Omega + color2.Blue / color2.Omega, 1);
            return new Color(color1.Red + color2.Red, color1.Green + color2.Green, color1.Blue + color2.Blue, color1.Omega + color2.Omega);
        }
        public static Color operator /(Color color1, double value)
        {
            //return new Color(
            //    (color1.Red/color1.Omega + color2.Red / color2.Omega), 
            //    color1.Green / color1.Omega + color2.Green / color2.Omega, 
            //    color1.Blue / color1.Omega + color2.Blue / color2.Omega, 1);
            return new Color(color1.Red / value, color1.Green / value, color1.Blue / value, color1.Omega / value);
        }
    }
}
