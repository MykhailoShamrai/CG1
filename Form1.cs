using CG1.Drawers;
using CG1.Shapes;
using System.Windows.Forms.VisualStyles;

namespace CG1
{
    public partial class Form1 : Form
    {
        // Here is the place for all logic for main bitmap
        public Bitmap Bitmap { get; set; }
        internal MyPolygon Polygon { get; set; } = new MyPolygon();
        internal IDrawer Drawer { get; set; }
        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(this.pictureBoxMain.Width, this.pictureBoxMain.Height);
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
    }
}
