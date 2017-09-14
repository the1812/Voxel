using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Win32.Enumerations
{
    /// <summary>
    /// 表示Shell图标状态
    /// </summary>
    [Flags]
    public enum ShellIconState
    {
        /// <summary>
        /// 普通状态
        /// </summary>
        ShellIconStateNormal = 0,
        /// <summary>
        /// 快捷方式
        /// </summary>
        ShellIconStateLinkOverlay = 0x8000,
        /// <summary>
        /// 选中状态
        /// </summary>
        ShellIconStateSelected = 0x10000,
        /// <summary>
        /// 打开状态
        /// </summary>
        ShellIconStateOpen = 0x2,
        /// <summary>
        /// 添加合适的覆盖图案
        /// </summary>
        ShellIconAddOverlays = 0x000000020,
    }
}
