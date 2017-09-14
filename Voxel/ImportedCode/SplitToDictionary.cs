using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public static partial class StringExtensions
    {
        /// <summary>
        /// 解析字符串中包含的键值对
        /// <para>例如: var dict = "a:apple,b:boy,c:circle".SplitToDictionary(",", ":");</para>
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <param name="itemSeparator">键值对分割字符串</param>
        /// <param name="keyValueSeparator">键值分割字符串</param>
        /// <param name="replaceIfExsit">指定出现重复键时是否覆盖,不覆盖则抛出异常</param>
        /// <returns>分割得到的字典</returns>
        public static Dictionary<string, string> SplitToDictionary(this string str, string itemSeparator, string keyValueSeparator, bool replaceIfExsit = false)
        {
            var array = str.Split(itemSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var result = new Dictionary<string, string>();
            foreach (var keyValuePair in array)
            {
                int index = keyValuePair.IndexOf(keyValueSeparator);
                if (index > -1)
                {
                    string key = keyValuePair.Substring(0, index);
                    string value = keyValuePair.Remove(0, index + 1);
                    if (result.ContainsKey(key))
                    {
                        if (replaceIfExsit)
                        {
                            result[key] = value;
                        }
                        else
                            throw new InvalidOperationException("出现了重复的键。");
                    }
                    else
                        result.Add(key, value);
                }
                else
                {
                    result.Add(keyValuePair, null);
                }
            }
            return result;
        }

    }
}
