using ImageStitcher.Properties;
using System;
using System.Diagnostics;
using System.Drawing;

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
        public int source_panel;

        public Form_Crop()
        {
            InitializeComponent();
            //Set crop control for each picturebox
        }

        public string sourcepath = "";
        public string tmpimgpath = "";

        public void Load_img(String img_path, int panelnum)
        {
            if (String.IsNullOrEmpty(sourcepath)) { sourcepath = img_path; } else { tmpimgpath = img_path; }
            if (source_image == null){
                using (Bitmap bm = new Bitmap(img_path))
                { // does not lock image file
                    source_image = new Bitmap(bm);
                }
            }
            source_panel = panelnum;
            if (System.IO.Path.GetExtension(img_path).ToLower().Equals(".gif"))
            {
                pictureBox1.ImageLocation = img_path;
            } else
            {
                pictureBox1.Image = source_image;
            }
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
        { // resizes the crop form window on load to match the size of the source picture
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

                if (this.mainForm.numberofimagepanels == 2)
                {
                    this.Width = pblw;
                    this.Height = pblh;
                }
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

            //pictureBox1.Image = source_image; // already handled in image load
            resize_to_picturebox();

            checkBox_overwrite.Checked = Settings.Default.CropOverwrite;

            DarkModeRefresh();
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

        private int xUp, yUp, xDown, yDown;
        private Rectangle rectCropArea;
        private Rectangle mRect;

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var coordinates = pictureBox1.PointToClient(Cursor.Position);
            xUp = coordinates.X;
            yUp = coordinates.Y;

            Rectangle rec = new Rectangle(Math.Min(xDown, xUp), Math.Min(yUp, yDown), Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));

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

            //rectCropArea = new Rectangle(offsetxDown, offsetyDown, scaledwidth, scaledheight);
            // allow crop area to be dragged from any corner, always start rectangle from top left corner
            rectCropArea = new Rectangle(Math.Min(xUp, xDown), Math.Min(yUp, yDown), Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Invalidate();
            var coordinates = pictureBox1.PointToClient(Cursor.Position);
            xDown = coordinates.X;
            yDown = coordinates.Y;

            mRect = new Rectangle(e.X, e.Y, 0, 0);
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

                if (mainForm.check_if_animated_gif(sourcepath))
                {
                    // crop video command example ffmpeg - i input.gif - vf "crop=640:480:0:0" output.gif
                    int cropAreaWidth = (int)bitmapRect.Width;
                    int cropAreaHeight = (int)bitmapRect.Height;
                    int cropAreaLeft = (int)bitmapRect.Left;
                    int cropAreaTop = (int)bitmapRect.Top;
                    string outputvisibility = "/C ";
                    // use "/C "+ for cmd.exe to close automatically "/K "+ for cmd.exe to stay open and view ffmpeg output

                    string tmpgifname = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " tmpgif.gif";
                    var tmpgifpath = mainForm.tmpAppDataPath + tmpgifname;

                    string cropgifcommand = "";
                    // cmd for ffmpeg. quality is bad
                    //cropgifcommand = $"ffmpeg -i \"{sourcepath}\" -vf \"crop={cropAreaWidth}:{cropAreaHeight}:{cropAreaLeft}:{cropAreaTop}\" \"{tmpgifpath}\"";
                    // cmd for imagemagick. good quality
                    cropgifcommand = $"magick convert \"{sourcepath}\" -coalesce -crop {cropAreaWidth}X{cropAreaHeight}+{cropAreaLeft}+{cropAreaTop} +repage -layers optimize \"{tmpgifpath}\"";
                    string arg = outputvisibility + cropgifcommand;
                    Process proc = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = arg,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };

                    proc.Start();
                    proc.WaitForExit();

                    pictureBox2.ImageLocation = tmpgifpath;
                    mainForm.LoadImage(source_panel,tmpgifpath);
                    tmpimgpath = tmpgifpath;
                }
                else
                {
                    var croppedBitmap = new Bitmap((int)bitmapRect.Width, (int)bitmapRect.Height, OriginalPictureboxImage.PixelFormat);
                    using (var g = Graphics.FromImage(croppedBitmap))
                    {
                        g.DrawImage(OriginalPictureboxImage, new Rectangle(Point.Empty, Size.Round(bitmapRect.Size)),
                                    bitmapRect, GraphicsUnit.Pixel);
                        pictureBox2.Image = croppedBitmap;
                        mainForm.LoadImage(source_panel, croppedBitmap);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            if (mainForm.check_if_animated_gif(sourcepath))
            {
                if (String.Equals(tmpimgpath, sourcepath)) { return; }
                mainForm.SaveGifImage(tmpimgpath, sourcepath, checkBox_overwrite.Checked);
            }
            else
            {
                this.mainForm.cropSaveImage(checkBox_overwrite.Checked, source_panel);
            }
        }

        private void Form_Crop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.CropOverwrite = checkBox_overwrite.Checked;
            Settings.Default.Save();
        }

        //check if mouse is down and being draged, then draw rectangle
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var coordinates = pictureBox1.PointToClient(Cursor.Position);
            xUp = coordinates.X;
            yUp = coordinates.Y;

            if (e.Button == MouseButtons.Left)
            {
                mRect = new Rectangle(Math.Min(xDown, xUp), Math.Min(yUp, yDown), Math.Abs(xUp - xDown), Math.Abs(yUp - yDown));
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Green, 2))
            {
                e.Graphics.DrawRectangle(pen, mRect);
            }
        }

        private void button_crop_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_revert_Click(object sender, EventArgs e)
        {
            if (mainForm.check_if_animated_gif(sourcepath))
            {
                mainForm.ReloadPanel(source_panel);
                tmpimgpath = sourcepath;
                //rectCropArea = new Rectangle(0,0, source_image.Width, source_image.Height);
            } else {
                mainForm.LoadImage(source_panel, source_image);
            }

            pictureBox1.Visible = true;
            Load_img(sourcepath, source_panel);

            //pictureBox1.Image = source_image;
        }

        private void DarkModeRefresh()
        {
            if (Settings.Default.DarkMode == true)
            {
                Color darkcolor = (Color)System.Drawing.ColorTranslator.FromHtml(Settings.Default.DarkModeColor);
                Color darkaccentcolor = (Color)System.Drawing.ColorTranslator.FromHtml(Settings.Default.DarkModeColorAccent);

                this.BackColor = darkaccentcolor;

                Color darkcolortext = (Color)System.Drawing.ColorTranslator.FromHtml(Settings.Default.DarkModeColorText);

                foreach (Control subC in panel1.Controls)
                {
                    subC.BackColor = darkaccentcolor;
                    subC.ForeColor = darkcolortext;

                    if (subC is Button)
                    {
                        ((Button)subC).FlatStyle = FlatStyle.Flat;
                        ((Button)subC).FlatAppearance.BorderColor = darkcolor;
                        ((Button)subC).ForeColor = darkcolortext;
                    }
                }
                DarkTitleBarClass.UseImmersiveDarkMode(Handle, true);
            }
            if (Settings.Default.DarkMode == false)
            {
                Color lightbackground = SystemColors.Control;
                this.BackColor = lightbackground;
                foreach (Control subC in panel1.Controls)
                {
                    subC.BackColor = lightbackground;
                    subC.ForeColor = Color.Black;

                    if (subC is Button)
                    {
                        ((Button)subC).FlatStyle = FlatStyle.Standard;
                        ((Button)subC).FlatAppearance.BorderColor = SystemColors.Control;
                    }
                }
                DarkTitleBarClass.UseImmersiveDarkMode(Handle, false);
                this.BackColor = SystemColors.Control;
            }
        }
    }
}