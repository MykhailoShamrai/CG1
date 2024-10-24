using CG1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Drawers
{
    // My idea is te create interface, that gives rules for drawing shapes like a line, a curve and a point. 
    // Accordding to different mode, all program will pass a different IDrawer, to create shapes.
    public interface IDrawer
    {
        public Bitmap canvas { get; set; }
        public void Draw(MyPoint center, Color color);
        public void Draw(MyLine line, Color color);

        public void Draw(MyLenghtLine line, Color color);
        public void Draw(MyVerticalLine verticalLine, Color color);
    }
}
