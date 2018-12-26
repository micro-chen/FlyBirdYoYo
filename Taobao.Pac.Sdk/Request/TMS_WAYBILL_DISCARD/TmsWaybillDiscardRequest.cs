using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request
{
    /// <summary>
    /// ISV电子面单取消请求
    /// </summary>
    [XmlRoot("request")]
    public class TmsWaybillDiscardRequest : BaseRequest<TmsWaybillDiscardResponse>
    {
        internal override string MsgType
        {
            get
            {
                return "TMS_WAYBILL_DISCARD";
            }
        }

        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }


        [JsonProperty("waybillCode")]
        [XmlElement("waybillCode")]
        public string WaybillCode { get; set; }


        public override bool Validate()
        {
            if (string.IsNullOrEmpty(this.CpCode))
            {
                throw new PacException("物流公司code不能为空！");
            }
            if (string.IsNullOrEmpty(this.WaybillCode))
            {
                throw new PacException("面单号不能为空！");
            }
            return true;
        }
    }

}
