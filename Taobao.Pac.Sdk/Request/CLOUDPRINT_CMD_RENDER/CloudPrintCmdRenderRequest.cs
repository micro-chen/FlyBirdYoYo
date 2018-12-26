using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request.CLOUDPRINT_CMD_RENDER
{



    [XmlRoot("request")]
    public class CloudPrintCmdRenderRequest:BaseRequest<CloudPrintCmdRenderResponse>
    {
        internal override string MsgType
        {
            get
            {
                return "CLOUDPRINT_CMD_RENDER";
            }
        }

        /// <summary>
        /// 客户端用户标识：请确保该字段值能唯一标识一个用户
        /// </summary>
        [JsonProperty("clientId")]
        [XmlElement("clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端类型：native-本地应用 weixin-微信小程序 alipay-支付宝小程序 qianniu-千牛
        /// </summary>
        [JsonProperty("clientType")]
        [XmlElement("clientType")]
        public string ClientType { get; set; }

        [JsonProperty("document")]
        [XmlElement("document")]
        public CloudPrintCmdRenderDocumentDto Document { get; set; }

        [JsonProperty("printerName")]
        [XmlElement("printerName")]
        public string PrinterName { get; set; }

        [JsonProperty("config")]
        [XmlElement("config")]
         public  CloudPrintCmdRenderConfigDto Config { get; set; }



        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.ClientId))
            {
                throw new PacException("ClientId 不能为空！");
            }
            if (string.IsNullOrEmpty(this.ClientType))
            {
                throw new PacException("ClientType 不能为空！");
            }

            return true;
        }

    }
}
