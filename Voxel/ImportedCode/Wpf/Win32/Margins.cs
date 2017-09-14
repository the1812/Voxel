using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    struct Margins
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    };
}
