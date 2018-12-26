using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("standardTemplateResult")]
    public class TemplatesDataDto
    {
        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }

        [JsonProperty("standardTemplateDOs")]
        [XmlArray("standardTemplateDOs")]
        [XmlArrayItem("standardTemplateDO")]
        public List<StandardTemplateDto> StandardTemplateDOs { get; set; }

    }
}
