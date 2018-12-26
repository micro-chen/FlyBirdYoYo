using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Response
{
    [XmlRoot("response")]
    public class TmsWaybillDiscardResponse:BaseResponse
    {
        [JsonProperty("discardResult")]
        [XmlElement("discardResult")]
        public string DiscardResult { get; set; }
    }
}
