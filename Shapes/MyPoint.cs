using CG1.ContextMenus;
using CG1.Drawers;
using System.ComponentModel;

namespace CG1.Shapes
{
    public class MyPoint : IElement
    {
        private Point _center;
        public MyPolygon ParentPolygon { get; set; }
        public Point Center
        {
            get => _center; set
            {
                if (_center.X != value.X || _center.Y != value.Y)
                {
                    _center = value;
                    OnPropertyChanged(nameof(Center));
                }
            }
        }
        public int Radius { get; set; }


        public ContextMenuStrip Menu { get; set; } = new PointMenu();

        public Color Color { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ContextMenuStrip GetMenu()
        {
            return Menu;
        }

        public void VisitDrawer(IDrawer drawer)
        {
            drawer.Draw(this, this.Color);
        }

        public MyPoint(Point center, int radius, MyPolygon polygon)
        {
            ParentPolygon = polygon;
            Center = center;
            Radius = radius;
            Color = Color.Black;
            Menu.Items[0].Click += polygon.DeleteVertex_Click;
        }
    }
}
