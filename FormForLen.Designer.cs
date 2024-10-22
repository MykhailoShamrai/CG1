namespace CG1
{
    partial class FormForLen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            LenBox = new TextBox();
            AcceptButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 23);
            label1.Name = "label1";
            label1.Size = new Size(218, 20);
            label1.TabIndex = 0;
            label1.Text = "Please, add a length of an edge";
            // 
            // LenBox
            // 
            LenBox.Location = new Point(92, 61);
            LenBox.Name = "LenBox";
            LenBox.Size = new Size(150, 27);
            LenBox.TabIndex = 1;
            LenBox.Text = "100";
            LenBox.TextChanged += textBox1_TextChanged;
            // 
            // AcceptButton
            // 
            AcceptButton.Location = new Point(226, 112);
            AcceptButton.Name = "AcceptButton";
            AcceptButton.Size = new Size(94, 29);
            AcceptButton.TabIndex = 2;
            AcceptButton.Text = "OK";
            AcceptButton.UseVisualStyleBackColor = true;
            AcceptButton.Click += AcceptButton_Click;
            // 
            // FormForLen
            // 
            AcceptButton = AcceptButton;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(350, 180);
            Controls.Add(AcceptButton);
            Controls.Add(LenBox);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            MaximumSize = new Size(350, 180);
            MinimumSize = new Size(350, 180);
            Name = "FormForLen";
            Text = "Lock a length";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox LenBox;
        private Button AcceptButton;
    }
}