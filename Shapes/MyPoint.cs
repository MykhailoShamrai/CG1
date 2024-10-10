using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class MyPoint
    {
        public Point Center { get; set; }
        public int Radius { get; set; }

        public Color Color { get; set; }

        public MyPoint(Point center, int radius)
        {
            Center = center;
            Radius = radius;
            Color = Color.Black;
        }
    }
}
