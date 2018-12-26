using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("addressDto")]
    public class AddressDto
    {
        [JsonProperty("province")]
        [XmlElement("province")]
        public string Province { get; set; }
        [JsonProperty("town")]
        [XmlElement("town")]
        public string Town { get; set; }
        [JsonProperty("city")]
        [XmlElement("city")]
        public string City { get; set; }
        [JsonProperty("district")]
        [XmlElement("district")]
        public string District { get; set; }
        [JsonProperty("detail")]
        [XmlElement("detail")]
        public string Detail { get; set; }
    }

}
