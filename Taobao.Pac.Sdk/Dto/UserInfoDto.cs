using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{

    [XmlRoot("sender")]
    public class SenderUserInfoDto : UserInfoDto
    {
    }

    [XmlRoot("recipient")]
    public class RecipientUserInfoDto : UserInfoDto
    {
    }



    public class UserInfoDto
    {
        [JsonProperty("address")]
        [XmlElement("address")]
        public AddressDto Address { get; set; }

        [JsonProperty("phone")]
        [XmlElement("phone")]
        public string Phone { get; set; }

        [JsonProperty("mobile")]
        [XmlElement("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("name")]
        [XmlElement("name")]
        public string Name { get; set; }
    }

}
