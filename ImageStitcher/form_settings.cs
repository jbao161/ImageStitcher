using ImageStitcher.Properties;
using System;
using System.Diagnostics;
using System.Drawing;

using System.Windows.Forms;

namespace ImageStitcher
{
    public partial class form_settings : Form
    {
        public form_settings()
        {
            InitializeComponent();
        }

        public Point pntLocation;
        private MainWindow mainForm = null;

        public form_settings(MainWindow callingForm)
        { // https://stackoverflow.com/questions/1665533/communicate-between-two-windows-forms-in-c-sharp
            mainForm = callingForm as MainWindow;
            InitializeComponent();
        }

        private void button_browsedir_Click(object sender, EventArgs e)
        {
            chooseFileOrFolder(textBox_defaultdir, false);
        }

        private void CheckBounds()
        { //https://social.msdn.microsoft.com/Forums/vstudio/en-US/aa5e617a-6955-47f5-8b8b-6839f38944ba/how-to-restrict-a-window-move-and-grow-within-screen-in-wpf?forum=wpf
            var height = System.Windows.SystemParameters.PrimaryScreenHeight;
            var width = System.Windows.SystemParameters.PrimaryScreenWidth;

            if (this.Left < 0)
                this.Left = 0;
            if (this.Top < 0)
                this.Top = 0;
            if (this.Top + this.Height > height)
                this.Top = (int)(height - this.Height);
            if (this.Left + this.Width > width)
                this.Left = (int)(width - this.Width);
        }

        private void form_settings_Load(object sender, EventArgs e)
        {
            pntLocation.X -= this.Size.Width / 2;
            pntLocation.Y -= this.Size.Height;
            this.Location = pntLocation;
            System.Drawing.Point topleft = this.PointToScreen(new System.Drawing.Point(this.Left, this.Top));
            CheckBounds();
            checkBox_reversefileorder.Checked = Settings.Default.ReverseFileOrder;
            checkBox_rememberlastfile.Checked = Settings.Default.rememberLastFile;
            checkBox_defaultdirectory.Checked = Settings.Default.loaddefaultdir;
            textBox_defaultdir.Text = Settings.Default.DefaultDirectory;
            checkBox_darkskin.Checked = Settings.Default.DarkMode;
            checkBox_script.Checked = Settings.Default.scriptonload;
            textBox_scriptloc.Text = Settings.Default.scriptloc;
            checkBox_scriptwait.Checked = Settings.Default.scriptwait;
            checkBox_loadsubfolders.Checked = Settings.Default.LoadSubfolders;
            textBox_openinwindows.Text = Settings.Default.DefaultWindowsOpen;
            textBox_defaulteditor.Text = Settings.Default.DefaultEditor;
            checkBox_bringToFront.Checked = Settings.Default.bringToFront;
            checkBox_loadNewFile.Checked = Settings.Default.loadNewFile;
            textBox_openFolderOnCut.Text = Settings.Default.openFolderOnCut;
        }

        private void form_settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.ReverseFileOrder = checkBox_reversefileorder.Checked;
            Settings.Default.rememberLastFile = checkBox_rememberlastfile.Checked;
            Settings.Default.loaddefaultdir = checkBox_defaultdirectory.Checked;
            Settings.Default.DefaultDirectory = textBox_defaultdir.Text;
            Settings.Default.DarkMode = checkBox_darkskin.Checked;
            Settings.Default.scriptonload = checkBox_script.Checked;
            Settings.Default.scriptloc = textBox_scriptloc.Text;
            Settings.Default.scriptwait = checkBox_scriptwait.Checked;
            Settings.Default.LoadSubfolders = checkBox_loadsubfolders.Checked;
            Settings.Default.DefaultWindowsOpen = textBox_openinwindows.Text;
            Settings.Default.DefaultEditor = textBox_defaulteditor.Text;
            Settings.Default.bringToFront = checkBox_bringToFront.Checked;
            Settings.Default.loadNewFile = checkBox_loadNewFile.Checked ;
            Settings.Default.openFolderOnCut = textBox_openFolderOnCut.Text;
        }

        private void checkBox_darkskin_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DarkMode = checkBox_darkskin.Checked;
            mainForm.DarkModeRefresh();
        }

        private void button_script_click(object sender, EventArgs e)
        {
                chooseFileOrFolder(textBox_scriptloc,true);
        }

        private void checkBox_loadsubfolders_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.LoadSubfolders = checkBox_loadsubfolders.Checked;
        }

        private void checkBox_loadNewFile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_openOnCut_Click(object sender, EventArgs e)
        {
            chooseFileOrFolder(textBox_openFolderOnCut, false);
        }

        private void button_defaultEditor_Click(object sender, EventArgs e)
        {
            chooseFileOrFolder(textBox_defaulteditor, true);
        }

        private void button_openInWindows_Click(object sender, EventArgs e)
        {
            chooseFileOrFolder(textBox_openinwindows, true);
        }
        private void chooseFileOrFolder(TextBox textBoxToChange, bool isFile)
        {
            if (isFile)
            {



                string initpath = "";
                if (String.IsNullOrEmpty(textBoxToChange.Text)) initpath = "C:";
                else
                {
                    try { initpath = System.IO.Path.GetDirectoryName(textBoxToChange.Text); } catch (Exception ex) { Debug.WriteLine(ex); }
                }
                    var dialog = new OpenFileDialog()
                    {
                        InitialDirectory = initpath,
                        Title = "Select a file"
                    };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxToChange.Text = dialog.FileName;
                }
            }
            else
            {
                var dialog = new FolderSelectDialog
                {
                    InitialDirectory = textBoxToChange.Text,
                    Title = "Select a directory"
                };
                if (dialog.Show(Handle))
                {
                    textBoxToChange.Text = dialog.FileName;
                }
            }

        }
    }
}