
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.Logging;
using FlyBirdYoYo.Utilities.Caching;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.DomainEntity.Message;

namespace FlyBirdYoYo.BusinessServices.Message
{
    /// <summary>
    /// 消息处理工作台
    /// </summary>
    public static class MessageWorkBench
    {
        private static object _locker_ThreadCount = new object();
        private static int _threadCount = 1;
        /// <summary>
        /// 最大执行线程数
        /// </summary>
        private static int ExcuteThreadCount
        {
            get
            {

                lock (_locker_ThreadCount)
                {
                    _threadCount = ConfigHelper.AppSettingsConfiguration.GetConfigInt("ThreadCount");
                    if (_threadCount <= 0)
                    {
                        _threadCount = 1;
                    }
                }

                return _threadCount;
            }
        }

        /// <summary>
        /// 最大消息阈值，超过该阈值 将暂停抓消息并等待
        /// </summary>
        private static int _max_msessage_limit
        {
            get
            {
                var limit = ConfigHelper.AppSettingsConfiguration.GetConfigInt("MaxMsessageLimit");
                if (limit <= 0)
                {
                    limit = 5000;
                }
                return limit;
            }

        }
        /// <summary>
        /// redis 扫描 游标
        /// </summary>
        private static long _redis_scan_cursor = 0;
        /// <summary>
        /// 休眠时间
        /// </summary>
        private static int _sleepTimeSpan;

        //private static bool _beStop = false;
        private static CancellationTokenSource _cts;
        private static CancellationToken _token;




        /// <summary>
        /// 记录已经处理过的消息的键集合
        /// 防止消息重复读取处理
        /// </summary>
        public static ConcurrentBag<string> LocalProcessdHistoryHashTable = new ConcurrentBag<string>();

        /// <summary>
        /// 进行【采集】消息处理的线程池
        /// </summary>
        private static ConcurrentQueue<Thread> _TheadQueuePool_FetchMessage = new ConcurrentQueue<Thread>();

        /// <summary>
        /// 进行【处理】消息处理的线程池
        /// </summary>
        private static ConcurrentQueue<Thread> _TheadQueuePool_Processor = new ConcurrentQueue<Thread>();

        private static ConcurrentQueue<IMessageProcessor> _LstProcessors = new ConcurrentQueue<IMessageProcessor>();
        /// <summary>
        /// 开启全部的任务
        /// </summary>
        /// <returns></returns>
        public static void Start()
        {

            Logger.Info("MessageWorkBench 开始......");
            //_beStop = false;
            try
            {
                //初始化取消标记源
                _cts = new CancellationTokenSource();
                _token = _cts.Token;

                //并行，将从redis 中抓取数据进行消费
                Task.Factory.StartNew(() =>
                {
                    Parallel.Invoke(
                         () => { StartFetchDataFromRedis(); },//抓取消息任务
                         () => { ProcessMessageDataQueue(); }//消费消息任务

                    );

                });


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static object _locker_CheckIsInProcessHistory = new object();
        /// <summary>
        /// 检索是否处在历史消息表中
        /// 历史消息表上限为1000，超过1000后，执行清理掉集合
        /// 在1000个消息期间，成功处理的消息会被移除掉！！！
        /// </summary>
        /// <returns></returns>
        public static bool CheckIsInProcessHistory(string key)
        {
            bool result = false;

            lock (_locker_CheckIsInProcessHistory)
            {


                if (LocalProcessdHistoryHashTable.Contains(key))
                {
                    result = true;
                }
                if (LocalProcessdHistoryHashTable.Count > 1000)
                {
                    LocalProcessdHistoryHashTable = null;
                    LocalProcessdHistoryHashTable = new ConcurrentBag<string>();
                }
            }
            return result;
        }

        /// <summary>
        /// 启动抓取消息的线程任务
        /// </summary>
        private static void StartFetchDataFromRedis()
        {
            for (int i = 0; i < ExcuteThreadCount; i++)
            {
                var th = new Thread(new ThreadStart(FetchDataFromRedis));
                th.IsBackground = true;
                th.Priority = ThreadPriority.Normal;
                //压入采集队列
                _TheadQueuePool_FetchMessage.Enqueue(th);

                th.Start();


            }
        }


        /// <summary>
        /// 从redis 中不间断的批量读取消息
        /// </summary>
        private static void FetchDataFromRedis()
        {

            while (true)
            {
                RedisCacheManager redisClient = null;
                try
                {

                    if (_token.IsCancellationRequested == true)
                    {
                        break;//终止信号
                    }
                    //目前仅处理千浪计划消息
                    string matchPattern = string.Concat(BaseMessageModel.CachePrefix, "*");




                    //Logging.Logger.Info("组件匹配的键模式："+matchPattern);

                    redisClient = new RedisCacheManager();
                    var scanResult = redisClient.Scan(matchPattern, 10, _redis_scan_cursor);

                    #region 限流策略---针对不同的积压阶段实施限流措施，防止拖死进程和redis

                    //游标置为0 的时候 标识已经回到起始的位置
            
                    if (null == scanResult || _redis_scan_cursor == 0)
                    {
                        
                        _sleepTimeSpan = 1000;
                        RunningLocker.CreateNewLock().CancelAfter(_sleepTimeSpan);
                        if (null == scanResult)
                        {
                            continue;//如果null 结果 那么不用继续下面的逻辑
                        }
                    }

                    //积压2000
                    if (QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.Count > 2000)
                    {
                        _sleepTimeSpan = 2000;
                        RunningLocker.CreateNewLock().CancelAfter(_sleepTimeSpan);
                    }
                    //积压5000
                    if (QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.Count > 5000)
                    {
                        _sleepTimeSpan = 3000;
                        RunningLocker.CreateNewLock().CancelAfter(_sleepTimeSpan);
                    }

                    if (QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.Count > 100000)
                    {
                        Logger.Error(new Exception("消费队列过于缓慢！内存积压严重！"));
                        break;
                    }

                    #endregion

                    //回写游标
                    _redis_scan_cursor = scanResult.Cursor;

                    var allKeys = scanResult.Results;
                    if (allKeys.IsEmpty())
                    {
                        RunningLocker.CreateNewLock().CancelAfter(1000);
                        continue;
                    }

                    if (null != allKeys && allKeys.IsNotEmpty())
                    {
                        //var redisClient = new RedisCacheManager().RedisClient;

                        foreach (var key in allKeys)
                        {
                            //Logging.Logger.Info("恭喜大人，key is："+key);
                            try
                            {
                                var isInHistory = CheckIsInProcessHistory(key);
                                if (isInHistory == true)
                                {
                                    continue;
                                }
                                else
                                {
                                    LocalProcessdHistoryHashTable.Add(key);
                                }

                                //处理管线
                                if (key.Contains(ProcessPipeLine.QianLangPlanQuan.ToString()))
                                {
                                    //将当前扫描的键  获取对象结果，添加到集合中
                                    QianLangQuanMessage item = redisClient.Get<QianLangQuanMessage>(key);
                                    if (null != item)
                                    {

                                        if (!QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.ContainsKey(key))
                                        {
                                            QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.TryAdd(key, item);
                                        }

                                    }
                                }
                                else
                                {
                                    redisClient.Remove(key);
                                    continue;//无效的消息  直接移除掉

                                }


                            }
                            catch (Exception ex)
                            {
                                Logger.Error(ex);
                            }

                        }




                    }



                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
                finally
                {
                    if (null != redisClient)
                    {
                        redisClient.Dispose();
                    }

                }



            }

        }

        //private static int count = 0;

        /// <summary>
        /// 开启N 个线程，将从redis 中抓取数据进行消费
        /// </summary>
        private static void ProcessMessageDataQueue()
        {
            //var tm = new System.Timers.Timer();
            //tm.Interval = 1000;
            //tm.Elapsed += (s, e) => {
            //    System.Diagnostics.Debug.WriteLine(count);
            //};
            //tm.Start();

            for (int i = 0; i < ExcuteThreadCount; i++)
            {
                var th = new Thread(new ThreadStart(() =>
                {
                    IMessageProcessor QianLangQuanProcessor = new QianLangPlanQuanProcessor();
                    _LstProcessors.Enqueue(QianLangQuanProcessor);

                    while (true)
                    {
                   

                        try
                        {
                            if (_token.IsCancellationRequested == true)
                            {
                                break;//终止信号
                            }

                            ////如果集合都是空 那么等待休眠
                            if (QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.IsEmpty == true)
                            {
                                RunningLocker.CreateNewLock().CancelAfter(1000 * 2);//休眠2秒
                            }

                            //count++;

                            //并行执行
                            Parallel.Invoke(

                               () =>
                               {
                                   //处理消息
                                   if (QianLangPlanQuanProcessor.MessageDataQueueOfQianLangQuan.IsEmpty)
                                   {
                                       return;
                                   }
                                   var pair = QianLangPlanQuanProcessor.PopOneQianLangQuanMessage();
                                   if (default(KeyValuePair<string, IMessage>).Equals(pair))
                                   {
                                       return;
                                   }
                                   string key = pair.Key;
                                   IMessage itemFee = pair.Value;

                                   QianLangQuanProcessor.Process(key, itemFee);


                               }



                               );



                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }
                    }



                }));
                th.IsBackground = true;
                th.Priority = ThreadPriority.Normal;

                //压入处理线程队列
                _TheadQueuePool_Processor.Enqueue(th);

                th.Start();

            }
        }


        /// <summary>
        /// 停止全部任务
        /// </summary>
        /// <returns></returns>
        public static void Stop()
        {
            Logger.Info("MessageWorkBench 停止......");
            try
            {
                //停止标识
                _cts.Cancel();


                if (null != _LstProcessors)
                {
                    ///停止消费；迭代堆栈 从里面建处理器停止并注销
                    while (!_LstProcessors.IsEmpty)
                    {
                        try
                        {
                            IMessageProcessor procesorItem = null;
                            _LstProcessors.TryDequeue(out procesorItem);
                            if (null != procesorItem)
                            {
                                procesorItem.Stop();
                                procesorItem.Dispose();//释放处理器对象
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex);
                        }

                    }

                    //停止线程
                    //------------停止采集线程-------------
                    while (!_TheadQueuePool_FetchMessage.IsEmpty)
                    {
                        Thread th = null;
                        _TheadQueuePool_FetchMessage.TryDequeue(out th);
                        //if (null != th && th.ThreadState != ThreadState.Stopped)
                        //{
                        //    try
                        //    {
                        //       // th.Abort();------注意：.net core 不支持 Abort方法
                        //      System.Threading.Thread.su
                        //    }
                        //    catch { }
                        //}
                    }

                    //----------停止处理消息线程---------------
                    while (!_TheadQueuePool_Processor.IsEmpty)
                    {
                        Thread th = null;
                        _TheadQueuePool_Processor.TryDequeue(out th);
                    }




                    //var enumtor_processor = _TheadQueuePool_Processor.GetEnumerator();
                    //while (enumtor_processor.MoveNext())
                    //{
                    //    var th = enumtor_processor.Current;
                    //    //if (null != th && th.ThreadState != ThreadState.Stopped)
                    //    //{
                    //    //    try
                    //    //    {
                    //    //        th.Abort();
                    //    //    }
                    //    //    catch { }

                    //    //}
                    //}




                }
            }
            catch (Exception ex)
            {

                Logger.Error(ex);
            }
            finally
            {
             
                _cts.Dispose();
            }
        }
    }
}
