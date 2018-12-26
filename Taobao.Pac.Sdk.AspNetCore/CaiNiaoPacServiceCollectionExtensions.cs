using System;
using Taobao.Pac.Sdk.AspNetCore;
using Taobao.Pac.Sdk.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PddServiceExtensions
    {

        /// <summary>
        /// 添加注册菜鸟打印isv服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        public static void AddCaiNiaoPac(this IServiceCollection services, Action<CaiNiaoPacOptions> optionsAction = null)
        {
            if (optionsAction != null)
            {
                services.Configure(optionsAction);
            }
            services.AddSingleton<ICaiNiaoPacService, CaiNiaoPacService>();
        }
    }
}
