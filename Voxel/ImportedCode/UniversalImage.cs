using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ace
{
    /// <summary>
    /// 表示通用的图像
    /// </summary>
    public sealed class UniversalImage
    {
        private Image image;
        private ImageSource imageSource;
        private string fileName;

        /// <summary>
        /// 从文件读取Image，并断开与原文件的连接
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>读取到的Image</returns>
        public static Image GetImageFromFile(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Image image = Image.FromStream(fileStream);
            fileStream.Close();
            fileStream.Dispose();
            return image;
        }
        /// <summary>
        /// 从文件读取ImageSource，并断开与原文件的连接
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>读取到的ImageSource</returns>
        public static ImageSource GetImageSourceFromFile(string fileName)
        {
            return GetImageFromFile(fileName).ToImageSource();
        }
        /// <summary>
        /// 从文件名初始化通用图像
        /// </summary>
        /// <param name="fileName">文件名</param>
        public UniversalImage(string fileName)
        {
            if (!File.Exists(fileName)) throw new ArgumentException($"找不到文件：{fileName}");
            this.fileName = fileName;
        }
        /// <summary>
        /// 从现有的Image初始化通用图像
        /// </summary>
        /// <param name="image">现有的Image</param>
        public UniversalImage(Image image)
        {
            this.image = image ?? throw new NullReferenceException();            
        }
        /// <summary>
        /// 从现有的ImageSource初始化通用图像
        /// </summary>
        /// <param name="imageSource">现有的ImageSource</param>
        public UniversalImage(ImageSource imageSource)
        {
            this.imageSource = imageSource ?? throw new 
                NullReferenceException();
        }
        /// <summary>
        /// 获取等价的Image
        /// </summary>
        public Image Image
        {
            get
            {
                if (image == null)
                {
                    if (imageSource == null)
                    {
                        return image = GetImageFromFile(fileName);
                    }
                    else
                    {
                        return image = imageSource.ToImage();
                    }
                }
                else return image;
            }
        }
        /// <summary>
        /// 获取等价的ImageSource
        /// </summary>
        public ImageSource ImageSource
        {
            get
            {
                if (imageSource == null)
                {
                    if (image == null)
                    {
                        return imageSource = GetImageSourceFromFile(fileName);
                    }
                    else
                    {
                        return imageSource = image.ToImageSource();
                    }
                }
                else return imageSource;
            }
        }
        public static implicit operator Image(UniversalImage universalImage)
        {
            return universalImage.Image;
        }
        public static implicit operator ImageSource(UniversalImage universalImage)
        {
            return universalImage.ImageSource;
        }
    }
}
