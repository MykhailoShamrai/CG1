using CG1.Drawers;
using System;

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
        public List<BezierControlVertex> BezierPoints { get; set; }
        public IElement? SelectedElement { get { return _chosenElement; } set { _chosenElement = value; } }
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
            BezierPoints = new List<BezierControlVertex>();
            VertexRadius = 6;
            SetDrawer(new LibraryDrawer());
        }

        public void DragPolygon(int dx, int dy)
        {
            foreach (MyPoint myPoint in Points)
            {
                myPoint.Center = new Point(myPoint.Center.X + dx, myPoint.Center.Y + dy);
            }
            foreach (BezierControlVertex bezierControlVertex in BezierPoints)
            {
                bezierControlVertex.Center = new Point(bezierControlVertex.Center.X + dx, bezierControlVertex.Center.Y + dy);
            }
                
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

                Point newCoord = line.GetCenter();

                MyPoint tmpPoint = new MyPoint(newCoord, VertexRadius, this);
                if (CheckIfVertexIsOnLegalPosition(tmpPoint.Center) is null)
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

            // Firstly check if clicked in any bezier control vertex
            element = CheckIfClickedInControlVertex(pos);
            if (element is null)
                element = CheckIfVertexIsOnLegalPosition(pos);

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

        private MyPoint? CheckIfClickedInControlVertex(Point point)
        {
            MyPoint? res = null;
            int x = point.X;
            int y = point.Y;
            int xx = 0;
            int yy = 0;
            MyPoint tmp;
            for (int i = 0; i < BezierPoints.Count; i++)
            {
                tmp = BezierPoints[i];
                xx = tmp.Center.X;
                yy = tmp.Center.Y;
                if (!(_chosenElement is MyPoint && BezierPoints[i].Equals((MyPoint)_chosenElement)) && Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) < 4 * VertexRadius * VertexRadius)
                {
                    res = tmp;
                    break;
                }
            }
            return res;
        }

        private MyPoint? CheckIfVertexIsOnLegalPosition(Point point)
        {
            MyPoint? res = null;
            int x = point.X;
            int y = point.Y;
            int xx = 0;
            int yy = 0;
            MyPoint tmp;
            for (int i = 0; i < Points.Count; i++)
            {
                tmp = Points[i];
                xx = tmp.Center.X;
                yy = tmp.Center.Y;
                if (!(_chosenElement is MyPoint && Points[i].Equals((MyPoint)_chosenElement)) && Math.Abs((x - xx) * (x - xx) + (y - yy) * (y - yy)) < 4 * VertexRadius * VertexRadius)
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
        public void DragVertex(Point vertexTmp, bool left = true, bool right = true)
        {
            if (_chosenElement is not null && TypeOfChosen == ChosenType.Vertex
                && CheckIfVertexIsOnLegalPosition(vertexTmp) is null)
            {
                MyPoint draggedVertex = (MyPoint)_chosenElement;
                draggedVertex.Center = vertexTmp;


                // If dragged vertex is not a bezier control point
                if (!(draggedVertex is BezierControlVertex))
                {
                    int index = Points.IndexOf(draggedVertex);
                    bool leftCont = true;
                    bool rightCont = true;
                    bool leftBez = true;
                    bool rightBez = true;

                    int counter = 0;
                    while (counter < Lines.Count)
                    {
                        int indexLeft = index - counter - 1 >= 0 ? index - counter - 1 : Lines.Count + index - counter - 1;
                        int indexRight = (index + counter) % Lines.Count;
                        if (left && leftCont)
                            leftCont = Lines[indexLeft].ModifyForConstraints(false, draggedVertex);
                        if (left && leftBez)
                            leftBez = Lines[indexLeft].ModifyForBezier(false, draggedVertex);
                        if (right && rightCont)
                            rightCont = Lines[indexRight].ModifyForConstraints(true, draggedVertex);
                        if (right && rightBez)
                            rightBez = Lines[indexRight]. ModifyForBezier(true, draggedVertex);
                        counter++;
                    }
                    leftCont = true;
                    rightCont = true;
                    leftBez = true;
                    rightBez = true;
                    counter = 0;
                    while (counter < Lines.Count)
                    {
                        int indexLeft = index - counter - 1 >= 0 ? index - counter - 1 : Lines.Count + index - counter - 1;
                        int indexRight = (index + counter) % Lines.Count;
                        if (right && rightCont)
                            rightCont = Lines[indexRight].ModifyForConstraints(true, draggedVertex);
                        if (left && leftCont)
                            leftCont = Lines[indexLeft].ModifyForConstraints(false, draggedVertex);
                        if (left && leftBez)
                            leftBez = Lines[indexLeft].ModifyForBezier(false, draggedVertex);
                        if (right && rightBez)
                            rightCont = Lines[indexRight].ModifyForBezier(true, draggedVertex);
                       
                        counter++;
                    }
                }
                else
                {
                    ((BezierControlVertex)_chosenElement).ModificateWhileDragging();
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
                point.VisitDrawer(_drawer);
            foreach (MyLine line in Lines)
                line.VisitDrawer(_drawer);
            foreach (MyPoint point in BezierPoints)
                point.VisitDrawer(_drawer);
        }

        public void DrawPolygon(Bitmap bitmap, MyPoint lastPoint, MyPoint tmp)
        {
            DrawPolygon(bitmap);
            // I must find better soluton, because creating a new line for each time is memory consuming
            tmp.VisitDrawer(_drawer);
            if (lastPoint != null)
            {
                MyLine tmpLine = new MyLine(lastPoint, tmp, Color.Green, this);
                tmpLine.VisitDrawer(_drawer);
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
                    res = CheckIfVertexIsOnLegalPosition(curp.Center) is null;
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
            if (length > 0)
                Lines[index] = new MyLenghtLine(tmpLine, length);
            else if (length == 0)
            {
                int leftInd = index - 1 >= 0 ? index - 1 : Lines.Count + index - 1;
                int rightInd = (index + 1) % Points.Count;
                BezierVertex tmp2 = new BezierVertex(Points[rightInd].Center,
                    Points[rightInd].Radius, this);
                Points[rightInd] = tmp2;
                BezierVertex tmp1 = new BezierVertex(Points[index].Center, Points[index].Radius, this);
                Points[index] = tmp1;
                Lines[leftInd].ChangeSecondEnd(tmp1);
                Lines[rightInd].ChangeFirstEnd(tmp2);
                Lines[index] = new MyBezier(tmp1, tmp2, Color.Black, this, Lines[leftInd].First, Lines[rightInd].Second);
                MyBezier tmpThis = (MyBezier)Lines[index];
                if (Lines[leftInd] is MyBezier)
                {
                    tmpThis.LeftPrev = ((MyBezier)Lines[leftInd]).SecondControlVertex;
                    ((MyBezier)Lines[leftInd]).RightNext = tmpThis.FirstControlVertex;
                }
                if (Lines[rightInd] is MyBezier)
                {
                    tmpThis.RightNext = ((MyBezier)Lines[rightInd]).FirstControlVertex;
                    ((MyBezier)Lines[rightInd]).LeftPrev = tmpThis.SecondControlVertex;
                }
            }
            else if (length == -1)
            {
                Lines[index] = new MyVerticalLine(tmpLine, true);
            }
            else
            {
                Lines[index] = new MyVerticalLine(tmpLine, false);
            }
            int indexLeft = index - 1 >= 0 ? index - 1 : Lines.Count + index - 1;
            int indexRight = (index + 1) % Lines.Count;
            Lines[index].ChangeMenuWhileCreating(Lines[indexLeft], Lines[indexRight]);
            _chosenElement = Lines[index].First;
            TypeOfChosen = ChosenType.Vertex;
            DragVertex(Lines[index].First.Center);
            _chosenElement = Lines[index].Second;
            DragVertex(Lines[index].Second.Center);
            _chosenElement = null;
        }

        // It shouldn't look like that
        public void HorizontalLock_Click(object? sender, EventArgs e)
        {
            ChangeEdgeType(-2);
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }

        public void VertLock_Click(object? sender, EventArgs e)
        {
            ChangeEdgeType(-1);
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


        public void AddBezier_Click(object? sender, EventArgs e)
        {
            ChangeEdgeType(0);
            Form1.ClearBitmap(_form1.Bitmap);
            DrawPolygon(_form1.Bitmap);
            _form1.Refresh();
        }
    }
}
