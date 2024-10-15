using CG1.ContextMenus;
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
        private ContextMenuStrip contextMenuStripForVertex = new ContextMenuStrip();
        private ContextMenuStrip contextMenuStripForEdge = new LineMenu();

        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            ClearBitmap(Bitmap);
            pictureBoxMain.Image = Bitmap;
            Drawer = new LibraryDrawer();
            contextMenuStripForEdge.Items[0].Click += AddVertex_Click;
        }

        private void AddVertex_Click(object? sender, EventArgs e)
        {
            Polygon.AddPointInsideChosenEdge();
            ClearBitmap(Bitmap);
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Refresh();
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
            switch (me.Button)
            {
                case MouseButtons.Left:
                    if (!Polygon.Valid)
                    {
                        ClearBitmap(Bitmap);

                        Point point = me.Location;
                        Polygon.AddPoint(point);
                    }
                    else if (!Polygon.Editing)
                    {
                        Element tmp = Polygon.CheckIfClickedInSomething(me.Location);
                        if (Polygon.TypeOfChosen == MyPolygon.ChosenType.Vertex)
                        {
                            Polygon.Editing = true;
                        }
                    }
                    else if (Polygon.Editing)
                    {
                        Polygon.Editing = false;
                        Polygon.UnchooseElement();
                    }
                    break;
                case MouseButtons.Right:
                    if (!Polygon.Valid)
                    {
                        break;
                    }
                    else
                    {
                        Element chosen = Polygon.CheckIfClickedInSomething(me.Location);
                        if (chosen != null)
                        {
                            switch (Polygon.TypeOfChosen)
                            {
                                case MyPolygon.ChosenType.None:
                                    break;
                                case MyPolygon.ChosenType.Vertex:
                                    contextMenuStripForVertex.Show(me.Location);
                                    break;
                                case MyPolygon.ChosenType.Edge:
                                    contextMenuStripForEdge.Show(pictureBoxMain, me.Location);
                                    break;
                                case MyPolygon.ChosenType.Bezier:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Refresh();
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = e;
            // If I want this work, I should change bitmaps 
            if (!Polygon.Valid)
            {
                ClearBitmap(Bitmap);
                //pictureBoxMain.Refresh();
                Point point = me.Location;
                tmpPoint[1].Center = point;
                tmpPoint[0] = (Polygon.Points.Count == 0) ? null : Polygon.Points.Last.Value;
                Polygon.DrawPolygon(Bitmap, tmpPoint[0], tmpPoint[1]);
            }
            else if (Polygon.Editing)
            {
                if (Polygon.CheckIfAnyElementIsChosen())
                {
                    ClearBitmap(Bitmap);
                    Polygon.DragVertex(me.Location);
                    Polygon.DrawPolygon(Bitmap);
                }
            }
            pictureBoxMain.Refresh();
            
        }
    }
}
