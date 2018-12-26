using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    /// <summary>
    /// 电子面单云打印取号接口响应具体内容
    /// 来自：https://global.link.cainiao.com/#/homepage/api/link/merchant_electronic_sheet/TMS_WAYBILL_GET?_k=ttno66
    /// </summary>
    [XmlRoot("waybillCloudPrintResponse")]
    public class WaybillCloudPrintResponseDto
    {
        /// <summary>
        /// 请求id
        /// </summary>
        [JsonProperty("objectId")]
        [XmlElement("objectId")]
        public string ObjectId { get; set; }

        /// <summary>
        /// 面单号
        /// </summary>
        [JsonProperty("waybillCode")]
        [XmlElement("waybillCode")]
        public string WaybillCode { get; set; }

        /// <summary>
        ///云打印内容
        /// </summary>
        [JsonProperty("printData")]
        [XmlElement("printData")]
        public string PrintData { get; set; }

    }
}
