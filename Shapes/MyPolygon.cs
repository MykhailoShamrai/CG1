using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.DirectoryServices.ActiveDirectory;
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
        private Form1 _form1;
        private FormForLen _formForLen = new FormForLen();
        public bool Dragging { get; set; }
        public int VertexRadius { get; set; }
        public bool Valid { get; set; } = false;
        public bool Editing { get; set; } = false;
        private IElement? _chosenElement = null;
        public Color Color { get; set; }
        private IDrawer _drawer = null;
        public List<MyPoint> Points { get; set; }
        public List<MyLine> Lines { get; set; }
        public enum ChosenType
        {
            None,
            Vertex,
            Edge,
            Bezier
        }

        public ChosenType TypeOfChosen { get; set; } = ChosenType.None;

        public MyPolygon(Form1 owner)
        {
            _form1 = owner;
            Points = new List<MyPoint>();
            Lines = new List<MyLine>();
            VertexRadius = 6;
            SetDrawer(new LibraryDrawer());
        }

        public bool CheckIfPointInsidePolygon(Point point)
        {
            // Vertical line
            Point x1 = point;
            Point x2 = new Point(point.X, 0);

            int counter = 0;
            for (int i = 0; i < Lines.Count; i++)
            {
                MyLine line = Lines[i];
                int tmp = IElement.CheckIfTwoLinesIntersect(x1, x2, line.First.Center, line.Second.Center);
                if (tmp == 0)
                {
                    //line.Color = Color.Red;
                    counter++;
                }
                else if (tmp == 1)
                {
                    MyLine prevLine = i == 0 ? Lines[^1] : Lines[i - 1];
                    MyLine nextLine = i == Lines.Count - 1 ? Lines[0] : Lines[i + 1];
                    // Prosta i odcinek sa wspolliniowe
                    if (IElement.Cross(x2, line.First.Center, x1) == 0 &&
                        IElement.Cross(x2, line.Second.Center, x1) == 0)
                    {
                        // Szukam elementu listy, sprawdzam dla poprzedniej i nastepnej krawedz
                        if (prevLine.First.Center.X < point.X &&
                            nextLine.Second.Center.X > point.X)
                            counter++;
                        else
                        {
                            counter += 2;
                        }
                    }
                    // Wierzcholek przecina nasza krawedz. Musimy sprawdzic krawedzie co sasiaduja
                    else
                    {
                        int tmp1 = prevLine.First.Center.X;
                        int tmp2 = line.Second.Center.X;
                        // If second end of our line is colinear with 
                        if (IElement.Cross(x2, line.Second.Center, x1) == 0)
                        {
                            tmp1 = line.First.Center.X;
                            tmp2 = nextLine.Second.Center.X;
                        }
                        if ((tmp1 - point.X) *
                        (tmp2 - point.X) < 0)
                            counter++;
                        else
                        {
                            counter += 2;
                        }
                    }
                }
            }
            return ((counter & 1) == 1);
        }

        public void DeleteChosenPoint()
        {
            if (_chosenElement != null)
            {
                MyPoint point = (MyPoint)_chosenElement;
                if (Points[0].Equals(point))
                {
                    Points.RemoveAt(0);
                    Lines.RemoveAt(0);
                    //Lines.RemoveAt(Lines.Count - 1);
                    //Lines[^1].ChangeSecondEnd(Points[0]);
                    Lines[^1] = new MyLine(Lines[^1].First, Points[0], Color.Black, this);
                }
                else if (Points[^1].Equals(point))
                {
                    Points.RemoveAt(Points.Count - 1);
                    Lines.RemoveAt(Lines.Count - 1);
                    //Lines[^1].ChangeSecondEnd(Points[0]);
                    Lines[^1] = new MyLine(Points[^1], Points[0], Color.Black, this);
                    //Lines[^2] = new MyLine(Points[^2], Points[^1], Color.Black, this);
                }
                else
                {
                    int index = Points.IndexOf(point);
                    MyLine lineTmp = Lines[index];
                    //MyPoint pointTmp = Points[index + 1];
                    //Lines[index - 1].ChangeSecondEnd(pointTmp);
                    Lines.RemoveAt(index);
                    Points.RemoveAt(index);
                    Lines[index - 1] = new MyLine(Points[index - 1], Points[index], Color.Black, this);

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
                Point newCoord = new Point(x_min + ((x_max - x_min) >> 1), y_min + ((y_max - y_min) >> 1));



                MyPoint tmpPoint = new MyPoint(newCoord, VertexRadius, this);
                if (CheckIfVertexIsOnLegalPosition(tmpPoint) is null)
                {
                    int index = Lines.IndexOf(line);
                    Lines.Insert(index + 1, new MyLine(tmpPoint, line.Second, Color.Black, this));
                    Points.Insert(index + 1, tmpPoint);
                    Lines[index] = new MyLine(Lines[index].First, tmpPoint, Color.Black, this);
                }
            }
        }

        public IElement? CheckIfClickedInSomething(Point pos)
        {
            IElement? element = null;

            element = CheckIfVertexIsOnLegalPosition(new MyPoint(pos, VertexRadius / 4, this));

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
                tmp = Points[i];
                xx = tmp.Center.X;
                yy = tmp.Center.Y;
                if (!(_chosenElement is MyPoint && Points[i].Equals((MyPoint)_chosenElement)) && Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) < 4 * VertexRadius * point.Radius)
                {
                    res = tmp;
                    break;
                }
            }
            return res;
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
                && CheckIfVertexIsOnLegalPosition(new MyPoint(vertexTmp, VertexRadius, this)) is null)
            {
                MyPoint draggedVertex = (MyPoint)_chosenElement;
                draggedVertex.Center = vertexTmp;
                int index = Points.IndexOf(draggedVertex);

                bool leftCont = true;
                bool rightCont = true;

                int counter = 0;
                while ((leftCont || rightCont))
                {
                    int indexLeft = index - counter - 1 >= 0 ? index - counter - 1 : Lines.Count + index - counter - 1;
                    int indexRight = (index + counter) % Lines.Count;
                    if (leftCont)
                        leftCont = Lines[indexLeft].ModifyForConstraints(false, draggedVertex);
                    if (rightCont)
                        rightCont = Lines[indexRight].ModifyForConstraints(true, draggedVertex);
                    counter++;
                }
                leftCont = true;
                rightCont = true;
                counter = 0;
                while ((leftCont || rightCont))
                {
                    int indexLeft = index - counter - 1 >= 0 ? index - counter - 1 : Lines.Count + index - counter - 1;
                    int indexRight = (index + counter) % Lines.Count;
                    if (rightCont)
                        rightCont = Lines[indexRight].ModifyForConstraints(true, draggedVertex);
                    if (leftCont)
                        leftCont = Lines[indexLeft].ModifyForConstraints(false, draggedVertex);
                
                    counter++;
                }
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
                _drawer.Draw(new MyLine(lastPoint, tmp, Color.Green, this), Color.Green, bitmap);
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
            MyPoint curp = new MyPoint(center, VertexRadius, this);
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
                Points.Add(curp);
            }
            else if (res)
            {
                if (Valid)
                {
                    Lines.Add(new MyLine(Points[^1], Points[0], Color.Black, this));
                }
                else
                {
                    Lines.Add(new MyLine(Points[^1], curp, Color.Black, this));
                    Points.Add(curp);
                }
            }
            return res;
        }


        // It's possible to merge this function 
        public void ChangeEdgeType(double length)
        {
            int index = Lines.IndexOf((MyLine)_chosenElement);
            MyLine tmpLine = Lines[index];
            Lines[index] = new MyLenghtLine(tmpLine, length);
            int indexLeft = index - 1 >= 0 ? index - 1 : Lines.Count + index - 1;
            int indexRight = (index + 1) % Lines.Count;
            Lines[index].ChangeMenuWhileCreating(Lines[indexLeft], Lines[indexRight]);
            _chosenElement = Lines[index].First;
            TypeOfChosen = ChosenType.Vertex;
            DragVertex(Lines[index].First.Center);
            DragVertex(Lines[index].Second.Center);
            _chosenElement = null;
        }

        // true means vertical, false means horizontal
        public void ChangeEdgeType(bool vertOrHorizontal)
        {
            int index = Lines.IndexOf((MyLine)_chosenElement);
            MyLine tmpLine = Lines[index];
            Lines[index] = vertOrHorizontal ? new MyVerticalLine(tmpLine, true) : new MyVerticalLine(tmpLine, false);
            int indexLeft = index - 1 >= 0 ? index - 1 : Lines.Count + index - 1;
            int indexRight = (index + 1) % Lines.Count;
            Lines[index].ChangeMenuWhileCreating(Lines[indexLeft], Lines[indexRight]);
            _chosenElement = Lines[index].First;
            TypeOfChosen = ChosenType.Vertex;
            DragVertex(Lines[index].First.Center);
            DragVertex(Lines[index].Second.Center);
            _chosenElement = null;
        }


        // It shouldn't look like that
        public void HorizontalLock_Click(object? sender, EventArgs e)
        {
            ChangeEdgeType(false);
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }

        public void VertLock_Click(object? sender, EventArgs e)
        {
            ChangeEdgeType(true);
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }

        public void LenLock_Click(object? sender, EventArgs e)
        {
            // here must be a massage box to write a length
            MyLine line = (MyLine)_chosenElement;
            _formForLen.SetToTextBox(line!.ReturnLen());
            _formForLen.ShowDialog();
            ChangeEdgeType(_formForLen.Len);
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }

        public void DeleteVertex_Click(object? sender, EventArgs e)
        { 
            DeleteChosenPoint();
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }

        public void AddVertex_Click(object? sender, EventArgs e)
        {
            AddPointInsideChosenEdge();
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }

    }
}
