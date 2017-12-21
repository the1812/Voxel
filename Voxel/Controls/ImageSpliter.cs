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
        static ImageSpliter() => 
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ImageSpliter),
                new FrameworkPropertyMetadata(typeof(ImageSpliter)));
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            IsSplit = !IsSplit;
        }
        public static Dictionary<Point, CroppedBitmap> Split(BitmapSource bitmap, Size gridSize)
        {
            var maxWidth = gridSize.Width * (TileSize.LargeWidthAndHeight + TileSize.Gap) - TileSize.Gap;
            var maxHeight = gridSize.Height * (TileSize.LargeWidthAndHeight + TileSize.Gap) - TileSize.Gap;
            var sourceRatio = bitmap.Width / bitmap.Height;
            var gridRatio = gridSize.Width / gridSize.Height;
            if (sourceRatio >= gridRatio)
            {
                bitmap = bitmap.Resize(new Size(maxWidth, maxWidth / sourceRatio), new Size(maxWidth, maxHeight));
            }
            else
            {
                bitmap = bitmap.Resize(new Size(maxHeight * sourceRatio, maxHeight), new Size(maxWidth, maxHeight));
            }

            var dpiX = MainView.Dpi.X;
            var dpiY = MainView.Dpi.Y;
            var width = Math.Truncate(bitmap.PixelWidth / (dpiX * (TileSize.LargeWidthAndHeight + TileSize.Gap)));
            var height = Math.Truncate(bitmap.PixelHeight / (dpiY * (TileSize.LargeWidthAndHeight + TileSize.Gap)));

            var dict = new Dictionary<Point, CroppedBitmap>();
            var dpi = MainView.Dpi;
            for (var column = 0; column < width; column++)
            {
                for (var row = 0; row < height; row++)
                {
                    dict.Add(new Point(column, row), new CroppedBitmap(
                        bitmap, new Int32Rect
                        {
                            Width = (int) (TileSize.LargeWidthAndHeight * dpi.X),
                            Height = (int) (TileSize.LargeWidthAndHeight * dpi.Y),
                            X = column * (int) ((TileSize.LargeWidthAndHeight + TileSize.Gap) * dpi.X),
                            Y = row * (int) ((TileSize.LargeWidthAndHeight + TileSize.Gap) * dpi.Y)
                        }
                        ));
                }
            }
            return dict;
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

        public bool IsSplit
        {
            get => (bool) GetValue(IsSplitProperty);
            set => SetValue(IsSplitProperty, value);
        }
        public BitmapSource BitmapSource
        {
            get => GetValue(BitmapSourceProperty) as BitmapSource;
            set => SetValue(BitmapSourceProperty, value);
        }
        public BitmapSource TopLeft
        {
            get => GetValue(TopLeftProperty) as BitmapSource;
            protected set => SetValue(TopLeftPropertyKey, value);
        }
        public BitmapSource TopRight
        {
            get => GetValue(TopRightProperty) as BitmapSource;
            protected set => SetValue(TopRightPropertyKey, value);
        }


        public BitmapSource BottomLeft
        {
            get => (BitmapSource) GetValue(BottomLeftProperty);
            protected set => SetValue(BottomLeftPropertyKey, value);
        }
        public BitmapSource BottomRight
        {
            get => (BitmapSource) GetValue(BottomRightProperty);
            protected set => SetValue(BottomRightPropertyKey, value);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




        public Stretch Stretch
        {
            get => (Stretch) GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageSpliter), new PropertyMetadata(Stretch.Fill));



    }
}
