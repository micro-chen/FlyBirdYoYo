using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request
{
    /// <summary>
    /// 获取云打印标准面单 Version:1
    /// </summary>
    [XmlRoot("request")]
    public class CloudprintStandardTemplatesRequest : BaseRequest<CloudprintStandardTemplatesResponse>
    {
        internal override string MsgType
        {
            get
            {
                return "CLOUDPRINT_STANDARD_TEMPLATES";
            }
        }

        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }


        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.CpCode))
            {
                throw new PacException("物流公司code不能为空！");
            }
          
            return true;
        }
    }

}
