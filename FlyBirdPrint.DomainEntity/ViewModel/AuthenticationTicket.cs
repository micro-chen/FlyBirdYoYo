using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DomainEntity.Login;

namespace FlyBirdYoYo.DomainEntity.ViewModel
{

    /// <summary>
    /// 登录验证凭据
    /// </summary>
    public class AuthenticationTicket:IPrincipal
    {

        public AuthenticationTicket()
        {
            this.Name = Contanst.Login_Cookie_Client_Key;
            this.IssueDate = DateTime.Now;
        }

        /// <summary>
        /// 获取 身份验证票证过期时的本地日期和时间。
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// 获取一个值，它指示 Forms 身份验证票证是否已过期。
        /// 是否不在有效期内
        /// </summary>
        public bool Expired
        {
            get
            {
                return this.Expiration < DateTime.Now ? true : false;
            }
        }

        /// <summary>
        /// 获取最初发出  身份验证票证时的本地日期和时间。
        /// </summary>
        public DateTime IssueDate { get; private set; }

        /// <summary>
        /// 获取与  身份验证票相关联的用户名。
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 获取一个存储在票证中的用户特定的字符串。
        /// </summary>
        public LoginAuthedUserDTO User { get; set; }





        #region Inner 对象 

        //内置的身份标识对象
        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
