using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Goods
{
    public partial class GetGoodsMallTemplateRequestModel : PddRequestModel
    {
        /// <summary>
        /// 类目id
        /// </summary>
        [JsonProperty("cat_id")]
        public int CatId { get; set; }

    }

}
