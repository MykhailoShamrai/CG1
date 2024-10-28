using Microsoft.VisualBasic;

namespace CG1.ContextMenus
{
    internal class LineMenu : ContextMenuStrip
    {
        public bool AntiAliasFlag {  get; set; }
        public LineMenu() : base()
        {
            ToolStripMenuItem makeNormal = new ToolStripMenuItem("Add normal");
            ToolStripMenuItem clear = new ToolStripMenuItem("Delete polygon");
            ToolStripMenuItem drawWithAntiAlias = new ToolStripMenuItem("Draw this edge with anti aliasing");
            ToolStripMenuItem addButton = new ToolStripMenuItem("Add vertex");
            ToolStripMenuItem lockButton = new ToolStripMenuItem("Lock the length");
            ToolStripMenuItem makeVertical = new ToolStripMenuItem("Create vertical edge");
            ToolStripMenuItem makeHorizontal = new ToolStripMenuItem("Create horizontal edge");
            ToolStripMenuItem makeBezier = new ToolStripMenuItem("Create bezier");
            
            Items.Add(makeNormal);
            Items.Add(clear);
            Items.Add(drawWithAntiAlias);
            Items.Add(addButton);
            Items.Add(lockButton);
            Items.Add(makeVertical);
            Items.Add(makeHorizontal);
            Items.Add(makeBezier);

            Items[2].Click += drawWithAntyAlias_Click;

            AntiAliasFlag = false;
        }

        private void drawWithAntyAlias_Click(object? sender, EventArgs e)
        {
            AntiAliasFlag = !AntiAliasFlag;
            Items[2].Text = AntiAliasFlag ? "Draw like default" : "Draw this edge with anti alising";
        }
    }
}
