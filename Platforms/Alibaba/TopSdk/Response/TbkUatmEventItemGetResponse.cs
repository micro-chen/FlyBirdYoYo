using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkUatmEventItemGetResponse.
    /// </summary>
    public class TbkUatmEventItemGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝联盟定向招商宝贝信息
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("uatm_tbk_item")]
        public List<Top.Api.Domain.UatmTbkItem> Results { get; set; }

        /// <summary>
        /// 宝贝总条数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
