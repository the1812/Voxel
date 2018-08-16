using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Voxel.Model;

namespace Voxel.Converters
{
    sealed class ColorToHueBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color c)
            {
                var color = new HsbColor(c);
                var brush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0),
                };
                for (var hue = 0M; hue <= 360M; hue += 60M)
                {
                    color.Hue = hue;
                    var stop = new GradientStop(color, (double) hue / 360.0);
                    brush.GradientStops.Add(stop);
                }
                return brush;
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
