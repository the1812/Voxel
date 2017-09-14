using System;
using System.ComponentModel;
using System.IO;

namespace Ace.Files
{
    /// <summary>
    /// 指定快捷方式启动的窗口状态
    /// </summary>
    public enum ShortcutWindowStyle
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 最大化状态
        /// </summary>
        Maximum = 3,
        /// <summary>
        /// 最小化状态
        /// </summary>
        Minimum = 7,
    }
    /// <summary>
    /// 表示一个快捷方式文件
    /// </summary>
    public sealed class ShortcutFile : FileBase, INotifyPropertyChanged
    {
        private string targetPath, arguments, description, workingDirectory, iconLocation;
        private char hotkey;
        private ShortcutWindowStyle windowStyle;
        /// <summary>
        /// 从路径初始化快捷方式
        /// </summary>
        /// <param name="path">快捷方式的路径</param>
        public ShortcutFile(string path) : base(path)
        {
            TargetPath = Arguments = Description = WorkingDirectory = IconLocation = "";
            Hotkey = '\0';
        }
        /// <summary>
        /// 获取指示文件是否存在的值
        /// </summary>
        public override bool Exists
        {
            get
            {
                if (!Path.EndsWith(".lnk"))
                    return false;
                return System.IO.File.Exists(Path);
            }
        }
        /// <summary>
        /// 获取或设置快捷方式的目标路径
        /// </summary>
        public string TargetPath
        {
            get => targetPath;
            set { targetPath = value; onPropertyChanged(nameof(TargetPath)); }
        }
        /// <summary>
        /// 获取或设置快捷方式的运行参数
        /// </summary>
        public string Arguments
        {
            get => arguments;
            set { arguments = value; onPropertyChanged(nameof(Arguments)); }
        }
        /// <summary>
        /// 获取或设置快捷方式的描述
        /// </summary>
        public string Description
        {
            get => description;
            set { description = value; onPropertyChanged(nameof(Description)); }
        }
        /// <summary>
        /// 获取或设置工作目录
        /// </summary>
        public string WorkingDirectory
        {
            get => workingDirectory;
            set { workingDirectory = value; onPropertyChanged(nameof(WorkingDirectory)); }
        }
        /// <summary>
        /// 获取或设置快捷方式的图标位置
        /// </summary>
        public string IconLocation
        {
            get => iconLocation;
            set { iconLocation = value; onPropertyChanged(nameof(IconLocation)); }
        }
        /// <summary>
        /// 获取或设置快捷方式的启动快捷键(不需要填写Ctrl+Alt)
        /// </summary>
        public char Hotkey
        {
            get => hotkey;
            set { hotkey = value; onPropertyChanged(nameof(Hotkey)); }
        }
        /// <summary>
        /// 获取或设置快捷方式启动时的窗口状态
        /// </summary>
        public ShortcutWindowStyle WindowsStyle
        {
            get => windowStyle;
            set { windowStyle = value; onPropertyChanged(nameof(WindowsStyle)); }
        }
        /// <summary>
        /// 属性值更改时触发此事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 触发PropertyChanged事件
        /// </summary>
        /// <param name="name">更改的属性名称</param>
        private void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 将内容写回文件
        /// </summary>
        public override void Flush()
        {
            if (File.Exists(Path))
            {
                try
                {
                    File.Delete(Path);
                }
                catch (Exception ex)
                { throw new IOException("无法删除原文件。", ex); }
            }
            try
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(Path) as IWshRuntimeLibrary.IWshShortcut;
                shortcut.TargetPath = TargetPath;
                shortcut.Arguments = Arguments;
                shortcut.Description = Description;
                shortcut.WorkingDirectory = WorkingDirectory;
                shortcut.IconLocation = File.Exists(IconLocation) ? IconLocation : ",0";
                if (Hotkey != '\0')
                    shortcut.Hotkey = "CTRL+ALT+" + Hotkey.ToString();
                else
                    shortcut.Hotkey = "";
                shortcut.WindowStyle = (int)WindowsStyle;
                shortcut.Save();
            }
            catch (Exception ex)
            { throw new IOException("无法写入文件。", ex); }
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        public override void Read()
        {
            if (!File.Exists(Path)) throw new IOException("系统找不到指定的文件。");
            try
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = shell.CreateShortcut(Path) as IWshRuntimeLibrary.IWshShortcut;
                if (shortcut.Hotkey == "")
                    Hotkey = '\0';
                else
                    Hotkey = shortcut.Hotkey.Substring(shortcut.Hotkey.LastIndexOf("+") + 1)[0];
                TargetPath = shortcut.TargetPath;
                Arguments = shortcut.Arguments;
                Description = shortcut.Description;
                WorkingDirectory = shortcut.WorkingDirectory;
                IconLocation = shortcut.IconLocation;
                WindowsStyle = (ShortcutWindowStyle)shortcut.WindowStyle;
            }
            catch (Exception ex)
            {
                throw new IOException("无法读取文件。", ex);
            }            
        }
    }
    
    
}
