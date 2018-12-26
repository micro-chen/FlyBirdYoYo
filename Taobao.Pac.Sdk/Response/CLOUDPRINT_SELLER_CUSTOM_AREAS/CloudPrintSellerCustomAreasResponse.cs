using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;

namespace Taobao.Pac.Sdk.Core.Response
{
     


    [XmlRoot("response")]
    public class CloudPrintSellerCustomAreasResponse:BaseResponse
    {
        [JsonProperty("data")]
        [XmlElement("data")]
        public List<CloudPrintSellerCustomAreasDataDto> Data { get; set; }
 

    }

}
