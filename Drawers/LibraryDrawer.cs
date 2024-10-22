using CG1.ContextMenus;
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
        private Font font = new Font("Comic Sans Ms", 10, FontStyle.Regular);
        private Graphics g;
        private Brush brush = new SolidBrush(Color.Black);
        // Magick number here, important to aoid!!! I must to change this
        private Pen pen = new Pen(Color.Black, 2);
        private Pen penThick = new Pen(Color.Violet, 1);
        private Image vertImage = Image.FromFile("./PaintVertOnly.png");
        private Image horImage = Image.FromFile("./PaintHorOnly.png");
        private int distToImage = 8;

        public void Draw(MyPoint point, Color color, Bitmap canvas)
        {
            g = Graphics.FromImage(canvas);
            pen.Color = color;
            g.DrawEllipse(pen, point.Center.X - point.Radius, point.Center.Y - point.Radius, 2 * point.Radius, 2 * point.Radius);
        }

        public void Draw(MyLine line, Color color, Bitmap canvas)
        {
            //Bitmap last = (Bitmap)canvas.Clone();
            g = Graphics.FromImage(canvas);
            pen.Color = color;
            g.DrawLine(pen, line.First.Center, line.Second.Center);
            if (line! is MyLenghtLine)
            {
                MyLenghtLine lineLen = (MyLenghtLine)line;
                string lenStr = lineLen.Length.ToString("F2");
                (double perpX, double perpY) = MyLine.FindPerpendicular(line.First.Center, line.Second.Center);
                Point newCenter = line.GetCenter();
                newCenter.X = newCenter.X - (int)(perpX * distToImage);
                newCenter.Y = newCenter.Y - (int)(perpY * distToImage);
                g.DrawString(lenStr, font, brush, newCenter);
            }
            else if (line! is MyVerticalLine)
            {
                MyVerticalLine vertLine = (MyVerticalLine)line;
                (double perpX, double perpY) = MyLine.FindPerpendicular(line.First.Center, line.Second.Center);
                Point newCenter = line.GetCenter();
                newCenter.X = newCenter.X - (int)(perpX * distToImage);
                newCenter.Y = newCenter.Y - (int)(perpY * distToImage);
                g.DrawImage(vertLine.IsVertical ? vertImage : horImage, newCenter);
            }
        }
    }
}
