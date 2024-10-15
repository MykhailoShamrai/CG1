using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace CG1.Shapes
{
    public class MyPolygon
    {
        public int VertexRadius { get; set; }
        public bool Valid { get; set; } = false;
        public bool Editing { get; set; } = false;
        private IElement? _chosenElement = null;
        public Color Color { get; set; }
        private IDrawer _drawer = null;
        public LinkedList<MyPoint> Points { get; set; }
        public LinkedList<MyLine> Lines { get; set; }
        public enum ChosenType
        {
            None,
            Vertex,
            Edge,
            Bezier
        }
        public ChosenType TypeOfChosen { get; set; } = ChosenType.None;
        
        public MyPolygon()
        {
            Points = new LinkedList<MyPoint>();
            Lines = new LinkedList<MyLine>();
            VertexRadius = 6;
            SetDrawer(new LibraryDrawer());
        }

        public void DeleteChosenPoint()
        {
            if (_chosenElement != null)
            {
                MyPoint point = (MyPoint)_chosenElement;
                LinkedListNode<MyPoint> nodeRef = Points.Find(point)!;
                if (Points.First!.Equals(nodeRef))
                {
                    Points.RemoveFirst();
                    Lines.RemoveFirst();
                    Lines.Last.Value.ChangeSecondEnd(Points.First.Value);
                }
                else if (Points.Last!.Equals(nodeRef))
                {
                    Points.RemoveLast();
                    Lines.RemoveLast();
                    Lines.Last.Value.ChangeSecondEnd(Points.First.Value);
                }
                else
                {
                    LinkedListNode<MyPoint>? pointTmp = Points.First;
                    LinkedListNode<MyLine>? lineTmp = Lines.First;
                    for (int i = 0; i < Points.Count; i++)
                    {
                        if (pointTmp.Equals(nodeRef))
                        {
                            break;
                        }
                        pointTmp = pointTmp.Next;
                        lineTmp = lineTmp.Next;
                    }
                    lineTmp.Previous.Value.ChangeSecondEnd(pointTmp.Next.Value);
                    Lines.Remove(lineTmp);
                    Points.Remove(pointTmp);
                }
                if (Points.Count < 3)
                {
                    Points.Clear();
                    Lines.Clear();
                    Valid = false;
                }
            }
        }

        public void AddPointInsideChosenEdge()
        {
            if (_chosenElement != null)
            {
                MyLine line = (MyLine)(_chosenElement!);
                int x_max = line.First.Center.X;
                int x_min = line.Second.Center.X;
                int y_max = line.First.Center.Y;
                int y_min = line.Second.Center.Y;
                (x_max, x_min) = x_min > x_max ? (x_min, x_max) : (x_max, x_min);
                (y_max, y_min) = y_min > y_max ? (y_min, y_max) : (y_max, y_min);
                Point newCoord = new Point(x_min + ((x_max - x_min) >> 1), y_min +((y_max - y_min) >> 1));



                MyPoint tmpPoint = new MyPoint(newCoord, VertexRadius);
                if (CheckIfVertexIsOnLegalPosition(tmpPoint) is null)
                {
                    LinkedListNode<MyLine> lineNode = Lines.Find(line)!;
                    Lines.AddAfter(lineNode, new MyLine(tmpPoint, line.Second, Color.Black));
                    line.ChangeSecondEnd(tmpPoint);
                    LinkedListNode<MyPoint> pointNode = Points.Find(line.First)!;
                    Points.AddAfter(pointNode, tmpPoint);
                }
            }
        }

        public IElement? CheckIfClickedInSomething(Point pos)
        {
            IElement? element = null;
            //foreach (MyPoint point in Points)
            
            element = CheckIfVertexIsOnLegalPosition(new MyPoint(pos, VertexRadius / 4));
            //if (element != null)
            //break;

            if (element is null)
            {
                foreach (MyLine line in Lines)
                {
                    element = line.CheckIfPointIsInsideBox(pos) ? line : null;
                    if (element != null)
                    {
                        TypeOfChosen = ChosenType.Edge;
                        break;
                    }
                }
            }
            else
            {
                TypeOfChosen = ChosenType.Vertex;
            }
            if (_chosenElement is not null)
            {
                _chosenElement.Color = Color.Black;
            }
            _chosenElement = element;
            if (_chosenElement is not null)
            {
                _chosenElement.Color = Color.Red;
            }

            return element;
        }

        private MyPoint? CheckIfVertexIsOnLegalPosition(MyPoint point)
        {
            MyPoint? res = null;
            int x = point.Center.X;
            int y = point.Center.Y;
            int xx = 0;
            int yy = 0;
            MyPoint tmp;
            for (int i = 0; i < Points.Count; i++)
            {
                tmp = Points.ElementAt(i);
                xx = tmp.Center.X;
                yy = tmp.Center.Y;
                if (Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) < 4 * VertexRadius * point.Radius)
                {
                    res = tmp;
                    break;
                }
            }
            return res;
        }

        // I should implement choosing an edge. For this I want rename function and reimplement 
        // chicking if i click in something. 
        public bool CheckIfClickedInVertex(Point point)
        {
            // Maybe I should change it and devide into two functions
            //if (_chosenElement != null)
            //    _chosenElement.Color = Color.Black;
            MyPoint? tmp = CheckIfVertexIsOnLegalPosition(new MyPoint(point, VertexRadius / 4));
            return tmp is not null;
        }

        public void UnchooseElement()
        {
            if (_chosenElement != null)
                _chosenElement.Color = Color.Black;
            _chosenElement = null;
        }

        /// <summary>
        /// Method that changes position of chosen vertex, but only if ths vertex is chosen and 
        /// there is no colistion with other vertices
        /// </summary>
        /// <param name="vertexTmp">Postion to which chosen vertex must go</param>
        public void DragVertex(Point vertexTmp)
        {
            if (_chosenElement is not null && TypeOfChosen == ChosenType.Vertex
                && CheckIfVertexIsOnLegalPosition(new MyPoint(vertexTmp, VertexRadius)) is null)
            {
                (_chosenElement as MyPoint)!.Center = vertexTmp;
            }
        }
        public void SetDrawer(IDrawer drawer)
        {
            _drawer = drawer; 
        }

        public bool CheckIfAnyElementIsChosen()
        {
            return _chosenElement is not null;
        }

        public void DrawPolygon(Bitmap bitmap)
        {
            foreach (MyPoint point in Points)
                _drawer.Draw(point, point.Color, bitmap);
            foreach (MyLine line in Lines)
                _drawer.Draw(line, line.Color, bitmap);
        }

        public void DrawPolygon(Bitmap bitmap, MyPoint lastPoint, MyPoint tmp)
        {
            DrawPolygon(bitmap);
            // I must find better soluton, because creating a new line for each time is memory consuming
            _drawer.Draw(tmp, Color.Green, bitmap);
            if (lastPoint != null)
            {
                _drawer.Draw(new MyLine(lastPoint, tmp, Color.Green), Color.Green, bitmap);
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
                xx = Points.ElementAt(0).Center.X;
                yy = Points.ElementAt(0).Center.Y;
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
                Points.AddLast(curp);
            }
            else if (res)
            {
                if (Valid)
                {
                    Lines.AddLast(new MyLine(Points.Last!.Value, Points.First!.Value, Color.Black));
                }
                else
                {
                    Lines.AddLast(new MyLine(Points.Last!.Value, curp, Color.Black));
                    Points.AddLast(curp);
                }
            }
            return res;
        }
    }
}
