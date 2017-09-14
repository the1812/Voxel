using System;
using System.Runtime.InteropServices;

namespace Ace.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShellFileInfo
    {
        public IntPtr IconHandle;
        public int IconIndex;
        public int Attributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string DisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string TypeName;
    }
}
