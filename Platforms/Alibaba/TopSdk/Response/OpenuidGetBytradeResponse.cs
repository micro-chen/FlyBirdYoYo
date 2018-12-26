using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenuidGetBytradeResponse.
    /// </summary>
    public class OpenuidGetBytradeResponse : TopResponse
    {
        /// <summary>
        /// 当前交易tid对应买家的openuid
        /// </summary>
        [XmlElement("open_uid")]
        public string OpenUid { get; set; }

    }
}
