using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("standardTemplateDO")]
    public class StandardTemplateDto
    {
        [JsonProperty("standardWaybillType")]
        [XmlElement("standardWaybillType")]
        public string StandardWaybillType { get; set; }

        [JsonProperty("standardTemplateUrl")]
        [XmlElement("standardTemplateUrl")]
        public string StandardTemplateUrl { get; set; }

        [JsonProperty("standardTemplateName")]
        [XmlElement("standardTemplateName")]
        public string StandardTemplateName { get; set; }

    }
}
