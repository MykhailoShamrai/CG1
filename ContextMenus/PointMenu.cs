using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG1.ContextMenus
{
    internal class PointMenu: ContextMenuStrip
    {
        public PointMenu(): base()
        {
            ToolStripMenuItem deleteButton = new ToolStripMenuItem("Delete vertex");
            Items.Add(deleteButton);
        }
    }
}
