using CG1.ContextMenus;
using CG1.Drawers;
using CG1.Shapes;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms.VisualStyles;

namespace CG1
{
    public partial class Form1 : Form
    {
        private MyPoint[] tmpPoint;
        private Point _startPointForDrag = new Point(0, 0);
        public Bitmap Bitmap { get; set; }
        internal MyPolygon Polygon { get; set; }
        internal IDrawer Drawer { get; set; }

        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            ClearBitmap(Bitmap);
            pictureBoxMain.Image = Bitmap;
            Drawer = new LibraryDrawer();
            Polygon = new MyPolygon(this);
            tmpPoint = [new MyPoint(new Point(0, 0), 4, Polygon), new MyPoint(new Point(0, 0), 4, Polygon)];
        }




        public static void ClearBitmap(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
            }
        }
        private void pictureBoxMain_Click(object sender, EventArgs e)
        {
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
                        // May be I should assign event handlers here
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
            ClearBitmap(Bitmap);
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Refresh();
        }

        private void Form1_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
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
