using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Goods
{
    public partial class GetGoodsCommitDetailRequestModel : PddRequestModel
    {
        /// <summary>
        /// 提交申请的序列id
        /// </summary>
        [JsonProperty("goods_commit_id")]
        public int GoodsCommitId { get; set; }

    }

}
