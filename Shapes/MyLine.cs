using CG1.ContextMenus;
using CG1.Drawers;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace CG1.Shapes
{
    public class MyLine : IElement
    {
        private int thicknes = 5;
        public double Len { get; set; }
        public MyPolygon ParentPolygon { get; set; }
        public MyPoint First { get; set; }
        public MyPoint Second { get; set; }
        public List<Triangle> BoundingBox { get; set; }
        public ContextMenuStrip Menu { get; set; } = new LineMenu();
        public ContextMenuStrip GetMenu()
        {
            return Menu;
        }

        public virtual void VisitDrawer(IDrawer drawer)
        {
            drawer.Draw(this, this.Color);
        }


        public Color Color { get; set; }

        public MyLine(MyPoint first, MyPoint second, Color color, MyPolygon polygon)
        {
            ParentPolygon = polygon;
            First = first;
            Second = second;
            Color = color;
            BoundingBox = new List<Triangle>();
            BoundingBox.Add(new Triangle());
            BoundingBox.Add(new Triangle());
            First.PropertyChanged += OnPointChanged;
            Second.PropertyChanged += OnPointChanged;
            CalcTheBoundingBox();

            Menu.Items[0].Click += polygon.AddVertex_Click;
            Menu.Items[1].Click += polygon.LenLock_Click;
            Menu.Items[2].Click += polygon.VertLock_Click;
            Menu.Items[3].Click += polygon.HorizontalLock_Click;
            Menu.Items[4].Click += polygon.AddBezier_Click;

            double dx = First.Center.X - Second.Center.X;
            double dy = First.Center.Y - Second.Center.Y;
            Len = (int)(Math.Sqrt(dx * dx + dy * dy));
        }


        public void ChangeFirstEnd(MyPoint point)
        {
            First.PropertyChanged -= OnPointChanged;
            First = point;
            First.PropertyChanged += OnPointChanged;
            CalcTheBoundingBox();
        }

        public void ChangeSecondEnd(MyPoint point)
        {
            Second.PropertyChanged -= OnPointChanged;
            Second = point;
            Second.PropertyChanged += OnPointChanged;
            CalcTheBoundingBox();
        }

        public static (double, double) FindPerpendicular(Point first, Point last)
        {
            double dx = last.X - first.X;
            double dy = last.Y - first.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length < 10e-6)
                length = 10e-6;
            double ux = dx / length;
            double uy = dy / length;
            double perpX = -uy;
            double perpY = ux;
            return (perpX, perpY);
        }

        public Point GetCenter()
        {
            int x_max = First.Center.X;
            int x_min = Second.Center.X;
            int y_max = First.Center.Y;
            int y_min = Second.Center.Y;
            (x_max, x_min) = x_min > x_max ? (x_min, x_max) : (x_max, x_min);
            (y_max, y_min) = y_min > y_max ? (y_min, y_max) : (y_max, y_min);
            Point newCoord = new Point(x_min + ((x_max - x_min) >> 1), y_min + ((y_max - y_min) >> 1));
            return newCoord;
        }

        protected virtual void CalcTheBoundingBox()
        {
            Point firstCenter = First.Center;
            Point secondCenter = Second.Center;
            (double perpX, double perpY) = FindPerpendicular(firstCenter, secondCenter);
            perpX = perpX * thicknes;
            perpY = perpY * thicknes;
            Point a = new Point((int)(firstCenter.X + perpX), (int)(firstCenter.Y + perpY));
            Point b = new Point((int)(firstCenter.X - perpX), (int)(firstCenter.Y - perpY));
            Point c = new Point((int)(secondCenter.X - perpX), (int)(secondCenter.Y - perpY));
            Point d = new Point((int)(secondCenter.X + perpX), (int)(secondCenter.Y + perpY));

            BoundingBox[0].ChangeVertices(a, b, c);
            BoundingBox[1].ChangeVertices(c, d, a);
        }

        protected virtual void OnPointChanged(object sender, PropertyChangedEventArgs e)
        {
            CalcTheBoundingBox();
            double dx = First.Center.X - Second.Center.X;
            double dy = First.Center.Y - Second.Center.Y;
            // Changing the Len for an edge. I have a problem, that errors of arithmetic are accumulated
            // and my points when I perform bezier control vertex dragging start flying away. With 
            // this condition it performs better
            if (Math.Abs(Len - Math.Sqrt(dx * dx + dy * dy)) > 1)
            {
                Len = Math.Sqrt(dx * dx + dy * dy);
            }
        }

        public bool CheckIfPointIsInsideBox(Point point)
        {
            bool res = BoundingBox[0].CheckIfPointIsInside(point) || BoundingBox[1].CheckIfPointIsInside(point);
            return res;
        }


        public virtual bool ModifyForConstraints(bool direction, MyPoint startVertex)
        {
            return false;
        }

        public virtual bool ModifyForBezier(bool direction, MyPoint startVertex)
        {
            return true;
        }

        public virtual void ChangeMenuWhileCreating(MyLine LeftLine, MyLine RightLine)
        {
        }

        public double ReturnLen()
        {
            return Len;
        }


        public static (double, double, double) LenBetweenTwoPoints(Point first, Point second)
        {
            double dx = second.X - first.X;
            double dy = second.Y - first.Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len < 10e-6)
                len = 10e-6;
            return (dx, dy, len);
        }
    }
}
