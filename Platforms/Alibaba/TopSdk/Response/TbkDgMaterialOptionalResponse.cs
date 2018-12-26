using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkDgMaterialOptionalResponse.
    /// </summary>
    public class TbkDgMaterialOptionalResponse : TopResponse
    {
        /// <summary>
        /// resultList
        /// </summary>
        [XmlArray("result_list")]
        [XmlArrayItem("map_data")]
        public List<MapDataDomain> ResultList { get; set; }

        /// <summary>
        /// 搜索到符合条件的结果总数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

	/// <summary>
/// MapDataDomain Data Structure.
/// </summary>
[Serializable]

public class MapDataDomain : TopObject
{
	        /// <summary>
	        /// 佣金比率
	        /// </summary>
	        [XmlElement("commission_rate")]
	        public string CommissionRate { get; set; }
	
	        /// <summary>
	        /// 佣金类型
	        /// </summary>
	        [XmlElement("commission_type")]
	        public string CommissionType { get; set; }
	
	        /// <summary>
	        /// 优惠券结束时间
	        /// </summary>
	        [XmlElement("coupon_end_time")]
	        public string CouponEndTime { get; set; }
	
	        /// <summary>
	        /// 优惠券id
	        /// </summary>
	        [XmlElement("coupon_id")]
	        public string CouponId { get; set; }
	
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
	        /// 券二合一页面链接
	        /// </summary>
	        [XmlElement("coupon_share_url")]
	        public string CouponShareUrl { get; set; }
	
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
	        /// 是否包含定向计划
	        /// </summary>
	        [XmlElement("include_dxjh")]
	        public string IncludeDxjh { get; set; }
	
	        /// <summary>
	        /// 是否包含营销计划
	        /// </summary>
	        [XmlElement("include_mkt")]
	        public string IncludeMkt { get; set; }
	
	        /// <summary>
	        /// 定向计划信息
	        /// </summary>
	        [XmlElement("info_dxjh")]
	        public string InfoDxjh { get; set; }
	
	        /// <summary>
	        /// 商品地址
	        /// </summary>
	        [XmlElement("item_url")]
	        public string ItemUrl { get; set; }
	
	        /// <summary>
	        /// 宝贝id
	        /// </summary>
	        [XmlElement("num_iid")]
	        public long NumIid { get; set; }
	
	        /// <summary>
	        /// 商品主图
	        /// </summary>
	        [XmlElement("pict_url")]
	        public string PictUrl { get; set; }
	
	        /// <summary>
	        /// 宝贝所在地
	        /// </summary>
	        [XmlElement("provcity")]
	        public string Provcity { get; set; }
	
	        /// <summary>
	        /// 商品一口价格
	        /// </summary>
	        [XmlElement("reserve_price")]
	        public string ReservePrice { get; set; }
	
	        /// <summary>
	        /// 卖家id
	        /// </summary>
	        [XmlElement("seller_id")]
	        public long SellerId { get; set; }
	
	        /// <summary>
	        /// 店铺dsr评分
	        /// </summary>
	        [XmlElement("shop_dsr")]
	        public long ShopDsr { get; set; }
	
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
	        /// 月支出佣金
	        /// </summary>
	        [XmlElement("tk_total_commi")]
	        public string TkTotalCommi { get; set; }
	
	        /// <summary>
	        /// 淘客30天月推广量
	        /// </summary>
	        [XmlElement("tk_total_sales")]
	        public string TkTotalSales { get; set; }
	
	        /// <summary>
	        /// 商品淘客链接
	        /// </summary>
	        [XmlElement("url")]
	        public string Url { get; set; }
	
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
	        /// 商品折扣价格
	        /// </summary>
	        [XmlElement("zk_final_price")]
	        public string ZkFinalPrice { get; set; }
}

    }
}
