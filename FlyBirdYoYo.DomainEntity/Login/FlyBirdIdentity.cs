using FlyBirdYoYo.Utilities.SystemEnum;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace FlyBirdYoYo.DomainEntity.Login
{
    /// <summary>
    /// 自定义认证标识
    /// </summary>
    public class FlyBirdIdentity : IIdentity
    {

        public FlyBirdIdentity(LoginTypeEnum authType, bool isAuth, string name)
        {
            this._AuthenticationType = authType.ToString();
            this._IsAuthenticated = isAuth;
            this._Name = name;
        }

        private string _AuthenticationType;
        public string AuthenticationType
        {
            get
            {
                return this._AuthenticationType;
            }
        }


        private bool _IsAuthenticated;
        public bool IsAuthenticated
        {
            get
            {
                return this._IsAuthenticated;
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return this._Name;
            }
        }
    }
}
