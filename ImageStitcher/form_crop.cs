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
        public Point pntLocation;
        private MainWindow mainForm = null;
        public Form_Crop(MainWindow callingForm)
        { // https://stackoverflow.com/questions/1665533/communicate-between-two-windows-forms-in-c-sharp
            mainForm = callingForm as MainWindow;
            InitializeComponent();
        }
        public Image source_image;
        public Form_Crop()
        {
            InitializeComponent();
            //Set crop control for each picturebox

        }
        public void Load_img(Image img)
        {
            source_image = img;
            Update();
        }

        public void Load_img(String img_path)
        {
            source_image = new Bitmap(img_path);
            Update();
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

        private void resize_to_picturebox()
        {
            int[] cropWindowPositions = this.mainForm.getCropWindowPositions();
            int pblw, pblh, pbll, pblt, pbrw, pbrh, pbrl, pbrt;
            pblw = cropWindowPositions[0];
            pblh = cropWindowPositions[1];
            pbll = cropWindowPositions[2];
            pblt = cropWindowPositions[3];
            pbrw = cropWindowPositions[4];
            pbrh = cropWindowPositions[5];
            pbrl = cropWindowPositions[6];
            pbrt = cropWindowPositions[7];
            int targetpicturebox = cropWindowPositions[8];
            if (targetpicturebox == 0)
            {
                this.Left = pbll;
                this.Top = pblt;
                this.Width = pblw;
                this.Height = pblh;
            }
            if (targetpicturebox == 1)
            {
                this.Left = pbrl;
                this.Top = pbrt;
                this.Width = pbrw;
                this.Height = pbrh;
            }
        }
        private void form_crop_load(object sender, EventArgs e)
        {
            pntLocation.X -= this.Size.Width / 2;
            pntLocation.Y -= this.Size.Height;
            this.Location = pntLocation;
            CheckBounds();

            pictureBox1.Image = source_image;
            resize_to_picturebox();
            //Set crop control for each picturebox
            //ControlrectPicturebox = new ControlCrop.ControlCrop(pictureBox1);
            //ControlrectPicturebox.SetControl(this.pictureBox1);
        }

        private Size GetDisplayedImageSize(PictureBox pictureBox)
        {
            Size containerSize = pictureBox.ClientSize;
            float containerAspectRatio = (float)containerSize.Height / (float)containerSize.Width;
            Size originalImageSize = pictureBox.Image.Size;
            float imageAspectRatio = (float)originalImageSize.Height / (float)originalImageSize.Width;

            Size result = new Size();
            if (containerAspectRatio > imageAspectRatio)
            {
                result.Width = containerSize.Width;
                result.Height = (int)(imageAspectRatio * (float)containerSize.Width);
            }
            else
            {
                result.Height = containerSize.Height;
                result.Width = (int)((1.0f / imageAspectRatio) * (float)containerSize.Height);
            }
            return result;
        }

        ControlCrop.ControlCrop ControlrectPicturebox;

        int xUp, yUp, xDown, yDown;
        Rectangle rectCropArea;
        void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            xUp = e.X;
            yUp = e.Y;

            Rectangle rec = new Rectangle(xDown, yDown, Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));

            using (Pen pen = new Pen(Color.Green, 2))
            {

                pictureBox1.CreateGraphics().DrawRectangle(pen, rec);
            }

            Size pbsize = GetDisplayedImageSize(pictureBox1);

    
            double xscale, yscale;
            xscale = (double)source_image.Width / (double)pbsize.Width;
            yscale = (double)source_image.Height / (double)pbsize.Height;

            int scaledwidth = Math.Abs(xUp - xDown);
            int scaledheight = Math.Abs(yUp - yDown);
            scaledwidth = (int)(Math.Abs(xUp - xDown) * xscale);
            scaledheight = (int)(Math.Abs(yUp - yDown) * yscale);

            int offsetxDown = xDown;
            int offsetyDown = yDown; 

            if (source_image.Height > source_image.Width)
            {
                offsetxDown = xDown - (int)((double)(source_image.Width - pbsize.Width) * 0.5);
            }
            if (source_image.Height < source_image.Width)
            {
                offsetyDown = yDown - (int)((double)(source_image.Height - pbsize.Height) * 0.5);
            }

            int offsetxUp = xUp - (int)((double)(source_image.Width - pbsize.Width) * 0.5);
            int offsetyUp = yUp - (int)((double)(source_image.Height - pbsize.Height) * 0.5);

            rectCropArea = new Rectangle(offsetxDown, offsetyDown, scaledwidth, scaledheight);
            rectCropArea = new Rectangle(xDown, yDown, Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
        }
        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Invalidate();

            xDown = e.X;
            yDown = e.Y;
        }
        public RectangleF GetRectangeOnImage(PictureBox p, Rectangle rectOnPictureBox)
        {
            var method = typeof(PictureBox).GetMethod("ImageRectangleFromSizeMode",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            var imageRect = (Rectangle)method.Invoke(p, new object[] { p.SizeMode });
            if (p.Image == null)
                return rectOnPictureBox;
            var cx = (float)p.Image.Width / (float)imageRect.Width;
            var cy = (float)p.Image.Height / (float)imageRect.Height;
            rectOnPictureBox.Offset(-imageRect.X, -imageRect.Y);
            return new RectangleF(rectOnPictureBox.X * cx, rectOnPictureBox.Y * cy,
                rectOnPictureBox.Width * cx, rectOnPictureBox.Height * cy);
        }
        
        private void btnCrop_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Visible = false;
                pictureBox2.Refresh();
                //Prepare a new Bitmap on which the cropped image will be drawn
                Bitmap OriginalPictureboxImage = new Bitmap(source_image, source_image.Width, source_image.Height);

                ZoomFactor zoomHelper = new ZoomFactor();


                RectangleF currentSelection = rectCropArea;
                RectangleF bitmapRectt = zoomHelper.TranslateZoomSelection(currentSelection, pictureBox1.Size, OriginalPictureboxImage.Size);
                RectangleF bitmapRect = zoomHelper.ConstrainCropAreaToImage(bitmapRectt, OriginalPictureboxImage.Size);



                var croppedBitmap = new Bitmap((int)bitmapRect.Width, (int)bitmapRect.Height, OriginalPictureboxImage.PixelFormat);
                using (var g = Graphics.FromImage(croppedBitmap))
                {
                    g.DrawImage(OriginalPictureboxImage, new Rectangle(Point.Empty, Size.Round(bitmapRect.Size)),
                                bitmapRect, GraphicsUnit.Pixel);
                    pictureBox2.Image = croppedBitmap;
                    mainForm.LoadActiveImage(croppedBitmap);
                }

            }
            catch (Exception ex)
            {

            }
        }
        private void Button_Crop_Yes_Click(object sender, EventArgs e)
        {
            try
            {
                PictureBox targetpicturebox = pictureBox1;

                double xscale, yscale;
                xscale = (double)pictureBox1.Image.Width / (double)pictureBox1.Width;
                yscale = (double)pictureBox1.Image.Height / (double)pictureBox1.Height;
                //Process Picturebox

                // xscale = 1.0 ;
                //yscale = 1.0 ;


                int nx, ny, nw, nh;
                nx = (int) (xscale * ControlrectPicturebox.rect.X);
                ny = (int) (yscale * ControlrectPicturebox.rect.Y);
                nw = (int) (xscale * ControlrectPicturebox.rect.Width);
                nh = (int) (yscale * ControlrectPicturebox.rect.Height);
                Rectangle rectPicturebox = new Rectangle(nx, ny, nw, nh);
                //set cropped image size and creat new bitmap
                Bitmap _pboximg = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);

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
                    mainForm.LoadActiveImage(_pboximg);
                    //targetpicturebox.Image = _pboximg;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button_crop_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_revert_Click(object sender, EventArgs e)
        {
            mainForm.LoadActiveImage(source_image);
        }
    }
}
