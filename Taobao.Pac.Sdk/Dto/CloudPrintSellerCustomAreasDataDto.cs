using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("StdCustomAreas")]
    public class CloudPrintSellerCustomAreasDataDto
    {
        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }

        [JsonProperty("customAreas")]
        [XmlElement("customAreas")]
        public List<CustomAreasDto> CustomAreas { get; set; }

    }
}
