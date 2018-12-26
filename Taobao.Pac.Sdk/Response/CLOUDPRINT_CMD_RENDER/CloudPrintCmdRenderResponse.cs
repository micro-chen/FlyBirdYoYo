using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Response
{

    [XmlRoot("response")]
    public class CloudPrintCmdRenderResponse:BaseResponse
    {
        [JsonProperty("cmdContent")]
        [XmlElement("cmdContent")]
        public string CmdContent { get; set; }

    

        [JsonProperty("cmdEncoding")]
        [XmlElement("cmdEncoding")]
        public string CmdEncoding { get; set; }


    }
}
