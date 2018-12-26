using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkCouponGetResponse.
    /// </summary>
    public class TbkCouponGetResponse : TopResponse
    {
        /// <summary>
        /// data
        /// </summary>
        [XmlElement("data")]
        public MapDataDomain Data { get; set; }

	/// <summary>
/// MapDataDomain Data Structure.
/// </summary>
[Serializable]

public class MapDataDomain : TopObject
{
	        /// <summary>
	        /// 券ID
	        /// </summary>
	        [XmlElement("coupon_activity_id")]
	        public string CouponActivityId { get; set; }
	
	        /// <summary>
	        /// 优惠券金额
	        /// </summary>
	        [XmlElement("coupon_amount")]
	        public string CouponAmount { get; set; }
	
	        /// <summary>
	        /// 优惠券结束时间
	        /// </summary>
	        [XmlElement("coupon_end_time")]
	        public string CouponEndTime { get; set; }
	
	        /// <summary>
	        /// 优惠券剩余量
	        /// </summary>
	        [XmlElement("coupon_remain_count")]
	        public long CouponRemainCount { get; set; }
	
	        /// <summary>
	        /// 券类型，1 表示全网公开券，4 表示妈妈渠道券
	        /// </summary>
	        [XmlElement("coupon_src_scene")]
	        public long CouponSrcScene { get; set; }
	
	        /// <summary>
	        /// 优惠券门槛金额
	        /// </summary>
	        [XmlElement("coupon_start_fee")]
	        public string CouponStartFee { get; set; }
	
	        /// <summary>
	        /// 优惠券开始时间
	        /// </summary>
	        [XmlElement("coupon_start_time")]
	        public string CouponStartTime { get; set; }
	
	        /// <summary>
	        /// 优惠券总量
	        /// </summary>
	        [XmlElement("coupon_total_count")]
	        public long CouponTotalCount { get; set; }
	
	        /// <summary>
	        /// 券属性，0表示店铺券，1表示单品券
	        /// </summary>
	        [XmlElement("coupon_type")]
	        public long CouponType { get; set; }
}

    }
}
