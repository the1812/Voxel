namespace Ace.Win32.Enumerations
{
    /// <summary>
    /// 表示系统指令(SC_*)
    /// </summary>
    public enum SystemCommand
    {
        /// <summary>
        /// 恢复窗口
        /// </summary>
        Restore = 0xF120,
        /// <summary>
        /// 最小化窗口
        /// </summary>
        Minimize = 0xF020,
        /// <summary>
        /// 使鼠标进入帮助状态
        /// </summary>
        ContextHelp = 0xF180,
        /// <summary>
        /// 关闭窗口
        /// </summary>
        Close = 0xF060,
        /// <summary>
        /// 选择默认项
        /// </summary>
        Default = 0xF160,
        /// <summary>
        /// 激活快捷键对应的窗口
        /// </summary>
        Hotkey = 0xF150,
        /// <summary>
        /// 水平滚动
        /// </summary>
        HorizontalScroll = 0xF080,
        /// <summary>
        /// 指示屏保是否安全
        /// </summary>
        IsSecure = 0x00_00_00_01,
        /// <summary>
        /// 发送Alt快捷键
        /// </summary>
        KeyMenu = 0xF100,
        /// <summary>
        /// 最大化窗口
        /// </summary>
        Maximize = 0xF030,
        /// <summary>
        /// 根据电池用量设置显示器的状态
        /// </summary>
        MonitorPower = 0xF170,
        /// <summary>
        /// 点击菜单
        /// </summary>
        MouseMenu = 0xF090,
        /// <summary>
        /// 移动窗口
        /// </summary>
        Move = 0xF010,
        /// <summary>
        /// 转到下一个窗口
        /// </summary>
        NextWindow = 0xF040,
        /// <summary>
        /// 转到上一个窗口
        /// </summary>
        PreviousWindow = 0xF050,
        /// <summary>
        /// 启动屏幕保护程序
        /// </summary>
        ScreenSaver = 0xF140,
        /// <summary>
        /// 调整窗口的大小
        /// </summary>
        Size = 0xF000,
        /// <summary>
        /// 打开"开始"菜单
        /// </summary>
        TaskList = 0xF130,
        /// <summary>
        /// 垂直滚动
        /// </summary>
        VerticalScroll = 0xF070
    }
}
