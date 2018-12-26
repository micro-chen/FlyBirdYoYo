using System;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.DomainEntity.Login
{
    /// <summary>
    /// OAuth登录参数
    /// </summary>
    public class OAuthLoginViewModel : BaseLoginViewModel
    {
        public OAuthLoginViewModel()
        {
            this.LoginType = LoginTypeEnum.OAuth2;
        }


        /// <summary>
        /// 使用的Code
        /// </summary>
        public virtual string Code { get; set; }


        /// <summary>
        /// 授权码
        /// </summary>
        public string Access_token { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string Refresh_token { get; set; }

        /// <summary>
        /// token的过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 登录授权成功的json
        /// </summary>
        public string AuthJson { get; set; }

        /// <summary>
        ///是否来自超管模拟登录
        /// </summary>
        public bool IsByAdmiSimulation { get; set; }


        public override bool IsValid(out string msg)
        {
            msg = "";
            return true;
        }
    }
}
