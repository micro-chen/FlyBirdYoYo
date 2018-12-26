using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkContentGetResponse.
    /// </summary>
    public class TbkContentGetResponse : TopResponse
    {
        /// <summary>
        /// result
        /// </summary>
        [XmlElement("result")]
        public RpcResultDomain Result { get; set; }

	/// <summary>
/// MapDataDomain Data Structure.
/// </summary>
[Serializable]

public class MapDataDomain : TopObject
{
	        /// <summary>
	        /// 达人头像 URL
	        /// </summary>
	        [XmlElement("author_avatar")]
	        public string AuthorAvatar { get; set; }
	
	        /// <summary>
	        /// 达人ID
	        /// </summary>
	        [XmlElement("author_id")]
	        public string AuthorId { get; set; }
	
	        /// <summary>
	        /// 达人昵称
	        /// </summary>
	        [XmlElement("author_nick")]
	        public string AuthorNick { get; set; }
	
	        /// <summary>
	        /// 文章链接
	        /// </summary>
	        [XmlElement("clink")]
	        public string Clink { get; set; }
	
	        /// <summary>
	        /// 内容分类，多个分类用英文逗号分隔
	        /// </summary>
	        [XmlElement("content_categories")]
	        public string ContentCategories { get; set; }
	
	        /// <summary>
	        /// 内容ID
	        /// </summary>
	        [XmlElement("content_id")]
	        public long ContentId { get; set; }
	
	        /// <summary>
	        /// 封面图，多张图用英文逗号分隔
	        /// </summary>
	        [XmlArray("images")]
	        [XmlArrayItem("string")]
	        public List<string> Images { get; set; }
	
	        /// <summary>
	        /// 宝贝id列表
	        /// </summary>
	        [XmlArray("item_ids")]
	        [XmlArrayItem("number")]
	        public List<long> ItemIds { get; set; }
	
	        /// <summary>
	        /// 大促标签
	        /// </summary>
	        [XmlElement("promotion_tag")]
	        public string PromotionTag { get; set; }
	
	        /// <summary>
	        /// 内容发布时间
	        /// </summary>
	        [XmlElement("publish_time")]
	        public string PublishTime { get; set; }
	
	        /// <summary>
	        /// 质量分。质量分越高代表内容质量和收益越好
	        /// </summary>
	        [XmlElement("score")]
	        public long Score { get; set; }
	
	        /// <summary>
	        /// 【未开放】内容摘要
	        /// </summary>
	        [XmlElement("summary")]
	        public string Summary { get; set; }
	
	        /// <summary>
	        /// 【未开放】内容标签，多个标签用英文逗号分隔
	        /// </summary>
	        [XmlElement("tags")]
	        public string Tags { get; set; }
	
	        /// <summary>
	        /// 内容的标题
	        /// </summary>
	        [XmlElement("title")]
	        public string Title { get; set; }
	
	        /// <summary>
	        /// 展示样式，内容类型：1.图文、2.图集
	        /// </summary>
	        [XmlElement("type")]
	        public string Type { get; set; }
	
	        /// <summary>
	        /// 本期仅支持3。展示样式，0:无图样式 1:单图样式 3:三图样式 4:大图样式
	        /// </summary>
	        [XmlElement("ui_style")]
	        public string UiStyle { get; set; }
	
	        /// <summary>
	        /// 内容更新时间
	        /// </summary>
	        [XmlElement("update_time")]
	        public string UpdateTime { get; set; }
}

	/// <summary>
/// RpcResultDomain Data Structure.
/// </summary>
[Serializable]

public class RpcResultDomain : TopObject
{
	        /// <summary>
	        /// data
	        /// </summary>
	        [XmlElement("data")]
	        public MapDataDomain Data { get; set; }
}

    }
}
