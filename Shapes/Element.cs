using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public abstract class Element
    {
        public static int Cross(Point x, Point y, Point o)
        {
            return (x.X - o.X) * (y.Y - o.Y) - (x.Y - o.Y) * (y.X - o.X);
        }
    }
}
