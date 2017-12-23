using Ace;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Voxel.View;
using Voxel.ViewModel;
using GdiPlus = System.Drawing;

namespace Voxel
{
    static class Extensions
    {
        public static Color ToColor(this int value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
        public static Color ToColor(this long value)
        {
            var bytes = BitConverter.GetBytes(value);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
        public static int ToInt32(this Color color) => (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        public static string ToHexString(this Color color)
        {
            var result = "#";
            result += color.R.ToString("X2");
            result += color.G.ToString("X2");
            result += color.B.ToString("X2");
            return result;
        }
        public static Color FromHexString(this string str)
        {
            if (str.StartsWith("#"))
            {
                str = str.Remove(0, 1);
            }
            if (!(str.Length == 6 && str.IsMatch("[a-fA-F0-9]*")))
            {
                return Ace.Wpf.DwmEffect.ColorizationColor;
            }
            var r = Convert.ToByte(str.Substring(0, 2), 16);
            var g = Convert.ToByte(str.Substring(2, 2), 16);
            var b = Convert.ToByte(str.Substring(4, 2), 16);
            return Color.FromRgb(r, g, b);
        }
        public static bool ShowMessage(this Window parent, string content, string title, bool showCancelButton)
        {
            var dialog = new MessageView()
            {
                Owner = parent
            };
            var viewModel = dialog.DataContext as MessageViewModel;
            if (string.IsNullOrWhiteSpace(content))
            {
                dialog.Height = 250.0;
                viewModel.TitleBackgroundOpacity = 1.0;
                viewModel.IsContentVisible = false;
            }
            viewModel.Content = content;
            viewModel.Title = title;
            viewModel.ShowCancelButton = showCancelButton;
            return dialog.ShowDialog() ?? false;
        }
        public static Size MeasureString(this string str)
        {
            if (str == null)
            {
                return new Size(0, 0);
            }
#pragma warning disable CS0618 // 类型或成员已过时
            var formattedText = new FormattedText(
                str,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                12.0,
                Brushes.Black
            );
#pragma warning restore CS0618 // 类型或成员已过时

            return new Size(formattedText.Width, formattedText.Height);
        }
        public static BitmapSource Resize(this BitmapSource source, Size size, Point dpi) => source.Resize(size.Width, size.Height, dpi);
        public static BitmapSource Resize(this BitmapSource source, double width, double height, Point dpi)
        {
            var bitmap = new TransformedBitmap(source,
                new ScaleTransform(
                    dpi.X * width / source.PixelWidth,
                    dpi.Y * height / source.PixelHeight));
            return bitmap;
        }
        public static Point GetDpi(this Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            double dpiX = 0.0, dpiY = 0.0;
            if (source != null)
            {
                dpiX = /*96.0 * */source.CompositionTarget.TransformToDevice.M11;
                dpiY = /*96.0 * */source.CompositionTarget.TransformToDevice.M22;
            }
            return new Point(dpiX, dpiY);
        }

        public static BitmapSource Extend(this BitmapSource image, Size newSize)
        {
            var newImage = new GdiPlus.Bitmap((int) newSize.Width, (int)newSize.Height);
            var gdiNewImage = GdiPlus.Graphics.FromImage(newImage);
            gdiNewImage.FillRectangle(new GdiPlus.SolidBrush(GdiPlus.Color.Transparent),
                0, 0, newImage.Width, newImage.Height);
            var x = newSize.Width / 2;
            x -= image.Width / 2;
            var y = newSize.Height / 2;
            y -= image.Height / 2;
            gdiNewImage.DrawImage(image.ToImage(), (float) x, (float) y);
            return newImage.ToImageSource() as BitmapSource;
        }
        public static BitmapSource Zoom(this BitmapSource image, double ratio)
            => image.Resize(image.Width * ratio, image.Height * ratio, MainView.Dpi);
        public static Point ToPoint(this Size size) => new Point(size.Width, size.Height);
        public static double Ratio(this Size size) => size.Width / size.Height;
        
    }
}
