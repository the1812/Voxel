using System.IO;

namespace Ace.Files
{
    /// <summary>
    /// Ace.Files中所有文件类的基类
    /// </summary>
    public abstract class FileBase
    {
        public FileBase(string path)
        {
            Path = path;
        }
        /// <summary>
        /// 获取指示文件是否存在的值
        /// </summary>
        public virtual bool Exists { get { return File.Exists(Path); } }
        /// <summary>
        /// 获取文件的路径
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// 读取文件内容
        /// </summary>
        public abstract void Read();
        /// <summary>
        /// 将内容写回文件
        /// </summary>
        public abstract void Flush();
        /// <summary>
        /// 删除当前文件
        /// </summary>
        public virtual void Delete()
        {
            File.Delete(Path);
        }
    }
}
