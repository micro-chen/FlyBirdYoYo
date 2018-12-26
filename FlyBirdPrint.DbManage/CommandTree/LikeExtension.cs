using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Linq.Expressions
{
    /// <summary>
    /// 辅助  转化表达式树（仅仅辅助ResolveLambdaTreeToCondition作用无其他作用）
    /// </summary>
    public static class LikeExtension
    {
        public static bool In<T>(this T obj, T[] array) where T : class, new()
        {
            return true;
        }
        public static bool NotIn<T>(this T obj, T[] array) where T : class, new()
        {
            return true;
        }
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }

    }
}
