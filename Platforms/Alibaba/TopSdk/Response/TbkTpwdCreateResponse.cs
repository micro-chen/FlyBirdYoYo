using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkTpwdCreateResponse.
    /// </summary>
    public class TbkTpwdCreateResponse : TopResponse
    {
        /// <summary>
        /// data
        /// </summary>
        [XmlElement("data")]
        public MapDataDomain Data { get; set; }

	/// <summary>
/// MapDataDomain Data Structure.
/// </summary>
[Serializable]

public class MapDataDomain : TopObject
{
	        /// <summary>
	        /// password
	        /// </summary>
	        [XmlElement("model")]
	        public string Model { get; set; }
}

    }
}
