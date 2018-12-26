using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Taobao.Pac.Sdk.Core.Dto
{
    [XmlRoot("orderInfo")]
    public class OrderInfoDto
    {
        /// <summary>
        /// 订单渠道平台编码：淘宝(TB)、天猫(TM)、京东(JD)、当当(DD)、拍拍(PP)、易讯(YX)、ebay(EBAY)、QQ网购(QQ) 、亚马逊(AMAZON)、苏宁(SN)、国美(GM)、唯品会(WPH)、聚美(JM)、乐蜂(LF)、蘑菇街(MGJ) 、聚尚(JS)、拍鞋(PX)、银泰(YT)、1号店(YHD)、凡客(VANCL)、邮乐(YL)、优购(YG)、阿里 巴巴(1688)、其他(OTHERS)
        /// </summary>
        [JsonProperty("orderChannelsType")]
        [XmlElement("orderChannelsType")]
        public string OrderChannelsType { get; set; }

        [JsonProperty("tradeOrderList")]
        [XmlArray("tradeOrderList")]
        [XmlArrayItem("tradeOrder")]
        public List<string> TradeOrderList { get; set; }
    }
}
