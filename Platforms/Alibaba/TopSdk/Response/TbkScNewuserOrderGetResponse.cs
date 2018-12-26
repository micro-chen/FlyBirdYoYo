using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkScNewuserOrderGetResponse.
    /// </summary>
    public class TbkScNewuserOrderGetResponse : TopResponse
    {
        /// <summary>
        /// data
        /// </summary>
        [XmlElement("results")]
        public ResultsDomain Results { get; set; }

	/// <summary>
/// OrderDataDomain Data Structure.
/// </summary>
[Serializable]

public class OrderDataDomain : TopObject
{
	        /// <summary>
	        /// 预估佣金
	        /// </summary>
	        [XmlElement("commission")]
	        public string Commission { get; set; }
	
	        /// <summary>
	        /// 收货时间
	        /// </summary>
	        [XmlElement("confirm_receive_time")]
	        public string ConfirmReceiveTime { get; set; }
	
	        /// <summary>
	        /// 订单号
	        /// </summary>
	        [XmlElement("order_no")]
	        public string OrderNo { get; set; }
	
	        /// <summary>
	        /// 支付时间
	        /// </summary>
	        [XmlElement("pay_time")]
	        public string PayTime { get; set; }
}

	/// <summary>
/// MapDataDomain Data Structure.
/// </summary>
[Serializable]

public class MapDataDomain : TopObject
{
	        /// <summary>
	        /// 确认收货时间
	        /// </summary>
	        [XmlElement("accept_time")]
	        public string AcceptTime { get; set; }
	
	        /// <summary>
	        /// 活动id
	        /// </summary>
	        [XmlElement("activity_id")]
	        public string ActivityId { get; set; }
	
	        /// <summary>
	        /// 活动类型，taobao-淘宝 alipay-支付宝 tmall-天猫
	        /// </summary>
	        [XmlElement("activity_type")]
	        public string ActivityType { get; set; }
	
	        /// <summary>
	        /// 来源广告位ID(pid中mm_1_2_3)中第3位
	        /// </summary>
	        [XmlElement("adzone_id")]
	        public long AdzoneId { get; set; }
	
	        /// <summary>
	        /// 来源广告位名称
	        /// </summary>
	        [XmlElement("adzone_name")]
	        public string AdzoneName { get; set; }
	
	        /// <summary>
	        /// 绑卡日期，仅适用于手淘拉新
	        /// </summary>
	        [XmlElement("bind_card_time")]
	        public string BindCardTime { get; set; }
	
	        /// <summary>
	        /// 当前活动为淘宝拉新活动时，bind_time为新激活时间； 当前活动为支付宝拉新活动时，bind_time为绑定时间。
	        /// </summary>
	        [XmlElement("bind_time")]
	        public string BindTime { get; set; }
	
	        /// <summary>
	        /// 日期，格式为"20180202"
	        /// </summary>
	        [XmlElement("biz_date")]
	        public string BizDate { get; set; }
	
	        /// <summary>
	        /// 首购时间
	        /// </summary>
	        [XmlElement("buy_time")]
	        public string BuyTime { get; set; }
	
	        /// <summary>
	        /// 来源媒体ID(pid中mm_1_2_3)中第1位
	        /// </summary>
	        [XmlElement("member_id")]
	        public long MemberId { get; set; }
	
	        /// <summary>
	        /// 来源媒体名称
	        /// </summary>
	        [XmlElement("member_nick")]
	        public string MemberNick { get; set; }
	
	        /// <summary>
	        /// 新人手机号
	        /// </summary>
	        [XmlElement("mobile")]
	        public string Mobile { get; set; }
	
	        /// <summary>
	        /// 订单淘客类型:1.淘客订单；2.非淘客订单
	        /// </summary>
	        [XmlElement("order_tk_type")]
	        public long OrderTkType { get; set; }
	
	        /// <summary>
	        /// 复购订单，仅适用于手淘拉新
	        /// </summary>
	        [XmlArray("orders")]
	        [XmlArrayItem("order_data")]
	        public List<OrderDataDomain> Orders { get; set; }
	
	        /// <summary>
	        /// 领取红包时间
	        /// </summary>
	        [XmlElement("receive_time")]
	        public string ReceiveTime { get; set; }
	
	        /// <summary>
	        /// 新注册时间
	        /// </summary>
	        [XmlElement("register_time")]
	        public string RegisterTime { get; set; }
	
	        /// <summary>
	        /// 来源站点ID(pid中mm_1_2_3)中第2位
	        /// </summary>
	        [XmlElement("site_id")]
	        public long SiteId { get; set; }
	
	        /// <summary>
	        /// 来源站点名称
	        /// </summary>
	        [XmlElement("site_name")]
	        public string SiteName { get; set; }
	
	        /// <summary>
	        /// 新人状态， 当前活动为淘宝拉新活动时，1: 新注册，2:激活，3:首购，4:确认收货； 当前活动为支付宝拉新活动时，1: 已绑定，2:拉新成功，3:无效用户。 当前活动为天猫拉新活动时，2:已领取，3:已首购，4:已收货
	        /// </summary>
	        [XmlElement("status")]
	        public long Status { get; set; }
	
	        /// <summary>
	        /// 拉新成功时间
	        /// </summary>
	        [XmlElement("success_time")]
	        public string SuccessTime { get; set; }
	
	        /// <summary>
	        /// 淘宝订单id
	        /// </summary>
	        [XmlElement("tb_trade_parent_id")]
	        public long TbTradeParentId { get; set; }
	
	        /// <summary>
	        /// 分享用户(unionid)
	        /// </summary>
	        [XmlElement("union_id")]
	        public string UnionId { get; set; }
}

	/// <summary>
/// DataDomain Data Structure.
/// </summary>
[Serializable]

public class DataDomain : TopObject
{
	        /// <summary>
	        /// 是否有下一页
	        /// </summary>
	        [XmlElement("has_next")]
	        public bool HasNext { get; set; }
	
	        /// <summary>
	        /// 页码
	        /// </summary>
	        [XmlElement("page_no")]
	        public long PageNo { get; set; }
	
	        /// <summary>
	        /// 每页大小
	        /// </summary>
	        [XmlElement("page_size")]
	        public long PageSize { get; set; }
	
	        /// <summary>
	        /// resultList
	        /// </summary>
	        [XmlArray("results")]
	        [XmlArrayItem("map_data")]
	        public List<MapDataDomain> Results { get; set; }
}

	/// <summary>
/// ResultsDomain Data Structure.
/// </summary>
[Serializable]

public class ResultsDomain : TopObject
{
	        /// <summary>
	        /// data
	        /// </summary>
	        [XmlElement("data")]
	        public DataDomain Data { get; set; }
}

    }
}
