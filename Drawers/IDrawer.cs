using CG1.Shapes;
using System.Drawing;
using System.Numerics;

namespace CG1.Drawers
{
    // My idea is te create interface, that gives rules for drawing shapes like a line, a curve and a point. 
    // Accordding to different mode, all program will pass a different IDrawer, to create shapes.
    public interface IDrawer
    {
        public Bitmap Canvas { get; }

        public Font Font { get; }
        public Graphics G { get; set; }
        public Brush Brush { get; }
        public Pen Pen { get; }
        public Pen PenViolet { get; }
        public Pen DashedPen { get; } 
        public Image VertImage { get; }
        public Image HorImage { get; }
        public int DistToImage { get; }


        public void Draw(MyPoint point, Color color)
        {
            using (Pen newPen = new Pen(color, 2))
                G.DrawEllipse(newPen, point.Center.X - point.Radius, point.Center.Y - point.Radius, 2 * point.Radius, 2 * point.Radius);
        }
        public void Draw(MyLine line, Color color);


        public void Draw(BezierVertex vertex, Color color)
        {
            Draw((MyPoint)vertex, color);
            G.DrawString(vertex.VertexState.ToString(), Font, Brush, vertex.Center);
        }

        public void Draw(BezierControlVertex control, Color color)
        {
            Draw((MyPoint)control, Color.BlueViolet);
        }

        public void Draw(MyLenghtLine lenghtLine, Color color)
        {
            Draw((MyLine)lenghtLine, color);
            string lenStr = lenghtLine.Length.ToString("F2");
            (double perpX, double perpY) = MyLine.FindPerpendicular(lenghtLine.First.Center, lenghtLine.Second.Center);
            Point newCenter = lenghtLine.GetCenter();
            newCenter.X = newCenter.X - (int)(perpX * DistToImage);
            newCenter.Y = newCenter.Y - (int)(perpY * DistToImage);
            G.DrawString(lenStr, Font, Brush, newCenter);
        }
        public void Draw(MyVerticalLine vertLine, Color color)
        {
            Draw((MyLine)vertLine, color);
            (double perpX, double perpY) = MyLine.FindPerpendicular(vertLine.First.Center, vertLine.Second.Center);
            Point newCenter = vertLine.GetCenter();
            newCenter.X = newCenter.X - (int)(perpX * DistToImage);
            newCenter.Y = newCenter.Y - (int)(perpY * DistToImage);
            G.DrawImage(vertLine.IsVertical ? VertImage : HorImage, newCenter);
        }
        public void Draw(MyBezier myBezier, Color color)
        {
            G.DrawLine(DashedPen, myBezier.First.Center, myBezier.Second.Center);
            G.DrawLine(DashedPen, myBezier.FirstControlVertex.Center, myBezier.SecondControlVertex.Center);
            G.DrawLine(DashedPen, myBezier.First.Center, myBezier.FirstControlVertex.Center);
            G.DrawLine(DashedPen, myBezier.SecondControlVertex.Center, myBezier.Second.Center);
            

            // Drawing all lines
            double t = myBezier.Shift;
            float d = (float)myBezier.Shift;
            Vector2 pNow = myBezier.A;
            Vector2 pThird = 6 * myBezier.D * (d * d * d);
            Vector2 pSecond = pThird + 2 * (d * d) * myBezier.C;
            Vector2 pFirst = myBezier.D * (d * d * d) + myBezier.C * (d * d) + myBezier.B * d;

            MyPoint tmp1 = new MyPoint(new Point((int)pNow.X, (int)pNow.Y), 1, myBezier.ParentPolygon);
            MyPoint tmp2 = new MyPoint(new Point(0, 0), 1, myBezier.ParentPolygon);
            MyLine tmpLine = new MyLine(tmp1, tmp2, Color.Magenta, myBezier.ParentPolygon);
            while (t < 1)
            {
                pNow = pNow + pFirst;
                pFirst = pFirst + pSecond;
                pSecond = pSecond + pThird;
                tmp2.Center = new Point((int)pNow.X, (int)pNow.Y);
                G.DrawLine(PenViolet, tmpLine.First.Center, tmpLine.Second.Center);
                tmp1.Center = new Point((int)pNow.X, (int)pNow.Y);
                t += d;
            }
            tmp2.Center = myBezier.Second.Center;
            G.DrawLine(PenViolet, tmpLine.First.Center, tmpLine.Second.Center);
        }
    }
}
