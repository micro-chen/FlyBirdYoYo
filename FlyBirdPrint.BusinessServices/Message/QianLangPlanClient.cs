using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FlyBirdYoYo.DomainEntity.Message;
using FlyBirdYoYo.DomainEntity.ViewModel;
using FlyBirdYoYo.Utilities.Caching;
using FlyBirdYoYo.Utilities.Interface;


namespace FlyBirdYoYo.BusinessServices.Message
{
    /// <summary>
    /// 千浪计划消息client
    /// </summary>
    public class QianLangPlanClient
    {
        /// <summary>
        /// 消息过期的存活时间 2*24小时（对应的秒值）
        /// 超过2*24小时的就丢弃过期
        /// </summary>
        private const int Message_Expire_Time = 60 * 60 * 24*2;

        /// <summary>
        /// 单例模式
        /// </summary>
        public static QianLangPlanClient Instance
        {
            get
            {
                QianLangPlanClient _instance = Singleton<QianLangPlanClient>.Instance;
                if (null == _instance)
                {
                    throw new Exception("创建 QianLangPlanClient单例失败！");
                    
                }
                return _instance;
            }
        }


        /// <summary>
        /// 消息存储的db
        /// </summary>
        private const int Message_In_WhichDb = 0;

        private RedisCacheManager _redisClient;


        public QianLangPlanClient()
        {
            this._redisClient = new RedisCacheManager();
        }



        /// <summary>
        /// 同步等待的发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SendMessage(QianLangQuanMessage model)
        {
            var result = SendMessageAsync(model)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return result;
        }
        /// <summary>
        ///异步发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<bool> SendMessageAsync(QianLangQuanMessage model)
        {
            return Task.Factory.StartNew(() =>
            {

                bool result = false;
                if (null == model)
                {
                    return result;
                }
                try
                {
                    //首先验证模型
                    var isValid = model.ValidModel();
                    if (!isValid)
                    {
                        throw new Exception("消息模型非法！");
                    }
                    string key = model.GetModelKey();
                    //使用reids 发送消息
                    _redisClient.Set(key, model,Message_Expire_Time);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return result;
            });

        }


    }

}
