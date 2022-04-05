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
            if (args.Length != 0)
            {
                MainWindow mainWindow = new MainWindow(args[0]);
                Application.Run(mainWindow);
            }
            else // open the program without loading anything
            {
                MainWindow mainWindow = new MainWindow();
                Application.Run(mainWindow);
            }

        }
    }
}