using System.Runtime.InteropServices;

namespace Ace.Win32.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        int Left;
        int Top;
        int Right;
        int Bottom;
    }
}
