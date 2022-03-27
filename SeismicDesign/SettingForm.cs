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
    public partial class SettingForm : Form
    {
        public delegate void ChildFormSnedDataHandler(int nDecimal, int skin, double NumberOfDecimals, double DefaultDampingRatio, double PGARatioForNOEC, double PGARatioForBD, double AccelerationValueForBD, double SDSCoefficient, int method);
        public event ChildFormSnedDataHandler ChildFormEvent;
        private bool bMove = false;
        private Point oldPt;
        private Point oldFormPt;

        private double m_dNumberOfDecimals = 5;
        private double m_dDefaultDampingRatio = 5;
        private double m_dPGARatioForNOEC = 65;
        private double m_dPGARatioForBD = 5;
        private double m_dAccelerationValueForBD = 0.05;
        private double m_dSDSCoefficient = 0.9;
        private int m_nSkinMode = 0;
        private int m_nMethod = 0;

        private int nDecimalSymbol = 1;

        private int m_nDecimalSeperator = 1;

        public SettingForm()
        {
            InitializeComponent();

            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                m_nDecimalSeperator = 1;
            else
                m_nDecimalSeperator = 2;
        }

        public void SetParameter(int nDecimal, double NOD, double DDR, double PGARFBOEC, double PRFBD, double AVFBD, int nMethod)
        {
            nDecimalSymbol = nDecimal;
            m_dNumberOfDecimals = NOD;
            m_dDefaultDampingRatio = DDR;
            m_dPGARatioForNOEC = PGARFBOEC;
            m_dPGARatioForBD = PRFBD;
            m_dAccelerationValueForBD = AVFBD;
            m_nMethod = nMethod;            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Double.TryParse(textNumberDecimals.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out m_dNumberOfDecimals);
            Double.TryParse(textDefaultDampingRatio.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out m_dDefaultDampingRatio);
            Double.TryParse(textPGARatioForNOEC.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out m_dPGARatioForNOEC);
            Double.TryParse(textPGARatioForBracktedDuration.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out m_dPGARatioForBD);
            Double.TryParse(textAccelerationVFBD.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out m_dAccelerationValueForBD);

            Double.TryParse(textSDSCoefficient.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out m_dSDSCoefficient);

            m_dSDSCoefficient = (double)((decimal)m_dSDSCoefficient / (decimal)100);


            if (this.radioPointSymbol.Checked == true)
            {
                nDecimalSymbol = 1;
            }
            else
            {
                nDecimalSymbol = 2;
            }
            this.ChildFormEvent(nDecimalSymbol, m_nSkinMode, m_dNumberOfDecimals, m_dDefaultDampingRatio, m_dPGARatioForNOEC, m_dPGARatioForBD, m_dAccelerationValueForBD, m_dSDSCoefficient, m_nMethod);
            Close();
        }

        private void btnDarkMode_Click(object sender, EventArgs e)
        {
            btnDarkMode.BackColor = Color.Gray;
            btnLightMode.BackColor = Color.Transparent;
            m_nSkinMode = 1;
        }

        private void btnLightMode_Click(object sender, EventArgs e)
        {
            btnDarkMode.BackColor = Color.Transparent;
            btnLightMode.BackColor = Color.Gray;
            m_nSkinMode = 2;
        }

        private void btnGraphColor_Click(object sender, EventArgs e)
        {

        }

        private void btnLineWidth_Click(object sender, EventArgs e)
        {

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            CenterToParent();

            if(nDecimalSymbol == 1)
            {
                this.radioPointSymbol.Checked = true;
            }
            else
            {
                this.radioComaSymbol.Checked = true;
            }

            this.textNumberDecimals.Text = m_dNumberOfDecimals.ToString();
            this.textDefaultDampingRatio.Text = m_dDefaultDampingRatio.ToString();
            this.textPGARatioForNOEC.Text = m_dPGARatioForNOEC.ToString();
            this.textPGARatioForBracktedDuration.Text = m_dPGARatioForBD.ToString();
            this.textAccelerationVFBD.Text = m_dAccelerationValueForBD.ToString();
        }

        private void SettingForm_MouseDown(object sender, MouseEventArgs e)
        {
//             bMove = true;
//             oldPt.X = e.X;
//             oldPt.Y = e.Y;
// 
//             oldFormPt.X = this.Left;
//             oldFormPt.Y = this.Top;
        }

        private void SettingForm_MouseMove(object sender, MouseEventArgs e)
        {
//             if(bMove)
//             {
//                 this.Left += (e.X - oldPt.X);
//                 this.Top += (e.Y - oldPt.Y);
//                 oldPt.X = e.X;
//                 oldPt.Y = e.Y;
//             }
        }

        private void SettingForm_MouseUp(object sender, MouseEventArgs e)
        {
            //bMove = false;
        }

        private void radioRatio_CheckedChanged(object sender, EventArgs e)
        {
            m_nMethod = 0;
            //radioValue.Checked = true;
        }

        private void radioValue_CheckedChanged(object sender, EventArgs e)
        {
            m_nMethod = 1;
            //radioRatio.Checked = true;
        }

        private void radioPointSymbol_CheckedChanged(object sender, EventArgs e)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;

            if(radioPointSymbol.Checked)
            {
                nDecimalSymbol = 1;
                if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ".")
                {
                    string strMsg;
                    strMsg = "The Decimal Symbol of your system is \"" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "\" !";
                    strMsg = strMsg + "\nIf you change decimal symbol, program might not work normally!";
                    MessageBox.Show(strMsg, "Warning");
                }
            }

        }

        private void radioComaSymbol_CheckedChanged(object sender, EventArgs e)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            if(radioComaSymbol.Checked)
            {
                nDecimalSymbol = 2;
                if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator != ",")
                {
                    string strMsg;
                    strMsg = "The Decimal Symbol of your system is \"" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "\" !";
                    strMsg = strMsg + "\nIf you change decimal symbol, program might not work normally!";
                    MessageBox.Show(strMsg, "Warning");
                }
            }

        }

        private void textAccelerationVFBD_TextChanged(object sender, EventArgs e)
        {
            string strText = textAccelerationVFBD.Text;
            if (m_nDecimalSeperator == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textAccelerationVFBD.Text = strText;
            textAccelerationVFBD.Focus();
            textAccelerationVFBD.SelectionStart = strText.Length;
        }

        private void textPGARatioForBracktedDuration_TextChanged(object sender, EventArgs e)
        {
            string strText = textPGARatioForBracktedDuration.Text;
            if (m_nDecimalSeperator == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textPGARatioForBracktedDuration.Text = strText;
            textPGARatioForBracktedDuration.Focus();
            textPGARatioForBracktedDuration.SelectionStart = strText.Length;
        }

        private void textPGARatioForNOEC_TextChanged(object sender, EventArgs e)
        {
            string strText = textPGARatioForNOEC.Text;
            if (m_nDecimalSeperator == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textPGARatioForNOEC.Text = strText;
            textPGARatioForNOEC.Focus();
            textPGARatioForNOEC.SelectionStart = strText.Length;
        }

        private void textDefaultDampingRatio_TextChanged(object sender, EventArgs e)
        {
            string strText = textDefaultDampingRatio.Text;
            if (m_nDecimalSeperator == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textDefaultDampingRatio.Text = strText;
            textDefaultDampingRatio.Focus();
            textDefaultDampingRatio.SelectionStart = strText.Length;
        }

        private void textNumberDecimals_TextChanged(object sender, EventArgs e)
        {
            string strText = textNumberDecimals.Text;
            if (m_nDecimalSeperator == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textNumberDecimals.Text = strText;
            textNumberDecimals.Focus();
            textNumberDecimals.SelectionStart = strText.Length;
        }
    }
}
