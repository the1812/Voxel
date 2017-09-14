using System;
using System.Collections.Generic;

namespace Ace
{
    /// <summary>
    /// 用于集合的扩展类
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 互换字典中的键值对，并返回互换后的字典。相同的键将被忽略
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="dict">原字典</param>
        /// <returns>互换后的字典</returns>
        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            Dictionary<TValue, TKey> newDict = new Dictionary<TValue, TKey>();
            foreach (var item in dict)
                if (!newDict.ContainsKey(item.Value))
                    newDict.Add(item.Value, item.Key);
            return newDict;
        }
        /// <summary>
        /// 将两个列表组合为字典，并返回组合成的字典。相同的键将被忽略，列表长度不一样将返回null
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="listKey">键的列表</param>
        /// <param name="listValue">值的列表</param>
        /// <returns>组合成的字典</returns>
        public static Dictionary<TKey, TValue> MakeDictionary<TKey, TValue>(this List<TKey> listKey, List<TValue> listValue)
        {
            if (listKey.Count != listValue.Count)
                return null;
            Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();
            foreach (var item in listKey)
                if (!newDict.ContainsKey(item))
                    newDict.Add(item, listValue[listKey.IndexOf(item)]);
            return newDict;
        }
        /// <summary>
        /// 组合两个字典中所有的键值对，并返回组合成的新字典。相同的键将被忽略
        /// </summary>
        /// <typeparam name="TKey">键的类型</typeparam>
        /// <typeparam name="TValue">值的类型</typeparam>
        /// <param name="dict">要组合的字典</param>
        /// <param name="another">要组合的另一个字典</param>
        /// <returns>组合成的新字典</returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> another)
        {
            if (dict == null || another == null)
                throw new NullReferenceException();
            Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();
            foreach (var item in dict)
                newDict.Add(item.Key, item.Value);
            foreach (var item in another)
                newDict.Add(item.Key, item.Value);
            return newDict;
        }
        /// <summary>
        /// 对集合中所有元素执行指定操作
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="action">指定的操作</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
        /// <summary>
        /// 判断集合中的元素是否全部满足指定条件
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="condition">指定的条件</param>
        /// <returns>判断结果</returns>
        public static bool All<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            foreach (var item in collection)
            {
                if (!condition(item)) return false;
            }
            return true;
        }
        /// <summary>
        /// 判断集合中是否存在元素满足指定条件
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="condition">指定的条件</param>
        /// <returns>判断结果</returns>
        public static bool Any<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            foreach (var item in collection)
            {
                if (condition(item)) return true;
            }
            return false;
        }
        /// <summary>
        /// 遍历集合中的索引并执行指定操作
        /// </summary>
        /// <typeparam name="T">集合的类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="action">指定的操作</param>
        public static void ForEachIndex<T>(this ICollection<T> collection, Action<int> action)
        {
            for (int index = 0; index < collection.Count; index++)
            {
                action(index);
            }
        }
    }
}
