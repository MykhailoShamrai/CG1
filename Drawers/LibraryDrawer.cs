using CG1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Drawers
{
    internal class LibraryDrawer : IDrawer
    {
        public Bitmap DrawCircle(MyPoint point, Bitmap canvas)
        {
            // Points must be solid
            Bitmap last = (Bitmap)canvas.Clone();
            Graphics g = Graphics.FromImage(canvas);
            Pen pen = new Pen(point.Color);
            g.DrawEllipse(pen, point.Center.X - point.Radius / 2, point.Center.Y - point.Radius / 2, point.Radius, point.Radius);
            return last;
        }

        public Bitmap DrawLine(MyPoint first, MyPoint second, Bitmap canvas)
        {
            throw new NotImplementedException();
        }
    }
}
