using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    /// <summary>
    /// 随机数生成器扩展
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// 返回指定范围内的随机浮点数
        /// </summary>
        /// <param name="random">使用的随机数生成器</param>
        /// <param name="min">范围的最小值</param>
        /// <param name="max">范围的最大值</param>
        /// <returns>随机浮点数</returns>
        public static double NextDouble(this Random random, double min, double max)
        {
            if (random == null) throw new NullReferenceException();
            if (min >= max) throw new ArgumentException("输入范围的最大值必须大于最小值");
            return random.NextDouble() * (max - min) + min;
        }
    }
}
