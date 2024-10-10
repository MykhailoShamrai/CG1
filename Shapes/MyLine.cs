using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class MyLine
    {
        public MyPoint First { get; set; }
        public MyPoint Second { get; set; }
        public Color Color { get; set; }
        public MyLine(MyPoint first, MyPoint second, Color color)
        {
            First = first;
            Second = second;
            Color = color;
        }
    }
}
