namespace CG1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBoxMain = new PictureBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            BrezenheimBox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxMain
            // 
            pictureBoxMain.Dock = DockStyle.Fill;
            pictureBoxMain.Location = new Point(0, 0);
            pictureBoxMain.Name = "pictureBoxMain";
            pictureBoxMain.Size = new Size(887, 541);
            pictureBoxMain.TabIndex = 0;
            pictureBoxMain.TabStop = false;
            pictureBoxMain.Click += pictureBoxMain_Click;
            pictureBoxMain.MouseDown += pictureBoxMain_MouseDown;
            pictureBoxMain.MouseMove += pictureBoxMain_MouseMove;
            pictureBoxMain.MouseUp += pictureBoxMain_MouseUp;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // BrezenheimBox
            // 
            BrezenheimBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BrezenheimBox.AutoSize = true;
            BrezenheimBox.Location = new Point(706, 12);
            BrezenheimBox.Name = "BrezenheimBox";
            BrezenheimBox.Size = new Size(105, 24);
            BrezenheimBox.TabIndex = 2;
            BrezenheimBox.Text = "Brezenham";
            BrezenheimBox.UseVisualStyleBackColor = true;
            BrezenheimBox.CheckedChanged += BrezenheimBox_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(887, 541);
            Controls.Add(BrezenheimBox);
            Controls.Add(pictureBoxMain);
            DoubleBuffered = true;
            Name = "Form1";
            Text = "Polygon Editor";
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxMain;
        private ContextMenuStrip contextMenuStrip1;
        private CheckBox BrezenheimBox;
    }
}
