using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.DEncrypt;
using FlyBirdYoYo.Utilities.Interface;

using FlyBirdYoYo.Utilities.Http;
using FlyBirdYoYo.Utilities.Ioc;
using Pdd.OpenSdk.Core;

namespace FlyBirdYoYo.Web.Mvc
{
    public class BaseMvcController : Controller
    {

        //private static object syncRoot = new Object();


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
        /// 重写  控制器的 action 执行方法
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //注意 web api 在本设计中 ，不让设置 页面标识 Cookie 。增加这个Cookie 是为了防止采集和蜘蛛
            if (context.Controller is BaseApiControllerAuth)
            {
                base.OnActionExecuting(context);
                return;
            }

            //1监测是否有了必须的Cookie标识  如果没有 那么追加
            var cookieWebbrowserSign = context.HttpContext.GetCookie(Contanst.Cookie_Key_BrowserSign);
            if (string.IsNullOrEmpty(cookieWebbrowserSign))
            {
                cookieWebbrowserSign = string.Concat(WorkContext.SiteName, "|", DateTime.Now.ToTimeStampMilliseconds());

                //将加密后的cookie  写入到响应客户端 
                string domain = null;
                //判断是否为正式环境
                if (WorkContext.HostingEnvironment.IsProduction())
                {
                    //正式环境cookie 过期为1天
                    domain = Contanst.Global_Site_Domain_Cookie;
                    context.HttpContext.SetCookie(domain, Contanst.Cookie_Key_BrowserSign, cookieWebbrowserSign, TimeSpan.FromDays(1), true);
                }
                else
                {
                    //测试环境 cookie 不过期
                    context.HttpContext.SetCookie(domain, Contanst.Cookie_Key_BrowserSign, cookieWebbrowserSign, true);
                }
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 参数逗号分隔符
        /// </summary>
        public char SplitChar
        {
            get
            {
                return ',';
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
