using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Ddk
{
    public partial class GetDdkOrderDetailRequestModel : PddRequestModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [JsonProperty("order_sn")]
        public string OrderSn { get; set; }

    }

}
