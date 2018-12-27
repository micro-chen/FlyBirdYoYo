using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.DbManage
{
    /// <summary>
    /// 数据库字段信息
    /// </summary>
    public class DbField
    {
        /// <summary>
        /// 属性CLR名称
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 对应的列名称
        /// </summary>
        public string FieldColumnName { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Lenth { get; set; }
    }
}
