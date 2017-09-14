using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Ace.Win32.Structures;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Ace.Win32
{
    /// <summary>
    /// 封装Windows API的类
    /// </summary>
    public static partial class Api
    {
        #region 跨平台调用声明
        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                   int x, int y, int width, int height, uint flags);
        [DllImport("user32.dll")]
        internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        /// <summary>
        /// 获取内核文件信息
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileAttributes">文件属性</param>
        /// <param name="fileInfo">接受结果的内核文件信息结构</param>
        /// <param name="fileInfoSize">结构的大小</param>
        /// <param name="fileInfoConfig">结构的设置</param>
        /// <returns>表示操作是否成功的值</returns>
        [DllImport("shell32", EntryPoint = "SHGetFileInfo")]
        internal static extern IntPtr ShellGetFileInfo(
            string path,
            int fileAttributes,
            ref ShellFileInfo fileInfo,
            uint fileInfoSize,
            uint fileInfoConfig);

        /// <summary>
        /// 图像列表获取图标的静态方法
        /// </summary>
        /// <param name="imageListHandle">图像列表句柄</param>
        /// <param name="index">图标的索引</param>
        /// <param name="flags">参数</param>
        /// <returns>图标的句柄</returns>
        [DllImport("comctl32", EntryPoint = "ImageList_GetIcon")]
        internal extern static IntPtr ImageListGetIcon(
            IntPtr imageListHandle,
            int index,
            int flags);

        /// <summary>
        /// SHGetImageList 在XP中没有正确导出。请参阅 KB316931
        /// http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q316931
        /// 希望该函数的序号 727 未来不会改变。
        /// </summary>
        [DllImport("shell32.dll", EntryPoint = "#727")]
        internal extern static int ShellGetImageList(
            int iconSize,
            ref Guid guid,
            ref IImageList interfacePointer
            );
        /// <summary>
        /// 获取图像列表的句柄
        /// </summary>
        /// <param name="iconSize">请求的图标大小</param>
        /// <param name="guid">图像列表的GUID</param>
        /// <param name="handle">接收结果的图像列表句柄</param>
        [DllImport("shell32.dll", EntryPoint = "#727")]
        internal extern static int ShellGetImageListHandle(
            int iconSize,
            ref Guid guid,
            ref IntPtr handle
            );
        [DllImport("Shell32.dll")]
        internal static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
        internal static extern bool EnumWindows(WindowEnumFunc lpEnumFunc, uint lParam);
        internal delegate bool WindowEnumFunc(IntPtr hwnd, uint lParam);
        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        internal static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        internal static extern bool IsWindow(IntPtr hWnd);
        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        internal static extern void SetLastError(uint dwErrCode);
        [DllImport("User32.dll")]
        internal static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern void ShowCursor(int status);
        
        #endregion
        #region 刷新图标缓存
        /// <summary>
        /// 刷新图标缓存
        /// </summary>
        public static void RefreshIconCache()
        {
            //SHChangeNotify(0x7FFFFFFF, 0x0003, IntPtr.Zero, IntPtr.Zero);
            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            //SHChangeNotify(0x00001000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
        #endregion
        #region 获取进程窗口句柄
        private static Hashtable processesTable = new Hashtable();
        /// <summary>
        /// 获取进程的主窗口句柄
        /// </summary>
        /// <returns>进程的主窗口句柄</returns>
        public static IntPtr GetProcessWindowHandle(Process p)
        {
            IntPtr ptrWnd = IntPtr.Zero;
            uint uiPid = (uint)p.Id;  // 进程 ID
            object objWnd = processesTable[uiPid];

            if (objWnd != null)
            {
                ptrWnd = (IntPtr)objWnd;
                if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))  // 从缓存中获取句柄
                {
                    return ptrWnd;
                }
                else
                {
                    ptrWnd = IntPtr.Zero;
                }
            }

            bool bResult = EnumWindows((hwnd, lParam) => {
                uint Pid = 0;

                if (GetParent(hwnd) == IntPtr.Zero)
                {
                    GetWindowThreadProcessId(hwnd, ref Pid);
                    if (Pid == lParam)    // 找到进程对应的主窗口句柄
                    {
                        processesTable[Pid] = hwnd;   // 把句柄缓存起来
                        SetLastError(0);    // 设置无错误
                        return false;   // 返回 false 以终止枚举窗口
                    }
                }

                return true;
            }, uiPid);

            // 枚举窗口返回 false 并且没有错误号时表明获取成功
            if (!bResult && Marshal.GetLastWin32Error() == 0)
            {
                objWnd = processesTable[uiPid];
                if (objWnd != null)
                {
                    ptrWnd = (IntPtr)objWnd;
                }
            }

            return ptrWnd;
        }
        //private static bool EnumWindowsProc(IntPtr hwnd, uint lParam)
        //{
        //    uint uiPid = 0;

        //    if (GetParent(hwnd) == IntPtr.Zero)
        //    {
        //        GetWindowThreadProcessId(hwnd, ref uiPid);
        //        if (uiPid == lParam)    // 找到进程对应的主窗口句柄
        //        {
        //            processesTable[uiPid] = hwnd;   // 把句柄缓存起来
        //            SetLastError(0);    // 设置无错误
        //            return false;   // 返回 false 以终止枚举窗口
        //        }
        //    }

        //    return true;
        //}
        #endregion
        #region 防止程序多开
        /// <summary>
        /// 获取与当前相同程序的进程，如果不存在，则返回null
        /// </summary>
        /// <returns>相同程序的进程</returns>
        public static Process GetSameRunningProgram()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    //if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    //{
                    return process;
                    //}
                }
            }
            return null;
        }
        /// <summary>
        /// 将进程激活
        /// </summary>
        /// <param name="p">要激活的进程</param>
        public static void BringToFront(Process p)
        {
            ShowWindowAsync(p.MainWindowHandle, 1);//1即WS_SHOWNORMAL
            SetForegroundWindow(p.MainWindowHandle);
        }
        #endregion        
        #region 控制光标显示
        /// <summary>
        /// 指定是否显示光标
        /// </summary>
        /// <param name="status">指示是否显示光标的值</param>
        public static void ShowCursor(bool status)
        {
            if (status)
                ShowCursor(1);
            else
                ShowCursor(0);
        }
        #endregion
        #region 移除WPF窗口图标
        /// <summary>
        /// <para>移除WPF窗口的图标</para>
        /// <para>必须在OnSourceInitialized方法中调用，否则无效</para>
        /// </summary>
        /// <param name="window">WPF窗口</param>
        public static void RemoveTitleIcon(this Window window)
        {
            var handle = window.GetHandle();
            var windowLong = GetWindowLong(handle, -20);
            SetWindowLong(handle, -20, windowLong | 0x0001);
            SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, 0x0001 | 0x0002 | 0x0004 | 0x0020);
        }
        #endregion
    }
    
}
