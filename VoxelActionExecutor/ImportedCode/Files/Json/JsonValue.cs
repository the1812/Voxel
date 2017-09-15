using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ace.Files.Json
{
    /// <summary>
    /// 表示JSON值
    /// </summary>
    public sealed class JsonValue
    {
        /// <summary>
        /// 表示JSON值的类型
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// 未知类型
            /// </summary>
            Unknown,
            /// <summary>
            /// 字符串
            /// </summary>
            String,
            /// <summary>
            /// 数字
            /// </summary>
            Number,
            /// <summary>
            /// 数组
            /// </summary>
            Array,
            /// <summary>
            /// 布尔值
            /// </summary>
            Boolean,
            /// <summary>
            /// 对象
            /// </summary>
            Object,
            /// <summary>
            /// null
            /// </summary>
            Null,
        }
        /// <summary>
        /// 获取值的类型
        /// </summary>
        public DataType Type { get; private set; }
        /// <summary>
        /// 获取此JSON值是否为null
        /// </summary>
        public bool IsNull
        {
            get
            {
                return Type == DataType.Null;
            }
        }
        /// <summary>
        /// 获取内部储存的值
        /// </summary>
        public object Value { get
            {
                switch (Type)
                {
                    case DataType.String:
                        return StringValue;
                    case DataType.Number:
                        return NumberValue;
                    case DataType.Array:
                        return ArrayValue;
                    case DataType.Object:
                        return ObjectValue;
                    case DataType.Boolean:
                        return BooleanValue;
                    case DataType.Null:
                        return null;
                    default:
                    case DataType.Unknown:
                        throw new InvalidOperationException("对象中没有储存值");
                }
            }
        }
        /// <summary>
        /// 获取储存的布尔值
        /// </summary>
        public bool? BooleanValue { get; private set; }
        /// <summary>
        /// 获取储存的字符串
        /// </summary>
        public string StringValue { get; private set; }
        /// <summary>
        /// 获取储存的数字
        /// </summary>
        public decimal? NumberValue { get; private set; }
        /// <summary>
        /// 获取储存的数组
        /// </summary>
        public JsonArray ArrayValue { get; private set; }
        /// <summary>
        /// 获取储存的对象
        /// </summary>
        public JsonObject ObjectValue { get; private set; }
        /// <summary>
        /// 初始化布尔类型的JSON值
        /// </summary>
        /// <param name="value">布尔值</param>
        public JsonValue(bool value) { BooleanValue = value;Type = DataType.Boolean; }
        /// <summary>
        /// 初始化字符串类型的JSON值
        /// </summary>
        /// <param name="value">字符串</param>
        public JsonValue(string value) { StringValue = value;Type = DataType.String; }
        /// <summary>
        /// 初始化数字类型的JSON值
        /// </summary>
        /// <param name="value">数字</param>
        public JsonValue(decimal value) { NumberValue = value;Type = DataType.Number; }
        /// <summary>
        /// 初始化数组类型的JSON值
        /// </summary>
        /// <param name="value">数组</param>
        public JsonValue(JsonArray value) { ArrayValue = value; Type = DataType.Array; }
        /// <summary>
        /// 初始化对象类型的JSON值
        /// </summary>
        /// <param name="value">对象</param>
        public JsonValue(JsonObject value) { ObjectValue = value; Type = DataType.Object; }
        /// <summary>
        /// 初始化null类型的JSON值
        /// </summary>
        public JsonValue() { Type = DataType.Null; }
        public override string ToString()
        {
            if (Type == DataType.String)
            {
                return $"\"{StringValue}\"";
            }
            else if (Type == DataType.Boolean)
            {
                return BooleanValue.ToString().ToLower();
            }
            return Value?.ToString() ?? "null";
        }
        /// <summary>
        /// 从字符串中解析JSON值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>解析结果，失败时返回null</returns>
        public static JsonValue Parse(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                throw new JsonParseException("值不能为空。若要使用空字符串，请使用双引号包括");
            }
            text = text.Trim();
            int rightQuoteIndex = text.Length - 1;
            Regex quote = new Regex("[\"]");

            if (text[0].IsMatch(quote) && text[rightQuoteIndex].IsMatch(quote))
            {
                return new JsonValue(text.Substring(1, rightQuoteIndex - 1));
            }
            if (text == "true")
            {
                return new JsonValue(true);
            }
            if (text == "false")
            {
                return new JsonValue(false);
            }
            if (text == "null")
            {
                return new JsonValue();
            }
            //Regex number = new Regex("^[0-9]*+(.[0-9]*)?$");
            if (/*number.IsMatch(text)*/decimal.TryParse(text, out decimal number))
            {
                return new JsonValue(number);
            }
            if (JsonArray.TryParse(text, out JsonArray jsonArray))
            {
                return new JsonValue(jsonArray);
            }
            if (JsonObject.TryParse(text, out JsonObject jsonObject))
            {
                return new JsonValue(jsonObject);
            }
            throw new JsonParseException($"不是有效的JSON值: {text}");
        }
        /// <summary>
        /// 尝试从字符串中解析JSON值
        /// </summary>
        /// <param name="text">字符串</param>
        /// <param name="jsonValue">接收解析结果的JsonValue</param>
        /// <returns>表示解析是否成功的值</returns>
        public static bool TryParse(string text, out JsonValue jsonValue)
        {
            try
            {
                jsonValue = Parse(text);
                return true;
            }
            catch (JsonParseException)
            {
                jsonValue = null;
                return false;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is JsonValue jsonValue)
            {
                return Type == jsonValue.Type && Value.Equals(jsonValue.Value);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator ==(JsonValue left ,JsonValue right)
        {
            if (ReferenceEquals(left,right))
            {
                return true;
            }
            if ((left is object) || (right is object))
            {
                return false;
            }
            return left.Equals(right);
        }
        public static bool operator !=(JsonValue left, JsonValue right)
        {
            return !(left == right);
        }

        public static implicit operator JsonValue(string value)=> new JsonValue(value);
        public static implicit operator JsonValue(bool value) => new JsonValue(value);
        public static implicit operator JsonValue(decimal value) => new JsonValue(value);
        public static implicit operator JsonValue(JsonArray value) => new JsonValue(value);
        public static implicit operator JsonValue(JsonObject value) => new JsonValue(value);

        public static explicit operator string(JsonValue value)=>value.StringValue;
        public static explicit operator bool?(JsonValue value) => value.BooleanValue;
        public static explicit operator decimal?(JsonValue value) => value.NumberValue;
        public static explicit operator JsonArray(JsonValue value) => value.ArrayValue;
        public static explicit operator JsonObject(JsonValue value) => value.ObjectValue;
    }
}
