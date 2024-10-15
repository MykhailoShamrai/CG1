using CG1.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.ContextMenus
{
    internal class LineMenu: ContextMenuStrip
    {
        //public MyPolygon Polygon { get; set; }
        public LineMenu(): base()
        {
            //Polygon = poly;
            ToolStripMenuItem addButton = new ToolStripMenuItem("Add vertex");
            ToolStripMenuItem lockButton = new ToolStripMenuItem("Lock the length");
            lockButton.Enabled = false;
            Items.Add(addButton);
            Items.Add(lockButton);
        }
    }
}
