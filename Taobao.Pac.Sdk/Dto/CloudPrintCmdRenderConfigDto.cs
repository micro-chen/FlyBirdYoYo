using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("config")]
    public class CloudPrintCmdRenderConfigDto
    {
        [JsonProperty("needMiddleLogo")]
        [XmlElement("needMiddleLogo")]
        public string NeedMiddleLogo { get; set; }

        [JsonProperty("needBottomLogo")]
        [XmlElement("needBottomLogo")]
        public string NeedBottomLogo { get; set; }

        [JsonProperty("orientation")]
        [XmlElement("orientation")]
        public string Orientation { get; set; }

        [JsonProperty("extra")]
        [XmlElement("extra")]
        public string Extra { get; set; }

        [JsonProperty("needTopLogo")]
        [XmlElement("needTopLogo")]
        public string NeedTopLogo { get; set; }

    }
}
