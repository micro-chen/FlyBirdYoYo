using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using FlyBirdYoYo.Utilities;

namespace FlyBirdYoYo.Web
{
    public class Program
    {
        /// <summary>
        /// 应用程序载入口
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }



        /// <summary>
        /// 配置承载 host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            /*
              从配置文件 加载 host server  port 配置
              注意：如果是使用 IDE ，那么是从  launchSettings.json 加载的配置。此配置 是为发布包的启动进行的配置
              使用 dotnet run  。启动此端口的配置；从appsettings.josn 文件加载配置：Configuration 对象。然后将键值对
              用环境变量的形式加载。donet core 对自身保留的环境变量进行自识别！！！如启动端口：urls 节点
             */

            var cfgBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json",false,true);

            var config = cfgBuilder.Build();

            var host = WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

            return host;

        }


    }
}