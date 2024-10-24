namespace CG1.ContextMenus
{
    internal class PointMenu : ContextMenuStrip
    {
        public PointMenu() : base()
        {
            ToolStripMenuItem deleteButton = new ToolStripMenuItem("Delete vertex");
            Items.Add(deleteButton);
        }
    }
}
