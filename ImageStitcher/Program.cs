using System;
using System.Windows.Forms;

namespace ImageStitcher
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // If a filepath is provided, load that image at start
            switch (args.Length)
            {
                case 0: // load nothing
                    {
                    MainWindow mainWindow = new MainWindow();
                    Application.Run(mainWindow);
                    }
                break;
                case 1: // load one image
                {
                    MainWindow mainWindow = new MainWindow(args[0]);
                    Application.Run(mainWindow);
                }
                break;
                case 2: // load two images or load one image with subfolder setting
                {
                        bool loadsubfolders = false;
                        if (args[2].ToString().Equals("-sub")|| args[2].ToString().Equals("-nsub"))
                        {
                            loadsubfolders = args[2].ToString().Equals("-sub") ? true : false;
                            MainWindow mainWindow = new MainWindow(args[0], loadsubfolders);
                            Application.Run(mainWindow);
                        }
                        else
                        {
                            MainWindow mainWindow = new MainWindow(args[0], args[1]);
                            Application.Run(mainWindow);
                        }
                }
                break;
                case 3: // load two file/folders. same subfolder settings
                    {
                        bool loadsubfolders = false;
                        if (args[2].ToString().Equals("-sub") || args[2].ToString().Equals("-nsub"))
                        {
                            loadsubfolders = args[2].ToString().Equals("-sub") ? true : false;

                            MainWindow mainWindow = new MainWindow(args[0], args[1], loadsubfolders);
                            Application.Run(mainWindow);
                        }
                    }
                    break;
                case 4: // load two files/folders. individual subfolder settings
                    {
                        bool lsub = false;
                        bool rsub = false;
                        if ((args[1].ToString().Equals("-sub") || args[1].ToString().Equals("-nsub"))
                            && (args[3].ToString().Equals("-sub") || args[3].ToString().Equals("-nsub")))
                        {
                            lsub = args[1].ToString().Equals("-sub") ? true : false;
                            rsub = args[3].ToString().Equals("-sub") ? true : false;
                            MainWindow mainWindow = new MainWindow(args[0], args[2], lsub, rsub);
                            Application.Run(mainWindow);
                        }
                    }
                    break;
            }
        }
    }
}