using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request
{
    /// <summary>
    /// 电子面单云打印取号请求
    /// </summary>
    [XmlRoot("request")]
    public class TmsWaybillGetRequest : BaseRequest<TmsWaybillGetResponse>
    {
        [JsonIgnore]
        internal override string MsgType
        {
            get
            {
                return "TMS_WAYBILL_GET";
            }
        }


        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }

        [JsonProperty("tradeOrderInfoDtos")]
        [XmlArray("tradeOrderInfoDtos")]
        [XmlArrayItem("tradeOrderInfoDto")]
        public List<TradeOrderInfoDto> TradeOrderInfoDtos { get; set; }

        [JsonProperty("needEncrypt")]
        [XmlElement("needEncrypt")]
        public bool NeedEncrypt { get; set; }

        [JsonProperty("resourceCode")]
        [XmlElement("resourceCode")]
        public string ResourceCode { get; set; }


        /// <summary>
        /// 发件人信息
        /// </summary>
        [JsonProperty("sender")]
        [XmlElement("sender")]
        public UserInfoDto Sender { get; set; }

        [JsonProperty("dmsSorting")]
        [XmlElement("dmsSorting")]
        public bool DmsSorting { get; set; }

        [JsonProperty("storeCode")]
        [XmlElement("storeCode")]
        public string StoreCode { get; set; }




        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.CpCode))
            {
                throw new PacException("物流公司Code不能为空！");
            }
            if (null == this.Sender)
            {
                throw new PacException("发件人不能为空！");
            }
            if (null == this.TradeOrderInfoDtos)
            {
                throw new PacException("请求面单信息不能为空！");
            }
            if (this.TradeOrderInfoDtos.Count > 10)
            {
                throw new PacException("请求面单信息最多10个！");
            }
            return true;
        }
    }




}
