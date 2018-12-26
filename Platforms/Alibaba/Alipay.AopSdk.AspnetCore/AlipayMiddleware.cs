using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Alipay.AopSdk.AspNetCore
{
    /// <summary>
    /// 支付宝支付 中间件
    /// </summary>
    public class AlipayMiddleware
    {
        private readonly RequestDelegate _next;

        public AlipayMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 钩子衔接
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            //return  this._next(context);
            await _next(context);
        }
    }
}
