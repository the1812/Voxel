using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using GdiPlus = System.Drawing;

namespace Voxel
{
    sealed class ImageScaler
    {
        public Thickness Margin { get; set; } = new Thickness(0);
        public Size TargetSize { get; set; } = new Size(0, 0);
        public BitmapSource OriginalImage { get; set; }

        public ImageScaler(BitmapSource image) => OriginalImage = image;
        private BitmapSource fitGridSize(Size panelSize)
        {
            panelSize.Height -= Margin.Top + Margin.Bottom;
            panelSize.Width -= Margin.Left + Margin.Right;
            var panelRatio = panelSize.Ratio();
            var imageRatio = OriginalImage.Width / OriginalImage.Height;
            if (panelRatio >= imageRatio) //height equals
            {
                return OriginalImage.Zoom(panelSize.Height / OriginalImage.Height);
            }
            else //width equals
            {
                return OriginalImage.Zoom(panelSize.Width / OriginalImage.Width);
            }
        }
        private BitmapSource extend(BitmapSource image, Size newSize)
        {
            if (image == null)
            {
                throw new NullReferenceException();
            }
            var newImage = new GdiPlus.Bitmap(
                (int) (newSize.Width),
                (int) (newSize.Height));
            var gdiNewImage = GdiPlus.Graphics.FromImage(newImage);
            gdiNewImage.FillRectangle(new GdiPlus.SolidBrush(GdiPlus.Color.Transparent),
                0, 0, newImage.Width, newImage.Height);

            var marginedWidth = newImage.Width - Margin.Left - Margin.Right;
            var marginedHeight = newImage.Height - Margin.Top - Margin.Bottom;

            var x = Margin.Left + marginedWidth / 2;
            x -= image.Width / 2;
            var y = Margin.Top + marginedHeight / 2;
            y -= image.Height / 2;

            gdiNewImage.DrawImage(image.ToImage(), (float) x, (float) y, (float) image.Width, (float) image.Height);
            return newImage.ToImageSource() as BitmapSource;
        }
        public BitmapSource Scale()
        {
            if (TargetSize.IsEmpty)
            {
                throw new InvalidOperationException();
            }
            if (OriginalImage == null)
            {
                return null;
            }
            return extend(fitGridSize(TargetSize), TargetSize);
        }
    }
}
