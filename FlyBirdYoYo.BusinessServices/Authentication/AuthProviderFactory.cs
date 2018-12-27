
using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.DomainEntity.Login;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.SystemEnum;
namespace FlyBirdYoYo.BusinessServices.Authentication
{
    /// <summary>
    /// 静态登录验证工厂
    /// </summary>
    public static class AuthProviderFactory
    {
        public static LoginAuthedUserDTO Login(BaseLoginViewModel model)
        {
            LoginAuthedUserDTO resultDtoModel = null;

            switch (model.LoginType)
            {
                case LoginTypeEnum.Password:
                    var provider = new PasswordLoginAuthProvider();
                    resultDtoModel= provider.Login(model as PasswordLoginViewModel);
                    break;
                case LoginTypeEnum.OAuth2:
                    var provider_oauth = new OAuthLoginAuthProvider();
                    resultDtoModel = provider_oauth.Login(model as OAuthLoginViewModel);
                    break;
                case LoginTypeEnum.WeChat:
                    ///throw new NotImplementedException();
                    break;
                default:
                    break;
            }
            if (null!=resultDtoModel)
            {
                resultDtoModel.LoginType = model.LoginType;
            }

            return resultDtoModel;
        }
    }
}
