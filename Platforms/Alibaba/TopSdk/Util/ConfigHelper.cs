using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Caching.Memory;


namespace Top.Api.Util
{
    internal static class ConfigHelper
    {



        #region 属性


        private static IConfiguration _appSettingsConfiguration;
        /// <summary>
        /// 应用程序的配置
        /// 系统启动 appsetting配置 DI对象
        /// </summary>
        public static IConfiguration AppSettingsConfiguration
        {
            get
            {
                if (null==_appSettingsConfiguration)
                {
                    _appSettingsConfiguration = GetConfiguration();
                }
                return _appSettingsConfiguration;
            }
            set
            {
                _appSettingsConfiguration = value;
            }
        }


       

        #endregion


        /// <summary>
        /// 从自定义的配置文件目录加载配置  /Configs 目录下的配置文件
        /// 系统默认的配置文件 appsettings.json 会跟随系统的DI 过来
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration()
        {

            string configFileName = "appsettings.json";
            string configFileFullPath = Path.Combine(Directory.GetCurrentDirectory(), configFileName);
            if (!File.Exists(configFileFullPath))
            {
                throw new FileNotFoundException(string.Format("the config file : {0} is not be found.", configFileFullPath));
            }

            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(configFileName)
          .AddEnvironmentVariables(prefix: "ASPNETCORE_");
        

           

            var configRoot = builder.Build();


            return configRoot;

        }

        /// <summary>
        /// 返回配置文件 *.json 中的指定key的 value
        /// </summary>
        /// <param name="config"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfig(this IConfiguration config, string key)
        {
            return config.GetValue<string>(key);
        }

        //public static string GetConfigString(string key)
        //{
        //    return AppSettingsConfiguration.GetValue<string>(key);
        //}

      

     
        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(this IConfiguration config, string key)
        {
            bool result = false;
            string cfgVal = GetConfig(config, key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }
            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(this IConfiguration config, string key)
        {
            int result = 0;
            string cfgVal = GetConfig(config, key);
            if (null != cfgVal && string.Empty != cfgVal)
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    // Ignore format exceptions.
                }
            }

            return result;
        }

    }

   
}
