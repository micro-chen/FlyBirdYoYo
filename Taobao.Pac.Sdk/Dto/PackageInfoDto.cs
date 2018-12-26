using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("packageInfo")]
    public class PackageInfoDto
    {
        [JsonProperty("volume")]
        [XmlElement("volume")]
        public string Volume { get; set; }

        [JsonProperty("weight")]
        [XmlElement("weight")]
        public string Weight { get; set; }

        [JsonProperty("id")]
        [XmlElement("id")]
        public string Id { get; set; }


        [JsonProperty("items")]
        [XmlArray("items")]
        [XmlArrayItem("item")]
        public List<ItemDto> Items { get; set; }
    }
}
