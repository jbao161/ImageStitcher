using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using System;
using System.IO;
using System.Security.Permissions;
using System.Drawing;
using System.Net;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Threading;
using System.Windows.Forms.Layout;
using Microsoft.Win32;


namespace ImageStitcher
{


    public class PictureBoxS : PictureBox
    {
        private bool currentlyAnimating;
        private Image image;

        public object ParentInternal { get; private set; }

         void  Animate()
        {
            Animate(!DesignMode && Visible && Enabled && ParentInternal != null);
        }

        private void StopAnimate()
        {
            Animate(false);
        }

        private void Animate(bool animate)
        {
            if (animate != this.currentlyAnimating)
            {
                if (animate)
                {
                    if (this.image != null)
                    {
                        ImageAnimator.Animate(this.image, new EventHandler(this.OnFrameChanged));
                        this.currentlyAnimating = animate;
                    }
                }
                else
                {
                    if (this.image != null)
                    {
                        ImageAnimator.StopAnimate(this.image, new EventHandler(this.OnFrameChanged));
                        this.currentlyAnimating = animate;
                    }
                }
            }
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
