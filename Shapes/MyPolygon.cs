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
        // I must create some better solution for having tmp new line
        public int VertexRadius { get; set; }
        public bool Valid { get; set; } = false;
        public bool Editing { get; set; } = false;
        private MyPoint? _chosenVertex = null;
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

        private MyPoint? CheckIfVertexIsOnLegalPosition(MyPoint point)
        {
            MyPoint? res = null;
            int x = point.Center.X;
            int y = point.Center.Y;
            int xx = 0;
            int yy = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                xx = Points[i].Center.X;
                yy = Points[i].Center.Y;
                if (Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) < 4 * VertexRadius * point.Radius)
                {
                    res = Points[i];
                    break;
                }
            }
            return res;
        }

        public bool CheckIfClickedInVertex(Point point)
        {
            _chosenVertex = CheckIfVertexIsOnLegalPosition(new MyPoint(point, VertexRadius / 4));
            if (_chosenVertex is not null)
            {
                _chosenVertex.Color = Color.Green;
            }
            return _chosenVertex is null;
        }

        /// <summary>
        /// Method that changes position of chosen vertex, but only if ths vertex is chosen and 
        /// there is no colistion with other vertices
        /// </summary>
        /// <param name="vertexTmp">Postion to which chosen vertex must go</param>
        public void DragVertex(Point vertexTmp)
        {
            if (_chosenVertex is not null && CheckIfVertexIsOnLegalPosition(new MyPoint(vertexTmp, VertexRadius)) is null)
            {
                _chosenVertex!.Center = vertexTmp;
            }
        }
        public void SetDrawer(IDrawer drawer)
        {
            _drawer = drawer; 
        }

        public bool CheckIfAnyVertexIsChosen()
        {
            return _chosenVertex is not null;
        }

        public void DrawPolygon(Bitmap bitmap)
        {
            foreach (MyPoint point in Points)
                _drawer.DrawCircle(point, point.Color, bitmap);
            foreach (MyLine line in Lines)
                _drawer.DrawLine(line, line.Color, bitmap);
        }

        public void DrawPolygon(Bitmap bitmap, MyPoint lastPoint, MyPoint tmp)
        {
            DrawPolygon(bitmap);
            // I must find better soluton, because creating a new line for each time is memory consuming
            _drawer.DrawCircle(tmp, Color.Green, bitmap);
            if (lastPoint != null)
            {
                _drawer.DrawLine(new MyLine(lastPoint, tmp, Color.Green), Color.Green, bitmap);
            }
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
                        Editing = false;
                    }
                }
                else
                {
                    // This part of code can be placed as another function, because it may be important in other part of code
                    res = CheckIfVertexIsOnLegalPosition(curp) is null;
                }
            }
            if (Points.Count == 0)
            {
                Editing = true;
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
