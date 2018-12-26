using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Taobao.Pac.Sdk.Core.Dto;

namespace Taobao.Pac.Sdk.Core.Response
{
    /// <summary>
    /// 电子面单云打印取号接口响应内容
    /// 来自：https://global.link.cainiao.com/#/homepage/api/link/merchant_electronic_sheet/TMS_WAYBILL_GET?_k=ttno66
    /// </summary>
    [XmlRoot("response")]
    public class TmsWaybillGetResponse:BaseResponse
    {
        [JsonProperty("waybillCloudPrintResponseList")]
        [XmlArray("waybillCloudPrintResponseList")]
        [XmlArrayItem("waybillCloudPrintResponse")]
        public List<WaybillCloudPrintResponseDto> WaybillCloudPrintResponseList { get; set; }
    }
}
