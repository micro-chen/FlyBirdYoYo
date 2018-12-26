using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkUatmEventGetResponse.
    /// </summary>
    public class TbkUatmEventGetResponse : TopResponse
    {
        /// <summary>
        /// 淘客定向招商活动基本信息
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("tbk_event")]
        public List<Top.Api.Domain.TbkEvent> Results { get; set; }

        /// <summary>
        /// 当前进行中的招商活动总条数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
