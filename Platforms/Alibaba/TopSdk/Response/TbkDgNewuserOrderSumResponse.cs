using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkDgNewuserOrderSumResponse.
    /// </summary>
    public class TbkDgNewuserOrderSumResponse : TopResponse
    {
        /// <summary>
        /// data
        /// </summary>
        [XmlElement("results")]
        public DataDomain Results { get; set; }

	/// <summary>
/// MapDomain Data Structure.
/// </summary>
[Serializable]

public class MapDomain : TopObject
{
	        /// <summary>
	        /// 活动ID
	        /// </summary>
	        [XmlElement("activity_id")]
	        public string ActivityId { get; set; }
	
	        /// <summary>
	        /// 首购用户数
	        /// </summary>
	        [XmlElement("alipay_user_cnt")]
	        public long AlipayUserCnt { get; set; }
	
	        /// <summary>
	        /// 结算CPA 奖励金额：仅支持member 维度的统计
	        /// </summary>
	        [XmlElement("alipay_user_cpa_pre_amt")]
	        public string AlipayUserCpaPreAmt { get; set; }
	
	        /// <summary>
	        /// 当日激活且首购结算的CPA 金额：仅适用于八天乐，仅支持member维度的统计
	        /// </summary>
	        [XmlElement("bind_buy_user_cpa_pre_amt")]
	        public string BindBuyUserCpaPreAmt { get; set; }
	
	        /// <summary>
	        /// 当日激活且首购的有效用户数：仅适用于八天乐，支持member，adzone维度的统计
	        /// </summary>
	        [XmlElement("bind_buy_valid_user_cnt")]
	        public long BindBuyValidUserCnt { get; set; }
	
	        /// <summary>
	        /// 日期
	        /// </summary>
	        [XmlElement("biz_date")]
	        public string BizDate { get; set; }
	
	        /// <summary>
	        /// 新激活用户数
	        /// </summary>
	        [XmlElement("login_user_cnt")]
	        public long LoginUserCnt { get; set; }
	
	        /// <summary>
	        /// 确认收货用户数
	        /// </summary>
	        [XmlElement("rcv_user_cnt")]
	        public long RcvUserCnt { get; set; }
	
	        /// <summary>
	        /// 结算有效用户数
	        /// </summary>
	        [XmlElement("rcv_valid_user_cnt")]
	        public long RcvValidUserCnt { get; set; }
	
	        /// <summary>
	        /// 新注册用户数
	        /// </summary>
	        [XmlElement("reg_user_cnt")]
	        public long RegUserCnt { get; set; }
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
	        [XmlArrayItem("map")]
	        public List<MapDomain> Results { get; set; }
}

    }
}
