using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;
using Taobao.Pac.Sdk.Core.Response;

namespace Taobao.Pac.Sdk.Core.Request
{
    /// <summary>
    /// 电子面单云打印更新请求
    /// </summary>
    [XmlRoot("request")]
    public class TmsWaybillUpdateRequest : BaseRequest<TmsWaybillUpdateResponse>
    {
        [JsonIgnore]
        internal override string MsgType
        {
            get
            {
                return "TMS_WAYBILL_UPDATE";
            }
        }


        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }

        [JsonProperty("logisticsServices")]
        [XmlElement("logisticsServices")]
        public string LogisticsServices { get; set; }

        [JsonProperty("sender")]
        [XmlElement("sender")]
        public SenderUserInfoDto Sender { get; set; }

        [JsonProperty("recipient")]
        [XmlElement("recipient")]
        public RecipientUserInfoDto Recipient { get; set; }

        [JsonProperty("waybillCode")]
        [XmlElement("waybillCode")]
        public string WaybillCode { get; set; }

        [JsonProperty("packageInfo")]
        [XmlElement("packageInfo")]
        public PackageInfoDto PackageInfo { get; set; }

        [JsonProperty("objectId")]
        [XmlElement("objectId")]
        public string ObjectId { get; set; }

        [JsonProperty("templateUrl")]
        [XmlElement("templateUrl")]
        public string TemplateUrl { get; set; }



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
