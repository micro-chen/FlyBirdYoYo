using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using System.Net;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.Http;
using FlyBirdYoYo.Utilities.DEncrypt;
using FlyBirdYoYo.Utilities.Logging;
using Microsoft.AspNetCore.Mvc;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.Ioc;

namespace FlyBirdYoYo.Web.Mvc
{
    /// <summary>
    /// WebApi 访问授权过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthAttributeFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 是否需要检测授权
        /// 可以给单个控制器添加一个属性，然后设置为false  表示不进行授权
        /// </summary>
        public bool IsCheck { get; set; }



        /// <summary>
        /// 登录认证服务
        /// 此服务实例是通过asp.net core 内置DI容器进行的注入。在Startup中进行的服务配置
        /// </summary>
        private IAuthenticationService AuthenticationService
        {
            get
            {
                var instance = ServiceLocator.GetInstance<IAuthenticationService>();
                return instance;
            }
        }
        

        public AuthAttributeFilter()
        {
            ///////this.IsCheck = false;
        }

        

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (false == IsCheck)
            {
                return base.OnActionExecutionAsync(context, next);
            }

            //优先检测是否为超管用户
            bool isSysAdmin = this.AuthenticationService.CheckUserIsSystemAdminFromHttpContext();

            //当前登录过的用户id
            ILoginAuthedUserDTO currentUser = null;

            currentUser = this.AuthenticationService.GetAuthenticatedUserFromHttpContext();


            //验证通过 那么直接执行 action  否则返回错误
            if (null != currentUser||isSysAdmin)
            {
                //设定当前登录用户到安全上下文中
                ApplicationContext.Current.User = currentUser;
                return base.OnActionExecutionAsync(context, next);//有权限的话 直接继续执行要访问的action
            }

            //非超管并且不是普通商户
            if (!isSysAdmin&&null==currentUser)
            {

                //记录错误日志
                string toAccessUrl = context.HttpContext.Request.Path.Value;
                string msg = string.Concat("非法访问;IP地址：", context.HttpContext.Request.GetIP(), "。访问地址：", toAccessUrl);
                Logger.Error(msg);

                //输出错误信息
                var result = new BusinessViewModelContainer<string>
                {
                    Status = (int)CodeStatusTable.NotHaveAuth,
                    Msg = CodeStatusTable.NotHaveAuth.GetEnumDescription()
                };
                //////context.HttpContext.Response = context.HttpContext.Request.CreateResponse(HttpStatusCode.OK, result);
                //////context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                //////context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
                ////// return context.HttpContext.Response.WriteAsync(result);


                context.Result = new JsonResult(result);
            }


            return Task.FromResult<object>(null);
        }





        /// <summary>
        /// 监测是否来自浏览器
        /// </summary>
        /// <param name="actionContext"></param>
        private bool CheckIsComeFromWebBrowser(ActionExecutingContext actionContext)
        {

            bool result = false;
            string userAgent = actionContext.HttpContext.Request.Headers[HttpServerProxy.RequestHeaderKeyUserAgent];
            //验证UA
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }

            //验证请求参数sign
            string requestSign = actionContext.HttpContext.Request.GetQuery<string>("sign");
            if (string.IsNullOrEmpty(userAgent))
            {
                return false;
            }
            else
            {
                //验证sign 标识
                var isValidSign = WorkContext.CheckIsValidRequestSign(requestSign);
                if (false == isValidSign)
                {
                    return false;
                }
            }

            //验证cookie 标识
            var cookieWebbrowserSign = actionContext.HttpContext.GetCookie<string>(Contanst.Cookie_Key_BrowserSign);
            if (string.IsNullOrEmpty(cookieWebbrowserSign))
            {
                return false;
            }

            try
            {
                //这是加密内容cookieWebbrowserSingn = string.Concat(WorkContext.SiteName, ":", DateTime.Now.ToString());
                //解密cookie
                string signText = LZString.Decompress(cookieWebbrowserSign, true);

                if (!string.IsNullOrEmpty(signText))
                {
                    if (signText.Contains(WorkContext.SiteName))
                    {
                        string time = signText.Split('|')[1];//获取里面的时间 超过3小时 必须刷新页面 否则过期
                        if (!string.IsNullOrEmpty(time) && DateTime.Now.Subtract(time.ToDatetime()).Hours < 3)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }


            return result;
        }

    }
}
