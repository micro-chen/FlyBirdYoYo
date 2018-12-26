using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request.CLOUDPRINT_SELLER_CUSTOM_AREAS
{
    
    [XmlRoot("request")]
    public class CloudPrintSellerCustomAreasRequest : BaseRequest<CloudPrintSellerCustomAreasResponse>
    {
        internal override string MsgType
        {
            get
            {
                return "CLOUDPRINT_SELLER_CUSTOM_AREAS";
            }
        }

        [JsonProperty("object_id")]
        [XmlElement("object_id")]
        public string ObjectId { get; set; }


        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.ObjectId))
            {
                throw new PacException("ObjectId 不能为空！");
            }

            return true;
        }
    }


}
