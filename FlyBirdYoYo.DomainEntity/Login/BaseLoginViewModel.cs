 using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.DomainEntity.Login
{

    /// <summary>
    /// 登录验证基模型
    /// </summary>
    public  class BaseLoginViewModel : IBaseLoginViewModel
    {

        /// <summary>
        /// 登录类型
        /// </summary>

        public LoginTypeEnum LoginType { get; set; }

        /// <summary>
        /// 归属的平台
        /// </summary>
        public PlatformEnum Platform { get; set; }


        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户店铺id
        /// </summary>
        public string ShopId { get; set; }


        /// <summary>
        ///店铺名称
        /// </summary>
        public string ShopName { get; set; }




        /// <summary>
        /// 是否合法
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid(out string msg)
        {
            msg = null;
            return true;
        }

    }
}
