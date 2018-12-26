using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Top.Api.Util;

namespace Top.Api.Request
{
    /// <summary>
    /// TOP API: taobao.wireless.share.tpwd.create
    /// </summary>
    public class WirelessShareTpwdCreateRequest : BaseTopRequest<Top.Api.Response.WirelessShareTpwdCreateResponse>
    {
        /// <summary>
        /// 口令参数
        /// </summary>
        public string TpwdParam { get; set; }

        public GenPwdIsvParamDtoDomain TpwdParam_ { set { this.TpwdParam = TopUtils.ObjectToJson(value); } } 

        #region ITopRequest Members

        public override string GetApiName()
        {
            return "taobao.wireless.share.tpwd.create";
        }

        public override IDictionary<string, string> GetParameters()
        {
            TopDictionary parameters = new TopDictionary();
            parameters.Add("tpwd_param", this.TpwdParam);
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
/// GenPwdIsvParamDtoDomain Data Structure.
/// </summary>
[Serializable]

public class GenPwdIsvParamDtoDomain : TopObject
{
	        /// <summary>
	        /// 扩展字段JSON格式
	        /// </summary>
	        [XmlElement("ext")]
	        public string Ext { get; set; }
	
	        /// <summary>
	        /// 口令弹框logoURL
	        /// </summary>
	        [XmlElement("logo")]
	        public string Logo { get; set; }
	
	        /// <summary>
	        /// 口令弹框内容
	        /// </summary>
	        [XmlElement("text")]
	        public string Text { get; set; }
	
	        /// <summary>
	        /// 口令跳转url
	        /// </summary>
	        [XmlElement("url")]
	        public string Url { get; set; }
	
	        /// <summary>
	        /// 生成口令的淘宝用户ID
	        /// </summary>
	        [XmlElement("user_id")]
	        public Nullable<long> UserId { get; set; }
}

        #endregion
    }
}
