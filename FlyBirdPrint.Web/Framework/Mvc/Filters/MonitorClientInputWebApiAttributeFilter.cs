using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FlyBirdYoYo.Web.Mvc
{
    /// <summary>
    /// WebApi 访问 对提交过来的参数进行日志监控过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class MonitorClientInputWebApiAttributeFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 是否需要 监视
        /// 可以给单个控制器添加一个属性，然后设置为false  表示不进行监视
        /// </summary>
        public bool IsCheck { get; set; }




        public MonitorClientInputWebApiAttributeFilter()
        {
            this.IsCheck = ConfigHelper.AppSettingsConfiguration.GetConfigBool("MonitorClientInput");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {

            if (false == IsCheck)
            {
                return base.OnActionExecutionAsync(actionContext, next);
            }

            try
            {


                //记录错误日志
                string toAccessUrl = actionContext.HttpContext.Request.Path.Value;//actionContext.ControllerContext.Request.RequestUri.ToString();
                string clientIP = actionContext.HttpContext.Request.GetIP();
                string routeDataString = string.Empty;
                if (null != actionContext.ActionArguments)
                {
                    //actionContext.RequestContext.RouteData
                    routeDataString = actionContext.ActionArguments.ToJson();
                }
                string msg = string.Concat(
                    "访问IP地址:", clientIP,
                    "。访问地址:", toAccessUrl,
                    "。提交路由参数:", routeDataString);
                Logger.Info(msg);

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return base.OnActionExecutionAsync(actionContext, next);
        }


    }
}
