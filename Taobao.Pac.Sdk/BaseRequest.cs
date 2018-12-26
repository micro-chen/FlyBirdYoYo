using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Serialization;
namespace Taobao.Pac.Sdk.Core
{
    /// <summary>
    /// 基础公共的请求参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseRequest<T> where T : BaseResponse
    {
        public BaseRequest()
        {
            this.SoapFormat = PacSupportFormat.JSON;
        }
        /// <summary>
        /// 消息类型--请求的API
        /// 如：TMS_WAYBILL_SUBSCRIPTION_QUERY
        /// </summary>
        [JsonProperty("msg_type")]
        [XmlIgnore]
        internal abstract string MsgType { get; }

        /// <summary>
        /// 来源CP编号(资源code)
        /// 商户的  ISV Token
        /// https://global.link.cainiao.com/#/homepage/doc/190?_k=e93idv
        /// 参数说明 LogisticProviderId 1、在您是物流商时，此值应为资源ID；2、您是商家时，此值应为商家ID； 3、三方授权场景时（ISV），此值应为AccessToken
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public string LogisticProviderId { get; set; }

        /// <summary>
        /// 数字签名
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public string DataDigest { get; set; }

        /// <summary>
        /// 目的方编码（可选，如不填使用该msg_type默认目的方）
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public string ToCode { get; set; }

        /// <summary>
        ///请求报文的格式
        ///默认：Json
        /// </summary>
        [JsonIgnore]
        [XmlIgnore]
        public PacSupportFormat SoapFormat { get; set; }
         


        public abstract bool Validate();
    }
}
