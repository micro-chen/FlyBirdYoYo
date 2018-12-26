using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkUatmFavoritesGetResponse.
    /// </summary>
    public class TbkUatmFavoritesGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝客选品库
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("tbk_favorites")]
        public List<Top.Api.Domain.TbkFavorites> Results { get; set; }

        /// <summary>
        /// 选品库总数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

    }
}
