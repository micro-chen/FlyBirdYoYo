using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 分组属性
    /// </summary>
    public class GroupAttribute:Attribute
    {
        public GroupAttribute(string name)
        {
            this.Name = name;
        }
        public GroupAttribute(string name,int order)
        {
            this.Name = name;
            this.Order = order;
        }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }
    }
}
