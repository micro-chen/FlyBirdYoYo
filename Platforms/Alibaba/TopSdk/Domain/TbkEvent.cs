using System;
using System.Xml.Serialization;

namespace Top.Api.Domain
{
    /// <summary>
    /// TbkEvent Data Structure.
    /// </summary>
    [Serializable]
    public class TbkEvent : TopObject
    {
        /// <summary>
        /// 定向招商活动结束开始时间
        /// </summary>
        [XmlElement("end_time")]
        public string EndTime { get; set; }

        /// <summary>
        /// 淘宝联盟定向招商活动id
        /// </summary>
        [XmlElement("event_id")]
        public long EventId { get; set; }

        /// <summary>
        /// 淘宝联盟定向招商活动名称
        /// </summary>
        [XmlElement("event_title")]
        public string EventTitle { get; set; }

        /// <summary>
        /// 定向招商活动结束开始时间
        /// </summary>
        [XmlElement("start_time")]
        public string StartTime { get; set; }
    }
}
