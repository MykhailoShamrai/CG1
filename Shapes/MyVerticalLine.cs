using CG1.ContextMenus;
using CG1.Drawers;

namespace CG1.Shapes
{
    public class MyVerticalLine : MyLine
    {
        public bool IsVertical { get; set; }
        public bool IsHorizontal { get; set; }
        public MyVerticalLine(MyPoint first, MyPoint second, Color color, bool isVertical, MyPolygon polygon) : base(first, second, color, polygon)
        {
            IsVertical = isVertical;
            IsHorizontal = !IsVertical;
            MakeTwoPointsOnSameAxisLine();
            Color = isVertical ? Color.DarkOrange : Color.LightPink;
        }

        public MyVerticalLine(MyLine line, bool isVertical) : base(line.First, line.Second, line.Color, line.ParentPolygon)
        {
            IsVertical = isVertical;
            IsHorizontal = !IsVertical;
            MakeTwoPointsOnSameAxisLine();
            Color = isVertical ? Color.DarkOrange : Color.LightPink;
        }

        private void MakeTwoPointsOnSameAxisLine()
        {
            int minX = First.Center.X < Second.Center.X ? First.Center.X : Second.Center.X;
            int maxX = First.Center.X < Second.Center.X ? Second.Center.X : First.Center.X;
            int minY = First.Center.Y < Second.Center.Y ? First.Center.Y : Second.Center.Y;
            int maxY = First.Center.Y < Second.Center.Y ? Second.Center.Y : First.Center.Y;
            int middleY = minY + (maxY - minY) / 2;
            int middleX = minX + (maxX - minX) / 2;
            Point newPosFirst = IsVertical ? new Point(middleX, First.Center.Y) : new Point(First.Center.X, middleY);
            Point newPosSecond = IsVertical ? new Point(middleX, Second.Center.Y) : new Point(Second.Center.X, middleY);

            First.Center = newPosFirst;
            Second.Center = newPosSecond;
        }

        public override bool ModifyForConstraints(bool direction, MyPoint startVertex)
        {
            MyPoint pointToMove = direction ? this.Second : this.First;
            MyPoint pointThatWasMoved = direction ? this.First : this.Second;

            if (pointToMove.Equals(startVertex))
                return false;

            // For point that was moved we already have changes in an object
            // dx - changes in x axis
            int d = IsVertical ? pointThatWasMoved.Center.X - pointToMove.Center.X :
                pointThatWasMoved.Center.Y - pointToMove.Center.Y;
            pointToMove.Center = IsVertical ? new Point(pointThatWasMoved.Center.X, pointToMove.Center.Y) :
                new Point(pointToMove.Center.X, pointThatWasMoved.Center.Y);

            // If any changes for second vertex were made
            if (Math.Abs(d) > 0)
            {
                return true;
            }
            return false;
        }

        public override void ChangeMenuWhileCreating(MyLine LeftLine, MyLine RightLine)
        {

            LineMenu leftMenu = (LineMenu)LeftLine.Menu;
            LineMenu rightMenu = (LineMenu)RightLine.Menu;
            if (IsHorizontal)
            {
                leftMenu.Items[3].Enabled = false;
                rightMenu.Items[3].Enabled = false;
            }
            else
            {
                leftMenu.Items[2].Enabled = false;
                rightMenu.Items[2].Enabled = false;
            }
        }

        public override void VisitDrawer(IDrawer drawer)
        {
            drawer.Draw(this, this.Color);
        }
    }
}
