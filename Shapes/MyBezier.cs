﻿using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace CG1.Shapes
{
    public class MyBezier : MyLine
    {
        private int _startDist = 30;
        public double Shift { get; set; }
        private Vector2 _A = new();
        private Vector2 _B = new();
        private Vector2 _C = new();
        private Vector2 _D = new();
        public Vector2 A { get { return _A; } }
        public Vector2 B { get { return _B; } }
        public Vector2 C { get { return _C; } }
        public Vector2 D { get { return _D; } }
        public BezierControlVertex FirstControlVertex { get; set; }
        public BezierControlVertex SecondControlVertex { get; set; }

        public double LineFromFirstToFirstControl { get; set; }
        public double LineFromSecondControlToSecond { get; set; }
        public MyPoint LeftPrev { get; set; }
        public MyPoint RightNext { get; set; }
        public MyBezier(MyPoint first, MyPoint second, Color color, MyPolygon polygon) : base(first, second, color, polygon)
        {
            (double ux, double uy) = FindPerpendicular(this.First.Center, this.Second.Center);
            double len = ReturnLen();
            double dx = First.Center.X - Second.Center.X;
            double dy = First.Center.Y - Second.Center.Y;
            double dxu = dx / len;
            double dyu = dy / len;

            Point centerFirst = new Point((int)(First.Center.X - dxu * len / 4), (int)(First.Center.Y - dyu * len / 4));
            Point centerSecond = new Point((int)(First.Center.X - dxu * 3 * len / 4), (int)(First.Center.Y - dyu * 3 * len / 4));

            FirstControlVertex = new BezierControlVertex(centerFirst, first.Radius, polygon, this);
            FirstControlVertex.PropertyChanged += OnPointChanged;
            FirstControlVertex.PropertyChanged += FirstControlVertex_PropertyChanged;
            SecondControlVertex = new BezierControlVertex(centerSecond, second.Radius, polygon, this);
            SecondControlVertex.PropertyChanged += OnPointChanged;
            SecondControlVertex.PropertyChanged += SecondControlVertex_PropertyChanged;
            First.PropertyChanged += FirstControlVertex_PropertyChanged;
            Second.PropertyChanged += SecondControlVertex_PropertyChanged;
            ParentPolygon.BezierPoints.Add(FirstControlVertex);
            ParentPolygon.BezierPoints.Add(SecondControlVertex);

            LineFromFirstToFirstControl = LenBetweenTwoPoints(FirstControlVertex.Center, First.Center).Item3;
            LineFromSecondControlToSecond = LenBetweenTwoPoints(SecondControlVertex.Center, Second.Center).Item3;
            for (int i = Menu.Items.Count - 1; i > 2; i--)
            {
                Menu.Items.RemoveAt(i);
            }
            
            CalcTheBoundingBox();
        }

        private void FirstControlVertex_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            LineFromFirstToFirstControl  = LenBetweenTwoPoints(FirstControlVertex.Center, First.Center).Item3;
        }

        private void SecondControlVertex_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            LineFromSecondControlToSecond = LenBetweenTwoPoints(SecondControlVertex.Center, Second.Center).Item3;
        }


        public MyBezier(BezierVertex first, BezierVertex second, Color color, MyPolygon polygon, MyPoint leftPrev, MyPoint rightNext) : this(first, second, color, polygon)
        {
            LeftPrev = leftPrev;
            RightNext = rightNext;
        }

        public MyBezier(MyLine myLine, MyPoint leftPrev, MyPoint rightNext) : this(myLine.First, myLine.Second, myLine.Color, myLine.ParentPolygon)
        {
        }

        public override void VisitDrawer(IDrawer drawer)
        {
            drawer.Draw(this, this.Color);
        }

        protected override void CalcTheBoundingBox()
        {
            if (FirstControlVertex != null)
            {
                Point a = new Point(First.Center.X, First.Center.Y);
                Point b = new Point(FirstControlVertex.Center.X, FirstControlVertex.Center.Y);
                Point d = new Point(SecondControlVertex.Center.X, SecondControlVertex.Center.Y);
                Point c = new Point(Second.Center.X, Second.Center.Y);

                BoundingBox[0].ChangeVertices(a, b, c);
                BoundingBox[1].ChangeVertices(c, d, a);
                // I want to count every length and according to that apply how many parts my curve will have
                double len = 0;
                MyLine tmp = new MyLine(FirstControlVertex, SecondControlVertex, Color.Black, ParentPolygon);
                // Change it if have enough time. It's better num of parts depends on any param
                double numOfParts = 200;
                Shift = 1 / numOfParts;
                _A.X = First.Center.X;
                _A.Y = First.Center.Y;
                _B.X = 3 * FirstControlVertex.Center.X - 3 * First.Center.X;
                _B.Y = 3 * FirstControlVertex.Center.Y - 3 * First.Center.Y;
                _C.X = 3 * SecondControlVertex.Center.X - 6 * FirstControlVertex.Center.X + 3 * First.Center.X;
                _C.Y = 3 * SecondControlVertex.Center.Y - 6 * FirstControlVertex.Center.Y + 3 * First.Center.Y;
                _D.X = Second.Center.X - 3 * SecondControlVertex.Center.X + 3 * FirstControlVertex.Center.X - First.Center.X;
                _D.Y = Second.Center.Y - 3 * SecondControlVertex.Center.Y + 3 * FirstControlVertex.Center.Y - First.Center.Y;
            }
            else
                base.CalcTheBoundingBox();
        }

        public override bool ModifyForBezier(bool direction, MyPoint startVertex, int index = 0)
        {
            // I think, that move after bezier isn't necessary
            BezierVertex pointThatWasMoved = direction ? (BezierVertex)this.First : (BezierVertex)this.Second;
            MyPoint pointToMove = direction ? this.FirstControlVertex : this.SecondControlVertex;
            MyPoint thirdPoint = direction ? this.LeftPrev : this.RightNext;
            (double dx, double dy, double len) = MyLine.LenBetweenTwoPoints(thirdPoint.Center, pointThatWasMoved.Center);
            double ux = dx / len;
            double uy = dy / len;
            if (pointThatWasMoved.VertexState == BezierVertex.State.C1)
            {
                // Here must be check if the third point is from another bezier
                if (thirdPoint is BezierControlVertex)
                {
                    pointToMove.Center = new Point((int)(pointThatWasMoved.Center.X + dx), (int)(pointThatWasMoved.Center.Y + dy));
                }
                else
                {
                    pointToMove.Center = new Point((int)(pointThatWasMoved.Center.X + dx / 3), (int)(pointThatWasMoved.Center.Y + dy / 3));
                }
            }
            else if (pointThatWasMoved.VertexState == BezierVertex.State.G1)
            {
                // It doesn't work
                len = direction ? LineFromFirstToFirstControl : LineFromSecondControlToSecond;
                pointToMove.Center = new Point((int)(pointThatWasMoved.Center.X + ux * len), (int)(pointThatWasMoved.Center.Y + uy * len));
            }
            
            return false;
        }

    }
}
