using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("item")]
    public class ItemDto
    {
        [JsonProperty("count")]
        [XmlElement("count")]
        public int Count { get; set; }


        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
