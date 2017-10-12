using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Voxel.Model;

namespace Voxel.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Voxel.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Voxel.Controls;assembly=Voxel.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ImageSpliter/>
    ///
    /// </summary>
    public class ImageSpliter : Control
    {
        static ImageSpliter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageSpliter), new FrameworkPropertyMetadata(typeof(ImageSpliter)));
        }

        public static readonly DependencyProperty IsSplitProperty = DependencyProperty.Register(nameof(IsSplit), typeof(bool), typeof(ImageSpliter), new PropertyMetadata(false));
        public static readonly DependencyProperty BitmapSourceProperty = DependencyProperty.Register(nameof(BitmapSource), typeof(BitmapSource), typeof(ImageSpliter), new PropertyMetadata(null));
        private static readonly DependencyPropertyKey TopRightPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TopRight), typeof(BitmapSource), typeof(ImageSpliter), new PropertyMetadata(null));
        public static readonly DependencyProperty TopRightProperty = TopRightPropertyKey.DependencyProperty;
        private static readonly DependencyPropertyKey TopLeftPropertyKey = DependencyProperty.RegisterReadOnly("s", typeof(BitmapSource), typeof(ImageSpliter), new PropertyMetadata(null));
        public static readonly DependencyProperty TopLeftProperty = TopLeftPropertyKey.DependencyProperty;

        public bool IsSplit
        {
            get
            {
                return (bool) GetValue(IsSplitProperty);
            }
            set
            {
                SetValue(IsSplitProperty, value);
            }
        }
        public BitmapSource BitmapSource
        {
            get
            {
                return GetValue(BitmapSourceProperty) as BitmapSource;
            }
            set
            {
                SetValue(BitmapSourceProperty, value);
                TopLeft = new CroppedBitmap(value, new Int32Rect(0, 0, 
                    (int)TileSize.SmallWidthAndHeight, 
                    (int)TileSize.SmallWidthAndHeight));
                TopRight = new CroppedBitmap(value, new Int32Rect(
                    (int) (TileSize.SmallWidthAndHeight + TileSize.Gap), 
                    0, 
                    (int) TileSize.SmallWidthAndHeight, 
                    (int) TileSize.SmallWidthAndHeight));
                //bottomLeft = new CroppedBitmap(value, new Int32Rect(
                //    0,
                //    (int) (TileSize.SmallWidthAndHeight + TileSize.Gap),
                //    (int) TileSize.SmallWidthAndHeight,
                //    (int) TileSize.SmallWidthAndHeight));
                //bottomRight = new CroppedBitmap(value, new Int32Rect(
                //    (int) (TileSize.SmallWidthAndHeight + TileSize.Gap),
                //    (int) (TileSize.SmallWidthAndHeight + TileSize.Gap),
                //    (int) TileSize.SmallWidthAndHeight,
                //    (int) TileSize.SmallWidthAndHeight));
            }
        }
        public BitmapSource TopLeft
        {
            get
            {
                return GetValue(TopLeftProperty) as BitmapSource;
            }
            protected set
            {
                SetValue(TopLeftPropertyKey, value);
            }
        }
        public BitmapSource TopRight
        {
            get
            {
                return GetValue(TopRightProperty) as BitmapSource;
            }
            protected set
            {
                SetValue(TopRightPropertyKey, value);
            }
        }

        //private CroppedBitmap upLeft, bottomLeft, bottomRight;
    }
}
