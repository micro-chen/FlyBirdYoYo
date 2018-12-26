using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenuidChangeResponse.
    /// </summary>
    public class OpenuidChangeResponse : TopResponse
    {
        /// <summary>
        /// 转换到新的openUId
        /// </summary>
        [XmlElement("new_open_uid")]
        public string NewOpenUid { get; set; }

    }
}
