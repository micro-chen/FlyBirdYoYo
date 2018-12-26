using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.IO;

namespace FlyBirdYoYo.Utilities.Logging
{

    /// <summary>
    /// .net core 自身的Logger
    /// </summary>
    public class Logger
    {


        private static object _locker_CreateLogFactory = new object();

        /// <summary>
        /// 系统自带的日志工厂类
        /// 在程序启动的时候 注册到这个地方
        /// </summary>
        public static ILoggerFactory LogFactory;



 

        public static ILogger _Logger;
        /// <summary>
        /// 程序内置的日志记录器
        /// </summary>
        public static ILogger LoggerWriter
        {
            get
            {
                if (null == _Logger)
                {
                    if (null==LogFactory)
                    {
                        //throw new Exception("app logfactory is null  !");
                        lock (_locker_CreateLogFactory)
                        {
                            var factory = new LoggerFactory();
                            factory.AddLog4Net();
                            LogFactory = factory;
                        }
                      
                    }
                    _Logger = LogFactory.CreateLogger("NativeLogger"); 
                  }
                return _Logger;
            }
            set
            {
                _Logger = value;
            }
        }

        /// <summary>
        /// 是否输出日志
        /// </summary>
        private static bool IsOutPutLog
        {
            get
            {
                var configValue = ConfigHelper.AppSettingsConfiguration.GetConfigBool("IsOutPutLog");// ConfigHelper.GetConfigBool(ConfigHelper.HostingConfiguration,"IsOutPutLog");
                return configValue;
            }
        }



        public static void Info(string msg)
        {
            if (!IsOutPutLog)
            {
                return;
            }
            LoggerWriter.LogInformation(msg);
        }
        public static void Error(string msg)
        {
            if (!IsOutPutLog)
            {
                return;
            }
            LoggerWriter.LogError(msg);
        }
       
        /// <summary>
        /// Error 级别的log
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="title"></param>

        public static void Error(Exception ex, string title = "Error")
        {
            
            string errMsg = string.Concat(title, ex.ToString());
            LoggerWriter.LogError(errMsg);

        }


    }
}
