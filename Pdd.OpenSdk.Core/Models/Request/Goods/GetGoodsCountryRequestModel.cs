using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Goods
{
    public partial class GetGoodsCountryRequestModel : PddRequestModel
    {
        /// <summary>
        /// 该值为：pdd.goods.country.get
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

    }

}
