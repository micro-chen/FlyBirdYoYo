using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkShopRecommendGetResponse.
    /// </summary>
    public class TbkShopRecommendGetResponse : TopResponse
    {
        /// <summary>
        /// 淘宝客店铺
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("n_tbk_shop")]
        public List<Top.Api.Domain.NTbkShop> Results { get; set; }

    }
}
