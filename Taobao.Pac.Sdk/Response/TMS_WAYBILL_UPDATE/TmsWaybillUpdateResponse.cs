using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Response
{
    [XmlRoot("response")]
    public class TmsWaybillUpdateResponse:BaseResponse
    {

        [JsonProperty("waybillCode")]
        [XmlElement("waybillCode")]
        public string WaybillCode { get; set; }


        [JsonProperty("printData")]
        [XmlElement("waybillCode")]
        public string PrintData { get; set; }
    }
}
