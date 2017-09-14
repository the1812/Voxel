using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    /// <summary>
    /// 表示产生随机数列表的选项
    /// </summary>
    public enum RandomOption
    {
        /// <summary>
        /// 没有特殊要求
        /// </summary>
        None,
        /// <summary>
        /// 无重复值
        /// </summary>
        NoRepeat,
    }
    /// <summary>
    /// 提供对Int32值的随机数操作
    /// </summary>
    public class RandomFactory
    {
        /// <summary>
        /// 随机数生成结果的筛选器
        /// </summary>
        /// <param name="num">要筛选的数字</param>
        /// <returns>指示是否通过筛选</returns>
        public delegate bool RandomResultFilter(int num);

        private int min, max;
        /// <summary>
        /// 获取或设置随机数生成选项
        /// </summary>
        public RandomOption Option { get; set; }
        /// <summary>
        /// 获取或设置随机数的最小取值
        /// </summary>
        public int Minimum { get => min;
            set
            {
                if (value > max) throw new ArgumentOutOfRangeException("最小值不能大于最大值。");
                min = value;
            }
        }
        /// <summary>
        /// 获取或设置随机数的最大取值
        /// </summary>
        public int Maximum
        {
            get => max;
            set
            {
                if (value < min) throw new ArgumentOutOfRangeException("最大值不能小于最小值。");
                max = value;
            }
        }
        /// <summary>
        /// 初始化最小值为0的RandomFactory并指定最大值
        /// </summary>
        /// <param name="max">最大值</param>
        public RandomFactory(int max) : this(0, max) { }
        /// <summary>
        /// 初始化RandomFactory并指定范围
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        public RandomFactory(int min, int max)
        {
            this.min = min;
            Maximum = max;
        }
        /// <summary>
        /// 产生随机数列表
        /// </summary>
        /// <param name="count">列表的长度</param>
        /// <returns></returns>
        public List<int> GetList(int count)
        {
            return GetList(min, max, count, null, Option);
        }
        /// <summary>
        /// 产生随机数列表
        /// </summary>
        /// <param name="count">列表的长度</param>
        /// <param name="filter">要使用的筛选器</param>
        /// <returns></returns>
        public List<int> GetList(int count, RandomResultFilter filter)
        {
            return GetList(min, max, count, filter, Option);
        }
        /// <summary>
        /// 产生随机数列表
        /// </summary>
        /// <param name="count">列表的长度</param>
        /// <param name="option">要使用的选项</param>
        /// <returns></returns>
        public List<int> GetList(int count, RandomOption option = RandomOption.None)
        {
            return GetList(min, max, count, null, option);
        }
        /// <summary>
        /// 产生随机数列表
        /// </summary>
        /// <param name="count">列表的长度</param>
        /// <param name="filter">要使用的筛选器</param>
        /// <param name="option">要使用的选项</param>
        /// <returns></returns>
        public List<int> GetList(int count, RandomResultFilter filter, RandomOption option = RandomOption.None)
        {
            return GetList(min, max, count, filter, option);
        }
        /// <summary>
        /// 产生随机数列表
        /// </summary>
        /// <param name="min">范围的最小值</param>
        /// <param name="max">范围的最大值</param>
        /// <param name="count">列表的长度</param>
        /// <param name="option">要使用的选项</param>
        /// <returns>随机数列表</returns>
        public static List<int> GetList(int min, int max, int count, RandomOption option = RandomOption.None)
        {
            return GetList(min, max, count, null, option);
        }
        /// <summary>
        /// 产生随机数列表
        /// </summary>
        /// <param name="min">范围的最小值</param>
        /// <param name="max">范围的最大值</param>
        /// <param name="count">列表的长度</param>
        /// <param name="filter">要使用的筛选器</param>
        /// <param name="option">要使用的选项</param>
        /// <returns>随机数列表</returns>
        public static List<int> GetList(int min, int max, int count, RandomResultFilter filter, RandomOption option = RandomOption.None)
        {
            List<int> result = new List<int>(count);
            Random r = new Random();
            switch (option)
            {
                default:
                case RandomOption.None:
                    while (result.Count < count)
                    {
                        int num = r.Next(min, max);
                        if (filter?.Invoke(num) ?? true)
                            result.Add(num);
                    }
                    break;
                case RandomOption.NoRepeat:
                    //for (int i = min; i <= max; i++)
                    //{
                    //    if (r.Next() % (max - i + 1) < count && (filter?.Invoke(i) ?? true))
                    //    {
                    //        result.Add(i);
                    //        count--;
                    //    }
                    //}
                    while (result.Count < count)
                    {
                        int num = r.Next(min, max);
                        if (!result.Contains(num) && (filter?.Invoke(num) ?? true))
                            result.Add(num);
                    }
                    break;
            }
            return result ?? new List<int>();
        }
    }
}
