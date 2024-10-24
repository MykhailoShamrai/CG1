﻿using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
        public MyPoint FirstControlVertex { get; set; }
        public MyPoint SecondControlVertex { get; set; }
        public MyBezier(MyPoint first, MyPoint second, Color color, MyPolygon polygon) : base(first, second, color, polygon)
        {
            (double ux, double uy) = FindPerpendicular(this.First.Center, this.Second.Center);
            double len = ReturnLen();
            double dx = First.Center.X - Second.Center.X;
            double dy = First.Center.Y - Second.Center.Y;
            double dxu = dx / len;
            double dyu = dy / len;

            Point centerFirst = new Point((int)(First.Center.X + dxu * len / 4 + ux * _startDist), (int)(First.Center.Y + dyu * len / 4 + uy * _startDist));
            Point centerSecond = new Point((int)(First.Center.X + dxu * 3 * len / 4 - ux * _startDist), (int)(First.Center.Y + dyu * 3 * len / 4 - uy * _startDist));

            FirstControlVertex = new MyPoint(centerFirst, first.Radius, polygon);
            SecondControlVertex = new MyPoint(centerSecond, second.Radius, polygon);
            OnPointChanged(this, new PropertyChangedEventArgs(""));
        }

        public MyBezier(MyLine myLine) : base(myLine.First, myLine.Second, myLine.Color, myLine.ParentPolygon)
        {
            (double ux, double uy) = FindPerpendicular(this.First.Center, this.Second.Center);
            double len = ReturnLen();
            double dx = First.Center.X - Second.Center.X;
            double dy = First.Center.Y - Second.Center.Y;
            double dxu = dx / len;
            double dyu = dy / len;

            Point centerFirst = new Point((int)(Second.Center.X + dxu * len / 4 + ux * _startDist), (int)(Second.Center.Y + dyu * len / 4 + uy * _startDist));
            Point centerSecond = new Point((int)(Second.Center.X + dxu * 3 * len / 4 - ux * _startDist ), (int)(Second.Center.Y + dyu * 3 * len / 4 - uy * _startDist));

            FirstControlVertex = new MyPoint(centerFirst, myLine.First.Radius, ParentPolygon);
            FirstControlVertex.PropertyChanged += OnPointChanged;
            SecondControlVertex = new MyPoint(centerSecond, myLine.Second.Radius, ParentPolygon);
            SecondControlVertex.PropertyChanged += OnPointChanged;
            ParentPolygon.BezierPoints.Add(FirstControlVertex);
            ParentPolygon.BezierPoints.Add(SecondControlVertex);
            CalcTheBoundingBox();
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
                //len += tmp.ReturnLen();
                //tmp = this;
                //len += tmp.ReturnLen();
                //tmp.First = First;
                //tmp.Second = FirstControlVertex;
                //len += tmp.ReturnLen();
                //tmp.First = SecondControlVertex;
                //tmp.Second = Second;
                //len += tmp.ReturnLen();
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
    }
}