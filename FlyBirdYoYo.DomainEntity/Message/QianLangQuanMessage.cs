using System;
using System.Collections.Generic;
using System.Text;


namespace FlyBirdYoYo.DomainEntity.Message
{
    /// <summary>
    /// 千浪计划-优惠券奖励提成分割消息模型
    /// 领券优惠券后进行的获利收税消息模型
    /// </summary>
    public class QianLangQuanMessage : BaseMessageModel
    {
        public QianLangQuanMessage()
        {
            this.PipeLine = ProcessPipeLine.QianLangPlanQuan;
        }


        ///// <summary>
        ///// 微信用户-消息的发起者
        ///// </summary>
        //public WxUsersModel WxUser { get; set; }

        ///// <summary>
        ///// 对应的获取的优惠券
        ///// </summary>
        //public UserCouponsModel UserQuan { get; set; }

        /// <summary>
        /// 获取缓存键
        /// </summary>
        /// <returns></returns>
        public override string GetModelKey()
        {
            //if (null == this.WxUser || null == this.UserQuan)
            //{
            //    throw new Exception("不能生成缓存key,未知的用户或者口令券！");
            //}
            //var paras = new string[]
            //{
            //   BaseMessageModel.CachePrefix_QianLang,this.PipeLine.ToString(), WxUser.Id.ToString(),UserQuan.Id.ToString(),this.TagIdentity
            //};
            //return string.Join(":", paras);

            return "";
        }
    }
}
