using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkDgOptimusMaterialResponse.
    /// </summary>
    public class TbkDgOptimusMaterialResponse : TopResponse
    {
        /// <summary>
        /// resultList
        /// </summary>
        [XmlArray("result_list")]
        [XmlArrayItem("map_data")]
        public List<MapDataDomain> ResultList { get; set; }

	/// <summary>
/// MapDataDomain Data Structure.
/// </summary>
[Serializable]

public class MapDataDomain : TopObject
{
	        /// <summary>
	        /// 类目id
	        /// </summary>
	        [XmlElement("category_id")]
	        public long CategoryId { get; set; }
	
	        /// <summary>
	        /// 单品淘客链接
	        /// </summary>
	        [XmlElement("click_url")]
	        public string ClickUrl { get; set; }
	
	        /// <summary>
	        /// 佣金比率(%)
	        /// </summary>
	        [XmlElement("commission_rate")]
	        public string CommissionRate { get; set; }
	
	        /// <summary>
	        /// 券面额
	        /// </summary>
	        [XmlElement("coupon_amount")]
	        public long CouponAmount { get; set; }
	
	        /// <summary>
	        /// 优惠券链接
	        /// </summary>
	        [XmlElement("coupon_click_url")]
	        public string CouponClickUrl { get; set; }
	
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
	        /// 优惠券起用门槛，满X元可用
	        /// </summary>
	        [XmlElement("coupon_start_fee")]
	        public string CouponStartFee { get; set; }
	
	        /// <summary>
	        /// 优惠券开始时间
	        /// </summary>
	        [XmlElement("coupon_start_time")]
	        public string CouponStartTime { get; set; }
	
	        /// <summary>
	        /// 券总量
	        /// </summary>
	        [XmlElement("coupon_total_count")]
	        public long CouponTotalCount { get; set; }
	
	        /// <summary>
	        /// 宝贝描述（推荐理由,不一定有）
	        /// </summary>
	        [XmlElement("item_description")]
	        public string ItemDescription { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：几人团
	        /// </summary>
	        [XmlElement("jdd_num")]
	        public long JddNum { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：拼成价，单位元
	        /// </summary>
	        [XmlElement("jdd_price")]
	        public string JddPrice { get; set; }
	
	        /// <summary>
	        /// 宝贝id
	        /// </summary>
	        [XmlElement("num_iid")]
	        public long NumIid { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：结束时间
	        /// </summary>
	        [XmlElement("oetime")]
	        public string Oetime { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：一人价（原价)，单位元
	        /// </summary>
	        [XmlElement("orig_price")]
	        public string OrigPrice { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：开始时间
	        /// </summary>
	        [XmlElement("ostime")]
	        public string Ostime { get; set; }
	
	        /// <summary>
	        /// 商品主图
	        /// </summary>
	        [XmlElement("pict_url")]
	        public string PictUrl { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：已售数量
	        /// </summary>
	        [XmlElement("sell_num")]
	        public long SellNum { get; set; }
	
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
	        /// 聚划算拼团：剩余库存
	        /// </summary>
	        [XmlElement("stock")]
	        public long Stock { get; set; }
	
	        /// <summary>
	        /// 商品标题
	        /// </summary>
	        [XmlElement("title")]
	        public string Title { get; set; }
	
	        /// <summary>
	        /// 聚划算拼团：库存数量
	        /// </summary>
	        [XmlElement("total_stock")]
	        public long TotalStock { get; set; }
	
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
