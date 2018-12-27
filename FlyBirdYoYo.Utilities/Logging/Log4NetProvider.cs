using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.IO;

namespace FlyBirdYoYo.Utilities.Logging
{
    /// <summary>
    /// 实现基于log4核心的 日志Provider
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers =
            new ConcurrentDictionary<string, Log4NetLogger>();


        /// <summary>
        /// log4的配置文件路径
        /// </summary>
        public static string Log4ConfigFilePath
        {
            get
            {
                string configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs");
                var log4ConfigFile = Path.Combine(configDir, "log4net.config");
                if (!File.Exists(log4ConfigFile))
                {
                    throw new Exception("未能找到 log4net 的配置文件！在路径：" + log4ConfigFile);
                }

                return log4ConfigFile;
            }
        }

        public Log4NetProvider(string log4NetConfigFile)
        {
            _log4NetConfigFile = log4NetConfigFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            return new Log4NetLogger(name, LoadLog4NetConfigFile(_log4NetConfigFile));
        }

        private static XmlElement LoadLog4NetConfigFile(string filename)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(filename));
            return log4netConfig["log4net"];
        }
    }
}
