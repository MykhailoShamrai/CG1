using CG1.Drawers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public interface IElement
    {
        public void VisitDrawer(IDrawer drawer);
        public ContextMenuStrip GetMenu();
        public abstract ContextMenuStrip Menu { get; set; }
        public Color Color { get; set; }
        public static int Cross(Point x, Point y, Point o)
        {
            double res = (x.X - o.X) * (y.Y - o.Y) - (x.Y - o.Y) * (y.X - o.X);
            if (res == 0)
                return 0;
            return res > 0 ? 1 : -1;
        }

        public static bool CheckIfPointIsOnLineIfCrossIsZero(Point point, Point x, Point y)
        {
            return (point.X <= Math.Max(x.X, y.X) &&
                point.X >= Math.Min(x.X, y.X) &&
                point.Y <= Math.Max(x.Y, y.Y) &&
                point.Y >= Math.Min(x.Y, y.Y));
            
        }

        // 0 - Intersect
        // 1 - One is part of another
        // 2 - Don't intersect
        public static int CheckIfTwoLinesIntersect(Point x1, Point x2, Point y1, Point y2)
        {
            int orient1 = Cross(x2, y1, x1);
            int orient2 = Cross(x2, y2, x1);
            int orient3 = Cross(y2, x1, y1);
            int orient4 = Cross(y2, x2, y1);

            if (orient1 * orient2 < 0 &&
                orient3 * orient4 < 0)
                return 0;

            // Points are part of another
            if (orient1 == 0 && CheckIfPointIsOnLineIfCrossIsZero(y1, x1, x2) ||
                orient2 == 0 && CheckIfPointIsOnLineIfCrossIsZero(y2, x1, x2) ||
                orient3 == 0 && CheckIfPointIsOnLineIfCrossIsZero(x1, y1, y2) ||
                orient4 == 0 && CheckIfPointIsOnLineIfCrossIsZero(x2, y1, y2))
                return 1;
            return 2;
        }

    }
}
