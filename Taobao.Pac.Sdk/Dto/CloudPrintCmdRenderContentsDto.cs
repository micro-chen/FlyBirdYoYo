using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("renderContent")]
    public class CloudPrintCmdRenderContentsDto
    {
        [JsonProperty("printData")]
        [XmlElement("printData")]
        public string PrintData { get; set; }

        [JsonProperty("templateUrl")]
        [XmlElement("templateUrl")]
        public string TemplateUrl { get; set; }

    }

}
