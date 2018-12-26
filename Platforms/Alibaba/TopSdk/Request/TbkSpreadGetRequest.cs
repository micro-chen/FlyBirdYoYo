using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.tbk.spread.get
    /// </summary>
    public class TbkSpreadGetRequest : BaseTopRequest<Top.Api.Response.TbkSpreadGetResponse>
    {
        /// <summary>
        /// 请求列表，内部包含多个url
        /// </summary>
        public string Requests { get; set; }

        public List<TbkSpreadRequestDomain> Requests_ { set { this.Requests = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.tbk.spread.get";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("requests", this.Requests);
            if (this.otherParams != null)
            {
                parameters.AddAll(this.otherParams);
            }
            return parameters;
        }

        public override void Validate()
        {
            RequestValidator.ValidateRequired("requests", this.Requests);
            RequestValidator.ValidateObjectMaxListSize("requests", this.Requests, 20);
        }

	/// <summary>
/// TbkSpreadRequestDomain Data Structure.
/// </summary>
[Serializable]

public class TbkSpreadRequestDomain : TopObject
{
	        /// <summary>
	        /// 原始url, 只支持uland.taobao.com，s.click.taobao.com， ai.taobao.com，temai.taobao.com的域名转换，否则判错
	        /// </summary>
	        [XmlElement("url")]
	        public string Url { get; set; }
}

        #endregion
    }
}
