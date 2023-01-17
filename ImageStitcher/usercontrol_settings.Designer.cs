namespace ImageStitcher
{
    partial class usercontrol_settings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(usercontrol_settings));
            this.button_browsedir = new System.Windows.Forms.Button();
            this.textBox_defaultdir = new System.Windows.Forms.TextBox();
            this.checkBox_defaultdirectory = new System.Windows.Forms.CheckBox();
            this.checkBox_reversefileorder = new System.Windows.Forms.CheckBox();
            this.checkBox_showfilename = new System.Windows.Forms.CheckBox();
            this.checkBox_rememberlastdir = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button_browsedir
            // 
            this.button_browsedir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_browsedir.BackgroundImage")));
            this.button_browsedir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_browsedir.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_browsedir.Location = new System.Drawing.Point(233, 84);
            this.button_browsedir.Name = "button_browsedir";
            this.button_browsedir.Size = new System.Drawing.Size(35, 31);
            this.button_browsedir.TabIndex = 27;
            this.button_browsedir.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_browsedir.UseVisualStyleBackColor = true;
            this.button_browsedir.Click += new System.EventHandler(this.button_browsedir_Click);
            // 
            // textBox_defaultdir
            // 
            this.textBox_defaultdir.Location = new System.Drawing.Point(3, 95);
            this.textBox_defaultdir.Name = "textBox_defaultdir";
            this.textBox_defaultdir.Size = new System.Drawing.Size(224, 20);
            this.textBox_defaultdir.TabIndex = 26;
            // 
            // checkBox_defaultdirectory
            // 
            this.checkBox_defaultdirectory.AutoSize = true;
            this.checkBox_defaultdirectory.Location = new System.Drawing.Point(3, 72);
            this.checkBox_defaultdirectory.Name = "checkBox_defaultdirectory";
            this.checkBox_defaultdirectory.Size = new System.Drawing.Size(130, 17);
            this.checkBox_defaultdirectory.TabIndex = 25;
            this.checkBox_defaultdirectory.Text = "Open default directory";
            this.checkBox_defaultdirectory.UseVisualStyleBackColor = true;
            // 
            // checkBox_reversefileorder
            // 
            this.checkBox_reversefileorder.AutoSize = true;
            this.checkBox_reversefileorder.Checked = true;
            this.checkBox_reversefileorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_reversefileorder.Location = new System.Drawing.Point(3, 3);
            this.checkBox_reversefileorder.Name = "checkBox_reversefileorder";
            this.checkBox_reversefileorder.Size = new System.Drawing.Size(115, 17);
            this.checkBox_reversefileorder.TabIndex = 22;
            this.checkBox_reversefileorder.Text = "Reverse FIle Order";
            this.checkBox_reversefileorder.UseVisualStyleBackColor = true;
            // 
            // checkBox_showfilename
            // 
            this.checkBox_showfilename.AutoSize = true;
            this.checkBox_showfilename.Location = new System.Drawing.Point(3, 49);
            this.checkBox_showfilename.Name = "checkBox_showfilename";
            this.checkBox_showfilename.Size = new System.Drawing.Size(98, 17);
            this.checkBox_showfilename.TabIndex = 23;
            this.checkBox_showfilename.Text = "Show Filename";
            this.checkBox_showfilename.UseVisualStyleBackColor = true;
            // 
            // checkBox_rememberlastdir
            // 
            this.checkBox_rememberlastdir.AutoSize = true;
            this.checkBox_rememberlastdir.Location = new System.Drawing.Point(3, 26);
            this.checkBox_rememberlastdir.Name = "checkBox_rememberlastdir";
            this.checkBox_rememberlastdir.Size = new System.Drawing.Size(141, 17);
            this.checkBox_rememberlastdir.TabIndex = 24;
            this.checkBox_rememberlastdir.Text = "Remember Last Opened";
            this.checkBox_rememberlastdir.UseVisualStyleBackColor = true;
            // 
            // usercontrol_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_browsedir);
            this.Controls.Add(this.textBox_defaultdir);
            this.Controls.Add(this.checkBox_defaultdirectory);
            this.Controls.Add(this.checkBox_reversefileorder);
            this.Controls.Add(this.checkBox_showfilename);
            this.Controls.Add(this.checkBox_rememberlastdir);
            this.Name = "usercontrol_settings";
            this.Size = new System.Drawing.Size(989, 593);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_browsedir;
        private System.Windows.Forms.TextBox textBox_defaultdir;
        private System.Windows.Forms.CheckBox checkBox_defaultdirectory;
        private System.Windows.Forms.CheckBox checkBox_reversefileorder;
        private System.Windows.Forms.CheckBox checkBox_showfilename;
        private System.Windows.Forms.CheckBox checkBox_rememberlastdir;
    }
}
