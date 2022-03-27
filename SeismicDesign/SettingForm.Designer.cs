namespace SeismicDesign
{
    partial class SettingForm
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.radioComaSymbol = new System.Windows.Forms.RadioButton();
            this.radioPointSymbol = new System.Windows.Forms.RadioButton();
            this.btnLineWidth = new System.Windows.Forms.Button();
            this.btnGraphColor = new System.Windows.Forms.Button();
            this.btnLightMode = new System.Windows.Forms.Button();
            this.btnDarkMode = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.radioValue = new System.Windows.Forms.RadioButton();
            this.radioRatio = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.textAccelerationVFBD = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textPGARatioForBracktedDuration = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textPGARatioForNOEC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textDefaultDampingRatio = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textNumberDecimals = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.textSDSCoefficient = new System.Windows.Forms.TextBox();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.ItemSize = new System.Drawing.Size(196, 30);
            this.tabControlMain.Location = new System.Drawing.Point(2, 36);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(395, 192);
            this.tabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.radioComaSymbol);
            this.tabPage1.Controls.Add(this.radioPointSymbol);
            this.tabPage1.Controls.Add(this.btnLineWidth);
            this.tabPage1.Controls.Add(this.btnGraphColor);
            this.tabPage1.Controls.Add(this.btnLightMode);
            this.tabPage1.Controls.Add(this.btnDarkMode);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(387, 142);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Visualization";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Decimal Seperator";
            // 
            // radioComaSymbol
            // 
            this.radioComaSymbol.AutoSize = true;
            this.radioComaSymbol.Location = new System.Drawing.Point(268, 115);
            this.radioComaSymbol.Name = "radioComaSymbol";
            this.radioComaSymbol.Size = new System.Drawing.Size(111, 17);
            this.radioComaSymbol.TabIndex = 6;
            this.radioComaSymbol.TabStop = true;
            this.radioComaSymbol.Text = "\",\" (Coma Symbol)";
            this.radioComaSymbol.UseVisualStyleBackColor = true;
            this.radioComaSymbol.CheckedChanged += new System.EventHandler(this.radioComaSymbol_CheckedChanged);
            // 
            // radioPointSymbol
            // 
            this.radioPointSymbol.AutoSize = true;
            this.radioPointSymbol.Checked = true;
            this.radioPointSymbol.Location = new System.Drawing.Point(147, 115);
            this.radioPointSymbol.Name = "radioPointSymbol";
            this.radioPointSymbol.Size = new System.Drawing.Size(111, 17);
            this.radioPointSymbol.TabIndex = 5;
            this.radioPointSymbol.TabStop = true;
            this.radioPointSymbol.Text = "\".\" (Point Symbol) ";
            this.radioPointSymbol.UseVisualStyleBackColor = true;
            this.radioPointSymbol.CheckedChanged += new System.EventHandler(this.radioPointSymbol_CheckedChanged);
            // 
            // btnLineWidth
            // 
            this.btnLineWidth.Location = new System.Drawing.Point(1, 69);
            this.btnLineWidth.Name = "btnLineWidth";
            this.btnLineWidth.Size = new System.Drawing.Size(385, 35);
            this.btnLineWidth.TabIndex = 3;
            this.btnLineWidth.Text = "Graph Line Width Change data";
            this.btnLineWidth.UseVisualStyleBackColor = true;
            this.btnLineWidth.Click += new System.EventHandler(this.btnLineWidth_Click);
            // 
            // btnGraphColor
            // 
            this.btnGraphColor.Location = new System.Drawing.Point(1, 35);
            this.btnGraphColor.Name = "btnGraphColor";
            this.btnGraphColor.Size = new System.Drawing.Size(385, 35);
            this.btnGraphColor.TabIndex = 2;
            this.btnGraphColor.Text = "Graph Color Change for 11 data";
            this.btnGraphColor.UseVisualStyleBackColor = true;
            this.btnGraphColor.Click += new System.EventHandler(this.btnGraphColor_Click);
            // 
            // btnLightMode
            // 
            this.btnLightMode.Location = new System.Drawing.Point(193, 1);
            this.btnLightMode.Name = "btnLightMode";
            this.btnLightMode.Size = new System.Drawing.Size(193, 35);
            this.btnLightMode.TabIndex = 1;
            this.btnLightMode.Text = "Light Mode";
            this.btnLightMode.UseVisualStyleBackColor = true;
            this.btnLightMode.Click += new System.EventHandler(this.btnLightMode_Click);
            // 
            // btnDarkMode
            // 
            this.btnDarkMode.Location = new System.Drawing.Point(1, 1);
            this.btnDarkMode.Name = "btnDarkMode";
            this.btnDarkMode.Size = new System.Drawing.Size(193, 35);
            this.btnDarkMode.TabIndex = 0;
            this.btnDarkMode.Text = "Dark Mode";
            this.btnDarkMode.UseVisualStyleBackColor = true;
            this.btnDarkMode.Click += new System.EventHandler(this.btnDarkMode_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.textSDSCoefficient);
            this.tabPage2.Controls.Add(this.radioValue);
            this.tabPage2.Controls.Add(this.radioRatio);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.textAccelerationVFBD);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.textPGARatioForBracktedDuration);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.textPGARatioForNOEC);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textDefaultDampingRatio);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.textNumberDecimals);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(387, 154);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Analysis";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // radioValue
            // 
            this.radioValue.AutoSize = true;
            this.radioValue.Location = new System.Drawing.Point(289, 101);
            this.radioValue.Name = "radioValue";
            this.radioValue.Size = new System.Drawing.Size(14, 13);
            this.radioValue.TabIndex = 11;
            this.radioValue.TabStop = true;
            this.radioValue.UseVisualStyleBackColor = true;
            this.radioValue.CheckedChanged += new System.EventHandler(this.radioValue_CheckedChanged);
            // 
            // radioRatio
            // 
            this.radioRatio.AutoSize = true;
            this.radioRatio.Checked = true;
            this.radioRatio.Location = new System.Drawing.Point(289, 78);
            this.radioRatio.Name = "radioRatio";
            this.radioRatio.Size = new System.Drawing.Size(14, 13);
            this.radioRatio.TabIndex = 10;
            this.radioRatio.TabStop = true;
            this.radioRatio.UseVisualStyleBackColor = true;
            this.radioRatio.CheckedChanged += new System.EventHandler(this.radioRatio_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(221, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Acceleration Value For Bracketed Duration(g)";
            // 
            // textAccelerationVFBD
            // 
            this.textAccelerationVFBD.Location = new System.Drawing.Point(309, 97);
            this.textAccelerationVFBD.Name = "textAccelerationVFBD";
            this.textAccelerationVFBD.Size = new System.Drawing.Size(72, 20);
            this.textAccelerationVFBD.TabIndex = 8;
            this.textAccelerationVFBD.Text = "0.05";
            this.textAccelerationVFBD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textAccelerationVFBD.TextChanged += new System.EventHandler(this.textAccelerationVFBD_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "PGA Ratio For Bracketed Duration(%)";
            // 
            // textPGARatioForBracktedDuration
            // 
            this.textPGARatioForBracktedDuration.Location = new System.Drawing.Point(309, 74);
            this.textPGARatioForBracktedDuration.Name = "textPGARatioForBracktedDuration";
            this.textPGARatioForBracktedDuration.Size = new System.Drawing.Size(72, 20);
            this.textPGARatioForBracktedDuration.TabIndex = 6;
            this.textPGARatioForBracktedDuration.Text = "5";
            this.textPGARatioForBracktedDuration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textPGARatioForBracktedDuration.TextChanged += new System.EventHandler(this.textPGARatioForBracktedDuration_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(203, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "PGA Ratio For Num. of Effective Cycle(%)";
            // 
            // textPGARatioForNOEC
            // 
            this.textPGARatioForNOEC.Location = new System.Drawing.Point(309, 51);
            this.textPGARatioForNOEC.Name = "textPGARatioForNOEC";
            this.textPGARatioForNOEC.Size = new System.Drawing.Size(72, 20);
            this.textPGARatioForNOEC.TabIndex = 4;
            this.textPGARatioForNOEC.Text = "65";
            this.textPGARatioForNOEC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textPGARatioForNOEC.TextChanged += new System.EventHandler(this.textPGARatioForNOEC_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Default Damping Ratio (%)";
            // 
            // textDefaultDampingRatio
            // 
            this.textDefaultDampingRatio.Location = new System.Drawing.Point(309, 28);
            this.textDefaultDampingRatio.Name = "textDefaultDampingRatio";
            this.textDefaultDampingRatio.Size = new System.Drawing.Size(72, 20);
            this.textDefaultDampingRatio.TabIndex = 2;
            this.textDefaultDampingRatio.Text = "5";
            this.textDefaultDampingRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textDefaultDampingRatio.TextChanged += new System.EventHandler(this.textDefaultDampingRatio_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Number of Decimals";
            // 
            // textNumberDecimals
            // 
            this.textNumberDecimals.Location = new System.Drawing.Point(309, 5);
            this.textNumberDecimals.Name = "textNumberDecimals";
            this.textNumberDecimals.Size = new System.Drawing.Size(72, 20);
            this.textNumberDecimals.TabIndex = 0;
            this.textNumberDecimals.Text = "5";
            this.textNumberDecimals.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textNumberDecimals.TextChanged += new System.EventHandler(this.textNumberDecimals_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(159, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Settings";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(101, 234);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(225, 234);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(183, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Design Spectrum SDS Coefficient (%)";
            // 
            // textSDSCoefficient
            // 
            this.textSDSCoefficient.Location = new System.Drawing.Point(309, 125);
            this.textSDSCoefficient.Name = "textSDSCoefficient";
            this.textSDSCoefficient.Size = new System.Drawing.Size(72, 20);
            this.textSDSCoefficient.TabIndex = 12;
            this.textSDSCoefficient.Text = "90";
            this.textSDSCoefficient.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(202)))), ((int)(((byte)(202)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(400, 273);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControlMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SettingForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SettingForm_MouseUp);
            this.tabControlMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioComaSymbol;
        private System.Windows.Forms.RadioButton radioPointSymbol;
        private System.Windows.Forms.Button btnLineWidth;
        private System.Windows.Forms.Button btnGraphColor;
        private System.Windows.Forms.Button btnLightMode;
        private System.Windows.Forms.Button btnDarkMode;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textAccelerationVFBD;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textPGARatioForBracktedDuration;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textPGARatioForNOEC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textDefaultDampingRatio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textNumberDecimals;
        private System.Windows.Forms.RadioButton radioValue;
        private System.Windows.Forms.RadioButton radioRatio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textSDSCoefficient;
    }
}