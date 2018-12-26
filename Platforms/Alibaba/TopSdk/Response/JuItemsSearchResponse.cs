using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// JuItemsSearchResponse.
    /// </summary>
    public class JuItemsSearchResponse : TopResponse
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        [XmlElement("result")]
        public PaginationResultDomain Result { get; set; }

	/// <summary>
/// ExtendDomain Data Structure.
/// </summary>
[Serializable]

public class ExtendDomain : TopObject
{
	        /// <summary>
	        /// empty
	        /// </summary>
	        [XmlElement("empty")]
	        public bool Empty { get; set; }
}

	/// <summary>
/// ItemsDomain Data Structure.
/// </summary>
[Serializable]

public class ItemsDomain : TopObject
{
	        /// <summary>
	        /// 聚划算价格，单位分
	        /// </summary>
	        [XmlElement("act_price")]
	        public string ActPrice { get; set; }
	
	        /// <summary>
	        /// 类目名称
	        /// </summary>
	        [XmlElement("category_name")]
	        public string CategoryName { get; set; }
	
	        /// <summary>
	        /// itemId
	        /// </summary>
	        [XmlElement("item_id")]
	        public long ItemId { get; set; }
	
	        /// <summary>
	        /// 商品卖点
	        /// </summary>
	        [XmlArray("item_usp_list")]
	        [XmlArrayItem("string")]
	        public List<string> ItemUspList { get; set; }
	
	        /// <summary>
	        /// 聚划算id
	        /// </summary>
	        [XmlElement("ju_id")]
	        public long JuId { get; set; }
	
	        /// <summary>
	        /// 开团结束时间
	        /// </summary>
	        [XmlElement("online_end_time")]
	        public long OnlineEndTime { get; set; }
	
	        /// <summary>
	        /// 开团时间
	        /// </summary>
	        [XmlElement("online_start_time")]
	        public long OnlineStartTime { get; set; }
	
	        /// <summary>
	        /// 原价
	        /// </summary>
	        [XmlElement("orig_price")]
	        public string OrigPrice { get; set; }
	
	        /// <summary>
	        /// 是否包邮
	        /// </summary>
	        [XmlElement("pay_postage")]
	        public bool PayPostage { get; set; }
	
	        /// <summary>
	        /// pc链接
	        /// </summary>
	        [XmlElement("pc_url")]
	        public string PcUrl { get; set; }
	
	        /// <summary>
	        /// pc主图
	        /// </summary>
	        [XmlElement("pic_url_for_p_c")]
	        public string PicUrlForPC { get; set; }
	
	        /// <summary>
	        /// 无线主图
	        /// </summary>
	        [XmlElement("pic_url_for_w_l")]
	        public string PicUrlForWL { get; set; }
	
	        /// <summary>
	        /// 频道id
	        /// </summary>
	        [XmlElement("platform_id")]
	        public long PlatformId { get; set; }
	
	        /// <summary>
	        /// 价格卖点
	        /// </summary>
	        [XmlArray("price_usp_list")]
	        [XmlArrayItem("string")]
	        public List<string> PriceUspList { get; set; }
	
	        /// <summary>
	        /// 展示结束时间
	        /// </summary>
	        [XmlElement("show_end_time")]
	        public long ShowEndTime { get; set; }
	
	        /// <summary>
	        /// 开始展示时间
	        /// </summary>
	        [XmlElement("show_start_time")]
	        public long ShowStartTime { get; set; }
	
	        /// <summary>
	        /// 淘宝类目id
	        /// </summary>
	        [XmlElement("tb_first_cat_id")]
	        public long TbFirstCatId { get; set; }
	
	        /// <summary>
	        /// 商品标题
	        /// </summary>
	        [XmlElement("title")]
	        public string Title { get; set; }
	
	        /// <summary>
	        /// 卖点描述
	        /// </summary>
	        [XmlArray("usp_desc_list")]
	        [XmlArrayItem("string")]
	        public List<string> UspDescList { get; set; }
	
	        /// <summary>
	        /// 无线链接
	        /// </summary>
	        [XmlElement("wap_url")]
	        public string WapUrl { get; set; }
}

	/// <summary>
/// TrackparamsDomain Data Structure.
/// </summary>
[Serializable]

public class TrackparamsDomain : TopObject
{
	        /// <summary>
	        /// empty
	        /// </summary>
	        [XmlElement("empty")]
	        public bool Empty { get; set; }
}

	/// <summary>
/// PaginationResultDomain Data Structure.
/// </summary>
[Serializable]

public class PaginationResultDomain : TopObject
{
	        /// <summary>
	        /// 当前页码
	        /// </summary>
	        [XmlElement("current_page")]
	        public long CurrentPage { get; set; }
	
	        /// <summary>
	        /// 扩展属性
	        /// </summary>
	        [XmlElement("extend")]
	        public ExtendDomain Extend { get; set; }
	
	        /// <summary>
	        /// 商品数据
	        /// </summary>
	        [XmlArray("model_list")]
	        [XmlArrayItem("items")]
	        public List<ItemsDomain> ModelList { get; set; }
	
	        /// <summary>
	        /// 错误码
	        /// </summary>
	        [XmlElement("msg_code")]
	        public string MsgCode { get; set; }
	
	        /// <summary>
	        /// 错误信息
	        /// </summary>
	        [XmlElement("msg_info")]
	        public string MsgInfo { get; set; }
	
	        /// <summary>
	        /// 一页大小
	        /// </summary>
	        [XmlElement("page_size")]
	        public long PageSize { get; set; }
	
	        /// <summary>
	        /// 请求是否成功
	        /// </summary>
	        [XmlElement("success")]
	        public bool Success { get; set; }
	
	        /// <summary>
	        /// 商品总数
	        /// </summary>
	        [XmlElement("total_item")]
	        public long TotalItem { get; set; }
	
	        /// <summary>
	        /// 总页数
	        /// </summary>
	        [XmlElement("total_page")]
	        public long TotalPage { get; set; }
	
	        /// <summary>
	        /// 埋点信息
	        /// </summary>
	        [XmlElement("track_params")]
	        public TrackparamsDomain TrackParams { get; set; }
}

    }
}
