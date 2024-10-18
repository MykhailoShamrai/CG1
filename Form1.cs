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
        private Point _startPointForDrag = new Point(0, 0);
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
            MyLine.Menu.Items[0].Click += AddVertex_Click;
            MyPoint.Menu.Items[0].Click += DeleteVertex_Click;
            MyLine.Menu.Items[1].Click += LenLock_Click;
        }

        private void LenLock_Click(object? sender, EventArgs e)
        {
            Polygon.ChangeEdgeType(50);
        }

        private void DeleteVertex_Click(object? sender, EventArgs e)
        {
            Polygon.DeleteChosenPoint();
            ClearBitmap(Bitmap);
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Refresh();
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
                        IElement tmp = Polygon.CheckIfClickedInSomething(me.Location);
                        if (tmp != null)
                        {
                            if (Polygon.TypeOfChosen == MyPolygon.ChosenType.Vertex)
                            {
                                Polygon.Editing = true;
                            }
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
                        IElement chosen = Polygon.CheckIfClickedInSomething(me.Location);
                        if (chosen != null)
                        {
                            chosen.GetMenu().Show(pictureBoxMain, me.Location);
                        }
                    }
                    break;
                default:
                    break;
            }
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Refresh();
        }


        private void pictureBoxMain_MouseDown(object sender, MouseEventArgs me)
        {
            if (me.Button == MouseButtons.Left && Polygon.Valid)
            {
                if (!Polygon.Dragging && Polygon.CheckIfPointInsidePolygon(me.Location))
                {
                    Polygon.Dragging = true;
                    _startPointForDrag = me.Location;
                }
            }
        }
        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs me)
        {
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
                if (Polygon.CheckIfAnyElementIsChosen())
                {
                    ClearBitmap(Bitmap);
                    Polygon.DragVertex(me.Location);
                    Polygon.DrawPolygon(Bitmap);
                }
            }
            else if (Polygon.Dragging)
            {
                ClearBitmap(Bitmap);
                int dx = me.Location.X - _startPointForDrag.X;
                int dy = me.Location.Y - _startPointForDrag.Y;
                foreach (MyPoint point in Polygon.Points)
                {
                    point.Center = new Point(point.Center.X + dx, point.Center.Y + dy);
                }
                _startPointForDrag = me.Location;
                Polygon.DrawPolygon(Bitmap);
            }
            pictureBoxMain.Refresh();

        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (Polygon.Valid && Polygon.Dragging)
            {
                Polygon.Dragging = false;
            }
        }
    }
}
