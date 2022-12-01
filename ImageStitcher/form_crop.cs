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
    public partial class Form_Crop : Form
    {

        public Form_Crop()
        {
            InitializeComponent();
            //Set crop control for each picturebox
            ControlrectPicturebox = new ControlCrop.ControlCrop(pictureBox1);
            ControlrectPicturebox.SetControl(this.pictureBox1);
            pictureBox1.Image = source_image;
        }
        public Form_Crop(Image img)
        {
            source_image = img;
            InitializeComponent();
            //Set crop control for each picturebox
            ControlrectPicturebox = new ControlCrop.ControlCrop(pictureBox1);
            ControlrectPicturebox.SetControl(this.pictureBox1);
            pictureBox1.Image = source_image;
        }

        ControlCrop.ControlCrop ControlrectPicturebox;

        private void Button_Crop_Yes_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox targetpicturebox = pictureBox1;
                //Process Picturebox
                Rectangle rectPicturebox = new Rectangle(ControlrectPicturebox.rect.X, ControlrectPicturebox.rect.Y, ControlrectPicturebox.rect.Width,
                ControlrectPicturebox.rect.Height);
                //set cropped image size and creat new bitmap
                Bitmap _pboximg = new Bitmap(ControlrectPicturebox.rect.Width, ControlrectPicturebox.rect.Height);

                //Create the original image to be cropped
                Bitmap OriginalPictureboxImage = new Bitmap(targetpicturebox.Image, targetpicturebox.Width, targetpicturebox.Height);

                //create graphic with using statement to auto close grahics
                using (Graphics gr = Graphics.FromImage(_pboximg))
                {
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    ////set image attributes
                    gr.DrawImage(OriginalPictureboxImage, 0, 0, rectPicturebox, GraphicsUnit.Pixel);
                    targetpicturebox.Image = _pboximg;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_crop_no_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
