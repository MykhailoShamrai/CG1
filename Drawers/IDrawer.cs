using CG1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Drawers
{
    // My idea is te create interface, that gives rules for drawing shapes like a line, a curve and a point. 
    // Accordding to different mode, all program will pass a different IDrawer, to create shapes.
    public interface IDrawer
    {
        public void DrawCircle(MyPoint center, Color color, Bitmap canvas);
        public void DrawLine(MyLine line, Color color, Bitmap canvas);
    }
}
