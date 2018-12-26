using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Taobao.Pac.Sdk.Core.Dto;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("waybillBranchAccount")]
    public class WaybillBranchAccountDto
    {
        [JsonProperty("branchCode")]
        [XmlElement("branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty("quantity")]
        [XmlElement("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("shippAddressCols")]
        [XmlArray("shippAddressCols")]
        [XmlArrayItem("addressDto")]
        public List<AddressDto> ShippAddressCols { get; set; }

        [JsonProperty("allocatedQuantity")]
        [XmlElement("allocatedQuantity")]
        public string AllocatedQuantity { get; set; }

        [JsonProperty("branchStatus")]
        [XmlElement("branchStatus")]
        public string BranchStatus { get; set; }

        [JsonProperty("printQuantity")]
        [XmlElement("printQuantity")]
        public string PrintQuantity { get; set; }

        [JsonProperty("serviceInfoCols")]
        [XmlArray("serviceInfoCols")]
        [XmlArrayItem("serviceInfoDto")]
        public List<ServiceInfoDto> ServiceInfoCols { get; set; }

        [JsonProperty("branchName")]
        [XmlElement("branchName")]
        public string BranchName { get; set; }

        [JsonProperty("cancelQuantity")]
        [XmlElement("cancelQuantity")]
        public string CancelQuantity { get; set; }
    }

}
