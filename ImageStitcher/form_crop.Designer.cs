using System.Drawing;

namespace ImageStitcher
{
    partial class Form_Crop
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
            this.button_crop = new System.Windows.Forms.Button();
            this.button_crop_no = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_accept = new System.Windows.Forms.Button();
            this.button_revert = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.checkBox_overwrite = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button_crop
            // 
            this.button_crop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_crop.BackColor = System.Drawing.Color.Orange;
            this.button_crop.Location = new System.Drawing.Point(318, 15);
            this.button_crop.Name = "button_crop";
            this.button_crop.Size = new System.Drawing.Size(100, 50);
            this.button_crop.TabIndex = 0;
            this.button_crop.Text = "Crop";
            this.button_crop.UseVisualStyleBackColor = false;
            this.button_crop.Click += new System.EventHandler(this.btnCrop_Click);
            // 
            // button_crop_no
            // 
            this.button_crop_no.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_crop_no.BackColor = System.Drawing.Color.Red;
            this.button_crop_no.Location = new System.Drawing.Point(530, 15);
            this.button_crop_no.Name = "button_crop_no";
            this.button_crop_no.Size = new System.Drawing.Size(75, 50);
            this.button_crop_no.TabIndex = 1;
            this.button_crop_no.Text = "Close";
            this.button_crop_no.UseVisualStyleBackColor = false;
            this.button_crop_no.Click += new System.EventHandler(this.button_crop_cancel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(617, 503);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button_crop);
            this.panel1.Controls.Add(this.button_revert);
            this.panel1.Controls.Add(this.checkBox_overwrite);
            this.panel1.Controls.Add(this.button_accept);
            this.panel1.Controls.Add(this.button_crop_no);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 503);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(617, 77);
            this.panel1.TabIndex = 3;
            // 
            // button_accept
            // 
            this.button_accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_accept.BackColor = System.Drawing.Color.LimeGreen;
            this.button_accept.Location = new System.Drawing.Point(424, 15);
            this.button_accept.Name = "button_accept";
            this.button_accept.Size = new System.Drawing.Size(100, 50);
            this.button_accept.TabIndex = 4;
            this.button_accept.Text = "Save File";
            this.button_accept.UseVisualStyleBackColor = false;
            this.button_accept.Click += new System.EventHandler(this.button_accept_Click);
            // 
            // button_revert
            // 
            this.button_revert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_revert.BackColor = System.Drawing.Color.Yellow;
            this.button_revert.Location = new System.Drawing.Point(237, 15);
            this.button_revert.Name = "button_revert";
            this.button_revert.Size = new System.Drawing.Size(75, 50);
            this.button_revert.TabIndex = 3;
            this.button_revert.Text = "Reset";
            this.button_revert.UseVisualStyleBackColor = false;
            this.button_revert.Click += new System.EventHandler(this.button_revert_Click);
            // 
            // panel2
            // 
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(617, 503);
            this.panel2.TabIndex = 4;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(617, 503);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // checkBox_overwrite
            // 
            this.checkBox_overwrite.AutoSize = true;
            this.checkBox_overwrite.Location = new System.Drawing.Point(12, 33);
            this.checkBox_overwrite.Name = "checkBox_overwrite";
            this.checkBox_overwrite.Size = new System.Drawing.Size(107, 17);
            this.checkBox_overwrite.TabIndex = 5;
            this.checkBox_overwrite.Text = "Overwrite original";
            this.checkBox_overwrite.UseVisualStyleBackColor = true;
            // 
            // Form_Crop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 580);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form_Crop";
            this.Text = "Crop image";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Crop_FormClosing);
            this.Load += new System.EventHandler(this.form_crop_load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_crop;
        private System.Windows.Forms.Button button_crop_no;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button_accept;
        private System.Windows.Forms.Button button_revert;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox checkBox_overwrite;
    }
}