using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicsTest
{
    interface Shape
    {
        bool Inside(Point point);
        Color Color();
        void SetColor(Color color);
    }
}
