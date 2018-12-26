using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// OpenuidGetBymixnickResponse.
    /// </summary>
    public class OpenuidGetBymixnickResponse : TopResponse
    {
        /// <summary>
        /// OpenUID
        /// </summary>
        [XmlElement("open_uid")]
        public string OpenUid { get; set; }

    }
}
