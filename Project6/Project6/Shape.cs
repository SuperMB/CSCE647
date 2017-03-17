using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project6
{
    interface Shape
    {
        bool Inside(Point point);
        ColorMatrix GetColorMatrix(Point point);
        void SetColorMatrix(ColorMatrix colorMatrix);
        ReturnData Intersection(Point point, Vector ray);
        ReturnData Outline(Point point, Vector ray);
    }
}
