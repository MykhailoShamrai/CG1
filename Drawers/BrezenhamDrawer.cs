using CG1.ContextMenus;
using CG1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Drawers
{
    public class BrezenhamDrawer : IDrawer
    {
        public Bitmap Canvas { get; }

        public Font Font => new Font("Comic Sans Ms", 10, FontStyle.Regular);

        public Graphics G { get; set; }

        public Brush Brush => new SolidBrush(Color.Black);

        public Pen Pen => new Pen(Color.Black, 2);

        public Pen PenViolet => new Pen(Color.Violet, 2);

        public Pen DashedPen => new Pen(Color.Violet, 1);

        public Image VertImage => Image.FromFile("./PaintVertOnly.png");

        public Image HorImage => Image.FromFile("./PaintHorOnly.png");

        
        public int DistToImage => 8;

        public BrezenhamDrawer(Bitmap canvas)
        {
            Canvas = canvas;
            DashedPen.DashPattern = [2, 2];
            G = Graphics.FromImage(Canvas);
        }

        public void Draw(MyLine line, Color color)
        {
            if (((LineMenu)line.Menu).AntiAliasFlag)
            {
                ((IDrawer)this).DrawWithW(line.First.Center, line.Second.Center);
                return;
            }
            Point first = line.First.Center;
            Point second = line.Second.Center;
            if (Math.Abs(second.Y - first.Y) < Math.Abs(second.X - first.X))
            {
                if (first.X > second.X)
                {
                    plotLineLow(second, first);
                }
                else
                {
                    plotLineLow(first, second);
                }
            }
            else
            {
                if (first.Y > second.Y)
                {
                    plotLineHigh(second, first);
                }
                else
                {
                    plotLineHigh(first, second);
                }
            }
        }

        private void plotLineLow(Point first, Point second)
        {
            int dx = second.X - first.X;
            int dy = second.Y - first.Y;
            int yi = 1;

            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int D = (2 * dy) - dx;
            int y = first.Y;

            for (int x = first.X; x <= second.X; x++)
            {
                if (x >= 0 && y >= 0)
                    Canvas.SetPixel(x, y, Color.Black);
                if (D > 0)
                {
                    y += yi;
                    D += (2 * (dy - dx));
                }
                else
                {
                    D += 2 * dy;
                }
            }
        }

        private void plotLineHigh(Point first, Point second)
        {
            int dx = second.X - first.X;
            int dy = second.Y - first.Y;
            int xi = 1;

            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int D = (2 * dx) - dy;
            int x = first.X;

            for (int y = first.Y; y <= second.Y; y++)
            {
                if (x >= 0 && y >= 0)
                    Canvas.SetPixel(x, y, Color.Black);
                if (D > 0)
                {
                    x += xi;
                    D += (2 * (dx - dy));
                }
                else
                {
                    D += 2 * dx;
                }
            }
        }
    }
}
