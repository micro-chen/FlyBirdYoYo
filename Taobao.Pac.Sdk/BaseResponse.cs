using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core
{
 
    public abstract class BaseResponse
    {

        [JsonProperty("success")]
        [XmlElement("success")]
        public bool Success { get; set; }

        [JsonProperty("errorCode")]
        [XmlElement("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorMsg")]
        [XmlElement("errorMsg")]
        public string ErrorMsg { get; set; }

    }
}
