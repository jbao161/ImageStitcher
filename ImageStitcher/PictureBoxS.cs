using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageStitcher
{
    public class PictureBoxS : PictureBox
    {
        private bool currentlyAnimating;
        private Image image;

        public object ParentInternal { get; private set; }

        private void Animate()
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