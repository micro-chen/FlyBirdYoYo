using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace FlyBirdYoYo.DomainEntity.Login
{
    /// <summary>
    /// 系统管理员登录结果模型
    /// </summary>
    public class LoginSystemAdminResultViewModel
    {


        public long AdminUserId { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>

        public string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>

        public bool IsSuccess { get; set; }


        public int Step { get; set; }

 
        public string Sign { get; set; }
    }
}
