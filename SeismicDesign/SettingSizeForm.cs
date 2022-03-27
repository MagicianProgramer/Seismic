using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
namespace SeismicDesign
{
    public partial class SettingSizeForm : Form
    {

        public delegate void ChildFormSnedDataHandler(int width, int height);
        public event ChildFormSnedDataHandler ChildFormEvent;

        public SettingSizeForm()
        {
            InitializeComponent();
        }

        private void SettingSizeForm_Load(object sender, EventArgs e)
        {
            CenterToParent();
        }

        public void SetData(int width, int height)
        {
            textBoxWidth.Text = width.ToString();
            textBoxHeight.Text = height.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            int width = 0;
            int height = 0;

            Int32.TryParse(textBoxWidth.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out width);
            Int32.TryParse(textBoxHeight.Text.ToString(), NumberStyles.Any,
                                               CultureInfo.CurrentCulture.NumberFormat, out height);
            ChildFormEvent(width, height);

            this.Close();
        }
    }
}
