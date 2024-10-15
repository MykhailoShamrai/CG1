using CG1.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Drawers
{
    public class LibraryDrawer : IDrawer
    {
        private Graphics g;
        // Magick number here, important to aoid!!! I must to change this
        private Pen pen = new Pen(Color.Black, 2);
        private Pen penThick = new Pen(Color.Violet, 1);

        public void Draw(MyPoint point, Color color, Bitmap canvas)
        {
            // Points must be solid
            //Bitmap last = (Bitmap)canvas.Clone();
            g = Graphics.FromImage(canvas);
            pen.Color = color;
            g.DrawEllipse(pen, point.Center.X - point.Radius, point.Center.Y - point.Radius, 2 * point.Radius, 2 * point.Radius);
            //return last;
        }

        public void Draw(MyLine line, Color color, Bitmap canvas)
        {
            //Bitmap last = (Bitmap)canvas.Clone();
            g = Graphics.FromImage(canvas);
            pen.Color = color;
            g.DrawLine(pen, line.First.Center, line.Second.Center);            
            //g.DrawLine(penThick, line.BoundingBox[0].points[0], line.BoundingBox[0].points[1]);
            //g.DrawLine(penThick, line.BoundingBox[0].points[1], line.BoundingBox[0].points[2]);
            //g.DrawLine(penThick, line.BoundingBox[0].points[2], line.BoundingBox[0].points[0]);
            //g.DrawLine(penThick, line.BoundingBox[1].points[0], line.BoundingBox[1].points[1]);
            //g.DrawLine(penThick, line.BoundingBox[1].points[1], line.BoundingBox[1].points[2]);
            //g.DrawLine(penThick, line.BoundingBox[1].points[2], line.BoundingBox[1].points[0]);
        }
    }
}
