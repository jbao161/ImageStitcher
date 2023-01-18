using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageStitcher
{
    /// <summary>
    /// Interaction logic for window_crop.xaml
    /// </summary>
    public partial class window_crop : Window
    {
        public System.Drawing.Point pntLocation;

        public window_crop()
        {
            InitializeComponent();
        }

        #region Private methods

        /// <summary>
        /// NOTE : This one method must be implemented to free up the temporary image created
        /// by the UcImageCropper
        /// </summary>
        private void Window1_Closed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// get a file for the UcImageCropper to work with
        /// </summary>
        public void Load_Image(string img_source)
        {
            this.UcImageCropper.ImageUrl = img_source;
        }
        #endregion
    }
}
