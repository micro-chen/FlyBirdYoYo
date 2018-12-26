using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 辅助  转化表达式树（仅仅辅助ResolveLambdaTreeToCondition作用无其他作用）
    /// sql  字符串 长度函数扩展
    /// </summary>
    public static class InnernalFunctionExtension
    {

        /// <summary>
        /// 执行sql 中内置的len 函数 比较字段长度
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int LenFuncInSql(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return 0;
            }
            return obj.Length;
        }
        

    }
}
