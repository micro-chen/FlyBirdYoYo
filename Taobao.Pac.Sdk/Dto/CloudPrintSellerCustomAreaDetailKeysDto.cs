using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("KeyResult")]
    public class CloudPrintSellerCustomAreaDetailKeysDto
    {
        [JsonProperty("keyName")]
        [XmlElement("keyName")]
        public string KeyName { get; set; }

    }

}
