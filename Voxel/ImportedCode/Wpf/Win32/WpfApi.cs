using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using Ace.Win32.Enumerations;

namespace Ace.Win32
{
	/// <summary>
    /// 封装供WPF应用程序使用的Windows API
    /// </summary>
    public static class WpfApi
    {
        [DllImport("DwmApi.dll")]
        internal static extern int DwmExtendFrameIntoClientArea(
            IntPtr hwnd,
            ref Margins pMarInset);
        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        /// <summary>
        /// 获取窗口的句柄
        /// </summary>
        /// <param name="window">要获取句柄的窗口</param>
        /// <returns>窗口的句柄</returns>
        public static IntPtr GetHandle(this Window window)
        {
            return new WindowInteropHelper(window).Handle;
        }
        /// <summary>
        /// 向窗口发送消息
        /// </summary>
        /// <param name="window">目标窗口</param>
        /// <param name="wm">消息类型</param>
        /// <param name="wParam">消息参数</param>
        public static void SendMessage(this Window window, WindowsMessage wm, IntPtr wParam)
        {
            SendMessage(window.GetHandle(), wm.To<int>(), wParam, IntPtr.Zero);
        }
        /// <summary>
        /// 向窗口发送含附加参数的消息
        /// </summary>
        /// <param name="window">目标窗口</param>
        /// <param name="wm">消息类型</param>
        /// <param name="wParam">消息参数</param>
        /// <param name="lParam">消息的附加参数</param>
        public static void SendMessage(this Window window, WindowsMessage wm, IntPtr wParam, IntPtr lParam)
        {
            SendMessage(window.GetHandle(), wm.To<int>(), wParam, lParam);
        }
        /// <summary>
        /// 向窗口发送系统消息
        /// </summary>
        /// <param name="window">目标窗口</param>
        /// <param name="sc">系统消息</param>
        public static void SendMessage(this Window window, SystemCommand sc)
        {
            SendMessage(window, sc, IntPtr.Zero);
        }
        /// <summary>
        /// 向窗口发送含附加参数的系统消息
        /// </summary>
        /// <param name="window">目标窗口</param>
        /// <param name="sc">系统消息</param>
        /// <param name="lParam">消息的附加参数</param>
        public static void SendMessage(this Window window, SystemCommand sc, IntPtr lParam)
        {
            SendMessage(window.GetHandle(), WindowsMessage.SystemCommand.To<int>(), new IntPtr(sc.To<int>()), lParam);
        }
        /// <summary>
        /// <para>为窗口注册系统消息接受函数</para>
        /// <para>注:不能在构造函数中使用</para>
        /// </summary>
        /// <param name="window">要注册的窗口</param>
        /// <param name="receiver">系统消息接受函数</param>
        public static void ReceiveMessage(this Window window, HwndSourceHook receiver)
        {
            IntPtr handle = window.GetHandle();
            HwndSource.FromHwnd(handle == IntPtr.Zero ? throw new InvalidOperationException("无效操作。" + Environment.NewLine + "不能在构造函数中使用。") : handle).AddHook(receiver);
        }
    }
}
