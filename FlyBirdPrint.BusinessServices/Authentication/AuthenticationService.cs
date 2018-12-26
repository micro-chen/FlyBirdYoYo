using System;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.DEncrypt;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.DomainEntity.Login;
using FlyBirdYoYo.Utilities.Logging;

namespace FlyBirdYoYo.BusinessServices.Authentication
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        #region  字段

        /// <summary>
        /// 用户设置的加密私钥 键值名
        /// </summary>
        private const string EncryptionKeyName = "EncryptionKey";
        private const string AuthKey = "Authorization";

        /// <summary>
        /// 如果用户未设置 那么使用默认的私钥
        /// </summary>
        private readonly string _defaultEncrytionKeyValue = StringExtension.DEFAULT_ENCRYPT_KEY;


        #endregion


        #region  属性




        private string _encryptionKeyValue;
        /// <summary>
        /// 数据库中用户设置的加密密钥
        /// </summary>
        public string EncryptionKeyValue
        {
            get
            {
                if (null == _encryptionKeyValue)
                {
                    //查询出私钥
                    //if (null != dal_Setting)
                    //{
                    //    var setting = dal_Setting.GetElementsByCondition(x => x.Name == EncryptionKeyName).FirstOrDefault();
                    //    if (null != setting)
                    //    {
                    //        _encryptionKeyValue = setting.Value;
                    //    }
                    //}
                    if (string.IsNullOrEmpty(_encryptionKeyValue))
                    {
                        _encryptionKeyValue = this._defaultEncrytionKeyValue;
                    }
                }
                return _encryptionKeyValue;

            }
            set { _encryptionKeyValue = value; }
        }


        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthenticationService()
        {

        }


        #region 授权


        /// <summary>
        /// 授权登录并生成加密token
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Authentication(IBaseLoginViewModel model, out string token)
        {
            var result = false;
            token = string.Empty;

            //-----根据不同的登录类型。进行授权判断---------

            try
            {


                // 验证通过后    创建票据 并加密
                var userDto = AuthProviderFactory.Login((BaseLoginViewModel)model);

                if (null == userDto)
                {
                    throw new Exception("验证授权失败！未经授权的用户！");
                }

                var ticket = this.GenerateAuthenticationTicket(userDto);
                var encryptedTicket = this.EncryptAuthenticationTicket(ticket);

                token = encryptedTicket;

            }
            catch (Exception ex)
            {

                throw ex;
            }

            result = true;
            return result;
        }

        #endregion


        #region 从上下文获取用户信息

        /// <summary>
        /// 检测用户是否为超管用户
        /// </summary>
        /// <returns></returns>
        public bool CheckUserIsSystemAdminFromHttpContext()
        {
            LoginSystemAdminResultViewModel sysAdminUserDtoModel = null;

            try
            {
                //从当前上下文先检索认证的用户信息
                if (ApplicationContext.Current.IsSystemAdmin==true)
                {
                    return true;
                }
                //逆向 支持从 cookie 读取
                string ticket = string.Empty;

                //1 尝试从Cookie读取
                if (ApplicationContext.HttpContext.Current.Request.Cookies.ContainsKey(Contanst.Login_Cookie_SystemAdminUserInfo)
                    && ApplicationContext.HttpContext.Current.GetCookie(Contanst.Login_Cookie_SystemAdminUserInfo).IsNotEmpty())
                {
                    ticket = ApplicationContext.HttpContext.Current.GetCookie(Contanst.Login_Cookie_SystemAdminUserInfo);
                }


                if (ticket.IsNullOrEmpty())
                {
                    return false;
                }

                sysAdminUserDtoModel = ticket.FromJsonToObject<LoginSystemAdminResultViewModel>();
                if (null != sysAdminUserDtoModel)
                {
                    #region 验证基础签名

                  
                    string deSign = string.Empty;
                    try
                    {
                        deSign = DESEncrypt.Decrypt(sysAdminUserDtoModel.Sign);
                    }
                    catch
                    { }
                    if (deSign.IsNullOrEmpty())
                    {
                        return false;
                    }
                    string[] arrSign = deSign.Split('|');
                    long timeSnamp = arrSign[0].ToLong();
                    int step = arrSign[1].ToInt();

                    //时间戳之间的间隔不能过长-不可超过8小时
                    if ((DateTime.Now.ToTimeStampMilliseconds() - timeSnamp) / 1000 > 60 * 60 * 8)
                    {
                        return false;
                    }
                    #endregion

                    ApplicationContext.Current.IsSystemAdmin = sysAdminUserDtoModel.IsSuccess;
                }


                return ApplicationContext.Current.IsSystemAdmin;

            }
            catch (Exception ex)
            {
                throw ex;
            }




        }

        /// <summary>
        /// 从验证过的Cookie中获取登录用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ILoginAuthedUserDTO GetAuthenticatedUserFromHttpContext()
        {
            LoginAuthedUserDTO userDtoModel = null;

            try
            {
                //从当前上下文先检索认证的用户信息
                if (null != ApplicationContext.Current.User && ApplicationContext.Current.User.Identity.IsAuthenticated)
                {
                    userDtoModel = ApplicationContext.Current.User as LoginAuthedUserDTO;
                }
                if (null != userDtoModel)
                {
                    return userDtoModel;
                }
                //逆向 支持从 cookie和Header和Form表单读取
                string encryptedTicket = string.Empty;
                StringValues valuePair;

                //1 尝试从Cookie读取
                if (ApplicationContext.HttpContext.Current.Request.Cookies.ContainsKey(Contanst.Login_Cookie_Client_Key)
                    && ApplicationContext.HttpContext.Current.GetCookie(Contanst.Login_Cookie_Client_Key).IsNotEmpty())
                {
                    encryptedTicket = ApplicationContext.HttpContext.Current.GetCookie(Contanst.Login_Cookie_Client_Key);
                }

                else if (true == ApplicationContext.HttpContext.Current.Request.Headers.TryGetValue(AuthKey, out valuePair))
                {
                    //2 从头部获取
                    encryptedTicket = valuePair[0].URLDecode().URLDecode();//Note:两次转义用来防止二次编码
                }
                else
                {
                    //3 尝试从Form表单读取
                    encryptedTicket = ApplicationContext.HttpContext.Current.Request.GetForm<string>(AuthKey);
                }


                if (encryptedTicket.IsNullOrEmpty())
                {
                    return null;
                }
                //解密得到凭据
                var ticket = this.DecryptAuthenticationTicket(encryptedTicket);
                //非法用户--或者登录凭据过期的
                if (null == ticket || null == ticket.User || ticket.Expired)
                {
                    return null;
                }

                userDtoModel = ticket.User;
                //注册登录用户到上下文信息中
                var cardIdentity = new FlyBirdIdentity(userDtoModel.LoginType, true, userDtoModel.UserName);
                userDtoModel.SetIdentity(cardIdentity);

 
 

                #endregion

                ApplicationContext.Current.User = userDtoModel;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userDtoModel;


        }


     



        /// <summary>
        /// 加密验证的票据
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        private string EncryptAuthenticationTicket(AuthenticationTicket ticket)
        {

            var jsonTicket = ticket.ToJson();
            return DESEncrypt.Encrypt(jsonTicket, this.EncryptionKeyValue);
        }

        /// <summary>
        /// 将加密的票据  解密处理
        /// </summary>
        /// <param name="encryptedTicket"></param>
        /// <returns></returns>
        private AuthenticationTicket DecryptAuthenticationTicket(string encryptedTicket)
        {
            AuthenticationTicket ticket = null;

            try
            {
                //解密出来json
                var jsonTicket = DESEncrypt.Decrypt(encryptedTicket, this.EncryptionKeyValue);
                ticket = jsonTicket.FromJson<AuthenticationTicket>();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return ticket;
        }

        /// <summary>
        /// 为用户生成一个登录票据
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private AuthenticationTicket GenerateAuthenticationTicket(LoginAuthedUserDTO user)
        {
            if (null == user)
            {
                throw new Exception("用户不能为空！");

            }
            var now = DateTime.UtcNow.ToLocalTime();
            var expirationTime = ConfigHelper.AppSettingsConfiguration.GetConfigInt("signTimeOut");// FormsAuthentication.Timeout;
            if (expirationTime <= 0)
            {
                expirationTime = Contanst.Default_SignTimeOut;//分钟
            }
            var expirationTimeSpan = TimeSpan.FromMinutes(expirationTime);
            var ticket = new AuthenticationTicket() { User = user, Expiration = now.Add(expirationTimeSpan) };

            return ticket;
        }




    }
}