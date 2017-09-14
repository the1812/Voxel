using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Ace;
using System.Drawing;
using System.IO;

namespace Ace.Wpf
{
    public static class WpfUtils
    {
        /// <summary>
        /// 获取文件的缩略图
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="width">指定的宽度</param>
        /// <param name="height">指定的高度</param>
        /// <returns>缩略图</returns>
        public static ImageSource GetThumbnail(string fileName, int width, int height)
        {
            var image = UniversalImage.GetImageFromFile(fileName);
            return image.GetThumbnailImage(width, height, () => true, new IntPtr()).ToImageSource();
        }
        /// <summary>
        /// 获取文件的缩略图
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="zoomRatio">指定的缩放比例</param>
        /// <returns>缩略图</returns>
        public static ImageSource GetThumbnail(string fileName, double zoomRatio)
        {
            var image = UniversalImage.GetImageFromFile(fileName);
            return image.GetThumbnailImage((int)(zoomRatio * image.Width), (int)(zoomRatio * image.Height), () => true, new IntPtr()).ToImageSource();
        }
    }
}
