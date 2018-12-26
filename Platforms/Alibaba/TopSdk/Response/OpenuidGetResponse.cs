using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenuidGetResponse.
    /// </summary>
    public class OpenuidGetResponse : TopResponse
    {
        /// <summary>
        /// OpenUID
        /// </summary>
        [XmlElement("open_uid")]
        public string OpenUid { get; set; }

    }
}
