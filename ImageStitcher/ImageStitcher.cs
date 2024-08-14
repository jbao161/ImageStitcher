using ImageStitcher.Properties;

using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;

using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Shapes;
using System.Windows.Threading;

using CRNG = System.Security.Cryptography.RandomNumberGenerator;
using Path = System.IO.Path;

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
            (IntPtr hwnd, out System.Windows.Shapes.Rectangle rect); // extern method: GetWindowRect

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
        private void FocusPanelAtCursor(Form form)
        {
            if (FindControlAtCursor(form) is PictureBox box)
            {
                if (box == pictureBox_leftpanel) { activePanel = 0; }
                if (box == pictureBox_rightpanel) { activePanel = 1; }
            }
            if (FindControlAtCursor(form) is Panel panel)
            {
                if (panel == splitContainer_bothimages.Panel1) activePanel = 0;
                if (panel == splitContainer_bothimages.Panel2) activePanel = 1;
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            UpdateLabelImageIndex();
        }

        public MainWindow(string filepath)
        {
            InitializeComponent();

            UpdateLabelImageIndex();
            DragDropHandler(0, new String[] { filepath });
        }

        private void UpdateLabelImageIndex()
        {
            label_imageindex_leftpanel.Text = (imageIndexLeftPanel == 0 && imageCountLeftPanel == 0) ? "" : ((imageIndexLeftPanel + 1) + "/" + imageCountLeftPanel);
            label_imageindex_rightpanel.Text = (imageIndexRightPanel == 0 && imageCountRightPanel == 0) ? "" : ((imageIndexRightPanel + 1) + "/" + imageCountRightPanel);

            if (checkBox_showfilename.Checked)
            {
                try { 
                label_filename_leftpanel.Text = (imageIndexLeftPanel == 0 && imageCountLeftPanel == 0) ? "" : " " + (System.IO.Path.GetFileName(imageFilesLeftPanel[imageIndexLeftPanel])) + " " +
                    (Utils.GetFileSizeString(imageFilesLeftPanel[imageIndexLeftPanel])) + " " +
                    Utils.GetDimensionString(pictureBox_leftpanel.Image);
                label_filename_rightpanel.Text = (imageIndexRightPanel == 0 && imageCountRightPanel == 0) ? "" : " " + (System.IO.Path.GetFileName(imageFilesRightPanel[imageIndexRightPanel])) + " " +
                    (Utils.GetFileSizeString(imageFilesRightPanel[imageIndexRightPanel])) + " " +
                    Utils.GetDimensionString(pictureBox_rightpanel.Image); ; }
                catch { }
            }
            else
            {
                label_filename_leftpanel.Text = "";
                label_filename_rightpanel.Text = "";
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            pictureBox_rightpanel.AllowDrop = true;
            pictureBox_leftpanel.AllowDrop = true;

            label_filename_leftpanel.Location = pictureBox_leftpanel.PointToClient(label_filename_leftpanel.Parent.PointToScreen(label_filename_leftpanel.Location));
            label_filename_leftpanel.Parent = pictureBox_leftpanel;
            label_filename_leftpanel.Text = "";
            //label_filename_leftpanel.BackColor = System.Drawing.Color.Transparent;
            label_filename_rightpanel.Location = pictureBox_rightpanel.PointToClient(label_filename_rightpanel.Parent.PointToScreen(label_filename_rightpanel.Location));
            label_filename_rightpanel.Parent = pictureBox_rightpanel;
            //label_filename_rightpanel.BackColor = System.Drawing.Color.Transparent;
            label_filename_rightpanel.Text = "";


            // Load settings
            try
            {// https://www.codeproject.com/Articles/30216/Handling-Corrupt-user-config-Settings

                if (Settings.Default.NeedsUpgrade)
                { // https://stackoverflow.com/questions/13772936/c-sharp-settings-file-how-to-load
                    Settings.Default.Upgrade();
                    Settings.Default.NeedsUpgrade = false;
                    Settings.Default.Save();
                }

                Settings.Default.Reload();


                //https://www.codeproject.com/Articles/15013/Windows-Forms-User-Settings-in-C
                // Set window location
                if (Settings.Default.WindowLocation != null)
                {
                    this.Location = Settings.Default.WindowLocation;
                    // If window location settings are corrupted, put the window back on screen
                    if (!Screen.FromControl(this).Bounds.Contains(this.Location))
                    {
                        this.DesktopLocation = new Point(0, 0);
                    }
                }

                // Set window size
                if (Settings.Default.WindowSize != null)
                {
                    this.Size = Settings.Default.WindowSize;
                }

                this.splitContainer_bothimages.SplitterDistance = Settings.Default.SplitContainerSplitterDistance;
                savesplitterdistance = Settings.Default.SplitContainerSplitterDistance;

                // load checkbox settings
                this.checkBox_randomOnClick.Checked = Settings.Default.RandomizeOnClick;
                this.checkBox_openaftersave.Checked = Settings.Default.OpenFolderAfterSave;
                this.checkBox_showfilename.Checked = Settings.Default.showinfo;
                this.checkBox_hotkeyboth.Checked = Settings.Default.HotkeyBoth;

                tmpAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ImageStitcher\\tmp\\";

                // try to load the last opened file, or its directory if file could not be opened
                if (Settings.Default.rememberLastFile && !String.IsNullOrEmpty(Settings.Default.LastFile) && imageCountLeftPanel == 0)
                {
                    try
                    {
                        DragDropHandler(0, new string[] { Settings.Default.LastFile });
                    }
                    catch (Exception)
                    {
                        try
                        {
                            DragDropHandler(0, new string[] { System.IO.Path.GetDirectoryName(Settings.Default.LastFile) });
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                // load default directory
                if (Settings.Default.loaddefaultdir && !String.IsNullOrEmpty(Settings.Default.DefaultDirectory) && imageCountLeftPanel == 0)
                {
                    try
                    {
                        // optionally run a custom external script
                        if (Settings.Default.scriptonload)
                        {
                            System.Diagnostics.Process process = new System.Diagnostics.Process();
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            process.StartInfo.FileName = "cmd.exe";
                            process.StartInfo.Arguments = "/C " + "\"" + Settings.Default.scriptloc + "\"";
                            process.Start();
                            if (Settings.Default.scriptwait) process.WaitForExit();
                        }
                        DragDropHandler(0, new string[] { Settings.Default.DefaultDirectory });
                        DragDropHandler(1, new string[] { Settings.Default.DefaultDirectory });
                    }
                    catch (Exception)
                    {
                    }
                }


                // set number of image panels
                set_NumberOfPanels(Settings.Default.NumberOfPanels);

                // apply dark light colors
                DarkModeRefresh();

                // deselect all elements


            }
            catch (ConfigurationErrorsException ex)
            { //(requires System.Configuration)
                string filename = ((ConfigurationErrorsException)ex.InnerException).Filename;

                if (System.Windows.MessageBox.Show("<ProgramName> has detected that your" +
                                      " user settings file has become corrupted. " +
                                      "This may be due to a crash or improper exiting" +
                                      " of the program. <ProgramName> must reset your " +
                                      "user settings in order to continue.\n\nClick" +
                                      " Yes to reset your user settings and continue.\n\n" +
                                      "Click No if you wish to attempt manual repair" +
                                      " or to rescue information before proceeding.",
                                      "Corrupt user settings",
                                      System.Windows.MessageBoxButton.YesNo,
                                      System.Windows.MessageBoxImage.Error) == System.Windows.MessageBoxResult.Yes)
                {
                    File.Delete(filename);
                    Settings.Default.Reload();
                    // you could optionally restart the app instead

                    Application.Restart();
                }
                else
                    Process.GetCurrentProcess().Kill();
                // avoid the inevitable crash
            }
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

        /* Drag and drop
         * picture files from Windows explorer onto the panel to load the image
         */
        // https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock

        private void DragDropHandler(int targetPanel, string[] filepaths)
        {
            //if (String.IsNullOrEmpty(filepaths[0])) return;
            try { 
            bool isFolder = File.GetAttributes(filepaths[0]).HasFlag(FileAttributes.Directory);
            bool isImage = allowedImageExtensions.Any(filepaths[0].ToLower().EndsWith);
            string folderPath = System.IO.Path.GetDirectoryName(filepaths[0]);
            if (isFolder) { folderPath = filepaths[0]; }
            List<string> imageList = new List<string>(); ;
            List<string> sublist = null;
            if (isFolder | isImage)
            {
                // set the pseudo-focus on the left or right panel
                // then enumerate a list of all image files in the same directory as the loaded image
                // then store the position of the loaded image in that list
                if (isFolder) { 
                    for (int i = 0; i< filepaths.Length; i++)
                {
                    sublist= EnumerateImageFiles(filepaths[i], allowedImageExtensions, Settings.Default.LoadSubfolders);
                    imageList.AddRange(sublist);
                }
                } else if (isImage)
                {
                     imageList = EnumerateImageFiles(folderPath, allowedImageExtensions, false);
                }
                imageList = imageList.OrderBy(System.IO.Path.GetFileName, StringComparer.InvariantCultureIgnoreCase).ToList();
                if (Settings.Default.ReverseFileOrder) imageList.Reverse();
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
            catch { }
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox activePictureBox = null;
            if (FindControlAtCursor(this) is PictureBox box) { activePictureBox = box; }
            if (activePictureBox == pictureBox_leftpanel) { activePanel = 0; }
            if (activePictureBox == pictureBox_rightpanel) { activePanel = 1; }
            if (FindControlAtCursor(this) is Panel panel)
            {
                if (panel == splitContainer_bothimages.Panel1) activePanel = 0;
                if (panel == splitContainer_bothimages.Panel2) activePanel = 1;
            }
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
            if (pictureBox_leftpanel.Image is null) splitContainer_bothimages.SplitterDistance = 0;
            if (pictureBox_rightpanel.Image is null) splitContainer_bothimages.SplitterDistance = panel_bothimages.Width;

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
            cleanupmemory();
        }

        /*  Create a new image by stitching the two panel images together
         */

        public class StitchSizeParams
        {
            public int width, height, rightImagePosition;
            public bool orientation;

            public StitchSizeParams(int stitchedWidth, int stitchedHeight, int rightImagePosition, bool sidebyside)
            {
                this.width = stitchedWidth; this.height = stitchedHeight; this.rightImagePosition = rightImagePosition; this.orientation = sidebyside;
            }

            public void setParams(int stitchedWidth, int stitchedHeight, int rightImagePosition, bool sidebyside)
            {
                this.width = stitchedWidth; this.height = stitchedHeight; this.rightImagePosition = rightImagePosition; this.orientation = sidebyside;
            }

            public StitchSizeParams()
            { }
        }

        private Bitmap Stitch_images()
        {
            return Stitch_images(new StitchSizeParams());
        }

        private Bitmap Stitch_images(StitchSizeParams stitchSize)
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
                    if (checkBox_screengrab.Checked)
                    {
                        stitchSize.setParams(splitContainer_bothimages.Panel1.Width + splitContainer_bothimages.Panel2.Width, splitContainer_bothimages.Panel1.Height, splitContainer_bothimages.Panel1.Width, true);
                        return Screen_stitch();
                    }
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
                    stitchSize.setParams(result_width, min_height, stitched_leftimg_width, true);
                    return stitchedimage;
                }

                if (this.splitContainer_bothimages.Orientation == Orientation.Horizontal) // left image becomes the one on top
                {
                    if (checkBox_screengrab.Checked)
                    {
                        stitchSize.setParams(splitContainer_bothimages.Panel1.Width, splitContainer_bothimages.Panel1.Height + splitContainer_bothimages.Panel2.Height, splitContainer_bothimages.Panel1.Height, false);
                        return Screen_stitch();
                    }
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
                    stitchSize.setParams(min_width, result_height, stitched_leftimg_height, false);
                    return stitchedimage;
                }
                return null;
            }
        }

        /*  Open the stitched image in a viewing window
         */

        private void Button_preview_Click(object sender, EventArgs e)
        {
            cleanupmemory();
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
            cleanupmemory();
            Bitmap stitchedimage = Stitch_images();
            if (!(stitchedimage is null))
            {
            }
            if (!(stitchedimage is null)) try
                {
                    // Displays a SaveFileDialog so the user can save the Image
                    saveFileDialog1.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
                    saveFileDialog1.Title = "Save an Image File";
                    saveFileDialog1.FileName = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " combined";                     // feature 1: timestamp
                    saveFileDialog1.RestoreDirectory = true;
                    bool isanimatedgif = false;
                    if (!(imageFilesLeftPanel is null) && imageCountLeftPanel != 0 && !(imageFilesRightPanel is null) && imageCountRightPanel != 0)
                    {

                        if (check_if_animated_gif(imageFilesLeftPanel[imageIndexLeftPanel])
                            || check_if_animated_gif(imageFilesRightPanel[imageIndexRightPanel]))
                        {
                            isanimatedgif = true;
                            saveFileDialog1.FilterIndex = 3;
                        }
                    }
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        // Saves the Image via a FileStream created by the OpenFile method.
                        System.IO.FileStream fs =
                           (System.IO.FileStream)saveFileDialog1.OpenFile();

                        if (!isanimatedgif)
                        {
                            // Saves the Image in the appropriate ImageFormat based upon the
                            // File type selected in the dialog box.
                            // NOTE that the FilterIndex property is one-based.

                            var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                            encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Settings.Default.ImageSaveQuality);

                            switch (saveFileDialog1.FilterIndex)
                            {
                                case 1:
                                    stitchedimage.Save(fs, GetEncoder(
                                       System.Drawing.Imaging.ImageFormat.Jpeg), encoderParameters);
                                    break;

                                case 2:
                                    stitchedimage.Save(fs, GetEncoder(
                                       System.Drawing.Imaging.ImageFormat.Bmp), encoderParameters);
                                    break;

                                case 3:
                                    stitchedimage.Save(fs, GetEncoder(
                                          System.Drawing.Imaging.ImageFormat.Gif), encoderParameters);
                                    break;

                                case 4:
                                    stitchedimage.Save(fs, GetEncoder(
                                          System.Drawing.Imaging.ImageFormat.Png), encoderParameters);
                                    break;
                            }
                        }
                        fs.Close();
                        if (isanimatedgif)
                        {
                            StitchAnimatedGif(System.IO.Path.GetFullPath(saveFileDialog1.FileName));
                        }
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
                    } // if dialog OK
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
        }
        private bool check_if_animated_gif(string imagepath)
        {
            // https://stackoverflow.com/questions/2848689/c-sharp-tell-static-gifs-apart-from-animated-ones
            using (Image image = Image.FromFile(imagepath))
            {
                if (ImageAnimator.CanAnimate(image))
                {
                    // GIF is animated
                    return true;
                }
                else
                {
                    // GIF is not animated
                    return false;
                }
            }
        }

        private Bitmap Screen_stitch()
        { // stitches a screenshot of the images as displayed in the viewer (i.e. with zoom, unsaved edit effects)
            cleanupmemory();
            Panel leftpanel = splitContainer_bothimages.Panel1;
            Panel rightpanel = splitContainer_bothimages.Panel2;
            Bitmap bmp1 = new Bitmap(leftpanel.Width, leftpanel.Height);
            splitContainer_bothimages.Panel1.DrawToBitmap(bmp1, new System.Drawing.Rectangle(0, 0, leftpanel.Width, leftpanel.Height));
            Bitmap bmp2 = new Bitmap(rightpanel.Width, rightpanel.Height);
            splitContainer_bothimages.Panel2.DrawToBitmap(bmp2, new System.Drawing.Rectangle(0, 0, rightpanel.Width, rightpanel.Height));
            if (this.splitContainer_bothimages.Orientation == Orientation.Vertical)
            {
                Bitmap stitchedimage = new Bitmap(bmp1.Width + bmp2.Width, bmp1.Height);
                using (Graphics g = Graphics.FromImage(stitchedimage))
                {
                    g.DrawImage(bmp1, 0, 0);
                    g.DrawImage(bmp2, bmp1.Width, 0);
                }
                return stitchedimage;
            }
            if (this.splitContainer_bothimages.Orientation == Orientation.Horizontal) // left image becomes the one on top
            {
                Bitmap stitchedimage = new Bitmap(bmp1.Width, bmp1.Height + bmp2.Height);
                using (Graphics g = Graphics.FromImage(stitchedimage))
                {
                    g.DrawImage(bmp1, 0, 0);
                    g.DrawImage(bmp2, 0, bmp1.Height);
                }
                return stitchedimage;
            }
            return null;
        }

        private void StitchAnimatedGif(string fileOutPath)
        {
            StitchSizeParams dim = new StitchSizeParams();
            _ = Stitch_images(dim);
            DirectoryInfo di = Directory.CreateDirectory(tmpAppDataPath);
            string tmpfilename = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " tmp.mp4";
            var tmpfilepath = tmpAppDataPath + tmpfilename;
            String deletetempfiles = $" && del  \"{tmpfilepath}\"";

            string leftimagepath = imageFilesLeftPanel[imageIndexLeftPanel];
            string rightimagepath = imageFilesRightPanel[imageIndexRightPanel];
            string twidth = (dim.width / 2 * 2).ToString();
            string theight = (dim.height / 2 * 2).ToString();

            bool sidebyside = true;
            string lwidth, rwidth, lheight, rheight, leftscale, rightscale, rightposition, videoEncoding;
            lwidth = rwidth = lheight = rheight = leftscale = rightscale = rightposition = videoEncoding = String.Empty;
            videoEncoding = $"-c:v libx264 -crf 0 -preset fast ";
            if (dim.orientation == sidebyside)
            {
                lwidth = (dim.rightImagePosition).ToString();
                rwidth = ((dim.width - dim.rightImagePosition)).ToString();

                leftscale = $"{lwidth}x{theight}";
                rightscale = $"{rwidth}x{theight}";
                rightposition = $"x={lwidth}";
            }
            if (dim.orientation != sidebyside)
            {
                lheight = (dim.rightImagePosition).ToString();
                rheight = ((dim.height - dim.rightImagePosition)).ToString();

                leftscale = $"{twidth}x{lheight}";
                rightscale = $"{twidth}x{rheight}";
                rightposition = $"x=0:y={lheight}";
            }

            TimeSpan durationl = TimeSpan.Zero;
            if (check_if_animated_gif(leftimagepath))
            {
                durationl = (TimeSpan)GifExtension.GetGifDuration(pictureBox_leftpanel.Image);
            }
            TimeSpan durationr = TimeSpan.Zero;
            if (check_if_animated_gif(rightimagepath))
            {
                durationr = (TimeSpan)GifExtension.GetGifDuration(pictureBox_rightpanel.Image);
            }

            string leftspeed = "1.0";
            string rightspeed = "1.0";
            string loopthisvideo = "-fflags +genpts -stream_loop -1";
            string leftloop = String.Empty;
            string rightloop = String.Empty;
            if (durationl > durationr) rightloop = loopthisvideo;
            if (durationr > durationl) leftloop = loopthisvideo;


            // If there is a still image, first convert it to a video with nonzero duration
            String stillimagepath = "";
            string tmpfilenamestill = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " tmpstill.mp4";
            var tmpfilepathstill = tmpAppDataPath + tmpfilenamestill;

            string stillduration = "";
            string ifstillimagepresent = "";
            if (durationl == TimeSpan.Zero)
            {
                stillimagepath = leftimagepath;
                leftimagepath = tmpfilepathstill;
                stillduration = durationr.TotalSeconds.ToString();
            }
            if (durationr == TimeSpan.Zero)
            {
                stillimagepath = rightimagepath;
                rightimagepath = tmpfilepathstill;
                stillduration = durationl.TotalSeconds.ToString();
            }
            if (durationl == TimeSpan.Zero || durationr == TimeSpan.Zero)
            {
                string stillimagecommand = $"ffmpeg -loop 1 -i \"{stillimagepath}\" -c:v libx264 -crf 18 -vf \"pad=ceil(iw/2)*2:ceil(ih/2)*2\" -r 30 -y -an -t {stillduration} -pix_fmt yuv420p \"{tmpfilepathstill}\" && ";
                ifstillimagepresent = stillimagecommand;
                deletetempfiles += $" && del  \"{tmpfilepathstill}\"";
            }
            // end still image section

            String joinasvideo = $"ffmpeg {leftloop} -i \"{leftimagepath}\" {rightloop} -i \"{rightimagepath}\" -filter_complex \"color=s={twidth}x{theight}:c=black:rate=60[base]; [0:v] setpts={leftspeed}*(PTS-STARTPTS), scale={leftscale}[left]; [1:v] setpts={rightspeed}*(PTS-STARTPTS), scale={rightscale}[right]; [base][left]overlay=shortest=1[tmp1]; [tmp1][right]overlay=shortest=1:{rightposition} \" {videoEncoding} \"{tmpfilepath}\"";

            // https://superuser.com/a/1256459 ffmpeg .mp4 to gif

            String converttogif = $" && ffmpeg -y -i \"{tmpfilepath}\" -filter_complex \"fps=10,scale=iw:ih:flags=lanczos, split [o1] [o2];[o1] palettegen=stats_mode=diff [p]; [o2] fifo [o3];[o3] [p] paletteuse=dither=sierra2_4a \" -r 10 \"{fileOutPath}\""; // -r 10 reduce sample rate for smaller file size
            string outputvisibility = "/C ";
            // use "/C "+ for cmd.exe to close automatically "/K "+ for cmd.exe to stay open and view ffmpeg output 
            string arg = outputvisibility + ifstillimagepresent + joinasvideo + converttogif + deletetempfiles;

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = arg,
                    UseShellExecute = false,
                    CreateNoWindow = false
                }
            };

            proc.Start();

            //proc.WaitForExit();//May need to wait for the process to exit too
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
            Resize_imagepanels();
            UpdateLabelImageIndex();
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearPanel(contextmenufocus);
        }// end section 2

        /* Section 3: Right click context menu commands
         */

        //https://stackoverflow.com/questions/4886327/determine-what-control-the-contextmenustrip-was-used-on
        private static int contextmenufocus = 0;

        private void ContextMenuStrip_Opened(object sender, EventArgs e)
        {
            contextmenufocus = activePanel;
        }

        private void contextMenu_image_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // disable 'fix webp image' when not applicable
            fixCorruptedImageToolStripMenuItem.Enabled = !(contextmenufocus == 0 && imageCountLeftPanel == 0) ||
                (contextmenufocus == 1 && imageCountRightPanel == 0) ||
            ((contextmenufocus == 0 && pictureBox_leftpanel.Image != null && imageCountLeftPanel != 0) &&
                System.IO.Path.GetExtension(imageFilesLeftPanel[imageIndexLeftPanel]).ToLower().EndsWith(".gif")) ||
            ((contextmenufocus == 1 && pictureBox_rightpanel.Image != null && imageCountRightPanel != 0) &&
                System.IO.Path.GetExtension(imageFilesRightPanel[imageIndexRightPanel]).ToLower().EndsWith(".gif"));
        }

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
            if (targetpanel == 0 && imageCountLeftPanel == 0 || targetpanel == 1 && imageCountRightPanel == 0) { return; }
            if (targetpanel == 0)
            {
                try
                {
                    imageFilesLeftPanel.RemoveAt(imageIndexLeftPanel);
                    imageCountLeftPanel -= 1;
                }
                catch (Exception) { throw; }
                int restorepriorimageindex = priorimageIndexLeftPanel;
                imageIndexLeftPanel -= 1;
                if (imageIndexLeftPanel < 0) imageIndexLeftPanel = 0;
                LoadNextImage(targetpanel);
                priorimageIndexLeftPanel = restorepriorimageindex;
            }
            if (targetpanel == 1)
            {
                try
                {
                    imageFilesRightPanel.RemoveAt(imageIndexRightPanel);
                    imageCountRightPanel--;
                }
                catch (Exception) { throw; }
                int restorepriorimageindex = priorimageIndexRightPanel;
                imageIndexRightPanel -= 1;
                if (imageIndexRightPanel < 0) imageIndexRightPanel = 0;
                LoadNextImage(targetpanel);
                priorimageIndexRightPanel = restorepriorimageindex;
            }
        }

        private void Copycut(int targetPanel, string datatype, bool movefiles)
        {
            PictureBox thispicturebox = targetPanel == 0 ? pictureBox_leftpanel : pictureBox_rightpanel;

            if (!(thispicturebox.Image is null))
            {
                DataObject dobj = new DataObject();
                switch (datatype)
                {
                    default:
                    case "file": // store in clipboard as filepath
                    case "filepath":
                        // https://stackoverflow.com/questions/211611/copy-files-to-clipboard-in-c-sharp
                        StringCollection paths = new StringCollection();
                        if (targetPanel == 0 && !(imageFilesLeftPanel is null) && imageCountLeftPanel != 0)
                        {
                            string filepath = imageFilesLeftPanel[imageIndexLeftPanel];
                            if (datatype.Equals("filepath")) Clipboard.SetText(filepath);
                            if (datatype.Equals("file"))
                            {
                                paths.Add(filepath);
                                PutFilesOnClipboard(paths, movefiles);
                            }
                        }
                        if (targetPanel == 1 && !(imageFilesRightPanel is null) && imageCountRightPanel != 0)
                        {
                            string filepath = imageFilesRightPanel[imageIndexRightPanel];
                            if (datatype.Equals("filepath")) Clipboard.SetText(filepath);
                            if (datatype.Equals("file"))
                            {
                                paths.Add(filepath);
                                PutFilesOnClipboard(paths, movefiles);
                            }
                        }
                        break;

                    case "bitmap":

                        dobj.SetData(DataFormats.Bitmap, true, thispicturebox.Image);
                        Clipboard.SetDataObject(dobj, true); // if shift is held, store in clipboard as image content
                        break;

                }
            }
        }

        // copy image to clipboard from panel
        private void ContextMenu_image_item_copy_Click(object sender, EventArgs e)
        {
            bool bool_movefiles = false;
            string datatype = "file";
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) datatype = "bitmap"; // if shift is held down, send image to clipbaord as bitmap
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control) datatype = "filepath";
            Copycut(contextmenufocus, datatype, bool_movefiles);
        }

        //https://stackoverflow.com/questions/2953254/cgetting-all-image-files-in-folder
        private static readonly String[] allowedImageExtensions = new String[] { "jpg", "jpeg", "png", "gif", "tiff", "bmp", "jfif", "webp" };

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

        private string GetCurrentImage(int targetPanel)
        {
            if (targetPanel == 0 && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null && imageCountLeftPanel != 0)
                {
                    return imageFilesLeftPanel[imageIndexLeftPanel];
                }

            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null && imageCountRightPanel != 0)
                {
                    return imageFilesRightPanel[imageIndexRightPanel];
                }
            }
            return null;
        }
        private void LoadPreviousImage(int targetPanel)
        {
            if (loadprevattempts > maxloadattempts) { return; }
            if (targetPanel == 0 && pictureBox_leftpanel.Image != null)
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
                    else { loadprevattempts++; LoadPreviousImage(targetPanel); }
                }
                Resize_imagepanels();
            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null && imageCountRightPanel != 0)
                {
                    int nextImageIndex = imageIndexRightPanel - 1;
                    if (nextImageIndex < 0) nextImageIndex = imageCountRightPanel - 1;

                    if (LoadImage(targetPanel, imageFilesRightPanel[nextImageIndex]))
                    {
                        priorimageIndexRightPanel = imageIndexRightPanel;
                        imageIndexRightPanel = nextImageIndex;
                    }
                    else { loadprevattempts++; LoadPreviousImage(targetPanel); }
                }
                Resize_imagepanels();
            }
            UpdateLabelImageIndex();
        }

        private void PreviousLeftArrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPreviousImage(activePanel);
        }

        int loadnextattempts = 0;
        int loadprevattempts = 0;
        static readonly int maxloadattempts = 1;
        private void LoadNextImage(int targetPanel)
        {
            if (loadnextattempts > maxloadattempts) { Console.WriteLine(loadnextattempts.ToString()); return; }
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
                    } else { loadnextattempts++; LoadNextImage(targetPanel); }
                }
                if (imageCountLeftPanel == 0) pictureBox_leftpanel.Image = null;
            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null)
            {
                if (imageFilesRightPanel != null && imageCountRightPanel != 0)
                {
                    int nextImageIndex = imageIndexRightPanel + 1;
                    if (nextImageIndex >= imageCountRightPanel) nextImageIndex = 0;
                    if (LoadImage(targetPanel, imageFilesRightPanel[nextImageIndex]))
                    {
                        priorimageIndexRightPanel = imageIndexRightPanel;
                        imageIndexRightPanel = nextImageIndex;
                    }
                    else { loadnextattempts++; LoadNextImage(targetPanel); }
                }
                if (imageCountRightPanel == 0) pictureBox_rightpanel.Image = null;
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
            Blur(contextmenufocus);
        }

        //https://stackoverflow.com/questions/16022188/open-an-image-with-the-windows-default-editor-in-c-sharp
        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextmenufocus == 0 && pictureBox_leftpanel.Image != null && imageCountLeftPanel != 0)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(imageFilesLeftPanel[imageIndexLeftPanel])
                {
                    Verb = "edit"
                };
                Process.Start(startInfo);
            }
            if (contextmenufocus == 1 && pictureBox_rightpanel.Image != null && imageCountRightPanel != 0)
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
            if (contextmenufocus == 0 && imageFilesLeftPanel != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                Process.Start("explorer.exe", "/select, " + imageFilesLeftPanel[imageIndexLeftPanel]);
            }
            if (contextmenufocus == 1 && imageFilesRightPanel != null && imageFilesRightPanel != null && imageCountRightPanel != 0)
            {
                Process.Start("explorer.exe", "/select, " + imageFilesRightPanel[imageIndexRightPanel]);
            }
        }

        //https://stackoverflow.com/questions/3282418/send-a-file-to-the-recycle-bin
        //https://www.c-sharpcorner.com/UploadFile/mahesh/how-to-remove-an-item-from-a-C-Sharp-list/
        private void SendToTrash(int targetPanel)
        {
            String deletefilepath = String.Empty;
            if (targetPanel == 0 && pictureBox_leftpanel.Image != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                deletefilepath = imageFilesLeftPanel[imageIndexLeftPanel];
            }
            if (targetPanel == 1 && pictureBox_rightpanel.Image != null && imageFilesRightPanel != null && imageCountRightPanel != 0)
            {
                deletefilepath = imageFilesRightPanel[imageIndexRightPanel];
            }
            if (File.Exists(deletefilepath))
                try
                {
                    FileSystem.DeleteFile(deletefilepath,
                        Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin,
                        Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
                }
                catch (Exception) { throw; }
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
            if (targetPanel == 1 && imageFilesRightPanel != null && imageCountRightPanel != 0)
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

        private void openInWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // https://stackoverflow.com/questions/6808029/open-image-in-windows-photo-viewer
            string tempFileName = "";
            string path = Environment.GetFolderPath(
                Environment.SpecialFolder.ProgramFiles);

            if (contextmenufocus == 0 && pictureBox_leftpanel.Image != null && imageCountLeftPanel != 0)
            {
                tempFileName = imageFilesLeftPanel[imageIndexLeftPanel];
            }
            if (contextmenufocus == 1 && pictureBox_rightpanel.Image != null && imageCountRightPanel != 0)
            {
                tempFileName = imageFilesRightPanel[imageIndexRightPanel];
            }
            if (!String.Equals("", tempFileName))
            {
                // create our startup process and argument
                var psi = new ProcessStartInfo(
                    "rundll32.exe",
                    String.Format(
                        "\"{0}{1}\", ImageView_Fullscreen {2}",
                        Environment.Is64BitOperatingSystem ?
                            path.Replace(" (x86)", "") :
                            path
                            ,
                        @"\Windows Photo Viewer\PhotoViewer.dll",
                        tempFileName)
                    );

                psi.UseShellExecute = false;

                var viewer = Process.Start(psi);
            }
        }

        private void button_trash_Click(object sender, EventArgs e)
        {
            SendToTrash(activePanel);
        }

        //https://stackoverflow.com/questions/2706500/how-do-i-generate-a-random-int-number
        private static readonly System.Random getrandom = new System.Random();

        static RandomLCG rng = new RandomLCG();

        public static int GetRandomNumber(int min, int max, int exclude)
        {
            int maxattempts = 3;
            int result = exclude;
            for (int i = 0; i < maxattempts && result == exclude; i++)
            {
                result = (int)(rng.Next(max - min) + min);
            }
            if (result == exclude) result = (exclude + 1) % max;
            return result;

        }
        
        private void cleanupmemory()
        {
            //Force garbage collection.
            GC.Collect();
            // Wait for all finalizers to complete before continuing.
            GC.WaitForPendingFinalizers();
        }

        private void LoadRandomImage(int targetPanel)
        {
            if (targetPanel == 0 && imageCountLeftPanel > 1)
            {
                int randomindex = GetRandomNumber(0, imageCountLeftPanel, imageIndexLeftPanel);
                if (LoadImage(targetPanel, imageFilesLeftPanel[randomindex]))
                {
                    priorimageIndexLeftPanel = imageIndexLeftPanel;
                    imageIndexLeftPanel = randomindex;
                }
            }
            if (targetPanel == 1 && imageCountRightPanel > 1)
            {
                int randomindex  = GetRandomNumber(0, imageCountRightPanel, imageIndexRightPanel);
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

        private void MirrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((contextmenufocus == 0) && pictureBox_leftpanel.Image != null)
            {
                Image img = pictureBox_leftpanel.Image;
                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox_leftpanel.Image = img;
            }
            if ((contextmenufocus == 1) && pictureBox_rightpanel.Image != null)
            {
                Image img = pictureBox_rightpanel.Image;
                img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox_rightpanel.Image = img;
            }
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bool_movefiles = true;
            string datatype = "file";
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) datatype = "bitmap"; // if shift is held down, send image to clipbaord as data
            Copycut(contextmenufocus, datatype, bool_movefiles);
        }

        private void removeFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Removefromlist(contextmenufocus);
        }

        private void webptojpgToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            string arg = "";
            string ofilepath = "";
            if ((contextmenufocus == 0) && pictureBox_leftpanel.Image != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                ofilepath = imageFilesLeftPanel[imageIndexLeftPanel];
            }
            if ((contextmenufocus == 1) && pictureBox_rightpanel.Image != null && imageFilesRightPanel != null && imageCountRightPanel != 0)
            {
                ofilepath = imageFilesRightPanel[imageIndexRightPanel];
            }
            string fileExt = System.IO.Path.GetExtension(ofilepath);
            string tmpfilename = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " tmp" + fileExt;
            string tmpImage = tmpAppDataPath + tmpfilename;
            DirectoryInfo di = Directory.CreateDirectory(tmpAppDataPath);
            System.IO.File.Move(ofilepath, tmpImage);
            arg = $"/C ffmpeg.exe -nostdin -i \"{tmpImage}\"  \"{ofilepath}\" && del \"{tmpImage}\"";
            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = arg,
                }
            };

            proc.Start();
            proc.WaitForExit();//May need to wait for the process to exit too

            LoadImage(contextmenufocus, ofilepath);
        }

        private void SaveImage(Image targetimage, string filename, string directorypath, Boolean savedialog)
        {
            bool savedYes = false;
            string newfilepath = System.IO.Path.Combine(directorypath, filename);

            var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Settings.Default.ImageSaveQuality);


            if (!(targetimage is null) && !savedialog)
            {
                try
                {
                    using (FileStream fs = System.IO.File.Create(newfilepath))

                    {

                        string extension = Path.GetExtension(filename);
                        switch (extension.ToLower())
                        {
                            case ".jpeg":
                            case ".jpg":
                                targetimage.Save(fs, GetEncoder(
                                   System.Drawing.Imaging.ImageFormat.Jpeg), encoderParameters);
                                break;

                            case ".bmp":
                                targetimage.Save(fs, GetEncoder(
                                   System.Drawing.Imaging.ImageFormat.Bmp), encoderParameters);
                                break;

                            case ".gif":
                                targetimage.Save(fs, GetEncoder(
                                      System.Drawing.Imaging.ImageFormat.Gif), encoderParameters);
                                break;

                            case ".png":
                                targetimage.Save(fs, GetEncoder(
                                      System.Drawing.Imaging.ImageFormat.Png), encoderParameters);
                                break;
                            default:
                                throw new NotSupportedException(
                                    "Unknown file extension " + extension);
                        }

                        fs.Close();
                        savedYes = true;
                    }
                } catch { }
            }
            if (!(targetimage is null) && savedialog) try
                {
                    // Displays a SaveFileDialog so the user can save the Image
                    saveFileDialog1.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
                    saveFileDialog1.Title = "Save an Image File";
                    saveFileDialog1.FileName = System.IO.Path.GetFileNameWithoutExtension(filename);
                    saveFileDialog1.RestoreDirectory = false;
                    saveFileDialog1.InitialDirectory = directorypath;
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
                                targetimage.Save(fs, GetEncoder(
                                   System.Drawing.Imaging.ImageFormat.Jpeg), encoderParameters);
                                break;

                            case 2:
                                targetimage.Save(fs, GetEncoder(
                                   System.Drawing.Imaging.ImageFormat.Bmp), encoderParameters);
                                break;

                            case 3:
                                targetimage.Save(fs, GetEncoder(
                                      System.Drawing.Imaging.ImageFormat.Gif), encoderParameters);
                                break;

                            case 4:
                                targetimage.Save(fs, GetEncoder(
                                      System.Drawing.Imaging.ImageFormat.Png), encoderParameters);
                                break;
                        }

                        fs.Close();
                        savedYes = true;
                        newfilepath = System.IO.Path.GetFullPath(saveFileDialog1.FileName);
                    } // if dialog OK
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex);
                }
            // Opens Fle Directory if checkbox enabled
            if (checkBox_openaftersave.Checked && savedYes)
            {
                string args = string.Format("/e, /select, \"{0}\"", newfilepath);

                ProcessStartInfo info = new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = args
                };
                Thread.Sleep(300); // fast and dirty way of waiting for file finish writing, otherwise file gets deselected.
                Process.Start(info);
            }
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " image";
            string directorypath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            bool savedialog = true;
            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift) savedialog = false;
            if (checkBox_screengrab.Checked)
            {
                Panel thispanel = activePanel == 0 ? splitContainer_bothimages.Panel1 : splitContainer_bothimages.Panel2;
                Bitmap bmp1 = new Bitmap(thispanel.Width, thispanel.Height);
                thispanel.DrawToBitmap(bmp1, new System.Drawing.Rectangle(0, 0, thispanel.Width, thispanel.Height));
                SaveImage((Image)bmp1, filename, directorypath, savedialog);
                return;
            }
            if ((contextmenufocus == 0) && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null && imageCountLeftPanel != 0) { filename = System.IO.Path.GetFileName(imageFilesLeftPanel[imageIndexLeftPanel]); directorypath = System.IO.Path.GetDirectoryName(imageFilesLeftPanel[imageIndexLeftPanel]); }
                Image targetimage = pictureBox_leftpanel.Image;
                SaveImage(targetimage, filename, directorypath, savedialog);
            }
            if ((contextmenufocus == 1) && pictureBox_rightpanel.Image != null)
            {
                Image targetimage = pictureBox_rightpanel.Image;
                if (imageFilesRightPanel != null && imageCountRightPanel != 0) { filename = System.IO.Path.GetFileName(imageFilesRightPanel[imageIndexRightPanel]); directorypath = System.IO.Path.GetDirectoryName(imageFilesRightPanel[imageIndexRightPanel]); }
                SaveImage(targetimage, filename, directorypath, savedialog);
            }
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

            // O hotkey for rotations
            if (keyData == Keys.O) { RotateImage(activePanel); return true; }

            // L hotkey for blur
            if ((keyData == Keys.L)) { Blur(activePanel); return true; }

            // Alt + R hotkey for alternate randomize panels
            //if (keyData == (Keys.Alt | Keys.R)) { LoadRandomImage(0); LoadRandomImage(1); return true; }
            if (keyData == (Keys.Alt | Keys.R)) { LoadRandomImage(activePanel); if (!checkBox_hotkeyboth.Checked & numberofimagepanels==2) { LoadRandomImage(1 - activePanel); } return true; }

            // R hotkey for randomize panel
            if (keyData == Keys.R) { LoadRandomImage(activePanel); if (checkBox_hotkeyboth.Checked & numberofimagepanels == 2) { LoadRandomImage(1 - activePanel); } return true; }

            // U hotkey for jump back 
            if (keyData == (Keys.U)) { JumpBack(activePanel); if (checkBox_hotkeyboth.Checked & numberofimagepanels == 2) { JumpBack(1 - activePanel); } return true; }

            // Alt + U hotkey for alternate jump back 
            if (keyData == (Keys.Alt | Keys.U)) { JumpBack(activePanel); if (!checkBox_hotkeyboth.Checked & numberofimagepanels == 2) { JumpBack(1 - activePanel); } return true; }

            // Delete or Shift + D hotkey for send to recycle
            if ((keyData == Keys.Delete) || (keyData == Keys.D)) { SendToTrash(activePanel); return true; }

            // Ctrl + X for cut
            if (keyData == (Keys.Control | Keys.X)) { Copycut(activePanel, "file", true); return true; }

            // Ctrl + Shift + X for cut as data
            if (keyData == (Keys.Control | Keys.Shift | Keys.X)) { Copycut(activePanel, "bitmap", true); return true; }

            // Ctrl + C for copy
            if (keyData == (Keys.Control | Keys.C)) { Copycut(activePanel, "file", false); return true; }

            // Ctrl + Shift + C for copy as data
            if (keyData == (Keys.Control | Keys.C)) { Copycut(activePanel, "bitmap", false); return true; }

            // Ctrl + Alt + X for cut and remove from list
            if (keyData == (Keys.Control | Keys.Alt | Keys.X)) { Copycut(activePanel, "file", true); Removefromlist(activePanel); return true; }

            // Alt + Spacebar to toggle if hotkeys affect both panels
            if (keyData == (Keys.Alt | Keys.Space )) {checkBox_hotkeyboth.Checked = !checkBox_hotkeyboth.Checked;
                return true; }

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

            Bitmap image_tempswap = (pictureBox_leftpanel.Image == null ? null : new Bitmap(pictureBox_leftpanel.Image));
            if (imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                LoadImage(0, imageFilesLeftPanel[imageIndexLeftPanel]);
            }
            else pictureBox_leftpanel.Image = (pictureBox_rightpanel.Image == null ? null : new Bitmap(pictureBox_rightpanel.Image));
            if (imageFilesRightPanel != null && imageCountRightPanel != 0)
            {
                LoadImage(1, imageFilesRightPanel[imageIndexRightPanel]);
            }
            else pictureBox_rightpanel.Image = image_tempswap;
            Resize_imagepanels();
        }

        /* Section:|Image display */
        /* scrollable zoom */

        // https://outofrangeexception.blogspot.com/2013/03/how-to-zoom-picturebox-with-mouse-in-c.html
        private bool panning = false;

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            panning = false;
        }

        private Point mousePos;

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            panning = true;
            mousePos = e.Location;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox thispicturebox = activePanel == 0 ? pictureBox_leftpanel : pictureBox_rightpanel;
            int dx = e.X - mousePos.X;
            int dy = e.Y - mousePos.Y;
            if (panning)
            {
                thispicturebox.Invalidate();
            }
            if (e.Button == MouseButtons.Left)
            {
                //UndockPicturebox(thispicturebox);
                Control c = sender as Control;
                if (panning && c != null && thispicturebox.Dock == DockStyle.None)
                {
                    int newposLeft = e.X + c.Left - mousePos.X;
                    int newposTop = e.Y + c.Top - mousePos.Y;

                    int containerwidth = thispicturebox.Parent.ClientSize.Width;
                    int containerheight = thispicturebox.Parent.ClientSize.Height;
                    int picturewidth = thispicturebox.Width;
                    int pictureheight = thispicturebox.Height;
                    int boundaryTop = 0;
                    int boundaryBottom = containerheight;
                    int boundaryLeft = 0;
                    int boundaryRight = containerwidth;

                    int finalposLeft = newposLeft;
                    int finalposTop = newposTop;
                    // for pan control
                    if (pictureheight > containerheight)
                    { // when zoomed in
                        if (newposTop > boundaryTop && dy > 0) finalposTop = boundaryTop;
                        if (newposTop + pictureheight < boundaryBottom && dy < 0) finalposTop = boundaryBottom - pictureheight;
                    }
                    else
                    { // when zoomed out
                        if (newposTop < boundaryTop) finalposTop = boundaryTop;
                        if (newposTop + pictureheight > boundaryBottom) finalposTop = boundaryBottom - pictureheight;
                    }
                    if (picturewidth > containerwidth)
                    {
                        if (newposLeft > boundaryLeft && dx > 0) finalposLeft = boundaryLeft;
                        if (newposLeft + picturewidth < boundaryRight && dx < 0) finalposLeft = boundaryRight - picturewidth;
                    }
                    else
                    {
                        if (newposLeft < boundaryLeft) finalposLeft = boundaryLeft;
                        if (newposLeft + picturewidth > boundaryRight) finalposLeft = boundaryRight - picturewidth;
                    }

                    c.Top = finalposTop;
                    c.Left = finalposLeft;
                }
            }
        }

        private void UndockPicturebox(PictureBox targetpicturebox)
        {
            if (targetpicturebox.Dock == DockStyle.Fill)
            {
                int containerwidth = targetpicturebox.Parent.ClientSize.Width;
                int containerheight = targetpicturebox.Parent.ClientSize.Height;
                Size imageSize = targetpicturebox.Image.Size;
                float ratio = Math.Min((float)containerwidth / (float)imageSize.Width, (float)containerheight / (float)imageSize.Height);
                int picwidth = (int)(imageSize.Width * ratio);
                int picheight = (int)(imageSize.Height * ratio);
                int picX = (containerwidth - picwidth) / 2;
                int picY = (containerheight - picheight) / 2;

                targetpicturebox.Size = new Size(picwidth, picheight);
                // https://stackoverflow.com/questions/9375588/keeping-a-picturebox-centered-inside-a-container
                targetpicturebox.Dock = DockStyle.None;
                targetpicturebox.Location = new Point(picX, picY);
                targetpicturebox.Refresh();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            FocusPanelAtCursor(this);
            PictureBox thispicturebox = activePanel == 0 ? pictureBox_leftpanel : pictureBox_rightpanel;
            if (thispicturebox.Image == null) return;
            UndockPicturebox(thispicturebox);
            int newWidth = thispicturebox.Image.Width, newHeight = thispicturebox.Image.Height, newX = thispicturebox.Location.X, newY = thispicturebox.Location.Y;
            double zoomfactor = -0.042 * e.Delta; // add or subtract 1/zoomfactor of the original width and height

            newWidth = thispicturebox.Size.Width - (int)(thispicturebox.Size.Width / zoomfactor);
            newHeight = thispicturebox.Size.Height - (int)(thispicturebox.Size.Height / zoomfactor);

            // keep image inside frame
            int containerwidth = thispicturebox.Parent.ClientSize.Width;
            int containerheight = thispicturebox.Parent.ClientSize.Height;
            if (newWidth < containerwidth && newHeight < containerheight)
            {
                thispicturebox.Invalidate();
                thispicturebox.Dock = DockStyle.Fill;

                return;
            }

            // zoom on cursor position
            int centerx = thispicturebox.Parent.ClientSize.Width / 2;
            int centery = thispicturebox.Parent.ClientSize.Height / 2;
            int rposX = e.X - thispicturebox.Parent.Left;
            int rposY = e.Y - thispicturebox.Parent.Top;
            newX = (int)(rposX - (1 - 1.0 / zoomfactor) * (rposX - thispicturebox.Left));
            newY = (int)(rposY - (1 - 1.0 / zoomfactor) * (rposY - thispicturebox.Top));
            // zoom on center of image
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                newX = thispicturebox.Location.X + (int)((thispicturebox.Size.Width / zoomfactor) / 2);
                newY = thispicturebox.Location.Y + (int)((thispicturebox.Size.Height / zoomfactor) / 2);
            }
            // zoom on center of panel
            else if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                newX = (int)(centerx - (1 - 1.0 / zoomfactor) * (centerx - thispicturebox.Left));
                newY = (int)(centery - (1 - 1.0 / zoomfactor) * (centery - thispicturebox.Top));
            }
            thispicturebox.Size = new Size(newWidth, newHeight);
            thispicturebox.Location = new Point(newX, newY);
            //MessageBox.Show("mousepos " + e.X + "," + e.Y + "\nparent " + thispicturebox.Parent.Name + " " + thispicturebox.Parent.Left + "," + thispicturebox.Parent.Top +"\npicturebox "+thispicturebox.Left +","+thispicturebox.Top+ "\ncenter of panel " + centerx + "," + centery+"\ncenter of picturebox "+ (thispicturebox.Location.X + (int)((thispicturebox.Size.Width / zoomfactor) / 2))+","+ (thispicturebox.Location.Y + (int)((thispicturebox.Size.Height / zoomfactor) / 2)));
        }

        /* load image */

        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }

        private void WriteTextOnImage(PictureBox targetpicturebox, String text)
        {
            Bitmap bmp = new Bitmap(600, 600);
            string tmptext = SpliceText(text, 45);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                System.Drawing.Rectangle ImageSize = new System.Drawing.Rectangle(0, 0, 600, 600);
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
                PictureBox targetpicturebox = null;
                using (var bmpTemp = new Bitmap(imagePath))
                {
                    if (targetPanel == 0) { targetpicturebox = pictureBox_leftpanel; }
                    if (targetPanel == 1) { targetpicturebox = pictureBox_rightpanel; }
                    if (System.IO.Path.GetExtension(imagePath).ToLower().Equals(".gif"))
                    {
                        targetpicturebox.ImageLocation = imagePath;
                    }
                    targetpicturebox.Size = new Size(bmpTemp.Width, bmpTemp.Height);
                    targetpicturebox.Image = new Bitmap(bmpTemp);
                    targetpicturebox.Dock = System.Windows.Forms.DockStyle.Fill;

                    return true;
                }
            }
            catch (Exception)
            {
                string loaderrormsg = "Failed to load image: " + imagePath;
                if (targetPanel == 0) { WriteTextOnImage(pictureBox_leftpanel, loaderrormsg); }
                if (targetPanel == 1) { WriteTextOnImage(pictureBox_rightpanel, loaderrormsg); }
                try
                {
                 //   DragDropHandler(targetPanel, new String[] { GetCurrentImage(targetPanel) }); ;
                    return true;
                }
                catch (Exception) { return false; }

                return false; // supposed to return false, but i want to load image path anyways so i can delete corrupted images --- too lazy to fix behavior in the usage right now
            }
        }

        /* app closing save settings */

        // https://www.codeproject.com/Articles/15013/Windows-Forms-User-Settings-in-C
        private String tmpAppDataPath;

        private void clearTmpAppData()
        {
            if (File.Exists(tmpAppDataPath))
                Directory.Delete(tmpAppDataPath, true);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try //https://stackoverflow.com/questions/42708868/user-config-corruption
            {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);


            // Copy window location to app settings
            Settings.Default.WindowLocation = this.Location;

            // Copy window size to app settings
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = this.Size;
            }
            else
            {
                Settings.Default.WindowSize = this.RestoreBounds.Size;
            }
            Settings.Default.SplitContainerSplitterDistance = savesplitterdistance;
            Settings.Default.RandomizeOnClick = this.checkBox_randomOnClick.Checked;
            Settings.Default.OpenFolderAfterSave = this.checkBox_openaftersave.Checked;
            Settings.Default.showinfo = this.checkBox_showfilename.Checked;
            Settings.Default.HotkeyBoth = this.checkBox_hotkeyboth.Checked;

            if (imageFilesLeftPanel!= null && imageCountLeftPanel != 0) { Settings.Default.LastFile = imageFilesLeftPanel[imageIndexLeftPanel]; } 
                else
                {
                    Settings.Default.LastFile = "";
                }

             // remember number of image panels
              Settings.Default.NumberOfPanels = numberofimagepanels;

            // Save settings
            Settings.Default.Save();
            }
            catch (ConfigurationErrorsException exception)
            {
                Console.WriteLine("Settings are Corrupt!!");
                // handle error or tell user
                // to manually delete the file, you can do the following:
                try
                {
                    // File.Delete(exception.Filename); // this can throw an exception too, so be wary!
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //Settings.Default.Reload();
                throw exception;
            }
            clearTmpAppData();
        }

        private int savesplitterdistance;

        private void splitContainer_bothimages_SplitterMoved(object sender, SplitterEventArgs e)
        {
            savesplitterdistance = splitContainer_bothimages.SplitterDistance;
        }

        private void button_slideshow_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            { // https://stackoverflow.com/questions/2018272/preventing-multiple-instance-of-one-form-from-displaying
                if (f is form_slideshow)
                {
                    f.Focus();
                    return;
                }
            }

            if (default_slideshowbot == null)
            {
                default_slideshowbot = new slideshow_bot("ordered", "ordered", new DispatcherTimer());
                default_slideshowbot.slideshow_timer.Tick += Tick;
            }

            form_slideshow slideshow_settings = new form_slideshow(this);
            Point location = button_slideshow.PointToScreen(Point.Empty);
            location.X += button_slideshow.Width / 2;
            slideshow_settings.pntLocation = location;
            slideshow_settings.Show();
        }

        public class slideshow_bot
        {
            public string slideshow_leftshufflemode;
            public string slideshow_rightshufflemode;
            public DispatcherTimer slideshow_timer;
            public slideshow_bot(string slideshow_leftshufflemode, string slideshow_rightshufflemode, DispatcherTimer slideshow_timer)
            {
                this.slideshow_leftshufflemode = slideshow_leftshufflemode;
                this.slideshow_rightshufflemode = slideshow_rightshufflemode;
                this.slideshow_timer = slideshow_timer;
            }   
        }
        public slideshow_bot default_slideshowbot;
        public void slideshow_start(int timeinterval_sec, string leftmode, string rightmode)
        {
            DispatcherTimer timer = default_slideshowbot.slideshow_timer;
            default_slideshowbot = new slideshow_bot(leftmode, rightmode, timer);
            timer.Interval = TimeSpan.FromSeconds(timeinterval_sec);
            timer.Tick += Tick;
            timer.Start();
        }

        public void slideshow_stop()
        {
            default_slideshowbot.slideshow_timer.Stop();
            default_slideshowbot.slideshow_timer.Tick -= Tick;
        }

        private void Tick(object sender, EventArgs e)
        {
            slideshow_advance(0, default_slideshowbot.slideshow_leftshufflemode);
            slideshow_advance(1, default_slideshowbot.slideshow_leftshufflemode);
        }
        
        private void slideshow_advance(int targetpicturebox, string shufflemode)
        {
            if(String.Equals(shufflemode, "random")){
                LoadRandomImage(targetpicturebox);
            }
            if (String.Equals(shufflemode, "ordered")){
                LoadNextImage(targetpicturebox);
            }
        }

        private void button_crop_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            { // https://stackoverflow.com/questions/2018272/preventing-multiple-instance-of-one-form-from-displaying
                if (f is Form_Crop)
                {
                    f.Dispose();
                    break;
                }
            }
            //ImageStitcher.Window1 icwin = new ImageStitcher.Window1();
            //icwin.Show();


            Image targetimage = activePanel == 0 ? pictureBox_leftpanel.Image : pictureBox_rightpanel.Image;

            Form_Crop crop_window = new Form_Crop(this);

            Point location = button_crop.PointToScreen(Point.Empty);
            location.X += button_crop.Width / 2;
            crop_window.pntLocation = location;

            String image_path = "";
            if (activePanel == 0 && imageFilesLeftPanel != null && imageCountLeftPanel != 0) image_path = imageFilesLeftPanel[imageIndexLeftPanel];
            if (activePanel == 1 && imageFilesRightPanel != null && imageCountRightPanel != 0) image_path = imageFilesRightPanel[imageIndexRightPanel];
            if (!String.IsNullOrEmpty(image_path))
            {
                crop_window.Load_img(image_path);
                crop_window.Show();
            }
        }
        
        public int[] getCropWindowPositions()
        {
            int[] result= new int[10];

            Point pbllocation = pictureBox_leftpanel.PointToScreen(new Point(0, 0));
            Point pbrlocation = pictureBox_rightpanel.PointToScreen(new Point(0, 0));

            result[0] = splitContainer_bothimages.Panel1.Width;
            result[1] = splitContainer_bothimages.Panel1.Height;
            result[2] = pbllocation.X;
            result[3] = pbllocation.Y;
            result[4] = splitContainer_bothimages.Panel2.Width;
            result[5] = splitContainer_bothimages.Panel2.Height;
            result[6] = pbrlocation.X;
            result[7] = pbrlocation.Y;
            result[8] = activePanel;

            return result;
        }
        public void cropSaveImage(bool overwrite)
        {
            string filename="",directorypath="";
            bool savedialog = true;
            if (overwrite) savedialog = false;
            if ((activePanel == 0) && pictureBox_leftpanel.Image != null)
            {
                if (imageFilesLeftPanel != null && imageCountLeftPanel != 0) { filename = System.IO.Path.GetFileName(imageFilesLeftPanel[imageIndexLeftPanel]); directorypath = System.IO.Path.GetDirectoryName(imageFilesLeftPanel[imageIndexLeftPanel]); }
                Image targetimage = pictureBox_leftpanel.Image;
                SaveImage(targetimage, filename, directorypath, savedialog);
            }
            if ((activePanel == 1) && pictureBox_rightpanel.Image != null)
            {
                Image targetimage = pictureBox_rightpanel.Image;
                if (imageFilesRightPanel != null && imageCountRightPanel != 0) { filename = System.IO.Path.GetFileName(imageFilesRightPanel[imageIndexRightPanel]); directorypath = System.IO.Path.GetDirectoryName(imageFilesRightPanel[imageIndexRightPanel]); }
                SaveImage(targetimage, filename, directorypath, savedialog);
            }
        }
        public void LoadActiveImage (Image targetimage)
        {
            PictureBox targetpictureBox = activePanel == 0 ? pictureBox_leftpanel : pictureBox_rightpanel;
            targetpictureBox.Image = targetimage;
            targetpictureBox.Invalidate();
            Resize_imagepanels();
        }

        private void checkBox_showfilename_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLabelImageIndex();
        }

        private void webpToGifToolStripMenuItem_Click(object sender, EventArgs e)
        { // requires Python, Pillow installed, and script set in path https://gist.github.com/nimatrueway/0e743d92056e2c5f995e25b848a1bdcd
            // in the script, rename Python3 to Python if not found
            string inputfilepath = "";
            if ((contextmenufocus == 0) && pictureBox_leftpanel.Image != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                inputfilepath = imageFilesLeftPanel[imageIndexLeftPanel];
            }
            if ((contextmenufocus == 1) && pictureBox_rightpanel.Image != null && imageFilesRightPanel != null && imageCountRightPanel != 0)
            {
                inputfilepath = imageFilesRightPanel[imageIndexRightPanel];
            }
            string fileExt = System.IO.Path.GetExtension(inputfilepath);
            string inputbackupfilename = DateTime.Now.ToString("yyyy_MM_dd_HHmmssfff") + " tmp" + fileExt;
            string inputbackupfilepath = tmpAppDataPath + inputbackupfilename;
            DirectoryInfo di = Directory.CreateDirectory(tmpAppDataPath);

            System.IO.File.Move(inputfilepath, inputbackupfilepath);
            string outputfilepath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(inputfilepath), System.IO.Path.GetFileNameWithoutExtension(inputfilepath)) + ".gif";
            string tmpoutputfilepath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(inputbackupfilepath), System.IO.Path.GetFileNameWithoutExtension(inputbackupfilepath)) + ".gif";

            String cmd_webptogif = $"python_pillow_webp2gif.py \"{inputbackupfilepath}\"";
            string outputvisibility = "/C ";
            // use "/C "+ for cmd.exe to close automatically "/K "+ for cmd.exe to stay open and view ffmpeg output 
            string arg = outputvisibility + cmd_webptogif;

            Process proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = arg,
                    UseShellExecute = false,
                    CreateNoWindow = false
                }
            };

            proc.Start();
            proc.WaitForExit();//May need to wait for the process to exit too

            System.IO.File.Move(tmpoutputfilepath, outputfilepath);

            LoadImage(contextmenufocus, outputfilepath);
            if ((contextmenufocus == 0) && pictureBox_leftpanel.Image != null && imageFilesLeftPanel != null && imageCountLeftPanel != 0)
            {
                imageFilesLeftPanel[imageIndexLeftPanel] = outputfilepath;
            }
            if ((contextmenufocus == 1) && pictureBox_rightpanel.Image != null && imageFilesRightPanel != null && imageCountRightPanel != 0)
            {
                imageFilesRightPanel[imageIndexRightPanel] = outputfilepath;
            }
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            foreach (Form f in Application.OpenForms)
            { // https://stackoverflow.com/questions/2018272/preventing-multiple-instance-of-one-form-from-displaying
                if (f is form_settings)
                {
                    f.Focus();
                    return;
                }
            }

            form_settings main_settings = new form_settings(this);
            Point location = button_settings.PointToScreen(Point.Empty);
            location.X += button_settings.Width / 2;
            main_settings.pntLocation = location;
            main_settings.Show();
        }

        // Skins dark/light mode

        public void DarkModeRefresh()
        {
            if (Settings.Default.DarkMode == true) {

                Color darkcolor = (Color)System.Drawing.ColorTranslator.FromHtml(Settings.Default.DarkModeColor);
                Color darkaccentcolor = (Color)System.Drawing.ColorTranslator.FromHtml(Settings.Default.DarkModeColorAccent);
                pictureBox_leftpanel.BackColor = darkcolor;
                pictureBox_rightpanel.BackColor = darkcolor;
                panel_controls.BackColor = darkaccentcolor;
                this.BackColor = darkaccentcolor;
                splitContainer_bothimages.BackColor = darkaccentcolor;

                Color darkcolortext = (Color)System.Drawing.ColorTranslator.FromHtml(Settings.Default.DarkModeColorText);
                

                foreach (Control subC in panel_controls.Controls)
                {
                    subC.BackColor = darkaccentcolor;
                    if (!(subC is Button))
                    {
                        subC.ForeColor = darkcolortext;
                    }
                    if (subC is Button)
                    {
                        ((Button)subC).FlatStyle = FlatStyle.Flat;
                        ((Button)subC).FlatAppearance.BorderColor = darkcolor;
                        if (subC.BackgroundImage is null) { subC.ForeColor = darkcolortext; }
                    }

                }
                DarkTitleBarClass.UseImmersiveDarkMode(Handle, true);

                label_filename_leftpanel.BackColor = darkaccentcolor;
                label_filename_rightpanel.BackColor = darkaccentcolor;
                //label_filename_leftpanel.ForeColor = darkcolortext;
                //label_filename_rightpanel.ForeColor = darkcolortext;
                label_imageindex_leftpanel.BackColor = darkaccentcolor;
                //label_imageindex_leftpanel.ForeColor = darkcolortext;
            }
            if (Settings.Default.DarkMode == false)
            {
                Color lightbackground = SystemColors.Control;
                Color lighttextcolor = Color.Black;
                pictureBox_leftpanel.BackColor = SystemColors.GradientInactiveCaption;
                pictureBox_rightpanel.BackColor = SystemColors.GradientInactiveCaption;
                panel_controls.BackColor = SystemColors.Control;
                splitContainer_bothimages.BackColor = lightbackground;
                foreach (Control subC in panel_controls.Controls)
                {
                    subC.BackColor = lightbackground;
                    subC.ForeColor = lighttextcolor;

                    if (subC is Button)
                    {
                        ((Button)subC).FlatStyle = FlatStyle.Standard;
                        ((Button)subC).FlatAppearance.BorderColor = SystemColors.Control;
                    }

                }
                DarkTitleBarClass.UseImmersiveDarkMode(Handle, false);
                this.BackColor = SystemColors.Control;

                label_filename_leftpanel.BackColor = lightbackground;
                label_filename_rightpanel.BackColor = lightbackground;
                //label_filename_leftpanel.ForeColor = lighttextcolor;
                //label_filename_rightpanel.ForeColor = lighttextcolor;
                label_imageindex_leftpanel.BackColor = lightbackground;
                //label_imageindex_leftpanel.ForeColor = lightforeground;
            }
        }

        // image quality when saving
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        { // https://efundies.com/csharp-save-jpg/
            var codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        int numberofimagepanels = 2;
        private void set_NumberOfPanels(int npanels)
        {
            if (npanels == 1)
            {
                panel_bothimages.Controls.Add(pictureBox_leftpanel);
                numberofimagepanels = 1;
                splitContainer_bothimages.Hide();
                button_numberofpanels.Text = "Switch to Dual";
                activePanel = 0;
            }
            else if (npanels == 2)
            {
                splitContainer_bothimages.Panel1.Controls.Add(pictureBox_leftpanel);
                Resize_imagepanels();
                numberofimagepanels = 2;
                splitContainer_bothimages.Show();
                button_numberofpanels.Text = "Switch to Single";
            }
        }
        private void button_numberofpanels_Click(object sender, EventArgs e)
        {
            // dirty hackish flip between 1 and 2
            int flip = numberofimagepanels - 1;
            flip = 1 - flip;
            set_NumberOfPanels(flip +1);
        }

        private void checkBox_hotkeyboth_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.HotkeyBoth = checkBox_hotkeyboth.Checked;
        }

        private void splitContainer_bothimages_MouseUp(object sender, MouseEventArgs e)
        {
            panel_bothimages.Focus(); // remove dotted line around splitter
        }
    } // end MainWindow : Form
}