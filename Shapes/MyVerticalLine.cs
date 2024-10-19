using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class MyVerticalLine : MyLine
    {
        public bool IsVertical { get; set; }
        public MyVerticalLine(MyPoint first, MyPoint second, Color color, bool isVertical) : base(first, second, color)
        {
            IsVertical = isVertical;
            MakeTwoPointsOnSameAxisLine();
            Color = isVertical ? Color.DarkOrange : Color.LightPink;
        }

        public MyVerticalLine(MyLine line, bool isVertical) : base(line.First, line.Second, line.Color)
        {
            IsVertical = isVertical;
            MakeTwoPointsOnSameAxisLine();
            Color = isVertical ? Color.DarkOrange : Color.LightPink;
        }

        private void MakeTwoPointsOnSameAxisLine()
        {
            int minX = First.Center.X < Second.Center.X ? First.Center.X : Second.Center.X;
            int maxX = First.Center.X < Second.Center.X ? Second.Center.X : First.Center.X;
            int minY = First.Center.Y < Second.Center.Y ? First.Center.Y : Second.Center.Y;
            int maxY = First.Center.Y < Second.Center.Y ? Second.Center.Y: First.Center.Y;
            int middleY = minY + (maxY - minY) / 2;
            int middleX = minX + (maxX - minX) / 2;
            Point newPosFirst = IsVertical ? new Point(middleX, First.Center.Y) : new Point(First.Center.X, middleY);
            Point newPosSecond = IsVertical ? new Point(middleX, Second.Center.Y) : new Point(Second.Center.X, middleY);

            First.Center = newPosFirst;
            Second.Center = newPosSecond;
        }

        public override bool ModifyForConstraints(bool direction)
        {

            MyPoint pointToMove = direction ? this.Second : this.First;
            MyPoint pointThatWasMoved = direction ? this.First : this.Second;

            // For point that was moved we already have changes in an object
            // dx - changes in x axis
            int d = IsVertical ? pointThatWasMoved.Center.X - pointToMove.Center.X : 
                pointThatWasMoved.Center.Y - pointToMove.Center.Y;
            pointToMove.Center = new Point(pointThatWasMoved.Center.X, pointToMove.Center.Y);


            if (Math.Abs(d) > 0)
            {
                return true;
            }
            return false;
        }
    }
}
