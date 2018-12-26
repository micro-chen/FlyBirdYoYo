using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("SellerCustomTemplate")]
    public class CloudPrintSellerISVtemplatesDataDto
    {
        [JsonProperty("templateName")]
        [XmlElement("templateName")]
        public string TemplateName { get; set; }

        [JsonProperty("keys")]
        [XmlArray("keys")]
        [XmlArrayItem("KeyResult")]
        public List<ISVtemplatesKeyDto> Keys { get; set; }

        [JsonProperty("templateUrl")]
        [XmlElement("templateUrl")]
        public string TemplateUrl { get; set; }

    }

    [XmlRoot("KeyResult")]
    public class ISVtemplatesKeyDto
    {
        [JsonProperty("keyName")]
        [XmlElement("keyName")]
        public string KeyName { get; set; }

    }
}
