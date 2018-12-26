using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace FlyBirdYoYo.Utilities.Logging
{
    /// <summary>
    /// 向日志工厂注册log4net 日志provider
    /// </summary>
    public static class Log4netExtensions
    {
        public static ILoggerFactory AddLog4Net(this LoggerFactory factory)
        {
            factory.AddProvider(new Log4NetProvider(Log4NetProvider.Log4ConfigFilePath));
            return factory;
        }

        public static ILoggerFactory AddLog4Net(this LoggerFactory factory, string log4NetConfigFile)
        {
            factory.AddProvider(new Log4NetProvider(log4NetConfigFile));
            return factory;
        }

      
    }
}
