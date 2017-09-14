using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    /// <summary>
    /// 表示数据大小的单位
    /// </summary>
    public enum DataSizeUnit : long
    {
        /// <summary>
        /// 位
        /// </summary>
        Bit = 1L,
        /// <summary>
        /// 字节
        /// </summary>
        Byte = 8L,
        /// <summary>
        /// 千字节
        /// </summary>
        KB = 1024L * 8,
        /// <summary>
        /// 兆字节
        /// </summary>
        MB = 1024L * 1024 * 8,
        /// <summary>
        /// 千兆字节/吉字节
        /// </summary>
        GB = 1024L * 1024 * 1024 * 8,
        /// <summary>
        /// 万亿字节/太字节
        /// </summary>
        TB = 1024L * 1024 * 1024 * 1024 * 8,
        /// <summary>
        /// 千万亿字节/拍字节
        /// </summary>
        PB = 1024L * 1024 * 1024 * 1024 * 1024 * 8,
    }
    /// <summary>
    /// 表示数据大小
    /// </summary>
    public class DataSize
    {
        private long size;
        /// <summary>
        /// 初始化DataSize的新实例
        /// </summary>
        /// <param name="size">数据大小</param>
        public DataSize(long size)
        {
            if (size < 0) throw new ArgumentOutOfRangeException("数据大小必须大于等于0。");
            this.size = size;
        }
        /// <summary>
        /// 初始化DataSize的新实例
        /// </summary>
        /// <param name="size">数据大小</param>
        public DataSize(int size) : this((long)size) { }
        /// <summary>
        /// 获取或设置度量单位，这将影响ToString()的结果
        /// </summary>
        public DataSizeUnit Unit { get; set; }
        /// <summary>
        /// 获取Int64形式的数据大小
        /// </summary>
        /// <returns>数据大小</returns>
        public long ToInt64() => size;
        /// <summary>
        /// 获取Int32形式的数据大小，这可能会超出Int32的表示范围
        /// </summary>
        /// <returns>数据大小</returns>
        public int ToInt32() => size > int.MaxValue ? throw new OverflowException() : (int)size;
        /// <summary>
        /// 获取数据的位大小
        /// </summary>
        private long BitSize => size * 8;
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return size == ((obj as DataSize)?.size ?? -1);
        }
        public override string ToString()
        {
            return $"{BitSize / (long)Unit}{Unit.ToString()}";
        }
    }
}
