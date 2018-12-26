using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// UatmTbkItem Data Structure.
    /// </summary>
    [Serializable]
    public class UatmTbkItem : TopObject
    {
        /// <summary>
        /// 后台一级类目
        /// </summary>
        [XmlElement("category")]
        public long Category { get; set; }

        /// <summary>
        /// 淘客地址
        /// </summary>
        [XmlElement("click_url")]
        public string ClickUrl { get; set; }

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
        /// 招行活动的结束时间；如果该宝贝取自普通的选品组，则取值为1970-01-01 00:00:00
        /// </summary>
        [XmlElement("event_end_time")]
        public string EventEndTime { get; set; }

        /// <summary>
        /// 招商活动开始时间；如果该宝贝取自普通选品组，则取值为1970-01-01 00:00:00；
        /// </summary>
        [XmlElement("event_start_time")]
        public string EventStartTime { get; set; }

        /// <summary>
        /// 商品地址
        /// </summary>
        [XmlElement("item_url")]
        public string ItemUrl { get; set; }

        /// <summary>
        /// 卖家昵称
        /// </summary>
        [XmlElement("nick")]
        public string Nick { get; set; }

        /// <summary>
        /// 商品ID
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
        /// 
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
        /// 宝贝状态，0失效，1有效；注：失效可能是宝贝已经下线或者是被处罚不能在进行推广
        /// </summary>
        [XmlElement("status")]
        public long Status { get; set; }

        /// <summary>
        /// 商品标题
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 收入比例，举例，取值为20.00，表示比例20.00%
        /// </summary>
        [XmlElement("tk_rate")]
        public string TkRate { get; set; }

        /// <summary>
        /// 宝贝类型：1 普通商品； 2 鹊桥高佣金商品；3 定向招商商品；4 营销计划商品;
        /// </summary>
        [XmlElement("type")]
        public long Type { get; set; }

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

        /// <summary>
        /// 无线折扣价，即宝贝在无线上的实际售卖价格。
        /// </summary>
        [XmlElement("zk_final_price_wap")]
        public string ZkFinalPriceWap { get; set; }
    }
}
