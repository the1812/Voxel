using System;
using System.Runtime.InteropServices;

namespace Ace.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImageInfo
    {
        public IntPtr ImageHandle;
        public IntPtr MaskHandle;
        public int Unused1;
        public int Unused2;
        public Rectangle ImageRectangle;
    }
}
