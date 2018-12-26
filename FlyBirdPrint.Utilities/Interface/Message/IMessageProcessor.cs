using System;

namespace FlyBirdYoYo.Utilities.Interface
{
    /// <summary>
    /// 消息处理组件接口
    /// </summary>
    public interface IMessageProcessor : IDisposable
    {
        void Process(string key, IMessage msg);
        void Stop();
    }
}
