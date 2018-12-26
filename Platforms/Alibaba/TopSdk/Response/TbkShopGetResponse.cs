using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkShopGetResponse.
    /// </summary>
    public class TbkShopGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝客店铺
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("n_tbk_shop")]
        public List<Top.Api.Domain.NTbkShop> Results { get; set; }

        /// <summary>
        /// 搜索到符合条件的结果总数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
