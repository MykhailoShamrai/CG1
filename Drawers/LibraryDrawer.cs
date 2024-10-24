using CG1.Shapes;

namespace CG1.Drawers
{
    public class LibraryDrawer : IDrawer
    {
        public Bitmap canvas { get; set; }

        private Font font = new Font("Comic Sans Ms", 10, FontStyle.Regular);
        private Graphics g;
        private Brush brush = new SolidBrush(Color.Black);
        // Magick number here, important to aoid!!! I must to change this
        private Pen pen = new Pen(Color.Black, 2);
        private Pen penThick = new Pen(Color.Violet, 1);
        private Pen dashedPen = new Pen(Color.Violet, 1);
        private Image vertImage = Image.FromFile("./PaintVertOnly.png");
        private Image horImage = Image.FromFile("./PaintHorOnly.png");
        private int distToImage = 8;
        
        public LibraryDrawer()
        {
            dashedPen.DashPattern = [2, 2];
        }

        public void Draw(MyPoint point, Color color)
        {
            g = Graphics.FromImage(canvas);
            pen.Color = color;
            g.DrawEllipse(pen, point.Center.X - point.Radius, point.Center.Y - point.Radius, 2 * point.Radius, 2 * point.Radius);
        }

        public void Draw(MyLine line, Color color)
        {
            //Bitmap last = (Bitmap)canvas.Clone();
            g = Graphics.FromImage(canvas);
            pen.Color = color;
            g.DrawLine(pen, line.First.Center, line.Second.Center);
        }

        public void Draw(MyVerticalLine vertLine, Color color)
        {
            Draw((MyLine)vertLine, color);
            (double perpX, double perpY) = MyLine.FindPerpendicular(vertLine.First.Center, vertLine.Second.Center);
            Point newCenter = vertLine.GetCenter();
            newCenter.X = newCenter.X - (int)(perpX * distToImage);
            newCenter.Y = newCenter.Y - (int)(perpY * distToImage);
            g.DrawImage(vertLine.IsVertical ? vertImage : horImage, newCenter);
        }

        public void Draw(MyLenghtLine lenghtLine, Color color)
        {
            Draw((MyLine)lenghtLine, color);
            string lenStr = lenghtLine.Length.ToString("F2");
            (double perpX, double perpY) = MyLine.FindPerpendicular(lenghtLine.First.Center, lenghtLine.Second.Center);
            Point newCenter = lenghtLine.GetCenter();
            newCenter.X = newCenter.X - (int)(perpX * distToImage);
            newCenter.Y = newCenter.Y - (int)(perpY * distToImage);
            g.DrawString(lenStr, font, brush, newCenter);
        }

        public void Draw(MyBezier myBezier, Color color)
        {
            pen.Color = color;
            g.DrawLine(dashedPen, myBezier.First.Center, myBezier.Second.Center);
            g.DrawLine(dashedPen, myBezier.FirstControlVertex.Center, myBezier.SecondControlVertex.Center);
            g.DrawLine(dashedPen, myBezier.First.Center, myBezier.FirstControlVertex.Center);
            g.DrawLine(dashedPen, myBezier.SecondControlVertex.Center, myBezier.Second.Center);
            Draw(myBezier.SecondControlVertex, Color.Blue);
            Draw(myBezier.FirstControlVertex, Color.Blue);
        }
    }
}
