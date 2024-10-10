using CG1.Drawers;
using CG1.Shapes;
using System.Windows.Forms.VisualStyles;

namespace CG1
{
    public partial class Form1 : Form
    {
        // Here is the place for all logic for main bitmap
        // I must remove this after
        private MyPoint[] tmpPoint = [new MyPoint(new Point(0, 0), 4), new MyPoint(new Point(0, 0), 4)];
        public Bitmap Bitmap { get; set; }
        public Bitmap LastBitmap { get; set; }
        internal MyPolygon Polygon { get; set; } = new MyPolygon();
        internal IDrawer Drawer { get; set; }
        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(this.pictureBoxMain.Width, this.pictureBoxMain.Height);
            LastBitmap = new Bitmap(this.pictureBoxMain.Width, this.pictureBoxMain.Height);
            pictureBoxMain.Image = Bitmap;
            Drawer = new LibraryDrawer();
        }

        private void pictureBoxMain_Click(object sender, EventArgs e)
        {
            // Here I must add evaluation if creating mode is ON
            if (Polygon.Valid)
            {
                return;
            }
            // Process of polygon creating
            // Now only points 
            MouseEventArgs me = (MouseEventArgs)e;
            Point point = me.Location;
            Polygon.AddPoint(point);
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Image = Bitmap;
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            // If I want this work, I should change bitmaps 
            if (!Polygon.Valid)
            {
                LastBitmap = Bitmap;
                MouseEventArgs me = e;
                Point point = me.Location;
                tmpPoint[1].Center = point;
                tmpPoint[0] = (Polygon.Points.Count == 0) ? null : Polygon.Points[^1];
                Bitmap = Polygon.DrawPolygon(LastBitmap, tmpPoint[0], tmpPoint[1]);
                pictureBoxMain.Image = LastBitmap;
            }
        }
    }
}
