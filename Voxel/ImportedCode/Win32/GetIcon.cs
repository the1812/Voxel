using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Ace.Win32.Enumerations;

namespace Ace.Win32
{
    public static partial class Api
    {
        /// <summary>
        /// 获取指定文件的图标
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="iconSize">图标的大小</param>
        public static UniversalImage GetIcon(string filePath, IconSize iconSize = IconSize.Maximum)
        {
            ImageList imageList = new ImageList(iconSize);
            Icon icon = imageList.GetIcon(imageList.GetIconIndex(filePath, true));
            return new UniversalImage(icon.ToBitmap());
        }
    }
}
