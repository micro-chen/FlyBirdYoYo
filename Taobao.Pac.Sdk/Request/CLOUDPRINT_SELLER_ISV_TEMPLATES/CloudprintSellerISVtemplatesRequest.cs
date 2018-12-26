using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request.CLOUDPRINT_SELLER_ISV_TEMPLATES
{


    /// <summary>
    /// 获取商家在某个isv下创建的自定义模板 Version:1
    /// </summary>
    [XmlRoot("request")]
    public class CloudPrintSellerISVtemplatesRequest : BaseRequest<CloudPrintSellerISVtemplatesResponse>
    {
        internal override string MsgType
        {
            get
            {
                return "CLOUDPRINT_SELLER_ISV_TEMPLATES";
            }
        }

        [JsonProperty("objectId")]
        [XmlElement("objectId")]
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
