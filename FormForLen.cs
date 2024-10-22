using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG1
{
    public partial class FormForLen : Form
    {
        public double Len { get; set; }
        public FormForLen()
        {
            InitializeComponent();
        }

        public void SetToTextBox(double len)
        {
            Len = len;
            LenBox.Text = len.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            char c = LenBox.Text[^1];
            if ((c < '0' || c > '9') && c != '.')
            {
                MessageBox.Show("It's not a number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LenBox.Text = "";
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            double tmp = Len;
            double.TryParse(LenBox.Text, out tmp);
            Len = tmp;
            this.Hide();
        }
    }
}
