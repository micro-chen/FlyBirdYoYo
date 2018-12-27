using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.DomainEntity.ViewModel
{

    public abstract class OAuthViewModel
    {
        /// <summary>
        /// 登录授权地址
        /// </summary>
        public string AuthCodeAddress { get; set; }
    }
}
