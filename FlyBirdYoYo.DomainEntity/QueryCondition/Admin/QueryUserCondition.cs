using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.DomainEntity.QueryCondition.Admin
{
    /// <summary>
    /// 检索平台商家店铺参数
    /// </summary>
    public class QueryUserCondition
    {
        public QueryUserCondition()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public PlatformEnum? Platform { get; set; }

        public string KeyWord { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
