using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkDgItemCouponGetResponse.
    /// </summary>
    public class TbkDgItemCouponGetResponse : TopResponse
    {
        /// <summary>
        /// TbkCoupon
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("tbk_coupon")]
        public List<TbkCouponDomain> Results { get; set; }

        /// <summary>
        /// 总请求数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

	/// <summary>
/// TbkCouponDomain Data Structure.
/// </summary>
[Serializable]

public class TbkCouponDomain : TopObject
{
	        /// <summary>
	        /// 后台一级类目
	        /// </summary>
	        [XmlElement("category")]
	        public long Category { get; set; }
	
	        /// <summary>
	        /// 佣金比率(%)
	        /// </summary>
	        [XmlElement("commission_rate")]
	        public string CommissionRate { get; set; }
	
	        /// <summary>
	        /// 商品优惠券推广链接
	        /// </summary>
	        [XmlElement("coupon_click_url")]
	        public string CouponClickUrl { get; set; }
	
	        /// <summary>
	        /// 优惠券结束时间
	        /// </summary>
	        [XmlElement("coupon_end_time")]
	        public string CouponEndTime { get; set; }
	
	        /// <summary>
	        /// 优惠券面额
	        /// </summary>
	        [XmlElement("coupon_info")]
	        public string CouponInfo { get; set; }
	
	        /// <summary>
	        /// 优惠券剩余量
	        /// </summary>
	        [XmlElement("coupon_remain_count")]
	        public long CouponRemainCount { get; set; }
	
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
	        /// 宝贝描述（推荐理由）
	        /// </summary>
	        [XmlElement("item_description")]
	        public string ItemDescription { get; set; }
	
	        /// <summary>
	        /// 商品详情页链接地址
	        /// </summary>
	        [XmlElement("item_url")]
	        public string ItemUrl { get; set; }
	
	        /// <summary>
	        /// 卖家昵称
	        /// </summary>
	        [XmlElement("nick")]
	        public string Nick { get; set; }
	
	        /// <summary>
	        /// itemId
	        /// </summary>
	        [XmlElement("num_iid")]
	        public long NumIid { get; set; }
	
	        /// <summary>
	        /// 商品主图
	        /// </summary>
	        [XmlElement("pict_url")]
	        public string PictUrl { get; set; }
	
	        /// <summary>
	        /// 卖家id
	        /// </summary>
	        [XmlElement("seller_id")]
	        public long SellerId { get; set; }
	
	        /// <summary>
	        /// 店铺名称
	        /// </summary>
	        [XmlElement("shop_title")]
	        public string ShopTitle { get; set; }
	
	        /// <summary>
	        /// 商品小图列表
	        /// </summary>
	        [XmlArray("small_images")]
	        [XmlArrayItem("string")]
	        public List<string> SmallImages { get; set; }
	
	        /// <summary>
	        /// 商品标题
	        /// </summary>
	        [XmlElement("title")]
	        public string Title { get; set; }
	
	        /// <summary>
	        /// 卖家类型，0表示集市，1表示商城
	        /// </summary>
	        [XmlElement("user_type")]
	        public long UserType { get; set; }
	
	        /// <summary>
	        /// 30天销量
	        /// </summary>
	        [XmlElement("volume")]
	        public long Volume { get; set; }
	
	        /// <summary>
	        /// 折扣价
	        /// </summary>
	        [XmlElement("zk_final_price")]
	        public string ZkFinalPrice { get; set; }
}

    }
}
