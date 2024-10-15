using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class Triangle
    {
        public List<Point> points;
        public Triangle()
        {
            points = new List<Point>();
            points.Add(new Point());
            points.Add(new Point());
            points.Add(new Point());
        }
        public Triangle(Point x, Point y, Point z)
        {
            points = new List<Point>();
            points.Add(x);
            points.Add(y);
            points.Add(z);
            SortVertices();
        }

        public void ChangeVertices(Point x, Point y, Point z)
        {
            points[0] = x;
            points[1] = y;
            points[2] = z;
            SortVertices();
        }

        public void SortVertices()
        {
            points.Sort((p1, p2) =>
            {
                if (p1.Y != p2.Y)
                    return p2.Y.CompareTo(p1.Y); 
                return p1.X.CompareTo(p2.X); 
            });

            // And now for conter clockwise
            if (IElement.Cross(points[1], points[2], points[0]) < 0)
            {
                Point tmp = new Point(points[1].X, points[1].Y);
                points[1] = points[2];
                points[2] = tmp;
            }
        }

        public bool CheckIfPointIsInside(Point point)
        {
            return IElement.Cross(points[1], point, points[0]) >= 0 &&
                   IElement.Cross(points[2], point, points[1]) >= 0 &&
                   IElement.Cross(points[0], point, points[2]) >= 0;
        }
    }
}
