using System;
using System.Xml.Serialization;

namespace QimenCloud.Api
{
    [Serializable]
    public abstract class QimenCloudResponse
    {
        [XmlElement("flag")]
        public string Flag { get; set; }

        [XmlElement("code")]
        public string Code { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }

        [XmlElement("sub_code")]
        public string SubCode { get; set; }

        [XmlElement("sub_message")]
        public string SubMessage { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public string Body { get; set; }

        public string RequestUrl { get; set; }

        public bool IsError
        {
            get
            {
                return !"success".Equals(Flag);
            }
        }
    }
}
