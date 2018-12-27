using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.DomainEntity.Message
{
    public abstract class BaseProcessor : IMessageProcessor
    {
        #region 字段
        protected bool _beStop = false;

        private object _locker_getDataFromRedis = new object();
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
         public BaseProcessor()
        {

        }




        /// <summary>
        /// 处理核心
        /// 不同的消息，归通的解析器自行处理实现
        /// </summary>
        /// <param name="key"></param>
        /// <param name="msg"></param>
        public abstract void Process(string key, IMessage msg);
        /// <summary>
        /// 停止处理
        /// </summary>
        public void Stop()
        {
            this._beStop = true;

        }

        #region Dispose


        public void Dispose()
        {
            this.Stop();
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;
        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 清理托管资源
                    _beStop = true;
                }
                // 清理非托管资源

                //让类型知道自己已经被释放
                disposed = true;
            }
        }

        #endregion
    }

}
