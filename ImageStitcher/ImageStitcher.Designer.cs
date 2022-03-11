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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.panel_bothimages = new System.Windows.Forms.Panel();
            this.splitContainer_bothimages = new System.Windows.Forms.SplitContainer();
            this.label_imageindex_leftpanel = new System.Windows.Forms.Label();
            this.pictureBox_leftpanel = new System.Windows.Forms.PictureBox();
            this.label_imageindex_rightpanel = new System.Windows.Forms.Label();
            this.pictureBox_rightpanel = new System.Windows.Forms.PictureBox();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.checkBox_randomOnClick = new System.Windows.Forms.CheckBox();
            this.button_verticalhorizontal = new System.Windows.Forms.Button();
            this.checkBox_openaftersave = new System.Windows.Forms.CheckBox();
            this.button_swapimages = new System.Windows.Forms.Button();
            this.button_preview = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.contextMenu_image = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToTrashToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenu_image_item_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu_image_item_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blurBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mirrorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.previousLeftArrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextRightArrowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.randomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_bothimages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bothimages)).BeginInit();
            this.splitContainer_bothimages.Panel1.SuspendLayout();
            this.splitContainer_bothimages.Panel2.SuspendLayout();
            this.splitContainer_bothimages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_leftpanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_rightpanel)).BeginInit();
            this.panel_controls.SuspendLayout();
            this.contextMenu_image.SuspendLayout();
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
            this.panel_bothimages.Size = new System.Drawing.Size(978, 499);
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
            this.splitContainer_bothimages.Panel1.Controls.Add(this.label_imageindex_leftpanel);
            this.splitContainer_bothimages.Panel1.Controls.Add(this.pictureBox_leftpanel);
            // 
            // splitContainer_bothimages.Panel2
            // 
            this.splitContainer_bothimages.Panel2.Controls.Add(this.label_imageindex_rightpanel);
            this.splitContainer_bothimages.Panel2.Controls.Add(this.pictureBox_rightpanel);
            this.splitContainer_bothimages.Size = new System.Drawing.Size(978, 499);
            this.splitContainer_bothimages.SplitterDistance = 344;
            this.splitContainer_bothimages.SplitterWidth = 10;
            this.splitContainer_bothimages.TabIndex = 0;
            // 
            // label_imageindex_leftpanel
            // 
            this.label_imageindex_leftpanel.AutoSize = true;
            this.label_imageindex_leftpanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label_imageindex_leftpanel.Location = new System.Drawing.Point(0, 486);
            this.label_imageindex_leftpanel.Name = "label_imageindex_leftpanel";
            this.label_imageindex_leftpanel.Size = new System.Drawing.Size(134, 13);
            this.label_imageindex_leftpanel.TabIndex = 1;
            this.label_imageindex_leftpanel.Text = "label_imageindex_leftpanel";
            // 
            // pictureBox_leftpanel
            // 
            this.pictureBox_leftpanel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureBox_leftpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_leftpanel.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_leftpanel.Name = "pictureBox_leftpanel";
            this.pictureBox_leftpanel.Size = new System.Drawing.Size(344, 499);
            this.pictureBox_leftpanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_leftpanel.TabIndex = 0;
            this.pictureBox_leftpanel.TabStop = false;
            this.pictureBox_leftpanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBox_DragDrop);
            this.pictureBox_leftpanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBox_DragEnter);
            this.pictureBox_leftpanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Control_MouseClick_copypastemenu);
            // 
            // label_imageindex_rightpanel
            // 
            this.label_imageindex_rightpanel.AutoSize = true;
            this.label_imageindex_rightpanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label_imageindex_rightpanel.Location = new System.Drawing.Point(0, 486);
            this.label_imageindex_rightpanel.Name = "label_imageindex_rightpanel";
            this.label_imageindex_rightpanel.Size = new System.Drawing.Size(140, 13);
            this.label_imageindex_rightpanel.TabIndex = 1;
            this.label_imageindex_rightpanel.Text = "label_imageindex_rightpanel";
            // 
            // pictureBox_rightpanel
            // 
            this.pictureBox_rightpanel.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pictureBox_rightpanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_rightpanel.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_rightpanel.Name = "pictureBox_rightpanel";
            this.pictureBox_rightpanel.Size = new System.Drawing.Size(624, 499);
            this.pictureBox_rightpanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_rightpanel.TabIndex = 0;
            this.pictureBox_rightpanel.TabStop = false;
            this.pictureBox_rightpanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBox_DragDrop);
            this.pictureBox_rightpanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBox_DragEnter);
            this.pictureBox_rightpanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Control_MouseClick_copypastemenu);
            // 
            // panel_controls
            // 
            this.panel_controls.Controls.Add(this.checkBox_randomOnClick);
            this.panel_controls.Controls.Add(this.button_verticalhorizontal);
            this.panel_controls.Controls.Add(this.checkBox_openaftersave);
            this.panel_controls.Controls.Add(this.button_swapimages);
            this.panel_controls.Controls.Add(this.button_preview);
            this.panel_controls.Controls.Add(this.button_save);
            this.panel_controls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_controls.Location = new System.Drawing.Point(0, 505);
            this.panel_controls.MaximumSize = new System.Drawing.Size(10000, 300);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(978, 80);
            this.panel_controls.TabIndex = 7;
            // 
            // checkBox_randomOnClick
            // 
            this.checkBox_randomOnClick.AutoSize = true;
            this.checkBox_randomOnClick.Checked = true;
            this.checkBox_randomOnClick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_randomOnClick.Location = new System.Drawing.Point(25, 17);
            this.checkBox_randomOnClick.Name = "checkBox_randomOnClick";
            this.checkBox_randomOnClick.Size = new System.Drawing.Size(152, 17);
            this.checkBox_randomOnClick.TabIndex = 9;
            this.checkBox_randomOnClick.Text = "Randomize Image on Click";
            this.checkBox_randomOnClick.UseVisualStyleBackColor = true;
            // 
            // button_verticalhorizontal
            // 
            this.button_verticalhorizontal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_verticalhorizontal.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_verticalhorizontal.Location = new System.Drawing.Point(268, 0);
            this.button_verticalhorizontal.Name = "button_verticalhorizontal";
            this.button_verticalhorizontal.Size = new System.Drawing.Size(300, 80);
            this.button_verticalhorizontal.TabIndex = 8;
            this.button_verticalhorizontal.Text = "Stack images vertically";
            this.button_verticalhorizontal.UseVisualStyleBackColor = true;
            this.button_verticalhorizontal.Click += new System.EventHandler(this.Button_verticalhorizontal_Click);
            // 
            // checkBox_openaftersave
            // 
            this.checkBox_openaftersave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_openaftersave.AutoSize = true;
            this.checkBox_openaftersave.Checked = true;
            this.checkBox_openaftersave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_openaftersave.Location = new System.Drawing.Point(839, 60);
            this.checkBox_openaftersave.Name = "checkBox_openaftersave";
            this.checkBox_openaftersave.Size = new System.Drawing.Size(136, 17);
            this.checkBox_openaftersave.TabIndex = 7;
            this.checkBox_openaftersave.Text = "Open Folder after Save";
            this.checkBox_openaftersave.UseVisualStyleBackColor = true;
            // 
            // button_swapimages
            // 
            this.button_swapimages.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_swapimages.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_swapimages.Location = new System.Drawing.Point(568, 0);
            this.button_swapimages.Name = "button_swapimages";
            this.button_swapimages.Size = new System.Drawing.Size(130, 80);
            this.button_swapimages.TabIndex = 5;
            this.button_swapimages.Text = "Swap Images";
            this.button_swapimages.UseVisualStyleBackColor = true;
            this.button_swapimages.Click += new System.EventHandler(this.Button_swapimages_Click);
            // 
            // button_preview
            // 
            this.button_preview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_preview.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_preview.Location = new System.Drawing.Point(698, 0);
            this.button_preview.Name = "button_preview";
            this.button_preview.Size = new System.Drawing.Size(130, 80);
            this.button_preview.TabIndex = 1;
            this.button_preview.Text = "Preview";
            this.button_preview.UseVisualStyleBackColor = true;
            this.button_preview.Click += new System.EventHandler(this.Button_preview_Click);
            // 
            // button_save
            // 
            this.button_save.AutoSize = true;
            this.button_save.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_save.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_save.Location = new System.Drawing.Point(828, 0);
            this.button_save.MinimumSize = new System.Drawing.Size(150, 0);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(150, 80);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.Button_save_Click);
            // 
            // contextMenu_image
            // 
            this.contextMenu_image.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.openFileLocationToolStripMenuItem,
            this.sendToTrashToolStripMenuItem,
            this.toolStripSeparator1,
            this.contextMenu_image_item_copy,
            this.contextMenu_image_item_paste,
            this.toolStripMenuItem1,
            this.rotateToolStripMenuItem,
            this.blurBToolStripMenuItem,
            this.mirrorToolStripMenuItem,
            this.toolStripSeparator3,
            this.clearToolStripMenuItem,
            this.previousLeftArrowToolStripMenuItem,
            this.nextRightArrowToolStripMenuItem,
            this.toolStripSeparator2,
            this.randomToolStripMenuItem,
            this.jumpBackToolStripMenuItem});
            this.contextMenu_image.Name = "contextMenuStrip1";
            this.contextMenu_image.Size = new System.Drawing.Size(186, 314);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.EditToolStripMenuItem_Click);
            // 
            // openFileLocationToolStripMenuItem
            // 
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openFileLocationToolStripMenuItem.Text = "Open File Location";
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.OpenFileLocationToolStripMenuItem_Click);
            // 
            // sendToTrashToolStripMenuItem
            // 
            this.sendToTrashToolStripMenuItem.Name = "sendToTrashToolStripMenuItem";
            this.sendToTrashToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.sendToTrashToolStripMenuItem.Text = "Send to Trash";
            this.sendToTrashToolStripMenuItem.Click += new System.EventHandler(this.SendToTrashToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(182, 6);
            // 
            // contextMenu_image_item_copy
            // 
            this.contextMenu_image_item_copy.Name = "contextMenu_image_item_copy";
            this.contextMenu_image_item_copy.Size = new System.Drawing.Size(185, 22);
            this.contextMenu_image_item_copy.Text = "Copy";
            this.contextMenu_image_item_copy.Click += new System.EventHandler(this.ContextMenu_image_item_copy_Click);
            // 
            // contextMenu_image_item_paste
            // 
            this.contextMenu_image_item_paste.Name = "contextMenu_image_item_paste";
            this.contextMenu_image_item_paste.Size = new System.Drawing.Size(185, 22);
            this.contextMenu_image_item_paste.Text = "Paste";
            this.contextMenu_image_item_paste.Click += new System.EventHandler(this.ContextMenu_image_item_paste_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // rotateToolStripMenuItem
            // 
            this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
            this.rotateToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.rotateToolStripMenuItem.Text = "Rotate (R)";
            this.rotateToolStripMenuItem.Click += new System.EventHandler(this.RotateToolStripMenuItem_Click);
            // 
            // blurBToolStripMenuItem
            // 
            this.blurBToolStripMenuItem.Name = "blurBToolStripMenuItem";
            this.blurBToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.blurBToolStripMenuItem.Text = "Blur (B)";
            this.blurBToolStripMenuItem.Click += new System.EventHandler(this.BlurBToolStripMenuItem_Click);
            // 
            // mirrorToolStripMenuItem
            // 
            this.mirrorToolStripMenuItem.Name = "mirrorToolStripMenuItem";
            this.mirrorToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.mirrorToolStripMenuItem.Text = "Mirror";
            this.mirrorToolStripMenuItem.Click += new System.EventHandler(this.MirrorToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.ClearToolStripMenuItem_Click);
            // 
            // previousLeftArrowToolStripMenuItem
            // 
            this.previousLeftArrowToolStripMenuItem.Name = "previousLeftArrowToolStripMenuItem";
            this.previousLeftArrowToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.previousLeftArrowToolStripMenuItem.Text = "Previous (Left Arrow)";
            this.previousLeftArrowToolStripMenuItem.Click += new System.EventHandler(this.PreviousLeftArrowToolStripMenuItem_Click);
            // 
            // nextRightArrowToolStripMenuItem
            // 
            this.nextRightArrowToolStripMenuItem.Name = "nextRightArrowToolStripMenuItem";
            this.nextRightArrowToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.nextRightArrowToolStripMenuItem.Text = "Next (Right Arrow)";
            this.nextRightArrowToolStripMenuItem.Click += new System.EventHandler(this.NextRightArrowToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(182, 6);
            // 
            // randomToolStripMenuItem
            // 
            this.randomToolStripMenuItem.Name = "randomToolStripMenuItem";
            this.randomToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.randomToolStripMenuItem.Text = "Random";
            this.randomToolStripMenuItem.Click += new System.EventHandler(this.RandomToolStripMenuItem_Click);
            // 
            // jumpBackToolStripMenuItem
            // 
            this.jumpBackToolStripMenuItem.Name = "jumpBackToolStripMenuItem";
            this.jumpBackToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.jumpBackToolStripMenuItem.Text = "Jump Back";
            this.jumpBackToolStripMenuItem.Click += new System.EventHandler(this.JumpBackToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 585);
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.panel_bothimages);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(470, 450);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Image Stitcher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel_bothimages.ResumeLayout(false);
            this.splitContainer_bothimages.Panel1.ResumeLayout(false);
            this.splitContainer_bothimages.Panel1.PerformLayout();
            this.splitContainer_bothimages.Panel2.ResumeLayout(false);
            this.splitContainer_bothimages.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_bothimages)).EndInit();
            this.splitContainer_bothimages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_leftpanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_rightpanel)).EndInit();
            this.panel_controls.ResumeLayout(false);
            this.panel_controls.PerformLayout();
            this.contextMenu_image.ResumeLayout(false);
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
        private System.Windows.Forms.ContextMenuStrip contextMenu_image;
        private System.Windows.Forms.ToolStripMenuItem contextMenu_image_item_copy;
        private System.Windows.Forms.ToolStripMenuItem contextMenu_image_item_paste;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.CheckBox checkBox_openaftersave;
        private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem previousLeftArrowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nextRightArrowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToTrashToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem randomToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpBackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blurBToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Button button_verticalhorizontal;
        private System.Windows.Forms.Button button_swapimages;
        private System.Windows.Forms.CheckBox checkBox_randomOnClick;
        private System.Windows.Forms.ToolStripMenuItem mirrorToolStripMenuItem;
        private System.Windows.Forms.Label label_imageindex_leftpanel;
        private System.Windows.Forms.Label label_imageindex_rightpanel;
    }
}

