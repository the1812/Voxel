using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Voxel.View;

namespace Voxel.Controls
{
    public class ImageSpliter : Control, INotifyPropertyChanged
    {
        static ImageSpliter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageSpliter), new FrameworkPropertyMetadata(typeof(ImageSpliter)));
        }
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            IsSplit = !IsSplit;
        }

        public static readonly DependencyProperty IsSplitProperty = DependencyProperty.Register(
            nameof(IsSplit),
            typeof(bool),
            typeof(ImageSpliter), 
            new PropertyMetadata(false));
        public static readonly DependencyProperty BitmapSourceProperty = DependencyProperty.Register(
            nameof(BitmapSource), 
            typeof(BitmapSource), 
            typeof(ImageSpliter), 
            new PropertyMetadata(null, (s, e)=>
            {
                if (s is ImageSpliter spliter)
                {
                    var dpi = MainView.Dpi;
                    spliter.TopLeft = new CroppedBitmap(e.NewValue as BitmapSource, new Int32Rect(
                        0, 
                        0,
                        (int) (dpi.X * TileSize.SmallWidthAndHeight),
                        (int) (dpi.Y * TileSize.SmallWidthAndHeight)));
                    spliter.TopRight = new CroppedBitmap(e.NewValue as BitmapSource, new Int32Rect(
                        (int) (dpi.X * (TileSize.SmallWidthAndHeight + TileSize.Gap)),
                        0,
                        (int) (dpi.X * TileSize.SmallWidthAndHeight),
                        (int) (dpi.Y * TileSize.SmallWidthAndHeight)));
                    spliter.BottomLeft = new CroppedBitmap(e.NewValue as BitmapSource, new Int32Rect(
                        0,
                        (int) (dpi.Y * (TileSize.SmallWidthAndHeight + TileSize.Gap)),
                        (int) (dpi.X * TileSize.SmallWidthAndHeight),
                        (int) (dpi.Y * TileSize.SmallWidthAndHeight)));
                    spliter.BottomRight = new CroppedBitmap(e.NewValue as BitmapSource, new Int32Rect(
                        (int) (dpi.X * (TileSize.SmallWidthAndHeight + TileSize.Gap)),
                        (int) (dpi.Y * (TileSize.SmallWidthAndHeight + TileSize.Gap)),
                        (int) (dpi.X * TileSize.SmallWidthAndHeight),
                        (int) (dpi.Y * TileSize.SmallWidthAndHeight)));
                    spliter.OnPropertyChanged(nameof(BitmapSource));
                    spliter.OnPropertyChanged(nameof(TopLeft));
                    spliter.OnPropertyChanged(nameof(TopRight));
                    spliter.OnPropertyChanged(nameof(BottomLeft));
                    spliter.OnPropertyChanged(nameof(BottomRight));
                }
            }));

        private static readonly DependencyPropertyKey TopRightPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(TopRight), 
            typeof(BitmapSource), 
            typeof(ImageSpliter), 
            new PropertyMetadata(null));
        public static readonly DependencyProperty TopRightProperty = TopRightPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey TopLeftPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(TopLeft),
            typeof(BitmapSource),
            typeof(ImageSpliter),
            new PropertyMetadata(null));
        public static readonly DependencyProperty TopLeftProperty = TopLeftPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey BottomLeftPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(BottomLeft),
            typeof(BitmapSource),
            typeof(ImageSpliter),
            new PropertyMetadata(null));
        public static readonly DependencyProperty BottomLeftProperty = BottomLeftPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey BottomRightPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(BottomRight),
            typeof(BitmapSource),
            typeof(ImageSpliter),
            new PropertyMetadata(null));
        public static readonly DependencyProperty BottomRightProperty = BottomRightPropertyKey.DependencyProperty;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            get { return GetValue(TopRightProperty) as BitmapSource; }
            protected set
            {
                SetValue(TopRightPropertyKey, value);
            }
        }


        public BitmapSource BottomLeft
        {
            get { return (BitmapSource) GetValue(BottomLeftProperty); }
            protected set { SetValue(BottomLeftPropertyKey, value); }
        }
        public BitmapSource BottomRight
        {
            get { return (BitmapSource) GetValue(BottomRightProperty); }
            protected set { SetValue(BottomRightPropertyKey, value); }
        }

    }
}