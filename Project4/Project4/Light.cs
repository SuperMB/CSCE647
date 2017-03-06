using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project4
{
    interface Light
    {
        Color ShineOnShape(ReturnData data, double alpha, List<Shape> shapes);
    }
}
