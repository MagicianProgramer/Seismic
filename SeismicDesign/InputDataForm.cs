using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SeismicDesign
{
    public partial class InputDataForm : Form
    {

        private delegate void SafeCallDelegate(int command, string param);

        public string strFilePath;
        public double[] dBufferTime;
        public double[] dBufferAcceleration;
        public int nBufferCnt;
        public double dMaxTime = -1;
        public double dMinTime = 1000;
        public double dMaxAcceleration = -1;
        public double dMinAcceleration = 1000;

        public string strStatus;
        public int peek;

        
        public int nTotalLine;
        public string[] strFileText;
        public Thread thDisplayData;
        public Thread thImportData;

        public int nDecimalSeperator;

        //Log Write..
        public bool bLogSave = false;


        public delegate void ChildFormSnedDataHandler(int nC, double[] dT, double[] dA, double dMaxT, double dMinT, double dMaxA, double dMinA);
        public event ChildFormSnedDataHandler ChildFormEvent;

        public int m_nDecimalSeperator1 = 1;

        enum ControlMessage
        {
            msgReadData = 0,
            msgImportData = 1,
            msgDisplayText = 2,
            msgClose = 3,
            msgImportBtn = 4
        }



        public InputDataForm()
        {
            InitializeComponent();
        }
     
        public void SetParameter(string _filename, int nDecimal)
        {
            strFilePath = _filename;
            nDecimalSeperator = nDecimal;
        }

        private void LogText(string log)
        {
            if(bLogSave)
            {
                StreamWriter writer;
                writer = File.AppendText("Log.txt");
                writer.WriteLine(log);
                writer.Close();
            }

        }

        private void InputDataForm_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            //LogText("----------" + strFilePath + "------------");
            btnImportData.Enabled = false;
            int nExt = strFilePath.LastIndexOf(".");
            if(nExt > 0 && strFilePath.Length - nExt < 6)
            {
                string strExtension = strFilePath.Substring(nExt + 1, strFilePath.Length - nExt - 1);

                if (strExtension == "txt")
                    radioSingleAcceleration.Checked = true;
                else if (strExtension == "dat")
                    radioTimeAndAcceleration.Checked = true;
                else
                    radioMuitiAcceleration.Checked = true;

            }
            else
            {
                radioSingleAcceleration.Checked = true;
            }

            CultureInfo culture = CultureInfo.CurrentCulture;

            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
            {
                m_nDecimalSeperator1 = 1;
            }
            else
            {
                m_nDecimalSeperator1 = 2;
            }

            //LogText("File Read Start");
            strFileText = System.IO.File.ReadAllLines(strFilePath);
            nTotalLine = strFileText.Length;
            //LogText("File Total Line = " + nTotalLine.ToString());
            if (nTotalLine < 0)
            {
                MessageBox.Show("Empty Data!");
                this.Close();
            }

            this.textLastLine.Text = nTotalLine.ToString();

            //ThreadDisplayText();
            thDisplayData = new Thread(new ThreadStart(ThreadDisplayText));
            thDisplayData.Start();

        }

        private void btnImportData_Click(object sender, EventArgs e)
        {
            double nTS;
            Double.TryParse(textTimeStep.Text.ToString(), NumberStyles.Any,
                                          CultureInfo.CurrentCulture.NumberFormat, out nTS);
            if (nTS <= 0)
            {
                MessageBox.Show("Time Step Number must be more than 0");
                return;
            }
            //ThreadImportData();
            thImportData = new Thread(new ThreadStart(ThreadImportData));
            thImportData.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void ThreadDisplayText()
        {

            string strTemp = "";
            //LogText("ThreadDisplayText Begin");
            PostControlMsg((Int32)ControlMessage.msgReadData, "Reading");

            bool result;

            //LogText("NumberDecimalSeparator : " + nDecimalSeperator.ToString());
            //             string teststr1 = "0.1";
            //             string teststr2 = "0,1";
            // 
            //             double test1 = 0;
            //             double test2 = 0;
            //             try { test1 = Convert.ToDouble(teststr1); }
            //             catch (FormatException) { }
            // 
            //             try { test2 = Convert.ToDouble(teststr2); }
            //             catch (FormatException) { }


            for (int i = 0; i < nTotalLine; i++)
            {
                strFileText[i] = strFileText[i].Replace("\t", "    ");
                if(nDecimalSeperator == 1)
                    strFileText[i] = strFileText[i].Replace(",", ".");
                else
                    strFileText[i] = strFileText[i].Replace(".", ",");

//                 if(strFileText[i] == "")
//                 {
//                     LogText(i.ToString() + "--" + strFileText[i]);
//                 }

                strTemp += strFileText[i];
                string newLine = Environment.NewLine;
                //strTemp += '\n';
                strTemp += newLine;
                
                //PostControlMsg((Int32)ControlMessage.msgReadData, i.ToString());
            }

            //LogText(nTotalLine.ToString() + " line : " + strFileText[nTotalLine - 1]);
            //LogText("All Line Display");
            PostControlMsg((Int32)ControlMessage.msgReadData, "Ready");
            PostControlMsg((Int32)ControlMessage.msgDisplayText, strTemp);
            PostControlMsg((Int32)ControlMessage.msgImportBtn, "");

        }

        private void ThreadImportData()
        {
            LogText("Data Importing Begin");
            double nLastLine = 0;
            Double.TryParse(textLastLine.Text.ToString(), NumberStyles.Any,
                                          CultureInfo.CurrentCulture.NumberFormat, out nLastLine);

            if(nLastLine > nTotalLine)
            {
                LogText("Last Line Number isn't Correct!");
                MessageBox.Show("Last Line Number isn't Correct!");
                return;
            }

            double nFirstLine = 0;
            Double.TryParse(textFirstLine.Text.ToString(), NumberStyles.Any,
                                          CultureInfo.CurrentCulture.NumberFormat, out nFirstLine);

            nBufferCnt = (int)(nLastLine - nFirstLine) + 1;
            if (nBufferCnt > 32000)
            {
                LogText("Warning !\nMax Line Number of Import Data = 32000!\n Set Line Number 32000!");
                MessageBox.Show("Warning !\nMax Line Number of Import Data = 32000!\n Set Line Number 32000!");
                nBufferCnt = 32000;
            }

            double[] dTimeTemp = new double[32000];
            double[] dAccelerationTemp = new double[32000];
            
            int nStartLineNum = (int)nFirstLine;            

            PostControlMsg((Int32)ControlMessage.msgImportData, "Data Improting");
            LogText("Data Improting..");
            if (radioSingleAcceleration.Checked)
            {
                LogText("-SingleAcceleration-");
                double dTimeStep;
                Double.TryParse(textTimeStep.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out dTimeStep);
                //double dTimeStep = Convert.ToDouble(textTimeStep.Text.ToString());
                try
                {
                    for (int i = 0; i < nBufferCnt; i++)
                    {
                        dTimeTemp[i] = i * dTimeStep;

                         bool res = Double.TryParse(strFileText[i + nStartLineNum - 1], NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out dAccelerationTemp[i]);
                        //dAccelerationTemp[i] = Convert.ToDouble(strFileText[i + nStartLineNum - 1]);

                        if(res == false)
                        {
                            MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                            return;
                        }
                        if (dMaxTime < dTimeTemp[i])
                        {
                            dMaxTime = dTimeTemp[i];
                        }

                        if (dMinTime > dTimeTemp[i])
                        {
                            dMinTime = dTimeTemp[i];                            
                        }

                        if (dMaxAcceleration < dAccelerationTemp[i])
                        {
                            dMaxAcceleration = dAccelerationTemp[i];
                        }

                        if (dMinAcceleration > dAccelerationTemp[i])
                        {
                            dMinAcceleration = dAccelerationTemp[i];
                        }
                    }
                }
                catch (FormatException)
                {
                    LogText("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    return;
                }
                catch (OverflowException)
                {
                    LogText("Error!\nData isn't correct!\nCheck the Data and retry!");
                    MessageBox.Show("Error!\nData isn't correct!\nCheck the Data and retry!");
                    return;
                }              
            }
            else if(radioTimeAndAcceleration.Checked)
            {
                LogText("-TimeAndAcceleration-");
                try
                {
                    for (int i = 0; i < nBufferCnt; i++)
                    {
                        string[] words = strFileText[i + nStartLineNum - 1].Split(' ');
                        int no = 0;
                        
                        int[] index = new int[words.Length];
                        int ii = -1;
                        foreach (var word in words)
                        {
                            ii++;
                            if (word == "")
                                continue;

                            index[no] = ii;
                            no++;
                        }

                        if (no != 2)
                        {
                            LogText("Error!\nImport Method isn't correct or Data isn't correct!\nReset import method and retry!");
                            MessageBox.Show("Error!\nImport Method isn't correct or Data isn't correct!\nReset import method and retry!");
                            return;
                        }

                        bool res1 = Double.TryParse(words[index[0]], NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out dTimeTemp[i]);
                        //dTimeTemp[i] = Convert.ToDouble(words[index[0]]);
                        bool res2 = Double.TryParse(words[index[1]], NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out dAccelerationTemp[i]);
                        //dAccelerationTemp[i] = Convert.ToDouble(words[index[1]]);

                        if (res1 == false || res2 == false)
                        {
                            LogText("Error!\nImport Method isn't correct!\nReset import method and retry!");
                            MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                            return;
                        }

                        if (dMaxTime < dTimeTemp[i])
                        {
                            dMaxTime = dTimeTemp[i];
                        }

                        if (dMinTime > dTimeTemp[i])
                        {
                            dMinTime = dTimeTemp[i];
                        }

                        if (dMaxAcceleration < dAccelerationTemp[i])
                        {
                            dMaxAcceleration = dAccelerationTemp[i];
                        }

                        if (dMinAcceleration > dAccelerationTemp[i])
                        {
                            dMinAcceleration = dAccelerationTemp[i];
                        }
                    }
                }
                catch (FormatException)
                {
                    LogText("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    return;
                }
                catch (OverflowException)
                {
                    LogText("Error!\nData isn't correct!\nCheck the Data and retry!");
                    MessageBox.Show("Error!\nData isn't correct!\nCheck the Data and retry!");
                    return;
                }
            }
            else if (radioMuitiAcceleration.Checked)
            {
                LogText("-MuitiAcceleration-");
                double dTimeStep;
                Double.TryParse(textTimeStep.Text.ToString(), NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out dTimeStep);
                //double dTimeStep = Convert.ToDouble(textTimeStep.Text.ToString());
                
                try
                {
                    int no = 0;
                    for (int i = 0; i < nBufferCnt; i++)
                    {
                        string[] words = strFileText[i + nStartLineNum - 1].Split(' ');
                        bool res = true;
                        foreach (var word in words)
                        {
                            if (word == "")
                                continue;
                            dTimeTemp[no] = no * dTimeStep;
                            
                            res = Double.TryParse(word, NumberStyles.Any,
                                              CultureInfo.CurrentCulture.NumberFormat, out dAccelerationTemp[no]);

                            //dAccelerationTemp[i] = Convert.ToDouble(word);

                            if (dMaxTime < dTimeTemp[no])
                            {
                                dMaxTime = dTimeTemp[no];
                            }

                            if (dMinTime > dTimeTemp[no])
                            {
                                dMinTime = dTimeTemp[no];
                            }

                            if (dMaxAcceleration < dAccelerationTemp[no])
                            {
                                dMaxAcceleration = dAccelerationTemp[no];
                            }

                            if (dMinAcceleration > dAccelerationTemp[no])
                            {
                                dMinAcceleration = dAccelerationTemp[no];
                            }


                            no++;
                        }
                        if (res == false)
                        {
                            LogText("Error!\nData isn't correct!\nCheck the Data and retry!");
                            MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                            return;
                        }
                    }

                    nBufferCnt = no;
                }
                catch (FormatException)
                {
                    LogText("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    return;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Error!\nImport Method isn't correct!\nReset import method and retry!");
                    MessageBox.Show("Error!\nData isn't correct!\nCheck the Data and retry!");
                    return;
                }
            }

            dBufferAcceleration = new double[nBufferCnt];
            dBufferTime = new double[nBufferCnt];

            Array.Copy(dAccelerationTemp, 0, dBufferAcceleration, 0, nBufferCnt);
            Array.Copy(dTimeTemp, 0, dBufferTime, 0, nBufferCnt);


            LogText("Data 10:");

            int logStep = nBufferCnt / 10;

//             for (int i = 0; i < 10; i ++)
//             {
//                 string strLog = (i * logStep).ToString() + " line: " + dTimeTemp[i * logStep].ToString() + "  " + dAccelerationTemp[i * logStep].ToString();
//                 LogText(strLog);
//             }

            LogText("Data Importing Finish");


            this.ChildFormEvent(nBufferCnt, dBufferTime, dBufferAcceleration, dMaxTime, dMinTime, dMaxAcceleration, dMinAcceleration);
            //MessageBox.Show("Successfully Data imported!");

            this.PostControlMsg((Int32)ControlMessage.msgClose, "Finish");
        }
        
        private void radioSingleAcceleration_CheckedChanged(object sender, EventArgs e)
        {
            this.textTimeStep.Enabled = true;
        }

        private void radioTimeAndAcceleration_CheckedChanged(object sender, EventArgs e)
        {
            this.textTimeStep.Enabled = false;
        }

        private void radioMuitiAcceleration_CheckedChanged(object sender, EventArgs e)
        {
            this.textTimeStep.Enabled = true;
        }

        private void PostControlMsg(int command, string param)
        {
            switch (command)
            {
                case (Int32)ControlMessage.msgReadData:
                    strStatus = param;                  
                    break;
                case (Int32)ControlMessage.msgImportData:
                    strStatus = param;
                    break;
                case (Int32)ControlMessage.msgDisplayText:
                    if (textBox1.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        textBox1.Invoke(d, new object[] { command, param });
                    }
                    else
                        //labelFileText.Text = param;
                        textBox1.Text = param;
                    break;
                case (Int32)ControlMessage.msgClose:
                    if (this.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        this.Invoke(d, new object[] { command, param });
                    }
                    else
                        Close();
                    break;
                case (Int32)ControlMessage.msgImportBtn:
                    if (btnImportData.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        btnImportData.Invoke(d, new object[] { command, param });
                    }
                    else
                        btnImportData.Enabled = true;
                    break;
            }
        }


        private void textFirstLine_TextChanged(object sender, EventArgs e)
        {
            string strText = textFirstLine.Text;
            if (m_nDecimalSeperator1 == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator1 == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textFirstLine.Text = strText;
            textFirstLine.Focus();
            textFirstLine.SelectionStart = strText.Length;

            double nfirstLine;
            Double.TryParse(textFirstLine.Text.ToString(), NumberStyles.Any,
                                          CultureInfo.CurrentCulture.NumberFormat, out nfirstLine);
            if(nfirstLine < 1)
            {
                MessageBox.Show("FirstLine Number isn't less than 1");
                return;
            }
        }

        private void textLastLine_TextChanged(object sender, EventArgs e)
        {
            string strText = textLastLine.Text;
            if (m_nDecimalSeperator1 == 1)
            {
                strText = strText.Replace(",", ".");

                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator1 == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textLastLine.Text = strText;
            textLastLine.Focus();
            textLastLine.SelectionStart = strText.Length;

            double nLastLine;
            Double.TryParse(textLastLine.Text.ToString(), NumberStyles.Any,
                                          CultureInfo.CurrentCulture.NumberFormat, out nLastLine);
            if (nLastLine < 1)
            {
                MessageBox.Show("LastLine Number isn't less than 1");
                return;
            }
        }

        private void textTimeStep_TextChanged(object sender, EventArgs e)
        {      

            string strText = textTimeStep.Text;
            if (m_nDecimalSeperator1 == 1)
            {
                strText = strText.Replace(",", ".");
                
                int IndexValue = strText.IndexOf(".");
                string strSub = "";
                if(strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(".", "");
            }
            else if (m_nDecimalSeperator1 == 2)
            {
                strText = strText.Replace(".", ",");

                int IndexValue = strText.IndexOf(",");

                string strSub = "";
                if (strText.Length - IndexValue - 1 != 0)
                    strSub = strText.Substring(IndexValue + 1, strText.Length - IndexValue - 1);
                strText = strText.Substring(0, IndexValue + 1) + strSub.Replace(",", "");
            }

            textTimeStep.Text = strText;
            textTimeStep.Focus();
            textTimeStep.SelectionStart = strText.Length;
        }
    }
}
