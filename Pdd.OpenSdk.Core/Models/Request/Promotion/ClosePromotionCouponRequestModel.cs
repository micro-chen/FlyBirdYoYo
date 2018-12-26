using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Promotion
{
    public partial class ClosePromotionCouponRequestModel : PddRequestModel
    {
        /// <summary>
        /// 券批次ID
        /// </summary>
        [JsonProperty("batch_id")]
        public int BatchId { get; set; }

    }

}
