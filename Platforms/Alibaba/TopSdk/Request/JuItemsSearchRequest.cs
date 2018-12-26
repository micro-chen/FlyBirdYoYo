using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.ju.items.search
    /// </summary>
    public class JuItemsSearchRequest : BaseTopRequest<Top.Api.Response.JuItemsSearchResponse>
    {
        /// <summary>
        /// query
        /// </summary>
        public string ParamTopItemQuery { get; set; }

        public TopItemQueryDomain ParamTopItemQuery_ { set { this.ParamTopItemQuery = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.ju.items.search";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("param_top_item_query", this.ParamTopItemQuery);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
        }

	/// <summary>
/// TopItemQueryDomain Data Structure.
/// </summary>
[Serializable]

public class TopItemQueryDomain : TopObject
{
	        /// <summary>
	        /// 页码,必传
	        /// </summary>
	        [XmlElement("current_page")]
	        public Nullable<long> CurrentPage { get; set; }
	
	        /// <summary>
	        /// 一页大小,必传
	        /// </summary>
	        [XmlElement("page_size")]
	        public Nullable<long> PageSize { get; set; }
	
	        /// <summary>
	        /// 媒体pid,必传
	        /// </summary>
	        [XmlElement("pid")]
	        public string Pid { get; set; }
	
	        /// <summary>
	        /// 是否包邮,可不传
	        /// </summary>
	        [XmlElement("postage")]
	        public Nullable<bool> Postage { get; set; }
	
	        /// <summary>
	        /// 状态，预热：1，正在进行中：2,可不传
	        /// </summary>
	        [XmlElement("status")]
	        public Nullable<long> Status { get; set; }
	
	        /// <summary>
	        /// 淘宝类目id,可不传
	        /// </summary>
	        [XmlElement("taobao_category_id")]
	        public Nullable<long> TaobaoCategoryId { get; set; }
	
	        /// <summary>
	        /// 搜索关键词,可不传
	        /// </summary>
	        [XmlElement("word")]
	        public string Word { get; set; }
}

        #endregion
    }
}
