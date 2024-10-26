using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace CG1.Shapes
{
    public class BezierControlVertex : MyPoint
    {
        private MyBezier _bezier;
        public BezierControlVertex(Point center, int radius, MyPolygon polygon) : base(center, radius, polygon)
        {
            Menu.Items.Clear();
        }

        public BezierControlVertex(Point center, int radius, MyPolygon polygon, MyBezier bezier): this(center, radius, polygon)
        {
            _bezier = bezier;
        }

        public void ModificateWhileDragging()
        {
            bool direction = this.Equals(_bezier.FirstControlVertex) ? false : true;
            // point that was moved == this
            // in right side
            // I think I should do large switch case here...
            BezierVertex middleVertex = direction ? (BezierVertex)_bezier.Second : (BezierVertex)_bezier.First;
            MyPoint thirdVertex = direction ? _bezier.RightNext : _bezier.LeftPrev;

            int index = ParentPolygon.Points.IndexOf(middleVertex);
            index = direction ? index : (index - 1 >= 0 ? index - 1 : ParentPolygon.Lines.Count + index - 1);
            MyLine lineBetween = ParentPolygon.Lines[index];
            if (!(thirdVertex is BezierControlVertex))
            {
                switch (middleVertex.VertexState)
                {
                    case BezierVertex.State.C0:
                        break;
                    case BezierVertex.State.C1:
                        // The length must be strikt
                        Point NewCenter = new Point();
                        // C1 for Vertical and horizontal
                        if (lineBetween is MyVerticalLine)
                        {
                            if (((MyVerticalLine)lineBetween).IsVertical)
                            {
                                int dy = this.Center.Y - middleVertex.Center.Y;
                                NewCenter = new Point(this.Center.X, middleVertex.Center.Y - dy * 3);
                            }
                            else
                            {
                                int dx = this.Center.X - middleVertex.Center.X;
                                NewCenter = new Point(middleVertex.Center.X - dx * 3, this.Center.Y);
                            }
                            ParentPolygon.SelectedElement = thirdVertex;
                            ParentPolygon.DragVertex(NewCenter, !direction, direction);
                            ParentPolygon.SelectedElement = this;
                        }
                        // C1 for lenght
                        else if (lineBetween is MyLenghtLine)
                        {
                            double L = ((MyLenghtLine)lineBetween).Length;
                            int dx = this.Center.X - middleVertex.Center.X;
                            int dy = this.Center.Y - middleVertex.Center.Y;
                            double len = Math.Sqrt(dx * dx + dy * dy);
                            double ux = dx / len;
                            double uy = dy / len;
                            NewCenter = new Point((int)(this.Center.X - L * ux / 3), (int)(this.Center.Y - L * uy / 3));
                            ParentPolygon.SelectedElement = middleVertex;
                            ParentPolygon.DragVertex(NewCenter, !direction, direction);
                            ParentPolygon.SelectedElement = this;
                            NewCenter = new Point((int)(middleVertex.Center.X - L * ux), (int)(middleVertex.Center.Y - L * uy));
                            ParentPolygon.SelectedElement = thirdVertex;
                            ParentPolygon.DragVertex(NewCenter, !direction, direction);
                            ParentPolygon.SelectedElement = this;
                        }
                        // C1 for normal edge
                        else
                        {
                            int dx = this.Center.X - middleVertex.Center.X;
                            int dy = this.Center.Y - middleVertex.Center.Y;
                            NewCenter = new Point((int)(middleVertex.Center.X - 3 * dx), (int)(middleVertex.Center.Y - 3 * dy));
                            ParentPolygon.SelectedElement = thirdVertex;
                            ParentPolygon.DragVertex(NewCenter, !direction, direction);
                            ParentPolygon.SelectedElement = this;
                        }
                        break;
                    case BezierVertex.State.G1:
                        NewCenter = new Point();
                        // G1 for Vertical and horizontal
                        if (lineBetween is MyVerticalLine)
                        {
                            if (((MyVerticalLine)lineBetween).IsVertical)
                            {
                                int dy = middleVertex.Center.Y - thirdVertex.Center.Y;
                                double len = Math.Sqrt(dy * dy);
                                if (thirdVertex.Center.Y < middleVertex.Center.Y && this.Center.Y < middleVertex.Center.Y)
                                    NewCenter = new Point(this.Center.X, (int)(middleVertex.Center.Y + len));
                                else if (thirdVertex.Center.Y > middleVertex.Center.Y && Center.Y > middleVertex.Center.Y)
                                    NewCenter = new Point(this.Center.X, (int)(middleVertex.Center.Y - len));
                                else
                                    NewCenter = new Point(this.Center.X, thirdVertex.Center.Y);
                            }
                            else
                            {
                                int dx = middleVertex.Center.X - thirdVertex.Center.X;
                                double len = Math.Sqrt(dx * dx);
                                if (thirdVertex.Center.X < middleVertex.Center.X && Center.X < middleVertex.Center.X)
                                    NewCenter = new Point((int)(middleVertex.Center.X + len), Center.Y);
                                else if (thirdVertex.Center.X > middleVertex.Center.X && Center.X > middleVertex.Center.X)
                                    NewCenter = new Point((int)(middleVertex.Center.X - len), Center.Y);
                                else
                                    NewCenter = new Point(thirdVertex.Center.X, this.Center.Y);
                            }
                            ParentPolygon.SelectedElement = thirdVertex;
                            ParentPolygon.DragVertex(NewCenter);
                            ParentPolygon.SelectedElement = this;
                        }
                        // G1 for lenght
                        else if (lineBetween is MyLenghtLine)
                        {
                            double L = ((MyLenghtLine)lineBetween).Length;
                            int dx = this.Center.X - middleVertex.Center.X;
                            int dy = this.Center.Y - middleVertex.Center.Y;
                            double len = Math.Sqrt(dx * dx + dy * dy);
                            double ux = dx / len;
                            double uy = dy / len;
                            NewCenter = new Point((int)(middleVertex.Center.X - L * ux), (int)(middleVertex.Center.Y - L * uy));
                            ParentPolygon.SelectedElement = thirdVertex;
                            ParentPolygon.DragVertex(NewCenter, !direction, direction);
                            ParentPolygon.SelectedElement = this;
                        }
                        // C1 for normal edge
                        else
                        {
                            int dx = this.Center.X - middleVertex.Center.X;
                            int dy = this.Center.Y - middleVertex.Center.Y;
                            double len = Math.Sqrt(dx * dx + dy * dy);
                            double ux = dx / len;
                            double uy = dy / len;
                            double L = lineBetween.ReturnLen();
                            NewCenter = new Point((int)(middleVertex.Center.X - L * ux), (int)(middleVertex.Center.Y - L * uy));
                            // Something wrong here

                            ParentPolygon.SelectedElement = thirdVertex;
                            ParentPolygon.DragVertex(NewCenter, !direction, direction);
                            ParentPolygon.SelectedElement = this;
                        }

                        break;
                    default:
                        break;
                }
            }
            else // on the other side wi also have bezier control vertex
            {
                Point NewCenter = new Point();
                switch (middleVertex.VertexState)
                {
                    case BezierVertex.State.C1:
                        int dx = this.Center.X - middleVertex.Center.X;
                        int dy = this.Center.Y - middleVertex.Center.Y;
                        double len = Math.Sqrt(dx * dx + dy * dy);
                        NewCenter = new Point((middleVertex.Center.X - dx), (middleVertex.Center.Y - dy));
                        thirdVertex.Center = NewCenter;
                        break;
                    case BezierVertex.State.G1:
                        dx = this.Center.X - middleVertex.Center.X;
                        dy = this.Center.Y - middleVertex.Center.Y;
                        len = Math.Sqrt(dx * dx + dy * dy);
                        double ux = dx / len;
                        double uy = dy / len;
                        double L = direction ? ((MyBezier)(lineBetween)).LineFromSecondControlToSecond.Len : ((MyBezier)(lineBetween)).LineFromFirstToFirstControl.Len;
                        NewCenter = new Point((int)(middleVertex.Center.X - L * ux), (int)(middleVertex.Center.Y - L * uy));
                        thirdVertex.Center = NewCenter;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
