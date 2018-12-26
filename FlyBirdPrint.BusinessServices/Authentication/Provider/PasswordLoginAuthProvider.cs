using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.DomainEntity.Login;
using FlyBirdYoYo.DomainEntity.ViewModel;

namespace FlyBirdYoYo.BusinessServices.Authentication
{
    /// <summary>
    /// 用户名密码登录验证
    /// </summary>
    public class PasswordLoginAuthProvider
    {
        public LoginAuthedUserDTO Login(PasswordLoginViewModel model)
        {
            if (model==null)
            {
                throw new Exception("用户登录对象为空！");
            }

            //UserInfoModel user = null;

            ////---------Todo:通过【统一登录授权管理服务】拉取用户信息
            ////user = dal_Users
            ////      .GetElementsByCondition(x => x.UserName==userName)
            ////      .FirstOrDefault();

            //if (null == user)
            //{
            //    return result;
            //}

            //2 查询出来用户后 对比加密过的授权信息，license key
            //var encryPwd = EncryptionService.CreatePasswordHash(pwd, user.PasswordSalt);
            //if (!string.Equals(encryPwd, user.Password))
            //{
            //    return result;
            //}
            //else
            //{
            //    result = true;
            //}


            var userDto = new LoginAuthedUserDTO { UserId = 1, GroupId = 1, UserName = "我在这里" };

            return userDto;


        }
    }
}
