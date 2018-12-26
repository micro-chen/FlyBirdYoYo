using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("document")]
    public class CloudPrintCmdRenderDocumentDto
    {
        [JsonProperty("contents")]
        [XmlArray("contents")]
        [XmlArrayItem("renderContent")]
        public List<CloudPrintCmdRenderContentsDto> Contents { get; set; }

    }
}
