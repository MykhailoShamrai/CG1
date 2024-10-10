using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    internal class MyPoint
    {
        public Point Center { get; set; }
        public int Radius { get; set; }
        public Color Color { get; set; }

        public MyPoint(Point center, int radius, Color color)
        {
            Center = center;
            Radius = radius;
            Color = color;
        }
    }
}
