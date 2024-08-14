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
                case 2: // load two images
                {
                    MainWindow mainWindow = new MainWindow(args[0], args[1]);
                    Application.Run(mainWindow);
                }
                break;

            }
            if (args.Length != 0)
            {

            } else if (args.Length > 1) // load two images if there are two filepaths
            {

            }
            else // open the program without loading anything
            {

            }
        }
    }
}