using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// WirelessShareTpwdCreateResponse.
    /// </summary>
    public class WirelessShareTpwdCreateResponse : TopResponse
    {
        /// <summary>
        /// 口令内容，用于口令宣传组织
        /// </summary>
        [XmlElement("model")]
        public string Model { get; set; }

    }
}
