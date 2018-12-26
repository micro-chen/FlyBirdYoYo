using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// NTbkShop Data Structure.
    /// </summary>
    [Serializable]
    public class NTbkShop : TopObject
    {
        /// <summary>
        /// 淘客地址
        /// </summary>
        [XmlElement("click_url")]
        public string ClickUrl { get; set; }

        /// <summary>
        /// 店标图片
        /// </summary>
        [XmlElement("pict_url")]
        public string PictUrl { get; set; }

        /// <summary>
        /// 卖家昵称
        /// </summary>
        [XmlElement("seller_nick")]
        public string SellerNick { get; set; }

        /// <summary>
        /// 店铺名称
        /// </summary>
        [XmlElement("shop_title")]
        public string ShopTitle { get; set; }

        /// <summary>
        /// 店铺类型，B：天猫，C：淘宝
        /// </summary>
        [XmlElement("shop_type")]
        public string ShopType { get; set; }

        /// <summary>
        /// 店铺地址
        /// </summary>
        [XmlElement("shop_url")]
        public string ShopUrl { get; set; }

        /// <summary>
        /// 卖家ID
        /// </summary>
        [XmlElement("user_id")]
        public long UserId { get; set; }
    }
}
