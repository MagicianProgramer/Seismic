using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
namespace SeismicDesign
{
    public partial class SignalDesignForm : Form
    {
        LoadingForm loadingForm = new LoadingForm();
        BackgroundWorker newThread;
        BackgroundWorker disThread;
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private delegate void SafeCallDelegate(int command);

        //         private Thread thInputMotion;
        //         private Thread thDraw;

        private int m_nSMDComboMotionIndex = -1;
        private int m_nOldSMDComboMotionIndex = -1;

        public int m_nDecimalSeperator = 1;

        private String[] m_strInputMotionFiles = new String[11];
        private int m_nCountMotion = 0;
        private String[] m_strListName = new String[11];
        private double[][] _T = new double[11][];
        private double[][] _A = new double[11][];
        private double[] delta_t = new double[11];
        private int[] m_nCnt = new int[11];
        private int[] m_listIndex = new int[11];

        private double[] m_dMaxT = new double[11];
        private double[] m_dMinT = new double[11];

        private double[] m_dMaxA = new double[11];
        private double[] m_dMinA = new double[11];

        private double[] m_dMaxV = new double[11];
        private double[] m_dMinV = new double[11];

        private double[] m_dMaxD = new double[11];
        private double[] m_dMinD = new double[11];

        private double[] m_dMaxFFTSA = new double[11];
        private double[] m_dMinFFTSA = new double[11];

        private double[] m_dMaxFFTSF = new double[11];
        private double[] m_dMinFFTSF = new double[11];

        private double[] m_dMaxPowerFFTSA = new double[11];
        private double[] m_dMinPowerFFTSA = new double[11];

        private double[] m_dMaxAI = new double[11];
        private double[] m_dMinAI = new double[11];

        private double[] m_dMaxSA = new double[11];
        private double[] m_dMinSA = new double[11];

        private double[] m_dMaxSV = new double[11];
        private double[] m_dMinSV = new double[11];

        private double[] m_dMaxSD = new double[11];
        private double[] m_dMinSD = new double[11];

        private double m_dMaxSE = -1;
        private double m_dMinSE = 1000;

        private double[][] _V = new double[11][];
        private double[][] _D = new double[11][];
        private double[] _meanS_A = null;

        private double[][] _S_A = new double[11][];
        private double[][] _S_V = new double[11][];
        private double[][] _S_D = new double[11][];
        private int[] nSLen = new int[11];

        private double[] _PGA = new double[11];
        private double[] _PGV = new double[11];
        private double[] _PGD = new double[11];
        private double[] _A_ams = new double[11];
        private double[] _V_ams = new double[11];
        private double[] _D_ams = new double[11];
        private double[] _Ic = new double[11];
        private double[] _SED = new double[11];
        private double[] _CAV = new double[11];
        private int[] _NoECA = new int[11];
        private double[] _AI_MP = new double[11];

        private double[] _D5AI = new double[11];
        private double[] _D95AI = new double[11];

        private int[] _D5AI_pos = new int[11];
        private int[] _D95AI_pos = new int[11];


        private double[] _sus_duration = new double[11];

        private double[] _Bracked_value = new double[11];
        private double[] _brack_duration = new double[11];

        private double[] _SusA3 = new double[11];
        private double[] _SusA5 = new double[11];

        private double[] _SusV3 = new double[11];
        private double[] _SusV5 = new double[11];

        private double[] _SusD3 = new double[11];
        private double[] _SusD5 = new double[11];


        private double[] _Alfa = new double[11];
        private double[] _Error = new double[11];

        private double[][] FFT_A = new double[11][];
        private double[][] FFT_F = new double[11][];

        private double[][] PowerFFT_A = new double[11][];

        private double[] delta_f = new double[11];

        private double[][] _AI = new double[11][];

        private double[] m_dMaxSA_Alfa1 = new double[11];
        private double[] m_dMinSA_Alfa1 = new double[11];

        private double[] m_dMaxSA_Alfa2 = new double[11];
        private double[] m_dMinSA_Alfa2 = new double[11];

        private double[] m_dMaxSA_Alfa0 = new double[11];
        private double[] m_dMinSA_Alfa0 = new double[11];

        private double[][] _S_A_Alfa0 = new double[11][];
        private double[][] _S_A_Alfa1 = new double[11][];
        private double[][] _S_A_Alfa2 = new double[11][];

        private string[] _SMD_AccInterval = new string[11];
        private string[] _SMD_AccTimeInterval = new string[11];

        private string[] _SMD_VccInterval = new string[11];
        private string[] _SMD_VccTimeInterval = new string[11];

        private string[] _SMD_DisInterval = new string[11];
        private string[] _SMD_DisTimeInterval = new string[11];

        private string[] _SMD_AIInterval = new string[11];
        private string[] _SMD_AITimeInterval = new string[11];
        

        private double m_dNumberOfDecimals = 5;
        private double m_dDefaultDampingRatio = 5;
        private double m_dPGARatioForNOEC = 65;
        private double m_dPGARatioForBD = 5;
        private double m_dAccelerationValueForBD = 0.05;
        private double m_dSDSCoefficient = 0.9;
        private int m_nMethodBD = 0;

        private double _TA;
        private double _TB;

        private double[] _Sae0;
        private double[] _Sae_T;

        private int nDesign = -1;

        private int m_nLenFourier = 32768;

        private int m_nSelSpectList = 0;
        private int m_nSelMethodList = 0;

        private string m_MSG = "";

        private int m_nSMDScrollPos = -1;

        private Color m_colorPicBack = Color.FromArgb(255, 255, 255);
        private Boolean isLoaded;

        public System.Windows.Forms.PictureBox DrawCtrl = null;

        public bool bLogSave = true;

        enum ControlMessage
        {
            msgDI_MotionList = 0,
            msgDI_ListS_AVD,
            msgDI_ComboS_AVD,
            msgSMD_ListTA,
            msgSMD_ListTV,
            msgSMD_ListTD,
            msgSMD_MotionDataCombo,
            msgSMD_ListTA_SCROOL,
            msgMP_MotionList,
            msgMP_MotionDataCombo,
            msgMP_ListIntensityParameters,
            msgMP_ListDurationParameters,
            msgMP_ListValues,
            msgMP_BracketDuration,

            msgSM_MD_MotionDataCombo,
            msgSM_MS_MotionDataCombo,
            msgSM_SM_Alfa,
            msgSM_MD_Alfa,
            msgSM_MotionList,

            msgSM_ListTarget,

            msgDS_MotionList,
            msgDS_ListMean,
            msgSMD_TEST,

            msgFormRefresh
        }

        public SignalDesignForm()
        {
            InitializeComponent();

            newThread = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            newThread.DoWork += new DoWorkEventHandler(PerformReading);
            //newThread.ProgressChanged += new ProgressChangedEventHandler(PerformReading);
            newThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadingCompleted);

            disThread = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            disThread.DoWork += new DoWorkEventHandler(DisThread);
            disThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DisThread_Completed);

            picDI_Acceleration.Paint += DrawAllGraph_DIAcceleration;
            picDI_Spectr.Paint += DrawAllGraph_DISpectr;

            //             picDI_Acceleration.Paint += DrawAllGraph_DISpectr;
            //             picDI_Spectr.Paint += DrawAllGraph_DIAcceleration;

            picSMD_Acceleration.Paint += DrawAllGraph_SMDAcceleration;
            picSMD_Velocity.Paint += DrawAllGraph_SMDVelocity;
            picSMD_Displacement.Paint += DrawAllGraph_SMDDisplacement;
            picSMD_AriasIntensity.Paint += DrawAllGraph_SMDAriasIntensity;


            //Spectral Matching..
            picSM_TargetSpectrum.Paint += DrawAllGraph_SM_TargetSpectrum;

            picSM_SM_Acceleration.Paint += DrawAllGraph_SM_SMAcceration;
            picSM_SM_Spectral.Paint += DrawAllGraph_SM_SMSpectral;

            picSM_MD_Acceleration.Paint += DrawAllGraph_SM_MDAcceration;
            picSM_MD_Spectral.Paint += DrawAllGraph_SM_MDSpectral;

            picSM_MS_Acceleration.Paint += DrawAllGraph_SM_MSAcceration;
            picSM_MS_Spectral.Paint += DrawAllGraph_SM_MSSpectral;

            picMP_FrequencyGraph.Paint += DrawAllGraph_MP_Frequency;
            picMP_AriasIntensity.Paint += DrawAllGraph_MP_AriasIntensity;
            picMP_Acceleration.Paint += DrawAllGraph_MP_Acceleration;

            picDS_TargetSpectrum.Paint += DrawAllGraph_DS_Target;
            picDS_Last.Paint += DrawAllGraph_DSAll;

            this.Resize += new EventHandler(FormResizeEvent);
            tabControl1.SelectedIndexChanged += new EventHandler(TabCtrlEvents);
            tabControlSM.SelectedIndexChanged += new EventHandler(SubTabCtrlEvents);

            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void FormResizeEvent(object sender, EventArgs e)
        {
            this.Refresh();
            //PostAllMessage();
        }

        private void TabCtrlEvents(object sender, EventArgs e)
        {
            //labelSMD_MotionDatas.Refresh();
            //comboSMD_MotionDatas.Refresh();
            this.Refresh();

            
        }

        private void SubTabCtrlEvents(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void _initVariableParameter()
        {
            for (int i = 0; i < 11; i++)
            {
                m_dMaxT[i] = m_dMaxA[i] = m_dMaxV[i] = m_dMaxD[i] = m_dMaxFFTSA[i] = m_dMaxFFTSF[i] = m_dMaxPowerFFTSA[i] = m_dMaxAI[i] =
                    m_dMaxSA[i] = m_dMaxSV[i] = m_dMaxSD[i] = m_dMaxSA_Alfa1[i] = m_dMaxSA_Alfa2[i] = m_dMaxSA_Alfa0[i] = -1;

                m_dMinT[i] = m_dMinA[i] = m_dMinV[i] = m_dMinD[i] = m_dMinFFTSA[i] = m_dMinFFTSF[i] = m_dMinPowerFFTSA[i] = m_dMinAI[i] =
                   m_dMinSA[i] = m_dMinSV[i] = m_dMinSD[i] = m_dMinSA_Alfa1[i] = m_dMinSA_Alfa2[i] = m_dMinSA_Alfa0[i] = 1000;

                m_nCnt[i] = 0;

                _SMD_AccInterval[i] = "0";
                _SMD_AccTimeInterval[i] = "0";

                _SMD_VccInterval[i] = "0";
                _SMD_VccTimeInterval[i] = "0";

                _SMD_DisInterval[i] = "0";
                _SMD_DisTimeInterval[i] = "0";

                _SMD_AIInterval[i] = "0";
                _SMD_AITimeInterval[i] = "0";
            }

        }

        private void SignalDesignForm_Load(object sender, EventArgs e)
        {
            LoadingForm loading = new LoadingForm();
            Skin(Color.FromArgb(245, 245, 245));
            loading.Show();

            this.CenterToScreen();

            comboDI_SpectList.SelectedIndex = 0;
            comboDL_MethodList.SelectedIndex = 0;

            comboSM_SOILTYPE.SelectedIndex = 0;
            comboDS_SOILTYPE.SelectedIndex = 0;

            comboMP_F_P_S.SelectedIndex = 0;

            _initVariableParameter();



            CultureInfo culture = CultureInfo.CurrentCulture;

            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
            {
                m_nDecimalSeperator = 1;
            }
            else
            {
                m_nDecimalSeperator = 2;
            }

            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            tabControl1.SelectedTab = tabDesignSpectrum;
            tabControl1.SelectedTab = tabSpectralMatching;
            tabControl1.SelectedTab = tabMotionParameters;
            tabControl1.SelectedTab = tabStrongMotionData;
            tabControl1.SelectedTab = tabDataInput;
        }

        private double MaxAbsValue(double first, double second)
        {
            double res = (Math.Abs(first) > Math.Abs(second)) ? Math.Abs(first) : Math.Abs(second);
            return res;
        }

        private double FixDecimal1(double num)
        {
            return Math.Floor(num * 10) / 10;
        }

        private double FixDecimal2(double num)
        {
            return Math.Floor(num * 100) / 100;
        }

        private double FixDecimal3(double num)
        {
            return Math.Floor(num * Math.Pow(10, m_dNumberOfDecimals)) / Math.Pow(10, m_dNumberOfDecimals);
        }

        private void btnDI_AddNewMotion_Click(object sender, EventArgs e)
        {
            if (m_nCountMotion == 11)
            {
                //MessageBox.Show("You can enter up to 11.\nDelete one if you want to enter more.");
                return;
            }
            this.openFileDialog_InputMotion.FileName = "*.dat; *.txt; *.AT2";
            //this.openFileDialog_InputMotion.Multiselect = true;
            if (this.openFileDialog_InputMotion.ShowDialog() == DialogResult.OK)
            {

                m_strInputMotionFiles[m_nCountMotion] = openFileDialog_InputMotion.FileName;

                m_strListName[m_nCountMotion] = openFileDialog_InputMotion.SafeFileName;
                InputDataForm formInputData;
                formInputData = new InputDataForm();
                formInputData.SetParameter(m_strInputMotionFiles[m_nCountMotion], m_nDecimalSeperator);


                formInputData.ChildFormEvent += EventMethodInputData;
                formInputData.Show();
            }

        }

        private void btnDS_AddNewMotion_Click(object sender, EventArgs e)
        {
            btnDI_AddNewMotion_Click(sender, e);
        }

        private void btnSM_AddNewMotion_Click(object sender, EventArgs e)
        {
            btnDI_AddNewMotion_Click(sender, e);
        }

        /// ///////////////////////////// Data Input //////////////////////////////////////////////////
        private void DrawAllGraph_DIAcceleration(object sender, PaintEventArgs e)
        {
            float[] padding = { 80, 80, 50, 50 };
            if (isLoaded)
            {
                //for (int i = 0; i < m_nCountMotion; i ++)                
                //int i = m_nCountMotion - 1;
                int[] nShowList = new int[checkedListDI_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDI_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }

                //DrawCtrl = picDI_Acceleration;
                Bitmap bit = null;

                if (nn > 0)
                {
                    bit = DrawAllGraph(sender, e, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, picDI_Acceleration.Width, picDI_Acceleration.Height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding, 0);
                    picDI_Acceleration.Image = bit;
                }
                else
                {
                    picDI_Acceleration.Image = null;
                }

                textDI_AccInterval.Refresh();
                textDI_TimeInterval.Refresh();

            }
        }

        private void DrawAllGraph_DISpectr(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {

                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[checkedListDI_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDI_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }
                //DrawCtrl = picDI_Spectr;
                if (nn > 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 0, nShowList, picDI_Spectr.Width, picDI_Spectr.Height, padding, m_nSelSpectList, false);
                    //if(bit != null)
                    {
                        picDI_Spectr.Image = bit;
                    }
                }
                else
                {
                    picDI_Spectr.Image = null;
                }

                textDI_SaInterval.Refresh();
                textDI_SaPeriodInterval.Refresh();

            }
        }

        /////////////////////////////  Strong Motion Data  //////////////////////////////////////////////////////
        private void DrawAllGraph_SMDAcceleration(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = m_nSMDComboMotionIndex;
                //DrawCtrl = picSMD_Acceleration;
                Bitmap bit = null;
                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraph(sender, e, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, picSMD_Acceleration.Width, picSMD_Acceleration.Height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                    picSMD_Acceleration.Image = bit;
                }
                else
                {
                    picSMD_Acceleration.Image = null;
                }

                textSMD_AccInterval.Refresh();
                textSMD_AccTimeInterval.Refresh();
            }
        }

        private void DrawAllGraph_SMDVelocity(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = m_nSMDComboMotionIndex;
                //DrawCtrl = picSMD_Velocity;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "Velocity (cm/s)", _V, _T, m_nCnt, nShowList, picSMD_Velocity.Width, picSMD_Velocity.Height, m_dMaxV, m_dMinV, m_dMaxT, m_dMinT, padding);
                    picSMD_Velocity.Image = bit;
                }
                else
                {
                    picSMD_Velocity.Image = null;
                }

                textSMD_VccInterval.Refresh();
                textSMD_VccTimeInterval.Refresh();
            }
        }

        private void DrawAllGraph_SMDDisplacement(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = m_nSMDComboMotionIndex;
                //DrawCtrl = picSMD_Displacement;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "Displacement (cm)", _D, _T, m_nCnt, nShowList, picSMD_Displacement.Width, picSMD_Displacement.Height, m_dMaxD, m_dMinD, m_dMaxT, m_dMinT, padding);
                    picSMD_Displacement.Image = bit;
                }
                else
                {
                    picSMD_Displacement.Image = null;
                }

                textSMD_DisInterval.Refresh();
                textSMD_DisTimeInterval.Refresh();
            }
        }

        private void DrawAllGraph_SMDAriasIntensity(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = m_nSMDComboMotionIndex;
                //DrawCtrl = picSMD_AriasIntensity;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "AriasIntensity (%)", _AI, _T, m_nCnt, nShowList, picSMD_AriasIntensity.Width, picSMD_AriasIntensity.Height, m_dMaxAI, m_dMinAI, m_dMaxT, m_dMinT, padding);
                    picSMD_AriasIntensity.Image = bit;
                }
                else
                {
                    picSMD_AriasIntensity.Image = null;
                }

                textSMD_AIInterval.Refresh();
                textSMD_AITimeInterval.Refresh();
            }
        }

        /////////////////////////////  Spectral Matching  //////////////////////////////////////////////////////
        private void DrawAllGraph_SM_SMAcceration(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[checkedListSM_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListSM_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListSM_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }
                //DrawCtrl = picSM_SM_Acceleration;


                if (nn > 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "Accerelation (g)", _A, _T, m_nCnt, nShowList, picSM_SM_Acceleration.Width, picSM_SM_Acceleration.Height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                    picSM_SM_Acceleration.Image = bit;
                }
                else
                {
                    picSM_SM_Acceleration.Image = null;
                }

            }
        }

        private void DrawAllGraph_SM_SMSpectral(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 120, 30, 50, 50 };
                int[] nShowList = new int[checkedListDI_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDI_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }

                //DrawCtrl = pictSM_SM_Spectral;

                if (nn > 0)
                {
                    Bitmap bit = null;
                    if (bTargetGenerated1)
                        bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 3, nShowList, picSM_SM_Spectral.Width, picSM_SM_Spectral.Height, padding, 0, true);
                    else
                        bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 3, nShowList, picSM_SM_Spectral.Width, picSM_SM_Spectral.Height, padding, 0, false);
                    picSM_SM_Spectral.Image = bit;
                }
                else
                {
                    picSM_SM_Spectral.Image = null;
                }

            }
        }

        private void DrawAllGraph_SM_MDAcceration(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MDMotionDatas.SelectedIndex;
                DrawCtrl = picSM_MD_Acceleration;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, picSM_MD_Acceleration.Width, picSM_MD_Acceleration.Height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                    picSM_MD_Acceleration.Image = bit;
                }
                else
                {
                    picSM_MD_Acceleration.Image = null;
                }
            }
        }

        private void DrawAllGraph_SM_MDSpectral(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 120, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MDMotionDatas.SelectedIndex;
                //DrawCtrl = pictSM_MD_Spectral;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 1, nShowList, picSM_MD_Spectral.Width, picSM_MD_Spectral.Height, padding, 0, true);
                    picSM_MD_Spectral.Image = bit;
                }
                else
                {
                    picSM_MD_Spectral.Image = null;
                }
            }
        }

        private void DrawAllGraph_SM_MSAcceration(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MSMotionDatas.SelectedIndex;
                //DrawCtrl = picSM_MS_Acceleration;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, picSM_MS_Acceleration.Width, picSM_MS_Acceleration.Height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                    picSM_MS_Acceleration.Image = bit;
                }
                else
                {
                    picSM_MS_Acceleration.Image = null;
                }
            }
        }

        private void DrawAllGraph_SM_MSSpectral(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 120, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MSMotionDatas.SelectedIndex;
                //DrawCtrl = pictSM_MS_Spectral;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 2, nShowList, picSM_MS_Spectral.Width, picSM_MS_Spectral.Height, padding, 0, true);
                    picSM_MS_Spectral.Image = bit;
                }
                else
                {
                    picSM_MS_Spectral.Image = null;
                }
            }
        }

        private void DrawAllGraph_SM_TargetSpectrum(object sender, PaintEventArgs e)
        {

            if (_Sae0 != null && bTargetGenerated1)
            {
                float[] padding = { 10, 10, 10, 30 };
                //DrawCtrl = picSM_TargetSpectrum;
                Bitmap bit = null;
                bit = DrawAllGraphTarget(sender, e, Color.Blue, _Sae0, _Sae_T, nSLen[m_nCountMotion - 1], picSM_TargetSpectrum.Width, picSM_TargetSpectrum.Height, m_dMaxSE, m_dMinSE, padding);
                picSM_TargetSpectrum.Image = bit;
            }
            else
            {
                picSM_TargetSpectrum.Image = null;
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        private void DrawAllGraph_MP_Frequency(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                int[] nShowList = new int[1];
                nShowList[0] = comboMP_MotionDatas.SelectedIndex;
                int style = comboMP_F_P_S.SelectedIndex;

                if (style == 0)
                {
                    float[] padding = { 80, 30, 50, 50 };
                    int[] cnt = new int[11];
                    for (int i = 0; i < 11; i++)
                    {
                        cnt[i] = m_nLenFourier;
                    }
                    //DrawCtrl = picMP_FrequencyGraph;

                    if (nShowList[0] >= 0)
                    {
                        Bitmap bit = null;
                        bit = DrawAllGraph(sender, e, Color.Blue, "Fourier Amplitude", FFT_A, FFT_F, cnt, nShowList, picMP_FrequencyGraph.Width, picMP_FrequencyGraph.Height, m_dMaxFFTSA, m_dMinFFTSA, m_dMaxFFTSF, m_dMinFFTSF, padding, 3);
                        picMP_FrequencyGraph.Image = bit;
                    }
                    else
                    {
                        picMP_FrequencyGraph.Image = null;
                    }
                }
                else if (style == 1)
                {
                    float[] padding = { 80, 30, 50, 50 };
                    int[] cnt = new int[11];
                    for (int i = 0; i < 11; i++)
                    {
                        cnt[i] = m_nLenFourier;
                    }
                    //DrawCtrl = picMP_FrequencyGraph;
                    if (nShowList[0] >= 0)
                    {
                        Bitmap bit = null;
                        bit = DrawAllGraph(sender, e, Color.Blue, "Power Spectrum", PowerFFT_A, FFT_F, cnt, nShowList, picMP_FrequencyGraph.Width, picMP_FrequencyGraph.Height, m_dMaxPowerFFTSA, m_dMinPowerFFTSA, m_dMaxFFTSF, m_dMinFFTSF, padding, 3);
                        picMP_FrequencyGraph.Image = bit;
                    }
                    else
                    {
                        picMP_FrequencyGraph.Image = null;
                    }
                }
                else if (style == 2)
                {
                    float[] padding = { 80, 30, 50, 50 };
                    //DrawCtrl = picMP_FrequencyGraph;


                    if (nShowList[0] >= 0)
                    {
                        Bitmap bit = null;
                        bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 0, nShowList, picMP_FrequencyGraph.Width, picMP_FrequencyGraph.Height, padding, 0, false);
                        picMP_FrequencyGraph.Image = bit;
                    }
                    else
                    {
                        picMP_FrequencyGraph.Image = null;
                    }

                }
            }
        }

        private void DrawAllGraph_MP_AriasIntensity(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboMP_MotionDatas.SelectedIndex;
                //DrawCtrl = picMP_AriasIntensity;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "AriasIntensity (%)", _AI, _T, m_nCnt, nShowList, picMP_AriasIntensity.Width, picMP_AriasIntensity.Height, m_dMaxAI, m_dMinAI, m_dMaxT, m_dMinT, padding, 1);
                    picMP_AriasIntensity.Image = bit;
                }
                else
                {
                    picMP_AriasIntensity.Image = null;
                }
            }
        }

        private void DrawAllGraph_MP_Acceleration(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboMP_MotionDatas.SelectedIndex;
                //DrawCtrl = picMP_Acceleration;

                if (nShowList[0] >= 0)
                {
                    Bitmap bit = null;
                    bit = DrawAllGraph(sender, e, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, picMP_Acceleration.Width, picMP_Acceleration.Height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding, 2);
                    picMP_Acceleration.Image = bit;
                }
                else
                {
                    picMP_Acceleration.Image = null;
                }
            }
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////
        private void DrawAllGraph_DS_Target(object sender, PaintEventArgs e)
        {

            if (_Sae0 != null && bTargetGenerated2)
            {
                float[] padding = { 10, 10, 10, 30 };
                //DrawCtrl = picDS_TargetSpectrum;
                Bitmap bit = null;
                bit = DrawAllGraphTarget(sender, e, Color.Blue, _Sae0, _Sae_T, nSLen[m_nCountMotion - 1], picDS_TargetSpectrum.Width, picDS_TargetSpectrum.Height, m_dMaxSE, m_dMinSE, padding);

                picDS_TargetSpectrum.Image = bit;
            }
            else
            {
                picDS_TargetSpectrum.Image = null;
            }
        }

        private void DrawAllGraph_DSAll(object sender, PaintEventArgs e)
        {
            if (isLoaded)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[checkedListDS_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDS_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDS_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }

                }
                //DrawCtrl = picDS_Last;


                if (nn > 0)
                {
                    Bitmap bit = null;
                    if (bTargetGenerated2)
                        bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 3, nShowList, picDS_Last.Width, picDS_Last.Height, padding, -2, true);
                    else
                        bit = DrawAllGraph_SM_Spectral(sender, e, Color.Blue, 3, nShowList, picDS_Last.Width, picDS_Last.Height, padding, -2, false);
                    picDS_Last.Image = bit;
                }
                else
                {
                    picDS_Last.Image = null;
                }
            }
        }

        private Bitmap DrawAllGraph_SM_Spectral(object sender, PaintEventArgs e, Color color, int matchingType, int[] nShowList, float nWidth, float nHeight, float[] Padding, int nShow, Boolean bDrawSae)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new PaintEventHandler(this.DrawAllGraph_SMDAcceleration), new
                object[] { sender, e });
                return null;
            }

            float padding_left = 50;
            float padding_right = 50;
            float padding_top = 30;
            float padding_bottom = 30;

            if (Padding != null)
            {
                padding_left = Padding[0];
                padding_top = Padding[1];
                padding_right = Padding[2];
                padding_bottom = Padding[3];
            }

            float xSperat = 0;
            float ySperat = 0;
            float xStep = 0;
            float yStep = 0;
            ///////////////////////////////////////////////////////////////          
            Boolean bSE = false;
            Boolean bSE0 = false;
            if (bDrawSae)
            {
                if (nShowList.Length == 1)
                {
                    if (_S_A_Alfa1[nShowList[0]] != null && matchingType == 1)
                        bSE = true;

                    if (_S_A_Alfa2[nShowList[0]] != null && matchingType == 2)
                        bSE = true;
                }

                if (_Sae0 != null)
                    bSE0 = true;
            }

            double[][] xBufferA = new double[nShowList.Length][];
            double[][] xBufferV = new double[nShowList.Length][];
            double[][] xBufferD = new double[nShowList.Length][];
            double[] xBufferSE = new double[nSLen[nShowList[0]]];
            double[] xBufferSE0 = new double[nSLen[nShowList[0]]];
            int nScale = 0;
            ///////////////////////////////////////////////////////////////
            //Get MaxA in datas;
            double totalMaxA = -1, totalMinA = 1000;
            double temMax = -1;
            double temMin = 1000;

            double periodTime;
            Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out periodTime);

            double totalMaxT = periodTime;
            //totalMaxT = Convert.ToDouble(textDI_MaxPeriod.Text.ToString());
            double totalMinT = 0;
            double deltT = 0.02;
            int nCnt = (int)(totalMaxT / deltT);
            for (int ls = 0; ls < nShowList.Length; ls++)
            {
                int ii = nShowList[ls];
                double nTemMaxA = -1;
                double nTemMinA = 1000;
                if (nShow == -1 || nShow == 0 || nShow == -2)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSA[ii]) ? nTemMaxA : m_dMaxSA[ii];
                    nTemMinA = (nTemMinA < m_dMinSA[ii]) ? nTemMinA : m_dMinSA[ii];
                }


                if (nShow == -1 || nShow == 1)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSV[ii]) ? nTemMaxA : m_dMaxSV[ii];
                    nTemMinA = (nTemMinA < m_dMinSV[ii]) ? nTemMinA : m_dMinSV[ii];
                }

                if (nShow == -1 || nShow == 2)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSD[ii]) ? nTemMaxA : m_dMaxSD[ii];
                    nTemMinA = (nTemMinA < m_dMinSD[ii]) ? nTemMinA : m_dMinSD[ii];
                }

                if (bSE)
                {
                    if (matchingType == 1)
                    {
                        nTemMaxA = (nTemMaxA > m_dMaxSA_Alfa1[ii]) ? nTemMaxA : m_dMaxSA_Alfa1[ii];
                        nTemMinA = (nTemMinA < m_dMinSA_Alfa1[ii]) ? nTemMinA : m_dMinSA_Alfa1[ii];
                    }
                    else if (matchingType == 2)
                    {
                        nTemMaxA = (nTemMaxA > m_dMaxSA_Alfa2[ii]) ? nTemMaxA : m_dMaxSA_Alfa2[ii];
                        nTemMinA = (nTemMinA < m_dMinSA_Alfa2[ii]) ? nTemMinA : m_dMinSA_Alfa2[ii];
                    }
                    //else if(matchingType == 3)

                }

                if (bSE0)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSE) ? nTemMaxA : m_dMaxSE;
                    nTemMinA = (nTemMinA < m_dMinSE) ? nTemMinA : m_dMinSE;
                }

                if (totalMaxA < nTemMaxA)
                    totalMaxA = nTemMaxA;

                if (totalMinA > nTemMinA)
                    totalMinA = nTemMinA;

            }

            for (int ls = 0; ls < nShowList.Length; ls++)
            {
                int ii = nShowList[ls];
                if (nShow == -1 || nShow == 0 || nShow == -2)
                {
                    temMax = totalMaxA;
                    temMin = totalMinA;
                    xBufferA[ls] = new double[nSLen[nShowList[ls]]];
                    Normalize(_S_A[ii], ref xBufferA[ls], _S_A[ii].Length, ref temMax, ref temMin, ref nScale);
                }
                if (nShow == -1 || nShow == 1)
                {
                    xBufferV[ls] = new double[nSLen[nShowList[ls]]];
                    temMax = totalMaxA;
                    temMin = totalMinA;
                    Normalize(_S_V[ii], ref xBufferV[ls], _S_V[ii].Length, ref temMax, ref temMin, ref nScale);
                }
                if (nShow == -1 || nShow == 2)
                {
                    xBufferD[ls] = new double[nSLen[nShowList[ls]]];
                    temMax = totalMaxA;
                    temMin = totalMinA;
                    Normalize(_S_D[ii], ref xBufferD[ls], _S_D[ii].Length, ref temMax, ref temMin, ref nScale);
                }
            }

            if (bSE)
            {
                temMax = totalMaxA;
                temMin = totalMinA;
                if (matchingType == 1)
                    Normalize(_S_A_Alfa1[nShowList[0]], ref xBufferSE, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);
                else if (matchingType == 2)
                    Normalize(_S_A_Alfa2[nShowList[0]], ref xBufferSE, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);
                //else if(matchingType == 3)                
            }

            double[] BufferMean = new double[nSLen[nShowList[0]]];

            temMax = totalMaxA;
            temMin = totalMinA;
            Normalize(_meanS_A, ref BufferMean, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);

            if (bSE0)
            {
                temMax = totalMaxA;
                temMin = totalMinA;
                Normalize(_Sae0, ref xBufferSE0, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);
            }

            totalMaxA = temMax;
            totalMinA = temMin;


            int yscale = 1;
            if (sender == picDI_Spectr)
            {
                double timeInterval = 0;
                Double.TryParse(textDI_SaPeriodInterval.Text.ToString(), NumberStyles.Any,
                    CultureInfo.CurrentCulture.NumberFormat, out timeInterval);
                double SaInterval = 0;
                Double.TryParse(textDI_SaInterval.Text.ToString(), NumberStyles.Any,
                    CultureInfo.CurrentCulture.NumberFormat, out SaInterval);

                if (timeInterval == 0)
                {
                    xStep = 1;
                    xSperat = (float)Math.Ceiling(periodTime / xStep);

                    if(SaInterval == 0)
                    {
                        yStep = 0.1f;
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                        if (ySperat < 4)
                        {
                            yStep /= 2;
                        }

                        double strAcc = Math.Round((yStep * Math.Pow(10, nScale)) * 10) / 10;
                        textDI_SaPeriodInterval.Text = xStep.ToString();
                        textDI_SaInterval.Text = strAcc.ToString();
                    }
                    else
                    {
                        yStep = (float)(SaInterval / Math.Pow(10, nScale));
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                    }

                }
                else
                {
                    xStep = (float)timeInterval;
                    xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                    if (SaInterval == 0)
                    {
                        yStep = 0.1f;
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                        if (ySperat < 4)
                        {
                            yStep /= 2;
                        }
                        
                        //textDI_SaInterval.Text = yStep.ToString();
                    }
                    else
                    {
                        yStep = (float)(SaInterval / Math.Pow(10, nScale));
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                    }
   
                }

            }
            else
            {
                xStep = 1;
                xSperat = (float)Math.Ceiling(periodTime / xStep);
                //if(totalMinA < 0)
                yStep = 0.1f;
                ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                if (ySperat < 4)
                {
                    yStep /= 2;
                }
            }
            

            totalMaxA = Math.Ceiling(totalMaxA / yStep) * yStep;
            totalMinA = Math.Floor(totalMinA / yStep) * yStep;
            ySperat = (float)Math.Round((totalMaxA - totalMinA) / yStep);



            //xStep = (float)((nWidth - padding_left - padding_right) / nCnt * Math.Floor(nCnt / xSperat));
            //yStep = (float)((nHeight - padding_top - padding_bottom) / ySperat);


            float center_y = (nHeight - padding_top - padding_bottom) / 2 + padding_top;
            float h_scale = (nHeight - padding_top - padding_bottom) / (float)(totalMaxA - totalMinA);
            float w_scale = (nWidth - padding_left - padding_right) / (float)totalMaxT;


            Bitmap bitmap = new Bitmap((int)nWidth, (int)nHeight);
            Graphics signGraphic = Graphics.FromImage(bitmap);
            Rectangle rectDC = new Rectangle(0, 0, (int)nWidth, (int)nHeight);
            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(signGraphic, rectDC))
            {
                bufferedgraphic.Graphics.Clear(m_colorPicBack);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                Pen p_axis = new Pen(Color.FromArgb(0, 0, 0), 2);
                float dis_axis = 5;
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)padding_top - dis_axis, padding_left, (float)nHeight - padding_bottom + dis_axis);
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)nHeight - padding_bottom + dis_axis, nWidth - padding_right + dis_axis, (float)nHeight - padding_bottom + dis_axis);

                Pen p_dot = new Pen(Color.Gray, 1);
                p_dot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                p_dot.DashPattern = new float[] { 1.0F, 2.0F, 1.0F, 3.0F };
                float tem = (float)nHeight - padding_bottom;
                Font fnt = new Font("Arial", 10);
                string mark;

                for (int yy = 0; yy <= ySperat; yy++)
                {
                    //if (Math.Abs(y) < 0.1f)
                    //y = 0f;
                    double y;
                    y = (double)((decimal)yy * (decimal)yStep + (decimal)totalMinA);
                    y = (double)((decimal)y * (decimal)Math.Pow(10, nScale));
                   

                    if (y > 0)
                        mark = " " + y.ToString();
                    else if (y == 0)
                        mark = " " + y.ToString();
                    else
                        mark = y.ToString();
                    bufferedgraphic.Graphics.DrawLine(p_dot, padding_left, tem, nWidth - padding_right, tem);
                    bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 45), (int)tem - 10));
                    tem -= yStep * h_scale;
                }

                if (nScale != 0)
                {
                    Font fnt_Scale = new Font("Arial", 5);
                    //bufferedgraphic.Graphics.DrawString("x 10", fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 35), (int)padding_top - 25));
                    //bufferedgraphic.Graphics.DrawString(nScale.ToString(), fnt_Scale, System.Drawing.Brushes.Black, new Point((int)(padding_left - 7), (int)padding_top - 25));
                }

                tem = (float)padding_left;
                for (int x = 0; x <= xSperat; x++)
                {
                    mark = "";
                    if (x != 0)
                    {
                        int nspace = (int)(Math.Log10(nCnt)) - (int)(Math.Log10(x));
                        for (int i = 0; i < nspace; i++)
                            mark += " ";

                        mark += ((decimal)x * (decimal)xStep).ToString();
                        bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                        bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 25), (int)(nHeight - padding_bottom + dis_axis + 2)));
                        tem += xStep * w_scale;                        
                    }
                    else
                    {
                        mark += x.ToString();
                        bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                        bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));
                        tem += xStep * w_scale;
                    }
                    if (tem > nWidth - padding_right)
                        break;
                }
                PointF[] meanPt = new PointF[nCnt];
                for (int i = 0; i < nCnt; i++)
                {
                    meanPt[i] = new PointF(0, 0);
                }

                int[] nMeanDiv = new int[nCnt];
                for (int i = 0; i < nCnt; i++)
                {
                    nMeanDiv[i] = 0;
                }

                for (int ls = 0; ls < nShowList.Length; ls++)
                {
                    int jj = nShowList[ls];
                    PointF[] pt1;
                    PointF[] pt2;
                    PointF[] pt3;
                    double oldx = 0, oldy = 0;
                    if (nShow == -1 || nShow == 0 || nShow == -2)
                    {
                        pt1 = new PointF[xBufferA[ls].Length];
                        for (int i = 0; i < nSLen[nShowList[ls]]; i++)
                        {
                            if (nShow != -1)
                                pt1[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferA[ls][i] - totalMinA) * h_scale);
                            else
                                pt1[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferA[ls][i] - totalMinA) * h_scale);
                            float temX = (float)(pt1[i].X + meanPt[i].X);
                            float temY = (float)(pt1[i].Y + meanPt[i].Y);
                            meanPt[i] = new PointF(temX, temY);
                            if (nShow == -2)
                            {
                                if (i == 0)
                                    continue;
                            }
                        }

                        if (nShow == -2)
                        {
                            Pen pGray = new Pen(Color.Gray, 1);
                            bufferedgraphic.Graphics.DrawLines(pGray, pt1);
                            pGray.Dispose();
                        }
                        else
                        {
                            Pen p1 = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 13) % 255, (jj * 11 + 11) % 255), 1);
                            bufferedgraphic.Graphics.DrawLines(p1, pt1);
                            p1.Dispose();
                        }
                    }

                    if (nShow == -1 || nShow == 1)
                    {
                        pt2 = new PointF[xBufferV[ls].Length];
                        for (int i = 0; i < xBufferV[ls].Length; i++)
                        {
                            pt2[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferV[ls][i] - totalMinA) * h_scale);
                        }

                        Pen p2 = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 170) % 255, (jj * 11 + 112) % 255), 1);
                        bufferedgraphic.Graphics.DrawLines(p2, pt2);
                        p2.Dispose();
                    }

                    if (nShow == -1 || nShow == 2)
                    {
                        pt3 = new PointF[xBufferD[ls].Length];
                        for (int i = 0; i < xBufferD[ls].Length; i++)
                        {
                            if (nShow != -1)
                                pt3[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferD[ls][i] - totalMinA) * h_scale);
                            else
                                pt3[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferD[ls][i] - totalMinA) * h_scale);
                        }
                        Pen p3 = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 150) % 255, 255 - (jj * 11 + 211) % 255), 1);
                        bufferedgraphic.Graphics.DrawLines(p3, pt3);
                        p3.Dispose();
                    }
                }

                PointF[] ptSE;

                if (bSE)
                {
                    ptSE = new PointF[xBufferSE.Length];
                    for (int i = 0; i < xBufferSE.Length; i++)
                    {
                        if (nShow != -1)
                            ptSE[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE[i] - totalMinA) * h_scale);
                        else
                            ptSE[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE[i] - totalMinA) * h_scale);
                    }

                    if (nShow == -2)
                    {
                        Pen pRed = new Pen(Color.Red, 2);
                        bufferedgraphic.Graphics.DrawLines(pRed, ptSE);
                        pRed.Dispose();
                    }
                    else
                    {
                        if (matchingType != 3)
                        {
                            Pen pBlue = new Pen(Color.Blue, 2);
                            bufferedgraphic.Graphics.DrawLines(pBlue, ptSE);
                            pBlue.Dispose();
                        }



                    }

                }

                if (bSE0)
                {
                    PointF[] ptSE0;
                    ptSE0 = new PointF[xBufferSE0.Length];
                    for (int i = 0; i < xBufferSE0.Length; i++)
                    {
                        if (nShow != -1)
                            ptSE0[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE0[i] - totalMinA) * h_scale);
                        else
                            ptSE0[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE0[i] - totalMinA) * h_scale);
                    }
                    Pen pBlue1 = new Pen(Color.FromArgb(211, 65, 208), 1);
                    bufferedgraphic.Graphics.DrawLines(pBlue1, ptSE0);
                    pBlue1.Dispose();
                }

                if (nShow == -1)
                {
                    string strTitile = "Spectral Acceleration (Sa,g)\nSpectral Acceleration (SV,cm/sn)\nSpectral Acceleration (Sa, g)";
                    //                     string strTitile2 = "Spectral Velocity (SV,cm/sn)";
                    //                     string strTitile3 = "Spectral Acceleration (Sa,g)";
                    System.Drawing.Drawing2D.Matrix matrixbackup = bufferedgraphic.Graphics.Transform;
                    try
                    {

                        System.Drawing.Drawing2D.Matrix cmatrix = bufferedgraphic.Graphics.Transform.Clone();
                        Font font = new Font("Arial", 10);
                        int fontWidth = (int)TextRenderer.MeasureText(strTitile, font).Width;
                        int fontHeight = (int)TextRenderer.MeasureText(strTitile, font).Height;
                        int nH = (int)(nHeight / 4 + fontWidth / 4);
                        PointF o = new PointF(35, nH);
                        cmatrix.RotateAt(180, o);
                        bufferedgraphic.Graphics.Transform = cmatrix;
                        Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                        RectangleF rect = new RectangleF(5, 0, 90, fontWidth + 10);
                        StringFormat ft = new StringFormat();
                        ft.FormatFlags = StringFormatFlags.DirectionVertical;
                        bufferedgraphic.Graphics.DrawString(strTitile, font, brush, rect, ft);
                    }
                    finally
                    {
                        bufferedgraphic.Graphics.Transform = matrixbackup;
                    }
                }
                else
                {
                    string strTitile = "";
                    if (nShow == 0)
                    {
                        strTitile = "Spectral Acceleration (Sa,g)";
                    }
                    else if (nShow == 1)
                    {
                        strTitile = "Spectral Velocity (SV,cm/sn)";
                    }
                    else if (nShow == 2)
                    {
                        strTitile = "Spectral Displacement (SD,cm)";
                    }
                    else if (nShow == -2)
                    {
                        strTitile = "Spectral Acceleration (Sa,g)";

                        for (int i = 0; i < (int)(totalMaxT / deltT); i++)
                        {
                            
                            float temY = nHeight - padding_bottom - (float)((float)BufferMean[i] - totalMinA) * h_scale ;
                            float temX = (float)(i * deltT) * w_scale + padding_left;
                            meanPt[i] = new PointF(temX, temY);
                        }
                        Pen pBlack = new Pen(Color.Black, 2);
                        //bufferedgraphic.Graphics.DrawLines(pBlack, meanPt);
                        bufferedgraphic.Graphics.DrawLines(pBlack, meanPt);
                        pBlack.Dispose();
                    }

                    System.Drawing.Drawing2D.Matrix matrixbackup = bufferedgraphic.Graphics.Transform;
                    try
                    {

                        System.Drawing.Drawing2D.Matrix cmatrix = bufferedgraphic.Graphics.Transform.Clone();
                        Font font = new Font("Arial", 10);
                        int fontWidth = (int)TextRenderer.MeasureText(strTitile, font).Width;
                        int fontHeight = (int)TextRenderer.MeasureText(strTitile, font).Height;
                        int nH = (int)(nHeight / 4 + fontWidth / 4);
                        PointF o = new PointF(15, nH);
                        cmatrix.RotateAt(180, o);
                        bufferedgraphic.Graphics.Transform = cmatrix;
                        Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                        RectangleF rect = new RectangleF(5, 0, 30, fontWidth + 10);
                        StringFormat ft = new StringFormat();
                        ft.FormatFlags = StringFormatFlags.DirectionVertical;
                        bufferedgraphic.Graphics.DrawString(strTitile, font, brush, rect, ft);
                    }
                    finally
                    {
                        bufferedgraphic.Graphics.Transform = matrixbackup;
                    }
                }

                string strBottrom = "Period (T)";
                Font font1 = new Font("Arial", 10);

                int fontWidth1 = (int)TextRenderer.MeasureText(strBottrom, font1).Width;
                Brush brush1 = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
                bufferedgraphic.Graphics.DrawString(strBottrom, font1, brush1, nWidth / 2, nHeight - 20);//, ft1);

                bufferedgraphic.Render(signGraphic);
                bufferedgraphic.Render(e.Graphics);
                //System.Windows.Forms.PictureBox ctrl = (System.Windows.Forms.PictureBox)sender;
                //ctrl.Image = bitmap;
                //DrawCtrl.Image = bitmap;
            }

            return bitmap;
        }

        private Bitmap DrawAllGraphTarget(object sender, PaintEventArgs e, Color color, double[] sBuf, double[] yBuf, int nCnt, float nWidth, float nHeight, double nMaxA, double nMinA, float[] Padding)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new PaintEventHandler(this.DrawAllGraph_SMDAcceleration), new
                object[] { sender, e });
                return null;
            }
            double periodTime;
            Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out periodTime);
            //periodTime = Convert.ToDouble(textDI_MaxPeriod.Text.ToString());
            float padding_left = 50;
            float padding_right = 50;
            float padding_top = 30;
            float padding_bottom = 30;
            if (Padding != null)
            {
                padding_left = Padding[0];
                padding_top = Padding[1];
                padding_right = Padding[2];
                padding_bottom = Padding[3];
            }

            float xSperat;
            float ySperat;
            float xStep;
            float yStep;
            ///////////////////////////////////////////////////////////////
            double[] xBuffer = new double[nCnt];
            int nScale = 0;
            Normalize(sBuf, ref xBuffer, nCnt, ref nMaxA, ref nMinA, ref nScale);
            ///////////////////////////////////////////////////////////////


            float center_y = (nHeight - padding_top - padding_bottom) / 2 + padding_top;
            float h_scale = (float)(nHeight - padding_top - padding_bottom) / (float)(nMaxA);
            float w_scale = (float)(nWidth - padding_left - padding_right) / (float)periodTime;

            xSperat = (int)Math.Ceiling(periodTime);
            ySperat = (float)((nMaxA) / 0.1);
            xStep = (float)((nWidth - padding_left - padding_right) / nCnt * Math.Floor(nCnt / xSperat));
            yStep = (float)((nHeight - padding_top - padding_bottom) / ySperat);

            PointF[] pt = new PointF[nCnt];

            for (int i = 0; i < nCnt; i++)
            {
                pt[i] = new PointF((float)yBuf[i] * w_scale + padding_left, nHeight - padding_bottom - (float)xBuffer[i] * h_scale);
            }

            Bitmap bitmap = new Bitmap((int)nWidth, (int)nHeight);
            Graphics signGraphic = Graphics.FromImage(bitmap);
            Rectangle rectDC = new Rectangle(0, 0, (int)nWidth, (int)nHeight);
            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(signGraphic, rectDC))
            {
                bufferedgraphic.Graphics.Clear(m_colorPicBack);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                Pen p_axis = new Pen(Color.FromArgb(0, 0, 0), 2);
                float dis_axis = 5;
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)padding_top - dis_axis, padding_left, (float)nHeight - padding_bottom + dis_axis);
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)nHeight - padding_bottom + dis_axis, nWidth - padding_left + dis_axis, (float)nHeight - padding_bottom + dis_axis);

                Pen p_dot = new Pen(Color.Gray, 1);
                p_dot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                p_dot.DashPattern = new float[] { 1.0F, 2.0F, 1.0F, 3.0F };
                float tem = (float)nHeight - padding_bottom;
                Font fnt = new Font("Arial", 10);
                string mark;

                for (int yy = 0; yy <= (int)(nMaxA); yy++)
                {
                    float y = (float)yy / 10;
                    if (Math.Abs(y) < 0.1f)
                        y = 0f;

                    //y = (float)(Math.Truncate(y * 10) / 10);

                    y = y * (float)Math.Pow(10, nScale);

                    if (y > 0)
                        mark = " " + y.ToString();
                    else if (y == 0)
                        mark = " " + y.ToString();
                    else
                        mark = y.ToString();
                    bufferedgraphic.Graphics.DrawLine(p_dot, padding_left, tem, nWidth - padding_right, tem);
                    //bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 20), (int)tem - 10));
                    tem -= yStep;
                }

                //if(nScale != 0)
                {
                    Font fnt_Scale = new Font("Arial", 5);
                    //bufferedgraphic.Graphics.DrawString("x 10", fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 35), (int)padding_top - 25));
                    //bufferedgraphic.Graphics.DrawString(nScale.ToString(), fnt_Scale, System.Drawing.Brushes.Black, new Point((int)(padding_left - 7), (int)padding_top - 25));
                }

                tem = (float)padding_left;
                for (int x = 0; x <= xSperat; x++)
                {
                    mark = x.ToString();
                    bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                    //bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));
                    tem += xStep;
                }

                Pen p = new Pen(color, 1);
                bufferedgraphic.Graphics.DrawLines(p, pt);
                p.Dispose();

                bufferedgraphic.Render(e.Graphics);
                bufferedgraphic.Render(signGraphic);
                //System.Windows.Forms.PictureBox ctrl = (System.Windows.Forms.PictureBox)sender;
                //ctrl.Image = bitmap;
                //DrawCtrl.Image = bitmap;
            }
            return bitmap;
        }

        private Bitmap DrawAllGraph(object sender, PaintEventArgs e, Color color, string strTitile, double[][] sBuf, double[][] tBuf, int[] nCnt, int[] showlist, float nWidth, float nHeight, double[] nMaxA, double[] nMinA, double[] nMaxT, double[] nMinT, float[] Padding, int style = 0)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new PaintEventHandler(this.DrawAllGraph_SMDAcceleration), new
                object[] { sender, e });
                return null;
            }

            float padding_left = 50;
            float padding_right = 50;
            float padding_top = 30;
            float padding_bottom = 30;

            if (Padding != null)
            {
                padding_left = Padding[0];
                padding_top = Padding[1];
                padding_right = Padding[2];
                padding_bottom = Padding[3];
            }

            float xSperat = 0;
            float ySperat = 0;
            float xStep = 0;
            float yStep = 0;

            ///////////////////////////////////////////////////////////////
            //Get MaxA in datas;
            double totalMaxA = nMaxA[showlist[0]], totalMinA = nMinA[showlist[0]];
            double totalMaxT = nMaxT[showlist[0]], totalMinT = nMinT[showlist[0]];
            int nMaxCnt = nCnt[showlist[0]];
            for (int ls = 1; ls < showlist.Length; ls++)
            {
                int ii = showlist[ls];
                if (totalMaxA < nMaxA[ii])
                    totalMaxA = nMaxA[ii];

                if (totalMinA > nMinA[ii])
                    totalMinA = nMinA[ii];

                if (totalMaxT < nMaxT[ii])
                    totalMaxT = nMaxT[ii];

                if (totalMinT > nMinT[ii])
                    totalMinT = nMinT[ii];

                if (nMaxCnt < nCnt[ii])
                    nMaxCnt = nCnt[ii];

            }
            /////////////////////////////////////////////////////////////

            double[][] xBuffer = new double[showlist.Length][];
            int nScale = 0;
            for (int ls = 0; ls < showlist.Length - 1; ls++)
            {
                int ii = showlist[ls];
                xBuffer[ls] = new double[nCnt[ii]];
                double temMax = totalMaxA;
                double temMin = totalMinA;
                Normalize(sBuf[ii], ref xBuffer[ls], nCnt[ii], ref temMax, ref temMin, ref nScale);
            }

            xBuffer[showlist.Length - 1] = new double[nCnt[showlist[showlist.Length - 1]]];
            Normalize(sBuf[showlist[showlist.Length - 1]], ref xBuffer[showlist.Length - 1], nCnt[showlist[showlist.Length - 1]], ref totalMaxA, ref totalMinA, ref nScale);


            float center_y;
            float h_scale;
            float w_scale;

            double yscale = 1;

            if (style == 3)
            {
                xSperat = (float)Math.Ceiling(Math.Log10(m_dMaxT[showlist[0]])) + 2;
                xStep = (nWidth - padding_left - padding_right) / xSperat;

                yStep = 0.1f;
                ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                if (ySperat < 4)
                {
                    yStep /= 2;
                    ySperat *= 2;
                }
                else if (ySperat > 10)
                {
                    yStep *= 2;
                    ySperat /= 2;
                }

                if (style == 1)
                {
                    yStep *= 2;
                    ySperat /= 2;
                }

                totalMaxA = (double)((decimal)Math.Ceiling((double)((decimal)totalMaxA / (decimal)yStep)) * (decimal)yStep);
                totalMinA = (double)((decimal)Math.Floor((double)((decimal)totalMinA / (decimal)yStep)) * (decimal)yStep);
                ySperat = (float)Math.Round((double)(((decimal)totalMaxA - (decimal)totalMinA) / (decimal)yStep));
            }
            else
            {
                int nIndexList = showlist[0];

                if (sender == picDI_Acceleration)
                {
                    double timeInterval = 0;
                    Double.TryParse(textDI_TimeInterval.Text.ToString(), NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double accInterval = 0;
                    Double.TryParse(textDI_AccInterval.Text.ToString(), NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out accInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if(accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textDI_AccInterval.Text = strAcc.ToString();
                            textDI_TimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(accInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                        
                        //LogText("TEST" + yStep.ToString());

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if(accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)accInterval;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                        
                    }
                    
                }
                else if (sender == picSMD_Acceleration)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_AccTimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double accInterval = 0;
                    Double.TryParse(_SMD_AccInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out accInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if(accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_AccInterval.Text = strAcc.ToString();
                            textSMD_AccTimeInterval.Text = xStep.ToString();
                            
                        }
                        else
                        {
                            yStep = (float)(accInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                        
                        

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if(accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(accInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                        
                    }
                    _SMD_AccInterval[nIndexList] = textSMD_AccInterval.Text;
                    _SMD_AccTimeInterval[nIndexList] = textSMD_AccTimeInterval.Text;
                }
                else if (sender == picSMD_Velocity)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_VccTimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double VccInterval = 0;
                    Double.TryParse(_SMD_VccInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out VccInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if(VccInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_VccInterval.Text = strAcc.ToString();
                            textSMD_VccTimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(VccInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                        

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if(VccInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(VccInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }
                        
                    }

                    _SMD_VccInterval[nIndexList] = textSMD_VccInterval.Text;
                    _SMD_VccTimeInterval[nIndexList] = textSMD_VccTimeInterval.Text;

                }
                else if (sender == picSMD_Displacement)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_DisTimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double DisInterval = 0;
                    Double.TryParse(_SMD_DisInterval[nIndexList], NumberStyles.Any,
                            CultureInfo.CurrentCulture.NumberFormat, out DisInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if(DisInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_DisInterval.Text = strAcc.ToString();
                            textSMD_DisTimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(DisInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if(DisInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(DisInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                      
                        
                    }

                    _SMD_DisInterval[nIndexList] = textSMD_DisInterval.Text;
                    _SMD_DisTimeInterval[nIndexList] = textSMD_DisTimeInterval.Text;

                }
                else if (sender == picSMD_AriasIntensity)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_AITimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double AIInterval = 0;
                    Double.TryParse(_SMD_AIInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out AIInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if(AIInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_AIInterval.Text = strAcc.ToString();
                            textSMD_AITimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(AIInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if(AIInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(AIInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }                        
                    }

                    _SMD_AIInterval[nIndexList] = textSMD_AIInterval.Text;
                    _SMD_AITimeInterval[nIndexList] = textSMD_AITimeInterval.Text;
                }
                else
                {
                    xStep = 5;
                    xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                    if (xSperat > 50)
                    {
                        xStep = 10;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                    }

                    yStep = 0.1f;
                    ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                    if (ySperat < 4)
                    {
                        yStep /= 2;
                        ySperat *= 2;
                    }
                    else if (ySperat > 10)
                    {
                        yStep *= 2;
                        ySperat /= 2;
                    }

                    if (style == 1)
                    {
                        yStep *= 2;
                        ySperat /= 2;
                    }
                }

                totalMaxA = (double)((decimal)Math.Ceiling((double)((decimal)totalMaxA / (decimal)yStep)) * (decimal)yStep);
                totalMinA = (double)((decimal)Math.Floor((double)((decimal)totalMinA / (decimal)yStep)) * (decimal)yStep);
                ySperat = (float)Math.Round((double)(((decimal)totalMaxA - (decimal)totalMinA) / (decimal)yStep));
            }

            

            //yStep = (float)((nHeight - padding_top - padding_bottom) / ySperat);


            center_y = (nHeight - padding_top - padding_bottom) / 2 + padding_top;
            h_scale = (nHeight - padding_top - padding_bottom) / (float)(totalMaxA - totalMinA);
            if (style == 3)
                w_scale = (nWidth - padding_left - padding_right);
            else
                w_scale = (nWidth - padding_left - padding_right) / (float)(Math.Ceiling(totalMaxT / 5.0) * 5);

            ///////////////////////////////////////////////////////////////            
            Bitmap bitmap = new Bitmap((int)nWidth, (int)nHeight);
            Graphics signGraphic = Graphics.FromImage(bitmap);
            Rectangle rectDC = new Rectangle(0, 0, (int)nWidth, (int)nHeight);
            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(e.Graphics, rectDC))
            {
                bufferedgraphic.Graphics.Clear(m_colorPicBack);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                Pen p_axis = new Pen(Color.FromArgb(0, 0, 0), 2);
                float dis_axis = 5;
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)padding_top - dis_axis, padding_left, (float)nHeight - padding_bottom + dis_axis);
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)nHeight - padding_bottom + dis_axis, nWidth - padding_right + dis_axis, (float)nHeight - padding_bottom + dis_axis);

                Pen p_dot = new Pen(Color.Gray, 1);
                p_dot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                p_dot.DashPattern = new float[] { 1.0F, 2.0F, 1.0F, 3.0F };
                float tem = (float)nHeight - padding_bottom;
                Font fnt = new Font("Arial", 10);
                string mark;

                int ys = 1;

                if (totalMaxA - totalMinA >= 1)
                    ys = 2;


                for (int yy = 0; yy <= ySperat; yy++)
                {
                    double y;
                    y = (double)((decimal)yy * (decimal)yStep + (decimal)totalMinA);
                    y = (double)((decimal)y * (decimal)Math.Pow(10, nScale));                    

                    if (y > 0)
                        mark = " " + y.ToString();
                    else if (y == 0)
                        mark = " " + y.ToString();
                    else
                        mark = y.ToString();
                    bufferedgraphic.Graphics.DrawLine(p_dot, padding_left, tem, nWidth - padding_right, tem);
                    bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 45), (int)tem - 10));
                    tem -= yStep * h_scale;
                }

                if (style == 3)
                {
                    for (int x = -2; x <= xSperat - 2; x++)
                    {
                        mark = "";
                        if (x != -2)
                        {
                            mark += (Math.Pow(10, x)).ToString();
                            tem = padding_left + xStep * (x + 2);
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem), (int)(nHeight - padding_bottom + dis_axis + 2)));

                        }
                        else
                        {
                            mark += "0";
                            tem = padding_left + xStep * (x + 2);
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));

                        }

                    }
                }
                else
                {
                    tem = (float)padding_left;
                    for (int x = 0; x <= xSperat; x++)
                    {
                        mark = "";
                        if (x != 0)
                        {
                            int nspace = (int)(Math.Log10(nMaxCnt)) - (int)(Math.Log10(x));
                            for (int i = 0; i < nspace; i++)
                                mark += " ";

                            mark += ((double)((decimal)x * (decimal)xStep)).ToString();
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 25), (int)(nHeight - padding_bottom + dis_axis + 2)));
                            tem += xStep * w_scale;
                        }
                        else
                        {
                            mark += "0";
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));
                            tem += xStep * w_scale;
                        }

                    }
                }

                //Draw Grapg ...
                for (int st = 0; st < showlist.Length; st++)
                {
                    int jj = showlist[st];
                    Pen p = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 13) % 255, (jj * 11 + 11) % 255), 1);

                    PointF[] pt = new PointF[nCnt[jj]];
                    if (totalMinA < 0)
                    {
                        for (int i = 0; i < nCnt[jj]; i++)
                        {
                            if (style == 3)
                                pt[i] = new PointF(i * w_scale + padding_left, center_y - (float)xBuffer[st][i] * h_scale);
                            else
                                pt[i] = new PointF((float)tBuf[jj][i] * w_scale + padding_left, center_y - (float)xBuffer[st][i] * h_scale);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nCnt[jj]; i++)
                        {
                            if (style == 3)
                            {
                                if (tBuf[jj][i] != 0)
                                {
                                    float www = (float)(Math.Log10(tBuf[jj][i]) + 2) / (float)(Math.Log10(totalMaxT) + 2);
                                    if (www < 0)
                                        www = 0;
                                    pt[i] = new PointF(www * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[st][i] - totalMinA) * h_scale);
                                }
                                else
                                    pt[i] = new PointF(padding_left, nHeight - padding_bottom - (float)(xBuffer[st][i] - totalMinA) * h_scale);

                            }
                            else
                            {
                                pt[i] = new PointF((float)tBuf[jj][i] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[st][i] - totalMinA) * h_scale);
                            }

                        }
                    }
                    bufferedgraphic.Graphics.DrawLines(p, pt);
                    p.Dispose();
                }

                if (style == 1)
                {
                    int index = showlist[0];
                    Pen pline = new Pen(Color.FromArgb(0, 0, 255), 1);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D5AI_pos[index]] - totalMinA) * h_scale, (float)tBuf[index][_D5AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D5AI_pos[index]] - totalMinA) * h_scale);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D95AI_pos[index]] - totalMinA) * h_scale, (float)tBuf[index][_D95AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D95AI_pos[index]] - totalMinA) * h_scale);

                    bufferedgraphic.Graphics.DrawLine(pline, (float)tBuf[index][_D5AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom + dis_axis, (float)tBuf[index][_D5AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D5AI_pos[index]] - totalMinA) * h_scale);
                    bufferedgraphic.Graphics.DrawLine(pline, (float)tBuf[index][_D95AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom + dis_axis, (float)tBuf[index][_D95AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D95AI_pos[index]] - totalMinA) * h_scale);
                }
                else if (style == 2)
                {
                    int index = showlist[0];
                    Pen pline = new Pen(Color.FromArgb(0, 0, 255), 1);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, center_y + (float)_Bracked_value[m_nCountMotion - 1] * h_scale, nWidth - padding_right, center_y + (float)_Bracked_value[m_nCountMotion - 1] * h_scale);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, center_y - (float)_Bracked_value[m_nCountMotion - 1] * h_scale, nWidth - padding_right, center_y - (float)_Bracked_value[m_nCountMotion - 1] * h_scale);
                }
                else if (style == 3)
                {

                    string strBottrom = "Frequency (Hz)";
                    Font font1 = new Font("Arial", 10);

                    int fontWidth1 = (int)TextRenderer.MeasureText(strBottrom, font1).Width;
                    Brush brush1 = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
                    bufferedgraphic.Graphics.DrawString(strBottrom, font1, brush1, nWidth / 2, nHeight - 20);//, ft1);

                }


                ///////////////////////
                System.Drawing.Drawing2D.Matrix matrixbackup = bufferedgraphic.Graphics.Transform;
                try
                {
                    System.Drawing.Drawing2D.Matrix cmatrix = bufferedgraphic.Graphics.Transform.Clone();
                    Font font = new Font("Arial", 10);
                    int fontWidth = (int)TextRenderer.MeasureText(strTitile, font).Width;
                    int fontHeight = (int)TextRenderer.MeasureText(strTitile, font).Height;
                    int nH = (int)(nHeight / 4 + fontWidth / 4);
                    PointF o = new PointF(15, nH);
                    cmatrix.RotateAt(180, o);
                    bufferedgraphic.Graphics.Transform = cmatrix;
                    Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                    RectangleF rect = new RectangleF(5, 0, 30, fontWidth + 10);
                    StringFormat ft = new StringFormat();
                    ft.FormatFlags = StringFormatFlags.DirectionVertical;
                    bufferedgraphic.Graphics.DrawString(strTitile, font, brush, rect, ft);
                }
                finally
                {
                    bufferedgraphic.Graphics.Transform = matrixbackup;
                }
                //////////////////////////
                bufferedgraphic.Render(e.Graphics);
                bufferedgraphic.Render(signGraphic);
                //signGraphic = e.Graphics;                
                //DrawCtrl.Image = bitmap;
            }

            return bitmap;
        }

        private Bitmap DrawAllGraph_SpectrToClipboard(object sender, Color color, int matchingType, int[] nShowList, float nWidth, float nHeight, float[] Padding, int nShow, Boolean bDrawSae)
        {
            float padding_left = 50;
            float padding_right = 50;
            float padding_top = 30;
            float padding_bottom = 30;

            if (Padding != null)
            {
                padding_left = Padding[0];
                padding_top = Padding[1];
                padding_right = Padding[2];
                padding_bottom = Padding[3];
            }

            float xSperat = 0;
            float ySperat = 0;
            float xStep = 0;
            float yStep = 0;
            ///////////////////////////////////////////////////////////////          
            Boolean bSE = false;
            Boolean bSE0 = false;
            if (bDrawSae)
            {
                if (nShowList.Length == 1)
                {
                    if (_S_A_Alfa1[nShowList[0]] != null && matchingType == 1)
                        bSE = true;

                    if (_S_A_Alfa2[nShowList[0]] != null && matchingType == 2)
                        bSE = true;
                }

                if (_Sae0 != null)
                    bSE0 = true;
            }

            double[][] xBufferA = new double[nShowList.Length][];
            double[][] xBufferV = new double[nShowList.Length][];
            double[][] xBufferD = new double[nShowList.Length][];
            double[] xBufferSE = new double[nSLen[nShowList[0]]];
            double[] xBufferSE0 = new double[nSLen[nShowList[0]]];
            int nScale = 0;
            ///////////////////////////////////////////////////////////////
            //Get MaxA in datas;
            double totalMaxA = -1, totalMinA = 1000;
            double temMax = -1;
            double temMin = 1000;

            double periodTime;
            Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out periodTime);

            double totalMaxT = periodTime;
            //totalMaxT = Convert.ToDouble(textDI_MaxPeriod.Text.ToString());
            double totalMinT = 0;
            double deltT = 0.02;
            int nCnt = (int)(totalMaxT / deltT);
            for (int ls = 0; ls < nShowList.Length; ls++)
            {
                int ii = nShowList[ls];
                double nTemMaxA = -1;
                double nTemMinA = 1000;
                if (nShow == -1 || nShow == 0 || nShow == -2)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSA[ii]) ? nTemMaxA : m_dMaxSA[ii];
                    nTemMinA = (nTemMinA < m_dMinSA[ii]) ? nTemMinA : m_dMinSA[ii];
                }


                if (nShow == -1 || nShow == 1)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSV[ii]) ? nTemMaxA : m_dMaxSV[ii];
                    nTemMinA = (nTemMinA < m_dMinSV[ii]) ? nTemMinA : m_dMinSV[ii];
                }

                if (nShow == -1 || nShow == 2)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSD[ii]) ? nTemMaxA : m_dMaxSD[ii];
                    nTemMinA = (nTemMinA < m_dMinSD[ii]) ? nTemMinA : m_dMinSD[ii];
                }

                if (bSE)
                {
                    if (matchingType == 1)
                    {
                        nTemMaxA = (nTemMaxA > m_dMaxSA_Alfa1[ii]) ? nTemMaxA : m_dMaxSA_Alfa1[ii];
                        nTemMinA = (nTemMinA < m_dMinSA_Alfa1[ii]) ? nTemMinA : m_dMinSA_Alfa1[ii];
                    }
                    else if (matchingType == 2)
                    {
                        nTemMaxA = (nTemMaxA > m_dMaxSA_Alfa2[ii]) ? nTemMaxA : m_dMaxSA_Alfa2[ii];
                        nTemMinA = (nTemMinA < m_dMinSA_Alfa2[ii]) ? nTemMinA : m_dMinSA_Alfa2[ii];
                    }
                    //else if(matchingType == 3)

                }

                if (bSE0)
                {
                    nTemMaxA = (nTemMaxA > m_dMaxSE) ? nTemMaxA : m_dMaxSE;
                    nTemMinA = (nTemMinA < m_dMinSE) ? nTemMinA : m_dMinSE;
                }

                if (totalMaxA < nTemMaxA)
                    totalMaxA = nTemMaxA;

                if (totalMinA > nTemMinA)
                    totalMinA = nTemMinA;

            }

            for (int ls = 0; ls < nShowList.Length; ls++)
            {
                int ii = nShowList[ls];
                if (nShow == -1 || nShow == 0 || nShow == -2)
                {
                    temMax = totalMaxA;
                    temMin = totalMinA;
                    xBufferA[ls] = new double[nSLen[nShowList[ls]]];
                    Normalize(_S_A[ii], ref xBufferA[ls], _S_A[ii].Length, ref temMax, ref temMin, ref nScale);
                }
                if (nShow == -1 || nShow == 1)
                {
                    xBufferV[ls] = new double[nSLen[nShowList[ls]]];
                    temMax = totalMaxA;
                    temMin = totalMinA;
                    Normalize(_S_V[ii], ref xBufferV[ls], _S_V[ii].Length, ref temMax, ref temMin, ref nScale);
                }
                if (nShow == -1 || nShow == 2)
                {
                    xBufferD[ls] = new double[nSLen[nShowList[ls]]];
                    temMax = totalMaxA;
                    temMin = totalMinA;
                    Normalize(_S_D[ii], ref xBufferD[ls], _S_D[ii].Length, ref temMax, ref temMin, ref nScale);
                }
            }

            if (bSE)
            {
                temMax = totalMaxA;
                temMin = totalMinA;
                if (matchingType == 1)
                    Normalize(_S_A_Alfa1[nShowList[0]], ref xBufferSE, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);
                else if (matchingType == 2)
                    Normalize(_S_A_Alfa2[nShowList[0]], ref xBufferSE, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);
                //else if(matchingType == 3)                
            }

            if (bSE0)
            {
                temMax = totalMaxA;
                temMin = totalMinA;
                Normalize(_Sae0, ref xBufferSE0, nSLen[nShowList[0]], ref temMax, ref temMin, ref nScale);
            }

            totalMaxA = temMax;
            totalMinA = temMin;


            int yscale = 1;
            if (sender == picDI_Spectr)
            {
                double timeInterval = 0;
                Double.TryParse(textDI_SaPeriodInterval.Text.ToString(), NumberStyles.Any,
                    CultureInfo.CurrentCulture.NumberFormat, out timeInterval);
                double SaInterval = 0;
                Double.TryParse(textDI_SaInterval.Text.ToString(), NumberStyles.Any,
                    CultureInfo.CurrentCulture.NumberFormat, out SaInterval);

                if (timeInterval == 0)
                {
                    xStep = 1;
                    xSperat = (float)Math.Ceiling(periodTime / xStep);

                    if (SaInterval == 0)
                    {
                        yStep = 0.1f;
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                        if (ySperat < 4)
                        {
                            yStep /= 2;
                        }

                        double strAcc = Math.Round((yStep * Math.Pow(10, nScale)) * 10) / 10;
                        textDI_SaPeriodInterval.Text = xStep.ToString();
                        textDI_SaInterval.Text = strAcc.ToString();
                    }
                    else
                    {
                        yStep = (float)(SaInterval / Math.Pow(10, nScale));
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                    }

                }
                else
                {
                    xStep = (float)timeInterval;
                    xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                    if (SaInterval == 0)
                    {
                        yStep = 0.1f;
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                        if (ySperat < 4)
                        {
                            yStep /= 2;
                        }

                        //textDI_SaInterval.Text = yStep.ToString();
                    }
                    else
                    {
                        yStep = (float)(SaInterval / Math.Pow(10, nScale));
                        ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                    }

                }

            }
            else
            {
                xStep = 1;
                xSperat = (float)Math.Ceiling(periodTime / xStep);
                //if(totalMinA < 0)
                yStep = 0.1f;
                ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                if (ySperat < 4)
                {
                    yStep /= 2;
                }
            }


            totalMaxA = Math.Ceiling(totalMaxA / yStep) * yStep;
            totalMinA = Math.Floor(totalMinA / yStep) * yStep;
            ySperat = (float)Math.Round((totalMaxA - totalMinA) / yStep);



            //xStep = (float)((nWidth - padding_left - padding_right) / nCnt * Math.Floor(nCnt / xSperat));
            //yStep = (float)((nHeight - padding_top - padding_bottom) / ySperat);


            float center_y = (nHeight - padding_top - padding_bottom) / 2 + padding_top;
            float h_scale = (nHeight - padding_top - padding_bottom) / (float)(totalMaxA - totalMinA);
            float w_scale = (nWidth - padding_left - padding_right) / (float)totalMaxT;


            Bitmap bitmap = new Bitmap((int)nWidth, (int)nHeight);
            Graphics signGraphic = Graphics.FromImage(bitmap);
            Rectangle rectDC = new Rectangle(0, 0, (int)nWidth, (int)nHeight);
            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(signGraphic, rectDC))
            {
                bufferedgraphic.Graphics.Clear(m_colorPicBack);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                Pen p_axis = new Pen(Color.FromArgb(0, 0, 0), 2);
                float dis_axis = 5;
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)padding_top - dis_axis, padding_left, (float)nHeight - padding_bottom + dis_axis);
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)nHeight - padding_bottom + dis_axis, nWidth - padding_right + dis_axis, (float)nHeight - padding_bottom + dis_axis);

                Pen p_dot = new Pen(Color.Gray, 1);
                p_dot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                p_dot.DashPattern = new float[] { 1.0F, 2.0F, 1.0F, 3.0F };
                float tem = (float)nHeight - padding_bottom;
                Font fnt = new Font("Arial", 10);
                string mark;

                for (int yy = 0; yy <= ySperat; yy++)
                {
                    //if (Math.Abs(y) < 0.1f)
                    //y = 0f;
                    double y;
                    y = (double)((decimal)yy * (decimal)yStep + (decimal)totalMinA);
                    y = (double)((decimal)y * (decimal)Math.Pow(10, nScale));


                    if (y > 0)
                        mark = " " + y.ToString();
                    else if (y == 0)
                        mark = " " + y.ToString();
                    else
                        mark = y.ToString();
                    bufferedgraphic.Graphics.DrawLine(p_dot, padding_left, tem, nWidth - padding_right, tem);
                    bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 45), (int)tem - 10));
                    tem -= yStep * h_scale;
                }

                if (nScale != 0)
                {
                    Font fnt_Scale = new Font("Arial", 5);
                    //bufferedgraphic.Graphics.DrawString("x 10", fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 35), (int)padding_top - 25));
                    //bufferedgraphic.Graphics.DrawString(nScale.ToString(), fnt_Scale, System.Drawing.Brushes.Black, new Point((int)(padding_left - 7), (int)padding_top - 25));
                }

                tem = (float)padding_left;
                for (int x = 0; x <= xSperat; x++)
                {
                    mark = "";
                    if (x != 0)
                    {
                        int nspace = (int)(Math.Log10(nCnt)) - (int)(Math.Log10(x));
                        for (int i = 0; i < nspace; i++)
                            mark += " ";

                        mark += ((decimal)x * (decimal)xStep).ToString();
                        bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                        bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 25), (int)(nHeight - padding_bottom + dis_axis + 2)));
                        tem += xStep * w_scale;
                    }
                    else
                    {
                        mark += x.ToString();
                        bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                        bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));
                        tem += xStep * w_scale;
                    }
                    if (tem > nWidth - padding_right)
                        break;
                }
                PointF[] meanPt = new PointF[nCnt];
                for (int i = 0; i < nCnt; i++)
                {
                    meanPt[i] = new PointF(0, 0);
                }

                int[] nMeanDiv = new int[nCnt];
                for (int i = 0; i < nCnt; i++)
                {
                    nMeanDiv[i] = 0;
                }

                for (int ls = 0; ls < nShowList.Length; ls++)
                {
                    int jj = nShowList[ls];
                    PointF[] pt1;
                    PointF[] pt2;
                    PointF[] pt3;
                    double oldx = 0, oldy = 0;
                    if (nShow == -1 || nShow == 0 || nShow == -2)
                    {
                        pt1 = new PointF[xBufferA[ls].Length];
                        for (int i = 0; i < nSLen[nShowList[ls]]; i++)
                        {
                            if (nShow != -1)
                                pt1[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferA[ls][i] - totalMinA) * h_scale);
                            else
                                pt1[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferA[ls][i] - totalMinA) * h_scale);
                            float temX = (float)(pt1[i].X + meanPt[i].X);
                            float temY = (float)(pt1[i].Y + meanPt[i].Y);
                            meanPt[i] = new PointF(temX, temY);
                            if (nShow == -2)
                            {
                                if (i == 0)
                                    continue;
                            }
                        }

                        if (nShow == -2)
                        {
                            Pen pGray = new Pen(Color.Gray, 1);
                            bufferedgraphic.Graphics.DrawLines(pGray, pt1);
                            pGray.Dispose();
                        }
                        else
                        {
                            Pen p1 = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 13) % 255, (jj * 11 + 11) % 255), 1);
                            bufferedgraphic.Graphics.DrawLines(p1, pt1);
                            p1.Dispose();
                        }
                    }

                    if (nShow == -1 || nShow == 1)
                    {
                        pt2 = new PointF[xBufferV[ls].Length];
                        for (int i = 0; i < xBufferV[ls].Length; i++)
                        {
                            pt2[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferV[ls][i] - totalMinA) * h_scale);
                        }

                        Pen p2 = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 170) % 255, (jj * 11 + 112) % 255), 1);
                        bufferedgraphic.Graphics.DrawLines(p2, pt2);
                        p2.Dispose();
                    }

                    if (nShow == -1 || nShow == 2)
                    {
                        pt3 = new PointF[xBufferD[ls].Length];
                        for (int i = 0; i < xBufferD[ls].Length; i++)
                        {
                            if (nShow != -1)
                                pt3[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferD[ls][i] - totalMinA) * h_scale);
                            else
                                pt3[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferD[ls][i] - totalMinA) * h_scale);
                        }
                        Pen p3 = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 150) % 255, 255 - (jj * 11 + 211) % 255), 1);
                        bufferedgraphic.Graphics.DrawLines(p3, pt3);
                        p3.Dispose();
                    }
                }

                PointF[] ptSE;

                if (bSE)
                {
                    ptSE = new PointF[xBufferSE.Length];
                    for (int i = 0; i < xBufferSE.Length; i++)
                    {
                        if (nShow != -1)
                            ptSE[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE[i] - totalMinA) * h_scale);
                        else
                            ptSE[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE[i] - totalMinA) * h_scale);
                    }

                    if (nShow == -2)
                    {
                        Pen pRed = new Pen(Color.Red, 2);
                        bufferedgraphic.Graphics.DrawLines(pRed, ptSE);
                        pRed.Dispose();
                    }
                    else
                    {
                        if (matchingType != 3)
                        {
                            Pen pBlue = new Pen(Color.Blue, 2);
                            bufferedgraphic.Graphics.DrawLines(pBlue, ptSE);
                            pBlue.Dispose();
                        }



                    }

                }

                if (bSE0)
                {
                    PointF[] ptSE0;
                    ptSE0 = new PointF[xBufferSE0.Length];
                    for (int i = 0; i < xBufferSE0.Length; i++)
                    {
                        if (nShow != -1)
                            ptSE0[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE0[i] - totalMinA) * h_scale);
                        else
                            ptSE0[i] = new PointF((float)(i * deltT) * w_scale + padding_left, nHeight - padding_bottom - (float)(xBufferSE0[i] - totalMinA) * h_scale);
                    }
                    Pen pBlue1 = new Pen(Color.FromArgb(211, 65, 208), 1);
                    bufferedgraphic.Graphics.DrawLines(pBlue1, ptSE0);
                    pBlue1.Dispose();
                }

                if (nShow == -1)
                {
                    string strTitile = "Spectral Acceleration (Sa,g)\nSpectral Acceleration (SV,cm/sn)\nSpectral Acceleration (Sa, g)";
                    //                     string strTitile2 = "Spectral Velocity (SV,cm/sn)";
                    //                     string strTitile3 = "Spectral Acceleration (Sa,g)";
                    System.Drawing.Drawing2D.Matrix matrixbackup = bufferedgraphic.Graphics.Transform;
                    try
                    {

                        System.Drawing.Drawing2D.Matrix cmatrix = bufferedgraphic.Graphics.Transform.Clone();
                        Font font = new Font("Arial", 10);
                        int fontWidth = (int)TextRenderer.MeasureText(strTitile, font).Width;
                        int fontHeight = (int)TextRenderer.MeasureText(strTitile, font).Height;
                        int nH = (int)(nHeight / 4 + fontWidth / 4);
                        PointF o = new PointF(35, nH);
                        cmatrix.RotateAt(180, o);
                        bufferedgraphic.Graphics.Transform = cmatrix;
                        Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                        RectangleF rect = new RectangleF(5, 0, 90, fontWidth + 10);
                        StringFormat ft = new StringFormat();
                        ft.FormatFlags = StringFormatFlags.DirectionVertical;
                        bufferedgraphic.Graphics.DrawString(strTitile, font, brush, rect, ft);
                    }
                    finally
                    {
                        bufferedgraphic.Graphics.Transform = matrixbackup;
                    }
                }
                else
                {
                    string strTitile = "";
                    if (nShow == 0)
                    {
                        strTitile = "Spectral Acceleration (Sa,g)";
                    }
                    else if (nShow == 1)
                    {
                        strTitile = "Spectral Velocity (SV,cm/sn)";
                    }
                    else if (nShow == 2)
                    {
                        strTitile = "Spectral Displacement (SD,cm)";
                    }
                    else if (nShow == -2)
                    {
                        strTitile = "Spectral Acceleration (Sa,g)";

                        for (int i = 0; i < (int)(totalMaxT / deltT); i++)
                        {
                            float temX = meanPt[i].X / nShowList.Length;
                            float temY = meanPt[i].Y / nShowList.Length;
                            meanPt[i] = new PointF(temX, temY);
                        }
                        Pen pBlack = new Pen(Color.Black, 2);
                        bufferedgraphic.Graphics.DrawLines(pBlack, meanPt);
                        pBlack.Dispose();
                    }

                    System.Drawing.Drawing2D.Matrix matrixbackup = bufferedgraphic.Graphics.Transform;
                    try
                    {

                        System.Drawing.Drawing2D.Matrix cmatrix = bufferedgraphic.Graphics.Transform.Clone();
                        Font font = new Font("Arial", 10);
                        int fontWidth = (int)TextRenderer.MeasureText(strTitile, font).Width;
                        int fontHeight = (int)TextRenderer.MeasureText(strTitile, font).Height;
                        int nH = (int)(nHeight / 4 + fontWidth / 4);
                        PointF o = new PointF(15, nH);
                        cmatrix.RotateAt(180, o);
                        bufferedgraphic.Graphics.Transform = cmatrix;
                        Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                        RectangleF rect = new RectangleF(5, 0, 30, fontWidth + 10);
                        StringFormat ft = new StringFormat();
                        ft.FormatFlags = StringFormatFlags.DirectionVertical;
                        bufferedgraphic.Graphics.DrawString(strTitile, font, brush, rect, ft);
                    }
                    finally
                    {
                        bufferedgraphic.Graphics.Transform = matrixbackup;
                    }
                }

                string strBottrom = "Period (T)";
                Font font1 = new Font("Arial", 10);

                int fontWidth1 = (int)TextRenderer.MeasureText(strBottrom, font1).Width;
                Brush brush1 = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
                bufferedgraphic.Graphics.DrawString(strBottrom, font1, brush1, nWidth / 2, nHeight - 20);//, ft1);

                bufferedgraphic.Render(signGraphic);

            }

            return bitmap;
        }

        private Bitmap DrawAllGraphToClipboard(Object sender, Color color, string strTitile, double[][] sBuf, double[][] tBuf, int[] nCnt, int[] showlist, float nWidth, float nHeight, double[] nMaxA, double[] nMinA, double[] nMaxT, double[] nMinT, float[] Padding, int style = 0)
        {

            float padding_left = 50;
            float padding_right = 50;
            float padding_top = 30;
            float padding_bottom = 30;

            if (Padding != null)
            {
                padding_left = Padding[0];
                padding_top = Padding[1];
                padding_right = Padding[2];
                padding_bottom = Padding[3];
            }

            float xSperat = 0;
            float ySperat = 0;
            float xStep = 0;
            float yStep = 0;

            ///////////////////////////////////////////////////////////////
            //Get MaxA in datas;
            double totalMaxA = nMaxA[showlist[0]], totalMinA = nMinA[showlist[0]];
            double totalMaxT = nMaxT[showlist[0]], totalMinT = nMinT[showlist[0]];
            int nMaxCnt = nCnt[showlist[0]];
            for (int ls = 1; ls < showlist.Length; ls++)
            {
                int ii = showlist[ls];
                if (totalMaxA < nMaxA[ii])
                    totalMaxA = nMaxA[ii];

                if (totalMinA > nMinA[ii])
                    totalMinA = nMinA[ii];

                if (totalMaxT < nMaxT[ii])
                    totalMaxT = nMaxT[ii];

                if (totalMinT > nMinT[ii])
                    totalMinT = nMinT[ii];

                if (nMaxCnt < nCnt[ii])
                    nMaxCnt = nCnt[ii];

            }
            /////////////////////////////////////////////////////////////

            double[][] xBuffer = new double[showlist.Length][];
            int nScale = 0;
            for (int ls = 0; ls < showlist.Length - 1; ls++)
            {
                int ii = showlist[ls];
                xBuffer[ls] = new double[nCnt[ii]];
                double temMax = totalMaxA;
                double temMin = totalMinA;
                Normalize(sBuf[ii], ref xBuffer[ls], nCnt[ii], ref temMax, ref temMin, ref nScale);
            }

            xBuffer[showlist.Length - 1] = new double[nCnt[showlist[showlist.Length - 1]]];
            Normalize(sBuf[showlist[showlist.Length - 1]], ref xBuffer[showlist.Length - 1], nCnt[showlist[showlist.Length - 1]], ref totalMaxA, ref totalMinA, ref nScale);


            float center_y;
            float h_scale;
            float w_scale;

            double yscale = 1;

            if (style == 3)
            {
                xSperat = (float)Math.Ceiling(Math.Log10(m_dMaxT[showlist[0]])) + 2;
                xStep = (nWidth - padding_left - padding_right) / xSperat;

                yStep = 0.1f;
                ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                if (ySperat < 4)
                {
                    yStep /= 2;
                    ySperat *= 2;
                }
                else if (ySperat > 10)
                {
                    yStep *= 2;
                    ySperat /= 2;
                }

                if (style == 1)
                {
                    yStep *= 2;
                    ySperat /= 2;
                }

                totalMaxA = (double)((decimal)Math.Ceiling((double)((decimal)totalMaxA / (decimal)yStep)) * (decimal)yStep);
                totalMinA = (double)((decimal)Math.Floor((double)((decimal)totalMinA / (decimal)yStep)) * (decimal)yStep);
                ySperat = (float)Math.Round((double)(((decimal)totalMaxA - (decimal)totalMinA) / (decimal)yStep));
            }
            else
            {
                int nIndexList = showlist[0];
                if (sender == picDI_Acceleration)
                {
                    double timeInterval = 0;
                    Double.TryParse(textDI_TimeInterval.Text.ToString(), NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double accInterval = 0;
                    Double.TryParse(textDI_AccInterval.Text.ToString(), NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out accInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if (accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textDI_AccInterval.Text = strAcc.ToString();
                            textDI_TimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(accInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }
                        //LogText("TEST" + yStep.ToString());

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)accInterval;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }
                    }

                }
                else if (sender == picSMD_Acceleration)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_AccTimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double accInterval = 0;
                    Double.TryParse(_SMD_AccInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out accInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if (accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_AccInterval.Text = strAcc.ToString();
                            textSMD_AccTimeInterval.Text = xStep.ToString();

                        }
                        else
                        {
                            yStep = (float)(accInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }


                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (accInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(accInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }
                    }
                    _SMD_AccInterval[nIndexList] = textSMD_AccInterval.Text;
                    _SMD_AccTimeInterval[nIndexList] = textSMD_AccTimeInterval.Text;
                }
                else if (sender == picSMD_Velocity)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_VccTimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double VccInterval = 0;
                    Double.TryParse(_SMD_VccInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out VccInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if (VccInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_VccInterval.Text = strAcc.ToString();
                            textSMD_VccTimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(VccInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (VccInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(VccInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }

                    _SMD_VccInterval[nIndexList] = textSMD_VccInterval.Text;
                    _SMD_VccTimeInterval[nIndexList] = textSMD_VccTimeInterval.Text;

                }
                else if (sender == picSMD_Displacement)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_DisTimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double DisInterval = 0;
                    Double.TryParse(_SMD_DisInterval[nIndexList], NumberStyles.Any,
                            CultureInfo.CurrentCulture.NumberFormat, out DisInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if (DisInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_DisInterval.Text = strAcc.ToString();
                            textSMD_DisTimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(DisInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (DisInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(DisInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }

                    _SMD_DisInterval[nIndexList] = textSMD_DisInterval.Text;
                    _SMD_DisTimeInterval[nIndexList] = textSMD_DisTimeInterval.Text;

                }
                else if (sender == picSMD_AriasIntensity)
                {
                    double timeInterval = 0;
                    Double.TryParse(_SMD_AITimeInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

                    double AIInterval = 0;
                    Double.TryParse(_SMD_AIInterval[nIndexList], NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out AIInterval);

                    if (timeInterval == 0)
                    {
                        xStep = 5;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (xSperat > 50)
                        {
                            xStep = 10;
                            xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                        }

                        if (AIInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                            double strAcc = (double)((decimal)yStep * (decimal)Math.Pow(10, nScale));
                            textSMD_AIInterval.Text = strAcc.ToString();
                            textSMD_AITimeInterval.Text = xStep.ToString();
                        }
                        else
                        {
                            yStep = (float)(AIInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }

                    }
                    else
                    {
                        xStep = (float)timeInterval;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                        if (AIInterval == 0)
                        {
                            yStep = 0.1f;
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                            if (ySperat < 4)
                            {
                                yStep /= 2;
                                ySperat *= 2;
                            }
                            else if (ySperat > 10)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }

                            if (style == 1)
                            {
                                yStep *= 2;
                                ySperat /= 2;
                            }
                        }
                        else
                        {
                            yStep = (float)(AIInterval / Math.Pow(10, nScale));
                            ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);
                        }
                    }

                    _SMD_AIInterval[nIndexList] = textSMD_AIInterval.Text;
                    _SMD_AITimeInterval[nIndexList] = textSMD_AITimeInterval.Text;
                }
                else
                {
                    xStep = 5;
                    xSperat = (float)Math.Ceiling(totalMaxT / xStep);

                    if (xSperat > 50)
                    {
                        xStep = 10;
                        xSperat = (float)Math.Ceiling(totalMaxT / xStep);
                    }

                    yStep = 0.1f;
                    ySperat = (float)Math.Ceiling((FixDecimal1(totalMaxA) - FixDecimal1(totalMinA)) / yStep);

                    if (ySperat < 4)
                    {
                        yStep /= 2;
                        ySperat *= 2;
                    }
                    else if (ySperat > 10)
                    {
                        yStep *= 2;
                        ySperat /= 2;
                    }

                    if (style == 1)
                    {
                        yStep *= 2;
                        ySperat /= 2;
                    }
                }

                totalMaxA = (double)((decimal)Math.Ceiling((double)((decimal)totalMaxA / (decimal)yStep)) * (decimal)yStep);
                totalMinA = (double)((decimal)Math.Floor((double)((decimal)totalMinA / (decimal)yStep)) * (decimal)yStep);
                ySperat = (float)Math.Round((double)(((decimal)totalMaxA - (decimal)totalMinA) / (decimal)yStep));
            }



            //yStep = (float)((nHeight - padding_top - padding_bottom) / ySperat);


            center_y = (nHeight - padding_top - padding_bottom) / 2 + padding_top;
            h_scale = (nHeight - padding_top - padding_bottom) / (float)(totalMaxA - totalMinA);
            if (style == 3)
                w_scale = (nWidth - padding_left - padding_right);
            else
                w_scale = (nWidth - padding_left - padding_right) / (float)(Math.Ceiling(totalMaxT / 5.0) * 5);

            ///////////////////////////////////////////////////////////////            
            Bitmap bitmap = new Bitmap((int)nWidth, (int)nHeight);
            Graphics signGraphic = Graphics.FromImage(bitmap);
            Rectangle rectDC = new Rectangle(0, 0, (int)nWidth, (int)nHeight);
            using (BufferedGraphics bufferedgraphic = BufferedGraphicsManager.Current.Allocate(signGraphic, rectDC))
            {
                bufferedgraphic.Graphics.Clear(m_colorPicBack);
                bufferedgraphic.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                bufferedgraphic.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                bufferedgraphic.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);

                Pen p_axis = new Pen(Color.FromArgb(0, 0, 0), 2);
                float dis_axis = 5;
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)padding_top - dis_axis, padding_left, (float)nHeight - padding_bottom + dis_axis);
                bufferedgraphic.Graphics.DrawLine(p_axis, padding_left, (float)nHeight - padding_bottom + dis_axis, nWidth - padding_right + dis_axis, (float)nHeight - padding_bottom + dis_axis);

                Pen p_dot = new Pen(Color.Gray, 1);
                p_dot.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                p_dot.DashPattern = new float[] { 1.0F, 2.0F, 1.0F, 3.0F };
                float tem = (float)nHeight - padding_bottom;
                Font fnt = new Font("Arial", 10);
                string mark;

                int ys = 1;

                if (totalMaxA - totalMinA >= 1)
                    ys = 2;


                for (int yy = 0; yy <= ySperat; yy++)
                {
                    double y;
                    y = (double)((decimal)yy * (decimal)yStep + (decimal)totalMinA);
                    y = (double)((decimal)y * (decimal)Math.Pow(10, nScale));

                    if (y > 0)
                        mark = " " + y.ToString();
                    else if (y == 0)
                        mark = " " + y.ToString();
                    else
                        mark = y.ToString();
                    bufferedgraphic.Graphics.DrawLine(p_dot, padding_left, tem, nWidth - padding_right, tem);
                    bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(padding_left - 45), (int)tem - 10));
                    tem -= yStep * h_scale;
                }

                if (style == 3)
                {
                    for (int x = -2; x <= xSperat - 2; x++)
                    {
                        mark = "";
                        if (x != -2)
                        {
                            mark += (Math.Pow(10, x)).ToString();
                            tem = padding_left + xStep * (x + 2);
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem), (int)(nHeight - padding_bottom + dis_axis + 2)));

                        }
                        else
                        {
                            mark += "0";
                            tem = padding_left + xStep * (x + 2);
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));

                        }

                    }
                }
                else
                {
                    tem = (float)padding_left;
                    for (int x = 0; x <= xSperat; x++)
                    {
                        mark = "";
                        if (x != 0)
                        {
                            int nspace = (int)(Math.Log10(nMaxCnt)) - (int)(Math.Log10(x));
                            for (int i = 0; i < nspace; i++)
                                mark += " ";

                            mark += ((double)((decimal)x * (decimal)xStep)).ToString();
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 25), (int)(nHeight - padding_bottom + dis_axis + 2)));
                            tem += xStep * w_scale;
                        }
                        else
                        {
                            mark += "0";
                            bufferedgraphic.Graphics.DrawLine(p_dot, tem, padding_top, tem, nHeight - padding_bottom + dis_axis);
                            bufferedgraphic.Graphics.DrawString(mark, fnt, System.Drawing.Brushes.Black, new Point((int)(tem - 5), (int)(nHeight - padding_bottom + dis_axis + 2)));
                            tem += xStep * w_scale;
                        }

                    }
                }

                //Draw Grapg ...
                for (int st = 0; st < showlist.Length; st++)
                {
                    int jj = showlist[st];
                    Pen p = new Pen(Color.FromArgb((jj * 171 + 236) % 255, (jj * 53 + 13) % 255, (jj * 11 + 11) % 255), 1);

                    PointF[] pt = new PointF[nCnt[jj]];
                    if (totalMinA < 0)
                    {
                        for (int i = 0; i < nCnt[jj]; i++)
                        {
                            if (style == 3)
                                pt[i] = new PointF(i * w_scale + padding_left, center_y - (float)xBuffer[st][i] * h_scale);
                            else
                                pt[i] = new PointF((float)tBuf[jj][i] * w_scale + padding_left, center_y - (float)xBuffer[st][i] * h_scale);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < nCnt[jj]; i++)
                        {
                            if (style == 3)
                            {
                                if (tBuf[jj][i] != 0)
                                {
                                    float www = (float)(Math.Log10(tBuf[jj][i]) + 2) / (float)(Math.Log10(totalMaxT) + 2);
                                    if (www < 0)
                                        www = 0;
                                    pt[i] = new PointF(www * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[st][i] - totalMinA) * h_scale);
                                }
                                else
                                    pt[i] = new PointF(padding_left, nHeight - padding_bottom - (float)(xBuffer[st][i] - totalMinA) * h_scale);

                            }
                            else
                            {
                                pt[i] = new PointF((float)tBuf[jj][i] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[st][i] - totalMinA) * h_scale);
                            }

                        }
                    }
                    bufferedgraphic.Graphics.DrawLines(p, pt);
                    p.Dispose();
                }

                if (style == 1)
                {
                    int index = showlist[0];
                    Pen pline = new Pen(Color.FromArgb(0, 0, 255), 1);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D5AI_pos[index]] - totalMinA) * h_scale, (float)tBuf[index][_D5AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D5AI_pos[index]] - totalMinA) * h_scale);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D95AI_pos[index]] - totalMinA) * h_scale, (float)tBuf[index][_D95AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D95AI_pos[index]] - totalMinA) * h_scale);

                    bufferedgraphic.Graphics.DrawLine(pline, (float)tBuf[index][_D5AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom + dis_axis, (float)tBuf[index][_D5AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D5AI_pos[index]] - totalMinA) * h_scale);
                    bufferedgraphic.Graphics.DrawLine(pline, (float)tBuf[index][_D95AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom + dis_axis, (float)tBuf[index][_D95AI_pos[index]] * w_scale + padding_left, nHeight - padding_bottom - (float)(xBuffer[0][_D95AI_pos[index]] - totalMinA) * h_scale);
                }
                else if (style == 2)
                {
                    int index = showlist[0];
                    Pen pline = new Pen(Color.FromArgb(0, 0, 255), 1);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, center_y + (float)_Bracked_value[m_nCountMotion - 1] * h_scale, nWidth - padding_right, center_y + (float)_Bracked_value[m_nCountMotion - 1] * h_scale);
                    bufferedgraphic.Graphics.DrawLine(pline, padding_left, center_y - (float)_Bracked_value[m_nCountMotion - 1] * h_scale, nWidth - padding_right, center_y - (float)_Bracked_value[m_nCountMotion - 1] * h_scale);
                }
                else if (style == 3)
                {

                    string strBottrom = "Frequency (Hz)";
                    Font font1 = new Font("Arial", 10);

                    int fontWidth1 = (int)TextRenderer.MeasureText(strBottrom, font1).Width;
                    Brush brush1 = new SolidBrush(Color.FromArgb(255, 0, 0, 255));
                    bufferedgraphic.Graphics.DrawString(strBottrom, font1, brush1, nWidth / 2, nHeight - 20);//, ft1);

                }


                ///////////////////////
                System.Drawing.Drawing2D.Matrix matrixbackup = bufferedgraphic.Graphics.Transform;
                try
                {
                    System.Drawing.Drawing2D.Matrix cmatrix = bufferedgraphic.Graphics.Transform.Clone();
                    Font font = new Font("Arial", 10);
                    int fontWidth = (int)TextRenderer.MeasureText(strTitile, font).Width;
                    int fontHeight = (int)TextRenderer.MeasureText(strTitile, font).Height;
                    int nH = (int)(nHeight / 4 + fontWidth / 4);
                    PointF o = new PointF(15, nH);
                    cmatrix.RotateAt(180, o);
                    bufferedgraphic.Graphics.Transform = cmatrix;
                    Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 255));

                    RectangleF rect = new RectangleF(5, 0, 30, fontWidth + 10);
                    StringFormat ft = new StringFormat();
                    ft.FormatFlags = StringFormatFlags.DirectionVertical;
                    bufferedgraphic.Graphics.DrawString(strTitile, font, brush, rect, ft);
                }
                finally
                {
                    bufferedgraphic.Graphics.Transform = matrixbackup;
                }
                //////////////////////////
                //bufferedgraphic.Render(e.Graphics);
                bufferedgraphic.Render(signGraphic);
                //signGraphic = e.Graphics;                
                //DrawCtrl.Image = bitmap;
            }

            return bitmap;
        }

        private void PostControlMsg(int command)
        {
            switch (command)
            {
                case (Int32)ControlMessage.msgDI_MotionList:
                    if (checkedListDI_Accelogram.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        checkedListDI_Accelogram.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        checkedListDI_Accelogram.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            checkedListDI_Accelogram.Items.Add(m_strListName[i], true);
                        }

                    }
                    break;
                case (Int32)ControlMessage.msgDI_ListS_AVD:
                    if (listViewDI_S_AVD.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewDI_S_AVD.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        int index = comboDI_S_AVD.SelectedIndex;

                        listViewDI_S_AVD.Items.Clear();
                        if (index < 0)
                            break;

                        try
                        {
                            listViewDI_S_AVD.BeginUpdate();

                            int ndisplay = comboDI_SpectList.SelectedIndex;

                            if (ndisplay == 0)
                            {
                                listViewDI_S_AVD.Columns[0].Text = "T(s)";
                                listViewDI_S_AVD.Columns[1].Text = "S(a)g";
                                double stepT = 0.02;
                                for (int i = 0; i < nSLen[index]; i++)
                                {
                                    ListViewItem lvi1 = new ListViewItem(FixDecimal3(stepT * i).ToString(), 0);
                                    lvi1.SubItems.Add(FixDecimal3(_S_A[index][i]).ToString());
                                    listViewDI_S_AVD.Items.Add(lvi1);
                                }
                            }
                            else if (ndisplay == 1)
                            {
                                listViewDI_S_AVD.Columns[0].Text = "T(s)";
                                listViewDI_S_AVD.Columns[1].Text = "S(v)g";
                                double stepT = 0.02;
                                for (int i = 0; i < nSLen[index]; i++)
                                {
                                    ListViewItem lvi1 = new ListViewItem(FixDecimal3(stepT * i).ToString(), 0);
                                    lvi1.SubItems.Add(FixDecimal3(_S_V[index][i]).ToString());
                                    listViewDI_S_AVD.Items.Add(lvi1);
                                }
                            }
                            else
                            {
                                listViewDI_S_AVD.Columns[0].Text = "T(s)";
                                listViewDI_S_AVD.Columns[1].Text = "S(d)g";
                                double stepT = 0.02;
                                for (int i = 0; i < nSLen[index]; i++)
                                {
                                    ListViewItem lvi1 = new ListViewItem(FixDecimal3(stepT * i).ToString(), 0);
                                    lvi1.SubItems.Add(FixDecimal3(_S_D[index][i]).ToString());
                                    listViewDI_S_AVD.Items.Add(lvi1);
                                }
                            }
                        }
                        finally
                        {
                            listViewDI_S_AVD.EndUpdate();
                        }



                    }
                    break;

                case (Int32)ControlMessage.msgDI_ComboS_AVD:
                    if (comboDI_S_AVD.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        comboDI_S_AVD.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        comboDI_S_AVD.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            comboDI_S_AVD.Items.Add(m_strListName[i]);
                        }

                        comboDI_S_AVD.SelectedIndex = m_nCountMotion - 1;
                    }
                    break;
                case (Int32)ControlMessage.msgSMD_MotionDataCombo:
                    if (comboSMD_MotionDatas.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        comboSMD_MotionDatas.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        comboSMD_MotionDatas.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            comboSMD_MotionDatas.Items.Add(m_strInputMotionFiles[i]);
                        }

                        comboSMD_MotionDatas.SelectedIndex = m_nCountMotion - 1;
                    }
                    break;
                case (Int32)ControlMessage.msgSM_MD_MotionDataCombo:
                    if (comboSM_MDMotionDatas.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        comboSM_MDMotionDatas.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        comboSM_MDMotionDatas.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            comboSM_MDMotionDatas.Items.Add(m_strInputMotionFiles[i]);
                        }
                        comboSM_MDMotionDatas.SelectedIndex = m_nCountMotion - 1;
                    }
                    break;
                case (Int32)ControlMessage.msgSM_MS_MotionDataCombo:
                    if (comboSM_MSMotionDatas.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        comboSM_MSMotionDatas.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        comboSM_MSMotionDatas.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            comboSM_MSMotionDatas.Items.Add(m_strInputMotionFiles[i]);
                        }
                        comboSM_MSMotionDatas.SelectedIndex = m_nCountMotion - 1;
                    }
                    break;
                case (Int32)ControlMessage.msgSMD_ListTA:
                    if (listViewSMD_T_A.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewSMD_T_A.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        try
                        {
                            listViewSMD_T_A.Items.Clear();
                            listViewSMD_T_A.BeginUpdate();
                            LogText("Clearitem");

                            LogText("Additem");
                            int currentIndex = m_nSMDComboMotionIndex;
                            if (currentIndex < 0)
                                break;

                            for (int i = 0; i < m_nCnt[currentIndex]; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(_T[currentIndex][i].ToString(), 0);
                                lvi1.SubItems.Add(FixDecimal3(_A[currentIndex][i]).ToString());
                                listViewSMD_T_A.Items.Add(lvi1);
                            }

                            LogText("Enditem");
                        }
                        finally
                        {
                            listViewSMD_T_A.EndUpdate();
                        }

                    }
                    break;
                case (Int32)ControlMessage.msgSMD_ListTV:
                    if (listViewSMD_T_V.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewSMD_T_V.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        try
                        {
                            listViewSMD_T_V.Items.Clear();
                            listViewSMD_T_V.BeginUpdate();
                            int currentIndex = m_nSMDComboMotionIndex;
                            if (currentIndex < 0)
                                break;


                            for (int i = 0; i < m_nCnt[currentIndex]; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(_T[currentIndex][i].ToString(), 0);
                                lvi1.SubItems.Add(FixDecimal3(_V[currentIndex][i]).ToString());
                                listViewSMD_T_V.Items.Add(lvi1);
                            }
                        }
                        finally
                        {
                            listViewSMD_T_V.EndUpdate();
                        }

                    }
                    break;
                case (Int32)ControlMessage.msgSMD_ListTD:
                    if (listViewSMD_T_D.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewSMD_T_D.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        try
                        {
                            listViewSMD_T_D.Items.Clear();
                            listViewSMD_T_D.BeginUpdate();
                            int currentIndex = m_nSMDComboMotionIndex;
                            if (currentIndex < 0)
                                break;

                            for (int i = 0; i < m_nCnt[currentIndex]; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(_T[currentIndex][i].ToString(), 0);
                                lvi1.SubItems.Add(FixDecimal3(_D[currentIndex][i]).ToString());
                                listViewSMD_T_D.Items.Add(lvi1);
                            }
                        }
                        finally
                        {
                            listViewSMD_T_D.EndUpdate();
                        }

                    }
                    break;
                case (Int32)ControlMessage.msgSM_ListTarget:
                    if (listViewSM_Target.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewSM_Target.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        listViewSM_Target.Items.Clear();

                        try
                        {
                            listViewSM_Target.BeginUpdate();
                            int currentIndex = comboSM_MDMotionDatas.SelectedIndex;
                            if (currentIndex < 0)
                                break;

                            for (int i = 0; i < nSLen[currentIndex]; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(_Sae_T[i].ToString(), 0);
                                lvi1.SubItems.Add(FixDecimal3(_Sae0[i]).ToString());
                                listViewSM_Target.Items.Add(lvi1);
                            }
                        }
                        finally
                        {
                            listViewSM_Target.EndUpdate();
                        }
                    }
                    break;
                case (Int32)ControlMessage.msgSM_SM_Alfa:
                    if (listSM_SM_Alfa.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listSM_SM_Alfa.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        listSM_SM_Alfa.Items.Clear();

                        try
                        {
                            listSM_SM_Alfa.BeginUpdate();
                            for (int i = 0; i < m_nCountMotion; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(m_strListName[i], 0);
                                lvi1.SubItems.Add(FixDecimal3(_Alfa[i]).ToString());
                                lvi1.SubItems.Add(FixDecimal3(_Error[i]).ToString());
                                listSM_SM_Alfa.Items.Add(lvi1);
                            }
                        }
                        finally
                        {
                            listSM_SM_Alfa.EndUpdate();
                        }
                    }
                    break;
                case (Int32)ControlMessage.msgSM_MD_Alfa:
                    if (listSM_MD_Alfa.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listSM_MD_Alfa.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        listSM_MD_Alfa.Items.Clear();
                        try
                        {
                            listSM_MD_Alfa.BeginUpdate();

                            for (int i = 0; i < m_nCountMotion; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(m_strListName[i], 0);
                                lvi1.SubItems.Add(FixDecimal3(_Alfa[i]).ToString());
                                lvi1.SubItems.Add(FixDecimal3(_Error[i]).ToString());
                                listSM_MD_Alfa.Items.Add(lvi1);
                            }
                        }
                        finally
                        {
                            listSM_SM_Alfa.EndUpdate();
                        }

                    }
                    break;
                case (Int32)ControlMessage.msgSM_MotionList:
                    if (checkedListSM_Accelogram.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        checkedListSM_Accelogram.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        checkedListSM_Accelogram.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            checkedListSM_Accelogram.Items.Add(m_strListName[i], true);
                        }
                    }
                    break;
                case (Int32)ControlMessage.msgDS_MotionList:
                    if (checkedListDS_Accelogram.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        checkedListDS_Accelogram.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        checkedListDS_Accelogram.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            checkedListDS_Accelogram.Items.Add(m_strListName[i], true);
                        }
                    }
                    break;
                case (Int32)ControlMessage.msgMP_MotionList:
                    if (checkedListCF_Accelaration.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        checkedListCF_Accelaration.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        checkedListCF_Accelaration.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            checkedListCF_Accelaration.Items.Add(m_strListName[i], true);
                        }
                    }
                    break;
                case (Int32)ControlMessage.msgMP_MotionDataCombo:
                    if (comboMP_MotionDatas.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        comboMP_MotionDatas.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        comboMP_MotionDatas.Items.Clear();
                        for (int i = 0; i < m_nCountMotion; i++)
                        {
                            comboMP_MotionDatas.Items.Add(m_strInputMotionFiles[i]);
                        }
                        comboMP_MotionDatas.SelectedIndex = m_nCountMotion - 1;
                    }
                    break;
                case (Int32)ControlMessage.msgMP_ListIntensityParameters:
                    if (listViewMP_IntensityParameter.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewMP_IntensityParameter.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        listViewMP_IntensityParameter.Items.Clear();
                        int currentIndex = comboMP_MotionDatas.SelectedIndex;
                        if (currentIndex < 0)
                            break;

                        ListViewItem lvi1 = new ListViewItem("PGA (g)", 0);
                        lvi1.SubItems.Add(FixDecimal3(_PGA[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi1);

                        ListViewItem lvi2 = new ListViewItem("PGV (cm/s)", 0);
                        lvi2.SubItems.Add(FixDecimal3(_PGV[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi2);

                        ListViewItem lvi3 = new ListViewItem("PGD (g)", 0);
                        lvi3.SubItems.Add(FixDecimal3(_PGD[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi3);

                        ListViewItem lvi4 = new ListViewItem("Vmax / Amax (s)", 0);
                        lvi4.SubItems.Add(FixDecimal3((_PGV[currentIndex] / (_PGA[currentIndex] * 980.7))).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi4);

                        ListViewItem lvi5 = new ListViewItem("ACC_ams (g)", 0);
                        lvi5.SubItems.Add(FixDecimal3(_A_ams[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi5);

                        ListViewItem lvi6 = new ListViewItem("Vel_ams (cn/sn)", 0);
                        lvi6.SubItems.Add(FixDecimal3(_V_ams[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi6);

                        ListViewItem lvi7 = new ListViewItem("DISP_ams (cm)", 0);
                        lvi7.SubItems.Add(FixDecimal3(_D_ams[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi7);

                        ListViewItem lvi8 = new ListViewItem("Arias Intensity (m/s)", 0);
                        lvi8.SubItems.Add(FixDecimal3(_AI_MP[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi8);

                        ListViewItem lvi9 = new ListViewItem("Characteristic Intensity (lc)", 0);
                        lvi9.SubItems.Add(FixDecimal3(_Ic[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi9);

                        ListViewItem lvi10 = new ListViewItem("SED (cm²/s)", 0);
                        lvi10.SubItems.Add(FixDecimal3(_SED[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi10);

                        ListViewItem lvi11 = new ListViewItem("CAV (cm/sec)", 0);
                        lvi11.SubItems.Add(FixDecimal3(_CAV[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi11);

                        ListViewItem lvi12 = new ListViewItem("NoEC", 0);
                        lvi12.SubItems.Add(_NoECA[currentIndex].ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi12);

                        ListViewItem lvi13 = new ListViewItem("Sustained Max Acc(3, 5)", 0);
                        lvi13.SubItems.Add(FixDecimal3(_SusA3[currentIndex]).ToString() + ",    " + FixDecimal3(_SusA5[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi13);

                        ListViewItem lvi14 = new ListViewItem("Sustained Max Val(3, 5)", 0);
                        lvi14.SubItems.Add(FixDecimal3(_SusV3[currentIndex]).ToString() + ",    " + FixDecimal3(_SusV5[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi14);

                        ListViewItem lvi15 = new ListViewItem("Sustained Max Disp(3, 5)", 0);
                        lvi15.SubItems.Add(FixDecimal3(_SusD3[currentIndex]).ToString() + ",    " + FixDecimal3(_SusD5[currentIndex]).ToString());
                        listViewMP_IntensityParameter.Items.Add(lvi15);

                    }
                    break;
                case (Int32)ControlMessage.msgMP_ListDurationParameters:
                    if (listViewMP_DurationParamer.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewMP_DurationParamer.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        listViewMP_DurationParamer.Items.Clear();
                        int currentIndex = comboMP_MotionDatas.SelectedIndex;
                        if (currentIndex < 0)
                            break;

                        try
                        {
                            listViewMP_DurationParamer.BeginUpdate();

                            ListViewItem lvi1 = new ListViewItem("D5(IA)   (s)", 0);
                            lvi1.SubItems.Add(FixDecimal3(_D5AI[currentIndex]).ToString());
                            listViewMP_DurationParamer.Items.Add(lvi1);

                            ListViewItem lvi2 = new ListViewItem("D95(IA)   (s)", 0);
                            lvi2.SubItems.Add(FixDecimal3(_D95AI[currentIndex]).ToString());
                            listViewMP_DurationParamer.Items.Add(lvi2);

                            ListViewItem lvi3 = new ListViewItem("Significant Duration(s)", 0);
                            lvi3.SubItems.Add(FixDecimal3(_sus_duration[currentIndex]).ToString());
                            listViewMP_DurationParamer.Items.Add(lvi3);

                            ListViewItem lvi4 = new ListViewItem("Bracketed Duration(s)", 0);
                            lvi4.SubItems.Add(FixDecimal3(_brack_duration[currentIndex]).ToString());
                            listViewMP_DurationParamer.Items.Add(lvi4);
                        }
                        finally
                        {
                            listViewMP_DurationParamer.EndUpdate();
                        }

                    }
                    break;

                case (Int32)ControlMessage.msgMP_ListValues:
                    if (listViewMP_Values.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewMP_Values.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        listViewMP_Values.Items.Clear();

                        int currentIndex = comboMP_MotionDatas.SelectedIndex;
                        if (currentIndex < 0)
                            break;

                        int nTypes = comboMP_F_P_S.SelectedIndex;
                        if (nTypes < 0)
                            break;

                        try
                        {
                            listViewMP_Values.BeginUpdate();

                            if (nTypes == 0)
                            {
                                listViewMP_Values.Columns[0].Text = "Freq(Hz)";
                                listViewMP_Values.Columns[1].Text = "Amplitude";
                                for (int i = 0; i < m_nCnt[currentIndex]; i++)
                                {
                                    ListViewItem lvi1 = new ListViewItem(FixDecimal3(FFT_F[currentIndex][i]).ToString(), 0);
                                    lvi1.SubItems.Add(FixDecimal3(FFT_A[currentIndex][i]).ToString());
                                    listViewMP_Values.Items.Add(lvi1);
                                }
                            }
                            else if (nTypes == 1)
                            {
                                listViewMP_Values.Columns[0].Text = "Freq(Hz)";
                                listViewMP_Values.Columns[1].Text = "Spectrum";
                                for (int i = 0; i < m_nCnt[currentIndex]; i++)
                                {
                                    ListViewItem lvi1 = new ListViewItem(FixDecimal3(FFT_F[currentIndex][i]).ToString(), 0);
                                    lvi1.SubItems.Add(FixDecimal3(PowerFFT_A[currentIndex][i]).ToString());
                                    listViewMP_Values.Items.Add(lvi1);
                                }
                            }
                            else if (nTypes == 2)
                            {
                                listViewMP_Values.Columns[0].Text = "Period(s)";
                                listViewMP_Values.Columns[1].Text = "Sa(g)";
                                double _ttt = 0.02;
                                for (int i = 0; i < nSLen[currentIndex]; i++)
                                {
                                    ListViewItem lvi1 = new ListViewItem(FixDecimal2(i * _ttt).ToString(), 0);
                                    lvi1.SubItems.Add(FixDecimal3(_S_A[currentIndex][i]).ToString());
                                    listViewMP_Values.Items.Add(lvi1);
                                }
                            }
                        }
                        finally
                        {
                            listViewMP_Values.EndUpdate();
                        }

                    }
                    break;

                case (Int32)ControlMessage.msgDS_ListMean:
                    if (listViewDS_Meadian.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        listViewDS_Meadian.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        int index = comboDI_S_AVD.SelectedIndex;

                        listViewDS_Meadian.Items.Clear();
                        if (index < 0)
                            break;

                        try
                        {
                            listViewDS_Meadian.BeginUpdate();

                            listViewDS_Meadian.Columns[0].Text = "T(s)";
                            listViewDS_Meadian.Columns[1].Text = "S_Mean(a)g";
                            double stepT = 0.02;
                            for (int i = 0; i < nSLen[index]; i++)
                            {
                                ListViewItem lvi1 = new ListViewItem(FixDecimal3(stepT * i).ToString(), 0);
                                lvi1.SubItems.Add(FixDecimal3(_meanS_A[i]).ToString());
                                listViewDS_Meadian.Items.Add(lvi1);
                            }

                        }
                        finally
                        {
                            listViewDS_Meadian.EndUpdate();
                        }
                    }
                    break;
                case (Int32)ControlMessage.msgMP_BracketDuration:
                    if (textMP_RatioBracketDuration.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        textMP_RatioBracketDuration.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        int index = comboMP_MotionDatas.SelectedIndex;
                        if (index >= 0)
                            labelMP_ValueBracketDuration.Text = (FixDecimal3(_Bracked_value[index])).ToString();
                    }
                    break;

                case (Int32)ControlMessage.msgFormRefresh:
                    if (this.InvokeRequired)
                    {
                        var d = new SafeCallDelegate(PostControlMsg);
                        this.Invoke(d, new object[] { command });
                    }
                    else
                    {
                        
                        this.Refresh();
                    }
                    break;

            }


            //PostControlMsg((Int32)ControlMessage.msgFromRefresh);

        }

        private void LogText(string log, bool bTime = true)
        {
            if (bLogSave)
            {
                StreamWriter writer;
                writer = File.AppendText("Log.txt");
                if (bTime)
                    writer.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " - " + log);
                else
                    writer.WriteLine(log);
                writer.Close();
            }

        }

        public void EventMethodInputData(int nCnt, double[] pDataTime, double[] pDataAcceleration, double dMaxT, double dMinT, double dMaxA, double dMinA)
        {
            //EventMethod 호출
            m_nCountMotion++;

            _SMD_AccInterval[m_nCountMotion - 1] = "0";
            _SMD_AccTimeInterval[m_nCountMotion - 1] = "0";

            _SMD_VccInterval[m_nCountMotion - 1] = "0";
            _SMD_VccTimeInterval[m_nCountMotion - 1] = "0";

            _SMD_DisInterval[m_nCountMotion - 1] = "0";
            _SMD_DisTimeInterval[m_nCountMotion - 1] = "0";

            _SMD_AIInterval[m_nCountMotion - 1] = "0";
            _SMD_AITimeInterval[m_nCountMotion - 1] = "0";

            m_nCnt[m_nCountMotion - 1] = nCnt;
            _T[m_nCountMotion - 1] = pDataTime;
            _A[m_nCountMotion - 1] = pDataAcceleration;

            m_dMaxT[m_nCountMotion - 1] = dMaxT;
            m_dMinT[m_nCountMotion - 1] = dMinT;
            m_dMaxA[m_nCountMotion - 1] = dMaxA;
            m_dMinA[m_nCountMotion - 1] = dMinA;
            delta_t[m_nCountMotion - 1] = _T[m_nCountMotion - 1][1] - _T[m_nCountMotion - 1][0];


            int logStep = m_nCnt[m_nCountMotion - 1] / 10;

            string strLog = "";
            LogText("--Data Receive--");
//             for (int i = 0; i < 10; i++)
//             {
//                 strLog = (i * logStep).ToString() + " line: " + _T[m_nCountMotion - 1][i * logStep].ToString() + "  " + _A[m_nCountMotion - 1][i * logStep].ToString();
//                 LogText(strLog);
//             }

//             strLog = "MaxT: " + m_dMaxT[m_nCountMotion - 1].ToString() + "  MinT: " + m_dMinT[m_nCountMotion - 1].ToString();
//             strLog = strLog + "  MaxA: " + m_dMaxA[m_nCountMotion - 1].ToString() + "  MinA" + m_dMinA[m_nCountMotion - 1].ToString();
//             strLog = strLog + "  DeltaT: " + delta_t[m_nCountMotion - 1].ToString();
//             LogText(strLog);
            LogText("--Process Start --");

            newThread.RunWorkerAsync();
        }

        private void PostAllMessage()
        {
            PostControlMsg((Int32)ControlMessage.msgDI_MotionList);

            PostControlMsg((Int32)ControlMessage.msgSM_MotionList);
            PostControlMsg((Int32)ControlMessage.msgDS_MotionList);
            PostControlMsg((Int32)ControlMessage.msgMP_MotionList);

            PostControlMsg((Int32)ControlMessage.msgMP_MotionDataCombo);

            PostControlMsg((Int32)ControlMessage.msgSM_MD_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgSM_MS_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgSMD_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgMP_ListValues);


            PostControlMsg((Int32)ControlMessage.msgSMD_ListTA);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTV);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTD);

            //PostControlMsg((Int32)ControlMessage.msgMP_ListIntensityParameters);
            //PostControlMsg((Int32)ControlMessage.msgMP_ListDurationParameters);
            PostControlMsg((Int32)ControlMessage.msgMP_BracketDuration);

            PostControlMsg((Int32)ControlMessage.msgDI_ComboS_AVD);
            PostControlMsg((Int32)ControlMessage.msgDS_ListMean);
        }

        private void DrawGraph()
        {
            //             if(!isLoaded)
            //                 return;

            PostAllMessage();

            

            PostControlMsg((Int32)ControlMessage.msgFormRefresh);
            //this.Refresh();
        }

        private void PerformReading(object sender, EventArgs e)
        {
            if (!_initVariable())
            {
                m_nCountMotion--;
                return;
            }
            DrawGraph();
        }

        private void ReadingCompleted(object sender, EventArgs e)
        {

        }

        private bool _initVariable()
        {
            if (!GetV_D())
                return false;
            GetSpectralAcc_Vel_Disp(m_nCountMotion - 1);
            isLoaded = true;
            return true;
        }

        private bool GetV_D()
        {

            _V[m_nCountMotion - 1] = new double[m_nCnt[m_nCountMotion - 1]];
            _D[m_nCountMotion - 1] = new double[m_nCnt[m_nCountMotion - 1]];
            _AI[m_nCountMotion - 1] = new double[m_nCnt[m_nCountMotion - 1]];


            double[] _Sort_A = new double[m_nCnt[m_nCountMotion - 1]];
            double[] _Sort_V = new double[m_nCnt[m_nCountMotion - 1]];
            double[] _Sort_D = new double[m_nCnt[m_nCountMotion - 1]];
            int peakLenA = 0;
            int peakLenV = 0;
            int peakLenD = 0;

            _V[m_nCountMotion - 1][0] = 0;
            double dt = delta_t[m_nCountMotion - 1];

            _V[m_nCountMotion - 1][0] = (_A[m_nCountMotion - 1][0]) / 2 * dt * (9.807 * 100);

            m_dMaxV[m_nCountMotion - 1] = -1;
            m_dMinV[m_nCountMotion - 1] = 1000;
            for (int n = 1; n < m_nCnt[m_nCountMotion - 1]; n++)
            {
                _V[m_nCountMotion - 1][n] = _V[m_nCountMotion - 1][n - 1] + (_A[m_nCountMotion - 1][n - 1] + _A[m_nCountMotion - 1][n]) / 2 * dt * (9.807 * 100);

                if (m_dMaxV[m_nCountMotion - 1] < _V[m_nCountMotion - 1][n])
                {
                    m_dMaxV[m_nCountMotion - 1] = _V[m_nCountMotion - 1][n];
                }

                if (m_dMinV[m_nCountMotion - 1] > _V[m_nCountMotion - 1][n])
                {
                    m_dMinV[m_nCountMotion - 1] = _V[m_nCountMotion - 1][n];
                }
            }

            m_dMaxD[m_nCountMotion - 1] = -1;
            m_dMinD[m_nCountMotion - 1] = 1000;
            _D[m_nCountMotion - 1][0] = (_V[m_nCountMotion - 1][1]) / 2 * dt + _A[m_nCountMotion - 1][0] / 2 * dt * dt * (9.807 * 100);
            for (int n = 1; n < m_nCnt[m_nCountMotion - 1]; n++)
            {
                _D[m_nCountMotion - 1][n] = _D[m_nCountMotion - 1][n - 1] + (_V[m_nCountMotion - 1][n - 1] + _V[m_nCountMotion - 1][n]) / 2 * dt + (_A[m_nCountMotion - 1][n - 1] + _A[m_nCountMotion - 1][n]) / 2 * dt * dt * (9.807 * 100);

                if (m_dMaxD[m_nCountMotion - 1] < _D[m_nCountMotion - 1][n])
                {
                    m_dMaxD[m_nCountMotion - 1] = _D[m_nCountMotion - 1][n];
                }

                if (m_dMinD[m_nCountMotion - 1] > _D[m_nCountMotion - 1][n])
                {
                    m_dMinD[m_nCountMotion - 1] = _D[m_nCountMotion - 1][n];
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////

            _PGA[m_nCountMotion - 1] = MaxAbsValue(m_dMaxA[m_nCountMotion - 1], m_dMinA[m_nCountMotion - 1]);
            _PGV[m_nCountMotion - 1] = MaxAbsValue(m_dMaxV[m_nCountMotion - 1], m_dMinV[m_nCountMotion - 1]);
            _PGD[m_nCountMotion - 1] = MaxAbsValue(m_dMaxD[m_nCountMotion - 1], m_dMinD[m_nCountMotion - 1]);



            _NoECA[m_nCountMotion - 1] = 0;
            int oldPosNOECA = 0;
            Boolean bSelectA = true;
            ///////////////////////////////////////////// _SusA3 _SusA5 ////////////////////////////////////////
            for (int i = 0; i < m_nCnt[m_nCountMotion - 1]; i++)
            {
                if (Math.Abs(_A[m_nCountMotion - 1][i]) > _PGA[m_nCountMotion - 1] * (m_dPGARatioForNOEC / 100) && bSelectA == true)
                {
                    if (_A[m_nCountMotion - 1][i] > 0)
                    {
                        oldPosNOECA = 1;
                    }
                    else
                    {
                        oldPosNOECA = -1;
                    }
                    _NoECA[m_nCountMotion - 1]++;
                    bSelectA = false;
                }

                if (Math.Abs(_A[m_nCountMotion - 1][i]) <= _PGA[m_nCountMotion - 1] * (m_dPGARatioForNOEC / 100))
                {
                    if ((_A[m_nCountMotion - 1][i] > 0 && oldPosNOECA == -1) || (_A[m_nCountMotion - 1][i] < 0 && oldPosNOECA == 1))
                        bSelectA = true;
                }
            }

            for (int i = 1; i < m_nCnt[m_nCountMotion - 1] - 1; i++)
            {
                if ((Math.Abs(_A[m_nCountMotion - 1][i]) > Math.Abs(_A[m_nCountMotion - 1][i - 1])) &&
                    (Math.Abs(_A[m_nCountMotion - 1][i]) > Math.Abs(_A[m_nCountMotion - 1][i + 1])))
                {
                    _Sort_A[peakLenA] = Math.Abs(_A[m_nCountMotion - 1][i]);
                    peakLenA++;
                }

                if ((Math.Abs(_V[m_nCountMotion - 1][i]) > Math.Abs(_V[m_nCountMotion - 1][i - 1])) &&
                            (Math.Abs(_V[m_nCountMotion - 1][i]) > Math.Abs(_V[m_nCountMotion - 1][i + 1])))
                {
                    _Sort_V[peakLenV] = Math.Abs(_V[m_nCountMotion - 1][i]);
                    peakLenV++;
                }

                if ((Math.Abs(_D[m_nCountMotion - 1][i]) > Math.Abs(_D[m_nCountMotion - 1][i - 1])) &&
                        (Math.Abs(_D[m_nCountMotion - 1][i]) > Math.Abs(_D[m_nCountMotion - 1][i + 1])))
                {
                    _Sort_D[peakLenD] = Math.Abs(_D[m_nCountMotion - 1][i]);
                    peakLenD++;
                }
            }

            for (int i = 0; i < peakLenA - 1; i++)
            {
                for (int j = i; j < peakLenA; j++)
                {
                    if (_Sort_A[i] < _Sort_A[j])
                    {
                        double dTem = _Sort_A[i];
                        _Sort_A[i] = _Sort_A[j];
                        _Sort_A[j] = dTem;
                    }
                }
            }


            for (int i = 0; i < peakLenV - 1; i++)
            {
                for (int j = i; j < peakLenV; j++)
                {
                    if (_Sort_V[i] < _Sort_V[j])
                    {
                        double dTem = _Sort_V[i];
                        _Sort_V[i] = _Sort_V[j];
                        _Sort_V[j] = dTem;
                    }
                }
            }

            for (int i = 0; i < peakLenD - 1; i++)
            {
                for (int j = i; j < peakLenD; j++)
                {
                    if (_Sort_D[i] < _Sort_D[j])
                    {
                        double dTem = _Sort_D[i];
                        _Sort_D[i] = _Sort_D[j];
                        _Sort_D[j] = dTem;
                    }
                }
            }

            _SusA3[m_nCountMotion - 1] = _Sort_A[2];
            _SusA5[m_nCountMotion - 1] = _Sort_A[4];
            ///////////////////////////////////////////// _SusV3 _SusV5 ////////////////////////////////////////
            _SusV3[m_nCountMotion - 1] = _Sort_V[2];
            _SusV5[m_nCountMotion - 1] = _Sort_V[4];

            ///////////////////////////////////////////// _SusV3 _SusV5 ////////////////////////////////////////
            _SusD3[m_nCountMotion - 1] = _Sort_D[2];
            _SusD5[m_nCountMotion - 1] = _Sort_D[4];
            ///////////////////////////////////////////////////////////////////////////////////////


            double g = 980.76;

            _A_ams[m_nCountMotion - 1] = 0;
            _V_ams[m_nCountMotion - 1] = 0;
            _D_ams[m_nCountMotion - 1] = 0;
            _SED[m_nCountMotion - 1] = 0;
            _CAV[m_nCountMotion - 1] = 0;

            for (int n = 0; n < m_nCnt[m_nCountMotion - 1]; n++)
            {
                _A_ams[m_nCountMotion - 1] += (_A[m_nCountMotion - 1][n] * _A[m_nCountMotion - 1][n] * delta_t[m_nCountMotion - 1]);
                _V_ams[m_nCountMotion - 1] += (_V[m_nCountMotion - 1][n] * _V[m_nCountMotion - 1][n] * delta_t[m_nCountMotion - 1]);
                _D_ams[m_nCountMotion - 1] += (_D[m_nCountMotion - 1][n] * _D[m_nCountMotion - 1][n] * delta_t[m_nCountMotion - 1]);
                _CAV[m_nCountMotion - 1] += Math.Abs(_A[m_nCountMotion - 1][n] * g * delta_t[m_nCountMotion - 1]);
            }
            _SED[m_nCountMotion - 1] = _V_ams[m_nCountMotion - 1];
            _A_ams[m_nCountMotion - 1] = Math.Sqrt(_A_ams[m_nCountMotion - 1] / _T[m_nCountMotion - 1][m_nCnt[m_nCountMotion - 1] - 1]);
            _V_ams[m_nCountMotion - 1] = Math.Sqrt(_V_ams[m_nCountMotion - 1] / _T[m_nCountMotion - 1][m_nCnt[m_nCountMotion - 1] - 1]);
            _D_ams[m_nCountMotion - 1] = Math.Sqrt(_D_ams[m_nCountMotion - 1] / _T[m_nCountMotion - 1][m_nCnt[m_nCountMotion - 1] - 1]);


            _AI[m_nCountMotion - 1][0] = 0;
            double _g = 9.807;
            for (int n = 1; n < m_nCnt[m_nCountMotion - 1]; n++)
            {
                _AI[m_nCountMotion - 1][n] = _AI[m_nCountMotion - 1][n - 1] + (Math.PI / (2 * _g)) * (_A[m_nCountMotion - 1][n] * _g) * (_A[m_nCountMotion - 1][n] * _g) * delta_t[m_nCountMotion - 1];
            }
            _AI_MP[m_nCountMotion - 1] = _AI[m_nCountMotion - 1][m_nCnt[m_nCountMotion - 1] - 1];


            //Bracked Duration..
            int first;
            double brack = 5;
            Double.TryParse(textMP_RatioBracketDuration.Text.ToString(), NumberStyles.Any,
                    CultureInfo.CurrentCulture.NumberFormat, out brack);

            _Bracked_value[m_nCountMotion - 1] = _PGA[m_nCountMotion - 1] * brack / 100;

            //labelMP_BracketDuration.Text = _Bracked_value[m_nCountMotion - 1].ToString();
            for (first = 0; first < m_nCnt[m_nCountMotion - 1]; first++)
            {
                if (Math.Abs(_A[m_nCountMotion - 1][first]) > _Bracked_value[m_nCountMotion - 1])
                    break;
            }

            int second;

            for (second = m_nCnt[m_nCountMotion - 1] - 1; second > -1; second--)
            {
                if (Math.Abs(_A[m_nCountMotion - 1][second]) > _Bracked_value[m_nCountMotion - 1])
                    break;
            }

            if (second == -1 || first == m_nCnt[m_nCountMotion - 1])
            {
                //MessageBox.Show("Warning! \nData is not right!");
                return false;
            }

            _brack_duration[m_nCountMotion - 1] = _T[m_nCountMotion - 1][second] - _T[m_nCountMotion - 1][first];
            //////

            _D5AI[m_nCountMotion - 1] = -1;
            _D95AI[m_nCountMotion - 1] = 10000;

            m_dMaxAI[m_nCountMotion - 1] = -1;
            m_dMinAI[m_nCountMotion - 1] = 1000;
            for (int n = 0; n < m_nCnt[m_nCountMotion - 1]; n++)
            {
                _AI[m_nCountMotion - 1][n] = _AI[m_nCountMotion - 1][n] / _AI[m_nCountMotion - 1][m_nCnt[m_nCountMotion - 1] - 1] * 100;
                if (_AI[m_nCountMotion - 1][n] < 5 && _D5AI[m_nCountMotion - 1] < _T[m_nCountMotion - 1][n])
                {
                    _D5AI[m_nCountMotion - 1] = _T[m_nCountMotion - 1][n];
                    _D5AI_pos[m_nCountMotion - 1] = n;
                }

                if (_AI[m_nCountMotion - 1][n] > 95 && _D95AI[m_nCountMotion - 1] > _T[m_nCountMotion - 1][n])
                {
                    _D95AI[m_nCountMotion - 1] = _T[m_nCountMotion - 1][n];
                    _D95AI_pos[m_nCountMotion - 1] = n;
                }


                if (m_dMaxAI[m_nCountMotion - 1] < _AI[m_nCountMotion - 1][n])
                {
                    m_dMaxAI[m_nCountMotion - 1] = _AI[m_nCountMotion - 1][n];
                }

                if (m_dMinAI[m_nCountMotion - 1] > _AI[m_nCountMotion - 1][n])
                {
                    m_dMinAI[m_nCountMotion - 1] = _AI[m_nCountMotion - 1][n];
                }
            }

            _sus_duration[m_nCountMotion - 1] = _D95AI[m_nCountMotion - 1] - _D5AI[m_nCountMotion - 1];
            int nlast = m_nCnt[m_nCountMotion - 1] - 1;
            _Ic[m_nCountMotion - 1] = Math.Sqrt(Math.Pow(_A_ams[m_nCountMotion - 1], 3) * _T[m_nCountMotion - 1][nlast]);
            //////////////////////////////////////////////////////////////////////////////////////////
            //FFT


            int bits = (int)Math.Ceiling(Math.Log(m_nCnt[m_nCountMotion - 1], 2));
            m_nLenFourier = (int)Math.Pow(2, bits);

            Complex[] com_SA = new Complex[m_nLenFourier];


            FFT_A[m_nCountMotion - 1] = new double[m_nLenFourier];
            FFT_F[m_nCountMotion - 1] = new double[m_nLenFourier];

            m_dMaxFFTSA[m_nCountMotion - 1] = -1;
            m_dMinFFTSA[m_nCountMotion - 1] = 1000;

            m_dMaxPowerFFTSA[m_nCountMotion - 1] = -1;
            m_dMinPowerFFTSA[m_nCountMotion - 1] = 1000;


            delta_f[m_nCountMotion - 1] = (1 / (m_nLenFourier * delta_t[m_nCountMotion - 1]));
            PowerFFT_A[m_nCountMotion - 1] = new double[m_nLenFourier];

            Complex[] imput = new Complex[m_nLenFourier];
            for (int n = 0; n < m_nLenFourier; n++)
            {
                com_SA[n] = Complex.Zero;
                imput[n] = Complex.Zero;

                if (n < m_nCnt[m_nCountMotion - 1])
                {
                    com_SA[n] = new Complex(_A[m_nCountMotion - 1][n], 0);
                    imput[n] = new Complex(_A[m_nCountMotion - 1][n], 0);
                    FFT_A[m_nCountMotion - 1][n] = _A[m_nCountMotion - 1][n];
                }
                else
                {
                    FFT_A[m_nCountMotion - 1][n] = 0;
                }
            }

            //FourierTransform.FFT(com_SA, FourierTransform.Direction.Forward);
            bool direct = true;
            //LomontFFT fft = new LomontFFT();
            //fft.FFT(FFT_A[m_nCountMotion - 1], true);

            //             for (int n = 0; n < m_nLenFourier; n ++)
            //             {
            //                 for ( int j = 0; j < m_nLenFourier; j ++)
            //                 {
            //                     double dis = Math.Cos(2.0 * Math.PI * n * j / m_nLenFourier) * Math.Cos(2.0 * Math.PI * n * j / m_nLenFourier) + Math.Sin(2.0 * Math.PI * n * j / m_nLenFourier) * Math.Sin(2.0 * Math.PI * n * j / m_nLenFourier);
            //                     com_SA[n].Re += (imput[j].Re * Math.Cos(2.0 * Math.PI * n * j / m_nLenFourier) + imput[j].Im * Math.Sin(2.0 * Math.PI * n * j / m_nLenFourier)) / dis;
            //                     com_SA[n].Im +=  (imput[j].Im * Math.Cos(2.0 * Math.PI * n * j / m_nLenFourier) - imput[j].Re * Math.Sin(2.0 * Math.PI * n * j / m_nLenFourier)) / dis;
            //                 }
            //             }

            FFT_Transform1.FFT(com_SA);
            for (int n = 0; n < m_nLenFourier; n++)
            {
                if (n > m_nLenFourier / 2)
                    FFT_A[m_nCountMotion - 1][n] = 0;
                else
                {
                    //FFT_A[m_nCountMotion - 1][n] = Math.Abs(FFT_A[m_nCountMotion - 1][n]);
                    FFT_A[m_nCountMotion - 1][n] = Math.Sqrt(com_SA[n].Re * com_SA[n].Re + com_SA[n].Im * com_SA[n].Im) / 100;
                }
                if (n != 0)
                {
                    FFT_F[m_nCountMotion - 1][n] = delta_f[m_nCountMotion - 1] * n;
                }
                else
                {
                    FFT_F[m_nCountMotion - 1][n] = 0;
                }

                PowerFFT_A[m_nCountMotion - 1][n] = (FFT_A[m_nCountMotion - 1][n] * FFT_A[m_nCountMotion - 1][n]) / (Math.PI * m_dMaxT[m_nCountMotion - 1] * _A_ams[m_nCountMotion - 1] * _A_ams[m_nCountMotion - 1]);
                //////////////////////////////////////////////////////////////////////////////////
                if (m_dMaxFFTSA[m_nCountMotion - 1] < FFT_A[m_nCountMotion - 1][n])
                {
                    m_dMaxFFTSA[m_nCountMotion - 1] = FFT_A[m_nCountMotion - 1][n];
                }

                if (m_dMinPowerFFTSA[m_nCountMotion - 1] > FFT_A[m_nCountMotion - 1][n])
                {
                    m_dMinPowerFFTSA[m_nCountMotion - 1] = FFT_A[m_nCountMotion - 1][n];
                }
                //////////////////////////////////////////////////////////////////////////////
                if (m_dMaxPowerFFTSA[m_nCountMotion - 1] < PowerFFT_A[m_nCountMotion - 1][n])
                {
                    m_dMaxPowerFFTSA[m_nCountMotion - 1] = PowerFFT_A[m_nCountMotion - 1][n];
                }

                if (m_dMinFFTSA[m_nCountMotion - 1] > PowerFFT_A[m_nCountMotion - 1][n])
                {
                    m_dMinFFTSA[m_nCountMotion - 1] = PowerFFT_A[m_nCountMotion - 1][n];
                }
                ////////////////////////////////////////////////////////////////////////////
                if (m_dMaxFFTSF[m_nCountMotion - 1] < FFT_F[m_nCountMotion - 1][n])
                {
                    m_dMaxFFTSF[m_nCountMotion - 1] = FFT_F[m_nCountMotion - 1][n];
                }

                if (m_dMinFFTSF[m_nCountMotion - 1] > FFT_F[m_nCountMotion - 1][n])
                {
                    m_dMinFFTSF[m_nCountMotion - 1] = FFT_F[m_nCountMotion - 1][n];
                }
            }
            return true;
        }

        private void GetSpectralAcc_Vel_Disp(int motionIndex)
        {
            //m_nSelMethodList = comboDL_MethodList.SelectedIndex;

            if (m_nSelMethodList == -1)
            {
                return;
            }
            if (m_nSelMethodList == 1)
            {
                double m = 0.2533;
                Double.TryParse(textDI_DampingRate.Text.ToString(), NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out m_dDefaultDampingRatio);
                double dao = m_dDefaultDampingRatio;
                dao = dao / 100;

                double Beta = 0.25;
                double Gama = 0.5;
                double g = 980.76;
                double periodTime;
                Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
                   CultureInfo.CurrentCulture.NumberFormat, out periodTime);

                //periodTime = Convert.ToDouble(textDI_MaxPeriod.Text.ToString());

                double delt = delta_t[motionIndex];
                //double deltaT = delta_t[motionIndex];

                double stepT = 0.02;
                nSLen[motionIndex] = (int)(periodTime / stepT);
                _S_A[motionIndex] = new double[nSLen[motionIndex]];
                _S_V[motionIndex] = new double[nSLen[motionIndex]];
                _S_D[motionIndex] = new double[nSLen[motionIndex]];
                m_dMaxSA[motionIndex] = -1;
                m_dMinSA[motionIndex] = 1000;
                m_dMaxSV[motionIndex] = -1;
                m_dMinSV[motionIndex] = 1000;

                m_dMaxSD[motionIndex] = -1;
                m_dMinSD[motionIndex] = 1000;
                for (int n = 0; n < nSLen[motionIndex]; n++)
                {
                    double T = (n + 1) * stepT;
                    double Wn = (2 * Math.PI / T);
                    double k = (Wn * Wn * m);
                    double c = (dao * 2 * Math.Sqrt(m * k));
                    double a1 = m / (Beta * delt * delt) + Gama / (Beta * delt) * c;
                    double a2 = m / (Beta * delt) + (Gama / Beta - 1) * c;
                    double a3 = (1 / (2 * Beta) - 1) * m + delt * (Gama / (2 * Beta) - 1) * c;
                    double _k = k + a1;
                    double u = 0, u1 = 0, u2 = 0;
                    double _p = 0;

                    double maxu = 0;
                    for (int i = 0; i < m_nCnt[motionIndex]; i++)
                    {
                        double oldu = u, oldu1 = u1, oldu2 = u2;
                        _p = _A[motionIndex][i] * g * m + a1 * oldu + a2 * oldu1 + a3 * oldu2;
                        u = _p / _k;
                        u1 = Gama / (Beta * delt) * (u - oldu) + (1 - Gama / Beta) * oldu1 + delt * (1 - Gama / (2 * Beta)) * oldu2;
                        u2 = 1 / (Beta * delt * delt) * (u - oldu) - 1 / (Beta * delt) * oldu1 - (1 / (2 * Beta) - 1) * oldu2;

                        if (Math.Abs(u) > maxu)
                            maxu = Math.Abs(u);
                    }

                    _S_A[motionIndex][n] = maxu * Wn * Wn / g;
                    _S_V[motionIndex][n] = maxu * Wn;
                    _S_D[motionIndex][n] = maxu / g;

                    if (m_dMaxSA[motionIndex] < _S_A[motionIndex][n])
                        m_dMaxSA[motionIndex] = _S_A[motionIndex][n];

                    if (m_dMinSA[motionIndex] > _S_A[motionIndex][n])
                        m_dMinSA[motionIndex] = _S_A[motionIndex][n];

                    if (m_dMaxSV[motionIndex] < _S_V[motionIndex][n])
                        m_dMaxSV[motionIndex] = _S_V[motionIndex][n];

                    if (m_dMinSV[motionIndex] > _S_V[motionIndex][n])
                        m_dMinSV[motionIndex] = _S_V[motionIndex][n];

                    if (m_dMaxSD[motionIndex] < _S_D[motionIndex][n])
                        m_dMaxSD[motionIndex] = _S_D[motionIndex][n];

                    if (m_dMinSD[motionIndex] > _S_D[motionIndex][n])
                        m_dMinSD[motionIndex] = _S_D[motionIndex][n];
                }
            }
            else
            {
                double m = 0.2533;
                Double.TryParse(textDI_DampingRate.Text.ToString(), NumberStyles.Any,
                        CultureInfo.CurrentCulture.NumberFormat, out m_dDefaultDampingRatio);
                double dao = m_dDefaultDampingRatio;
                dao = dao / 100;

                double Beta = 0.25;
                double Gama = 0.5;
                double g = 980.76;
                double periodTime;
                Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
                   CultureInfo.CurrentCulture.NumberFormat, out periodTime);

                double delt = delta_t[motionIndex];

                double stepT = 0.02;
                nSLen[motionIndex] = (int)(periodTime / stepT);
                _S_A[motionIndex] = new double[nSLen[motionIndex]];
                _S_V[motionIndex] = new double[nSLen[motionIndex]];
                _S_D[motionIndex] = new double[nSLen[motionIndex]];
                m_dMaxSA[motionIndex] = -1;
                m_dMinSA[motionIndex] = 1000;
                m_dMaxSV[motionIndex] = -1;
                m_dMinSV[motionIndex] = 1000;

                m_dMaxSD[motionIndex] = -1;
                m_dMinSD[motionIndex] = 1000;

                for (int n = 0; n < nSLen[motionIndex]; n++)
                {
                    double T = (n + 1) * stepT;
                    double Wn = (2 * Math.PI / T);
                    double Wd = Wn * Math.Sqrt(1 - dao * dao);
                    double k = (Wn * Wn * m);
                    double c = (dao * 2 * Math.Sqrt(m * k));
                    double u = 0, u1 = 0;
                    double _p = 0;

                    double maxu = 0;

                    double qusiVal = Math.Pow(Math.E, (-1 * dao * Wn * delt));
                    double aa = qusiVal * (dao / Math.Sqrt(1 - dao * dao) * Math.Sin(Wd * delt) + Math.Cos(Wd * delt));
                    double bb = qusiVal * (1 / Wd * Math.Sin(Wd * delt));
                    double cc = (1 / k) * ((2 * dao) / (Wn * delt) + qusiVal * (((1 - 2 * dao * dao) / (Wd * delt) - (dao / Math.Sqrt(1 - dao * dao))) * Math.Sin(Wd * delt) - (1 + (2 * dao) / (Wn * delt)) * Math.Cos(Wd * delt)));
                    double dd = (1 / k) * (1 - (2 * dao) / (Wn * delt) + qusiVal * (((2 * dao * dao - 1) / (Wd * delt)) * Math.Sin(Wd * delt) + ((2 * dao) / (Wn * delt)) * Math.Cos(Wd * delt)));

                    double aa1 = (-1) * qusiVal * (Wn / (Math.Sqrt(1 - dao * dao)) * Math.Sin(Wd * delt));
                    double bb1 = qusiVal * (Math.Cos(Wd * delt) - (dao / (Math.Sqrt(1 - dao * dao)) * Math.Sin(Wd * delt)));
                    double cc1 = (1 / k) * (-1 / delt + qusiVal * ((Wn / Math.Sqrt(1 - dao * dao) + dao / (delt * Math.Sqrt(1 - dao * dao))) * Math.Sin(Wd * delt) + 1 / delt * Math.Cos(Wd * delt)));
                    double dd1 = (1 / k / delt) * (1 - qusiVal * (dao / (Math.Sqrt(1 - dao * dao)) * Math.Sin(Wd * delt) + Math.Cos(Wd * delt)));
                    for (int i = 0; i < m_nCnt[motionIndex]; i++)
                    {
                        double oldu = u, oldu1 = u1;
                        if (i != m_nCnt[motionIndex] - 1)
                        {
                            u = aa * oldu + bb * oldu1 + cc * (-1 * m * _A[motionIndex][i] * g) + dd * (-1 * m * _A[motionIndex][i + 1] * g);
                            u1 = aa1 * oldu + bb1 * oldu1 + cc1 * (-1 * m * _A[motionIndex][i] * g) + dd1 * (-1 * m * _A[motionIndex][i + 1] * g);
                        }
                        else
                        {
                            u = aa * oldu + bb * oldu1 + cc * (-1 * m * _A[motionIndex][i] * g);
                            u1 = aa1 * oldu + bb1 * oldu1 + cc1 * (-1 * m * _A[motionIndex][i] * g);
                        }

                        if (Math.Abs(u) > maxu)
                            maxu = Math.Abs(u);
                    }

                    _S_A[motionIndex][n] = maxu * Wn * Wn / g;
                    _S_V[motionIndex][n] = maxu * Wn;
                    _S_D[motionIndex][n] = maxu / g;

                    if (m_dMaxSA[motionIndex] < _S_A[motionIndex][n])
                        m_dMaxSA[motionIndex] = _S_A[motionIndex][n];

                    if (m_dMinSA[motionIndex] > _S_A[motionIndex][n])
                        m_dMinSA[motionIndex] = _S_A[motionIndex][n];

                    if (m_dMaxSV[motionIndex] < _S_V[motionIndex][n])
                        m_dMaxSV[motionIndex] = _S_V[motionIndex][n];

                    if (m_dMinSV[motionIndex] > _S_V[motionIndex][n])
                        m_dMinSV[motionIndex] = _S_V[motionIndex][n];

                    if (m_dMaxSD[motionIndex] < _S_D[motionIndex][n])
                        m_dMaxSD[motionIndex] = _S_D[motionIndex][n];

                    if (m_dMinSD[motionIndex] > _S_D[motionIndex][n])
                        m_dMinSD[motionIndex] = _S_D[motionIndex][n];
                }
            }

            _meanS_A = new double[nSLen[motionIndex]];

            for (int n = 0; n < nSLen[motionIndex]; n++)
            {
                _meanS_A[n] = 0;
                int div = 0;
                for (int i = 0; i < m_nCountMotion; i++)
                {
                    if (n >= _S_A[i].Length)
                        continue;

                    div++;
                    _meanS_A[n] += _S_A[i][n];
                }

                _meanS_A[n] /= div;
            }

            
        }

        private void Normalize(double[] srcBuffer, ref double[] desBuffer, int len, ref double max, ref double min, ref int scale)
        {
            double dis = MaxAbsValue(max, min);
            //dis = 105;

            scale = (int)Math.Ceiling(Math.Log10(dis));

            dis = dis / Math.Pow(10, scale);
            for (int i = 0; i < len; i++)
            {
                desBuffer[i] = srcBuffer[i] / Math.Pow(10, scale);
            }

            dis = Math.Ceiling(dis * 100) / 100.0;
            max = dis;

            if (min < 0)
                min = -1 * dis;
            else
                min = min / Math.Pow(10, scale);
        }

        private void checkedListDI_Accelogram_SelectedIndexChanged(object sender, EventArgs e)
        {
            //checkedListDI_Accelogram.Refresh();
            //this.Refresh();
        }

        private void comboDI_SpectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nSelSpectList = comboDI_SpectList.SelectedIndex;

            PostControlMsg((Int32)ControlMessage.msgDI_ListS_AVD);
            //if (m_nCountMotion > 0)
            //    picDI_Spectr.Invalidate();

            picDI_Spectr.Refresh();
            //this.Refresh();
        }

        private void comboDL_MotionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nSelMethodList = comboDL_MethodList.SelectedIndex;

            for (int i = 0; i < m_nCountMotion; i++)
                GetSpectralAcc_Vel_Disp(i);

            //if (m_nCountMotion > 0)
            //    picDI_Spectr.Invalidate();

            picDI_Spectr.Refresh();
        }

        /////////////////////////////  Spectral Matching  //////////////////////////////////////////////////////

        private bool bTargetGenerated1 = false;
        private bool bTargetGenerated2 = false;

        private void GenerateTarget1()
        {

            double SD1;
            double SDS;
            double TL;
            double TN;

            Double.TryParse(textSM_SD1.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out SD1);
            Double.TryParse(textSM_SDS.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out SDS);
            Double.TryParse(textSM_TL.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out TL);
            Double.TryParse(textSM_Tn.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out TN);

            //SD1 = Convert.ToDouble(textSM_SD1.Text.ToString());
            //SDS = Convert.ToDouble(textSM_SDS.Text.ToString());
            //TL = Convert.ToDouble(textSM_TL.Text.ToString());
            //TN = Convert.ToDouble(textSM_Tn.Text.ToString());

            _TA = SD1 / SDS * 0.2;
            _TB = SD1 / SDS;

            double PeriodTime;
            Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out PeriodTime);
            //PeriodTime = Convert.ToDouble(textDI_MaxPeriod.Text.ToString());

            if (m_nCountMotion == 0)
                return;
            //nSLen[m_nCountMotion - 1] = (int)(PeriodTime / delta_t[m_nCountMotion - 1]);

            nDesign = 0;

            _Sae0 = new double[nSLen[m_nCountMotion - 1]];
            for (int i = 0; i < nSLen[m_nCountMotion - 1]; i++)
            {
                double t = i * 0.02;
                if (t >= 0 && t < _TA)
                    _Sae0[i] = (0.4 + 0.6 * t / _TA) * SDS;
                else if (t >= _TA && t < _TB)
                    _Sae0[i] = SDS;
                else if (t >= _TB && t < TL)
                    _Sae0[i] = SD1 / t;
                else
                    _Sae0[i] = SD1 * TL / t / t;
            }


            _Sae_T = new double[nSLen[m_nCountMotion - 1]];

            m_dMaxSE = -1;
            m_dMinSE = 1000;
            for (int i = 0; i < nSLen[m_nCountMotion - 1]; i++)
            {
                _Sae_T[i] = i * 0.02;
                if (m_dMaxSE < _Sae0[i])
                    m_dMaxSE = _Sae0[i];

                if (m_dMinSE > _Sae0[i])
                    m_dMinSE = _Sae0[i];
            }

            for (int j = 0; j < m_nCountMotion; j++)
            {
                m_dMaxSA_Alfa0[j] = -1;
                m_dMinSA_Alfa0[j] = 1000;
                _S_A_Alfa0[j] = new double[nSLen[m_nCountMotion - 1]];
                for (int i = 0; i < nSLen[m_nCountMotion - 1]; i++)
                {
                    _S_A_Alfa0[j][i] = _Sae0[i];

                    if (m_dMaxSA_Alfa0[j] < _S_A_Alfa0[j][i])
                        m_dMaxSA_Alfa0[j] = _S_A_Alfa0[j][i];

                    if (m_dMinSA_Alfa0[j] > _S_A_Alfa0[j][i])
                        m_dMinSA_Alfa0[j] = _S_A_Alfa0[j][i];
                }
            }

            PostControlMsg((Int32)ControlMessage.msgSM_ListTarget);
            //picSM_TargetSpectrum.Invalidate();
            //pictSM_SM_Spectral.Invalidate();
            bTargetGenerated1 = true;
            picSM_TargetSpectrum.Refresh();
            picSM_SM_Spectral.Refresh();

        }

        private void btnSM_DefineTargetSectra_Click(object sender, EventArgs e)
        {
            GenerateTarget1();
        }        

        private void GenerateTarget2()
        {
            double SD1;
            double SDS;
            double TL;
            double TN;

            Double.TryParse(textDS_SD1.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out SD1);
            Double.TryParse(textDS_SDS.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out SDS);
            Double.TryParse(textDS_TL.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out TL);
            Double.TryParse(textDS_Tn.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out TN);

            //SD1 = Convert.ToDouble(textDS_SD1.Text.ToString());
            //SDS = Convert.ToDouble(textDS_SDS.Text.ToString());
            //TL = Convert.ToDouble(textDS_TL.Text.ToString());
            //TN = Convert.ToDouble(textDS_Tn.Text.ToString());



            _TA = (double)((decimal)SD1 / (decimal)SDS * (decimal)0.2);
            _TB = (double)((decimal)SD1 / (decimal)SDS);

            textDS_TA.Text = FixDecimal2(_TA).ToString();
            textDS_TB.Text = FixDecimal2(_TB).ToString();

            double PeriodTime;
            Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out PeriodTime);
            //PeriodTime = Convert.ToDouble(textDI_MaxPeriod.Text.ToString());
            if (m_nCountMotion == 0)
                return;
            nDesign = 1;
            _Sae0 = new double[nSLen[m_nCountMotion - 1]];
            for (int i = 0; i < nSLen[m_nCountMotion - 1]; i++)
            {
                double t = i * 0.02;
                if (t >= 0 && t < _TA)
                    _Sae0[i] = (0.4 + 0.6 * t / _TA) * SDS;
                else if (t >= _TA && t < _TB)
                    _Sae0[i] = SDS;
                else if (t >= _TB && t < TL)
                    _Sae0[i] = SD1 / t;
                else
                    _Sae0[i] = SD1 * TL / t / t;
            }

            _Sae_T = new double[nSLen[m_nCountMotion - 1]];

            m_dMaxSE = -1;
            m_dMinSE = 1000;
            for (int i = 0; i < nSLen[m_nCountMotion - 1]; i++)
            {
                _Sae_T[i] = 0.02 * i;

                if (m_dMaxSE < _Sae0[i])
                    m_dMaxSE = _Sae0[i];

                if (m_dMinSE > _Sae0[i])
                    m_dMinSE = _Sae0[i];
            }

            for (int j = 0; j < m_nCountMotion; j++)
            {
                m_dMaxSA_Alfa0[j] = -1;
                m_dMinSA_Alfa0[j] = 1000;
                _S_A_Alfa0[j] = new double[nSLen[m_nCountMotion - 1]];
                for (int i = 0; i < nSLen[m_nCountMotion - 1]; i++)
                {
                    _S_A_Alfa0[j][i] = _Sae0[i] * m_dMaxSA[j];

                    if (m_dMaxSA_Alfa0[j] < _S_A_Alfa0[j][i])
                        m_dMaxSA_Alfa0[j] = _S_A_Alfa0[j][i];

                    if (m_dMinSA_Alfa0[j] > _S_A_Alfa0[j][i])
                        m_dMinSA_Alfa0[j] = _S_A_Alfa0[j][i];
                }
            }

            //picDS_TargetSpectrum.Invalidate();
            //picDS_Last.Invalidate();
            bTargetGenerated2 = true;
            picDS_Last.Refresh();
            picDS_TargetSpectrum.Refresh();

        }

        private void btnDS_ManualDesign_Click(object sender, EventArgs e)
        {
            GenerateTarget2();
        }

        private void btnDS_Design_Click(object sender, EventArgs e)
        {
            double SDS = 0;

            double step = 0.02;
            double TN = 0;
            Double.TryParse(textDS_Tn.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out TN);
            

            int indexSS = (int)Math.Round(0.2 / step);
            int indexEE = (int)Math.Round(1 / step);
            for (int i = indexSS + 1; i <= indexEE; i++)
            {
                if (SDS < _meanS_A[i])
                    SDS = _meanS_A[i];
            }

            SDS = (double)((decimal)SDS * (decimal)m_dSDSCoefficient);

            double SD1 = 0;

            int indexS = 0;
            int indexE = 0;
            if(checkUseTnForSD1.Checked)
            {
                indexS = (int)Math.Round(1 / step);
                indexE = (int)Math.Round(1.5 * TN / step);

                if(1 > 1.5 * TN)
                {
                    SD1 = _meanS_A[indexS];
                }
                else
                {
                    for (int t = indexS; t <= indexE; t++)
                    {
                        if (SD1 < _meanS_A[t])
                            SD1 = _meanS_A[t];
                    }
                }

            }
            else
            {

                double periodTime;
                Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
                   CultureInfo.CurrentCulture.NumberFormat, out periodTime);

                indexS = (int)Math.Round(1 / step);
                indexE = (int)Math.Round(periodTime / step);

                if (indexS > indexE)
                {
                    int tem = indexS;
                    indexS = indexE;
                    indexE = tem;
                }

                for (int t = indexS; t < indexE; t++)
                {
                    if (SD1 < _meanS_A[t])
                        SD1 = _meanS_A[t];
                }

            }

            textDS_SDS.Text = FixDecimal2(SDS).ToString();
            textDS_SD1.Text = FixDecimal2(SD1).ToString();

            GenerateTarget2();
        }

        private void DisThread_Completed(object sender, EventArgs e)
        {

        }

        private void DisThread(object sender, EventArgs e)
        {
            //if (nSMDMotionDataIndex != comboSMD_MotionDatas.SelectedIndex)
            {
                PostControlMsg((Int32)ControlMessage.msgSMD_ListTA);
                PostControlMsg((Int32)ControlMessage.msgSMD_ListTV);
                PostControlMsg((Int32)ControlMessage.msgSMD_ListTD);
            }


            //picSMD_Acceleration.Refresh();
            //picSMD_Velocity.Refresh();
            //picSMD_Displacement.Refresh();
            //picSMD_AriasIntensity.Refresh();
        }

        private void comboSMD_MotionDatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            int nIndex = comboSMD_MotionDatas.SelectedIndex;
            if (nIndex < 0)
                return;            

            m_nSMDComboMotionIndex = nIndex;
            textSMD_AccInterval.Text = _SMD_AccInterval[nIndex];
            textSMD_AccTimeInterval.Text = _SMD_AccTimeInterval[nIndex];

            textSMD_VccInterval.Text = _SMD_VccInterval[nIndex];
            textSMD_VccTimeInterval.Text = _SMD_VccTimeInterval[nIndex];

            textSMD_DisInterval.Text = _SMD_DisInterval[nIndex];
            textSMD_DisTimeInterval.Text = _SMD_DisTimeInterval[nIndex];

            textSMD_AIInterval.Text = _SMD_AIInterval[nIndex];
            textSMD_AITimeInterval.Text = _SMD_AITimeInterval[nIndex];

            //disThread.RunWorkerAsync();
            {
                PostControlMsg((Int32)ControlMessage.msgSMD_ListTA);
                PostControlMsg((Int32)ControlMessage.msgSMD_ListTV);
                PostControlMsg((Int32)ControlMessage.msgSMD_ListTD);
            }

            
            //m_nOldSMDComboMotionIndex = nIndex;
            picSMD_Acceleration.Refresh();
            picSMD_Velocity.Refresh();
            picSMD_Displacement.Refresh();
            picSMD_AriasIntensity.Refresh();

            if (m_nCountMotion != 1)
                m_nOldSMDComboMotionIndex = m_nSMDComboMotionIndex;
            else
                m_nOldSMDComboMotionIndex = 0;
            //             Thread m_threadID = new Thread(SelectChangeSMD);
            //             m_threadID.Start();

        }

        private void comboSM_MDMotionDatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            picSM_MD_Acceleration.Refresh();
            picSM_MD_Spectral.Refresh();
            //this.Refresh();
        }

        private void btnSM_Match_Click(object sender, EventArgs e)
        {
            if (m_nCountMotion == 0 || !bTargetGenerated1)
                return;

            double TN;

            Double.TryParse(textSM_Tn.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out TN);

            int st = (int)(0.2 * TN / 0.02);
            int ed = (int)(1.5 * TN / 0.02);
            double alfa1 = 0;
            double alfa2 = 0;
            for (int n = 0; n < m_nCountMotion; n++)
            {
                alfa1 = 0;
                alfa2 = 0;
                for (int i = st; i <= ed; i++)
                {
                    alfa1 += (_Sae0[i] * _S_A[n][i]);
                    alfa2 += _S_A[n][i] * _S_A[n][i];
                }
                _Alfa[n] = alfa1 / alfa2;
                _Error[n] = 0;
                for (int i = st; i <= ed; i++)
                {
                    _Error[n] += Math.Abs(_Alfa[n] * _S_A[n][i] - _Sae0[i]) / _Sae0[i];
                }
            }

            PostControlMsg((Int32)ControlMessage.msgSM_SM_Alfa);
            PostControlMsg((Int32)ControlMessage.msgSM_MD_Alfa);



            int pos = comboSM_MDMotionDatas.SelectedIndex;

            if (_Alfa[pos] == 0)
            {
                //MessageBox.Show("There is no Alfa Value for this data!\nClick \"Define Target Spectra\" Button and retry !");
                return;
            }
            //double alfa2;
            Double.TryParse(textSM_MS_Alfa.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out alfa2);
            //alfa2 = Convert.ToDouble(textSM_MS_Alfa.Text.ToString());

            _S_A_Alfa1[pos] = new double[nSLen[pos]];

            m_dMaxSA_Alfa1[pos] = -1;
            m_dMinSA_Alfa1[pos] = 1000;
            for (int ii = 0; ii < nSLen[pos]; ii++)
            {
                _S_A_Alfa1[pos][ii] = _S_A[pos][ii] * _Alfa[pos];

                if (_S_A_Alfa1[pos][ii] > m_dMaxSA_Alfa1[pos])
                    m_dMaxSA_Alfa1[pos] = _S_A_Alfa1[pos][ii];

                if (_S_A_Alfa1[pos][ii] < m_dMinSA_Alfa1[pos])
                    m_dMinSA_Alfa1[pos] = _S_A_Alfa1[pos][ii];
            }
            picSM_MD_Spectral.Refresh();

        }

        private void btnSM_MS_ScaleMotion_Click(object sender, EventArgs e)
        {
            if (m_nCountMotion == 0)
                return;

            if (_Sae0 == null)
            {
                //MessageBox.Show("No Define Target Spectral!");
                return;
            }
            int pos = comboSM_MSMotionDatas.SelectedIndex;
            double alfa2;
            Double.TryParse(textSM_MS_Alfa.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out alfa2);
            //alfa2 = Convert.ToDouble(textSM_MS_Alfa.Text.ToString());

            _S_A_Alfa2[pos] = new double[nSLen[pos]];

            m_dMaxSA_Alfa2[pos] = -1;
            m_dMinSA_Alfa2[pos] = 1000;
            for (int ii = 0; ii < nSLen[pos]; ii++)
            {
                _S_A_Alfa2[pos][ii] = _S_A[pos][ii] * alfa2;

                if (_S_A_Alfa2[pos][ii] > m_dMaxSA_Alfa2[pos])
                    m_dMaxSA_Alfa2[pos] = _S_A_Alfa2[pos][ii];

                if (_S_A_Alfa2[pos][ii] < m_dMinSA_Alfa2[pos])
                    m_dMinSA_Alfa2[pos] = _S_A_Alfa2[pos][ii];
            }

            double TempError = 0;

            double TN;
            Double.TryParse(textSM_Tn.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out TN);
            //TN = Convert.ToDouble(textSM_Tn.Text.ToString());
            int st = (int)(0.2 * TN / 0.02);
            int ed = (int)(1.5 * TN / 0.02);
            for (int i = st; i <= ed; i++)
            {
                TempError += Math.Abs(alfa2 * _S_A[pos][i] - _Sae0[i]) / _Sae0[i];
            }
            TempError = Math.Ceiling(TempError * 100) / 100;
            textSM_MS_Error.Text = TempError.ToString();

            picSM_MS_Spectral.Refresh();
            //this.Refresh();
        }

        private void comboMP_MotionDatas_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if(isLoaded)
            {
                PostControlMsg((Int32)ControlMessage.msgMP_ListIntensityParameters);
                PostControlMsg((Int32)ControlMessage.msgMP_ListDurationParameters);
                PostControlMsg((Int32)ControlMessage.msgMP_ListValues);
                this.picMP_FrequencyGraph.Refresh();
                this.picMP_AriasIntensity.Refresh();
                this.picMP_Acceleration.Refresh();
                this.listViewMP_DurationParamer.Refresh();
                this.listViewMP_IntensityParameter.Refresh();
                this.listViewMP_Values.Refresh();
            }

            //this.Refresh();
        }

        private void comboMP_F_P_S_SelectedIndexChanged(object sender, EventArgs e)
        {
            picMP_FrequencyGraph.Invalidate();
            PostControlMsg((Int32)ControlMessage.msgMP_ListValues);
            this.picMP_FrequencyGraph.Refresh();
            this.listViewMP_Values.Refresh();
            //this.Refresh();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void btnSM_MDSaveScaledMotion_Click(object sender, EventArgs e)
        {
            if (m_nCountMotion == 0)
                return;
            if (_Sae0 == null)
            {
                //MessageBox.Show("No Define Target Spectral!");
                return;
            }
            int index = comboSM_MDMotionDatas.SelectedIndex;
            if (_S_A_Alfa1[index] != null)
            {
                string filename = m_strListName[index];
                filename = filename.Replace(".", "_");
                this.saveFileDialog_ModifyData.FileName = filename + ".txt";
                string[] strText = new string[m_nCnt[index]];
                if (this.saveFileDialog_ModifyData.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < m_nCnt[index]; i++)
                    {
                        strText[i] = (Math.Floor(_A[index][i] * _Alfa[index] * 1000000000) / 1000000000).ToString();
                    }
                    System.IO.File.WriteAllLines(this.saveFileDialog_ModifyData.FileName, strText);
                    MessageBox.Show("Saved successfully!");
                }
            }

            //
        }

        private void btnSM_MS_SaveScaledMotion_Click(object sender, EventArgs e)
        {
            int index = comboSM_MSMotionDatas.SelectedIndex;
            double alfa2;
            Double.TryParse(textSM_MS_Alfa.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out alfa2);

            if (_S_A_Alfa2[index] != null)
            {
                string filename = m_strListName[index];
                filename = filename.Replace(".", "_");
                this.saveFileDialog_ModifyData.FileName = filename + ".txt";
                string[] strText = new string[m_nCnt[index]];

                if (this.saveFileDialog_ModifyData.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < m_nCnt[index]; i++)
                    {
                        strText[i] = (Math.Floor(_A[index][i] * alfa2 * 1000000000) / 1000000000).ToString();
                    }
                    System.IO.File.WriteAllLines(this.saveFileDialog_ModifyData.FileName, strText);
                    //MessageBox.Show("Saved successfully!");
                }
            }
        }

        private void listViewSMD_T_A_Scrolled(object sender, ScrollEventArgs e)
        {
            if (isLoaded && listViewSMD_T_A.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_A.GetItemAt(listViewSMD_T_A.ClientRectangle.X + 6, listViewSMD_T_A.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_A.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_A.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_A.TopItem.Index;
                }
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_A_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLoaded && listViewSMD_T_A.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_A.GetItemAt(listViewSMD_T_A.ClientRectangle.X + 6, listViewSMD_T_A.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_A.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_A.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_A.TopItem.Index;
                }
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_A_MouseHover(object sender, EventArgs e)
        {
            if (isLoaded && listViewSMD_T_A.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_A.GetItemAt(listViewSMD_T_A.ClientRectangle.X + 6, listViewSMD_T_A.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_A.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_A.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_A.TopItem.Index;
                }
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_A_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded && listViewSMD_T_A.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_A.GetItemAt(listViewSMD_T_A.ClientRectangle.X + 6, listViewSMD_T_A.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_A.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_A.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_A.TopItem.Index;
                }
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_A_Wheeled()
        {

            if (isLoaded && listViewSMD_T_A.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_A.GetItemAt(listViewSMD_T_A.ClientRectangle.X + 6, listViewSMD_T_A.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_A.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_A.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_A.TopItem.Index;
                }
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);

            }

        }

        private void listViewSMD_T_V_Scrolled(object sender, ScrollEventArgs e)
        {
            if (isLoaded && listViewSMD_T_V.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_V.GetItemAt(listViewSMD_T_V.ClientRectangle.X + 6, listViewSMD_T_V.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_V.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_V.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_V.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_V_MouseHover(object sender, EventArgs e)
        {
//             if (isLoaded && listViewSMD_T_V.Items.Count > 0)
//             {
//                 ListViewItem lvi2 = listViewSMD_T_V.GetItemAt(listViewSMD_T_V.ClientRectangle.X + 6, listViewSMD_T_V.ClientRectangle.Bottom - 10);
//                 int endidx = listViewSMD_T_V.Items.IndexOf(lvi2);
// 
//                 if (endidx == -1) endidx = listViewSMD_T_V.Items.Count;
//                 if (m_nSMDScrollPos < endidx)
//                 {
//                     m_nSMDScrollPos = endidx;
//                 }
//                 else
//                 {
//                     m_nSMDScrollPos = listViewSMD_T_V.TopItem.Index;
//                 }
//                 listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
//                 listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
//             }
        }

        private void listViewSMD_T_V_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLoaded && listViewSMD_T_V.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_V.GetItemAt(listViewSMD_T_V.ClientRectangle.X + 6, listViewSMD_T_V.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_V.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_V.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_V.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_V_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded && listViewSMD_T_V.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_V.GetItemAt(listViewSMD_T_V.ClientRectangle.X + 6, listViewSMD_T_V.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_V.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_V.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_V.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_D.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_D_Scrolled(object sender, ScrollEventArgs e)
        {
            if (isLoaded && listViewSMD_T_D.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_D.GetItemAt(listViewSMD_T_D.ClientRectangle.X + 6, listViewSMD_T_D.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_D.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_D.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_D.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_D_MouseHover(object sender, EventArgs e)
        {
            if (isLoaded && listViewSMD_T_D.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_D.GetItemAt(listViewSMD_T_D.ClientRectangle.X + 6, listViewSMD_T_D.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_D.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_D.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_D.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_D_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLoaded && listViewSMD_T_D.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_D.GetItemAt(listViewSMD_T_D.ClientRectangle.X + 6, listViewSMD_T_D.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_D.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_D.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_D.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void listViewSMD_T_D_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoaded && listViewSMD_T_D.Items.Count > 0)
            {
                ListViewItem lvi2 = listViewSMD_T_D.GetItemAt(listViewSMD_T_D.ClientRectangle.X + 6, listViewSMD_T_D.ClientRectangle.Bottom - 10);
                int endidx = listViewSMD_T_D.Items.IndexOf(lvi2);

                if (endidx == -1) endidx = listViewSMD_T_D.Items.Count;
                if (m_nSMDScrollPos < endidx)
                {
                    m_nSMDScrollPos = endidx;
                }
                else
                {
                    m_nSMDScrollPos = listViewSMD_T_D.TopItem.Index;
                }
                listViewSMD_T_A.EnsureVisible(m_nSMDScrollPos);
                listViewSMD_T_V.EnsureVisible(m_nSMDScrollPos);
            }
        }

        private void DeleteMotionDataList(int[] dellist)
        {
            for (int i = 0; i < dellist.Length; i++)
            {
                for (int j = dellist[i]; j < m_nCountMotion - 1; j++)
                {
                    m_strInputMotionFiles[j] = m_strInputMotionFiles[j + 1];
                    m_strListName[j] = m_strListName[j + 1];
                    _T[j] = _T[j + 1];
                    _A[j] = _A[j + 1];
                    delta_t[j] = delta_t[j + 1];
                    m_nCnt[j] = m_nCnt[j + 1];
                    m_listIndex[j] = m_listIndex[j + 1];
                    m_dMaxT[j] = m_dMaxT[j + 1];
                    m_dMinT[j] = m_dMinT[j + 1];
                    m_dMaxA[j] = m_dMaxA[j + 1];
                    m_dMinA[j] = m_dMinA[j + 1];
                    m_dMaxV[j] = m_dMaxV[j + 1];
                    m_dMinV[j] = m_dMinV[j + 1];
                    m_dMaxSD[j] = m_dMaxSD[j + 1];
                    m_dMinSD[j] = m_dMinSD[j + 1];
                    _V[j] = _V[j + 1];
                    _D[j] = _D[j + 1];
                    _S_A[j] = _S_A[j + 1];
                    _S_V[j] = _S_V[j + 1];
                    _S_D[j] = _S_D[j + 1];
                    nSLen[j] = nSLen[j + 1];
                    _PGA[j] = _PGA[j + 1];
                    _PGD[j] = _PGD[j + 1];
                    _A_ams[j] = _A_ams[j + 1];
                    _V_ams[j] = _V_ams[j + 1];
                    _D_ams[j] = _D_ams[j + 1];
                    _Ic[j] = _Ic[j + 1];
                    _SED[j] = _SED[j + 1];
                    _CAV[j] = _CAV[j + 1];
                    _NoECA[j] = _NoECA[j + 1];
                    _AI_MP[j] = _AI_MP[j + 1];
                    _D5AI[j] = _D5AI[j + 1];
                    _D95AI[j] = _D95AI[j + 1];
                    _D5AI_pos[j] = _D5AI_pos[j + 1];
                    _D95AI_pos[j] = _D95AI_pos[j + 1];
                    _sus_duration[j] = _sus_duration[j + 1];
                    _Bracked_value[j] = _Bracked_value[j + 1];
                    _brack_duration[j] = _brack_duration[j + 1];
                    _SusA3[j] = _SusA3[j + 1];
                    _SusA5[j] = _SusA5[j + 1];
                    _SusV3[j] = _SusV3[j + 1];
                    _SusV5[j] = _SusV5[j + 1];
                    _SusD3[j] = _SusD3[j + 1];
                    _SusD5[j] = _SusD5[j + 1];
                    _Alfa[j] = _Alfa[j + 1];
                    _Error[j] = _Error[j + 1];
                    FFT_A[j] = FFT_A[j + 1];
                    FFT_F[j] = FFT_F[j + 1];
                    PowerFFT_A[j] = PowerFFT_A[j + 1];
                    delta_f[j] = delta_f[j + 1];
                    _AI[j] = _AI[j + 1];
                    m_dMaxSA_Alfa1[j] = m_dMaxSA_Alfa1[j + 1];
                    m_dMinSA_Alfa1[j] = m_dMinSA_Alfa1[j + 1];
                    m_dMaxSA_Alfa2[j] = m_dMaxSA_Alfa2[j + 1];
                    m_dMinSA_Alfa2[j] = m_dMinSA_Alfa2[j + 1];
                    m_dMaxSA_Alfa0[j] = m_dMaxSA_Alfa0[j + 1];
                    m_dMinSA_Alfa0[j] = m_dMinSA_Alfa0[j + 1];
                    _S_A_Alfa0[j] = _S_A_Alfa0[j + 1];
                    _S_A_Alfa1[j] = _S_A_Alfa1[j + 1];
                    _S_A_Alfa2[j] = _S_A_Alfa2[j + 1];
                }

                m_strInputMotionFiles[m_nCountMotion - 1] = null;
                m_strListName[m_nCountMotion - 1] = null;
                _T[m_nCountMotion - 1] = null;
                _A[m_nCountMotion - 1] = null;
                delta_t[m_nCountMotion - 1] = 0;
                m_nCnt[m_nCountMotion - 1] = 0;
                m_listIndex[m_nCountMotion - 1] = 0;
                m_dMaxT[m_nCountMotion - 1] = 0;
                m_dMinT[m_nCountMotion - 1] = 0;
                m_dMaxA[m_nCountMotion - 1] = 0;
                m_dMinA[m_nCountMotion - 1] = 0;
                m_dMaxV[m_nCountMotion - 1] = 0;
                m_dMinV[m_nCountMotion - 1] = 0;
                m_dMaxSA[m_nCountMotion - 1] = 0;
                m_dMinSA[m_nCountMotion - 1] = 0;
                m_dMaxSV[m_nCountMotion - 1] = 0;
                m_dMinSV[m_nCountMotion - 1] = 0;
                m_dMaxSD[m_nCountMotion - 1] = 0;
                m_dMinSD[m_nCountMotion - 1] = 0;
                _V[m_nCountMotion - 1] = null;
                _D[m_nCountMotion - 1] = null;
                _S_A[m_nCountMotion - 1] = null;
                _S_V[m_nCountMotion - 1] = null;
                _S_D[m_nCountMotion - 1] = null;
                nSLen[m_nCountMotion - 1] = 0;
                _PGA[m_nCountMotion - 1] = 0;
                _PGD[m_nCountMotion - 1] = 0;
                _A_ams[m_nCountMotion - 1] = 0;
                _V_ams[m_nCountMotion - 1] = 0;
                _D_ams[m_nCountMotion - 1] = 0;
                _Ic[m_nCountMotion - 1] = 0;
                _SED[m_nCountMotion - 1] = 0;
                _CAV[m_nCountMotion - 1] = 0;
                _NoECA[m_nCountMotion - 1] = 0;
                _AI_MP[m_nCountMotion - 1] = 0;
                _D5AI[m_nCountMotion - 1] = 0;
                _D95AI[m_nCountMotion - 1] = 0;
                _D5AI_pos[m_nCountMotion - 1] = 0;
                _D95AI_pos[m_nCountMotion - 1] = 0;
                _sus_duration[m_nCountMotion - 1] = 0;
                _Bracked_value[m_nCountMotion - 1] = 0;
                _brack_duration[m_nCountMotion - 1] = 0;
                _SusA3[m_nCountMotion - 1] = 0;
                _SusA5[m_nCountMotion - 1] = 0;
                _SusV3[m_nCountMotion - 1] = 0;
                _SusV5[m_nCountMotion - 1] = 0;
                _SusD3[m_nCountMotion - 1] = 0;
                _SusD5[m_nCountMotion - 1] = 0;
                _Alfa[m_nCountMotion - 1] = 0;
                _Error[m_nCountMotion - 1] = 0;
                FFT_A[m_nCountMotion - 1] = null;
                FFT_F[m_nCountMotion - 1] = null;
                PowerFFT_A[m_nCountMotion - 1] = null;
                delta_f[m_nCountMotion - 1] = 0;
                _AI[m_nCountMotion - 1] = null;
                m_dMaxSA_Alfa1[m_nCountMotion - 1] = 0;
                m_dMinSA_Alfa1[m_nCountMotion - 1] = 0;
                m_dMaxSA_Alfa2[m_nCountMotion - 1] = 0;
                m_dMinSA_Alfa2[m_nCountMotion - 1] = 0;
                m_dMaxSA_Alfa0[m_nCountMotion - 1] = 0;
                m_dMinSA_Alfa0[m_nCountMotion - 1] = 0;
                _S_A_Alfa0[m_nCountMotion - 1] = null;
                _S_A_Alfa1[m_nCountMotion - 1] = null;
                _S_A_Alfa2[m_nCountMotion - 1] = null;

                m_nCountMotion--;
            }
        }

        private void btnDI_RemoveSelectedMotion_Click(object sender, EventArgs e)
        {
            int[] nDelList = new int[checkedListDI_Accelogram.CheckedItems.Count];
            int nn = 0;
            for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
            {
                if (checkedListDI_Accelogram.GetItemChecked(i))
                {
                    nDelList[nn] = i;
                    nn++;
                }
            }
            DeleteMotionDataList(nDelList);

            PostAllMessage();
            PostControlMsg((Int32)ControlMessage.msgDI_ComboS_AVD);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTA);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTV);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTD);

            PostControlMsg((Int32)ControlMessage.msgMP_ListIntensityParameters);
            PostControlMsg((Int32)ControlMessage.msgMP_ListDurationParameters);
            this.Refresh();
        }

        private void btnDI_RefreshGraphs_Click(object sender, EventArgs e)
        {
            picDI_Acceleration.Refresh();
            picDI_Spectr.Refresh();

            //this.Refresh();
        }

        private void btnSM_RemoveSelectedMotion_Click(object sender, EventArgs e)
        {
            int[] nDelList = new int[checkedListSM_Accelogram.CheckedItems.Count];
            int nn = 0;
            for (int i = 0; i < checkedListSM_Accelogram.Items.Count; i++)
            {
                //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                if (checkedListSM_Accelogram.GetItemChecked(i))
                {
                    nDelList[nn] = i;
                    nn++;
                }
            }
            DeleteMotionDataList(nDelList);
            PostAllMessage();

            PostControlMsg((Int32)ControlMessage.msgDI_ComboS_AVD);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTA);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTV);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTD);

            PostControlMsg((Int32)ControlMessage.msgMP_ListIntensityParameters);
            PostControlMsg((Int32)ControlMessage.msgMP_ListDurationParameters);

            //             picDI_Acceleration.Invalidate();
            //             picDI_Spectr.Invalidate();
            // 
            //             picSMD_Acceleration.Invalidate();
            //             picSMD_Velocity.Invalidate();
            //             picSMD_Displacement.Invalidate();
            //             picSMD_AriasIntensity.Invalidate();
            // 
            //             //Spectral Matching
            //             picSM_SM_Acceleration.Invalidate();
            //             picSM_MD_Acceleration.Invalidate();
            //             picSM_MS_Acceleration.Invalidate();
            // 
            //             pictSM_SM_Spectral.Invalidate();
            //             pictSM_MD_Spectral.Invalidate();
            //             pictSM_MS_Spectral.Invalidate();
            // 
            //             picMP_FrequencyGraph.Invalidate();
            //             picMP_AriasIntensity.Invalidate();
            //             picMP_Acceleration.Invalidate();
            // 
            //             picDS_Last.Invalidate();
            this.Refresh();
        }

        private void btnSM_RefreshGraphs_Click(object sender, EventArgs e)
        {
            picSM_SM_Acceleration.Refresh();
            picSM_SM_Spectral.Refresh();
        }

        private void btnDS_RemoveSelectedMotion_Click(object sender, EventArgs e)
        {
            int[] nDelList = new int[checkedListDS_Accelogram.CheckedItems.Count];
            int nn = 0;
            for (int i = 0; i < checkedListDS_Accelogram.Items.Count; i++)
            {
                //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                if (checkedListDS_Accelogram.GetItemChecked(i))
                {
                    nDelList[nn] = i;
                    nn++;
                }
            }
            DeleteMotionDataList(nDelList);

            PostControlMsg((Int32)ControlMessage.msgDI_MotionList);
            PostControlMsg((Int32)ControlMessage.msgSM_MotionList);
            PostControlMsg((Int32)ControlMessage.msgDS_MotionList);
            PostControlMsg((Int32)ControlMessage.msgMP_MotionList);

            PostControlMsg((Int32)ControlMessage.msgSM_MD_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgSM_MS_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgSMD_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTA);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTV);
            PostControlMsg((Int32)ControlMessage.msgSMD_ListTD);

            PostControlMsg((Int32)ControlMessage.msgMP_ListValues);
            PostControlMsg((Int32)ControlMessage.msgMP_MotionDataCombo);
            PostControlMsg((Int32)ControlMessage.msgMP_ListIntensityParameters);
            PostControlMsg((Int32)ControlMessage.msgMP_ListDurationParameters);


            //             picDI_Acceleration.Invalidate();
            //             picDI_Spectr.Invalidate();
            // 
            //             picSMD_Acceleration.Invalidate();
            //             picSMD_Velocity.Invalidate();
            //             picSMD_Displacement.Invalidate();
            //             picSMD_AriasIntensity.Invalidate();
            // 
            //             //Spectral Matching
            //             picSM_SM_Acceleration.Invalidate();
            //             picSM_MD_Acceleration.Invalidate();
            //             picSM_MS_Acceleration.Invalidate();
            // 
            //             pictSM_SM_Spectral.Invalidate();
            //             pictSM_MD_Spectral.Invalidate();
            //             pictSM_MS_Spectral.Invalidate();
            // 
            //             picMP_FrequencyGraph.Invalidate();
            //             picMP_AriasIntensity.Invalidate();
            //             picMP_Acceleration.Invalidate();
            // 
            //             picDS_Last.Invalidate();
            this.Refresh();
        }

        private void btnDS_RefreshGraphs_Click(object sender, EventArgs e)
        {
            picDS_Last.Refresh();
        }        

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabPage current = (sender as TabControl).SelectedTab;

            if (tabControl1.SelectedIndex == 3 && bTargetGenerated1)
            {
                GenerateTarget1();
            }
            else if (tabControl1.SelectedIndex == 4 && bTargetGenerated2)
            {
                GenerateTarget2();
            }
            // Validate the current page. To cancel the select, use:
            //e.Cancel = true;
        }

        private void comboSM_MSMotionDatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //             picDI_Acceleration.Invalidate();
            //             picDI_Spectr.Invalidate();
            // 
            //             picSMD_Acceleration.Invalidate();
            //             picSMD_Velocity.Invalidate();
            //             picSMD_Displacement.Invalidate();
            //             picSMD_AriasIntensity.Invalidate();
            // 
            //             //Spectral Matching
            //             picSM_SM_Acceleration.Invalidate();
            //             picSM_MD_Acceleration.Invalidate();
            //             picSM_MS_Acceleration.Invalidate();
            // 
            //             pictSM_SM_Spectral.Invalidate();
            //             pictSM_MD_Spectral.Invalidate();
            //             pictSM_MS_Spectral.Invalidate();
            // 
            //             picMP_FrequencyGraph.Invalidate();
            //             picMP_AriasIntensity.Invalidate();
            //             picMP_Acceleration.Invalidate();
            // 
            //             picDS_Last.Invalidate();

            picSM_MS_Acceleration.Refresh();
            picSM_MS_Spectral.Refresh();

        }

        private System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, int wid, int hei, int leftPadding = 50, int bottomPadding = 50)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)wid / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)hei / (float)sourceHeight);

            //             if (nPercentH < nPercentW)
            //                 nPercent = nPercentH;
            //             else
            //                 nPercent = nPercentW;
            //New Width  
            //             int destWidth = (int)(sourceWidth * nPercent);
            //             //New Height  
            //             int destHeight = (int)(sourceHeight * nPercent);

            int destWidth = (int)(wid);
            //New Height  
            int destHeight = (int)(hei);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height 

            float padding_left = 60;
            float padding_right = 50;
            float padding_top = 30;
            float padding_bottom = 42;

            RectangleF desRect = new RectangleF(padding_left, 0, destWidth - padding_left, destHeight - padding_bottom);
            RectangleF srcRect = new RectangleF(padding_left, 0, sourceWidth - padding_left, sourceHeight - padding_bottom);

            RectangleF desLetterX = new RectangleF(0, destHeight - padding_bottom, destWidth, padding_bottom);
            RectangleF srcLetterX = new RectangleF(0, sourceHeight - padding_bottom, sourceWidth, padding_bottom);

            RectangleF desLetterY = new RectangleF(padding_left - 20, 0, 20, destHeight - padding_bottom);
            RectangleF srcLetterY = new RectangleF(padding_left - 20, 0, 20, sourceHeight - padding_bottom);

            g.Clear(m_colorPicBack);
            g.DrawImage(imgToResize, desRect, srcRect, GraphicsUnit.Pixel);
            g.DrawImage(imgToResize, desLetterX, srcLetterX, GraphicsUnit.Pixel);
            g.DrawImage(imgToResize, desLetterY, srcLetterY, GraphicsUnit.Pixel);

            //g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        private void EventMethodInputImageSize(int width, int height)
        {
            if (width <= 0 && height <= 0)
                return;

            Bitmap bit = null;

            if(m_ClipboardControlType == PicControl.DI_Acceleration)
            {
                float[] padding = { 80, 80, 50, 50 };

                //for (int i = 0; i < m_nCountMotion; i ++)                
                //int i = m_nCountMotion - 1;
                int[] nShowList = new int[checkedListDI_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDI_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }

                if (nn > 0)
                {
                    bit = DrawAllGraphToClipboard(picDI_Acceleration, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, width, height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                    //picDI_Acceleration.Image = bit;
                }
            }
            else if (m_ClipboardControlType == PicControl.DI_Spectr)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[checkedListDI_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDI_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }
                //DrawCtrl = picDI_Spectr;
                if (nn > 0)
                {
                    bit = DrawAllGraph_SpectrToClipboard(picDI_Spectr, Color.Blue, 0, nShowList, width, height, padding, m_nSelSpectList, false);
                    //bit = DrawAllToClipboard(Color.Blue, 0, nShowList, width, height, padding, m_nSelSpectList, false);
                }
            }
            else if (m_ClipboardControlType == PicControl.SMD_Acceleration)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSMD_MotionDatas.SelectedIndex;
                
                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picSMD_Acceleration, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, width, height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                    picSMD_Acceleration.Image = bit;
                }


            }
            else if (m_ClipboardControlType == PicControl.SMD_Velocity)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSMD_MotionDatas.SelectedIndex;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picSMD_Velocity, Color.Blue, "Velocity (cm/s)", _V, _T, m_nCnt, nShowList, width, height, m_dMaxV, m_dMinV, m_dMaxT, m_dMinT, padding);
                }
            }
            else if (m_ClipboardControlType == PicControl.SMD_Displacement)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSMD_MotionDatas.SelectedIndex;
                //DrawCtrl = picSMD_Displacement;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picSMD_Displacement, Color.Blue, "Displacement (cm)", _D, _T, m_nCnt, nShowList, width, height, m_dMaxD, m_dMinD, m_dMaxT, m_dMinT, padding);
                }
            }
            else if(m_ClipboardControlType == PicControl.SMD_AriasIntensity)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSMD_MotionDatas.SelectedIndex;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picSMD_AriasIntensity, Color.Blue, "AriasIntensity (%)", _AI, _T, m_nCnt, nShowList, width, height, m_dMaxAI, m_dMinAI, m_dMaxT, m_dMinT, padding);
                }
            }
            else if(m_ClipboardControlType == PicControl.MP_FrequencyGraph)
            {
                int[] nShowList = new int[1];
                nShowList[0] = comboMP_MotionDatas.SelectedIndex;
                int style = comboMP_F_P_S.SelectedIndex;

                if (style == 0)
                {
                    float[] padding = { 80, 30, 50, 50 };
                    int[] cnt = new int[11];
                    for (int i = 0; i < 11; i++)
                    {
                        cnt[i] = m_nLenFourier;
                    }
                    //DrawCtrl = picMP_FrequencyGraph;

                    if (nShowList[0] >= 0)
                    {
                        bit = DrawAllGraphToClipboard(picMP_FrequencyGraph, Color.Blue, "Fourier Amplitude", FFT_A, FFT_F, cnt, nShowList, width, height, m_dMaxFFTSA, m_dMinFFTSA, m_dMaxFFTSF, m_dMinFFTSF, padding, 3);
                        picMP_FrequencyGraph.Image = bit;
                    }
                    else
                    {
                        picMP_FrequencyGraph.Image = null;
                    }
                }
                else if (style == 1)
                {
                    float[] padding = { 80, 30, 50, 50 };
                    int[] cnt = new int[11];
                    for (int i = 0; i < 11; i++)
                    {
                        cnt[i] = m_nLenFourier;
                    }
                    //DrawCtrl = picMP_FrequencyGraph;
                    if (nShowList[0] >= 0)
                    {
                        bit = DrawAllGraphToClipboard(picMP_FrequencyGraph, Color.Blue, "Power Spectrum", PowerFFT_A, FFT_F, cnt, nShowList, width, height, m_dMaxPowerFFTSA, m_dMinPowerFFTSA, m_dMaxFFTSF, m_dMinFFTSF, padding, 3);
                    }

                }
                else if (style == 2)
                {
                    float[] padding = { 80, 30, 50, 50 };
                    //DrawCtrl = picMP_FrequencyGraph;


                    if (nShowList[0] >= 0)
                    {
                        bit = DrawAllGraph_SpectrToClipboard(picMP_FrequencyGraph, Color.Blue, 0, nShowList, width, height, padding, 0, false);
                    }
                }
            }
            else if(m_ClipboardControlType == PicControl.MP_AriasIntensity)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboMP_MotionDatas.SelectedIndex;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picMP_AriasIntensity, Color.Blue, "AriasIntensity (%)", _AI, _T, m_nCnt, nShowList, width, height, m_dMaxAI, m_dMinAI, m_dMaxT, m_dMinT, padding, 1);
                }
            }
            else if(m_ClipboardControlType == PicControl.MP_Acceleration)
            {
                float[] padding = { 80, 30, 50, 30 };
                int[] nShowList = new int[1];
                nShowList[0] = comboMP_MotionDatas.SelectedIndex;
                //DrawCtrl = picMP_Acceleration;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picMP_Acceleration, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, width, height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding, 2);
                }
            }
            else if(m_ClipboardControlType == PicControl.SM_SM_Acceleration)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[checkedListSM_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListSM_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListSM_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }

                if (nn > 0)
                {
                    bit = DrawAllGraphToClipboard(picSM_SM_Acceleration, Color.Blue, "Accerelation (g)", _A, _T, m_nCnt, nShowList, width, height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                }
            }
            else if (m_ClipboardControlType == PicControl.SM_SM_Spectral)
            {
                float[] padding = { 120, 30, 50, 50 };
                int[] nShowList = new int[checkedListDI_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDI_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }
                }

                //DrawCtrl = pictSM_SM_Spectral;

                if (nn > 0)
                {
                    if (bTargetGenerated1)
                        bit = DrawAllGraph_SpectrToClipboard(picSM_SM_Spectral, Color.Blue, 3, nShowList, width, height, padding, 0, true);
                    else
                        bit = DrawAllGraph_SpectrToClipboard(picSM_SM_Spectral, Color.Blue, 3, nShowList, width, height, padding, 0, false);
                }
            }
            else if (m_ClipboardControlType == PicControl.SM_MD_Acceleration)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MDMotionDatas.SelectedIndex;
                DrawCtrl = picSM_MD_Acceleration;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picSM_MD_Acceleration, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, width, height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                }
            }
            else if (m_ClipboardControlType == PicControl.SM_MD_Spectral)
            {
                float[] padding = { 120, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MDMotionDatas.SelectedIndex;
                //DrawCtrl = pictSM_MD_Spectral;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraph_SpectrToClipboard(picSM_MD_Acceleration, Color.Blue, 1, nShowList, width, height, padding, 0, true);      
                }
            }
            else if (m_ClipboardControlType == PicControl.SM_MS_Acceleration)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MSMotionDatas.SelectedIndex;
                //DrawCtrl = picSM_MS_Acceleration;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraphToClipboard(picSM_MS_Acceleration, Color.Blue, "Acceleration (g)", _A, _T, m_nCnt, nShowList, width, height, m_dMaxA, m_dMinA, m_dMaxT, m_dMinT, padding);
                }
            }
            else if (m_ClipboardControlType == PicControl.SM_MS_Spectral)
            {
                float[] padding = { 120, 30, 50, 50 };
                int[] nShowList = new int[1];
                nShowList[0] = comboSM_MSMotionDatas.SelectedIndex;
                //DrawCtrl = pictSM_MS_Spectral;

                if (nShowList[0] >= 0)
                {
                    bit = DrawAllGraph_SpectrToClipboard(picSM_MS_Spectral, Color.Blue, 2, nShowList, width, height, padding, 0, true);
                }
            }
            else if (m_ClipboardControlType == PicControl.DS_Last)
            {
                float[] padding = { 80, 30, 50, 50 };
                int[] nShowList = new int[checkedListDS_Accelogram.CheckedItems.Count];
                int nn = 0;
                for (int i = 0; i < checkedListDS_Accelogram.Items.Count; i++)
                {
                    //nShowList[nn] = checkedListDI_Accelogram.CheckedItems[i];
                    if (checkedListDS_Accelogram.GetItemChecked(i))
                    {
                        nShowList[nn] = i;
                        nn++;
                    }

                }
                //DrawCtrl = picDS_Last;


                if (nn > 0)
                {
                    if (bTargetGenerated2)
                        bit = DrawAllGraph_SpectrToClipboard(picDS_Last, Color.Blue, 3, nShowList, width, height, padding, -2, true);
                    else
                        bit = DrawAllGraph_SpectrToClipboard(picDS_Last, Color.Blue, 3, nShowList, width, height, padding, -2, false);
                }
            }

            if (bit != null)
                System.Windows.Forms.Clipboard.SetImage(bit);            
        }

        enum PicControl
        {
            DI_Acceleration = 1,
            DI_Spectr,
            SMD_Acceleration,
            SMD_Velocity,
            SMD_Displacement,
            SMD_AriasIntensity,
            MP_FrequencyGraph,
            MP_AriasIntensity,
            MP_Acceleration,
            SM_SM_Acceleration,
            SM_SM_Spectral,
            SM_MD_Acceleration,
            SM_MD_Spectral,
            SM_MS_Acceleration,
            SM_MS_Spectral,
            DS_Last
        }
        
        private PicControl m_ClipboardControlType = 0;

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.PictureBox ctrol = (System.Windows.Forms.PictureBox)FindControlAtCursor(this);
            if (ctrol.Image != null)
            {
                //Image img = resizeImage(ctrol.Image, ctrol.Image.Width, ctrol.Image.Height * 2, 50, 50);

                if(ctrol == picDI_Acceleration)
                {
                    m_ClipboardControlType = PicControl.DI_Acceleration;
                }
                else if(ctrol == picDI_Spectr)
                {
                    m_ClipboardControlType = PicControl.DI_Spectr;
                }
                else if (ctrol == picSMD_Acceleration)
                {
                    m_ClipboardControlType = PicControl.SMD_Acceleration;
                }
                else if (ctrol == picSMD_Velocity)
                {
                    m_ClipboardControlType = PicControl.SMD_Velocity;
                }
                else if (ctrol == picSMD_Displacement)
                {
                    m_ClipboardControlType = PicControl.SMD_Displacement;
                }
                else if (ctrol == picSMD_AriasIntensity)
                {
                    m_ClipboardControlType = PicControl.SMD_AriasIntensity;
                }
                else if(ctrol == picMP_FrequencyGraph)
                {
                    m_ClipboardControlType = PicControl.MP_FrequencyGraph;
                }
                else if(ctrol == picMP_AriasIntensity)
                {
                    m_ClipboardControlType = PicControl.MP_AriasIntensity;
                }
                else if(ctrol == picMP_Acceleration)
                {
                    m_ClipboardControlType = PicControl.MP_Acceleration;
                }
                else if(ctrol == picSM_SM_Acceleration)
                {
                    m_ClipboardControlType = PicControl.SM_SM_Acceleration;
                }
                else if(ctrol == picSM_SM_Spectral)
                {
                    m_ClipboardControlType = PicControl.SM_SM_Spectral;
                }
                else if(ctrol == picSM_MD_Acceleration)
                {
                    m_ClipboardControlType = PicControl.SM_MD_Acceleration;
                }
                else if (ctrol == picSM_MD_Spectral)
                {
                    m_ClipboardControlType = PicControl.SM_MD_Spectral;
                }
                else if (ctrol == picSM_MS_Acceleration)
                {
                    m_ClipboardControlType = PicControl.SM_MS_Acceleration;
                }
                else if (ctrol == picSM_MS_Spectral)
                {
                    m_ClipboardControlType = PicControl.SM_MS_Spectral;
                }
                else if(ctrol == picDS_Last)
                {
                    m_ClipboardControlType = PicControl.DS_Last;
                }

                SettingSizeForm settingSizeForm = new SettingSizeForm();
                settingSizeForm.SetData(ctrol.Image.Width, ctrol.Image.Height);
                settingSizeForm.ChildFormEvent += EventMethodInputImageSize;
                settingSizeForm.Show();

                
                //System.Windows.Forms.Clipboard.SetImage(ctrol.Image);
            }

        }

        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtCursor(Form form)
        {
            Point pos = System.Windows.Forms.Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(System.Windows.Forms.Cursor.Position));
            return null;
        }

        private void SignalDesignForm_Activated(object sender, EventArgs e)
        {
            if(isLoaded)
            {
                this.Refresh();
                //PostAllMessage();
            }
            
        }

        private bool bKeyCtrl = false;
        private bool bCopy = false;

        private void listViewEveryKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bKeyCtrl = true;
            }
            else if (bKeyCtrl && e.KeyCode == Keys.C)
            {
                string CopyText = "";
                ListView ctrl = (ListView)sender;
                int SelCount = ctrl.Items.Count;
                for (int i = 0; i < SelCount; i++)
                {
                    ListViewItem item = ctrl.Items[i];
                    for (int j = 0; j < item.SubItems.Count - 1; j++)
                    {
                        CopyText += item.SubItems[j].Text + "\t";
                    }
                    CopyText += item.SubItems[item.SubItems.Count - 1].Text + "\n";
                }
                System.Windows.Forms.Clipboard.SetText(CopyText);
                bCopy = false;
            }

        }

        private void listViewEveryKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                bKeyCtrl = false;
            }
        }

        private void contactUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContactUS contactUs = new ContactUS();
            contactUs.Show();
        }

        private void newProjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int[] nDelList = new int[checkedListDI_Accelogram.Items.Count];
            int nn = 0;
            for (int i = 0; i < checkedListDI_Accelogram.Items.Count; i++)
            {
                nDelList[nn] = i;
            }
            DeleteMotionDataList(nDelList);

            PostAllMessage();

            this.Refresh();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingForm setting = new SettingForm();
            setting.SetParameter(m_nDecimalSeperator ,m_dNumberOfDecimals, m_dDefaultDampingRatio, m_dPGARatioForNOEC, m_dPGARatioForBD, m_dAccelerationValueForBD, m_nMethodBD);
            setting.ChildFormEvent += EventMethodSettingData;
            setting.Show();
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
        private void EventMethodSettingData(int nDecimal, int skin,  double NumberOfDecimals, double DefaultDampingRatio, double PGARatioForNOEC, double PGARatioForBD, double AccelerationValueForBD, double SDSCoefficient, int method)
        {
            if (skin == 1)
            {
                Skin(Color.FromArgb(100, 100, 100));
                m_colorPicBack = Color.FromArgb(100, 100, 100);
                this.Refresh();
            }
            else
            {
                Skin(Color.FromArgb(245, 245, 245));
                m_colorPicBack = Color.FromArgb(255, 255, 255);
                this.Refresh();
            }
            m_nDecimalSeperator = nDecimal;
            m_dNumberOfDecimals = NumberOfDecimals;
            m_dDefaultDampingRatio = DefaultDampingRatio;
            m_dPGARatioForNOEC = PGARatioForNOEC;
            m_dPGARatioForBD = PGARatioForBD;
            m_dAccelerationValueForBD = AccelerationValueForBD;
            m_dSDSCoefficient = SDSCoefficient;
            m_nMethodBD = method;
            textDI_DampingRate.Text = m_dDefaultDampingRatio.ToString();

            int index = comboMP_MotionDatas.SelectedIndex;
            if (index < 0)
                return;

            if (m_nMethodBD == 0)
            {
                textMP_RatioBracketDuration.Text = m_dPGARatioForBD.ToString();
                _Bracked_value[index] = _PGA[index] * m_dPGARatioForBD / 100;
                labelMP_ValueBracketDuration.Text = _Bracked_value[index].ToString();
            }
            else
            {
                for(int i = 0; i < m_nCountMotion; i ++)
                {
                    _Bracked_value[i] = m_dAccelerationValueForBD;
                }
                if(index > 0)
                {
                    m_dPGARatioForBD = m_dAccelerationValueForBD * 100 / _PGA[index];
                    textMP_RatioBracketDuration.Text = FixDecimal1(m_dPGARatioForBD).ToString();
                }
                labelMP_ValueBracketDuration.Text = m_dAccelerationValueForBD.ToString();
            }



            _NoECA[index] = 0;
            int oldPosNOECA = 0;
            Boolean bSelectA = true;
            ///////////////////////////////////////////// _SusA3 _SusA5 ////////////////////////////////////////
            for (int i = 0; i < m_nCnt[index]; i++)
            {
                if (Math.Abs(_A[index][i]) > _PGA[index] * (m_dPGARatioForNOEC / 100) && bSelectA == true)
                {
                    if (_A[index][i] > 0)
                    {
                        oldPosNOECA = 1;
                    }
                    else
                    {
                        oldPosNOECA = -1;
                    }
                    _NoECA[index]++;
                    bSelectA = false;
                }

                if (Math.Abs(_A[index][i]) <= _PGA[index] * (m_dPGARatioForNOEC / 100))
                {
                    if ((_A[index][i] > 0 && oldPosNOECA == -1) || (_A[index][i] < 0 && oldPosNOECA == 1))
                        bSelectA = true;
                }
            }


            PostControlMsg((Int32)ControlMessage.msgMP_ListIntensityParameters);
            
        }

        private void comboDI_S_AVD_SelectedIndexChanged(object sender, EventArgs e)
        {
            PostControlMsg((Int32)ControlMessage.msgDI_ListS_AVD);
        }

        public void Skin(Color color)
        {
            this.tabControl1.BackColor = color;
            this.tabDataInput.BackColor = color;
            this.tabStrongMotionData.BackColor = color;
            this.tabMotionParameters.BackColor = color;
            this.tabSpectralMatching.BackColor = color;
            this.tabDesignSpectrum.BackColor = color;
            this.contextMenuStrip1.BackColor = color;
            this.newProjectToolStripMenuItem.BackColor = color;
            this.saveProjectToolStripMenuItem.BackColor = color;
            this.exitToolStripMenuItem.BackColor = color;
            this.menuStrip1.BackColor = color;
            this.fileToolStripMenuItem.BackColor = color;
            this.newProjectToolStripMenuItem1.BackColor = color;
            this.exitToolStripMenuItem1.BackColor = color;
            this.settingToolStripMenuItem.BackColor = color;
            this.reportToolStripMenuItem.BackColor = color;
            this.contactUsToolStripMenuItem.BackColor = color;
            this.btnDI_RefreshGraphs.BackColor = color;
            this.btnDI_RemoveSelectedMotion.BackColor = color;
            this.btnDI_AddNewMotion.BackColor = color;
            this.splitter1.BackColor = color;
            this.checkedListDI_Accelogram.BackColor = color;
            this.label1.BackColor = color;
            this.label4.BackColor = color;
            this.textDI_MaxPeriod.BackColor = color;
            this.label3.BackColor = color;
            this.textDI_DampingRate.BackColor = color;
            this.label2.BackColor = color;
            this.btnDI_DeselectAll.BackColor = color;
            this.btnDI_SelectAll.BackColor = color;
            this.splitter2.BackColor = color;
            this.listViewSMD_T_D.BackColor = color;
            this.label7.BackColor = color;
            this.label6.BackColor = color;
            this.listViewSMD_T_A.BackColor = color;
            this.label5.BackColor = color;
            this.labelSMD_MotionDatas.BackColor = color;
            this.comboSMD_MotionDatas.BackColor = color;
            this.btnCF_DeselectAll.BackColor = color;
            this.btnCF_SelectAll.BackColor = color;
            this.label9.BackColor = color;
            this.checkedListCF_Accelaration.BackColor = color;
            this.splitter3.BackColor = color;
            this.tabControlSM.BackColor = color;
            this.tabSM_SpectralMatching.BackColor = color;
            this.btnSM_DeselectAll.BackColor = color;
            this.btnSM_SelectAll.BackColor = color;
            this.label14.BackColor = color;
            this.checkedListSM_Accelogram.BackColor = color;
            this.btnSM_RefreshGraphs.BackColor = color;
            this.btnSM_RemoveSelectedMotion.BackColor = color;
            this.btnSM_AddNewMotion.BackColor = color;
            this.splitter4.BackColor = color;
            this.btnSM_DefineTargetSectra.BackColor = color;
            this.label11.BackColor = color;
            this.textSM_Tn.BackColor = color;
            this.textSM_TL.BackColor = color;
            this.textSM_SD1.BackColor = color;
            this.textSM_SDS.BackColor = color;
            this.btnSM_Match.BackColor = color;
            this.label18.BackColor = color;
            this.label17.BackColor = color;
            this.label16.BackColor = color;
            this.label13.BackColor = color;
            this.label12.BackColor = color;
            this.label27.BackColor = color;
            this.label26.BackColor = color;
            this.textDS_TA.BackColor = color;
            this.textDS_TB.BackColor = color;
            this.label19.BackColor = color;
            this.label20.BackColor = color;
            this.label21.BackColor = color;
            this.label22.BackColor = color;
            this.label23.BackColor = color;
            this.btnDS_ManuelDesign.BackColor = color;
            this.label24.BackColor = color;
            this.textDS_Tn.BackColor = color;
            this.textDS_TL.BackColor = color;
            this.textDS_SD1.BackColor = color;
            this.textDS_SDS.BackColor = color;
            this.btnDS_DeselectAll.BackColor = color;
            this.btnDS_SelectAll.BackColor = color;
            this.label25.BackColor = color;
            this.checkedListDS_Accelogram.BackColor = color;
            this.btnDS_RefreshGraphs.BackColor = color;
            this.btnDS_RemoveSelectedMotion.BackColor = color;
            this.btnDS_AddNewMotion.BackColor = color;
            this.splitter5.BackColor = color;
            this.label28.BackColor = color;
            this.picDI_Spectr.BackColor = color;
            this.tableLayoutPane_DI.BackColor = color;
            this.panel2.BackColor = color;
            this.panel3.BackColor = color;
            this.picDI_Acceleration.BackColor = color;
            this.tableLayoutPanel2.BackColor = color;
            this.picSMD_Acceleration.BackColor = color;
            this.picSMD_AriasIntensity.BackColor = color;
            this.picSMD_Velocity.BackColor = color;
            this.picSMD_Displacement.BackColor = color;
            this.tableLayoutPanel5.BackColor = color;
            this.panel1.BackColor = color;
            this.listSM_SM_Alfa.BackColor = color;
            this.picSM_SM_Spectral.BackColor = color;
            this.picSM_SM_Acceleration.BackColor = color;
            this.tabSM_MatchedData.BackColor = color;
            this.tableLayoutPanel6.BackColor = color;
            this.panel4.BackColor = color;
            this.picSM_MD_Spectral.BackColor = color;
            this.picSM_MD_Acceleration.BackColor = color;
            this.tabSM_ManuelScaling.BackColor = color;
            this.tableLayoutPanel7.BackColor = color;
            this.panel5.BackColor = color;
            this.picSM_MS_Spectral.BackColor = color;
            this.picSM_MS_Acceleration.BackColor = color;
            this.picDS_Last.BackColor = color;
            this.comboDI_SpectList.BackColor = color;
            this.comboDL_MethodList.BackColor = color;
            this.comboSM_SOILTYPE.BackColor = color;
            this.picSM_TargetSpectrum.BackColor = color;
            this.tableLayoutPanel8.BackColor = color;
            this.panel6.BackColor = color;
            this.panel7.BackColor = color;
            this.panel8.BackColor = color;

            this.panel9.BackColor = color;
            this.label15.BackColor = color;

            this.panel10.BackColor = color;
            this.label29.BackColor = color;
            this.listSM_MD_Alfa.BackColor = color;
            this.btnSM_MDSaveScaledMotion.BackColor = color;
            this.panel11.BackColor = color;
            this.btnSM_MS_SaveScaledMotion.BackColor = color;
            this.btnSM_MS_ScaleMotion.BackColor = color;
            this.label30.BackColor = color;
            this.textSM_MS_Alfa.BackColor = color;
            this.comboDS_SOILTYPE.BackColor = color;
            this.picDS_TargetSpectrum.BackColor = color;
            this.label31.BackColor = color;
            this.comboSM_MDMotionDatas.BackColor = color;
            this.comboSM_MSMotionDatas.BackColor = color;
            this.label32.BackColor = color;
            this.textSM_MS_Error.BackColor = color;
            this.label33.BackColor = color;
            this.panel12.BackColor = color;
            this.listViewMP_IntensityParameter.BackColor = color;
            this.label10.BackColor = color;
            this.comboMP_MotionDatas.BackColor = color;
            this.tableLayoutPanel1.BackColor = color;
            this.picMP_FrequencyGraph.BackColor = color;
            this.picMP_AriasIntensity.BackColor = color;
            this.picMP_Acceleration.BackColor = color;
            this.listViewMP_DurationParamer.BackColor = color;
            this.label34.BackColor = color;
            this.panel13.BackColor = color;
            this.comboMP_F_P_S.BackColor = color;
            this.tableLayoutPanel3.BackColor = color;
            this.panel14.BackColor = color;
            this.panel15.BackColor = color;
            this.panel16.BackColor = color;
            this.label_MP_String.BackColor = color;
            this.panel17.BackColor = color;
            this.textMP_RatioBracketDuration.BackColor = color;
            this.labelMP_ValueBracketDuration.BackColor = color;
            this.menuRClick.BackColor = color;
            this.copyToolStripMenuItem.BackColor = color;
            this.listViewMP_Values.BackColor = color;

            this.listViewSMD_T_V.BackColor = color;

            this.listViewDI_S_AVD.BackColor = color;

            this.comboDI_S_AVD.BackColor = color;
        } 

        private void pictSM_MS_Spectral_Click(object sender, EventArgs e)
        {

        }

        private void textDI_DampingRate_TextChanged(object sender, EventArgs e)
        {
            string strText = textDI_DampingRate.Text;
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

            textDI_DampingRate.Text = strText;
            textDI_DampingRate.Focus();
            textDI_DampingRate.SelectionStart = strText.Length;

            Double.TryParse(textDI_DampingRate.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out m_dDefaultDampingRatio);

            double dump = m_dDefaultDampingRatio;
            if (dump <= 0)
            {
                //MessageBox.Show("Max Dump Rateshould be more than 0s.");
                textDI_DampingRate.Text = "5";
                m_dDefaultDampingRatio = 5;
                return;
            }

            isLoaded = false;

            if (m_nCountMotion > 0)
            {
                for (int i = 0; i < m_nCountMotion; i++)
                    GetSpectralAcc_Vel_Disp(i);

                DrawGraph();
            }



            isLoaded = true;
            picDI_Spectr.Refresh();
        }

        private void textDI_MaxPeriod_TextChanged(object sender, EventArgs e)
        {
            string strText = textDI_MaxPeriod.Text;
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

            textDI_MaxPeriod.Text = strText;
            textDI_MaxPeriod.Focus();
            textDI_MaxPeriod.SelectionStart = strText.Length;

            double periodTime;
            Double.TryParse(textDI_MaxPeriod.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out periodTime);

            if (periodTime <= 0)
            {
                //MessageBox.Show("Max Peroid Time should be more than 0s.");
                textDI_MaxPeriod.Text = "10";
                return;
            }
            if (m_nCountMotion > 0)
            {
                for (int i = 0; i < m_nCountMotion; i++)
                    GetSpectralAcc_Vel_Disp(i);

                DrawGraph();
            }

            picDI_Spectr.Refresh();

            //this.Refresh();
        }

        private void textMP_RatioBracketDuration_TextChanged(object sender, EventArgs e)
        {
            string strText = textMP_RatioBracketDuration.Text;
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

            textMP_RatioBracketDuration.Text = strText;
            textMP_RatioBracketDuration.Focus();
            textMP_RatioBracketDuration.SelectionStart = strText.Length;

            Double.TryParse(textMP_RatioBracketDuration.Text.ToString(), NumberStyles.Any,
               CultureInfo.CurrentCulture.NumberFormat, out m_dPGARatioForBD);

            if (m_dPGARatioForBD <= 0)
            {
                //MessageBox.Show("Bracket Ratio must be more than 0%.");
                textMP_RatioBracketDuration.Text = "5";
                m_dPGARatioForBD = 5;
                return;
            }
            else if (m_dPGARatioForBD >= 100)
            {
                //MessageBox.Show(this, "Bracket Ratio can't be more than 100%.");
                textMP_RatioBracketDuration.Text = "5";
                m_dPGARatioForBD = 5;
                return;
            }


            int index = comboMP_MotionDatas.SelectedIndex;
            if (index < 0)
                return;

            int first;
            _Bracked_value[index] = _PGA[index] * m_dPGARatioForBD / 100;

            //labelMP_BracketDuration.Text = _Bracked_value[index].ToString();
            for (first = 0; first < m_nCnt[index]; first++)
            {
                if (Math.Abs(_A[index][first]) > _Bracked_value[index])
                    break;
            }

            int second;

            for (second = m_nCnt[index] - 1; second > -1; second--)
            {
                if (Math.Abs(_A[index][second]) > _Bracked_value[index])
                    break;
            }

            if (second == -1 || first == m_nCnt[index])
            {
                //MessageBox.Show("Warning! \nData is not right!");
                return;
            }

            _brack_duration[index] = _T[index][second] - _T[index][first];



            labelMP_ValueBracketDuration.Text = _Bracked_value[index].ToString();
            isLoaded = false;


            PostControlMsg((Int32)ControlMessage.msgMP_ListDurationParameters);

            //picMP_Acceleration.Invalidate();

            //picDS_Last.Invalidate();

            listViewMP_DurationParamer.Refresh();

            isLoaded = true;
        }

        private void textDI_AccInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textDI_AccInterval.Text;
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

            textDI_AccInterval.Text = strText;
            textDI_AccInterval.Focus();
            textDI_AccInterval.SelectionStart = strText.Length;

            double accInterval = 0;
            Double.TryParse(textDI_AccInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out accInterval);

            if (accInterval == 0)
                return;

            picDI_Acceleration.Refresh();
//             picSMD_Acceleration.Refresh();
//             picSMD_Velocity.Refresh();
//             picSMD_Displacement.Refresh();
//             picSMD_AriasIntensity.Refresh();
        }

        private void textDI_TimeInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textDI_TimeInterval.Text;
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

            textDI_TimeInterval.Text = strText;
            textDI_TimeInterval.Focus();
            textDI_TimeInterval.SelectionStart = strText.Length;

            double timeInterval = 0;
            Double.TryParse(textDI_TimeInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out timeInterval);

            if (timeInterval == 0)
                return;

            picDI_Acceleration.Refresh();
//             picSMD_Acceleration.Refresh();
//             picSMD_Velocity.Refresh();
//             picSMD_Displacement.Refresh();
//             picSMD_AriasIntensity.Refresh();
        }

        private void textDI_SaInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textDI_SaInterval.Text;
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

            textDI_SaInterval.Text = strText;
            textDI_SaInterval.Focus();
            textDI_SaInterval.SelectionStart = strText.Length;

            double SaInterval = 0;
            Double.TryParse(textDI_SaInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out SaInterval);

            if (SaInterval == 0)
                return;

            picDI_Spectr.Refresh();
        }

        private void textDI_SaPeriodInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textDI_SaPeriodInterval.Text;
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

            textDI_SaPeriodInterval.Text = strText;
            textDI_SaPeriodInterval.Focus();
            textDI_SaPeriodInterval.SelectionStart = strText.Length;

            double PeriodInterval = 0;
            Double.TryParse(textDI_SaPeriodInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out PeriodInterval);

            if (PeriodInterval == 0)
                return;

            picDI_Spectr.Refresh();
        }

        private void textSMD_AccInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_AccInterval.Text;
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

            textSMD_AccInterval.Text = strText;
            textSMD_AccInterval.Focus();
            textSMD_AccInterval.SelectionStart = strText.Length;

            double AccInterval = 0;
            Double.TryParse(textSMD_AccInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out AccInterval);

            if (AccInterval == 0)
                return;

            _SMD_AccInterval[m_nSMDComboMotionIndex] = textSMD_AccInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_Acceleration.Refresh();
        }

        private void textSMD_AccTimeInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_AccTimeInterval.Text;
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

            textSMD_AccTimeInterval.Text = strText;
            textSMD_AccTimeInterval.Focus();
            textSMD_AccTimeInterval.SelectionStart = strText.Length;

            double AccTimeInterval = 0;
            Double.TryParse(textSMD_AccTimeInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out AccTimeInterval);

            if (AccTimeInterval == 0)
                return;

            //if(_SMD_AccTimeInterval[m_nSMDComboMotionIndex] == textSMD_AccTimeInterval.Text)
            _SMD_AccTimeInterval[m_nSMDComboMotionIndex] = textSMD_AccTimeInterval.Text;

            if(m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_Acceleration.Refresh();
        }

        private void textSMD_VccInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_VccInterval.Text;
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

            textSMD_VccInterval.Text = strText;
            textSMD_VccInterval.Focus();
            textSMD_VccInterval.SelectionStart = strText.Length;

            double VccInterval = 0;
            Double.TryParse(textSMD_VccInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out VccInterval);

            if (VccInterval == 0)
                return;

            _SMD_VccInterval[m_nSMDComboMotionIndex] = textSMD_VccInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_Velocity.Refresh();
        }

        private void textSMD_VccTimeInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_VccTimeInterval.Text;
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

            textSMD_VccTimeInterval.Text = strText;
            textSMD_VccTimeInterval.Focus();
            textSMD_VccTimeInterval.SelectionStart = strText.Length;

            double VccTimeInterval = 0;
            Double.TryParse(textSMD_VccTimeInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out VccTimeInterval);

            if (VccTimeInterval == 0)
                return;

            _SMD_VccTimeInterval[m_nSMDComboMotionIndex] = textSMD_VccTimeInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_Velocity.Refresh();
        }

        private void textSMD_DisInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_DisInterval.Text;
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

            textSMD_DisInterval.Text = strText;
            textSMD_DisInterval.Focus();
            textSMD_DisInterval.SelectionStart = strText.Length;

            double DisInterval = 0;
            Double.TryParse(textSMD_DisInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out DisInterval);

            if (DisInterval == 0)
                return;

            _SMD_DisInterval[m_nSMDComboMotionIndex] = textSMD_DisInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_Displacement.Refresh();
        }

        private void textSMD_DisTimeInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_DisTimeInterval.Text;
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

            textSMD_DisTimeInterval.Text = strText;
            textSMD_DisTimeInterval.Focus();
            textSMD_DisTimeInterval.SelectionStart = strText.Length;

            double DisTimeInterval = 0;
            Double.TryParse(textSMD_DisTimeInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out DisTimeInterval);

            if (DisTimeInterval == 0)
                return;

            _SMD_DisTimeInterval[m_nSMDComboMotionIndex] = textSMD_DisTimeInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_Displacement.Refresh();
        }

        private void textSMD_AIInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_AIInterval.Text;
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

            textSMD_AIInterval.Text = strText;
            textSMD_AIInterval.Focus();
            textSMD_AIInterval.SelectionStart = strText.Length;

            double AIInterval = 0;
            Double.TryParse(textSMD_AIInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out AIInterval);

            if (AIInterval == 0)
                return;

            _SMD_AIInterval[m_nSMDComboMotionIndex] = textSMD_AIInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_AriasIntensity.Refresh();
        }

        private void textSMD_AITimeInterval_TextChanged(object sender, EventArgs e)
        {
            string strText = textSMD_AITimeInterval.Text;
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

            textSMD_AITimeInterval.Text = strText;
            textSMD_AITimeInterval.Focus();
            textSMD_AITimeInterval.SelectionStart = strText.Length;

            double AITimeInterval = 0;
            Double.TryParse(textSMD_AITimeInterval.Text.ToString(), NumberStyles.Any,
                CultureInfo.CurrentCulture.NumberFormat, out AITimeInterval);

            if (AITimeInterval == 0)
                return;

            _SMD_AITimeInterval[m_nSMDComboMotionIndex] = textSMD_AITimeInterval.Text;

            if (m_nOldSMDComboMotionIndex == m_nSMDComboMotionIndex)
                picSMD_AriasIntensity.Refresh();
        }

        private void textSM_SDS_TextChanged(object sender, EventArgs e)
        {
            string strText = textSM_SDS.Text;
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

            textSM_SDS.Text = strText;
            textSM_SDS.Focus();
            textSM_SDS.SelectionStart = strText.Length;
        }

        private void textSM_SD1_TextChanged(object sender, EventArgs e)
        {
            string strText = textSM_SD1.Text;
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

            textSM_SD1.Text = strText;
            textSM_SD1.Focus();
            textSM_SD1.SelectionStart = strText.Length;
        }

        private void textSM_TL_TextChanged(object sender, EventArgs e)
        {
            string strText = textSM_TL.Text;
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

            textSM_TL.Text = strText;
            textSM_TL.Focus();
            textSM_TL.SelectionStart = strText.Length;            
        }

        private void textSM_Tn_TextChanged(object sender, EventArgs e)
        {
            string strText = textSM_Tn.Text;
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

            textSM_Tn.Text = strText;
            textSM_Tn.Focus();
            textSM_Tn.SelectionStart = strText.Length;
        }

        private void textSM_MS_Alfa_TextChanged(object sender, EventArgs e)
        {
            string strText = textSM_MS_Alfa.Text;
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

            textSM_MS_Alfa.Text = strText;
            textSM_MS_Alfa.Focus();
            textSM_MS_Alfa.SelectionStart = strText.Length;
        }

        private void textDS_SDS_TextChanged(object sender, EventArgs e)
        {
            string strText = textDS_SDS.Text;
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

            textDS_SDS.Text = strText;
            textDS_SDS.Focus();
            textDS_SDS.SelectionStart = strText.Length;
        }

        private void textDS_SD1_TextChanged(object sender, EventArgs e)
        {
            string strText = textDS_SD1.Text;
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

            textDS_SD1.Text = strText;
            textDS_SD1.Focus();
            textDS_SD1.SelectionStart = strText.Length;
        }

        private void textDS_TL_TextChanged(object sender, EventArgs e)
        {
            string strText = textDS_TL.Text;
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

            textDS_TL.Text = strText;
            textDS_TL.Focus();
            textDS_TL.SelectionStart = strText.Length;
        }

        private void textDS_Tn_TextChanged(object sender, EventArgs e)
        {
            string strText = textDS_Tn.Text;
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

            textDS_Tn.Text = strText;
            textDS_Tn.Focus();
            textDS_Tn.SelectionStart = strText.Length;
        }

        private void textDS_TA_TextChanged(object sender, EventArgs e)
        {
            string strText = textDS_TA.Text;
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

            textDS_TA.Text = strText;
            textDS_TA.Focus();
            textDS_TA.SelectionStart = strText.Length;
        }

        private void textDS_TB_TextChanged(object sender, EventArgs e)
        {
            string strText = textDS_TB.Text;
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

            textDS_TB.Text = strText;
            textDS_TB.Focus();
            textDS_TB.SelectionStart = strText.Length;            
        }

        private void checkUseTnForSD1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void labelSMD_MotionDatas_Click(object sender, EventArgs e)
        {

        }
    }
}
