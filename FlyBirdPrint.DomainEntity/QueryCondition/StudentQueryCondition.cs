using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.DomainEntity.QueryCondition
{
    /// <summary>
    /// 学生查询条件对象
    /// </summary>
    public class StudentQueryCondition
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string KeyWord { get; set; }
    }
}
