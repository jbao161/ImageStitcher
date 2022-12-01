using System.Drawing;

namespace ImageStitcher
{
    partial class Form_Crop
    {
        public Point pntLocation;
        private MainWindow mainForm = null;
        public Form_Crop(MainWindow callingForm)
        { // https://stackoverflow.com/questions/1665533/communicate-between-two-windows-forms-in-c-sharp
            mainForm = callingForm as MainWindow;
            InitializeComponent();
        }
        public Image source_image;
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
            this.button_crop_yes = new System.Windows.Forms.Button();
            this.button_crop_no = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_crop_yes
            // 
            this.button_crop_yes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_crop_yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.button_crop_yes.Location = new System.Drawing.Point(564, 15);
            this.button_crop_yes.Name = "button_crop_yes";
            this.button_crop_yes.Size = new System.Drawing.Size(50, 50);
            this.button_crop_yes.TabIndex = 0;
            this.button_crop_yes.Text = "Crop";
            this.button_crop_yes.UseVisualStyleBackColor = false;
            this.button_crop_yes.Click += new System.EventHandler(this.Button_Crop_Yes_Click);
            // 
            // button_crop_no
            // 
            this.button_crop_no.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_crop_no.BackColor = System.Drawing.Color.Red;
            this.button_crop_no.Location = new System.Drawing.Point(508, 15);
            this.button_crop_no.Name = "button_crop_no";
            this.button_crop_no.Size = new System.Drawing.Size(50, 50);
            this.button_crop_no.TabIndex = 1;
            this.button_crop_no.Text = "Cancel";
            this.button_crop_no.UseVisualStyleBackColor = false;
            this.button_crop_no.Click += new System.EventHandler(this.button_crop_no_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(617, 503);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_crop_no);
            this.panel1.Controls.Add(this.button_crop_yes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 503);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 77);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(617, 503);
            this.panel2.TabIndex = 4;
            // 
            // Form_Crop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 580);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form_Crop";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_crop_yes;
        private System.Windows.Forms.Button button_crop_no;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}