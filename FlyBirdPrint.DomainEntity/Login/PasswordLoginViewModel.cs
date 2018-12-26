using System;
using System.Collections.Generic;
using System.Text;

namespace FlyBirdYoYo.DomainEntity.Login
{
    /// <summary>
    /// 登录表单参数
    /// </summary>
    public class PasswordLoginViewModel: BaseLoginViewModel
    {


        /// <summary>
        ///密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string CheckCode { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        public override bool IsValid(out string msg)
        {
            bool result = false;
            msg = string.Empty;
            if (this.UserName.IsNullOrEmpty()||this.Pwd.IsNullOrEmpty())
            {
                msg = "用户名、密码不能为空！";
                return result;
            }



            return result=true;
        }
    }
}
