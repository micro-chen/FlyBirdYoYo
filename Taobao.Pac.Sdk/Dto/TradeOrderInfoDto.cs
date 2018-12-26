using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("tradeOrderInfoDto")]
    public class TradeOrderInfoDto
    {
        [JsonProperty("logisticsServices")]
        [XmlElement("logisticsServices")]
        public string LogisticsServices { get; set; }

        [JsonProperty("orderInfo")]
        [XmlElement("orderInfo")]
        public OrderInfoDto OrderInfo { get; set; }

        /// <summary>
        /// 收件人信息
        /// </summary>
        [JsonProperty("recipient")]
        [XmlElement("recipient")]
        public UserInfoDto Recipient { get; set; }

        [JsonProperty("packageInfo")]
        [XmlElement("packageInfo")]
        public PackageInfoDto PackageInfo { get; set; }
        [JsonProperty("userId")]
        [XmlElement("userId")]
        public string UserId { get; set; }
        [JsonProperty("objectId")]
        [XmlElement("objectId")]
        public string ObjectId { get; set; }
        [JsonProperty("templateUrl")]
        [XmlElement("templateUrl")]
        public string TemplateUrl { get; set; }
    }

}
