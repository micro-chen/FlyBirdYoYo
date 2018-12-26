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
    /// WebApi 系统管理员 访问授权过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthSysAdminAttributeFilter : ActionFilterAttribute
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
        

        public AuthSysAdminAttributeFilter()
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
             


            //验证通过 那么直接执行 action  否则返回错误
            if (isSysAdmin==true)
            {
                //设定当前登录用户到安全上下文中
                return base.OnActionExecutionAsync(context, next);//有权限的话 直接继续执行要访问的action
            }
             else
            {
                //非超管
                //记录错误日志
                string toAccessUrl = context.HttpContext.Request.Path.Value;
                string msg = string.Concat("非法访问超管资源;IP地址：", context.HttpContext.Request.GetIP(), "。访问地址：", toAccessUrl);
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

 
     
    }
}
