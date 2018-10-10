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
    public partial class MainWindow : Form
    {

        private bool debugflag = true;
        private System.Windows.Forms.PictureBox this_picturebox;
        private void debug(string message)
        {
            if (debugflag) Console.WriteLine(message);
        }
        int imgleft_height, imgleft_width, imgright_height, imgright_width, result_height, result_width = 0, splitter_position, panel_height, panel_width, leftpicturebox_width, rightpicturebox_width;
        float imgsize_ratio, splittersize_ratio;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            pictureBox_rightpanel.AllowDrop = true;
            pictureBox_leftpanel.AllowDrop = true;
        }
        private void pictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            this_picturebox = ((System.Windows.Forms.PictureBox)sender);
            try
            {
                this_picturebox.Image = Bitmap.FromFile(s[0]);
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("File is not a valid image: {0}. \n {1}", s[0], ex);
                return;
            }
            // resize the splitcontainer
            // * get the image dimensions and compare with splitcontainer dimensions
            if (debugflag) Console.WriteLine("Image dimensions: {0} height and {1} width", this_picturebox.Image.Height, this_picturebox.Image.Width);

        }

        private Bitmap stitch_images()
        {

            if ((pictureBox_leftpanel.Image is null) || (pictureBox_rightpanel.Image is null))
            {
                debug("error. don't have both images loaded");
                return null;
            }
            else
            {
                // resize the taller image to the height of the smaller image
                imgleft_height = pictureBox_leftpanel.Image.Height;
                imgleft_width = pictureBox_leftpanel.Image.Width;
                imgright_height = pictureBox_rightpanel.Image.Height;
                imgright_width = pictureBox_rightpanel.Image.Width;
                result_height = Math.Min(imgleft_height, imgright_height);

                if (imgleft_height > result_height)
                {
                    this_picturebox = pictureBox_leftpanel;
                    imgsize_ratio = (float)result_height / (float)imgleft_height;
                    this_picturebox.Image = new Bitmap(this_picturebox.Image,
                    (int)(imgleft_width * imgsize_ratio), result_height);
                    imgleft_width = this_picturebox.Image.Width;

                }
                else
                {
                    this_picturebox = pictureBox_rightpanel;
                    imgsize_ratio = (float)result_height / (float)imgright_height;
                    this_picturebox.Image = new Bitmap(this_picturebox.Image,
                    (int)(imgright_width * imgsize_ratio), result_height);
                    imgright_width = this_picturebox.Image.Width;
                }
                result_width = imgleft_width + imgright_width;
                Bitmap stitchedimage = new Bitmap(result_width, result_height);
                using (Graphics g = Graphics.FromImage(stitchedimage))
                {
                    g.DrawImage(pictureBox_leftpanel.Image, 0, 0);
                    g.DrawImage(pictureBox_rightpanel.Image, imgleft_width, 0);
                }
                return stitchedimage;
            }
        }
        private void button_preview_Click(object sender, EventArgs e)
        {
            Bitmap stitchedimage = stitch_images();
            if (!(stitchedimage is null)) try
                {
                    using (Form form = new Form())
                    {

                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.Size = stitchedimage.Size;

                        PictureBox pb = new PictureBox();
                        pb.Dock = DockStyle.Fill;
                        pb.Image = stitchedimage;
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        form.Controls.Add(pb);
                        form.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
        }
        private void button_save_Click(object sender, EventArgs e)
        {
            Bitmap stitchedimage = stitch_images();
            if (!(stitchedimage is null)) try
                {
                    // Displays a SaveFileDialog so the user can save the Image   
                    saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
                    saveFileDialog1.Title = "Save an Image File";
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.ShowDialog();

                    // If the file name is not an empty string open it for saving.  
                    if (saveFileDialog1.FileName != "")
                    {
                        // Saves the Image via a FileStream created by the OpenFile method.  
                        System.IO.FileStream fs =
                           (System.IO.FileStream)saveFileDialog1.OpenFile();
                        // Saves the Image in the appropriate ImageFormat based upon the  
                        // File type selected in the dialog box.  
                        // NOTE that the FilterIndex property is one-based.  
                        switch (saveFileDialog1.FilterIndex)
                        {
                            case 1:
                                stitchedimage.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Jpeg);
                                break;

                            case 2:
                                stitchedimage.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Bmp);
                                break;

                            case 3:
                                stitchedimage.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Gif);
                                break;
                            case 4:
                                stitchedimage.Save(fs,
                                   System.Drawing.Imaging.ImageFormat.Png);
                                break;
                        }

                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
        }
    }
}


