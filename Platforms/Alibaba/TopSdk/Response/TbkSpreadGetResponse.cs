using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkSpreadGetResponse.
    /// </summary>
    public class TbkSpreadGetResponse : TopResponse
    {
        /// <summary>
        /// 传播形式对象列表
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("tbk_spread")]
        public List<TbkSpreadDomain> Results { get; set; }

        /// <summary>
        /// totalResults
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

	/// <summary>
/// TbkSpreadDomain Data Structure.
/// </summary>
[Serializable]

public class TbkSpreadDomain : TopObject
{
	        /// <summary>
	        /// 传播形式, 目前只支持短链接
	        /// </summary>
	        [XmlElement("content")]
	        public string Content { get; set; }
	
	        /// <summary>
	        /// 调用错误信息；由于是批量接口，请重点关注每条请求返回的结果，如果非OK，则说明该结果对应的content不正常，请酌情处理;
	        /// </summary>
	        [XmlElement("err_msg")]
	        public string ErrMsg { get; set; }
}

    }
}
