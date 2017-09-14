using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Ace.Win32;
using System.Windows.Media.Effects;

namespace Ace.Wpf
{
    /// <summary>
    /// 表示桌面窗口管理器相关的效果
    /// </summary>
    public static class DwmEffect
    {
        /// <summary>
        /// 对窗口启用 Aero Glass 效果
        /// </summary>
        /// <param name="window">要启用的窗口</param>
        public static void AeroGlassBlur(this Window window)
        {
            if (Utils.OSVersion.GetWindowsVersion() >= WindowsVersion.Eight)
            {
                throw new NotSupportedException("Aero Glass 效果只能用于 Windows 7 以下的系统");
            }
            IntPtr mainWindowPtr = new WindowInteropHelper(window).Handle;
            HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
            mainWindowSrc.CompositionTarget.BackgroundColor = ColorizationColor;

            Margins margins = new Margins
            {
                Left = -1,
                Right = -1,
                Top = -1,
                Bottom = -1
            };

            WpfApi.DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
        }
        /// <summary>
        /// 获取适用于 Aero Glass 背景下的文字效果
        /// </summary>
        public static DropShadowEffect AeroGlassForegroundEffect
        {
            get
            {
                if (Utils.OSVersion.GetWindowsVersion() < WindowsVersion.Eight)
                {
                    return new DropShadowEffect()
                    {
                        Color = Colors.White,
                        BlurRadius = 20,
                        ShadowDepth = 0,
                    };
                }
                return null;
            }
        }
        /// <summary>
        /// 获取根据系统的自定义窗口颜色计算出的合适的前景色
        /// </summary>
        public static Color ForegroundColor
        {
            get
            {
                double gray = 1 - (0.299 * ColorizationColor.R + 0.587 * ColorizationColor.G + 0.114 * ColorizationColor.B) / 255;

                if (gray < 0.5)
                    return Colors.Black;
                else
                    return Colors.White;
            }
        }
        /// <summary>
        /// 获取系统的自定义窗口颜色
        /// </summary>
        public static Color ColorizationColor
        {
            get
            {
                Color colorFromInt(int value)
                {
                    byte r = (byte)(value >> 16);
                    byte g = (byte)(value >> 8);
                    byte b = (byte)value;
                    return Color.FromRgb(r, g, b);
                }
                var key = Utils.OpenRegistryKey(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM");
                int colorValue = (int)(key.GetValue("ColorizationColor") ?? 0);
                if (colorValue == 0)
                {
                    return Colors.White;
                }
                if (Utils.OSVersion.GetWindowsVersion() >= WindowsVersion.Eight)
                {
                    if ((int)(key.GetValue("EnableWindowColorization") ?? 0) == 1)
                    {
                        return colorFromInt(colorValue);
                    }
                    else
                    {
                        return Colors.White;
                    }
                }
                else
                {
                    return colorFromInt(colorValue);
                }
            }
        }
        /// <summary>
        /// 根据系统版本自动对窗口应用效果
        /// </summary>
        /// <param name="window">要应用的窗口</param>
        public static void ApplyDwmEffect(this Window window)
        {
            if (Utils.OSVersion.GetWindowsVersion() < WindowsVersion.Eight)
            {
                window.AeroGlassBlur();
                window.Foreground = new SolidColorBrush(Colors.Black);
                window.Effect = AeroGlassForegroundEffect;
            }
            else
            {
                window.Background = new SolidColorBrush(ColorizationColor);
                window.Foreground = new SolidColorBrush(ForegroundColor);
            }
        }
    }
}
