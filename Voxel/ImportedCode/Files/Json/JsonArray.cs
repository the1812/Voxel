using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Files.Json
{
    /// <summary>
    /// 表示JSON数组
    /// </summary>
    public sealed class JsonArray : IEnumerable<JsonValue>, ICollection<JsonValue>
    {
        private List<JsonValue> items = new List<JsonValue>();
        /// <summary>
        /// 初始化JSON数组
        /// </summary>
        /// <param name="values">数组的元素</param>
        public JsonArray(params JsonValue[] values)
        {
            if (values != null)
            {
                items.AddRange(values);
            }
        }
        /// <summary>
        /// 初始化JSON数组
        /// </summary>
        /// <param name="values">可枚举的元素</param>
        public JsonArray(IEnumerable<JsonValue> values)
        {
            if (values != null)
            {
                items.AddRange(values);
            }
        }
        /// <summary>
        /// 获取或设置指定索引处的值
        /// </summary>
        /// <param name="index">指定的索引</param>
        /// <returns>对应的JSON值</returns>
        public JsonValue this[int index]
        {
            get => items[index];
            set
            {
                if (index < 0 || index >= items.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                items[index] = value;
            }
        }
        /// <summary>
        /// 数组的元素数量
        /// </summary>
        public int Count => items.Count;
        /// <summary>
        /// 获取只读性，总是为false
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// 添加新的值
        /// </summary>
        /// <param name="item">要添加的值</param>
        public void Add(JsonValue item)
        {
            items.Add(item);
        }
        /// <summary>
        /// 尝试将字符串解析为JSON值并添加
        /// </summary>
        /// <param name="valueString">要解析的字符串</param>
        public void Add(string valueString)
        {
            if (JsonValue.TryParse(valueString, out JsonValue jsonValue))
            {
                Add(jsonValue);
            }
            else
            {
                throw new FormatException("无法将字符串解析为JSON值");
            }
        }
        /// <summary>
        /// 清空数组
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }
        /// <summary>
        /// 判断数组是否包含指定值
        /// </summary>
        /// <param name="item">指定的值</param>
        /// <returns>是否包含指定值</returns>
        public bool Contains(JsonValue item)
        {
            return items.Contains(item);
        }
        /// <summary>
        /// 将内容复制到指定的数组
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="arrayIndex">数组的起始索引</param>
        public void CopyTo(JsonValue[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 获取对值的枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        public IEnumerator<JsonValue> GetEnumerator()
        {
            foreach (var value in items)
            {
                yield return value;
            }
        }
        /// <summary>
        /// 删除指定的值
        /// </summary>
        /// <param name="item">指定的值</param>
        /// <returns>删除是否成功的值</returns>
        public bool Remove(JsonValue item)
        {
            return items.Remove(item);
        }
        /// <summary>
        /// 获取对值的枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public override string ToString()
        {
            string result = "[" + Environment.NewLine;
            items.ForEachIndex((index) =>
            {
                var value = items[index];
                if (value.Type == JsonValue.DataType.String)
                {
                    //string data = value.StringValue.ForEachLine(line =>
                    //{
                    //    return "\t" + line;
                    //});
                    result += $"\t{value}";
                }
                else
                {
                    result += value.ToString().ForEachLine(line =>
                    {
                        return "\t" + line;
                    });
                }
                if (index != items.Count - 1)
                {
                    result += ", " + Environment.NewLine;
                }
            });
            return result + Environment.NewLine + "]";
        }
        /// <summary>
        /// 从字符串解析JSON数组
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>解析结果，失败时返回null</returns>
        public static JsonArray Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            text = text.Trim();
            int rightBraceIndex = text.Length - 1;
            if (text[0] != '[' || text[rightBraceIndex] != ']')
            {
                throw new JsonParseException("JSON数组必须由[]包含");
            }
            text = text.Substring(1, rightBraceIndex - 1).Trim();
            if (text == "")
            {
                return new JsonArray();
            }
            int depth = 0;
            List<int> commaIndexes = new List<int>();
            for (int index = 0; index < text.Length; index++)
            {
                char character = text[index];
                if (character == '{' || character == '[')
                {
                    depth++;
                }
                else if (character == '}' || character == ']')
                {
                    depth--;
                }
                else if (depth == 0 && character == ',')
                {
                    commaIndexes.Add(index);
                }
            }
            var elements = text.SplitByIndexes(commaIndexes);
            JsonArray jsonArray = new JsonArray();
            foreach (var element in elements)
            {
                try
                {
                    jsonArray.Add(JsonValue.Parse(element));
                }
                catch (JsonParseException ex)
                {
                    throw new JsonParseException($"不是有效的JSON值: {element}", ex);
                }
            }
            return jsonArray;
        }
        /// <summary>
        /// 尝试从字符串解析JSON数组
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="jsonArray">接收解析结果的JSON数组</param>
        /// <returns>解析是否成功的值</returns>
        public static bool TryParse(string text, out JsonArray jsonArray)
        {
            try
            {
                jsonArray = Parse(text);
                return true;
            }
            catch (JsonParseException)
            {
                jsonArray = null;
                return false;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is JsonArray jsonArray)
            {
                foreach (var jsonValue in items)
                {
                    if (!jsonArray.Contains(jsonValue))
                    {
                        return false;
                    }
                }
                return items.Count == jsonArray.Count;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(JsonArray left, JsonArray right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            if ((left is object) || (right is object))
            {
                return false;
            }
            return left.Equals(right);
        }
        public static bool operator !=(JsonArray left, JsonArray right)
        {
            return !(left == right);
        }
        /// <summary>
        /// 选出所有的字符串子元素并创建列表
        /// </summary>
        /// <returns>列表</returns>
        public List<string> ToStringList()
        {
            var result = from item in items
                         where item.Type == JsonValue.DataType.String
                         select item.StringValue;
            return new List<string>(result);
        }
        /// <summary>
        /// 选出所有的数字子元素并创建列表
        /// </summary>
        /// <returns>列表</returns>
        public List<decimal> ToNumberList()
        {
            var result = from item in items
                         where item.Type == JsonValue.DataType.Number
                         select item.NumberValue.Value;
            return new List<decimal>(result);
        }
        /// <summary>
        /// 选出所有的布尔子元素并创建列表
        /// </summary>
        /// <returns>列表</returns>
        public List<bool> ToBooleanList()
        {
            var result = from item in items
                         where item.Type == JsonValue.DataType.Boolean
                         select item.BooleanValue.Value;
            return new List<bool>(result);
        }
        /// <summary>
        /// 选出所有的JSON数组子元素并创建列表
        /// </summary>
        /// <returns>列表</returns>
        public List<JsonArray> ToArrayList()
        {
            var result = from item in items
                         where item.Type == JsonValue.DataType.Array
                         select item.ArrayValue;
            return new List<JsonArray>(result);
        }
        /// <summary>
        /// 选出所有的JSON对象子元素并创建列表
        /// </summary>
        /// <returns>列表</returns>
        public List<JsonObject> ToObjectList()
        {
            var result = from item in items
                         where item.Type == JsonValue.DataType.Object
                         select item.ObjectValue;
            return new List<JsonObject>(result);
        }
    }
}
