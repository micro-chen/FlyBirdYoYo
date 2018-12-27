using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Newtonsoft.Json;

using FlyBirdYoYo.Utilities;
using FlyBirdYoYo.Utilities.Caching;
using FlyBirdYoYo.Utilities.Interface;
using FlyBirdYoYo.Utilities.Logging;
using FlyBirdYoYo.DomainEntity.Message;

namespace FlyBirdYoYo.BusinessServices.Message
{
    /// <summary>
    /// 千浪计划-优惠券消息处理器
    /// </summary>
    public class QianLangPlanQuanProcessor : BaseProcessor
    {

        /// <summary>
        /// 千浪计划-优惠券-消息处理容器（线程安全集合）
        /// </summary>
        public static ConcurrentDictionary<string, IMessage> MessageDataQueueOfQianLangQuan = new ConcurrentDictionary<string, IMessage>();
        private static object _LockSlimOfQianLangQuan = new object();


        #region 字段

        private RedisCacheManager redisClient = new  RedisCacheManager();

        #endregion

 


        /// <summary>
        /// 弹出一个消息
        /// 多线程并发，防止脏读集合
        /// </summary>
        /// <returns></returns>
        public static KeyValuePair<string, IMessage> PopOneQianLangQuanMessage()
        {
            //进入线程互斥
            lock (_LockSlimOfQianLangQuan)
            {

                if (MessageDataQueueOfQianLangQuan.IsEmpty)
                {
                    return default(KeyValuePair<string, IMessage>);
                }

                var pair = MessageDataQueueOfQianLangQuan.ElementAt(0);
                if (MessageWorkBench.CheckIsInProcessHistory(pair.Key) == false)
                {
                    MessageWorkBench.LocalProcessdHistoryHashTable.Add(pair.Key);//放置到历史集合中

                }
                IMessage tempMsg;
                MessageDataQueueOfQianLangQuan.TryRemove(pair.Key, out tempMsg);

                return pair;
            }
        }

        public override void Process(string key, IMessage msg)
        {
            if (null == msg)
            {
                return;
            }
            if (base._beStop==true)
            {
                return;
            }
            var bizMsg = msg as QianLangQuanMessage;
            if (null == bizMsg || !msg.ValidModel())
            {
                return;
            }
            try
            {

              //todo:在这里处理你的消息对象业务
              

                //处理完毕后，将消息从缓存移除掉
                redisClient.Remove(key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }


        
    }
}
