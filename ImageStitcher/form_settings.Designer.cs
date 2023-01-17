using System.Drawing;

namespace ImageStitcher
{
    partial class form_settings
    {
        public Point pntLocation;
        private MainWindow mainForm = null;
        public form_settings(MainWindow callingForm)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_settings));
            this.checkBox_reversefileorder = new System.Windows.Forms.CheckBox();
            this.checkBox_rememberlastfile = new System.Windows.Forms.CheckBox();
            this.checkBox_defaultdirectory = new System.Windows.Forms.CheckBox();
            this.textBox_defaultdir = new System.Windows.Forms.TextBox();
            this.button_browsedir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox_reversefileorder
            // 
            this.checkBox_reversefileorder.AutoSize = true;
            this.checkBox_reversefileorder.Checked = true;
            this.checkBox_reversefileorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_reversefileorder.Location = new System.Drawing.Point(12, 12);
            this.checkBox_reversefileorder.Name = "checkBox_reversefileorder";
            this.checkBox_reversefileorder.Size = new System.Drawing.Size(115, 17);
            this.checkBox_reversefileorder.TabIndex = 16;
            this.checkBox_reversefileorder.Text = "Reverse FIle Order";
            this.checkBox_reversefileorder.UseVisualStyleBackColor = true;
            // 
            // checkBox_rememberlastfile
            // 
            this.checkBox_rememberlastfile.AutoSize = true;
            this.checkBox_rememberlastfile.Location = new System.Drawing.Point(12, 35);
            this.checkBox_rememberlastfile.Name = "checkBox_rememberlastfile";
            this.checkBox_rememberlastfile.Size = new System.Drawing.Size(141, 17);
            this.checkBox_rememberlastfile.TabIndex = 18;
            this.checkBox_rememberlastfile.Text = "Remember Last Opened";
            this.checkBox_rememberlastfile.UseVisualStyleBackColor = true;
            // 
            // checkBox_defaultdirectory
            // 
            this.checkBox_defaultdirectory.AutoSize = true;
            this.checkBox_defaultdirectory.Location = new System.Drawing.Point(12, 58);
            this.checkBox_defaultdirectory.Name = "checkBox_defaultdirectory";
            this.checkBox_defaultdirectory.Size = new System.Drawing.Size(130, 17);
            this.checkBox_defaultdirectory.TabIndex = 19;
            this.checkBox_defaultdirectory.Text = "Open default directory";
            this.checkBox_defaultdirectory.UseVisualStyleBackColor = true;
            // 
            // textBox_defaultdir
            // 
            this.textBox_defaultdir.Location = new System.Drawing.Point(12, 81);
            this.textBox_defaultdir.Name = "textBox_defaultdir";
            this.textBox_defaultdir.Size = new System.Drawing.Size(224, 20);
            this.textBox_defaultdir.TabIndex = 20;
            // 
            // button_browsedir
            // 
            this.button_browsedir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_browsedir.BackgroundImage")));
            this.button_browsedir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_browsedir.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_browsedir.Location = new System.Drawing.Point(242, 70);
            this.button_browsedir.Name = "button_browsedir";
            this.button_browsedir.Size = new System.Drawing.Size(35, 31);
            this.button_browsedir.TabIndex = 21;
            this.button_browsedir.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_browsedir.UseVisualStyleBackColor = true;
            this.button_browsedir.Click += new System.EventHandler(this.button_browsedir_Click);
            // 
            // form_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 139);
            this.Controls.Add(this.button_browsedir);
            this.Controls.Add(this.textBox_defaultdir);
            this.Controls.Add(this.checkBox_defaultdirectory);
            this.Controls.Add(this.checkBox_reversefileorder);
            this.Controls.Add(this.checkBox_rememberlastfile);
            this.Name = "form_settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.form_settings_FormClosing);
            this.Load += new System.EventHandler(this.form_settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_reversefileorder;
        private System.Windows.Forms.CheckBox checkBox_rememberlastfile;
        private System.Windows.Forms.CheckBox checkBox_defaultdirectory;
        private System.Windows.Forms.TextBox textBox_defaultdir;
        private System.Windows.Forms.Button button_browsedir;
    }
}