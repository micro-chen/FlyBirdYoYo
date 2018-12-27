using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.DomainEntity.ViewModel.Admin
{
    /// <summary>
    /// 超管账号查看检索商家信息
    /// </summary>
    public class UserShopInfoViewModel
    {

        /// <summary>
        /// 用户系统ID
        /// </summary>		
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 平台ID
        /// </summary>		
        public int PlatformId
        {
            get;
            set;
        }
        /// <summary>
        /// 平台名称
        /// </summary>		
        public string PlatformName
        {
            get;
            set;
        }
        /// <summary>
        /// 平台店铺ID
        /// </summary>		
        public string ShopId
        {
            get;
            set;
        }
        /// <summary>
        /// 店铺名称
        /// </summary>		
        public string ShopName
        {
            get;
            set;
        }

        /// <summary>
        /// 登录次数
        /// </summary>		
        public int LoginCount
        {
            get;
            set;
        }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>		
        public string LastLoginTime
        {
            get;
            set;
        }
        /// <summary>
        /// 服务过期时间
        /// </summary>		
        public string ExpireTime
        {
            get;
            set;
        }

        /// <summary>
        /// 进入对应商家地址
        /// </summary>
        public string EnterShopUrl { get; set; }

    }
}
