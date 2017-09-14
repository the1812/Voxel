using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Files.Json
{
    /// <summary>
    /// 表示JSON属性
    /// </summary>
    public class JsonProperty
    {
        /// <summary>
        /// 初始化JSON属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">属性值</param>
        public JsonProperty(string key, JsonValue value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("属性名不能为空白或null");
            }
            Name = key;
            Value = value;
        }
        /// <summary>
        /// 初始化JSON属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">字符串属性值</param>
        public JsonProperty(string key, string value) : this(key, new JsonValue(value)) { }
        /// <summary>
        /// 初始化JSON属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">布尔属性值</param>
        public JsonProperty(string key, bool value) : this(key, new JsonValue(value)) { }
        /// <summary>
        /// 初始化JSON属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">数字属性值</param>
        public JsonProperty(string key, decimal value) : this(key, new JsonValue(value)) { }
        /// <summary>
        /// 初始化JSON属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">JSON数组属性值</param>
        public JsonProperty(string key, JsonArray value) : this(key, new JsonValue(value)) { }
        /// <summary>
        /// 初始化JSON属性
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">JSON对象属性值</param>
        public JsonProperty(string key, JsonObject value) : this(key, new JsonValue(value)) { }
        /// <summary>
        /// 获取属性名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 获取或设置属性值
        /// </summary>
        public JsonValue Value { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is JsonProperty jsonProperty)
            {
                return Name == jsonProperty.Name && Value == jsonProperty.Value;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"\"{Name}\" : {Value}";
        }
        /// <summary>
        /// 将字符串解析为JSON属性
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>解析结果，失败时返回null</returns>
        public static JsonProperty Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            text = text.Trim();
            string quote = "[\"]";
            int secondQuoteIndex = text.Match(quote).NextMatch().Index;
            if (secondQuoteIndex == -1 || !text[0].IsMatch(quote))
            {
                throw new JsonParseException($"无法从中解析属性名: {text}");
            }
            string name = text.Substring(1, secondQuoteIndex - 1);
            text = text.Remove(0, name.Length + 2).Trim();
            if (string.IsNullOrWhiteSpace(text)) return null;
            if (text[0] != ':')
            {
                throw new JsonParseException("属性名和属性值之间必须用:分隔");
            }
            text = text.Remove(0, 1).Trim();
            if (JsonValue.TryParse(text, out JsonValue value))
            {
                return new JsonProperty(name, value);
            }
            else
            {
                throw new JsonParseException($"不是有效的JSON值: {text}");
            }
        }
        /// <summary>
        /// 尝试将字符串解析为JSON属性
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="jsonProperty">接收解析结果的JSON属性</param>
        /// <returns>解析是否成功的值</returns>
        public static bool TryParse(string text, out JsonProperty jsonProperty)
        {
            try
            {
                jsonProperty = Parse(text);
                return true;
            }
            catch (JsonParseException)
            {
                jsonProperty = null;
                return false;
            }
        }
        public static bool operator ==(JsonProperty left, JsonProperty right)
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
        public static bool operator !=(JsonProperty left, JsonProperty right)
        {
            return !(left == right);
        }
    }
}
