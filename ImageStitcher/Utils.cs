using System;
using System.Drawing;

namespace ImageStitcher
{
    internal class Utils
    {
        public static long GetFileSize(String path)
        {
            return new System.IO.FileInfo(path).Length;
        }

        public static string FileSizetoString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + " " + suf[place];
        }

        public static string GetFileSizeString(String path)
        {
            return FileSizetoString(GetFileSize(path));
        }

        public static string GetDimensionString(Image image)
        {
            String result = image.Width.ToString() + "x" + image.Height.ToString();
            return result;
        }
    }
}