using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    /// <summary>
    /// 表示Windows版本
    /// </summary>
    public enum WindowsVersion
    {
        /// <summary>
        /// 未知的Windows版本
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Windows Vista
        /// </summary>
        Vista = 1,
        /// <summary>
        /// Windows 7
        /// </summary>
        Seven = 2,
        /// <summary>
        /// Windows 7 SP1
        /// </summary>
        SevenSP1 = 3,
        /// <summary>
        /// Windows 8
        /// </summary>
        Eight = 4,
        /// <summary>
        /// Windows 8.1
        /// </summary>
        EightDotOne = 5,
        /// <summary>
        /// Windows 8.1 (含更新)
        /// </summary>
        EightDotOneWithUpdate = 6,
        /// <summary>
        /// WIndows 10 初版
        /// </summary>
        Ten = 7,
        /// <summary>
        /// Windows 10 版本1511
        /// </summary>
        Ten1511 = 8,
        /// <summary>
        /// Windows 10 版本1607
        /// </summary>
        Ten1607 = 9,
        /// <summary>
        /// Windows 10 版本1703
        /// </summary>
        Ten1703 = 10,
    }
}
