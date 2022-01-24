using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ImageStitcher
{
    public partial class MainWindow : Form
    {
        private bool debugflag = true;
        private static int panelfocus = 0;
        private static int bluramount = 10;

        private void debug(string message)
        {
            if (debugflag) Console.WriteLine(message);
        }

        /* Section 1 : Find a control at mouse location
         * this is to find the picture being right clicked on when we get the copy paste context menu
         * code stolen from https://stackoverflow.com/a/16543294
        */

        public static Control FindControlAtPoint(Control container, Point pos)
        {
            Control child;
            foreach (Control c in container.Controls)
            {
                if (c.Visible && c.Bounds.Contains(pos))
                {
                    child = FindControlAtPoint(c, new Point(pos.X - c.Left, pos.Y - c.Top));
                    if (child == null) return c;
                    else return child;
                }
            }
            return null;
        }

        public static Control FindControlAtCursor(Form form)
        {
            Point pos = Cursor.Position;
            if (form.Bounds.Contains(pos))
                return FindControlAtPoint(form, form.PointToClient(pos));
            return null;
        } // end Section 1

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
            {
                e.Effect = DragDropEffects.All;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        /*
         * Drag and drop
         * picture files from Windows explorer onto the panel to load the image
         */

        private void pictureBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            try
            {
                // https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                using (var bmpTemp = new Bitmap(s[0]))
                {
                    ((System.Windows.Forms.PictureBox)sender).Image = new Bitmap(bmpTemp);
                }

                // set the pseudo-focus on the left or right panel
                // then enumerate a list of all image files in the same directory as the loaded image
                // then store the position of the loaded image in that list
                if (sender == pictureBox_leftpanel)
                {
                    panelfocus = 0;

                    imageFilesLeftPanel = EnumerateImageFiles(Path.GetDirectoryName(s[0]));
                    imageCountLeftPanel = imageFilesLeftPanel.Count();
                    for (int i = 0; i < imageFilesLeftPanel.Count; i++)
                    {
                        if (imageFilesLeftPanel[i] == s[0]) { imageIndexLeftPanel = i; previmageIndexLeftPanel = imageIndexLeftPanel; }
                        }
                }
                if (sender == pictureBox_rightpanel)
                {
                    panelfocus = 1;

                    imageFilesRightPanel = EnumerateImageFiles(Path.GetDirectoryName(s[0]));
                    imageCountRightPanel = imageFilesRightPanel.Count();
                    for (int i = 0; i < imageFilesRightPanel.Count; i++)
                    {
                        if (imageFilesRightPanel[i] == s[0]) { imageIndexRightPanel = i; previmageIndexRightPanel = imageIndexRightPanel; }
                        }
                }
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine("File is not a valid image: {0}. \n {1}", s[0], ex);
                return;
            }
            resize_imagepanels();
        }

        /* automatically resize the image
        * concept: calculate the height and width of each image when they are stitched together
        * (scale the taller image to the height of the shorter image, and keep aspect ratio)
        * now you know the proportional width the left image takes up in the total stitched width
        * multiply this proportion by the width of the viewing window and you get the width the left picture
        * should take up. Put a divider there and the two images in 'Zoom' display will be of same height
        */

        private void resize_imagepanels()
        {
            if (!(pictureBox_leftpanel.Image is null || pictureBox_rightpanel.Image is null))
            {
                if (this.splitContainer_bothimages.Orientation == Orientation.Vertical)
                {
                    int min_height = Math.Min(pictureBox_leftpanel.Image.Height, pictureBox_rightpanel.Image.Height);
                    int stitched_leftimg_width = (int)(pictureBox_leftpanel.Image.Width * min_height / (double)pictureBox_leftpanel.Image.Height);
                    int stitched_rightimg_width = (int)(pictureBox_rightpanel.Image.Width * min_height / (double)pictureBox_rightpanel.Image.Height);
                    int result_width = stitched_leftimg_width + stitched_rightimg_width;
                    this.splitContainer_bothimages.SplitterDistance = panel_bothimages.Width * stitched_leftimg_width / result_width;
                }
                if (this.splitContainer_bothimages.Orientation == Orientation.Horizontal)
                {
                    int min_width = Math.Min(pictureBox_leftpanel.Image.Width, pictureBox_rightpanel.Image.Width);
                    int stitched_leftimg_height = (int)(min_width * pictureBox_leftpanel.Image.Height / (double)pictureBox_leftpanel.Image.Width);
                    int stitched_rightimg_height = (int)(min_width * pictureBox_rightpanel.Image.Height / (double)pictureBox_rightpanel.Image.Width);
                    int result_height = stitched_leftimg_height + stitched_rightimg_height;
                    this.splitContainer_bothimages.SplitterDistance = panel_bothimages.Height * stitched_leftimg_height / result_height;
                }
            }
        }

        /*  Create a new image by stitching the two panel images together
         */

        private Bitmap stitch_images()
        {
            if ((pictureBox_leftpanel.Image is null) || (pictureBox_rightpanel.Image is null))
            {
                debug("error. don't have both images loaded");
                return null;
            }
            else
            {
                if (this.splitContainer_bothimages.Orientation == Orientation.Vertical)
                {
                    // scale the taller image to the height of the shorter image, and keep aspect ratio
                    int min_height = Math.Min(pictureBox_leftpanel.Image.Height, pictureBox_rightpanel.Image.Height);
                    int stitched_leftimg_width = (int)(min_height * pictureBox_leftpanel.Image.Width / (double)pictureBox_leftpanel.Image.Height);
                    int stitched_rightimg_width = (int)(min_height * pictureBox_rightpanel.Image.Width / (double)pictureBox_rightpanel.Image.Height);
                    int result_width = stitched_leftimg_width + stitched_rightimg_width;

                    Bitmap stitched_leftimg = new Bitmap(pictureBox_leftpanel.Image,
                        stitched_leftimg_width, min_height);
                    Bitmap stitched_rightimg = new Bitmap(pictureBox_rightpanel.Image,
                        stitched_rightimg_width, min_height);

                    Bitmap stitchedimage = new Bitmap(result_width, min_height);
                    using (Graphics g = Graphics.FromImage(stitchedimage))
                    {
                        g.DrawImage(stitched_leftimg, 0, 0);
                        g.DrawImage(stitched_rightimg, stitched_leftimg.Width, 0);
                    }
                    return stitchedimage;
                }

                if (this.splitContainer_bothimages.Orientation == Orientation.Horizontal) // left image becomes the one on top
                {
                    // scale the wider image down to width of the thinner image, and keep aspect ratio
                    int min_width = Math.Min(pictureBox_leftpanel.Image.Width, pictureBox_rightpanel.Image.Width);
                    int stitched_leftimg_height = (int)(min_width * pictureBox_leftpanel.Image.Height / (double)pictureBox_leftpanel.Image.Width);
                    int stitched_rightimg_height = (int)(min_width * pictureBox_rightpanel.Image.Height / (double)pictureBox_rightpanel.Image.Width);
                    int result_height = stitched_leftimg_height + stitched_rightimg_height;

                    Bitmap stitched_leftimg = new Bitmap(pictureBox_leftpanel.Image,
                        min_width, stitched_leftimg_height);
                    Bitmap stitched_rightimg = new Bitmap(pictureBox_rightpanel.Image,
                        min_width, stitched_rightimg_height);

                    Bitmap stitchedimage = new Bitmap(min_width, result_height);
                    using (Graphics g = Graphics.FromImage(stitchedimage))
                    {
                        g.DrawImage(stitched_leftimg, 0, 0);
                        g.DrawImage(stitched_rightimg, 0, stitched_leftimg_height);
                    }
                    return stitchedimage;
                }

                return new Bitmap(pictureBox_leftpanel.Image);
            }
        }

        /*  Open the stitched image in a viewing window
         */

        private void button_preview_Click(object sender, EventArgs e)
        {
            Bitmap stitchedimage = stitch_images();
            if (!(stitchedimage is null)) try
                {
                    using (Form form = new Form())
                    {
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.ClientSize = stitchedimage.Size;
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

        /*  Save the stiched image to a new file
         *  feature 1: automatically generate a filename with a sortable and copypaste friendly timestamp
         */

        private void button_save_Click(object sender, EventArgs e)
        {
            Bitmap stitchedimage = stitch_images();
            if (!(stitchedimage is null)) try
                {
                    // Displays a SaveFileDialog so the user can save the Image
                    saveFileDialog1.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
                    saveFileDialog1.Title = "Save an Image File";
                    saveFileDialog1.FileName = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " combined";                     // feature 1: timestamp
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
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
                    } // if dialog OK
                    // Opens Fle Directory if checkbox enabled
                    if (checkBox_openaftersave.Checked)
                    {
                        string args = string.Format("/e, /select, \"{0}\"", System.IO.Path.GetFullPath(saveFileDialog1.FileName));

                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "explorer";
                        info.Arguments = args;
                        Thread.Sleep(300); // fast and dirty way of waiting for file finish writing, otherwise file gets deselected.
                        Process.Start(info);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
        }

        /*  Section 2: Button controls to clear the picture panels
         */

        private void button_ClearRightPanel_Click(object sender, EventArgs e)
        {
            pictureBox_rightpanel.Image = null;
            imageFilesRightPanel = null;
            imageIndexRightPanel = 0;
            previmageIndexRightPanel = imageIndexRightPanel;
        }

        private void button_ClearLeftPanel_Click(object sender, EventArgs e)
        {
            pictureBox_leftpanel.Image = null;
            imageFilesLeftPanel = null;
            imageIndexLeftPanel = 0;
            previmageIndexLeftPanel = imageIndexLeftPanel;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Try to cast the sender to a ToolStripItem
            ToolStripItem menuItem = sender as ToolStripItem;
            if (menuItem != null)
            {
                // Retrieve the ContextMenuStrip that owns this ToolStripItem
                ContextMenuStrip owner = menuItem.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    // Get the control that is displaying this context menu
                    ((PictureBox)owner.SourceControl).Image = null;
                }
            }
        }// end section 2

        /* Section 3: Context menu for copy and paste
         */

        // open a copy paste menu at right click mouse location
        private void control_MouseClick_copypastemenu(object sender, MouseEventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (thispicturebox == pictureBox_leftpanel) { panelfocus = 0; }
            if (thispicturebox == pictureBox_rightpanel) { panelfocus = 1; }
            if (e.Button == MouseButtons.Right)
            {
            }
                if (e.Button == MouseButtons.Right)
            {
                Point mPointWhenClicked = new Point(e.X, e.Y);
                contextMenu_image.Show((Control)sender, e.X, e.Y);
            }
        }

        // copy image to clipboard from panel
        private void contextMenu_image_item_copy_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (!(thispicturebox.Image is null))
            {
                DataObject dobj = new DataObject();
                dobj.SetData(DataFormats.Bitmap, true, thispicturebox.Image);
                Clipboard.SetDataObject(dobj, true);
            }
        }

        // paste image to panel from file or clipboard
        private void contextMenu_image_item_paste_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (Clipboard.ContainsImage())
            {
                object o = Clipboard.GetImage();
                if (o != null)
                {
                    thispicturebox.Image = (Image)o;
                }
            }
            if (Clipboard.GetDataObject().GetDataPresent("FileDrop"))
            {
                string[] s = (string[])Clipboard.GetDataObject().GetData(DataFormats.FileDrop, false);
                try
                {
                    using (var bmpTemp = new Bitmap(s[0]))
                    {
                        thispicturebox.Image = new Bitmap(bmpTemp);
                    }
                    if (thispicturebox == pictureBox_leftpanel)
                    {
                        panelfocus = 0;

                        imageFilesLeftPanel = EnumerateImageFiles(Path.GetDirectoryName(s[0]));
                        imageCountLeftPanel = imageFilesLeftPanel.Count();
                        for (int i = 0; i < imageCountLeftPanel; i++)
                        {
                            if (imageFilesLeftPanel[i] == s[0]) { imageIndexLeftPanel = i; previmageIndexLeftPanel = imageIndexLeftPanel; }
                            }
                    }
                    if (thispicturebox == pictureBox_rightpanel)
                    {
                        panelfocus = 1;

                        imageFilesRightPanel = EnumerateImageFiles(Path.GetDirectoryName(s[0]));
                        imageCountRightPanel = imageFilesRightPanel.Count();
                        for (int i = 0; i < imageCountRightPanel; i++)
                        {
                            if (imageFilesRightPanel[i] == s[0]) { imageIndexRightPanel = i; previmageIndexRightPanel = imageIndexRightPanel; }
                            }
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    Console.WriteLine("File is not a valid image: {0}. \n {1}", s[0], ex);
                    return;
                }
            }
            resize_imagepanels();
        } // end section 3

        /*  Section 4: Keyboard arrows change image to next file in folder
        */
        // Local field to store the files enumerator;

        private static List<string> imageFilesLeftPanel;
        private static List<string> imageFilesRightPanel;
        private static int imageIndexLeftPanel = 0;
        private static int imageIndexRightPanel = 0;
        private static int previmageIndexLeftPanel = 0;
        private static int previmageIndexRightPanel = 0;
        private static int imageCountLeftPanel = 0;
        private static int imageCountRightPanel = 0;

        // You can wrap the calls to MoveNext, and Current property in a simple wrapper method..
        // Can also add your error handling here.
        public static string GetNextFile()
        {
            // You can choose to throw exception if you like..
            // How you handle things like this, is up to you.
            return null;
        }

        // Call GetNextFile() whenever you user clicks the next button on your UI.

        private static List<string> EnumerateImageFiles(string folderPath)
        {
            // https://stackoverflow.com/questions/7039580/multiple-file-extensions-searchpattern-for-system-io-directory-getfiles
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".gif", ".png", ".tiff", ".exif", ".bmp" };
            var filteredFiles = Directory
            .EnumerateFiles(folderPath) //<--- .NET 4.5
            .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
            .ToList();
            filteredFiles.Sort();
            return filteredFiles;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // left and right arrow keys for changing image
            if ((panelfocus == 0) && pictureBox_leftpanel.Image != null && (keyData == Keys.Left))
            {
                if (imageFilesLeftPanel != null)
                {
                    int nextImageIndex = imageIndexLeftPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountLeftPanel - 1;
                    previmageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = nextImageIndex;
                    using (var bmpTemp = new Bitmap(imageFilesLeftPanel[nextImageIndex]))
                    {
                        pictureBox_leftpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                resize_imagepanels();
                return true;
            }
            if ((panelfocus == 0) && pictureBox_leftpanel.Image != null && (keyData == Keys.Right))
            {
                if (imageFilesLeftPanel != null)
                {
                    int nextImageIndex = imageIndexLeftPanel + 1;
                    if (nextImageIndex >= imageCountLeftPanel) nextImageIndex = 0;
                    previmageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = nextImageIndex;
                    using (var bmpTemp = new Bitmap(imageFilesLeftPanel[nextImageIndex]))
                    {
                        pictureBox_leftpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                resize_imagepanels();
                return true;
            }
            if ((panelfocus == 1) && pictureBox_rightpanel.Image != null && (keyData == Keys.Left))
            {
                if (imageFilesRightPanel != null)
                {
                    int nextImageIndex = imageIndexRightPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountRightPanel - 1;
                    previmageIndexRightPanel = imageIndexRightPanel;
                    imageIndexRightPanel = nextImageIndex;
                    using (var bmpTemp = new Bitmap(imageFilesRightPanel[nextImageIndex]))
                    {
                        pictureBox_rightpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                resize_imagepanels();
                return true;
            }
            if ((panelfocus == 1) && pictureBox_rightpanel.Image != null && (keyData == Keys.Right))
            {
                if (imageFilesRightPanel != null)
                {
                    int nextImageIndex = imageIndexRightPanel + 1;
                    if (nextImageIndex >= imageCountRightPanel) nextImageIndex = 0;
                    previmageIndexRightPanel = imageIndexRightPanel;
                    imageIndexRightPanel = nextImageIndex;
                    using (var bmpTemp = new Bitmap(imageFilesRightPanel[nextImageIndex]))
                    {
                        pictureBox_rightpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                resize_imagepanels();
                return true;
            }

            // R hotkey for rotations
            if ((panelfocus == 0) && pictureBox_leftpanel.Image != null && (keyData == Keys.R))
            {
                Image img = pictureBox_leftpanel.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox_leftpanel.Image = img;
                return true;
            }
            if ((panelfocus == 1) && pictureBox_rightpanel.Image != null && (keyData == Keys.R))
            {
                Image img = pictureBox_rightpanel.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox_rightpanel.Image = img;
                return true;
            }

            // B hotkey for blur
            if ((panelfocus == 0) && pictureBox_leftpanel.Image != null && (keyData == Keys.B))
            {
                var blur = new GaussianBlur(pictureBox_leftpanel.Image as Bitmap);
                Bitmap result = blur.Process(bluramount);
                pictureBox_leftpanel.Image = result;
                return true;
            }
            if ((panelfocus == 1) && pictureBox_rightpanel.Image != null && (keyData == Keys.B))
            {
                var blur = new GaussianBlur(pictureBox_rightpanel.Image as Bitmap);
                Bitmap result = blur.Process(bluramount);
                pictureBox_rightpanel.Image = result;
                return true;
            }

            // end of hotkeys. ignore the keystroke
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        /*  Section 5: Toggle top/bottom or side/side stitching
        */

        private void button_verticalhorizontal_Click(object sender, EventArgs e)
        {
            splitContainer_bothimages.Orientation = (splitContainer_bothimages.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical);
            button_verticalhorizontal.Text = (splitContainer_bothimages.Orientation == Orientation.Vertical ? "Stack images vertically" : "Put images side by side");
            resize_imagepanels();
        }

        private void button_swapimages_Click(object sender, EventArgs e)
        {
            Bitmap image_tempswap = (pictureBox_leftpanel.Image == null ? null : new Bitmap(pictureBox_leftpanel.Image));
            pictureBox_leftpanel.Image = (pictureBox_rightpanel.Image == null ? null : new Bitmap(pictureBox_rightpanel.Image));
            pictureBox_rightpanel.Image = image_tempswap;
            resize_imagepanels();
            var tmp1 = imageFilesLeftPanel;
            imageFilesLeftPanel = imageFilesRightPanel;
            imageFilesRightPanel = tmp1;
            var tmp2 = imageIndexLeftPanel;
            previmageIndexLeftPanel = imageIndexLeftPanel;
            imageIndexLeftPanel = imageIndexRightPanel;
            previmageIndexRightPanel = imageIndexRightPanel;
            imageIndexRightPanel = tmp2;
            var tmp3 = imageCountLeftPanel;
            imageCountLeftPanel = imageCountRightPanel;
            imageCountRightPanel = tmp3;
        }

        private void rotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (thispicturebox is null) return;
            if (!(thispicturebox.Image is null))
            {
                Image img = thispicturebox.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                thispicturebox.Image = img;
            }
        }

        private void previousLeftArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (panelfocus == 0 && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null)
                {
                    int nextImageIndex = imageIndexLeftPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountLeftPanel - 1;
                    previmageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = nextImageIndex;
                    using (var bmpTemp = new Bitmap(imageFilesLeftPanel[nextImageIndex]))
                    {
                        pictureBox_leftpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                resize_imagepanels();
            }
            if (panelfocus == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null)
                {
                    int nextImageIndex = imageIndexRightPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountRightPanel - 1;
                    previmageIndexRightPanel = imageIndexRightPanel;
                    imageIndexRightPanel = nextImageIndex;
                    using (var bmpTemp = new Bitmap(imageFilesRightPanel[nextImageIndex]))
                    {
                        pictureBox_rightpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                resize_imagepanels();
            }
        }

        private void nextRightArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (panelfocus == 0 && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null)
                {
                    int nextImageIndex = imageIndexLeftPanel + 1;
                    if (nextImageIndex >= imageCountLeftPanel) nextImageIndex = 0;
                    previmageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = nextImageIndex;
                    try
                    {
                        using (var bmpTemp = new Bitmap(imageFilesLeftPanel[nextImageIndex]))
                        {
                            pictureBox_leftpanel.Image = new Bitmap(bmpTemp);
                        }
                    }
                    catch (Exception) { throw; }

                }
            }
            if (panelfocus == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null)
                {
                    int nextImageIndex = imageIndexRightPanel + 1;
                    if (nextImageIndex >= imageCountRightPanel) nextImageIndex = 0;
                    previmageIndexRightPanel = imageIndexRightPanel;
                    imageIndexRightPanel = nextImageIndex;
                    try
                    {
                        using (var bmpTemp = new Bitmap(imageFilesRightPanel[nextImageIndex]))
                        {
                            pictureBox_rightpanel.Image = new Bitmap(bmpTemp);
                        }
                    }
                    catch (Exception) { throw; } 
                }
            }
            resize_imagepanels();
        }

        private void blurBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (thispicturebox is null) return;
            if (!(thispicturebox.Image is null))
            {
                var blur = new GaussianBlur(thispicturebox.Image as Bitmap);
                Bitmap result = blur.Process(bluramount);
                thispicturebox.Image = result;
            }
        }

        //https://stackoverflow.com/questions/16022188/open-an-image-with-the-windows-default-editor-in-c-sharp
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (panelfocus == 0 && pictureBox_leftpanel.Image != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(imageFilesLeftPanel[imageIndexLeftPanel]);
                startInfo.Verb = "edit";
                Process.Start(startInfo);
            }
            if (panelfocus == 1 && pictureBox_rightpanel.Image != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(imageFilesRightPanel[imageIndexRightPanel]);
                startInfo.Verb = "edit";
                Process.Start(startInfo);
            }
        }

        //https://stackoverflow.com/questions/9646114/open-file-location
        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (panelfocus == 0 && imageFilesLeftPanel[imageIndexLeftPanel] != null)
            {
                Process.Start("explorer.exe", "/select, " + imageFilesLeftPanel[imageIndexLeftPanel]);
            }
            if (panelfocus == 1 && imageFilesRightPanel[imageIndexRightPanel] != null)
            {
                Process.Start("explorer.exe", "/select, " + imageFilesRightPanel[imageIndexRightPanel]);
            }
        }

        //https://stackoverflow.com/questions/3282418/send-a-file-to-the-recycle-bin
        //https://www.c-sharpcorner.com/UploadFile/mahesh/how-to-remove-an-item-from-a-C-Sharp-list/
        private void sendToTrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (panelfocus == 0 && imageFilesLeftPanel[imageIndexLeftPanel] != null)
            {
                FileSystem.DeleteFile(imageFilesLeftPanel[imageIndexLeftPanel],
            Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
            Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                imageFilesLeftPanel.RemoveAt(imageIndexLeftPanel);
                imageCountLeftPanel--;
                previousLeftArrowToolStripMenuItem_Click(sender, e);
            }
            if (panelfocus == 1 && imageFilesRightPanel[imageIndexRightPanel] != null)
            {
                FileSystem.DeleteFile(imageFilesRightPanel[imageIndexRightPanel],
            Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
            Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                imageFilesRightPanel.RemoveAt(imageIndexRightPanel);
                imageCountRightPanel--;
                previousLeftArrowToolStripMenuItem_Click(sender, e);
            }
        }

        private void jumpBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (panelfocus == 0 && imageFilesLeftPanel[previmageIndexLeftPanel] != null)
            {
                int swapImageIndex = imageIndexLeftPanel;
                imageIndexLeftPanel = previmageIndexLeftPanel;
                previmageIndexLeftPanel = swapImageIndex;
                try
                {
                    using (var bmpTemp = new Bitmap(imageFilesLeftPanel[imageIndexLeftPanel]))
                    {
                        pictureBox_leftpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                catch (Exception) { throw; }
            }
            if (panelfocus == 1 && imageFilesRightPanel[previmageIndexRightPanel] != null)
            {
                int swapImageIndex = imageIndexRightPanel;
                imageIndexRightPanel = previmageIndexRightPanel;
                previmageIndexRightPanel = swapImageIndex;
                try
                {
                    using (var bmpTemp = new Bitmap(imageFilesRightPanel[imageIndexRightPanel]))
                    {
                        pictureBox_rightpanel.Image = new Bitmap(bmpTemp);
                    }
                }
                catch (Exception) { throw; }
            }
        }
    } // end MainWindow : Form
}