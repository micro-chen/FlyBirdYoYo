
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.Utilities.Interface
{
    public interface IBaseLoginViewModel
    {
         
        LoginTypeEnum LoginType { get; set; }


        /// <summary>
        /// 归属的平台
        /// </summary>
         PlatformEnum Platform { get; set; }


        /// <summary>
        /// 用户id
        /// </summary>
         long UserId { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
         string UserName { get; set; }

        /// <summary>
        /// 用户店铺id
        /// </summary>
         string ShopId { get; set; }


        /// <summary>
        ///店铺名称
        /// </summary>
         string ShopName { get; set; }



        bool IsValid(out string msg);
    }
}