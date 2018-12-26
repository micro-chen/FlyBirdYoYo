using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.SystemEnum;

namespace FlyBirdYoYo.DomainEntity.Login
{

    /// <summary>
    /// 登录验证通过后的用户对象
    /// </summary>
    public class LoginAuthedUserDTO : ClaimsPrincipal, ILoginAuthedUserDTO
    {

        /// <summary>
        /// 主店铺Id
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 归属的总账户id
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }


        public PlatformEnum Platform { get; set; }

        /// <summary>
        /// 用户店铺id
        /// </summary>
        public string ShopId { get; set; }


        /// <summary>
        ///店铺名称
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string Access_token { get; set; }



        /// <summary>
        /// Token过期时间
        /// </summary>
        public DateTime TokenExpireTime { get; set; }




        #region 认证

        /// <summary>
        /// 登录认证类型
        /// </summary>
        public LoginTypeEnum LoginType { get; set; }

        /// <summary>
        /// 设置登录标识
        /// </summary>
        /// <param name="identity"></param>
        public void SetIdentity(FlyBirdIdentity identity)
        {
            this._Identity = identity;
        }

        private FlyBirdIdentity _Identity;
        /// <summary>
        /// 认证标识
        /// </summary>
        public override IIdentity Identity
        {
            get
            {
                return _Identity;
            }

        }

        #endregion

    }


}
