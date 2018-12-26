using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkItemGetResponse.
    /// </summary>
    public class TbkItemGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝客商品
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("n_tbk_item")]
        public List<Top.Api.Domain.NTbkItem> Results { get; set; }

        /// <summary>
        /// 搜索到符合条件的结果总数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
