using CG1.ContextMenus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class MyPoint: IElement
    {
        private Point _center;
        public Point Center { get => _center; set 
            {
                if (_center.X != value.X || _center.Y != value.Y)
                {
                    _center = value;
                    OnPropertyChanged(nameof(Center));
                }
            }}
        public int Radius { get; set; }

        public static ContextMenuStrip Menu { get; } = new PointMenu();

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

        public MyPoint(Point center, int radius)
        {
            Center = center;
            Radius = radius;
            Color = Color.Black;
        }
    }
}
