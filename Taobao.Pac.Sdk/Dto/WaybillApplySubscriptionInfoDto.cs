using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("waybillApplySubscriptionInfo")]
    public class WaybillApplySubscriptionInfoDto
    {
        [JsonProperty("cpCode")]
        [XmlElement("cpCode")]
        public string CpCode { get; set; }

        [JsonProperty("cpType")]
        [XmlElement("cpType")]
        public string CpType { get; set; }

        [JsonProperty("branchAccountCols")]
        [XmlArray("branchAccountCols")]
        [XmlArrayItem("waybillBranchAccount")]
        public List<WaybillBranchAccountDto> BranchAccountCols { get; set; }
    }
}
