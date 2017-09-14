using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace Ace
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 获取字符串表示的Decimal值
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <returns>Decimal值</returns>
        public static decimal ToDecimal(this string str)
        {
            return decimal.TryParse(str, out decimal result) ? result : 0;
        }
        /// <summary>
        /// 获取字符串表示的Int32值
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <returns>Int32值</returns>
        public static int ToInt32(this string str)
        {
            return int.TryParse(str, out int result) ? result : 0;
        }
        /// <summary>
        /// 获取字符串表示的Int64值
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <returns>Int64值</returns>
        public static long ToInt64(this string str)
        {
            return long.TryParse(str, out long result) ? result : 0;
        }
        /// <summary>
        /// 获取字符串表示的Double值
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <returns>Double值</returns>
        public static double ToDouble(this string str)
        {
            return double.TryParse(str, out double result) ? result : 0;
        }
        /// <summary>
        /// 获取字符串表示的Single值
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <returns>Single值</returns>
        public static float ToSingle(this string str)
        {
            return float.TryParse(str, out float result) ? result : 0;
        }
        /// <summary>
        /// 获取字符串表示的Byte值
        /// </summary>
        /// <param name="str">要解析的字符串</param>
        /// <returns>Byte值</returns>
        public static byte ToByte(this string str)
        {
            return byte.TryParse(str, out byte result) ? result : (byte)0;
        }
        /// <summary>
        /// 检测字符串是否匹配一个正则表达式
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>匹配的结果</returns>
        public static bool IsMatch(this string str, Regex regex)
        {
            return regex.IsMatch(str);
        }
        /// <summary>
        /// 检测字符串是否匹配一个正则表达式
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <param name="regex">表示正则表达式的字符串</param>
        /// <returns>匹配的结果</returns>
        public static bool IsMatch(this string str, string regex)
        {
            return str.IsMatch(new Regex(regex));
        }
        /// <summary>
        /// 检测字符是否匹配一个正则表达式
        /// </summary>
        /// <param name="character">要检测的字符</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>匹配的结果</returns>
        public static bool IsMatch(this char character, Regex regex)
        {
            return character.ToString().IsMatch(regex);
        }
        /// <summary>
        /// 检测字符是否匹配一个正则表达式
        /// </summary>
        /// <param name="character">要检测的字符</param>
        /// <param name="regex">表示正则表达式的字符串</param>
        /// <returns>匹配的结果</returns>
        public static bool IsMatch(this char character, string regex)
        {
            return character.IsMatch(new Regex(regex));
        }
        /// <summary>
        /// 返回字符串在正则表达式中的匹配项
        /// </summary>
        /// <param name="str"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static Match Match(this string str, Regex regex)
        {
            return regex.Match(str);
        }
        /// <summary>
        /// 返回字符串在正则表达式中的匹配项
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <param name="regex">表示正则表达式的字符串</param>
        /// <returns>匹配项</returns>
        public static Match Match(this string str, string regex)
        {
            return str.Match(new Regex(regex));
        }
        /// <summary>
        /// 返回字符串在一个正则表达式中所有匹配项
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <param name="regex">表示正则表达式的字符串</param>
        /// <returns></returns>
        public static List<string> Matches(this string str, string regex)
        {
            var matches = (new Regex(regex)).Matches(str);
            List<string> result = new List<string>();
            foreach (var item in matches)
            {
                if (item is Match match)
                {
                    result.Add(match.Value);
                }
            }
            return result;
        }
        /// <summary>
        /// 返回字符串根据指定索引列表分割成的字符串列表
        /// </summary>
        /// <param name="text">要分割的字符串</param>
        /// <param name="indexes">指定的索引列表</param>
        /// <returns>字符串列表</returns>
        public static List<string> SplitByIndexes(this string text, List<int> indexes)
        {
            List<string> result = new List<string>();
            int removedCount = 0;
            foreach (var index in indexes)
            {
                result.Add(text.Substring(0, index - removedCount));
                text = text.Remove(0, index - removedCount + 1);
                removedCount += index - removedCount + 1;
            }
            result.Add(text);
            return result;
        }
        /// <summary>
        /// 将字符串按照指定的子串分割为列表
        /// </summary>
        /// <param name="text">要分割的字符串</param>
        /// <param name="spliter">指定的子串</param>
        /// <returns>字符串列表</returns>
        public static List<string> Split(this string text, string spliter)
        {
            int splitIndex = text.IndexOf(spliter);
            List<string> result = new List<string>();
            while (splitIndex != -1)
            {
                result.Add(text.Substring(0, splitIndex));
                text = text.Remove(0, splitIndex + spliter.Length);
                splitIndex = text.IndexOf(spliter);                
            }
            result.Add(text);
            return result;
        }
        /// <summary>
        /// <para>对字符串中的每一行执行指定的操作，并返回新的字符串</para>
        /// <para>不指定换行符或换行符为null时将使用Environment.NewLine作为换行符</para>
        /// </summary>
        /// <param name="text">原字符串</param>
        /// <param name="func">要执行的操作，返回操作后的字符串</param>
        /// <param name="lineBreak">指定的换行符</param>
        /// <returns>操作执行后的新字符串</returns>
        public static string ForEachLine(this string text, Func<string, string> func, string lineBreak = null)
        {
            if (lineBreak == null)
            {
                lineBreak = Environment.NewLine;
            }
            string result = "";
            var lines = text.Split(lineBreak);
            foreach (var line in lines)
            {
                result += func(line) + lineBreak;
            }
            //删除最后多出来的换行符
            result = result.Remove(result.Length - lineBreak.Length);
            return result;
        }
    }
}
