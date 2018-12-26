using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Request.Logisticscs
{
    public partial class GetLogisticsCsHistoryMessageRequestModel : PddRequestModel
    {
        /// <summary>
        /// pdd会话id
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

    }

}
