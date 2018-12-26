using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("StdCustomAreas")]
    public class CloudPrintSellerCustomAreaDetailDataDto
    {
        [JsonProperty("customAreaUrl")]
        [XmlElement("customAreaUrl")]
        public string CustomAreaUrl { get; set; }
        [JsonProperty("customAreaName")]
        [XmlElement("customAreaName")]
        public string CustomAreaName { get; set; }

        [JsonProperty("keys")]
        [XmlArray("keys")]
        [XmlArrayItem("KeyResult")]
        public List<CloudPrintSellerCustomAreaDetailKeysDto> Keys { get; set; }



    }
}
