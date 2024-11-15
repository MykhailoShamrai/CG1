﻿using CG1.Drawers;

namespace CG1.Shapes
{
    public class MyLenghtLine : MyLine
    {
        public double Length { get; set; }
        public MyLenghtLine(MyPoint first, MyPoint second, Color color, MyPolygon polygon) : base(first, second, color, polygon)
        {

            Length = 0;
            Color = Color.MediumVioletRed;
            CountTwoPositionsOnSameLine();
        }

        public MyLenghtLine(MyPoint first, MyPoint second, Color color, int len, MyPolygon polygon) : base(first, second, color, polygon)
        {
            Length = len;
            Color = Color.MediumVioletRed;
            CountTwoPositionsOnSameLine();
        }

        public MyLenghtLine(MyLine line, double len) : base(line.First, line.Second, line.Color, line.ParentPolygon)
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

        public override bool ModifyForConstraints(bool direction, MyPoint startVertex)
        {
            MyPoint pointToMove = direction ? this.Second : this.First;
            MyPoint pointThatWasMoved = direction ? this.First : this.Second;
            if (pointToMove.Equals(startVertex))
                return false;
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

        public override void VisitDrawer(IDrawer drawer)
        {
            drawer.Draw(this, this.Color);
        }
    }
}
