using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Files.Json
{
    /// <summary>
    /// 表示JSON解析过程中发生的异常
    /// </summary>
    public sealed class JsonParseException : Exception
    {
        /// <summary>
        /// 初始化JsonParseException的新实例
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        public JsonParseException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
