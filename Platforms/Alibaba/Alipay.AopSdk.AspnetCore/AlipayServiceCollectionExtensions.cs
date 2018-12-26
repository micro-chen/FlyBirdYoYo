using System;
using Alipay.AopSdk.AspNetCore;
using Alipay.AopSdk.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
	/// 支付宝 支付服务
	/// </summary>
	public static class AlipayServiceCollectionExtensions
    {
        /// <summary>
        /// 注册支付宝服务模块组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddAlipay(this IServiceCollection services, Action<AlipayOptions> options)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (options == null)
                throw new ArgumentNullException(nameof(options));
            services.AddOptions();
            services.Configure(options);
            services.AddSingleton<IAlipayService, AlipayService>();
            return services;
        }
    }
}
