using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkUatmFavoritesItemGetResponse.
    /// </summary>
    public class TbkUatmFavoritesItemGetResponse : TopResponse
    {
        /// <summary>
        /// 招商宝贝信息
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("uatm_tbk_item")]
        public List<Top.Api.Domain.UatmTbkItem> Results { get; set; }

        /// <summary>
        /// 选品库中的商品总条数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
