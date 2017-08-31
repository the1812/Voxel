using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Voxel
{
    static class Extensions
    {
        public static Color ToColor(this int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }
        public static int ToInt32(this Color color)
        {
            return (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
        }
    }
}
