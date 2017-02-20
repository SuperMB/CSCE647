using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3
{
    class ReturnData
    {
        public ReturnData()
        {
            Point = null;
            ColorMatrix = null;
            NormalVector = null;
        }

        public Point Point { get; set; }
        public ColorMatrix ColorMatrix { get; set; }
        public Vector NormalVector { get; set; }
    }
}
