using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Win32.Enumerations
{
    /// <summary>
    /// 表示Windows消息(WM_*) (部分)
    /// </summary>
    public enum WindowsMessage
    {
        /// <summary>
        /// 系统指令
        /// </summary>
        SystemCommand = 0x0112,
        /// <summary>
        /// 设置图标
        /// </summary>
        SetIcon = 0x0080,
    }
}
