using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Top.Api.Response
{
    /// <summary>
    /// TbkJuTqgGetResponse.
    /// </summary>
    public class TbkJuTqgGetResponse : TopResponse
    {
        /// <summary>
        /// 淘抢购对象
        /// </summary>
        [XmlArray("results")]
        [XmlArrayItem("results")]
        public List<ResultsDomain> Results { get; set; }

        /// <summary>
        /// 返回的结果数
        /// </summary>
        [XmlElement("total_results")]
        public long TotalResults { get; set; }

	/// <summary>
/// ResultsDomain Data Structure.
/// </summary>
[Serializable]

public class ResultsDomain : TopObject
{
	        /// <summary>
	        /// 类目名称
	        /// </summary>
	        [XmlElement("category_name")]
	        public string CategoryName { get; set; }
	
	        /// <summary>
	        /// 商品链接（是淘客商品返回淘客链接，非淘客商品返回普通h5链接）
	        /// </summary>
	        [XmlElement("click_url")]
	        public string ClickUrl { get; set; }
	
	        /// <summary>
	        /// 结束时间
	        /// </summary>
	        [XmlElement("end_time")]
	        public string EndTime { get; set; }
	
	        /// <summary>
	        /// 商品ID
	        /// </summary>
	        [XmlElement("num_iid")]
	        public long NumIid { get; set; }
	
	        /// <summary>
	        /// 商品主图
	        /// </summary>
	        [XmlElement("pic_url")]
	        public string PicUrl { get; set; }
	
	        /// <summary>
	        /// 商品原价
	        /// </summary>
	        [XmlElement("reserve_price")]
	        public string ReservePrice { get; set; }
	
	        /// <summary>
	        /// 已抢购数量
	        /// </summary>
	        [XmlElement("sold_num")]
	        public long SoldNum { get; set; }
	
	        /// <summary>
	        /// 开团时间
	        /// </summary>
	        [XmlElement("start_time")]
	        public string StartTime { get; set; }
	
	        /// <summary>
	        /// 商品标题
	        /// </summary>
	        [XmlElement("title")]
	        public string Title { get; set; }
	
	        /// <summary>
	        /// 总库存
	        /// </summary>
	        [XmlElement("total_amount")]
	        public long TotalAmount { get; set; }
	
	        /// <summary>
	        /// 淘抢购活动价
	        /// </summary>
	        [XmlElement("zk_final_price")]
	        public string ZkFinalPrice { get; set; }
}

    }
}
