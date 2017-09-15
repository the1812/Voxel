using System;
using System.IO;

namespace Ace.Files
{
    /// <summary>
    /// 表示一个纯文本文件
    /// </summary>
    public class TextFile : FileBase
    {
        /// <summary>
        /// 从路径初始化文本文件
        /// </summary>
        /// <param name="path">文本文件的路径</param>
        public TextFile(string path) : base(path) { }
        /// <summary>
        /// 获取或设置文件的文本内容
        /// </summary>
        public string Text { get; set; } = "";
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
                using (var writer = File.CreateText(Path))
                {
                    writer.Write(Text);
                    writer.Flush();
                }
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
                using (var reader = File.OpenText(Path))
                {
                    Text = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new IOException("无法读取文件。", ex);
            }
        }
    }
    
    
}
