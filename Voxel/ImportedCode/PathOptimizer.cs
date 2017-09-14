namespace Ace
{
    /// <summary>
    /// 提供一组调整包含路径的字符串的方法
    /// </summary>
    public static class PathOptimizer
    {
        private const string quotes = @"""", backslash = @"\";
        /// <summary>
        /// 确保返回的字符串不用双引号包括
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string NoQuotes(this string str)
        {
            if (str.StartsWith(quotes))
            {
                int index = -1; string indexer = null;
                if (str.LastIndexOf(".") < (index = str.LastIndexOf(",")))
                {
                    indexer = str.Substring(index);
                    str = str.Remove(index).NoQuotes();
                }
                return index == -1 ? str.Substring(1, str.Length - 2) : str + indexer;
            }
            return str.StartsWith(quotes) ? str.Substring(1, str.Length - 2) : str;
        }
        /// <summary>
        /// 确保返回的字符串用双引号包括
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string Quotes(this string str)
        {
            if (!str.StartsWith(quotes))
            {
                int index = -1; string indexer = null;
                if (str.LastIndexOf(".") < (index = str.LastIndexOf(",")))
                {
                    indexer = str.Substring(index);
                    str = str.Remove(index);
                }
                return quotes + str + quotes + indexer ?? "";
            }
            return str;
        }
        /// <summary>
        /// 确保返回的字符串以反斜杠结尾
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string Backslash(this string str)
        {
            return str.EndsWith(backslash) ? str : str + backslash;
        }
        /// <summary>
        /// 确保返回的字符串不以反斜杠结尾
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string NoBackslash(this string str)
        {
            return str.EndsWith(backslash) ? str.Substring(0, str.Length - 1) : str;
        }
        /// <summary>
        /// 移除文件名
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string RemoveFileName(this string str)
        {
            int inedx;
            str = str.NoQuotes();
            inedx = str.LastIndexOf(backslash);
            return inedx > -1 ? str.Remove(inedx + 1) : str;
        }
        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string GetFileName(this string str)
        {
            int index;
            str = str.NoQuotes();
            index = str.LastIndexOf(backslash);
            return index > -1 ? str.Substring(index + 1) : str;
        }
        /// <summary>
        /// 移除扩展名
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string RemoveExtension(this string str)
        {
            str = str.NoQuotes();
            int index = str.LastIndexOf(".");
            if (index == -1) return str;
            return str.Remove(index);
        }
        /// <summary>
        /// 获取扩展名
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string GetExtension(this string str)
        {
            str = str.NoQuotes();
            int index = str.LastIndexOf(".");
            if (index == -1) return str;
            return str.Substring(index);
        }
        /// <summary>
        /// 获取文件夹名
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string GetFolderName(this string str)
        {
            str = str.NoQuotes().NoBackslash();
            int index;
            index = str.LastIndexOf(backslash);
            return index > -1 ? str.Substring(index + 1) : str;
        }
        /// <summary>
        /// 获取父文件夹路径
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns>调整后的字符串</returns>
        public static string GetParentFolder(this string str)
        {
            str = str.NoQuotes().NoBackslash().RemoveFileName().NoBackslash();
            int index;
            index = str.LastIndexOf(backslash);
            return index > -1 ? str.Substring(index + 1) : str;
        }
    }    
}
