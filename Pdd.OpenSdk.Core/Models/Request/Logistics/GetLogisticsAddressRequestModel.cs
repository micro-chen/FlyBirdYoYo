using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Logistics
{
    public partial class GetLogisticsAddressRequestModel : PddRequestModel
    {
        /// <summary>
        /// 该值为：pdd.logistics.address.get
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

    }

}
