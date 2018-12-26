using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using FlyBirdYoYo.Utilities.Logging;

namespace FlyBirdYoYo.Web.Mvc
{
	public class CustomErrorHandlerFilter : ExceptionFilterAttribute
	{

		/// <summary>
		/// 重写基类中的异常处理方法
		/// </summary>
		/// <param name="filterContext"></param>
		public override void OnException(ExceptionContext filterContext)
		{

			//记录日志
			try
			{


				StringBuilder sb = new StringBuilder();
				sb.AppendFormat("Controller :{0}", filterContext.RouteData.Values["controller"].ToString());
				sb.Append(Environment.NewLine);
				sb.AppendFormat("Action  :{0}", filterContext.RouteData.Values["action"].ToString());
				sb.Append(Environment.NewLine);
				sb.AppendFormat("Exception :{0}", filterContext.Exception.ToString());
				sb.Append(Environment.NewLine);
				Logger.Error(sb.ToString());

				var webStatus = WorkContext.HostingEnvironment.EnvironmentName;
				if (webStatus == EnvironmentName.Production)
				{
					//正式环境
					//正式环境 跳转到错误页防止看到错误信息
					WorkContext.GoToErrorPage(filterContext.HttpContext);

				}
				else
				{
                    //开发模式

                    var result = new { Type = 0, Msg = "您没有权限或者访问错误" };
                    filterContext.HttpContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                    filterContext.HttpContext.Response.WriteAsync(result.ToJson(), Encoding.UTF8)
                        .ConfigureAwait(true)
                        .GetAwaiter();

                    base.OnException(filterContext);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				//一旦使用 此过滤器 那么 最终标志 异常被处理
				filterContext.ExceptionHandled = true;
			}


			//base.OnException(filterContext);
		}
	}
}
