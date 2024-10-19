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
            Color = Color.MediumVioletRed;
            CountTwoPositionsOnSameLine();
        }

        public MyLenghtLine(MyPoint first, MyPoint second, Color color, int len) : base(first, second, color)
        {
            Length = len;
            Color = Color.MediumVioletRed;
            CountTwoPositionsOnSameLine();
        }

        public MyLenghtLine(MyLine line, int len): base(line.First, line.Second, line.Color)
        {
            Length = len;
            Color = Color.MediumVioletRed;
            CountTwoPositionsOnSameLine();
        }


        private void CountTwoPositionsOnSameLine()
        {
            int dx = Second.Center.X - First.Center.X;
            int dy = Second.Center.Y - First.Center.Y;
            double realLength = Math.Sqrt(dx * dx + dy * dy);
            double ux = dx / realLength;
            double uy = dy / realLength;
            double xdif = ux * (realLength - Length) / 2;
            double ydif = uy * (realLength - Length) / 2;
            Point newPosFirst = new Point((int)(First.Center.X + xdif), (int)(First.Center.Y + ydif));
            Point newPosSecond = new Point((int)(Second.Center.X - xdif), (int)(Second.Center.Y - ydif));

            First.Center = newPosFirst;
            Second.Center = newPosSecond;
        }

        public override bool ModifyForConstraints(bool direction)
        {
            MyPoint pointToMove = direction ? this.Second : this.First;
            MyPoint pointThatWasMoved = direction ? this.First : this.Second;

            // Compute the current vector between the moved point and the point to move
            int dx = pointToMove.Center.X - pointThatWasMoved.Center.X;
            int dy = pointToMove.Center.Y - pointThatWasMoved.Center.Y;
            double currentLength = Math.Sqrt(dx * dx + dy * dy);

            double ux = dx / currentLength;
            double uy = dy / currentLength;

            // Now set the new position of pointToMove by moving along this unit vector
            Point newPosition = new Point(
                ((int)(pointThatWasMoved.Center.X + Length * ux)),  // Length is the locked length of the edge
                ((int)(pointThatWasMoved.Center.Y + Length * uy))
            );
            pointToMove.Center = newPosition;

            return true;
        }
    }
}
