using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// TbkFavorites Data Structure.
    /// </summary>
    [Serializable]
    public class TbkFavorites : TopObject
    {
        /// <summary>
        /// 选品库id
        /// </summary>
        [XmlElement("favorites_id")]
        public long FavoritesId { get; set; }

        /// <summary>
        /// 选品组名称
        /// </summary>
        [XmlElement("favorites_title")]
        public string FavoritesTitle { get; set; }

        /// <summary>
        /// 选品库类型，1：普通类型，2高佣金类型
        /// </summary>
        [XmlElement("type")]
        public long Type { get; set; }
    }
}
