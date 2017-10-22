using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Voxel
{
    sealed class BooleanStretchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (b)
                {
                    return Stretch.Uniform;
                }
                else
                {
                    return Stretch.Fill;
                }
            }
            return Stretch.Uniform;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Stretch stretch)
            {
                if (stretch == Stretch.Uniform)
                {
                    return true;
                }
                else if (stretch == Stretch.Fill)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
