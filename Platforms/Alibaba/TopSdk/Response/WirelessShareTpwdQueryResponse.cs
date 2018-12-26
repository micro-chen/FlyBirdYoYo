using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// WirelessShareTpwdQueryResponse.
    /// </summary>
    public class WirelessShareTpwdQueryResponse : TopResponse
    {
        /// <summary>
        /// 淘口令-文案
        /// </summary>
        [XmlElement("content")]
        public string Content { get; set; }

        /// <summary>
        /// nativeUrl
        /// </summary>
        [XmlElement("native_url")]
        public string NativeUrl { get; set; }

        /// <summary>
        /// 图片url
        /// </summary>
        [XmlElement("pic_url")]
        public string PicUrl { get; set; }

        /// <summary>
        /// 如果是宝贝，则为宝贝价格
        /// </summary>
        [XmlElement("price")]
        public string Price { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [XmlElement("suc")]
        public bool Suc { get; set; }

        /// <summary>
        /// thumbPicUrl
        /// </summary>
        [XmlElement("thumb_pic_url")]
        public string ThumbPicUrl { get; set; }

        /// <summary>
        /// 淘口令-宝贝
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        /// <summary>
        /// 跳转url(长链)
        /// </summary>
        [XmlElement("url")]
        public string Url { get; set; }

    }
}
