using System.Security.Principal;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.Utilities.Interface
{
    public interface ILoginAuthedUserDTO: IPrincipal
    {
        /// <summary>
        /// 主店铺Id
        /// </summary>
        long CreateUserId { get; set; }

        /// <summary>
        /// 店铺所在组Id
        /// </summary>

        long GroupId { get; set; }

        /// <summary>
        /// 商户Id
        /// </summary>

        long UserId { get; set; }

        string UserName { get; set; }

        /// <summary>
        /// 当前电商平台
        /// </summary>
        PlatformEnum Platform { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        string Access_token { get; set; }

        

    }
}