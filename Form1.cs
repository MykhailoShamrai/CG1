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
        internal MyPolygon Polygon { get; set; } = new MyPolygon();
        internal IDrawer Drawer { get; set; }
        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            ClearBitmap(Bitmap);
            pictureBoxMain.Image = Bitmap;
            Drawer = new LibraryDrawer();
        }


        private static void ClearBitmap(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
            }
        }
        private void pictureBoxMain_Click(object sender, EventArgs e)
        {
            // Here I must add evaluation if creating mode is ON
            MouseEventArgs me = (MouseEventArgs)e;
            if (!Polygon.Valid)
            {
                ClearBitmap(Bitmap);

                Point point = me.Location;
                Polygon.AddPoint(point);
            }
            else if (!Polygon.Editing && Polygon.CheckIfClickedInVertex(me.Location))
            {
                Polygon.Editing = true;
            }
            else if (Polygon.Editing)
            {
                Polygon.Editing = false;
                Polygon.UnchooseVertex();
            }
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Image = Bitmap;
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = e;
            // If I want this work, I should change bitmaps 
            if (!Polygon.Valid)
            {
                ClearBitmap(Bitmap);
                Point point = me.Location;
                tmpPoint[1].Center = point;
                tmpPoint[0] = (Polygon.Points.Count == 0) ? null : Polygon.Points[^1];
                Polygon.DrawPolygon(Bitmap, tmpPoint[0], tmpPoint[1]);
            }
            else if (Polygon.Editing)
            {
                if (Polygon.CheckIfAnyVertexIsChosen())
                {
                    ClearBitmap(Bitmap);
                    Polygon.DragVertex(me.Location);
                    Polygon.DrawPolygon(Bitmap);
                }
            }
            pictureBoxMain.Image = Bitmap;
        }
    }
}
