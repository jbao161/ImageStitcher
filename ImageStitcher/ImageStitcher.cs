using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ImageStitcher
{
    public partial class MainWindow : Form
    {
        // begin https://www.codeproject.com/tips/472294/position-a-windows-forms-messagebox-in-csharp
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(IntPtr classname, string title); // extern method: FindWindow

        [DllImport("user32.dll")]
        private static extern void MoveWindow(IntPtr hwnd, int X, int Y,
            int nWidth, int nHeight, bool rePaint); // extern method: MoveWindow

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect
            (IntPtr hwnd, out Rectangle rect); // extern method: GetWindowRect

        // end https://www.codeproject.com/tips/472294/position-a-windows-forms-messagebox-in-csharp

        private readonly bool debugflag = true;
        private static int activePanel = 0;
        private static readonly int bluramount = 10;

        // private static readonly int maxLengthFileList = (int)1.0e9;
        private void MsgDebug(string message)
        {
            if (debugflag) Console.WriteLine(message);
        }

        /* Section 1 : Find a control at mouse location
         * this is to find the picture being right clicked on when we get the copy paste context menu
         * https://stackoverflow.com/a/16543294
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
            UpdateLabelImageIndex();
        }

        private void UpdateLabelImageIndex()
        {
            label_imageindex_leftpanel.Text = (imageIndexLeftPanel == 0 && imageCountLeftPanel == 0) ? "" : ((imageIndexLeftPanel + 1) + "/" + imageCountLeftPanel);
            label_imageindex_rightpanel.Text = (imageIndexRightPanel == 0 && imageCountRightPanel == 0) ? "" : ((imageIndexRightPanel + 1) + "/" + imageCountRightPanel);
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
        // https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock

        private void DragDropHandler(int targetPanel, string[] filepaths)
        {
            bool isFolder = File.GetAttributes(filepaths[0]).HasFlag(FileAttributes.Directory);
            bool isImage = allowedImageExtensions.Any(filepaths[0].ToLower().EndsWith);
            string folderPath = Path.GetDirectoryName(filepaths[0]);
            if (isFolder) { folderPath = filepaths[0]; }
            List<string> imageList = null;
            if (isFolder | isImage)
            {
                // set the pseudo-focus on the left or right panel
                // then enumerate a list of all image files in the same directory as the loaded image
                // then store the position of the loaded image in that list

                imageList = EnumerateImageFiles(folderPath, allowedImageExtensions, isFolder);
                int imageCount = imageList.Count();
                int imageIndex = 0;
                string imagepath = filepaths[0];

                if (isFolder) { imageIndex = 0; imagepath = imageList[0]; }

                if (LoadImage(targetPanel, imagepath))
                {
                    for (int i = 0; i < imageCount; i++)
                    {
                        if (imageList[i] == filepaths[0]) { imageIndex = i; }
                    }

                    if (targetPanel == 0)
                    {
                        imageFilesLeftPanel = imageList;
                        imageCountLeftPanel = imageCount;
                        imageIndexLeftPanel = imageIndex;
                        priorimageIndexLeftPanel = imageIndex;
                    }
                    if (targetPanel == 1)
                    {
                        imageFilesRightPanel = imageList;
                        imageCountRightPanel = imageCount;
                        imageIndexRightPanel = imageIndex;
                        priorimageIndexRightPanel = imageIndex;
                    }
                }
                Resize_imagepanels();
                UpdateLabelImageIndex();
            }
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox activePictureBox = null;
            if (FindControlAtCursor(this) is PictureBox box) { activePictureBox = box; }

            if (activePictureBox == pictureBox_leftpanel) { activePanel = 0; }
            if (activePictureBox == pictureBox_rightpanel) { activePanel = 1; }
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            DragDropHandler(activePanel, s);
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
            Form form = new Form
            {
                Text = "Preview"
            };
            if (!(stitchedimage is null)) try
                {
                    var width = Screen.PrimaryScreen.Bounds.Width;
                    var height = Screen.PrimaryScreen.Bounds.Width;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ClientSize = stitchedimage.Size;
                    // https://stackoverflow.com/questions/21273520/how-do-i-startposition-of-the-form-from-the-current-mouse-cursor-position
                    if (form.ClientSize.Height > 0.9 * height || form.ClientSize.Width > width)
                    {
                        form.WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        // center the image on screen
                        form.StartPosition = FormStartPosition.Manual;
                        form.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - form.Width) / 2,
                        (Screen.PrimaryScreen.WorkingArea.Height - form.Height) / 2);
                        // Nudge the image so it is always underneath the mouse cursor and immediately clickable.
                        if (Cursor.Position.X < form.Left)
                        {
                            form.Left = Cursor.Position.X - 20;
                            //MessageBox.Show("nudging image to the left.");
                        }
                        else if (Cursor.Position.X > form.Right)
                        {
                            form.Left = (Cursor.Position.X - form.ClientSize.Width);
                            // MessageBox.Show("nudging image to the right.");
                        }
                        if (Cursor.Position.Y < form.Top)
                        {
                            form.Top = Cursor.Position.Y;
                            // MessageBox.Show("nudging the image up.");
                        }
                        else if (Cursor.Position.Y > form.Bottom)
                        {
                            form.Top = (Cursor.Position.Y - form.ClientSize.Height);
                            // MessageBox.Show("nudging the image down.");
                        }
                    }

                    PictureBox pb = new PictureBox
                    {
                        Dock = DockStyle.Fill,
                        Image = stitchedimage,
                        SizeMode = PictureBoxSizeMode.Zoom
                    };
                    void PreviewFormClose(object sen, EventArgs eve)
                    {
                        form.Close();
                    }
                    pb.MouseClick += PreviewFormClose;
                    form.Controls.Add(pb);
                    //Subscribe for event using designer or in constructor or form load
                    form.Show();
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

        private void ClearPanel(int targetPanel)
        {
            if (targetPanel == 0)
            {
                pictureBox_leftpanel.Image = null;
                imageFilesLeftPanel = null;
                imageCountLeftPanel = 0;
                imageIndexLeftPanel = 0;
                priorimageIndexLeftPanel = imageIndexLeftPanel;
            }
            if (targetPanel == 1)
            {
                pictureBox_rightpanel.Image = null;
                imageFilesRightPanel = null;
                imageCountRightPanel = 0;
                imageIndexRightPanel = 0;
                priorimageIndexRightPanel = imageIndexRightPanel;
            }
            UpdateLabelImageIndex();
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
            ClearPanel(activePanel);
        }// end section 2

        /* Section 3: Context menu for copy and paste
         */

        // open a copy paste menu at right click mouse location
        private void Control_MouseClick_copypastemenu(object sender, MouseEventArgs e)
        {
            PictureBox thispicturebox = FindControlAtCursor(this) as PictureBox;
            if (thispicturebox == pictureBox_leftpanel) { activePanel = 0; }
            if (thispicturebox == pictureBox_rightpanel) { activePanel = 1; }
            if ((MainWindow.ModifierKeys == Keys.Alt) && e.Button == MouseButtons.Left)
            {
                JumpBack(activePanel);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                if (checkBox_randomOnClick.Checked) { RandomToolStripMenuItem_Click(sender, e); }
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                contextmenufocus = activePanel;
                contextMenu_image.Show((Control)sender, e.X, e.Y);
                return;
            }
        }

        // https://stackoverflow.com/questions/2077981/cut-files-to-clipboard-in-c-sharp
        private void PutFilesOnClipboard(StringCollection filesAndFolders, bool moveFilesOnPaste = false)
        {
            var dropEffect = moveFilesOnPaste ? DragDropEffects.Move : DragDropEffects.Copy;

            var data = new DataObject();
            data.SetFileDropList(filesAndFolders);
            data.SetData("Preferred Dropeffect", new MemoryStream(BitConverter.GetBytes((int)dropEffect)));
            Clipboard.SetDataObject(data);
        }
        private void Removefromlist(int targetpanel)
        {
            if (targetpanel == 0)
            {
                try
                {
                    imageFilesLeftPanel.RemoveAt(imageIndexLeftPanel);
                    imageCountLeftPanel--;
                }
                catch (Exception) { throw; }
                LoadPreviousImage(targetpanel);
            }
            if (targetpanel == 1)
            {
                try
                {
                    imageFilesRightPanel.RemoveAt(imageIndexRightPanel);
                    imageCountRightPanel--;
                }
                catch (Exception) { throw; }
                LoadPreviousImage(targetpanel);
            }
        }
        private void Copycut( int targetPanel, bool bool_movefiles)
        {
            PictureBox thispicturebox = targetPanel == 0 ? pictureBox_leftpanel : pictureBox_rightpanel;

            if (!(thispicturebox.Image is null))
            {
                DataObject dobj = new DataObject();
                dobj.SetData(DataFormats.Bitmap, true, thispicturebox.Image);
                Clipboard.SetDataObject(dobj, true); // if shift is held, store in clipboard as image content

                if (!((Control.ModifierKeys & Keys.Shift) == Keys.Shift)) // if shift is not held at click, store in clipboard as filepath
                {
                    // https://stackoverflow.com/questions/211611/copy-files-to-clipboard-in-c-sharp
                    StringCollection paths = new StringCollection();
                    if (targetPanel == 0 && !(imageFilesLeftPanel is null) && imageCountLeftPanel != 0)
                    {
                        paths.Add(imageFilesLeftPanel[imageIndexLeftPanel]);
                        PutFilesOnClipboard(paths, bool_movefiles);
                    }
                    if (targetPanel == 1 && !(imageFilesRightPanel is null) && imageCountRightPanel != 0)
                    {
                        paths.Add(imageFilesRightPanel[imageIndexRightPanel]);
                        PutFilesOnClipboard(paths, bool_movefiles);
                    }
                }
            }
        }

        // copy image to clipboard from panel
        private void ContextMenu_image_item_copy_Click(object sender, EventArgs e)
        {
            bool movefiles = false;
            Copycut(contextmenufocus, movefiles);
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

        //https://stackoverflow.com/questions/4886327/determine-what-control-the-contextmenustrip-was-used-on
        private static int contextmenufocus = 0;

        private void ContextMenuStrip_Opened(object sender, EventArgs e)
        {
            contextmenufocus = activePanel;
        }

        // paste image to panel from file or clipboard
        private void ContextMenu_image_item_paste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                object o = Clipboard.GetImage();
                if (o != null)
                {
                    if (contextmenufocus == 0) { pictureBox_leftpanel.Image = (Image)o; }
                    if (contextmenufocus == 1) { pictureBox_rightpanel.Image = (Image)o; }
                }
            }
            if (Clipboard.GetDataObject().GetDataPresent("FileDrop"))
            {
                string[] s = (string[])Clipboard.GetDataObject().GetData(DataFormats.FileDrop, false);
                DragDropHandler(contextmenufocus, s);
            }
            Resize_imagepanels();
        }

        // end section 3

        /*  Section 4: Keyboard shortcuts 
        */
        // Local field to store the files enumerator;

        private static List<string> imageFilesLeftPanel;
        private static List<string> imageFilesRightPanel;
        private static int imageIndexLeftPanel = 0;
        private static int imageIndexRightPanel = 0;
        private static int priorimageIndexLeftPanel = 0;
        private static int priorimageIndexRightPanel = 0;
        private static int imageCountLeftPanel = 0;
        private static int imageCountRightPanel = 0;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // left and right arrow keys for changing image
            if (keyData == Keys.Left) { LoadPreviousImage(activePanel); return true; }
            if (keyData == Keys.Right) { LoadNextImage(activePanel); return true; }

            // R hotkey for rotations
            if (keyData == Keys.R) { RotateImage(activePanel); return true; }

            // B hotkey for blur
            if ((keyData == Keys.B)) { Blur(activePanel); return true; }

            // C hotkey for randomize both panels
            if (keyData == Keys.C) { LoadRandomImage(0); LoadRandomImage(1); return true; }

            // Alt + C hotkey for jump back both panels
            if (keyData == (Keys.Alt | Keys.C)) { JumpBack(0); JumpBack(1); return true; }

            // Delete hotkey for send to recycle
            if ((keyData == Keys.Delete)) { SendToTrash(activePanel); return true; }

            // Ctrl + X for cut
            if (keyData == (Keys.Control | Keys.X)) { Copycut(activePanel,true);  return true; }

            // Ctrl + Shift + X for cut and remove from list
            if (keyData == (Keys.Control | Keys.Shift | Keys.X)) { Copycut(activePanel, true); Removefromlist(activePanel);  return true; }

            // end of hotkeys. ignore the keystroke
            else { return base.ProcessCmdKey(ref msg, keyData); }
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
            priorimageIndexLeftPanel = imageIndexLeftPanel;
            imageIndexLeftPanel = imageIndexRightPanel;
            priorimageIndexRightPanel = imageIndexRightPanel;
            imageIndexRightPanel = tmp2;
            var tmp3 = imageCountLeftPanel;
            imageCountLeftPanel = imageCountRightPanel;
            imageCountRightPanel = tmp3;
            UpdateLabelImageIndex();
        }

        private void RotateImage(int targetPanel)
        {
            if ((targetPanel == 0) && pictureBox_leftpanel.Image != null)
            {
                Image img = pictureBox_leftpanel.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox_leftpanel.Image = img;
                Resize_imagepanels();
            }
            if ((targetPanel == 1) && pictureBox_rightpanel.Image != null)
            {
                Image img = pictureBox_rightpanel.Image;
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureBox_rightpanel.Image = img;
                Resize_imagepanels();
            }
        }

        private void RotateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotateImage(activePanel);
        }

        private void LoadPreviousImage(int targetPanel)
        {
            if (targetPanel == 0 && pictureBox_leftpanel.Image != null )
            {
                if (imageFilesLeftPanel != null && imageCountLeftPanel != 0)
                {
                    int nextImageIndex = imageIndexLeftPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountLeftPanel - 1;
                    if (LoadImage(targetPanel, imageFilesLeftPanel[nextImageIndex]))
                    {
                        priorimageIndexLeftPanel = imageIndexLeftPanel;
                        imageIndexLeftPanel = nextImageIndex;
                    }
                }
                Resize_imagepanels();
            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null )
            {
                if (imageFilesRightPanel != null && imageCountRightPanel!= 0)
                {
                    int nextImageIndex = imageIndexRightPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex =  imageCountRightPanel - 1;

                    if (LoadImage(targetPanel, imageFilesRightPanel[nextImageIndex]))
                    {
                        priorimageIndexRightPanel = imageIndexRightPanel;
                        imageIndexRightPanel = nextImageIndex;
                    }
                }
                Resize_imagepanels();
            }
            UpdateLabelImageIndex();
        }

        private void PreviousLeftArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPreviousImage(activePanel);
        }

        private void LoadNextImage(int targetPanel)
        {
            if (targetPanel == 0 && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null && imageCountLeftPanel != 0)
                {
                    int nextImageIndex = imageIndexLeftPanel + 1;
                    if (nextImageIndex >= imageCountLeftPanel) nextImageIndex = 0;

                    if (LoadImage(targetPanel, imageFilesLeftPanel[nextImageIndex]))
                    {
                        priorimageIndexLeftPanel = imageIndexLeftPanel;
                        imageIndexLeftPanel = nextImageIndex;
                    }
                }
            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null && imageCountRightPanel !=0)
                {
                    int nextImageIndex = imageIndexRightPanel + 1;
                    if (nextImageIndex >= imageCountRightPanel) nextImageIndex = 0;
                    if (LoadImage(targetPanel, imageFilesRightPanel[nextImageIndex]))
                    {
                        priorimageIndexRightPanel = imageIndexRightPanel;
                        imageIndexRightPanel = nextImageIndex;
                    }
                }
            }
            Resize_imagepanels();
            UpdateLabelImageIndex();
        }

        private void NextRightArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadNextImage(activePanel);
        }

        private void Blur(int targetPanel)
        {
            if ((targetPanel == 0) && pictureBox_leftpanel.Image != null)
            {
                var blur = new GaussianBlur(pictureBox_leftpanel.Image as Bitmap);
                Bitmap result = blur.Process(bluramount);
                pictureBox_leftpanel.Image = result;
            }
            if ((targetPanel == 1) && pictureBox_rightpanel.Image != null)
            {
                var blur = new GaussianBlur(pictureBox_rightpanel.Image as Bitmap);
                Bitmap result = blur.Process(bluramount);
                pictureBox_rightpanel.Image = result;
            }
        }

        private void BlurBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Blur(activePanel);
        }

        //https://stackoverflow.com/questions/16022188/open-an-image-with-the-windows-default-editor-in-c-sharp
        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (activePanel == 0 && pictureBox_leftpanel.Image != null && imageCountLeftPanel != 0)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(imageFilesLeftPanel[imageIndexLeftPanel])
                {
                    Verb = "edit"
                };
                Process.Start(startInfo);
            }
            if (activePanel == 1 && pictureBox_rightpanel.Image != null && imageCountRightPanel != 0)
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
            if (activePanel == 0 && imageFilesLeftPanel != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                Process.Start("explorer.exe", "/select, " + imageFilesLeftPanel[imageIndexLeftPanel]);
            }
            if (activePanel == 1 && imageFilesRightPanel != null && imageFilesRightPanel != null && imageCountRightPanel !=0)
            {
                Process.Start("explorer.exe", "/select, " + imageFilesRightPanel[imageIndexRightPanel]);
            }
        }

        //https://stackoverflow.com/questions/3282418/send-a-file-to-the-recycle-bin
        //https://www.c-sharpcorner.com/UploadFile/mahesh/how-to-remove-an-item-from-a-C-Sharp-list/

        private void SendToTrash(int targetPanel)
        {
            if (targetPanel == 0 && pictureBox_leftpanel.Image != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                try
                {
                    FileSystem.DeleteFile(imageFilesLeftPanel[imageIndexLeftPanel],
                        Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                        Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                }
                catch (Exception) { throw; }
            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null && imageFilesRightPanel != null && imageCountRightPanel !=0)
            {
                try
                {
                    FileSystem.DeleteFile(imageFilesRightPanel[imageIndexRightPanel],
                        Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                        Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            Removefromlist(targetPanel);
            UpdateLabelImageIndex();
        }

        private void SendToTrashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendToTrash(activePanel);
        }

        private void JumpBack(int targetPanel)
        {
            if (targetPanel == 0 && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                if (LoadImage(targetPanel, imageFilesLeftPanel[priorimageIndexLeftPanel]))
                {
                    int swapImageIndex = imageIndexLeftPanel;
                    imageIndexLeftPanel = priorimageIndexLeftPanel;
                    priorimageIndexLeftPanel = swapImageIndex;
                }
            }
            if (targetPanel == 1 && imageFilesRightPanel != null && imageCountRightPanel!=0)
            {
                if (LoadImage(targetPanel, imageFilesRightPanel[priorimageIndexRightPanel]))
                {
                    int swapImageIndex = imageIndexRightPanel;
                    imageIndexRightPanel = priorimageIndexRightPanel;
                    priorimageIndexRightPanel = swapImageIndex;
                }
            }
            Resize_imagepanels();
            UpdateLabelImageIndex();
        }

        private void JumpBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            JumpBack(activePanel);
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

        //https://www.codeproject.com/tips/472294/position-a-windows-forms-messagebox-in-csharp
        private void FindAndMoveMsgBox(int x, int y, bool repaint, string title)
        {
            Thread thr = new Thread(() => // create a new thread
            {
                IntPtr msgBox = IntPtr.Zero;
                // while there's no MessageBox, FindWindow returns IntPtr.Zero
                while ((msgBox = FindWindow(IntPtr.Zero, title)) == IntPtr.Zero) ;
                // after the while loop, msgBox is the handle of your MessageBox
                Rectangle r = new Rectangle();
                GetWindowRect(msgBox, out r); // Gets the rectangle of the message box
                MoveWindow(msgBox /* handle of the message box */, x, y,
                   r.Width - r.X /* width of originally message box */,
                   r.Height - r.Y /* height of originally message box */,
                   repaint /* if true, the message box repaints */);
            });
            thr.Start(); // starts the thread
        }

        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine );
        }
        private void WriteTextOnImage(PictureBox targetpicturebox, String text)
        {
            Bitmap bmp = new Bitmap(600, 600);
            string tmptext = SpliceText(text, 45);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 600, 600);
                graph.FillRectangle(Brushes.Gray, ImageSize);
                graph.DrawString(tmptext,
                new Font("Arial", 16),
                new SolidBrush(Color.Black),
                50, 200,
                new System.Drawing.StringFormat());
            }
            targetpicturebox.Image = bmp;
        }
        private bool LoadImage(int targetPanel, string imagePath)
        {
            try
            {
                using ( var bmpTemp = new Bitmap(imagePath))
                {
                    if (targetPanel == 0) { pictureBox_leftpanel.Image = new Bitmap(bmpTemp); }
                    if (targetPanel == 1) { pictureBox_rightpanel.Image = new Bitmap(bmpTemp); }
                    return true;
                }
            }
            catch (Exception)
            {
                //FindAndMoveMsgBox(Cursor.Position.X - 250, Cursor.Position.Y - 120, true, "Attention"); // can't find the dimensions of MessageBox!
                //System.Windows.Forms.MessageBox.Show("Failed to load. Image is corrupt or missing: " + imagePath, "Attention");
                if (targetPanel == 0) { WriteTextOnImage(pictureBox_leftpanel, "Failed to load. Image is corrupt or missing: " + imagePath); }
                if (targetPanel == 1) { WriteTextOnImage(pictureBox_rightpanel, "Failed to load. Image is corrupt or missing: " + imagePath); }
                return true; // supposed to return false, but i want to load image path anyways so i can delete corrupted images --- too lazy to fix behavior in the usage right now
            }
        }

        private void LoadRandomImage(int targetPanel)
        {
            if (targetPanel == 0 && imageCountLeftPanel > 1)
            {
                int randomindex = imageIndexLeftPanel;
                for (int i = 0; i < 10 && randomindex == imageIndexLeftPanel; i++)
                {
                    randomindex = GetRandomNumber(0, imageCountLeftPanel);
                }
                if (LoadImage(targetPanel, imageFilesLeftPanel[randomindex]))
                {
                    priorimageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = randomindex;
                }
            }
            if (targetPanel == 1 && imageCountRightPanel > 1)
            {
                int randomindex = imageIndexRightPanel;
                for (int i = 0; i < 10 && randomindex == imageIndexRightPanel; i++)
                {
                    randomindex = GetRandomNumber(0, imageCountRightPanel);
                }
                if (LoadImage(targetPanel, imageFilesRightPanel[randomindex]))
                {
                    priorimageIndexRightPanel = imageIndexRightPanel;
                    imageIndexRightPanel = randomindex;
                }
            }
            Resize_imagepanels();
            UpdateLabelImageIndex();
        }

        private void RandomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadRandomImage(activePanel);
        }

        private void Button_releaseleft_MouseClick(object sender, MouseEventArgs e)
        {
            ClearPanel(0);
        }

        private void Button_releaseright_MouseClick(object sender, MouseEventArgs e)
        {
            ClearPanel(1);
        }

        private void MirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((activePanel == 0) && pictureBox_leftpanel.Image != null)
            {
                Image img = pictureBox_leftpanel.Image;
                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox_leftpanel.Image = img;
            }
            if ((activePanel == 1) && pictureBox_rightpanel.Image != null)
            {
                Image img = pictureBox_rightpanel.Image;
                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox_rightpanel.Image = img;
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool movefiles = true;
            Copycut(contextmenufocus, movefiles);
        }

        private void removeFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Removefromlist(contextmenufocus);
        }
    } // end MainWindow : Form
}