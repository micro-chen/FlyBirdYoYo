using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request
{
    /// <summary>
    /// 获取发货地，CP开通状态，账户的使用情况请求
    /// </summary>
    [XmlRoot("request")]
    public class TmsWaybillSubscriptionQueryRequest : BaseRequest<TmsWaybillSubscriptionQueryResponse>
    {
        [JsonIgnore]
        internal override string MsgType
        {
            get
            {
                return "TMS_WAYBILL_SUBSCRIPTION_QUERY";
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
