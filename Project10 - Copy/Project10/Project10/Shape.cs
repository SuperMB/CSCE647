using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10
{
    interface Shape
    {
        Color GetColor(Point point);
        void SetColor(Color colorMatrix);
        ReturnData Intersection(Point point, Vector ray);
        ReturnData Outline(Point point, Vector ray);
    }
}
