using CG1.Shapes;
using System.Numerics;

namespace CG1.Drawers
{
    public class LibraryDrawer : IDrawer
    {
        public Bitmap Canvas { get; }

        public Font Font => new Font("Comic Sans Ms", 10, FontStyle.Regular);

        public Graphics G { get; } 

        public Brush Brush => new SolidBrush(Color.Black);

        public Pen Pen => new Pen(Color.Black, 2);

        public Pen PenViolet => new Pen(Color.Violet, 2);

        public Pen DashedPen => new Pen(Color.Violet, 1);

        public Image VertImage => Image.FromFile("./PaintVertOnly.png");

        public Image HorImage => Image.FromFile("./PaintHorOnly.png");

        public int DistToImage => 8;

        
        public LibraryDrawer(Bitmap canvas)
        {
            Canvas = canvas;
            DashedPen.DashPattern = [2, 2];
            G = Graphics.FromImage(Canvas);
        }


        public void Draw(MyLine line, Color color)
        {
            // I don't know why color aren't now visible
            Pen.Color = color;
            G.DrawLine(Pen, line.First.Center, line.Second.Center);
        }
    }
}
