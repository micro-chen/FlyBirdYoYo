using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;

namespace Taobao.Pac.Sdk.Core.Response
{
    [XmlRoot("response")]
    public class TmsWaybillSubscriptionQueryResponse:BaseResponse
    {
        [JsonProperty("waybillApplySubscriptionCols")]
        [XmlArray("waybillApplySubscriptionCols")]
        [XmlArrayItem("waybillApplySubscriptionInfo")]
        public List<WaybillApplySubscriptionInfoDto> WaybillApplySubscriptionCols { get; set; }
    }

}
