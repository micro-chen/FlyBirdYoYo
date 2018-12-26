using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;

namespace Taobao.Pac.Sdk.Core.Response.CLOUDPRINT_SELLER_CUSTOM_AREA_DETAIL
{
 


   


    [XmlRoot("response")]
    public class CloudPrintSellerCustomAreaDetailResponse:BaseResponse
    {
        [JsonProperty("data")]
        [XmlArray("data")]
        [XmlArrayItem("StdCustomAreas")]
        public CloudPrintSellerCustomAreaDetailDataDto Data { get; set; }

       
    }
}
