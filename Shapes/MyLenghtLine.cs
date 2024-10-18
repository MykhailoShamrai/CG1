using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    internal class MyLenghtLine : MyLine
    {
        public int Length {  get; set; }
        public MyLenghtLine(MyPoint first, MyPoint second, Color color) : base(first, second, color)
        {
            Length = 0;
        }

        public MyLenghtLine(MyPoint first, MyPoint second, Color color, int len) : base(first, second, color)
        {
            Length = len;
        }

        public MyLenghtLine(MyLine line, int len): base(line.First, line.Second, line.Color)
        {
            Length = len;
        }

        public override MyPoint? ModifyForConstraints(MyPoint oldStateOfDraggedPoint, bool direction)
        {
            // pointToMove is the one that needs to be adjusted to maintain the locked length
            MyPoint pointToMove = direction ? this.Second : this.First;

            // pointThatWasMoved is the vertex that has already been dragged
            MyPoint pointThatWasMoved = direction ? this.First : this.Second;

            // Compute the current vector between the moved point and the point to move
            int dx = pointToMove.Center.X - pointThatWasMoved.Center.X;
            int dy = pointToMove.Center.Y - pointThatWasMoved.Center.Y;

            // Calculate the current length of the edge
            double currentLength = Math.Sqrt(dx * dx + dy * dy);

            // Calculate the unit vector in the direction from pointThatWasMoved to pointToMove
            double ux = dx / currentLength;
            double uy = dy / currentLength;

            // Now set the new position of pointToMove by moving along this unit vector
            Point newPosition = new Point(
                (int)(pointThatWasMoved.Center.X + Length * ux),  // Length is the locked length of the edge
                (int)(pointThatWasMoved.Center.Y + Length * uy)
            );

            // I have null ref here sometimes
            // Update the old dragged point's center for reference
            oldStateOfDraggedPoint.Center = new Point(pointThatWasMoved.Center.X, pointThatWasMoved.Center.Y);

            // Finally, update the position of the pointToMove
            pointToMove.Center = newPosition;

            return oldStateOfDraggedPoint;
        }
    }
}
