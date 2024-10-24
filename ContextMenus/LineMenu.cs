namespace CG1.ContextMenus
{
    internal class LineMenu : ContextMenuStrip
    {
        public LineMenu() : base()
        {
            ToolStripMenuItem addButton = new ToolStripMenuItem("Add vertex");
            ToolStripMenuItem lockButton = new ToolStripMenuItem("Lock the length");
            ToolStripMenuItem makeVertical = new ToolStripMenuItem("Create vertical edge");
            ToolStripMenuItem makeHorizontal = new ToolStripMenuItem("Create horizontal edge");
            ToolStripMenuItem makeBezier = new ToolStripMenuItem("Create bezier");
            Items.Add(addButton);
            Items.Add(lockButton);
            Items.Add(makeVertical);
            Items.Add(makeHorizontal);
            Items.Add(makeBezier);
        }
    }
}
