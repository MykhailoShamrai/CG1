using CG1.Drawers;
using CG1.Shapes;

namespace CG1
{
    public partial class Form1 : Form
    {
        private MyPoint[] tmpPoint;
        private Point _startPointForDrag = new Point(0, 0);
        public Bitmap Bitmap { get; set; }
        internal MyPolygon Polygon { get; set; }
        internal IDrawer LibDrawer { get; set; }
        internal IDrawer BrDrawer { get; set; }
        private int _bitmapSize = 2500;
        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(_bitmapSize, _bitmapSize);
            ClearBitmap(Bitmap);
            pictureBoxMain.Image = Bitmap;
            LibDrawer = new LibraryDrawer(Bitmap);
            BrDrawer = new BrezenhamDrawer(Bitmap);
            Polygon = new MyPolygon(this);
            Polygon.SetDrawer(LibDrawer);
            tmpPoint = [new MyPoint(new Point(0, 0), 4, Polygon), new MyPoint(new Point(0, 0), 4, Polygon)];


            Polygon.Points.Add(new MyPoint(new Point(321, 145), 6, Polygon));
            Polygon.Points.Add(new MyPoint(new Point(495, 69), 6, Polygon));
            Polygon.Points.Add(new MyPoint(new Point(704, 97), 6, Polygon));
            Polygon.Points.Add(new MyPoint(new Point(704, 387), 6, Polygon));
            Polygon.Points.Add(new MyPoint(new Point(324, 307), 6, Polygon));
            Polygon.Points.Add(new MyPoint(new Point(247, 307), 6, Polygon));
            Polygon.Lines.Add(new MyLine(Polygon.Points[0], Polygon.Points[1], Color.Black, Polygon));
            Polygon.Lines.Add(new MyLine(Polygon.Points[1], Polygon.Points[2], Color.Black, Polygon));
            Polygon.Lines.Add(new MyLine(Polygon.Points[2], Polygon.Points[3], Color.Black, Polygon));
            Polygon.Lines.Add(new MyLine(Polygon.Points[3], Polygon.Points[4], Color.Black, Polygon));
            Polygon.Lines.Add(new MyLine(Polygon.Points[4], Polygon.Points[5], Color.Black, Polygon));
            Polygon.Lines.Add(new MyLine(Polygon.Points[5], Polygon.Points[0], Color.Black, Polygon));

            Polygon.SelectedElement = Polygon.Lines[0];
            Polygon.ChangeEdgeType(MyLine.LenBetweenTwoPoints(Polygon.Points[0].Center, Polygon.Points[1].Center).Item3);
            Polygon.SelectedElement = Polygon.Lines[2];
            Polygon.ChangeEdgeType(-1);
            Polygon.SelectedElement = Polygon.Lines[3];
            Polygon.ChangeEdgeType(0);
            Polygon.SelectedElement = Polygon.Lines[4];
            Polygon.ChangeEdgeType(-2);
            Polygon.SelectedElement = null;
            Polygon.Valid = true;
            Polygon.DrawPolygon(Bitmap);
            Refresh();
        }




        public static void ClearBitmap(Bitmap bitmap)
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.WhiteSmoke);
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
                //Polygon.DrawPolygon(Bitmap, tmpPoint[0], tmpPoint[1]);
            }
            else if (Polygon.Editing)
            {
                if (Polygon.CheckIfAnyElementIsChosen())
                {
                    ClearBitmap(Bitmap);
                    Polygon.DragVertex(me.Location);
                    //Polygon.DrawPolygon(Bitmap);
                }
            }
            else if (Polygon.Dragging)
            {
                ClearBitmap(Bitmap);
                int dx = me.Location.X - _startPointForDrag.X;
                int dy = me.Location.Y - _startPointForDrag.Y;
                Polygon.DragPolygon(dx, dy);
                _startPointForDrag = me.Location;
                //Polygon.DrawPolygon(Bitmap);
            }
            ClearBitmap(Bitmap);
            Polygon.DrawPolygon(Bitmap);
            pictureBoxMain.Refresh();

        }

        private void pictureBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (Polygon.Valid && Polygon.Dragging)
            {
                Polygon.Dragging = false;
            }
        }

        private void BrezenheimBox_CheckedChanged(object sender, EventArgs e)
        {
            Polygon.SetDrawer(BrezenheimBox.Checked ? BrDrawer : LibDrawer);
            ClearBitmap(Bitmap);
            Polygon.DrawPolygon(Bitmap);
            Refresh();
        }
    }
}
