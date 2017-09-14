using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ace.Files
{
    /// <summary>
    /// 表示一个XML文件
    /// </summary>
    public sealed class XmlFile : FileBase
    {
        XElement content;
        /// <summary>
        /// 从路径初始化XML文件
        /// </summary>
        /// <param name="path">路径</param>
        public XmlFile(string path) : base(path)
        {
           
        }
        /// <summary>
        /// 将内容写回文件
        /// </summary>
        public override void Flush()
        {
            content.Save(Path);
        }        
        /// <summary>
        /// 读取文件内容
        /// </summary>
        public override void Read()
        {
            content = XElement.Load(Path);
        }
        /// <summary>
        /// 获取XML文件的内容
        /// </summary>
        public XElement Content => content;
    }
}
