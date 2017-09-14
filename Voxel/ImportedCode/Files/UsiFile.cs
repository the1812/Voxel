using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Ace.Files
{
    /// <summary>
    /// 表示Usi文件内容的错误
    /// </summary>
    public class BadUsiFileException : Exception
    {
        private const string message = "无法识别的Usi文件，文件的结构不正确。";
        /// <summary>
        /// 初始化 BadUsiFileException 类的新实例
        /// </summary>
        public BadUsiFileException() : base(message) { }
        /// <summary>
        /// 初始化 BadUsiFileException 类的新实例
        /// </summary>
        /// <param name="innerException">指定内部异常</param>
        public BadUsiFileException(Exception innerException) : base(message, innerException) { }
    }
    /// <summary>
    /// 表示一个Usi文件中的选项
    /// </summary>
    public class UsiOption
    {
        /// <summary>
        /// 使用指定的名称初始化选项
        /// </summary>
        /// <param name="name">选项的名称</param>
        public UsiOption(string name)
        {
            Name = name;
            InstallCommand = UninstallCommand = "";
        }
        /// <summary>
        /// 获取或设置选项的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置选项的安装指令
        /// </summary>
        public string InstallCommand { get; set; }
        /// <summary>
        /// 获取或设置选项的卸载指令
        /// </summary>
        public string UninstallCommand { get; set; }
    }
    /// <summary>
    /// 表示一个Usi文件
    /// </summary>
    public class UsiFile : FileBase
    {
        /// <summary>
        /// 获取或设置表示选项的集合
        /// </summary>
        public List<UsiOption> Options { get; set; }
        /// <summary>
        /// 获取或设置图标的路径
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 获取或设置发布者的名称
        /// </summary>
        public string Publisher { get; set; }
        /// <summary>
        /// 获取或设置显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 获取或设置主程序的路径
        /// </summary>
        public string MainProgramPath { get; set; }
        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置安装指令
        /// </summary>
        public string InstallCommand { get; set; }
        /// <summary>
        /// 获取或设置卸载指令
        /// </summary>
        public string UninstallCommand { get; set; }
        /// <summary>
        /// 获取或设置版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 从路径初始化Usi文件
        /// </summary>
        /// <param name="path">Usi文件的路径</param>
        public UsiFile(string path) : base(path)
        {
            Options = new List<UsiOption>();
            Name = InstallCommand = UninstallCommand = Version = DisplayName = Publisher = MainProgramPath = Icon = "";
        }
        /// <summary>
        /// 将内容写回文件
        /// </summary>
        public override void Flush()
        {
            //if (File.Exists(Path))
            //{
            //    try
            //    {
            //        File.Delete(Path);
            //    }
            //    catch (Exception ex)
            //    { throw new IOException("无法删除原文件。", ex); }
            //}
            try
            {
                XElement xe = new XElement("Body");
                XElement main = new XElement("Main",
                    new XAttribute("Name", Name),
                    new XAttribute("Version", Version),
                    new XAttribute("InstallCommand", InstallCommand),
                    new XAttribute("UninstallCommand", UninstallCommand),
                    new XAttribute("DisplayName", DisplayName),
                    new XAttribute("Publisher", Publisher),
                    new XAttribute("MainProgramPath", MainProgramPath),
                    new XAttribute("Icon", Icon)
                    );
                XElement options = new XElement("Options");
                if (Options != null)
                {
                    foreach (var option in Options)
                    {
                        options.Add(
                            new XElement("Option",                            
                                new XAttribute("Name", option.Name),
                                new XAttribute("InstallCommand", option.InstallCommand),
                                new XAttribute("UninstallCommand", option.UninstallCommand)
                            )                            
                        );
                    }
                }
                xe.Add(main, options);
                xe.Save(Path);
            }
            catch (Exception ex)
            { throw new IOException("无法写入文件。", ex); }
        }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        public override void Read()
        {
            if (!File.Exists(Path)) throw new FileNotFoundException();
            try
            {                
                XElement xe = XElement.Load(Path);
                var main = xe.Element("Main");
                Name = main.Attribute("Name").Value;
                Version = main.Attribute("Version").Value;
                InstallCommand = main.Attribute("InstallCommand").Value;
                UninstallCommand = main.Attribute("UninstallCommand").Value;
                DisplayName = main.Attribute("DisplayName").Value;
                Publisher = main.Attribute("Publisher").Value;
                MainProgramPath = main.Attribute("MainProgramPath").Value;
                Icon = main.Attribute("Icon").Value;
                var options = xe.Element("Options");
                if (Options == null) Options = new List<UsiOption>();
                Options.Clear();
                foreach (var option in options.Elements())
                {
                    UsiOption uo = new UsiOption(option.Attribute("Name").Value)
                    {
                        InstallCommand = option.Attribute("InstallCommand").Value,
                        UninstallCommand = option.Attribute("UninstallCommand").Value
                    };
                    Options.Add(uo);
                }
            }
            catch (Exception ex)
            {
                throw new BadUsiFileException(ex);
                //Clipboard.SetText(ex.Message);
            }

        }
    }    
}
