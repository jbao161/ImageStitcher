using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ImageStitcher
{
    public class ZoomFactor
    { //https://stackoverflow.com/questions/53800328/translate-rectangle-position-in-zoom-mode-picturebox
        public ZoomFactor() { }

        public PointF TranslateZoomPosition(PointF coordinates, SizeF containerSize, SizeF imageSize)
        {
            PointF imageOrigin = TranslateCoordinatesOrigin(coordinates, containerSize, imageSize);
            float scaleFactor = GetScaleFactor(containerSize, imageSize);
            return new PointF(imageOrigin.X / scaleFactor, imageOrigin.Y / scaleFactor);
        }

        public RectangleF TranslateZoomSelection(RectangleF selectionRect, SizeF containerSize, SizeF imageSize)
        {
            PointF selectionTrueOrigin = TranslateZoomPosition(selectionRect.Location, containerSize, imageSize);
            float scaleFactor = GetScaleFactor(containerSize, imageSize);

            SizeF selectionTrueSize = new SizeF(selectionRect.Width / scaleFactor, selectionRect.Height / scaleFactor);
            return new RectangleF(selectionTrueOrigin, selectionTrueSize);
        }

        public RectangleF TranslateSelectionToZoomedSel(RectangleF selectionRect, SizeF containerSize, SizeF imageSize)
        {
            float scaleFactor = GetScaleFactor(containerSize, imageSize);
            RectangleF zoomedSelectionRect = new
                RectangleF(selectionRect.X * scaleFactor, selectionRect.Y * scaleFactor,
                           selectionRect.Width * scaleFactor, selectionRect.Height * scaleFactor);

            PointF imageScaledOrigin = GetImageScaledOrigin(containerSize, imageSize);
            zoomedSelectionRect.Location = new PointF(zoomedSelectionRect.Location.X + imageScaledOrigin.X,
                                                      zoomedSelectionRect.Location.Y + imageScaledOrigin.Y);
            return zoomedSelectionRect;
        }

        public PointF TranslateCoordinatesOrigin(PointF coordinates, SizeF containerSize, SizeF imageSize)
        {
            PointF imageOrigin = GetImageScaledOrigin(containerSize, imageSize);
            return new PointF(coordinates.X - imageOrigin.X, coordinates.Y - imageOrigin.Y);
        }

        public PointF GetImageScaledOrigin(SizeF containerSize, SizeF imageSize)
        {
            SizeF imageScaleSize = GetImageScaledSize(containerSize, imageSize);
            return new PointF((containerSize.Width - imageScaleSize.Width) / 2,
                              (containerSize.Height - imageScaleSize.Height) / 2);
        }

        public SizeF GetImageScaledSize(SizeF containerSize, SizeF imageSize)
        {
            float scaleFactor = GetScaleFactor(containerSize, imageSize);
            return new SizeF(imageSize.Width * scaleFactor, imageSize.Height * scaleFactor);

        }
        internal float GetScaleFactor(SizeF scaled, SizeF original)
        {
            return (original.Width / original.Height > scaled.Width / scaled.Height) ? (scaled.Width / original.Width)
                                                      : (scaled.Height / original.Height);
        }

        public RectangleF ConstrainCropAreaToImage(RectangleF rect, SizeF imageSize)
        { // prevent cropping area from going outside the image
            float rl, rt, rw, rh;
            rl = rect.Left;
            rt = rect.Top;
            rw = rect.Width;
            rh = rect.Height;
            if (rect.Top < 0) { rt = 0; rh -= Math.Abs(rect.Top); }
            if (rect.Top > imageSize.Height - rect.Height) { rh -= Math.Abs(rect.Top+rect.Height - imageSize.Height); }
            if (rect.Left < 0) { rl = 0; rw -= Math.Abs(rect.Left); }
            if (rect.Left > imageSize.Width - rect.Width) { rw -= Math.Abs(rect.Left+rect.Width - imageSize.Width); }
    
            return new RectangleF(rl, rt, rw, rh);
        }
    }
}
