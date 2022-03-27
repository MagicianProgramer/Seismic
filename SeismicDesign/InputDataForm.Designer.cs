namespace SeismicDesign
{
    partial class InputDataForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textFirstLine = new System.Windows.Forms.TextBox();
            this.textLastLine = new System.Windows.Forms.TextBox();
            this.textTimeStep = new System.Windows.Forms.TextBox();
            this.radioSingleAcceleration = new System.Windows.Forms.RadioButton();
            this.radioTimeAndAcceleration = new System.Windows.Forms.RadioButton();
            this.radioMuitiAcceleration = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnImportData = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Line :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Last Line :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time Step :";
            // 
            // textFirstLine
            // 
            this.textFirstLine.Location = new System.Drawing.Point(69, 12);
            this.textFirstLine.Name = "textFirstLine";
            this.textFirstLine.Size = new System.Drawing.Size(57, 20);
            this.textFirstLine.TabIndex = 3;
            this.textFirstLine.Text = "6";
            this.textFirstLine.TextChanged += new System.EventHandler(this.textFirstLine_TextChanged);
            // 
            // textLastLine
            // 
            this.textLastLine.Location = new System.Drawing.Point(69, 39);
            this.textLastLine.Name = "textLastLine";
            this.textLastLine.Size = new System.Drawing.Size(57, 20);
            this.textLastLine.TabIndex = 4;
            this.textLastLine.Text = "1000";
            this.textLastLine.TextChanged += new System.EventHandler(this.textLastLine_TextChanged);
            // 
            // textTimeStep
            // 
            this.textTimeStep.Location = new System.Drawing.Point(69, 66);
            this.textTimeStep.Name = "textTimeStep";
            this.textTimeStep.Size = new System.Drawing.Size(57, 20);
            this.textTimeStep.TabIndex = 5;
            this.textTimeStep.Text = "0.01";
            this.textTimeStep.TextChanged += new System.EventHandler(this.textTimeStep_TextChanged);
            // 
            // radioSingleAcceleration
            // 
            this.radioSingleAcceleration.AutoSize = true;
            this.radioSingleAcceleration.Location = new System.Drawing.Point(11, 12);
            this.radioSingleAcceleration.Name = "radioSingleAcceleration";
            this.radioSingleAcceleration.Size = new System.Drawing.Size(188, 17);
            this.radioSingleAcceleration.TabIndex = 6;
            this.radioSingleAcceleration.Text = "Single Acceleration Value Per Line";
            this.radioSingleAcceleration.UseVisualStyleBackColor = true;
            this.radioSingleAcceleration.CheckedChanged += new System.EventHandler(this.radioSingleAcceleration_CheckedChanged);
            // 
            // radioTimeAndAcceleration
            // 
            this.radioTimeAndAcceleration.AutoSize = true;
            this.radioTimeAndAcceleration.Location = new System.Drawing.Point(11, 36);
            this.radioTimeAndAcceleration.Name = "radioTimeAndAcceleration";
            this.radioTimeAndAcceleration.Size = new System.Drawing.Size(204, 17);
            this.radioTimeAndAcceleration.TabIndex = 7;
            this.radioTimeAndAcceleration.Text = "Time And Acceleration Value Per Line";
            this.radioTimeAndAcceleration.UseVisualStyleBackColor = true;
            this.radioTimeAndAcceleration.CheckedChanged += new System.EventHandler(this.radioTimeAndAcceleration_CheckedChanged);
            // 
            // radioMuitiAcceleration
            // 
            this.radioMuitiAcceleration.AutoSize = true;
            this.radioMuitiAcceleration.Location = new System.Drawing.Point(11, 60);
            this.radioMuitiAcceleration.Name = "radioMuitiAcceleration";
            this.radioMuitiAcceleration.Size = new System.Drawing.Size(200, 17);
            this.radioMuitiAcceleration.TabIndex = 8;
            this.radioMuitiAcceleration.Text = "Multiple Acceleration Values Per Line";
            this.radioMuitiAcceleration.UseVisualStyleBackColor = true;
            this.radioMuitiAcceleration.CheckedChanged += new System.EventHandler(this.radioMuitiAcceleration_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(5, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(417, 334);
            this.panel1.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textTimeStep);
            this.groupBox1.Controls.Add(this.textLastLine);
            this.groupBox1.Controls.Add(this.textFirstLine);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(7, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 92);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioMuitiAcceleration);
            this.groupBox2.Controls.Add(this.radioTimeAndAcceleration);
            this.groupBox2.Controls.Add(this.radioSingleAcceleration);
            this.groupBox2.Location = new System.Drawing.Point(155, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(266, 92);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            // 
            // btnImportData
            // 
            this.btnImportData.Location = new System.Drawing.Point(5, 438);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(202, 35);
            this.btnImportData.TabIndex = 14;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(219, 438);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(202, 35);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(-1, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(414, 330);
            this.textBox1.TabIndex = 11;
            // 
            // InputDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 478);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImportData);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "InputDataForm";
            this.Text = "Input Data";
            this.Load += new System.EventHandler(this.InputDataForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textFirstLine;
        private System.Windows.Forms.TextBox textLastLine;
        private System.Windows.Forms.TextBox textTimeStep;
        private System.Windows.Forms.RadioButton radioSingleAcceleration;
        private System.Windows.Forms.RadioButton radioTimeAndAcceleration;
        private System.Windows.Forms.RadioButton radioMuitiAcceleration;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox textBox1;
    }
}