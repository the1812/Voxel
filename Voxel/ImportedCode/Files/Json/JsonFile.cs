using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ace.Files.Json
{    
    /// <summary>
    /// 表示一个JSON文件
    /// </summary>
    public sealed class JsonFile : FileBase
    {
        /// <summary>
        /// 从路径初始化JSON文件
        /// </summary>
        /// <param name="path">路径</param>
        public JsonFile(string path) : base(path) { }
        /// <summary>
        /// 获取或设置表示该JSON文件内容的JsonObject
        /// </summary>
        public JsonObject Content { get; set; }
        public override void Flush()
        {
            var textFile = new TextFile(Path)
            {
                Text = Content.ToString()
            };
            textFile.Flush();
        }
        public override void Read()
        {
            var textFile = new TextFile(Path);
            textFile.Read();
            string text = textFile.Text;
            
            Content = JsonObject.Parse(text);
        }
        public override string ToString()
        {
            return Content?.ToString() ?? "<Empty>";
        }
    }
}
