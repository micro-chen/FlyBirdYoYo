using System;
using System.Collections.Generic;
using System.Text;
using FlyBirdYoYo.Utilities.Interface;

namespace FlyBirdYoYo.DomainEntity.Message
{
    /// <summary>
    /// 消息处理管道类型
    /// </summary>
    public enum ProcessPipeLine
    {
        /// <summary>
        /// 默认（无）
        /// </summary>
        None,
        /// <summary>
        /// 千浪计划-优惠券类型
        /// </summary>
        QianLangPlanQuan,
    }

    public abstract class BaseMessageModel : IMessage
    {
        /// <summary>
        /// 缓存的前缀
        /// </summary>
        public const string CachePrefix = "HuidangMessage:";

        /// <summary>
        /// 千浪计划消息缓存前缀
        /// </summary>
        public const string CachePrefix_QianLang = CachePrefix + "QianLang";


        public BaseMessageModel()
        {
            this._CreateTime = DateTime.Now;
            this._TagIdentity = Guid.NewGuid().ToString().ToLower();
        }


        /// <summary>
        /// 管道类型
        /// </summary>
        public ProcessPipeLine PipeLine { get; set; }




        #region 实体系统字段


        private string _TagIdentity;

        /// <summary>
        /// 消息的唯一标识
        /// </summary>
        public string TagIdentity
        {
            get
            {
                return this._TagIdentity;
            }
            set
            {
                this._TagIdentity = value;
            }
        }


        private DateTime _CreateTime;
        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return this._CreateTime;
            }
            set
            {
                this._CreateTime = value;
            }
        }

        /// <summary>
        /// 验证模型
        /// </summary>
        /// <returns></returns>
        public virtual bool ValidModel()
        {
            return true;
        }

        /// <summary>
        /// 获取模型的缓存键
        /// </summary>
        /// <returns></returns>
        public abstract string GetModelKey();

        #endregion
    }

    /// <summary>
    /// 错误信息类
    /// </summary>
    public class ErrorMessageModel
    {
        /// <summary>
        /// 是否有错误，当查询的所有店铺都有错误时，为false，其他为true
        /// </summary>
        public bool IsError { get; set; } = false;

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
