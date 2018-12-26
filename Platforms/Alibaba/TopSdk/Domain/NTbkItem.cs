using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Domain
{
    /// <summary>
    /// NTbkItem Data Structure.
    /// </summary>
    [Serializable]
    public class NTbkItem : TopObject
    {
        /// <summary>
        /// 淘客地址
        /// </summary>
        [XmlElement("click_url")]
        public string ClickUrl { get; set; }

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
        /// 商品标题
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("tk_rate")]
        public string TkRate { get; set; }

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
        /// 
        /// </summary>
        [XmlElement("zk_final_price_wap")]
        public string ZkFinalPriceWap { get; set; }
    }
}
