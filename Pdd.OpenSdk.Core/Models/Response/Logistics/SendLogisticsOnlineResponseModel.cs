using Newtonsoft.Json;
namespace Pdd.OpenSdk.Core.Models.Response.Logistics
{
    public class LogisticsOnlineSendResponse
    {

        /// <summary>
        /// Examples: 1
        /// </summary>
        [JsonProperty("is_success")]
        public bool IsSuccess { get; set; }
    }

    public class SendLogisticsOnlineResponseModel
    {

        /// <summary>
        /// Examples: {"is_success":1}
        /// </summary>
        [JsonProperty("logistics_online_send_response")]
        public LogisticsOnlineSendResponse LogisticsOnlineSendResponse { get; set; }
    }


}
