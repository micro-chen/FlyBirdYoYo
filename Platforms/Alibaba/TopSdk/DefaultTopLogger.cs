using Top.Api.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Top.Api
{

    /// <summary>
    /// 日志类 
    /// 队列 可年/月/周/日/大小分割
    /// 调用方法：
    ///  Log.Instance.LogDirectory=@"C:\"; 默认为程序运行目录
    ///  Log.Instance.FileNamePrefix="cxd";默认为log_
    ///  Log.Instance.CurrentMsgType = MsgLevel.Debug;默认为Error
    ///  Log.Instance.logFileSplit = LogFileSplit.Daily; 日志拆分类型LogFileSplit.Sizely 大小
    ///  Log.Instance.MaxFileSize = 1024; 只有LogFileSplit.Sizely的时候配置有效
    ///  Log.Instance.LogWrite("aa");
    ///  Log.Instance.LogWrite("aa", MsgLevel.Debug);
    /// </summary>
    public class Log : IDisposable, ITopLogger
    {
        private static Log _instance = null;
        private static readonly object _synObject = new object();
        /// <summary>
        ///单例
        /// </summary>
        public static Log Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_synObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new Log();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// 日志对象的缓存队列
        /// </summary>
        private static Queue<Msg> _msgs;
        /// <summary>
        /// 日志写入线程的控制标记  ture写中|false没有写
        /// </summary>
        private bool _state;
        private string _logDirectory = AppDomain.CurrentDomain.BaseDirectory;

        private bool isDebugEnabled = false;
        /// <summary>
        /// 日志文件存放目录
        /// </summary>
        public string LogDirectory
        {
            get { return _logDirectory; }
            set { _logDirectory = value; }
        }
        private LogFileSplit _logFileSplit = LogFileSplit.Sizely;
        /// <summary>
        /// 日志拆分类型
        /// </summary>
        public LogFileSplit logFileSplit
        {
            get { return _logFileSplit; }
            set { _logFileSplit = value; }
        }
        private MsgLevel _currentLogLevel = MsgLevel.Infor;
        /// <summary>
        /// 当前日志记录等级
        /// </summary>
        public MsgLevel CurrentMsgType
        {
            get { return _currentLogLevel; }
            set { _currentLogLevel = value; }
        }
        /// <summary>
        /// 当前负责记录日志文件的名称
        /// </summary>
        private string _currentFileName = "1.log";
        private string _fileNamePrefix = "topsdk_";
        /// <summary>
        /// 日志的前缀名称，默认为log_
        /// </summary>
        public string FileNamePrefix
        {
            get { return _fileNamePrefix; }
            set { _fileNamePrefix = value; }
        }
        /// <summary>
        /// 日志文件生命周期的时间标记
        /// </summary>
        private DateTime _CurrentFileTimeSign = new DateTime();
        private int _maxFileSize = 1024;
        /// <summary>
        /// 单个日志文件默认大小(单位：兆)
        /// </summary>
        public int MaxFileSize
        {
            get { return _maxFileSize; }
            set { _maxFileSize = value; }
        }
        /// <summary>
        /// 文件后缀号
        /// </summary>
        private int _fileSymbol = 0;

        /// <summary>
        /// 当前文件大小(单位：B)
        /// </summary>
        private long _fileSize = 0;
        /// <summary>
        /// 日志文件写入流对象
        /// </summary>
        private StreamWriter _writer;

        /// <summary>
        /// 创建日志对象的新实例,根据指定的日志文件路径和指定的日志文件创建类型
        /// </summary>
        private Log()
        {
            if (_msgs == null)
            {
                GetCurrentFilename();
                _state = true;
                _msgs = new Queue<Msg>();
                Thread thread = new Thread(work);
                thread.Start();
            }
        }
        //日志文件写入线程执行的方法
        private void work()
        {
            while (true)
            {
                //判断队列中是否存在待写入的日志
                if (_msgs.Count > 0)
                {
                    Msg msg = null;
                    lock (_msgs)
                    {
                        msg = _msgs.Dequeue();
                        if (msg != null)
                        {
                            FileWrite(msg);
                        }
                    }
                }
                else
                {
                    //判断是否已经发出终止日志并关闭的消息
                    if (_state)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        FileClose();
                    }
                }
            }
        }
        /// <summary>
        /// 根据日志类型获取日志文件名,并同时创建文件到期的时间标记
        /// 通过判断文件的到期时间标记将决定是否创建新文件。
        /// </summary>
        /// <returns></returns>
        private
            void GetCurrentFilename()
        {
            DateTime now = DateTime.Now;
            string format = "";
            switch (_logFileSplit)
            {
                case LogFileSplit.Daily:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, now.Day);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddDays(1);
                    format = now.ToString("yyyyMMdd'.log'");
                    break;
                case LogFileSplit.Weekly:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, now.Day);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddDays(7);
                    format = now.ToString("yyyyMMdd'.log'");
                    break;
                case LogFileSplit.Monthly:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, 1);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddMonths(1);
                    format = now.ToString("yyyyMM'.log'");
                    break;
                case LogFileSplit.Annually:
                    _CurrentFileTimeSign = new DateTime(now.Year, 1, 1);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddYears(1);
                    format = now.ToString("yyyy'.log'");
                    break;
                default:
                    _fileSymbol++;
                    format = _fileSymbol.ToString() + ".log";
                    break;
            }
            if (File.Exists(Path.Combine(LogDirectory, _currentFileName)))
            {
                _fileSize = new FileInfo(Path.Combine(LogDirectory, _currentFileName)).Length;
            }
            else
            {
                _fileSize = 0;
            }
            _currentFileName = _fileNamePrefix + format.Trim();
        }
        //写入日志文本到文件的方法
        private void FileWrite(Msg msg)
        {
            try
            {
                if (_writer == null)
                {
                    FileOpen();
                }
                if (_writer != null)
                {
                    //判断文件到期标志,如果当前文件到期则关闭当前文件创建新的日志文件
                    if ((_logFileSplit != LogFileSplit.Sizely && DateTime.Now >= _CurrentFileTimeSign) ||
                        (_logFileSplit == LogFileSplit.Sizely && ((double)_fileSize / 1048576) > _maxFileSize))
                    {
                        GetCurrentFilename();
                        FileClose();
                        FileOpen();
                    }
                    _writer.Write(msg.datetime);
                    _writer.Write('\t');
                    _writer.Write(msg.type);
                    _writer.Write('\t');
                    _writer.WriteLine(msg.text);
                    _fileSize += System.Text.Encoding.UTF8.GetBytes(msg.ToString()).Length;
                    _writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }
        }
        //打开文件准备写入
        private void FileOpen()
        {
            _writer = new StreamWriter(LogDirectory + _currentFileName, true, Encoding.UTF8);
        }
        //关闭打开的日志文件
        private void FileClose()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }
        }
        /// <summary>
        /// 写入新日志,根据指定的日志对象Msg
        /// </summary>
        /// <param name="msg">日志内容对象</param>
        private void LogWrite(Msg msg)
        {
            if (msg.type < CurrentMsgType)
                return;
            if (_msgs != null)
            {
                lock (_msgs)
                {
                    _msgs.Enqueue(msg);
                }
            }
        }
        /// <summary>
        /// 写入新日志,根据指定的日志内容和信息类型,采用当前时间为日志时间写入新日志
        /// </summary>
        /// <param name="text">日志内容</param>
        /// <param name="type">信息类型</param>
        public void LogWrite(string text, MsgLevel type)
        {
            LogWrite(new Msg(text, type));
        }
        /// <summary>
        /// 写入新日志,根据指定的日志内容
        /// </summary>
        /// <param name="text">日志内容</param>
        public void LogWrite(string text)
        {
            LogWrite(text, MsgLevel.Debug);
        }
        /// <summary>
        /// 写入新日志,根据指定的日志时间、日志内容和信息类型写入新日志
        /// </summary>
        /// <param name="dt">日志时间</param>
        /// <param name="text">日志内容</param>
        /// <param name="type">信息类型</param>
        public void LogWrite(DateTime dt, string text, MsgLevel type)
        {
            LogWrite(new Msg(dt, text, type));
        }
        /// <summary>
        /// 写入新日志,根据指定的异常类和信息类型写入新日志
        /// </summary>
        /// <param name="e">异常对象</param>
        /// <param name="type">信息类型</param>
        public void LogWrite(Exception e)
        {
            LogWrite(new Msg(e.Message, MsgLevel.Error));
        }
        /// <summary>
        /// 销毁日志对象
        /// </summary>
        public void Dispose()
        {
            _state = false;
        }

        public void TraceApiError(string appKey, string apiName, string url, System.Collections.Generic.Dictionary<string, string> parameters, double latency, string errorMessage)
        {
            StringBuilder info = new StringBuilder();
            info.Append(appKey);
            info.Append(Constants.LOG_SPLIT);
            info.Append(apiName);
            info.Append(Constants.LOG_SPLIT);
            info.Append(TopUtils.GetIntranetIp());
            info.Append(Constants.LOG_SPLIT);
            info.Append(System.Environment.OSVersion.VersionString);
            info.Append(Constants.LOG_SPLIT);
            info.Append(latency);
            info.Append(Constants.LOG_SPLIT);
            info.Append(url);
            info.Append(Constants.LOG_SPLIT);
            info.Append(WebUtils.BuildQuery(parameters));
            info.Append(Constants.LOG_SPLIT);
            info.Append(errorMessage);
            this.Error(info.ToString());
        }

        public void Error(string message)
        {
            this.LogWrite(message,MsgLevel.Error);
        }

        public void Error(string format, params object[] args)
        {
            this.LogWrite(string.Format(format, args), MsgLevel.Error);
        }

        public void Warn(string message)
        {
            this.LogWrite(message, MsgLevel.Warn);
        }

        public void Warn(string format, params object[] args)
        {
            this.LogWrite(string.Format(format, args), MsgLevel.Warn);
        }

        public void Info(string message)
        {
            this.LogWrite(message, MsgLevel.Infor);
        }

        public void Info(string format, params object[] args)
        {
            this.LogWrite(string.Format(format, args), MsgLevel.Infor);
        }

        public bool IsDebugEnabled()
        {
            return this.isDebugEnabled;
        }

        public void Debug(string message)
        {
            if (isDebugEnabled)
            {
                this.LogWrite(message, MsgLevel.Debug);
            }
        }

        public void Debug(string format, params object[] args)
        {
            if (isDebugEnabled)
            {
                this.LogWrite(string.Format(format, args), MsgLevel.Debug);
            }
        }
    }

    /// <summary>
    /// 一个日志记录的对象
    /// </summary>
    public class Msg
    {
        /// <summary>
        /// 创建新的日志记录实例;日志记录的内容为空,消息类型为MsgType.Unknown,日志时间为当前时间
        /// </summary>
        public Msg()
            : this("", MsgLevel.Debug)
        {
        }
        /// <summary>
        /// 创建新的日志记录实例;日志事件为当前时间
        /// </summary>
        /// <param name="t">日志记录的文本内容</param>
        /// <param name="p">日志记录的消息类型</param>
        public Msg(string t, MsgLevel p)
            : this(DateTime.Now, t, p)
        {
        }
        /// <summary>
        /// 创建新的日志记录实例;
        /// </summary>
        /// <param name="dt">日志记录的时间</param>
        /// <param name="t">日志记录的文本内容</param>
        /// <param name="p">日志记录的消息类型</param>
        public Msg(DateTime dt, string t, MsgLevel p)
        {
            datetime = dt;
            type = p;
            text = t;
        }
        /// <summary>
        /// 日志记录的时间
        /// </summary>
        public DateTime datetime { get; set; }
        /// <summary>
        ///日志记录的内容
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 日志等级
        /// </summary>
        public MsgLevel type { get; set; }

        public new string ToString()
        {
            return datetime.ToString(CultureInfo.InvariantCulture) + "\t" + text + "\n";
        }
    }
    /// <summary>
    /// 日志文件拆分的枚举
    /// </summary>
    /// <remarks>日志类型枚举指示日志文件创建的方式,如果日志比较多可考虑每天创建一个日志文件
    /// 如果日志量比较小可考虑每周、每月或每年创建一个日志文件</remarks>
    public enum LogFileSplit
    {
        /// <summary>
        /// 此枚举指示每天创建一个新的日志文件
        /// </summary>
        Daily,
        /// <summary>
        /// 此枚举指示每周创建一个新的日志文件
        /// </summary>
        Weekly,
        /// <summary>
        /// 此枚举指示每月创建一个新的日志文件
        /// </summary>
        Monthly,
        /// <summary>
        /// 此枚举指示每年创建一个新的日志文件
        /// </summary>
        Annually,
        /// <summary>
        /// 日志文件大小超过指定的创建一个新的日志文件，MaxFileSize指定大小
        /// </summary>
        Sizely
    }
    /// <summary>
    /// 日志等级类型 Debug=0 Infor Warn Error
    /// </summary>
    public enum MsgLevel
    {
        /// <summary>
        /// 调试信息
        /// </summary>
        Debug = 0,
        /// <summary>
        /// 指示普通信息类型的日志记录
        /// </summary>
        Infor,
        /// <summary>
        /// 指示警告信息类型的日志记录
        /// </summary>
        Warn,
        /// <summary>
        /// 指示错误信息类型的日志记录
        /// </summary>
        Error
    }
}
