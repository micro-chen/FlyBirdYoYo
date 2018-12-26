using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("CustomArea")]
    public class CustomAreasDto
    {
        [JsonProperty("customAreaName")]
        [XmlElement("customAreaName")]
        public string CustomAreaName { get; set; }

        [JsonProperty("standardTemplateUrl")]
        [XmlElement("standardTemplateUrl")]
        public string StandardTemplateUrl { get; set; }

        [JsonProperty("customAreaMappingId")]
        [XmlElement("customAreaMappingId")]
        public string CustomAreaMappingId { get; set; }

    }
}
