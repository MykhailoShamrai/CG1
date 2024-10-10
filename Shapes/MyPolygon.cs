using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class MyPolygon
    {
        public int VertexRadius { get; set; }
        public bool Valid = false;
        public Color Color { get; set; }
        private IDrawer _drawer = null;
        public List<MyPoint> Points { get; set; }
        public List<MyLine> Lines { get; set; }
        
        public MyPolygon()
        {
            Points = new List<MyPoint>();
            Lines = new List<MyLine>();
            VertexRadius = 4;
            SetDrawer(new LibraryDrawer());
        }

        public void SetDrawer(IDrawer drawer)
        {
            _drawer = drawer; 
        }

        public void DrawPolygon(Bitmap bitmap)
        {
            foreach (MyPoint point in Points)
                _drawer.DrawCircle(point, point.Color, bitmap);
            foreach (MyLine line in Lines)
                _drawer.DrawLine(line, line.Color, bitmap);
        }
        public bool AddPoint(Point point)
        {
            // Firstly I must check, if I won't have intersection of points
            // I think, that intersection is a bit harder problem, then I want 
            // not to have possibility of intersection
            bool res = true;
            Point center = point;
            int x = center.X;
            int y = center.Y;
            int xx = 0;
            int yy = 0;
            MyPoint curp = new MyPoint(center, VertexRadius);
            if (Points.Count > 0)
            {
                // If click is in border of first vertex, we must end creating process.
                xx = Points[0].Center.X;
                yy = Points[0].Center.Y;
                if (Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) <= VertexRadius * VertexRadius)
                {
                    if (Points.Count > 2)
                    {
                        Valid = true;
                    }
                }
                else
                {
                    for (int i = 0; i < Points.Count; i++)
                    {
                        xx = Points[i].Center.X;
                        yy = Points[i].Center.Y;
                        if (Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) < 4 * VertexRadius * VertexRadius)
                        {
                            res = false;
                            break;
                        }
                    }
                }
            }
            if (Points.Count == 0)
            {
                Points.Add(curp);
            }
            else if (res)
            {
                if (Valid)
                {
                    Lines.Add(new MyLine(Points[^1], Points[0], Color.Black));
                }
                else
                {
                    Lines.Add(new MyLine(Points[^1], curp, Color.Black));
                    Points.Add(curp);
                }
            }
            return res;
        }
    }
}
