using System.Drawing;

namespace ImageStitcher
{
    partial class form_slideshow
    {

        public Point pntLocation;
        private MainWindow mainForm = null;
        public form_slideshow(MainWindow callingForm)
        { // https://stackoverflow.com/questions/1665533/communicate-between-two-windows-forms-in-c-sharp
            mainForm = callingForm as MainWindow;
            InitializeComponent();
        }


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
            this.button_slideStart = new System.Windows.Forms.Button();
            this.radioButton_leftordered = new System.Windows.Forms.RadioButton();
            this.radioButton_leftrandom = new System.Windows.Forms.RadioButton();
            this.label_leftimage = new System.Windows.Forms.Label();
            this.label_rightimage = new System.Windows.Forms.Label();
            this.radioButton_rightrandom = new System.Windows.Forms.RadioButton();
            this.radioButton_rightordered = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_slideEnd = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_slideStart
            // 
            this.button_slideStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_slideStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_slideStart.Location = new System.Drawing.Point(166, 152);
            this.button_slideStart.Name = "button_slideStart";
            this.button_slideStart.Size = new System.Drawing.Size(90, 64);
            this.button_slideStart.TabIndex = 0;
            this.button_slideStart.Text = "Start";
            this.button_slideStart.UseVisualStyleBackColor = false;
            this.button_slideStart.Click += new System.EventHandler(this.button_slideStart_Click);
            // 
            // radioButton_leftordered
            // 
            this.radioButton_leftordered.AutoSize = true;
            this.radioButton_leftordered.Location = new System.Drawing.Point(6, 25);
            this.radioButton_leftordered.Name = "radioButton_leftordered";
            this.radioButton_leftordered.Size = new System.Drawing.Size(63, 17);
            this.radioButton_leftordered.TabIndex = 1;
            this.radioButton_leftordered.Text = "Ordered";
            this.radioButton_leftordered.UseVisualStyleBackColor = true;
            // 
            // radioButton_leftrandom
            // 
            this.radioButton_leftrandom.AutoSize = true;
            this.radioButton_leftrandom.Checked = true;
            this.radioButton_leftrandom.Location = new System.Drawing.Point(6, 48);
            this.radioButton_leftrandom.Name = "radioButton_leftrandom";
            this.radioButton_leftrandom.Size = new System.Drawing.Size(65, 17);
            this.radioButton_leftrandom.TabIndex = 2;
            this.radioButton_leftrandom.TabStop = true;
            this.radioButton_leftrandom.Text = "Random";
            this.radioButton_leftrandom.UseVisualStyleBackColor = true;
            // 
            // label_leftimage
            // 
            this.label_leftimage.AutoSize = true;
            this.label_leftimage.Location = new System.Drawing.Point(3, 0);
            this.label_leftimage.Name = "label_leftimage";
            this.label_leftimage.Size = new System.Drawing.Size(57, 13);
            this.label_leftimage.TabIndex = 5;
            this.label_leftimage.Text = "Left Image";
            // 
            // label_rightimage
            // 
            this.label_rightimage.AutoSize = true;
            this.label_rightimage.Location = new System.Drawing.Point(0, 0);
            this.label_rightimage.Name = "label_rightimage";
            this.label_rightimage.Size = new System.Drawing.Size(64, 13);
            this.label_rightimage.TabIndex = 6;
            this.label_rightimage.Text = "Right Image";
            // 
            // radioButton_rightrandom
            // 
            this.radioButton_rightrandom.AutoSize = true;
            this.radioButton_rightrandom.Checked = true;
            this.radioButton_rightrandom.Location = new System.Drawing.Point(3, 48);
            this.radioButton_rightrandom.Name = "radioButton_rightrandom";
            this.radioButton_rightrandom.Size = new System.Drawing.Size(65, 17);
            this.radioButton_rightrandom.TabIndex = 8;
            this.radioButton_rightrandom.TabStop = true;
            this.radioButton_rightrandom.Text = "Random";
            this.radioButton_rightrandom.UseVisualStyleBackColor = true;
            // 
            // radioButton_rightordered
            // 
            this.radioButton_rightordered.AutoSize = true;
            this.radioButton_rightordered.Location = new System.Drawing.Point(3, 25);
            this.radioButton_rightordered.Name = "radioButton_rightordered";
            this.radioButton_rightordered.Size = new System.Drawing.Size(63, 17);
            this.radioButton_rightordered.TabIndex = 7;
            this.radioButton_rightordered.Text = "Ordered";
            this.radioButton_rightordered.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_leftimage);
            this.panel1.Controls.Add(this.radioButton_leftordered);
            this.panel1.Controls.Add(this.radioButton_leftrandom);
            this.panel1.Location = new System.Drawing.Point(18, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(88, 73);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButton_rightordered);
            this.panel2.Controls.Add(this.label_rightimage);
            this.panel2.Controls.Add(this.radioButton_rightrandom);
            this.panel2.Location = new System.Drawing.Point(171, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(88, 73);
            this.panel2.TabIndex = 10;
            // 
            // button_slideEnd
            // 
            this.button_slideEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_slideEnd.BackColor = System.Drawing.Color.Red;
            this.button_slideEnd.Location = new System.Drawing.Point(11, 152);
            this.button_slideEnd.Name = "button_slideEnd";
            this.button_slideEnd.Size = new System.Drawing.Size(90, 64);
            this.button_slideEnd.TabIndex = 11;
            this.button_slideEnd.Text = "End";
            this.button_slideEnd.UseVisualStyleBackColor = false;
            this.button_slideEnd.Click += new System.EventHandler(this.button_slideEnd_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1 s",
            "2 s",
            "3 s",
            "5 s",
            "10 s",
            "15 s",
            "30 s",
            "1 m",
            "2 m",
            "3 m",
            "5 m",
            "10 m",
            "15 m",
            "20 m",
            "30 m",
            "45 m",
            "1 h"});
            this.comboBox1.Location = new System.Drawing.Point(107, 110);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(115, 21);
            this.comboBox1.TabIndex = 12;
            this.comboBox1.Text = "5 s";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Time interval";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_slideEnd);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.button_slideStart);
            this.panel3.Controls.Add(this.comboBox1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Location = new System.Drawing.Point(12, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(266, 228);
            this.panel3.TabIndex = 14;
            // 
            // form_slideshow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 242);
            this.Controls.Add(this.panel3);
            this.Name = "form_slideshow";
            this.Text = "Slideshow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_slideshow_FormClosing);
            this.Load += new System.EventHandler(this.form_slideshow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_slideStart;
        private System.Windows.Forms.RadioButton radioButton_leftordered;
        private System.Windows.Forms.RadioButton radioButton_leftrandom;
        private System.Windows.Forms.Label label_leftimage;
        private System.Windows.Forms.Label label_rightimage;
        private System.Windows.Forms.RadioButton radioButton_rightrandom;
        private System.Windows.Forms.RadioButton radioButton_rightordered;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_slideEnd;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
    }
}