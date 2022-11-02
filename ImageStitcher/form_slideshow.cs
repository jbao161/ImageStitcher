using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageStitcher
{
    public partial class form_slideshow : Form
    {
        public form_slideshow()
        {
            InitializeComponent();
        }

        private int timeParser(string pTime)
        {
            int iResult = 0;
            double dTime = 0.0;
            NumberStyles style = NumberStyles.Number;
            CultureInfo culture = CultureInfo.InvariantCulture;

            if (pTime.Contains("ms"))
            {
                if (Double.TryParse(pTime.Trim().Replace("ms", ""), style, culture, out dTime))
                {
                    iResult = (int)Math.Round(TimeSpan.FromMilliseconds(dTime).TotalSeconds);
                }
                else
                {
                    throw new FormatException("Unable to convert " + pTime);
                }
            }
            else if (pTime.Contains("s"))
            {
                if (Double.TryParse(pTime.Trim().Replace("s", ""), style, culture, out dTime))
                {
                    iResult = (int)Math.Round(TimeSpan.FromSeconds(dTime).TotalSeconds);
                }
                else
                {
                    throw new FormatException("Unable to convert " + pTime);
                }
            }
            else if (pTime.Contains("m"))
            {
                if (Double.TryParse(pTime.Trim().Replace("m", ""), style, culture, out dTime))
                {
                    iResult = (int)Math.Round(TimeSpan.FromMinutes(dTime).TotalSeconds);
                }
                else
                {
                    throw new FormatException("Unable to convert " + pTime);
                }
            }
            else if (pTime.Contains("h"))
            {
                if (Double.TryParse(pTime.Trim().Replace("h", ""), style, culture, out dTime))
                {
                    iResult = (int)Math.Round(TimeSpan.FromHours(dTime).TotalSeconds);
                }
                else
                {
                    throw new FormatException("Unable to convert " + pTime);
                }
            }
            else if (pTime.Contains("d"))
            {
                if (Double.TryParse(pTime.Trim().Replace("d", ""), style, culture, out dTime))
                {
                    iResult = (int)Math.Round(TimeSpan.FromDays(dTime).TotalSeconds);
                }
                else
                {
                    throw new FormatException("Unable to convert " + pTime);
                }
            }
            else
            {
                throw new FormatException(pTime + " is not a valid timeformat");
            }
            return iResult;
        }

        private void button_slideStart_Click(object sender, EventArgs e)
        {
            string leftmode = "";
            string rightmode = "";
            if (radioButton_leftordered.Checked) leftmode = "ordered";
            if (radioButton_leftrandom.Checked) leftmode = "random";
            if (radioButton_rightordered.Checked) rightmode = "ordered";
            if (radioButton_rightrandom.Checked) rightmode = "random";
            string timestring = comboBox1.Text;
            int timeinterval_sec = timeParser(timestring);
            mainForm.slideshow_stop();
            mainForm.slideshow_start(timeinterval_sec, leftmode, rightmode);
            this.Close();
        }

        private void form_slideshow_Load(object sender, EventArgs e)
        {
            pntLocation.X -= this.Size.Width/2;
            pntLocation.Y -= this.Size.Height ;
            this.Location = pntLocation;

            if (String.Equals(Properties.Settings.Default.SlideLmode, "ordered")){ radioButton_leftordered.Checked = true; }
            if (String.Equals(Properties.Settings.Default.SlideLmode, "random")){ radioButton_leftrandom.Checked = true; }
            if (String.Equals(Properties.Settings.Default.SlideRmode, "ordered")){ radioButton_rightordered.Checked = true; }
            if (String.Equals(Properties.Settings.Default.SlideRmode, "random")){ radioButton_rightrandom.Checked = true; }

            comboBox1.Text = Properties.Settings.Default.SlideTimeInterval;
        }

        private void button_slideEnd_Click(object sender, EventArgs e)
        {
            mainForm.slideshow_stop();
            this.Close();
        }

        private void form_slideshow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (radioButton_leftordered.Checked) Properties.Settings.Default.SlideLmode = "ordered";
            if (radioButton_leftrandom.Checked) Properties.Settings.Default.SlideLmode = "random";
            if (radioButton_rightordered.Checked) Properties.Settings.Default.SlideRmode = "ordered";
            if (radioButton_rightrandom.Checked) Properties.Settings.Default.SlideRmode = "random";

            Properties.Settings.Default.SlideTimeInterval = comboBox1.Text;

            Properties.Settings.Default.Save();
        }
    }
}
