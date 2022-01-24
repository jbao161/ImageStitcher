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
        private readonly bool debugflag = true;
        private static int panelfocus = 0;
        private static readonly int bluramount = 10;

        private void MsgDebug(string message)
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

        private void PictureBox_DragEnter(object sender, DragEventArgs e)
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
        private static readonly int maxLengthFileList = (int)1.0e9;

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            int maxFilesCount = maxLengthFileList;
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            bool isFolder = File.GetAttributes(s[0]).HasFlag(FileAttributes.Directory);
            bool isImage = allowedImageExtensions.Any(s[0].ToLower().EndsWith);
            string folderPath = Path.GetDirectoryName(s[0]);
            if (isFolder) { folderPath = s[0]; }
            List<string> imageList = null;
            if (isFolder | isImage)
            {
                // set the pseudo-focus on the left or right panel
                // then enumerate a list of all image files in the same directory as the loaded image
                // then store the position of the loaded image in that list
                if (sender == pictureBox_leftpanel)
                {
                    panelfocus = 0;

                    imageFilesLeftPanel = EnumerateImageFiles(folderPath, allowedImageExtensions, isFolder);
                    imageCountLeftPanel = imageFilesLeftPanel.Count();
                    if (imageCountLeftPanel < maxFilesCount) maxFilesCount = imageCountLeftPanel;
                    for (int i = 0; i < maxFilesCount; i++)
                    {
                        if (imageFilesLeftPanel[i] == s[0]) { imageIndexLeftPanel = i; previmageIndexLeftPanel = imageIndexLeftPanel; }
                    }
                    imageList = imageFilesLeftPanel;
                    if (isFolder) { imageIndexLeftPanel = 0; previmageIndexLeftPanel = imageIndexLeftPanel; }
                }
                if (sender == pictureBox_rightpanel)
                {
                    panelfocus = 1;

                    imageFilesRightPanel = EnumerateImageFiles(folderPath, allowedImageExtensions, isFolder);
                    imageCountRightPanel = imageFilesRightPanel.Count();
                    if (imageCountRightPanel < maxFilesCount) maxFilesCount = imageCountRightPanel;
                    for (int i = 0; i < maxFilesCount; i++)
                    {
                        if (imageFilesRightPanel[i] == s[0]) { imageIndexRightPanel = i; previmageIndexRightPanel = imageIndexRightPanel; }
                    }
                    imageList = imageFilesRightPanel;
                    if (isFolder) { imageIndexRightPanel = 0; previmageIndexRightPanel = imageIndexRightPanel; }
                }
            }
            try
            {
                // https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                string imagepath = s[0];
                if (isFolder) imagepath = imageList[0];
                using (var bmpTemp = new Bitmap(imagepath))
                {
                    ((System.Windows.Forms.PictureBox)sender).Image = new Bitmap(bmpTemp);
                }
            }
            catch (Exception) { throw; }
            Resize_imagepanels();
        }

        /* automatically resize the image
        * concept: calculate the height and width of each image when they are stitched together
        * (scale the taller image to the height of the shorter image, and keep aspect ratio)
        * now you know the proportional width the left image takes up in the total stitched width
        * multiply this proportion by the width of the viewing window and you get the width the left picture
        * should take up. Put a divider there and the two images in 'Zoom' display will be of same height
        */

        private void Resize_imagepanels()
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

        private Bitmap Stitch_images()
        {
            if ((pictureBox_leftpanel.Image is null) || (pictureBox_rightpanel.Image is null))
            {
                MsgDebug("error. don't have both images loaded");
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

        private void Button_preview_Click(object sender, EventArgs e)
        {
            Bitmap stitchedimage = Stitch_images();
            if (!(stitchedimage is null)) try
                {
                    using (Form form = new Form())
                    {
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.ClientSize = stitchedimage.Size;
                        PictureBox pb = new PictureBox
                        {
                            Dock = DockStyle.Fill,
                            Image = stitchedimage,
                            SizeMode = PictureBoxSizeMode.Zoom
                        };
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

        private void Button_save_Click(object sender, EventArgs e)
        {
            Bitmap stitchedimage = Stitch_images();
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

                        ProcessStartInfo info = new ProcessStartInfo
                        {
                            FileName = "explorer",
                            Arguments = args
                        };
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

        private void ClearPanel(int activePanel)
        {
            if (activePanel == 0)
            {
                pictureBox_leftpanel.Image = null;
                imageFilesLeftPanel = null;
                imageCountLeftPanel = 0;
                imageIndexLeftPanel = 0;
                previmageIndexLeftPanel = imageIndexLeftPanel;
            }
            if (activePanel == 1)
            {
                pictureBox_rightpanel.Image = null;
                imageFilesRightPanel = null;
                imageCountRightPanel = 0;
                imageIndexRightPanel = 0;
                previmageIndexRightPanel = imageIndexRightPanel;
            }
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // how to get active panel for context menu
            /*
             if (sender is ToolStripItem menuItem)
                {
                    if (menuItem.Owner is ContextMenuStrip owner)
                    {
                        ((PictureBox) owner.SourceControl).Image = null;
                    }
                 } 
            */
            ClearPanel(panelfocus);
        }// end section 2

        /* Section 3: Context menu for copy and paste
         */

        // open a copy paste menu at right click mouse location
        private void Control_MouseClick_copypastemenu(object sender, MouseEventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (thispicturebox == pictureBox_leftpanel) { panelfocus = 0; }
            if (thispicturebox == pictureBox_rightpanel) { panelfocus = 1; }
            if ((MainWindow.ModifierKeys == Keys.Alt) && e.Button == MouseButtons.Left)
            {
                JumpBack(panelfocus);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                RandomToolStripMenuItem_Click(sender, e);
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                contextMenu_image.Show((Control)sender, e.X, e.Y);
                return;
            }
        }

        // copy image to clipboard from panel
        private void ContextMenu_image_item_copy_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (!(thispicturebox.Image is null))
            {
                DataObject dobj = new DataObject();
                dobj.SetData(DataFormats.Bitmap, true, thispicturebox.Image);
                Clipboard.SetDataObject(dobj, true);
            }
        }

        //https://stackoverflow.com/questions/2953254/cgetting-all-image-files-in-folder
        private static readonly String[] allowedImageExtensions = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "jfif" };

        public static List<String> EnumerateImageFiles(String searchFolder, String[] filters, bool isRecursive)
        {
            List<String> filesFound = new List<String>();
            var searchOption = isRecursive ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;
            foreach (var filter in filters)
            {
                filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
            }
            return filesFound;
        }

        /*        private static List<string> EnumerateImageFiles(string folderPath)
        {
            // https://stackoverflow.com/questions/7039580/multiple-file-extensions-searchpattern-for-system-io-directory-getfiles
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".gif", ".png", ".tiff", ".exif", ".bmp" };
            var filteredFiles = Directory
            .EnumerateFiles(folderPath) //<--- .NET 4.5
            .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
            .ToList();
            filteredFiles.Sort();
            return filteredFiles;
        }*/

        // paste image to panel from file or clipboard
        private void ContextMenu_image_item_paste_Click(object sender, EventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (sender is ToolStripItem menuItem)
            {
                if (menuItem.Owner is ContextMenuStrip owner)
                {
                    thispicturebox = (PictureBox)owner.SourceControl;
                }
            }
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
                this.DoDragDrop(Clipboard.GetDataObject(), DragDropEffects.All);
            }
            Resize_imagepanels();
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // left and right arrow keys for changing image
            if ((panelfocus == 0) && pictureBox_leftpanel.Image != null && (keyData == Keys.Left))
            {
                int nextImageIndex = imageIndexLeftPanel - 1;
                if (nextImageIndex < 0) nextImageIndex = imageCountLeftPanel - 1;
                if (imageFilesLeftPanel[nextImageIndex] != null)
                {
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
                Resize_imagepanels();
                return true;
            }
            if ((panelfocus == 0) && pictureBox_leftpanel.Image != null && (keyData == Keys.Right))
            {
                int nextImageIndex = imageIndexLeftPanel + 1;
                if (nextImageIndex >= imageCountLeftPanel) nextImageIndex = 0;
                if (imageFilesLeftPanel[nextImageIndex] != null)
                {
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
                Resize_imagepanels();
                return true;
            }
            if ((panelfocus == 1) && pictureBox_rightpanel.Image != null && (keyData == Keys.Left))
            {
                int nextImageIndex = imageIndexRightPanel - 1;
                if (nextImageIndex < 0) nextImageIndex = imageCountRightPanel - 1;
                if (imageFilesRightPanel[nextImageIndex] != null)
                {
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
                Resize_imagepanels();
                return true;
            }
            if ((panelfocus == 1) && pictureBox_rightpanel.Image != null && (keyData == Keys.Right))
            {
                int nextImageIndex = imageIndexRightPanel + 1;
                if (nextImageIndex >= imageCountRightPanel) nextImageIndex = 0;
                if (imageFilesRightPanel[nextImageIndex] != null)
                {
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
                Resize_imagepanels();
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

            // C hotkey for randomize both panels
            if (keyData == Keys.C)
            {
                LoadRandomImage(0); LoadRandomImage(1);
                return true;
            }

            // Alt + C hotkey for jump back both panels
            if (keyData == (Keys.Alt | Keys.C))
            {
                JumpBack(0); JumpBack(1);
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

        private void Button_verticalhorizontal_Click(object sender, EventArgs e)
        {
            splitContainer_bothimages.Orientation = (splitContainer_bothimages.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical);
            button_verticalhorizontal.Text = (splitContainer_bothimages.Orientation == Orientation.Vertical ? "Stack images vertically" : "Put images side by side");
            Resize_imagepanels();
        }

        private void Button_swapimages_Click(object sender, EventArgs e)
        {
            Bitmap image_tempswap = (pictureBox_leftpanel.Image == null ? null : new Bitmap(pictureBox_leftpanel.Image));
            pictureBox_leftpanel.Image = (pictureBox_rightpanel.Image == null ? null : new Bitmap(pictureBox_rightpanel.Image));
            pictureBox_rightpanel.Image = image_tempswap;
            Resize_imagepanels();
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

        private void RotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(FindControlAtCursor(this) is PictureBox thispicturebox)) return;
            if (!(thispicturebox.Image is null))
            {
                Image img = thispicturebox.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                thispicturebox.Image = img;
            }
        }

        private void LoadPreviousImage(int activePanel)
        {
            if (activePanel == 0 && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null)
                {
                    int nextImageIndex = imageIndexLeftPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountLeftPanel - 1;
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
                Resize_imagepanels();
            }
            if (activePanel == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null)
                {
                    int nextImageIndex = imageIndexRightPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountRightPanel - 1;
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
                Resize_imagepanels();
            }
        }

        private void PreviousLeftArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPreviousImage(panelfocus);
        }

        private void LoadNextImage(int activePanel)
        {
            if (activePanel == 0 && pictureBox_leftpanel.Image != null)
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
            if (activePanel == 1 && pictureBox_rightpanel.Image != null)
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
            Resize_imagepanels();
        }

        private void NextRightArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadNextImage(panelfocus);
        }

        private void BlurBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(FindControlAtCursor(this) is PictureBox thispicturebox)) return;
            if (!(thispicturebox.Image is null))
            {
                var blur = new GaussianBlur(thispicturebox.Image as Bitmap);
                Bitmap result = blur.Process(bluramount);
                thispicturebox.Image = result;
            }
        }

        //https://stackoverflow.com/questions/16022188/open-an-image-with-the-windows-default-editor-in-c-sharp
        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panelfocus == 0 && pictureBox_leftpanel.Image != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(imageFilesLeftPanel[imageIndexLeftPanel])
                {
                    Verb = "edit"
                };
                Process.Start(startInfo);
            }
            if (panelfocus == 1 && pictureBox_rightpanel.Image != null)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(imageFilesRightPanel[imageIndexRightPanel])
                {
                    Verb = "edit"
                };
                Process.Start(startInfo);
            }
        }

        //https://stackoverflow.com/questions/9646114/open-file-location
        private void OpenFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
        private void SendToTrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (panelfocus == 0 && imageFilesLeftPanel[imageIndexLeftPanel] != null)
            {
                FileSystem.DeleteFile(imageFilesLeftPanel[imageIndexLeftPanel],
            Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
            Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                imageFilesLeftPanel.RemoveAt(imageIndexLeftPanel);
                imageCountLeftPanel--;
                PreviousLeftArrowToolStripMenuItem_Click(sender, e);
            }
            if (panelfocus == 1 && imageFilesRightPanel[imageIndexRightPanel] != null)
            {
                FileSystem.DeleteFile(imageFilesRightPanel[imageIndexRightPanel],
            Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
            Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                imageFilesRightPanel.RemoveAt(imageIndexRightPanel);
                imageCountRightPanel--;
                PreviousLeftArrowToolStripMenuItem_Click(sender, e);
            }
        }

        private void JumpBack(int activepanel)
        {
            if (activepanel == 0 && imageFilesLeftPanel != null)
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
            if (activepanel == 1 && imageFilesRightPanel != null)
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

        private void JumpBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JumpBack(panelfocus);
        }

        //https://stackoverflow.com/questions/2706500/how-do-i-generate-a-random-int-number
        private static readonly Random getrandom = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }

        private void LoadRandomImage(int activePanel)
        {
            if (activePanel == 0 && imageCountLeftPanel > 1)
            {
                int randomindex = imageIndexLeftPanel;
                for (int i = 0; i < 10 && randomindex == imageIndexLeftPanel; i++)
                {
                    randomindex = GetRandomNumber(0, imageCountLeftPanel);
                }
                try
                {
                    using (var bmpTemp = new Bitmap(imageFilesLeftPanel[randomindex]))
                    {
                        pictureBox_leftpanel.Image = new Bitmap(bmpTemp);
                    }
                    previmageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = randomindex;
                }
                catch (Exception) { throw; }
            }
            if (activePanel == 1 && imageCountRightPanel > 1)
            {
                int randomindex = imageIndexRightPanel;
                for (int i = 0; i < 10 && randomindex == imageIndexRightPanel; i++)
                {
                    randomindex = GetRandomNumber(0, imageCountRightPanel);
                }
                try
                {
                    using (var bmpTemp = new Bitmap(imageFilesRightPanel[randomindex]))
                    {
                        pictureBox_rightpanel.Image = new Bitmap(bmpTemp);
                    }
                    previmageIndexRightPanel = imageIndexRightPanel;
                    imageIndexRightPanel = randomindex;
                }
                catch (Exception) { throw; }
            }
        }

        private void RandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadRandomImage(panelfocus);
        }
    } // end MainWindow : Form
}