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
    public partial class usercontrol_settings : UserControl
    {
        public usercontrol_settings()
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
    }
}
