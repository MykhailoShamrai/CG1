﻿using CG1.ContextMenus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace CG1.Shapes
{
    public class MyLine: IElement
    {
        private int thicknes = 5;
        public MyPoint First { get; set; }
        public MyPoint Second { get; set; }
        public List<Triangle> BoundingBox { get; set; }
        public static ContextMenuStrip Menu { get; } = new LineMenu();
        public ContextMenuStrip GetMenu()
        {
            return Menu;
        }

        public Color Color { get; set; }

        public MyLine(MyPoint first, MyPoint second, Color color)
        {
            First = first;
            Second = second;
            Color = color;
            BoundingBox = new List<Triangle>();
            BoundingBox.Add(new Triangle());
            BoundingBox.Add(new Triangle());
            First.PropertyChanged += OnPointChanged;
            Second.PropertyChanged += OnPointChanged;
            CalcTheBoundingBox();
        }
        public void ChangeFirstEnd(MyPoint point)
        {
            First.PropertyChanged -= OnPointChanged;
            First = point;
            Second.PropertyChanged += OnPointChanged;
            CalcTheBoundingBox();
        }

        public void ChangeSecondEnd(MyPoint point)
        {
            Second.PropertyChanged -= OnPointChanged;
            Second = point;
            Second.PropertyChanged += OnPointChanged;
            CalcTheBoundingBox();
        }

        protected void CalcTheBoundingBox()
        {
            Point firstCenter = First.Center;
            Point secondCenter = Second.Center;
            double dx = secondCenter.X - firstCenter.X;
            double dy = secondCenter.Y - firstCenter.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            double ux = dx / length;
            double uy = dy / length;
            // Counting a perpendicular
            double perpX = -uy * thicknes;
            double perpY = ux * thicknes;
            // Calculating the corners of the bounding box
            Point a = new Point((int)(firstCenter.X + perpX), (int)(firstCenter.Y + perpY));
            Point b = new Point((int)(firstCenter.X - perpX), (int)(firstCenter.Y - perpY));
            Point c = new Point((int)(secondCenter.X - perpX), (int)(secondCenter.Y - perpY));
            Point d = new Point((int)(secondCenter.X + perpX), (int)(secondCenter.Y + perpY));

            BoundingBox[0].ChangeVertices(a, b, c);  
            BoundingBox[1].ChangeVertices(c, d, a);  
        }

        protected virtual void OnPointChanged(object sender, PropertyChangedEventArgs e)
        {
            CalcTheBoundingBox();
        }

        public bool CheckIfPointIsInsideBox(Point point)
        {
            bool res = BoundingBox[0].CheckIfPointIsInside(point) || BoundingBox[1].CheckIfPointIsInside(point);
            return res;
        }

        /// <summary>
        /// The function should be used for modifying the edge and send 
        /// </summary>
        /// <param name="pointWhichIsDragged"></param>
        /// <param name="index"></param>
        /// <param name="direction"></param>
        public virtual bool ModifyForConstraints(bool direction, MyPoint startVertex)
        {
            return false;
        }
    }
}
