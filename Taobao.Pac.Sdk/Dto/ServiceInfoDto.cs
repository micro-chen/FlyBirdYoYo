using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("serviceInfoDto")]
    public class ServiceInfoDto
    {
        [JsonProperty("serviceCode")]
        [XmlElement("serviceCode")]
        public string ServiceCode { get; set; }
        [JsonProperty("serviceName")]
        [XmlElement("serviceName")]
        public string ServiceName { get; set; }
    }
}
