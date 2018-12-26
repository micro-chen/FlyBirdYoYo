using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// FilesGetResponse.
    /// </summary>
    public class FilesGetResponse : TopResponse
    {
        /// <summary>
        /// results
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("top_download_record_do")]
        public List<TopDownloadRecordDoDomain> Results { get; set; }

	/// <summary>
/// TopDownloadRecordDoDomain Data Structure.
/// </summary>
[Serializable]

public class TopDownloadRecordDoDomain : TopObject
{
	        /// <summary>
	        /// 文件创建时间
	        /// </summary>
	        [XmlElement("created")]
	        public string Created { get; set; }
	
	        /// <summary>
	        /// 下载链接状态。1:未下载。2:已下载
	        /// </summary>
	        [XmlElement("status")]
	        public long Status { get; set; }
	
	        /// <summary>
	        /// 下载链接
	        /// </summary>
	        [XmlElement("url")]
	        public string Url { get; set; }
}

    }
}
