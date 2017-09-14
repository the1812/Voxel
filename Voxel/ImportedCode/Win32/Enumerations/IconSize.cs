using System;

namespace Ace.Win32.Enumerations
{
    /// <summary>
    /// 表示图标大小
    /// </summary>
    [Flags]
    public enum IconSize : int
    {        
        /// <summary>
        /// 最小（16x16）
        /// </summary>
        Minimum = 0x1,
        /// <summary>
        /// 中等（32x32）
        /// </summary>
        Medium = 0x0,
        /// <summary>
        /// 大（48x48）
        /// </summary>
        Large = 0x2,
        /// <summary>
        /// 最大（256x256）
        /// </summary>
        Maximum = 0x4
    }
    
}
