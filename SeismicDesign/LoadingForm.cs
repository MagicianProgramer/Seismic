using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeismicDesign
{
    public partial class LoadingForm : Form
    {
        private int peek = 0;
        public LoadingForm()
        {
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            CenterToScreen();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            peek++;
            if (peek == 5)
            {
                this.Close();
            }

        }

        private void LoadingForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
