using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Voxel.View;
using Voxel.ViewModel;

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
        public static string ToHexString(this Color color)
        {
            string result = "#";
            result += color.R.ToString("X2");
            result += color.G.ToString("X2");
            result += color.B.ToString("X2");
            return result;
        }
        public static bool ShowMessage(this Window parent, string content, string title, bool showCancelButton)
        {
            var dialog = new MessageView()
            {
                Owner = parent
            };
            var viewModel = dialog.DataContext as MessageViewModel;
            viewModel.Content = content;
            viewModel.Title = title;
            viewModel.ShowCancelButton = showCancelButton;
            return dialog.ShowDialog() ?? false;
        }
        public static string ToJsonPath(this string path)
        {
            return path.Replace("\\", "\\\\");
        }
        public static string FromJsonPath(this string path)
        {
            return path.Replace("\\\\", "\\");
        }
    }
}
