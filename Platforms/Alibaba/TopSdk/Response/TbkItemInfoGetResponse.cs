using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkItemInfoGetResponse.
    /// </summary>
    public class TbkItemInfoGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝客商品
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("n_tbk_item")]
        public List<NTbkItemDomain> Results { get; set; }

	/// <summary>
/// NTbkItemDomain Data Structure.
/// </summary>
[Serializable]

public class NTbkItemDomain : TopObject
{
	        /// <summary>
	        /// 叶子类目名称
	        /// </summary>
	        [XmlElement("cat_leaf_name")]
	        public string CatLeafName { get; set; }
	
	        /// <summary>
	        /// 一级类目名称
	        /// </summary>
	        [XmlElement("cat_name")]
	        public string CatName { get; set; }
	
	        /// <summary>
	        /// 是否包邮
	        /// </summary>
	        [XmlElement("free_shipment")]
	        public bool FreeShipment { get; set; }
	
	        /// <summary>
	        /// 好评率是否高于行业均值
	        /// </summary>
	        [XmlElement("h_good_rate")]
	        public bool HGoodRate { get; set; }
	
	        /// <summary>
	        /// 成交转化是否高于行业均值
	        /// </summary>
	        [XmlElement("h_pay_rate30")]
	        public bool HPayRate30 { get; set; }
	
	        /// <summary>
	        /// 退款率是否低于行业均值
	        /// </summary>
	        [XmlElement("i_rfd_rate")]
	        public bool IRfdRate { get; set; }
	
	        /// <summary>
	        /// 是否加入消费者保障
	        /// </summary>
	        [XmlElement("is_prepay")]
	        public bool IsPrepay { get; set; }
	
	        /// <summary>
	        /// 商品链接
	        /// </summary>
	        [XmlElement("item_url")]
	        public string ItemUrl { get; set; }
	
	        /// <summary>
	        /// 店铺名称
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
	        /// 商品所在地
	        /// </summary>
	        [XmlElement("provcity")]
	        public string Provcity { get; set; }
	
	        /// <summary>
	        /// 卖家等级
	        /// </summary>
	        [XmlElement("ratesum")]
	        public long Ratesum { get; set; }
	
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
	        /// 店铺dsr 评分
	        /// </summary>
	        [XmlElement("shop_dsr")]
	        public long ShopDsr { get; set; }
	
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
	        /// 商品折扣价格
	        /// </summary>
	        [XmlElement("zk_final_price")]
	        public string ZkFinalPrice { get; set; }
}

    }
}
