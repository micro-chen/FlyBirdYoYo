using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Response.CLOUDPRINT_SELLER_CUSTOM_AREA_DETAIL;

namespace Taobao.Pac.Sdk.Core.Request.CLOUDPRINT_SELLER_CUSTOM_AREA_DETAIL
{

    /// <summary>
    /// 商家自定义区详情 Version:1请求
    /// </summary>
    [XmlRoot("request")]
    public class CloudPrintSellerCustomAreaDetailRequest : BaseRequest<CloudPrintSellerCustomAreaDetailResponse>
    {
        internal override string MsgType
        {
            get
            {
                return "CLOUDPRINT_SELLER_CUSTOM_AREA_DETAIL";
            }
        }

        [JsonProperty("mappingId")]
        [XmlElement("mappingId")]
        public string MappingId { get; set; }


        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.MappingId))
            {
                throw new PacException("MappingId 不能为空！");
            }

            return true;
        }
    }

}
