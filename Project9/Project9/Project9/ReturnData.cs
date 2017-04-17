using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    class ReturnData
    {
        public ReturnData()
        {
            Point = null;
            LightPoint = null;
            Color = null;
            NormalVector = null;
            AngleDirection = AngleDirection.AngleIncreasing;
            NonIntersectingShapes = null;
            IgnoreShadow = false;
        }

        public Point Point { get; set; }
        public Point LightPoint { get; set; }
        public Color Color { get; set; }
        public Vector NormalVector { get; set; }
        public AngleDirection AngleDirection { get; set; }
        public List<Shape> NonIntersectingShapes { get; set; }
        public bool IgnoreShadow { get; set; }
    }
}
