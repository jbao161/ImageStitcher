using System.Drawing;

namespace ImageStitcher
{
    partial class form_settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(form_settings));
            this.checkBox_reversefileorder = new System.Windows.Forms.CheckBox();
            this.checkBox_rememberlastfile = new System.Windows.Forms.CheckBox();
            this.checkBox_defaultdirectory = new System.Windows.Forms.CheckBox();
            this.textBox_defaultdir = new System.Windows.Forms.TextBox();
            this.button_browsedir = new System.Windows.Forms.Button();
            this.checkBox_darkskin = new System.Windows.Forms.CheckBox();
            this.checkBox_script = new System.Windows.Forms.CheckBox();
            this.textBox_scriptloc = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_scriptwait = new System.Windows.Forms.CheckBox();
            this.checkBox_loadsubfolders = new System.Windows.Forms.CheckBox();
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
            this.checkBox_defaultdirectory.Size = new System.Drawing.Size(212, 17);
            this.checkBox_defaultdirectory.TabIndex = 19;
            this.checkBox_defaultdirectory.Text = "Open default directory if no file selected";
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
            // checkBox_darkskin
            // 
            this.checkBox_darkskin.AutoSize = true;
            this.checkBox_darkskin.Location = new System.Drawing.Point(182, 16);
            this.checkBox_darkskin.Name = "checkBox_darkskin";
            this.checkBox_darkskin.Size = new System.Drawing.Size(78, 17);
            this.checkBox_darkskin.TabIndex = 22;
            this.checkBox_darkskin.Text = "Dark mode";
            this.checkBox_darkskin.UseVisualStyleBackColor = true;
            this.checkBox_darkskin.CheckedChanged += new System.EventHandler(this.checkBox_darkskin_CheckedChanged);
            // 
            // checkBox_script
            // 
            this.checkBox_script.AutoSize = true;
            this.checkBox_script.Location = new System.Drawing.Point(11, 107);
            this.checkBox_script.Name = "checkBox_script";
            this.checkBox_script.Size = new System.Drawing.Size(132, 17);
            this.checkBox_script.TabIndex = 23;
            this.checkBox_script.Text = "Also load startup script";
            this.checkBox_script.UseVisualStyleBackColor = true;
            // 
            // textBox_scriptloc
            // 
            this.textBox_scriptloc.Location = new System.Drawing.Point(12, 130);
            this.textBox_scriptloc.Name = "textBox_scriptloc";
            this.textBox_scriptloc.Size = new System.Drawing.Size(224, 20);
            this.textBox_scriptloc.TabIndex = 24;
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(242, 119);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 31);
            this.button1.TabIndex = 25;
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_script_click);
            // 
            // checkBox_scriptwait
            // 
            this.checkBox_scriptwait.AutoSize = true;
            this.checkBox_scriptwait.Location = new System.Drawing.Point(149, 107);
            this.checkBox_scriptwait.Name = "checkBox_scriptwait";
            this.checkBox_scriptwait.Size = new System.Drawing.Size(124, 17);
            this.checkBox_scriptwait.TabIndex = 26;
            this.checkBox_scriptwait.Text = "Wait until script ends";
            this.checkBox_scriptwait.UseVisualStyleBackColor = true;
            // 
            // checkBox_loadsubfolders
            // 
            this.checkBox_loadsubfolders.AutoSize = true;
            this.checkBox_loadsubfolders.Checked = true;
            this.checkBox_loadsubfolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_loadsubfolders.Location = new System.Drawing.Point(182, 35);
            this.checkBox_loadsubfolders.Name = "checkBox_loadsubfolders";
            this.checkBox_loadsubfolders.Size = new System.Drawing.Size(101, 17);
            this.checkBox_loadsubfolders.TabIndex = 27;
            this.checkBox_loadsubfolders.Text = "Load subfolders";
            this.checkBox_loadsubfolders.UseVisualStyleBackColor = true;
            this.checkBox_loadsubfolders.CheckedChanged += new System.EventHandler(this.checkBox_loadsubfolders_CheckedChanged);
            // 
            // form_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 203);
            this.Controls.Add(this.checkBox_loadsubfolders);
            this.Controls.Add(this.checkBox_scriptwait);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_scriptloc);
            this.Controls.Add(this.checkBox_script);
            this.Controls.Add(this.checkBox_darkskin);
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
        private System.Windows.Forms.CheckBox checkBox_darkskin;
        private System.Windows.Forms.CheckBox checkBox_script;
        private System.Windows.Forms.TextBox textBox_scriptloc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_scriptwait;
        private System.Windows.Forms.CheckBox checkBox_loadsubfolders;
    }
}