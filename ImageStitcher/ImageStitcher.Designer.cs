namespace ImageStitcher
{
    partial class MainWindow
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
            this.panel_bothimages = new System.Windows.Forms.Panel();
            this.splitContainer_bothimages = new System.Windows.Forms.SplitContainer();
            this.pictureBox_leftpanel = new System.Windows.Forms.PictureBox();
            this.pictureBox_rightpanel = new System.Windows.Forms.PictureBox();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.button_preview = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel_bothimages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bothimages)).BeginInit();
            this.splitContainer_bothimages.Panel1.SuspendLayout();
            this.splitContainer_bothimages.Panel2.SuspendLayout();
            this.splitContainer_bothimages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_leftpanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_rightpanel)).BeginInit();
            this.panel_controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_bothimages
            // 
            this.panel_bothimages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_bothimages.Controls.Add(this.splitContainer_bothimages);
            this.panel_bothimages.Location = new System.Drawing.Point(0, 0);
            this.panel_bothimages.Name = "panel_bothimages";
            this.panel_bothimages.Size = new System.Drawing.Size(853, 615);
            this.panel_bothimages.TabIndex = 0;
            // 
            // splitContainer_bothimages
            // 
            this.splitContainer_bothimages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_bothimages.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_bothimages.Name = "splitContainer_bothimages";
            // 
            // splitContainer_bothimages.Panel1
            // 
            this.splitContainer_bothimages.Panel1.Controls.Add(this.pictureBox_leftpanel);
            // 
            // splitContainer_bothimages.Panel2
            // 
            this.splitContainer_bothimages.Panel2.Controls.Add(this.pictureBox_rightpanel);
            this.splitContainer_bothimages.Size = new System.Drawing.Size(853, 615);
            this.splitContainer_bothimages.SplitterDistance = 303;
            this.splitContainer_bothimages.SplitterWidth = 10;
            this.splitContainer_bothimages.TabIndex = 0;
            // 
            // pictureBox_leftpanel
            // 
            this.pictureBox_leftpanel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureBox_leftpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_leftpanel.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_leftpanel.Name = "pictureBox_leftpanel";
            this.pictureBox_leftpanel.Size = new System.Drawing.Size(303, 615);
            this.pictureBox_leftpanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_leftpanel.TabIndex = 0;
            this.pictureBox_leftpanel.TabStop = false;
            this.pictureBox_leftpanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragDrop);
            this.pictureBox_leftpanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragEnter);
            // 
            // pictureBox_rightpanel
            // 
            this.pictureBox_rightpanel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureBox_rightpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_rightpanel.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_rightpanel.Name = "pictureBox_rightpanel";
            this.pictureBox_rightpanel.Size = new System.Drawing.Size(540, 615);
            this.pictureBox_rightpanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_rightpanel.TabIndex = 0;
            this.pictureBox_rightpanel.TabStop = false;
            this.pictureBox_rightpanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragDrop);
            this.pictureBox_rightpanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.pictureBox_DragEnter);
            // 
            // panel_controls
            // 
            this.panel_controls.Controls.Add(this.button_preview);
            this.panel_controls.Controls.Add(this.button_save);
            this.panel_controls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_controls.Location = new System.Drawing.Point(0, 621);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(853, 122);
            this.panel_controls.TabIndex = 1;
            // 
            // button_preview
            // 
            this.button_preview.Location = new System.Drawing.Point(535, 1);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(159, 120);
            this.button_preview.TabIndex = 2;
            this.button_preview.Text = "Preview";
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.button_preview_Click);
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(700, 3);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(150, 116);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 743);
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.panel_bothimages);
            this.Name = "MainWindow";
            this.Text = "Image Stitcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel_bothimages.ResumeLayout(false);
            this.splitContainer_bothimages.Panel1.ResumeLayout(false);
            this.splitContainer_bothimages.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bothimages)).EndInit();
            this.splitContainer_bothimages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_leftpanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_rightpanel)).EndInit();
            this.panel_controls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_bothimages;
        private System.Windows.Forms.Panel panel_controls;
        private System.Windows.Forms.SplitContainer splitContainer_bothimages;
        private System.Windows.Forms.PictureBox pictureBox_leftpanel;
        private System.Windows.Forms.PictureBox pictureBox_rightpanel;
        private System.Windows.Forms.Button button_preview;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

