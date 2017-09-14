using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ace
{
    public static class ImageConverter
    {
        public static Image ToImage(this ImageSource imageSource)
        {
            MemoryStream memoryStream = new MemoryStream();
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));
            encoder.Save(memoryStream);
            memoryStream.Flush();
            return Image.FromStream(memoryStream);
        }
        public static ImageSource ToImageSource(this Image image)
        {
            BitmapImage imageSource = new BitmapImage();

            imageSource.BeginInit();

            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);
            imageSource.StreamSource = memoryStream;

            imageSource.EndInit();

            return imageSource;
        }
    }
}
