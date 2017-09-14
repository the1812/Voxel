using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Files.Json
{
    /// <summary>
    /// 表示JSON对象
    /// </summary>
    public class JsonObject : IEnumerable<JsonProperty>, ICollection<JsonProperty>
    {

        private List<JsonProperty> properties = new List<JsonProperty>();
        /// <summary>
        /// 创建空白内容的JSON对象
        /// </summary>
        public JsonObject() { }
        /// <summary>
        /// 获取属性的数量
        /// </summary>
        public int Count => properties.Count;
        /// <summary>
        /// 获取只读性，总是为false
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// 获取或设置JSON对象中的属性值
        /// </summary>
        /// <param name="key">属性的名称</param>
        /// <returns>属性值</returns>
        [IndexerName("Properties")]
        public JsonValue this[string key]
        {
            get
            {
                foreach (var property in properties)
                {
                    if (property.Name == key)
                    {
                        return property.Value;
                    }
                }
                throw new KeyNotFoundException($"找不到拥有此名称的属性: {key}");
            }
            set
            {
                foreach (var property in properties)
                {
                    if (property.Name == key)
                    {
                        property.Value = value;
                        return;
                    }
                }
                //throw new KeyNotFoundException($"找不到拥有此名称的属性: {key}");
                properties.Add(new JsonProperty(key, value));
            }
        }
        /// <summary>
        /// 从字符串中解析JSON对象
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>解析结果，失败时返回null</returns>
        public static JsonObject Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            text = text.Trim();
            int rightBraceIndex = text.Length - 1;
            if (text[rightBraceIndex] != '}' || text[0] != '{')
            {
                throw new JsonParseException("JSON对象必须由{}包含");
            }
            text = text.Substring(1, rightBraceIndex - 1).Trim();
            int depth = 0;
            List<int> commaIndexes = new List<int>();
            for (int index = 0; index < text.Length; index++)
            {
                char character = text[index];
                if (character == '[' || character == '{')
                {
                    depth++;
                }
                else if (character == ']' || character == '}')
                {
                    depth--;
                }
                else if (depth == 0 && character == ',')
                {
                    commaIndexes.Add(index);
                }
            }
            var properties = text.SplitByIndexes(commaIndexes);
            JsonObject jsonObject = new JsonObject();
            foreach (var propertyText in properties)
            {
                try
                {
                    jsonObject.Add(JsonProperty.Parse(propertyText));
                }
                catch (JsonParseException ex)
                {
                    throw new JsonParseException($"不是有效的JSON属性: {propertyText}", ex);
                }
            }
            return jsonObject;
        }
        /// <summary>
        /// 尝试从字符串中解析JSON对象
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="jsonObject">接收解析结果的JsonObject</param>
        /// <returns>表示解析是否成功的值</returns>
        public static bool TryParse(string text, out JsonObject jsonObject)
        {
            try
            {
                jsonObject = Parse(text);
                return true;
            }
            catch (JsonParseException)
            {
                jsonObject = null;
                return false;
            }
        }
        /// <summary>
        /// 获取对象中属性的枚举器
        /// </summary>
        /// <returns>对象中属性的枚举器</returns>
        public IEnumerator<JsonProperty> GetEnumerator()
        {
            foreach (var property in properties)
            {
                yield return property;
            }
        }
        /// <summary>
        /// 获取对象中属性的枚举器
        /// </summary>
        /// <returns>对象中属性的枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public override bool Equals(object obj)
        {
            if (obj is JsonObject jsonObject)
            {
                foreach (var property in properties)
                {
                    if (!jsonObject.Contains(property))
                    {
                        return false;
                    }
                }
                return properties.Count == jsonObject.Count;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            string result = "{" + Environment.NewLine;
            properties?.ForEachIndex((index) =>
            {
                var property = properties[index];
                result += property.ToString().ForEachLine(line =>
                {
                    return "\t" + line;
                });
                if (index != properties.Count - 1)
                {
                    result += ", " + Environment.NewLine;
                }
            });
            return result + Environment.NewLine + "}";
        }
        /// <summary>
        /// 添加一项属性
        /// </summary>
        /// <param name="item">要添加的属性</param>
        public void Add(JsonProperty item)
        {
            if (ContainsKey(item.Name))
            {
                throw new InvalidOperationException("不允许添加拥有相同属性名的属性");
            }
            properties.Add(item);
        }
        /// <summary>
        /// 解析字符串为属性并添加
        /// </summary>
        /// <param name="propertyString">字符串</param>
        public void Add(string propertyString)
        {
            if (JsonProperty.TryParse(propertyString, out JsonProperty jsonProperty))
            {
                Add(jsonProperty);
            }
            else
            {
                throw new FormatException("无法将字符串解析为JSON属性");
            }
        }
        /// <summary>
        /// 清空对象所有的属性
        /// </summary>
        public void Clear()
        {
            properties.Clear();
        }
        /// <summary>
        /// 获取对象是否为空的值
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return properties.Count == 0;
            }
        }
        /// <summary>
        /// 获取对象是否包含指定的属性
        /// </summary>
        /// <param name="item">指定的属性</param>
        /// <returns>是否包含的值</returns>
        public bool Contains(JsonProperty item)
        {
            return properties.Contains(item);
        }
        /// <summary>
        /// 获取对象是否包含指定的属性名
        /// </summary>
        /// <param name="key">属性名</param>
        /// <returns>是否包含的值</returns>
        public bool ContainsKey(string key)
        {
            var result = from property in properties
                         where property.Name == key
                         select property;
            return result.Count() > 0;
        }
        /// <summary>
        /// 将属性复制到数组
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="arrayIndex">数组的起始索引</param>
        public void CopyTo(JsonProperty[] array, int arrayIndex)
        {
            properties.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 删除指定的属性
        /// </summary>
        /// <param name="item">指定的属性</param>
        /// <returns>删除是否成功的值</returns>
        public bool Remove(JsonProperty item)
        {
            return properties.Remove(item);
        }
        /// <summary>
        /// 根据属性名删除指定的属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <returns>删除是否成功的值</returns>
        public bool Remove(string key)
        {
            for (int index = 0; index < properties.Count; index++)
            {
                if (properties[index].Name == key)
                {
                    properties.Remove(properties[index]);
                    return true;
                }
            }
            return false;
        }
        public static bool operator ==(JsonObject left, JsonObject right)
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
        public static bool operator !=(JsonObject left, JsonObject right)
        {
            return !(left == right);
        }
    }
}
