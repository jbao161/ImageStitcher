using ImageStitcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var dialog = new FolderSelectDialog
            {
                InitialDirectory = textBox_defaultdir.Text,
                Title = "Select a default directory to open on load"
            };
            if (dialog.Show(Handle))
            {
                textBox_defaultdir.Text = dialog.FileName;
            }
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
        }

        private void form_settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.ReverseFileOrder = checkBox_reversefileorder.Checked;
            Settings.Default.rememberLastFile = checkBox_rememberlastfile.Checked;
            Settings.Default.loaddefaultdir = checkBox_defaultdirectory.Checked;
            Settings.Default.DefaultDirectory = textBox_defaultdir.Text;
            Settings.Default.DarkMode = checkBox_darkskin.Checked;
        }

        private void checkBox_darkskin_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DarkMode = checkBox_darkskin.Checked;
            mainForm.DarkModeRefresh();
        }
    }
}
