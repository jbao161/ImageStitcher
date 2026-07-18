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
            this.button_startupscript = new System.Windows.Forms.Button();
            this.checkBox_scriptwait = new System.Windows.Forms.CheckBox();
            this.checkBox_loadsubfolders = new System.Windows.Forms.CheckBox();
            this.textBox_openinwindows = new System.Windows.Forms.TextBox();
            this.label_openinwindows = new System.Windows.Forms.Label();
            this.label_defaulteditor = new System.Windows.Forms.Label();
            this.textBox_defaulteditor = new System.Windows.Forms.TextBox();
            this.checkBox_bringToFront = new System.Windows.Forms.CheckBox();
            this.checkBox_loadNewFile = new System.Windows.Forms.CheckBox();
            this.textBox_openFolderOnCut = new System.Windows.Forms.TextBox();
            this.label_openFolderOnCut = new System.Windows.Forms.Label();
            this.button_openOnCut = new System.Windows.Forms.Button();
            this.button_openInWindows = new System.Windows.Forms.Button();
            this.button_defaultEditor = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_fastMoveFolder1 = new System.Windows.Forms.TextBox();
            this.FastMoveFolderLabel1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_fastMoveFolder2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox_fastMoveFolder3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox_reversefileorder
            // 
            this.checkBox_reversefileorder.AutoSize = true;
            this.checkBox_reversefileorder.Checked = true;
            this.checkBox_reversefileorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_reversefileorder.Location = new System.Drawing.Point(12, 12);
            this.checkBox_reversefileorder.Name = "checkBox_reversefileorder";
            this.checkBox_reversefileorder.Size = new System.Drawing.Size(114, 17);
            this.checkBox_reversefileorder.TabIndex = 16;
            this.checkBox_reversefileorder.Text = "Reverse File Order";
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
            this.checkBox_defaultdirectory.Location = new System.Drawing.Point(12, 81);
            this.checkBox_defaultdirectory.Name = "checkBox_defaultdirectory";
            this.checkBox_defaultdirectory.Size = new System.Drawing.Size(212, 17);
            this.checkBox_defaultdirectory.TabIndex = 19;
            this.checkBox_defaultdirectory.Text = "Open default directory if no file selected";
            this.checkBox_defaultdirectory.UseVisualStyleBackColor = true;
            // 
            // textBox_defaultdir
            // 
            this.textBox_defaultdir.Location = new System.Drawing.Point(12, 104);
            this.textBox_defaultdir.Name = "textBox_defaultdir";
            this.textBox_defaultdir.Size = new System.Drawing.Size(224, 20);
            this.textBox_defaultdir.TabIndex = 20;
            // 
            // button_browsedir
            // 
            this.button_browsedir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_browsedir.BackgroundImage")));
            this.button_browsedir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_browsedir.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_browsedir.Location = new System.Drawing.Point(242, 93);
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
            this.checkBox_script.Location = new System.Drawing.Point(11, 130);
            this.checkBox_script.Name = "checkBox_script";
            this.checkBox_script.Size = new System.Drawing.Size(132, 17);
            this.checkBox_script.TabIndex = 23;
            this.checkBox_script.Text = "Also load startup script";
            this.checkBox_script.UseVisualStyleBackColor = true;
            // 
            // textBox_scriptloc
            // 
            this.textBox_scriptloc.Location = new System.Drawing.Point(12, 153);
            this.textBox_scriptloc.Name = "textBox_scriptloc";
            this.textBox_scriptloc.Size = new System.Drawing.Size(224, 20);
            this.textBox_scriptloc.TabIndex = 24;
            // 
            // button_startupscript
            // 
            this.button_startupscript.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_startupscript.BackgroundImage")));
            this.button_startupscript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_startupscript.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_startupscript.Location = new System.Drawing.Point(242, 151);
            this.button_startupscript.Name = "button_startupscript";
            this.button_startupscript.Size = new System.Drawing.Size(35, 31);
            this.button_startupscript.TabIndex = 25;
            this.button_startupscript.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_startupscript.UseVisualStyleBackColor = true;
            this.button_startupscript.Click += new System.EventHandler(this.button_script_click);
            // 
            // checkBox_scriptwait
            // 
            this.checkBox_scriptwait.AutoSize = true;
            this.checkBox_scriptwait.Location = new System.Drawing.Point(149, 130);
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
            // textBox_openinwindows
            // 
            this.textBox_openinwindows.Location = new System.Drawing.Point(12, 194);
            this.textBox_openinwindows.Name = "textBox_openinwindows";
            this.textBox_openinwindows.Size = new System.Drawing.Size(224, 20);
            this.textBox_openinwindows.TabIndex = 28;
            // 
            // label_openinwindows
            // 
            this.label_openinwindows.AutoSize = true;
            this.label_openinwindows.Location = new System.Drawing.Point(12, 178);
            this.label_openinwindows.Name = "label_openinwindows";
            this.label_openinwindows.Size = new System.Drawing.Size(122, 13);
            this.label_openinwindows.TabIndex = 29;
            this.label_openinwindows.Text = "Open in Windows with...";
            // 
            // label_defaulteditor
            // 
            this.label_defaulteditor.AutoSize = true;
            this.label_defaulteditor.Location = new System.Drawing.Point(12, 217);
            this.label_defaulteditor.Name = "label_defaulteditor";
            this.label_defaulteditor.Size = new System.Drawing.Size(71, 13);
            this.label_defaulteditor.TabIndex = 30;
            this.label_defaulteditor.Text = "Default Editor";
            // 
            // textBox_defaulteditor
            // 
            this.textBox_defaulteditor.Location = new System.Drawing.Point(11, 233);
            this.textBox_defaulteditor.Name = "textBox_defaulteditor";
            this.textBox_defaulteditor.Size = new System.Drawing.Size(224, 20);
            this.textBox_defaulteditor.TabIndex = 31;
            // 
            // checkBox_bringToFront
            // 
            this.checkBox_bringToFront.AutoSize = true;
            this.checkBox_bringToFront.Location = new System.Drawing.Point(182, 58);
            this.checkBox_bringToFront.Name = "checkBox_bringToFront";
            this.checkBox_bringToFront.Size = new System.Drawing.Size(111, 17);
            this.checkBox_bringToFront.TabIndex = 32;
            this.checkBox_bringToFront.Text = "Show Immediately";
            this.checkBox_bringToFront.UseVisualStyleBackColor = true;
            // 
            // checkBox_loadNewFile
            // 
            this.checkBox_loadNewFile.AutoSize = true;
            this.checkBox_loadNewFile.Location = new System.Drawing.Point(12, 58);
            this.checkBox_loadNewFile.Name = "checkBox_loadNewFile";
            this.checkBox_loadNewFile.Size = new System.Drawing.Size(131, 17);
            this.checkBox_loadNewFile.TabIndex = 33;
            this.checkBox_loadNewFile.Text = "Auto-Update Directory";
            this.checkBox_loadNewFile.UseVisualStyleBackColor = true;
            this.checkBox_loadNewFile.CheckedChanged += new System.EventHandler(this.checkBox_loadNewFile_CheckedChanged);
            // 
            // textBox_openFolderOnCut
            // 
            this.textBox_openFolderOnCut.Location = new System.Drawing.Point(11, 272);
            this.textBox_openFolderOnCut.Name = "textBox_openFolderOnCut";
            this.textBox_openFolderOnCut.Size = new System.Drawing.Size(224, 20);
            this.textBox_openFolderOnCut.TabIndex = 35;
            // 
            // label_openFolderOnCut
            // 
            this.label_openFolderOnCut.AutoSize = true;
            this.label_openFolderOnCut.Location = new System.Drawing.Point(12, 256);
            this.label_openFolderOnCut.Name = "label_openFolderOnCut";
            this.label_openFolderOnCut.Size = new System.Drawing.Size(99, 13);
            this.label_openFolderOnCut.TabIndex = 34;
            this.label_openFolderOnCut.Text = "Open Folder on Cut";
            // 
            // button_openOnCut
            // 
            this.button_openOnCut.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_openOnCut.BackgroundImage")));
            this.button_openOnCut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_openOnCut.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_openOnCut.Location = new System.Drawing.Point(242, 265);
            this.button_openOnCut.Name = "button_openOnCut";
            this.button_openOnCut.Size = new System.Drawing.Size(35, 31);
            this.button_openOnCut.TabIndex = 36;
            this.button_openOnCut.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_openOnCut.UseVisualStyleBackColor = true;
            this.button_openOnCut.Click += new System.EventHandler(this.button_openOnCut_Click);
            // 
            // button_openInWindows
            // 
            this.button_openInWindows.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_openInWindows.BackgroundImage")));
            this.button_openInWindows.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_openInWindows.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_openInWindows.Location = new System.Drawing.Point(242, 188);
            this.button_openInWindows.Name = "button_openInWindows";
            this.button_openInWindows.Size = new System.Drawing.Size(35, 31);
            this.button_openInWindows.TabIndex = 37;
            this.button_openInWindows.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_openInWindows.UseVisualStyleBackColor = true;
            this.button_openInWindows.Click += new System.EventHandler(this.button_openInWindows_Click);
            // 
            // button_defaultEditor
            // 
            this.button_defaultEditor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_defaultEditor.BackgroundImage")));
            this.button_defaultEditor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_defaultEditor.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_defaultEditor.Location = new System.Drawing.Point(242, 228);
            this.button_defaultEditor.Name = "button_defaultEditor";
            this.button_defaultEditor.Size = new System.Drawing.Size(35, 31);
            this.button_defaultEditor.TabIndex = 38;
            this.button_defaultEditor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_defaultEditor.UseVisualStyleBackColor = true;
            this.button_defaultEditor.Click += new System.EventHandler(this.button_defaultEditor_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(242, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(35, 31);
            this.button1.TabIndex = 41;
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button_FastMoveFolder1);
            // 
            // textBox_fastMoveFolder1
            // 
            this.textBox_fastMoveFolder1.Location = new System.Drawing.Point(11, 309);
            this.textBox_fastMoveFolder1.Name = "textBox_fastMoveFolder1";
            this.textBox_fastMoveFolder1.Size = new System.Drawing.Size(224, 20);
            this.textBox_fastMoveFolder1.TabIndex = 40;
            // 
            // FastMoveFolderLabel1
            // 
            this.FastMoveFolderLabel1.AutoSize = true;
            this.FastMoveFolderLabel1.Location = new System.Drawing.Point(12, 293);
            this.FastMoveFolderLabel1.Name = "FastMoveFolderLabel1";
            this.FastMoveFolderLabel1.Size = new System.Drawing.Size(98, 13);
            this.FastMoveFolderLabel1.TabIndex = 39;
            this.FastMoveFolderLabel1.Text = "Fast Move Folder 1";
            // 
            // button2
            // 
            this.button2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button2.BackgroundImage")));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(242, 339);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(35, 31);
            this.button2.TabIndex = 44;
            this.button2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button_FastMoveFolder2);
            // 
            // textBox_fastMoveFolder2
            // 
            this.textBox_fastMoveFolder2.Location = new System.Drawing.Point(11, 346);
            this.textBox_fastMoveFolder2.Name = "textBox_fastMoveFolder2";
            this.textBox_fastMoveFolder2.Size = new System.Drawing.Size(224, 20);
            this.textBox_fastMoveFolder2.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Fast Move Folder 2";
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Font = new System.Drawing.Font("Carlito", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(242, 376);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(35, 31);
            this.button3.TabIndex = 47;
            this.button3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button_FastMoveFolder3);
            // 
            // textBox_fastMoveFolder3
            // 
            this.textBox_fastMoveFolder3.Location = new System.Drawing.Point(11, 383);
            this.textBox_fastMoveFolder3.Name = "textBox_fastMoveFolder3";
            this.textBox_fastMoveFolder3.Size = new System.Drawing.Size(224, 20);
            this.textBox_fastMoveFolder3.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 367);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 45;
            this.label2.Text = "Fast Move Folder 3";
            // 
            // form_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 444);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox_fastMoveFolder3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox_fastMoveFolder2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_fastMoveFolder1);
            this.Controls.Add(this.FastMoveFolderLabel1);
            this.Controls.Add(this.button_defaultEditor);
            this.Controls.Add(this.button_openInWindows);
            this.Controls.Add(this.button_openOnCut);
            this.Controls.Add(this.textBox_openFolderOnCut);
            this.Controls.Add(this.label_openFolderOnCut);
            this.Controls.Add(this.checkBox_loadNewFile);
            this.Controls.Add(this.checkBox_bringToFront);
            this.Controls.Add(this.textBox_defaulteditor);
            this.Controls.Add(this.label_defaulteditor);
            this.Controls.Add(this.label_openinwindows);
            this.Controls.Add(this.textBox_openinwindows);
            this.Controls.Add(this.checkBox_loadsubfolders);
            this.Controls.Add(this.checkBox_scriptwait);
            this.Controls.Add(this.button_startupscript);
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
        private System.Windows.Forms.Button button_startupscript;
        private System.Windows.Forms.CheckBox checkBox_scriptwait;
        private System.Windows.Forms.CheckBox checkBox_loadsubfolders;
        private System.Windows.Forms.TextBox textBox_openinwindows;
        private System.Windows.Forms.Label label_openinwindows;
        private System.Windows.Forms.Label label_defaulteditor;
        private System.Windows.Forms.TextBox textBox_defaulteditor;
        private System.Windows.Forms.CheckBox checkBox_bringToFront;
        private System.Windows.Forms.CheckBox checkBox_loadNewFile;
        private System.Windows.Forms.TextBox textBox_openFolderOnCut;
        private System.Windows.Forms.Label label_openFolderOnCut;
        private System.Windows.Forms.Button button_openOnCut;
        private System.Windows.Forms.Button button_openInWindows;
        private System.Windows.Forms.Button button_defaultEditor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_fastMoveFolder1;
        private System.Windows.Forms.Label FastMoveFolderLabel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_fastMoveFolder2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox_fastMoveFolder3;
        private System.Windows.Forms.Label label2;
    }
}