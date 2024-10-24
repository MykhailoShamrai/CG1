using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class MyBezier : MyLine
    {
        private int _startDist = 30;
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
            CalcTheBoundingBox();
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
            }
            else
                base.CalcTheBoundingBox();
        }


    }
}
