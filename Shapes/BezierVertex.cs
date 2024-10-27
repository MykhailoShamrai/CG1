using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.Shapes
{
    public class BezierVertex : MyPoint
    {
        public enum State
        {
            C0, C1, G1
        }
        public State VertexState { get; set; } = State.G1;
        public BezierVertex(Point center, int radius, MyPolygon polygon) : base(center, radius, polygon)
        {
            ToolStripMenuItem C0Button = new ToolStripMenuItem("C0");
            Menu.Items.Add(C0Button);
            ToolStripMenuItem C1Button = new ToolStripMenuItem("C1");
            Menu.Items.Add(C1Button);
            ToolStripMenuItem G1Button = new ToolStripMenuItem("G1");
            Menu.Items.Add(G1Button);
            Menu.Items[1].Click += ParentPolygon.C0_click;
            Menu.Items[2].Click += ParentPolygon.C1_click;
            Menu.Items[3].Click += ParentPolygon.G1_click;
        }

    }
}
