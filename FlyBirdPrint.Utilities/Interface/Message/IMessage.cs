using System;

namespace FlyBirdYoYo.Utilities.Interface
{
    /// <summary>
    /// 消息模型接口
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 消息创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息唯一标识
        /// </summary>
        string TagIdentity { get; set; }

        /// <summary>
        /// 获取该消息的缓存键
        /// </summary>
        /// <returns></returns>
        string GetModelKey();

        /// <summary>
        /// 验证消息模型
        /// </summary>
        /// <returns></returns>
        bool ValidModel();
    }
}
