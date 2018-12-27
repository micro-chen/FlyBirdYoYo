using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.Ioc;
using Pdd.OpenSdk.Core;
using FlyBirdYoYo.Utilities.Http;

namespace FlyBirdYoYo.Web.Mvc
{
    /// <summary>
    /// web api 的基础控制器
    /// 需要进行身份验证
    /// </summary>
    [ApiController]
    [AuthAttributeFilter(IsCheck =false)]//控制是否需要授权验证
    [CustomErrorHandlerFilter]
    [EnableCors("AllowAllDomain")]
    [Route("api/[controller]/[action]")]
    public class BaseApiControllerAuth : ControllerBase
    {

       

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public ILoginAuthedUserDTO CurrentUser
        {
            get
            {
                return ApplicationContext.Current.User;
            }
        }


        /// <summary>
        /// 拼多多的sdk 服务实例
        /// 依赖注入
        /// </summary>
        public IPddService PddSdkService
        {
            get
            {
                return ServiceLocator.GetInstance<IPddService>();
            }
        }

        /// <summary>
        /// 登录认证服务
        /// 此服务实例是通过asp.net core 内置DI容器进行的注入。在Startup中进行的服务配置
        /// </summary>
        protected IAuthenticationService AuthenticationServiceImpl
        {
            get
            {
                var instance = ServiceLocator.GetInstance<IAuthenticationService>();
                return instance;
            }
        }


    }

    /// <summary>
    /// web api 的基础控制器
    /// 不需要进行身份验证
    /// </summary>
    [ApiController]
    [CustomErrorHandlerFilter]
    [EnableCors("AllowAllDomain")]
    [Route("api/[controller]/[action]")]
    public class BaseApiControllerNoAuth : ControllerBase
    {
        /// <summary>
        /// 登录认证服务
        /// 此服务实例是通过asp.net core 内置DI容器进行的注入。在Startup中进行的服务配置
        /// </summary>
        protected IAuthenticationService AuthenticationServiceImpl
        {
            get
            {
                var instance = ServiceLocator.GetInstance<IAuthenticationService>();
                return instance;
            }
        }

        /// <summary>
        /// 客户端代理标识
        /// </summary>
        public string UserAgent
        {
            get
            {

                string userAgent = HttpContext.Request.Headers[HttpServerProxy.RequestHeaderKeyUserAgent];
                return userAgent;
            }
        }




        /// <summary>
        /// 当前用户的登录请求IP地址
        /// </summary>
        public string IpAddress
        {
            get
            {
                string userIP = Request.GetIP();
                return userIP;
            }
        }
    }
}
