using ImageStitcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ImageStitcher
{
    public partial class form_settings : Form
    {
        public form_settings()
        {
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

        private void form_settings_Load(object sender, EventArgs e)
        {
            pntLocation.X -= this.Size.Width / 2;
            pntLocation.Y -= this.Size.Height;
            this.Location = pntLocation;
            System.Drawing.Point topleft = this.PointToScreen(new System.Drawing.Point(this.Left, this.Top));
            if (topleft.X < 0) { this.Left =  0; } // opens settings form within viewable screen
            checkBox_reversefileorder.Checked = Settings.Default.ReverseFileOrder;
            checkBox_rememberlastfile.Checked = Settings.Default.rememberLastFile;
            checkBox_defaultdirectory.Checked = Settings.Default.loaddefaultdir;
            textBox_defaultdir.Text = Settings.Default.DefaultDirectory;
        }

        private void form_settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.ReverseFileOrder = checkBox_reversefileorder.Checked;
            Settings.Default.rememberLastFile = checkBox_rememberlastfile.Checked;
            Settings.Default.loaddefaultdir = checkBox_defaultdirectory.Checked;
            Settings.Default.DefaultDirectory = textBox_defaultdir.Text;
        }
    }
}
